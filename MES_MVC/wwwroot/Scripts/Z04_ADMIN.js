let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let Now;

let EXCELIsTableSort = false;
let EXCELBaseResult = new Object();
let EXCELDataGridView1RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;

    $('.modal').modal();

    CB_DATASET();

    document.getElementById("TextBox1").readOnly = true;
    document.getElementById("TextBox15").readOnly = true;

    EXCELBaseResult.DataGridView1 = [];
    document.getElementById("EXCELRadioButton1").checked = true;
});

function CB_DATASET() {

}

$("#CB_01").change(function (e) {
    TextBox14_KeyDown();
});
function TextBox14_KeyDown() {
    $("#TextBox13").focus();
}
$("#TextBox13").keydown(function (e) {
    if (e.keyCode == 13) { TextBox13_KeyDown(); }
});
function TextBox13_KeyDown() {
    $("#TextBox13").focus();
}
$("#TextBox8").keydown(function (e) {
    if (e.keyCode == 13) { TextBox8_KeyDown(); }
});
function TextBox8_KeyDown() {
    $("#TextBox16").focus();
}
$("#TextBox16").keydown(function (e) {
    if (e.keyCode == 13) { TextBox16_KeyDown(); }
});
function TextBox16_KeyDown() {
    Buttonfind_Click();
    $("#TextBox2").focus();
}
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) { TextBox2_KeyDown(); }
});
function TextBox2_KeyDown() {
    $("#TextBox3").focus();
}
$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13) { TextBox3_KeyDown(); }
});
function TextBox3_KeyDown() {
    $("#TextBox5").focus();
}
$("#TextBox5").keydown(function (e) {
    if (e.keyCode == 13) { TextBox5_KeyDown(); }
});
function TextBox5_KeyDown() {
    $("#TextBox6").focus();
}

$("#Buttonfind").click(function () { Buttonfind_Click(); });
$("#Buttonadd").click(function () { Buttonadd_Click(); });
$("#Buttonsave").click(function () { Buttonsave_Click(); });
$("#Buttondelete").click(function () { Buttondelete_Click(); });
$("#Buttoncancel").click(function () { Buttoncancel_Click(); });
$("#Buttoninport").click(function () { Buttoninport_Click(); });
$("#Buttonexport").click(function () { Buttonexport_Click(); });
$("#Buttonprint").click(function () { Buttonprint_Click(); });
$("#Buttonhelp").click(function () { Buttonhelp_Click(); });
$("#Buttonclose").click(function () { Buttonclose_Click(); });

function Buttonfind_Click() {
    if (TagIndex == 1) {
        document.getElementById("TextBox1").readOnly = true;
        document.getElementById("TextBox15").readOnly = true;

        let MES_NO = $("#TextBox7").val();
        let DEP = $("#TextBox16").val();
        let S1 = $("#TextBox13").val();
        let S2 = $("#TextBox8").val();
        let AA = $("#CB_01").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = { Action: TagIndex, ListSearchString: [] };
        BaseParameter.ListSearchString.push(MES_NO, DEP, S1, S2, AA);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04_ADMIN/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Buttonadd_Click() {
    if (TagIndex == 1) {
        $("#BackGround").css("display", "block");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify({}));

        fetch("/Z04_ADMIN/Buttonadd_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("TextBox1").readOnly = false;
                document.getElementById("TextBox15").readOnly = false;
                $("#TextBox1").val("");
                $("#TextBox15").val(data.ErrorNumber);
                $("#TextBox2").val("");
                $("#TextBox3").val("");
                $("#TextBox4").val(GetCookieValue("UserID"));
                $("#TextBox5").val("");
                $("#TextBox6").val("");
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Buttonsave_Click() {
    if (TagIndex == 1) {
        let AA = $("#TextBox1").val();
        let BB = $("#TextBox15").val();
        if (AA == "" || BB == "") return;

        let CC = $("#TextBox2").val();
        let DD = $("#TextBox3").val();
        let EE = $("#TextBox4").val();
        let FF = $("#TextBox5").val();
        let GG = $("#TextBox6").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = { Action: TagIndex, ListSearchString: [] };
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AA, BB, CC, DD, EE, FF, GG);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04_ADMIN/Buttonsave_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                Buttonfind_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Buttondelete_Click() {

}

function Buttoncancel_Click() {
    if (TagIndex == 1) {
        document.getElementById("TextBox1").readOnly = true;
        document.getElementById("TextBox15").readOnly = true;
        $("#TextBox1, #TextBox15, #TextBox2, #TextBox3, #TextBox4, #TextBox5, #TextBox6").val("");
    }
}

function Buttoninport_Click() {
    $("#FileToUpload").val("");
    $("#FileToUpload").click();
}

function Buttonexport_Click() {
    if (TagIndex == 1) {
        TableHTMLToExcel("DataGridView1Table", "Z04_ADMIN", "Z04_ADMIN");
    }
}

function Buttonprint_Click() {

}

function Buttonhelp_Click() {
    OpenWindowByURL("/WMP_PLAY", 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        DataGridView1_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_YEAR + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_MESNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_DEPART + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_PKILOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INPUTER + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO1 + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO2 + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].COUNT + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    $("#TextBox1").val(BaseResult.DataGridView1[i].TSYEAR_YEAR);
    $("#TextBox15").val(BaseResult.DataGridView1[i].TSYEAR_MESNO);
    $("#TextBox2").val(BaseResult.DataGridView1[i].TSYEAR_DEPART);
    $("#TextBox3").val(BaseResult.DataGridView1[i].TSYEAR_PKILOC);
    $("#TextBox4").val(BaseResult.DataGridView1[i].TSYEAR_INPUTER);
    $("#TextBox5").val(BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO1);
    $("#TextBox6").val(BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO2);
}

// ==================== EXCEL MODAL ====================

$("#FileToUpload").change(function () {
    EXCEL_Buttoninport_Click();
});
$("#EXCELRadioButton1").click(function () { EXCEL_MES_CDD(); });
$("#EXCELRadioButton2").click(function () { EXCEL_MES_CDD(); });
$("#EXCELRadioButton3").click(function () { EXCEL_MES_CDD(); });

$("#Z04_ADMIN_EXCELRadioButton3OK").click(function () {
    let INPUT_A = $("#Z04_ADMIN_EXCELRadioButtonTextBox1").val();
    let INPUT_B = $("#Z04_ADMIN_EXCELRadioButtonTextBox15").val();
    $("#EXCELTextBox1").val(INPUT_A);
    $("#EXCELTextBox15").val(INPUT_B);
    $('#Z04_ADMIN_EXCELRadioButton3').modal('close');
});

$("#EXCELButtonsave").click(function () { EXCEL_Buttonsave_Click(); });
$("#EXCELButtondelete").click(function () { EXCEL_Buttondelete_Click(); });

function EXCEL_MES_CDD() {
    let RadioButton3 = document.getElementById("EXCELRadioButton3").checked;

    if (RadioButton3 == true) {
        $('#Z04_ADMIN_EXCELRadioButton3').modal('open');
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
        RadioButton1: document.getElementById("EXCELRadioButton1").checked,
        RadioButton2: document.getElementById("EXCELRadioButton2").checked,
        RadioButton3: false,
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/Z04_ADMIN_EXCEL/MES_CDD", {
        method: "POST", body: formUpload, headers: {}
    }).then((response) => {
        response.json().then((data) => {
            console.log("[MES_CDD] Response:", data);
            $("#EXCELTextBox1").val(data.Error);
            $("#EXCELTextBox15").val(data.ErrorNumber);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("[MES_CDD] Parse error:", err);
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}

function EXCEL_Buttoninport_Click() {
    var FileToUpload = $('#FileToUpload').prop('files');
    if (!FileToUpload || FileToUpload.length <= 0) return;

    console.log("[IMPORT] File selected:", FileToUpload[0].name);

    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    for (var i = 0; i < FileToUpload.length; i++) {
        formUpload.append('file[]', FileToUpload[i]);
    }

    fetch("/Z04_ADMIN_EXCEL/Buttoninport_Click", {
        method: "POST", body: formUpload, headers: {}
    }).then((response) => {
        response.json().then((data) => {
            console.log("[IMPORT] Server response:", data);
            console.log("[IMPORT] DataGridView1 count:", data.DataGridView1 ? data.DataGridView1.length : 0);
            if (data.Error && data.Error != "") {
                console.error("[IMPORT] Server error:", data.Error);
            }
            EXCELBaseResult = data;
            document.getElementById("EXCELRadioButton1").checked = true;
            $("#EXCELButtonsave").prop("disabled", false);
            EXCEL_MES_CDD();
            EXCEL_DataGridView1Render();
            $("#EXCELLabel3").val(EXCELBaseResult.DataGridView1.length);
            $("#BackGround").css("display", "none");
            $('#Z04_ADMIN_EXCEL_Modal').modal('open');
        }).catch((err) => {
            console.error("[IMPORT] Parse error:", err);
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}

function EXCEL_Buttonsave_Click() {
    console.log("[SAVE] EXCELBaseResult.DataGridView1:", EXCELBaseResult.DataGridView1);
    console.log("[SAVE] EXCELTextBox1:", $("#EXCELTextBox1").val());
    console.log("[SAVE] EXCELTextBox15:", $("#EXCELTextBox15").val());
    console.log("[SAVE] RadioButton3:", document.getElementById("EXCELRadioButton3").checked);
    console.log("[SAVE] USER_ID:", GetCookieValue("UserID"));

    if (!EXCELBaseResult.DataGridView1 || EXCELBaseResult.DataGridView1.length <= 0) {
        console.warn("[SAVE] Blocked: DataGridView1 is empty");
        return;
    }
    if ($("#EXCELTextBox1").val() == "") {
        console.warn("[SAVE] Blocked: EXCELTextBox1 is empty");
        return;
    }
    if ($("#EXCELTextBox15").val() == "") {
        console.warn("[SAVE] Blocked: EXCELTextBox15 is empty");
        return;
    }

    let AA = $("#EXCELTextBox1").val();
    let BB = $("#EXCELTextBox15").val();

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        Action: TagIndex,
        ListSearchString: [AA, BB],
        RadioButton3: document.getElementById("EXCELRadioButton3").checked,
        DataGridView1: EXCELBaseResult.DataGridView1,
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX"),
    };

    console.log("[SAVE] BaseParameter sending:", BaseParameter);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/Z04_ADMIN_EXCEL/Buttonsave_Click", {
        method: "POST", body: formUpload, headers: {}
    }).then((response) => {
        response.json().then((data) => {
            console.log("[SAVE] Server response:", data);
            if (data.Error && data.Error != "") {
                console.error("[SAVE] Server error:", data.Error);
                alert(data.Error);
                $("#BackGround").css("display", "none");
                return;
            }
            alert(localStorage.getItem("SaveSuccess"));
            $('#Z04_ADMIN_EXCEL_Modal').modal('close');
            Buttonfind_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("[SAVE] Parse error:", err);
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}

function EXCEL_Buttondelete_Click() {
    EXCELBaseResult.DataGridView1 = [];
    EXCEL_DataGridView1Render();
    $("#EXCELLabel3").val(0);
}

function EXCEL_DataGridView1Render() {
    let HTML = "";
    if (EXCELBaseResult && EXCELBaseResult.DataGridView1 && EXCELBaseResult.DataGridView1.length > 0) {
        EXCEL_DataGridView1_DataBindingComplete();
        EXCEL_DataGridView1_SelectionChanged(0);
        for (let i = 0; i < EXCELBaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='EXCEL_DataGridView1_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_MESNO + "</td>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_DEPART + "</td>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_PKILOC + "</td>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_INPUTER + "</td>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_SERIAL_NO1 + "</td>";
            HTML += "<td>" + EXCELBaseResult.DataGridView1[i].TSYEAR_SERIAL_NO2 + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("EXCELDataGridView1").innerHTML = HTML;
}

function EXCELDataGridView1Sort() {
    EXCELIsTableSort = !EXCELIsTableSort;
    DataGridViewSort(EXCELBaseResult.DataGridView1, EXCELIsTableSort);
    EXCEL_DataGridView1Render();
}

function EXCEL_DataGridView1_SelectionChanged(i) {
    EXCELDataGridView1RowIndex = i;
}

function EXCEL_DataGridView1_DataBindingComplete() {
    if (!EXCELBaseResult.DataGridView1 || EXCELBaseResult.DataGridView1.length <= 0) return;

    for (let i = 0; i < EXCELBaseResult.DataGridView1.length; i++) {
        let MES_NO_CHK = EXCELBaseResult.DataGridView1[i].TSYEAR_MESNO;
        for (let j = 0; j < EXCELBaseResult.DataGridView1.length; j++) {
            if (j == i) continue;
            if (MES_NO_CHK == EXCELBaseResult.DataGridView1[j].TSYEAR_MESNO) {
                document.getElementById("EXCELButtonsave").disabled = true;
                alert("MES NO 중복 발생. Phát sinh trùng MES NO. (Sự cố mạng Internet)");
                return;
            }
        }
    }
    document.getElementById("EXCELButtonsave").disabled = false;
}