let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 6;
let DateNow;
let DataGridView1RowIndex = 0;
let C05_STARTTimer;

let C05_STOP_Timer1;
let C05_STOP_StartTime
let C05_STOP_STOPW_ORING_IDX = 0;

window.addEventListener('beforeunload', function (event) {    
    C05_STOP_Button1_ClickSub();
});

window.addEventListener('load', function () {
    const navigationEntry = performance.getEntriesByType('navigation')[0];
    if (navigationEntry && navigationEntry.type === 'reload') {
        C05_STOP_Button1_ClickSub();
    }
});
function C05_STOP_Button1_ClickSub() {
    try {
        let C05_STOP_Label6 = Number($("#C05_STOP_Label6").val());
        if (C05_STOP_Label6 > 0) {
            C05_STOP_Button1_Click();
        }
    }
    catch (e) {

    }
}

$(document).ready(function () {      

    $(".modal").modal({
        dismissible: false
    });

    document.getElementById("Button3").style.display = "none";
    var now = new Date();
    DateNow = DateToString(now);

    $("#DateTimePicker1").val(DateNow);
    Lan_Change();
    MC_LIST(0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MCBOX").val(SettingsMC_NM);
    //đóng đơn sau 20 ngày
   // DB_LISECHK();
    TS_USER(0);
    $("#BC_TEXT").val("");
    $("#BC_TEXT").focus();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    $('#C05_STOP_MaintenanceModal').modal({
        dismissible: false
    });
    $("#C05_STOP_MaintenanceSaveBtn").click(function () {
        C05_STOP_SaveMaintenanceHistory();
    });
});

function Lan_Change() {
    Form_Label2();
}
function Form_Label2() {
    let CODE_S1 = "S";
    let CODE_I1 = "I";
    let CODE_Q1 = "Q";
    let CODE_M1 = "M";
    let CODE_T1 = "T";
    let CODE_L1 = "L";
    let CODE_E1 = "E";

    let CODE_S0 = $("#CODE_S").val() + " (" + CODE_S1 + ")";
    let CODE_I0 = $("#CODE_I").val() + " (" + CODE_I1 + ")";
    let CODE_Q0 = $("#CODE_Q").val() + " (" + CODE_Q1 + ")";
    let CODE_M0 = $("#CODE_M").val() + " (" + CODE_M1 + ")";
    let CODE_T0 = $("#CODE_T").val() + " (" + CODE_T1 + ")";
    let CODE_L0 = $("#CODE_L").val() + " (" + CODE_L1 + ")";
    let CODE_E0 = $("#CODE_E").val() + " (" + CODE_E1 + ")";

    $("#ComboBox3").empty();

    var ComboBox3 = document.getElementById("ComboBox3");

    var option = document.createElement("option");
    option.text = CODE_S0;
    option.value = CODE_S0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_I0;
    option.value = CODE_I0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_Q0;
    option.value = CODE_Q0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_M0;
    option.value = CODE_M0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_T0;
    option.value = CODE_T0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_L0;
    option.value = CODE_L0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_E0;
    option.value = CODE_E0;
    ComboBox3.add(option);
}

function MC_LIST(Flag) {
    let FAC = $("#ComboBox4").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FAC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/MC_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.ComboBox1 = BaseResultSub.ComboBox1;
            let CHK_MC = $("#ComboBox1").val();
            $("#ComboBox1").empty();

            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox1[i].MC;
                option.value = BaseResult.ComboBox1[i].MC;

                var ComboBox1 = document.getElementById("ComboBox1");
                ComboBox1.add(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

// thủ tục đóng đơn quá hạn
function DB_LISECHK() {
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/DB_LISECHK";

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
function TS_USER(Flag) {
    let MCBOX = $("#MCBOX").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCBOX,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/TS_USER";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.Search = data.Search;
            BaseResult.Search1 = data.Search1;

            ToolStripLOGIDX = BaseResult.Search[0].TUSER_IDX;
            localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
            $("#C05_START_Label67").val(BaseResult.Search1[0].Name);
            $("#C05_START_Label70").val(BaseResult.Search1[0].Description);


            if (Flag == 1) {
                let BB0 = BaseResult.DataGridView3[0].WIRE;
                if (BB0 == "") {

                }
                else {
                    let PO1 = BB0.indexOf("  ") - BB0.indexOf(" ") - 1;
                    let TT2 = BB0.substr(BB0.indexOf(" ") + 1, PO1);
                    $("#C05_START_ST_DWIRE1").val(TT2);
                }
                if (BaseResult.DataGridView4.length > 0) {
                    let ERROR_CHK = BaseResult.DataGridView4[0].ERROR_CHK;
                    if (ERROR_CHK == "Y") {
                        $("#C05_START").modal("open");
                        C05_START_PageLoad();
                    }
                    else {
                        $("#C05_START").modal("open");
                        C05_START_PageLoad();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#ComboBox4").change(function () {
    ComboBox4_SelectedIndexChanged();
});
function ComboBox4_SelectedIndexChanged() {
    MC_LIST(0);
}
$("#BC_TEXT").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox2_KeyDown();
    }
});
function TextBox2_KeyDown() {
    Button1_Click();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {

    let IsCheck = true;
    let BC_TEXT = $("#BC_TEXT").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.ListSearchString.push(BC_TEXT);
 
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/Button1_Click";
    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView2 = data.DataGridView2;
            BaseResult.DataGridView3 = data.DataGridView3;
            BaseResult.DataGridView4 = data.DataGridView4;
            BaseResult.DataGridView5 = data.DataGridView5;
            if (BaseResult.DataGridView2.length > 0) {
                if (BaseResult.DataGridView3.length <= 0) {
                    IsCheck = false;
                    // alert("It is not a list of orders for LP. Please Check Barcode Again.");
                    ShowMessage('It is not a list of orders for LP. Please Check Barcode Again', 'error');
                    $("#BC_TEXT").val("");
                    $("#BC_TEXT").focus();
                }
                if (IsCheck == true) {
                    let CHKA = BaseResult.DataGridView3[0].CONDITION;
                    if (CHKA == "Complete") {
                        IsCheck = false;
                        MC_LIST(0);
                        $("#BC_TEXT").val("");
                        $("#BC_TEXT").focus();
                      //  alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                        ShowMessage('작업을 종료 하였습니다. Đã dừng làm việc.', 'error');
                    }
                    if (IsCheck == true) {
                        let AAA = BaseResult.DataGridView3[0].LEAD_NO;
                        let BBB = BaseResult.DataGridView3[0].BUNDLE_SIZE;
                        let CCC = BaseResult.DataGridView3[0].DT;
                        let DDD = BaseResult.DataGridView3[0].TOT_QTY;
                        let FFF = BaseResult.DataGridView3[0].ORDER_IDX;
                        let GGG = BaseResult.DataGridView3[0].SP_ST;
                        let HHH = BaseResult.DataGridView3[0].PROJECT;
                        let LLL = BaseResult.DataGridView3[0].MC;
                        let MMM = BaseResult.DataGridView3[0].ADJ_AF_QTY;

                        SettingsMC_NM = localStorage.getItem("SettingsMC_NM");

                        $("#C05_START_Label3").val(FFF);
                        $("#C05_START_Label4").val(AAA);
                        $("#C05_START_Label8").val(FFF);
                        $("#C05_START_Label42").val(GGG);
                        $("#C05_START_Label43").val(HHH);
                        $("#C05_START_L_MCNM").val(SettingsMC_NM);
                        $("#C05_START_Label48").val(DDD);
                        $("#C05_START_Label50").val(BBB);
                        $("#C05_START_Label77").val(MMM);
                        $("#C05_ERROR_Label1").val(FFF);

                        if (CHKA == "Complete") {
                        }
                        else {
                            TS_USER(1);
                        }
                    }
                }
            }
            else {
                IsCheck = false;
                //alert("It is not a list of orders for LP. Please Check Barcode Again.");
                ShowMessage('It is not a list of orders for LP. Please Check Barcode Again', 'error');
                $("#BC_TEXT").val("");
                $("#BC_TEXT").focus();
            }
            if (IsCheck == true) {
                MC_LIST(0);
                $("#BC_TEXT").val("");
                $("#BC_TEXT").focus();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("An error occurred: " + err.message);
        })
    });
}

$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    TS_USER(0);
    let CODE_MC = "";
    let ComboBox3 = document.getElementById("ComboBox3").selectedIndex;
    switch (ComboBox3) {
        case 0:
            CODE_MC = "S";
            break;
        case 1:
            CODE_MC = "I";
            break;
        case 2:
            CODE_MC = "Q";
            break;
        case 3:
            CODE_MC = "M";
            break;
        case 4:
            CODE_MC = "T";
            break;
        case 5:
            CODE_MC = "L";
            break;
        case 6:
            CODE_MC = "E";
            break;
    }
    localStorage.setItem("C05_STOP_Close", 0);
    localStorage.setItem("C05_STOP_Label5", CODE_MC);
    localStorage.setItem("C05_STOP_STOP_MC", $("#MCBOX").val());
    localStorage.setItem("C05_STOP_Label2", $("#ComboBox3").val());

    //let url = "/C05_STOP";
    //OpenWindowByURL(url, 800, 460);

    $("#C05_STOP_Label5").val(CODE_MC);
    $("#C05_STOP_STOP_MC").val($("#MCBOX").val());
    $("#C05_STOP_Label2").val($("#ComboBox3").val());
    $("#C05_STOP_Label4").val("00 : 00 : 00");


    $("#C05_STOP").modal("open");
    C05_STOP_PageLoad();
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
}
$("#rbchk1").click(function () {
    Rbchk1_Click();
});
function Rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = true;
        }
    }
}
$("#rbchk2").click(function () {
    Rbchk2_Click();
});
function Rbchk2_Click() {
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
        }
    }
}
$("#Buttonfind").click(function () {
    Buttonfind_Click();
});
$("#Buttonadd").click(function () {
    Buttonadd_Click();
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#Buttondelete").click(function () {
    Buttondelete_Click();
});
$("#Buttoncancel").click(function () {
    Buttoncancel_Click();
});
$("#Buttoninport").click(function () {
    Buttoninport_Click();
});
$("#Buttonexport").click(function () {
    Buttonexport_Click();
});
$("#Buttonprint").click(function () {
    Buttonprint_Click();
});
$("#Buttonhelp").click(function () {
    Buttonhelp_Click();
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonfind_Click() {
    let AA = $("#DateTimePicker1").val();
    let BB = $("#ComboBox1").val();
    let CC = $("#TextBox1").val();
    let DD = $("#ComboBox2").val();
    let EEE = $("#TextBox2").val();
    let FFF = $("#TextBox3").val();
    let FAC = $("#ComboBox4").val();

    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.ListSearchString.push(AA);
    BaseParameter.ListSearchString.push(BB);
    BaseParameter.ListSearchString.push(CC);
    BaseParameter.ListSearchString.push(DD);
    BaseParameter.ListSearchString.push(EEE);
    BaseParameter.ListSearchString.push(FFF);
    BaseParameter.ListSearchString.push(FAC);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView1 = data.DataGridView1;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonadd_Click() {
}
function Buttonsave_Click() {
    let IsCheck = true;
    if (BaseResult.DataGridView1.length > 0) {
        ////$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                MC_LIST(0);
                alert("Edit data Completed.");
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    let AA = $("#DateTimePicker1").val();
    let BB = $("#ComboBox1").val();
    let CC = $("#TextBox1").val();
    let DD = $("#ComboBox2").val();
    let EEE = $("#TextBox2").val();
    let FFF = $("#TextBox3").val();
    let FAC = $("#ComboBox4").val();

    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.ListSearchString.push(AA);
    BaseParameter.ListSearchString.push(BB);
    BaseParameter.ListSearchString.push(CC);
    BaseParameter.ListSearchString.push(DD);
    BaseParameter.ListSearchString.push(EEE);
    BaseParameter.ListSearchString.push(FFF);
    BaseParameter.ListSearchString.push(FAC);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView1 = data.DataGridView1;
            DataGridView1Render();
            let fileName = "C05" + DateNow;
            TableHTMLToExcel("DataGridView1Table", fileName, fileName);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonprint_Click() {
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                let RowCount = BaseResult.DataGridView1.length;
                if (RowCount > 1000) {
                    RowCount = 1000;
                }
                for (let i = 0; i < RowCount; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView1[i].CHK == true) {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    if (BaseResult.DataGridView1[i].OR_NO == "") {
                        HTML = HTML + "<td style='background-color: green; padding-left: 10px; padding-right: 10px;'>" + BaseResult.DataGridView1[i].OR_NO + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: darkorange; padding-left: 10px; padding-right: 10px;'>" + BaseResult.DataGridView1[i].OR_NO + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td><input onblur='DataGridView1MCChange(" + i + ", this)' id='DataGridView1MC" + i + "' type='text' class='form-control' value='" + BaseResult.DataGridView1[i].MC + "' style='width: 100px;'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LS_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CUR_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CT_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CT_LEADS_PR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView1[i].SP_ST) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ORDER_IDX + "</td>";
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
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}

let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView1MCChange(i, input) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].MC = input.value;
}
function C05_STOP_Timer1StartInterval() {
    C05_STOP_Timer1 = setInterval(function () {
        C05_STOP_Timer1_Tick();
    }, 1000);
}
function C05_STOP_Timer1_Tick() {
    let End = new Date();
    $("#C05_STOP_Label4").val(CounterByBegin_EndToString(C05_STOP_StartTime, End));
}

function C05_STOP_ToggleButton3() {
    if ($("#C05_STOP_Label5").val() === "M") {
        $("#C05_STOP_Button3").show();
    } else {
        $("#C05_STOP_Button3").hide();
    }
}
function C05_STOP_PageLoad() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    C05_STOP_StartTime = now;
    C05_STOP_Timer1StartInterval();
    let STOP_MC = $("#C05_STOP_STOP_MC").val();
    let Label5 = $("#C05_STOP_Label5").val();
    let Label2 = $("#C05_STOP_Label2").val();

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label2);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_STOP/PageLoad";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    if (BaseResult.DataGridView1.length > 0) {
                        $("#C05_STOP_Label6").val(BaseResult.DataGridView1[0].TSNON_OPER_IDX);
                        SettingsNON_OPER_CHK = true;
                        SettingsNON_OPER_IDX = BaseResult.DataGridView1[0].TSNON_OPER_IDX;
                        localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
                        localStorage.setItem("SettingsNON_OPER_IDX", SettingsNON_OPER_IDX);
                    }
                }
            }
            C05_STOP_SW_TIME();
            C05_STOP_EW_TIME_Timer1StartInterval();

            // ← THÊM LOGIC NÀY
            document.getElementById("C05_STOP_Button1").disabled = true;
            document.getElementById("C05_STOP_Button2").disabled = true;
            if ($("#C05_STOP_Label5").val() == "M") {
                document.getElementById("C05_STOP_Button2").disabled = false;
            } else {
                document.getElementById("C05_STOP_Button1").disabled = false;
            }
            C05_STOP_ToggleButton3();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_STOP_FormClosed() {
    let Label6 = $("#C05_STOP_Label6").val();
    let STOP_MC = $("#C05_STOP_STOP_MC").val();
    let Label5 = $("#C05_STOP_Label5").val(); 

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C05_STOP_StartTime);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_STOP/C05_STOP_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            clearInterval(C05_STOP_Timer1);
            C05_STOP_OPER_TIME();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_STOP_OPER_TIME() {
    let STOP_MC = $("#C05_STOP_STOP_MC").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            try {
                BaseResult.C05_STOP_Search = data.Search;
                let TOT_SUM = BaseResult.C05_STOP_Search[0].SUM_TIME;

                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C05_START_Label71 = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C05_START_Label71.substr(0, 2);
                let M_TIME1 = C05_START_Label71.substr(2, 2);
                let S_TIME1 = C05_START_Label71.substr(4, 2);
                C05_START_Label71 = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                $("#C05_START_Label71").val(C05_START_Label71);
            }
            catch (e) {
            }
            C05_STOP_EW_TIME(1);
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_STOP_Button2").click(function () {
    C05_STOP_Button2_Click();
});
function C05_STOP_Button2_Click() {
    let STOP_MC = $("#C05_STOP_STOP_MC").val();
    let Label6 = $("#C05_STOP_Label6").val();

    let BaseParameter = { SearchString: STOP_MC };
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C05_STOP/Button2_Click", { method: "POST", body: formUpload })
        .then((response) => {
            response.json().then((data) => {
                $("#C05_STOP_TSNON_OPER_IDX").val(Label6);
                $("#C05_STOP_Label2").val("점검 중 / Đang kiểm tra");

                document.getElementById("C05_STOP_Button2").disabled = true;
                document.getElementById("C05_STOP_Button3").disabled = false;
                document.getElementById("C05_STOP_Button1").disabled = false;

                M.toast({
                    html: 'Đã chuyển trạng thái sang <b>ĐANG KIỂM TRA</b> (VÀNG)<br>Dashboard đã cập nhật!',
                    classes: 'yellow darken-2',
                    displayLength: 3000
                });
            }).catch((err) => {
                alert("Lỗi: " + err.message);
            })
        });
}
$("#C05_STOP_Button1").click(function () {
    C05_STOP_Button1_Click();
});
$("#C05_STOP_Button3").click(function () {
    let formElement = $("#C05_STOP_MaintenanceForm");
    if (formElement.length > 0) formElement[0].reset();
    $("#C05_STOP_MaintenanceModal").modal("open");
});
function C05_STOP_Button1_Click() {
    let Label5 = $("#C05_STOP_Label5").val();

    // ← THÊM VALIDATION
    if (Label5 == "M" && document.getElementById("C05_STOP_Button1").disabled) {
        M.toast({
            html: 'Vui lòng nhấn nút <b>"점검 중 / Đang kiểm tra"</b> trước khi khởi động lại!',
            classes: 'red darken-1',
            displayLength: 5000
        });
        return;
    }

    if (Label5 == "M") {
        let STOP_MC = $("#C05_STOP_STOP_MC").val();
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: STOP_MC,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_STOP/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                C05_STOP_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C05_STOP_FormClosed();
    }
}
function C05_STOP_SaveMaintenanceHistory() {
    let currentStatus = $("#C05_STOP_CurrentStatus").val();
    let solution = $("#C05_STOP_Solution").val();
    let maintenedBy = $("#C05_STOP_MaintenedBy").val();
    let sparePartsUsed = $("#C05_STOP_SparePartsUsed").val().trim();
    // Validate required fields
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

    let STOP_MC = $("#C05_STOP_STOP_MC").val();

    let BaseParameter = {
        STOP_MC: STOP_MC,
        CurrentStatus: currentStatus,
        Solution: solution,
        SparePartsUsed: sparePartsUsed,
        MaintenedBy: maintenedBy,
        Notes: $("#C05_STOP_Notes").val().trim(),
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C05_STOP/SaveMaintenanceHistory", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                M.toast({ html: 'Lưu chi tiết bảo trì thành công!', classes: 'green' });
                $("#C05_STOP_MaintenanceModal").modal("close");
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không thể lưu dữ liệu'), classes: 'red' });
            }
        })
        .catch(err => {
            M.toast({ html: 'Có lỗi xảy ra: ' + err.message, classes: 'red' });
        });
}
function C05_STOP_EW_TIME_Timer1StartInterval() {
    C05_STOP_EW_TIME_Timer1 = setInterval(function () {
        C05_STOP_EW_TIME_Timer1_Tick();
    }, 60000);
}
function C05_STOP_EW_TIME_Timer1_Tick() {
    C05_STOP_EW_TIME(0);
}
function C05_STOP_EW_TIME(Flag) {
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C05_STOP_STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_STOP/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (Flag == 1) {
                $("#C05_STOP").modal("close");
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_STOP_SW_TIME() {
    try {
        let USER_MC = localStorage.getItem("C02_MCbox");
        let USER_ORIDX = localStorage.getItem("C02_START_V2_Label8");
        let Label5 = $("#C05_STOP_Label5").val();;
        ////$("#BackGround").css("display", "block");
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
        let url = "/C05_STOP/SW_TIME";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C05_STOP_DGV_WT = data.DGV_WT;
                try {
                    C05_STOP_STOPW_ORING_IDX = BaseResult.C05_STOP_DGV_WT[0].TOWT_INDX;
                }
                catch (e) {
                    C05_STOP_STOPW_ORING_IDX = 0;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        C05_STOP_STOPW_ORING_IDX = 0;
    }
}

let C05_START_Timer1;
let C05_START_Timer2;
let C05_START_Timer3;
let C05_START_Timer4;
let C05_START_TI_CONT = 0;
let C05_START_StartTime;
let C05_START_RunTime;
let C05_START_SPC_EXIT = false;
let C05_START_DC_STR;
let C05_START_BARCODE_QR;
let C05_START_MCBOX;
let C05_START_TB_BARCODE_Text;
let C05_START_C05_FormClosedIsClick = 1;
let C05_START_C05_FormClosedCounter = 0;
function C05_START_PageLoad() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    C05_START_MCBOX = SettingsMC_NM;

    document.getElementById("C05_START_SPC1").innerHTML = "First";
    document.getElementById("C05_START_SPC2").innerHTML = "Middle";
    document.getElementById("C05_START_SPC3").innerHTML = "End";

    document.getElementById("C05_START_BT_APP1").disabled = true;
    document.getElementById("C05_START_BT_APP2").disabled = true;
    $("#C05_START_VLA1").val("");
    $("#C05_START_VLA2").val("");
    $("#C05_START_VLA11").val("");
    $("#C05_START_VLA22").val("");
    document.getElementById("C05_START_Buttonprint").disabled = true;
    C05_START_C05_start_Load();
    C05_START_ORDER_LOAD();
    C05_START_DB_COUTN();

    document.getElementById("C05_START_RadioButton1").checked = true;
    C05_START_OPER_TIME();
    C05_START_Timer2StartInterval();
    C05_START_Timer1StartInterval();
    C05_START_Timer4StartInterval();
    $("#C05_START_TB_BARCODE").val("");
}

$("#C05_START_Buttonclose").click(function (e) {
    C05_START_C05_FormClosedIsClick = 0;
    C05_START_Buttonclose_Click();
});
function C05_START_Buttonclose_Click() {
    C05_START_C05_FormClosed();
}
function C05_START_C05_FormClosed() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    ////$("#BackGround").css("display", "block");
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
            if (C05_START_C05_FormClosedIsClick == 0) {
                $("#C05_START").modal("close");
            }
            else {
                C05_START_C05_FormClosedTimerStartInterval();
            }
            //$("#C05_START").modal("close");
            $("#BC_TEXT").focus();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_START_C05_FormClosedTimerStartInterval() {
    let C05_START_C05_FormClosedTimer = setInterval(function () {
        C05_START_C05_FormClosedCounter = C05_START_C05_FormClosedCounter + 1;
        if (C05_START_C05_FormClosedCounter > 0) {
            C05_START_C05_FormClosedCounter = 0;
            clearInterval(C05_START_C05_FormClosedTimer);
            $("#C05_START").modal("close");
        }
    }, 1000);
}
function C05_START_C05_start_Load() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    let Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
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
function C05_START_ORDER_LOAD() {
    let IsCheck = true;
    let LH_CHK = false;
    let RH_CHK = false;
    document.getElementById("C05_START_BT_APP1").disabled = false;
    document.getElementById("C05_START_BT_APP2").disabled = false;

    let Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
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

            BaseResult.C05_START_DataGridView = data.DataGridView;
            BaseResult.C05_START_Search = data.Search;
            BaseResult.C05_START_Search1 = data.Search1;
            if (BaseResult.C05_START_Search.length <= 0) {
                let COM_COUNT = BaseResult.C05_START_Search1.length;
                if (COM_COUNT == 0) {
                    IsCheck = false;
                    M.toast({ html: "작업을 종료하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                    C05_START_Buttonclose_Click();
                }
                else {
                    IsCheck = false;
                    M.toast({ html: "등록된 자료가 없습니다. Không có dữ liệu đăng ký MES.", classes: 'green', displayLength: 6000 });
                    C05_START_Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                //C05_START_StartTime = BaseResult.Search1[0].TS_DATE; 
                C05_START_StartTime = new Date($("#C05_START_Label70").val());
                $("#C05_START_Label10").val(BaseResult.C05_START_Search[0].TERM1);
                $("#C05_START_VLT1").val(BaseResult.C05_START_Search[0].TERM1);
                $("#C05_START_Label14").val(BaseResult.C05_START_Search[0].TERM2);
                $("#C05_START_VLT2").val(BaseResult.C05_START_Search[0].TERM2);
                $("#C05_START_Label12").val(BaseResult.C05_START_Search[0].SEAL1);
                $("#C05_START_VLS1").val(BaseResult.C05_START_Search[0].SEAL1);
                $("#C05_START_Label16").val(BaseResult.C05_START_Search[0].SEAL2);
                $("#C05_START_VLS2").val(BaseResult.C05_START_Search[0].SEAL2);
                $("#C05_START_Label18").val(BaseResult.C05_START_Search[0].WIRE);
                C05_START_Label18_TextChanged();
                $("#C05_START_VLW").val(BaseResult.C05_START_Search[0].WIRE);
                $("#C05_START_ST_DSTR1").val(BaseResult.C05_START_Search[0].STRIP1);
                $("#C05_START_ST_DSTR2").val(BaseResult.C05_START_Search[0].STRIP2);
                $("#C05_START_Label24").val(BaseResult.C05_START_Search[0].CCH_W1);
                $("#C05_START_Label28").val(BaseResult.C05_START_Search[0].ICH_W1);
                $("#C05_START_Label26").val(BaseResult.C05_START_Search[0].CCH_W2);
                $("#C05_START_Label30").val(BaseResult.C05_START_Search[0].ICH_W2);
                $("#C05_START_Label48").val(BaseResult.C05_START_Search[0].TOT_QTY);
                $("#C05_START_Label59").val(BaseResult.C05_START_Search[0].PERFORMN);
                $("#C05_START_Label42").val(BaseResult.C05_START_Search[0].SP_ST);
                $("#C05_START_Label43").val(BaseResult.C05_START_Search[0].PROJECT);
                $("#C05_START_Label54").val(BaseResult.C05_START_Search[0].HOOK_RACK);
                $("#C05_START_barsq").val(BaseResult.C05_START_Search[0].Barcode_SEQ);
                $("#C05_START_Label77").val(BaseResult.C05_START_Search[0].ADJ_AF_QTY);
                $("#C05_START_TOOL_CONT1").val(BaseResult.C05_START_Search[0].T1_CONT);
                $("#C05_START_TOOL_CONT2").val(BaseResult.C05_START_Search[0].T2_CONT);
                $("#C05_START_Label50").val(BaseResult.C05_START_Search[0].BUNDLE_SIZE);
                $("#C05_START_T1_IDX").val(BaseResult.C05_START_Search[0].T1_PN_IDX);
                $("#C05_START_T2_IDX").val(BaseResult.C05_START_Search[0].T2_PN_IDX);
                $("#C05_START_S1_IDX").val(BaseResult.C05_START_Search[0].S1_PN_IDX);
                $("#C05_START_S2_IDX").val(BaseResult.C05_START_Search[0].S2_PN_IDX);
                $("#C05_START_WIRE_IDX").val(BaseResult.C05_START_Search[0].W_PN_IDX);
                $("#C05_START_TOOL1_IDX").val(BaseResult.C05_START_Search[0].T1_TOO_IDX);
                $("#C05_START_TOOL2_IDX").val(BaseResult.C05_START_Search[0].T2_TOOL_IDX);
                $("#C05_START_COUNT_LH").val(BaseResult.C05_START_Search[0].PERFORMN_L);
                $("#C05_START_COUNT_RH").val(BaseResult.C05_START_Search[0].PERFORMN_R);

                let T1_CHK = BaseResult.C05_START_Search[0].TERM1.indexOf("(");
                let T2_CHK = BaseResult.C05_START_Search[0].TERM2.indexOf("(");
                if (T1_CHK > -1) {
                    document.getElementById("C05_START_RB_LH").disabled = false;
                    let NOT_TERM1 = BaseResult.C05_START_Search[0].TERM1.replace("(", "").replace(")", "");
                    for (let i = 0; i < BaseResult.C05_START_DataGridView.length; i++) {
                        if (BaseResult.C05_START_DataGridView[i].CLIMP_TERM == NOT_TERM1) {
                            document.getElementById("C05_START_RB_LH").disabled = true;
                            document.getElementById("C05_START_BT_APP1").disabled = true;
                            document.getElementById("C05_START_RB_RH").checked = true;
                            C05_START_RB_LH_CheckedChanged();
                            LH_CHK = true;
                        }
                    }
                }
                else {
                    document.getElementById("C05_START_RB_LH").disabled = true;
                    document.getElementById("C05_START_BT_APP1").disabled = true;
                    LH_CHK = true;
                    document.getElementById("C05_START_RB_RH").checked = true;
                    C05_START_RB_LH_CheckedChanged();
                }
                if (T2_CHK > -1) {
                    document.getElementById("C05_START_RB_RH").disabled = false;
                    let NOT_TERM2 = BaseResult.C05_START_Search[0].TERM2.replace("(", "").replace(")", "");
                    for (let i = 0; i < BaseResult.C05_START_DataGridView.length; i++) {
                        if (BaseResult.C05_START_DataGridView[i].CLIMP_TERM == NOT_TERM2) {
                            document.getElementById("C05_START_RB_RH").disabled = true;
                            document.getElementById("C05_START_BT_APP2").disabled = true;
                            document.getElementById("C05_START_RB_LH").checked = true;
                            C05_START_RB_LH_CheckedChanged();
                            RH_CHK = true;
                        }
                    }
                }
                else {
                    document.getElementById("C05_START_RB_RH").disabled = true;
                    document.getElementById("C05_START_BT_APP2").disabled = true;
                    RH_CHK = true;
                    document.getElementById("C05_START_RB_LH").checked = true;
                    C05_START_RB_LH_CheckedChanged();
                }
                let T1_NM = BaseResult.C05_START_Search[0].T1_NM;
                if (T1_NM == "") {
                    $("#C05_START_VLA1").val("");
                    $("#C05_START_VLA11").val("");
                    $("#C05_START_TOOL_CONT1").val("");
                }
                else {
                    $("#C05_START_VLA1").val(BaseResult.C05_START_Search[0].TERM1);
                    $("#C05_START_VLA11").val(BaseResult.C05_START_Search[0].T1_NM);
                }
                let BB0 = BaseResult.C05_START_Search[0].WIRE;
                if (BB0 == "") {
                }
                else {
                    let PO1 = BB0.indexOf("  ") - BB0.indexOf(" ") - 1;
                    let TT2 = BB0.substr(BB0.indexOf(" ") + 1, PO1);
                    $("#C05_START_ST_DWIRE1").val(TT2);
                }
                let T2_NM = BaseResult.C05_START_Search[0].T2_NM;
                if (T2_NM == "") {
                    $("#C05_START_VLA2").val("");
                    $("#C05_START_VLA22").val("");
                    $("#C05_START_TOOL_CONT2").val("");
                }
                else {
                    $("#C05_START_VLA2").val(BaseResult.C05_START_Search[0].TERM2);
                    $("#C05_START_VLA22").val(BaseResult.C05_START_Search[0].T2_NM);
                }
                try {
                    let TOOL_CONT1 = $("#C05_START_TOOL_CONT1").val();
                    if (TOOL_CONT1 == "") {
                        document.getElementById("C05_START_TOOL_CONT1").style.color = "black";
                    }
                    else {
                        let T1_CONT = BaseResult.C05_START_Search[0].T1_CONT;
                        let TOOL_MAX1 = BaseResult.C05_START_Search[0].TOOL_MAX1;
                        TOOL_MAX1 = TOOL_MAX1 * 0.9;
                        if (T1_CONT <= TOOL_MAX1) {
                            document.getElementById("C05_START_TOOL_CONT1").style.color = "green";
                        }
                        if (T1_CONT > TOOL_MAX1) {
                            document.getElementById("C05_START_TOOL_CONT1").style.color = "yellow";
                        }
                        TOOL_MAX1 = BaseResult.C05_START_Search[0].TOOL_MAX1;
                        TOOL_MAX1 = TOOL_MAX1 * 1.0;
                        if (T1_CONT >= TOOL_MAX1) {
                            document.getElementById("C05_START_TOOL_CONT1").style.color = "red";
                        }
                    }
                }
                catch (e) {
                    $("#C05_START_TOOL_CONT1").val("ERROR");
                    document.getElementById("C05_START_TOOL_CONT1").style.color = "red";
                }
                try {
                    let TOOL_CONT2 = $("#C05_START_TOOL_CONT2").val();
                    if (TOOL_CONT2 == "") {
                        document.getElementById("C05_START_TOOL_CONT2").style.color = "black";
                    }
                    else {
                        let T2_CONT = BaseResult.C05_START_Search[0].T2_CONT;
                        let TOOL_MAX2 = BaseResult.C05_START_Search[0].TOOL_MAX2;
                        TOOL_MAX2 = TOOL_MAX2 * 0.9;
                        if (T2_CONT <= TOOL_MAX2) {
                            document.getElementById("C05_START_TOOL_CONT2").style.color = "green";
                        }
                        if (T2_CONT > TOOL_MAX2) {
                            document.getElementById("C05_START_TOOL_CONT2").style.color = "yellow";
                        }
                        TOOL_MAX2 = BaseResult.C05_START_Search[0].TOOL_MAX2;
                        TOOL_MAX2 = TOOL_MAX2 * 1.0;
                        if (T2_CONT >= TOOL_MAX2) {
                            document.getElementById("C05_START_TOOL_CONT2").style.color = "red";
                        }
                    }
                }
                catch (e) {
                    $("#C05_START_TOOL_CONT2").val("ERROR");
                    document.getElementById("C05_START_TOOL_CONT2").style.color = "red";
                }
                let AAA = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN;
                let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
                if (AAA >= BUNDLE_SIZE) {
                    $("#C05_START_Label50").val(BUNDLE_SIZE);
                }
                else {
                    $("#C05_START_Label50").val(AAA);
                }
                let LH = BaseResult.C05_START_Search[0].LH;
                document.getElementById("C05_START_RB_LHText").innerHTML = "LH";
                document.getElementById("C05_START_RB_RHText").innerHTML = "RH";
                if (LH == "L") {
                    document.getElementById("C05_START_RB_LH").disabled = true;
                    document.getElementById("C05_START_RB_LHText").innerHTML = "Complete";
                    document.getElementById("C05_START_RB_RH").checked = true;
                    document.getElementById("C05_START_BT_APP1").disabled = true;
                    C05_START_RB_LH_CheckedChanged();
                }
                else {
                    if (document.getElementById("C05_START_RB_LH").checked == true) {
                        document.getElementById("C05_START_RB_LH").checked = true;
                        C05_START_RB_LH_CheckedChanged();
                    }
                    else {
                        document.getElementById("C05_START_RB_RH").checked = true;
                        C05_START_RB_LH_CheckedChanged();
                    }
                }
                let RH = BaseResult.C05_START_Search[0].RH;
                if (RH == "R") {
                    document.getElementById("C05_START_RB_RH").disabled = true;
                    document.getElementById("C05_START_RB_RHText").innerHTML = "Complete";
                    document.getElementById("C05_START_RB_LH").checked = true;
                    document.getElementById("C05_START_BT_APP2").disabled = true;
                    C05_START_RB_LH_CheckedChanged();
                }
                else {
                    if (document.getElementById("C05_START_RB_LH").checked == true) {
                    }
                    else {
                        document.getElementById("C05_START_RB_RH").checked = true;
                        C05_START_RB_LH_CheckedChanged();
                    }
                }

                if (LH_CHK == true && RH_CHK == true) {
                    M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc. It is not a list of orders for LP. Please Check Barcode Again.", classes: 'green', displayLength: 6000 });
                    C05_START_Buttonclose_Click();
                }
                C05_START_BARCODE_LOADEnd = true;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(err);
            $("#BackGround").css("display", "none");
        })
    });

}
function C05_START_DB_COUTN() {
    $("#C05_START_Label65").val(0);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    ////$("#BackGround").css("display", "block");
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
            BaseResult.DataGridView1 = data.DataGridView1;
            let TOT_QTY = BaseResult.DataGridView1[0].SUM;
            if (TOT_QTY <= 0) {
                TOT_QTY = 0;
            }
            else {
            }
            $("#C05_START_Label65").val(TOT_QTY);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_START_OPER_TIME() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    ////$("#BackGround").css("display", "block");
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
            BaseResult.DataGridView2 = data.DataGridView2;
            let TOT_SUM = 0;
            try {
                TOT_SUM = BaseResult.DataGridView2[0].SUM_TIME;
            }
            catch (e) {
                TOT_SUM = 0;
            }
            try {
                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C05_START_Label71 = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C05_START_Label71.substr(0, 2);
                let M_TIME1 = C05_START_Label71.substr(2, 2);
                let S_TIME1 = C05_START_Label71.substr(4, 2);
                C05_START_Label71 = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                $("#C05_START_Label71").val(C05_START_Label71);
            }
            catch (e) {
                $("#C05_START_Label71").val("00:00:00");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_START_Timer1StartInterval() {
    C05_START_Timer1 = setInterval(function () {
        C05_START_Timer1_Tick();
    }, 500);
}
function C05_START_Timer1_Tick() {
    if (C05_START_TI_CONT >= 30) {
        document.getElementById("C05_START_Buttonprint").disabled = false;
        clearInterval(C05_START_Timer1);
    }
    else {
        if (C05_START_TI_CONT > 0) {
            C05_START_TI_CONT = C05_START_TI_CONT + 1;
            document.getElementById("C05_START_Buttonprint").innerHTML = "Complete(Barcode Reading) (" + C05_START_TI_CONT + ")";
            document.getElementById("C05_START_Buttonprint").disabled = true;
        }
        else {
            document.getElementById("C05_START_Buttonprint").innerHTML = "Complete(Barcode Reading)";
            document.getElementById("C05_START_Buttonprint").disabled = false;
        }
    }
}
function C05_START_Timer2StartInterval() {
    C05_START_Timer2 = setInterval(function () {
        C05_START_Timer2_Tick();
    }, 1000);
}
function C05_START_Timer2_Tick() {

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
    C05_START_RunTime = new Date();
    let myTimeSpan;
    let Text_time = $("#C05_START_Label71").val();
    let hh_time1 = Text_time.substr(0, 2);
    let mm_time1 = Text_time.substr(3, 2);
    let ss_time2 = Text_time.substr(6, 2);
    let hh_time = Number(Text_time.split(":")[0]) * 60 * 60;
    let mm_time = Number(Text_time.split(":")[1]) * 60;
    let ss_time = Number(Text_time.split(":")[2]);
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = C05_START_RunTime - C05_START_StartTime;
    myTimeSpan = Math.floor(myTimeSpan / 1000) - tot_time;
    let Label69 = ConvertSecondToString(myTimeSpan, ":");
    $("#C05_START_Label69").val(Label69);
    if (myTimeSpan < 0) {
        document.getElementById("C05_START_Label69").style.color = "red";
        document.getElementById("C05_START_Label72").style.color = "red";
        $("#C05_START_Label72").val("-");
    }
    else {
        document.getElementById("C05_START_Label69").style.color = "black";
        document.getElementById("C05_START_Label72").style.color = "black";
        $("#C05_START_Label72").val("+");
    }
    let M_STIME = new Date(NOW_DATE_S.getFullYear(), NOW_DATE_S.getMonth(), NOW_DATE_S.getDate(), 6, 0, 0);
    let M_SPAN = C05_START_RunTime - M_STIME;
    M_SPAN = Math.floor(M_SPAN / 1000);

    let DTIME11 = M_SPAN;
    let DTIME15 = DTIME11;

    let DTIME21 = $("#C05_START_Label69").val();
    let DTIME22 = Number(DTIME21.split(":")[0]) * 60 * 60;
    let DTIME23 = Number(DTIME21.split(":")[1]) * 60;
    let DTIME24 = Number(DTIME21.split(":")[2]);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    let Label75 = ((DTIME25 / DTIME15) * 100).toFixed(2) + " %";
    $("#C05_START_Label75").val(Label75);
    try {
        if ($("#C05_START_Label72").val() == "+") {
            let UPH_T1 = $("#C05_START_Label69").val();
            let UPH_H1 = Number(UPH_T1.split(":")[0]);
            let UPH_M1 = Number(UPH_T1.split(":")[1]);
            let Label65 = Number($("#C05_START_Label65").val());
            let Label62 = 0;
            try {
                let division = (UPH_H1 + Math.floor((UPH_M1 / 60)));
                if (division != 0) {
                    Label62 = Math.floor(Label65 / division);
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#C05_START_Label62").val(Label62);

        }
    }
    catch (e) {
        console.log(e);
    }
}
function C05_START_Timer3StartInterval() {
    C05_START_Timer3 = setInterval(function () {
        C05_START_Timer3_Tick();
    }, 10000);
}
function C05_START_Timer3_Tick() {
    C05_START_OPER_TIME();
}
function C05_START_Timer4StartInterval() {
    C05_START_Timer4 = setInterval(function () {
        C05_START_Timer4_Tick();
    }, 1000);
}
function C05_START_Timer4_Tick() {
    clearInterval(C05_START_Timer4);
    C05_START_F_SPC();
}
function C05_START_F_SPC() {
    try {
        let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
        let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
        if (TOT_QTY == BUNDLE_SIZE) {
            let SPC1 = document.getElementById("C05_START_SPC1").innerHTML;
            if (SPC1 == "First") {
                C05_SPC_Flag = 1;
                $("#C05_SPC_Label10").val("First");
                $("#C05_SPC").modal("open");
                C05_SPC_PageLoad();
            }
        }
    }
    catch (e) {
        C05_START_Buttonclose_Click();
    }
}
$("#C05_START_Label18").change(function (e) {
    C05_START_Label18_TextChanged();
});
function C05_START_Label18_TextChanged() {
    let aaw = $("#C05_START_Label18").val();
    aaw = aaw.trim();
    let bbw = aaw.substr(aaw.indexOf("  "));
    bbw = bbw.trim();
    let ccw = bbw.substr(bbw.indexOf("  "));
    $("#C05_START_WIRE_Length").val(ccw);
}
$("#C05_START_RB_LH").click(function (e) {
    C05_START_RB_LH_CheckedChanged();
});
$("#C05_START_RB_RH").click(function (e) {
    C05_START_RB_LH_CheckedChanged();
});
function C05_START_RB_LH_CheckedChanged() {
    if (document.getElementById("C05_START_RB_LH").checked == true) {
        document.getElementById("C05_START_LEAD_PIC").src = "/Image/LP_T1.png";
        document.getElementById("C05_START_C11_L11").style.display = "block";
        document.getElementById("C05_START_ST_DSTR1").style.display = "block";
        document.getElementById("C05_START_C11_L12").style.display = "none";
        document.getElementById("C05_START_ST_DSTR2").style.display = "none";
    }
    if (document.getElementById("C05_START_RB_RH").checked == true) {
        document.getElementById("C05_START_LEAD_PIC").src = "/Image/LP_T2.png";
        document.getElementById("C05_START_C11_L11").style.display = "none";
        document.getElementById("C05_START_ST_DSTR1").style.display = "none";
        document.getElementById("C05_START_C11_L12").style.display = "block";
        document.getElementById("C05_START_ST_DSTR2").style.display = "block";
    }
    C05_START_SPC_LOAD();
}
function C05_START_SPC_LOAD() {
    let Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: Label8,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.RB_LH = document.getElementById("C05_START_RB_LH").checked;
    BaseParameter.RB_RH = document.getElementById("C05_START_RB_RH").checked;
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
            BaseResult.C05_START_DataGridView3 = data.DataGridView3;
            let COUNT_SPC = Number($("#C05_START_Label48").val());
            try {
                COUNT_SPC = BaseResult.C05_START_Search[0].TOT_QTY;
            }
            catch (e) {
                COUNT_SPC = Number($("#C05_START_Label48").val());
            }
            try {
                //if (COUNT_SPC < 11) {
            
                   
                //}
                //else {
                //    document.getElementById("C05_START_SPC1").innerHTML = "First";
                //    document.getElementById("C05_START_SPC1").disabled = false;
                //}
                if (COUNT_SPC < 501) {
                    document.getElementById("C05_START_SPC1").innerHTML = "First";
                    document.getElementById("C05_START_SPC2").innerHTML = "----";
                    document.getElementById("C05_START_SPC3").innerHTML = "End";
                    document.getElementById("C05_START_SPC1").disabled = false;
                    document.getElementById("C05_START_SPC2").disabled = true;
                    document.getElementById("C05_START_SPC3").disabled = false;
                }
                else {
                    document.getElementById("C05_START_SPC1").innerHTML = "First";
                    document.getElementById("C05_START_SPC2").innerHTML = "Middle";
                    document.getElementById("C05_START_SPC3").innerHTML = "End";
                    document.getElementById("C05_START_SPC1").disabled = false;
                    document.getElementById("C05_START_SPC2").disabled = false;
                    document.getElementById("C05_START_SPC3").disabled = false;
                }
            }
            catch (e) {
                document.getElementById("C05_START_SPC1").innerHTML = "ERROR";
                document.getElementById("C05_START_SPC2").innerHTML = "ERROR";
                document.getElementById("C05_START_SPC3").innerHTML = "ERROR";
            }
            for (let i = 0; i < BaseResult.C05_START_DataGridView3.length; i++) {
                let COLSIP = BaseResult.C05_START_DataGridView3[i].COLSIP;
                if (COLSIP == "First") {
                    document.getElementById("C05_START_SPC1").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC1").disabled = true;
                }
                if (COLSIP == "Middle") {
                    document.getElementById("C05_START_SPC2").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC2").disabled = true;
                }
                if (COLSIP == "End") {
                    document.getElementById("C05_START_SPC3").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC3").disabled = true;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_START_Button2").click(function (e) {
    C05_START_Button2_Click_1();
});
function C05_START_Button2_Click_1() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    let RadioButton1 = document.getElementById("C05_START_RadioButton1").checked;
    if (RadioButton1 == true) {
        Non_idx = 1;
    }
    let RadioButton2 = document.getElementById("C05_START_RadioButton2").checked;
    if (RadioButton2 == true) {
        Non_idx = 2;
    }
    let RadioButton3 = document.getElementById("C05_START_RadioButton3").checked;
    if (RadioButton3 == true) {
        Non_idx = 3;
    }
    let RadioButton4 = document.getElementById("C05_START_RadioButton4").checked;
    if (RadioButton4 == true) {
        Non_idx = 4;
    }
    let RadioButton5 = document.getElementById("C05_START_RadioButton5").checked;
    if (RadioButton5 == true) {
        Non_idx = 5;
    }
    let RadioButton6 = document.getElementById("C05_START_RadioButton6").checked;
    if (RadioButton6 == true) {
        Non_idx = 6;
    }
    let RadioButton7 = document.getElementById("C05_START_RadioButton7").checked;
    if (RadioButton7 == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S";
            Non_text_NM = document.getElementById("C05_START_RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I";
            Non_text_NM = document.getElementById("C05_START_RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q";
            Non_text_NM = document.getElementById("C05_START_RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M";
            Non_text_NM = document.getElementById("C05_START_RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T";
            Non_text_NM = document.getElementById("C05_START_RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L";
            Non_text_NM = document.getElementById("C05_START_RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E";
            Non_text_NM = document.getElementById("C05_START_RadioButton7Text").innerHTML;
            break;
    }
    $("#C05_STOP_Label5").val(Non_text);
    $("#C05_STOP_STOP_MC").val($("#MCBOX").val());
    $("#C05_STOP_Label2").val(Non_text_NM + "(" + Non_text + ")");

    $("#C05_STOP").modal("open");
    C05_STOP_PageLoad();
}
$("#C05_START_Button1").click(function (e) {
    C05_START_Button1_Click();
});
function C05_START_Button1_Click() {
    let TERM_C = "-";
    let RB_LH = document.getElementById("C05_START_RB_LH").checked;
    let RB_RH = document.getElementById("C05_START_RB_RH").checked;
    if (RB_LH == true) {
        TERM_C = "LH"
    }
    if (RB_RH == true) {
        TERM_C = "RH"
    }
    $("#C05_DC_READ_Label2").val(TERM_C);
    $("#C05_DC_READ").modal("open");
    C05_DC_READ_PageLoad();
}

$("#C05_START_TB_BARCODE").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_START_TB_BARCODE_Text = $("#C05_START_TB_BARCODE").val();
        C05_START_TextBox1_KeyDown();
    }
});
function C05_START_TextBox1_KeyDown() {
    let Buttonprint = document.getElementById("C05_START_Buttonprint").disabled;
    if (Buttonprint == false) {
        C05_START_Buttonprint_Click();
        $("#C05_START_TB_BARCODE").val("");
        $("#C05_START_TB_BARCODE").focus();
    }
    else {

        //  M.toast({ html: "실적 처리 대기 중. Chờ đợi.", classes: 'red', displayLength: 6000 });
        ShowMessage('"실적 처리 대기 중. Chờ đợi.', 'warning');
        $("#C05_START_TB_BARCODE").val("");
        $("#C05_START_TB_BARCODE").focus();
    }
}
$("#C05_START_BT_APP1").click(function (e) {
    C05_START_Button4_Click();
});
function C05_START_Button4_Click() {
    let ERR_CHK_LH = false;
    let RB_LH = document.getElementById("C05_START_RB_LH").disabled;
    if (RB_LH == false) {
        let VLA1 = $("#C05_START_VLA1").val();
        if (VLA1 == "") {
            ERR_CHK_LH = true;
        }
    }
    RB_LH = document.getElementById("C05_START_RB_LH").checked;
    if (RB_LH == true) {
        if (ERR_CHK_LH == true) {
            let Label8 = $("#C05_START_Label8").val();
            let Label10 = $("#C05_START_Label10").val();
            Label10 = Label10.replace("(", "").replace(")", "");

            $("#C05_ER_L_LBT2S").val(Label10);
            $("#C05_ER_L_LBA2S").val(Label10);
            $("#C05_ER_L_Label1").val(Label8);
            $("#C05_ER_L").modal("open");
            C05_ER_L_PageLoad();

        }
        else {
            let VLA1 = $("#C05_START_VLA1").val();
            let VLA11 = $("#C05_START_VLA11").val();
            let Label8 = $("#C05_START_Label8").val();
            let Label10 = $("#C05_START_Label10").val();
            $("#C05_APPLICATION_Label3").val("APPLICATION #1");
            $("#C05_APPLICATION_Label1").val(VLA1);
            $("#C05_APPLICATION_Label2").val(VLA11);
            if (VLA1 == "") {

                $("#C05_ER_L_LBT2S").val(Label10);
                $("#C05_ER_L_LBA2S").val(Label10);
                $("#C05_ER_L_Label1").val(Label8);
                $("#C05_ER_L").modal("open");
                C05_ER_L_PageLoad();

            }
            else {
                $("#C05_APPLICATION").modal("open");
                C05_APPLICATION_PageLoad();
            }
        }
    }
}
$("#C05_START_BT_APP2").click(function (e) {
    C05_START_Button5_Click();
});
function C05_START_Button5_Click() {
    let ERR_CHK_RH = false;
    let RB_RH = document.getElementById("C05_START_RB_RH").disabled;
    if (RB_RH == false) {
        let VLA2 = $("#C05_START_VLA2").val();
        if (VLA2 == "") {
            ERR_CHK_RH = true;
        }
    }
    RB_RH = document.getElementById("C05_START_RB_RH").checked;
    if (RB_RH == true) {
        if (ERR_CHK_RH == true) {
            let Label8 = $("#C05_START_Label8").val();
            let Label14 = $("#C05_START_Label14").val();
            Label14 = Label14.replace("(", "").replace(")", "");

            $("#C05_ER_R_LBT2S").val(Label14);
            $("#C05_ER_R_LBA2S").val(Label14);
            $("#C05_ER_R_Label1").val(Label8);
            $("#C05_ER_R").modal("open");
            C05_ER_R_PageLoad();
        }
        else {
            let VLA2 = $("#C05_START_VLA2").val();
            let VLA22 = $("#C05_START_VLA22").val();
            let Label8 = $("#C05_START_Label8").val();
            let Label14 = $("#C05_START_Label14").val();

            $("#C05_APPLICATION_Label3").val("APPLICATION #2");
            $("#C05_APPLICATION_Label1").val(VLA2);
            $("#C05_APPLICATION_Label2").val(VLA22);


            if (VLA2 == "") {

                $("#C05_ER_R_LBT2S").val(Label10);
                $("#C05_ER_R_LBA2S").val(Label10);
                $("#C05_ER_R_Label1").val(Label8);
                $("#C05_ER_R").modal("open");
                C05_ER_R_PageLoad();
            }
            else {
                $("#C05_APPLICATION").modal("open");
                C05_APPLICATION_PageLoad();
            }
        }
    }
}
$("#C05_START_SPC1").click(function (e) {
    C05_START_SPC1_Click();
});
function C05_START_SPC1_Click() {
    let IsCheck = true;
    $("#C05_SPC_Label10").val("First");
    let RB_LH = document.getElementById("C05_START_RB_LH").checked;
    let RB_RH = document.getElementById("C05_START_RB_RH").checked;
    if (RB_LH == true) {
        $("#C05_SPC_Label11").val("L");
        let VLA1 = $("#C05_START_VLA1").val();
        if (VLA1 == "") {
            IsCheck = false;
        }
    }
    if (IsCheck == true) {
        if (RB_RH == true) {
            $("#C05_SPC_Label11").val("R");
            let VLA2 = $("#C05_START_VLA2").val();
            if (VLA2 == "") {
                IsCheck = false;
            }
        }
    }
    if (IsCheck == true) {
        C05_SPC_Flag = 0;
        $("#C05_SPC").modal("open");
        C05_SPC_PageLoad();
    }
}
$("#C05_START_SPC2").click(function (e) {
    C05_START_SPC2_Click();
});
function C05_START_SPC2_Click() {
    let IsCheck = true;
    let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
    let PERFORMN = BaseResult.C05_START_Search[0].PERFORMN;

    if (TOT_QTY > 501) {
        if (TOT_QTY / 2 <= PERFORMN) {
            $("#C05_SPC_Label10").val("Middle");
            let RB_LH = document.getElementById("C05_START_RB_LH").checked;
            let RB_RH = document.getElementById("C05_START_RB_RH").checked;
            if (RB_LH == true) {
                $("#C05_SPC_Label11").val("L");
                let VLA1 = $("#C05_START_VLA1").val();
                if (VLA1 == "") {
                    IsCheck = false;
                }
            }
            if (IsCheck == true) {
                if (RB_RH == true) {
                    $("#C05_SPC_Label11").val("R");
                    let VLA2 = $("#C05_START_VLA2").val();
                    if (VLA2 == "") {
                        IsCheck = false;
                    }
                }
            }
            if (IsCheck == true) {
                C05_SPC_Flag = 0;
                $("#C05_SPC").modal("open");
                C05_SPC_PageLoad();
            }
        }
    }
}

$("#C05_START_SPC3").click(function (e) {
    C05_START_SPC3_Click();
});
function C05_START_SPC3_Click() {
    let IsCheck = true;
    let AA = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN;
    let BB = AA - BaseResult.C05_START_Search[0].BUNDLE_SIZE;
    if (BB <= 0) {
        $("#C05_SPC_Label10").val("End");
        let RB_LH = document.getElementById("C05_START_RB_LH").checked;
        let RB_RH = document.getElementById("C05_START_RB_RH").checked;
        if (RB_LH == true) {
            $("#C05_SPC_Label11").val("L");
            let VLA1 = $("#C05_START_VLA1").val();
            if (VLA1 == "") {
                IsCheck = false;
            }
        }
        if (IsCheck == true) {
            if (RB_RH == true) {
                $("#C05_SPC_Label11").val("R");
                let VLA2 = $("#C05_START_VLA2").val();
                if (VLA2 == "") {
                    IsCheck = false;
                }
            }
        }
        if (IsCheck == true) {
            C05_SPC_Flag = 0;
            $("#C05_SPC").modal("open");
            C05_SPC_PageLoad();
        }
    }
}
function C05_START_SPC_LOAD() {
    let Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: Label8,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.RB_LH = document.getElementById("C05_START_RB_LH").checked;
    BaseParameter.RB_RH = document.getElementById("C05_START_RB_RH").checked;
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
            BaseResult.C05_START_DataGridView3 = data.DataGridView3;
            let COUNT_SPC = Number($("#C05_START_Label48").val());
            try {
                COUNT_SPC = BaseResult.C05_START_Search[0].TOT_QTY;
            }
            catch (e) {
                COUNT_SPC = Number($("#C05_START_Label48").val());
            }
            try {
                
                if (COUNT_SPC < 501) {          
                    document.getElementById("C05_START_SPC1").innerHTML = "First";
                    document.getElementById("C05_START_SPC2").innerHTML = "----";
                    document.getElementById("C05_START_SPC3").innerHTML = "End";
                    document.getElementById("C05_START_SPC1").disabled = false;
                    document.getElementById("C05_START_SPC2").disabled = true;
                    document.getElementById("C05_START_SPC3").disabled = false;
                    document.getElementById("C05_START_SPC2").disabled = true;
                    document.getElementById("C05_START_SPC2").disabled = false;
                }
                else {
                    document.getElementById("C05_START_SPC1").innerHTML = "First";
                    document.getElementById("C05_START_SPC2").innerHTML = "Midle";
                    document.getElementById("C05_START_SPC3").innerHTML = "End";
                    document.getElementById("C05_START_SPC1").disabled = false;
                    document.getElementById("C05_START_SPC2").disabled = false;
                    document.getElementById("C05_START_SPC3").disabled = false;
                }
            }
            catch (e) {
                document.getElementById("C05_START_SPC1").innerHTML = "ERROR";
                document.getElementById("C05_START_SPC2").innerHTML = "ERROR";
                document.getElementById("C05_START_SPC3").innerHTML = "ERROR";
            }
            for (let i = 0; i < BaseResult.C05_START_DataGridView3.length; i++) {
                let COLSIP = BaseResult.C05_START_DataGridView3[i].COLSIP;
                if (COLSIP == "First") {
                    document.getElementById("C05_START_SPC1").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC1").disabled = true;
                }
                if (COLSIP == "Middle") {
                    document.getElementById("C05_START_SPC2").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC2").disabled = true;
                }
                if (COLSIP == "End") {
                    document.getElementById("C05_START_SPC3").innerHTML = "Complete";
                    document.getElementById("C05_START_SPC3").disabled = true;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_START_Buttonprint").click(function (e) {
    C05_START_Buttonprint_Click();
});
function C05_START_Buttonprint_Click() {
    let barcode = $("#C05_START_TB_BARCODE").val().toUpperCase();
    let LP_UworkCode = $("#C05_START_Label4").val().toUpperCase();

    if (barcode && LP_UworkCode && barcode.includes(LP_UworkCode.trim())) {
        //ShowMessage('Đúng mã đang làm', 'ok');
    } else {
        ShowMessage('Sai mã đang làm', 'error');
        return;
    }

    let IsCheck = true;
    let ERR_CHK_LH = false;
    let ERR_CHK_RH = false;
    let RB_LH = document.getElementById("C05_START_RB_LH").disabled;
    if (RB_LH == false) {
        let VLA1 = $("#C05_START_VLA1").val();
        if (VLA1 == "") {
            ERR_CHK_LH = true;
        }
    }
    let RB_RH = document.getElementById("C05_START_RB_RH").disabled;
    if (RB_RH == false) {
        let VLA2 = $("#C05_START_VLA2").val();
        if (VLA2 == "") {
            ERR_CHK_RH = true;
        }
    }
    RB_LH = document.getElementById("C05_START_RB_LH").checked;
    if (RB_LH == true) {
        if (ERR_CHK_LH == true) {
            IsCheck = false;
            alert("NOT ERROR PROOF. Please Check Again.");
            //M.toast({ html: "NOT ERROR PROOF. Please Check Again.", classes: 'red', displayLength: 6000 });
            $("#C05_START_TB_BARCODE").val("");
        }
    }
    if (IsCheck == true) {
        RB_RH = document.getElementById("C05_START_RB_RH").checked;
        if (RB_RH == true) {
            if (ERR_CHK_RH == true) {
                IsCheck = false;
                alert("NOT ERROR PROOF. Please Check Again.");
                //M.toast({ html: "NOT ERROR PROOF. Please Check Again.", classes: 'red', displayLength: 6000 });
                $("#C05_START_TB_BARCODE").val("");
            }
        }
    }
    if (IsCheck == true) {
        let TB_BARCODE = $("#C05_START_TB_BARCODE").val();
        if (TB_BARCODE == "") {
            IsCheck = false;
            alert("NOT ERROR PROOF. Please Check Again.");
            //M.toast({ html: "NOT ERROR PROOF. Please Check Again.", classes: 'red', displayLength: 6000 });
            $("#C05_START_TB_BARCODE").val("");
        }
        else {
            C05_START_DC_STR = "";
            C05_START_BARCODE_LOAD();
        }
    }
}
function C05_START_BARCODE_LOAD() {
    let IsCheck = true;
    let TB_BARCODE = $("#C05_START_TB_BARCODE").val();
   // TB_BARCODE = C05_START_TB_BARCODE_Text;
    let Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(TB_BARCODE);
    BaseParameter.ListSearchString.push(C05_START_DC_STR);
    BaseParameter.ListSearchString.push(Label8);
    BaseParameter.RB_LH = document.getElementById("C05_START_RB_LH").checked;
    BaseParameter.RB_RH = document.getElementById("C05_START_RB_RH").checked;

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
            BaseResult.C05_START_DataGridView4 = data.DataGridView4;
            if (BaseResult.C05_START_DataGridView4.length <= 0) {
                IsCheck = false;
                M.toast({ html: "NOT Order Barcode(Wrong). Please Check Again.", classes: 'red', displayLength: 6000 });
                $("#C05_START_TB_BARCODE").val("");
                $("#C05_START_TB_BARCODE").focus();
            }
            if (IsCheck == true) {
                let DSCN_YN = BaseResult.C05_START_DataGridView4[0].DSCN_YN;
                let LH = BaseResult.C05_START_DataGridView4[0].LH;
                let RH = BaseResult.C05_START_DataGridView4[0].RH;
                if (DSCN_YN == "Y" || (LH == "L" && BaseParameter.RB_LH == true) || (RH == "R" && BaseParameter.RB_RH == true)) {
                    IsCheck = false;
                    M.toast({ html: "이미 처리된 바코드. Đã xử lý Barcode trước đó", classes: 'red', displayLength: 6000 });
                    $("#C05_START_TB_BARCODE").val("");
                    $("#C05_START_TB_BARCODE").focus();
                }

                if (IsCheck == true) {
                    C05_START_BARCODE_QR = $("#C05_START_TB_BARCODE").val();
                    C05_START_BARCODE_QR = C05_START_TB_BARCODE_Text;
                    document.getElementById("C05_START_Buttonprint").disabled = true;
                    let Tcount = $("#C05_START_TB_BARCODE").val();
                    Tcount = C05_START_TB_BARCODE_Text;
                    let RB_LH = document.getElementById("C05_START_RB_LH").checked;
                    if (RB_LH == true) {
                        let RB_LHVisible = document.getElementById("C05_START_RB_LH").disabled;
                        if (RB_LHVisible == true) {
                            IsCheck = false;
                        }
                        if (IsCheck == true) {
                            let ATOOL1 = true;
                            let BT_APP1 = document.getElementById("C05_START_BT_APP1").disabled;
                            let VLA1 = $("#C05_START_VLA1").val();
                            if (BT_APP1 == false) {
                                if (VLA1 == "") {
                                    IsCheck = false;
                                    M.toast({ html: "APPLICATOR #1 data null. Please Check Again.", classes: 'red', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                            }
                            else {
                                ATOOL1 = false;
                            }
                            if (IsCheck == true) {
                                let TOOLA1 = Number($("#C05_START_TOOL_CONT1").val());
                                let TOOLC1 = 0;
                                try {
                                    TOOLC1 = BaseResult.C05_START_Search[0].TOOL_MAX1;
                                }
                                catch (e) {
                                    TOOLC1 = 1000000;
                                }
                                if (TOOLA1 >= TOOLC1) {
                                    IsCheck = false;
                                    M.toast({ html: "Application #1 check(Check counter). Please Check Again.", classes: 'red', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                                if (IsCheck == true) {
                                    C05_START_SPC_EXIT = true;
                                    if (ATOOL1 == true) {
                                        let IsModalOpen = false;
                                        let SPC1 = document.getElementById("C05_START_SPC1").innerHTML;
                                        if (SPC1 == "First") {
                                            C05_SPC_IsShow = false;
                                            C05_SPC_Flag = 2;
                                            $("#C05_SPC_Label10").val("First");
                                            $("#C05_SPC_Label11").val("L");
                                            $("#C05_SPC").modal("open");
                                            C05_SPC_PageLoad();
                                            IsModalOpen = true;
                                        }
                                        if (C05_SPC_IsShow == true) {
                                            if (C05_SPC_IsShowMiddle == false) {
                                                let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
                                                let PERFORMN_L = BaseResult.C05_START_Search[0].PERFORMN_L;
                                                if (TOT_QTY > 500) {
                                                    if (TOT_QTY / 2 <= PERFORMN_L) {
                                                        let SPC2 = document.getElementById("C05_START_SPC2").innerHTML;
                                                        if (SPC2 == "Middle") {
                                                            C05_SPC_IsShow = false;
                                                            C05_SPC_IsShowMiddle = true;
                                                            C05_SPC_Flag = 2;
                                                            $("#C05_SPC_Label10").val("Middle");
                                                            $("#C05_SPC_Label11").val("L");
                                                            $("#C05_SPC").modal("open");
                                                            C05_SPC_PageLoad();
                                                            IsModalOpen = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (C05_SPC_IsShow == true) {
                                            if (C05_SPC_IsShowEnd == false) {
                                                let A_CSPC = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN_L;
                                                let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
                                                if (BUNDLE_SIZE >= A_CSPC) {
                                                    let SPC3 = document.getElementById("C05_START_SPC3").innerHTML;
                                                    if (SPC3 == "End") {
                                                        C05_SPC_IsShow = false;
                                                        C05_SPC_IsShowEnd = true;
                                                        C05_SPC_Flag = 2;
                                                        $("#C05_SPC_Label10").val("End");
                                                        $("#C05_SPC_Label11").val("L");
                                                        $("#C05_SPC").modal("open");
                                                        C05_SPC_PageLoad();
                                                        IsModalOpen = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (IsModalOpen == false) {
                                            C05_SPC_Flag = 2;
                                            C05_SPC_FormClosed();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    let RB_RH = document.getElementById("C05_START_RB_RH").checked;
                    if (RB_RH == true) {
                        let RB_RHVisible = document.getElementById("C05_START_RB_RH").disabled;
                        if (RB_RHVisible == true) {
                            IsCheck = false;
                        }
                        if (IsCheck == true) {
                            let ATOOL2 = true;
                            let BT_APP2 = document.getElementById("C05_START_BT_APP2").disabled;
                            let VLA2 = $("#C05_START_VLA2").val();
                            if (BT_APP2 == false) {
                                if (VLA2 == "") {
                                    IsCheck = false;
                                    M.toast({ html: "APPLICATOR #2 data null. Please Check Again.", classes: 'red', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                            }
                            else {
                                ATOOL2 = false;
                            }
                            if (IsCheck == true) {
                                let TOOLA2 = Number($("#C05_START_TOOL_CONT2").val());
                                let TOOLC2 = 0;
                                try {
                                    TOOLC2 = BaseResult.C05_START_Search[0].TOOL_MAX2;
                                }
                                catch (e) {
                                    TOOLC2 = 1000000;
                                }
                                if (TOOLA2 >= TOOLC2) {
                                    IsCheck = false;
                                    M.toast({ html: "Application #2 check(Check counter). Please Check Again.", classes: 'red', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                                if (IsCheck == true) {
                                    C05_START_SPC_EXIT = true;
                                    if (ATOOL2 == true) {
                                        let IsModalOpen = false;
                                        let SPC1 = document.getElementById("C05_START_SPC1").innerHTML;
                                        if (SPC1 == "First") {
                                            C05_SPC_IsShow = false;
                                            C05_SPC_Flag = 3;
                                            $("#C05_SPC_Label10").val("First");
                                            $("#C05_SPC_Label11").val("R");
                                            $("#C05_SPC").modal("open");
                                            C05_SPC_PageLoad();
                                            IsModalOpen = true;
                                        }
                                        if (C05_SPC_IsShow == true) {
                                            if (C05_SPC_IsShowMiddle == false) {
                                                let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
                                                let PERFORMN_L = BaseResult.C05_START_Search[0].PERFORMN_L;
                                                if (TOT_QTY > 500) {
                                                    if (TOT_QTY / 2 <= PERFORMN_L) {
                                                        let SPC2 = document.getElementById("C05_START_SPC2").innerHTML;
                                                        if (SPC2 == "Middle") {
                                                            C05_SPC_IsShow = false;
                                                            C05_SPC_IsShowMiddle = true;
                                                            C05_SPC_Flag = 3;
                                                            $("#C05_SPC_Label10").val("Middle");
                                                            $("#C05_SPC_Label11").val("R");
                                                            $("#C05_SPC").modal("open");
                                                            C05_SPC_PageLoad();
                                                            IsModalOpen = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (C05_SPC_IsShow == true) {
                                            if (C05_SPC_IsShowEnd == false) {
                                                let A_CSPC = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN_L;
                                                let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
                                                if (BUNDLE_SIZE >= A_CSPC) {
                                                    let SPC3 = document.getElementById("C05_START_SPC3").innerHTML;
                                                    if (SPC3 == "End") {
                                                        C05_SPC_IsShow = false;
                                                        C05_SPC_IsShowEnd = true;
                                                        C05_SPC_Flag = 3;
                                                        $("#C05_SPC_Label10").val("End");
                                                        $("#C05_SPC_Label11").val("R");
                                                        $("#C05_SPC").modal("open");
                                                        C05_SPC_PageLoad();
                                                        IsModalOpen = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (IsModalOpen == false) {
                                            C05_SPC_Flag = 3;
                                            C05_SPC_FormClosed();
                                        }
                                    }
                                }
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

function C05_START_BARCODE_LOADSub003() {
    try {
        let Tcount = $("#C05_START_TB_BARCODE").val();
        Tcount = C05_START_TB_BARCODE_Text;
        let Label8 = $("#C05_START_Label8").val();
        ////$("#BackGround").css("display", "block");
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
                document.getElementById("C05_START_Buttonprint").disabled = true;
                C05_START_TI_CONT = 1;
                C05_START_Timer1StartInterval();
                M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                C05_DC_READ_COUNT_BL = true;
                try {
                    C05_START_SPC_LOAD();
                }
                catch (e) {
                    M.toast({ html: "#1 Please Check Again.", classes: 'green', displayLength: 6000 });
                    document.getElementById("C05_START_Buttonprint").disabled = false;
                }
                try {
                    C05_START_ORDER_LOAD();
                }
                catch (e) {
                    M.toast({ html: "#2 Please Check Again.", classes: 'green', displayLength: 6000 });
                    document.getElementById("C05_START_Buttonprint").disabled = false;
                }
                $("#C05_START_TB_BARCODE").val("");
                $("#C05_START_TB_BARCODE").focus();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        M.toast({ html: "Please Check Again.", classes: 'green', displayLength: 6000 });
        document.getElementById("C05_START_Buttonprint").disabled = false;
    }
}

function C05_APPLICATION_PageLoad() {
    $("#C05_APPLICATION_Label5").val("---");
    $("#C05_APPLICATION_Label4").val("---");

    $("#C05_APPLICATION_TextBox1").val("");
    $("#C05_APPLICATION_TextBox1").focus();
}
$("#C05_APPLICATION_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_APPLICATION_TextBox1_KeyDown();
    }
});
function C05_APPLICATION_TextBox1_KeyDown() {
    C05_APPLICATION_Button1_Click();
}
$("#C05_APPLICATION_Button1").click(function (e) {
    C05_APPLICATION_Button1_Click();
});
function C05_APPLICATION_Button1_Click() {
    let IsCheck = true;
    let APPT = $("#C05_APPLICATION_TextBox1").val();
    let Label1 = $("#C05_APPLICATION_Label1").val();
    Label1 = Label1.toUpperCase();
    let CO = APPT.length;
    let AAA = APPT.substr(0, CO - 1);
    if (AAA == Label1) {
        ////$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(APPT);
        BaseParameter.ListSearchString.push(Label1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_APPLICATION/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C05_APPLICATION_Search = data.Search;
                if (BaseResult.C05_APPLICATION_Search.length <= 0) {
                    IsCheck = false
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'green', displayLength: 6000 });
                }
                if (IsCheck == true) {
                    $("#C05_APPLICATION_Label5").val(BaseResult.C05_APPLICATION_Search[0].APP_NAME);
                    $("#C05_APPLICATION_Label4").val(BaseResult.C05_APPLICATION_Search[0].SEQ);
                    $("#C05_APPLICATION_Label9").val(BaseResult.C05_APPLICATION_Search[0].WK_CNT);
                    $("#C05_APPLICATION_Label10").val(BaseResult.C05_APPLICATION_Search[0].TOOLMASTER_IDX);
                }
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        IsCheck = false;
        M.toast({ html: "APPLICATOR을 잘못 등록했습니다. Đăng ký sai của ỨNG DỤNG.", classes: 'green', displayLength: 6000 });
    }
}
$("#C05_APPLICATION_Button2").click(function (e) {
    C05_APPLICATION_Button2_Click();
});
function C05_APPLICATION_Button2_Click() {
    let IsCheck = true;
    let Label5 = $("#C05_APPLICATION_Label5").val();
    if (Label5 == "---") {
        IsCheck = false;
    }
    let Label4 = $("#C05_APPLICATION_Label4").val();
    if (Label4 == "---") {
        IsCheck = false;
    }
    let Label3 = $("#C05_APPLICATION_Label3").val();
    let Label10 = $("#C05_APPLICATION_Label10").val();

    let C05_START_Label8 = $("#C05_START_Label8").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label3);
    BaseParameter.ListSearchString.push(Label10);
    BaseParameter.ListSearchString.push(C05_START_Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_APPLICATION/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            C05_APPLICATION_Buttonclose_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_APPLICATION_Buttonclose").click(function (e) {
    C05_APPLICATION_Buttonclose_Click();
});
function C05_APPLICATION_Buttonclose_Click() {
    $("#C05_APPLICATION").modal("close");
    C05_START_ORDER_LOAD();
}


let C05_DC_READ_COUNT_BL;
let C05_DC_READ_BARCODE_TEXT;
let C05_DC_READ_DataGridView1RowIndex;

function C05_DC_READ_PageLoad() {

    BaseResult.C05_DC_READ_DataGridView1 = [];
    C05_DC_READ_DataGridView1Render();
    $("#C05_DC_READ_Label4").val("-");
    $("#C05_DC_READ_TB_BARCODE").val("");
    $("#C05_DC_READ_TB_BARCODE").focus();
}
$("#C05_DC_READ_Buttonclose").click(function () {
    C05_DC_READ_Buttonclose_Click();
});
function C05_DC_READ_Buttonclose_Click() {
    $("#C05_DC_READ").modal("close");
}

$("#C05_DC_READ_TB_BARCODE").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_DC_READ_TB_BARCODE_KeyDown();
    }
});
function C05_DC_READ_TB_BARCODE_KeyDown() {
    let IsCheck = true;
    let BARNM = $("#C05_DC_READ_TB_BARCODE").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: BARNM,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.C05_DC_READ_DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_DC_READ/TB_BARCODE_KeyDown";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C05_DC_READ_Search1 = data.Search1;
            let LIST_CHK = true;
            let TB_BARCODE = $("#C05_DC_READ_TB_BARCODE").val();
            for (let i = 0; i < BaseResult.C05_DC_READ_DataGridView1.length; i++) {
                if (TB_BARCODE == BaseResult.C05_DC_READ_DataGridView1[i].Barcode) {
                    LIST_CHK = false;
                }
            }
            if (LIST_CHK == true) {
                if (BaseResult.C05_DC_READ_Search1.length <= 0) {
                    IsCheck = false;
                    M.toast({ html: "NOT Order Barcode(Wrong). Please Check Again.", classes: 'red', displayLength: 6000 });
                    $("#C05_DC_READ_TB_BARCODE").val("");
                    $("#C05_DC_READ_TB_BARCODE").focus();
                }
                if (IsCheck == true) {
                    let DSCN_YN = BaseResult.C05_DC_READ_Search1[0].DSCN_YN;
                    if (DSCN_YN == "Y") {
                        IsCheck = false;
                        M.toast({ html: "이미 처리된 바코드. Đã xử lý Barcode trước đó", classes: 'red', displayLength: 6000 });
                        $("#C05_DC_READ_TB_BARCODE").val("");
                        $("#C05_DC_READ_TB_BARCODE").focus();
                    }
                }
                if (IsCheck == true) {
                    let DGV_ORNO = BaseResult.C05_DC_READ_Search1[0].ORDER_IDX;
                    let DGV_T1 = BaseResult.C05_DC_READ_Search1[0].TERM1;
                    let DGV_T2 = BaseResult.C05_DC_READ_Search1[0].TERM2;
                    let DGV_LEAD = BaseResult.C05_DC_READ_Search1[0].LEAD_NO;
                    for (let i = 0; i < BaseResult.C05_DC_READ_DataGridView1.length; i++) {
                        let ORDER_IDX = BaseResult.C05_DC_READ_DataGridView1[i].ORDER_IDX;
                        if (DGV_ORNO == ORDER_IDX) {
                            IsCheck = false;
                            M.toast({ html: "이미 처리된 ORDER NO. Đã xử lý ORDER NO trước đó", classes: 'red', displayLength: 6000 });
                            $("#C05_DC_READ_TB_BARCODE").val("");
                            $("#C05_DC_READ_TB_BARCODE").focus();
                        }
                    }
                    let Label4 = $("#C05_DC_READ_Label4").val();
                    if (Label4 == "-") {
                        let Label2 = $("#C05_DC_READ_Label2").val();
                        if (Label2 == "LH") {
                            $("#C05_DC_READ_Label4").val(BaseResult.C05_DC_READ_Search1[0].TERM1);
                        }
                        if (Label2 == "RH") {
                            $("#C05_DC_READ_Label4").val(BaseResult.C05_DC_READ_Search1[0].TERM2);
                        }

                        let DataGridView1Item = {
                            ORDER_IDX: DGV_ORNO,
                            TERM1: DGV_T1,
                            TERM2: DGV_T2,
                            LEAD_NO: DGV_LEAD,
                            Barcode: TB_BARCODE,
                        }
                        BaseResult.C05_DC_READ_DataGridView1.push(DataGridView1Item);
                        C05_DC_READ_DataGridView1Render();
                    }
                    else {
                        let Label2 = $("#C05_DC_READ_Label2").val();
                        if (Label2 == "LH") {
                            if (Label4 == BaseResult.C05_DC_READ_Search1[0].TERM1) {
                                let DataGridView1Item = {
                                    ORDER_IDX: DGV_ORNO,
                                    TERM1: DGV_T1,
                                    TERM2: DGV_T2,
                                    LEAD_NO: DGV_LEAD,
                                    Barcode: TB_BARCODE,
                                }
                                BaseResult.C05_DC_READ_DataGridView1.push(DataGridView1Item);
                                C05_DC_READ_DataGridView1Render();
                            }
                            else {
                                IsCheck = false;
                                M.toast({ html: "NOT Order Barcode(Wrong). TERM 1. Please Check Again.", classes: 'red', displayLength: 6000 });
                                $("#C05_DC_READ_TB_BARCODE").val("");
                                $("#C05_DC_READ_TB_BARCODE").focus();
                            }
                        }
                        if (Label2 == "RH") {
                            if (Label4 == BaseResult.C05_DC_READ_Search1[0].TERM2) {
                                let DataGridView1Item = {
                                    ORDER_IDX: DGV_ORNO,
                                    TERM1: DGV_T1,
                                    TERM2: DGV_T2,
                                    LEAD_NO: DGV_LEAD,
                                    Barcode: TB_BARCODE,
                                }
                                BaseResult.C05_DC_READ_DataGridView1.push(DataGridView1Item);
                                C05_DC_READ_DataGridView1Render();
                            }
                            else {
                                IsCheck = false;
                                M.toast({ html: "NOT Order Barcode(Wrong). TERM 2. Please Check Again.", classes: 'red', displayLength: 6000 });
                                $("#C05_DC_READ_TB_BARCODE").val("");
                                $("#C05_DC_READ_TB_BARCODE").focus();
                            }
                        }
                    }
                }
            }
            else {
                M.toast({ html: "Check BarCode ID.   Please Check Again.", classes: 'red', displayLength: 6000 });
            }

            if (IsCheck == true) {
                $("#C05_DC_READ_TB_BARCODE").val("");
                $("#C05_DC_READ_TB_BARCODE").focus();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_DC_READ_Buttonprint").click(function (e) {
    C05_DC_READ_Buttonprint_Click();
});
function C05_DC_READ_Buttonprint_Click() {
    let IsCheck = true;
    if (BaseResult.C05_DC_READ_DataGridView1.length >= 2) {
        let Label2 = $("#C05_DC_READ_Label2").val();
        if (Label2 == "LH") {
            let C05_START_VLA1 = $("#C05_START_VLA1").val();
            if (C05_START_VLA1 == "") {
                IsCheck = false;
                M.toast({ html: "NOT ERROR PROOF. Please Check Again.", classes: 'red', displayLength: 6000 });
                $("#C05_DC_READ_TB_BARCODE").val();
            }
        }
        if (IsCheck == true) {
            if (Label2 == "RH") {
                let C05_START_VLA2 = $("#C05_START_VLA2").val();
                if (C05_START_VLA2 == "") {
                    IsCheck = false;
                    M.toast({ html: "NOT ERROR PROOF. Please Check Again.", classes: 'red', displayLength: 6000 });
                    $("#C05_DC_READ_TB_BARCODE").val();
                }
            }
        }
        if (IsCheck == true) {
            C05_DC_READ_COUNT_BL = false;
            //for (let i = 0; i < BaseResult.C05_DC_READ_DataGridView1.length; i++) {
            //    if (i == 0) {
            //        $("#C05_START_TB_BARCODE").val(BaseResult.C05_DC_READ_DataGridView1[i].Barcode);
            //        C05_START_DC_STR = "DC";
            //        C05_START_BARCODE_LOAD();
            //    }
            //    if (C05_DC_READ_COUNT_BL == true) {
            //        if (i > 0) {
            //            C05_DC_READ_BARCODE_TEXT = BaseResult.C05_DC_READ_DataGridView1[i].Barcode;
            //            C05_DC_READ_BARCODE_LOAD2();
            //        }
            //    }
            //}
            C05_DC_READ_Buttonprint_ClickTimerInterval();
        }
    }
    else {
        M.toast({ html: "Hai hoặc nhiều LEAD KHÔNG cần đăng ký. Please Check Again.", classes: 'red', displayLength: 6000 });
    }
}
let C05_DC_READ_Buttonprint_ClickCounter = -1;
let C05_START_BARCODE_LOADEnd = true;
let C05_DC_READ_Buttonprint_ClickTimer;
function C05_DC_READ_Buttonprint_ClickTimerInterval() {
    C05_DC_READ_Buttonprint_ClickTimer = setInterval(function () {
        if (C05_START_BARCODE_LOADEnd == true) {
            C05_START_BARCODE_LOADEnd = false;
            C05_DC_READ_Buttonprint_ClickCounter = C05_DC_READ_Buttonprint_ClickCounter + 1;
            if (C05_DC_READ_Buttonprint_ClickCounter > BaseResult.C05_DC_READ_DataGridView1.length - 1) {
                C05_DC_READ_Buttonprint_ClickCounter = BaseResult.C05_DC_READ_DataGridView1.length - 1;
            }
            if (C05_DC_READ_Buttonprint_ClickCounter == 0) {
                $("#C05_START_TB_BARCODE").val(BaseResult.C05_DC_READ_DataGridView1[C05_DC_READ_Buttonprint_ClickCounter].Barcode);
                C05_START_TB_BARCODE_Text = $("#C05_START_TB_BARCODE").val();
                C05_START_DC_STR = "DC";
                C05_START_BARCODE_LOAD();
            }
            if (C05_DC_READ_COUNT_BL == true) {
                if (C05_DC_READ_Buttonprint_ClickCounter > 0) {
                    C05_DC_READ_BARCODE_TEXT = BaseResult.C05_DC_READ_DataGridView1[C05_DC_READ_Buttonprint_ClickCounter].Barcode;
                    C05_DC_READ_BARCODE_LOAD2();
                }
            }
            if (C05_DC_READ_Buttonprint_ClickCounter == BaseResult.C05_DC_READ_DataGridView1.length - 1) {
                C05_DC_READ_Buttonprint_ClickCounter = -1;
                clearInterval(C05_DC_READ_Buttonprint_ClickTimer);
                $("#C05_DC_READ").modal("close");
            }
        }
    }, 100);
}
function C05_DC_READ_BARCODE_LOAD2() {
    let IsCheck = true;
    let Label2 = $("#C05_DC_READ_Label2").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C05_DC_READ_BARCODE_TEXT);
    BaseParameter.ListSearchString.push(Label2);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_DC_READ/BARCODE_LOAD2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C05_DC_READ_DataGridView1 = data.DataGridView1;
            BaseResult.C05_DC_READ_DataGridView2 = data.DataGridView2;
            if (BaseResult.C05_DC_READ_DataGridView1.length <= 0) {
                IsCheck = false;
                M.toast({ html: "NOT Order Barcode(Wrong). Please Check Again.", classes: 'red', displayLength: 6000 });
                $("#C05_DC_READ_TB_BARCODE").val("");
                $("#C05_DC_READ_TB_BARCODE").focus();
            }
            if (IsCheck == true) {
                if (BaseResult.C05_DC_READ_DataGridView1[0].DSCN_YN == "Y") {
                    IsCheck = false;
                    M.toast({ html: "Đã xử lý Barcode trước đó. Đã xử lý Barcode trước đó", classes: 'red', displayLength: 6000 });
                    $("#C05_DC_READ_TB_BARCODE").val("");
                    $("#C05_DC_READ_TB_BARCODE").focus();
                }
            }
            if (IsCheck == true) {
                if (Label2 == "LH") {

                }
                if (Label2 == "RH") {
                    let COM_COUNT = BaseResult.C05_DC_READ_DataGridView2.length;
                    if (COM_COUNT == 0) {
                        $("#C05_DC_READ").modal("close");
                    }
                }
            }
            C05_START_BARCODE_LOADEnd = true;
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_DC_READ_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C05_DC_READ_DataGridView1) {
            if (BaseResult.C05_DC_READ_DataGridView1.length > 0) {
                C05_DC_READ_DataGridView1_SelectionChanged(0);
                let RowCount = BaseResult.C05_DC_READ_DataGridView1.length;
                for (let i = 0; i < RowCount; i++) {
                    HTML = HTML + "<tr onclick='C05_DC_READ_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C05_DC_READ_DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C05_DC_READ_DataGridView1[i].ORDER_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C05_DC_READ_DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C05_DC_READ_DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C05_DC_READ_DataGridView1[i].Barcode + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C05_DC_READ_DataGridView1").innerHTML = HTML;
}

function C05_DC_READ_DataGridView1_SelectionChanged(i) {
    C05_DC_READ_DataGridView1RowIndex = i;
}
let C05_DC_READ_DataGridView1Table = document.getElementById("C05_DC_READ_DataGridView1Table");
C05_DC_READ_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C05_DC_READ_DataGridView1, key, text, IsTableSort);
        C05_DC_READ_DataGridView1Render();
    }
});
let C05_SPC_OR_IDX;
let C05_SPC_Flag = 0;
let C05_SPC_IsShow = true;
let C05_SPC_IsShowMiddle = false;
let C05_SPC_IsShowEnd = false;
function C05_SPC_PageLoad() {
    $("#C05_SPC_MRUS").val("-");

    C05_SPC_OR_IDX = Number($("#C05_START_Label8").val());
    C05_START_SPC_EXIT = false;
    document.getElementById("C05_SPC_TextBox1").disabled = true;
    document.getElementById("C05_SPC_TextBox2").disabled = true;
    document.getElementById("C05_SPC_TextBox3").disabled = true;
    document.getElementById("C05_SPC_TextBox4").disabled = true;
    document.getElementById("C05_SPC_TextBox5").disabled = true;
    document.getElementById("C05_SPC_TextBox6").disabled = true;
    document.getElementById("C05_SPC_TextBox7").disabled = true;
    document.getElementById("C05_SPC_TextBox8").disabled = true;

    $("#C05_SPC_ST11").val($("#C05_START_Label24").val());
    $("#C05_SPC_ST12").val($("#C05_START_Label28").val());
    $("#C05_SPC_ST21").val($("#C05_START_Label26").val());
    $("#C05_SPC_ST22").val($("#C05_START_Label30").val());
    $("#C05_SPC_Label8").val($("#C05_START_VLW").val());
    $("#C05_SPC_Label18").val($("#C05_START_WIRE_Length").val());
    $("#C05_SPC_Label19").val($("#C05_START_ST_DWIRE1").val().replace(",", "."));

    C05_SPC_ORDER_CHG();
    C05_SPC_C02_SPC_Load();

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#C05_SPC_ST11").val());
    BBBB.push($("#C05_SPC_ST12").val());
    BBBB.push($("#C05_SPC_ST21").val());
    BBBB.push($("#C05_SPC_ST22").val());


    for (let i = 0; i < 4; i++) {
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

    //let OO = 0;
    //for (let i = 0; i < 4; i++) {
    //    let AA = BBBB[i];
    //    try {
    //        let BB = AA.split("/")[0];
    //        BB = BB.trim();
    //        if (BB.length > 0) {
    //            let CC = BB.split("±")[0];
    //            let CCC = "";
    //            let DD = BB.split("±")[1].replace(/[^0-9.]/g, "");
    //            try {
    //                CCC = CC.split("(")[0].replace(/[^0-9.]/g, "");
    //            }
    //            catch (e) {
    //                CCC = CC;
    //            }
    //            OO = OO + 1;
    //            CCCC.push(CCC);
    //            let CCCDD = Number(CCC) - Number(DD);
    //            LLLL.push(CCCDD.toFixed(2));
    //            CCCDD = Number(CCC) + Number(DD);
    //            MMMM.push(CCCDD.toFixed(2));
    //        }
    //        else {
    //            OO = OO + 1;
    //            CCCC.push("");
    //            LLLL.push("");
    //            MMMM.push("");
    //        }



    //        let BB1 = AA.split("/")[1];
    //        BB1 = BB1.trim();
    //        if (BB1.length > 0) {
    //            let CC1 = BB1.split("±")[0];
    //            let CCC1 = "";
    //            let DD1 = BB1.split("±")[1].replace(/[^0-9.]/g, "");
    //            try {
    //                CCC1 = CC1.split("(")[0].replace(/[^0-9.]/g, "");;
    //            }
    //            catch (e) {
    //                CCC1 = CC1;
    //            }
    //            OO = OO + 1
    //            CCCC.push(CCC1);
    //            let CCC1DD1 = Number(CCC1) - Number(DD1);
    //            LLLL.push(CCC1DD1.toFixed(2));
    //            CCC1DD1 = Number(CCC1) + Number(DD1);
    //            MMMM.push(CCC1DD1.toFixed(2));
    //        }
    //        else {
    //            OO = OO + 1;
    //            CCCC.push("");
    //            LLLL.push("");
    //            MMMM.push("");
    //        }
    //    }
    //    catch (e) {
    //        OO = OO + 1;
    //        CCCC.push("");
    //        LLLL.push("");
    //        MMMM.push("");

    //        OO = OO + 1;
    //        CCCC.push("");
    //        LLLL.push("");
    //        MMMM.push("");
    //    }
    //}


    $("#C05_SPC_STS1").val(CCCC[0]);
    $("#C05_SPC_STS2").val(CCCC[1]);
    $("#C05_SPC_STS3").val(CCCC[2]);
    $("#C05_SPC_STS4").val(CCCC[3]);
    $("#C05_SPC_STS5").val(CCCC[4]);
    $("#C05_SPC_STS6").val(CCCC[5]);
    $("#C05_SPC_STS7").val(CCCC[6]);
    $("#C05_SPC_STS8").val(CCCC[7]);

    if ($("#C05_SPC_STS1").val() == "NaN") {
        $("#C05_SPC_STS1").val() = "";
    }
    if ($("#C05_SPC_STS2").val() == "NaN") {
        $("#C05_SPC_STS2").val() = "";
    }
    if ($("#C05_SPC_STS3").val() == "NaN") {
        $("#C05_SPC_STS3").val() = "";
    }
    if ($("#C05_SPC_STS4").val() == "NaN") {
        $("#C05_SPC_STS4").val() = "";
    }
    if ($("#C05_SPC_STS5").val() == "NaN") {
        $("#C05_SPC_STS5").val() = "";
    }
    if ($("#C05_SPC_STS6").val() == "NaN") {
        $("#C05_SPC_STS6").val() = "";
    }
    if ($("#C05_SPC_STS7").val() == "NaN") {
        $("#C05_SPC_STS7").val() = "";
    }
    if ($("#C05_SPC_STS8").val() == "NaN") {
        $("#C05_SPC_STS8").val() = "";
    }

    $("#C05_SPC_STL1").val(LLLL[0]);
    $("#C05_SPC_STL2").val(LLLL[1]);
    $("#C05_SPC_STL3").val(LLLL[2]);
    $("#C05_SPC_STL4").val(LLLL[3]);
    $("#C05_SPC_STL5").val(LLLL[4]);
    $("#C05_SPC_STL6").val(LLLL[5]);
    $("#C05_SPC_STL7").val(LLLL[6]);
    $("#C05_SPC_STL8").val(LLLL[7]);

    if ($("#C05_SPC_STL1").val() == "NaN") {
        $("#C05_SPC_STL1").val() = "";
    }
    if ($("#C05_SPC_STL2").val() == "NaN") {
        $("#C05_SPC_STL2").val() = "";
    }

    if ($("#C05_SPC_STL3").val() == "NaN") {
        $("#C05_SPC_STL3").val() = "";
    }
    if ($("#C05_SPC_STL4").val() == "NaN") {
        $("#C05_SPC_STL4").val() = "";
    }

    if ($("#C05_SPC_STL5").val() == "NaN") {
        $("#C05_SPC_STL5").val() = "";
    }
    if ($("#C05_SPC_STL6").val() == "NaN") {
        $("#C05_SPC_STL6").val() = "";
    }
    if ($("#C05_SPC_STL7").val() == "NaN") {
        $("#C05_SPC_STL7").val() = "";
    }
    if ($("#C05_SPC_STL8").val() == "NaN") {
        $("#C05_SPC_STL8").val() = "";
    }

    $("#C05_SPC_STM1").val(MMMM[0]);
    $("#C05_SPC_STM2").val(MMMM[1]);
    $("#C05_SPC_STM3").val(MMMM[2]);
    $("#C05_SPC_STM4").val(MMMM[3]);
    $("#C05_SPC_STM5").val(MMMM[4]);
    $("#C05_SPC_STM6").val(MMMM[5]);
    $("#C05_SPC_STM7").val(MMMM[6]);
    $("#C05_SPC_STM8").val(MMMM[7]);

    if ($("#C05_SPC_STM1").val() == "NaN") {
        $("#C05_SPC_STM1").val() = "";
    }
    if ($("#C05_SPC_STM2").val() == "NaN") {
        $("#C05_SPC_STM2").val() = "";
    }
    if ($("#C05_SPC_STM3").val() == "NaN") {
        $("#C05_SPC_STM3").val() = "";
    }
    if ($("#C05_SPC_STM4").val() == "NaN") {
        $("#C05_SPC_STM4").val() = "";
    }
    if ($("#C05_SPC_STM5").val() == "NaN") {
        $("#C05_SPC_STM5").val() = "";
    }
    if ($("#C05_SPC_STM6").val() == "NaN") {
        $("#C05_SPC_STM6").val() = "";
    }
    if ($("#C05_SPC_STM7").val() == "NaN") {
        $("#C05_SPC_STM7").val() = "";
    }
    if ($("#C05_SPC_STM8").val() == "NaN") {
        $("#C05_SPC_STM8").val() = "";
    }

    if ($("#C05_SPC_STS1").val() == "") {
        document.getElementById("C05_SPC_TextBox1").disabled = true;
    }
    if ($("#C05_SPC_STS2").val() == "") {
        document.getElementById("C05_SPC_TextBox2").disabled = true;
    }
    if ($("#C05_SPC_STS3").val() == "") {
        document.getElementById("C05_SPC_TextBox3").disabled = true;
    }
    if ($("#C05_SPC_STS4").val() == "") {
        document.getElementById("C05_SPC_TextBox4").disabled = true;
    }
    if ($("#C05_SPC_STS5").val() == "") {
        document.getElementById("C05_SPC_TextBox5").disabled = true;
    }
    if ($("#C05_SPC_STS6").val() == "") {
        document.getElementById("C05_SPC_TextBox6").disabled = true;
    }
    if ($("#C05_SPC_STS7").val() == "") {
        document.getElementById("C05_SPC_TextBox7").disabled = true;
    }
    if ($("#C05_SPC_STS8").val() == "") {
        document.getElementById("C05_SPC_TextBox8").disabled = true;
    }

    $("#C05_SPC_Label4").val($("#C05_START_VLT1").val());
    $("#C05_SPC_Label5").val($("#C05_START_VLT1").val());
    $("#C05_SPC_Label6").val($("#C05_START_VLT2").val());
    $("#C05_SPC_Label7").val($("#C05_START_VLT2").val());

    let Label4 = $("#C05_SPC_Label4").val();
    let Label6 = $("#C05_SPC_Label6").val();

    let BBB = Label4.includes("(");
    let DDD = Label6.includes("(");

    if (BBB == true) {
        document.getElementById("C05_SPC_TextBox1").disabled = false;
        document.getElementById("C05_SPC_TextBox2").disabled = false;
        document.getElementById("C05_SPC_TextBox3").disabled = false;
        document.getElementById("C05_SPC_TextBox4").disabled = false;
    }

    if (DDD == true) {
        document.getElementById("C05_SPC_TextBox5").disabled = false;
        document.getElementById("C05_SPC_TextBox6").disabled = false;
        document.getElementById("C05_SPC_TextBox7").disabled = false;
        document.getElementById("C05_SPC_TextBox8").disabled = false;
    }

    $("#C05_SPC_TextBox1").val("");
    $("#C05_SPC_TextBox2").val("");
    $("#C05_SPC_TextBox3").val("");
    $("#C05_SPC_TextBox4").val("");
    $("#C05_SPC_TextBox5").val("");
    $("#C05_SPC_TextBox6").val("");
    $("#C05_SPC_TextBox7").val("");
    $("#C05_SPC_TextBox8").val("");
    $("#C05_SPC_TextBox10").val("");

    $("#C05_SPC_LR1").val("");
    $("#C05_SPC_LR2").val("");
    $("#C05_SPC_LR3").val("");
    $("#C05_SPC_LR4").val("");
    $("#C05_SPC_LR5").val("");
    $("#C05_SPC_LR6").val("");
    $("#C05_SPC_LR7").val("");
    $("#C05_SPC_LR8").val("");
    $("#C05_SPC_LR9").val("");

    document.getElementById("C05_SPC_MRUS").style.backgroundColor = "white";

    document.getElementById("C05_SPC_LR1").style.backgroundColor = "white";
    document.getElementById("C05_SPC_LR2").style.backgroundColor = "white";
    document.getElementById("C05_SPC_LR3").style.backgroundColor = "whitesmoke";
    document.getElementById("C05_SPC_LR4").style.backgroundColor = "whitesmoke";
    document.getElementById("C05_SPC_LR5").style.backgroundColor = "white";
    document.getElementById("C05_SPC_LR6").style.backgroundColor = "white";
    document.getElementById("C05_SPC_LR7").style.backgroundColor = "whitesmoke";
    document.getElementById("C05_SPC_LR8").style.backgroundColor = "whitesmoke";
    document.getElementById("C05_SPC_LR9").style.backgroundColor = "white";



    if ((BBB == false) && (DDD == false)) {
        C05_SPC_Buttonclose_Click();
    }

    if ($("#C05_SPC_STL1").val() == "") {
        document.getElementById("C05_SPC_TextBox1").disabled = true;
    }
    if ($("#C05_SPC_STL2").val() == "") {
        document.getElementById("C05_SPC_TextBox2").disabled = true;
    }
    if ($("#C05_SPC_STL3").val() == "") {
        document.getElementById("C05_SPC_TextBox3").disabled = true;
    }
    if ($("#C05_SPC_STL4").val() == "") {
        document.getElementById("C05_SPC_TextBox4").disabled = true;
    }
    if ($("#C05_SPC_STL5").val() == "") {
        document.getElementById("C05_SPC_TextBox5").disabled = true;
    }
    if ($("#C05_SPC_STL6").val() == "") {
        document.getElementById("C05_SPC_TextBox6").disabled = true;
    }
    if ($("#C05_SPC_STL7").val() == "") {
        document.getElementById("C05_SPC_TextBox7").disabled = true;
    }
    if ($("#C05_SPC_STL8").val() == "") {
        document.getElementById("C05_SPC_TextBox8").disabled = true;
    }
    if ($("#C05_SPC_Label11").val() == "L") {
        document.getElementById("C05_SPC_TextBox5").disabled = true;
        document.getElementById("C05_SPC_TextBox6").disabled = true;
        document.getElementById("C05_SPC_TextBox7").disabled = true;
        document.getElementById("C05_SPC_TextBox8").disabled = true;
        $("#C05_SPC_TextBox1").focus();
    }
    if ($("#C05_SPC_Label11").val() == "R") {
        document.getElementById("C05_SPC_TextBox1").disabled = true;
        document.getElementById("C05_SPC_TextBox2").disabled = true;
        document.getElementById("C05_SPC_TextBox3").disabled = true;
        document.getElementById("C05_SPC_TextBox4").disabled = true;
        $("#C05_SPC_TextBox5").focus();
    }
}
function C05_SPC_C02_SPC_Load() {
    let F_CO = $("#C05_SPC_Label19").val();
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_SPC/C02_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C05_SPC_Search = data.Search;
            if (BaseResult.C05_SPC_Search.length == 0) {
                $("#C05_SPC_Label17").val(0);
            }
            else {
                $("#C05_SPC_Label17").val(BaseResult.C05_SPC_Search[0].STRENGTH);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_SPC_ORDER_CHG() {
    ////$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C05_SPC_OR_IDX,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_SPC/ORDER_CHG";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C05_SPC_Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("C05_SPC_TextBox1").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox2").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox3").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox4").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox5").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox6").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox7").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox8").disabled);
    CHK.push(document.getElementById("C05_SPC_TextBox10").disabled);

    let AA = [];
    AA.push($("#C05_SPC_LR1").val());
    AA.push($("#C05_SPC_LR2").val());
    AA.push($("#C05_SPC_LR3").val());
    AA.push($("#C05_SPC_LR4").val());
    AA.push($("#C05_SPC_LR5").val());
    AA.push($("#C05_SPC_LR6").val());
    AA.push($("#C05_SPC_LR7").val());
    AA.push($("#C05_SPC_LR8").val());
    AA.push($("#C05_SPC_LR9").val());


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
    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true) && (VAL[5] == true) && (VAL[6] == true) && (VAL[7] == true) && (VAL[8] == true)) {
        $("#C05_SPC_MRUS").val("OK");
        document.getElementById("C05_SPC_MRUS").style.color = "green";
    }
    else {
        $("#C05_SPC_MRUS").val("NG");
        document.getElementById("C05_SPC_MRUS").style.color = "red";
    }
}
$("#C05_SPC_Button1").click(function (e) {
    C05_SPC_Button1_Click();
});
function C05_SPC_Button1_Click() {
    let IsCheck = true;
    let TextBox1 = $("#C05_SPC_TextBox1").val();
    let TextBox2 = $("#C05_SPC_TextBox2").val();
    let TextBox3 = $("#C05_SPC_TextBox3").val();
    let TextBox4 = $("#C05_SPC_TextBox4").val();
    let TextBox5 = $("#C05_SPC_TextBox5").val();
    let TextBox6 = $("#C05_SPC_TextBox6").val();
    let TextBox7 = $("#C05_SPC_TextBox7").val();
    let TextBox8 = $("#C05_SPC_TextBox8").val();
    let TextBox10 = $("#C05_SPC_TextBox10").val();

    let Label8 = $("#C05_START_Label8").val();
    let Label10 = $("#C05_SPC_Label10").val();
    let Label11 = $("#C05_SPC_Label11").val();

    let MRUS = $("#C05_SPC_MRUS").val();

    let MC2 = $("#MCBOX").val();
    let STRENGTH = $("#C05_SPC_Label17").val();

    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            ////$("#BackGround").css("display", "block");
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
            BaseParameter.ListSearchString.push(TextBox5);
            BaseParameter.ListSearchString.push(TextBox6);
            BaseParameter.ListSearchString.push(TextBox7);
            BaseParameter.ListSearchString.push(TextBox8);
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label8);
            BaseParameter.ListSearchString.push(Label10);
            BaseParameter.ListSearchString.push(Label11);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(MC2);
            BaseParameter.ListSearchString.push(STRENGTH);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C05_SPC/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    C05_START_SPC_EXIT = true;
                    C05_SPC_Buttonclose_Click();
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
$("#C05_SPC_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox1_KeyDown();
    }
});
function C05_SPC_TextBox1_KeyDown() {
    $("#C05_SPC_TextBox2").focus();
}
$("#C05_SPC_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox2_KeyDown();
    }
});
function C05_SPC_TextBox2_KeyDown() {
    $("#C05_SPC_TextBox3").focus();
}
$("#C05_SPC_TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox3_KeyDown();
    }
});
function C05_SPC_TextBox3_KeyDown() {
    $("#C05_SPC_TextBox4").focus();
}
$("#C05_SPC_TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox4_KeyDown();
    }
});
function C05_SPC_TextBox4_KeyDown() {
    let Label11 = $("#C05_SPC_Label11").val();
    if (Label11 == "L") {
        $("#C05_SPC_TextBox10").focus();
    }
    else {
        $("#C05_SPC_TextBox5").focus();
    }
}
$("#C05_SPC_TextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox5_KeyDown();
    }
});
function C05_SPC_TextBox5_KeyDown() {
    $("#C05_SPC_TextBox6").focus();
}
$("#C05_SPC_TextBox6").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox6_KeyDown();
    }
});
function C05_SPC_TextBox6_KeyDown() {
    $("#C05_SPC_TextBox7").focus();
}
$("#C05_SPC_TextBox7").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox7_KeyDown();
    }
});
function C05_SPC_TextBox7_KeyDown() {
    $("#C05_SPC_TextBox8").focus();
}
$("#C05_SPC_TextBox8").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox8_KeyDown();
    }
});
function C05_SPC_TextBox8_KeyDown() {
    $("#C05_SPC_TextBox10").focus();
}
$("#C05_SPC_TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_SPC_TextBox10_KeyDown();
    }
});
function C05_SPC_TextBox10_KeyDown() {
    $("#C05_SPC_TextBox1").focus();
}
$("#C05_SPC_TextBox1").change(function (e) {
    C05_SPC_TextBox1_TextChanged()
});
function C05_SPC_TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#C05_SPC_TextBox1").val();
        let STL1 = $("#C05_SPC_STL1").val();
        let STM1 = $("#C05_SPC_STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#C05_SPC_LR1").val("OK");
            document.getElementById("C05_SPC_LR1").style.color = "green";
        }
        else {
            $("#C05_SPC_LR1").val("NG");
            document.getElementById("C05_SPC_LR1").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox2").change(function (e) {
    C05_SPC_TextBox2_TextChanged()
});
function C05_SPC_TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#C05_SPC_TextBox2").val();
        let STL2 = $("#C05_SPC_STL2").val();
        let STM2 = $("#C05_SPC_STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#C05_SPC_LR2").val("OK");
            document.getElementById("C05_SPC_LR2").style.color = "green";
        }
        else {
            $("#C05_SPC_LR2").val("NG");
            document.getElementById("C05_SPC_LR2").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox3").change(function (e) {
    C05_SPC_TextBox3_TextChanged()
});
function C05_SPC_TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#C05_SPC_TextBox3").val();
        let STL3 = $("#C05_SPC_STL3").val();
        let STM3 = $("#C05_SPC_STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#C05_SPC_LR3").val("OK");
            document.getElementById("C05_SPC_LR3").style.color = "green";
        }
        else {
            $("#C05_SPC_LR3").val("NG");
            document.getElementById("C05_SPC_LR3").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox4").change(function (e) {
    C05_SPC_TextBox4_TextChanged()
});
function C05_SPC_TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#C05_SPC_TextBox4").val();
        let STL4 = $("#C05_SPC_STL4").val();
        let STM4 = $("#C05_SPC_STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#C05_SPC_LR4").val("OK");
            document.getElementById("C05_SPC_LR4").style.color = "green";
        }
        else {
            $("#C05_SPC_LR4").val("NG");
            document.getElementById("C05_SPC_LR4").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox5").change(function (e) {
    C05_SPC_TextBox5_TextChanged()
});
function C05_SPC_TextBox5_TextChanged() {
    try {
        let TextBox5 = $("#C05_SPC_TextBox5").val();
        let STL5 = $("#C05_SPC_STL5").val();
        let STM5 = $("#C05_SPC_STM5").val();
        if (SPCValuesCheckValue(TextBox5, STL5, STM5)) {
            $("#C05_SPC_LR5").val("OK");
            document.getElementById("C05_SPC_LR5").style.color = "green";
        }
        else {
            $("#C05_SPC_LR5").val("NG");
            document.getElementById("C05_SPC_LR5").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox6").change(function (e) {
    C05_SPC_TextBox6_TextChanged()
});
function C05_SPC_TextBox6_TextChanged() {
    try {
        let TextBox6 = $("#C05_SPC_TextBox6").val();
        let STL6 = $("#C05_SPC_STL6").val();
        let STM6 = $("#C05_SPC_STM6").val();
        if (SPCValuesCheckValue(TextBox6, STL6, STM6)) {
            $("#C05_SPC_LR6").val("OK");
            document.getElementById("C05_SPC_LR6").style.color = "green";
        }
        else {
            $("#C05_SPC_LR6").val("NG");
            document.getElementById("C05_SPC_LR6").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox7").change(function (e) {
    C05_SPC_TextBox7_TextChanged()
});
function C05_SPC_TextBox7_TextChanged() {
    try {
        let TextBox7 = $("#C05_SPC_TextBox7").val();
        let STL7 = $("#C05_SPC_STL7").val();
        let STM7 = $("#C05_SPC_STM7").val();
        if (SPCValuesCheckValue(TextBox7, STL7, STM7)) {
            $("#C05_SPC_LR7").val("OK");
            document.getElementById("C05_SPC_LR7").style.color = "green";
        }
        else {
            $("#C05_SPC_LR7").val("NG");
            document.getElementById("C05_SPC_LR7").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox8").change(function (e) {
    C05_SPC_TextBox8_TextChanged()
});
function C05_SPC_TextBox8_TextChanged() {
    try {
        let TextBox8 = $("#C05_SPC_TextBox8").val();
        let STL8 = $("#C05_SPC_STL8").val();
        let STM8 = $("#C05_SPC_STM8").val();
        if (SPCValuesCheckValue(TextBox8, STL8, STM8)) {
            $("#C05_SPC_LR8").val("OK");
            document.getElementById("C05_SPC_LR8").style.color = "green";
        }
        else {
            $("#C05_SPC_LR8").val("NG");
            document.getElementById("C05_SPC_LR8").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
$("#C05_SPC_TextBox10").change(function (e) {
    C05_SPC_TextBox10_TextChanged()
});
function C05_SPC_TextBox10_TextChanged() {
    try {
        let AA = Number($("#C05_SPC_TextBox10").val());
        let CC = Number($("#C05_SPC_Label17").val());

        if (CC <= AA) {
            $("#C05_SPC_LR9").val("OK");
            document.getElementById("C05_SPC_LR9").style.color = "green";
        }
        else {
            $("#C05_SPC_LR9").val("NG");
            document.getElementById("C05_SPC_LR9").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C05_SPC_Form_RUS();
}
function C05_SPC_Buttonclose_Click() {
    $("#C05_SPC").modal("close");
    C05_SPC_FormClosed();
}

function C05_SPC_FormClosed() {
    if (C05_SPC_Flag == 0) {
        C05_START_SPC_LOAD();
    }
    if (C05_SPC_Flag == 1) {
        if (C05_START_SPC_EXIT == true) {
            clearInterval(C05_START_Timer4);
            C05_START_SPC_LOAD();
        }
    }

    if (C05_SPC_Flag == 2) {
        C05_SPC_IsShow = true;
        if (C05_SPC_IsShow == true) {
            if (C05_SPC_IsShowMiddle == false) {
                let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
                let PERFORMN_L = BaseResult.C05_START_Search[0].PERFORMN_L;
                if (TOT_QTY > 500) {
                    if (Math.floor(TOT_QTY / 2) <= PERFORMN_L) {
                        let SPC2 = document.getElementById("C05_START_SPC2").innerHTML;
                        if (SPC2 == "Middle") {
                            C05_SPC_IsShow = false;
                            C05_SPC_IsShowMiddle = true;
                            C05_SPC_Flag = 2;
                            $("#C05_SPC_Label10").val("Middle");
                            $("#C05_SPC_Label11").val("L");
                            $("#C05_SPC").modal("open");
                            C05_SPC_PageLoad();
                        }
                    }
                }
            }
        }
        if (C05_SPC_IsShow == true) {
            if (C05_SPC_IsShowEnd == false) {
                let A_CSPC = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN_L;
                let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
                if (BUNDLE_SIZE >= A_CSPC) {
                    let SPC3 = document.getElementById("C05_START_SPC3").innerHTML;
                    if (SPC3 == "End") {
                        C05_SPC_IsShow = false;
                        C05_SPC_IsShowEnd = true;
                        C05_SPC_Flag = 2;
                        $("#C05_SPC_Label10").val("End");
                        $("#C05_SPC_Label11").val("L");
                        $("#C05_SPC").modal("open");
                        C05_SPC_PageLoad();
                    }
                }
            }
        }
        if (C05_SPC_IsShow == true) {
            C05_SPC_IsShowMiddle = false;
            C05_SPC_IsShowEnd = false;
            let IsCheck = true;
            if (C05_START_SPC_EXIT == false) {
                IsCheck = false;
                alert("검사 누락. Kiểm tra mất tích.");
                document.getElementById("C05_START_Buttonprint").disabled = false;
            }
            if (IsCheck == true) {
                try {
                    let Tcount = $("#C05_START_TB_BARCODE").val();
                    Tcount = C05_START_TB_BARCODE_Text;
                    let VLA1 = $("#C05_START_VLA1").val();
                    let TOOL1_IDX = $("#C05_START_TOOL1_IDX").val();
                    let Label4 = $("#C05_START_Label4").val();
                    let Label8 = $("#C05_START_Label8").val();
                    let VLT1 = $("#C05_START_VLT1").val();
                    let MCBOX = $("#MCBOX").val();

                    //$("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.Search = BaseResult.C05_START_Search;
                    BaseParameter.ListSearchString.push(Tcount);
                    BaseParameter.ListSearchString.push(VLA1);
                    BaseParameter.ListSearchString.push(TOOL1_IDX);
                    BaseParameter.ListSearchString.push(Label4);
                    BaseParameter.ListSearchString.push(Label8);
                    BaseParameter.ListSearchString.push(MCBOX);
                    BaseParameter.ListSearchString.push(VLT1);
                    BaseParameter.ListSearchString.push(C05_START_DC_STR);
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
                            if (IsCheck == true) {
                                C05_START_BARCODE_LOADSub003();
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
                    document.getElementById("C05_START_Buttonprint").disabled = false;
                }
            }
        }
    }
    if (C05_SPC_Flag == 3) {
        C05_SPC_IsShow = true;
        if (C05_SPC_IsShow == true) {
            if (C05_SPC_IsShowMiddle == false) {
                let TOT_QTY = BaseResult.C05_START_Search[0].TOT_QTY;
                let PERFORMN_L = BaseResult.C05_START_Search[0].PERFORMN_L;
                if (TOT_QTY > 500) {
                    if (Math.floor(TOT_QTY / 2) <= PERFORMN_L) {
                        let SPC2 = document.getElementById("C05_START_SPC2").innerHTML;
                        if (SPC2 == "Middle") {
                            C05_SPC_IsShow = false;
                            C05_SPC_IsShowMiddle = true;
                            C05_SPC_Flag = 3;
                            $("#C05_SPC_Label10").val("Middle");
                            $("#C05_SPC_Label11").val("R");
                            $("#C05_SPC").modal("open");
                            C05_SPC_PageLoad();
                        }
                    }
                }
            }
        }
        if (C05_SPC_IsShow == true) {
            if (C05_SPC_IsShowEnd == false) {
                let A_CSPC = BaseResult.C05_START_Search[0].TOT_QTY - BaseResult.C05_START_Search[0].PERFORMN_L;
                let BUNDLE_SIZE = BaseResult.C05_START_Search[0].BUNDLE_SIZE;
                if (BUNDLE_SIZE >= A_CSPC) {
                    let SPC3 = document.getElementById("C05_START_SPC3").innerHTML;
                    if (SPC3 == "End") {
                        C05_SPC_IsShow = false;
                        C05_SPC_IsShowEnd = true;
                        C05_SPC_Flag = 3;
                        $("#C05_SPC_Label10").val("End");
                        $("#C05_SPC_Label11").val("R");
                        $("#C05_SPC").modal("open");
                        C05_SPC_PageLoad();
                    }
                }
            }
        }
        if (C05_SPC_IsShow == true) {
            C05_SPC_IsShowMiddle = false;
            C05_SPC_IsShowEnd = false;
            let IsCheck = true;
            if (C05_START_SPC_EXIT == false) {
                IsCheck = false;
                alert("검사 누락. Kiểm tra mất tích.");
                document.getElementById("C05_START_Buttonprint").disabled = false;
            }
            if (IsCheck == true) {
                try {
                    let Tcount = $("#C05_START_TB_BARCODE").val();
                    Tcount = C05_START_TB_BARCODE_Text;
                    let VLA2 = $("#C05_START_VLA2").val();
                    let TOOL2_IDX = $("#C05_START_TOOL2_IDX").val();
                    let Label4 = $("#C05_START_Label4").val();
                    let Label8 = $("#C05_START_Label8").val();
                    let VLT2 = $("#C05_START_VLT2").val();
                    let MCBOX = $("#MCBOX").val();
                    //$("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.Search = BaseResult.C05_START_Search;
                    BaseParameter.ListSearchString.push(Tcount);
                    BaseParameter.ListSearchString.push(VLA2);
                    BaseParameter.ListSearchString.push(TOOL2_IDX);
                    BaseParameter.ListSearchString.push(Label4);
                    BaseParameter.ListSearchString.push(Label8);
                    BaseParameter.ListSearchString.push(MCBOX);
                    BaseParameter.ListSearchString.push(VLT2);
                    BaseParameter.ListSearchString.push(C05_START_DC_STR);
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
                            let COM_COUNT = data.DataGridView5.length;
                            if (COM_COUNT == 0) {
                                IsCheck = false;
                                C05_START_Buttonclose_Click();
                            }
                            if (IsCheck == true) {
                                try {
                                    C05_START_SPC_LOAD();
                                }
                                catch (e) {
                                    M.toast({ html: "#1 Please Check Again.", classes: 'green', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                                try {
                                    C05_START_ORDER_LOAD();
                                }
                                catch (e) {
                                    M.toast({ html: "#2 Please Check Again.", classes: 'green', displayLength: 6000 });
                                    document.getElementById("C05_START_Buttonprint").disabled = false;
                                }
                            }
                            if (IsCheck == true) {
                                C05_START_BARCODE_LOADSub003();
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
                    document.getElementById("C05_START_Buttonprint").disabled = false;
                }
            }

        }
    }
}



let C05_ER_L_StartTime;
let C05_ER_L_RunTime;
let C05_ER_L_LAST_ID;
function C05_ER_L_PageLoad() {
    C05_ER_L_StartTime = new Date();
    $("#C05_ER_L_TextBox1").val("");
    $("#C05_ER_L_LBT2R").val("");
    $("#C05_ER_L_LBA2R").val("");
    $("#C05_ER_L_LBT2V").val("");
    $("#C05_ER_L_LBA2V").val("");
    $("#C05_ER_L_LBA2SEQ").val("");
    $("#C05_ER_L_LBA2IDX").val("");
    $("#C05_ER_L_LBR2").val("");
    $("#C05_ER_L_LBR4").val("");
    $("#C05_ER_L_LBR22").val("");

    document.getElementById("C05_ER_L_CHBT2").checked = true;
    document.getElementById("C05_ER_L_CHBA2").checked = true;

    document.getElementById("C05_ER_L_LBR2").style.backgroundColor = "white";
    document.getElementById("C05_ER_L_LBR4").style.backgroundColor = "white";
    document.getElementById("C05_ER_L_LBR22").style.backgroundColor = "white";

    $("#C05_ER_L_ComboBox1").empty();

    if ($("#C05_ER_L_LBT2S").val() == "") {
        C05_ER_L_Buttonclose_Click();
    }

    var ComboBox1 = document.getElementById("C05_ER_L_ComboBox1");

    var option = document.createElement("option");
    option.text = "TERM";
    option.value = "TERM";
    ComboBox1.add(option);

    option = document.createElement("option");
    option.text = "APPLICATOR";
    option.value = "APPLICATOR";
    ComboBox1.add(option);
    C05_ER_L_C11_ERROR_Load();
}
function C05_ER_L_C11_ERROR_Load() {
    let MCbox = localStorage.getItem("C11_MCbox");
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_ER_L/C11_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C05_ER_L_Search = data.Search;
            C05_ER_L_LAST_ID = BaseResult.C05_ER_L_Search[0].TSNON_OPER_IDX;
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_ER_L_Button1").click(function (e) {
    C05_ER_L_Button1_Click();
});
function C05_ER_L_Button1_Click() {
    let IsCheck = true;
    let AA = $("#C05_ER_L_TextBox1").val();
    AA = AA.toUpperCase();
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    let ComboBox1 = $("#C05_ER_L_ComboBox1").val();
    switch (ComboBox1) {
        case "TERM":
            try {
                let LBT2S = $("#C05_ER_L_LBT2S").val();
                let T2 = "";
                if (AA == BC_MAST) {
                    T2 = LBT2S;
                }
                else {
                    T2 = AA.substr(0, AA.indexOf("$$"));
                }
                if (LBT2S == T2) {
                    $("#C05_ER_L_LBT2R").val("OK");
                    $("#C05_ER_L_LBR2").val("OK");
                    document.getElementById("C05_ER_L_LBR2").style.color = "green";
                    try {
                        document.getElementById("C05_ER_L_ComboBox1").selectedIndex = document.getElementById("C05_ER_L_ComboBox1").selectedIndex + 1;
                    }
                    catch (e) {
                        alert(e);
                    }
                }
                else {
                    $("#C05_ER_L_LBT2R").val("NG");
                    $("#C05_ER_L_LBR2").val("NG");
                    document.getElementById("LBR2").style.color = "red";
                }
                $("#C05_ER_L_LBT2V").val(T2);
                $("#C05_ER_L_TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again. " + e);
            }
            break;
        case "APPLICATOR":
            let A2 = AA.substr(0, AA.length - 1);
            let LBA2S = $("#C05_ER_L_LBA2S").val();
            if (LBA2S == A2) {
                $("#C05_ER_L_LBA2R").val("OK");
                let LBA2SEQ = AA.substr(AA.length - 1, 1);
                $("#C05_ER_L_LBA2SEQ").val(LBA2SEQ);
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
                    LBA2SEQ = $("#C05_ER_L_LBA2SEQ").val();
                    let Label1 = $("#C05_ER_L_Label1").val();
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
                    let url = "/C05_ER_L/Button1_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            BaseResult.C05_ER_L_Search = data.Search;
                            BaseResult.C05_ER_L_Search1 = data.Search1;
                            BaseResult.C05_ER_L_DataGridView = data.DataGridView;
                            if (BaseResult.C05_ER_L_Search.length <= 0) {
                                if (BaseResult.C05_ER_L_Search1.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#C05_ER_L_LBA2R").val("Not data");
                                    $("#C05_ER_L_LBA2V").val(A2);
                                    $("#C05_ER_L_TextBox1").val("");
                                }
                                else {
                                    let WK_CNT = BaseResult.C05_ER_L_DataGridView[0].WK_CNT;
                                    $("#C05_ER_L_LBR22").val(WK_CNT);
                                    document.getElementById("C05_ER_L_LBR22").style.color = "green";
                                }
                            }
                            else {
                                let WK_CNT = BaseResult.C05_ER_L_Search[0].WK_CNT;
                                $("#C05_ER_L_LBR22").val(WK_CNT);
                                document.getElementById("C05_ER_L_LBR22").style.color = "green";
                                let TOOL_IDX = BaseResult.C05_ER_L_Search[0].TOOL_IDX;
                                $("#C05_ER_L_LBA2IDX").val(TOOL_IDX);
                            }
                            if (IsCheck == true) {

                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                $("#C05_ER_L_LBA2V").val(A2);
                $("#C05_ER_L_TextBox1").val("");
            }
            break;
    }
    $("#C05_ER_L_TextBox1").focus();
}
$("#C05_ER_L_Button2").click(function (e) {
    C05_ER_L_Button2_Click();
});
function C05_ER_L_Button2_Click() {
    let AA2 = document.getElementById("C05_ER_L_CHBT2").checked;
    let AA4 = document.getElementById("C05_ER_L_CHBA2").checked;
    let BB2 = false;
    let BB4 = false;
    if (AA2 == true) {
        if ($("#C05_ER_L_LBT2R").val("OK")) {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA4 == true) {
        if ($("#C05_ER_L_LBA2R").val("OK")) {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if ((AA2 == true) && (BB2 == true) && (AA4 == true) && (BB4 == true)) {
        let Label1 = $("#C05_ER_L_Label1").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: Label1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_ER_L/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                C05_START_EXIT = false;
                C05_ER_L_C11_ERROR_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert("Please Check Again.");
        C05_START_EXIT = true;
    }
}
$("#C05_ER_L_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_ER_L_TextBox1_KeyDown();
    }
});
function C05_ER_L_TextBox1_KeyDown() {
    C05_ER_L_Button1_Click();
}
$("#C05_ER_L_Buttonclose").click(function (e) {
    C05_ER_L_Buttonclose_Click();
});
function C05_ER_L_Buttonclose_Click() {
    C05_ER_L_C11_ERROR_FormClosed();
}
function C05_ER_L_C11_ERROR_FormClosed() {
    //let MCbox = localStorage.getItem("C11_MCbox");
    let MCbox = $("#MCBOX").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C05_ER_L_LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_ER_L/C11_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            localStorage.setItem("SettingsNON_OPER_CHK", false);
            $("#C05_ER_L").modal("close");
            C05_START_ORDER_LOAD();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
let C05_ER_R_StartTime;
let C05_ER_R_RunTime;
let C05_ER_R_LAST_ID;
function C05_ER_R_PageLoad() {
    C05_ER_R_StartTime = new Date();
    $("#C05_ER_R_TextBox1").val("");
    $("#C05_ER_R_LBT2R").val("");
    $("#C05_ER_R_LBA2R").val("");
    $("#C05_ER_R_LBT2V").val("");
    $("#C05_ER_R_LBA2V").val("");
    $("#C05_ER_R_LBA2SEQ").val("");
    $("#C05_ER_R_LBA2IDX").val("");
    $("#C05_ER_R_LBR2").val("");
    $("#C05_ER_R_LBR4").val("");
    $("#C05_ER_R_LBR22").val("");

    document.getElementById("C05_ER_R_CHBT2").checked = true;
    document.getElementById("C05_ER_R_CHBA2").checked = true;

    document.getElementById("C05_ER_R_LBR2").style.backgroundColor = "white";
    document.getElementById("C05_ER_R_LBR4").style.backgroundColor = "white";
    document.getElementById("C05_ER_R_LBR22").style.backgroundColor = "white";

    $("#C05_ER_R_ComboBox1").empty();

    if ($("#C05_ER_R_LBT2S").val() == "") {
        C05_ER_R_Buttonclose_Click();
    }
    var ComboBox1 = document.getElementById("C05_ER_R_ComboBox1");

    var option = document.createElement("option");
    option.text = "TERM";
    option.value = "TERM";
    ComboBox1.add(option);

    option = document.createElement("option");
    option.text = "APPLICATOR";
    option.value = "APPLICATOR";
    ComboBox1.add(option);
    C05_ER_R_C11_ERROR_Load();
}
function C05_ER_R_C11_ERROR_Load() {
    let MCbox = $("#MCBOX").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_ER_R/C11_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C05_ER_R_Search = data.Search;
            C05_ER_R_LAST_ID = BaseResult.C05_ER_R_Search[0].TSNON_OPER_IDX;
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C05_ER_R_Button1").click(function (e) {
    C05_ER_R_Button1_Click();
});
function C05_ER_R_Button1_Click() {
    let IsCheck = true;
    let AA = $("#C05_ER_R_TextBox1").val();
    AA = AA.toUpperCase();
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    let ComboBox1 = $("#C05_ER_R_ComboBox1").val();
    switch (ComboBox1) {
        case "TERM":
            try {
                let LBT2S = $("#C05_ER_R_LBT2S").val();
                let T2 = "";
                if (AA == BC_MAST) {
                    T2 = LBT2S;
                }
                else {
                    T2 = AA.substr(0, AA.indexOf("$$"));
                }
                if (LBT2S == T2) {
                    $("#C05_ER_R_LBT2R").val("OK");
                    $("#C05_ER_R_LBR2").val("OK");
                    document.getElementById("C05_ER_R_LBR2").style.backgroundColor = "green";
                    try {
                        document.getElementById("C05_ER_R_ComboBox1").selectedIndex = document.getElementById("C05_ER_R_ComboBox1").selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C05_ER_R_LBT2R").val("NG");
                    $("#C05_ER_R_LBR2").val("NG");
                    document.getElementById("C05_ER_R_LBR2").style.backgroundColor = "red";
                }
                $("#C05_ER_R_LBT2V").val(T2);
                $("#C05_ER_R_TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again. " + e);
            }
            break;
        case "APPLICATOR":
            let A2 = AA.substr(0, AA.length - 1);
            let LBA2S = $("#C05_ER_R_LBA2S").val();
            if (LBA2S == A2) {
                $("#C05_ER_R_LBA2R").val("OK");
                let LBA2SEQ = AA.substr(AA.length - 1, 1);
                $("#C05_ER_R_LBA2SEQ").val(LBA2SEQ);
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
                    LBA2SEQ = $("#C05_ER_R_LBA2SEQ").val();
                    let Label1 = $("#C05_ER_R_Label1").val();
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
                    let url = "/C05_ER_R/Button1_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            BaseResult.C05_ER_R_Search = data.Search;
                            BaseResult.C05_ER_R_Search1 = data.Search1;
                            BaseResult.C05_ER_R_DataGridView = data.DataGridView;
                            if (BaseResult.Search.length <= 0) {
                                if (BaseResult.Search1.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#C05_ER_R_LBA2R").val("Not data");
                                    $("#C05_ER_R_LBA2V").val(A2);
                                    $("#C05_ER_R_TextBox1").val("");
                                }
                                else {
                                    let WK_CNT = BaseResult.C05_ER_R_DataGridView[0].WK_CNT;
                                    $("#C05_ER_R_LBR22").val(WK_CNT);
                                    document.getElementById("C05_ER_R_LBR22").style.color = "green";
                                }
                            }
                            else {
                                let WK_CNT = BaseResult.C05_ER_R_Search[0].WK_CNT;
                                $("#C05_ER_R_LBR22").val(WK_CNT);
                                document.getElementById("C05_ER_R_LBR22").style.color = "green";
                                let TOOL_IDX = BaseResult.C05_ER_R_Search[0].TOOL_IDX;
                                $("#C05_ER_R_LBA2IDX").val(TOOL_IDX);
                            }
                            if (IsCheck == true) {

                            }
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                $("#C05_ER_R_LBA2V").val(A2);
                $("#C05_ER_R_TextBox1").val("");
            }
            break;
    }
    $("#C05_ER_R_TextBox1").focus();
}
$("#C05_ER_R_Button2").click(function (e) {
    C05_ER_R_Button2_Click();
});
function C05_ER_R_Button2_Click() {
    let AA2 = document.getElementById("C05_ER_R_CHBT2").checked;
    let AA4 = document.getElementById("C05_ER_R_CHBA2").checked;
    let BB2 = false;
    let BB4 = false;
    if (AA2 == true) {
        if ($("#C05_ER_R_LBT2R").val("OK")) {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA4 == true) {
        if ($("#C05_ER_R_LBA2R").val("OK")) {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if ((AA2 == true) && (BB2 == true) && (AA4 == true) && (BB4 == true)) {
        let Label1 = $("#C05_ER_R_Label1").val();
        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: Label1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_ER_R/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                C05_START_EXIT = false;
                C05_ER_R_C11_ERROR_FormClosed();

            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert("Please Check Again.");
        C05_START_EXIT = true;
    }
}
$("#C05_ER_R_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C05_ER_R_TextBox1_KeyDown();
    }
});
function C05_ER_R_TextBox1_KeyDown() {
    C05_ER_R_Button1_Click();
}
$("#C05_ER_R_Buttonclose").click(function (e) {
    C05_ER_R_Buttonclose_Click();
});
function C05_ER_R_Buttonclose_Click() {
    C05_START_C_EXIT = true;
    C05_ER_R_C11_ERROR_FormClosed();
}
function C05_ER_R_C11_ERROR_FormClosed() {
    //let MCbox = localStorage.getItem("C11_MCbox");
    let MCbox = $("#MCBOX").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C05_ER_R_LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_ER_R/C11_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            $("#C05_ER_R").modal("close");
            C05_START_ORDER_LOAD();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}