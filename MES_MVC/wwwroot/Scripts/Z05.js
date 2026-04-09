let BaseResult;
let selectedRowIndex = -1;

$(document).ready(function () {
    Load();
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
    let url = "/Z05/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            LoadComboBoxData();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function LoadComboBoxData() {
    if (BaseResult && BaseResult.Success) {
        // Load ComboBox1 - Department (Search)
        $("#ComboBox1").empty();
        $("#ComboBox1").append('<option value="ALL">ALL</option>');
        if (BaseResult.ComboBox1) {
            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
                $("#ComboBox1").append('<option value="' + BaseResult.ComboBox1[i].CD_SYS_NOTE + '">' + BaseResult.ComboBox1[i].CD_SYS_NOTE + '</option>');
            }
        }

        // Load ComboBox2 - Department (Edit)
        $("#ComboBox2").empty();
        if (BaseResult.ComboBox1) {
            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
                $("#ComboBox2").append('<option value="' + BaseResult.ComboBox1[i].CD_SYS_NOTE + '">' + BaseResult.ComboBox1[i].CD_SYS_NOTE + '</option>');
            }
        }
    }
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];

    // Get search parameters
    BaseParameter.ListSearchString.push($("#TextBox5").val() || ""); // Asset ID
    BaseParameter.ListSearchString.push($("#TextBox1").val() || ""); // Asset Name
    BaseParameter.ListSearchString.push($("#TextBox2").val() || ""); // Specification
    BaseParameter.ListSearchString.push($("#ComboBox1").val() || "ALL"); // Department
    BaseParameter.ListSearchString.push($("#TextBox3").val() || ""); // User
    BaseParameter.ListSearchString.push($("#ComboBox4").val() || "ALL"); // Status

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z05/Buttonfind_Click";

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
    let url = "/Z05/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success && data.SuperResultTranfer) {
                $("#TextBox4").val(data.SuperResultTranfer.ASSET_ID);
                $("#TextBox8").val("");
                $("#TextBox7").val("");
                $("#DateTimePicker4").val(formatDate(data.SuperResultTranfer.PURCHASE_DATE));
                $("#ComboBox2").val("");
                $("#TextBox6").val("");
                $("#DateTimePicker3").val(formatDate(data.SuperResultTranfer.USE_DATE));
                $("#ComboBox3").val("Y");
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
    BaseParameter.ListSearchString.push($("#TextBox4").val()); // Asset ID
    BaseParameter.ListSearchString.push($("#TextBox8").val()); // Asset Name
    BaseParameter.ListSearchString.push($("#TextBox7").val()); // Specification
    BaseParameter.ListSearchString.push($("#DateTimePicker4").val()); // Purchase Date
    BaseParameter.ListSearchString.push($("#ComboBox2").val()); // Department
    BaseParameter.ListSearchString.push($("#TextBox6").val()); // User
    BaseParameter.ListSearchString.push($("#DateTimePicker3").val()); // Use Start Date
    BaseParameter.ListSearchString.push($("#ComboBox3").val()); // Status

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z05/Buttonsave_Click";

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

    let assetID = $("#TextBox4").val();
    if (!assetID) {
        showError("Please select an asset to delete.");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push(assetID);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z05/Buttondelete_Click";

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

function Buttoncancel_Click() {
    clearForm();
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z05/Buttoninport_Click";

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
    let url = "/Z05/Buttonexport_Click";

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
    let url = "/Z05/Buttonprint_Click";

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
            html += "<td>" + (BaseResult.DataGridView1[i].ASSET_NAME || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].SPECIFICATION || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].PURCHASE_DATE || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].DEPARTMENT || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].USER_NAME || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].USE_DATE || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].ASSET_ID || "") + "</td>";
            html += "<td>" + (BaseResult.DataGridView1[i].STATUS || "") + "</td>";
            html += "</tr>";
        }
    }

    $("#DataGridView1").html(html);
}

function selectRow(index) {
    selectedRowIndex = index;
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1[index]) {
        let item = BaseResult.DataGridView1[index];
        $("#TextBox4").val(item.ASSET_ID || "");
        $("#TextBox8").val(item.ASSET_NAME || "");
        $("#TextBox7").val(item.SPECIFICATION || "");
        $("#DateTimePicker4").val(formatDate(item.PURCHASE_DATE));
        $("#ComboBox2").val(item.DEPARTMENT || "");
        $("#TextBox6").val(item.USER_NAME || "");
        $("#DateTimePicker3").val(formatDate(item.USE_DATE));
        $("#ComboBox3").val(item.STATUS || "Y");
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
    if (!$("#TextBox4").val()) {
        showError("Asset ID is required.");
        return false;
    }

    if (!$("#TextBox8").val()) {
        showError("Asset Name is required.");
        return false;
    }

    if (!$("#ComboBox2").val()) {
        showError("Department is required.");
        return false;
    }

    return true;
}

function clearForm() {
    $("#TextBox4").val("");
    $("#TextBox8").val("");
    $("#TextBox7").val("");
    $("#DateTimePicker4").val("");
    $("#ComboBox2").val("");
    $("#TextBox6").val("");
    $("#DateTimePicker3").val("");
    $("#ComboBox3").val("Y");
    selectedRowIndex = -1;
}

function showMessage(message) {
    alert(message);
}

function showError(message) {
    alert(message);
}

