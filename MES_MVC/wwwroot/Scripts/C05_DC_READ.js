let IsTableSort = false;
let BaseResult = new Object();
let COUNT_BL;
let BARCODE_TEXT;
$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("C05_DC_READ_Close", 1);
    window.close();
});

$(document).ready(function () {
    localStorage.setItem("C05_DC_READ_Close", 0);

    COUNT_BL = Boolean(localStorage.getItem("C05_DC_READ_COUNT_BL"));
    $("#Label2").val(localStorage.getItem("C05_DC_READ_Label2"));
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    DataGridView1Render();
    $("#Label4").val("-");
    $("#TB_BARCODE").val("");
    $("#TB_BARCODE").focus();
});
$("#TB_BARCODE").keydown(function (e) {
    if (e.keyCode == 13) {
        TB_BARCODE_KeyDown();
    }
});
function TB_BARCODE_KeyDown() {
    let IsCheck = true;
    let BARNM = $("#TB_BARCODE").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: BARNM,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_APPLICATION/TB_BARCODE_KeyDown";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search1 = BaseResultSub.Search1;
            let LIST_CHK = true;
            let TB_BARCODE = $("#TB_BARCODE").val();
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                if (TB_BARCODE == BaseResult.DataGridView1[i].Barcode) {
                    LIST_CHK = false;
                }
            }
            if (LIST_CHK == true) {
                if (BaseResult.Search1.length <= 0) {
                    IsCheck = false;
                    alert("NOT Order Barcode(Wrong). Please Check Again.");
                    $("#TB_BARCODE").val("");
                    $("#TB_BARCODE").focus();
                }
                if (IsCheck == true) {
                    let DSCN_YN = BaseResult.Search1[0].DSCN_YN;
                    if (DSCN_YN == "Y") {
                        IsCheck = false;
                        alert("이미 처리된 바코드. Đã xử lý Barcode trước đó");
                        $("#TB_BARCODE").val("");
                        $("#TB_BARCODE").focus();
                    }
                }
                let DGV_ORNO = BaseResult.Search1[0].ORDER_IDX;
                let DGV_T1 = BaseResult.Search1[0].TERM1;
                let DGV_T2 = BaseResult.Search1[0].TERM2;
                let DGV_LEAD = BaseResult.Search1[0].LEAD_NO;
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    let ORDER_IDX = BaseResult.DataGridView1[i].ORDER_IDX;
                    if (DGV_ORNO == ORDER_IDX) {
                        IsCheck = false;
                        alert("이미 처리된 ORDER NO. Đã xử lý ORDER NO trước đó");
                        $("#TB_BARCODE").val("");
                        $("#TB_BARCODE").focus();
                    }
                }
                let Label4 = $("#Label4").val();
                if (Label4 == "-") {
                    let Label2 = $("#Label2").val();
                    if (Label2 == "LH") {
                        $("#Label4").val(BaseResult.Search1[0].TERM1);
                    }
                    if (Label2 == "RH") {
                        $("#Label4").val(BaseResult.Search1[0].TERM2);
                    }

                    let DataGridView1Item = {
                        ORDER_IDX: DGV_ORNO,
                        TERM1: DGV_T1,
                        TERM2: DGV_T2,
                        LEAD_NO: DGV_LEAD,
                        Barcode: TB_BARCODE,
                    }
                    BaseResult.DataGridView1.push(DataGridView1Item);
                    DataGridView1Render();
                }
                else {
                    let Label2 = $("#Label2").val();
                    if (Label2 == "LH") {
                        if (Label4 == BaseResult.Search1[0].TERM1) {
                            let DataGridView1Item = {
                                ORDER_IDX: DGV_ORNO,
                                TERM1: DGV_T1,
                                TERM2: DGV_T2,
                                LEAD_NO: DGV_LEAD,
                                Barcode: TB_BARCODE,
                            }
                            BaseResult.DataGridView1.push(DataGridView1Item);
                            DataGridView1Render();
                        }
                        else {
                            IsCheck = false;
                            alert("NOT Order Barcode(Wrong). TERM 1. Please Check Again.");
                            $("#TB_BARCODE").val("");
                            $("#TB_BARCODE").focus();
                        }
                    }
                    if (Label2 == "RH") {
                        if (Label4 == BaseResult.Search1[0].TERM2) {
                            let DataGridView1Item = {
                                ORDER_IDX: DGV_ORNO,
                                TERM1: DGV_T1,
                                TERM2: DGV_T2,
                                LEAD_NO: DGV_LEAD,
                                Barcode: TB_BARCODE,
                            }
                            BaseResult.DataGridView1.push(DataGridView1Item);
                            DataGridView1Render();
                        }
                        else {
                            IsCheck = false;
                            alert("NOT Order Barcode(Wrong). TERM 2. Please Check Again.");
                            $("#TB_BARCODE").val("");
                            $("#TB_BARCODE").focus();
                        }
                    }
                }
            }
            else {
                alert("Check BarCode ID.   Please Check Again.");
            }

            if (IsCheck == true) {
                $("#TB_BARCODE").val("");
                $("#TB_BARCODE").focus();
            }
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
    if (BaseResult.DataGridView1.length >= 2) {
        let Label2 = $("#Label2").val();
        if (Label2 == "LH") {
            let C05_START_VLA1 = localStorage.getItem("C05_START_VLA1");
            if (C05_START_VLA1 == "") {
                IsCheck = false;
                alert("NOT ERROR PROOF. Please Check Again.");
                $("#TB_BARCODE").val();
            }
        }
        if (IsCheck == true) {
            if (Label2 == "RH") {
                let C05_START_VLA2 = localStorage.getItem("C05_START_VLA2");
                if (C05_START_VLA2 == "") {
                    IsCheck = false;
                    alert("NOT ERROR PROOF. Please Check Again.");
                    $("#TB_BARCODE").val();
                }
            }
        }
        if (IsCheck == true) {
            COUNT_BL = false;
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                if (i == 0) {
                    localStorage.setItem("C05_START_TB_BARCODE", BaseResult.DataGridView1[i].Barcode);
                    localStorage.setItem("C05_START_DC_STR", "DC");
                    //Call C05_START.BARCODE_LOAD()
                }
                if (COUNT_BL == true) {
                    if (i > 0) {
                        BARCODE_TEXT = BaseResult.DataGridView1[i].Barcode;
                        BARCODE_LOAD2();
                    }
                }
            }
            localStorage.setItem("C05_DC_READ_Close", 1);
            window.close();
        }
    }
    else {
        alert("Hai hoặc nhiều LEAD KHÔNG cần đăng ký. Please Check Again.");
    }
}

function BARCODE_LOAD2() {
    let IsCheck = true;
    let Label2 = $("#Label2").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(BARCODE_TEXT);
    BaseParameter.ListSearchString.push(Label2);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_APPLICATION/BARCODE_LOAD2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Search.length <= 0) {
                IsCheck = false;
                alert("NOT Order Barcode(Wrong). Please Check Again.");
                $("#TB_BARCODE").val("");
                $("#TB_BARCODE").focus();
            }
            if (IsCheck == true) {
                let DSCN_YN = BaseResult.Search[0].DSCN_YN;
                if (DSCN_YN == "Y") {
                    IsCheck = false;
                    alert("Đã xử lý Barcode trước đó. Đã xử lý Barcode trước đó");
                    $("#TB_BARCODE").val("");
                    $("#TB_BARCODE").focus();
                }
            }
            if (IsCheck == true) {
                if (Label2 == "LH") {

                }
                if (Label2 == "RH") {
                    if (BaseResult.Search1.length == 0) {
                        localStorage.setItem("C05_DC_READ_Close", 1);
                        window.close();
                    }
                }
            }
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
                DataGridView1_SelectionChanged(0);
                let RowCount = BaseResult.DataGridView1.length;
                for (let i = 0; i < RowCount; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ORDER_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Barcode + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});