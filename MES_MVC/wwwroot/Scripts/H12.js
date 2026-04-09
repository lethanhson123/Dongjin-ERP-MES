let BaseResult = {};

$(document).ready(function () {
    let now = new Date();
    let day = ("0" + now.getDate()).slice(-2);
    let month = ("0" + (now.getMonth() + 1)).slice(-2);
    let today = now.getFullYear() + "-" + month + "-" + day;
    $("#DateTimePicker1").val(today);
    Buttonfind_Click();

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);
    $("#Button2").click(Buttonfind_Click);
    $("#Button1").click(Buttonfind_Click);
    $("#DateTimePicker1").change(Buttonfind_Click);
});

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { DateTimePicker1: $("#DateTimePicker1").val() };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/H12/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            DataGridView2Render();
            $("#BackGround").css("display", "none");
        }).catch(() => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonadd_Click() { ActionCall("/H12/Buttonadd_Click"); }
function Buttonsave_Click() { ActionCall("/H12/Buttonsave_Click"); }
function Buttondelete_Click() { ActionCall("/H12/Buttondelete_Click"); }
function Buttoncancel_Click() { ActionCall("/H12/Buttoncancel_Click"); }
function Buttoninport_Click() { ActionCall("/H12/Buttoninport_Click"); }
function Buttonexport_Click() { ActionCall("/H12/Buttonexport_Click"); }
function Buttonprint_Click() { ActionCall("/H12/Buttonprint_Click"); }
function Buttonhelp_Click() { OpenWindowByURL("/WMP_PLAY", 800, 460); }
function Buttonclose_Click() { history.back(); }

function ActionCall(url) {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(() => {
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function DataGridView1Render() {
    let html = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        BaseResult.DataGridView1.forEach(row => {
            html += "<tr>";
            html += `<td>${row.Hour || ""}</td>`;
            html += `<td>${row.A801 || ""}</td>`;
            html += `<td>${row.A802 || ""}</td>`;
            html += `<td>${row.A803 || ""}</td>`;
            html += `<td>${row.A804 || ""}</td>`;
            html += `<td>${row.A805 || ""}</td>`;
            html += `<td>${row.A806 || ""}</td>`;
            html += `<td>${row.A807 || ""}</td>`;
            html += `<td>${row.A808 || ""}</td>`;
            html += `<td>${row.A809 || ""}</td>`;
            html += `<td>${row.A810 || ""}</td>`;
            html += `<td>${row.A811 || ""}</td>`;
            html += `<td>${row.A812 || ""}</td>`;
            html += `<td>${row.A813 || ""}</td>`;
            html += `<td>${row.A814 || ""}</td>`;
            html += `<td>${row.A815 || ""}</td>`;
            html += `<td>${row.A816 || ""}</td>`;
            html += `<td>${row.A501 || ""}</td>`;
            html += `<td>${row.A502 || ""}</td>`;
            html += `<td>${row.SUM || ""}</td>`;
            html += "</tr>";
        });
    }
    $("#DataGridView1Body").html(html);
}

function DataGridView2Render() {
    let html = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        BaseResult.DataGridView2.forEach(row => {
            html += "<tr>";
            html += `<td>${row.Hour || ""}</td>`;
            html += `<td>${row.A801 || ""}</td>`;
            html += `<td>${row.A802 || ""}</td>`;
            html += `<td>${row.A803 || ""}</td>`;
            html += `<td>${row.A804 || ""}</td>`;
            html += `<td>${row.A805 || ""}</td>`;
            html += `<td>${row.A806 || ""}</td>`;
            html += `<td>${row.A807 || ""}</td>`;
            html += `<td>${row.A808 || ""}</td>`;
            html += `<td>${row.A809 || ""}</td>`;
            html += `<td>${row.A810 || ""}</td>`;
            html += `<td>${row.A811 || ""}</td>`;
            html += `<td>${row.A812 || ""}</td>`;
            html += `<td>${row.A813 || ""}</td>`;
            html += `<td>${row.A814 || ""}</td>`;
            html += `<td>${row.A815 || ""}</td>`;
            html += `<td>${row.A816 || ""}</td>`;
            html += `<td>${row.A501 || ""}</td>`;
            html += `<td>${row.A502 || ""}</td>`;
            html += `<td>${row.SUM || ""}</td>`;
            html += "</tr>";
        });
    }
    $("#DataGridView2Body").html(html);
}
