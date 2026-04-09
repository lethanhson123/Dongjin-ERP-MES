let IsTableSort = false;
let BaseResult = new Object();
let ORDR = 0;

$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C02_MT_Close", 0);

    ORDR = Number(localStorage.getItem("C02_MT_ORDR"));
    document.getElementById("Buttonsave").disabled = !Boolean(localStorage.getItem("C02_MT_Buttonsave_Enabled"));

    C02_MT_Load();

});
function C02_MT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDR);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_MT/C02_MT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = data.Search;
            $("#OR_IDX").val(ORDR);
            if (BaseResult.Search.length > 0) {
                $("#Label18").val("LEAD NAME : " + BaseResult.Search[0].LEAD_NO);
                $("#Label19").val("WORK QTY : " + BaseResult.Search[0].TOT_QTY);
                $("#Label9").val(BaseResult.Search[0].TERM1);
                $("#Label17").val(BaseResult.Search[0].T1_QTY);
                $("#Label8").val(BaseResult.Search[0].TERM2);
                $("#Label16").val(BaseResult.Search[0].T2_QTY);
                $("#Label7").val(BaseResult.Search[0].SEAL1);
                $("#Label15").val(BaseResult.Search[0].S1_QTY);
                $("#Label6").val(BaseResult.Search[0].SEAL2);
                $("#Label14").val(BaseResult.Search[0].S2_QTY);
                $("#Label5").val(BaseResult.Search[0].WIRE);
                $("#Label20").val(BaseResult.Search[0].WIRE_NM + " " + BaseResult.Search[0].W_Length);
                $("#Label13").val(BaseResult.Search[0].W_Length1 + "M");

                $("#T1_OR").val(BaseResult.Search[0].T1_ORDER);
                $("#T1_CP").val(BaseResult.Search[0].T1_COMP);
                $("#T2_OR").val(BaseResult.Search[0].T2_ORDER);
                $("#T2_CP").val(BaseResult.Search[0].T2_COMP);
                $("#S1_OR").val(BaseResult.Search[0].S1_ORDER);
                $("#S1_CP").val(BaseResult.Search[0].S1_COMP);
                $("#S2_OR").val(BaseResult.Search[0].S2_ORDER);
                $("#S2_CP").val(BaseResult.Search[0].S2_COMP);
                $("#WR_OR").val(BaseResult.Search[0].WR_ORDER);
                $("#WR_CP").val(BaseResult.Search[0].WR_COMP);

                try {
                    let T1_TXT = BaseResult.Search[0].TERM1;
                    if (T1_TXT.indexOf("(") > 0) {
                        document.getElementById("TM_CB1").checked = false;
                    }
                    else {
                        if (T1_TXT.length > 0) {
                            document.getElementById("TM_CB1").checked = true;
                        }
                        else {
                            document.getElementById("TM_CB1").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("TM_CB1").checked = false;
                }
                try {
                    let T2_TXT = BaseResult.Search[0].TERM2;
                    if (T2_TXT.indexOf("(") > 0) {
                        document.getElementById("TM_CB2").checked = false;
                    }
                    else {
                        if (T2_TXT.length > 0) {
                            document.getElementById("TM_CB2").checked = true;
                        }
                        else {
                            document.getElementById("TM_CB2").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("TM_CB2").checked = false;
                }
                try {
                    let S1_TXT = BaseResult.Search[0].SEAL1;
                    if (S1_TXT.indexOf("(") > 0) {
                        document.getElementById("SL_CB1").checked = false;
                    }
                    else {
                        if (S1_TXT.length > 0) {
                            document.getElementById("SL_CB1").checked = true;
                        }
                        else {
                            document.getElementById("SL_CB1").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("SL_CB1").checked = false;
                }
                try {
                    let S2_TXT = BaseResult.Search[0].SEAL2;
                    if (S2_TXT.indexOf("(") > 0) {
                        document.getElementById("SL_CB2").checked = false;
                    }
                    else {
                        if (S2_TXT.length > 0) {
                            document.getElementById("SL_CB2").checked = true;
                        }
                        else {
                            document.getElementById("SL_CB2").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("SL_CB2").checked = false;
                }
                try {
                    let WR_TXT = BaseResult.Search[0].WIRE;
                    if (WR_TXT.indexOf("(") > 0) {
                        document.getElementById("WR_CB").checked = false;
                    }
                    else {
                        if (WR_TXT.length > 0) {
                            document.getElementById("WR_CB").checked = true;
                        }
                        else {
                            document.getElementById("WR_CB").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("WR_CB").checked = false;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonsave").click(function (e) {
    Buttonsave_Click();
});
function Buttonsave_Click() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDR);
    BaseParameter.ListSearchString.push(SettingsMC_NM);
    BaseParameter.TM_CB1 = document.getElementById("TM_CB1").checked;
    BaseParameter.TM_CB2 = document.getElementById("TM_CB2").checked;
    BaseParameter.SL_CB1 = document.getElementById("SL_CB1").checked;
    BaseParameter.SL_CB1 = document.getElementById("SL_CB1").checked;
    BaseParameter.WR_CB = document.getElementById("WR_CB").checked;

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_MT/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            alert("정상처리 되었습니다. Đã được lưu.");
            Buttonclose_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#TM_CB1").click(function (e) {
    TM_CB1_Click();
});
function TM_CB1_Click() {
    let TM_CB1 = document.getElementById("TM_CB1").checked;
    if (TM_CB1 == true) {
        try {
            let T1_TXT = $("#Label9").val();
            if (T1_TXT.indexOf("(") > 0) {
                document.getElementById("TM_CB1").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("TM_CB1").checked = true;
                }
                else {
                    document.getElementById("TM_CB1").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#TM_CB2").click(function (e) {
    TM_CB2_Click();
});
function TM_CB2_Click() {
    let TM_CB2 = document.getElementById("TM_CB2").checked;
    if (TM_CB2 == true) {
        try {
            let T1_TXT = $("#Label8").val();
            if (T1_TXT.indexOf("(") > 0) {
                document.getElementById("TM_CB2").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("TM_CB2").checked = true;
                }
                else {
                    document.getElementById("TM_CB2").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#SL_CB1").click(function (e) {
    SL_CB1_Click();
});
function SL_CB1_Click() {
    let SL_CB1 = document.getElementById("SL_CB1").checked;
    if (SL_CB1 == true) {
        try {
            let T1_TXT = $("#Label7").val();
            if (T1_TXT.indexOf("(") > 0) {
                document.getElementById("SL_CB1").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("SL_CB1").checked = true;
                }
                else {
                    document.getElementById("SL_CB1").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#SL_CB2").click(function (e) {
    SL_CB2_Click();
});
function SL_CB2_Click() {
    let SL_CB2 = document.getElementById("SL_CB2").checked;
    if (SL_CB2 == true) {
        try {
            let T1_TXT = $("#Label6").val();
            if (T1_TXT.indexOf("(") > 0) {
                document.getElementById("SL_CB2").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("SL_CB2").checked = true;
                }
                else {
                    document.getElementById("SL_CB2").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#Label5").click(function (e) {
    Label5_Click();
});
function Label5_Click() {
    let WR_CB = document.getElementById("WR_CB").checked;
    if (WR_CB == true) {
        try {
            let T1_TXT = $("#Label5").val();
            if (T1_TXT.indexOf("(") > 0) {
                document.getElementById("WR_CB").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("WR_CB").checked = true;
                }
                else {
                    document.getElementById("WR_CB").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}

$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C02_MT_Close", 1);
    window.close();
}
