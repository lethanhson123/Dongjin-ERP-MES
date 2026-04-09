let BaseResult;
let DataGridView1Data = [];
let selectedRowIndex = -1;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DateTimePicker1").val(today);

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

$("#ATag001").click(function () {
    switchTab("TabPage1");
});
$("#ATag002").click(function () {
    switchTab("TabPage2");
});
$("#ATag003").click(function () {
    switchTab("TabPage3");
});

function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z02/Load";

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
        $("#ComboBox1").empty();
        $("#ComboBox1").append('<option value="">Select Item....</option>');
        if (BaseResult.ComboBox1) {
            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
                $("#ComboBox1").append('<option value="' + BaseResult.ComboBox1[i].CD_NM_HAN + '">' + BaseResult.ComboBox1[i].CD_NM_HAN + '</option>');
            }
        }

        $("#ComboBox2").empty();
        $("#ComboBox2").append('<option value="">Select Item....</option>');

        $("#ComboBox3").empty();
        $("#ComboBox3").append('<option value="">Select Item....</option>');
        if (BaseResult.ComboBox3) {
            for (let i = 0; i < BaseResult.ComboBox3.length; i++) {
                $("#ComboBox3").append('<option value="' + BaseResult.ComboBox3[i].AUTH_NM + '">' + BaseResult.ComboBox3[i].AUTH_NM + '</option>');
            }
        }

        $("#ComboBox6").empty();
        $("#ComboBox6").append('<option value="">Select Item....</option>');
        if (BaseResult.ComboBox2) {
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {
                $("#ComboBox6").append('<option value="' + BaseResult.ComboBox2[i].AUTH_IDX + '">' + BaseResult.ComboBox2[i].AUTH_NM + '</option>');
            }
        }
    }
}

function Buttonfind_Click() {
    let currentTab = getCurrentTab();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];

    if (currentTab === "TabPage2") {
        BaseParameter.Action = 1;
        let dateValue = $('#DateTimePicker1').val();
        if (dateValue) {
            let date = new Date(dateValue);
            let yearMonth = date.getFullYear() + "-" + ("0" + (date.getMonth() + 1)).slice(-2);
            BaseParameter.ListSearchString.push(yearMonth);
        } else {
            BaseParameter.ListSearchString.push("");
        }
        BaseParameter.ListSearchString.push($('#ComboBox5').val() || "ALL");
        BaseParameter.ListSearchString.push($('#TextBox3').val() || "");
    } else if (currentTab === "TabPage3") {
        BaseParameter.Action = 2;
        let filterValue = $("#RadioButton1").is(":checked") ? "Pending" : "All";
        BaseParameter.ListSearchString.push(filterValue);
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z02/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (currentTab === "TabPage2") {
                DataGridView2Render();
            } else if (currentTab === "TabPage3") {
                DataGridView3Render();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonadd_Click() {
    let currentTab = getCurrentTab();
    if (currentTab !== "TabPage1") return;

    if (!validateAddForm()) return;

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.Action = 1;
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push($('#TextBox1').val());
    BaseParameter.ListSearchString.push($('#TextBox2').val());
    BaseParameter.ListSearchString.push($('#ComboBox1').val());
    BaseParameter.ListSearchString.push($('#ComboBox2').val());
    BaseParameter.ListSearchString.push($('#ComboBox3').val());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z02/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success && data.DataGridView1) {
                DataGridView1Data.push(data.DataGridView1[0]);
                DataGridView1Render();
                clearAddForm();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonsave_Click() {
    let currentTab = getCurrentTab();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();

    if (currentTab === "TabPage1") {
        BaseParameter.Action = 1;
        BaseParameter.DataGridView1 = DataGridView1Data;
    } else if (currentTab === "TabPage3") {
        if (!validateSaveForm()) {
            $("#BackGround").css("display", "none");
            return;
        }
        BaseParameter.Action = 2;
        BaseParameter.ListSearchString = [];
        BaseParameter.ListSearchString.push($('#Label7').text());
        BaseParameter.ListSearchString.push($('#TextBox5').val());
        BaseParameter.ListSearchString.push($('#TextBox6').val());
        BaseParameter.ListSearchString.push($('#TextBox8').val());
        BaseParameter.ListSearchString.push($('#ComboBox6').val());
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z02/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                if (currentTab === "TabPage1") {
                    DataGridView1Data = [];
                    DataGridView1Render();
                    showMessage("정상처리 되었습니다.\nĐã được lưu.");
                } else if (currentTab === "TabPage3") {
                    showMessage("Saving completed.");
                    Buttonfind_Click();
                }
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
    let currentTab = getCurrentTab();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();

    if (currentTab === "TabPage1") {
        if (selectedRowIndex >= 0 && selectedRowIndex < DataGridView1Data.length) {
            DataGridView1Data.splice(selectedRowIndex, 1);
            DataGridView1Render();
            selectedRowIndex = -1;
        }
        $("#BackGround").css("display", "none");
        return;
    }

    if (currentTab === "TabPage2") {
        if (!confirm("Confirm Delete?")) {
            $("#BackGround").css("display", "none");
            return;
        }

        let selectedRow = getSelectedRowDataGridView2();
        if (!selectedRow) {
            $("#BackGround").css("display", "none");
            return;
        }

        BaseParameter.Action = 2;
        BaseParameter.ListSearchString = [];
        BaseParameter.ListSearchString.push(selectedRow.REQU_DES);
        BaseParameter.ListSearchString.push(selectedRow.REQU_IDX);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/Z02/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                if (data.Success) {
                    showMessage("정상처리 되었습니다.\nĐã được lưu.");
                    Buttonfind_Click();
                } else if (data.Error === "ALREADY_APPROVED") {
                    showError("이미 승인된 내용은 삭제 할 수 없음.\nKhông thể xoá nội dung đã hoàn thành.");
                } else {
                    showError(data.Error);
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    } else {
        $("#BackGround").css("display", "none");
    }
}

function Buttoncancel_Click() {
    let currentTab = getCurrentTab();
    if (currentTab === "TabPage1") {
        clearAddForm();
    }
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z02/Buttoninport_Click";

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
    let url = "/Z02/Buttonexport_Click";

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
    let url = "/Z02/Buttonprint_Click";

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
    let HTML = "<thead><tr><th>User</th><th>Date</th><th>MES ID</th><th>Name</th><th>Department</th><th>Usage Purpose</th><th>Authority</th></tr></thead>";
    HTML += "<tbody>";

    for (let i = 0; i < DataGridView1Data.length; i++) {
        HTML += "<tr onclick='selectRow(" + i + ")'>";
        HTML += "<td>" + (DataGridView1Data[i].USER || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].DateRDCE || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].REQU_NID || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].REQU_NAME || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].REQU_DEP || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].REQU_NOTE || "") + "</td>";
        HTML += "<td>" + (DataGridView1Data[i].REQU_TSAUTH || "") + "</td>";
        HTML += "</tr>";
    }
    HTML += "</tbody>";

    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView2Render() {
    let HTML = "<thead><tr><th>REQU_DES</th><th>REQU_APP</th><th>REQU_DATE</th><th>REQU_NOTE</th><th>REQU_NID</th><th>REQU_NAME</th><th>REQU_DEP</th><th>REQU_TSAUTH</th><th>REQU_IDX</th></tr></thead>";
    HTML += "<tbody>";

    if (BaseResult && BaseResult.DataGridView2) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            HTML += "<tr onclick='selectDataGridView2Row(" + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_DES || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_APP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_DATE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_NOTE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_NID || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_DEP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_TSAUTH || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].REQU_IDX || "") + "</td>";
            HTML += "</tr>";
        }
    }
    HTML += "</tbody>";

    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView3Render() {
    let HTML = "<thead><tr><th>REQU_DES</th><th>REQU_APP</th><th>REQU_DATE</th><th>REQU_NOTE</th><th>REQU_NID</th><th>REQU_NAME</th><th>REQU_DEP</th><th>REQU_TSAUTH</th><th>REQU_IDX</th></tr></thead>";
    HTML += "<tbody>";

    if (BaseResult && BaseResult.DataGridView3) {
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            HTML += "<tr onclick='selectDataGridView3Row(" + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_DES || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_APP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_DATE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_NOTE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_NID || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_DEP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_TSAUTH || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].REQU_IDX || "") + "</td>";
            HTML += "</tr>";
        }
    }
    HTML += "</tbody>";

    document.getElementById("DataGridView3").innerHTML = HTML;
}

function selectRow(index) {
    selectedRowIndex = index;
}

function selectDataGridView2Row(index) {
    selectedRowIndex = index;
}

function selectDataGridView3Row(index) {
    if (BaseResult && BaseResult.DataGridView3 && BaseResult.DataGridView3[index]) {
        let item = BaseResult.DataGridView3[index];
        $('#Label7').text(item.REQU_IDX || "-");
        $('#TextBox5').val(item.REQU_NID || "");
        $('#TextBox6').val(item.REQU_NAME || "");
        $('#TextBox8').val(item.REQU_DEP || "");
        $('#TextBox7').val(item.REQU_TSAUTH || "");
    }
}

function getCurrentTab() {
    if ($("#TabPage1").is(":visible")) return "TabPage1";
    if ($("#TabPage2").is(":visible")) return "TabPage2";
    if ($("#TabPage3").is(":visible")) return "TabPage3";
    return "TabPage1";
}

function switchTab(tabName) {
    $("#TabPage1, #TabPage2, #TabPage3").hide();
    $("#" + tabName).show();

    $(".tab a").removeClass("active");
    if (tabName === "TabPage2") $("#ATag002").addClass("active");
    if (tabName === "TabPage1") $("#ATag001").addClass("active");
  
    if (tabName === "TabPage3") $("#ATag003").addClass("active");
}

function validateAddForm() {
    if (!$('#TextBox1').val()) {
        showError("MES ID 오류가 발생 하였습니다.\nMES ID  Một lỗi đã xảy ra.");
        return false;
    }
    if (!$('#TextBox2').val()) {
        showError("NAME 오류가 발생 하였습니다.\nNAME  Một lỗi đã xảy ra.");
        return false;
    }
    if (!$('#ComboBox1').val() || $('#ComboBox1').val() === "Select Item....") {
        showError("Department 오류가 발생 하였습니다.\nDepartment Một lỗi đã xảy ra.");
        return false;
    }
    if (!$('#ComboBox2').val() || $('#ComboBox2').val() === "Select Item....") {
        showError("사용목적 오류가 발생 하였습니다.\nMục đích sử dụng  Một lỗi đã xảy ra.");
        return false;
    }
    if (!$('#ComboBox3').val() || $('#ComboBox3').val() === "Select Item....") {
        showError("사용권한 오류가 발생 하였습니다.\nQuyền hạn sử dụng  Một lỗi đã xảy ra.");
        return false;
    }

    let mesId = $('#TextBox1').val();
    for (let i = 0; i < DataGridView1Data.length; i++) {
        if (DataGridView1Data[i].REQU_NID === mesId) {
            showError("MES ID 중복 오류가 발생 하였습니다.\nMES ID chồng lên nhau Một lỗi đã xảy ra.");
            clearAddForm();
            return false;
        }
    }
    return true;
}

function validateSaveForm() {
    if (!$('#TextBox5').val()) {
        showError("USER ID Error. Please check the error.");
        return false;
    }
    return true;
}

function clearAddForm() {
    $('#TextBox1').val("");
    $('#TextBox2').val("");
    $('#ComboBox1').val("");
    $('#ComboBox2').val("");
    $('#ComboBox3').val("");
}

function getSelectedRowDataGridView2() {
    if (BaseResult && BaseResult.DataGridView2 && selectedRowIndex >= 0 && selectedRowIndex < BaseResult.DataGridView2.length) {
        return BaseResult.DataGridView2[selectedRowIndex];
    }
    return null;
}

function showMessage(message) {
    alert(message);
}

function showError(message) {
    alert(message);
}

$("#TextBox1").keydown(function (e) {
    if (e.keyCode === 13) {
        $("#TextBox2").focus();
    }
});

switchTab("TabPage2");