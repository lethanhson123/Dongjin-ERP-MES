let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let C_EXIT;
let C_USER;
let Timer1;
let Timer2;
let Timer3;
let Timer4;
let EW_TIME_Timer1;
let MAIN_CLOSE;
let TI_CONT;
let BARCODE_QR;
let SPC_EXIT;
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {
    let IsCheck = true;
    localStorage.setItem("C02_START_V2_Close", 0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    WORING_IDX = localStorage.getItem("WORING_IDX");

    C_EXIT = Boolean(localStorage.getItem("C02_START_V2_C_EXIT"));
    $("#BI_STIME").val(localStorage.getItem("C02_START_V2_BI_STIME"));
    $("#Label70").val(localStorage.getItem("C02_START_V2_Label70"));
    $("#Label8").val(localStorage.getItem("C02_START_V2_Label8"));
    $("#Label4").val(localStorage.getItem("C02_START_V2_Label4"));
    $("#Label8").val(localStorage.getItem("C02_START_V2_Label8"));
    $("#Label42").val(localStorage.getItem("C02_START_V2_Label42"));
    $("#Label43").val(localStorage.getItem("C02_START_V2_Label43"));
    $("#L_MCNM").val(localStorage.getItem("C02_START_V2_L_MCNM"));
    $("#Label48").val(localStorage.getItem("C02_START_V2_Label48"));
    $("#Label50").val(localStorage.getItem("C02_START_V2_Label50"));
    $("#Label77").val(localStorage.getItem("C02_START_V2_Label77"));

    document.getElementById("SPC1").innerHTML = "First";
    document.getElementById("SPC2").innerHTML = "Middle";
    document.getElementById("SPC3").innerHTML = "End";
    document.getElementById("SPC1").disabled = true;
    document.getElementById("SPC2").disabled = true;

    document.getElementById("BT_APP1").disabled = true;
    document.getElementById("BT_APP2").disabled = true;

    document.getElementById("Buttonprint").disabled = true;

    if (C_EXIT == true) {
        IsCheck = false;
        Buttonclose_Click();
    }

    MAIN_CLOSE = false;

    C02_start_Load();

    ORDER_LOAD(0);

});
function C02_start_Load() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
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
    let url = "/C02_START_V2/C02_start_Load";

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
function Timer1StartInterval() {
    Timer1 = setInterval(function () {
        Timer1_Tick();
    }, 500);
}
function Timer1_Tick() {
    if (TI_CONT >= 60) {
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
            document.getElementById("Buttonprint").innerHTML = "Complete / Print";
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
    let Text_time = $("#BI_STOPTIME").val();
    let hh_time = Number(Text_time.substring(0, 2)) * 60 * 60;
    let mm_time = Number(Text_time.substring(3, 2)) * 60;
    let ss_time = Number(Text_time.substring(6, 2));
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = RunTime.getTime() - StartTime.getTime() - tot_time;
    $("#BI_RTIME").val(myTimeSpan);
    if (myTimeSpan < 0) {
        document.getElementById("BI_RTIME").style.color = "red";
        document.getElementById("BI_RCK").style.color = "red";
        $("#BI_RCK").val("-");
    }
    else {
        document.getElementById("BI_RTIME").style.color = "black";
        document.getElementById("BI_RCK").style.color = "black";
        $("#BI_RCK").val("+");
    }
    let M_STIME = new Date(NOW_DATE_S.getFullYear(), NOW_DATE_S.getMonth(), NOW_DATE_S.getDate(), 6, 0, 0);
    let M_SPAN = RunTime.getTime() - M_STIME.getTime();
    let DTIME11 = M_SPAN.toString();
    let DTIME12 = Number(DTIME11, 0, 2) * 60 * 60;
    let DTIME13 = Number(DTIME11, 3, 2) * 60;
    let DTIME14 = Number(DTIME11, 6, 2);
    let DTIME15 = DTIME12 + DTIME13 + DTIME14;

    let DTIME21 = $("#BI_RTIME").val();
    let DTIME22 = Number(DTIME21, 0, 2) * 60 * 60;
    let DTIME23 = Number(DTIME21, 3, 2) * 60;
    let DTIME24 = Number(DTIME21, 6, 2);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    $("#BI_RVA").val(DTIME25 / DTIME15);
    let BI_RCK = $("#BI_RCK").val();
    if (BI_RCK == "+") {
        let UPH_T1 = $("#BI_RTIME").val();
        let UPH_H1 = Number(UPH_T1, 0, 2);
        let UPH_M1 = Number(UPH_T1, 3, 2);
        let BI_UPH = Number(("#BI_WQTY").val()) / (UPH_H1 + (UPH_M1 / 60));
        $("#BI_UPH").val(BI_UPH);
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
}
function EW_TIME_Timer1StartInterval() {
    EW_TIME_Timer1 = setInterval(function () {
        EW_TIME_Timer1_Tick();
    }, 60000);
}
function EW_TIME_Timer1_Tick() {
    EW_TIME();
}
function EW_TIME() {
    WORING_IDX = localStorage.getItem("WORING_IDX");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(WORING_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/EW_TIME";

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
$("#BT_APP1").click(function (e) {
    Button4_Click();
});
function Button4_Click() {
    let VLA1 = $("#VLA1").val();
    let VLA11 = $("#VLA11").val();
    localStorage.setItem("C02_APPLICATION_Close", 0);
    localStorage.setItem("C02_APPLICATION_Label3", "APPLICATION #1");
    localStorage.setItem("C02_APPLICATION_Label1", VLA1);
    localStorage.setItem("C02_APPLICATION_Label2", VLA11);
    let url = "/C02_APPLICATION";
    OpenWindowByURL(url, 800, 460);
    C02_APPLICATIONTimerStartInterval();
}
$("#BT_APP2").click(function (e) {
    Button5_Click();
});
function Button5_Click() {
    let VLA2 = $("#VLA2").val();
    let VLA22 = $("#VLA22").val();
    localStorage.setItem("C02_APPLICATION_Close", 0);
    localStorage.setItem("C02_APPLICATION_Label3", "APPLICATION #2");
    localStorage.setItem("C02_APPLICATION_Label1", VLA2);
    localStorage.setItem("C02_APPLICATION_Label2", VLA22);
    let url = "/C02_APPLICATION";
    OpenWindowByURL(url, 800, 460);
    C02_APPLICATIONTimerStartInterval();
}
$("#SPC1").click(function (e) {
    SPC1_Click();
});
function SPC1_Click() {
    localStorage.setItem("C02_SPC_Label10", "First");
    localStorage.setItem("C02_SPC_Close", 0);
    let url = "/C02_SPC";
    OpenWindowByURL(url, 800, 460);
    C02_SPCTimerStartInterval(0);
}
function C02_SPCTimerStartInterval(Flag) {
    let C02_SPCTimer = setInterval(function () {
        let C02_SPC_Close = localStorage.getItem("C02_SPC_Close");
        if (C02_SPC_Close == "1") {
            clearInterval(C02_SPCTimer);
            localStorage.setItem("C02_SPC_Close", 0);
            let url = "/C02_SPC";
            OpenWindowByURL(url, 800, 460);
            if (Flag == 0) {
                SPC_LOAD(0);
            }
            if (Flag == 1) {
                Buttonprint_ClickSub();
            }
        }
    }, 100);
}

function C02_APPLICATIONTimerStartInterval() {
    let C02_APPLICATIONTimer = setInterval(function () {
        let C02_APPLICATION_Close = localStorage.getItem("C02_APPLICATION_Close");
        if (C02_APPLICATION_Close == "1") {
            clearInterval(C02_APPLICATIONTimer);
            localStorage.setItem("C02_APPLICATION_Close", 0);
            let url = "/C02_APPLICATION";
            OpenWindowByURL(url, 800, 460);
            ORDER_LOAD(1);
        }
    }, 100);
}

$("#SPC2").click(function (e) {
    SPC2_Click();
});
function SPC2_Click() {
    let TOT_QTY = BaseResult.Search[0].TOT_QTY;
    let PERFORMN = BaseResult.Search[0].PERFORMN;

    if (TOT_QTY > 501) {
        if (TOT_QTY / 2 <= PERFORMN) {
            localStorage.setItem("C02_SPC_Label10", "Middle");
            localStorage.setItem("C02_SPC_Close", 0);
            let url = "/C02_SPC";
            OpenWindowByURL(url, 800, 460);
            C02_SPCTimerStartInterval(0);
        }
    }
}
$("#SPC3").click(function (e) {
    SPC3_Click();
});
function SPC3_Click() {
    let AA = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN;
    let BB = AA - BaseResult.Search[0].BUNDLE_SIZE;
    if (BB <= 0) {
        localStorage.setItem("C02_SPC_Label10", "End");
        localStorage.setItem("C02_SPC_Close", 0);
        let url = "/C02_SPC";
        OpenWindowByURL(url, 800, 460);
        C02_SPCTimerStartInterval(0);
    }
}
$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});

function Buttonprint_Click() {
    let IsCheck = true;
    if ($("#BARCODE_TEXT").val() == "None") {
        IsCheck = false;
        alert("NOT New Barcode. Please Check Again.");
    }
    else {
        BARCODE_QR = $("#BARCODE_TEXT").val();
        document.getElementById("Buttonprint").disabled = true;
        let ATOOL1 = true;
        let ATOOL2 = true;
        if (document.getElementById("BT_APP1").disabled == false) {
            if ($("#VLA1").val() == "") {
                IsCheck = false;
                alert("APPLICATOR #1 data null. Please Check Again.");
                document.getElementById("Buttonprint").disabled = false;
            }
        }
        else {
            ATOOL1 = true;
        }
        if (document.getElementById("BT_APP2").disabled == false) {
            if ($("#VLA2").val() == "") {
                IsCheck = false;
                alert("APPLICATOR #2 data null. Please Check Again.");
                document.getElementById("Buttonprint").disabled = false;
            }
        }
        else {
            ATOOL2 = true;
        }
        if (IsCheck == true) {
            let Tcount = $("#BARCODE_TEXT").val();
            let TOOLA1 = Number($("#TOOL_CONT1").val());
            let TOOLA2 = Number($("#TOOL_CONT2").val());
            let TOOLC1 = 0;
            let TOOLC2 = 0;
            try {
                TOOLC1 = BaseResult.Search[0].TOOL_MAX1;
            }
            catch (e) {
                TOOLC1 = 1000000;
            }
            try {
                TOOLC2 = BaseResult.Search[0].TOOL_MAX2;
            }
            catch (e) {
                TOOLC2 = 1000000;
            }
            if (TOOLA1 >= TOOLC1) {
                IsCheck = false;
                alert("Application #1 check(Check counter). Please Check Again.");
                document.getElementById("Buttonprint").disabled = false;
            }
            if (IsCheck == true) {
                if (TOOLA2 >= TOOLC2) {
                    IsCheck = false;
                    alert("Application #2 check(Check counter). Please Check Again.");
                    document.getElementById("Buttonprint").disabled = false;
                }
            }
            if (IsCheck == true) {
                SPC_EXIT = true;
                if ((ATOOL1 == true) || (ATOOL2 == true)) {
                    if (document.getElementById("SPC1").innerHTML == "First") {
                        localStorage.setItem("C02_SPC_Label10", "First");
                        localStorage.setItem("C02_SPC_Close", 0);
                        let url = "/C02_SPC";
                        OpenWindowByURL(url, 800, 460);
                        C02_SPCTimerStartInterval(1);
                    }
                    if (BaseResult.Search[0].TOT_QTY > 500) {
                        if (BaseResult.Search[0].TOT_QTY / 2 <= BaseResult.Search[0].PERFORMN) {
                            if (document.getElementById("SPC2").innerHTML == "Middle") {
                                localStorage.setItem("C02_SPC_Label10", "Middle");
                                localStorage.setItem("C02_SPC_Close", 0);
                                let url = "/C02_SPC";
                                OpenWindowByURL(url, 800, 460);
                                C02_SPCTimerStartInterval(1);
                            }
                        }
                    }
                    let A_CSPC = BaseResult.Search[0].TOT_QTY - BaseResult.Search[0].PERFORMN;
                    if (BaseResult.Search[0].BUNDLE_SIZE >= A_CSPC) {
                        if (document.getElementById("SPC3").innerHTML == "End") {
                            localStorage.setItem("C02_SPC_Label10", "End");
                            localStorage.setItem("C02_SPC_Close", 0);
                            let url = "/C02_SPC";
                            OpenWindowByURL(url, 800, 460);
                            C02_SPCTimerStartInterval(1);
                        }
                    }
                }
            }
        }
    }
}
function Buttonprint_ClickSub() {
    let IsCheck = true;
    SPC_EXIT = localStorage.getItem("C02_START_V2_SPC_EXIT");
    if (SPC_EXIT == false) {
        IsCheck = false;
        alert("검사 누락. Kiểm tra mất tích.");
        document.getElementById("Buttonprint").disabled = false;
    }
    try {
        let Label5 = $("#Label5").val();
        let Tcount = $("#BARCODE_TEXT").val();
        let SHIELDWIRE_CHK = localStorage.getItem("C02_SHIELDWIRE_CHK");
        let VLA1 = $("#VLA1").val();
        let TOOL1_IDX = $("#TOOL1_IDX").val();
        let T1_CONT = BaseResult.Search[0].T1_CONT;
        let Label4 = $("#Label4").val();
        let VLA2 = $("#VLA2").val();
        let TOOL2_IDX = $("#TOOL2_IDX").val();
        let T2_CONT = BaseResult.Search[0].T2_CONT;
        let Label8 = $("#Label8").val();
        let PERFORMN = BaseResult.Search[0].PERFORMN;
        let MCbox = localStorage.getItem("C02_MCbox");

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(BARCODE_QR);
        BaseParameter.ListSearchString.push(Label5);
        BaseParameter.ListSearchString.push(Tcount);
        BaseParameter.ListSearchString.push(SHIELDWIRE_CHK);
        BaseParameter.ListSearchString.push(VLA1);
        BaseParameter.ListSearchString.push(TOOL1_IDX);
        BaseParameter.ListSearchString.push(T1_CONT);
        BaseParameter.ListSearchString.push(Label4);
        BaseParameter.ListSearchString.push(VLA2);
        BaseParameter.ListSearchString.push(TOOL2_IDX);
        BaseParameter.ListSearchString.push(T2_CONT);
        BaseParameter.ListSearchString.push(Label8);
        BaseParameter.ListSearchString.push(PERFORMN);
        BaseParameter.ListSearchString.push(MCbox);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_START_V2/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                PrintDocument1_PrintPage();
                BaseResult.DataGridView5 = data.DataGridView5;
                let COM_COUNT = BaseResult.DataGridView5.length;
                if (COM_COUNT == 0) {
                    IsCheck = false;
                    Buttonclose_Click();
                }
                if (IsCheck == true) {
                    document.getElementById("Buttonprint").disabled = true;
                    TI_CONT = 1;
                    EW_TIME_Timer1Interval();
                    alert("정상처리 되었습니다. Đã được lưu.");

                    try {
                        ORDER_LOAD(1);
                    }
                    catch (e) {
                        alert("Please Check Again.");
                        document.getElementById("Buttonprint").disabled = false;
                    }

                    SHIELDWIRE_CHK = localStorage.getItem("C02_SHIELDWIRE_CHK");
                    if (SHIELDWIRE_CHK == true) {
                        IsCheck = false;
                    }
                    if (IsCheck == true) {
                        try {
                            SPC_LOAD(1);
                        }
                        catch (e) {
                            alert("Please Check Again.");
                            document.getElementById("Buttonprint").disabled = false;
                        }
                    }
                }
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
function PrintDocument1_PrintPage() {

    let PR1 = $("#Label43").val();
    let PR4 = $("#Label54").val();
    let PR5 = $("#Label4").val();
    let PR6 = $("#Label18").val();
    let PR7 = $("#Label48").val();
    let PR8 = $("#Label50").val();
    let PR9 = $("#VLT1").val();
    let PR10 = $("#VLT2").val();
    let PR11 = $("#VLS1").val();
    let PR12 = $("#VLS2").val();
    let PR13 = $("#ST_DCC1").val();
    let PR14 = $("#ST_DCC2").val();
    let PR15 = $("#ST_DIC1").val();
    let PR16 = $("#ST_DIC2").val();
    let PR17 = $("#Label42").val();
    let PR20 = $("#barsq").val();
    let PR21 = $("#ST_DSTR1").val();
    let PR22 = $("#Label34").val();
    let PR23 = $("#Label77").val();
    let OR_NO = BaseResult.Search[0].OR_NO;
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(PR1);
    BaseParameter.ListSearchString.push(PR4);
    BaseParameter.ListSearchString.push(PR5);
    BaseParameter.ListSearchString.push(PR6);
    BaseParameter.ListSearchString.push(PR7);
    BaseParameter.ListSearchString.push(PR8);
    BaseParameter.ListSearchString.push(PR9);
    BaseParameter.ListSearchString.push(PR10);
    BaseParameter.ListSearchString.push(PR11);
    BaseParameter.ListSearchString.push(PR12);
    BaseParameter.ListSearchString.push(PR13);
    BaseParameter.ListSearchString.push(PR14);
    BaseParameter.ListSearchString.push(PR15);
    BaseParameter.ListSearchString.push(PR16);
    BaseParameter.ListSearchString.push(PR17);
    BaseParameter.ListSearchString.push(PR20);
    BaseParameter.ListSearchString.push(PR21);
    BaseParameter.ListSearchString.push(PR22);
    BaseParameter.ListSearchString.push(PR23);
    BaseParameter.ListSearchString.push(OR_NO);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/PrintDocument1_PrintPage";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.Code) {
                let url = data.Code;
                OpenWindowByURL(url, 200, 200);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
$("#Button3").click(function (e) {
    Button3_Click();
});
function Button3_Click() {
    localStorage.setItem("C02_REPRINT_Label4", $("#Label8").val());
    localStorage.setItem("C02_REPRINT_Close", 0);
    let url = "/C02_REPRINT";
    OpenWindowByURL(url, 800, 460);
}
$("#BI_10").click(function (e) {
    Button2_Click_1();
});
function Button2_Click_1() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    let BI_RadioButton1 = document.getElementById("BI_RadioButton1").checked;
    if (BI_RadioButton1 == true) {
        Non_idx = 1;
    }
    let BI_RadioButton2 = document.getElementById("BI_RadioButton2").checked;
    if (BI_RadioButton2 == true) {
        Non_idx = 2;
    }
    let BI_RadioButton3 = document.getElementById("BI_RadioButton3").checked;
    if (BI_RadioButton3 == true) {
        Non_idx = 3;
    }
    let BI_RadioButton4 = document.getElementById("BI_RadioButton4").checked;
    if (BI_RadioButton4 == true) {
        Non_idx = 4;
    }
    let BI_RadioButton5 = document.getElementById("BI_RadioButton5").checked;
    if (BI_RadioButton5 == true) {
        Non_idx = 5;
    }
    let BI_RadioButton6 = document.getElementById("BI_RadioButton6").checked;
    if (BI_RadioButton6 == true) {
        Non_idx = 6;
    }
    let BI_RadioButton7 = document.getElementById("BI_RadioButton7").checked;
    if (BI_RadioButton7 == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S";
            Non_text_NM = document.getElementById("BI_RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I";
            Non_text_NM = document.getElementById("BI_RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q";
            Non_text_NM = document.getElementById("BI_RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M";
            Non_text_NM = document.getElementById("BI_RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T";
            Non_text_NM = document.getElementById("BI_RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L";
            Non_text_NM = document.getElementById("BI_RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E";
            Non_text_NM = document.getElementById("BI_RadioButton7Text").innerHTML;
            break;
    }
    localStorage.setItem("C02_STOP_Close", 0);
    localStorage.setItem("C02_STOP_Label5", Non_text);
    localStorage.setItem("C02_STOP_STOP_MC", localStorage.getItem("C02_MCbox"));
    localStorage.setItem("C02_STOP_Label2", Non_text_NM & "(" + Non_text + ")");
    let url = "/C02_STOP";
    OpenWindowByURL(url, 800, 460);
    C05_STOPTimerStartInterval();
}

function ORDER_LOAD(Flag) {
    let IsCheck = true;
    let Label8 = $("#Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/ORDER_LOAD";

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
                    Buttonclose_Click();
                }
                else {
                    IsCheck = false;
                    alert("등록된 자료가 없습니다. Không có dữ liệu đăng ký MES.");
                    Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                StartTime = $("#Label70").val();
                $("#BARCODE_TEXT").val(BaseResult.Search[0].TORDER_BARCODENM);
                $("#Label10").val(BaseResult.Search[0].TERM1);
                $("#VLT1").val(BaseResult.Search[0].TERM1);
                $("#Label14").val(BaseResult.Search[0].TERM2);
                $("#VLT2").val(BaseResult.Search[0].TERM2);
                $("#Label12").val(BaseResult.Search[0].SEAL1);
                $("#VLS1").val(BaseResult.Search[0].SEAL1);
                $("#Label16").val(BaseResult.Search[0].SEAL2);
                $("#VLS2").val(BaseResult.Search[0].SEAL2);
                $("#Label18").val(BaseResult.Search[0].WIRE);
                Label18_TextChanged();
                $("#VLW").val(BaseResult.Search[0].WIRE);
                $("#ST_DSTR1").val(BaseResult.Search[0].STRIP1);
                $("#Label34").val(BaseResult.Search[0].STRIP2);
                $("#ST_DCC1").val(BaseResult.Search[0].CCH_W1);
                $("#ST_DIC1").val(BaseResult.Search[0].ICH_W1);
                $("#ST_DCC2").val(BaseResult.Search[0].CCH_W2);
                $("#ST_DIC2").val(BaseResult.Search[0].ICH_W2);
                $("#Label48").val(BaseResult.Search[0].TOT_QTY);
                $("#Label59").val(BaseResult.Search[0].PERFORMN);
                $("#Label42").val(BaseResult.Search[0].SP_ST);
                $("#Label43").val(BaseResult.Search[0].PROJECT);
                $("#Label54").val(BaseResult.Search[0].HOOK_RACK);
                $("#barsq").val(BaseResult.Search[0].Barcode_SEQ);
                $("#Label77").val(BaseResult.Search[0].ADJ_AF_QTY);
                $("#TOOL_CONT1").val(BaseResult.Search[0].T1_CONT);
                $("#TOOL_CONT2").val(BaseResult.Search[0].T2_CONT);
                $("#Label5").val(BaseResult.Search[0].TORDER_BARCODE_IDX);
                $("#Label50").val(BaseResult.Search[0].BUNDLE_SIZE);
                $("#T1_IDX").val(BaseResult.Search[0].T1_PN_IDX);
                $("#T2_IDX").val(BaseResult.Search[0].T2_PN_IDX);
                $("#S1_IDX").val(BaseResult.Search[0].S1_PN_IDX);
                $("#S2_IDX").val(BaseResult.Search[0].S2_PN_IDX);
                $("#WIRE_IDX").val(BaseResult.Search[0].W_PN_IDX);
                $("#TOOL1_IDX").val(BaseResult.Search[0].T1_TOO_IDX);
                $("#TOOL2_IDX").val(BaseResult.Search[0].T2_TOOL_IDX);

                if (BaseResult.Search[0].T1_NM == "") {
                    $("#VLA1").val("");
                    $("#VLA11").val("");
                    $("#TOOL_CONT1").val("");
                    document.getElementById("BT_APP1").disabled = true;
                }
                else {
                    $("#VLA1").val(BaseResult.Search[0].TERM1);
                    $("#VLA11").val(BaseResult.Search[0].T1_NM);
                    document.getElementById("BT_APP1").disabled = false;
                }
                let BB0 = BaseResult.Search[0].WIRE;
                if (BB0 = "") {

                }
                else {
                    let PO1 = BB0.indexOf("  ") - BB0.indexOf(" ") - 1;
                    let TT2 = BB0.substring(BB0.indexOf(" ") + 1, PO1);
                    $("#ST_DWIRE1").val(TT2);
                }
                if (BaseResult.Search[0].T2_NM == "") {
                    $("#VLA2").val("");
                    $("#VLA22").val("");
                    $("#TOOL_CONT2").val("");
                    document.getElementById("BT_APP2").disabled = true;
                }
                else {
                    $("#VLA2").val(BaseResult.Search[0].TERM2);
                    $("#VLA22").val(BaseResult.Search[0].T2_NM);
                    document.getElementById("BT_APP2").disabled = false;
                }
                try {
                    if ($("#TOOL_CONT1").val() == "") {
                        document.getElementById("TOOL_CONT1").style.backgroundColor = "black";
                    }
                    else {
                        let T1_CONT = Number(BaseResult.Search[0].T1_CONT);
                        let TOOL_MAX1 = Number(BaseResult.Search[0].TOOL_MAX1);
                        if (T1_CONT <= TOOL_MAX1 * 0.9) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "lime";
                        }
                        if (T1_CONT > TOOL_MAX1 * 0.9) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "yellow";
                        }
                        if (T1_CONT >= TOOL_MAX1 * 1.0) {
                            document.getElementById("TOOL_CONT1").style.backgroundColor = "red";
                        }
                    }
                }
                catch (e) {
                    document.getElementById("TOOL_CONT1").style.backgroundColor = "red";
                    $("#TOOL_CONT1").val("ERROR");
                }
                try {
                    if ($("#TOOL_CONT2").val() == "") {
                        document.getElementById("TOOL_CONT2").style.backgroundColor = "black";
                    }
                    else {
                        let T2_CONT = Number(BaseResult.Search[0].T2_CONT);
                        let TOOL_MAX2 = Number(BaseResult.Search[0].TOOL_MAX2);
                        if (T2_CONT <= TOOL_MAX2 * 0.9) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "lime";
                        }
                        if (T2_CONT > TOOL_MAX2 * 0.9) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "yellow";
                        }
                        if (T2_CONT >= TOOL_MAX2 * 1.0) {
                            document.getElementById("TOOL_CONT2").style.backgroundColor = "red";
                        }
                    }
                }
                catch (e) {
                    document.getElementById("TOOL_CONT2").style.backgroundColor = "red";
                    $("#TOOL_CONT2").val("ERROR");
                }
                let AAA = Number(BaseResult.Search[0].TOT_QTY) - Number(BaseResult.Search[0].PERFORMN);
                let BUNDLE_SIZE = Number(BaseResult.Search[0].BUNDLE_SIZE);
                if (AAA > BUNDLE_SIZE) {
                    $("#Label50").val(BUNDLE_SIZE);
                }
                else {
                    $("#Label50").val(AAA - BUNDLE_SIZE);
                }

                if (Flag == 0) {
                    DB_COUTN(0);

                    document.getElementById("BI_RadioButton1").checked = true;

                    OPER_TIME(0);

                    SPC_LOAD(0);

                    Timer2StartInterval();

                    Timer1StartInterval();

                    SW_TIME(0);

                    EW_TIME_Timer1Interval();
                }
                if (Flag == 1) {
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function SW_TIME(Flag) {
    let MCbox = localStorage.getItem("C02_MCbox");
    let Label8 = $("#Label8").val();
    let FormText = "KOMAX_WORK";
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    BaseParameter.ListSearchString.push(Label8);
    BaseParameter.ListSearchString.push(FormText);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView4 = data.DataGridView4;

            try {
                WORING_IDX = BaseResult.DataGridView4[0].TOWT_INDX;
            }
            catch (e) {
                WORING_IDX = 0;
            }
            localStorage.setItem("WORING_IDX", WORING_IDX);

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function SPC_LOAD(Flag) {
    let MCbox = localStorage.getItem("C02_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView3 = data.DataGridView3;
            let COUNT_SPC = 0;
            try {
                COUNT_SPC = Number(BaseResult.DataGridView3[0].WORK_QTY);
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
                    document.getElementById("SPC2").innerHTML = "Middle";
                    document.getElementById("SPC2").disabled = false;
                }
            }
            catch (e) {
                document.getElementById("SPC1").innerHTML = "ERROR";
                document.getElementById("SPC2").innerHTML = "ERROR";
                document.getElementById("SPC3").innerHTML = "ERROR";
            }
            for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                if (BaseResult.DataGridView3[i].COLSIP == "First") {
                    document.getElementById("SPC1").innerHTML = "Complete";
                    document.getElementById("SPC1").disabled = true;
                }
                if (BaseResult.DataGridView3[i].COLSIP == "Middle") {
                    document.getElementById("SPC2").innerHTML = "Complete";
                    document.getElementById("SPC2").disabled = true;
                }
                if (BaseResult.DataGridView3[i].COLSIP == "End") {
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
function OPER_TIME(Flag) {
    let MCbox = localStorage.getItem("C02_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView2 = data.DataGridView2;
            $("#BI_STOPTIME").val("00:00:00");
            let TOT_SUM = 0;
            if (BaseResult.DataGridView2.length > 0) {
                TOT_SUM = Number(BaseResult.DataGridView1[0].SUM_TIME);
                let H_TIME = TOT_SUM / 60 / 60;
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = TOT_SUM / 60;
                let S_TIME = TOT_SUM - (M_TIME * 60);
                $("#BI_STOPTIME").val(H_TIME.padStart(2, '0') + ":" + M_TIME.padStart(2, '0') + ":" + S_TIME.padStart(2, '0'));
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DB_COUTN(Flag) {
    let MCbox = localStorage.getItem("C02_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/DB_COUTN";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView1 = data.DataGridView1;
            $("#BI_WQTY").val(0);
            if (BaseResult.DataGridView1.length > 0) {
                let TOT_QTY = Number(BaseResult.DataGridView1[0].SUM);
                if (TOT_QTY <= 0) {
                    TOT_QTY = 0;
                }
                else {
                    TOT_QTY = Number(BaseResult.DataGridView1[0].SUM);
                }
                $("#BI_WQTY").val(TOT_QTY);
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
    localStorage.setItem("C02_START_V2_Close", 1);
    window.close();
}