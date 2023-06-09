
class RxNode {
    constructor() {
        this.gain = 0.0;
        this.nfig = 0.0;
        this.iip3 = 0.0;
        this.oip3 = 0.0;
        this.ip1db = 0.0;
        this.op1db = 0.0;
        this.ivswr = 1.0;
        this.ovswr = 1.0;
        this.voltage = 9.0;
        this.current = 0.1;

        this.c_gain = 0.0;
        this.c_nfig = 0.0;
        this.c_iip3 = 0.0;
        this.c_oip3 = 0.0;
        this.c_ip1db = 0.0;
        this.c_op1db = 0.0;
        this.c_spower = 0.0;
        this.c_inpower = 0.0;
        this.c_outpower = 0.0;
    }

    parse(element) {
        this.gain = Number.parseFloat(element.querySelector(".node-powergain").value);
        this.nfig = Number.parseFloat(element.querySelector(".node-noisefigure").value);
        this.ivswr = Number.parseFloat(element.querySelector(".node-ivswr").value);
        this.ovswr = Number.parseFloat(element.querySelector(".node-ovswr").value);
        this.voltage = Number.parseFloat(element.querySelector(".node-voltage").value);
        this.current = Number.parseFloat(element.querySelector(".node-current").value);

        if(element.querySelector(".node-xip3-type").selectedIndex === 0) {
            this.iip3 = Number.parseFloat(element.querySelector(".node-xip3").value);
            this.oip3 = this.iip3 + this.gain;
        }
        else {
            this.oip3 = Number.parseFloat(element.querySelector(".node-xip3").value);
            this.iip3 = this.oip3 - this.gain;
        }

        if(element.querySelector(".node-xp1db-type").selectedIndex === 0) {
            this.ip1db = Number.parseFloat(element.querySelector(".node-xp1db").value);
            this.op1db = this.ip1db + this.gain - 1.0;
        }
        else {
            this.op1db = Number.parseFloat(element.querySelector(".node-xp1db").value);
            this.ip1db = this.op1db - this.gain + 1.0;
        }

        this.c_gain = 0.0;
        this.c_nfig = 0.0;
        this.c_iip3 = 0.0;
        this.c_oip3 = 0.0;
        this.c_ip1db = 0.0;
        this.c_op1db = 0.0;
        this.c_spower = 0.0;
        this.c_inpower = 0.0;
        this.c_outpower = 0.0;
    }
};

// Input parameters elements
const in_inpower = document.querySelector("#inpower");
const in_noiseband = document.querySelector("#noiseband");
const in_nodecount = document.querySelector("#nodecount");
const in_applybutton = document.querySelector("#applybutton");

// Node list elements
const nl_button = document.querySelector("#calcbutton");
const nl_template = document.querySelector("#nodetempl");
let nl_nodelist = [];

// Report elements
const rep_gain = document.querySelector("#rep-powergain");
const rep_nfig = document.querySelector("#rep-noisefigure");
const rep_iip3 = document.querySelector("#rep-iip3");
const rep_oip3 = document.querySelector("#rep-oip3");
const rep_ip1db = document.querySelector("#rep-ip1db");
const rep_op1db = document.querySelector("#rep-op1db");
const rep_noisepower = document.querySelector("#rep-noisepower");
const rep_supplypower = document.querySelector("#rep-supplypower");
const rep_rxsens = document.querySelector("#rep-rxsens");

const onApply = function() {
    const amount = Number.parseInt(nodecount.value, 10);
    const frag = document.createDocumentFragment();
    
    for(let i = 0; i < amount; ++i) {
        const elem = nodetempl.content.cloneNode(true);
        frag.append(elem);
    }

    const nodes_list = document.querySelector("#nodes-list");
    nodes_list.innerText = "";
    nodes_list.append(frag);

    document.querySelector("#nodes").classList.remove("hidden");
    document.querySelector("#report").classList.add("hidden");
};

const onCalculate = function() {
    const get_dB = function(k) { return 10.0 * Math.log10(k); };
    const get_k = function(db) { return Math.pow(10.0, db / 10.0); };
    const boltz = 1.38e-23;
    const temp = 290.0;

    const val_inpower = Number.parseFloat(in_inpower.value);
    const val_noiseband = Number.parseFloat(in_noiseband.value);
    const val_nodes = document.querySelectorAll(".node-class");

    let casc_gain = 0.0;
    let casc_nfig = 0.0;
    let casc_iip3 = Number.NaN;
    let casc_oip3 = Number.NaN;
    let casc_ip1db = Number.NaN;
    let casc_op1db = Number.NaN;
    let casc_spower = 0.0;
    let casc_outpower = 0.0;
    let casc_noisepower = 0.0;
    let casc_rxsens = 0.0;

    nl_nodelist = [];

    for(let i = 0; i < val_nodes.length; ++i) {
        let obj_node = new RxNode();
        obj_node.parse(val_nodes[i]);
        nl_nodelist.push(obj_node);
    }

    //
    // CASCADED POWER GAIN
    //
    for(let i = 0; i < val_nodes.length; ++i) {
        casc_gain += nl_nodelist[i].gain;
        nl_nodelist[i].c_gain = casc_gain;
    }

    //
    // CASCADED NOISE FIGURE
    //
    for(let i = 0, denom = 1.0; i < val_nodes.length; ++i) {
        if(i === 0)
            casc_nfig += (get_k(nl_nodelist[i].nfig));
        else
            casc_nfig += (get_k(nl_nodelist[i].nfig) - 1.0) / denom;
        nl_nodelist[i].c_nfig = get_dB(casc_nfig);
        denom *= get_k(nl_nodelist[i].gain);
    }

    casc_nfig = get_dB(casc_nfig);

    //
    // CASCADED INPUT POWER
    //
    for(let i = 0, acc = 0.0; i < val_nodes.length; ++i) {
        if(i === 0)
            nl_nodelist[i].c_inpower = in_inpower;
        else
            nl_nodelist[i].c_inpower = acc;
        acc = nl_nodelist[i].c_inpower + nl_nodelist[i].gain;
        nl_nodelist[i].c_outpower = acc;
    }

    casc_outpower = val_inpower + casc_gain;

    //
    // CASCADED SUPPLY POWER
    //
    for(let i = 0; i < val_nodes.length; ++i) {
        casc_spower += nl_nodelist[i].voltage * nl_nodelist[i].current;
        nl_nodelist[i].c_spower = casc_spower;
    }

    //
    // CASCADED OP1dB/IP1dB
    //
    for(let i = 0; i < val_nodes.length; ++i) {
        if(i === 0)
            casc_op1db = get_k(nl_nodelist[i].op1db);
        else
            casc_op1db = 1.0 / ((1.0 / casc_op1db / get_k(nl_nodelist[i].gain)) + (1.0 / get_k(nl_nodelist[i].op1db)));
        nl_nodelist[i].c_op1db = get_dB(casc_op1db);
        nl_nodelist[i].c_ip1db = nl_nodelist[i].c_op1db - nl_nodelist[i].gain + 1.0;
    }

    casc_op1db = get_dB(casc_op1db);
    casc_ip1db = casc_op1db - casc_gain + 1.0;

    //
    // CASCADED IIP3/OIP3
    //
    for(let i = 0; i < val_nodes.length; ++i) {
        if(i === 0)
            casc_oip3 = get_k(nl_nodelist[i].oip3);
        else
            casc_oip3 = 1.0 / ((1.0 / casc_oip3 / get_k(nl_nodelist[i].gain)) + (1.0 / get_k(nl_nodelist[i].oip3)));
        nl_nodelist[i].c_oip3 = get_dB(casc_oip3);
        nl_nodelist[i].c_iip3 = nl_nodelist[i].c_oip3 - nl_nodelist[i].gain;
    }

    casc_oip3 = get_dB(casc_oip3);
    casc_iip3 = casc_oip3 - casc_gain;

    casc_noisepower = get_dB(boltz * temp * val_noiseband * 1000.0);
    casc_rxsens = casc_noisepower + casc_nfig;
    console.log(casc_outpower);
    console.log(casc_noisepower);
    console.log(boltz * temp * val_noiseband);

    rep_gain.value = casc_gain.toFixed(3);
    rep_nfig.value = casc_nfig.toFixed(3);
    rep_iip3.value = casc_iip3.toFixed(3);
    rep_oip3.value = casc_oip3.toFixed(3);
    rep_ip1db.value = casc_ip1db.toFixed(3);
    rep_op1db.value = casc_op1db.toFixed(3);
    rep_noisepower.value = casc_noisepower.toFixed(3);
    rep_supplypower.value = casc_spower.toFixed(2);
    rep_rxsens.value = casc_rxsens.toFixed(3);

    document.querySelector("#report").classList.remove("hidden");
};

in_applybutton.addEventListener("click", onApply);
nl_button.addEventListener("click", onCalculate);
