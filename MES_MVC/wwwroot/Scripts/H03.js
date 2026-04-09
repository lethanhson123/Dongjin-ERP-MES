let BaseResult = {};
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let currentFilter = "ALL";
$(document).ready(function () {
    let now = new Date();
    let day = ("0" + now.getDate()).slice(-2);
    let month = ("0" + (now.getMonth() + 1)).slice(-2);
    let today = now.getFullYear() + "-" + month + "-" + day;
    $("#DateTimePicker1").val(today);

    Buttonfind_Click();
    $("#DateTimePicker1").on("change", function () {
        Buttonfind_Click();
    });

    $("#Button2").click(function () {
        Buttonfind_Click();
    });
    $("#Button1").click(function () {
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
    $("#MachineFilter").on("change", function () {
        currentFilter = $(this).val();
        applyFilter();
    });
});

function applyFilter() {
    if (currentFilter === "ALL") {
        $(".cutting-machine, .fa-machine").show();
    } else if (currentFilter === "CUTTING") {
        $(".cutting-machine").show();
        $(".fa-machine").hide();
    } else if (currentFilter === "FA") {
        $(".cutting-machine").hide();
        $(".fa-machine").show();
    }
    recalculateSum();
}

function recalculateSum() {
    $("#DataGridView1Body tr, #DataGridView2Body tr").each(function () {
        let sum = 0;
        let $row = $(this);
        $row.find("td:not(:first-child):not(:last-child)").each(function () {
            let $cell = $(this);
            if ($cell.is(":visible") && $cell.text() !== "NO_WORKER" && $cell.text() !== "") {
                let value = parseInt($cell.text()) || 0;
                sum += value;
            }
        });
        $row.find("td:last-child").text(sum || "");
    });
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    BaseParameter.DateTimePicker1 = $("#DateTimePicker1").val();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttonfind_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            DataGridView2Render();
            applyFilter();
            $("#BackGround").css("display", "none");
        }).catch(() => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttonadd_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttonsave_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttondelete_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttoncancel_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttoninport_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttonexport_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H03/Buttonprint_Click";
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
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
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let row = BaseResult.DataGridView1[i];
            html += "<tr>";
            html += `<td>${row.Hour || ""}</td>`;
            html += `<td class="cutting-machine">${row.A801 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A802 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A803 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A804 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A805 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A806 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A807 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A808 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A809 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A810 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A811 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A812 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A813 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A814 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A815 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A816 || ""}</td>`;
            html += `<td class="fa-machine">${row.A830 || ""}</td>`;
            html += `<td class="fa-machine">${row.A831 || ""}</td>`;
            html += `<td class="fa-machine">${row.A832 || ""}</td>`;
            html += `<td class="fa-machine">${row.A833 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A501 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A502 || ""}</td>`;
            html += `<td>${row.SUM || ""}</td>`;
            html += "</tr>";
        }
    }
    $("#DataGridView1Body").html(html);
}

function DataGridView2Render() {
    let html = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            let row = BaseResult.DataGridView2[i];
            html += "<tr>";
            html += `<td>${row.Hour || ""}</td>`;
            html += `<td class="cutting-machine">${row.A801 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A802 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A803 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A804 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A805 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A806 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A807 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A808 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A809 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A810 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A811 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A812 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A813 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A814 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A815 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A816 || ""}</td>`;
            html += `<td class="fa-machine">${row.A830 || ""}</td>`;
            html += `<td class="fa-machine">${row.A831 || ""}</td>`;
            html += `<td class="fa-machine">${row.A832 || ""}</td>`;
            html += `<td class="fa-machine">${row.A833 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A501 || ""}</td>`;
            html += `<td class="cutting-machine">${row.A502 || ""}</td>`;
            html += `<td>${row.SUM || ""}</td>`;
            html += "</tr>";
        }
    }
    $("#DataGridView2Body").html(html);
}
