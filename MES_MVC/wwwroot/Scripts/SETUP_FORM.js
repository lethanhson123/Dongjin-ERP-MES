let IsTableSort = false;
let BaseResult = new Object();
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {
    localStorage.setItem("SETUP_FORM_Close", 0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MC_Label").val(SettingsMC_NM);
    document.getElementById("A_RD_01").checked = true;
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("SETUP_FORM_Close", 1);
    window.close();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    SettingsMC_NM = $("#ComboBox3").val(); 
    localStorage.setItem("SettingsMC_NM", SettingsMC_NM);    
    Buttonclose_Click();
}
$("#A_RD_01").click(function () {
    A_RD_01_Click();
});
function A_RD_01_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "A_01";
    document.getElementById("A_RD_01").checked = true;
    CB_LOAD();
}
$("#A_RD_02").click(function () {
    A_RD_02_Click();
});
function A_RD_02_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "A_02";
    document.getElementById("A_RD_02").checked = true;
    CB_LOAD();
}
$("#B_RD_01").click(function () {
    B_RD_01_Click();
});
function B_RD_01_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "B_01";
    document.getElementById("B_RD_01").checked = true;
    CB_LOAD();
}
$("#B_RD_02").click(function () {
    B_RD_02_Click();
});
function B_RD_02_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "B_02";
    document.getElementById("B_RD_02").checked = true;
    CB_LOAD();
}
$("#B_RD_03").click(function () {
    B_RD_03_Click();
});
function B_RD_03_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "B_03";
    document.getElementById("B_RD_03").checked = true;
    CB_LOAD();
}
$("#B_RD_04").click(function () {
    B_RD_04_Click();
});
function B_RD_04_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "B_04";
    document.getElementById("B_RD_04").checked = true;
    CB_LOAD();
}
$("#B_RD_05").click(function () {
    B_RD_05_Click();
});
function B_RD_05_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "B_05";
    document.getElementById("B_RD_05").checked = true;
    CB_LOAD();
}
$("#C_RD_01").click(function () {
    C_RD_01_Click();
});
function C_RD_01_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "C_01";
    document.getElementById("C_RD_01").checked = true;
    CB_LOAD();
}
$("#C_RD_02").click(function () {
    C_RD_02_Click();
});
function C_RD_02_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "C_02";
    document.getElementById("C_RD_02").checked = true;
    CB_LOAD();
}
$("#C_RD_03").click(function () {
    C_RD_03_Click();
});

$("#C_RD_04").click(function () {
    C_RD_04_Click();
});

function C_RD_03_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "C_03";
    document.getElementById("C_RD_03").checked = true;
    CB_LOAD();
}

function C_RD_04_Click() {
    ALL_RD_FALSE();
    SettingsMC_LIST_GRP = "C_04";
    document.getElementById("C_RD_04").checked = true;
    CB_LOAD();
}

function ALL_RD_FALSE() {
}
function CB_LOAD() {
    DB_MC_LIST();
}
function DB_MC_LIST() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_LIST_GRP,
    }
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/SETUP_FORM/DB_MC_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;

            $("#ComboBox3").empty();

            var ComboBox3 = document.getElementById("ComboBox3");
            if (BaseResult.Search.length > 0) {
                for (let i = 0; i < BaseResult.Search.length; i++) {
                    var option = document.createElement("option");
                    option.value = BaseResult.Search[i].CD_NM_EN;
                    option.text = BaseResult.Search[i].CD_NM_EN;
                    ComboBox3.appendChild(option);
                }
            }
            else {
                var option = document.createElement("option");
                option.value = "---";
                option.text = "---";
                ComboBox3.appendChild(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}