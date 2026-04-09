let BaseResult;

$(document).ready(function () {
    let today = new Date().toISOString().split('T')[0];
    $("#DateTimePicker1").val(today);
    $("#DateTimePicker2").val(today);
    Load();
    $('.tabs').tabs();
    $('.tabs').on('click', function () {
        let activeTab = $('.tabs .active').attr('href');
        if (activeTab === "#TabPage1") {
        } else if (activeTab === "#TabPage2") {
        }
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
    fetch("/G04/Load", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Error) {
                alert(BaseResult.Error);
                $("#BackGround").css("display", "none");
                return;
            }
            FillComboBoxes(BaseResult);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function FillComboBoxes(data) {
    $("#ComboBox1").empty();
    $("#ComboBox2").empty();
    $("#ComboBox1").append('<option value="Select Line">Select Line</option>');
    $("#ComboBox2").append('<option value="Select Line">Select Line</option>');
    if (data.DGV_G04_CB1 && data.DGV_G04_CB1.length > 0) {
        data.DGV_G04_CB1.forEach(item => {
            $("#ComboBox1").append(`<option value="${item.PART_CAR}">${item.PART_CAR}</option>`);
            $("#ComboBox2").append(`<option value="${item.PART_CAR}">${item.PART_CAR}</option>`);
        });
    }
    $("#ComboBox1").val("Select Line");
    $("#ComboBox2").val("Select Line");
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let activeTab = $('.tabs .active').attr('href');
    let tabSelected = activeTab === "#TabPage1" ? "TabPage1" : "TabPage2";
    let BaseParameter = {
        TabSelected: tabSelected,
        TextBox1: $("#TextBox1").val(),
        TextBox2: $("#TextBox2").val(),
        ComboBox1: $("#ComboBox1").val(),
        ComboBox2: $("#ComboBox2").val(),
        DateTimePicker1: $("#DateTimePicker1").val(),
        DateTimePicker2: $("#DateTimePicker2").val()
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/G04/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Error) {
                alert(BaseResult.Error);
                $("#BackGround").css("display", "none");
                return;
            }
            if (tabSelected === "TabPage1") {
                DisplayDataGridView1(BaseResult);
            } else {
                DisplayDataGridView2(BaseResult);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function DisplayDataGridView1(data) {
    $("#DataGridView1").empty();
    $("#DataGridView1Table th:contains('W1')").text(data.WW1 + "WEEK");
    $("#DataGridView1Table th:contains('W2')").text(data.WW2 + "WEEK");
    $("#DataGridView1Table th:contains('W3')").text(data.WW3 + "WEEK");
    $("#DataGridView1Table th:contains('W4')").text(data.WW4 + "WEEK");
    $("#DataGridView1Table th:contains('W5')").text(data.WW5 + "WEEK");
    $("#DataGridView1Table th:contains('W6')").text(data.WW6 + "WEEK");
    $("#DataGridView1Table th:contains('DATE_STOCK')").text(data.AUTO_DT);
    data.DataGridView1.forEach(row => {
        let tr = $("<tr></tr>");
        if (row.ROW_NO && row.ROW_NO.startsWith("A")) {
            tr.css("background-color", "ivory");
            tr.css("font-weight", "bold");
        } else if (row.ROW_NO && row.ROW_NO.startsWith("B")) {
            tr.css("background-color", "lightgreen");
            tr.css("text-align", "center");
            tr.css("font-weight", "bold");
        }
        tr.append(`<td>${row.PART_CAR || ''}</td>`);
        tr.append(`<td>${row.PART_NO || ''}</td>`);
        tr.append(`<td>${row.PART_NM || ''}</td>`);
        tr.append(`<td>${row.PART_FML || ''}</td>`);
        tr.append(`<td>${row.PART_SNP1 || ''}</td>`);
        for (let i = 1; i <= 31; i++) {
            let dayKey = i < 10 ? `D0${i}` : `D${i}`;
            tr.append(`<td>${row[dayKey] || 0}</td>`);
        }
        tr.append(`<td>${row.P_SUM || 0}</td>`);
        tr.append(`<td>${row.DATE_STOCK || ''}</td>`);
        tr.append(`<td>${row.NOW_QTY || 0}</td>`);
        tr.append(`<td>${row.SHIP_SUM || 0}</td>`);
        tr.append(`<td>${row.W1 || 0}</td>`);
        tr.append(`<td>${row.W2 || 0}</td>`);
        tr.append(`<td>${row.W3 || 0}</td>`);
        tr.append(`<td>${row.W4 || 0}</td>`);
        tr.append(`<td>${row.W5 || 0}</td>`);
        tr.append(`<td>${row.W6 || 0}</td>`);
        $("#DataGridView1").append(tr);
    });
}

function DisplayDataGridView2(data) {
    $("#DataGridView2").empty();
    $("#DataGridView2Table th:contains('PW1')").text(data.WW1 + "WEEK");
    $("#DataGridView2Table th:contains('PW2')").text(data.WW2 + "WEEK");
    $("#DataGridView2Table th:contains('PW3')").text(data.WW3 + "WEEK");
    $("#DataGridView2Table th:contains('PW4')").text(data.WW4 + "WEEK");
    $("#DataGridView2Table th:contains('PW5')").text(data.WW5 + "WEEK");
    $("#DataGridView2Table th:contains('PW6')").text(data.WW6 + "WEEK");
    $("#DataGridView2Table th:contains('W1')").text(data.WW1 + "WEEK");
    $("#DataGridView2Table th:contains('W2')").text(data.WW2 + "WEEK");
    $("#DataGridView2Table th:contains('W3')").text(data.WW3 + "WEEK");
    $("#DataGridView2Table th:contains('W4')").text(data.WW4 + "WEEK");
    $("#DataGridView2Table th:contains('W5')").text(data.WW5 + "WEEK");
    $("#DataGridView2Table th:contains('W6')").text(data.WW6 + "WEEK");
    $("#DataGridView2Table th:contains('DATE_STOCK')").text(data.AUTO_DT);
    data.DataGridView2.forEach(row => {
        let tr = $("<tr></tr>");
        if (row.ROW_NO && row.ROW_NO.startsWith("A")) {
            tr.css("background-color", "ivory");
            tr.css("font-weight", "bold");
        } else if (row.ROW_NO && row.ROW_NO.startsWith("B")) {
            tr.css("background-color", "lightgreen");
            tr.css("text-align", "center");
            tr.css("font-weight", "bold");
        }
        tr.append(`<td>${row.PART_CAR || ''}</td>`);
        tr.append(`<td>${row.PART_NO || ''}</td>`);
        tr.append(`<td>${row.PART_NM || ''}</td>`);
        tr.append(`<td>${row.PART_FML || ''}</td>`);
        tr.append(`<td>${row.PART_SNP1 || ''}</td>`);
        tr.append(`<td>${row.PW1 || 0}</td>`);
        tr.append(`<td>${row.PW2 || 0}</td>`);
        tr.append(`<td>${row.PW3 || 0}</td>`);
        tr.append(`<td>${row.PW4 || 0}</td>`);
        tr.append(`<td>${row.PW5 || 0}</td>`);
        tr.append(`<td>${row.PW6 || 0}</td>`);
        tr.append(`<td>${row.P_SUM || 0}</td>`);
        tr.append(`<td>${row.DATE_STOCK || ''}</td>`);
        tr.append(`<td>${row.NOW_QTY || 0}</td>`);
        tr.append(`<td>${row.SHIP_SUM || 0}</td>`);
        tr.append(`<td>${row.W1 || 0}</td>`);
        tr.append(`<td>${row.W2 || 0}</td>`);
        tr.append(`<td>${row.W3 || 0}</td>`);
        tr.append(`<td>${row.W4 || 0}</td>`);
        tr.append(`<td>${row.W5 || 0}</td>`);
        tr.append(`<td>${row.W6 || 0}</td>`);
        $("#DataGridView2").append(tr);
    });
}

function DataGridView1Sort() {
}

function DataGridView2Sort() {
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttonadd_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttonsave_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttondelete_Click() {
    if (!confirm("Are you sure you want to delete this item?")) {
        return;
    }
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttondelete_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttoncancel_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttoninport_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttonexport_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G04/Buttonprint_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}
document.getElementById("Buttonexport").addEventListener("click", function () {
    const activeTab = $('.tabs .active').attr('href');

    if (activeTab === "#TabPage1") {
        const sheetName = "Month_DAY";
        const fileName = "Month_DAY.xlsx";
        TableHTMLToExcel('DataGridView1Table', fileName, sheetName);
    } else if (activeTab === "#TabPage2") {
        const sheetName = "Month_WEEK";
        const fileName = "Month_WEEK.xlsx";
        TableHTMLToExcel('DataGridView2Table', fileName, sheetName);
    } else {
        alert("No active tab selected.");
    }
});

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}