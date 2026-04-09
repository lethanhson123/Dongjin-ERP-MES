let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let SHIELDWIRE_CHK = false;
window.addEventListener('beforeunload', function (event) {
    C09_STOP_Button1_ClickSub();
});

window.addEventListener('load', function () {
    const navigationEntry = performance.getEntriesByType('navigation')[0];
    if (navigationEntry && navigationEntry.type === 'reload') {
        C09_STOP_Button1_ClickSub();
    }
});
function C09_STOP_Button1_ClickSub() {
    try {
        let C09_STOP_Label6 = Number($("#C09_STOP_Label6").val());
        if (C09_STOP_Label6 > 0) {
            C09_STOP_Button1_Click();
        }
    }
    catch (e) {

    }
}

$(document).ready(function () {
    $('#C09_STOP_MaintenanceModal').modal({
        dismissible: false
    });
    $("#C09_STOP_MaintenanceSaveBtn").click(function () {
        C09_STOP_SaveMaintenanceHistory();
    });

    

    $(".modal").modal({
        dismissible: false
    });
    var now = new Date();
    DateNow = DateToString(now);
    $("#DateTimePicker1").val(DateNow);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MCbox").val(SettingsMC_NM);
    Lan_Change();
    MC_LIST(0);
    DB_LISECHK();
    TS_USER(0);

    SHIELDWIRE_CHK = false;

    C09_Load();

    document.getElementById("rbchk2").checked = true;

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    
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
    let FCT = $("#ComboBox4").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FCT,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/MC_LIST";

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
            if (Flag == 1) {
                $("#ComboBox1").val(CHK_MC);
                Buttonfind_Click();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DB_LISECHK() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/DB_LISECHK";

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
    let MCbox = $("#MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/TS_USER";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.Search1 = data.Search1;
            if (Flag == 0) {
                ToolStripLOGIDX = BaseResult.Search1[0].TUSER_IDX;
                localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
                $("#C09_START_V3_BI_STIME").val(BaseResult.Search1[0].Name);
                $("#C09_START_V3_Label70").val(BaseResult.Search1[0].Description);
            }
            if (Flag == 1) {
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_Load() {
    let MCbox = $("#MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/C09_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.Listtsnon_oper) {
                if (data.Listtsnon_oper.length > 0) {
                    let dtIDX = data.Listtsnon_oper[0].TSNON_OPER_IDX;
                    let dtCode = data.Listtsnon_oper[0].TSNON_OPER_CODE;
                    let MC = data.Listtsnon_oper[0].TSNON_OPER_MCNM;
                    let startTime = FormatDateTime(data.Listtsnon_oper[0].TSNON_OPER_STIME);

                    C09_STOP_ShowDowntime(dtIDX, dtCode, MC, startTime);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}


$("#Button2").click(function (e) {
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
    //localStorage.setItem("C09_STOP_Label5", CODE_MC);
    //localStorage.setItem("C09_STOP_STOP_MC", $("#MCbox").val());
    //localStorage.setItem("C09_STOP_Label2", $("#ComboBox3").val());
    //let url = "/C09_STOP";
    //OpenWindowByURL(url, 800, 460);

    $("#C09_STOP_Label5").val(CODE_MC);
    $("#C09_STOP_STOP_MC").val($("#MCbox").val());
    $("#C09_STOP_Label2").val($("#ComboBox3").val());
    $("#C09_STOP").modal("open");
    C09_STOP_PageLoad();
}
$("#ComboBox4").change(function (e) {
    ComboBox4_SelectedIndexChanged();
});
function ComboBox4_SelectedIndexChanged() {
    MC_LIST();
}

$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Buttonfind_Click();
}
$("#rbchk1").click(function (e) {
    rbchk1_Click();
});
$("#rbchk2").click(function (e) {
    rbchk1_Click();
});
function rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = true;
        }
    }
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
        }
    }
    DataGridView1Render();
}

$("#Button3").click(function (e) {
    Button3_Click();
});
function Button3_Click() {
    $("#C09_COUNT").modal("open");
    C09_COUNT_PageLoad();
}
$("#Button1").click(function (e) {
    Button1_Click_1();
});
function Button1_Click_1() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/Button1_Click_1";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data) {
                if (data.Code) {
                    window.location.href = data.Code
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
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
    $("#Label2").val("-");
    let AA = $("#DateTimePicker1").val();
    let BB = $("#ComboBox1").val();
    let CC = $("#TextBox1").val();
    let DD = $("#ComboBox2").val();
    let FCT = $("#ComboBox4").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(AA);
    BaseParameter.ListSearchString.push(BB);
    BaseParameter.ListSearchString.push(CC);
    BaseParameter.ListSearchString.push(DD);
    BaseParameter.ListSearchString.push(FCT);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
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
    if (BaseResult.DataGridView1.length > 0) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C09/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert("Edit data Completed.");
                MC_LIST(1);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {
    if (confirm("Delete the selected Data?.")) {
        if (BaseResult.DataGridView1.length > 0) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView1 = BaseResult.DataGridView1;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C09/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert("Delete Order Data Completed. ");
                    Buttonfind_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            alert("Edit data Completed.");
            MC_LIST(0);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    TableHTMLToExcel("DataGridView1Table", "C09", "C09");
}
function Buttonprint_Click() {
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    if (CHKA == "Stay") {

    }
    else {
        if (BaseResult.DataGridView1.length > 0) {
            let ORDER_IDX = BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX;
            $("#C09_REPRINT_ORDER_NO").val(ORDER_IDX);
            $("#C09_REPRINT").modal("open");
            C09_REPRINT_PageLoad();
        }
    }
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
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked ><span></span></td>";
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
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOEXCEL_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY_REM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td><input onblur='DataGridView1_CellBeginEdit(" + i + ", this)' type='text' class='form-control' style='width: 100px;' value='" + BaseResult.DataGridView1[i].MC + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;

    let SELECT_LN = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO;
    let COUNT_LN = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_COUNT;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(SELECT_LN);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09/DataGridView1_SelectionChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView2 = BaseResultSub.DataGridView2;
            DataGridView2Render();
            let DGV2_CNT = BaseResult.DataGridView2.length;
            $("#Label2").val("");
            if (DGV2_CNT == COUNT_LN) {
                $("#Label2").val("OK");
            }
            else {
                $("#Label2").val("Different");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}
function DataGridView1_CellBeginEdit(i, input) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = true;
    BaseResult.DataGridView1[DataGridView1RowIndex].MC = input.value;
    DataGridView1Render();
}
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
    let IsCheck = true;
    TS_USER(0);
    SHIELDWIRE_CHK = false;
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    let W_CHK = false;
    switch (CHKA) {
        case "Complete":
            IsCheck = false;
            W_CHK = false;
            break;
        case "Close":
            IsCheck = false;
            W_CHK = false;
            break;
        case "Working":
            W_CHK = true;
            break;
        case "Stay":
            W_CHK = true;
            let XXX = 0;
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                if (BaseResult.DataGridView1[i].CONDITION == "Working") {
                    XXX = XXX + 1;
                }
            }
            if (XXX >= 3) {
                W_CHK = false;
                alert("이전 작업이 있습니다. Có một công việc trước đây.");
            }
            break;
    }
    if (IsCheck == true) {
        if (W_CHK == true) {
            let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO;
            let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE;
            let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].PO_DT;
            let DDD = BaseResult.DataGridView1[DataGridView1RowIndex].PO_QTY;
            let LLL = BaseResult.DataGridView1[DataGridView1RowIndex].MC;
            let MMM = BaseResult.DataGridView1[DataGridView1RowIndex].SAFTY_QTY;
            let CRT_DTM = BaseResult.DataGridView1[DataGridView1RowIndex].CREATE_DTM;


            C09_LIST_AA_1 = AAA;
            C09_LIST_BB_1 = CRT_DTM;
            $("#C09_LIST").modal("open");
            C09_LIST_PageLoad();


        }
    }
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
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].LEAD_NO1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].S_LR + "</td>";
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
        let key = "LEAD_NO1";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
        DataGridView2Render();
    }
});

let C09_STOP_Timer1;
let C09_STOP_EW_TIME_Timer1;
let C09_STOP_Begin;
let C09_STOP_STOPW_ORING_IDX = 0;
let C09_STOP_StartTime;
function C09_STOP_PageLoad() {
    $("#C09_STOP_STOP_MC").val($("#MCbox").val());
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    C09_STOP_StartTime = now;
    C09_STOP_Timer1StartInterval();
    let STOP_MC = $("#C09_STOP_STOP_MC").val();
    let Label5 = $("#C09_STOP_Label5").val();
    let Label2 = $("#C09_STOP_Label2").val();

    let BaseParameter = new Object();
    BaseParameter = { ListSearchString: [] }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label2);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C09_STOP/PageLoad", {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    if (BaseResult.DataGridView1.length > 0) {
                        $("#C09_STOP_Label6").val(BaseResult.DataGridView1[0].TSNON_OPER_IDX);
                        SettingsNON_OPER_CHK = true;
                        SettingsNON_OPER_IDX = BaseResult.DataGridView1[0].TSNON_OPER_IDX;
                        localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
                        localStorage.setItem("SettingsNON_OPER_IDX", SettingsNON_OPER_IDX);
                    }
                }
            }
            C09_STOP_SW_TIME();
            C09_STOP_EW_TIME_Timer1StartInterval();

            document.getElementById("C09_STOP_Button1").disabled = true;
            document.getElementById("C09_STOP_Button2").disabled = true;
            document.getElementById("C09_STOP_Button3").disabled = true;

            if ($("#C09_STOP_Label5").val() == "M") {
                document.getElementById("C09_STOP_Button2").disabled = false;
            } else {
                document.getElementById("C09_STOP_Button1").disabled = false;
            }

            C09_STOP_ToggleButton3();

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function C09_STOP_SaveMaintenanceHistory() {
    let currentStatus = $("#C09_STOP_CurrentStatus").val();
    let solution = $("#C09_STOP_Solution").val();
    let maintenedBy = $("#C09_STOP_MaintenedBy").val();
    let sparePartsUsed = $("#C09_STOP_SparePartsUsed").val().trim();
    // Validate
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

    let STOP_MC = $("#C09_STOP_STOP_MC").val();

    let BaseParameter = {
        STOP_MC: STOP_MC,
        CurrentStatus: currentStatus,
        Solution: solution,
        SparePartsUsed: sparePartsUsed,
        MaintenedBy: maintenedBy,
        Notes: $("#C09_STOP_Notes").val().trim(),
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C09_STOP/SaveMaintenanceHistory", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                M.toast({ html: 'Lưu chi tiết bảo trì thành công!', classes: 'green' });
                $("#C09_STOP_MaintenanceModal").modal("close");
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không thể lưu dữ liệu'), classes: 'red' });
            }
        })
        .catch(err => {
            M.toast({ html: 'Có lỗi xảy ra: ' + err.message, classes: 'red' });
        });
}
function C09_STOP_ShowDowntime(ID, downtimeCode, MC_Name, startTime) {
    $("#C09_STOP_Label6").val(ID);
    $("#C09_STOP_Label5").val(downtimeCode);
    $("#C09_STOP_STOP_MC").val(MC_Name);
    let stopIfor = "Chuan bi Thiet Bi";

    document.getElementById("C09_STOP_Button1").disabled = true;
    document.getElementById("C09_STOP_Button2").disabled = true;
    document.getElementById("C09_STOP_Button3").disabled = true;

    if (downtimeCode === 'S') {
        stopIfor = "Chuẩn bị thiết bị (S)";
    } else if (downtimeCode === 'E') {
        stopIfor = "Khác (E)";
    } else if (downtimeCode === 'I') {
        stopIfor = "Thiếu nguyên vật liệu (I)";
    } else if (downtimeCode === 'L') {
        stopIfor = "Giờ ăn (L)";
    } else if (downtimeCode === 'M') {
        stopIfor = "Máy hư (M)";
        document.getElementById("C09_STOP_Button2").disabled = false;
    } else if (downtimeCode === 'N') {
        stopIfor = "Đào tạo/ họp (N)";
    } else if (downtimeCode === 'Q') {
        stopIfor = "Vấn đề chất lượng (Q)";
    } else if (downtimeCode === 'T') {
        stopIfor = "Đào tạo/ họp (T)";
    }

    if (downtimeCode !== 'M') {
        document.getElementById("C09_STOP_Button1").disabled = false;
    }

    $("#C09_STOP_Label2").val(stopIfor);
    C09_STOP_ToggleButton3();

    C09_STOP_StartTime = new Date(startTime);
    $("#C09_STOP").modal("open");
    C09_STOP_Timer1StartInterval();
}

function C09_STOP_ToggleButton3() {
    if ($("#C09_STOP_Label5").val() === "M") {
        $("#C09_STOP_Button3").show();
    } else {
        $("#C09_STOP_Button3").hide();
    }
}
function C09_STOP_Timer1StartInterval() {
    C09_STOP_Timer1 = setInterval(function () {
        C09_STOP_Timer1_Tick();
    }, 1000);
}
function C09_STOP_Timer1_Tick() {
    let End = new Date();
    $("#C09_STOP_Label4").val(CounterByBegin_EndToString(C09_STOP_StartTime, End));
}
function C09_STOP_FormClosed() {
    let Label6 = $("#C09_STOP_Label6").val();
    let STOP_MC = $("#C09_STOP_STOP_MC").val();
    let Label5 = $("#C09_STOP_Label5").val(); // ← THÊM

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C09_STOP_StartTime);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5); // ← THÊM

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_STOP/C09_STOP_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            clearInterval(C09_STOP_Timer1);
            C09_STOP_OPER_TIME();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_STOP_OPER_TIME() {
    let STOP_MC = $("#C09_STOP_STOP_MC").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            try {
                BaseResult.Search = data.Search;
                let TOT_SUM = BaseResult.Search[0].SUM_TIME;

                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C09_START_V3_BI_STOPTIME = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C09_START_V3_BI_STOPTIME.substr(0, 2);
                let M_TIME1 = C09_START_V3_BI_STOPTIME.substr(2, 2);
                let S_TIME1 = C09_START_V3_BI_STOPTIME.substr(4, 2);
                C09_START_V3_BI_STOPTIME = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                $("#C09_START_V3_BI_STOPTIME").val(C09_START_V3_BI_STOPTIME);
            }
            catch (e) {
            }
            C09_STOP_EW_TIME(1);
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C09_STOP_Button2").click(function () {
    C09_STOP_Button2_Click();
});
$("#C09_STOP_Button3").click(function () {
    let formElement = $("#C09_STOP_MaintenanceForm");
    if (formElement.length > 0) formElement[0].reset();
    $("#C09_STOP_MaintenanceModal").modal("open");
});
function C09_STOP_Button2_Click() {
    let STOP_MC = $("#C09_STOP_STOP_MC").val();
    let Label6 = $("#C09_STOP_Label6").val();

    let BaseParameter = new Object();
    BaseParameter = { SearchString: STOP_MC };
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C09_STOP/Button2_Click", {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#C09_STOP_TSNON_OPER_IDX").val(Label6);
            $("#C09_STOP_Label2").val("점검 중 / Đang kiểm tra");

            document.getElementById("C09_STOP_Button2").disabled = true;
            document.getElementById("C09_STOP_Button3").disabled = false;
            document.getElementById("C09_STOP_Button1").disabled = false;

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
$("#C09_STOP_Button1").click(function () {
    C09_STOP_Button1_Click();
});
function C09_STOP_Button1_Click() {
    let Label5 = $("#C09_STOP_Label5").val();

    // ← THÊM VALIDATION
    if (Label5 == "M" && document.getElementById("C09_STOP_Button1").disabled) {
        M.toast({
            html: 'Vui lòng nhấn nút <b>"점검 중 / Đang kiểm tra"</b> trước khi khởi động lại!',
            classes: 'red darken-1',
            displayLength: 5000
        });
        return;
    }

    if (Label5 == "M") {
        let STOP_MC = $("#C09_STOP_STOP_MC").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: STOP_MC,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C09_STOP/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                C09_STOP_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C09_STOP_FormClosed();
    }
}
function C09_STOP_EW_TIME_Timer1StartInterval() {
    C09_STOP_EW_TIME_Timer1 = setInterval(function () {
        C09_STOP_EW_TIME_Timer1_Tick();
    }, 60000);
}
function C09_STOP_EW_TIME_Timer1_Tick() {
    C09_STOP_EW_TIME(0);
}
function C09_STOP_EW_TIME(Flag) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C09_STOP_STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_STOP/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (Flag == 1) {
                $("#C09_STOP").modal("close");
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_STOP_SW_TIME() {
    try {
        let USER_MC = localStorage.getItem("C02_MCbox");
        let USER_ORIDX = localStorage.getItem("C02_START_V2_Label8");
        let Label5 = $("#C09_STOP_Label5").val();
        $("#BackGround").css("display", "block");
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
        let url = "/C09_STOP/SW_TIME";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C09_STOP_DGV_WT = data.DGV_WT;
                try {
                    C09_STOP_STOPW_ORING_IDX = BaseResult.C09_STOP_DGV_WT[0].TOWT_INDX;
                }
                catch (e) {
                    C09_STOP_STOPW_ORING_IDX = 0;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    catch (e) {
        C09_STOP_STOPW_ORING_IDX = 0;
    }
}

let C09_COUNT_DataGridView1RowIndex;
function C09_COUNT_PageLoad() {
    BaseResult.C09_COUNT_DataGridView1 = new Object();
    BaseResult.C09_COUNT_DataGridView1 = [];
    C09_COUNT_Load();
    document.getElementById("C09_COUNT_RadioButton1").checked = true;
}
function C09_COUNT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_COUNT/C09_COUNT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_COUNT_DataGridView1 = data.DataGridView1;
            C09_COUNT_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C09_COUNT_RadioButton1").click(function (e) {
    C09_COUNT_RadioButton2_Click();
});
$("#C09_COUNT_RadioButton2").click(function (e) {
    C09_COUNT_RadioButton2_Click();
});
function C09_COUNT_RadioButton2_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.RadioButton1 = document.getElementById("C09_COUNT_RadioButton1").checked;
    BaseParameter.RadioButton2 = document.getElementById("C09_COUNT_RadioButton2").checked;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_COUNT/RadioButton2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_COUNT_DataGridView1 = data.DataGridView1;
            C09_COUNT_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_COUNT_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_COUNT_DataGridView1) {
            if (BaseResult.C09_COUNT_DataGridView1.length > 0) {
                C09_COUNT_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_COUNT_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C09_COUNT_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].SUM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].Stay + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].Working + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].Complete + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_COUNT_DataGridView1[i].Close + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_COUNT_DataGridView1").innerHTML = HTML;
}
function C09_COUNT_DataGridView1_SelectionChanged(i) {
    C09_COUNT_DataGridView1RowIndex = i;
}
let C09_COUNT_DataGridView1Table = document.getElementById("C09_COUNT_DataGridView1Table");
C09_COUNT_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_COUNT_DataGridView1, key, text, IsTableSort);
        C09_COUNT_DataGridView1Render();
    }
});

let C09_LIST_DateNow;
let C09_LIST_DataGridView1RowIndex = 0;
let C09_LIST_DataGridView2RowIndex = 0;
let C09_LIST_SHIELDWIRE_CHK = false;
let C09_LIST_AA_1;
let C09_LIST_BB_1;
let C09_LIST_BARCODE_MT;
function C09_LIST_PageLoad() {
    var now = new Date();
    C09_LIST_DateNow = DateToString(now);
    $("#C09_LIST_DateTimePicker1").val(DateNow);
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C09_LIST_MCbox").val(SettingsMC_NM);
    C09_LIST_Lan_Change();
    C09_LIST_DB_LISECHK();
    C09_LIST_TS_USER(0);

    C09_LIST_SHIELDWIRE_CHK = false;

    C09_LIST_Load();

    document.getElementById("C09_LIST_rbchk2").checked = true;

    BaseResult.C09_LIST_DataGridView1 = new Object();
    BaseResult.C09_LIST_DataGridView1 = [];
    BaseResult.C09_LIST_DataGridView2 = new Object();
    BaseResult.C09_LIST_DataGridView2 = [];

    C09_LIST_Buttonfind_Click();
}
function C09_LIST_Lan_Change() {
    C09_LIST_Form_Label2();
}
function C09_LIST_Form_Label2() {
    let CODE_S1 = "S";
    let CODE_I1 = "I";
    let CODE_Q1 = "Q";
    let CODE_M1 = "M";
    let CODE_T1 = "T";
    let CODE_L1 = "L";
    let CODE_E1 = "E";

    let CODE_S0 = $("#C09_LIST_CODE_S").val() + " (" + CODE_S1 + ")";
    let CODE_I0 = $("#C09_LIST_CODE_I").val() + " (" + CODE_I1 + ")";
    let CODE_Q0 = $("#C09_LIST_CODE_Q").val() + " (" + CODE_Q1 + ")";
    let CODE_M0 = $("#C09_LIST_CODE_M").val() + " (" + CODE_M1 + ")";
    let CODE_T0 = $("#C09_LIST_CODE_T").val() + " (" + CODE_T1 + ")";
    let CODE_L0 = $("#C09_LIST_CODE_L").val() + " (" + CODE_L1 + ")";
    let CODE_E0 = $("#C09_LIST_CODE_E").val() + " (" + CODE_E1 + ")";

    $("#C09_LIST_ComboBox3").empty();

    var ComboBox3 = document.getElementById("C09_LIST_ComboBox3");

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
function C09_LIST_MC_LIST(Flag) {
    let FCT = $("#C09_LIST_ComboBox4").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FCT,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/MC_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_LIST_ComboBox1 = data.ComboBox1;
            let CHK_MC = $("#C09_LIST_ComboBox1").val();
            $("#C09_LIST_ComboBox1").empty();
            for (let i = 0; i < BaseResult.C09_LIST_ComboBox1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox1[i].MC;
                option.value = BaseResult.ComboBox1[i].MC;

                var ComboBox1 = document.getElementById("C09_LIST_ComboBox1");
                ComboBox1.add(option);
            }
            if (Flag == 1) {
                $("#C09_LIST_ComboBox1").val(CHK_MC);
                C09_LIST_Buttonfind_Click();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_LIST_DB_LISECHK() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/DB_LISECHK";

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
function C09_LIST_TS_USER(Flag) {
    let MCbox = $("#C09_LIST_MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/DB_LISECHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_LIST_Search1 = data.Search1;
            if (Flag == 0) {
                ToolStripLOGIDX = BaseResult.C09_LIST_Search1[0].TUSER_IDX;
                localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
                $("#C09_LIST_START_V3_BI_STIME").val(BaseResult.C09_LIST_Search1[0].Name);
                $("#C09_LIST_START_V3_Label70").val(BaseResult.C09_LIST_Search1[0].Description);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_LIST_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/C09_LIST_Load";

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
$("#C09_LIST_Button2").click(function (e) {
    C09_LIST_Button2_Click();
});
function C09_LIST_Button2_Click() {
    C09_LIST_TS_USER(0);
    let CODE_MC = "";
    let ComboBox3 = document.getElementById("C09_LIST_ComboBox3").selectedIndex;
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


    $("#C09_STOP_Label5").val(CODE_MC);
    $("#C09_STOP_STOP_MC").val($("#C09_LIST_MCbox").val());
    $("#C09_STOP_Label2").val($("#C09_LIST_ComboBox3").val());
    $("#C09_STOP").modal("open");
    C09_STOP_PageLoad();
}
$("#C09_LIST_ComboBox4").change(function (e) {
    C09_LIST_ComboBox4_SelectedIndexChanged();
});
function C09_LIST_ComboBox4_SelectedIndexChanged() {
    C09_LIST_MC_LIST();
}
$("#C09_LIST_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C09_LIST_TextBox1_KeyDown();
    }
});
function C09_LIST_TextBox1_KeyDown() {
    C09_LIST_Buttonfind_Click();
}
$("#C09_LIST_rbchk1").click(function (e) {
    C09_LIST_Rbchk1_Click();
});
$("#C09_LIST_rbchk2").click(function (e) {
    C09_LIST_Rbchk2_Click();
});
function C09_LIST_Rbchk1_Click() {
    let rbchk1 = document.getElementById("C09_LIST_rbchk1").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.C09_LIST_DataGridView1.length; i++) {
            BaseResult.C09_LIST_DataGridView1[i].CHK = true;
        }
    }
    C09_LIST_DataGridView1Render();
}
function C09_LIST_Rbchk2_Click() {
    let rbchk2 = document.getElementById("C09_LIST_rbchk2").checked;
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.C09_LIST_DataGridView1.length; i++) {
            BaseResult.C09_LIST_DataGridView1[i].CHK = !BaseResult.C09_LIST_DataGridView1[i].CHK;
        }
    }
    C09_LIST_DataGridView1Render();
}
$("#C09_LIST_Button3").click(function (e) {
    C09_LIST_Button3_Click();
});
function Button3_Click() {
    $("#C09_COUNT").modal("open");
    C09_COUNT_PageLoad();
}
$("#C09_LIST_Button1").click(function (e) {
    C09_LIST_Button1_Click_1();
});
function C09_LIST_Button1_Click_1() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/Button1_Click_1";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data) {
                if (data.Code) {
                    window.location.href = data.Code;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C09_LIST_Buttonfind").click(function () {
    C09_LIST_Buttonfind_Click();
});
$("#C09_LIST_Buttonadd").click(function () {
    C09_LIST_Buttonadd_Click();
});
$("#C09_LIST_Buttonsave").click(function () {
    C09_LIST_Buttonsave_Click();
});
$("#C09_LIST_Buttondelete").click(function () {
    C09_LIST_Buttondelete_Click();
});
$("#C09_LIST_Buttoncancel").click(function () {
    C09_LIST_Buttoncancel_Click();
});
$("#C09_LIST_Buttoninport").click(function () {
    C09_LIST_Buttoninport_Click();
});
$("#C09_LIST_Buttonexport").click(function () {
    C09_LIST_Buttonexport_Click();
});
$("#C09_LIST_Buttonprint").click(function () {
    C09_LIST_Buttonprint_Click();
});
$("#C09_LIST_Buttonhelp").click(function () {
    C09_LIST_Buttonhelp_Click();
});
$("#C09_LIST_Buttonclose").click(function () {
    C09_LIST_Buttonclose_Click();
});
function C09_LIST_Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C09_LIST_AA_1,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_LIST_DataGridView1 = data.DataGridView1;
            C09_LIST_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_LIST_Buttonadd_Click() {
}
function C09_LIST_Buttonsave_Click() {
    if (BaseResult.C09_LIST_DataGridView1.length > 0) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView1 = BaseResult.C09_LIST_DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C09_LIST/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert("Edit data Completed.");
                C09_LIST_MC_LIST(1);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C09_LIST_Buttondelete_Click() {
    if (confirm("Delete the selected Data?.")) {
        if (BaseResult.C09_LIST_DataGridView1.length > 0) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView1 = BaseResult.C09_LIST_DataGridView1;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C09_LIST/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert("Delete Order Data Completed. ");
                    C09_LIST_Buttonfind_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function C09_LIST_Buttoncancel_Click() {

}
function C09_LIST_Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.C09_LIST_DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            alert("Edit data Completed.");
            C09_LIST_MC_LIST(0);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_LIST_Buttonexport_Click() {
    TableHTMLToExcel("C09_LIST_DataGridView1Table", "C09_LIST", "C09_LIST");
}
function C09_LIST_Buttonprint_Click() {
    let CHKA = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CONDITION;
    if (CHKA == "Stay") {

    }
    else {
        if (BaseResult.C09_LIST_DataGridView1.length > 0) {
            let ORDER_IDX = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].ORDER_IDX;
            $("#C09_REPRINT_ORDER_NO").val(ORDER_IDX);
            $("#C09_REPRINT").modal("open");
            C09_REPRINT_PageLoad();
        }
    }
}
function C09_LIST_Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function C09_LIST_Buttonclose_Click() {
    C09_LIST_FormClosed();
}
function C09_LIST_FormClosed() {
    Buttonfind_Click();
    $("#C09_LIST").modal("close");
}

function C09_LIST_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_LIST_DataGridView1) {
            if (BaseResult.C09_LIST_DataGridView1.length > 0) {
                C09_LIST_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_LIST_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C09_LIST_DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.C09_LIST_DataGridView1[i].CHK) {
                        HTML = HTML + "<td onclick='C09_LIST_DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked ><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='C09_LIST_DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    if (BaseResult.C09_LIST_DataGridView1[i].OR_NO == "") {
                        HTML = HTML + "<td style='background-color: green; padding-left: 10px; padding-right: 10px;'>" + BaseResult.C09_LIST_DataGridView1[i].OR_NO + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: darkorange; padding-left: 10px; padding-right: 10px;'>" + BaseResult.C09_LIST_DataGridView1[i].OR_NO + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td onclick='C09_LIST_DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.C09_LIST_DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td><input onblur='C09_LIST_DataGridView1_CellBeginEdit(" + i + ", this)' type='text' class='form-control' style='width: 100px;' value='" + BaseResult.C09_LIST_DataGridView1[i].MC + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].LS_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].ORDER_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView1[i].DSCN_YN + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_LIST_DataGridView1").innerHTML = HTML;
}
function C09_LIST_DataGridView1_SelectionChanged(i) {
    C09_LIST_DataGridView1RowIndex = i;

    let SELECT_LN = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].LEAD_NO;
    let COUNT_LN = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].LEAD_COUNT;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(SELECT_LN);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_LIST/DataGridView1_SelectionChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_LIST_DataGridView2 = data.DataGridView2;
            C09_LIST_DataGridView2Render();
            let DGV2_CNT = BaseResult.C09_LIST_DataGridView2.length;
            $("#C09_LIST_Label2").val(DGV2_CNT);
            document.getElementById("C09_LIST_PictureBox1").src = "";
            let SP_LD = SELECT_LN.substr(0, 2);
            if (SP_LD == "SP") {
                C09_LIST_BARCODE_MT = SELECT_LN;
                let BaseParameter = new Object();
                BaseParameter = {
                    SearchString: C09_LIST_BARCODE_MT,
                }
                BaseParameter.USER_ID = GetCookieValue("UserID");
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/C09_LIST/GenerateBarcode_MT";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        document.getElementById("C09_LIST_PictureBox1").src = data.Code;
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                    })
                });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_LIST_DataGridView1CHKChanged(i) {
    C09_LIST_DataGridView1RowIndex = i;
    BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CHK = !BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CHK;
    C09_LIST_DataGridView1Render();
}
function C09_LIST_DataGridView1_CellBeginEdit(i, input) {
    C09_LIST_DataGridView1RowIndex = i;
    if (BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CONDITION == "Stay") {
        BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CHK = true;
        BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].MC = input.value;
        C09_LIST_DataGridView1Render();
    }
}
function C09_LIST_DataGridView1_CellClick(i) {
    C09_LIST_DataGridView1RowIndex = i;
    let IsCheck = true;
    C09_LIST_TS_USER(0);
    C09_LIST_SHIELDWIRE_CHK = false;
    let CHKA = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].CONDITION;
    let W_CHK = false;
    switch (CHKA) {
        case "Complete":
            IsCheck = false;
            W_CHK = false;
            break;
        case "Close":
            IsCheck = false;
            W_CHK = false;
            break;
        case "Working":
            W_CHK = true;
            break;
        case "Stay":
            W_CHK = true;
            let XXX = 0;
            for (let i = 0; i < BaseResult.C09_LIST_DataGridView1.length; i++) {
                if (BaseResult.C09_LIST_DataGridView1[i].CONDITION == "Working") {
                    XXX = XXX + 1;
                }
            }
            if (XXX >= 3) {
                IsCheck = false;
                W_CHK = false;
                alert("이전 작업이 있습니다. Có một công việc trước đây.");
            }
            break;
    }
    if (IsCheck == true) {
        if (W_CHK == true) {
            let AAA = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].LEAD_NO;
            let BBB = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].BUNDLE_SIZE;
            let CCC = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].PO_DT;
            let DDD = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].PO_QTY;
            let FFF = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].ORDER_IDX;
            let LLL = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].MC;
            let MMM = BaseResult.C09_LIST_DataGridView1[C09_LIST_DataGridView1RowIndex].SAFTY_QTY;


            $("#C09_START_V3_partbox").val(AAA);
            $("#C09_START_V3_ORDER_NO_TEXT").val(FFF);
            $("#C09_START_V3_Label7").val(BBB);
            //$("#C09_START_V3").modal("open");
            C09_START_V3_BackGround = 0;
            C09_START_V3_BackGroundStartInterval();
            C09_START_V3_PageLoad();
        }
    }
}
let C09_START_V3_BackGround = 0;;
function C09_START_V3_BackGroundStartInterval() {
    let C09_START_V3_BackGroundTimer = setInterval(function () {
        $("#BackGround").css("display", "block");
        C09_START_V3_BackGround = C09_START_V3_BackGround + 1;
        if (C09_START_V3_BackGround > 25) {
            $("#BackGround").css("display", "none");
            clearInterval(C09_START_V3_BackGroundTimer);
        }
    }, 100);
}
let C09_LIST_DataGridView1Table = document.getElementById("C09_LIST_DataGridView1Table");
C09_LIST_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_LIST_DataGridView1, key, text, IsTableSort);
        C09_LIST_DataGridView1Render();
    }
});

function C09_LIST_DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_LIST_DataGridView2) {
            if (BaseResult.C09_LIST_DataGridView2.length > 0) {
                C09_LIST_DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_LIST_DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='C09_LIST_DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView2[i].LEAD_NO1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_LIST_DataGridView2[i].S_LR + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_LIST_DataGridView2").innerHTML = HTML;
}
function C09_LIST_DataGridView2_SelectionChanged(i) {
    C09_LIST_DataGridView2RowIndex = i;
}

let C09_LIST_DataGridView2Table = document.getElementById("C09_LIST_DataGridView2Table");
C09_LIST_DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "LEAD_NO1";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_LIST_DataGridView2, key, text, IsTableSort);
        C09_LIST_DataGridView2Render();
    }
});




let C09_REPRINT_BARCODE_QR;
let C09_REPRINT_DataGridView1RowIndex;
function C09_REPRINT_PageLoad() {
    BaseResult = new Object();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    C02_REPRINT_Load();
}
function C02_REPRINT_Load() {
    let ORDER_NO = $("#C09_REPRINT_ORDER_NO").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: ORDER_NO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_REPRINT/C02_REPRINT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_REPRINT_DataGridView = data.DataGridView;
            BaseResult.C09_REPRINT_DataGridView1 = data.DataGridView1;
            C09_REPRINT_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_REPRINT_Buttonprint_Click() {
    let IsCheck = true;
    if (BaseResult.C09_REPRINT_DataGridView1.length <= 0) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    let PRT_1 = $("#C09_REPRINT_PRT_1").val();
    if (PRT_1.length < 8) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    if (IsCheck == true) {
        C09_REPRINT_BARCODE_QR = PRT_1;
        C09_REPRINT_PrintDocument1_PrintPage();
    }
}
function C09_REPRINT_PrintDocument1_PrintPage() {
    let PR4 = $("#C09_REPRINT_PRT_6").val();
    let PR5 = $("#C09_REPRINT_PRT_3").val();
    let PR7 = $("#C09_REPRINT_PRT_7").val();
    let PR8 = $("#C09_REPRINT_PRT_8").val();
    let PR20 = $("#C09_REPRINT_PRT_2").val();
    let PR23 = $("#C09_REPRINT_PRT_9").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView = BaseResult.C09_REPRINT_DataGridView;
    BaseParameter.ListSearchString.push(C09_REPRINT_BARCODE_QR);
    BaseParameter.ListSearchString.push(PR4);
    BaseParameter.ListSearchString.push(PR5);
    BaseParameter.ListSearchString.push(PR7);
    BaseParameter.ListSearchString.push(PR8);
    BaseParameter.ListSearchString.push(PR20);
    BaseParameter.ListSearchString.push(PR23);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_REPRINT/PrintDocument1_PrintPage";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let url = data.Code;
            OpenWindowByURL(url, 200, 200);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

$("#C09_REPRINT_Buttonclose").click(function (e) {
    C09_REPRINT_Buttonclose_Click();
});
function C09_REPRINT_Buttonclose_Click() {
    $("#C09_REPRINT").modal("close");
}
function C09_REPRINT_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_REPRINT_DataGridView1) {
            if (BaseResult.C09_REPRINT_DataGridView1.length > 0) {
                C09_REPRINT_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_REPRINT_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C09_REPRINT_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].TORDER_BARCODENM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_REPRINT_DataGridView1[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_REPRINT_DataGridView1").innerHTML = HTML;
}
function C09_REPRINT_DataGridView1_SelectionChanged(i) {
    C09_REPRINT_DataGridView1RowIndex = i;
    $("#C09_REPRINT_PRT_1").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].TORDER_BARCODENM);
    $("#C09_REPRINT_PRT_2").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].Barcode_SEQ);
    $("#C09_REPRINT_PRT_3").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].LEAD_NO);
    $("#C09_REPRINT_PRT_4").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].PO_DT);
    $("#C09_REPRINT_PRT_5").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].MC);
    $("#C09_REPRINT_PRT_6").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].HOOK_RACK);
    $("#C09_REPRINT_PRT_7").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].PO_QTY);
    $("#C09_REPRINT_PRT_8").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].BUNDLE_SIZE);
    $("#C09_REPRINT_PRT_9").val(BaseResult.DataGridView1[C09_REPRINT_DataGridView1RowIndex].SAFTY_QTY);
}
let C09_REPRINT_DataGridView1Table = document.getElementById("C09_REPRINT_DataGridView1Table");
C09_REPRINT_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_REPRINT_DataGridView1, key, text, IsTableSort);
        C09_REPRINT_DataGridView1Render();
    }
});

let C09_START_V3_DataGridView1RowIndex = 0;
let C09_START_V3_DataGridView2RowIndex = 0;
let C09_START_V3_Timer2;
let C09_START_V3_C09_SPCTimer;
let C09_START_V3_SPC_EXIT = false;
let C09_START_V3_StartTime = new Date();
let C09_START_V3_RunTime = new Date();
function C09_START_V3_PageLoad() {
  
    document.getElementById("C09_START_V3_SPC1").innerHTML = "First";
    document.getElementById("C09_START_V3_SPC2").innerHTML = "Middle";
    document.getElementById("C09_START_V3_SPC3").innerHTML = "End";

    BaseResult.C09_START_V3_DataGridView1 = new Object();
    BaseResult.C09_START_V3_DataGridView1 = [];
    BaseResult.C09_START_V3_DataGridView2 = new Object();
    BaseResult.C09_START_V3_DataGridView2 = [];
    BaseResult.C09_START_V3_DGV_C09_ST_00 = new Object();
    BaseResult.C09_START_V3_DGV_C09_ST_00 = [];
    BaseResult.C09_START_V3_DGV_C09_ST_BOM = new Object();
    BaseResult.C09_START_V3_DGV_C09_ST_BOM = [];
    BaseResult.C09_START_V3_Search = new Object();
    BaseResult.C09_START_V3_Search = [];
    BaseResult.C09_START_V3_Search1 = new Object();
    BaseResult.C09_START_V3_Search1 = [];

    C09_START_V3_DataGridView1Render();
    C09_START_V3_DataGridView2Render();

    document.getElementById("C09_START_V3_R_BUTT_LH").disabled = false;
    document.getElementById("C09_START_V3_R_BUTT_LH").checked = true;
    document.getElementById("C09_START_V3_R_BUTT_RH").disabled = false;
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C09_START_V3_MCbox").val(SettingsMC_NM);
    document.getElementById("C09_START_V3_MRUS").style.color = "black";

    C09_START_V3_Timer2StartInterval();

    document.getElementById("C09_START_V3_BI_RadioButton1").checked = true;


    C09_START_V3_ORDER_LOAD(-1);
    C09_START_V3_OPER_TIME();
    
}
function C09_START_V3_FormClosed() {
    clearInterval(C09_START_V3_Timer2);
    $("#C09_START_V3").modal("close");
    C09_LIST_Buttonfind_Click();
}
function C09_START_V3_Timer2StartInterval() {
    C09_START_V3_Timer2 = setInterval(function () {
        C09_START_V3_Timer2_Tick();
    }, 1000);
}
function C09_START_V3_Timer2_Tick() {
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
    C09_START_V3_RunTime = new Date();
    let myTimeSpan;
    let Text_time = $("#C09_START_V3_BI_STOPTIME").val();
    let hh_time1 = Text_time.substr(0, 2);
    let mm_time1 = Text_time.substr(3, 2);
    let ss_time2 = Text_time.substr(6, 2);
    let hh_time = Number(Text_time.split(":")[0]) * 60 * 60;
    let mm_time = Number(Text_time.split(":")[1]) * 60;
    let ss_time = Number(Text_time.split(":")[2]);
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = C09_START_V3_RunTime - C09_START_V3_StartTime;
    myTimeSpan = Math.floor(myTimeSpan / 1000) - tot_time;
    let BI_RTIME = ConvertSecondToString(myTimeSpan, ":");
    $("#C09_START_V3_BI_RTIME").val(BI_RTIME);
    if (myTimeSpan < 0) {
        document.getElementById("C09_START_V3_BI_RTIME").style.color = "red";
        document.getElementById("C09_START_V3_BI_RCK").style.color = "red";
        $("#C09_START_V3_BI_RCK").val("-");
    }
    else {
        document.getElementById("C09_START_V3_BI_RTIME").style.color = "black";
        document.getElementById("C09_START_V3_BI_RCK").style.color = "black";
        $("#C09_START_V3_BI_RCK").val("+");
    }
    let M_STIME = new Date(NOW_DATE_S.getFullYear(), NOW_DATE_S.getMonth(), NOW_DATE_S.getDate(), 6, 0, 0);
    let M_SPAN = C09_START_V3_RunTime - M_STIME;
    M_SPAN = Math.floor(M_SPAN / 1000);

    let DTIME11 = M_SPAN;
    let DTIME15 = DTIME11;

    let DTIME21 = $("#C09_START_V3_BI_RTIME").val();
    let DTIME22 = Number(DTIME21.split(":")[0]) * 60 * 60;
    let DTIME23 = Number(DTIME21.split(":")[1]) * 60;
    let DTIME24 = Number(DTIME21.split(":")[2]);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    let BI_RVA = ((DTIME25 / DTIME15) * 100).toFixed(2) + " %";
    $("#C09_START_V3_BI_RVA").val(BI_RVA);
    try {
        if ($("#C09_START_V3_BI_RCK").val() == "+") {
            let UPH_T1 = $("#C09_START_V3_BI_RTIME").val();
            let UPH_H1 = Number(UPH_T1.split(":")[0]);
            let UPH_M1 = Number(UPH_T1.split(":")[1]);
            let BI_WQTY = 0;
            let C09_START_V3_BI_WQTY = $("#C09_START_V3_BI_WQTY").val();
            if (C09_START_V3_BI_WQTY != "--") {
                BI_WQTY = Number($("#C09_START_V3_BI_WQTY").val());
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
                $("#C09_START_V3_BI_UPH").val(BI_UPH);
            }
        }
    }
    catch (e) {
        console.log(e);
    }
}
function C09_START_V3_ORDER_LOAD(Flag) {

    //$("#BackGround").css("display", "block");
    let C09_START_V3_ORDER_NO_TEXT = $("#C09_START_V3_ORDER_NO_TEXT").val();
    let MCName= $("#C09_START_V3_MCbox").val();

    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C09_START_V3_ORDER_NO_TEXT,
        MC_NAME: MCName
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
            BaseResult.C09_START_V3_DataGridView1 = [];
            BaseResult.C09_START_V3_DataGridView2 = [];
            C09_START_V3_DataGridView1Render();
            C09_START_V3_DataGridView2Render();

            BaseResult.C09_START_V3_DGV_C09_ST_00 = data.DGV_C09_ST_00;
            BaseResult.C09_START_V3_DGV_C09_ST_BOM = data.DGV_C09_ST_BOM;
            C09_START_V3_StartTime = new Date($("#C09_START_V3_Label70").val());

            let BUNDLE_SIZE = BaseResult.C09_START_V3_DGV_C09_ST_00[0].BUNDLE_SIZE;
            let PO_QTY = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PO_QTY;
            let PERFORMN = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PERFORMN;
            $("#C09_START_V3_Label7").val(BUNDLE_SIZE);
            $("#C09_START_V3_Label48").val(PO_QTY);
            $("#C09_START_V3_Label59").val(PERFORMN);

            let AAAA = PO_QTY - PERFORMN;
            let BBBB = BUNDLE_SIZE;

            if (AAAA < BBBB) {
                $("#C09_START_V3_Label7").val(AAAA);
            }

            let L_COUNT = 1;
            let R_COUNT = 1;
            let L_MIN_QYT = 1000001;
            let R_MIN_QYT = 1000001;

            for (let i = 0; i < BaseResult.C09_START_V3_DGV_C09_ST_BOM.length; i++) {
                let BOM_LEAD_NO = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].LEAD_NO;
                let BOM_BSIZE = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].B_SIZE;
                let LEAD_ERROR_CHK = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].DC_YN;
                let LEAD_QTY = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].S_SUM;
                let ERROR_CHK = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].E_CHK;

                let S_LR = BaseResult.C09_START_V3_DGV_C09_ST_BOM[i].S_LR;
                if (S_LR == "L") {
                    let Label48 = Number($("#C09_START_V3_Label48").val());
                    let Label59 = Number($("#C09_START_V3_Label59").val());
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
                    BaseResult.C09_START_V3_DataGridView1.push(DataGridView1Sub);
                    C09_START_V3_DataGridView1Render();
                    L_COUNT = L_COUNT + 1;
                    if (L_MIN_QYT > LEAD_QTY) {
                        L_MIN_QYT = LEAD_QTY
                    }
                }
                if (S_LR == "R") {
                    let Label48 = Number($("#C09_START_V3_Label48").val());
                    let Label59 = Number($("#C09_START_V3_Label59").val());
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
                    BaseResult.C09_START_V3_DataGridView2.push(DataGridView2Sub);
                    C09_START_V3_DataGridView2Render();
                    R_COUNT = R_COUNT + 1;
                    if (R_MIN_QYT > LEAD_QTY) {
                        R_MIN_QYT = LEAD_QTY
                    }
                }
            }

            if (L_MIN_QYT > 1000000) {
                $("#C09_START_V3_LH_QTY").val("---");
            }
            else {
                $("#C09_START_V3_LH_QTY").val(L_MIN_QYT);
            }

            if (R_MIN_QYT > 1000000) {
                $("#C09_START_V3_RH_QTY").val("---");
            }
            else {
                $("#C09_START_V3_RH_QTY").val(R_MIN_QYT);
            }
            if (L_MIN_QYT > R_MIN_QYT) {
                let RH_QTY = 0;
                let C09_START_V3_RH_QTY = $("#C09_START_V3_RH_QTY").val();
                if (C09_START_V3_RH_QTY != "---") {
                    RH_QTY = Number($("#C09_START_V3_RH_QTY").val());
                }
                let Label7 = Number($("#C09_START_V3_Label7").val());
                if (RH_QTY >= Label7) {
                    document.getElementById("C09_START_V3_MRUS").style.color = "green";
                    $("#C09_START_V3_MRUS").val("OK");
                }
                else {
                    document.getElementById("C09_START_V3_MRUS").style.color = "black";
                    $("#C09_START_V3_MRUS").val("STAY");
                }
            }
            else {
                let LH_QTY = 0;
                let C09_START_V3_LH_QTY = $("#C09_START_V3_LH_QTY").val();
                if (C09_START_V3_LH_QTY != "---") {
                    LH_QTY = Number($("#C09_START_V3_LH_QTY").val());
                }
                let Label7 = Number($("#C09_START_V3_Label7").val());
                if (LH_QTY >= Label7) {
                    document.getElementById("C09_START_V3_MRUS").style.color = "green";
                    $("#C09_START_V3_MRUS").val("OK");
                }
                else {
                    document.getElementById("C09_START_V3_MRUS").style.color = "black";
                    $("#C09_START_V3_MRUS").val("STAY");
                }
            }

            if (BaseResult.C09_START_V3_DataGridView1.length == 0) {
                document.getElementById("C09_START_V3_R_BUTT_LH").disabled = true;
                document.getElementById("C09_START_V3_R_BUTT_RH").checked = true;
            }
            if (BaseResult.C09_START_V3_DataGridView2.length == 0) {
                document.getElementById("C09_START_V3_R_BUTT_RH").disabled = true;
                document.getElementById("C09_START_V3_R_BUTT_LH").checked = true;
            }
            C09_START_V3_SPC_LOAD();
            if (Flag == -1) {
                $("#C09_START_V3").modal("open");
                $("#C09_START_V3_Barcodebox").val("");
                $("#C09_START_V3_Barcodebox").focus();
            }
            if (Flag == 1) {
                let R_BUTT_LH = document.getElementById("C09_START_V3_R_BUTT_LH").checked;
                if (R_BUTT_LH == true) {
                    let D1_COUNT = 0;
                    for (let i = 0; i < BaseResult.C09_START_V3_DataGridView1.length; i++) {
                        if (BaseResult.C09_START_V3_DataGridView1[i].ERROR_CHK == "Y") {
                            D1_COUNT = D1_COUNT + 1;
                        }
                    }
                    if (BaseResult.C09_START_V3_DataGridView1.length == D1_COUNT) {
                        if (document.getElementById("C09_START_V3_R_BUTT_RH").disabled == false) {
                            document.getElementById("C09_START_V3_R_BUTT_RH").checked = true;
                        }
                    }
                }
                else {
                    let D2_COUNT = 0;
                    for (let i = 0; i < BaseResult.C09_START_V3_DataGridView2.length; i++) {
                        if (BaseResult.C09_START_V3_DataGridView2[i].ERROR_CHK == "Y") {
                            D2_COUNT = D2_COUNT + 1;
                        }
                    }
                    if (BaseResult.C09_START_V3_DataGridView2.length == D2_COUNT) {
                        if (document.getElementById("C09_START_V3_R_BUTT_LH").disabled == false) {
                            document.getElementById("C09_START_V3_R_BUTT_LH").checked = true;
                        }
                    }
                }
                $("#C09_START_V3_Barcodebox").val("");
                $("#C09_START_V3_Barcodebox").focus();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(err);
            $("#BackGround").css("display", "none");
        })
    });
}

function C09_START_V3_OPER_TIME() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#C09_START_V3_MCbox").val(),
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
            BaseResult.C09_START_V3_Search = data.Search;

            let TOT_SUM = 0;
            try {
                TOT_SUM = BaseResult.C09_START_V3_Search[0].SUM_TIME;
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
                $("#C09_START_V3_BI_STOPTIME").val(BI_STOPTIME);
            }
            catch (e) {
                $("#C09_START_V3_BI_STOPTIME").val("00:00:00");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_START_V3_SPC_LOAD() {
    document.getElementById("C09_START_V3_SPC1").disabled = false;
    document.getElementById("C09_START_V3_SPC2").disabled = false;
    document.getElementById("C09_START_V3_SPC3").disabled = false;
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#C09_START_V3_ORDER_NO_TEXT").val(),
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

            BaseResult.C09_START_V3_Search1 = data.Search1;

            let COUNT_SPC = Number($("#C09_START_V3_Label48").val());
            try {
                COUNT_SPC = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PO_QTY;
            }
            catch (err) {
                COUNT_SPC = Number($("#C09_START_V3_Label48").val());
            }
            let partbox = $("#C09_START_V3_partbox").val();
            partbox = partbox.substr(0, 2);

            if (partbox == "SP") {
                try {
                    if (COUNT_SPC < 11) {
                        document.getElementById("C09_START_V3_SPC1").innerHTML = "----";
                        document.getElementById("C09_START_V3_SPC1").disabled = true;
                    }
                    else {
                        document.getElementById("C09_START_V3_SPC1").innerHTML = "First";
                        document.getElementById("C09_START_V3_SPC1").disabled = false;
                    }
                    if (COUNT_SPC < 501) {
                        document.getElementById("C09_START_V3_SPC2").innerHTML = "----";
                        document.getElementById("C09_START_V3_SPC2").disabled = true;
                    }
                    else {
                        document.getElementById("C09_START_V3_SPC2").innerHTML = "Middle";
                        document.getElementById("C09_START_V3_SPC2").disabled = false;
                    }
                }
                catch (err) {
                    document.getElementById("C09_START_V3_SPC1").innerHTML = "ERROR";
                    document.getElementById("C09_START_V3_SPC2").innerHTML = "ERROR";
                    document.getElementById("C09_START_V3_SPC3").innerHTML = "ERROR";
                }

                for (let i = 0; i < BaseResult.C09_START_V3_Search1.length; i++) {
                    let COLSIP = BaseResult.C09_START_V3_Search1[i].COLSIP;
                    if (COLSIP == "First") {
                        document.getElementById("C09_START_V3_SPC1").innerHTML = "Complete";
                        document.getElementById("C09_START_V3_SPC1").disabled = true;
                    }
                    if (COLSIP == "Middle") {
                        document.getElementById("C09_START_V3_SPC2").innerHTML = "Complete";
                        document.getElementById("C09_START_V3_SPC2").disabled = true;
                    }
                    if (COLSIP == "End") {
                        document.getElementById("C09_START_V3_SPC3").innerHTML = "Complete";
                        document.getElementById("C09_START_V3_SPC3").disabled = true;
                    }
                }
            }
            else {
                document.getElementById("C09_START_V3_SPC1").innerHTML = "----";
                document.getElementById("C09_START_V3_SPC1").disabled = true;
                document.getElementById("C09_START_V3_SPC2").innerHTML = "----";
                document.getElementById("C09_START_V3_SPC2").disabled = true;
                document.getElementById("C09_START_V3_SPC3").innerHTML = "----";
                document.getElementById("C09_START_V3_SPC3").disabled = true;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
$("#C09_START_V3_R_BUTT_LH").click(function (e) {
    C09_START_V3_R_BUTT_LH_Click();
});
function C09_START_V3_R_BUTT_LH_Click() {
    $("#C09_START_V3_Barcodebox").focus();
}
$("#C09_START_V3_R_BUTT_RH").click(function (e) {
    C09_START_V3_R_BUTT_RH_Click();
});
function C09_START_V3_R_BUTT_RH_Click() {
    $("#C09_START_V3_Barcodebox").focus();
}
$("#C09_START_V3_Buttonclose").click(function (e) {
    C09_START_V3_Buttonclose_Click();
});
function C09_START_V3_Buttonclose_Click() {
    C09_START_V3_FormClosed();
}
$("#C09_START_V3_Barcodebox").keydown(function (e) {
    if (e.keyCode == 13) {
        C09_START_V3_Barcodebox_KeyDown_1();
    }
});
function C09_START_V3_Barcodebox_KeyDown_1() {
    //$("#BackGround").css("display", "block");
    let IsCheck = true;
    let TEXT_BARCODE = $("#C09_START_V3_Barcodebox").val();
    $("#C09_START_V3_Barcodebox").val("");
    $("#C09_START_V3_Barcodebox").focus();
    TEXT_BARCODE = TEXT_BARCODE.toUpperCase().trim();
    if (TEXT_BARCODE.length < 35) {
        IsCheck = false;
        M.toast({ html: "BARCODE SCAN 을 잘못했습니다. Scan sai Barcode.", classes: 'red', displayLength: 6000 });
        $("#C09_START_V3_Barcodebox").val("");
        $("#C09_START_V3_Barcodebox").focus();
        $("#BackGround").css("display", "none");
    }
    if (IsCheck == true) {
        document.getElementById("C09_START_V3_Barcodebox").disabled = true;
        //$("#BackGround").css("display", "block");
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
                BaseResult.C09_START_V3_DGV_C09_13 = data.DGV_C09_13;
                let D3_JJ = BaseResult.C09_START_V3_DGV_C09_13.length;
                let BAR_NM_CHK = false;
                if (D3_JJ > 0) {
                    if (BaseResult.C09_START_V3_DGV_C09_13[0].DSCN_YN == "N") {
                        BAR_NM_CHK = true;
                    }
                    else {
                        IsCheck = false;
                        M.toast({ html: "이미 처리된 바코드. Đã xử lý Barcode trước đó", classes: 'red', displayLength: 6000 });
                        $("#C09_START_V3_Barcodebox").val("");
                        $("#C09_START_V3_Barcodebox").focus();
                    }
                }
                let F_LEAD_T = TEXT_BARCODE.substr(0, TEXT_BARCODE.indexOf("$$") - 1);
                let AAT = TEXT_BARCODE.substr(TEXT_BARCODE.indexOf("$$") + 2, 12);
                F_LEAD_T = TEXT_BARCODE.split("$$")[0];
                let BC_QTY = TEXT_BARCODE.split("$$")[1];

                if (IsCheck == true) {
                    let R_BUTT_LH = document.getElementById("C09_START_V3_R_BUTT_LH").checked;
                    if (R_BUTT_LH == true) {
                        let LH_LEAD_CHK = false;
                        for (let i = 0; i < BaseResult.C09_START_V3_DataGridView1.length; i++) {
                            if (F_LEAD_T == BaseResult.C09_START_V3_DataGridView1[i].BOM_LEAD_NO) {
                                if (BaseResult.C09_START_V3_DataGridView1[i].ERROR_CHK == "Y") {
                                    IsCheck = false;
                                    M.toast({ html: "ERROR PROOF 완료 상태. CHỨNG NHẬN LỖI đã hoàn thành", classes: 'red', displayLength: 6000 });
                                    $("#C09_START_V3_Barcodebox").val("");
                                    $("#C09_START_V3_Barcodebox").focus();
                                }
                                else {
                                    LH_LEAD_CHK = true;
                                    i = BaseResult.C09_START_V3_DataGridView1.length;
                                }
                            }
                        }
                        if (IsCheck == true) {
                            if (LH_LEAD_CHK == true) {
                                //$("#BackGround").css("display", "block");
                                let ORDER_NO_TEXT = $("#C09_START_V3_ORDER_NO_TEXT").val();
                                let MC_Name = $("#C09_START_V3_MCbox").val();
                                let BaseParameter = new Object();
                                BaseParameter = {
                                    ListSearchString: [],
                                    MC_NAME: MC_Name
                                }
                                BaseParameter.USER_ID = GetCookieValue("UserID");
                                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                                BaseParameter.ListSearchString.push(TEXT_BARCODE);
                                BaseParameter.ListSearchString.push(ORDER_NO_TEXT);
                                BaseParameter.ListSearchString.push(BC_QTY);
                                BaseParameter.ListSearchString.push(BAR_NM_CHK);
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
                                        if (IsCheck == true) {
                                            C09_START_V3_ORDER_LOAD(1);
                                        }
                                    }).catch((err) => {
                                        $("#BackGround").css("display", "none");
                                    })
                                });
                            }
                            else {
                                IsCheck = false;
                                M.toast({ html: "일치하는 바코드가 없습니다. Không có mã vạch phù hợp", classes: 'red', displayLength: 6000 });
                                $("#C09_START_V3_Barcodebox").val("");
                                $("#C09_START_V3_Barcodebox").focus();
                                $("#BackGround").css("display", "none");
                            }
                        }
                    }
                }
                if (IsCheck == true) {
                    let R_BUTT_RH = document.getElementById("C09_START_V3_R_BUTT_RH").checked;
                    if (R_BUTT_RH == true) {
                        let LH_LEAD_CHK = false;
                        for (let i = 0; i < BaseResult.C09_START_V3_DataGridView2.length; i++) {
                            if (F_LEAD_T == BaseResult.C09_START_V3_DataGridView2[i].BOM_LEAD_NO) {
                                if (BaseResult.C09_START_V3_DataGridView2[i].ERROR_CHK == "Y") {
                                    IsCheck = false;
                                    M.toast({ html: "ERROR PROOF 완료 상태. CHỨNG NHẬN LỖI đã hoàn thành", classes: 'red', displayLength: 6000 });
                                    $("#C09_START_V3_Barcodebox").val("");
                                    $("#C09_START_V3_Barcodebox").focus();
                                }
                                else {
                                    LH_LEAD_CHK = true;
                                    i = BaseResult.C09_START_V3_DataGridView2.length;
                                }
                            }
                        }
                        if (IsCheck == true) {
                            if (LH_LEAD_CHK == true) {
                                //$("#BackGround").css("display", "block");
                                let ORDER_NO_TEXT = $("#C09_START_V3_ORDER_NO_TEXT").val();
                                let MC_Name = $("#C09_START_V3_MCbox").val();

                                let BaseParameter = new Object();
                                BaseParameter = {
                                    ListSearchString: [],
                                    MC_NAME: MC_Name
                                }
                                BaseParameter.USER_ID = GetCookieValue("UserID");
                                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                                BaseParameter.ListSearchString.push(TEXT_BARCODE);
                                BaseParameter.ListSearchString.push(ORDER_NO_TEXT);
                                BaseParameter.ListSearchString.push(BC_QTY);
                                BaseParameter.ListSearchString.push(BAR_NM_CHK);
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
                                        if (IsCheck == true) {
                                            C09_START_V3_ORDER_LOAD(1);
                                        }
                                    }).catch((err) => {
                                        $("#BackGround").css("display", "none");
                                    })
                                });
                            }
                            else {
                                IsCheck = false;
                                M.toast({ html: "일치하는 바코드가 없습니다. Không có mã vạch phù hợp", classes: 'red', displayLength: 6000 });
                                $("#C09_START_V3_Barcodebox").val("");
                                $("#C09_START_V3_Barcodebox").focus();
                                $("#BackGround").css("display", "none");
                            }
                        }
                    }
                }
                document.getElementById("C09_START_V3_Barcodebox").disabled = false;
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                document.getElementById("C09_START_V3_Barcodebox").disabled = false;
                $("#BackGround").css("display", "none");
            })
        });
    }

    document.getElementById("C09_START_V3_Barcodebox").disabled = false;
}
$("#C09_START_V3_SPC1").click(function (e) {
    C09_START_V3_SPC1_Click();
});
function C09_START_V3_SPC1_Click() {
    $("#C09_SPC_INSP_TEXT").val("First");
    $("#C09_SPC").modal("open");
    C09_SPC_PageLoad();
}
$("#C09_START_V3_SPC2").click(function (e) {
    C09_START_V3_SPC2_Click();
});
function C09_START_V3_SPC2_Click() {
    let PO_QTY = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PO_QTY;
    let PERFORMN = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PERFORMN;
    let BUNDLE_SIZE = BaseResult.C09_START_V3_DGV_C09_ST_00[0].BUNDLE_SIZE;

    if (PO_QTY > 501) {
        if (PO_QTY / 2 <= PERFORMN) {
            $("#C02_SPC_Label10").val("Middle");
            $("#C02_SPC").modal("open");
            C09_SPC_PageLoad();
        }
    }
}
$("#C09_START_V3_SPC3").click(function (e) {
    C09_START_V3_SPC3_Click();
});
function C09_START_V3_SPC3_Click() {
    let PO_QTY = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PO_QTY;
    let PERFORMN = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PERFORMN;
    let BUNDLE_SIZE = BaseResult.C09_START_V3_DGV_C09_ST_00[0].BUNDLE_SIZE;

    let AA = PO_QTY - PERFORMN;
    let BB = AA - BUNDLE_SIZE;
    if (BB <= 0) {
        $("#C02_SPC_Label10").val("End");
        $("#C02_SPC").modal("open");
        C09_SPC_PageLoad();
    }
}
$("#C09_START_V3_BI_10").click(function () {
    C09_START_V3_Button5_Click();
});
function C09_START_V3_Button5_Click() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    let BI_RadioButton1 = document.getElementById("C09_START_V3_BI_RadioButton1").checked;
    if (BI_RadioButton1 == true) {
        Non_idx = 1;
    }
    let BI_RadioButton2 = document.getElementById("C09_START_V3_BI_RadioButton2").checked;
    if (BI_RadioButton2 == true) {
        Non_idx = 2;
    }
    let BI_RadioButton3 = document.getElementById("C09_START_V3_BI_RadioButton3").checked;
    if (BI_RadioButton3 == true) {
        Non_idx = 3;
    }
    let BI_RadioButton4 = document.getElementById("C09_START_V3_BI_RadioButton4").checked;
    if (BI_RadioButton4 == true) {
        Non_idx = 4;
    }
    let BI_RadioButton5 = document.getElementById("C09_START_V3_BI_RadioButton5").checked;
    if (BI_RadioButton5 == true) {
        Non_idx = 5;
    }
    let BI_RadioButton6 = document.getElementById("C09_START_V3_BI_RadioButton6").checked;
    if (BI_RadioButton6 == true) {
        Non_idx = 6;
    }
    let BI_RadioButton7 = document.getElementById("C09_START_V3_BI_RadioButton7").checked;
    if (BI_RadioButton7 == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E";
            Non_text_NM = document.getElementById("C09_START_V3_BI_RadioButton7Text").innerHTML;
            break;
    }


    $("#C09_STOP_Label5").val(Non_text);
    $("#C09_STOP_Label2").val(Non_text_NM);
    $("#C09_STOP").modal("open");
    C09_STOP_PageLoad();
}
$("#C09_START_V3_Buttonprint").click(function (e) {
    C09_START_V3_Buttonprint_Click();
});
function C09_START_V3_Buttonprint_Click() {
    let IsCheck = true;
    let MRUS = $("#C09_START_V3_MRUS").val();
    if (MRUS == "STAY") {
        IsCheck = false;
        alert("LEAD BARCODE 수량이 부족합니다. LEAD BARCODE Một lỗi đã xảy ra.");
    }
    if (IsCheck == true) {
        let IsC09_SPC_PageLoad = false;
        C09_START_V3_SPC_EXIT = true;
        let SPC1 = document.getElementById("C09_START_V3_SPC1").innerHTML;
        if (SPC1 == "First") {
            IsC09_SPC_PageLoad = true;
            $("#C09_SPC_INSP_TEXT").val("First");
            $("#C09_SPC").modal("open");
            C09_SPC_PageLoad();
        }
        let PO_QTY = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PO_QTY;
        let PERFORMN = BaseResult.C09_START_V3_DGV_C09_ST_00[0].PERFORMN;
        let BUNDLE_SIZE = BaseResult.C09_START_V3_DGV_C09_ST_00[0].BUNDLE_SIZE;

        if (PO_QTY > 500) {
            if (PO_QTY / 2 <= PERFORMN) {
                let SPC2 = document.getElementById("C09_START_V3_SPC2").innerHTML;
                if (SPC2 == "Middle") {
                    IsC09_SPC_PageLoad = true;
                    $("#C09_SPC_INSP_TEXT").val("Middle");
                    $("#C09_SPC").modal("open");
                    C09_SPC_PageLoad();
                }
            }
        }
        let A_CSPC = PO_QTY - PERFORMN;
        if (BUNDLE_SIZE >= A_CSPC) {
            let SPC3 = document.getElementById("C09_START_V3_SPC3").innerHTML;
            if (SPC3 == "End") {
                IsC09_SPC_PageLoad = true;
                $("#C09_SPC_INSP_TEXT").val("End");
                $("#C09_SPC").modal("open");
                C09_SPC_PageLoad();
            }
        }
        if (IsC09_SPC_PageLoad == false) {
            C09_START_V3_Buttonprint_ClickSub();
        }
    }

}
function C09_START_V3_Buttonprint_ClickSub() {
    let IsCheck = true;
    if (C09_START_V3_SPC_EXIT == false) {
        IsCheck = false;
        alert("검사 누락. Kiểm tra mất tích.");
        document.getElementById("C09_START_V3_Buttonprint").disabled = false;
        return;
    }
    if (IsCheck == true) {
        let Label7 = $("#C09_START_V3_Label7").val();
        let Label59 = $("#C09_START_V3_Label59").val();
        let ORDER_NO_TEXT = $("#C09_START_V3_ORDER_NO_TEXT").val();
        let partbox = $("#C09_START_V3_partbox").val();
        let MCbox = $("#C09_START_V3_MCbox").val();
        let Label48 = $("#C09_START_V3_Label48").val();
        let MCName = $("#C09_START_V3_MCbox").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
            MC_NAME: MCName
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DGV_C09_ST_00 = BaseResult.C09_START_V3_DGV_C09_ST_00;
        BaseParameter.DGV_C09_ST_BOM = BaseResult.C09_START_V3_DGV_C09_ST_BOM;
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
                BaseResult.C09_START_V3_DataGridView1 = [];
                BaseResult.C09_START_V3_DataGridView2 = [];
                C09_START_V3_DataGridView1Render();
                C09_START_V3_DataGridView2Render();
                if (data) {
                    if (data.Code) {
                        let url = data.Code;
                        OpenWindowByURL(url, 200, 200);
                    }
                }
                let WORK_QTY = Number(Label7) + Number(Label59);
                if (WORK_QTY >= Number(Label48)) {
                    IsCheck = false;
                    M.toast({ html: "작업을 종료하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                    C09_START_V3_Buttonclose_Click();
                }
                else {
                    M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                    C09_START_V3_ORDER_LOAD(0);
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(err);
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C09_START_V3_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_START_V3_DataGridView1) {
            if (BaseResult.C09_START_V3_DataGridView1.length > 0) {
                C09_START_V3_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_START_V3_DataGridView1.length; i++) {
                    if (BaseResult.C09_START_V3_DataGridView1[i].ERROR_CHK == "Y") {
                        HTML = HTML + "<tr onclick='C09_START_V3_DataGridView1_SelectionChanged(" + i + ")' style='background-color: lime;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='C09_START_V3_DataGridView1_SelectionChanged(" + i + ")' style='background-color: pink;'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView1[i].L_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView1[i].BOM_LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView1[i].BOM_BSIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView1[i].ERROR_CHK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView1[i].LEAD_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_START_V3_DataGridView1").innerHTML = HTML;
}
function C09_START_V3_DataGridView1_SelectionChanged(i) {
    C09_START_V3_DataGridView1RowIndex = i;
}
let C09_START_V3_DataGridView1Table = document.getElementById("C09_START_V3_DataGridView1Table");
C09_START_V3_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "L_COUNT";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_START_V3_DataGridView1, key, text, IsTableSort);
        C09_START_V3_DataGridView1Render();
    }
});
function C09_START_V3_DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_START_V3_DataGridView2) {
            if (BaseResult.C09_START_V3_DataGridView2.length > 0) {
                C09_START_V3_DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_START_V3_DataGridView2.length; i++) {
                    if (BaseResult.C09_START_V3_DataGridView2[i].ERROR_CHK == "Y") {
                        HTML = HTML + "<tr onclick='C09_START_V3_DataGridView2_SelectionChanged(" + i + ")' style='background-color: lime;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='C09_START_V3_DataGridView2_SelectionChanged(" + i + ")' style='background-color: pink;'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView2[i].L_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView2[i].BOM_LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView2[i].BOM_BSIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView2[i].ERROR_CHK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_START_V3_DataGridView2[i].LEAD_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_START_V3_DataGridView2").innerHTML = HTML;
}
function C09_START_V3_DataGridView2_SelectionChanged(i) {
    C09_START_V3_DataGridView2RowIndex = i;
}
let C09_START_V3_DataGridView2Table = document.getElementById("C09_START_V3_DataGridView2Table");
C09_START_V3_DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "L_COUNT";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_START_V3_DataGridView2, key, text, IsTableSort);
        C09_START_V3_DataGridView2Render();
    }
});


let C09_SPC_DataGridView1RowIndex;
let C09_SPC_Timer;
function C09_SPC_PageLoad() {

    $("#C09_SPC_MRUS").val("-");

    C09_START_V3_SPC_EXIT = false;
    $("#C09_SPC_RUS_01").val("");
    $("#C09_SPC_RUS_02").val("");
    $("#C09_SPC_MRUS").val("");
    document.getElementById("C09_SPC_MRUS").style.color = "white";
    C09_SPC_Load();
}
function C09_SPC_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#C09_START_V3_partbox").val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_SPC/C09_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C09_SPC_Search = data.Search;
            BaseResult.C09_SPC_DataGridView1 = data.DataGridView1;
            C09_SPC_DataGridView1Render();
            $("#C09_SPC_RUS_TEXT_01").val("1");
            $("#C09_SPC_RUS_TEXT_02").val("1");
            if (BaseResult.C09_SPC_Search.length > 0) {
                if (BaseResult.C09_SPC_Search[0].Coln2) {
                    $("#C09_SPC_RUS_TEXT_01").val(BaseResult.C09_SPC_Search[0].Coln2);
                }
                if (BaseResult.C09_SPC_Search[0].Coln3) {
                    $("#C09_SPC_RUS_TEXT_02").val(BaseResult.C09_SPC_Search[0].Coln3);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C09_SPC_Buttonclose").click(function (e) {
    C09_SPC_Buttonclose_Click();
});
function C09_SPC_Buttonclose_Click() {
    $("#C09_SPC").modal("close");
    C09_START_V3_SPC_LOAD();
    //C09_START_V3_Buttonprint_ClickSub();
}
$("#C09_SPC_Button1").click(function (e) {
    C09_SPC_Button1_Click();
});
function C09_SPC_Button1_Click() {
    let IsCheck = true;
    let MRUS = $("#C09_SPC_MRUS").val();
    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (MRUS == "") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            let C09_START_V3_ORDER_NO_TEXT = $("#C09_START_V3_ORDER_NO_TEXT").val();
            let RUS_01 = $("#C09_SPC_RUS_01").val();
            let RUS_02 = $("#C09_SPC_RUS_02").val();
            let INSP_TEXT = $("#C09_SPC_INSP_TEXT").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(C09_START_V3_ORDER_NO_TEXT);
            BaseParameter.ListSearchString.push(RUS_01);
            BaseParameter.ListSearchString.push(RUS_02);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(INSP_TEXT);


            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C09_SPC/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    C09_START_V3_SPC_EXIT = true;
                    C09_SPC_Buttonclose_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
$("#C09_SPC_RUS_01").change(function (e) {
    C09_SPC_TextBox10_TextChanged();
});
function C09_SPC_TextBox10_TextChanged() {
    let RUS_01 = $("#C09_SPC_RUS_01").val();
    if (RUS_01 == "") {

    }
    else {
        C09_SPC_Form_RUS();
    }
}
$("#C09_SPC_RUS_02").change(function (e) {
    C09_SPC_TextBox1_TextChanged();
});
function C09_SPC_TextBox1_TextChanged() {
    let RUS_02 = $("#C09_SPC_RUS_02").val();
    if (RUS_02 == "") {

    }
    else {
        C09_SPC_Form_RUS();
    }
}
function C09_SPC_Form_RUS() {
    let Val1 = false;
    let Val2 = false;
    let RUS_TEXT_01 = Number($("#C09_SPC_RUS_TEXT_01").val());
    let RUS_01 = Number($("#C09_SPC_RUS_01").val());
    if (RUS_TEXT_01 < RUS_01) {
        Val1 = true;
    }
    let RUS_TEXT_02 = Number($("#C09_SPC_RUS_TEXT_02").val());
    let RUS_02 = Number($("#C09_SPC_RUS_02").val());
    if (RUS_TEXT_02 < RUS_02) {
        Val2 = true;
    }
    if (Val1 == true && Val2 == true) {
        $("#C09_SPC_MRUS").val("OK");
        document.getElementById("C09_SPC_MRUS").style.color = "green";
    }
    else {
        $("#C09_SPC_MRUS").val("NG");
        document.getElementById("C09_SPC_MRUS").style.color = "red";
    }
}
$("#C09_SPC_RUS_01").keydown(function (e) {
    if (e.keyCode == 13) {
        C09_SPC_RUS_01_KeyDown();
    }
});
function C09_SPC_RUS_01_KeyDown() {
    $("#C09_SPC_RUS_02").focus();
}
$("#C09_SPC_RUS_02").keydown(function (e) {
    if (e.keyCode == 13) {
        C09_SPC_RUS_02_KeyDown();
    }
});
function C09_SPC_RUS_02_KeyDown() {
    $("#C09_SPC_Button1").focus();
}
function C09_SPC_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C09_SPC_DataGridView1) {
            if (BaseResult.C09_SPC_DataGridView1.length > 0) {
                C09_SPC_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C09_SPC_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C09_SPC_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C09_SPC_DataGridView1[i].LEAD_PN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C09_SPC_DataGridView1[i].W_Diameter + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C09_SPC_DataGridView1").innerHTML = HTML;
}
function C09_SPC_DataGridView1_SelectionChanged(i) {
    C09_SPC_DataGridView1RowIndex = i;
}
let C09_SPC_DataGridView1Table = document.getElementById("C09_SPC_DataGridView1Table");
C09_SPC_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "LEAD_PN";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C09_SPC_DataGridView1, key, text, IsTableSort);
        C09_SPC_DataGridView1Render();
    }
});