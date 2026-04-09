let IsTableSort = false;
let BaseResult;

$(window).focus(function () {
}).blur(function () {
    window.close();
});

$(document).ready(function () {
    Label4 = localStorage.getItem("B08_REPRINT_Label4");
    $("#Label4").val(Label4);
    C02_REPRINT_Load();
});

$("#Buttonclose").click(function () {
    window.close();
});
$("#Buttonprint").click(function () {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.SuperResultTranfer = BaseResult.DataGridView1[DataGridView1RowIndex];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08_REPRINT/Buttonprint_Click";
    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultButtonprint = data;
            if (BaseResultButtonprint) {
                if (BaseResultButtonprint.Code) {
                    let url = BaseResultButtonprint.Code;
                    OpenWindowByURL(url, 1000, 200);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_REPRINT_Load() {
    $("#BackGround").css("display", "block");

    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#Label4").val()
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08_REPRINT/Load";
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
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0)
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TTC_BARCODENM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TTC_PO_DT + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    $("#PRT_BC").val(BaseResult.DataGridView1[DataGridView1RowIndex].TTC_BARCODENM);
    $("#PRT_PN").val(BaseResult.DataGridView1[DataGridView1RowIndex].TC_PART_NM);
    $("#PRT_DEC").val(BaseResult.DataGridView1[DataGridView1RowIndex].TC_DESC);
    $("#PRT_QTY").val(BaseResult.DataGridView1[DataGridView1RowIndex].QTY);
    $("#PRT_LOC").val(BaseResult.DataGridView1[DataGridView1RowIndex].TC_LOC);
    $("#PO_DATE").val(BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO_DT);
}