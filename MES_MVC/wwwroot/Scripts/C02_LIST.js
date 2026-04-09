let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let SHIELDWIRE_CHK;
let MCLIST_CHK;
let LEAD_NO_SK;
let CRT_DATE_SK;
$(document).ready(function () {

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];

    $("#MCbox").val(localStorage.getItem("C02_LIST_MCbox"));
    LEAD_NO_SK = localStorage.getItem("C02_LIST_LEAD_NO_SK");
    CRT_DATE_SK = localStorage.getItem("C02_LIST_CRT_DATE_SK");

    Lan_Change();

    $("#MCbox").val(localStorage.getItem("C02_MCbox"));

  //  DB_LISECHK();

    TS_USER(0);

    SHIELDWIRE_CHK = false;

    PageLoad();

    document.getElementById("rbchk2").checked = true;

    MCLIST_CHK = false;

    SetMCLIST_CHK();

    Buttonfind_Click();

});

/// cập nhật và đông đơn khi load UI
function DB_LISECHK() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/DB_LISECHK";

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

//cập nhật thông tin người dùng khi load form (sẽ cải tiến để khi vào chức năng thì cập nhật luôn mà không cần load nhiều lần)
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
    let url = "/C02_LIST/TS_USER";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultTS_USER = data;
            BaseResult.USER_TIME = BaseResultTS_USER.USER_TIME;
            if (BaseResult.USER_TIME) {
                if (BaseResult.USER_TIME.length > 0) {
                    let TUSER_IDX = BaseResult.USER_TIME[0].TUSER_IDX;
                    localStorage.setItem("ToolStripLOGIDX", TUSER_IDX);
                }
            }
            BaseResult.USER_TIME1 = BaseResultTS_USER.USER_TIME1;
            if (BaseResult.USER_TIME1) {
                if (BaseResult.USER_TIME1.length > 0) {
                    let S_DATE = BaseResult.USER_TIME1[0].TS_DATE;
                    localStorage.setItem("C02_START_V2_BI_STIME", BaseResult.USER_TIME1[0].Name);
                    localStorage.setItem("C02_START_V2_Label70", BaseResult.USER_TIME1[0].Description);
                }
            }
            if (Flag == 1) {
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
                localStorage.setItem("C02_STOP_Close", 0);
                localStorage.setItem("C02_STOP_Label5", CODE_MC);
                localStorage.setItem("C02_STOP_STOP_MC", $("#MCbox").val());
                localStorage.setItem("C02_STOP_Label2", $("#ComboBox3").val());

                localStorage.setItem("C02_MCbox", $("#MCbox").val());
                localStorage.setItem("C02_START_V2_Label8", "");

                let url = "/C02_STOP";
                OpenWindowByURL(url, 800, 460);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//load trang lần đầu tiên kho trang được tải UI (cần cải tiến để C02 load lên thì ui này ko cần laod lại)
function PageLoad() {
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
    let url = "/C02_LIST/PageLoad";

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
function SetMCLIST_CHK() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/SetMCLIST_CHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSetMCLIST_CHK = data;
            if (BaseResultSetMCLIST_CHK.DGV_C02_ML.length > 0) {
                MCLIST_CHK = true;
            }
            else {
                MCLIST_CHK = false;
            }
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
$("#rbchk1").click(function () {
    rbchk1_Click();
});

function rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = true;
        }
        DataGridView1Render();
    }
}
$("#rbchk2").click(function () {
    rbchk2_CheckedChanged();
});
function rbchk2_CheckedChanged() {
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
        }
        DataGridView1Render();
    }
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Button1_Click";

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
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    TS_USER(1);
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    localStorage.setItem("C02_COUNT_Close", 0);
    let url = "/C02_COUNT";
    OpenWindowByURL(url, 800, 460);
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(LEAD_NO_SK);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView1 = BaseResultSub.DataGridView1;
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            alert("Edit data Completed.");
            Buttonfind_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttondelete_Click() {
    if (confirm("Delete the selected Data?.")) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                alert("Delete Order Data Completed. ");
                Buttonfind_Click();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttoncancel_Click() {
    if (confirm("Cancel MT Request(Huỷ  Yêu Cầu vật NVL)?")) {
        let IDX_CHK = BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX;
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(IDX_CHK);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/Buttoncancel_Click";

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
    let url = "/C02_LIST/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            alert("Edit data Completed.");
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    TableHTMLToExcel("DataGridView1Table", "C02_LIST", "C02_LIST");
}
function Buttonprint_Click() {
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    if (CHKA == "Stay") {

    }
    else {
        if (BaseResult.DataGridView1.length > 0) {
            localStorage.setItem("C02_REPRINT_Close", 0);
            localStorage.setItem("C02_REPRINT_Label4", BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX);
            localStorage.setItem("C02_REPRINT_WHERE_TEXT", "");
            let url = "/C02_REPRINT";
            OpenWindowByURL(url, 800, 460);
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
                    if (BaseResult.DataGridView1[i].CHK == true) {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "'' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    if (BaseResult.DataGridView1[i].OR_NO == "") {
                        HTML = HTML + "<td style='background-color: green; padding-left: 20px; padding-right: 20px;'>" + BaseResult.DataGridView1[i].OR_NO + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: darkorange; padding-left: 20px; padding-right: 20px;'>" + BaseResult.DataGridView1[i].OR_NO + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td onclick='DataGridView1_CellClick3(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td onclick='DataGridView1_CellClick5(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].MTRL_RQUST + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOEXCEL_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td onclick='DataGridView1_CellBeginEdit(" + i + ")'><input type='number' class='form-control' value='" + BaseResult.DataGridView1[i].MC + "' style='width: 100px;'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LS_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";
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
    TS_USER(0);
    RE_TIME();
    SHIELDWIRE_CHK = false;
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
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}
function DataGridView1_CellClick5(i) {
    DataGridView1RowIndex = i;
    let IsCheck = true;
    if (MCLIST_CHK == true) {
        CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].MTRL_RQUST;
        if (CHKA == "N") {
            localStorage.setItem("C02_MT_ORDR", BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX);
            localStorage.setItem("C02_MT_Buttonsave_Enabled", true);

            localStorage.setItem("C02_MT_Close", 0);
            let url = "/C02_MT";
            OpenWindowByURL(url, 800, 460);
            C02_MTTimerStartInterval();
        }
        else {
            localStorage.setItem("C02_MT_ORDR", BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX);
            localStorage.setItem("C02_MT_Buttonsave_Enabled", false);

            localStorage.setItem("C02_MT_Close", 0);
            let url = "/C02_MT";
            OpenWindowByURL(url, 800, 460);
            C02_MTTimerStartInterval();
        }
    }
}
function DataGridView1_CellClick3(i) {
    DataGridView1RowIndex = i;
    let IsCheck = true;
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    if (CHKA == "Complete") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let XXX = 0;
        if (CHKA == "Working") {
        }
        else {
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                if (BaseResult.DataGridView1[i].CONDITION == "Working") {
                    XXX = XXX + 1;
                }
            }
            if (XXX >= 3) {
                IsCheck = false;
                alert("이전 작업이 있습니다. Có một công việc trước đây.");
            }
        }
    }
    if (IsCheck == true) {
        if (BaseResult.DataGridView1[DataGridView1RowIndex].MC == "SHIELD WIRE") {
            SHIELDWIRE_CHK = true;
        }
        let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO;
        let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE;
        let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].DT;
        let DDD = BaseResult.DataGridView1[DataGridView1RowIndex].TOT_QTY;
        let FFF = BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX;
        let GGG = BaseResult.DataGridView1[DataGridView1RowIndex].SP_ST;
        let HHH = BaseResult.DataGridView1[DataGridView1RowIndex].PROJECT;
        let LLL = BaseResult.DataGridView1[DataGridView1RowIndex].MC;
        let MMM = BaseResult.DataGridView1[DataGridView1RowIndex].ADJ_AF_QTY;

        SettingsMC_NM = localStorage.getItem("SettingsMC_NM");

        localStorage.setItem("C02_START_V2_Label4", AAA);
        localStorage.setItem("C02_START_V2_Label8", FFF);
        localStorage.setItem("C02_START_V2_Label42", GGG);
        localStorage.setItem("C02_START_V2_Label43", HHH);
        localStorage.setItem("C02_START_V2_L_MCNM", SettingsMC_NM);
        localStorage.setItem("C02_START_V2_Label48", DDD);
        localStorage.setItem("C02_START_V2_Label50", BBB);
        localStorage.setItem("C02_START_V2_Label77", MMM);
        localStorage.setItem("C02_ERROR_Label1", FFF);

        let ERROR_CHK = "";

        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(FFF);
        BaseParameter.ListSearchString.push(GGG);
        BaseParameter.ListSearchString.push(HHH);
        BaseParameter.ListSearchString.push(LLL);
        BaseParameter.ListSearchString.push(MMM);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/DataGridView1_CellClick3";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.Search = BaseResultSub.Search;
                if (BaseResult.Search.length <= 0) {
                    ERROR_CHK = "N";
                    localStorage.setItem("C02_START_V2_C_EXIT", false);
                }
                else {
                    ERROR_CHK = BaseResult.Search[0].ERROR_CHK;
                    localStorage.setItem("C02_START_V2_C_EXIT", false);
                }
                if (CHKA == "Complete") {

                }
                else {
                    if (ERROR_CHK == "Y") {
                        localStorage.setItem("C02_START_V2_C_EXIT", false);

                        localStorage.setItem("C02_START_V2_Close", 0);
                        let url = "/C02_START_V2";
                        OpenWindowByURL(url, 800, 460);
                        C02_START_V2TimerStartInterval();
                    }
                    else {
                        localStorage.setItem("C02_START_V2_C_EXIT", false);

                        localStorage.setItem("C02_ERROR_Close", 0);
                        let url = "/C02_ERROR";
                        OpenWindowByURL(url, 800, 460);
                        C02_ERRORTimerStartInterval();
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C02_START_V2TimerStartInterval() {
    let C02_START_V2Timer = setInterval(function () {
        let C02_START_V2_Close = localStorage.getItem("C02_START_V2_Close");
        if (C02_START_V2_Close == "1") {
            clearInterval(C02_START_V2Timer);
            Buttonfind_Click();
        }
    }, 100);
}
function C02_ERRORTimerStartInterval() {
    let C02_ERRORTimer = setInterval(function () {
        let C02_ERROR_Close = localStorage.getItem("C02_ERROR_Close");
        if (C02_ERROR_Close == "1") {
            clearInterval(C02_ERRORTimer);
            localStorage.setItem("C02_START_V2_Close", 0);
            let url = "/C02_START_V2";
            OpenWindowByURL(url, 800, 460);
            C02_START_V2TimerStartInterval();
        }
    }, 100);
}
function C02_MTTimerStartInterval() {
    let C02_MTTimer = setInterval(function () {
        let C02_MT_Close = localStorage.getItem("C02_MT_Close");
        if (C02_MT_Close == "1") {
            clearInterval(C02_MTTimer);
            Buttonfind_Click();
        }
    }, 100);
}

function DataGridView1_CellBeginEdit() {
    DataGridView1RowIndex = i;
    if (BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION == "Stay") {
        BaseResult.DataGridView1[DataGridView1RowIndex].CHK = true;
    }
    DataGridView1Render();
}
function DataGridView1_CellPainting() {
    DGV_PNT();
}
function DGV_PNT() {

}