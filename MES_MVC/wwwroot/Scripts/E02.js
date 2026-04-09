let BaseResult;


$(document).ready(function () {
    
});

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
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E02/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json().then(data => {
        BaseResult = data;
        DataGridView1Render();
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert("Search failed: " + err.message);
        $("#BackGround").css("display", "none");
    }));
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    try {
        if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
            // Tạo tên file động theo định dạng E02_YYYYMMDD
            const today = new Date();
            const fileName = `E02_${today.getFullYear()}${(today.getMonth() + 1).toString().padStart(2, "0")}${today.getDate().toString().padStart(2, "0")}`;
            TableHTMLToExcel("DataGridView1Table", "E02", fileName);
        } else {
            alert("No data to export!");
        }
    } catch (err) {
        alert("Export failed: " + err.message);
    } finally {
        $("#BackGround").css("display", "none");
    }
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let rat = parseFloat(BaseResult.DataGridView1[i].RAT);
            let rowClass = rat > 100 ? "red-row" : rat > 90 ? "orange-row" : rat > 80 ? "yellow-row" : "";
            HTML += `<tr class="${rowClass}">`;
            HTML += `<td>${BaseResult.DataGridView1[i].CODE || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].APPLICATOR || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].SEQ || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].MAX_COUNT || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].TOTAL_COUNT || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].WORK_COUNT || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].FREE_COUNT || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].OVER_COUNT || ""}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].RAT || ""}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
let IsTableSort = false;
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}

function Buttonadd_Click() { $("#BackGround").css("display", "none"); }
function Buttonsave_Click() { $("#BackGround").css("display", "none"); }
function Buttondelete_Click() { $("#BackGround").css("display", "none"); }
function Buttoncancel_Click() { $("#BackGround").css("display", "none"); }
function Buttoninport_Click() { $("#BackGround").css("display", "none"); }
function Buttonprint_Click() { $("#BackGround").css("display", "none"); }