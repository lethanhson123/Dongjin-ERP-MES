let IsTableSort = false;
let BaseResult;
let RowIndex;
let Timer;


$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {
    localStorage.setItem("C02_REPRINT_Close", 0);

    $("#ORDER_NO").val(localStorage.getItem("C02_REPRINT_ORDER_NO"));
    $("#Label4").val(localStorage.getItem("C02_REPRINT_Label4"));

    BaseResult = new Object();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];  

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
    let url = "/C02_REPRINT/C02_REPRINT_Load";

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
        PrintDocument1_PrintPage();
    }
}
function PrintDocument1_PrintPage() {
    let BARCODE_QR = $("#PRT_1").val();
    let PR1 = $("#PRT_20").val();   
    let PR4 = $("#PRT_6").val();
    let PR5 = $("#PRT_3").val();
    let PR6 = $("#PRT_9").val();
    let PR7 = $("#PRT_7").val();
    let PR8 = $("#PRT_8").val();
    let PR9 = $("#PRT_10").val();
    let PR10 = $("#PRT_15").val();
    let PR11 = $("#PRT_12").val();
    let PR12 = $("#PRT_17").val();
    let PR13 = $("#PRT_13").val();
    let PR14 = $("#PRT_18").val();
    let PR15 = $("#PRT_14").val();
    let PR16 = $("#PRT_19").val();
    let PR17 = $("#PRT_21").val();
    let PR20 = $("#PRT_2").val();
    let PR21 = $("#PRT_11").val();
    let PR22 = $("#PRT_16").val();
    let PR25 = $("#PRT_22").val();
    let PRT_3 = $("#PRT_3").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString:[],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(BARCODE_QR);
    BaseParameter.ListSearchString.push(PR1);
    BaseParameter.ListSearchString.push(PR4);
    BaseParameter.ListSearchString.push(PR5);
    BaseParameter.ListSearchString.push(PR6);
    BaseParameter.ListSearchString.push(PR7);
    BaseParameter.ListSearchString.push(PR8);
    BaseParameter.ListSearchString.push(PR9);
    BaseParameter.ListSearchString.push(PR10);
    BaseParameter.ListSearchString.push(PR11);
    BaseParameter.ListSearchString.push(PR12);
    BaseParameter.ListSearchString.push(PR13);
    BaseParameter.ListSearchString.push(PR14);
    BaseParameter.ListSearchString.push(PR15);
    BaseParameter.ListSearchString.push(PR16);
    BaseParameter.ListSearchString.push(PR17);
    BaseParameter.ListSearchString.push(PR20);
    BaseParameter.ListSearchString.push(PR21);
    BaseParameter.ListSearchString.push(PR22);
    BaseParameter.ListSearchString.push(PR25);
    BaseParameter.ListSearchString.push(PRT_3);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_REPRINT/PrintDocument1_PrintPage";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            if (BaseResultSub) {
                if (BaseResultSub.Code) {
                    let url = BaseResultSub.Code;
                    OpenWindowByURL(url, 200, 200);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C02_REPRINT_Close", 1);
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
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PROJECT + "</td>";                    
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SP_ST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_AF_QTY + "</td>";
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
    $("#PRT_4").val(BaseResult.DataGridView1[DataGridView1RowIndex].DT);
    $("#PRT_5").val(BaseResult.DataGridView1[DataGridView1RowIndex].MC);
    $("#PRT_6").val(BaseResult.DataGridView1[DataGridView1RowIndex].HOOK_RACK);
    $("#PRT_7").val(BaseResult.DataGridView1[DataGridView1RowIndex].TOT_QTY);
    $("#PRT_8").val(BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE);
    $("#PRT_9").val(BaseResult.DataGridView1[DataGridView1RowIndex].WIRE);    
    $("#PRT_10").val(BaseResult.DataGridView1[DataGridView1RowIndex].TERM1);
    $("#PRT_11").val(BaseResult.DataGridView1[DataGridView1RowIndex].STRIP1);
    $("#PRT_12").val(BaseResult.DataGridView1[DataGridView1RowIndex].SEAL1);
    $("#PRT_13").val(BaseResult.DataGridView1[DataGridView1RowIndex].CCH_W1);
    $("#PRT_14").val(BaseResult.DataGridView1[DataGridView1RowIndex].ICH_W1);
    $("#PRT_15").val(BaseResult.DataGridView1[DataGridView1RowIndex].TERM2);
    $("#PRT_16").val(BaseResult.DataGridView1[DataGridView1RowIndex].STRIP2);
    $("#PRT_17").val(BaseResult.DataGridView1[DataGridView1RowIndex].SEAL2);
    $("#PRT_18").val(BaseResult.DataGridView1[DataGridView1RowIndex].CCH_W2);          
    $("#PRT_19").val(BaseResult.DataGridView1[DataGridView1RowIndex].ICH_W2);          
    $("#PRT_20").val(BaseResult.DataGridView1[DataGridView1RowIndex].PROJECT);          
    $("#PRT_21").val(BaseResult.DataGridView1[DataGridView1RowIndex].SP_ST);          
    $("#PRT_22").val(BaseResult.DataGridView1[DataGridView1RowIndex].ADJ_AF_QTY);
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "TORDER_BARCODENM";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});