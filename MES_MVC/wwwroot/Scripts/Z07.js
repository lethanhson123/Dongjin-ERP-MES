let BaseResult;
let selectedRowIndex = -1;

$(document).ready(function () {
    Load();
    $("#RadioButton1, #RadioButton2").change(function () {
        updateInterface();
        Buttonfind_Click();
    });
});

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

function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            LoadComboBoxData();
            updateInterface();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}


function updateInterface() {
    if ($("#RadioButton1").prop("checked")) {
       
        $("#GroupBox1").find("h6").text("Development request");
        $("#Label1").text("Department : ");
        $("#Label2").show();
        $("#TextBox2").show();
        $("#Label3").show();
        $("#Label6").show();
        $("#ComboBox1").show();
        $("#ComboBox3").show();
        $("#TextBox1Container").hide();
    } else {
       
        $("#GroupBox1").find("h6").text("Development completed");
        $("#Label1").text("MES Ver : ");
        $("#Label2").hide();
        $("#TextBox2").hide();
        $("#Label3").hide();
        $("#Label6").hide();
        $("#ComboBox1").hide();
        $("#ComboBox3").hide();
        $("#TextBox1Container").hide();
    }
}
function LoadComboBoxData() {
    if (BaseResult && BaseResult.Success) {
        // Load ComboBox2 - MES Menu
        $("#ComboBox2").empty();
        $("#ComboBox2").append('<option value="ALL">ALL</option>');
        if (BaseResult.ComboBox2) {
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {
                $("#ComboBox2").append('<option value="' + BaseResult.ComboBox2[i].SCRN_PATH + '">' + BaseResult.ComboBox2[i].Name + '</option>');
            }
        }

        // Set default values for DateTimePicker1 and DateTimePicker2 (from and to dates)
        let today = new Date();
        let firstDay = new Date(today.getFullYear(), today.getMonth(), 1);

        $("#DateTimePicker1").val(formatDate(firstDay));
        $("#DateTimePicker2").val(formatDate(today));

        // Set default values for ComboBox1 (Result), ComboBox3 (State), ComboBox4 (Department)
        $("#ComboBox1").val("ALL");
        $("#ComboBox3").val("ALL");
        $("#ComboBox4").val("ALL");

        // Default mode selection
        $("#RadioButton1").prop("checked", true);
    }
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();

    // Get search parameters
    BaseParameter.RadioButton1 = $("#RadioButton1").prop("checked"); // true: Development request, false: Development completed
    BaseParameter.DateTimePicker1 = $("#DateTimePicker1").val(); // From date
    BaseParameter.DateTimePicker2 = $("#DateTimePicker2").val(); // To date
    BaseParameter.ComboBox2 = $("#ComboBox2").val(); // MES Menu
    BaseParameter.ComboBox4 = $("#ComboBox4").val(); // Department
    BaseParameter.TextBox2 = $("#TextBox2").val(); // User ID
    BaseParameter.ComboBox3 = $("#ComboBox3").val(); // State
    BaseParameter.ComboBox1 = $("#ComboBox1").val(); // Result
    BaseParameter.TextBox1 = $("#TextBox1").val(); // MES Version

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success && data.SuperResultTranfer) {
                clearForm();

                // Set values from service response
                $("#DateTimePicker3").val(formatDate(data.SuperResultTranfer.DELP_DATE));
                $("#ComboBox5").val(data.SuperResultTranfer.DELP_DEPT);
                $("#ComboBox6").val(data.SuperResultTranfer.MES_MENU);
                $("#TextBox3").val(data.SuperResultTranfer.DELP_NAME);
                $("#TextBox4").val(data.SuperResultTranfer.DELP_DETIL);
                $("#ComboBox7").val(data.SuperResultTranfer.FILE_DSYN);
                $("#TextBox5").val(data.SuperResultTranfer.FILE_NM);
                $("#ComboBox8").val(data.SuperResultTranfer.STATE);
                $("#TextBox6").val(data.SuperResultTranfer.PROGRESS);
                $("#ComboBox9").val(data.SuperResultTranfer.RESULT);

                // Set focus to first field
                $("#ComboBox5").focus();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonsave_Click() {
    if (!validateForm()) {
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];

    // Get form values
    BaseParameter.ListSearchString.push($("#HiddenField1").val() || "0"); // DELP_IDX
    BaseParameter.ListSearchString.push($("#DateTimePicker3").val()); // DELP_DATE
    BaseParameter.ListSearchString.push($("#ComboBox5").val()); // DELP_DEPT
    BaseParameter.ListSearchString.push($("#ComboBox6").val()); // MES_MENU
    BaseParameter.ListSearchString.push($("#TextBox3").val()); // DELP_NAME
    BaseParameter.ListSearchString.push($("#TextBox4").val()); // DELP_DETIL
    BaseParameter.ListSearchString.push($("#ComboBox7").val() || "N"); // FILE_DSYN
    BaseParameter.ListSearchString.push($("#TextBox5").val()); // FILE_NM
    BaseParameter.ListSearchString.push($("#HiddenField2").val()); // CREATE_USER
    BaseParameter.ListSearchString.push($("#ComboBox8").val()); // STATE
    BaseParameter.ListSearchString.push($("#TextBox6").val() || "0"); // PROGRESS
    BaseParameter.ListSearchString.push($("#ComboBox9").val()); // RESULT
    BaseParameter.ListSearchString.push($("#DateTimePicker4").val()); // DONE_DATE
    BaseParameter.ListSearchString.push($("#TextBox7").val()); // MES_VER

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                showMessage("정상처리 되었습니다.\nĐã được lưu.");
                clearForm();
                Buttonfind_Click();
            } else {
                showError(data.Error);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttondelete_Click() {
    if (!confirm("Confirm Delete?")) {
        return;
    }

    let delpIdx = $("#HiddenField1").val();
    if (!delpIdx || delpIdx === "0") {
        showError("Please select a record to delete.");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push(delpIdx);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                showMessage("정상처리 되었습니다.\nĐã được xóa.");
                clearForm();
                Buttonfind_Click();
            } else {
                showError(data.Error);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttoncancel_Click() {
    clearForm();
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let html = "";

    if (BaseResult && BaseResult.DataGridView1) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            html += "<tr onclick='selectRow(" + i + ")'>";
            html += "<td>" + (BaseResult.DataGridView1[i].BUTT || "") + "</td>";
            html += "<td>" + formatDate(BaseResult.DataGridView1[i].DELP_DATE) + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].DELP_DEPT || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].MES_MENU || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].DELP_NAME || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].FILE_DSYN || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].STATE || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].PROGRESS || "") + "%</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].RESULT || "") + "</td>";
            html += "<td>" + formatDate(BaseResult.DataGridView1[i].DONE_DATE) + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].MES_VER || "") + "</td>";
            html += "</tr>";
        }
    }

    $("#DataGridView1").html(html);
}

function selectRow(index) {
    selectedRowIndex = index;
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1[index]) {
        let item = BaseResult.DataGridView1[index];

        // Populate form fields with selected row data
        $("#HiddenField1").val(item.DELP_IDX || "0");
        $("#DateTimePicker3").val(formatDate(item.DELP_DATE));
        $("#ComboBox5").val(item.DELP_DEPT || "");
        $("#ComboBox6").val(item.MES_MENU || "");
        $("#TextBox3").val(item.DELP_NAME || "");
        $("#TextBox4").val(item.DELP_DETIL || "");
        $("#ComboBox7").val(item.FILE_DSYN || "N");
        $("#TextBox5").val(item.FILE_NM || "");
        $("#HiddenField2").val(item.CREATE_USER || "");
        $("#ComboBox8").val(item.STATE || "Waiting");
        $("#TextBox6").val(item.PROGRESS || "0");
        $("#ComboBox9").val(item.RESULT || "Waiting");
        $("#DateTimePicker4").val(formatDate(item.DONE_DATE));
        $("#TextBox7").val(item.MES_VER || "");
    }
}

function formatDate(dateString) {
    if (!dateString) return "";

    try {
        let date = new Date(dateString);
        return date.toISOString().split('T')[0];
    } catch (e) {
        return dateString;
    }
}

function validateForm() {
    if (!$("#DateTimePicker3").val()) {
        showError("Request Date is required.");
        return false;
    }

    if (!$("#ComboBox5").val()) {
        showError("Department is required.");
        return false;
    }

    if (!$("#ComboBox6").val()) {
        showError("MES Menu is required.");
        return false;
    }

    if (!$("#TextBox3").val()) {
        showError("Request Title is required.");
        return false;
    }

    return true;
}

function clearForm() {
    $("#HiddenField1").val("0");
    $("#DateTimePicker3").val(formatDate(new Date()));
    $("#ComboBox5").val("");
    $("#ComboBox6").val("");
    $("#TextBox3").val("");
    $("#TextBox4").val("");
    $("#ComboBox7").val("N");
    $("#TextBox5").val("");
    $("#HiddenField2").val("");
    $("#ComboBox8").val("Waiting");
    $("#TextBox6").val("0");
    $("#ComboBox9").val("Waiting");
    $("#DateTimePicker4").val("");
    $("#TextBox7").val("");
    selectedRowIndex = -1;
}

function showMessage(message) {
    alert(message);
}

function showError(message) {
    alert(message);
}