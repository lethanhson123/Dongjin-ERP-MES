let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let Timer1;
let Timer2;
let Timer3;
let Timer4;
let TI_CONT = 0;
let StartTime;
let RunTime;
let SPC_EXIT;
let DC_STR;
let BARCODE_QR;
let MCBOX;
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {


    localStorage.setItem("C05_START_Close", 0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    MCBOX = SettingsMC_NM;
    $("#Label3").val(localStorage.getItem("C05_START_Label3"));
    $("#Label4").val(localStorage.getItem("C05_START_Label4"));
    $("#Label8").val(localStorage.getItem("C05_START_Label8"));
    $("#Label42").val(localStorage.getItem("C05_START_Label42"));
    $("#Label43").val(localStorage.getItem("C05_START_Label43"));
    $("#L_MCNM").val(localStorage.getItem("C05_START_L_MCNM"));
    $("#Label48").val(localStorage.getItem("C05_START_Label48"));
    $("#Label50").val(localStorage.getItem("C05_START_Label50"));
    $("#Label77").val(localStorage.getItem("C05_START_Label77"));

    document.getElementById("SPC1").innerHTML = "First";
    document.getElementById("SPC2").innerHTML = "Middle";
    document.getElementById("SPC3").innerHTML = "End";

    document.getElementById("BT_APP1").disabled = true;
    document.getElementById("BT_APP2").disabled = true;
    $("#VLA1").val("");
    $("#VLA2").val("");
    $("#VLA11").val("");
    $("#VLA22").val("");
    document.getElementById("Buttonprint").disabled = true;
    C05_start_Load();
    ORDER_LOAD();
    DB_COUTN();
    Lan_Change();
    document.getElementById("RadioButton1").checked = true;
    OPER_TIME();
    Timer2StartInterval();
    Timer1StartInterval();
    Timer4StartInterval();
    $("#TB_BARCODE").val("");
});
function C05_start_Load() {

    let Label8 = $("#Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(SettingsMC_NM);
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/C05_start_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_FormClosed() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/C05_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            localStorage.setItem("C05_START_Close", 1);
            window.close();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function ORDER_LOAD() {
    let IsCheck = true;
    let LH_CHK = false;
    let RH_CHK = false;
    document.getElementById("BT_APP1").disabled = false;
    document.getElementById("BT_APP2").disabled = false;

    let Label8 = $("#Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: Label8,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView = BaseResultSub.DataGridView;
            BaseResult.Search = BaseResultSub.Search;
            BaseResult.Search1 = BaseResultSub.Search1;
            if (BaseResult.Search.length <= 0) {
                let COM_COUNT = BaseResult.Search1.length;
                if (COM_COUNT == 0) {
                    IsCheck = false;
                    alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                }
                else {
                    IsCheck = false;
                    alert("등록된 자료가 없습니다. Không có dữ liệu đăng ký MES.");
                }
            }
            if (IsCheck == true) {
                StartTime = $("#Label70").val();
                $("#Label10").val(BaseResult.Search[0].TERM1);
                $("#VLT1").val(BaseResult.Search[0].TERM1);
                $("#Label14").val(BaseResult.Search[0].TERM2);
                $("#VLT2").val(BaseResult.Search[0].TERM2);
                $("#Label12").val(BaseResult.Search[0].SEAL1);
                $("#VLS1").val(BaseResult.Search[0].SEAL1);
                $("#Label16").val(BaseResult.Search[0].SEAL2);
                $("#VLS2").val(BaseResult.Search[0].SEAL2);
                $("#Label18").val(BaseResult.Search[0].WIRE);
                $("#VLW").val(BaseResult.Search[0].WIRE);
                $("#ST_DSTR1").val(BaseResult.Search[0].STRIP1);
                $("#ST_DSTR2").val(BaseResult.Search[0].STRIP2);
                $("#Label24").val(BaseResult.Search[0].CCH_W1);
                $("#Label28").val(BaseResult.Search[0].ICH_W1);
                $("#Label26").val(BaseResult.Search[0].CCH_W2);
                $("#Label30").val(BaseResult.Search[0].ICH_W2);
                $("#Label48").val(BaseResult.Search[0].TOT_QTY);
                $("#Label59").val(BaseResult.Search[0].PERFORMN);
                $("#Label42").val(BaseResult.Search[0].SP_ST);
                $("#Label43").val(BaseResult.Search[0].PROJECT);
                $("#Label54").val(BaseResult.Search[0].HOOK_RACK);
                $("#barsq").val(BaseResult.Search[0].Barcode_SEQ);
                $("#Label77").val(BaseResult.Search[0].ADJ_AF_QTY);
                $("#TOOL_CONT1").val(BaseResult.Search[0].T1_CONT);
                $("#TOOL_CONT2").val(BaseResult.Search[0].T2_CONT);
                $("#Label50").val(BaseResult.Search[0].BUNDLE_SIZE);
                $("#T1_IDX").val(BaseResult.Search[0].T1_PN_IDX);
                $("#T2_IDX").val(BaseResult.Search[0].T2_PN_IDX);
                $("#S1_IDX").val(BaseResult.Search[0].S1_PN_IDX);
                $("#S2_IDX").val(BaseResult.Search[0].S2_PN_IDX);
                $("#WIRE_IDX").val(BaseResult.Search[0].W_PN_IDX);
                $("#TOOL1_IDX").val(BaseResult.Search[0].T1_TOO_IDX);
                $("#TOOL2_IDX").val(BaseResult.Search[0].T2_TOOL_IDX);
                $("#COUNT_LH").val(BaseResult.Search[0].PERFORMN_L);
                $("#COUNT_RH").val(BaseResult.Search[0].PERFORMN_R);

                let T1_CHK = Number(BaseResult.Search[0].TERM1);
                let T2_CHK = Number(BaseResult.Search[0].TERM2);

                if (T1_CHK > 0) {
                    document.getElementById("RB_LH").disabled = false;
                    let NOT_TERM1 = BaseResult.Search[0].TERM1.replace("(", "").replace(")", "");
                    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
                        if (BaseResult.DataGridView[i].CLIMP_TERM == NOT_TERM1) {
                            document.getElementById("RB_LH").disabled = true;
                            document.getElementById("BT_APP1").disabled = true;
                            document.getElementById("RB_RH").checked = true;
                            LH_CHK = true;
                        }
                    }
                }
                else {
                    document.getElementById("RB_LH").disabled = true;
                    document.getElementById("BT_APP1").disabled = true;
                    LH_CHK = true;
                    document.getElementById("RB_RH").checked = true;
                }
                if (T2_CHK > 0) {
                    document.getElementById("RB_RH").disabled = false;
                    let NOT_TERM2 = BaseResult.Search[0].TERM2.replace("(", "").replace(")", "");
                    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
                        if (BaseResult.DataGridView[i].CLIMP_TERM == NOT_TERM2) {
                            document.getElementById("RB_RH").disabled = true;
                            document.getElementById("BT_APP2").disabled = true;
                            document.getElementById("RB_LH").checked = true;
                            RH_CHK = true;
                        }
                    }
                }
                else {
                    document.getElementById("RB_RH").disabled = true;
                    document.getElementById("BT_APP2").disabled = true;
                    RH_CHK = true;
                    document.getElementById("RB_LH").checked = true;
                }
                let T1_NM = BaseResult.Search[0].T1_NM;
                if (T1_NM == "") {
                    $("#VLA1").val("");
                    $("#VLA11").val("");
                    $("#TOOL_CONT1").val("");
                }
                else {
                    $("#VLA1").val(BaseResult.Search[0].TERM1);
                    $("#VLA11").val(BaseResult.Search[0].T1_NM);
                }
                let BB0 = BaseResult.Search[0].WIRE;
                if (BB0 == "") {
                }
                else {
                    let PO1 = BB0.indexOf("  ") - BB0.indexOf(" ") - 1;
                    let TT2 = BB0.substring(BB0.indexOf(" ") + 1, PO1);
                    $("#ST_DWIRE1").val(TT2);
                }
                let T2_NM = BaseResult.Search[0].T2_NM;
                if (T2_NM == "") {
                    $("#VLA2").val("");
                    $("#VLA22").val("");
                    $("#TOOL_CONT2").val("");
                }
                else {
                    $("#VLA2").val(BaseResult.Search[0].TERM2);
                    $("#VLA22").val(BaseResult.Search[0].T2_NM);
                }
                try {
                    let TOOL_CONT1 = $("#TOOL_CONT1").val();
                    if (TOOL_CONT1 == "") {
                        document.getElementById("TOOL_CONT1").style.backgroundColor = "black";
                    }
                    else {
                        let T1_CONT = BaseResult.Search[0].T1_CONT;
                        let TOOL_MAX1 = BaseResult.Search[0].TOOL_MAX1;
                        TOOL_MAX1 = TOOL_MAX1 * 0.9;
                        if (T1_CONT <= TOOL_MAX1) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "lime";
                        }
                        if (T1_CONT > TOOL_MAX1) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "yellow";
                        }
                        TOOL_MAX1 = BaseResult.Search[0].TOOL_MAX1;
                        TOOL_MAX1 = TOOL_MAX1 * 1.0;
                        if (T1_CONT >= TOOL_MAX1) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "red";
                        }
                    }
                }
                catch (e) {
                    $("#TOOL_CONT1").val("ERROR");
                    document.getElementById("TOOL_CONT1").style.backgroundColor = "red";
                }
                try {
                    let TOOL_CONT2 = $("#TOOL_CONT2").val();
                    if (TOOL_CONT2 == "") {
                        document.getElementById("TOOL_CONT2").style.backgroundColor = "black";
                    }
                    else {
                        let T2_CONT = BaseResult.Search[0].T2_CONT;
                        let TOOL_MAX2 = BaseResult.Search[0].TOOL_MAX2;
                        TOOL_MAX2 = TOOL_MAX2 * 0.9;
                        if (T2_CONT <= TOOL_MAX2) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "lime";
                        }
                        if (T2_CONT > TOOL_MAX2) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "yellow";
                        }
                        TOOL_MAX2 = BaseResult.Search[0].TOOL_MAX2;
                        TOOL_MAX2 = TOOL_MAX2 * 1.0;
                        if (T2_CONT >= TOOL_MAX2) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "red";
                        }
                    }
                }
                catch (e) {
                    $("#TOOL_CONT2").val("ERROR");
                    document.getElementById("TOOL_CONT2").style.backgroundColor = "red";
                }
                let AAA = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN;
                let BUNDLE_SIZE = BaseResult.Search[0].BUNDLE_SIZE;
                if (AAA >= BUNDLE_SIZE) {
                    $("#Label50").val(BUNDLE_SIZE);
                }
                else {
                    $("#Label50").val(AAA);
                }
                let LH = BaseResult.Search[0].LH;
                if (LH == "L") {
                    document.getElementById("RB_LH").disabled = true;
                    document.getElementById("RB_LHText").innerHTML = "Complete";
                    document.getElementById("RB_RH").checked = true;
                }
                else {
                    let RB_LH = document.getElementById("RB_LH").checked;
                    if (RB_LH == true) {
                        document.getElementById("RB_LH").checked = true;
                    }
                    else {
                        document.getElementById("RB_RH").checked = true;
                    }
                }
                let RH = BaseResult.Search[0].RH;
                if (RH == "R") {
                    document.getElementById("RB_RH").disabled = true;
                    document.getElementById("RB_RHText").innerHTML = "Complete";
                    document.getElementById("RB_LH").checked = true;
                }
                else {
                    let RB_LH = document.getElementById("RB_LH").checked;
                    if (RB_LH == true) {
                        document.getElementById("RB_LH").checked = true;
                    }
                    else {
                        document.getElementById("RB_RH").checked = true;
                    }
                }

                if (LH_CHK == true && RH_CHK == true) {
                    alert("작업을 종료 하였습니다. Đã dừng làm việc. It is not a list of orders for LP. Please Check Barcode Again.");
                    Buttonclose_Click();
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function DB_COUTN() {
    $("#Label65").val(0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/DB_COUTN";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView1 = BaseResultSub.DataGridView1;
            let TOT_QTY = BaseResult.DataGridView1[0].SUM;
            if (TOT_QTY <= 0) {
                TOT_QTY = 0;
            }
            else {
            }
            $("#Label65").val(TOT_QTY);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Lan_Change() {
    Form_Label2();
}
function Form_Label2() {

}
function OPER_TIME() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView2 = BaseResultSub.DataGridView2;
            let TOT_SUM = 0;
            try {
                TOT_SUM = BaseResult.DataGridView2[0].SUM_TIME;
            }
            catch (e) {
                TOT_SUM = 0;
            }
            try {
                let H_TIME = TOT_SUM / 60 / 60;
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = TOT_SUM / 60;
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let HH = Math.floor(H_TIME).toString().padStart(2, '0');
                let MM = Math.floor(M_TIME).toString().padStart(2, '0');
                let SS = Math.floor(S_TIME).toString().padStart(2, '0');
                let HH_MM_SS = HH + ":" + MM + ":" + SS;
                $("#Label71").val(HH_MM_SS);
            }
            catch (e) {
                $("#Label71").val("00:00:00");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    C05_FormClosed();
}

$("#TB_BARCODE").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    let Buttonprint = document.getElementById("Buttonprint").disabled;
    if (Buttonprint == true) {
        Buttonprint_Click();
        $("#TB_BARCODE").val("");
        $("#TB_BARCODE").focus();
    }
    else {
        M.toast({ html: "실적 처리 대기 중. Chờ đợi.", classes: 'green', displayLength: 6000 });
        $("#TB_BARCODE").val("");
        $("#TB_BARCODE").focus();
    }
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let TERM_C = "-";
    let RB_LH = document.getElementById("RB_LH").checked;
    let RB_RH = document.getElementById("RB_RH").checked;
    if (RB_LH == true) {
        TERM_C = "LH"
    }
    if (RB_RH == true) {
        TERM_C = "RH"
    }
    localStorage.setItem("C05_DC_READ_Label2", TERM_C);
    localStorage.setItem("C05_DC_READ_Close", 0);
    let url = "/C05_DC_READ";
    OpenWindowByURL(url, 800, 460);
    C05_DC_READTimerStartInterval();
}
function C05_DC_READTimerStartInterval() {
    let C05_DC_READTimer = setInterval(function () {
        let C05_DC_READ_Close = localStorage.getItem("C05_DC_READ_Close");
        if (C05_DC_READ_Close == "1") {
            clearInterval(C05_DC_READTimer);
            C05_DC_READ_Close = 0;
            localStorage.setItem("C05_DC_READ_Close", C05_DC_READ_Close);
        }
    }, 100);
}

$("#Label18").change(function (e) {
    Label18_TextChanged();
});
function Label18_TextChanged() {
    let aaw = $("#Label18").val();
    aaw = aaw.trim();
    let bbw = aaw.substring(aaw.indexOf("  "));
    bbw = bbw.trim();
    let ccw = bbw.substring(bbw.indexOf("  "));
    $("#WIRE_Length").val(ccw);
}
$("#BT_APP1").click(function (e) {
    Button4_Click();
});
function Button4_Click() {
    let ERR_CHK_LH = false;
    let RB_LH = document.getElementById("RB_LH").disabled;
    if (RB_LH == false) {
        let VLA1 = $("#VLA1").val();
        if (VLA1 == "") {
            ERR_CHK_LH = true;
        }
    }
    RB_LH = document.getElementById("RB_LH").checked;
    if (RB_LH == true) {
        if (ERR_CHK_LH == true) {
            let Label8 = $("#Label8").val();
            let Label10 = $("#Label10").val();
            Label10 = Label10.replace("(", "").replace(")", "");
            localStorage.setItem("C05_ER_L_Close", 0);
            localStorage.setItem("C05_ER_L_LBT2S", Label10);
            localStorage.setItem("C05_ER_L_LBA2S", Label10);
            localStorage.setItem("C05_ER_L_Label1", Label8);
            let url = "/C05_ER_L";
            OpenWindowByURL(url, 800, 460);
            C05_ER_LTimerStartInterval();
        }
        else {
            let VLA1 = $("#VLA1").val();
            let VLA11 = $("#VLA11").val();
            let Label8 = $("#Label8").val();
            let Label10 = $("#Label10").val();
            localStorage.setItem("C05_APPLICATION_Close", 0);
            localStorage.setItem("C05_APPLICATION_Label3", "APPLICATION #1");
            localStorage.setItem("C05_APPLICATION_Label1", VLA1);
            localStorage.setItem("C05_APPLICATION_Label2", VLA11);
            if (VLA1 == "") {
                localStorage.setItem("C05_ER_L_Close", 0);
                localStorage.setItem("C05_ER_L_LBT2S", Label10);
                localStorage.setItem("C05_ER_L_LBA2S", Label10);
                localStorage.setItem("C05_ER_L_Label1", Label8);
                let url = "/C05_ER_L";
                OpenWindowByURL(url, 800, 460);
                C05_ER_LTimerStartInterval();
            }
            else {
                let url = "/C05_APPLICATION";
                OpenWindowByURL(url, 800, 460);
                C05_APPLICATIONTimerStartInterval();
            }
        }
    }
}
function C05_ER_LTimerStartInterval() {
    let C05_ER_LTimer = setInterval(function () {
        let C05_ER_L_Close = localStorage.getItem("C05_ER_L_Close");
        if (C05_ER_L_Close == "1") {
            clearInterval(C05_ER_LTimer);
            C05_ER_L_Close = 0;
            localStorage.setItem("C05_ER_L_Close", C05_ER_L_Close);
        }
    }, 100);
}
function C05_ER_RTimerStartInterval() {
    let C05_ER_RTimer = setInterval(function () {
        let C05_ER_R_Close = localStorage.getItem("C05_ER_R_Close");
        if (C05_ER_R_Close == "1") {
            clearInterval(C05_ER_RTimer);
            C05_ER_R_Close = 0;
            localStorage.setItem("C05_ER_R_Close", C05_ER_R_Close);
        }
    }, 100);
}
function C05_APPLICATIONTimerStartInterval() {
    let C05_APPLICATIONTimer = setInterval(function () {
        let C05_APPLICATION_Close = localStorage.getItem("C05_APPLICATION_Close");
        if (C05_ER_L_Close == "1") {
            clearInterval(C05_APPLICATIONTimer);
            C05_APPLICATION_Close = 0;
            localStorage.setItem("C05_APPLICATION_Close", C05_APPLICATION_Close);
        }
    }, 100);
}
$("#BT_APP2").click(function (e) {
    Button5_Click();
});
function Button5_Click() {
    let ERR_CHK_RH = false;
    let RB_RH = document.getElementById("RB_RH").disabled;
    if (RB_RH == false) {
        let VLA2 = $("#VLA2").val();
        if (VLA2 == "") {
            ERR_CHK_RH = true;
        }
    }
    RB_RH = document.getElementById("RB_RH").checked;
    if (RB_RH == true) {
        if (ERR_CHK_RH == true) {
            let Label8 = $("#Label8").val();
            let Label14 = $("#Label14").val();
            Label14 = Label14.replace("(", "").replace(")", "");
            localStorage.setItem("C05_ER_R_Close", 0);
            localStorage.setItem("C05_ER_R_LBT2S", Label14);
            localStorage.setItem("C05_ER_R_LBA2S", Label14);
            localStorage.setItem("C05_ER_R_Label1", Label8);
            let url = "/C05_ER_R";
            OpenWindowByURL(url, 800, 460);
            C05_ER_RTimerStartInterval();
        }
        else {
            let VLA2 = $("#VLA2").val();
            let VLA22 = $("#VLA22").val();
            let Label8 = $("#Label8").val();
            let Label14 = $("#Label14").val();
            localStorage.setItem("C05_APPLICATION_Close", 0);
            localStorage.setItem("C05_APPLICATION_Label3", "APPLICATION #2");
            localStorage.setItem("C05_APPLICATION_Label1", VLA2);
            localStorage.setItem("C05_APPLICATION_Label2", VLA22);
            if (VLA1 == "") {
                localStorage.setItem("C05_ER_R_Close", 0);
                localStorage.setItem("C05_ER_R_LBT2S", Label14);
                localStorage.setItem("C05_ER_R_LBA2S", Label14);
                localStorage.setItem("C05_ER_R_Label1", Label8);
                let url = "/C05_ER_R";
                OpenWindowByURL(url, 800, 460);
                C05_ER_RTimerStartInterval();
            }
            else {
                let url = "/C05_APPLICATION";
                OpenWindowByURL(url, 800, 460);
                C05_APPLICATIONTimerStartInterval();
            }
        }
    }
}
$("#RB_RH").click(function (e) {
    RB_LH_CheckedChanged();
});
$("#RB_LH").click(function (e) {
    RB_LH_CheckedChanged();
});
function RB_LH_CheckedChanged() {
    let RB_LH = document.getElementById("RB_LH").checked;
    let RB_RH = document.getElementById("RB_RH").checked;
    if (RB_LH == true) {
        document.getElementById("LEAD_PIC").src = "/Image/LP_T1.png";
    }
    if (RB_RH == true) {
        document.getElementById("LEAD_PIC").src = "/Image/LP_T2.png";
    }
}
$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    let IsCheck = true;
    let ERR_CHK_LH = false;
    let ERR_CHK_RH = false;
    let RB_LH = document.getElementById("RB_LH").disabled;
    if (RB_LH == true) {
        let VLA1 = $("#VLA1").val();
        if (VLA1 == "") {
            ERR_CHK_LH = true;
        }
    }
    let RB_RH = document.getElementById("RB_RH").disabled;
    if (RB_RH == true) {
        let VLA2 = $("#VLA2").val();
        if (VLA2 == "") {
            ERR_CHK_RH = true;
        }
    }
    RB_LH = document.getElementById("RB_LH").checked;
    if (RB_LH == true) {
        if (ERR_CHK_LH == true) {
            IsCheck = false;
            alert("NOT ERROR PROOF. Please Check Again.");
            $("#TB_BARCODE").val("");
        }
    }
    if (IsCheck == true) {
        RB_RH = document.getElementById("RB_RH").checked;
        if (RB_RH == true) {
            if (ERR_CHK_RH == true) {
                IsCheck = false;
                alert("NOT ERROR PROOF. Please Check Again.");
                $("#TB_BARCODE").val("");
            }
        }
    }
    if (IsCheck == true) {
        let TB_BARCODE = $("#TB_BARCODE").val();
        if (TB_BARCODE == "") {
            IsCheck = false;
            alert("NOT ERROR PROOF. Please Check Again.");
            $("#TB_BARCODE").val("");
        }
        else {
            DC_STR = "";
            BARCODE_LOAD();
        }
    }
}
function BARCODE_LOAD() {
    let IsCheck = true;
    let TB_BARCODE = $("#TB_BARCODE").val();
    let Label8 = $("#Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(TB_BARCODE);
    BaseParameter.ListSearchString.push(DC_STR);
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/BARCODE_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView4 = BaseResultSub.DataGridView4;
            if (BaseResult.DataGridView4.length <= 0) {
                IsCheck = false;
                alert("NOT Order Barcode(Wrong). Please Check Again.");
                $("#TB_BARCODE").val("");
                $("#TB_BARCODE").focus();
            }
            if (IsCheck == true) {
                let DSCN_YN = result.DataGridView4[0].DSCN_YN;
                if (DSCN_YN == "Y") {
                    IsCheck = false;
                    alert("이미 처리된 바코드. Đã xử lý Barcode trước đó");
                    $("#TB_BARCODE").val("");
                    $("#TB_BARCODE").focus();
                }
            }
            BARCODE_QR = $("#TB_BARCODE").val();
            document.getElementById("Buttonprint").disabled = true;
            let Tcount = $("#TB_BARCODE").val();

            let RB_LH = document.getElementById("RB_LH").checked;
            if (RB_LH == true) {
                let RB_LHVisible = document.getElementById("RB_LH").disabled;
                if (RB_LHVisible == true) {
                    IsCheck = false;
                }
                if (IsCheck == true) {
                    let ATOOL1 = true;
                    let BT_APP1 = document.getElementById("BT_APP1").disabled;
                    let VLA1 = $("#VLA1").val();
                    if (BT_APP1 == false) {
                        if (VLA1 == "") {
                            IsCheck = false;
                            alert("APPLICATOR #1 data null. Please Check Again.");
                            document.getElementById("Buttonprint").disabled = false;
                        }
                    }
                    else {
                        ATOOL1 = false;
                    }
                }
                if (IsCheck == true) {
                    let TOOLA1 = Number(("#TOOL_CONT1").val());
                    let TOOLC1 = 0;
                    try {
                        TOOLC1 = BaseResult.Search[0].TOOL_MAX1;
                    }
                    catch (e) {
                        TOOLC1 = 1000000;
                    }
                    if (TOOLA1 >= TOOLC1) {
                        IsCheck = false;
                        alert("Application #1 check(Check counter). Please Check Again.");
                        document.getElementById("Buttonprint").disabled = false;
                    }
                }
                if (IsCheck == true) {
                    SPC_EXIT = true;
                    if (ATOOL1 == true) {
                        let SPC1 = document.getElementById("SPC1").innerHTML;
                        if (SPC1 == "First") {
                            localStorage.setItem("C05_SPC_Close", 0);
                            localStorage.setItem("C05_SPC_Label10", "First");
                            localStorage.setItem("C05_SPC_Label11", "L");
                            let url = "/C05_SPC";
                            OpenWindowByURL(url, 800, 460);
                            C05_SPCTimerStartInterval(2);
                        }
                        let TOT_QTY = BaseResult.Search[0].TOT_QTY;
                        let PERFORMN_L = BaseResult.Search[0].PERFORMN_L;
                        if (TOT_QTY > 500) {
                            if (TOT_QTY / 2 <= PERFORMN_L) {
                                let SPC2 = document.getElementById("SPC2").innerHTML;
                                if (SPC2 == "Middle") {
                                    localStorage.setItem("C05_SPC_Close", 0);
                                    localStorage.setItem("C05_SPC_Label10", "Middle");
                                    localStorage.setItem("C05_SPC_Label11", "L");
                                    let url = "/C05_SPC";
                                    OpenWindowByURL(url, 800, 460);
                                    C05_SPCTimerStartInterval(2);
                                }
                            }
                        }
                        let A_CSPC = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN_L;
                        let BUNDLE_SIZE = BaseResult.Search[0].BUNDLE_SIZE;
                        if (BUNDLE_SIZE >= A_CSPC) {
                            let SPC3 = document.getElementById("SPC3").innerHTML;
                            if (SPC3 == "End") {
                                localStorage.setItem("C05_SPC_Close", 0);
                                localStorage.setItem("C05_SPC_Label10", "End");
                                localStorage.setItem("C05_SPC_Label11", "L");
                                let url = "/C05_SPC";
                                OpenWindowByURL(url, 800, 460);
                                C05_SPCTimerStartInterval(2);
                            }
                        }
                    }
                }
            }
            let RB_RH = document.getElementById("RB_RH").checked;
            if (RB_RH == true) {
                let RB_RHVisible = document.getElementById("RB_RH").disabled;
                if (RB_RHVisible == true) {
                    IsCheck = false;
                }
                if (IsCheck == true) {
                    let ATOOL2 = true;
                    let BT_APP2 = document.getElementById("BT_APP2").disabled;
                    let VLA2 = $("#VLA2").val();
                    if (BT_APP2 == false) {
                        if (VLA2 == "") {
                            IsCheck = false;
                            alert("APPLICATOR #2 data null. Please Check Again.");
                            document.getElementById("Buttonprint").disabled = false;
                        }
                    }
                    else {
                        ATOOL2 = false;
                    }
                }
                if (IsCheck == true) {
                    let TOOLA2 = Number($("#TOOL_CONT2").val());
                    let TOOLC2 = 0;
                    try {
                        TOOLC2 = BaseResult.Search[0].TOOL_MAX2;
                    }
                    catch (e) {
                        TOOLC2 = 1000000;
                    }
                    if (TOOLA2 >= TOOLC2) {
                        IsCheck = false;
                        alert("Application #2 check(Check counter). Please Check Again.");
                        document.getElementById("Buttonprint").disabled = false;
                    }
                }
                if (IsCheck == true) {
                    SPC_EXIT = true;
                    if (ATOOL2 == true) {
                        let SPC1 = document.getElementById("SPC1").innerHTML;
                        if (SPC1 == "First") {
                            localStorage.setItem("C05_SPC_Close", 0);
                            localStorage.setItem("C05_SPC_Label10", "First");
                            localStorage.setItem("C05_SPC_Label11", "L");
                            let url = "/C05_SPC";
                            OpenWindowByURL(url, 800, 460);
                            C05_SPCTimerStartInterval(3);
                        }
                        let TOT_QTY = BaseResult.Search[0].TOT_QTY;
                        let PERFORMN_L = BaseResult.Search[0].PERFORMN_L;
                        if (TOT_QTY > 500) {
                            if (TOT_QTY / 2 <= PERFORMN_L) {
                                let SPC2 = document.getElementById("SPC2").innerHTML;
                                if (SPC2 == "Middle") {
                                    localStorage.setItem("C05_SPC_Close", 0);
                                    localStorage.setItem("C05_SPC_Label10", "Middle");
                                    localStorage.setItem("C05_SPC_Label11", "L");
                                    let url = "/C05_SPC";
                                    OpenWindowByURL(url, 800, 460);
                                    C05_SPCTimerStartInterval(3);
                                }
                            }
                        }
                        let A_CSPC = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN_L;
                        let BUNDLE_SIZE = BaseResult.Search[0].BUNDLE_SIZE;
                        if (BUNDLE_SIZE >= A_CSPC) {
                            let SPC3 = document.getElementById("SPC3").innerHTML;
                            if (SPC3 == "End") {
                                localStorage.setItem("C05_SPC_Close", 0);
                                localStorage.setItem("C05_SPC_Label10", "End");
                                localStorage.setItem("C05_SPC_Label11", "L");
                                let url = "/C05_SPC";
                                OpenWindowByURL(url, 800, 460);
                                C05_SPCTimerStartInterval(3);
                            }
                        }
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_SPCTimerStartInterval(Flag) {
    let C05_SPCTimer = setInterval(function () {
        let C05_SPC_Close = localStorage.getItem("C05_SPC_Close");
        if (C05_SPC_Close == "1") {
            clearInterval(C05_SPCTimer);
            C05_SPC_Close = 0;
            localStorage.setItem("C05_SPC_Close", C05_SPC_Close);
            if (Flag == 0) {
                SPC_LOAD();
            }
            if (Flag == 1) {
                if (SPC_EXIT == true) {
                    clearInterval(Timer4);
                    SPC_LOAD();
                }
            }
            if (Flag == 2) {
                let IsCheck = true;
                if (SPC_EXIT == false) {
                    IsCheck = false;
                    alert("검사 누락. Kiểm tra mất tích.");
                    document.getElementById("Buttonprint").disabled = false;
                }
                if (IsCheck == true) {
                    try {
                        let Tcount = $("#TB_BARCODE").val();
                        let VLA1 = $("#VLA1").val();
                        let TOOL1_IDX = $("#TOOL1_IDX").val();
                        let Label4 = $("#Label4").val();
                        let Label8 = $("#Label8").val();
                        let VLT1 = $("#VLT1").val();
                        $("#BackGround").css("display", "block");
                        let BaseParameter = new Object();
                        BaseParameter = {
                            ListSearchString: [],
                        }
                        BaseParameter.USER_ID = GetCookieValue("UserID");
                        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                        BaseParameter.Search = BaseResult.Search;
                        BaseParameter.ListSearchString.push(Tcount);
                        BaseParameter.ListSearchString.push(VLA1);
                        BaseParameter.ListSearchString.push(TOOL1_IDX);
                        BaseParameter.ListSearchString.push(Label4);
                        BaseParameter.ListSearchString.push(Label8);
                        BaseParameter.ListSearchString.push(MCBOX);
                        BaseParameter.ListSearchString.push(VLT1);
                        BaseParameter.ListSearchString.push(DC_STR);
                        let formUpload = new FormData();
                        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                        let url = "/C05_START/BARCODE_LOADSub001";

                        fetch(url, {
                            method: "POST",
                            body: formUpload,
                            headers: {
                            }
                        }).then((response) => {
                            response.json().then((data) => {
                                $("#BackGround").css("display", "none");
                            }).catch((err) => {
                                $("#BackGround").css("display", "none");
                            })
                        });
                    }
                    catch (e) {
                        IsCheck = false;
                        alert("Please Check Again.(SEND)");
                        document.getElementById("Buttonprint").disabled = false;
                    }
                }
                if (IsCheck == true) {
                    BARCODE_LOADSub003();
                }
            }
            if (Flag == 3) {
                let IsCheck = true;
                if (SPC_EXIT == false) {
                    IsCheck = false;
                    alert("검사 누락. Kiểm tra mất tích.");
                    document.getElementById("Buttonprint").disabled = false;
                }
                if (IsCheck == true) {
                    try {
                        let Tcount = $("#TB_BARCODE").val();
                        let VLA2 = $("#VLA2").val();
                        let TOOL2_IDX = $("#TOOL2_IDX").val();
                        let Label4 = $("#Label4").val();
                        let Label8 = $("#Label8").val();
                        let VLT2 = $("#VLT2").val();
                        $("#BackGround").css("display", "block");
                        let BaseParameter = new Object();
                        BaseParameter = {
                            ListSearchString: [],
                        }
                        BaseParameter.USER_ID = GetCookieValue("UserID");
                        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                        BaseParameter.Search = BaseResult.Search;
                        BaseParameter.ListSearchString.push(Tcount);
                        BaseParameter.ListSearchString.push(VLA2);
                        BaseParameter.ListSearchString.push(TOOL2_IDX);
                        BaseParameter.ListSearchString.push(Label4);
                        BaseParameter.ListSearchString.push(Label8);
                        BaseParameter.ListSearchString.push(MCBOX);
                        BaseParameter.ListSearchString.push(VLT2);
                        BaseParameter.ListSearchString.push(DC_STR);
                        let formUpload = new FormData();
                        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                        let url = "/C05_START/BARCODE_LOADSub002";

                        fetch(url, {
                            method: "POST",
                            body: formUpload,
                            headers: {
                            }
                        }).then((response) => {
                            response.json().then((data) => {
                                let BaseResultSub = data;
                                let COM_COUNT = BaseResultSub.DataGridView5.length;
                                if (COM_COUNT == 0) {
                                    IsCheck = false;
                                    Buttonclose_Click();
                                }
                                if (IsCheck == true) {
                                    try {
                                        SPC_LOAD();
                                    }
                                    catch (e) {
                                        alert("#1 Please Check Again." + e);
                                        document.getElementById("Buttonprint").disabled = false;
                                    }
                                    try {
                                        ORDER_LOAD();
                                    }
                                    catch (e) {
                                        alert("#2 Please Check Again." + e);
                                        document.getElementById("Buttonprint").disabled = false;
                                    }
                                }
                                $("#BackGround").css("display", "none");
                            }).catch((err) => {
                                $("#BackGround").css("display", "none");
                            })
                        });
                    }
                    catch (e) {
                        IsCheck = false;
                        alert("Please Check Again.(SEND)");
                        document.getElementById("Buttonprint").disabled = false;
                    }
                }
                if (IsCheck == true) {
                    BARCODE_LOADSub003();
                }
            }
        }
    }, 100);
}
function BARCODE_LOADSub003() {
    try {
        let Tcount = $("#TB_BARCODE").val();
        let Label8 = $("#Label8").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(Tcount);
        BaseParameter.ListSearchString.push(Label8);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_START/BARCODE_LOADSub003";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("Buttonprint").disabled = true;
                TI_CONT = 1;
                Timer1StartInterval();
                alert("정상처리 되었습니다. Đã được lưu.");
                localStorage.setItem("C05_DC_READ_COUNT_BL", true);
                try {
                    SPC_LOAD();
                }
                catch (e) {
                    alert("#1 Please Check Again." + e);
                    document.getElementById("Buttonprint").disabled = false;
                }
                try {
                    ORDER_LOAD();
                }
                catch (e) {
                    alert("#2 Please Check Again." + e);
                    document.getElementById("Buttonprint").disabled = false;
                }
                $("#TB_BARCODE").val("");
                $("#TB_BARCODE").focus();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        alert("Please Check Again.");
        document.getElementById("Buttonprint").disabled = false;
    }
}
$("#SPC1").click(function (e) {
    SPC1_Click();
});
function SPC1_Click() {
    let IsCheck = true;
    localStorage.setItem("C05_SPC_Label10", "First");
    let RB_LH = document.getElementById("RB_LH").checked;
    let RB_RH = document.getElementById("RB_RH").checked;
    if (RB_LH == true) {
        localStorage.setItem("C05_SPC_Label11", "L");
        let VLA1 = $("#VLA1").val();
        if (VLA1 == "") {
            IsCheck = false;
        }
    }
    if (IsCheck == true) {
        if (RB_RH == true) {
            localStorage.setItem("C05_SPC_Label11", "R");
            let VLA2 = $("#VLA2").val();
            if (VLA2 == "") {
                IsCheck = false;
            }
        }
    }
    if (IsCheck == true) {
        localStorage.setItem("C05_SPC_Close", 0);
        let url = "/C05_SPC";
        OpenWindowByURL(url, 800, 460);
        C05_SPCTimerStartInterval(0);
    }
}
$("#SPC2").click(function (e) {
    SPC2_Click();
});
function SPC2_Click() {
    let IsCheck = true;
    let TOT_QTY = BaseResult.Search[0].TOT_QTY;
    let PERFORMN = BaseResult.Search[0].PERFORMN;

    if (TOT_QTY > 501) {
        if (TOT_QTY / 2 <= PERFORMN) {
            localStorage.setItem("C05_SPC_Label10", "Middle");
            let RB_LH = document.getElementById("RB_LH").checked;
            let RB_RH = document.getElementById("RB_RH").checked;
            if (RB_LH == true) {
                localStorage.setItem("C05_SPC_Label11", "L");
                let VLA1 = $("#VLA1").val();
                if (VLA1 == "") {
                    IsCheck = false;
                }
            }
            if (IsCheck == true) {
                if (RB_RH == true) {
                    localStorage.setItem("C05_SPC_Label11", "R");
                    let VLA2 = $("#VLA2").val();
                    if (VLA2 == "") {
                        IsCheck = false;
                    }
                }
            }
            if (IsCheck == true) {
                localStorage.setItem("C05_SPC_Close", 0);
                let url = "/C05_SPC";
                OpenWindowByURL(url, 800, 460);
                C05_SPCTimerStartInterval(0);
            }
        }
    }
}

$("#SPC3").click(function (e) {
    SPC3_Click();
});
function SPC3_Click() {
    let IsCheck = true;
    let AA = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN;
    let BB = AA - BaseResult.Search[0].BUNDLE_SIZE;
    if (BB <= 0) {
        localStorage.setItem("C05_SPC_Label10", "End");
        let RB_LH = document.getElementById("RB_LH").checked;
        let RB_RH = document.getElementById("RB_RH").checked;
        if (RB_LH == true) {
            localStorage.setItem("C05_SPC_Label11", "L");
            let VLA1 = $("#VLA1").val();
            if (VLA1 == "") {
                IsCheck = false;
            }
        }
        if (IsCheck == true) {
            if (RB_RH == true) {
                localStorage.setItem("C05_SPC_Label11", "R");
                let VLA2 = $("#VLA2").val();
                if (VLA2 == "") {
                    IsCheck = false;
                }
            }
        }
        if (IsCheck == true) {
            localStorage.setItem("C05_SPC_Close", 0);
            let url = "/C05_SPC";
            OpenWindowByURL(url, 800, 460);
            C05_SPCTimerStartInterval(0);
        }
    }
}

function SPC_LOAD() {
    let Label8 = $("#Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: Label8,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.RB_LH = document.getElementById("RB_LH").checked;
    BaseParameter.RB_RH = document.getElementById("RB_RH").checked;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_START/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView3 = BaseResultSub.DataGridView3;
            let COUNT_SPC = Number($("#Label48").val());
            try {
                COUNT_SPC = BaseResult.Search[0].TOT_QTY;
            }
            catch (e) {
                COUNT_SPC = Number($("#Label48").val());
            }
            try {
                if (COUNT_SPC < 11) {
                    document.getElementById("SPC1").innerHTML = "----";
                    document.getElementById("SPC1").disabled = true;
                }
                else {
                    document.getElementById("SPC1").innerHTML = "First";
                    document.getElementById("SPC1").disabled = false;
                }
                if (COUNT_SPC < 501) {
                    document.getElementById("SPC2").innerHTML = "----";
                    document.getElementById("SPC2").disabled = true;
                }
                else {
                    document.getElementById("SPC2").innerHTML = "First";
                    document.getElementById("SPC2").disabled = false;
                }
            }
            catch (e) {
                document.getElementById("SPC1").innerHTML = "ERROR";
                document.getElementById("SPC2").innerHTML = "ERROR";
                document.getElementById("SPC3").innerHTML = "ERROR";
            }
            for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                let COLSIP = BaseResult.DataGridView3[i].COLSIP;
                if (COLSIP == "First") {
                    document.getElementById("SPC1").innerHTML = "Complete";
                    document.getElementById("SPC1").disabled = true;
                }
                if (COLSIP == "Middle") {
                    document.getElementById("SPC2").innerHTML = "Complete";
                    document.getElementById("SPC2").disabled = true;
                }
                if (COLSIP == "End") {
                    document.getElementById("SPC3").innerHTML = "Complete";
                    document.getElementById("SPC3").disabled = true;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button2").click(function (e) {
    Button2_Click_1();
});
function Button2_Click_1() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    if (RadioButton1 == true) {
        Non_idx = 1;
    }
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton2 == true) {
        Non_idx = 2;
    }
    let RadioButton3 = document.getElementById("RadioButton3").checked;
    if (RadioButton3 == true) {
        Non_idx = 3;
    }
    let RadioButton4 = document.getElementById("RadioButton4").checked;
    if (RadioButton4 == true) {
        Non_idx = 4;
    }
    let RadioButton5 = document.getElementById("RadioButton5").checked;
    if (RadioButton5 == true) {
        Non_idx = 5;
    }
    let RadioButton6 = document.getElementById("RadioButton6").checked;
    if (RadioButton6 == true) {
        Non_idx = 6;
    }
    let RadioButton7 = document.getElementById("RadioButton7").checked;
    if (RadioButton7 == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S";
            Non_text_NM = document.getElementById("RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I";
            Non_text_NM = document.getElementById("RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q";
            Non_text_NM = document.getElementById("RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M";
            Non_text_NM = document.getElementById("RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T";
            Non_text_NM = document.getElementById("RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L";
            Non_text_NM = document.getElementById("RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E";
            Non_text_NM = document.getElementById("RadioButton7Text").innerHTML;
            break;
    }
    localStorage.setItem("C05_STOP_Close", 0);
    localStorage.setItem("C05_STOP_Label5", Non_text);
    localStorage.setItem("C05_STOP_STOP_MC", MCBOX);
    localStorage.setItem("C05_STOP_Label2", Non_text_NM & "(" + Non_text + ")");
    let url = "/C05_STOP";
    OpenWindowByURL(url, 800, 460);
    C05_STOPTimerStartInterval();
}
function C05_STOPTimerStartInterval() {
    let C05_STOPTimer = setInterval(function () {
        let C05_STOP_Close = localStorage.getItem("C05_STOP_Close");
        if (C05_STOP_Close == "1") {
            clearInterval(C05_STOPTimer);
            C05_STOP_Close = 0;
            localStorage.setItem("C05_STOP_Close", C05_STOP_Close);
        }
    }, 100);
}
function Timer1StartInterval() {
    Timer1 = setInterval(function () {
        Timer1_Tick();
    }, 500);
}
function Timer1_Tick() {
    if (TI_CONT >= 30) {
        document.getElementById("Buttonprint").disabled = false;
        clearInterval(Timer1);
    }
    else {
        if (TI_CONT > 0) {
            TI_CONT = TI_CONT + 1;
            document.getElementById("Buttonprint").innerHTML = "Complete(Barcode Reading) (" + TI_CONT + ")";
            document.getElementById("Buttonprint").disabled = true;
            Timer1StartInterval();
        }
        else {
            document.getElementById("Buttonprint").innerHTML = "Complete(Barcode Reading)";
            document.getElementById("Buttonprint").disabled = false;
        }
    }
}
function Timer2StartInterval() {
    Timer2 = setInterval(function () {
        Timer2_Tick();
    }, 1000);
}
function Timer2_Tick() {
    let now = new Date();
    let NOW_HH = now.getHours();
    let NOW_DD = now.getDate();
    let NOW_DATE_S = new Date();
    let NOW_DATE_E = new Date();
    if (NOW_DD == now.getDate()) {
        if (NOW_HH < 6) {
            let nowSub = now.setDate(now.getDate() - 1);
            NOW_DATE_S = new Date(nowSub.getFullYear(), nowSub.getMonth(), nowSub.getDate(), 6, 0, 0);
        }
        else {
            NOW_DATE_S = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 6, 0, 0);
        }
    }
    NOW_DATE_S = NOW_DATE_S;
    NOW_DATE_E = NOW_DATE_S.setDate(now.getDate() - 1);
    RunTime = new Date();
    let myTimeSpan;
    let Text_time = $("#Label71").val();
    let hh_time = Number(Text_time.substring(0, 2)) * 60 * 60;
    let mm_time = Number(Text_time.substring(3, 2)) * 60;
    let ss_time = Number(Text_time.substring(6, 2));
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = RunTime.getTime() - StartTime.getTime() - tot_time;
    $("#Label69").val(myTimeSpan);
    if (myTimeSpan < 0) {
        document.getElementById("Label69").style.color = "red";
        document.getElementById("Label72").style.color = "red";
        $("#Label72").val("-");
    }
    else {
        document.getElementById("Label69").style.color = "black";
        document.getElementById("Label72").style.color = "black";
        $("#Label72").val("+");
    }
    let M_STIME = new Date(NOW_DATE_S.getFullYear(), NOW_DATE_S.getMonth(), NOW_DATE_S.getDate(), 6, 0, 0);
    let M_SPAN = RunTime.getTime() - M_STIME.getTime();
    let DTIME11 = M_SPAN.toString();
    let DTIME12 = Number(DTIME11, 0, 2) * 60 * 60;
    let DTIME13 = Number(DTIME11, 3, 2) * 60;
    let DTIME14 = Number(DTIME11, 6, 2);
    let DTIME15 = DTIME12 + DTIME13 + DTIME14;

    let DTIME21 = $("#Label69").val();
    let DTIME22 = Number(DTIME21, 0, 2) * 60 * 60;
    let DTIME23 = Number(DTIME21, 3, 2) * 60;
    let DTIME24 = Number(DTIME21, 6, 2);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    $("#Label75").val(DTIME25 / DTIME15);
    let Label72 = $("#Label72").val();
    if (Label72 == "+") {
        let UPH_T1 = $("#Label69").val();
        let UPH_H1 = Number(UPH_T1, 0, 2);
        let UPH_M1 = Number(UPH_T1, 3, 2);
        let Label65 = Number(("#Label65").val()) / (UPH_H1 + (UPH_M1 / 60));
        $("#Label62").val(Label65);
    }
}
function Timer3StartInterval() {
    Timer3 = setInterval(function () {
        Timer3_Tick();
    }, 10000);
}
function Timer3_Tick() {
    OPER_TIME();
}
function Timer4StartInterval() {
    Timer4 = setInterval(function () {
        Timer4_Tick();
    }, 1000);
}
function Timer4_Tick() {
    clearInterval(Timer4);
    F_SPC();
}
function F_SPC() {
    try {
        let TOT_QTY = BaseResult.Search[0].TOT_QTY;
        let BUNDLE_SIZE = BaseResult.Search[0].BUNDLE_SIZE;
        if (TOT_QTY == BUNDLE_SIZE) {
            let SPC1 = document.getElementById("SPC1").innerHTML;
            if (SPC1 == "First") {
                localStorage.setItem("C05_SPC_Label10", "First");
                localStorage.setItem("C05_SPC_Close", 0);
                let url = "/C05_SPC";
                OpenWindowByURL(url, 800, 460);
                C05_SPCTimerStartInterval(1);
            }
        }
    }
    catch (e) {
        Buttonclose_Click();
    }
}