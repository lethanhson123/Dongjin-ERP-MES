let BaseResult = {};
let DGV_II = 0;
let IsTableSort = false;

$(document).ready(function () {
    $('.modal').modal();
    $("#DateTimePicker1, #DateTimePicker2").val(new Date().toISOString().split('T')[0]);

    $("#DateTimePicker1").change(function () {
        let dt1 = new Date($(this).val());
        let dt2 = new Date($("#DateTimePicker2").val());
        if (dt1 > dt2) {
            $("#DateTimePicker2").val($(this).val());
        }
    });

    $("#DateTimePicker2").change(function () {
        let dt1 = new Date($("#DateTimePicker1").val());
        let dt2 = new Date($(this).val());
        if (dt2 < dt1) {
            $("#DateTimePicker1").val($(this).val());
        }
    });

    $("#rbchk1, #rbchk2").click(function () {
        rbchk_Click();
    });

    $("#Button4").click(function () {
        Button4_Click();
    });

    $("#TextBox_C1, #TextBox_C2, #TextBox_C3, #TextBox_C4, #TextBox_C5").val(0);
    $("#Label33, #Label35, #Label46, #Label47, #Label48").text(0);
    $("#VatPercentDropdown").val(10);
    $("#OR_NO_CHK").parent().parent().hide();

    $("#TextBox1").keydown(function (e) {
        if (e.keyCode === 13) {
            Buttonfind_Click_1();
        }
    });
    $("#TextBox_C1, #TextBox_C3, #TextBox_C4").on("input", CAL_COST);
    $("#TextBox9").change(function () {
        $("#TextBox9Value").val($(this).val() || "");
        Tab2_Buttonfind_Click();
    });
    $(".tabs").on("click", "a", function () {
        let tabId = $(this).attr("id");

        if (tabId === "ATabPage2") {
            
            $("#OR_NO_CHK").prop("checked", true);
            $("#CMPY_NO_CHK").prop("checked", false);
            COMB_CHG();
        }
        else if (tabId === "ATabPage3") {
            $("#RadioButton1").prop("checked", true);
            LoadComboBox1();
            Tab3_Buttonfind_Click();
        }
        else if (tabId === "ATabPage4") {
            Tab4_Buttonfind_Click();
        }
    });
    $(document).on("click", "#DataGridView2Body tr", function () {
        $("#DataGridView2Body tr").removeClass("selected");
        $(this).addClass("selected");
    });

    $("#Buttonfind").click(function () {
        if ($("#ATabPage2").hasClass("active")) {
            Tab2_Buttonfind_Click();
        } else if ($("#ATabPage3").hasClass("active")) {
            Tab3_Buttonfind_Click();
        } else if ($("#ATabPage4").hasClass("active")) {
            Tab4_Buttonfind_Click();
        } else {
            Buttonfind_Click_1();
        }
    });
    $("#Buttonadd").click(function () {
        Buttonadd_Click_1();
    });
    $("#Buttonsave").click(function () {
        if ($("#ATabPage2").hasClass("active")) {
            Tab2_Buttonsave_Click();
        } else {
            Buttonsave_Click_1();
        }
    });
    $("#Buttondelete").click(function () {
        if ($("#ATabPage2").hasClass("active")) {
            Tab2_Buttondelete_Click();
        } else if ($("#ATabPage3").hasClass("active")) {
            Tab3_Buttondelete_Click();
        } else {
            Buttondelete_Click_1();
        }
    });
    $("#Buttoncancel").click(function () {
        if ($("#ATabPage2").hasClass("active")) {
            Tab2_Buttoncancel_Click();
        } else {
            Buttoncancel_Click_1();
        }
    });
    $("#Buttoninport").click(function () {
        Buttoninport_Click_1();
    });
    $("#Buttonexport").click(function () {
        if ($("#ATabPage3").hasClass("active")) {
            Tab3_Buttonexport_Click();
        } else {
            Buttonexport_Click_1();
        }
    });

    $("#Buttonprint").click(function () {
        if ($("#ATabPage3").hasClass("active")) {
            Tab3_Buttonprint_Click();
        } else {
            Buttonprint_Click_1();
        }
    });

    $("#Buttonhelp").click(function () {
        Buttonhelp_Click_1();
    });
    $("#Buttonclose").click(function () {
        Buttonclose_Click_1();
    });
    $("#Button1").click(function () {
        Button1_Click_1();
    });
    $("#Button2").click(function () {
        if (DGV_II >= 0 && BaseResult.DataGridView3[DGV_II]) {
            sessionStorage.setItem("currentPartName", BaseResult.DataGridView3[DGV_II].PN_NM || "");
        }
        OpenWindowByURL("/V03_1", 800, 400);
    });
    $("#Button3").click(function () {
        OpenWindowByURL("/V03_3", 800, 400);
    });
    $(document).on("click", "#DataGridView4 td", function () {
        let columnIndex = $(this).index();
        let rowIndex = $(this).parent().index();
        DataGridView4_CellClick(rowIndex, columnIndex);
    });
    $(".tabs").on("click", "a", function () {
        if ($(this).attr("id") === "ATabPage2") {
            COMB_CHG();
        } else if ($(this).attr("id") === "ATabPage3") {
            $("#RadioButton1").prop("checked", true);
        } else if ($(this).attr("id") === "ATabPage4") {
            Tab4_Buttonfind_Click();
        }
    });
    $("#DT_MONTH1").change(function () {
        let dt1 = new Date($(this).val());
        let dt2 = new Date($("#DT_MONTH2").val());
        if (dt1 > dt2) {
            $("#DT_MONTH2").val($(this).val());
        }
    });

    $("#DT_MONTH2").change(function () {
        let dt1 = new Date($("#DT_MONTH1").val());
        let dt2 = new Date($(this).val());
        if (dt2 < dt1) {
            $("#DT_MONTH1").val($(this).val());
        }
    });
    if (!localStorage.getItem("selectedFactory")) {
        localStorage.setItem("selectedFactory", "1");
        $("#RadioButton1").prop("checked", true);
    } else {
        // Khôi phục trạng thái đã chọn
        let factory = localStorage.getItem("selectedFactory");
        if (factory === "1") {
            $("#RadioButton1").prop("checked", true);
        } else {
            $("#RadioButton2").prop("checked", true);
        }
    }

    // Xử lý sự kiện khi chọn Factory
    $("#RadioButton1, #RadioButton2").change(function () {
        let factoryIdx = $(this).attr("id") === "RadioButton1" ? "1" : "2";
        localStorage.setItem("selectedFactory", factoryIdx);
    });
    $("#TextBox9Value").keydown(function (e) {
        if (e.keyCode === 13) {
            let searchValue = $(this).val().trim();
            if (searchValue) {
                let matchOption = $("#TextBox9 option").filter(function () {
                    return $(this).val().toUpperCase() === searchValue.toUpperCase();
                });
                if (matchOption.length > 0) {
                    $("#TextBox9").val(matchOption.val()).trigger('change');
                } else {
                    Tab2_Buttonfind_Click_Direct(searchValue);
                }
            }
        }
    });
    $("#TextBox9Value").on("input", function () {
        let searchText = $(this).val().trim().toUpperCase();

        if (searchText === "") {
            $("#TextBox9 option").show();
            return;
        }
        let foundFirst = false;
        $("#TextBox9 option").each(function () {
            let optionValue = $(this).val().toUpperCase();

            if (optionValue.includes(searchText)) {
                $(this).show();
                if (!foundFirst) {
                    $("#TextBox9").val($(this).val());
                    foundFirst = true;
                }
            } else {
                $(this).hide();
            }
        });
    });
});
function Tab3_Buttonexport_Click() {
    const data = BaseResult.DataGridView4.map(row => ({
        "PDP_NO": row.PDP_NO || "",
        "Ngày": FORMAT_DATE(row.PDP_DATE1) || "",
        "Department": row.DEP || "",
        "Mã SP": row.PN_NM || "",
        "Tên SP": (row.PN_V || "") + " / " + (row.PN_K || ""),
        "Spec": (row.PSPEC_V || "") + " / " + (row.PSPEC_K || ""),
        "Đơn vị": (row.UNIT_VN || "") + " / " + (row.UNIT_KR || ""),
        "Số lượng PO": FORMAT_NUM(row.PDP_QTY) || "0",
        "Tồn kho": FORMAT_NUM(row.STOCK) || "0",
        "Giá trước": FORMAT_NUM(row.PDP_BE_COST) || "0",
        "Giá mới": FORMAT_NUM(row.PDP_COST) || "0",
        "Tổng tiền": FORMAT_NUM(row.SUM_COST) || "0",
        "VAT": FORMAT_NUM(row.PDP_VAT) || "0",
        "Chi phí khác": FORMAT_NUM(row.PDP_ECTCOST) || "0",
        "Tổng cộng": FORMAT_NUM(row.PDP_TOTCOST) || "0",
        "Trạng thái": getStatusText(row.ORDER_ST),
        "Công ty": row.COMP_NM || "",
        "Ghi chú": row.PDP_MEMO || ""
    }));
    const ws = XLSX.utils.json_to_sheet(data);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "DonHang");

    const filename = `DonHang_${new Date().toISOString().slice(0, 10)}.xlsx`;
    XLSX.writeFile(wb, filename);
}
function getStatusText(st) {
    const s = String(st);
    if (s === "1") return "Waiting";
    if (s === "2") return "Shipping";
    if (s === "3") return "Complete";
    if (s === "4") return "Cancel";
    if (s === "5") return "Ing...";
    return "-";
}
function LoadComboBox1() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/V03/LoadComboBox1", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            let options = '<option value="ALL">ALL</option>';
            if (data.ComboBox1 && data.ComboBox1.length > 0) {
                for (let i = 0; i < data.ComboBox1.length; i++) {
                    let item = data.ComboBox1[i];
                    if (item.CD_IDX !== "ALL") {
                        options += `<option value="${item.CD_IDX}">${item.CD_NM}</option>`;
                    }
                }
            }
            $("#ComboBox1").html(options);
            $("#BackGround").css("display", "none");
        }).catch(err => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
}
function Tab2_Buttonfind_Click_Direct(orderNo) {
    $("#BackGround").css("display", "block");

    if (!orderNo) {
        Swal.fire('Warning', 'Please enter Order Number!', 'warning');
        $("#BackGround").css("display", "none");
        return;
    }

    let BaseParameter = {
        Action: 2,
        ListSearchString: ["true", orderNo]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.DataGridView2 && data.DataGridView2.length > 0) {
                BaseResult.DataGridView2 = data.DataGridView2;
                DataGridView2Render();
                Swal.fire('Success', `Found order: ${orderNo}`, 'success');
            } else {
                Swal.fire('Warning', `Order "${orderNo}" not found or already confirmed!`, 'warning');
                $("#TextBox9Value").val("");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            Swal.fire('Error', 'Search failed!', 'error');
            $("#BackGround").css("display", "none");
        });
    });
}
function Tab3_Buttonfind_Click() {
    let SU01 = $("#DateTimePicker1").val();
    let SU02 = $("#DateTimePicker2").val();
    let SU03 = $("#TextBox10").val();   // Product Number
    let SU04 = $("#TextBox11").val();   // Product Name
    let SU05 = $("#TextBox12").val();   // Product Specification
    let SU06 = $("#TextBox13").val();   // Company Name
    let SU07 = $("#TextBox14").val();   // Department Name
    let SU08 = $("#TextBox15").val();   // Order Number

    let SU09 = $("#ComboBox1").val();
    if (SU09 === "ALL") {
        SU09 = "%%";
    } else if (SU09 === "6") {
        SU09 = "0";
    }

    let dateType = $("input[name='dateType']:checked").val();

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        Action: 3,
        ListSearchString: [
            SU01, SU02, SU03, SU04, SU05, SU06, SU07, SU08, SU09, dateType
        ]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView4 = data.DataGridView4;
            DataGridView4Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function DataGridView4Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            let row = BaseResult.DataGridView4[i];

            // Xác định trạng thái hiển thị
            let status = "0";
            let statusText = "-";
            let img1 = "/image/b2.png",
                img2 = "/image/b2.png",
                img3 = "/image/b2.png",
                img4 = "/image/b2.png";

            if (row.ORDER_ST) {
                status = row.ORDER_ST;
                switch (status) {
                    case "0":
                        statusText = "-";
                        break;
                    case "1":
                        statusText = "Waiting";
                        img1 = "/image/g1.png";
                        break;
                    case "2":
                        statusText = "Shipping";
                        img3 = "/image/g1.png";
                        break;
                    case "3":
                        statusText = "Complete";
                        img4 = "/image/g1.png";
                        break;
                    case "4":
                        statusText = "Cancel";
                        img2 = "/image/r1.png";
                        break;
                    case "5":
                        statusText = "Ing...";
                        img3 = "/image/g1.png";
                        img4 = "/image/g2.png";
                        break;
                }
            }

            HTML += `<tr>
               <td><input type="checkbox" class="D4_CHB" ${status === "3" ? "disabled" : ""}><span></span></td>
               <td><img src="${img1}" class="status-icon"></td>
               <td><img src="${img2}" class="status-icon"></td>
               <td><img src="${img3}" class="status-icon"></td>
               <td><img src="${img4}" class="status-icon"></td>
               <td>${row.PDP_CONF || ""}</td>
               <td>${statusText}</td>
               <td>${row.PDP_NO || ""}</td>
               <td>${FORMAT_DATE(row.PDP_DATE1) || ""}</td>
               <td>${row.DEP || ""}</td>
               <td>${row.PDP_CNF_DATE || ""}</td>
               <td>${row.PN_NM || ""}</td>
               <td>${row.PN_V || ""}<br>${row.PN_K || ""}</td>
               <td>${row.PSPEC_V || ""}<br>${row.PSPEC_K || ""}</td>
               <td>${row.MCNO || ""}</td>
               <td>${row.UNIT_VN || ""}<br>${row.UNIT_KR || ""}</td>
               <td>${FORMAT_NUM(row.PQTY) || 0}</td>
               <td>${FORMAT_NUM(row.STOCK) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_BE_COST) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_IN_QTY) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_QTY) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_COST) || 0}</td>
               <td>${FORMAT_NUM(row.SUM_COST) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_ECTCOST) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_VAT) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_TOTCOST) || 0}</td>
               <td>${row.PDP_MEMO || ""}</td>
               <td>${row.PDP_CMPY || ""}</td>
               <td>${row.COMP_NM || ""}</td>
               <td>${FORMAT_DATE(row.CREATE_DTM) || ""}</td>
               <td>${row.CREATE_USER || ""}</td>
               <td>${row.USER_NAME || ""}</td>
               <td>${row.ORDER_ST || ""}</td>
           </tr>`;
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}

function DataGridView4Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView4, IsTableSort);
    DataGridView4Render();
}

function DataGridView4_CellClick(rowIndex, columnIndex) {
    if (!BaseResult.DataGridView4 || rowIndex >= BaseResult.DataGridView4.length) return;

    let row = BaseResult.DataGridView4[rowIndex];

    if (row.ORDER_ST === "3") return; // Nếu đã hoàn thành thì không xử lý

    // Xử lý click vào checkbox (cột 0)
    if (columnIndex === 0) {
        let checkbox = $("#DataGridView4 tr").eq(rowIndex).find("td").eq(0).find("input");
        checkbox.prop("checked", !checkbox.prop("checked"));
        return;
    }

    // Xử lý click vào các cột trạng thái (1-4)
    if (columnIndex >= 1 && columnIndex <= 4) {
        $("#BackGround").css("display", "block");

        let OR_NO = row.PDPUSCH_IDX;
        let PDP_IN_QTY = row.PDP_IN_QTY || 0;

        let BaseParameter = {
            ListSearchString: [OR_NO, columnIndex.toString(), PDP_IN_QTY.toString()],
            USER_IDX: GetCookieValue("USER_IDX")
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/V03/DataGridView4_CellClick", {
            method: "POST",
            body: formUpload
        }).then((response) => {
            response.json().then((data) => {
                if (data.Success) {
                    Tab3_Buttonfind_Click(); // Refresh data
                } else {
                    alert("Error: " + (data.Error || "Unknown error"));
                    $("#BackGround").css("display", "none");
                }
            }).catch((err) => {
                console.error(err);
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Tab3_Buttondelete_Click() {
    let selectedRow = $("#DataGridView4 tr.selected");
    if (selectedRow.length === 0) {
        alert("Please select a row to delete.");
        return;
    }

    let rowIndex = selectedRow.index();
    let row = BaseResult.DataGridView4[rowIndex];

    if (row.ORDER_ST === "3") {
        alert("Cannot delete completed order.");
        return;
    }

    if (!confirm("Order 취소 하시겠습니까? / Order Delete?")) {
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        Action: 3,
        ListSearchString: [row.PDPUSCH_IDX, row.PDPUSCH_IDX, row.ORDER_ST],
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttondelete_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("정상처리 되었습니다.\nĐã được lưu.");
                Tab3_Buttonfind_Click();
            } else {
                alert("Error: " + (data.Error || "Unknown error"));
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function Button4_Click() {
    let checkedRows = [];

    $("#DataGridView4 tr").each(function (index) {
        let checkbox = $(this).find("td:first-child input[type='checkbox']");
        if (checkbox.prop("checked")) {
            checkedRows.push(index);
        }
    });

    if (checkedRows.length === 0) {
        alert("Please select at least one row.");
        return;
    }

    $("#BackGround").css("display", "block");

    let processRows = function (index) {
        if (index >= checkedRows.length) {
            alert("정상처리 되었습니다.\nĐã được lưu.");
            Tab3_Buttonfind_Click();
            return;
        }

        let rowIndex = checkedRows[index];
        let row = BaseResult.DataGridView4[rowIndex];

        let PO_QTY = parseInt(row.PDP_QTY) || 0;
        let BEIN_QTY = parseInt(row.PDP_IN_QTY) || 0;
        let STOCK = parseInt(row.STOCK) || 0;

        let AAA = row.PDPUSCH_IDX;
        let BBB = row.PDP_PART;
        let CCC = STOCK + (PO_QTY - BEIN_QTY);
        let DDD = PO_QTY;
        let IN_QTY = PO_QTY - BEIN_QTY;

        let BaseParameter = {
            ListSearchString: [AAA, BBB, CCC.toString(), DDD.toString(), IN_QTY.toString()],
            USER_IDX: GetCookieValue("USER_IDX")
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/V03/Button4_Click", {
            method: "POST",
            body: formUpload
        }).then((response) => {
            response.json().then((data) => {
                processRows(index + 1);
            }).catch((err) => {
                console.error(err);
                $("#BackGround").css("display", "none");
            });
        });
    };

    processRows(0);
}

function rbchk_Click() {
    if (!BaseResult.DataGridView4 || BaseResult.DataGridView4.length === 0) return;

    if ($("#rbchk1").prop("checked")) {
        $("#DataGridView4 tr").each(function (i) {
            let checkbox = $(this).find("td:first-child input[type='checkbox']");
            let row = BaseResult.DataGridView4[i];
            if (row && row.ORDER_ST !== "3") {
                checkbox.prop("checked", true);
            } else {
                checkbox.prop("checked", false);
            }
        });
    } else if ($("#rbchk2").prop("checked")) {
        $("#DataGridView4 tr").each(function (i) {
            let checkbox = $(this).find("td:first-child input[type='checkbox']");
            let row = BaseResult.DataGridView4[i];
            if (row && row.ORDER_ST !== "3") {
                checkbox.prop("checked", !checkbox.prop("checked"));
            } else {
                checkbox.prop("checked", false);
            }
        });
    }
}

function Tab3_Buttonprint_Click() {
    if (!BaseResult.DataGridView4 || BaseResult.DataGridView4.length === 0) {
        alert("No data to print!");
        return;
    }

    let printData = [];
    $("#DataGridView4 tr").each(function (i) {
        let checkbox = $(this).find("td:first-child input[type='checkbox']");
        if (checkbox.prop("checked") || $("#DataGridView4 tr td:first-child input[type='checkbox']:checked").length === 0) {
            let row = BaseResult.DataGridView4[i];
            printData.push({
                dep: row.DEP,
                createUser: row.CREATE_USER,
                pdpDate1: FORMAT_DATE(row.PDP_DATE1),
                pdpCnfDate: FORMAT_DATE(row.PDP_CNF_DATE),
                pnNm: row.PN_NM,
                pnV: row.PN_V,
                pnK: row.PN_K,
                pspecV: row.PSPEC_V,
                pspecK: row.PSPEC_K,
                unitVn: row.UNIT_VN,
                unitKr: row.UNIT_KR,
                pqty: row.PQTY,
                pdpMemo: row.PDP_MEMO,
                stock: row.STOCK,
                pdpQty: row.PDP_QTY,
                pdpBeCost: row.PDP_BE_COST,
                pdpCost: row.PDP_COST,
                sumCost: row.SUM_COST,
                pdpVat: row.PDP_VAT,
                pdpEctcost: row.PDP_ECTCOST,
                pdpTotcost: row.PDP_TOTCOST,
                compNm: row.COMP_NM,
                pdpNo: row.PDP_NO
            });
        }
    });

    let printWindow = window.open("/V03/Print", "_blank", "width=800,height=600");

    sessionStorage.setItem("printData", JSON.stringify(printData));
}


// Tab1 - Functions
function Buttonfind_Click_1() {
    if ($("#TextBox1").val().length === 0) {
        alert("Please input Order No.");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        Action: 1,
        ListSearchString: [$("#TextBox1").val()]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView3Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    let row = BaseResult.DataGridView3[i];
                    let rowClass = row.PDP_PRIENT === "C" ? "class='bg-light-blue'" : "";
                    HTML += `<tr ${rowClass} onclick='DGV_CLK(${i})'>
                       <td>${row.PDP_CONF || ""}</td>
                       <td>${row.PDP_NO || ""}</td>
                       <td>${FORMAT_DATE(row.PDP_DATE1) || ""}</td>
                       <td>${row.DEPARTMENT || ""}</td>
                       <td>${row.PN_NM || ""}</td>
                       <td>${row.PN_V || ""}<br>${row.PN_K || ""}</td>
                       <td>${row.PSPEC_V || ""}<br>${row.PSPEC_K || ""}</td>
                       <td>${row.UNIT_VN || ""}<br>${row.UNIT_KR || ""}</td>
                       <td>${FORMAT_NUM(row.PQTY)}</td>
                       <td>${FORMAT_NUM(row.STOCK)}</td>
                       <td>${FORMAT_NUM(row.PDP_QTY)}</td>
                       <td>${FORMAT_NUM(row.PDP_COST)}</td>
                       <td>${FORMAT_NUM(row.SUM_COST)}</td>
                       <td>${FORMAT_NUM(row.PDP_VAT)}</td>
                       <td>${FORMAT_NUM(row.PDP_ECTCOST)}</td>
                       <td>${FORMAT_NUM(row.PDP_TOTCOST)}</td>
                       <td>${row.PDP_MEMO || ""}</td>
                       <td>${row.PDP_CMPY || ""}</td>
                       <td>${row.COMP_NM || ""}</td>
                       <td>${FORMAT_DATE(row.CREATE_DTM) || ""}</td>
                       <td>${row.CREATE_USER || ""}</td>
                       <td>${row.USER_NAME}</td>
                       <td>${FORMAT_NUM(row.PDP_BE_COST)}</td>
                       <td>${row.PDP_PRIENT || ""}</td>
                   </tr>`;
                }
            }
        }
    }
    document.getElementById("DataGridView3Body").innerHTML = HTML;
    if (HTML !== "") {
        DGV_CLK(0);
    } else {
        RESET_FORM();
    }
}

function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    DataGridView3Render();
}

function DGV_CLK(rowIndex) {
    if (!BaseResult.DataGridView3 || rowIndex >= BaseResult.DataGridView3.length) return;

    DGV_II = rowIndex;
    let row = BaseResult.DataGridView3[rowIndex];

    $("#TextBox_C1").val(row.PDP_COST || 0);
    $("#TextBox_C2").val(row.SUM_COST || 0);
    $("#TextBox_C3").val(row.PDP_VAT || 0);
    $("#TextBox_C4").val(row.PDP_ECTCOST || 0);
    $("#TextBox_C5").val(row.PDP_TOTCOST || 0);

    $("#TextBox7").val(row.PDP_CMPY || "");
    $("#TextBox8").val(row.COMP_NM || "");

    $("#Label49").text(row.PN_NM || "");
    $("#Label2").text(`${row.PN_V || ""}\n${row.PN_K || ""}`);
    $("#Label3").text(`${row.PSPEC_V || ""}\n${row.PSPEC_K || ""}`);
    $("#Label11").text(`${row.UNIT_VN || ""}(${row.PQTY || 0})`);
    $("#Label8").text(row.PDP_QTY || 0);
    $("#Label14").text(row.STOCK || 0);

    $("#Label33").text(FORMAT_NUM(row.PDP_COST));
    $("#Label35").text(FORMAT_NUM(row.SUM_COST));
    $("#Label46").text(FORMAT_NUM(row.PDP_VAT));
    $("#Label47").text(FORMAT_NUM(row.PDP_ECTCOST));
    $("#Label48").text(FORMAT_NUM(row.PDP_TOTCOST));

    GET_COST_INFO(row.PDP_PART);
}

function GET_COST_INFO(partIdx) {
    if (!partIdx) return;

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [partIdx]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/DataGridView3_SelectionChanged", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            DataGridView1Render(data.DataGridView1);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function DataGridView1Render(data) {
    let HTML = "";
    if (data) {
        if (data.length > 0) {
            for (let i = 0; i < data.length; i++) {
                let row = data[i];
                HTML += `<tr>
                   <td>${row.STATE || ""}</td>
                   <td>${FORMAT_NUM(row.PDP_COST) || 0}</td>
                   <td>${FORMAT_NUM(row.PDP_VAT) || 0}</td>
                   <td>${FORMAT_NUM(row.PDP_ECTCOST) || 0}</td>
               </tr>`;

                if (row.STATE === "LAST" && BaseResult.DataGridView3 && BaseResult.DataGridView3[DGV_II]) {
                    BaseResult.DataGridView3[DGV_II].PDP_BE_COST = row.PDP_COST;
                }
            }
        }
    }
    document.getElementById("DataGridView1Body").innerHTML = HTML;
}

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView1) {
        DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
        DataGridView1Render(BaseResult.DataGridView1);
    }
}

function DataGridViewSort(array, ascending) {
    if (array && array.length > 0) {
        array.sort(function (a, b) {
            for (let prop in a) {
                if (a.hasOwnProperty(prop) && b.hasOwnProperty(prop)) {
                    if (a[prop] != b[prop]) {
                        return ascending ?
                            ((a[prop] < b[prop]) ? -1 : 1) :
                            ((a[prop] > b[prop]) ? -1 : 1);
                    }
                    break;
                }
            }
            return 0;
        });
    }
}

// Hàm tính toán chi phí
function CAL_COST() {
    try {
        let cost = parseFloat($("#TextBox_C1").val() || 0);
        let qty = parseFloat($("#Label8").text() || 0);
        let otherCost = parseFloat($("#TextBox_C4").val() || 0);

        // Lấy giá trị VAT từ dropdown
        let vatRate = parseFloat($("#VatPercentDropdown").val() || 0) / 100;

        if (isNaN(cost) || isNaN(qty)) {
            throw new Error();
        }

        let totalCost = cost * qty;
        let vat = (totalCost * vatRate).toFixed(1);
        let totalWithVat = (parseFloat(totalCost) + parseFloat(vat) + parseFloat(otherCost)).toFixed(1);

        $("#TextBox_C2").val(totalCost.toFixed(1));
        $("#TextBox_C3").val(vat);
        $("#TextBox_C5").val(totalWithVat);

        $("#Label33").text(FORMAT_NUM(cost));
        $("#Label35").text(FORMAT_NUM(totalCost));
        $("#Label46").text(FORMAT_NUM(Math.round(vat)));
        $("#Label47").text(FORMAT_NUM(otherCost));
        $("#Label48").text(FORMAT_NUM(totalWithVat));
    } catch (ex) {
        $("#TextBox_C1").val(0);
        $("#TextBox_C2").val(0);
        $("#TextBox_C3").val(0);
        $("#TextBox_C4").val(0);
        $("#TextBox_C5").val(0);
        $("#Label33").text("0");
        $("#Label35").text("0");
        $("#Label46").text("0");
        $("#Label47").text("0");
        $("#Label48").text("0");
        console.error(ex);
    }
}


function Button1_Click_1() {
    if (!BaseResult.DataGridView3 || BaseResult.DataGridView3.length === 0 || DGV_II < 0) {
        return;
    }

    if ($("#TextBox7").val().length === 0 || $("#TextBox8").val().length === 0) {
        alert("구입처(Nơi mua) / Please Check Again.");
        return;
    }

    $("#BackGround").css("display", "block");

    let row = BaseResult.DataGridView3[DGV_II];
    let BaseParameter = {
        ListSearchString: [
            $("#TextBox7").val(),
            $("#TextBox_C1").val(),
            $("#TextBox_C3").val(),
            $("#TextBox_C4").val(),
            $("#TextBox_C5").val(),
            row.PDP_BE_COST || 0,
            row.PDPUSCH_IDX || "",
            $("#TextBox1").val(),
            $("#TextBox8").val(),
            $("#Label8").text(),
            row.PDP_PART || ""
        ],
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Button1_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("Data saved successfully!");
                Buttonfind_Click_1();
            } else {
                alert("Error: " + (data.Error || "Unknown error"));
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonprint_Click_1() {
    $("#BackGround").css("display", "block");

    // Tính tổng của tất cả các dòng
    let totalSumCost = 0;
    let totalVat = 0;
    let totalEctCost = 0;
    let totalTotCost = 0;

    for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
        let row = BaseResult.DataGridView3[i];
        totalSumCost += parseFloat(row.SUM_COST || 0);
        totalVat += parseFloat(row.PDP_VAT || 0);
        totalEctCost += parseFloat(row.PDP_ECTCOST || 0);
        totalTotCost += parseFloat(row.PDP_TOTCOST || 0);
    }

    // Sử dụng thông tin từ dòng đầu tiên cho header
    let firstRow = BaseResult.DataGridView3[0];

    // Chuẩn bị danh sách dòng HTML
    let contentRows = "";

    // Duyệt qua từng dòng để tạo HTML
    for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
        let row = BaseResult.DataGridView3[i];

        // Định dạng dữ liệu cho mỗi dòng
        let unitPart = (row.UNIT_VN || "") + "/" + (row.UNIT_KR || "");
        let pqtyPart = FORMAT_NUM(row.PQTY);
        let unitWithPQTY = unitPart + "\n" + pqtyPart;

        // Tạo đối tượng dữ liệu cho mỗi dòng
        let rowData = {
            PDP_NO: row.PDP_NO || "",
            DEPARTMENT: row.DEPARTMENT || "",
            USER_NAME: row.USER_NAME || "",
            PDP_DATE1: FORMAT_DATE(row.PDP_DATE1) || "",
            PN_NM: row.PN_NM || "",  // Product_NO
            PN_KRVN: (row.PN_V || "") + "\n" + (row.PN_K || ""),  // Part Name KR/VN
            SPEC_KRVN: (row.PSPEC_V || "") + "\n" + (row.PSPEC_K || ""),
            UNIT_KRVN: unitWithPQTY,
            PQTY: FORMAT_NUM(row.PQTY) || "0",
            PDP_MEMO: row.PDP_MEMO || "",
            STOCK: FORMAT_NUM(row.STOCK) || "0",
            PDP_QTY: FORMAT_NUM(row.PDP_QTY) || "0",
            PDP_BE_COST: FORMAT_NUM(row.PDP_BE_COST) || "0",
            PDP_COST: FORMAT_NUM(row.PDP_COST) || "0",
            SUM_COST: FORMAT_NUM(row.SUM_COST) || "0",
            PDP_VAT: FORMAT_NUM(row.PDP_VAT) || "0",
            PDP_ECTCOST: FORMAT_NUM(row.PDP_ECTCOST) || "0",
            PDP_TOTCOST: FORMAT_NUM(row.PDP_TOTCOST) || "0",
            COMP_NM: row.COMP_NM || ""
        };

        // Tạo HTML cho dòng này
        let rowHTML = createRowHTML(rowData);
        contentRows += rowHTML;
    }

    // Lấy thông tin Factory được chọn
    let selectedFactory = localStorage.getItem("selectedFactory") || "1";

    // Chuẩn bị thông tin cho header
    let listSearchString = [
        firstRow.PDP_NO || "",                         // 0. Order NO
        firstRow.DEPARTMENT || "",                      // 1. Department
        firstRow.USER_NAME || "",                       // 2. User
        FORMAT_DATE(firstRow.PDP_DATE1) || "",          // 3. Date
        firstRow.PN_NM || "",                           // 4. Product Name
        (firstRow.PN_V || "") + "\n" + (firstRow.PN_K || ""),  // 5. Product Code KR/VN
        (firstRow.PSPEC_V || "") + "\n" + (firstRow.PSPEC_K || ""),  // 6. Spec KR/VN
        (firstRow.UNIT_VN || "") + "/" + (firstRow.UNIT_KR || "") + "\n" + FORMAT_NUM(firstRow.PQTY),  // 7. Unit KR/VN + PQTY
        FORMAT_NUM(firstRow.PQTY) || "0",               // 8. Quantity
        firstRow.PDP_MEMO || "",                        // 9. Memo
        FORMAT_NUM(firstRow.STOCK) || "0",              // 10. Stock
        FORMAT_NUM(firstRow.PDP_QTY) || "0",            // 11. Order Quantity
        FORMAT_NUM(firstRow.PDP_BE_COST) || "0",        // 12. Last Cost
        FORMAT_NUM(firstRow.PDP_COST) || "0",           // 13. Cost
        FORMAT_NUM(totalSumCost) || "0",                // 14. Sum Cost (tổng của tất cả các dòng)
        FORMAT_NUM(totalVat) || "0",                    // 15. VAT (tổng của tất cả các dòng)
        FORMAT_NUM(totalEctCost) || "0",                // 16. Other Cost (tổng của tất cả các dòng)
        FORMAT_NUM(totalTotCost) || "0",                // 17. Total Cost (tổng của tất cả các dòng)
        firstRow.COMP_NM || "",                         // 18. Company Name
        GetCookieValue("USER_IDX"),                     // 19. User ID
        firstRow.PDP_NO || "",                          // 20. Order NO
        selectedFactory,                                // 21. Factory selection
        contentRows                                     // 22. Nội dung HTML của tất cả các dòng
    ];

    // Chuẩn bị BaseParameter
    let BaseParameter = {
        Action: 1,
        ListSearchString: listSearchString,
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonprint_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Success && data.Code) {
                OpenWindowByURL(data.Code, 800, 600);
                Buttonfind_Click_1();
            } else {
                alert(data.Error || "Unknown error");
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            console.error(err);
            alert("Network error");
            $("#BackGround").css("display", "none");
        });
}

// Hàm tạo HTML cho một dòng
function createRowHTML(row) {
    let specPart1 = "";
    let specPart2 = "";
    if (row.SPEC_KRVN) {
        let specParts = row.SPEC_KRVN.split('\n');
        specPart1 = specParts[0];
        specPart2 = specParts.length > 1 ? specParts[1] : "";
    }

    let unitPart = "";
    let pqtyPart = "";
    if (row.UNIT_KRVN) {
        let unitParts = row.UNIT_KRVN.split('\n');
        unitPart = unitParts[0];
        pqtyPart = unitParts.length > 1 ? unitParts[1] : "";
    }

    let pnPart1 = "";
    let pnPart2 = "";
    if (row.PN_KRVN) {
        let pnParts = row.PN_KRVN.split('\n');
        pnPart1 = pnParts[0];
        pnPart2 = pnParts.length > 1 ? pnParts[1] : "";
    }

    return `
<tr>
    <td rowspan="2">${row.DEPARTMENT}</td>
    <td>
        <div class="cell-top">${pnPart1}</div>
        <div class="cell-divider"></div>
        <div class="cell-bottom">${pnPart2}</div>
    </td>
    <td>
        <div class="cell-top">${specPart1}</div>
        <div class="cell-divider"></div>
        <div class="cell-bottom">${specPart2}</div>
    </td>
    <td rowspan="2">
        <div class="cell-top">${unitPart}</div>
        <div class="cell-divider"></div>
        <div class="cell-bottom">${pqtyPart}</div>
    </td>
    <td rowspan="2">${row.PDP_MEMO}</td>
    <td rowspan="2">${row.PDP_QTY}</td>
    <td rowspan="2">${row.PDP_BE_COST}</td>
    <td rowspan="2">
        <div class="cell-top">${row.PDP_COST}</div>
        <div class="cell-divider"></div>
        <div class="cell-bottom">${row.SUM_COST}</div>
    </td>
    <td rowspan="2">${row.PDP_VAT}</td>
    <td rowspan="2">${row.PDP_ECTCOST}</td>
    <td rowspan="2">${row.PDP_TOTCOST}</td>
    <td rowspan="2">${row.COMP_NM}</td>
</tr>
<tr>
    <td colspan="2" class="product-no-row">
        ${row.PN_NM || ""}
    </td>
</tr>`;
}

function Buttoncancel_Click_1() {
    RESET_FORM();
}

function RESET_FORM() {
    $("#TextBox_C1, #TextBox_C2, #TextBox_C3, #TextBox_C4, #TextBox_C5").val(0);
    $("#TextBox7, #TextBox8").val("");
    $("#Label49, #Label2, #Label3, #Label11").text("");
    $("#Label8, #Label14").text(0);
    $("#Label33, #Label35, #Label46, #Label47, #Label48").text(0);
    $("#DataGridView1Body").html("");
}

// Tab2 - Functions
function COMB_CHG() {
    $("#BackGround").css("display", "block");

    // ⭐ THÊM: Gửi mode
    let mode = $("#OR_NO_CHK").prop("checked") ? "ORDER" : "COMPANY";

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({
        ListSearchString: [mode]  
    }));

    fetch("/V03/COMB_CHG", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.ComboBox1 && data.ComboBox1.length > 0) {
                let options = "";
                for (let i = 0; i < data.ComboBox1.length; i++) {
                    let pdpNo = data.ComboBox1[i].PDP_NO || "";
                    options += `<option value="${pdpNo}">${pdpNo}</option>`;
                }
                $("#TextBox9").html(options);
                let firstValue = data.ComboBox1[0].PDP_NO;
                $("#TextBox9").val(firstValue);
                $("#TextBox9Value").val(firstValue);

                Tab2_Buttonfind_Click(); 
            } else {
                $("#TextBox9").html("");
                $("#TextBox9Value").val("");
                $("#DataGridView2Body").html("");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function Tab2_Buttonfind_Click() {
    $("#BackGround").css("display", "block");

    let orderNo = $("#TextBox9").val();

    if (!orderNo) {
        $("#BackGround").css("display", "none");
        return;
    }
    let isOrderNoCheck = $("#OR_NO_CHK").prop("checked");
    let isCompanyNoCheck = $("#CMPY_NO_CHK").prop("checked");

    let BaseParameter = {
        Action: 2,
        ListSearchString: [
            isOrderNoCheck ? "true" : "false",      
            isCompanyNoCheck ? "true" : "false",    
            orderNo
        ]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView2 = data.DataGridView2;
            DataGridView2Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    let row = BaseResult.DataGridView2[i];
                    HTML += `<tr onclick='DataGridView2_SelectionChanged(${i})' data-idx="${row.PDPUSCH_IDX}">
                       <td>${row.PDP_CONF || ""}</td>
                       <td>${row.PDP_NO || ""}</td>
                       <td>${FORMAT_DATE(row.PDP_DATE1) || ""}</td>
                       <td>${row.DEPARTMENT || ""}</td>
                       <td>${row.PN_NM || ""}</td>
                       <td>${row.PN_V || ""}<br>${row.PN_K || ""}</td>
                       <td>${row.PSPEC_V || ""}<br>${row.PSPEC_K || ""}</td>
                        <td></td>
                       <td>${row.UNIT_VN || ""}<br>${row.UNIT_KR || ""}</td>
                       <td>${row.PQTY || 0}</td>
                       <td>${row.STOCK || 0}</td>
                       <td>${row.PDP_QTY || 0}</td>
                       <td>${row.PDP_COST || 0}</td>
                       <td>${row.PDP_COST || 0}</td>
                        <td style="background-color: #e3f2fd; cursor: pointer;">${row.SUM_COST || 0}</td>
                       <td>${row.PDP_VAT || 0}</td>
                       <td>${row.PDP_ECTCOST || 0}</td>
                       <td>${row.PDP_TOTCOST || 0}</td>
                       <td>${row.PDP_MEMO || ""}</td>
                       <td>${row.PDP_CMPY || ""}</td>
                       <td>${row.COMP_NM || ""}</td>
                       <td>${FORMAT_DATE(row.CREATE_DTM) || ""}</td>
                       <td>${row.CREATE_USER || ""}</td>
                       <td>${row.USER_NAME || ""}</td>
                   </tr>`;
                }
            }
        }
    }
    document.getElementById("DataGridView2Body").innerHTML = HTML;
}

function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    DataGridView2Render();
}

function DataGridView2_SelectionChanged(i) {
    $("#DataGridView2Body tr").removeClass("selected");
    $("#DataGridView2Body tr:eq(" + i + ")").addClass("selected");
    window.DataGridView2RowIndex = i;
}

function Tab2_Buttonsave_Click() {
    if ($("#TextBox9").val() === "") {
        alert("Please select an order number.");
        return;
    }

    if (!confirm("승인 하시겠습니까? / Bạn có đồng ý không?")) {
        return;
    }

    $("#BackGround").css("display", "block");

    let orderNo = $("#TextBox9").val();

    let isOrderNoCheck = $("#OR_NO_CHK").prop("checked");
    let isCompanyNoCheck = $("#CMPY_NO_CHK").prop("checked");

    let BaseParameter = {
        Action: 2,
        ListSearchString: [
            isOrderNoCheck ? "true" : "false",     
            isCompanyNoCheck ? "true" : "false",   
            orderNo
        ],
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonsave_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("정상처리 되었습니다.\nĐã được lưu.");
                COMB_CHG();  // ⭐ Reload combo
            } else {
                alert("Error: " + (data.Error || "Unknown error"));
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function Tab2_Buttondelete_Click() {
    if ($("#TextBox9").val() === "") {
        alert("Please select an order number.");
        return;
    }

    if (!confirm("Order 취소 하시겠습니까? / Order Delete?")) {
        return;
    }

    $("#BackGround").css("display", "block");

    let orderNo = $("#TextBox9").val();

    let BaseParameter = {
        Action: 2,
        ListSearchString: [orderNo],
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttondelete_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("정상처리 되었습니다.\nĐã được lưu.");
                COMB_CHG();
            } else {
                alert("Error: " + (data.Error || "Unknown error"));
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function Tab2_Buttoncancel_Click() {
    $("#DataGridView2Body").html("");
    COMB_CHG();
}

function Buttonadd_Click_1() { }
function Buttonsave_Click_1() { }
function Buttondelete_Click_1() { }
function Buttoninport_Click_1() { }
function Buttonexport_Click_1() { }
function Buttonhelp_Click_1() {
    OpenWindowByURL("/WMP_PLAY", 800, 460);
}
function Buttonclose_Click_1() {
    history.back();
}

function FORMAT_DATE(dateString) {
    if (!dateString) return "";

    let date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString;


    const year = date.getFullYear();

    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

function FORMAT_NUM(num) {
    if (num === null || num === undefined || isNaN(parseFloat(num))) return "0";
    return parseFloat(num).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}

function GetCookieValue(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return "";
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", `width=${width},height=${height},location=no,menubar=no,toolbar=no`);
}
function Tab4_Buttonfind_Click() {
    let DATE_D1 = $("#DT_MONTH1").val();
    let DATE_D2 = $("#DT_MONTH2").val();

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        Action: 4,
        ListSearchString: [DATE_D1, DATE_D2]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            console.log("API Response:", data);

            // Sử dụng DataGridView5Raw nếu có
            if (data.DataGridView5Raw && data.DataGridView5Raw.length > 0) {
                BaseResult.DataGridView5 = data.DataGridView5Raw;
            } else {
                BaseResult.DataGridView5 = data.DataGridView5 || [];
            }

            DataGridView5Render();
            renderChart();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function DataGridView5Render() {
    if (!BaseResult.DataGridView5 || BaseResult.DataGridView5.length === 0) {
        $("#DataGridView5").html("<tr><td colspan='15'>No data available</td></tr>");
        $("#DataGridView5Table thead").html("");
        return;
    }

    // Lấy tất cả các key từ đối tượng đầu tiên
    let allKeys = Object.keys(BaseResult.DataGridView5[0]);

    // Sắp xếp lại các cột: DATE đầu tiên, TOTAL thứ hai, còn lại theo alphabet
    allKeys.sort((a, b) => {
        if (a === "DATE") return -1;
        if (b === "DATE") return 1;
        if (a === "TOTAL") return -1;
        if (b === "TOTAL") return 1;
        return a.localeCompare(b);
    });

    // Tạo header
    let headerHTML = "<tr>";
    for (let key of allKeys) {
        headerHTML += `<th>${key}</th>`;
    }
    headerHTML += "</tr>";
    $("#DataGridView5Table thead").html(headerHTML);

    // Hiển thị dữ liệu
    let rowsHTML = "";
    for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
        let row = BaseResult.DataGridView5[i];
        rowsHTML += "<tr>";

        for (let key of allKeys) {
            let value = row[key] !== null ? row[key] : "";

            // Format số
            if (key !== "DATE" && !isNaN(parseFloat(value))) {
                value = FORMAT_NUM(value);
            }

            rowsHTML += `<td>${value}</td>`;
        }

        rowsHTML += "</tr>";
    }

    $("#DataGridView5").html(rowsHTML);

    // Vẽ biểu đồ chỉ với các cột không phải DATE và TOTAL
    let chartKeys = allKeys.filter(key => key !== "DATE" && key !== "TOTAL");
    renderChart(chartKeys);
}
$(document).on("dblclick", "#DataGridView2Body tr td", function () {
    let columnIndex = $(this).index();
    let rowIndex = $(this).parent().index();
    if (columnIndex === 14) {
        let P_NO = BaseResult.DataGridView2[rowIndex].PN_NM;
        openV03_4Modal(P_NO);
    }
});

function openV03_4Modal(productCode) {
    $("#V03_4ModalLabel").text(productCode);
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [productCode]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/V03/LoadV03_4Modal", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            renderV03_4ModalData(data);
            $("#BackGround").css("display", "none");
            $('#V03_4Modal').modal('open');
        }).catch((err) => {
            console.error(err);
            $("#BackGround").css("display", "none");
        });
    });
}

function renderV03_4ModalData(data) {
    let HTML = "";

    if (data && data.DataGridView9 && data.DataGridView9.length > 0) {
        for (let i = 0; i < data.DataGridView9.length; i++) {
            let row = data.DataGridView9[i];
            HTML += `<tr>
               <td>${row.PDP_NO || ""}</td>
               <td>${FORMAT_DATE(row.PDP_DATE1) || ""}</td>
               <td>${row.PN_V || ""}</td>
               <td>${row.PN_K || ""}</td>
               <td>${FORMAT_NUM(row.PDP_COST) || 0}</td>
               <td>${FORMAT_NUM(row.PDP_BE_COST) || 0}</td>
               <td>${row.Company || ""}</td>
           </tr>`;
        }
    }

    document.getElementById("V03_4ModalTable").innerHTML = HTML;
}


function renderChart(chartKeys) {
    if (!BaseResult.DataGridView5 || BaseResult.DataGridView5.length === 0 || !chartKeys || chartKeys.length === 0) return;

    // Lấy dữ liệu từ hàng đầu tiên
    let row = BaseResult.DataGridView5[0];
    let labels = [];
    let data = [];

    for (let key of chartKeys) {
        labels.push(key);
        data.push(parseFloat(row[key] || 0));
    }

    if (window.departmentChart) {
        window.departmentChart.destroy();
    }

    const ctx = document.getElementById('Chart').getContext('2d');
    window.departmentChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Department Costs',
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.5)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString();
                        }
                    }
                }
            },
            plugins: {
                title: {
                    display: true,
                    text: 'Department Cost Analysis',
                    font: {
                        size: 16
                    }
                },
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return context.parsed.y.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}