let BaseResult = {};

$(function () {
    let now = new Date();
    let today = now.getFullYear() + "-" + ("0" + (now.getMonth() + 1)).slice(-2) + "-" + ("0" + now.getDate()).slice(-2);
    $("#DateTimePicker1").val(today);
    Buttonfind_Click();

    $("#Button2, #Button1").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);
    $("#DateTimePicker1").change(Buttonfind_Click);
});

function Buttonfind_Click() {
    $("#BackGround").show();
    let BaseParameter = { DateTimePicker1: $("#DateTimePicker1").val() };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/H07/Buttonfind_Click", { method: "POST", body: formUpload })
        .then(r => r.json())
        .then(data => {
            BaseResult = data;
            DataGridView1Render();
            DataGridView2Render();
            $("#BackGround").hide();
        })
        .catch(() => $("#BackGround").hide());
}

function Buttonadd_Click() { ButtonAction("/H07/Buttonadd_Click"); }
function Buttonsave_Click() { ButtonAction("/H07/Buttonsave_Click"); }
function Buttondelete_Click() { ButtonAction("/H07/Buttondelete_Click"); }
function Buttoncancel_Click() { ButtonAction("/H07/Buttoncancel_Click"); }
function Buttoninport_Click() { ButtonAction("/H07/Buttoninport_Click"); }
function Buttonexport_Click() { ButtonAction("/H07/Buttonexport_Click"); }
function Buttonprint_Click() { ButtonAction("/H07/Buttonprint_Click"); }
function ButtonAction(url) {
    $("#BackGround").show();
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch(url, { method: "POST", body: formUpload })
        .finally(() => $("#BackGround").hide());
}
function Buttonhelp_Click() { OpenWindowByURL("/WMP_PLAY", 800, 460); }
function Buttonclose_Click() { history.back(); }

function DataGridView1Render() {
    let html = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        BaseResult.DataGridView1.forEach(row => {
            html += "<tr>";
            html += `<td>${row.Hour || ""}</td>`;
            html += `<td>${row.ZA801 || ""}</td>`;
            html += `<td>${row.ZA802 || ""}</td>`;
            html += `<td>${row.ZA803 || ""}</td>`;
            html += `<td>${row.ZA804 || ""}</td>`;
            html += `<td>${row.ZA805 || ""}</td>`;
            html += `<td>${row.ZA806 || ""}</td>`;
            html += `<td>${row.ZA807 || ""}</td>`;
            html += `<td>${row.ZA808 || ""}</td>`;
            html += `<td>${row.ZA809 || ""}</td>`;
            html += `<td>${row.ZA810 || ""}</td>`;
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
            html += `<td>${row.ZA801 || ""}</td>`;
            html += `<td>${row.ZA802 || ""}</td>`;
            html += `<td>${row.ZA803 || ""}</td>`;
            html += `<td>${row.ZA804 || ""}</td>`;
            html += `<td>${row.ZA805 || ""}</td>`;
            html += `<td>${row.ZA806 || ""}</td>`;
            html += `<td>${row.ZA807 || ""}</td>`;
            html += `<td>${row.ZA808 || ""}</td>`;
            html += `<td>${row.ZA809 || ""}</td>`;
            html += `<td>${row.ZA810 || ""}</td>`;
            html += `<td>${row.SUM || ""}</td>`;
            html += "</tr>";
        });
    }
    $("#DataGridView2Body").html(html);
}
