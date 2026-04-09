let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let SW_BOMRowIndex = 0;
let L_DGVRowIndex = 0;
let R_DGVRowIndex = 0;
let StartTime;

let Timer2;
let Timer3;
let C_MIN = [];
let ERROR_L = "A";
let ERROR_R = "A";
let ERROR_J = "A";
let ERROR_J2 = "A";
let COMP_L = "A";
let COMP_R = "A";
let COMP_J = "A";
let COMP_J2 = "A";
let ORDER_IDX;
let APPIDX_L;
let APPIDX_R;
let APPIDX_J;
let APPIDX_J2;

let C11_STOP_Timer1;
let C05_STOP_EW_TIME_Timer1;
let C11_STOP_StartTime;
let C11_STOP_STOPW_ORING_IDX = 0;

let IsC11_1 = false;
let IsC11_2 = false;
let IsC11_3 = false;
let IsC11_4 = false;

window.addEventListener('beforeunload', function (event) {    
    C11_STOP_Button1_ClickSub();
});

window.addEventListener('load', function () {
    const navigationEntry = performance.getEntriesByType('navigation')[0];
    if (navigationEntry && navigationEntry.type === 'reload') {
        C11_STOP_Button1_ClickSub();
    }
});

function C11_STOP_Button1_ClickSub() {
    try {
        let C11_STOP_Label6 = Number($("#C11_STOP_Label6").val());
        if (C11_STOP_Label6 > 0) {
            C11_STOP_Button1_Click();
        }
    }
    catch (e) {

    }
}

$(document).ready(function () {

    $(".modal").modal({
        dismissible: false
    });
    BaseResult.SW_BOM = new Object();
    BaseResult.SW_BOM = [];
    BaseResult.L_DGV = new Object();
    BaseResult.L_DGV = [];
    BaseResult.R_DGV = new Object();
    BaseResult.R_DGV = [];

    TS_USER();
    START_Enable();

    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MCbox").val(SettingsMC_NM);
    localStorage.setItem("C11_MCbox", $("#MCbox").val());

    $("#Barcodebox").focus();
    document.getElementById("BI_RadioButton1").checked = true;
    DB_COUTN();
    OPER_TIME(0);
    Lan_Change();

    LoadButtonStatus();
    $('#C11_STOP_MaintenanceModal').modal({
        dismissible: false
    });

    $("#C11_STOP_MaintenanceSaveBtn").click(function () {
        C11_STOP_SaveMaintenanceHistory();
    });
});

function LoadButtonStatus() {
    let totalPlan = Number($("#Label48").val());
    let COUNT_L = Number($("#COUNT_L").val());
    let COUNT_J = Number($("#COUNT_J").val());
    let COUNT_J2 = Number($("#COUNT_J2").val());
    let COUNT_R = Number($("#COUNT_R").val());

    if (!isNaN(totalPlan) && totalPlan > 0) {
        if (!isNaN(COUNT_L) && COUNT_L >= totalPlan) {
            $("#Button_L").prop("disabled", true); // disable button
        } else {
            $("#Button_L").prop("disabled", false); // enable lại nếu chưa đủ
        }

        if (!isNaN(COUNT_J) && COUNT_J >= totalPlan) {
            $("#Button_J").prop("disabled", true); // disable button
        } else {
            $("#Button_J").prop("disabled", false); // enable lại nếu chưa đủ
        }

        if (!isNaN(COUNT_J2) && COUNT_J2 >= totalPlan) {
            $("#Button_J2").prop("disabled", true); // disable button
        } else {
            $("#Button_J2").prop("disabled", false); // enable lại nếu chưa đủ
        }

        if (!isNaN(COUNT_R) && COUNT_R >= totalPlan) {
            $("#Button_R").prop("disabled", true); // disable button
        } else {
            $("#Button_R").prop("disabled", false); // enable lại nếu chưa đủ
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
    let NOW_DATE_S;
    let NOW_DATE_E;
    if (NOW_DD == now.getDate()) {
        if (NOW_HH < 6) {
            let nowSub = now.setDate(now.getDate() - 1);
            NOW_DATE_S = new Date(nowSub.getFullYear(), nowSub.getMonth(), nowSub.getDate(), 6, 0, 0);
            NOW_DATE_E = new Date(nowSub.getFullYear(), nowSub.getMonth(), nowSub.getDate(), 6, 0, 0);
        }
        else {
            NOW_DATE_S = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 6, 0, 0);
            NOW_DATE_E = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 6, 0, 0);
        }
    }
    NOW_DATE_E.setDate(NOW_DATE_E.getDate() + 1);
    RunTime = new Date();
    let myTimeSpan;
    let Text_time = $("#BI_STOPTIME").val();
    let hh_time1 = Text_time.substr(0, 2);
    let mm_time1 = Text_time.substr(3, 2);
    let ss_time2 = Text_time.substr(6, 2);
    let hh_time = Number(Text_time.split(":")[0]) * 60 * 60;
    let mm_time = Number(Text_time.split(":")[1]) * 60;
    let ss_time = Number(Text_time.split(":")[2]);
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = RunTime - StartTime;
    myTimeSpan = Math.floor(myTimeSpan / 1000) - tot_time;
    let BI_RTIME = ConvertSecondToString(myTimeSpan, ":");
    $("#BI_RTIME").val(BI_RTIME);
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
    let M_SPAN = RunTime - M_STIME;
    M_SPAN = Math.floor(M_SPAN / 1000);

    let DTIME11 = M_SPAN;
    let DTIME15 = DTIME11;

    let DTIME21 = $("#BI_RTIME").val();
    let DTIME22 = Number(DTIME21.split(":")[0]) * 60 * 60;
    let DTIME23 = Number(DTIME21.split(":")[1]) * 60;
    let DTIME24 = Number(DTIME21.split(":")[2]);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    let BI_RVA = ((DTIME25 / DTIME15) * 100).toFixed(2) + " %";
    $("#BI_RVA").val(BI_RVA);
    try {
        if ($("#BI_RCK").val() == "+") {
            let UPH_T1 = $("#BI_RTIME").val();
            let UPH_H1 = Number(UPH_T1.split(":")[0]);
            let UPH_M1 = Number(UPH_T1.split(":")[1]);
            let BI_WQTY = Number($("#BI_WQTY").val());
            let BI_UPH = 0;
            try {
                let division = (UPH_H1 + Math.floor((UPH_M1 / 60)));
                if (division != 0) {
                    BI_UPH = Math.floor(BI_WQTY / division);
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#BI_UPH").val(BI_UPH);
        }
    }
    catch (e) {
        console.log(e);
    }
}
function Timer3StartInterval() {
    Timer3 = setInterval(function () {
        Timer3_Tick();
    }, 10000);
}
function Timer3_Tick() {
    OPER_TIME(1);
}
function TS_USER() {
    try {
        let MCbox = $("#MCbox").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: MCbox,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11/TS_USER";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.Search = BaseResultSub.Search;
                BaseResult.Search1 = BaseResultSub.Search1;
                ToolStripLOGIDX = BaseResult.Search[0].TUSER_IDX;
                localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
                $("#BI_STIME").val(BaseResult.Search1[0].Name);
                $("#Label70").val(BaseResult.Search1[0].Description);
                StartTime = new Date($("#Label70").val());
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
    }
}
function START_Enable() {
    //$("#MCbox").val("");
    $("#Barcodebox").val("");
    clearInterval(Timer2);
    BaseResult.L_DGV = [];
    L_DGVRender();
    BaseResult.R_DGV = [];
    R_DGVRender();
}
function DB_COUTN() {
    try {
        let MCbox = $("#MCbox").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: MCbox,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11/DB_COUTN";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView = BaseResultSub.DataGridView;
                let TOT_QTY = BaseResult.DataGridView[0].SUM;
                if (TOT_QTY <= 0) {
                    TOT_QTY = 0;
                }
                else {
                    TOT_QTY = BaseResult.DataGridView[0].SUM;
                }
                $("#BI_WQTY").val(TOT_QTY);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        $("#BI_WQTY").val(0);
    }
}
function OPER_TIME(Flag) {
    try {
        let MCbox = $("#MCbox").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: MCbox,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11/OPER_TIME";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView1 = BaseResultSub.DataGridView1;
                let TOT_SUM = 0;
                try {
                    TOT_SUM = BaseResult.DataGridView1[0].SUM_TIME;
                }
                catch (e) {
                    TOT_SUM = 0;
                }
                try {
                    let H_TIME = Math.floor(Math.floor(TOT_SUM / 60) / 60);
                    TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                    let M_TIME = Math.floor(TOT_SUM / 60);
                    let S_TIME = TOT_SUM - (M_TIME * 60);
                    let BI_STOPTIME = String(H_TIME).padStart(2, '0') + ":" + String(M_TIME).padStart(2, '0') + ":" + String(S_TIME).padStart(2, '0');
                    $("#BI_STOPTIME").val(BI_STOPTIME);
                }
                catch (e) {
                    $("#BI_STOPTIME").val("00:00:00");
                }
                if (Flag == 0) {
                    Timer2StartInterval();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
    }
}
function Lan_Change() {
    Form_Label2();
}
function Form_Label2() {

}
$("#Barcodebox").keydown(function (e) {
    if (e.keyCode == 13) {
        Barcodebox_KeyDown();
    }
});
function Barcodebox_KeyDown() {
    Barcod_read();
}
function Barcod_read() {
    let IsCheck = true;
    let AAA = "";
    let BBB = "";
    let WORK_QTY = 0;
    C_MIN.push(2000000);
    C_MIN.push(2000000);
    C_MIN.push(2000000);
    C_MIN.push(2000000);
    let Barcodebox = $("#Barcodebox").val();
    if (Barcodebox == "") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        try {
            AAA = $("#Barcodebox").val();
            BBB = AAA.substr(0, AAA.indexOf("$$"));
            let AAT = AAA.substr(AAA.indexOf("$$") + 2, 12);
            WORK_QTY = AAT.substr(0, AAT.indexOf("$$") - 1);
        }
        catch (e) {
            IsCheck = false;
            alert("BARCODE data error. Please Check Again.");
        }
        if (IsCheck == true) {
            BaseResult.L_DGV = [];
            L_DGVRender();
            BaseResult.R_DGV = [];
            R_DGVRender();

            //$("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                SearchString: $("#Barcodebox").val(),
            }
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11/Barcod_read";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {

                    BaseResult.DataGridView12 = data.DataGridView12;
                    try {
                        ORDER_IDX = BaseResult.DataGridView12[0].ORDER_IDX;
                    }
                    catch (e) {
                        IsCheck = false;
                        ORDER_IDX = 0;
                        alert("SHIELD WIRE 바코드가 아닙니다. Không phải là Barcode Shield Wire");
                        $("#Barcodebox").val("");
                        $("#Barcodebox").focus();
                    }
                    if (IsCheck == true) {
                        if (BaseResult.DataGridView12.length <= 0) {
                            IsCheck = false;
                            alert("SHIELD WIRE 바코드가 아닙니다. Không phải là Barcode Shield Wire");
                            $("#Barcodebox").val("");
                            $("#Barcodebox").focus();
                        }
                        if (IsCheck == true) {
                            document.getElementById("Button_L").disabled = true;
                            document.getElementById("Button_R").disabled = true;
                            document.getElementById("Button_J").disabled = true;
                            document.getElementById("Button_J2").disabled = true;
                            BOM_CHK();
                        }
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });

        }
    }

    LoadButtonStatus();
}
function Barcod_readSub() {
    let IsCheck = true;
    let AAA = "";
    let BBB = "";
    let WORK_QTY = 0;
    let Barcodebox = $("#Barcodebox").val();
    let Label48 = $("#Label48").val();
    try {
        AAA = $("#Barcodebox").val();
        BBB = AAA.substr(0, AAA.indexOf("$$"));
        let AAT = AAA.substr(AAA.indexOf("$$") + 2, 12);
        WORK_QTY = AAT.substr(0, AAT.indexOf("$$"));
    }
    catch (e) {
        IsCheck = false;
        alert("BARCODE data error. Please Check Again.");
    }
    if (IsCheck == true) {
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.C_MIN = C_MIN;
        BaseParameter.ListSearchString.push(Barcodebox);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(ERROR_J);
        BaseParameter.ListSearchString.push(ERROR_L);
        BaseParameter.ListSearchString.push(ERROR_J2);
        BaseParameter.ListSearchString.push(ERROR_R);
        BaseParameter.ListSearchString.push(Label48);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11/Barcod_readSub";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView2 = BaseResultSub.DataGridView2;
                BaseResult.DataGridView3 = BaseResultSub.DataGridView3;
                BaseResult.SW_BOM = BaseResultSub.SW_BOM;
                SW_BOMRender();
                try {
                    ORDER_IDX = BaseResult.DataGridView2[0].ORDER_IDX;
                }
                catch (e) {
                    IsCheck = false;
                    ORDER_IDX = 0;
                    //alert("SHIELD WIRE 바코드가 아닙니다. Không phải là Barcode Shield Wire");
                    $("#Barcodebox").val("");
                    $("#Barcodebox").focus();
                }
                if (IsCheck == true) {
                    if (BaseResult.DataGridView2.length <= 0) {
                        IsCheck = false;
                        ORDER_IDX = 0;
                        //alert("SHIELD WIRE 바코드가 아닙니다. Không phải là Barcode Shield Wire");
                        $("#Barcodebox").val("");
                        $("#Barcodebox").focus();
                    }
                    if (IsCheck == true) {
                        $("#VLW").val(BaseResult.DataGridView2[0].WIRE);
                        $("#Label48").val(BaseResult.DataGridView2[0].TOT_QTY);
                        $("#ST_BCIDX").val(BaseResult.DataGridView2[0].ORDER_IDX);
                        $("#ST_BC").val(BaseResult.DataGridView2[0].TORDER_BARCODENM);
                        $("#Label50").val(WORK_QTY);

                        if (BaseResult.SW_BOM.length <= 0) {
                            IsCheck = false;
                            alert("BOM DATA 오류가 발생 하였습니다. Một lỗi đã xảy ra BOM DATA .");
                        }
                        if (IsCheck == true) {
                            let TERM1_CONT = 0;
                            let TERM2_CONT = 0;
                            let SEAL1_CONT = 0;
                            let SEAL2_CONT = 0;
                            $("#ST_LEAD").val(BBB);
                            $("#ST_WC").val(BaseResult.SW_BOM.length);
                            for (let i = 0; i < BaseResult.SW_BOM.length; i++) {
                                let item = BaseResult.SW_BOM[i];
                                if (item.T1_PARTNO == "") {
                                    TERM1_CONT = TERM1_CONT;
                                }
                                else {
                                    if (item.T1_PARTNO == "899997") {
                                        TERM1_CONT = TERM1_CONT;
                                    }
                                    else {
                                        if (item.T1_PARTNO == "899998") {
                                            TERM1_CONT = TERM1_CONT;
                                        }
                                        else {
                                            if (item.T1_PARTNO == "899999") {
                                                TERM1_CONT = TERM1_CONT;
                                            }
                                            else {
                                                if (item.Work == "JOINT") {
                                                    if (ERROR_J == "A") {
                                                        ERROR_J = "N";
                                                        COMP_J = "N";
                                                        C11_1_C_EXIT = true;
                                                        document.getElementById("Button_J").disabled = false;

                                                    }
                                                    TERM1_CONT = TERM1_CONT + 1;
                                                    C11_3_TL_NM = item.T1_PARTNO;
                                                    let L_DGVItem = {
                                                        NO: "J",
                                                        TERM1: item.T1_PARTNO,
                                                        ERR: ERROR_J,
                                                        Com: COMP_J,
                                                    }
                                                    BaseResult.L_DGV.push(L_DGVItem);
                                                    L_DGVRender();
                                                }
                                                else {
                                                    if (ERROR_L == "A") {
                                                        ERROR_L = "N";
                                                        COMP_L = "N";
                                                        C11_3_C_EXIT = true;
                                                        document.getElementById("Button_L").disabled = false;
                                                    }
                                                    TERM1_CONT = TERM1_CONT + 1;
                                                    let NO = (i + 1).toString();
                                                    let L_DGVItem = {
                                                        NO: NO,
                                                        TERM1: item.T1_PARTNO,
                                                        ERR: ERROR_L,
                                                        Com: COMP_L,
                                                    }
                                                    BaseResult.L_DGV.push(L_DGVItem);
                                                    L_DGVRender();
                                                }
                                            }
                                        }
                                    }
                                }


                                if (item.T2_PARTNO == "") {
                                    TERM2_CONT = TERM2_CONT;
                                }
                                else {
                                    if (item.T2_PARTNO == "899997") {
                                        TERM2_CONT = TERM2_CONT;
                                    }
                                    else {
                                        if (item.T2_PARTNO == "899998") {
                                            TERM2_CONT = TERM2_CONT;
                                        }
                                        else {
                                            if (item.T2_PARTNO == "899999") {
                                                TERM2_CONT = TERM2_CONT;
                                            }
                                            else {
                                                if (item.Work == "JOINT") {
                                                    if (ERROR_J2 == "A") {
                                                        ERROR_J2 = "N";
                                                        COMP_J2 = "N";
                                                        C11_4_C_EXIT = true;
                                                        document.getElementById("Button_J2").disabled = false;
                                                    }
                                                    TERM2_CONT = TERM2_CONT + 1;
                                                    C11_4_TR_NM = item.T2_PARTNO;
                                                    let R_DGVItem = {
                                                        NO: "J2",
                                                        TERM1: item.T2_PARTNO,
                                                        ERR: ERROR_J2,
                                                        Com: COMP_J2,
                                                    }
                                                    BaseResult.R_DGV.push(R_DGVItem);
                                                    R_DGVRender();
                                                }
                                                else {
                                                    if (ERROR_R == "A") {
                                                        ERROR_R = "N";
                                                        COMP_R = "N";
                                                        C11_2_C_EXIT = true;
                                                        document.getElementById("Button_R").disabled = false;
                                                    }
                                                    TERM2_CONT = TERM2_CONT + 1;
                                                    let NO = (i + 1).toString();
                                                    let R_DGVItem = {
                                                        NO: NO,
                                                        TERM1: item.T2_PARTNO,
                                                        ERR: ERROR_R,
                                                        Com: COMP_R,
                                                    }
                                                    BaseResult.R_DGV.push(R_DGVItem);
                                                    R_DGVRender();
                                                }
                                            }
                                        }
                                    }
                                }

                                if (item.S1_PARTNO == "") {
                                    SEAL1_CONT = SEAL1_CONT;
                                }
                                else {
                                    SEAL1_CONT = SEAL1_CONT + 1;
                                }
                                if (item.S2_PARTNO == "") {
                                    SEAL2_CONT = SEAL2_CONT;
                                }
                                else {
                                    SEAL2_CONT = SEAL2_CONT + 1;
                                }
                            }

                            let ROW_II = 0;
                            for (let i = 1; i < BaseResult.SW_BOM.length; i++) {
                                if (BaseResult.SW_BOM[i].Work == "LP") {
                                    i = BaseResult.SW_BOM.length;
                                }
                                ROW_II = ROW_II + 1;
                            }
                            $("#ST_DSTR1").val(BaseResult.SW_BOM[ROW_II].STRIP1);
                            $("#ST_DSTR2").val(BaseResult.SW_BOM[ROW_II].STRIP2);
                            $("#ST_DWIRE1").val(BaseResult.SW_BOM[ROW_II].W_Diameter);
                            $("#WIRE_Length").val(BaseResult.SW_BOM[ROW_II].W_Length);
                            $("#ST_DCC1").val(BaseResult.SW_BOM[ROW_II].CCH_W1);
                            $("#ST_DCC2").val(BaseResult.SW_BOM[ROW_II].CCH_W2);
                            $("#ST_DIC1").val(BaseResult.SW_BOM[ROW_II].ICH_W1);
                            $("#ST_DIC2").val(BaseResult.SW_BOM[ROW_II].ICH_W2);

                            $("#ST_TC").val(TERM1_CONT + " / " + TERM2_CONT);
                            $("#ST_SC").val(SEAL1_CONT + " / " + SEAL2_CONT);

                            let MIN = 999999;
                            for (var i = 0; i < 4; i++) {
                                if (MIN > Number(C_MIN[i])) {
                                    MIN = Number(C_MIN[i]);
                                }
                            }
                            $("#TOT_COUNT").val(MIN);

                            let OR_LIST = "";
                            if (BaseResult.DataGridView3.Count <= 0) {

                            }
                            else {
                                OR_LIST = BaseResult.DataGridView3[0].CONDITION;
                            }
                            if (OR_LIST == "Complete") {
                                IsCheck = false;
                                M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                                $("#Barcodebox").val("");
                                $("#Barcodebox").focus();
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

    $("#Barcodebox").val("");
    $("#Barcodebox").focus();
}
function BOM_CHK() {
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: ORDER_IDX,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11/BOM_CHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView4 = BaseResultSub.DataGridView4;
            if (BaseResult.DataGridView4.length > 0) {
                ERROR_L = "E";
                ERROR_R = "E";
                ERROR_J = "E";
                ERROR_J2 = "E";
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    if (BaseResult.DataGridView4[i].LOC_LRJ == "L") {
                        $("#COUNT_L").val(BaseResult.DataGridView4[i].PERFORMN);
                        if (BaseResult.DataGridView4[i].DSCN_YN == "Y") {
                            COMP_L = "Y";
                            document.getElementById("Button_L").disabled = true;
                            C_MIN[0] = $("#COUNT_L").val();
                        }
                        else {
                            if (BaseResult.DataGridView4[i].DSCN_YN == "N") {
                                COMP_L = "N";
                                document.getElementById("Button_L").disabled = false;
                                C_MIN[0] = $("#COUNT_L").val();
                            }
                            else {
                                COMP_L = "E";
                                document.getElementById("Button_L").disabled = true;
                                C_MIN[0] = 1000000;
                            }
                        }
                        if (BaseResult.DataGridView4[i].ERROR_CHK == "Y") {
                            ERROR_L = "Y";
                            APPIDX_L = BaseResult.DataGridView4[i].T1_TOOL_IDX;
                            C11_1_C_EXIT = false;
                        }
                        else {
                            ERROR_L = "N";
                            APPIDX_L = 0;
                            C11_1_C_EXIT = true;
                        }
                    }
                    if (BaseResult.DataGridView4[i].LOC_LRJ == "R") {
                        $("#COUNT_R").val(BaseResult.DataGridView4[i].PERFORMN);
                        if (BaseResult.DataGridView4[i].DSCN_YN == "Y") {
                            COMP_R = "Y";
                            document.getElementById("Button_R").disabled = true;
                            C_MIN[1] = $("#COUNT_R").val();
                        }
                        else {
                            if (BaseResult.DataGridView4[i].DSCN_YN == "N") {
                                COMP_R = "N";
                                document.getElementById("Button_R").disabled = false;
                                C_MIN[1] = $("#COUNT_R").val();
                            }
                            else {
                                COMP_R = "E";
                                document.getElementById("Button_R").disabled = true;
                                C_MIN[1] = 1000000;
                            }
                        }
                        if (BaseResult.DataGridView4[i].ERROR_CHK == "Y") {
                            ERROR_R = "Y";
                            APPIDX_R = BaseResult.DataGridView4[i].T1_TOOL_IDX;
                            C11_2_C_EXIT = false;
                        }
                        else {
                            ERROR_R = "N";
                            APPIDX_R = 0;
                            C11_2_C_EXIT = true;
                        }
                    }
                    if (BaseResult.DataGridView4[i].LOC_LRJ == "J") {
                        $("#COUNT_J").val(BaseResult.DataGridView4[i].PERFORMN);
                        if (BaseResult.DataGridView4[i].DSCN_YN == "Y") {
                            COMP_J = "Y";
                            document.getElementById("Button_J").disabled = true;
                            C_MIN[2] = $("#COUNT_J").val();
                        }
                        else {
                            if (BaseResult.DataGridView4[i].DSCN_YN == "N") {
                                COMP_J = "N";
                                document.getElementById("Button_J").disabled = false;
                                C_MIN[2] = $("#COUNT_J").val();
                            }
                            else {
                                COMP_J = "E";
                                document.getElementById("Button_J").disabled = true;
                                C_MIN[2] = 1000000;
                            }
                        }
                        if (BaseResult.DataGridView4[i].ERROR_CHK == "Y") {
                            ERROR_J = "Y";
                            APPIDX_J = BaseResult.DataGridView4[i].T1_TOOL_IDX;
                        }
                        else {
                            ERROR_J = "N";
                            APPIDX_J = 0;
                        }
                    }
                    if (BaseResult.DataGridView4[i].LOC_LRJ == "J2") {
                        $("#COUNT_J2").val(BaseResult.DataGridView4[i].PERFORMN);
                        if (BaseResult.DataGridView4[i].DSCN_YN == "Y") {
                            COMP_J2 = "Y";
                            document.getElementById("Button_J2").disabled = true;
                            C_MIN[3] = $("#COUNT_J2").val();
                        }
                        else {
                            if (BaseResult.DataGridView4[i].DSCN_YN == "N") {
                                COMP_J2 = "N";
                                document.getElementById("Button_J2").disabled = false;
                                C_MIN[3] = $("#COUNT_J2").val();
                            }
                            else {
                                COMP_J2 = "E";
                                document.getElementById("Button_J2").disabled = true;
                                C_MIN[3] = 1000000;
                            }
                        }
                        if (BaseResult.DataGridView4[i].ERROR_CHK == "Y") {
                            ERROR_J2 = "Y";
                            APPIDX_J2 = BaseResult.DataGridView4[i].T1_TOOL_IDX;
                        }
                        else {
                            ERROR_J2 = "N";
                            APPIDX_J2 = 0;
                        }
                    }

                }
            }
            else {
                ERROR_L = "A";
                ERROR_R = "A";
                ERROR_J = "A";
                ERROR_J2 = "A";
            }
            Barcod_readSub();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    history.back();
}
$("#BI_10").click(function () {
    Button5_Click();
});
function Button5_Click() {
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

    localStorage.setItem("C11_STOP_Close", 0);
    localStorage.setItem("C11_STOP_Label5", Non_text);
    localStorage.setItem("C11_STOP_STOP_MC", $("#MCbox").val());
    let C11_STOP_Label2 = Non_text_NM + "(" + Non_text + ")";
    localStorage.setItem("C11_STOP_Label2", C11_STOP_Label2);


    $("#C11_STOP_Label5").val(Non_text);
    $("#C11_STOP_STOP_MC").val($("#MCbox").val());
    $("#C11_STOP_Label2").val(C11_STOP_Label2);
    $("#C11_STOP_Label4").val("00 : 00 : 00");
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    C11_STOP_StartTime = now;
    $("#C11_STOP").modal("open");
    C11_STOP_Timer1StartInterval();
    C11_STOP_PageLoad();
}
function C11_STOP_ToggleButton3() {
    if ($("#C11_STOP_Label5").val() === "M") {
        $("#C11_STOP_Button3").show();
    } else {
        $("#C11_STOP_Button3").hide();
    }
}
function C11_STOP_Timer1StartInterval() {
    C11_STOP_Timer1 = setInterval(function () {
        C11_STOP_Timer1_Tick();
    }, 1000);
}
function C11_STOP_Timer1_Tick() {
    let End = new Date();
    $("#C11_STOP_Label4").val(CounterByBegin_EndToString(C11_STOP_StartTime, End));
}


function C11_STOP_PageLoad() {
    let STOP_MC = $("#C11_STOP_STOP_MC").val();
    let Label5 = $("#C11_STOP_Label5").val();
    let Label2 = $("#C11_STOP_Label2").val();

    let BaseParameter = new Object();
    BaseParameter = { ListSearchString: [] }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label2);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C11_STOP/PageLoad", {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_STOP_DataGridView1 = data.DataGridView1;
            if (BaseResult) {
                if (BaseResult.C11_STOP_DataGridView1) {
                    if (BaseResult.C11_STOP_DataGridView1.length > 0) {
                        $("#C11_STOP_Label6").val(BaseResult.C11_STOP_DataGridView1[0].TSNON_OPER_IDX);
                        SettingsNON_OPER_CHK = true;
                        SettingsNON_OPER_IDX = BaseResult.C11_STOP_DataGridView1[0].TSNON_OPER_IDX;
                        localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
                        localStorage.setItem("SettingsNON_OPER_IDX", SettingsNON_OPER_IDX);
                    }
                }
            }
            C11_STOP_SW_TIME();
            C11_STOP_EW_TIME_Timer1StartInterval();

            document.getElementById("C11_STOP_Button1").disabled = true;
            document.getElementById("C11_STOP_Button2").disabled = true;
            document.getElementById("C11_STOP_Button3").disabled = true;

            if ($("#C11_STOP_Label5").val() == "M") {
                document.getElementById("C11_STOP_Button2").disabled = false;
            } else {
                document.getElementById("C11_STOP_Button1").disabled = false;
            }

            C11_STOP_ToggleButton3();

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_STOP_FormClosed() {
    let Label6 = $("#C11_STOP_Label6").val();
    let STOP_MC = $("#C11_STOP_STOP_MC").val();
    let Label5 = $("#C11_STOP_Label5").val(); // ← THÊM

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C11_STOP_StartTime);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5); // ← THÊM

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/C11_STOP_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            clearInterval(C11_STOP_Timer1);
            C11_STOP_OPER_TIME();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_STOP_OPER_TIME() {
    let STOP_MC = $("#C11_STOP_STOP_MC").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            try {
                BaseResult.C11_STOP_Search = data.Search;
                let TOT_SUM = BaseResult.C11_STOP_Search[0].SUM_TIME;

                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C11_BI_STOPTIME = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C11_BI_STOPTIME.substr(0, 2);
                let M_TIME1 = C11_BI_STOPTIME.substr(2, 2);
                let S_TIME1 = C11_BI_STOPTIME.substr(4, 2);
                C11_BI_STOPTIME = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                $("#BI_STOPTIME").val(C11_BI_STOPTIME);
            }
            catch (e) {
            }
            C11_STOP_EW_TIME(1);
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_STOP_Button2").click(function () {
    C11_STOP_Button2_Click();
});
function C11_STOP_Button2_Click() {
    let STOP_MC = $("#C11_STOP_STOP_MC").val();
    let Label6 = $("#C11_STOP_Label6").val();

    let BaseParameter = new Object();
    BaseParameter = { SearchString: STOP_MC };
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C11_STOP/Button2_Click", {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#C11_STOP_TSNON_OPER_IDX").val(Label6);
            $("#C11_STOP_Label2").val("점검 중 / Đang kiểm tra");

            document.getElementById("C11_STOP_Button2").disabled = true;
            document.getElementById("C11_STOP_Button3").disabled = false;
            document.getElementById("C11_STOP_Button1").disabled = false;

            M.toast({
                html: 'Đang kiểm tra',
                classes: 'yellow darken-2',
                displayLength: 3000
            });
        }).catch((err) => {
            alert("Lỗi: " + err.message);
        })
    });
}
$("#C11_STOP_Button3").click(function () {
    let formElement = $("#C11_STOP_MaintenanceForm");
    if (formElement.length > 0) formElement[0].reset();
    $("#C11_STOP_MaintenanceModal").modal("open");
});
$("#C11_STOP_Button1").click(function () {
    C11_STOP_Button1_Click();
});
function C11_STOP_Button1_Click() {
    let Label5 = $("#C11_STOP_Label5").val();

    // ← THÊM VALIDATION
    if (Label5 == "M" && document.getElementById("C11_STOP_Button1").disabled) {
        M.toast({
            html: 'Vui lòng nhấn nút <b>"점검 중 / Đang kiểm tra"</b> trước khi khởi động lại!',
            classes: 'red darken-1',
            displayLength: 5000
        });
        return;
    }

    if (Label5 == "M") {
        let STOP_MC = $("#C11_STOP_STOP_MC").val();
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: STOP_MC,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_STOP/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                C11_STOP_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C11_STOP_FormClosed();
    }
}+

function C11_STOP_EW_TIME_Timer1StartInterval() {
    C11_STOP_EW_TIME_Timer1 = setInterval(function () {
        C11_STOP_EW_TIME_Timer1_Tick();
    }, 60000);
}
function C11_STOP_EW_TIME_Timer1_Tick() {
    C11_STOP_EW_TIME(0);
}
function C11_STOP_EW_TIME(Flag) {
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C11_STOP_STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (Flag == 1) {
                $("#C11_STOP").modal("close");
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_STOP_SW_TIME() {
    try {
        let USER_MC = localStorage.getItem("C02_MCbox");
        let USER_ORIDX = localStorage.getItem("C02_START_V2_Label8");
        let Label5 = $("#C11_STOP_Label5").val();;
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(USER_MC);
        BaseParameter.ListSearchString.push(USER_ORIDX);
        BaseParameter.ListSearchString.push(Label5);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_STOP/SW_TIME";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C11_STOP_DGV_WT = data.DGV_WT;
                try {
                    C11_STOP_STOPW_ORING_IDX = BaseResult.C11_STOP_DGV_WT[0].TOWT_INDX;
                }
                catch (e) {
                    C11_STOP_STOPW_ORING_IDX = 0;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        C11_STOP_STOPW_ORING_IDX = 0;
    }
}
function C11_STOP_SaveMaintenanceHistory() {
    let currentStatus = $("#C11_STOP_CurrentStatus").val();
    let solution = $("#C11_STOP_Solution").val();
    let maintenedBy = $("#C11_STOP_MaintenedBy").val();
    let sparePartsUsed = $("#C11_STOP_SparePartsUsed").val();
    if (!currentStatus) {
        M.toast({ html: 'Vui lòng chọn Nội dung lỗi!', classes: 'red' });
        return;
    }
    if (!solution) {
        M.toast({ html: 'Vui lòng chọn Biện pháp!', classes: 'red' });
        return;
    }
    if (!sparePartsUsed) {
        M.toast({ html: 'Vui lòng nhập Phụ tùng sửa chữa!', classes: 'red' });
        return;
    }
    if (!maintenedBy) {
        M.toast({ html: 'Vui lòng chọn Người sửa!', classes: 'red' });
        return;
    }

    let STOP_MC = $("#C11_STOP_STOP_MC").val();

    let BaseParameter = {
        STOP_MC: STOP_MC,
        CurrentStatus: currentStatus,
        Solution: solution,
        SparePartsUsed: sparePartsUsed,
        MaintenedBy: maintenedBy,
        Notes: $("#C11_STOP_Notes").val().trim(),
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C11_STOP/SaveMaintenanceHistory", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                M.toast({ html: 'Lưu chi tiết bảo trì thành công!', classes: 'green' });
                $("#C11_STOP_MaintenanceModal").modal("close");
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không thể lưu dữ liệu'), classes: 'red' });
            }
        })
        .catch(err => {
            M.toast({ html: 'Có lỗi xảy ra: ' + err.message, classes: 'red' });
        });
}
function C11_STOPTimerStartInterval() {
    let C11_STOPTimer = setInterval(function () {
        let C11_STOP_Close = localStorage.getItem("C11_STOP_Close");
        if (C11_STOP_Close == "1") {
            clearInterval(C11_STOPTimer);
            $("#BI_STOPTIME").val(localStorage.getItem("C11_BI_STOPTIME"));

        }
    }, 100);
}
$("#Button_L").click(function () {
    Button1_Click();
});
function Button1_Click() {
    try {
        let C11_1_DGV_LH = new Object();
        C11_1_DGV_LH = [];
        for (let i = 0; i < BaseResult.L_DGV.length; i++) {
            if (BaseResult.L_DGV[i].NO == "J") {

            }
            else {
                let C11_1_DGV_LHItem = {
                    NO: i + 1,
                    TERM1: BaseResult.L_DGV[i].TERM1,
                    ERR: BaseResult.L_DGV[i].ERR,
                }
                C11_1_DGV_LH.push(C11_1_DGV_LHItem);
            }
        }
        BaseResult.C11_1_DGV_LH = C11_1_DGV_LH;

        $("#C11_1_C11_D04").val(BaseResult.L_DGV[0].TERM1);
        $("#C11_1_ST_DWIRE1").val($("#ST_DWIRE1").val());
        $("#C11_1_WIRE_Length").val($("#WIRE_Length").val());
        $("#C11_1_ST_DSTR1").val($("#ST_DSTR1").val());
        $("#C11_1_ST_DCC1").val($("#ST_DCC1").val());
        $("#C11_1_ST_DIC1").val($("#ST_DIC1").val());
        $("#C11_1_C11_COUNT1").val($("#Label48").val());
        $("#C11_1_C11_COUNT3").val($("#Label50").val());
        $("#C11_1_C11_D01").val($("#ST_LEAD").val());
        $("#C11_1_ORDER").val($("#ST_BCIDX").val());

        if (C11_1_C_EXIT == true) {
            $("#C11_ER_L_LBT2S").val(BaseResult.L_DGV[0].TERM1);
            $("#C11_ER_L_LBA2S").val(BaseResult.L_DGV[0].TERM1);
            $("#C11_ER_L_Label1").val($("#ST_BCIDX").val());
            $("#C11_ER_L").modal("open");
            C11_ER_L_PageLoad();

        }
        else {
            $("#C11_1").modal("open");
            C11_1_PageLoad();
        }
    }
    catch (e) {
        console.log(e);
    }
}

$("#Button_R").click(function () {
    Button2_Click();
});
function Button2_Click() {
    try {
        let C11_2_DGV_RH = new Object();
        C11_2_DGV_RH = [];
        for (let i = 0; i < BaseResult.R_DGV.length; i++) {
            let NO = BaseResult.R_DGV[i].NO;
            if (NO == "J2") {

            }
            else {
                let C11_2_DGV_RHItem = {
                    NO: i + 1,
                    TERM1: BaseResult.R_DGV[i].TERM1,
                    ERR: BaseResult.R_DGV[i].ERR,
                }
                C11_2_DGV_RH.push(C11_2_DGV_RHItem);
            }
        }
        BaseResult.C11_2_DGV_RH = C11_2_DGV_RH;

        $("#C11_2_C11_D04").val(BaseResult.R_DGV[0].TERM1);
        $("#C11_2_ST_DWIRE1").val($("#ST_DWIRE1").val());
        $("#C11_2_WIRE_Length").val($("#WIRE_Length").val());
        $("#C11_2_ST_DSTR2").val($("#ST_DSTR2").val());
        $("#C11_2_ST_DCC1").val($("#ST_DCC2").val());
        $("#C11_2_ST_DIC1").val($("#ST_DIC2").val());
        $("#C11_2_C11_COUNT1").val($("#Label48").val());
        $("#C11_2_C11_COUNT3").val($("#Label50").val());
        $("#C11_2_C11_D01").val($("#ST_LEAD").val());
        $("#C11_2_ORDER").val($("#ST_BCIDX").val());

        if (C11_2_C_EXIT == true) {

            $("#C11_ER_R_LBT2S").val(BaseResult.R_DGV[0].TERM1);
            $("#C11_ER_R_LBA2S").val(BaseResult.R_DGV[0].TERM1);
            $("#C11_ER_R_Label1").val($("#ST_BCIDX").val());
            $("#C11_ER_R").modal("open");
            C11_ER_R_PageLoad();
        }
        else {

            $("#C11_2").modal("open");
            C11_2_PageLoad();
        }
    }
    catch (e) {

    }
}
$("#Button_J").click(function () {
    Button_J_Click();
});
function Button_J_Click() {

    $("#C11_3_C11_COUNT3").val($("#Label50").val());
    $("#C11_3_C11_D01").val($("#ST_LEAD").val());
    $("#C11_3_C11_COUNT1").val($("#Label48").val());
    $("#C11_3_ORDER").val($("#ST_BCIDX").val());
    $("#C11_3").modal("open");
    C11_3_PageLoad();

}
$("#Button_J2").click(function () {
    Button_J2_Click();
});
function Button_J2_Click() {
    $("#C11_4_C11_COUNT3").val($("#Label50").val());
    $("#C11_4_C11_D01").val($("#ST_LEAD").val());
    $("#C11_4_C11_COUNT1").val($("#Label48").val());
    $("#C11_4_ORDER").val($("#ST_BCIDX").val());
    $("#C11_4").modal("open");
    C11_4_PageLoad();
}
function SW_BOMRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.SW_BOM) {
            if (BaseResult.SW_BOM.length > 0) {
                SW_BOM_SelectionChanged(0);
                for (let i = 0; i < BaseResult.SW_BOM.length; i++) {
                    HTML = HTML + "<tr onclick='SW_BOM_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].Work + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].LEAD_PN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].WIRE_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T1_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].S1_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T2_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].S2_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T1NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T2NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].W_LINK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].WR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].WIRE_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].W_Diameter + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].W_Color + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].W_Length + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].LEAD_INDEX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].W_PN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T1_PN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].S1_PN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].T2_PN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.SW_BOM[i].S2_PN_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("SW_BOM").innerHTML = HTML;
}
function SW_BOM_SelectionChanged(i) {
    SW_BOMRowIndex = i;
}
let SW_BOMTable = document.getElementById("SW_BOMTable");
SW_BOMTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "Work";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.SW_BOM, key, text, IsTableSort);
        SW_BOMRender();
    }
});
function L_DGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.L_DGV) {
            if (BaseResult.L_DGV.length > 0) {
                L_DGV_SelectionChanged(0);
                for (let i = 0; i < BaseResult.L_DGV.length; i++) {
                    HTML = HTML + "<tr onclick='L_DGV_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.L_DGV[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.L_DGV[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.L_DGV[i].ERR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.L_DGV[i].Com + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("L_DGV").innerHTML = HTML;
}
function L_DGV_SelectionChanged(i) {
    L_DGVRowIndex = i;
}
let L_DGVTable = document.getElementById("L_DGVTable");
L_DGVTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.L_DGV, key, text, IsTableSort);
        L_DGVRender();
    }
});
function R_DGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.R_DGV) {
            if (BaseResult.R_DGV.length > 0) {
                R_DGV_SelectionChanged(0);
                for (let i = 0; i < BaseResult.R_DGV.length; i++) {
                    HTML = HTML + "<tr onclick='R_DGV_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.R_DGV[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.R_DGV[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.R_DGV[i].ERR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.R_DGV[i].Com + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("R_DGV").innerHTML = HTML;
}
function R_DGV_SelectionChanged(i) {
    R_DGVRowIndex = i;
}
let R_DGVTable = document.getElementById("R_DGVTable");
R_DGVTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.R_DGV, key, text, IsTableSort);
        R_DGVRender();
    }
});

let C11_ER_L_StartTime;
let C11_ER_L_RunTime;
let C11_ER_L_LAST_ID;
function C11_ER_L_PageLoad() {

    C11_ER_L_StartTime = new Date();
    $("#C11_ER_L_TextBox1").val("");
    $("#C11_ER_L_LBT2R").val("");
    $("#C11_ER_L_LBA2R").val("");
    $("#C11_ER_L_LBT2V").val("");
    $("#C11_ER_L_LBA2V").val("");
    $("#C11_ER_L_LBA2SEQ").val("");
    $("#C11_ER_L_LBA2IDX").val("");
    $("#C11_ER_L_LBR2").val("");
    $("#C11_ER_L_LBR4").val("");
    $("#C11_ER_L_LBR22").val("");

    document.getElementById("C11_ER_L_CHBT2").checked = true;
    document.getElementById("C11_ER_L_CHBA2").checked = true;

    document.getElementById("C11_ER_L_LBR2").style.backgroundColor = "white";
    document.getElementById("C11_ER_L_LBR4").style.backgroundColor = "white";
    document.getElementById("C11_ER_L_LBR22").style.backgroundColor = "white";


    $("#C11_ER_L_ComboBox1").empty();

    if ($("#C11_ER_L_LBT2S").val() == "") {

        C11_ER_L_Buttonclose_Click();
    }

    var ComboBox1 = document.getElementById("C11_ER_L_ComboBox1");

    var option = document.createElement("option");
    option.text = "TERM";
    option.value = "TERM";
    ComboBox1.add(option);

    option = document.createElement("option");
    option.text = "APPLICATOR";
    option.value = "APPLICATOR";
    C11_ER_L_ComboBox1.add(option);

    C11_ERROR_Load();
}
function C11_ERROR_Load() {
    let MCbox = $("#MCbox").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_ER_L/C11_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_ER_L_Search = data.Search;
            C11_ER_L_LAST_ID = BaseResult.C11_ER_L_Search[0].TSNON_OPER_IDX;
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_ER_L_Button1").click(function (e) {
    C11_ER_L_Button1_Click();
});

function C11_ER_L_Button1_Click() {
    let IsCheck = true;
    let AA = $("#C11_ER_L_TextBox1").val();
    AA = AA.toUpperCase();
    let BAR_TEXT = false;
    let ComboBox1 = $("#C11_ER_L_ComboBox1").val();
    if (ComboBox1 == "APPLICATOR") {
        BAR_TEXT = true;
    }
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");

    let BC_MAST = SettingsMASTER_BC;
    if (AA == BC_MAST) {
        BAR_TEXT = true;
    }
    C11_ER_L_Button1_ClickSub();
    //if (BAR_TEXT == false) {
    //    //$("#BackGround").css("display", "block");
    //    let BaseParameter = new Object();
    //    BaseParameter = {
    //        ListSearchString: [],
    //    }
    //    BaseParameter.USER_ID = GetCookieValue("UserID");
    //    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    //    BaseParameter.ListSearchString.push(AA);
    //    let formUpload = new FormData();
    //    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //    let url = "/C11_ER_L/Button1_ClickSub";

    //    fetch(url, {
    //        method: "POST",
    //        body: formUpload,
    //        headers: {
    //        }
    //    }).then((response) => {
    //        response.json().then((data) => {
    //            BaseResult.C11_ER_L_DataGridView1 = data.DataGridView1;
    //            if (BaseResult.C11_ER_L_DataGridView1.length <= 0) {
    //                IsCheck = false;
    //                alert("오류가 발생 하였습니다.(바코드 이력 없음). BARCODE Một lỗi đã xảy ra.");
    //            }
    //            if (IsCheck == true) {
    //                C11_ER_L_Button1_ClickSub();
    //            }
    //            $("#BackGround").css("display", "none");
    //        }).catch((err) => {
    //            $("#BackGround").css("display", "none");
    //        })
    //    });
    //}
    //else {
    //    C11_ER_L_Button1_ClickSub();
    //}
}
function C11_ER_L_Button1_ClickSub() {
    let IsCheck = true;
    let AA = $("#C11_ER_L_TextBox1").val();
    AA = AA.toUpperCase();
    let ComboBox1 = $("#C11_ER_L_ComboBox1").val();
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    switch (ComboBox1) {
        case "TERM":
            try {
                let LBT2S = $("#C11_ER_L_LBT2S").val();
                let T2 = "";
                if (AA == BC_MAST) {
                    T2 = LBT2S;
                }
                else {
                    T2 = AA.substr(0, AA.indexOf("$$"));
                }
                if (LBT2S == T2) {
                    $("#C11_ER_L_LBT2R").val("OK");
                    $("#C11_ER_L_LBR2").val("OK");
                    document.getElementById("C11_ER_L_LBR2").style.color = "green";

                    try {
                        document.getElementById("C11_ER_L_ComboBox1").selectedIndex = document.getElementById("C11_ER_L_ComboBox1").selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C11_ER_L_LBT2R").val("NG");
                    $("#C11_ER_L_LBR2").val("NG");
                    document.getElementById("C11_ER_L_LBR2").style.color = "red";
                }
                $("#C11_ER_L_LBT2V").val(T2);
                $("#C11_ER_L_TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again. " + e);
            }
            break;
        case "APPLICATOR":
            let A2 = AA.substr(0, AA.length - 1);
            let LBA2S = $("#C11_ER_L_LBA2S").val();
            if (LBA2S == A2) {
                $("#C11_ER_L_LBA2R").val("OK");
                let LBA2SEQ = AA.substr(AA.length - 1, 1);
                $("#C11_ER_L_LBA2SEQ").val(LBA2SEQ);
                let ASC_SEQ = LBA2SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (ASC_SEQ >= 72) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    LBA2SEQ = $("#C11_ER_L_LBA2SEQ").val();
                    let Label1 = $("#C11_ER_L_Label1").val();
                    //$("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A2);
                    BaseParameter.ListSearchString.push(LBA2SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C11_ER_L/Button1_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            BaseResult.C11_ER_L_Search = data.Search;
                            BaseResult.C11_ER_L_Search1 = data.Search1;
                            BaseResult.C11_ER_L_DataGridView = data.DataGridView;
                            if (BaseResult.C11_ER_L_Search.length <= 0) {
                                if (BaseResult.C11_ER_L_Search1.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#C11_ER_L_LBA2R").val("Not data");
                                    $("#C11_ER_L_LBA2V").val(A2);
                                    $("#C11_ER_L_TextBox1").val("");
                                }
                                else {
                                    let WK_CNT = BaseResult.C11_ER_L_DataGridView[0].WK_CNT;
                                    $("#C11_ER_L_LBR22").val(WK_CNT);
                                    document.getElementById("C11_ER_L_LBR22").style.color = "green";
                                }
                            }
                            else {
                                let WK_CNT = BaseResult.C11_ER_L_Search[0].WK_CNT;
                                $("#C11_ER_L_LBR22").val(WK_CNT);
                                document.getElementById("C11_ER_L_LBR22").style.color = "green";
                                let TOOL_IDX = BaseResult.C11_ER_L_Search[0].TOOL_IDX;
                                $("#C11_ER_L_LBA2IDX").val(TOOL_IDX);
                            }
                            if (IsCheck == true) {

                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                $("#C11_ER_L_LBA2V").val(A2);
                $("#C11_ER_L_TextBox1").val("");
            }
            break;
    }
    $("#C11_ER_L_TextBox1").focus();
}
$("#C11_ER_L_Button2").click(function (e) {
    C11_ER_L_Button2_Click();
});
function C11_ER_L_Button2_Click() {
    let AA2 = document.getElementById("C11_ER_L_CHBT2").checked;
    let AA4 = document.getElementById("C11_ER_L_CHBA2").checked;
    let BB2 = false;
    let BB4 = false;
    if (AA2 == true) {
        if ($("#C11_ER_L_LBT2R").val() == "OK") {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA4 == true) {
        if ($("#C11_ER_L_LBA2R").val() == "OK") {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if ((AA2 == true) && (BB2 == true) && (AA4 == true) && (BB4 == true)) {
        let Label1 = $("#C11_ER_L_Label1").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: Label1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_ER_L/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                C11_1_C_EXIT = false;
                C11_ERROR_FormClosed(1);

            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert("Please Check Again.");
        C11_1_C_EXIT = true;
    }
}
$("#C11_ER_L_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_ER_L_TextBox1_KeyDown();
    }
});
function C11_ER_L_TextBox1_KeyDown() {
    C11_ER_L_Button1_Click();
}
$("#C11_ER_L_Buttonclose").click(function (e) {
    C11_ER_L_Buttonclose_Click();
});
function C11_ER_L_Buttonclose_Click() {
    C11_1_C_EXIT = true;
    C11_ERROR_FormClosed(0);
}
function C11_ERROR_FormClosed(Flag) {
    let MCbox = $("#MCbox").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C11_ER_L_LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_ER_L/C11_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            localStorage.setItem("SettingsNON_OPER_CHK", false);
            if (Flag == 1) {
                C11_1_C_EXIT = false;
                $("#C11_1").modal("open");
                C11_1_PageLoad();
            }
            $("#C11_ER_L").modal("close");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

let C11_1_Timer1;
let C11_1_C_EXIT = true;
let C11_1_DGV_LHRowIndex = 0;
let C11_1_TI_CONT;
let C11_1_APP_MAX;
let C11_1_SPC_EXIT;
let C11_SPC_L_Flag = 0;
function C11_1_PageLoad() {
    C11_1_DGV_LHRender();
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_1_C11_D02").val(SettingsMC_NM);
    C11_1_ORDER_LOAD(0);
}
function C11_1_ORDER_LOAD(Flag) {
    let IsCheck = true;
    let ORDER = $("#C11_1_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_1/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.C11_1_Search = BaseResultSub.Search;
            if (BaseResult.C11_1_Search.length <= 0) {
                IsCheck = false;
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra.");
                C11_1_Buttonclose_Click();
            }
            else {
                try {
                    $("#C11_1_VLA1").val(BaseResult.C11_1_Search[0].APP);
                    $("#C11_1_VLA11").val(BaseResult.C11_1_Search[0].SEQ);
                    $("#C11_1_TOOL_CONT").val(BaseResult.C11_1_Search[0].COUNT);
                    $("#C11_1_C11_COUNT2").val(BaseResult.C11_1_Search[0].PERFORMN);
                    $("#C11_1_VLT1").val($("#C11_1_C11_D04").val());
                    C11_1_APP_MAX = BaseResult.C11_1_Search[0].MAX;

                    if ($("#C11_1_TOOL_CONT").val() == "") {
                        $("#C11_1_TOOL_CONT").val(0);
                    }
                    if ($("#C11_1_C11_COUNT2").val() == "") {
                        $("#C11_1_C11_COUNT2").val(0);
                    }
                    $("#C11_1_Label1").val(Number($("#C11_1_C11_COUNT2").val()) * BaseResult.C11_1_DGV_LH.length);
                    if (BaseResult.C11_1_Search[0].PERFORMN >= BaseResult.C11_1_Search[0].TOT_COUNT) {
                        IsCheck = false;
                        M.toast({ html: "작업을 종료하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                        C11_1_Buttonclose_Click();
                    }
                }
                catch (e) {
                    IsCheck = false;
                    $("#C11_1_VLA1").val("");
                    $("#C11_1_VLA11").val("");
                    $("#C11_1_TOOL_CONT").val("");
                    $("#C11_1_C11_COUNT2").val("");
                    $("#C11_1_VLT1").val("");

                    $("#C11_ER_L_LBT2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_L_LBA2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_L_Label1").val($("#ST_BCIDX").val());

                    BaseResult.C11_1_DGV_LH = [];
                    C11_1_DGV_LHRender();

                    $("#C11_1").modal("close");
                    C11_1_Buttonclose_Click();

                    $("#C11_ER_L").modal("close");
                    C11_ER_L_Buttonclose_Click();

                    $("#C11_ER_L_LBT2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_L_LBA2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_L_Label1").val($("#ST_BCIDX").val());

                    $("#C11_ER_L").modal("open");
                }
            }

            if (IsCheck == true) {
                if (Flag == 0) {
                    if (C11_1_C_EXIT == true) {
                        IsCheck = false;
                        C11_1_Buttonclose_Click();
                    }
                    if (IsCheck == true) {
                        C11_1_SPC_LOAD();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function C11_1_SPC_LOAD() {
    let ORDER = $("#C11_1_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_1/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_1_Search1 = data.Search1;
            try {
                if (Number($("#C11_1_C11_COUNT1").val()) < 11) {
                    document.getElementById("C11_1_SPC1").innerHTML = "----";
                    document.getElementById("C11_1_SPC1").disabled = true;
                }
                else {
                    document.getElementById("C11_1_SPC1").innerHTML = "First";
                    document.getElementById("C11_1_SPC1").disabled = false;
                }
                if (Number($("#C11_1_C11_COUNT1").val()) < 501) {
                    document.getElementById("C11_1_SPC2").innerHTML = "----";
                    document.getElementById("C11_1_SPC2").disabled = true;
                }
                else {
                    document.getElementById("C11_1_SPC2").innerHTML = "Middle";
                    document.getElementById("C11_1_SPC2").disabled = false;
                }
            }
            catch (e) {

            }
            if (BaseResult.C11_1_Search1.length > 0) {
                for (let i = 0; i < BaseResult.C11_1_Search1.length; i++) {
                    if (BaseResult.C11_1_Search1[i].COLSIP == "First") {
                        document.getElementById("C11_1_SPC1").innerHTML = "Complete";
                        document.getElementById("C11_1_SPC1").disabled = true;
                    }
                    if (BaseResult.C11_1_Search1[i].COLSIP == "Middle") {
                        document.getElementById("C11_1_SPC2").innerHTML = "Complete";
                        document.getElementById("C11_1_SPC2").disabled = true;
                    }
                    if (BaseResult.C11_1_Search1[i].COLSIP == "End") {
                        document.getElementById("C11_1_SPC3").innerHTML = "Complete";
                        document.getElementById("C11_1_SPC3").disabled = true;
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_1_Buttonclose").click(function (e) {
    C11_1_Buttonclose_Click();
});
function C11_1_Buttonclose_Click() {
    C11_1_FormClosed();
}
function C11_1_FormClosed() {
    $("#C11_1").modal("close");
    $("#Barcodebox").val($("#ST_BC").val());
    Barcod_read();
}
function C11_1_Timer1StartInterval() {
    C11_1_Timer1 = setInterval(function () {
        C11_1_Timer1_Tick();
    }, 200);
}
function C11_1_Timer1_Tick() {
    if (C11_1_TI_CONT >= 60) {
        document.getElementById("C11_1_Buttonprint").disabled = false;
        clearInterval(C11_1_Timer1);
    }
    else {
        C11_1_TI_CONT = C11_1_TI_CONT + 1;
        document.getElementById("C11_1_Buttonprint").innerHTML = "Complete" + "  (" + C11_1_TI_CONT + ")";
    }
}
$("#C11_1_SPC1").click(function (e) {
    C11_1_SPC1_Click();
});
function C11_1_SPC1_Click() {
    localStorage.setItem("C11_SPC_L_Close", 0);
    localStorage.setItem("C11_SPC_L_Label10", "First");
    localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());

    $("#C11_SPC_L_Label10").val("First");
    $("#C11_SPC_L_Label6").val($("#C11_D03").val());
    $("#C11_SPC_L").modal("open");
    C11_SPC_L_Load();
}
$("#C11_1_SPC2").click(function (e) {
    C11_1_SPC2_Click();
});
function C11_1_SPC2_Click() {
    let C11_COUNT1 = Number($("#C11_1_C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_1_C11_COUNT2").val());
    if (C11_COUNT1 > 501) {
        if (C11_COUNT1 / 2 <= C11_COUNT2) {
            localStorage.setItem("C11_SPC_L_Close", 0);
            localStorage.setItem("C11_SPC_L_Label10", "Middle");
            localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());

            $("#C11_SPC_L_Label10").val("Middle");
            $("#C11_SPC_L_Label6").val($("#C11_D03").val());
            $("#C11_SPC_L").modal("open");
            C11_SPC_L_Load();
        }
    }
}
$("#C11_1_SPC3").click(function (e) {
    C11_1_SPC3_Click();
});
function C11_1_SPC3_Click() {
    let C11_COUNT1 = Number($("#C11_1_C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_1_C11_COUNT2").val());
    let C11_COUNT3 = Number($("#C11_1_C11_COUNT3").val());
    let AA = C11_COUNT1 - C11_COUNT2;
    let BB = AA - C11_COUNT3;
    if (BB <= 0) {
        localStorage.setItem("C11_SPC_L_Close", 0);
        localStorage.setItem("C11_SPC_L_Label10", "End");
        localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());

        $("#C11_SPC_L_Label10").val("End");
        $("#C11_SPC_L_Label6").val($("#C11_D03").val());
        $("#C11_SPC_L").modal("open");
        C11_SPC_L_Load();
    }
}
$("#C11_1_Buttonprint").click(function (e) {
    C11_1_Buttonprint_Click();
});
function C11_1_Buttonprint_Click() {
    C11_SPC_L_Flag = 1;
    let IsCheck = true;
    document.getElementById("C11_1_Buttonprint").disabled = true;
    if ($("#C11_1_VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("C11_1_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let TOOLAPP = 0;
        let TOOLAPP_CNT = Number($("#C11_1_TOOL_CONT").val());
        try {
            TOOLAPP = C11_1_APP_MAX;
        }
        catch (e) {
            TOOLAPP = 1000000
        }
        if (TOOLAPP <= TOOLAPP_CNT) {
            IsCheck = false;
            alert("Application check(Check counter). Please Check Again.");
            document.getElementById("C11_1_Buttonprint").disabled = false;
        }
        if (IsCheck == true) {

            C11_1_SPC_EXIT = true;
            if (document.getElementById("C11_1_SPC1").innerHTML == "First") {
                IsCheck = false;
                $("#C11_SPC_L_Label10").val("First");
                $("#C11_SPC_L_Label6").val($("#C11_1_C11_D03").val());
                $("#C11_SPC_L").modal("open");
                C11_SPC_L_Load();
            }

            let C11_COUNT1 = Number($("#C11_1_C11_COUNT1").val());
            let C11_COUNT2 = Number($("#C11_1_C11_COUNT2").val());

            if (C11_COUNT1 > 500) {
                if (C11_COUNT1 / 2 <= C11_COUNT2) {
                    if (document.getElementById("C11_1_SPC2").innerHTML == "Middle") {
                        IsCheck = false;
                        $("#C11_SPC_L_Label10").val("Middle");
                        $("#C11_SPC_L_Label6").val($("#C11_1_C11_D03").val());
                        $("#C11_SPC_L").modal("open");
                        C11_SPC_L_Load();
                    }
                }
            }

            let A_CSPC = C11_COUNT1 - C11_COUNT2;
            let C11_COUNT3 = Number($("#C11_1_C11_COUNT3").val());
            if (C11_COUNT3 * 2 >= A_CSPC) {
                if (document.getElementById("C11_1_SPC3").innerHTML == "End") {
                    IsCheck = false;
                    $("#C11_SPC_L_Label10").val("End");
                    $("#C11_SPC_L_Label6").val($("#C11_1_C11_D03").val());
                    $("#C11_SPC_L").modal("open");
                    C11_SPC_L_Load();
                }
            }

            if (IsCheck == true) {
                C11_1_Buttonprint_ClickSub();
            }
        }
    }
}
function C11_1_Buttonprint_ClickSub() {
    let IsCheck = true;
    if (C11_1_SPC_EXIT == false) {
        IsCheck = false;
        alert("Application check(Check counter). Please Check Again.");
        document.getElementById("C11_1_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_1_C11_COUNT3").val());
        let TM_QTY = C11_COUNT3 - BaseResult.C11_1_DGV_LH.length;
        $("#C11_1_Label1").val(TM_QTY);

        let ORDER = $("#C11_1_ORDER").val();
        let VLA1 = $("#C11_1_VLA1").val();
        let VLA11 = $("#C11_1_VLA11").val();
        let C11_D01 = $("#C11_1_C11_D01").val();
        let TOOL_CONT = $("#C11_1_TOOL_CONT").val();
        let C11_D02 = $("#C11_1_C11_D02").val();
        let VLT1 = $("#C11_1_VLT1").val();

        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(C11_COUNT3);
        BaseParameter.ListSearchString.push(ORDER);
        BaseParameter.ListSearchString.push(TM_QTY);
        BaseParameter.ListSearchString.push(VLA1);
        BaseParameter.ListSearchString.push(VLA11);
        BaseParameter.ListSearchString.push(C11_D01);
        BaseParameter.ListSearchString.push(TOOL_CONT);
        BaseParameter.ListSearchString.push(C11_D02);
        BaseParameter.ListSearchString.push(VLT1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_1/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("C11_1_Buttonprint").disabled = true;
                C11_1_TI_CONT = 0;
                C11_1_Timer1StartInterval();
                M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                try {
                    C11_1_SPC_LOAD();
                }
                catch (e) {
                    alert("#1 Please Check Again. " + e);
                    document.getElementById("C11_1_Buttonprint").disabled = false;
                }
                try {
                    C11_1_ORDER_LOAD();
                }
                catch (e) {
                    alert("#2 Please Check Again. " + e);
                    document.getElementById("C11_1_Buttonprint").disabled = false;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#C11_1_BT_APP1").click(function (e) {
    C11_1_BT_APP1_Click();
});
function C11_1_BT_APP1_Click() {
    let IsCheck = true;
    if ($("#C11_1_VLA1").val() == "") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        IsC11_1 = true;
        C11_APPLICATION_ORDER_IDX = Number($("#C11_1_ORDER").val());
        $("#C11_APPLICATION_Label1").val($("#C11_1_VLA1").val());
        $("#C11_APPLICATION_Label2").val($("#C11_1_VLA11").val());
        $("#C11_APPLICATION_Label3").val("APPLICATION #L");
        $("#C11_APPLICATION").modal("open");
        C11_APPLICATION_PageLoad();
    }
}
function C11_1_DGV_LHRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C11_1_DGV_LH) {
            if (BaseResult.C11_1_DGV_LH.length > 0) {
                C11_1_DGV_LH_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C11_1_DGV_LH.length; i++) {
                    HTML = HTML + "<tr onclick='C11_1_DGV_LH_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C11_1_DGV_LH[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C11_1_DGV_LH[i].TERM1 + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C11_1_DGV_LH").innerHTML = HTML;
}
function C11_1_DGV_LH_SelectionChanged(i) {
    C11_1_DGV_LHRowIndex = i;
}
let C11_1_DGV_LHTable = document.getElementById("C11_1_DGV_LHTable");
C11_1_DGV_LHTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C11_1_DGV_LH, key, text, IsTableSort);
        C11_1_DGV_LHRender();
    }
});
function C11_SPC_L_Load() {

    $("#C11_SPC_L_MRUS").val("-");

    C11_1_SPC_EXIT = false;
    document.getElementById("C11_SPC_L_TextBox1").disabled = false;
    document.getElementById("C11_SPC_L_TextBox2").disabled = false;
    document.getElementById("C11_SPC_L_TextBox3").disabled = false;
    document.getElementById("C11_SPC_L_TextBox4").disabled = false;


    $("#C11_SPC_L_ST11").val($("#C11_1_ST_DCC1").val());
    $("#C11_SPC_L_ST12").val($("#C11_1_ST_DIC1").val());
    $("#C11_SPC_L_Label8").val($("#VLW").val());
    $("#C11_SPC_L_Label18").val($("#C11_1_WIRE_Length").val());
    $("#C11_SPC_L_Label19").val($("#C11_1_ST_DWIRE1").val());


    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#C11_SPC_L_ST11").val());
    BBBB.push($("#C11_SPC_L_ST12").val());

    let OO = 0;
    for (let i = 0; i < 2; i++) {
        if (BBBB[i].length > 0) {
            let AA = BBBB[i];
            let Flag = AA.indexOf("/");
            try {
                let BB = AA.substr(0, Flag);
                if (BB.length > 0) {
                    BB = BB.trim();
                    //let CC = BB.substr(0, 4);
                    let CC = BB.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC.length > 4) {
                        if (BB.split("-").length > 1) {
                            CC = BB.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            CC = BB.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC = "";
                    let DD = BB.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD.length > 4) {
                        if (BB.split("-").length > 1) {
                            DD = BB.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            DD = BB.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC = CC.split("(")[0].replace(/[^0-9.]/g, "");
                    }
                    catch (e) {
                        CCC = CC;
                    }
                    CCCC.push(CCC);
                    let CCCDD = Number(CCC) - Number(DD);
                    LLLL.push(CCCDD.toFixed(2));
                    CCCDD = Number(CCC) + Number(DD);
                    MMMM.push(CCCDD.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
            try {
                let BB1 = AA.substr(Flag + 1, AA.length - 1);
                if (BB1.length > 0) {
                    BB1 = BB1.trim();
                    //let CC1 = BB1.substr(0, 4);
                    let CC1 = BB1.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            CC1 = BB1.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            CC1 = BB1.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC1 = "";
                    let DD1 = BB1.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            DD1 = BB1.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            DD1 = BB1.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC1 = CC1.split("(")[0].replace(/[^0-9.]/g, "");;
                    }
                    catch (e) {
                        CCC1 = CC1;
                    }
                    CCCC.push(CCC1);
                    let CCC1DD1 = Number(CCC1) - Number(DD1);
                    LLLL.push(CCC1DD1.toFixed(2));
                    CCC1DD1 = Number(CCC1) + Number(DD1);
                    MMMM.push(CCC1DD1.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
        }
        else {
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
        }
    }

    $("#C11_SPC_L_STS1").val(CCCC[0]);
    $("#C11_SPC_L_STS2").val(CCCC[1]);
    $("#C11_SPC_L_STS3").val(CCCC[2]);
    $("#C11_SPC_L_STS4").val(CCCC[3]);

    if ($("#C11_SPC_L_STS1").val() == "NaN") {
        $("#C11_SPC_L_STS1").val("");
    }
    if ($("#C11_SPC_L_STS2").val() == "NaN") {
        $("#C11_SPC_L_STS2").val("");
    }
    if ($("#C11_SPC_L_STS3").val() == "NaN") {
        $("#C11_SPC_L_STS3").val("");
    }
    if ($("#C11_SPC_L_STS4").val() == "NaN") {
        $("#C11_SPC_L_STS4").val("");
    }

    $("#C11_SPC_L_STL1").val(LLLL[0]);
    $("#C11_SPC_L_STL2").val(LLLL[1]);
    $("#C11_SPC_L_STL3").val(LLLL[2]);
    $("#C11_SPC_L_STL4").val(LLLL[3]);

    if ($("#C11_SPC_L_STL1").val() == "NaN") {
        $("#C11_SPC_L_STL1").val("");
    }
    if ($("#C11_SPC_L_STL2").val() == "NaN") {
        $("#C11_SPC_L_STL2").val("");
    }
    if ($("#C11_SPC_L_STL3").val() == "NaN") {
        $("#C11_SPC_L_STL3").val("");
    }
    if ($("#C11_SPC_L_STL4").val() == "NaN") {
        $("#C11_SPC_L_STL4").val("");
    }

    $("#C11_SPC_L_STM1").val(MMMM[0]);
    $("#C11_SPC_L_STM2").val(MMMM[1]);
    $("#C11_SPC_L_STM3").val(MMMM[2]);
    $("#C11_SPC_L_STM4").val(MMMM[3]);

    if ($("#C11_SPC_L_STM1").val() == "NaN") {
        $("#C11_SPC_L_STM1").val("");
    }
    if ($("#C11_SPC_L_STM2").val() == "NaN") {
        $("#C11_SPC_L_STM2").val("");
    }
    if ($("#C11_SPC_L_STM3").val() == "NaN") {
        $("#C11_SPC_L_STM3").val("");
    }
    if ($("#C11_SPC_L_STM4").val() == "NaN") {
        $("#C11_SPC_L_STM4").val("");
    }

    if ($("#C11_SPC_L_STS1").val() == "") {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_L_STS2").val() == "") {
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_L_STS3").val() == "") {
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_L_STS4").val() == "") {
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }


    $("#C11_SPC_L_Label4").val($("#VLT1").val());
    $("#C11_SPC_L_Label5").val($("#VLT1").val());


    let Label4 = $("#C11_SPC_L_Label4").val();


    let BBB = Label4.includes("(");

    if (BBB == true) {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }

    $("#C11_SPC_L_TextBox1").val("");
    $("#C11_SPC_L_TextBox2").val("");
    $("#C11_SPC_L_TextBox3").val("");
    $("#C11_SPC_L_TextBox4").val("");
    $("#C11_SPC_L_TextBox10").val("");

    $("#C11_SPC_L_LR1").val("");
    $("#C11_SPC_L_LR2").val("");
    $("#C11_SPC_L_LR3").val("");
    $("#C11_SPC_L_LR4").val("");
    $("#C11_SPC_L_LR9").val("");

    document.getElementById("C11_SPC_L_MRUS").style.backgroundColor = "white";

    document.getElementById("C11_SPC_L_LR1").style.backgroundColor = "white";
    document.getElementById("C11_SPC_L_LR2").style.backgroundColor = "white";
    document.getElementById("C11_SPC_L_LR3").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_L_LR4").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_L_LR9").style.backgroundColor = "white";


    C02_SPC_Load();

    if (BBB == true) {
        $("#C11_SPC_L").modal("close");
    }

    if ($("#C11_SPC_L_STL1").val() == "") {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_L_STL2").val() == "") {
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_L_STL3").val() == "") {
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_L_STL4").val() == "") {
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }
    $("#C11_SPC_L_TextBox1").focus();
}
function C02_SPC_Load() {
    let F_CO = $("#C11_SPC_L_Label19").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_SPC_L/C02_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_SPC_L_Search = data.Search;
            if (BaseResult.C11_SPC_L_Search.length == 0) {
                $("#C11_SPC_L_Label17").val(0);
            }
            $("#C11_SPC_L_Label17").val(BaseResult.C11_SPC_L_Search[0].STRENGTH);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_SPC_L_Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("C11_SPC_L_TextBox1").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox2").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox3").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox4").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox10").disabled);

    let AA = [];
    AA.push($("#C11_SPC_L_LR1").val());
    AA.push($("#C11_SPC_L_LR2").val());
    AA.push($("#C11_SPC_L_LR3").val());
    AA.push($("#C11_SPC_L_LR4").val());
    AA.push($("#C11_SPC_L_LR9").val());


    for (let i = 0; i < CHK.length; i++) {
        if (CHK[i] == false) {
            if (AA[i] == "OK") {
                VAL.push(true);
            }
        }
        else {
            VAL.push(true);
        }
    }
    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true)) {
        $("#C11_SPC_L_MRUS").val("OK");
        document.getElementById("C11_SPC_L_MRUS").style.color = "green";
    }
    else {
        $("#C11_SPC_L_MRUS").val("NG");
        document.getElementById("C11_SPC_L_MRUS").style.color = "red";
    }
}
$("#C11_SPC_L_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox1_KeyDown();
    }
});
function C11_SPC_L_TextBox1_KeyDown() {
    $("#C11_SPC_L_TextBox2").focus();
}
$("#C11_SPC_L_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox2_KeyDown();
    }
});
function C11_SPC_L_TextBox2_KeyDown() {
    $("#C11_SPC_L_TextBox3").focus();
}
$("#C11_SPC_L_TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox3_KeyDown();
    }
});
function C11_SPC_L_TextBox3_KeyDown() {
    $("#C11_SPC_L_TextBox4").focus();
}
$("#C11_SPC_L_TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox4_KeyDown();
    }
});
function C11_SPC_L_TextBox4_KeyDown() {
    $("#C11_SPC_L_TextBox10").focus();
}

$("#C11_SPC_L_TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox10_KeyDown();
    }
});
function C11_SPC_L_TextBox10_KeyDown() {
    $("#C11_SPC_L_TextBox1").focus();
}
$("#C11_SPC_L_TextBox1").change(function (e) {
    C11_SPC_L_TextBox1_TextChanged()
});
function C11_SPC_L_TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#C11_SPC_L_TextBox1").val();
        let STL1 = $("#C11_SPC_L_STL1").val();
        let STM1 = $("#C11_SPC_L_STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#C11_SPC_L_LR1").val("OK");
            document.getElementById("C11_SPC_L_LR1").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR1").val("NG");
            document.getElementById("C11_SPC_L_LR1").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox2").change(function (e) {
    C11_SPC_L_TextBox2_TextChanged()
});
function C11_SPC_L_TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#C11_SPC_L_TextBox2").val();
        let STL2 = $("#C11_SPC_L_STL2").val();
        let STM2 = $("#C11_SPC_L_STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#C11_SPC_L_LR2").val("OK");
            document.getElementById("C11_SPC_L_LR2").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR2").val("NG");
            document.getElementById("C11_SPC_L_LR2").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox3").change(function (e) {
    C11_SPC_L_TextBox3_TextChanged()
});
function C11_SPC_L_TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#C11_SPC_L_TextBox3").val();
        let STL3 = $("#C11_SPC_L_STL3").val();
        let STM3 = $("#C11_SPC_L_STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#C11_SPC_L_LR3").val("OK");
            document.getElementById("C11_SPC_L_LR3").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR3").val("NG");
            document.getElementById("C11_SPC_L_LR3").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox4").change(function (e) {
    C11_SPC_L_TextBox4_TextChanged()
});
function C11_SPC_L_TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#C11_SPC_L_TextBox4").val();
        let STL4 = $("#C11_SPC_L_STL4").val();
        let STM4 = $("#C11_SPC_L_STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#C11_SPC_L_LR4").val("OK");
            document.getElementById("C11_SPC_L_LR4").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR4").val("NG");
            document.getElementById("C11_SPC_L_LR4").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}

$("#C11_SPC_L_TextBox10").change(function (e) {
    C11_SPC_L_TextBox10_TextChanged()
});
function C11_SPC_L_TextBox10_TextChanged() {
    try {
        let AA = Number($("#C11_SPC_L_TextBox10").val());
        let CC = Number($("#C11_SPC_L_Label17").val());

        if (CC <= AA) {
            $("#C11_SPC_L_LR9").val("OK");
            document.getElementById("C11_SPC_L_LR9").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR9").val("NG");
            document.getElementById("C11_SPC_L_LR9").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_Button1").click(function (e) {
    C11_SPC_L_Button1_Click();
});
function C11_SPC_L_Button1_Click() {
    let IsCheck = true;
    let OR_IDX = $("#ST_BCIDX").val();
    let TextBox1 = $("#C11_SPC_L_TextBox1").val();
    let TextBox2 = $("#C11_SPC_L_TextBox2").val();
    let TextBox3 = $("#C11_SPC_L_TextBox3").val();
    let TextBox4 = $("#C11_SPC_L_TextBox4").val();
    let TextBox10 = $("#C11_SPC_L_TextBox10").val();

    let Label6 = $("#C11_SPC_L_Label6").val();
    let Label10 = $("#C11_SPC_L_Label10").val();

    let MRUS = $("#C11_SPC_L_MRUS").val();
    let MC2 = $("#MCbox").val();
    let STRENGTH = $("#C11_SPC_L_Label17").val();

    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            //$("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox1);
            BaseParameter.ListSearchString.push(TextBox2);
            BaseParameter.ListSearchString.push(TextBox3);
            BaseParameter.ListSearchString.push(TextBox4);
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label6);
            BaseParameter.ListSearchString.push(Label10);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(OR_IDX);
            BaseParameter.ListSearchString.push(MC2);
            BaseParameter.ListSearchString.push(STRENGTH);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_SPC_L/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    C11_1_SPC_EXIT = true;
                    C11_SPC_L_Close();
                    $("#C11_SPC_L").modal("close");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function C11_SPC_L_Close() {
    if (C11_SPC_L_Flag == 0) {
        C11_1_SPC_LOAD();
    }
    if (C11_SPC_L_Flag == 1) {
        C11_1_Buttonprint_ClickSub();
    }
}

let C11_APPLICATION_ORDER_IDX;
function C11_APPLICATION_PageLoad() {

    $("#C11_APPLICATION_Label5").val("---");
    $("#C11_APPLICATION_Label4").val("---");

    $("#C11_APPLICATION_TextBox1").val("");
    $("#C11_APPLICATION_TextBox1").focus();
}
$("#C11_APPLICATION_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_APPLICATION_TextBox1_KeyDown();
    }
});
function C11_APPLICATION_TextBox1_KeyDown() {
    C11_APPLICATION_Button1_Click();
}
$("#C11_APPLICATION_Button1").click(function (e) {
    C11_APPLICATION_Button1_Click();
});
function C11_APPLICATION_Button1_Click() {
    let IsCheck = true;
    let APPT = $("#C11_APPLICATION_TextBox1").val();
    APPT = APPT.toUpperCase();
    let CO = APPT.length;
    let AAA = APPT.substr(0, CO - 1);
    let BBB = APPT.substr(CO - 1, 1);
    if ($("#C11_APPLICATION_Label3").val() == "APPLICATION #J") {
        let APP_12 = false;
        if (AAA == C11_3_TL_NM) {
            $("#C11_APPLICATION_Label1").val(C11_3_TL_NM);
        }
    }
    if ($("#C11_APPLICATION_Label3").val() == "APPLICATION #J2") {
        let APP_12 = false;
        if (AAA == C11_4_TR_NM) {
            $("#C11_APPLICATION_Label1").val(C11_4_TR_NM);
        }
    }
    let Label1 = $("#C11_APPLICATION_Label1").val();
    Label1 = Label1.toUpperCase();
    if (AAA == Label1) {
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_APPLICATION/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C11_APPLICATION_Search = data.Search;
                if (BaseResult.C11_APPLICATION_Search.length <= 0) {
                    IsCheck = false
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    $("#C11_APPLICATION_Label5").val(BaseResult.C11_APPLICATION_Search[0].APP_NAME);
                    $("#C11_APPLICATION_Label4").val(BaseResult.C11_APPLICATION_Search[0].SEQ);
                    $("#C11_APPLICATION_Label9").val(BaseResult.C11_APPLICATION_Search[0].WK_CNT);
                    $("#C11_APPLICATION_Label10").val(BaseResult.C11_APPLICATION_Search[0].TOOLMASTER_IDX);
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        IsCheck = false
        alert("APPLICATOR을 잘못 등록했습니다. Đăng ký sai của ỨNG DỤNG.");
    }
}
$("#C11_APPLICATION_Button2").click(function (e) {
    C11_APPLICATION_Button2_Click();
});
function C11_APPLICATION_Button2_Click() {
    let IsCheck = true;
    let Label5 = $("#C11_APPLICATION_Label5").val();
    if (Label5 == "---") {
        IsCheck = false;
    }
    let Label4 = $("#C11_APPLICATION_Label4").val();
    if (Label4 == "---") {
        IsCheck = false;
    }
    let Label3 = $("#C11_APPLICATION_Label3").val();
    let Label10 = $("#C11_APPLICATION_Label10").val();

    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label3);
    BaseParameter.ListSearchString.push(Label10);
    BaseParameter.ListSearchString.push(ORDER_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_APPLICATION/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            C11_APPLICATION_Buttonclose_Click_1();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_APPLICATION_Buttonclose").click(function (e) {
    C11_APPLICATION_Buttonclose_Click_1();
});
function C11_APPLICATION_Buttonclose_Click_1() {
    $("#C11_APPLICATION").modal("close");
    if (IsC11_1 == true) {
        C11_1_ORDER_LOAD(1);
        IsC11_1 = false;
    }
    if (IsC11_2 == true) {
        C11_2_ORDER_LOAD(1);
        IsC11_2 = false;
    }
    if (IsC11_3 == true) {
        C11_3_ORDER_LOAD(1);
        IsC11_3 = false;
    }
    if (IsC11_4 == true) {
        C11_4_ORDER_LOAD(1);
        IsC11_4 = false;
    }
}

let C11_2_Timer1;
let C11_2_C_EXIT = true;
let C11_2_DGV_RHRowIndex = 0;
let C11_2_TI_CONT;
let C11_2_APP_MAX;
let C11_2_SPC_EXIT;
let C11_SPC_R_Flag = 0;

function C11_2_PageLoad() {
    C11_2_DGV_RHRender();

    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_2_C11_D02").val(SettingsMC_NM);

    C11_2_ORDER_LOAD(0);
}
function C11_2_ORDER_LOAD(Flag) {
    let IsCheck = true;
    let ORDER = $("#C11_2_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_2/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_2_Search = data.Search;
            if (BaseResult.C11_2_Search.length <= 0) {
                IsCheck = false;
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra.");
                C11_2_Buttonclose_Click();
            }
            else {
                try {
                    $("#C11_2_VLA1").val(BaseResult.C11_2_Search[0].APP);
                    $("#C11_2_VLA11").val(BaseResult.C11_2_Search[0].SEQ);
                    $("#C11_2_TOOL_CONT").val(BaseResult.C11_2_Search[0].COUNT);
                    $("#C11_2_C11_COUNT2").val(BaseResult.C11_2_Search[0].PERFORMN);
                    $("#C11_2_VLT1").val($("#C11_2_C11_D04").val());
                    C11_2_APP_MAX = BaseResult.C11_2_Search[0].MAX;
                    if ($("#C11_2_TOOL_CONT").val() == "") {
                        $("#C11_2_TOOL_CONT").val(0);
                    }
                    if ($("#C11_2_C11_COUNT2").val() == "") {
                        $("#C11_2_C11_COUNT2").val(0);
                    }
                    $("#C11_2_Label1").val(Number($("#C11_2_C11_COUNT2").val()) * BaseResult.C11_2_DGV_RH.length);
                    if (BaseResult.C11_2_Search[0].PERFORMN >= BaseResult.C11_2_Search[0].TOT_COUNT) {
                        IsCheck = false;
                        alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                        C11_2_Buttonclose_Click();
                    }
                }
                catch (e) {
                    IsCheck = false;
                    $("#C11_2_VLA1").val("");
                    $("#C11_2_VLA11").val("");
                    $("#C11_2_TOOL_CONT").val("");
                    $("#C11_2_C11_COUNT2").val("");
                    $("#C11_2_VLT1").val("");
                    $("#C11_ER_R_LBT2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_R_LBA2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_R_Label1").val($("#ST_BCIDX").val());

                    BaseResult.C11_2_DGV_RH = [];
                    C11_2_DGV_RHRender();

                    $("#C11_2").modal("close");
                    C11_2_Buttonclose_Click();

                    $("#C11_ER_R").modal("close");
                    C11_ER_R_Buttonclose_Click();

                    $("#C11_ER_R_LBT2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_R_LBA2S").val(BaseResult.R_DGV[0].TERM1);
                    $("#C11_ER_R_Label1").val($("#ST_BCIDX").val());

                    $("#C11_ER_R").modal("open");
                }
            }

            if (IsCheck == true) {
                if (Flag == 0) {
                    if (C11_2_C_EXIT == true) {
                        IsCheck = false;
                        C11_2_Buttonclose_Click();
                    }
                    if (IsCheck == true) {
                        C11_2_SPC_LOAD();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_2_SPC_LOAD() {
    let ORDER = $("#C11_2_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_2/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_2_Search1 = data.Search1;
            try {
                if (Number($("#C11_2_C11_COUNT1").val()) < 11) {
                    document.getElementById("C11_2_SPC1").innerHTML = "----";
                    document.getElementById("C11_2_SPC1").disabled = true;
                }
                else {
                    document.getElementById("C11_2_SPC1").innerHTML = "First";
                    document.getElementById("C11_2_SPC1").disabled = false;
                }
                if (Number($("#C11_2_C11_COUNT1").val()) < 501) {
                    document.getElementById("C11_2_SPC2").innerHTML = "----";
                    document.getElementById("C11_2_SPC2").disabled = true;
                }
                else {
                    document.getElementById("C11_2_SPC2").innerHTML = "Middle";
                    document.getElementById("C11_2_SPC2").disabled = false;
                }
            }
            catch (e) {

            }
            if (BaseResult.C11_2_Search1.length > 0) {
                for (let i = 0; i < BaseResult.C11_2_Search1.length; i++) {
                    if (BaseResult.C11_2_Search1[i].COLSIP == "First") {
                        document.getElementById("C11_2_SPC1").innerHTML = "Complete";
                        document.getElementById("C11_2_SPC1").disabled = true;
                    }
                    if (BaseResult.C11_2_Search1[i].COLSIP == "Middle") {
                        document.getElementById("C11_2_SPC2").innerHTML = "Complete";
                        document.getElementById("C11_2_SPC2").disabled = true;
                    }
                    if (BaseResult.C11_2_Search1[i].COLSIP == "End") {
                        document.getElementById("C11_2_SPC3").innerHTML = "Complete";
                        document.getElementById("C11_2_SPC3").disabled = true;
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_2_Timer1StartInterval() {
    C11_2_Timer1 = setInterval(function () {
        C11_2_Timer1_Tick();
    }, 200);
}
function C11_2_Timer1_Tick() {
    if (C11_2_TI_CONT >= 60) {
        document.getElementById("C11_2_Buttonprint").disabled = false;
        clearInterval(C11_2_Timer1);
    }
    else {
        C11_2_TI_CONT = C11_2_TI_CONT + 1;
        document.getElementById("C11_2_Buttonprint").innerHTML = "Complete" + "  (" + C11_2_TI_CONT + ")";
    }
}
$("#C11_2_SPC1").click(function (e) {
    C11_2_SPC1_Click();
});
function C11_2_SPC1_Click() {
    $("#C11_SPC_R_Label10").val("First");
    $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
    $("#C11_SPC_R").modal("open");
    C11_SPC_R_Load();
}
$("#C11_2_SPC2").click(function (e) {
    C11_2_SPC2_Click();
});
function C11_2_SPC2_Click() {
    let C11_COUNT1 = Number($("#C11_2_C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_2_C11_COUNT2").val());
    if (C11_COUNT1 > 501) {
        if (C11_COUNT1 / 2 <= C11_COUNT2) {
            $("#C11_SPC_R_Label10").val("Middle");
            $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
            $("#C11_SPC_R").modal("open");
            C11_SPC_R_Load();
        }
    }
}
$("#C11_2_SPC3").click(function (e) {
    C11_2_SPC3_Click();
});
function C11_2_SPC3_Click() {
    let C11_COUNT1 = Number($("#C11_2_C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_2_C11_COUNT2").val());
    let C11_COUNT3 = Number($("#C11_2_C11_COUNT3").val());
    let AA = C11_COUNT1 - C11_COUNT2;
    let BB = AA - C11_COUNT3;
    if (BB <= 0) {
        $("#C11_SPC_R_Label10").val("End");
        $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
        $("#C11_SPC_R").modal("open");
        C11_SPC_R_Load();
    }
}
$("#C11_2_Buttonprint").click(function (e) {
    C11_2_Buttonprint_Click();
});
function C11_2_Buttonprint_Click() {
    C11_SPC_R_Flag = 1;
    let IsCheck = true;
    document.getElementById("C11_2_Buttonprint").disabled = true;
    if ($("#C11_2_VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("C11_2_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let TOOLAPP = 0;
        let TOOLAPP_CNT = Number($("#C11_2_TOOL_CONT").val());
        try {
            TOOLAPP = C11_2_APP_MAX;
        }
        catch (e) {
            TOOLAPP = 1000000
        }
        if (TOOLAPP <= TOOLAPP_CNT) {
            IsCheck = false;
            alert("Application check(Check counter). Please Check Again.");
            document.getElementById("C11_2_Buttonprint").disabled = false;
        }
        if (IsCheck == true) {
            C11_2_SPC_EXIT = true;
            if (document.getElementById("C11_2_SPC1").innerHTML == "First") {
                $("#C11_SPC_R_Label10").val("First");
                $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
                $("#C11_SPC_R").modal("open");
                C11_SPC_R_Load();
                IsCheck = false;
            }
            let C11_COUNT1 = Number($("#C11_2_C11_COUNT1").val());
            let C11_COUNT2 = Number($("#C11_2_C11_COUNT2").val());

            if (C11_COUNT1 > 500) {
                if (C11_COUNT1 / 2 <= C11_COUNT2) {
                    if (document.getElementById("C11_2_SPC2").innerHTML == "Middle") {
                        $("#C11_SPC_R_Label10").val("Middle");
                        $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
                        $("#C11_SPC_R").modal("open");
                        C11_SPC_R_Load();
                        IsCheck = false;
                    }
                }
            }
            let A_CSPC = C11_COUNT1 - C11_COUNT2;
            let C11_COUNT3 = Number($("#C11_2_C11_COUNT3").val());
            if (C11_COUNT3 * 2 >= A_CSPC) {
                if (document.getElementById("C11_2_SPC3").innerHTML == "End") {
                    $("#C11_SPC_R_Label10").val("End");
                    $("#C11_SPC_R_Label6").val($("#C11_2_C11_D03").val());
                    $("#C11_SPC_R").modal("open");
                    C11_SPC_R_Load();
                    IsCheck = false;
                }
            }
            if (IsCheck == true) {
                C11_2_Buttonprint_ClickSub();
            }
        }
    }
}
function C11_2_Buttonprint_ClickSub() {
    let IsCheck = true;
    if (C11_2_SPC_EXIT == false) {
        IsCheck = false;
        alert("Application check(Check counter). Please Check Again.");
        document.getElementById("C11_2_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_2_C11_COUNT3").val());
        let TM_QTY = C11_COUNT3 - BaseResult.C11_2_DGV_RH.length;
        $("#C11_2_Label1").val(TM_QTY);

        let ORDER = $("#C11_2_ORDER").val();
        let VLA1 = $("#C11_2_VLA1").val();
        let VLA11 = $("#C11_2_VLA11").val();
        let C11_D01 = $("#C11_2_C11_D01").val();
        let TOOL_CONT = $("#C11_2_TOOL_CONT").val();
        let C11_D02 = $("#C11_2_C11_D02").val();
        let VLT1 = $("#C11_2_VLT1").val();

        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(C11_COUNT3);
        BaseParameter.ListSearchString.push(ORDER);
        BaseParameter.ListSearchString.push(TM_QTY);
        BaseParameter.ListSearchString.push(VLA1);
        BaseParameter.ListSearchString.push(VLA11);
        BaseParameter.ListSearchString.push(C11_D01);
        BaseParameter.ListSearchString.push(TOOL_CONT);
        BaseParameter.ListSearchString.push(C11_D02);
        BaseParameter.ListSearchString.push(VLT1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_2/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("C11_2_Buttonprint").disabled = true;
                C11_2_TI_CONT = 0;
                C11_2_Timer1StartInterval();
                M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                try {
                    C11_2_SPC_LOAD();
                }
                catch (e) {
                    alert("#1 Please Check Again. " + e);
                    document.getElementById("C11_2_Buttonprint").disabled = false;
                }
                try {
                    C11_2_ORDER_LOAD();
                }
                catch (e) {
                    alert("#2 Please Check Again. " + e);
                    document.getElementById("C11_2_Buttonprint").disabled = false;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#C11_2_BT_APP1").click(function (e) {
    C11_2_BT_APP1_Click();
});
function C11_2_BT_APP1_Click() {
    let IsCheck = true;
    if ($("#C11_2_VLA1").val() == "") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        IsC11_2 = true;
        C11_APPLICATION_ORDER_IDX = Number($("#C11_2_ORDER").val());
        $("#C11_APPLICATION_Label1").val($("#C11_2_VLA1").val());
        $("#C11_APPLICATION_Label2").val($("#C11_2_VLA11").val());
        $("#C11_APPLICATION_Label3").val("APPLICATION #R");
        $("#C11_APPLICATION").modal("open");
        C11_APPLICATION_PageLoad();
    }
}
$("#C11_2_Buttonclose").click(function (e) {
    C11_2_Buttonclose_Click();
});
function C11_2_Buttonclose_Click() {
    C11_2_FormClosed();
}
function C11_2_FormClosed() {
    $("#C11_2").modal("close");
    $("#Barcodebox").val($("#ST_BC").val());
    Barcod_read();
}
function C11_2_DGV_RHRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C11_2_DGV_RH) {
            if (BaseResult.C11_2_DGV_RH.length > 0) {
                C11_2_DGV_RH_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C11_2_DGV_RH.length; i++) {
                    HTML = HTML + "<tr onclick='C11_2_DGV_RH_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C11_2_DGV_RH[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C11_2_DGV_RH[i].TERM1 + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C11_2_DGV_RH").innerHTML = HTML;
}
function C11_2_DGV_RH_SelectionChanged(i) {
    C11_2_DGV_RHRowIndex = i;
}
let C11_2_DGV_RHTable = document.getElementById("C11_2_DGV_RHTable");
C11_2_DGV_RHTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C11_2_DGV_RH, key, text, IsTableSort);
        C11_2_DGV_RHRender();
    }
});

function C11_SPC_R_Load() {

    $("#C11_SPC_R_MRUS").val("-");

    C11_2_SPC_EXIT = false;

    document.getElementById("C11_SPC_R_TextBox1").disabled = false;
    document.getElementById("C11_SPC_R_TextBox2").disabled = false;
    document.getElementById("C11_SPC_R_TextBox3").disabled = false;
    document.getElementById("C11_SPC_R_TextBox4").disabled = false;


    $("#C11_SPC_R_ST11").val($("#C11_2_ST_DCC1").val());
    $("#C11_SPC_R_ST12").val($("#C11_2_ST_DIC1").val());
    $("#C11_SPC_R_Label8").val($("#VLW").val());
    $("#C11_SPC_R_Label18").val($("#C11_2_WIRE_Length").val());
    $("#C11_SPC_R_Label19").val($("#C11_2_ST_DWIRE1").val());

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#C11_SPC_R_ST11").val());
    BBBB.push($("#C11_SPC_R_ST12").val());

    let OO = 0;
    for (let i = 0; i < 2; i++) {
        if (BBBB[i].length > 0) {
            let AA = BBBB[i];
            let Flag = AA.indexOf("/");
            try {
                let BB = AA.substr(0, Flag);
                if (BB.length > 0) {
                    BB = BB.trim();
                    //let CC = BB.substr(0, 4);
                    let CC = BB.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC.length > 4) {
                        if (BB.split("-").length > 1) {
                            CC = BB.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            CC = BB.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC = "";
                    let DD = BB.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD.length > 4) {
                        if (BB.split("-").length > 1) {
                            DD = BB.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            DD = BB.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC = CC.split("(")[0].replace(/[^0-9.]/g, "");
                    }
                    catch (e) {
                        CCC = CC;
                    }
                    CCCC.push(CCC);
                    let CCCDD = Number(CCC) - Number(DD);
                    LLLL.push(CCCDD.toFixed(2));
                    CCCDD = Number(CCC) + Number(DD);
                    MMMM.push(CCCDD.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
            try {
                let BB1 = AA.substr(Flag + 1, AA.length - 1);
                if (BB1.length > 0) {
                    BB1 = BB1.trim();
                    //let CC1 = BB1.substr(0, 4);
                    let CC1 = BB1.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            CC1 = BB1.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            CC1 = BB1.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC1 = "";
                    let DD1 = BB1.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            DD1 = BB1.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            DD1 = BB1.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC1 = CC1.split("(")[0].replace(/[^0-9.]/g, "");;
                    }
                    catch (e) {
                        CCC1 = CC1;
                    }
                    CCCC.push(CCC1);
                    let CCC1DD1 = Number(CCC1) - Number(DD1);
                    LLLL.push(CCC1DD1.toFixed(2));
                    CCC1DD1 = Number(CCC1) + Number(DD1);
                    MMMM.push(CCC1DD1.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
        }
        else {
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
        }
    }
    $("#C11_SPC_R_STS1").val(CCCC[0]);
    $("#C11_SPC_R_STS2").val(CCCC[1]);
    $("#C11_SPC_R_STS3").val(CCCC[2]);
    $("#C11_SPC_R_STS4").val(CCCC[3]);

    if ($("#C11_SPC_R_STS1").val() == "NaN") {
        $("#C11_SPC_R_STS1").val("");
    }
    if ($("#C11_SPC_R_STS2").val() == "NaN") {
        $("#C11_SPC_R_STS2").val("");
    }
    if ($("#C11_SPC_R_STS3").val() == "NaN") {
        $("#C11_SPC_R_STS3").val("");
    }
    if ($("#C11_SPC_R_STS4").val() == "NaN") {
        $("#C11_SPC_R_STS4").val("");
    }

    $("#C11_SPC_R_STL1").val(LLLL[0]);
    $("#C11_SPC_R_STL2").val(LLLL[1]);
    $("#C11_SPC_R_STL3").val(LLLL[2]);
    $("#C11_SPC_R_STL4").val(LLLL[3]);

    if ($("#C11_SPC_R_STL1").val() == "NaN") {
        $("#C11_SPC_R_STL1").val("");
    }
    if ($("#C11_SPC_R_STL2").val() == "NaN") {
        $("#C11_SPC_R_STL2").val("");
    }
    if ($("#C11_SPC_R_STL3").val() == "NaN") {
        $("#C11_SPC_R_STL3").val("");
    }
    if ($("#C11_SPC_R_STL4").val() == "NaN") {
        $("#C11_SPC_R_STL4").val("");
    }

    $("#C11_SPC_R_STM1").val(MMMM[0]);
    $("#C11_SPC_R_STM2").val(MMMM[1]);
    $("#C11_SPC_R_STM3").val(MMMM[2]);
    $("#C11_SPC_R_STM4").val(MMMM[3]);

    if ($("#C11_SPC_R_STM1").val() == "NaN") {
        $("#C11_SPC_R_STM1").val("");
    }
    if ($("#C11_SPC_R_STM2").val() == "NaN") {
        $("#C11_SPC_R_STM2").val("");
    }
    if ($("#C11_SPC_R_STM3").val() == "NaN") {
        $("#C11_SPC_R_STM3").val("");
    }
    if ($("#C11_SPC_R_STM4").val() == "NaN") {
        $("#C11_SPC_R_STM4").val("");
    }

    if ($("#C11_SPC_R_STS1").val() == "") {
        document.getElementById("C11_SPC_R_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_R_STS2").val() == "") {
        document.getElementById("C11_SPC_R_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_R_STS3").val() == "") {
        document.getElementById("C11_SPC_R_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_R_STS4").val() == "") {
        document.getElementById("C11_SPC_R_TextBox4").disabled = true;
    }


    $("#C11_SPC_R_Label4").val($("#C11_2_VLT1").val());
    $("#C11_SPC_R_Label5").val($("#C11_2_VLT1").val());


    let Label4 = $("#C11_SPC_R_Label4").val();


    let BBB = Label4.includes("(");

    if (BBB == true) {
        document.getElementById("C11_SPC_R_TextBox1").disabled = true;
        document.getElementById("C11_SPC_R_TextBox2").disabled = true;
        document.getElementById("C11_SPC_R_TextBox3").disabled = true;
        document.getElementById("C11_SPC_R_TextBox4").disabled = true;
    }

    $("#C11_SPC_R_TextBox1").val("");
    $("#C11_SPC_R_TextBox2").val("");
    $("#C11_SPC_R_TextBox3").val("");
    $("#C11_SPC_R_TextBox4").val("");
    $("#C11_SPC_R_TextBox10").val("");

    $("#C11_SPC_R_LR1").val("");
    $("#C11_SPC_R_LR2").val("");
    $("#C11_SPC_R_LR3").val("");
    $("#C11_SPC_R_LR4").val("");
    $("#C11_SPC_R_LR9").val("");

    document.getElementById("C11_SPC_R_MRUS").style.backgroundColor = "white";

    document.getElementById("C11_SPC_R_LR1").style.backgroundColor = "white";
    document.getElementById("C11_SPC_R_LR2").style.backgroundColor = "white";
    document.getElementById("C11_SPC_R_LR3").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_R_LR4").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_R_LR9").style.backgroundColor = "white";


    C11_SPC_R_C02_SPC_Load();

    if ((BBB == true)) {
        $("#C11_SPC_R").modal("close");
    }

    if ($("#C11_SPC_R_STL1").val() == "") {
        document.getElementById("C11_SPC_R_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_R_STL2").val() == "") {
        document.getElementById("C11_SPC_R_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_R_STL3").val() == "") {
        document.getElementById("C11_SPC_R_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_R_STL4").val() == "") {
        document.getElementById("C11_SPC_R_TextBox4").disabled = true;
    }
    $("#C11_SPC_R_TextBox1").focus();
}

function C11_SPC_R_C02_SPC_Load() {
    let F_CO = $("#C11_SPC_R_Label19").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_SPC_R/C02_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_SPC_R_Search = data.Search;
            if (BaseResult.C11_SPC_R_Search.length == 0) {
                $("#C11_SPC_R_Label17").val(0);
            }
            $("#C11_SPC_R_Label17").val(BaseResult.C11_SPC_R_Search[0].STRENGTH);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_SPC_R_Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("C11_SPC_R_TextBox1").disabled);
    CHK.push(document.getElementById("C11_SPC_R_TextBox2").disabled);
    CHK.push(document.getElementById("C11_SPC_R_TextBox3").disabled);
    CHK.push(document.getElementById("C11_SPC_R_TextBox4").disabled);
    CHK.push(document.getElementById("C11_SPC_R_TextBox10").disabled);

    let AA = [];
    AA.push($("#C11_SPC_R_LR1").val());
    AA.push($("#C11_SPC_R_LR2").val());
    AA.push($("#C11_SPC_R_LR3").val());
    AA.push($("#C11_SPC_R_LR4").val());
    AA.push($("#C11_SPC_R_LR9").val());


    for (let i = 0; i < CHK.length; i++) {
        if (CHK[i] == false) {
            if (AA[i] == "OK") {
                VAL.push(true);
            }
        }
        else {
            VAL.push(true);
        }
    }
    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true)) {
        $("#C11_SPC_R_MRUS").val("OK");
        document.getElementById("C11_SPC_R_MRUS").style.color = "green";
    }
    else {
        $("#C11_SPC_R_MRUS").val("NG");
        document.getElementById("C11_SPC_R_MRUS").style.color = "red";
    }
}
$("#C11_SPC_R_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_R_TextBox1_KeyDown();
    }
});
function C11_SPC_R_TextBox1_KeyDown() {
    $("#C11_SPC_R_TextBox2").focus();
}
$("#C11_SPC_R_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_R_TextBox2_KeyDown();
    }
});
function C11_SPC_R_TextBox2_KeyDown() {
    $("#C11_SPC_R_TextBox3").focus();
}
$("#C11_SPC_R_TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_R_TextBox3_KeyDown();
    }
});
function C11_SPC_R_TextBox3_KeyDown() {
    $("#C11_SPC_R_TextBox4").focus();
}
$("#C11_SPC_R_TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_R_TextBox4_KeyDown();
    }
});
function C11_SPC_R_TextBox4_KeyDown() {
    $("#C11_SPC_R_TextBox10").focus();
}

$("#C11_SPC_R_TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_R_TextBox10_KeyDown();
    }
});
function C11_SPC_R_TextBox10_KeyDown() {
    $("#C11_SPC_R_TextBox1").focus();
}
$("#C11_SPC_R_TextBox1").change(function (e) {
    C11_SPC_R_TextBox1_TextChanged()
});
function C11_SPC_R_TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#C11_SPC_R_TextBox1").val();
        let STL1 = $("#C11_SPC_R_STL1").val();
        let STM1 = $("#C11_SPC_R_STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#C11_SPC_R_LR1").val("OK");
            document.getElementById("C11_SPC_R_LR1").style.color = "green";
        }
        else {
            $("#C11_SPC_R_LR1").val("NG");
            document.getElementById("C11_SPC_R_LR1").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_R_Form_RUS();
}
$("#C11_SPC_R_TextBox2").change(function (e) {
    C11_SPC_R_TextBox2_TextChanged()
});
function C11_SPC_R_TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#C11_SPC_R_TextBox2").val();
        let STL2 = $("#C11_SPC_R_STL2").val();
        let STM2 = $("#C11_SPC_R_STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#C11_SPC_R_LR2").val("OK");
            document.getElementById("C11_SPC_R_LR2").style.color = "green";
        }
        else {
            $("#C11_SPC_R_LR2").val("NG");
            document.getElementById("C11_SPC_R_LR2").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_R_Form_RUS();
}
$("#C11_SPC_R_TextBox3").change(function (e) {
    C11_SPC_R_TextBox3_TextChanged()
});
function C11_SPC_R_TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#C11_SPC_R_TextBox3").val();
        let STL3 = $("#C11_SPC_R_STL3").val();
        let STM3 = $("#C11_SPC_R_STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#C11_SPC_R_LR3").val("OK");
            document.getElementById("C11_SPC_R_LR3").style.color = "green";
        }
        else {
            $("#C11_SPC_R_LR3").val("NG");
            document.getElementById("C11_SPC_R_LR3").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_R_Form_RUS();
}
$("#C11_SPC_R_TextBox4").change(function (e) {
    C11_SPC_R_TextBox4_TextChanged()
});
function C11_SPC_R_TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#C11_SPC_R_TextBox4").val();
        let STL4 = $("#C11_SPC_R_STL4").val();
        let STM4 = $("#C11_SPC_R_STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#C11_SPC_R_LR4").val("OK");
            document.getElementById("C11_SPC_R_LR4").style.color = "green";
        }
        else {
            $("#C11_SPC_R_LR4").val("NG");
            document.getElementById("C11_SPC_R_LR4").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_R_Form_RUS();
}

$("#C11_SPC_R_TextBox10").change(function (e) {
    C11_SPC_R_TextBox10_TextChanged()
});
function C11_SPC_R_TextBox10_TextChanged() {
    try {
        let AA = Number($("#C11_SPC_R_TextBox10").val());
        let CC = Number($("#C11_SPC_R_Label17").val());

        if (CC <= AA) {
            $("#C11_SPC_R_LR9").val("OK");
            document.getElementById("C11_SPC_R_LR9").style.color = "green";
        }
        else {
            $("#C11_SPC_R_LR9").val("NG");
            document.getElementById("C11_SPC_R_LR9").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_R_Form_RUS();
}
$("#C11_SPC_R_Button1").click(function (e) {
    C11_SPC_R_Button1_Click();
});
function C11_SPC_R_Button1_Click() {
    let IsCheck = true;
    let OR_IDX = $("#ST_BCIDX").val();
    let TextBox1 = $("#C11_SPC_R_TextBox1").val();
    let TextBox2 = $("#C11_SPC_R_TextBox2").val();
    let TextBox3 = $("#C11_SPC_R_TextBox3").val();
    let TextBox4 = $("#C11_SPC_R_TextBox4").val();
    let TextBox10 = $("#C11_SPC_R_TextBox10").val();

    let Label6 = $("#C11_SPC_R_Label6").val();
    let Label10 = $("#C11_SPC_R_Label10").val();

    let MRUS = $("#C11_SPC_R_MRUS").val();
    let MC2 = $("#MCbox").val();
    let STRENGTH = $("#C11_SPC_R_Label17").val();

    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            //$("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox1);
            BaseParameter.ListSearchString.push(TextBox2);
            BaseParameter.ListSearchString.push(TextBox3);
            BaseParameter.ListSearchString.push(TextBox4);
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label6);
            BaseParameter.ListSearchString.push(Label10);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(OR_IDX);
            BaseParameter.ListSearchString.push(MC2);
            BaseParameter.ListSearchString.push(STRENGTH);

            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_SPC_R/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    C11_2_SPC_EXIT = true;
                    $("#C11_SPC_R").modal("close");
                    C11_SPC_R_Close();
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function C11_SPC_R_Close() {
    if (C11_SPC_R_Flag == 0) {
        C11_2_SPC_LOAD();
    }
    if (C11_SPC_R_Flag == 1) {
        C11_2_Buttonprint_ClickSub();
    }
}

let C11_ER_R_StartTime;
let C11_ER_R_RunTime;
let C11_ER_R_LAST_ID;
function C11_ER_R_PageLoad() {

    C11_ER_R_StartTime = new Date();
    $("#C11_ER_R_TextBox1").val("");
    $("#C11_ER_R_LBT2R").val("");
    $("#C11_ER_R_LBA2R").val("");
    $("#C11_ER_R_LBT2V").val("");
    $("#C11_ER_R_LBA2V").val("");
    $("#C11_ER_R_LBA2SEQ").val("");
    $("#C11_ER_R_LBA2IDX").val("");
    $("#C11_ER_R_LBR2").val("");
    $("#C11_ER_R_LBR4").val("");
    $("#C11_ER_R_LBR22").val("");

    document.getElementById("C11_ER_R_CHBT2").checked = true;
    document.getElementById("C11_ER_R_CHBA2").checked = true;

    document.getElementById("C11_ER_R_LBR2").style.backgroundColor = "white";
    document.getElementById("C11_ER_R_LBR4").style.backgroundColor = "white";
    document.getElementById("C11_ER_R_LBR22").style.backgroundColor = "white";

    $("#C11_ER_R_ComboBox1").empty();

    if ($("#C11_ER_R_LBT2S").val() == "") {
        C11_ER_R_Buttonclose_Click();
    }

    var ComboBox1 = document.getElementById("C11_ER_R_ComboBox1");

    var option = document.createElement("option");
    option.text = "TERM";
    option.value = "TERM";
    ComboBox1.add(option);

    option = document.createElement("option");
    option.text = "APPLICATOR";
    option.value = "APPLICATOR";
    ComboBox1.add(option);
    C11_ER_R_C11_ERROR_Load();
}
function C11_ER_R_C11_ERROR_Load() {
    let MCbox = $("#MCbox").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_ER_R/C11_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_ER_R_Search = data.Search;
            C11_ER_R_LAST_ID = BaseResult.C11_ER_R_Search[0].TSNON_OPER_IDX;
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_ER_R_Button1").click(function (e) {
    C11_ER_R_Button1_Click();
});
function C11_ER_R_Button1_Click() {
    let IsCheck = true;
    let AA = $("#C11_ER_R_TextBox1").val();
    AA = AA.toUpperCase();

    let BAR_TEXT = false;
    let ComboBox1 = $("#C11_ER_R_ComboBox1").val();
    if (ComboBox1 == "APPLICATOR") {
        BAR_TEXT = true;
    }
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    if (AA == BC_MAST) {
        BAR_TEXT = true;
    }
    C11_ER_R_Button1_ClickSub();
    //if (BAR_TEXT == false) {
    //    //$("#BackGround").css("display", "block");
    //    let BaseParameter = new Object();
    //    BaseParameter = {
    //        ListSearchString: [],
    //    }
    //    BaseParameter.USER_ID = GetCookieValue("UserID");
    //    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    //    BaseParameter.ListSearchString.push(AA);
    //    let formUpload = new FormData();
    //    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //    let url = "/C11_ER_R/Button1_ClickSub";

    //    fetch(url, {
    //        method: "POST",
    //        body: formUpload,
    //        headers: {
    //        }
    //    }).then((response) => {
    //        response.json().then((data) => {
    //            BaseResult.C11_ER_R_DataGridView1 = data.DataGridView1;
    //            if (BaseResult.C11_ER_R_DataGridView1.length <= 0) {
    //                IsCheck = false;
    //                alert("오류가 발생 하였습니다.(바코드 이력 없음). BARCODE Một lỗi đã xảy ra.");
    //            }
    //            if (IsCheck == true) {
    //                C11_ER_R_Button1_ClickSub();
    //            }
    //            $("#BackGround").css("display", "none");
    //        }).catch((err) => {
    //            $("#BackGround").css("display", "none");
    //        })
    //    });
    //}
    //else {
    //    C11_ER_R_Button1_ClickSub();
    //}
}
function C11_ER_R_Button1_ClickSub() {
    let IsCheck = true;
    let AA = $("#C11_ER_R_TextBox1").val();
    AA = AA.toUpperCase();
    let ComboBox1 = $("#C11_ER_R_ComboBox1").val();
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    switch (ComboBox1) {
        case "TERM":
            try {
                let LBT2S = $("#C11_ER_R_LBT2S").val();
                let T2 = "";
                if (AA == BC_MAST) {
                    T2 = LBT2S;
                }
                else {
                    T2 = AA.substr(0, AA.indexOf("$$"));
                }
                if (LBT2S == T2) {
                    $("#C11_ER_R_LBT2R").val("OK");
                    $("#C11_ER_R_LBR2").val("OK");
                    document.getElementById("C11_ER_R_LBR2").style.backgroundColor = "lime";
                    try {
                        document.getElementById("C11_ER_R_ComboBox1").selectedIndex = document.getElementById("C11_ER_R_ComboBox1").selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C11_ER_R_LBT2R").val("NG");
                    $("#C11_ER_R_LBR2").val("NG");
                    document.getElementById("C11_ER_R_LBR2").style.backgroundColor = "red";
                }
                $("#C11_ER_R_LBT2V").val(T2);
                $("#C11_ER_R_TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again. " + e);
            }
            break;
        case "APPLICATOR":           
            let A2 = AA.substr(0, AA.length - 1);            
            let LBA2S = $("#C11_ER_R_LBA2S").val();
            if (LBA2S == A2) {
                $("#C11_ER_R_LBA2R").val("OK");
                let LBA2SEQ = AA.substr(AA.length - 1, 1);
                $("#C11_ER_R_LBA2SEQ").val(LBA2SEQ);
                let ASC_SEQ = LBA2SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (ASC_SEQ >= 72) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    LBA2SEQ = $("#C11_ER_R_LBA2SEQ").val();
                    let Label1 = $("#C11_ER_R_Label1").val();
                    //$("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A2);
                    BaseParameter.ListSearchString.push(LBA2SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C11_ER_R/Button1_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            BaseResult.C11_ER_R_Search = data.Search;
                            BaseResult.C11_ER_R_Search1 = data.Search1;
                            BaseResult.C11_ER_R_DataGridView = data.DataGridView;
                            if (BaseResult.C11_ER_R_Search.length <= 0) {
                                if (BaseResult.C11_ER_R_Search1.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#C11_ER_R_LBA2R").val("Not data");
                                    $("#C11_ER_R_LBA2V").val(A2);
                                    $("#C11_ER_R_TextBox1").val("");
                                }
                                else {
                                    let WK_CNT = BaseResult.C11_ER_R_DataGridView[0].WK_CNT;
                                    $("#C11_ER_R_LBR22").val(WK_CNT);
                                    document.getElementById("C11_ER_R_LBR22").style.backgroundColor = "lime";
                                }
                            }
                            else {
                                let WK_CNT = BaseResult.C11_ER_R_Search[0].WK_CNT;
                                $("#C11_ER_R_LBR22").val(WK_CNT);
                                document.getElementById("C11_ER_R_LBR22").style.backgroundColor = "lime";
                                let TOOL_IDX = BaseResult.C11_ER_R_Search[0].TOOL_IDX;
                                $("#C11_ER_R_LBA2IDX").val(TOOL_IDX);
                            }
                            if (IsCheck == true) {

                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                $("#C11_ER_R_LBA2V").val(A2);
                $("#C11_ER_R_TextBox1").val("");
            }
            break;
    }
    $("#C11_ER_R_TextBox1").focus();
}
$("#C11_ER_R_Button2").click(function (e) {
    C11_ER_R_Button2_Click();
});
function C11_ER_R_Button2_Click() {
    let AA2 = document.getElementById("C11_ER_R_CHBT2").checked;
    let AA4 = document.getElementById("C11_ER_R_CHBA2").checked;
    let BB2 = false;
    let BB4 = false;
    if (AA2 == true) {
        if ($("#C11_ER_R_LBT2R").val() == "OK") {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA4 == true) {
        if ($("#C11_ER_R_LBA2R").val() == "OK") {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if ((AA2 == true) && (BB2 == true) && (AA4 == true) && (BB4 == true)) {
        let Label1 = $("#C11_ER_R_Label1").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: Label1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_ER_R/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                C11_2_C_EXIT = false;
                C11_ER_R_C11_ERROR_FormClosed(1);

            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert("Please Check Again.");
        C11_2_C_EXIT = true;
    }
}
$("#C11_ER_R_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_ER_R_TextBox1_KeyDown();
    }
});
function C11_ER_R_TextBox1_KeyDown() {
    C11_ER_R_Button1_Click();
}
$("#C11_ER_R_Buttonclose").click(function (e) {
    C11_ER_R_Buttonclose_Click();
});
$("#C11_ER_R_Buttonclose").click(function (e) {
    C11_ER_R_Buttonclose_Click();
});
function C11_ER_R_Buttonclose_Click() {
    C11_2_C_EXIT = true;
    C11_ER_R_C11_ERROR_FormClosed(0);
}
function C11_ER_R_C11_ERROR_FormClosed(Flag) {
    let MCbox = $("#MCbox").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C11_ER_R_LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_ER_R/C11_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            if (Flag == 1) {
                C11_2_C_EXIT = false;
                $("#C11_2").modal("open");
                C11_2_PageLoad();
            }
            $("#C11_ER_R").modal("close");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

let C11_3_Timer1;
let C11_3_Timer2;
let C11_3_C_EXIT;
let C11_3_TI_CONT;
let C11_3_APP_MAX;
let C11_3_SPC_EXIT;
let C11_3_TL_NM;
let C11_3_MAX_COUNT = 0;

function C11_3_PageLoad() {

    document.getElementById("C11_3_Buttonprint").disabled = false;
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_3_C11_D02").val(SettingsMC_NM);

    C11_3_ORDER_COUNT(0);
    C11_3_Timer2StartInterval();


}
function C11_3_ORDER_COUNT(Flag) {
    let ORDER = $("#C11_3_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_3/ORDER_COUNT";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_3_Search = data.Search;
            if (BaseResult.C11_3_Search.length > 0) {
                for (let i = 0; i < BaseResult.C11_3_Search.length; i++) {
                    if (BaseResult.C11_3_Search[i].LOC_LRJ == "L") {
                        $("#C11_3_COUNT_L").val(BaseResult.C11_3_Search[i].PERFORMN);
                    }
                    if (BaseResult.C11_3_Search[i].LOC_LRJ == "R") {
                        $("#C11_3_COUNT_R").val(BaseResult.C11_3_Search[i].PERFORMN);
                    }
                }
            }
            if (Flag == 0) {
                C11_3_ORDER_LOAD(0);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_3_ORDER_LOAD(Flag) {
    let IsCheck = true;
    let ORDER = $("#C11_3_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_3/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_3_Search1 = data.Search1;
            if (BaseResult.C11_3_Search1.length <= 0) {
                IsCheck = false;
                C11_3_Buttonclose_Click();
            }
            else {
                try {
                    $("#C11_3_VLA1").val(BaseResult.C11_3_Search1[0].APP);
                    $("#C11_3_VLA11").val(BaseResult.C11_3_Search1[0].SEQ);
                    $("#C11_3_TOOL_CONT").val(BaseResult.C11_3_Search1[0].COUNT);
                    $("#C11_3_C11_COUNT2").val(BaseResult.C11_3_Search1[0].PERFORMN);
                    C11_3_APP_MAX = BaseResult.C11_3_Search1[0].MAX;

                    if ($("#C11_3_TOOL_CONT").val() == "") {
                        $("#C11_3_TOOL_CONT").val(0);
                    }
                    if ($("#C11_3_C11_COUNT2").val() == "") {
                        $("#C11_3_C11_COUNT2").val(0);
                    }
                    let COUNT_L = Number($("#C11_3_COUNT_L").val());
                    let COUNT_R = Number($("#C11_3_COUNT_R").val());

                    if (COUNT_L > COUNT_R) {
                        C11_3_MAX_COUNT = COUNT_R - BaseResult.C11_3_Search1[0].PERFORMN;
                    }
                    else {
                        C11_3_MAX_COUNT = COUNT_L - BaseResult.C11_3_Search1[0].PERFORMN;
                    }
                    console.log("C11_3_ORDER_LOAD");
                    console.log(COUNT_L);
                    console.log(COUNT_R);
                    console.log(BaseResult.C11_3_Search1[0].PERFORMN);
                    console.log(C11_3_MAX_COUNT);
                    if (BaseResult.C11_3_Search1[0].PERFORMN >= BaseResult.C11_3_Search1[0].TOT_COUNT) {
                        IsCheck = false;
                        M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                        C11_3_Buttonclose_Click();
                    }

                }
                catch (e) {
                    IsCheck = false;
                    $("#C11_3_VLA1").val("");
                    $("#C11_3_VLA11").val("");
                    $("#C11_3_TOOL_CONT").val(0);
                    $("#C11_3_C11_COUNT2").val(0);
                    M.toast({ html: "Please Check Again." + e, classes: 'red', displayLength: 6000 });
                    C11_3_Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                if (Flag == 0) {
                    if ($("#C11_3_VLA1").val() == "") {
                        C11_3_BT_APP1_Click();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

$("#C11_3_Buttonprint").click(function (e) {
    C11_3_Buttonprint_Click();
});
function C11_3_Buttonprint_Click() {
    let IsCheck = true;
    document.getElementById("C11_3_Buttonprint").disabled = true;
    if ($("#C11_3_VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("C11_3_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_3_C11_COUNT3").val());
        console.log("C11_3_Buttonprint_Click");
        console.log(C11_COUNT3);
        if (C11_COUNT3 > C11_3_MAX_COUNT) {
            IsCheck = false;
            alert("실적처리 오류(WORK_COUNT). Không thể kết nối MES(WORK_COUNT).");
            document.getElementById("C11_3_Buttonprint").disabled = false;
        }
    }
    if (IsCheck == true) {
        let TOOLAPP = 0;
        let TOOLAPP_CNT = Number($("#C11_3_TOOL_CONT").val());
        try {
            TOOLAPP = C11_3_APP_MAX;
        }
        catch (e) {
            TOOLAPP = 1000000
        }
        if (TOOLAPP <= TOOLAPP_CNT) {
            IsCheck = false;
            alert("Application check(Check counter). Please Check Again.");
            document.getElementById("C11_3_Buttonprint").disabled = false;
        }
        if (IsCheck == true) {
            let C11_COUNT3 = Number($("#C11_3_C11_COUNT3").val());
            let ORDER = $("#C11_3_ORDER").val();
            let VLA1 = $("#C11_3_VLA1").val();
            let VLA11 = $("#C11_3_VLA11").val();
            let C11_D01 = $("#C11_3_C11_D01").val();
            let TOOL_CONT = $("#C11_3_TOOL_CONT").val();
            let C11_D02 = $("#C11_3_C11_D02").val();

            //$("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(C11_COUNT3);
            BaseParameter.ListSearchString.push(ORDER);
            BaseParameter.ListSearchString.push(VLA1);
            BaseParameter.ListSearchString.push(VLA11);
            BaseParameter.ListSearchString.push(C11_D01);
            BaseParameter.ListSearchString.push(TOOL_CONT);
            BaseParameter.ListSearchString.push(C11_D02);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_3/Buttonprint_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    document.getElementById("C11_3_Buttonprint").disabled = true;
                    C11_3_TI_CONT = 0;
                    C11_3_Timer1StartInterval();
                    M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                    try {
                        C11_3_ORDER_LOAD(1);
                    }
                    catch (e) {
                        alert("#2 Please Check Again. " + e);
                        document.getElementById("C11_3_Buttonprint").disabled = false;
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });

        }
    }
}
$("#C11_3_BT_APP1").click(function (e) {
    C11_3_BT_APP1_Click();
});
function C11_3_BT_APP1_Click() {
    IsC11_3 = true;
    C11_APPLICATION_ORDER_IDX = Number($("#C11_3_ORDER").val());
    $("#C11_APPLICATION_Label1").val($("#C11_3_VLA1").val());
    $("#C11_APPLICATION_Label2").val($("#C11_3_VLA11").val());
    $("#C11_APPLICATION_Label3").val("APPLICATION #J");
    $("#C11_APPLICATION").modal("open");
    C11_APPLICATION_PageLoad();
}
function C11_3_Timer1StartInterval() {
    C11_3_Timer1 = setInterval(function () {
        C11_3_Timer1_Tick();
    }, 100);
}
function C11_3_Timer1_Tick() {
    if (C11_3_TI_CONT >= 30) {
        document.getElementById("C11_3_Buttonprint").disabled = false;
        clearInterval(C11_3_Timer1);
    }
    else {
        C11_3_TI_CONT = C11_3_TI_CONT + 1;
        document.getElementById("C11_3_Buttonprint").innerHTML = "Complete" + "  (" + C11_3_TI_CONT + ")";
    }
}
function C11_3_Timer2StartInterval() {
    C11_3_Timer2 = setInterval(function () {
        C11_3_Timer2_Tick();
    }, 5000);
}
function C11_3_Timer2_Tick() {
    C11_3_ORDER_COUNT(1);
}
$("#C11_3_Buttonclose").click(function (e) {
    C11_3_Buttonclose_Click();
});
function C11_3_Buttonclose_Click() {
    C11_3_FormClosed();
}
function C11_3_FormClosed() {
    $("#C11_3").modal("close");
    $("#Barcodebox").val($("#ST_BC").val());
    Barcod_read();
}

let C11_4_Timer1;
let C11_4_Timer2;
let C11_4_C_EXIT;
let C11_4_TI_CONT;
let C11_4_APP_MAX;
let C11_4_SPC_EXIT;
let C11_4_TR_NM;
let C11_4_MAX_COUNT = 0;
function C11_4_PageLoad() {
    document.getElementById("C11_4_Buttonprint").disabled = false;
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_4_C11_D02").val(SettingsMC_NM);
    C11_4_ORDER_COUNT(0);
    C11_4_Timer2StartInterval();

}
function C11_4_ORDER_COUNT(Flag) {
    let ORDER = $("#C11_4_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_4/ORDER_COUNT";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_4_Search = data.Search;
            if (BaseResult.C11_4_Search.length > 0) {
                for (let i = 0; i < BaseResult.C11_4_Search.length; i++) {
                    if (BaseResult.C11_4_Search[i].LOC_LRJ == "L") {
                        $("#C11_4_COUNT_L").val(BaseResult.C11_4_Search[i].PERFORMN);
                    }
                    if (BaseResult.C11_4_Search[i].LOC_LRJ == "R") {
                        $("#C11_4_COUNT_R").val(BaseResult.C11_4_Search[i].PERFORMN);
                    }
                }
            }
            if (Flag == 0) {
                C11_4_ORDER_LOAD(0);
            }
            //C11_4_Timer2StartInterval();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_4_ORDER_LOAD(Flag) {
    let IsCheck = true;
    let ORDER = $("#C11_4_ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_4/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C11_4_Search1 = data.Search1;
            if (BaseResult.C11_4_Search1.length <= 0) {
                IsCheck = false;
                C11_4_Buttonclose_Click();
            }
            else {
                try {
                    $("#C11_4_VLA1").val(BaseResult.C11_4_Search1[0].APP);
                    $("#C11_4_VLA11").val(BaseResult.C11_4_Search1[0].SEQ);
                    $("#C11_4_TOOL_CONT").val(BaseResult.C11_4_Search1[0].COUNT);
                    $("#C11_4_C11_COUNT2").val(BaseResult.C11_4_Search1[0].PERFORMN);
                    C11_4_APP_MAX = BaseResult.C11_4_Search1[0].MAX;

                    if ($("#C11_4_TOOL_CONT").val() == "") {
                        $("#C11_4_TOOL_CONT").val(0);
                    }
                    if ($("#C11_4_C11_COUNT2").val() == "") {
                        $("#C11_4_C11_COUNT2").val(0);
                    }
                    let COUNT_L = Number($("#C11_4_COUNT_L").val());
                    let COUNT_R = Number($("#C11_4_COUNT_R").val());
                    if (COUNT_L > COUNT_R) {
                        C11_4_MAX_COUNT = COUNT_R - BaseResult.C11_4_Search1[0].PERFORMN;
                    }
                    else {
                        C11_4_MAX_COUNT = COUNT_L - BaseResult.C11_4_Search1[0].PERFORMN;
                    }
                    if (BaseResult.C11_4_Search1[0].PERFORMN == BaseResult.C11_4_Search1[0].TOT_COUNT) {
                        IsCheck = false;
                        M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                        C11_4_Buttonclose_Click();
                    }

                }
                catch (e) {
                    IsCheck = false;
                    $("#C11_4_VLA1").val("");
                    $("#C11_4_VLA11").val("");
                    $("#C11_4_TOOL_CONT").val(0);
                    $("#C11_4_C11_COUNT2").val(0);
                    alert("Please Check Again." + e);
                    C11_4_Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                if (Flag == 0) {
                    if ($("#C11_4_VLA1").val() == "") {
                        C11_4_BT_APP1_Click();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C11_4_Buttonprint").click(function (e) {
    C11_4_Buttonprint_Click();
});
function C11_4_Buttonprint_Click() {
    let IsCheck = true;
    document.getElementById("C11_4_Buttonprint").disabled = true;
    if ($("#C11_4_VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("C11_4_Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_4_C11_COUNT3").val());
        if (C11_COUNT3 > C11_4_MAX_COUNT) {
            IsCheck = false;
            alert("실적처리 오류(WORK_COUNT). Không thể kết nối MES(WORK_COUNT).");
            document.getElementById("C11_4_Buttonprint").disabled = false;
        }
    }
    if (IsCheck == true) {
        let TOOLAPP = 0;
        let TOOLAPP_CNT = Number($("#C11_4_TOOL_CONT").val());
        try {
            TOOLAPP = C11_4_APP_MAX;
        }
        catch (e) {
            TOOLAPP = 1000000
        }
        if (TOOLAPP <= TOOLAPP_CNT) {
            IsCheck = false;
            alert("Application check(Check counter). Please Check Again.");
            document.getElementById("C11_4_Buttonprint").disabled = false;
        }
        if (IsCheck == true) {
            let C11_COUNT3 = Number($("#C11_4_C11_COUNT3").val());
            let ORDER = $("#C11_4_ORDER").val();
            let VLA1 = $("#C11_4_VLA1").val();
            let VLA11 = $("#C11_4_VLA11").val();
            let C11_D01 = $("#C11_4_C11_D01").val();
            let TOOL_CONT = $("#C11_4_TOOL_CONT").val();
            let C11_D02 = $("#C11_4_C11_D02").val();

            //$("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(C11_COUNT3);
            BaseParameter.ListSearchString.push(ORDER);
            BaseParameter.ListSearchString.push(VLA1);
            BaseParameter.ListSearchString.push(VLA11);
            BaseParameter.ListSearchString.push(C11_D01);
            BaseParameter.ListSearchString.push(TOOL_CONT);
            BaseParameter.ListSearchString.push(C11_D02);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_4/Buttonprint_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    document.getElementById("C11_4_Buttonprint").disabled = true;
                    C11_4_TI_CONT = 0;
                    C11_4_Timer1StartInterval();
                    M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                    try {
                        C11_4_ORDER_LOAD(1);
                    }
                    catch (e) {
                        alert("#2 Please Check Again. " + e);
                        document.getElementById("C11_4_Buttonprint").disabled = false;
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });

        }
    }
}
$("#C11_4_BT_APP1").click(function (e) {
    C11_4_BT_APP1_Click();
});
function C11_4_BT_APP1_Click() {
    IsC11_4 = true;
    C11_APPLICATION_ORDER_IDX = Number($("#C11_4_ORDER").val());
    $("#C11_APPLICATION_Label1").val($("#C11_4_VLA1").val());
    $("#C11_APPLICATION_Label2").val($("#C11_4_VLA11").val());
    $("#C11_APPLICATION_Label3").val("APPLICATION #J2");
    $("#C11_APPLICATION").modal("open");
    C11_APPLICATION_PageLoad();
}
function C11_4_Timer1StartInterval() {
    C11_4_Timer1 = setInterval(function () {
        C11_4_Timer1_Tick();
    }, 100);
}
function C11_4_Timer1_Tick() {
    if (C11_4_TI_CONT >= 30) {
        document.getElementById("C11_4_Buttonprint").disabled = false;
        clearInterval(C11_4_Timer1);
    }
    else {
        C11_4_TI_CONT = C11_4_TI_CONT + 1;
        document.getElementById("C11_4_Buttonprint").innerHTML = "Complete" + "  (" + C11_4_TI_CONT + ")";
    }
}
function C11_4_Timer2StartInterval() {
    C11_4_Timer2 = setInterval(function () {
        C11_4_Timer2_Tick();
    }, 5000);
}
function C11_4_Timer2_Tick() {
    C11_4_ORDER_COUNT(1);
    //C11_4_Timer2StartInterval();
}
$("#C11_4_Buttonclose").click(function (e) {
    C11_4_Buttonclose_Click();
});
function C11_4_Buttonclose_Click() {
    C11_4_FormClosed();
}
function C11_4_FormClosed() {
    $("#C11_4").modal("close");
    $("#Barcodebox").val($("#ST_BC").val());
    Barcod_read();
}
