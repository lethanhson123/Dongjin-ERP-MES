let BaseResult = {};
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;

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

    $("#DataGridView1").scroll(function () {
        $("#DataGridView2").scrollLeft($("#DataGridView1").scrollLeft());
    });
});

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    BaseParameter.DateTimePicker1 = $("#DateTimePicker1").val();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H10/Buttonfind_Click";
    fetch(url, {
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

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H10/Buttonadd_Click";
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
    let url = "/H10/Buttonsave_Click";
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
    let url = "/H10/Buttondelete_Click";
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
    let url = "/H10/Buttoncancel_Click";
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
    let url = "/H10/Buttoninport_Click";
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
    let url = "/H10/Buttonexport_Click";
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
    let url = "/H10/Buttonprint_Click";
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
            html += `<td>${row.Hour || ''}</td>`;
            html += `<td>${row.A801 || ''}</td>`;
            html += `<td>${row.A802 || ''}</td>`;
            html += `<td>${row.A803 || ''}</td>`;
            html += `<td>${row.A804 || ''}</td>`;
            html += `<td>${row.A805 || ''}</td>`;
            html += `<td>${row.A806 || ''}</td>`;
            html += `<td>${row.A807 || ''}</td>`;
            html += `<td>${row.A808 || ''}</td>`;
            html += `<td>${row.A809 || ''}</td>`;
            html += `<td>${row.A810 || ''}</td>`;
            html += `<td>${row.A811 || ''}</td>`;
            html += `<td>${row.A812 || ''}</td>`;
            html += `<td>${row.A813 || ''}</td>`;
            html += `<td>${row.A501 || ''}</td>`;
            html += `<td>${row.A502 || ''}</td>`;
            html += `<td>${row.Coln1 || ''}</td>`;
            html += `<td>${row.Coln2 || ''}</td>`;
            html += `<td>${row.Coln3 || ''}</td>`;
            html += `<td>${row.Coln4 || ''}</td>`;
            html += `<td>${row.Coln5 || ''}</td>`;
            html += `<td>${row.Coln6 || ''}</td>`;
            html += `<td>${row.Coln7 || ''}</td>`;
            html += `<td>${row.D01 || ''}</td>`;
            html += `<td>${row.D02 || ''}</td>`;
            html += `<td>${row.D03 || ''}</td>`;
            html += `<td>${row.D04 || ''}</td>`;
            html += `<td>${row.D05 || ''}</td>`;
            html += `<td>${row.D06 || ''}</td>`;
            html += `<td>${row.D07 || ''}</td>`;
            html += `<td>${row.D08 || ''}</td>`;
            html += `<td>${row.D09 || ''}</td>`;
            html += `<td>${row.D10 || ''}</td>`;
            html += `<td>${row.D11 || ''}</td>`;
            html += `<td>${row.D12 || ''}</td>`;
            html += `<td>${row.D13 || ''}</td>`;
            html += `<td>${row.D14 || ''}</td>`;
            html += `<td>${row.D15 || ''}</td>`;
            html += `<td>${row.D16 || ''}</td>`;
            html += `<td>${row.D17 || ''}</td>`;
            html += `<td>${row.D18 || ''}</td>`;
            html += `<td>${row.D19 || ''}</td>`;
            html += `<td>${row.D20 || ''}</td>`;
            html += `<td>${row.D21 || ''}</td>`;
            html += `<td>${row.SUM || ''}</td>`;
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
            html += `<td>${row.Hour || ''}</td>`;
            html += `<td>${row.A801 || ''}</td>`;
            html += `<td>${row.A802 || ''}</td>`;
            html += `<td>${row.A803 || ''}</td>`;
            html += `<td>${row.A804 || ''}</td>`;
            html += `<td>${row.A805 || ''}</td>`;
            html += `<td>${row.A806 || ''}</td>`;
            html += `<td>${row.A807 || ''}</td>`;
            html += `<td>${row.A808 || ''}</td>`;
            html += `<td>${row.A809 || ''}</td>`;
            html += `<td>${row.A810 || ''}</td>`;
            html += `<td>${row.A811 || ''}</td>`;
            html += `<td>${row.A812 || ''}</td>`;
            html += `<td>${row.A813 || ''}</td>`;
            html += `<td>${row.A501 || ''}</td>`;
            html += `<td>${row.A502 || ''}</td>`;
            html += `<td>${row.Coln1 || ''}</td>`;
            html += `<td>${row.Coln2 || ''}</td>`;
            html += `<td>${row.Coln3 || ''}</td>`;
            html += `<td>${row.Coln4 || ''}</td>`;
            html += `<td>${row.Coln5 || ''}</td>`;
            html += `<td>${row.Coln6 || ''}</td>`;
            html += `<td>${row.Coln7 || ''}</td>`;
            html += `<td>${row.D01 || ''}</td>`;
            html += `<td>${row.D02 || ''}</td>`;
            html += `<td>${row.D03 || ''}</td>`;
            html += `<td>${row.D04 || ''}</td>`;
            html += `<td>${row.D05 || ''}</td>`;
            html += `<td>${row.D06 || ''}</td>`;
            html += `<td>${row.D07 || ''}</td>`;
            html += `<td>${row.D08 || ''}</td>`;
            html += `<td>${row.D09 || ''}</td>`;
            html += `<td>${row.D10 || ''}</td>`;
            html += `<td>${row.D11 || ''}</td>`;
            html += `<td>${row.D12 || ''}</td>`;
            html += `<td>${row.D13 || ''}</td>`;
            html += `<td>${row.D14 || ''}</td>`;
            html += `<td>${row.D15 || ''}</td>`;
            html += `<td>${row.D16 || ''}</td>`;
            html += `<td>${row.D17 || ''}</td>`;
            html += `<td>${row.D18 || ''}</td>`;
            html += `<td>${row.D19 || ''}</td>`;
            html += `<td>${row.D20 || ''}</td>`;
            html += `<td>${row.D21 || ''}</td>`;
            html += `<td>${row.SUM || ''}</td>`;
            html += "</tr>";
        }
    }
    $("#DataGridView2Body").html(html);
}