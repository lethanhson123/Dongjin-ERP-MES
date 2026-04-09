let BaseResult = {};
let Timer1;

$(document).ready(function () {
    DT_LOOAD();
    ANDON_LIST();
    Timer1 = setInterval(function () {
        DT_LOOAD();
        ANDON_LIST();
    }, 10000);
});

$("#Buttonfind").click(Buttonfind_Click);
$("#Buttonadd").click(Buttonadd_Click);
$("#Buttonsave").click(Buttonsave_Click);
$("#Buttondelete").click(Buttondelete_Click);
$("#Buttoncancel").click(Buttoncancel_Click);
$("#Buttoninport").click(Buttoninport_Click);
$("#Buttonexport").click(Buttonexport_Click);
$("#Buttonprint").click(Buttonprint_Click);
$("#Buttonhelp").click(Buttonhelp_Click);
$("#Buttonclose").click(Buttonclose_Click);

function DT_LOOAD() {
    $("#BackGround").css("display", "block");
    let url = "/H11/GetMachineStatus";
    fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            updateMachineStatus();
            $("#BackGround").css("display", "none");
        }).catch(() => {
            $("#BackGround").css("display", "none");
        });
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function ANDON_LIST() {
    updateErrorDisplay();
}

function updateMachineStatus() {
    if (BaseResult && BaseResult.DataGridView1) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let MC_NAME = BaseResult.DataGridView1[i].MC_NO;
            let MC_PQTY = BaseResult.DataGridView1[i].SUM;
            let MC_DSYN = BaseResult.DataGridView1[i].tsnon_oper_mitor_RUNYN;
            switch (MC_NAME) {
                case "ZA801":
                    setPanelColor("#Panel1", MC_PQTY, MC_DSYN); break;
                case "ZA802":
                    setPanelColor("#Panel2", MC_PQTY, MC_DSYN); break;
                case "ZA803":
                    setPanelColor("#Panel3", MC_PQTY, MC_DSYN); break;
                case "ZA804":
                    setPanelColor("#Panel4", MC_PQTY, MC_DSYN); break;
                case "ZA805":
                    setPanelColor("#Panel5", MC_PQTY, MC_DSYN); break;
                case "ZA806":
                    setPanelColor("#Panel6", MC_PQTY, MC_DSYN); break;
                case "ZA807":
                    setPanelColor("#Panel7", MC_PQTY, MC_DSYN); break;
                case "ZA808":
                    setPanelColor("#Panel8", MC_PQTY, MC_DSYN); break;
                case "ZA809":
                    setPanelColor("#Panel9", MC_PQTY, MC_DSYN); break;
                case "ZA810":
                    setPanelColor("#Panel10", MC_PQTY, MC_DSYN); break;
                case "ZA811":
                    setPanelColor("#Panel11", MC_PQTY, MC_DSYN); break;
                case "ZA812":
                    setPanelColor("#Panel12", MC_PQTY, MC_DSYN); break;
                case "ZA813":
                    setPanelColor("#Panel13", MC_PQTY, MC_DSYN); break;
                case "ZA814":
                    setPanelColor("#Panel14", MC_PQTY, MC_DSYN); break;
                case "ZA815":
                    setPanelColor("#Panel15", MC_PQTY, MC_DSYN); break;
                case "ZA816":
                    setPanelColor("#Panel16", MC_PQTY, MC_DSYN); break;
                case "ZZ101":
                    setPanelColor("#Panel29", MC_PQTY, MC_DSYN); break;
                case "ZZ102":
                    setPanelColor("#Panel30", MC_PQTY, MC_DSYN); break;
                case "ZZ103":
                    setPanelColor("#Panel31", MC_PQTY, MC_DSYN); break;
                case "ZZ104":
                    setPanelColor("#Panel32", MC_PQTY, MC_DSYN); break;
                case "ZZ105":
                    setPanelColor("#Panel33", MC_PQTY, MC_DSYN); break;
                case "ZZ106":
                    setPanelColor("#Panel34", MC_PQTY, MC_DSYN); break;
                case "ZZ107":
                    setPanelColor("#Panel35", MC_PQTY, MC_DSYN); break;
                case "ZS101":
                    setPanelColor("#Panel43", MC_PQTY, MC_DSYN); break;
                case "ZS102":
                    setPanelColor("#Panel44", MC_PQTY, MC_DSYN); break;
                case "ZS103":
                    setPanelColor("#Panel45", MC_PQTY, MC_DSYN); break;
                case "ZS104":
                    setPanelColor("#Panel46", MC_PQTY, MC_DSYN); break;
                case "ZS105":
                    setPanelColor("#Panel47", MC_PQTY, MC_DSYN); break;
                case "ZS106":
                    setPanelColor("#Panel48", MC_PQTY, MC_DSYN); break;
                case "ZS107":
                    setPanelColor("#Panel49", MC_PQTY, MC_DSYN); break;
            }
        }
    }
}

function setPanelColor(panelId, pqty, dsyn) {
    if (pqty > 0) {
        $(panelId).css("background-color", "lime");
    } else {
        $(panelId).css("background-color", "silver");
    }
    if (dsyn === "Y") {
        $(panelId).css("background-color", "red");
    } else if (dsyn === "I") {
        $(panelId).css("background-color", "gold");
    }
}

function updateErrorDisplay() {
    for (let i = 1; i <= 8; i++) {
        $("#ERR_LEB0" + i).text("").css("background-color", "white");
    }
    if (BaseResult && BaseResult.DataGridView2) {
        for (let i = 0; i < BaseResult.DataGridView2.length && i < 8; i++) {
            let leb = "#ERR_LEB0" + (i + 1);
            $(leb).text(BaseResult.DataGridView2[i].tsnon_oper_mitor_MCNM);
            if (BaseResult.DataGridView2[i].tsnon_oper_mitor_RUNYN === "Y") {
                $(leb).css("background-color", "red");
            } else if (BaseResult.DataGridView2[i].tsnon_oper_mitor_RUNYN === "I") {
                $(leb).css("background-color", "gold");
            } else {
                $(leb).css("background-color", "white");
            }
        }
    }
}

function Buttonfind_Click() { DT_LOOAD(); ANDON_LIST(); }
function Buttonadd_Click() { }
function Buttonsave_Click() { }
function Buttondelete_Click() { }
function Buttoncancel_Click() { }
function Buttoninport_Click() { }
function Buttonexport_Click() { }
function Buttonprint_Click() { }
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() { history.back(); }
