let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let Timer2;
let C09_SPCTimer;
let SPC_EXIT = false;
let StartTime = new Date();
let RunTime = new Date();
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {

    localStorage.setItem("C09_START_V3_Close", 0);

    $("#partbox").val(localStorage.getItem("C09_START_V3_partbox"));
    $("#ORDER_NO_TEXT").val(localStorage.getItem("C09_START_V3_ORDER_NO_TEXT"));
    $("#Label7").val(localStorage.getItem("C09_START_V3_Label7"));

    document.getElementById("SPC1").innerHTML = "First";
    document.getElementById("SPC2").innerHTML = "Middle";
    document.getElementById("SPC3").innerHTML = "End";

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    BaseResult.DGV_C09_ST_00 = new Object();
    BaseResult.DGV_C09_ST_00 = [];
    BaseResult.DGV_C09_ST_BOM = new Object();
    BaseResult.DGV_C09_ST_BOM = [];
    BaseResult.Search = new Object();
    BaseResult.Search = [];
    BaseResult.Search1 = new Object();
    BaseResult.Search1 = [];

    document.getElementById("R_BUTT_LH").disabled = false;
    document.getElementById("R_BUTT_LH").checked = true;
    document.getElementById("R_BUTT_RH").disabled = false;
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MCbox").val(SettingsMC_NM);
    document.getElementById("MRUS").style.backgroundColor = "gray";

    Timer2_Tick();

    document.getElementById("BI_RadioButton1").checked = true;

    ORDER_LOAD();
    OPER_TIME();
    SPC_LOAD();


    $("#Barcodebox").val("");
    $("#Barcodebox").focus();

});
function Timer2_Tick() {
    Timer2tartInterval();
}
function Timer2tartInterval(Flag) {
    Timer2 = setInterval(function () {
        StartTime = $("#Label70").val();
        RunTime = new Date();
        let Text_time = $("#BI_STOPTIME").val();
        let hh_time = Number(Text_time.substring(0, 2)) * 60 * 60;
        let mm_time = Number(Text_time.substring(3, 2)) * 60;
        let ss_time = Number(Text_time.substring(6, 2));
        let tot_time = hh_time + mm_time + ss_time;
        let myTimeSpan = RunTime - StartTime - tot_time;
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
        let M_STIME = new Date(DateToString(new Date()) + " 06:00:00");
        let M_SPAN = RunTime - M_STIME;
        let DTIME11 = M_SPAN;
        let DTIME12 = Number(DTIME11.substring(0, 2)) * 60 * 60;
        let DTIME13 = Number(DTIME11.substring(3, 2)) * 60;
        let DTIME14 = Number(DTIME11.substring(6, 2));
        let DTIME15 = DTIME12 + DTIME13 + DTIME14;

        let DTIME21 = $("#BI_RTIME").val();
        let DTIME22 = Number(DTIME21.substring(0, 2)) * 60 * 60;
        let DTIME23 = Number(DTIME21.substring(3, 2)) * 60;
        let DTIME24 = Number(DTIME21.substring(6, 2));
        let DTIME25 = DTIME22 + DTIME23 + DTIME24;

        $("#BI_RVA").val(DTIME25 / DTIME15);

        if ($("#BI_RCK").val() == "+") {
            let UPH_T1 = $("#BI_RTIME").val();
            let UPH_H1 = Number(UPH_T1.substring(0, 2));
            let UPH_M1 = Number(UPH_T1.substring(3, 2));
            let BI_UPH = Number($("#BI_WQTY").val()) / (UPH_H1 + (UPH_M1 / 60));
            $("#BI_UPH").val(BI_UPH);
        }

    }, 1000);
}
function ORDER_LOAD() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#ORDER_NO_TEXT").val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_START_V3/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DGV_C09_ST_00 = BaseResultSub.DGV_C09_ST_00;
            BaseResult.DGV_C09_ST_BOM = BaseResultSub.DGV_C09_ST_BOM;
            StartTime = $("#Label70").val();
            BaseResult.DataGridView1 = [];
            BaseResult.DataGridView2 = [];
            let BUNDLE_SIZE = BaseResult.DGV_C09_ST_00[0].BUNDLE_SIZE;
            let PO_QTY = BaseResult.DGV_C09_ST_00[0].PO_QTY;
            let PERFORMN = BaseResult.DGV_C09_ST_00[0].PERFORMN;
            $("#Label7").val(BUNDLE_SIZE);
            $("#Label48").val(PO_QTY);
            $("#Label59").val(PERFORMN);

            let AAAA = PO_QTY - PERFORMN;
            let BBBB = BUNDLE_SIZE;
            if (AAAA < BBBB) {
                $("#Label7").val(AAAA);
            }

            let L_COUNT = 1;
            let R_COUNT = 1;
            let L_MIN_QYT = 1000001;
            let R_MIN_QYT = 1000001;

            for (let i = 0; i < BaseResult.DGV_C09_ST_BOM.length; i++) {
                let BOM_LEAD_NO = BaseResult.DGV_C09_ST_BOM[i].LEAD_NO;
                let BOM_BSIZE = BaseResult.DGV_C09_ST_BOM[i].B_SIZE;
                let LEAD_ERROR_CHK = BaseResult.DGV_C09_ST_BOM[i].DC_YN;
                let LEAD_QTY = BaseResult.DGV_C09_ST_BOM[i].S_SUM;
                let ERROR_CHK = BaseResult.DGV_C09_ST_BOM[i].E_CHK;

                let S_LR = BaseResult.DGV_C09_ST_BOM[i].S_LR;
                if (S_LR == "L") {
                    let Label48 = Number($("#Label48").val());
                    let Label59 = Number($("#Label59").val());
                    if (BOM_BSIZE <= Label48 - Label59) {
                        BOM_BSIZE = BOM_BSIZE;
                    }
                    else {
                        BOM_BSIZE = Label48 - Label59;
                    }
                    if (LEAD_QTY >= BOM_BSIZE) {
                        ERROR_CHK = "Y"
                    }
                    let DataGridView1Sub = new Object();
                    DataGridView1Sub = {
                        L_COUNT: L_COUNT,
                        BOM_LEAD_NO: BOM_LEAD_NO,
                        BOM_BSIZE: BOM_BSIZE,
                        ERROR_CHK: ERROR_CHK,
                        LEAD_QTY: LEAD_QTY,
                    }
                    BaseResult.DataGridView1.push(DataGridView1Sub);
                    DataGridView1Render();
                    L_COUNT = L_COUNT + 1;
                    if (L_MIN_QYT > LEAD_QTY) {
                        L_MIN_QYT = LEAD_QTY
                    }
                }
                if (S_LR == "R") {
                    let Label48 = Number($("#Label48").val());
                    let Label59 = Number($("#Label59").val());
                    if (BOM_BSIZE <= Label48 - Label59) {
                        BOM_BSIZE = BOM_BSIZE;
                    }
                    else {
                        BOM_BSIZE = Label48 - Label59;
                    }
                    if (LEAD_QTY >= BOM_BSIZE) {
                        ERROR_CHK = "Y"
                    }
                    let DataGridView2Sub = new Object();
                    DataGridView2Sub = {
                        L_COUNT: R_COUNT,
                        BOM_LEAD_NO: BOM_LEAD_NO,
                        BOM_BSIZE: BOM_BSIZE,
                        ERROR_CHK: ERROR_CHK,
                        LEAD_QTY: LEAD_QTY,
                    }
                    BaseResult.DataGridView2.push(DataGridView2Sub);
                    DataGridView2Render();
                    R_COUNT = R_COUNT + 1;
                    if (R_MIN_QYT > LEAD_QTY) {
                        R_MIN_QYT = LEAD_QTY
                    }
                }
            }

            if (L_MIN_QYT > 1000000) {
                $("#LH_QTY").val("---");
            }
            else {
                $("#LH_QTY").val(L_MIN_QYT);
            }

            if (R_MIN_QYT > 1000000) {
                $("#RH_QTY").val("---");
            }
            else {
                $("#RH_QTY").val(R_MIN_QYT);
            }
            let RH_QTY = Number($("#RH_QTY").val());
            let Label7 = Number($("#Label7").val());
            if (RH_QTY >= Label7) {
                document.getElementById("MRUS").style.backgroundColor = "lime";
                $("#RH_QTY").val("OK");
            }
            else {
                document.getElementById("MRUS").style.backgroundColor = "gray";
                $("#RH_QTY").val("STAY");
            }

            if (BaseResult.DataGridView1.length == 0) {
                document.getElementById("R_BUTT_LH").disabled = true;
                document.getElementById("R_BUTT_RH").checked = true;
            }
            if (BaseResult.DataGridView2.length == 0) {
                document.getElementById("R_BUTT_RH").disabled = true;
                document.getElementById("R_BUTT_LH").checked = true;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function OPER_TIME() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#MCbox").val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_START_V3/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResultSub.Search;

            let TOT_SUM = BaseResult.Search[0].SUM_TIME;
            let H_TIME = TOT_SUM / 60 / 60;
            TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
            let M_TIME = TOT_SUM / 60;
            let S_TIME = TOT_SUM - (M_TIME * 60);
            let BI_STOPTIME = String(H_TIME).padStart(2, '0') + ":" + String(M_TIME).padStart(2, '0') + ":" + String(S_TIME).padStart(2, '0');
            $("#BI_STOPTIME").val(BI_STOPTIME);

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function SPC_LOAD() {
    document.getElementById("SPC1").disabled = false;
    document.getElementById("SPC2").disabled = false;
    document.getElementById("SPC3").disabled = false;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#ORDER_NO_TEXT").val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_START_V3/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search1 = BaseResultSub.Search1;

            let COUNT_SPC = Number($("#Label48").val());
            try {
                COUNT_SPC = BaseResult.DGV_C09_ST_00[0].PO_QTY;
            }
            catch (err) {
                COUNT_SPC = Number($("#Label48").val());
            }
            let partbox = $("#partbox").val();
            partbox = partbox.substring(0, 2);

            if (partbox == "SP") {
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
                catch (err) {
                    document.getElementById("SPC1").innerHTML = "ERROR";
                    document.getElementById("SPC2").innerHTML = "ERROR";
                    document.getElementById("SPC3").innerHTML = "ERROR";
                }

                for (let i = 0; i < BaseResult.Search1.length; i++) {
                    let COLSIP = BaseResult.Search1[i].COLSIP;
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
            }
            else {
                document.getElementById("SPC1").innerHTML = "----";
                document.getElementById("SPC1").disabled = true;
                document.getElementById("SPC2").innerHTML = "----";
                document.getElementById("SPC2").disabled = true;
                document.getElementById("SPC3").innerHTML = "----";
                document.getElementById("SPC3").disabled = true;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
$("#R_BUTT_LH").click(function (e) {
    R_BUTT_LH_Click();
});
function R_BUTT_LH_Click() {
    $("#Barcodebox").focus();
}
$("#R_BUTT_RH").click(function (e) {
    R_BUTT_RH_Click();
});
function R_BUTT_RH_Click() {
    $("#Barcodebox").focus();
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C09_START_V3_Close", 1);
    window.close();
}
$("#Barcodebox").keydown(function (e) {
    if (e.keyCode == 13) {
        Barcodebox_KeyDown_1();
    }
});
function Barcodebox_KeyDown_1() {
    let IsCheck = true;
    let TEXT_BARCODE = $("#Barcodebox").val();
    TEXT_BARCODE = TEXT_BARCODE.toUpperCase();
    if (TEXT_BARCODE.length < 35) {
        IsCheck = false;
        alert("BARCODE SCAN 을 잘못했습니다. Scan sai Barcode.");
        $("#Barcodebox").val("");
        $("#Barcodebox").focus();
    }
    if (IsCheck == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: TEXT_BARCODE,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C09_START_V3/Barcodebox_KeyDown_1";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DGV_C09_13 = BaseResultSub.DGV_C09_13;
                let D3_JJ = BaseResult.DGV_C09_13.length;
                let BAR_NM_CHK = false;
                if (D3_JJ > 0) {
                    if (BaseResult.DGV_C09_13[0].DSCN_YN == "N") {
                        BAR_NM_CHK = true;
                    }
                    else {
                        IsCheck = false;
                        alert("이미 처리된 바코드. Đã xử lý Barcode trước đó");
                        $("#Barcodebox").val("");
                        $("#Barcodebox").focus();
                    }
                }
                let F_LEAD_T = TEXT_BARCODE.substring(1, TEXT_BARCODE.indexOf("$$") - 1);
                let AAT = TEXT_BARCODE.substring(TEXT_BARCODE.indexOf("$$") + 2, 12);
                let BC_QTY = AAT.substring(1, AAT.indexOf("$$") - 1);
                if (IsCheck == true) {
                    let R_BUTT_LH = document.getElementById("R_BUTT_LH").checked;
                    if (R_BUTT_LH == true) {
                        let LH_LEAD_CHK = false;
                        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                            if (F_LEAD_T == BaseResult.DataGridView1[i].BOM_LEAD_NO) {
                                if (BaseResult.DataGridView1[i].ERROR_CHK == "Y") {
                                    IsCheck = false;
                                    alert("ERROR PROOF 완료 상태. CHỨNG NHẬN LỖI đã hoàn thành");
                                    $("#Barcodebox").val("");
                                    $("#Barcodebox").focus();
                                }
                                else {
                                    LH_LEAD_CHK = true;
                                    i = BaseResult.DataGridView1.length;
                                }
                            }
                        }
                        if (IsCheck == true) {
                            if (LH_LEAD_CHK == true) {
                                $("#BackGround").css("display", "block");
                                let BaseParameter = new Object();
                                BaseParameter = {
                                    SearchString: TEXT_BARCODE,
                                }
                                BaseParameter.USER_ID = GetCookieValue("UserID");
                                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                                let formUpload = new FormData();
                                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                                let url = "/C09_START_V3/Barcodebox_KeyDown_1Sub01";

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
                            else {
                                IsCheck = false;
                                alert("일치하는 바코드가 없습니다. Không có mã vạch phù hợp");
                                $("#Barcodebox").val("");
                                $("#Barcodebox").focus();
                            }
                        }
                    }
                }
                if (IsCheck == true) {
                    let R_BUTT_RH = document.getElementById("R_BUTT_RH").checked;
                    if (R_BUTT_RH == true) {
                        let LH_LEAD_CHK = false;
                        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                            if (F_LEAD_T == BaseResult.DataGridView2[i].BOM_LEAD_NO) {
                                if (BaseResult.DataGridView1[i].ERROR_CHK == "Y") {
                                    IsCheck = false;
                                    alert("ERROR PROOF 완료 상태. CHỨNG NHẬN LỖI đã hoàn thành");
                                    $("#Barcodebox").val("");
                                    $("#Barcodebox").focus();
                                }
                                else {
                                    LH_LEAD_CHK = true;
                                    i = BaseResult.DataGridView1.length;
                                }
                            }
                        }
                        if (IsCheck == true) {
                            if (LH_LEAD_CHK == true) {
                                $("#BackGround").css("display", "block");
                                let BaseParameter = new Object();
                                BaseParameter = {
                                    SearchString: TEXT_BARCODE,
                                }
                                BaseParameter.USER_ID = GetCookieValue("UserID");
                                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                                let formUpload = new FormData();
                                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                                let url = "/C09_START_V3/Barcodebox_KeyDown_1Sub02";

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
                            else {
                                IsCheck = false;
                                alert("일치하는 바코드가 없습니다. Không có mã vạch phù hợp");
                                $("#Barcodebox").val("");
                                $("#Barcodebox").focus();
                            }
                        }
                    }
                }

                if (IsCheck == true) {
                    ORDER_LOAD();
                    let R_BUTT_LH = document.getElementById("R_BUTT_LH").checked;
                    if (R_BUTT_LH == true) {
                        let D1_COUNT = 0;
                        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                            if (BaseResult.DataGridView1[i].ERROR_CHK == "Y") {
                                D1_COUNT = D1_COUNT + 1;
                            }
                        }
                        if (BaseResult.DataGridView1.length == D1_COUNT) {
                            if (document.getElementById("R_BUTT_RH").disabled = false) {
                                document.getElementById("R_BUTT_RH").checked = true;
                            }
                        }
                    }
                    else {
                        let D2_COUNT = 0;
                        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                            if (BaseResult.DataGridView2[i].ERROR_CHK == "Y") {
                                D2_COUNT = D2_COUNT + 1;
                            }
                        }
                        if (BaseResult.DataGridView2.length == D1_COUNT) {
                            if (document.getElementById("R_BUTT_LH").disabled = false) {
                                document.getElementById("R_BUTT_LH").checked = true;
                            }
                        }
                    }
                    $("#Barcodebox").val("");
                    $("#Barcodebox").focus();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#SPC1").click(function (e) {
    SPC1_Click();
});
function SPC1_Click() {
    localStorage.setItem("C09_SPC_Close", 0);
    localStorage.setItem("C09_SPC_INSP_TEXT", "First");
    let url = "/C09_SPC";
    OpenWindowByURL(url, 800, 460);
    C09_SPCStartInterval(0);
}
$("#SPC2").click(function (e) {
    SPC2_Click();
});
function SPC2_Click() {
    localStorage.setItem("C09_SPC_Close", 0);
    localStorage.setItem("C09_SPC_INSP_TEXT", "Middle");
    let url = "/C09_SPC";
    OpenWindowByURL(url, 800, 460);
    C09_SPCStartInterval(0);
}
$("#SPC3").click(function (e) {
    SPC3_Click();
});
function SPC3_Click() {
    localStorage.setItem("C09_SPC_Close", 0);
    localStorage.setItem("C09_SPC_INSP_TEXT", "End");
    let url = "/C09_SPC";
    OpenWindowByURL(url, 800, 460);
    C09_SPCStartInterval(0);
}
$("#BI_10").click(function (e) {
    Button5_Click();
});
function Button5_Click() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    if (document.getElementById("BI_RadioButton1").checked == true) {
        Non_idx = 1;
    }
    if (document.getElementById("BI_RadioButton2").checked == true) {
        Non_idx = 2;
    }
    if (document.getElementById("BI_RadioButton3").checked == true) {
        Non_idx = 3;
    }
    if (document.getElementById("BI_RadioButton4").checked == true) {
        Non_idx = 4;
    }
    if (document.getElementById("BI_RadioButton5").checked == true) {
        Non_idx = 5;
    }
    if (document.getElementById("BI_RadioButton6").checked == true) {
        Non_idx = 6;
    }
    if (document.getElementById("BI_RadioButton7").checked == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S"
            Non_text_NM = document.getElementById("BI_RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I"
            Non_text_NM = document.getElementById("BI_RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q"
            Non_text_NM = document.getElementById("BI_RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M"
            Non_text_NM = document.getElementById("BI_RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T"
            Non_text_NM = document.getElementById("BI_RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L"
            Non_text_NM = document.getElementById("BI_RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E"
            Non_text_NM = document.getElementById("BI_RadioButton7Text").innerHTML;
            break;
    }
    localStorage.setItem("C09_STOP_Close", 0);
    localStorage.setItem("C09_STOP_Label5", Non_text);
    localStorage.setItem("C09_STOP_Label2", Non_text_NM);
    let url = "/C09_STOP";
    OpenWindowByURL(url, 800, 460);
}
$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    let IsCheck = true;
    let MRUS = $("#MRUS").val();
    if (MRUS == "STAY") {
        IsCheck = false;
        alert("LEAD BARCODE 수량이 부족합니다. LEAD BARCODE Một lỗi đã xảy ra.");
    }
    if (IsCheck == true) {
        SPC_EXIT = true;
        let SPC1 = document.getElementById("SPC1").innerHTML;
        if (SPC1 == "First") {
            localStorage.setItem("C09_SPC_Close", 0);
            localStorage.setItem("C09_SPC_INSP_TEXT", "First");
            let url = "/C09_SPC";
            OpenWindowByURL(url, 800, 460);
            C09_SPCStartInterval(1);
        }
        if (BaseResult.DGV_C09_ST_00[0].PO_QTY > 500) {
            if (BaseResult.DGV_C09_ST_00[0].PO_QTY / 2 <= BaseResult.DGV_C09_ST_00[0].PERFORMN) {
                let SPC2 = document.getElementById("SPC2").innerHTML;
                if (SPC2 == "Middle") {
                    localStorage.setItem("C09_SPC_Close", 0);
                    localStorage.setItem("C09_SPC_INSP_TEXT", "Middle");
                    let url = "/C09_SPC";
                    OpenWindowByURL(url, 800, 460);
                    C09_SPCStartInterval(1);
                }
            }
        }
        let A_CSPC = BaseResult.DGV_C09_ST_00[0].PO_QTY - BaseResult.DGV_C09_ST_00[0].PERFORMN;
        if (BaseResult.DGV_C09_ST_00[0].BUNDLE_SIZE >= A_CSPC) {
            let SPC3 = document.getElementById("SPC3").innerHTML;
            if (SPC3 == "End") {
                localStorage.setItem("C09_SPC_Close", 0);
                localStorage.setItem("C09_SPC_INSP_TEXT", "End");
                let url = "/C09_SPC";
                OpenWindowByURL(url, 800, 460);
                C09_SPCStartInterval(1);
            }
        }
    }
}
function Buttonprint_ClickSub() {
    if (SPC_EXIT == false) {
        IsCheck = false;
        alert("검사 누락. Kiểm tra mất tích.");
        document.getElementById("Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let Label7 = $("#Label7").val();
        let Label59 = $("#Label59").val();
        let ORDER_NO_TEXT = $("#ORDER_NO_TEXT").val();
        let partbox = $("#partbox").val();
        let MCbox = $("#MCbox").val();
        let Label48 = $("#Label48").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: []
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DGV_C09_ST_00 = BaseResult.DGV_C09_ST_00;
        BaseParameter.ListSearchString.push(Label7);
        BaseParameter.ListSearchString.push(Label59);
        BaseParameter.ListSearchString.push(ORDER_NO_TEXT);
        BaseParameter.ListSearchString.push(partbox);
        BaseParameter.ListSearchString.push(MCbox);
        BaseParameter.ListSearchString.push(Label48);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C09_START_V3/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                if (BaseResultSub) {
                    if (BaseResultSub.Code) {
                        let url = BaseResultSub.Code;
                        OpenWindowByURL(url, 200, 200);
                    }
                }
                let WORK_QTY = Number(Label7) + Number(Label59);
                if (WORK_QTY >= Number(Label48)) {
                    IsCheck = false;
                    alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                }
                else {
                    alert("정상처리 되었습니다. Đã được lưu.");
                    ORDER_LOAD();
                    SPC_LOAD();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C09_SPCStartInterval(Flag) {
    C09_SPCTimer = setInterval(function () {
        let IsClose = localStorage.getItem("C09_SPC_Close");
        if (IsClose == 1) {
            clearInterval(C09_SPCTimer);
            IsClose = 0;
            localStorage.setItem("C09_SPC_Close", IsClose);

            if (Flag == 0) {
                SPC_LOAD();
            }
            if (Flag == 1) {
                SPC_EXIT = localStorage.getItem("C09_START_V3_SPC_EXIT");
                Buttonprint_ClickSub();
            }
        }
    }, 100);
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    if (BaseResult.DataGridView1[i].ERROR_CHK == "Y") {
                        HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' style='background-color: lime;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' style='background-color: pink;'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].L_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOM_LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOM_BSIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ERROR_CHK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    if (BaseResult.DataGridView2[i].ERROR_CHK == "Y") {
                        HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")' style='background-color: lime;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")' style='background-color: pink;'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].L_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BOM_LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BOM_BSIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ERROR_CHK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].LEAD_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
let DataGridView2Table = document.getElementById("DataGridView2Table");
DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
        DataGridView2Render();
    }
});