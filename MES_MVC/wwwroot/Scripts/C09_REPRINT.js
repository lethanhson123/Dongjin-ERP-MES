let IsTableSort = false;
let BaseResult;
let RowIndex;
let Timer;


$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
    BaseResult = new Object();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    $("#ORDER_NO").val(localStorage.getItem("C09_REPRINT_ORDER_NO"));
    C02_REPRINT_Load();
});
function C02_REPRINT_Load() {
    let ORDER_NO = $("#ORDER_NO").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: ORDER_NO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_REPRINT/C02_REPRINT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
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
$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    let IsCheck = true;
    if (BaseResult.DataGridView1.length <= 0) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    let PRT_1 = $("#PRT_1").val();
    if (PRT_1.length < 8) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    if (IsCheck == true) {

    }
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    window.close();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";                 
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TORDER_BARCODENM + "</td>";                   
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SAFTY_QTY + "</td>";                    
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    $("#PRT_1").val(BaseResult.DataGridView1[DataGridView1RowIndex].TORDER_BARCODENM);
    $("#PRT_2").val(BaseResult.DataGridView1[DataGridView1RowIndex].Barcode_SEQ);
    $("#PRT_3").val(BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO);
    $("#PRT_4").val(BaseResult.DataGridView1[DataGridView1RowIndex].PO_DT);
    $("#PRT_5").val(BaseResult.DataGridView1[DataGridView1RowIndex].MC);
    $("#PRT_6").val(BaseResult.DataGridView1[DataGridView1RowIndex].HOOK_RACK);
    $("#PRT_7").val(BaseResult.DataGridView1[DataGridView1RowIndex].PO_QTY);
    $("#PRT_8").val(BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE);
    $("#PRT_9").val(BaseResult.DataGridView1[DataGridView1RowIndex].SAFTY_QTY);          
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});