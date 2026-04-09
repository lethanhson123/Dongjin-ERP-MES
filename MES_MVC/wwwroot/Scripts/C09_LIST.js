let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let SHIELDWIRE_CHK = false;
let AA_1;
let BB_1;
$(document).ready(function () {
    AA_1 = localStorage.getItem("C09_LIST_AA_1");
    BB_1 = localStorage.getItem("C09_LIST_BB_1");
    var now = new Date();
    DateNow = DateToString(now);
    $("#DateTimePicker1").val(DateNow);
    $("#MCbox").val(SettingsMC_NM);
    Lan_Change();    
    DB_LISECHK();
    TS_USER(0);

    SHIELDWIRE_CHK = false;

    C09_LIST_Load();

    document.getElementById("rbchk2").checked = true;

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];

    Buttonfind_Click();
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
    let url = "/C09_LIST/MC_LIST";

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
function TS_USER(Flag) {
    let MCbox = $("#MCbox").val();
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
            let BaseResultSub = data;
            BaseResult.Search1 = BaseResultSub.Search1;
            if (Flag == 0) {
                ToolStripLOGIDX = BaseResult.Search1[0].TUSER_IDX;
                localStorage.setItem("C09_LIST_START_V3_BI_STIME", BaseResult.Search1[0].Name);
                localStorage.setItem("C09_LIST_START_V3_Label70", BaseResult.Search1[0].Description);
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
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    TS_USER(0);
    let CODE_MC = "";
    let ComboBox3 = document.getElementById("ComboBox3").selectedIndex;
    switch (ComboBox3) {
        case "0":
            CODE_MC = "S";
            break;
        case "1":
            CODE_MC = "I";
            break;
        case "2":
            CODE_MC = "Q";
            break;
        case "3":
            CODE_MC = "M";
            break;
        case "4":
            CODE_MC = "T";
            break;
        case "5":
            CODE_MC = "L";
            break;
        case "6":
            CODE_MC = "E";
            break;
    }
    localStorage.setItem("C09_LIST_STOP_Label5", CODE_MC);
    localStorage.setItem("C09_LIST_STOP_STOP_MC", $("#MCbox").val());
    localStorage.setItem("C09_LIST_STOP_Label2", $("#ComboBox3").val());

    let url = "/C09_LIST_STOP";
    OpenWindowByURL(url, 800, 460);
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
    Rbchk1_Click();
});
$("#rbchk2").click(function (e) {
    Rbchk2_Click();
});
function Rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = true;
        }
    }
    DataGridView1Render();
}
function Rbchk2_Click() {
    let rbchk2 = document.getElementById("rbchk2").checked;
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
    let url = "/C09_LIST_COUNT";
    OpenWindowByURL(url, 800, 460);
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
    let url = "/C09_LIST/Button1_Click_1";

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
        SearchString: AA_1,
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
        let url = "/C09_LIST/Buttonsave_Click";

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
            let url = "/C09_LIST/Buttondelete_Click";

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
    let url = "/C09_LIST/Buttoninport_Click";

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
    TableHTMLToExcel("DataGridView1Table", "C09_LIST", "C09_LIST");
}
function Buttonprint_Click() {
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    if (CHKA == "Stay") {

    }
    else {
        if (BaseResult.DataGridView1.length > 0) {
            let ORDER_NO = BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX;
            localStorage.setItem("C09_LIST_REPRINT_ORDER_NO", ORDER_NO);
            let url = "/C09_LIST_REPRINT";
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
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'>" + BaseResult.DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td onclick='DataGridView1_CellBeginEdit(" + i + ")'><input type='text' class='form-control' style='width: 100px;' value='" + BaseResult.DataGridView1[i].MC + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LS_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ORDER_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DSCN_YN + "</td>";
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
    SHIELDWIRE_CHK = false;

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
    let url = "/C09_LIST/DataGridView1_SelectionChanged";

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
            $("#Label2").val(DGV2_CNT);
            document.getElementById("PictureBox1").src = "";
            let SP_LD = SELECT_LN.substring(0, 2);
            if (SP_LD.includes("P")) {
                //Tạo Barcode;
                document.getElementById("PictureBox1").src = "URL";
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
function DataGridView1_CellBeginEdit(i) {
    DataGridView1RowIndex = i;
    if (BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION == "Stay") {
        BaseResult.DataGridView1[DataGridView1RowIndex].CHK = true;
        DataGridView1Render();
    }   
}
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    let W_CHK = false;
    switch (CHKA) {
        case "Complete":
            W_CHK = false;
            break;
        case "Close":
            W_CHK = false;
            break;
        case "Working":
            W_CHK = true;
            break;
        case "Stay":
            W_CHK = true;
            let XXX = 0;
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                XXX = XXX + 1;
            }
            if (XXX >= 3) {
                W_CHK = false;
                alert("이전 작업이 있습니다. Có một công việc trước đây.");
            }
            break;
    }
    if (W_CHK == true) {
        let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO;
        let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE;
        let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].PO_DT;
        let DDD = BaseResult.DataGridView1[DataGridView1RowIndex].PO_QTY;
        let FFF = BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX;
        let LLL = BaseResult.DataGridView1[DataGridView1RowIndex].MC;
        let MMM = BaseResult.DataGridView1[DataGridView1RowIndex].SAFTY_QTY;        

        localStorage.setItem("C09_START_V3_partbox", AAA);
        localStorage.setItem("C09_START_V3_ORDER_NO_TEXT", FFF);
        localStorage.setItem("C09_START_V3_Label7", BBB);
        let url = "/C09_START_V3";        
        OpenWindowByURL(url, 800, 460);
        Buttonfind_Click();
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
