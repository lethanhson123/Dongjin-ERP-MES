let IsTableSort = false;
let BaseResult = new Object();
let TabIndex = 1;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = -1;

$(document).ready(function () {
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = [];
    BaseResult.DataGridView3 = [];

    COMBO_LIST1();

    $("#ComboBox4").val("Factory 1");
    $("#RadioButton1").prop("checked", true);
});

$("#ATabPage1").click(function (e) {
    TabIndex = 1;
});

$("#ATabPage2").click(function (e) {
    TabIndex = 2;
    COMBO_LIST2();
});

$("#ATabPage3").click(function (e) {
    TabIndex = 3;
});

$("#ATabPage4").click(function (e) {
    TabIndex = 4;
});

$("#Buttonfind").click(function () {
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

$("#RadioButton1, #RadioButton2, #RadioButton3").change(function () {
    RadioButton1_CheckedChanged();
});

$("#ComboBox1").change(function () {
    ComboBox1_SelectedIndexChanged();
});

$("#ComboBox2").change(function () {
    if (TabIndex == 2) {
        Buttonfind_Click();
    }
});
$("#ComboBox3").change(function () {
    if (TabIndex == 3) {
        Buttonfind_Click();
    }
});

$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});

$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox2_KeyDown();
    }
});

$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13 && TabIndex == 4) {
        Buttonfind_Click();
    }
});

$("#TextBox5").keydown(function (e) {
    if (e.keyCode == 13 && TabIndex == 3) {
        Buttonfind_Click();
    }
});
$("#TextBox4").keydown(function (e) {
    if (e.keyCode == 13 && TabIndex == 3) {
        let memoText = $("#TextBox4").val();
        if (BaseResult.DataGridView6 && BaseResult.DataGridView6.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
                BaseResult.DataGridView6[i].ADD_2 = memoText;
            }
            DataGridView6Render();
        }
    }
});
$("#OR_NO_CHK, #CMPY_NO_CHK").change(function () {
    // Optionally refresh when radio group changes
});
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TabIndex,
        ListSearchString: []
    };

    if (TabIndex == 1) {
        BaseParameter.ListSearchString.push($("#TextBox1").val());
    } else if (TabIndex == 2) {
        BaseParameter.ListSearchString.push($("#ComboBox2").val());
    } else if (TabIndex == 3) {
        BaseParameter.ListSearchString.push($("#TextBox5").val());
    } else if (TabIndex == 4) {
        BaseParameter.ListSearchString.push($("#TextBox3").val());
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V02/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (TabIndex == 1) {
                BaseResult.DataGridView1 = data.DataGridView1 || [];
                $("#Label3").val("검색 항목 수량 : " + BaseResult.DataGridView1.length);
                DataGridView1Render();
            } else if (TabIndex == 2) {
                BaseResult.DataGridView3 = data.DataGridView3 || [];
                DataGridView3Render();
            } else if (TabIndex == 3) {
                BaseResult.DataGridView5 = data.DataGridView5 || [];
                $("#Label8").val("검색 항목 수량 : " + BaseResult.DataGridView5.length);
                DataGridView5Render();
            } else if (TabIndex == 4) {
                BaseResult.DataGridView4 = data.DataGridView4 || [];
                DataGridView4Render();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonsave_Click() {
    let isSave = false;
    let confirmMessage = "";
    let successMessage = "";

    if (TabIndex == 1 && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        isSave = true;
        successMessage = "정상처리 되었습니다.\nĐã được lưu.";
    } else if (TabIndex == 2) {
        isSave = true;
        confirmMessage = "주문을 확인하시겠습니까?\nBạn có muốn xác nhận đơn đặt hàng này?";
        successMessage = "정상처리 되었습니다.\nĐã được lưu.";
    } else if (TabIndex == 3 && BaseResult.DataGridView6 && BaseResult.DataGridView6.length > 0) {
        isSave = true;
        successMessage = "정상처리 되었습니다.\nĐã được lưu.";
    }

    if (!isSave) return;

    if (confirmMessage && !confirm(confirmMessage)) {
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TabIndex,
        ListSearchString: [],
        // Sử dụng hàm getCurrentUser() để lấy ID người dùng
        USER_IDX: getCurrentUser()
    };

    if (TabIndex == 1) {
        BaseParameter.DataGridView2 = BaseResult.DataGridView2;
        BaseParameter.ListSearchString.push($("#ComboBox1").val());
    } else if (TabIndex == 2) {
        BaseParameter.ListSearchString.push($("#ComboBox2").val());
    } else if (TabIndex == 3) {
        BaseParameter.DataGridView6 = BaseResult.DataGridView6;
    }

    let url = "/V02/Buttonsave_Click";
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (TabIndex == 1 && data.ErrorNumber === 0) {
                alert(successMessage);
                BaseResult.DataGridView2 = [];
                DataGridView2Render();
            } else if (TabIndex == 2 && data.ErrorNumber === 0) {
                COMBO_LIST2();
                BaseResult.DataGridView3 = [];
                DataGridView3Render();
                alert(successMessage);
            } else if (TabIndex == 3) {
                if (data.ErrorNumber === 0) {
                    alert(successMessage);
                    BaseResult.DataGridView6 = [];
                    DataGridView6Render();
                } else if (data.ErrorNumber === 1234) {
                    alert("CODE 오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                } else {
                    alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttondelete_Click() {
    if (TabIndex == 1) {
        if (DataGridView2RowIndex >= 0 && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > DataGridView2RowIndex) {
            if (confirm("삭제 하시겠습니까?\nBạn có muốn xóa?")) {
                BaseResult.DataGridView2.splice(DataGridView2RowIndex, 1);
                DataGridView2Render();
            }
        }
    } else if (TabIndex == 2) {
        if (!$("#ComboBox2").val()) {
            alert("주문 번호를 선택하세요.\nVui lòng chọn số đơn hàng.");
            return;
        }
        if (confirm("주문을 삭제하시겠습니까?\nBạn có muốn xóa đơn đặt hàng này?")) {
            $("#BackGround").css("display", "block");
            let BaseParameter = {
                Action: TabIndex,
                ListSearchString: [$("#ComboBox2").val()],
                USER_IDX: GetCookieValue("USER_IDX")
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/V02/Buttondelete_Click";
            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    if (data.ErrorNumber === 0) {
                        alert("정상처리 되었습니다.\nĐã được lưu.");
                        COMBO_LIST2();
                        BaseResult.DataGridView3 = [];
                        DataGridView3Render();
                    } else {
                        alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                    $("#BackGround").css("display", "none");
                });
            });
        }
    } else if (TabIndex == 4) {
        if (!$("#TextBox3").val()) {
            alert("주문 번호를 입력하세요.\nVui lòng nhập số đơn hàng.");
            return;
        }
        if (BaseResult.DataGridView4 && BaseResult.DataGridView4.length === 0) {
            alert("데이터가 없습니다.\nKhông có dữ liệu.");
            return;
        }
        if (confirm("삭제 하시겠습니까?\nBạn có muốn xóa?")) {
            $("#BackGround").css("display", "block");
            let BaseParameter = {
                Action: TabIndex,
                ListSearchString: [$("#TextBox3").val()],
                USER_IDX: GetCookieValue("USER_IDX"),
                OR_NO_CHK: $("#OR_NO_CHK").prop("checked"),
                CMPY_NO_CHK: $("#CMPY_NO_CHK").prop("checked"),
            };

            if ($("#CMPY_NO_CHK").prop("checked")) {
                let selectedIdx = DataGridView4RowIndex;
                if (selectedIdx === undefined || selectedIdx < 0) {
                    alert("제품을 선택하세요.\nVui lòng chọn sản phẩm.");
                    $("#BackGround").css("display", "none");
                    return;
                }
                BaseParameter.PDPUSCH_IDX = BaseResult.DataGridView4[selectedIdx].PDPUSCH_IDX;
            }

            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/V02/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    if (data.ErrorNumber === 0) {
                        alert("정상처리 되었습니다.\nĐã được lưu.");
                        Buttonfind_Click();
                    } else {
                        alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert("오류가 발생 하였습니다.\nMột lỗi đã xảy ra.");
                    $("#BackGround").css("display", "none");
                });
            });
        }
    }
}

function Buttoncancel_Click() {
    if (TabIndex == 1) {
        BaseResult.DataGridView2 = [];
        DataGridView2Render();
    } else if (TabIndex == 3) {
        BaseResult.DataGridView6 = [];
        DataGridView6Render();
    } else if (TabIndex == 4) {
        BaseResult.DataGridView4 = [];
        DataGridView4Render();
        $("#TextBox3").val("");
    }
}


function Buttoninport_Click() { }

function Buttonexport_Click() { }

function Buttonprint_Click() {
    let url = "/V02_1";
    OpenWindowByURL(url, 800, 600);
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function COMBO_LIST1() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    let url = "/V02/COMBO_LIST1";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.ComboBox1 = data.ComboBox1 || [];
            ComboBox1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function COMBO_LIST2() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    let url = "/V02/COMBO_LIST2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.ComboBox2 = data.ComboBox2 || [];
            ComboBox2Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function ComboBox1Render() {
    $("#ComboBox1").empty();
    if (BaseResult.ComboBox1 && BaseResult.ComboBox1.length > 0) {
        for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
            let displayValue = "";
            if (localStorage.getItem("UserLanguage") === "KR") {
                displayValue = BaseResult.ComboBox1[i].CD_NM_HAN;
            } else if (localStorage.getItem("UserLanguage") === "VN") {
                displayValue = BaseResult.ComboBox1[i].CD_NM_VN;
            } else {
                displayValue = BaseResult.ComboBox1[i].CD_SYS_NOTE;
            }

            let option = document.createElement("option");
            option.text = displayValue;
            option.value = BaseResult.ComboBox1[i].CD_IDX;
            document.getElementById("ComboBox1").add(option);
        }
    }
}

function ComboBox2Render() {
    $("#ComboBox2").empty();
    if (BaseResult.ComboBox2 && BaseResult.ComboBox2.length > 0) {
        for (let i = 0; i < BaseResult.ComboBox2.length; i++) {
            let option = document.createElement("option");
            option.text = BaseResult.ComboBox2[i].PDP_NO;
            option.value = BaseResult.ComboBox2[i].PDP_NO;
            document.getElementById("ComboBox2").add(option);
        }
        Buttonfind_Click();
    } else {
        BaseResult.DataGridView3 = [];
        DataGridView3Render();
    }
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr>";
            HTML += "<td onclick='DataGridView1_CellClick(" + i + ", 0)'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].ORDER + "</button></td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PN_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PDPART_IDX + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PN_V + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PSPEC_V + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].UNIT_VN + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PN_K + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PSPEC_K + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].UNIT_KR + "</td>";
          
            HTML += "<td>" + BaseResult.DataGridView1[i].STOCK + "</td>";
          
            HTML += "<td>" + BaseResult.DataGridView1[i].PQTY + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1Body").innerHTML = HTML;
    RadioButton1_CheckedChanged();
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            let selectedClass = (i === DataGridView2RowIndex) ? " class='selected-row'" : "";
            HTML += "<tr" + selectedClass + " onclick='DataGridView2_SelectionChanged(event, " + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView2[i].ADD_1 + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].DEP + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PN_V + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PSPEC_V + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].UNIT_VN + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PN_K + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PSPEC_K + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].UNIT_KR + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PQTY + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PN_NM + "</td>";
            HTML += "<td><input type='number' class='qty-input' data-row='" + i + "' value='" + BaseResult.DataGridView2[i].QTY + "' style='width:100%;'></td>";
            HTML += "<td><input type='text' class='purpose-input' data-row='" + i + "' value='" + BaseResult.DataGridView2[i].ADD_2 + "' style='width:100%;'></td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].ADD_3 + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView2Body").innerHTML = HTML;

    $(".qty-input").on('change', function () {
        let rowIndex = $(this).data('row');
        BaseResult.DataGridView2[rowIndex].QTY = $(this).val();
    });

    $(".purpose-input").on('change', function () {
        let rowIndex = $(this).data('row');
        BaseResult.DataGridView2[rowIndex].ADD_2 = $(this).val();
    });

    $(".qty-input, .purpose-input").on('click', function (e) {
        e.stopPropagation();
    });
}

function DataGridView3Render() {
    let HTML = "";
    if (BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            let selectedClass = (i === DataGridView3RowIndex) ? " class='selected-row'" : "";
            HTML += "<tr" + selectedClass + " onclick='DataGridView3_SelectionChanged(event, " + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_CONF || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_NO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_DATE1 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].DEPARTMENT || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PN_NM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PN_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PSPEC_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].UNIT_VN || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PN_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PSPEC_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].UNIT_KR || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PQTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_QTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_MEMO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].CREATE_DTM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].CREATE_USER || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].PDP_REMARK || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView3Body").innerHTML = HTML;
}
function DataGridView5Render() {
    let HTML = "";
    if (BaseResult.DataGridView5 && BaseResult.DataGridView5.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            HTML += "<tr>";
            HTML += "<td onclick='DataGridView5_CellClick(" + i + ", 0)'><button class='btn waves-effect waves-light grey darken-1'>" + (BaseResult.DataGridView5[i].ORDER || "") + "</button></td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].pdpart_IDX || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PN_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PSPEC_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].UNIT_VN || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PN_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PSPEC_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].UNIT_KR || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PQTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PN_NM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].STOCK || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PN_PHOTO || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView5Body").innerHTML = HTML;
}

function DataGridView6Render() {
    let HTML = "";
    if (BaseResult.DataGridView6 && BaseResult.DataGridView6.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
            HTML += "<tr>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].ADD_1 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].DEP_CODE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].DJG_CODE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PN_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PSPEC_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].UNIT_VN || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PN_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PSPEC_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].UNIT_KR || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PQTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].PN_NM || "") + "</td>";
            HTML += "<td><input type='number' class='qty-input6' data-row='" + i + "' value='" + (BaseResult.DataGridView6[i].QTY || "") + "' style='width:100%;'></td>";
            HTML += "<td><input type='text' class='purpose-input6' data-row='" + i + "' value='" + (BaseResult.DataGridView6[i].ADD_2 || "") + "' style='width:100%;'></td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].ADD_3 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView6[i].DEP_CODE2 || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView6Body").innerHTML = HTML;

    $(".qty-input6").on('change', function () {
        let rowIndex = $(this).data('row');
        BaseResult.DataGridView6[rowIndex].QTY = $(this).val();
    });

    $(".purpose-input6").on('change', function () {
        let rowIndex = $(this).data('row');
        BaseResult.DataGridView6[rowIndex].ADD_2 = $(this).val();
    });

    $(".qty-input6, .purpose-input6").on('click', function (e) {
        e.stopPropagation();
    });
}
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            let selectedClass = (i === DataGridView4RowIndex) ? " class='selected-row'" : "";
            HTML += "<tr" + selectedClass + " onclick='DataGridView4_SelectionChanged(event, " + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_CONF || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_NO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_DATE1 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].DEP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PN_NM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PN_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PSPEC_V || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].UNIT_VN || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PN_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PSPEC_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].UNIT_KR || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PQTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_QTY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_MEMO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].CREATE_DTM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].CREATE_USER || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDPUSCH_IDX || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView4Body").innerHTML = HTML;
}
function DataGridView4_SelectionChanged(e, rowIndex) {
    DataGridView4RowIndex = rowIndex;
    $("#DataGridView4Body tr").removeClass("selected-row");
    $(e.currentTarget).addClass("selected-row");
}

function DataGridView5_CellClick(rowIndex, colIndex) {
    let stockValue = BaseResult.DataGridView5[rowIndex].STOCK;
    if (stockValue > 0) {
        if (!confirm("There is stock. Would you like to order?\n재고가 있습니다. 오더 하시겠습니까?")) {
            return;
        }
    }

    let AA = $("#ComboBox3 option:selected").text();
    let AA_C = $("#ComboBox3").val();
    let BB = BaseResult.DataGridView5[rowIndex].pdpart_IDX;
    let CC = BaseResult.DataGridView5[rowIndex].PN_V;
    let DD = BaseResult.DataGridView5[rowIndex].PSPEC_V;
    let EE = BaseResult.DataGridView5[rowIndex].UNIT_VN;
    let FF = BaseResult.DataGridView5[rowIndex].PN_K;
    let GG = BaseResult.DataGridView5[rowIndex].PSPEC_K;
    let HH = BaseResult.DataGridView5[rowIndex].UNIT_KR;
    let II = BaseResult.DataGridView5[rowIndex].PQTY;
    let KK = BaseResult.DataGridView5[rowIndex].PN_NM;

    let newItem = {
        ADD_1: AA,
        DEP_CODE: AA_C,
        DJG_CODE: BB,
        PN_V: CC,
        PSPEC_V: DD,
        UNIT_VN: EE,
        PN_K: FF,
        PSPEC_K: GG,
        UNIT_KR: HH,
        PQTY: II,
        PN_NM: KK,
        QTY: "0",
        ADD_2: "",
        ADD_3: "Stay",
        DEP_CODE2: AA_C
    };

    if (!BaseResult.DataGridView6) {
        BaseResult.DataGridView6 = [];
    }

    BaseResult.DataGridView6.push(newItem);
    DataGridView6Render();
}

function DataGridView1_CellClick(rowIndex, colIndex) {
    DataGridView1RowIndex = rowIndex;

    let stockValue = BaseResult.DataGridView1[rowIndex].STOCK;
    if (stockValue > 0) {
        if (!confirm("There is stock. Would you like to order?\n재고가 있습니다. 오더 하시겠습니까?")) {
            return;
        }
    }

    let ZZ = $("#ComboBox4").val();
    let AA = $("#ComboBox1 option:selected").text();
    let AA_C = $("#ComboBox1").val();
    let BB = BaseResult.DataGridView1[rowIndex].PDPART_IDX;
    let CC = BaseResult.DataGridView1[rowIndex].PN_V;
    let DD = BaseResult.DataGridView1[rowIndex].PSPEC_V;
    let EE = BaseResult.DataGridView1[rowIndex].UNIT_VN;
    let FF = BaseResult.DataGridView1[rowIndex].PN_K;
    let GG = BaseResult.DataGridView1[rowIndex].PSPEC_K;
    let HH = BaseResult.DataGridView1[rowIndex].UNIT_KR;
    let II = BaseResult.DataGridView1[rowIndex].PQTY;
    let KK = BaseResult.DataGridView1[rowIndex].PN_NM;

    let newItem = {
        ADD_1: ZZ,
        DEP: AA,
        DEP_CODE: AA_C,
        DJG_CODE: BB,
        PN_V: CC,
        PSPEC_V: DD,
        UNIT_VN: EE,
        PN_K: FF,
        PSPEC_K: GG,
        UNIT_KR: HH,
        PQTY: II,
        PN_NM: KK,
        QTY: "0",
        ADD_2: "",
        ADD_3: "Stay"
    };

    if (!BaseResult.DataGridView2) {
        BaseResult.DataGridView2 = [];
    }

    BaseResult.DataGridView2.push(newItem);
    DataGridView2Render();
}

function DataGridView2_SelectionChanged(e, rowIndex) {
    if (!$(e.target).is('input')) {
        DataGridView2RowIndex = rowIndex;
        $("#DataGridView2Body tr").removeClass("selected-row");
        $(e.currentTarget).addClass("selected-row");
    }
}

function DataGridView3_SelectionChanged(e, rowIndex) {
    DataGridView3RowIndex = rowIndex;
    $("#DataGridView3Body tr").removeClass("selected-row");
    $(e.currentTarget).addClass("selected-row");
}

function RadioButton1_CheckedChanged() {
    let rb1 = $("#RadioButton1").prop("checked");
    let rb2 = $("#RadioButton2").prop("checked");
    let rb3 = $("#RadioButton3").prop("checked");

    if (rb1) {
        $("#DataGridView1 th:nth-child(3), #DataGridView1 td:nth-child(3)").show();
        $("#DataGridView1 th:nth-child(4), #DataGridView1 td:nth-child(4)").show();
        $("#DataGridView1 th:nth-child(5), #DataGridView1 td:nth-child(5)").show();
        $("#DataGridView1 th:nth-child(6), #DataGridView1 td:nth-child(6)").show();
        $("#DataGridView1 th:nth-child(7), #DataGridView1 td:nth-child(7)").show();
        $("#DataGridView1 th:nth-child(8), #DataGridView1 td:nth-child(8)").show();
    } else if (rb2) {
        $("#DataGridView1 th:nth-child(3), #DataGridView1 td:nth-child(3)").show();
        $("#DataGridView1 th:nth-child(4), #DataGridView1 td:nth-child(4)").hide();
        $("#DataGridView1 th:nth-child(5), #DataGridView1 td:nth-child(5)").hide();
        $("#DataGridView1 th:nth-child(6), #DataGridView1 td:nth-child(6)").hide();
        $("#DataGridView1 th:nth-child(7), #DataGridView1 td:nth-child(7)").show();
        $("#DataGridView1 th:nth-child(8), #DataGridView1 td:nth-child(8)").show();
    } else if (rb3) {
        $("#DataGridView1 th:nth-child(3), #DataGridView1 td:nth-child(3)").show();
        $("#DataGridView1 th:nth-child(4), #DataGridView1 td:nth-child(4)").show();
        $("#DataGridView1 th:nth-child(5), #DataGridView1 td:nth-child(5)").show();
        $("#DataGridView1 th:nth-child(6), #DataGridView1 td:nth-child(6)").show();
        $("#DataGridView1 th:nth-child(7), #DataGridView1 td:nth-child(7)").hide();
        $("#DataGridView1 th:nth-child(8), #DataGridView1 td:nth-child(8)").hide();
        $("#DataGridView1 th:nth-child(9), #DataGridView1 td:nth-child(9)").hide();
    }
}

function TextBox2_KeyDown() {
    let memoText = $("#TextBox2").val();
    if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            BaseResult.DataGridView2[i].ADD_2 = memoText;
        }
        DataGridView2Render();
    }
}

function ComboBox1_SelectedIndexChanged() { }

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        BaseResult.DataGridView1.sort((a, b) => {
            return IsTableSort ?
                (a.PN_NM > b.PN_NM ? 1 : -1) :
                (a.PN_NM < b.PN_NM ? 1 : -1);
        });
        DataGridView1Render();
    }
}

function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        BaseResult.DataGridView2.sort((a, b) => {
            return IsTableSort ?
                (a.PN_NM > b.PN_NM ? 1 : -1) :
                (a.PN_NM < b.PN_NM ? 1 : -1);
        });
        DataGridView2Render();
    }
}

function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        BaseResult.DataGridView3.sort((a, b) => {
            return IsTableSort ?
                (a.PN_NM > b.PN_NM ? 1 : -1) :
                (a.PN_NM < b.PN_NM ? 1 : -1);
        });
        DataGridView3Render();
    }
}
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

