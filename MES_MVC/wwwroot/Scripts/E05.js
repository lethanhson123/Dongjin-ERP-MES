let BaseResult = {};
let TagIndex = 1;
let FAC_ST = "";
let Now;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let T2_DGV1RowIndex = 0;

$(function () {
    let now = new Date();
    let day = ("0" + now.getDate()).slice(-2);
    let month = ("0" + (now.getMonth() + 1)).slice(-2);
    Now = now.getFullYear() + "-" + month + "-" + day;
    $("#DateTimePicker2, #T3_S2, #T2_S2").val(Now);
    $("#LB_DT, #LB_STG").val("-");
    LoadFactory();
});

$("#ATagTabPage1").click(function () { TagIndex = 1; });
$("#ATagTabPage2").click(function () { TagIndex = 2; });
$("#ATagTabPage3").click(function () { TagIndex = 3; });

$("#CB_FCTRY1").change(function () { SetFacSt($(this).val()); });
$("#CB_FCTRY2").change(function () { SetFacSt($(this).val()); });
$("#CB_FCTRY3").change(function () { SetFacSt($(this).val()); });

function SetFacSt(val) {
    FAC_ST = val === "Factory 1" ? " AND RIGHT(`CD_SYS_NOTE`, 2) <= 60 " : " AND RIGHT(`CD_SYS_NOTE`, 2) > 60 ";
}

$("#DateTimePicker2").change(function () { Buttonfind_Click(); });
$("#T2_S3, #T2_S4, #T3_S3").keydown(function (e) { if (e.keyCode === 13) Buttonfind_Click(); });

$("#RadioButton2").click(function () { RBchk0(true); });
$("#RadioButton1").click(function () { RBchk0(false); });
$("#rbchk1").click(function () { RBchk(true); });
$("#rbchk2").click(function () { RBchk(false); });

function RBchk0(val) {
    let d = BaseResult.DataGridView3 || [];
    let n = d.length > 9 ? 9 : d.length;
    for (let i = 0; i < n; i++) d[i].CHK = val;
    DataGridView3Render();
}

function RBchk(val) {
    let d = BaseResult.DataGridView4 || [];
    for (let i = 0; i < d.length; i++) d[i].CHK = val;
    DataGridView4Render();
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
    $("#BackGround").show();
    let url = "/E05/Buttonfind_Click";
    let BaseParameter = { Action: TagIndex, ListSearchString: [] };
    if (TagIndex === 1) {
        BaseParameter.ListSearchString.push($("#CB_FCTRY1").val());
        BaseParameter.ListSearchString.push($("#DateTimePicker2").val());
    }
    if (TagIndex === 2) {
        BaseParameter.ListSearchString.push($("#CB_FCTRY2").val());
        BaseParameter.ListSearchString.push($("#T2_S2").val());
        BaseParameter.ListSearchString.push($("#T2_S3").val());
        BaseParameter.ListSearchString.push($("#T2_S4").val());
        BaseParameter.ListSearchString.push($("#T2_S6").val());
    }
    if (TagIndex === 3) {
        BaseParameter.ListSearchString.push($("#CB_FCTRY3").val());
        BaseParameter.ListSearchString.push($("#T3_S2").val());
        BaseParameter.ListSearchString.push($("#T3_S3").val());
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch(url, { method: "POST", body: formUpload })
        .then(res => res.json())
        .then(data => {
            if (TagIndex === 1) {
                BaseResult.DataGridView2 = data.DataGridView2;
                DataGridView2Render();
            }
            if (TagIndex === 2) {
                BaseResult.T2_DGV1 = data.T2_DGV1;
                T2_DGV1Render();
            }
            if (TagIndex === 3) {
                BaseResult.DataGridView4 = data.DataGridView4;
                DataGridView4Render();
            }
            $("#BackGround").hide();
        }).catch(() => { $("#BackGround").hide(); });
}

function Buttonadd_Click() { }

function Buttonsave_Click() {
    $("#BackGround").show();
    let url = "/E05/Buttonsave_Click";
    let BaseParameter = { Action: TagIndex, ListSearchString: [] };
    if (TagIndex === 1) {
        if ($("#LB_DT").val() === "-" || $("#LB_STG").val() === "-" || !(BaseResult.DataGridView3 && BaseResult.DataGridView3.length)) {
            $("#BackGround").hide();
            return;
        }
        BaseParameter.ListSearchString.push($("#LB_DT").val());
        BaseParameter.ListSearchString.push($("#LB_STG").val());
        BaseParameter.DataGridView3 = BaseResult.DataGridView3;
    }
    if (TagIndex === 3) {
        if (!(BaseResult.DataGridView4 && BaseResult.DataGridView4.length)) {
            $("#BackGround").hide();
            return;
        }
        BaseParameter.DataGridView4 = BaseResult.DataGridView4;
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch(url, { method: "POST", body: formUpload })
        .then(res => res.json())
        .then(data => {
            alert("Save success");
            Buttonfind_Click();
            $("#BackGround").hide();
        }).catch(() => { $("#BackGround").hide(); });
}

function Buttondelete_Click() {
    $("#BackGround").show();
    let url = "/E05/Buttondelete_Click";
    let BaseParameter = { Action: TagIndex, DataGridView4: BaseResult.DataGridView4 };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch(url, { method: "POST", body: formUpload })
        .then(res => res.json())
        .then(data => {
            alert("Delete success");
            Buttonfind_Click();
            $("#BackGround").hide();
        }).catch(() => { $("#BackGround").hide(); });
}

function Buttoncancel_Click() {
    if (TagIndex === 2) {
        $("#T2_S2").val(Now);
        $("#T2_S3, #T2_S4").val("");
        $("#T2_S6").prop("selectedIndex", 0);
    }
    if (TagIndex === 3) {
        $("#T3_S2").val(Now);
        $("#T3_S3").val("");
    }
}

function Buttoninport_Click() { }
function Buttonexport_Click() { }
function Buttonprint_Click() { }
function Buttonhelp_Click() { window.open("/WMP_PLAY", "", "width=800,height=460"); }
function Buttonclose_Click() { history.back(); }

function LoadFactory() {
    $("#CB_FCTRY1, #CB_FCTRY2, #CB_FCTRY3").empty();
    $("#BackGround").show();
    fetch("/E05/Load", { method: "POST", body: new FormData() })
        .then(res => res.json())
        .then(data => {
            BaseResult.CB_FCTRY1 = data.CB_FCTRY1;
            for (let i = 0; i < data.CB_FCTRY1.length; i++) {
                $("<option>").text(data.CB_FCTRY1[i].CD_SYS_NOTE)
                    .val(data.CB_FCTRY1[i].CD_SYS_NOTE).appendTo("#CB_FCTRY1");
                $("<option>").text(data.CB_FCTRY1[i].CD_SYS_NOTE)
                    .val(data.CB_FCTRY1[i].CD_SYS_NOTE).appendTo("#CB_FCTRY2");
                $("<option>").text(data.CB_FCTRY1[i].CD_SYS_NOTE)
                    .val(data.CB_FCTRY1[i].CD_SYS_NOTE).appendTo("#CB_FCTRY3");
            }
            $("#BackGround").hide();
        }).catch(() => { $("#BackGround").hide(); });
}

function DataGridView2Render() {
    let html = "";
    BaseResult.DataGridView3 = [];
    DataGridView3Render();
    let d = BaseResult.DataGridView2 || [];
    if (d.length > 0) {
        DataGridView2_SelectionChanged(0);
        for (let i = 0; i < d.length; i++) {
            html += "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
            html += "<td>" + d[i].DATE + "</td>";
            html += "<td>" + d[i].STAGE + "</td>";
            html += "</tr>";
        }
    }
    document.getElementById("DataGridView2Body").innerHTML = html;
}

function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
    let d = BaseResult.DataGridView2[i];
    $("#LB_DT").val(d.DATE);
    $("#LB_STG").val(d.STAGE);
    DataT2DGV_LOAD();
}

function DataT2DGV_LOAD() {
    $("#BackGround").show();
    let BaseParameter = { ListSearchString: [$("#LB_DT").val(), $("#LB_STG").val()] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/E05/DataT2DGV_LOAD", { method: "POST", body: formUpload })
        .then(res => res.json())
        .then(data => {
            BaseResult.DataGridView3 = data.DataGridView3;
            DataGridView3Render();
            $("#BackGround").hide();
        }).catch(() => { $("#BackGround").hide(); });
}

function DataGridView3Render() {
    let html = "";
    let d = BaseResult.DataGridView3 || [];
    if (d.length > 0) {
        DataGridView3_SelectionChanged(0);
        for (let i = 0; i < d.length; i++) {
            html += "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
            html += "<td><label><input id='DataGridView3CHK" + i + "' class='form-check-input' type='checkbox' " + (d[i].CHK ? "checked" : "") + " onclick='event.stopPropagation(); DataGridView3CHKChanged(" + i + ")'><span></span></label></td>";
            html += "<td>" + (d[i].CODE || "") + "</td>";
            html += "<td>" + (d[i].STAGE || "") + "</td>";
            html += "<td>" + (d[i].DATE || "") + "</td>";
            html += "<td>" + (d[i].DJG_CODE || "") + "</td>";
            html += "<td>" + (d[i].PART_NO || "") + "</td>";
            html += "<td>" + (d[i].PART_NAME || "") + "</td>";
            html += "<td>" + (d[i].FAMILY || "") + "</td>";
            html += "<td>" + (d[i].SNP || "") + "</td>";
            html += "<td>" + (d[i].QTY || "") + "</td>";
            html += "<td>" + (d[i].STOCK || "") + "</td>";
            html += "</tr>";
        }
    }
    document.getElementById("DataGridView3Body").innerHTML = html;
}

function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
}
function DataGridView3CHKChanged(i) {
    BaseResult.DataGridView3[i].CHK = !BaseResult.DataGridView3[i].CHK;
}

function DataGridView4Render() {
    let d = BaseResult.DataGridView4 || [];
    let html = "";
    for (let i = 0; i < d.length; i++) {
        html += "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
        html += "<td><input type='checkbox' " + (d[i].CHK ? "checked" : "") + " onclick='event.stopPropagation(); DataGridView4CHKChanged(" + i + ")'></td>";
        html += "<td>" + (d[i].STAGE || "") + "</td>";
        html += "<td>" + (d[i].DSCN || "") + "</td>";
        html += "<td>" + (d[i].DATE || "") + "</td>";
        html += "<td>" + (d[i].DJG_CODE || "") + "</td>";
        html += "<td>" + (d[i].PART_NO || "") + "</td>";
        html += "<td>" + (d[i].PART_NAME || "") + "</td>";
        html += "<td>" + (d[i].FAMILY || "") + "</td>";
        html += "<td>" + (d[i].SNP || "") + "</td>";
        html += "<td>" + (d[i].QTY || "") + "</td>";
        html += "</tr>";
    }
    document.getElementById("DataGridView4Body").innerHTML = html;
}
function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}
function DataGridView4CHKChanged(i) {
    BaseResult.DataGridView4[i].CHK = !BaseResult.DataGridView4[i].CHK;
}

function T2_DGV1Render() {
    let d = BaseResult.T2_DGV1 || [];
    let html = "";
    for (let i = 0; i < d.length; i++) {
        html += "<tr onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
        html += "<td>" + (d[i].TMMTIN_CNF_YN || "") + "</td>";
        html += "<td>" + (d[i].TMMTIN_DSCN_YN || "") + "</td>";
        html += "<td>" + (d[i].STAGE || "") + "</td>";
        html += "<td>" + (d[i].PART_NO || "") + "</td>";
        html += "<td>" + (d[i].SNP || "") + "</td>";
        html += "<td>" + (d[i].QTY || "") + "</td>";
        html += "<td>" + (d[i].PART_NAME || "") + "</td>";
        html += "<td>" + (d[i].CREATE_DTM || "") + "</td>";
        html += "<td>" + (d[i].ORDER || "") + "</td>";
        html += "<td>" + (d[i].TMMTIN_SHEETNO || "") + "</td>";
        html += "</tr>";
    }
    document.getElementById("T2_DGV1Body").innerHTML = html;
}
function T2_DGV1_SelectionChanged(i) {
    T2_DGV1RowIndex = i;
}
