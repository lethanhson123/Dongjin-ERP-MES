let BaseResult;
var ScanList = $('#scanlist');
let customerCode = "DH";
let controller = "/D16/";
let table2;

$(document).ready(function () {
    table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "62vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: 1, orderable: false, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ],
        createdRow: function (row, data) {
            if (data[14] === "D") {
                $(row).addClass("red-text");
            }
        }
    });

    // Thêm min-height sau khi bảng đã khởi tạo, tùy vào kích thước mà điều chỉnh cho phù hợp màn hình
    $(".dataTables_scrollBody").css("min-height", "60vh");

    // Chọn tất cả các dòng
    $('#checkAll').on('click', function () {
        const isChecked = $(this).is(':checked');
        $('.row-check').prop('checked', isChecked);
    });

    // Nếu bỏ chọn từng dòng → cập nhật lại checkbox tổng
    $('#Table2 tbody').on('change', '.row-check', function () {
        const allChecked = $('.row-check').length === $('.row-check:checked').length;
        $('#checkAll').prop('checked', allChecked);
    });
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

$("#btn_PrintLabel").on("click", function (e) {
    e.preventDefault(); // ngăn submit form nếu có
    PrintLabel();
    // Thêm xử lý tại đây
});
function Buttonfind_Click() {
    // Lấy tab đang được chọn
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            $("#tab1_QRCodeScan").val("");
            $("#tab1_Date").val(new Date().toLocaleDateString('en-CA'));
            $("#tab1_PartCode").val("");
            $("#tab1_ECN").val("");
            $("#tab1_SNP").val("");
            $("#tab1_Line").val("");
            $("#tab1_Family").val("");
            $("#tab1_TotalQty").val("0");
            $("#tab1_PackingLot").val("");
            $("#tab1_QRCodeScan").focus();
            $("#Table1 tbody").empty();
        } else if (currentTabId === 'Tag002') {
            SearchHistory();
        } else if (currentTabId === 'Tag003') {

        }
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const radios = document.querySelectorAll('input[name="STATUS"]');
    radios.forEach(function (radio) {
        radio.addEventListener("change", function () {
            SearchHistory();
        });
    });
});

function formatToDateInput(dateString) {
    if (!dateString) return "";

    const date = new Date(dateString);
    if (isNaN(date)) return "";

    const yyyy = date.getFullYear();
    const mm = (date.getMonth() + 1).toString().padStart(2, '0'); // getMonth() từ 0–11
    const dd = date.getDate().toString().padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}
async function LoadPackingList(lotcode) {
    if (!lotcode) {
        return;
    }

    const BaseParameter = {
        Code: "LoadPackingList",
        Barcode: lotcode
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    const url = controller + "Buttonfind_Click";

    try {
        $("#BackGround").css("display", "block");
        const response = await fetch(url, {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();

        if (data.ErrorNumber === 0) {
            $("#tab1_PartCode").val(data.ListtspartTranfer[0].PART_NO);
            $("#tab1_SNP").val(data.ListtspartTranfer[0].PART_SNP);
            $("#tab1_Line").val(data.ListtspartTranfer[0].PART_CAR);
            $("#tab1_Family").val(data.ListtspartTranfer[0].PART_FML);
            $("#tab1_PartName").val(data.ListtspartTranfer[0].PART_NM);
            $("#tab1_PackingLot").val(data.PackingLotCode);

            if (data.ListtdpdmtimTranfer.length > 0) {
                $("#tab1_Date").val(formatToDateInput(data.ListtdpdmtimTranfer[0].VLID_DTM));
                $("#tab1_ECN").val(getECN(data.ListtdpdmtimTranfer[0].VLID_BARCODE));
                $("#tab1_Shipped").val(data.ListtdpdmtimTranfer[0].VLID_DSCN_YN);
                $("#Table1 tbody").empty();

                for (let i = 0; i < data.ListtdpdmtimTranfer.length; i++) {
                    prependRow({
                        PartNo: data.ListtspartTranfer[0].PART_NO,
                        LotCode: data.ListtdpdmtimTranfer[i].VLID_BARCODE,
                        Line: data.ListtspartTranfer[0].PART_CAR,
                        Family: data.ListtspartTranfer[0].PART_FML,
                        ECN: getECN(data.ListtdpdmtimTranfer[i].VLID_BARCODE),
                        CreateDate: data.ListtdpdmtimTranfer[i].CREATE_DTM,
                        CreatedBy: data.ListtdpdmtimTranfer[i].CREATE_USER,
                        DSCN_YN: data.ListtdpdmtimTranfer[i].VLID_DSCN_YN
                    });
                }
            }

            ShowMessage(localizedText.Success, 'OK');
        } else if (data.ErrorNumber === -1) {
            ShowMessage(localizedText.Empty, 'error');
        }

        $("#BackGround").css("display", "none");

    } catch (err) {
        ShowMessage(err.message, 'error');
    }

    $("#tab1_QRCodeScan").val("");
    $("#tab1_QRCodeScan").focus();
}


function Buttonadd_Click() {

}
function Buttonsave_Click() {

}
function Buttondelete_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            const IDCard = $("#tab1_PackingLot").val().trim().toUpperCase();
            const shipped = $("#tab1_Shipped").val().trim().toUpperCase();
            if (IDCard.includes(customerCode)) {
                if (shipped === "N") {

                    showDeleteModal(function (result) {
                        if (result) {
                            PrintLabel();
                        } else {
                            $("#BackGround").css("display", "none");
                        }
                    });

                } else {
                    ShowMessage(localizedText.CannotDelete, "error");
                }

            }
            else {
                ShowMessage("Mã Packing không hợp lệ", "error");
            }
        } else if (currentTabId === 'Tag002') {
            deleteMultiRows();
        }
    }

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            TableHTMLToExcel("Table1", "ScanList", "D03_Scan_list");

        } else if (currentTabId === 'Tag002') {
            ExportToExcelFull()

        }
    }
}
function Buttonprint_Click() {
    PrintLabel();
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

//tạo thông báo khi thao tác
function ShowMessage(message, type) {

    if (type === 'error') {
        M.toast({ html: message, classes: 'red' });
    } else if (type === 'warning') {
        M.toast({ html: message, classes: 'orange' });
    }
    else {
        M.toast({ html: message, classes: 'green' });
    }
    $("#BackGround").css("display", "none");
}

$(document).ready(function () {

});

// xóa các dong dữ liệu đã chọn
async function deleteMultiRows() {
    const selectedParts = getSelectedRowsData();

    if (selectedParts.length === 0) {
        ShowMessage(localizedText.Empty, "error");
        return;
    }

    const message = `${localizedText.TotalDelete} = ${selectedParts.length} (EA)`;

    showConfirmDeleteAllModal(message, async function (confirmed) {
        if (!confirmed) {
            $("#BackGround").hide();
            return;
        }

        const remark = $("#inputConfirmDelete").val()?.trim() || "";
        if (!remark) {
            ShowMessage(`${localizedText.Empty}: ${localizedText.Remark}`, "error");
            return;
        }

        const baseParameter = {
            ListtspartTranfer: selectedParts,
            USER_ID: GetCookieValue("UserID"),
            REMARK: remark,
            Code: "DeleteAll"
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(baseParameter));

        $("#BackGround").fadeIn();

        try {
            const response = await fetch(controller + "Buttondelete_Click", {
                method: "POST",
                body: formUpload
            });

            const data = await response.json();
            ShowMessage(data.Error, data.ErrorNumber === 0 ? "ok" : "error");

            // Chỉ reload lại lịch sử nếu xóa thành công
            if (data.ErrorNumber === 0) {
                SearchHistory();
            }
        } catch (err) {
            ShowMessage(err.message || err, "error");
        } finally {
            $("#BackGround").fadeOut();
        }
    });
}

function showConfirmDeleteAllModal(message, callback) {
    $('#confirmDeleteMessage').text(message);

    var modalElem = document.getElementById('confirmDeleteModal');
    var instance = M.Modal.getInstance(modalElem);
    instance.open();

    $('#btn_confirmDeleteAllYes').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#btn_confirmDeleteAllCancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}

function getSelectedRowsData() {
    const selectedRows = [];

    $('.row-check:checked').each(function () {
        const row = $(this).closest('tr'); // Lấy dòng chứa checkbox
        const cells = row.find('td');      // Lấy tất cả ô trong dòng

        const rowData = {
            IV_IDX: $(cells[0]).text().trim(),
            PART_NO: $(cells[2]).text().trim(),
            PART_NM: $(cells[3]).text().trim(),
            PART_CAR: $(cells[4]).text().trim(),
            PART_FML: $(cells[5]).text().trim(),
            SNP_QTY: $(cells[6]).text().trim(),
            PKG_GRP: $(cells[7]).text().trim(),
            QTY: $(cells[8]).text().trim(),
            MTIN_DTM: $(cells[9]).text().trim(),
            CREATE_DTM: $(cells[10]).text().trim(),
            LOT_CODE: $(cells[11]).text().trim(),
            REMARK: $(cells[12]).text().trim(),
            CREATE_USER: $(cells[13]).text().trim(),
            DSCN_YN: $(cells[14]).text().trim(),
            DeleteTime: $(cells[15]).text().trim(),
            DeleteBy: $(cells[16]).text().trim()
        };

        selectedRows.push(rowData);
    });

    return selectedRows;
}

async function SearchHistory() {
    const fromDate = $("#tab2_fromDate").val()?.trim();
    const toDate = $("#tab2_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS"]:checked')?.value || "all";
    const PartCode = $("#tab2_PartCode").val()?.trim() || "";
    const partName = $("#tab2_PartName").val()?.trim() || "";
    const PackingLot = $("#tab2_PackingCode").val()?.trim() || "";
    const Stage = $("#tab2_Stage").val()?.trim() || "";

    //truyền các biến trước khi gửi lên server
    const BaseParameter = {
        FilterType: selectedValue,
        LeadData: Stage,
        PartNo: PartCode,
        PartName: partName,
        PackingLotCode: PackingLot,
        FromDate: fromDate,
        ToDate: toDate,
        Code: "LoadHistory"
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    //thục hiện hiên thị process loadding...
    $("#BackGround").fadeIn();

    try {
        //truyền và chờ server trả kết quả
        const response = await fetch(controller + "Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });
        //chuyển đổi dữ liệu nhận được
        const data = await response.json();

        if (data.ListtspartTranfer && data.ListtspartTranfer.length > 0) {

            //khai báo biến lưu trữ duex liệu nhận về
            const rows = [];
            const stats = {
                totalY: 0,
                totalN: 0,
                totalD: 0,
                uniqueSet: new Set(),
                deletedSet: new Set()
            };

            //thục hiện đẩy dữ liệu vào bảng
            data.ListtspartTranfer.forEach((rowData, i) => {
                const isDeleted = rowData.DSCN_YN === "D";
                const isShipped = rowData.DSCN_YN === "Y";

                if (isDeleted) {
                    stats.totalD++;
                    stats.deletedSet.add(rowData.PKG_GRP);
                } else if (isShipped) {
                    stats.totalY++;
                } else {
                    stats.totalN++;
                }

                stats.uniqueSet.add(rowData.PKG_GRP);
                let addcheckbox = `<label><input type="checkbox" class="row-check" data-part="${rowData.PART_NO}"><span></span></label>`;
                if (rowData.DSCN_YN !== "N") {
                    addcheckbox = '';
                }
                //khai báo 1 dong dữ liệu cần đẩy vào table
                const row = [
                    i + 1,
                    addcheckbox,
                    rowData.PART_NO || "",
                    rowData.PART_NM || "",
                    rowData.PART_CAR || "",
                    rowData.PART_FML || "",
                    rowData.SNP_QTY ?? "",
                    rowData.PKG_GRP || "",
                    rowData.QTY ?? "",
                    FormatDate(rowData.MTIN_DTM),
                    FormatDateTime(rowData.CREATE_DTM),
                    `<span class="preserve-space">${rowData.LOT_CODE || ""}</span>`,
                    rowData.REMARK || "",
                    rowData.CREATE_USER || "",
                    rowData.DSCN_YN || "",
                    FormatDateTime(rowData.DeleteTime),
                    rowData.DeleteBy || ""
                ];
                //đẩy dòng dữ liệu vào danh sách giúp cải thiện hiệu năng
                rows.push(row);
            });

            //load dữ liệu lên table vói thư viện DataTable 
            if (table2) {
                table2.clear();
                table2.rows.add(rows);
                table2.draw();
            }

            $('#checkAll').prop('checked', false);
            // Gán thống kê
            $("#tab2_TotalQty").val(stats.totalY + stats.totalN);
            $("#tab2_DistinctPartNo").val(stats.uniqueSet.size - stats.deletedSet.size);
            $("#tab2_TotalY").val(stats.totalY);
            $("#tab2_totalN").val(stats.totalN);

            ShowMessage(localizedText.Success, "ok");
        } else {
            ShowMessage(localizedText.Empty, "error");
            if (table2) {
                table2.clear();
                table2.draw();
            }
        }

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}

let isProcessing = false;

$("#tab1_QRCodeScan").on('keydown', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        const barcode = $("#tab1_QRCodeScan").val().trim().toUpperCase();
        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập

        if (barcode) {
            queueScan(barcode);
        }
    }
});

let scanQueue = [];
let isRunningScan = false;

// Thêm mã vào hàng đợi
function queueScan(barcode) {
    scanQueue.push(barcode);
    if (!isRunningScan) {
        processScanQueue();
    }
}

// Xử lý từng mã một
async function processScanQueue() {
    isRunningScan = true;

    const input = document.getElementById("tab1_QRCodeScan");

    while (scanQueue.length > 0) {
        const barcode = scanQueue.shift();
        input.readOnly = true;

        try {
            if (barcode.length <= 15 && barcode.includes(customerCode)) {
                await LoadPackingList(barcode);
            }
            else {
                $("#BackGround").css("display", "block");
                await ScanQRCode(barcode);
                $("#BackGround").css("display", "none");
            }
        } catch (err) {

            ShowMessage("Lỗi khi xử lý mã vạch:" + err, "error");
        }

        input.readOnly = false;
        await delay(200); // có thể tăng lên nếu cần thời gian render
    }

    isRunningScan = false;
}

// Hàm delay nhỏ giữa các lần xử lý
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}


async function ScanQRCode(lotCode) {
    if (!lotCode || lotCode.trim().length < 15) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    let isExist = false;
    $("#DataGridView1 tr").each(function () {
        const existingLotCode = $(this).find("td:eq(1)").text().trim().toUpperCase();
        if (existingLotCode === lotCode) {
            isExist = true;
            return false;
        }
    });

    if (isExist) {
        ShowMessage(localizedText.Scanned, 'warning');
        return;
    }

    const PartCode = $("#tab1_PartCode").val()?.trim() || "";
    const ECN = $("#tab1_ECN").val()?.trim() || "";
    const SNP = $("#tab1_SNP").val()?.trim() || "0";
    const PackingLot = $("#tab1_PackingLot").val()?.trim() || "";
    const TotalQty = $("#tab1_TotalQty").val()?.trim() || "0";

    const snpQty = parseInt(SNP, 10);
    const totalQty = parseInt(TotalQty, 10);

    if (totalQty >= snpQty && totalQty > 0) {
        ShowMessage(localizedText.FullBox, 'error');
        return;
    }

    if (!PartCode || !ECN || snpQty <= 0 || totalQty === 0) {
        await FirstScan(lotCode);
    } else {
        await NextScan(lotCode);
    }

    $("#tab1_QRCodeScan").val("").focus();
}

$("#Delete_QRCodeScan").on('keydown', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();

        const Delete_Remark = $("#Delete_Remark").val().trim().toUpperCase();
        if (!Delete_Remark) {
            ShowMessage(localizedText.Remark + " (empty data)", 'error');
            $("#Delete_QRCodeScan").val("").focus();
            return;
        }
        const lotCode = $("#Delete_QRCodeScan").val().trim().toUpperCase();

        if (lotCode) {
            $("#BackGround").css("display", "block");
            // Kiểm tra xem lotCode đã tồn tại trong bảng chưa
            let isExist = false;
            $("#DataGridView1 tr").each(function () {
                const existingLotCode = $(this).find("td:eq(1)").text().trim().toUpperCase(); // Cột thứ 2
                if (existingLotCode === lotCode) {
                    isExist = true;
                    return false; // Dừng vòng lặp
                }
            });

            if (!isExist) {
                ShowMessage(localizedText.WrongProduct, 'error');
                $("#Delete_QRCodeScan").val("").focus();
                return;
            }

            let BaseParameter = new Object();
            BaseParameter = {
                Barcode: lotCode,
                PartNo: $("#tab1_PartCode").val().trim(),
                PackingLotCode: $("#tab1_PackingLot").val().trim(),
                Remark: Delete_Remark,
                USER_ID: GetCookieValue("UserID")
            }
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = controller + "Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    if (data.ErrorNumber === 0) {
                        //them vào danh sách đã xóa ở đây
                        const tbody = document.getElementById("DataGridView3");
                        const newRow = document.createElement("tr");

                        newRow.innerHTML = `
                    <td>${1}</td>
                    <td>${BaseParameter.PackingLotCode}</td>
                    <td>${getCurrentDateTime()}</td>
                    <td  class="preserve-space">${BaseParameter.Barcode}</td>
                    <td>${BaseParameter.USER_ID}</td>`;
                        // Chèn dòng vào đầu tbody
                        tbody.insertBefore(newRow, tbody.firstChild);

                        //thục hiện xóa trong lưới chính
                        removeRowByLotCode(BaseParameter.Barcode);

                        ShowMessage(localizedText.Success, 'ok');
                    }
                    else if (data.ErrorNumber === -1) {
                        ShowMessage(localizedText.WrongCode, 'error');
                    }
                    else {
                        ShowMessage(localizedText.CannotDelete, 'error');
                    }

                    $("#Delete_QRCodeScan").val("").focus();
                }).catch((err) => {
                    ShowMessage(err, 'error');
                    $("#Delete_QRCodeScan").val("").focus();
                })
            });
        }
    }
});


async function FirstScan(lotCode) {
    if (!lotCode.includes("0000")) {
        ShowMessage(localizedText.WrongProduct + " (" + customerCode + ")", "ok");
        return;
    }

    // Kiểm tra lấy được mã SP
    const part_Code = getPartNo(lotCode);
    if (!part_Code) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const ecnNo = getECN(lotCode);
    if (!ecnNo) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    $("#tab1_PartCode").val(part_Code);
    $("#tab1_ECN").val(ecnNo);

    const BaseParameter = {
        Barcode: lotCode.trim().toUpperCase(),
        PartNo: part_Code.toUpperCase(),
        PackingLotCode: " ",
        USER_ID: GetCookieValue("UserID"),
        CustomerCode: customerCode
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    const url = controller + "Buttonadd_Click";

    try {
        const response = await fetch(url, {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();
        RenderDatatoView(data, lotCode, ecnNo);

    } catch (err) {
        ShowMessage(err.message || err, 'error');
    }
}


async function NextScan(lotCode) {
    // Kiểm tra lấy được mã SP
    const part_Code = getPartNo(lotCode);
    if (!part_Code) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const ecnNo = getECN(lotCode);
    if (!ecnNo) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const D13_PartCode = $("#tab1_PartCode").val()?.trim() || "";
    const D13_ECN = $("#tab1_ECN").val()?.trim() || "";
    const D13_SNP = $("#tab1_SNP").val()?.trim() || "";

    // Kiểm tra mã SP và ECN khớp với dữ liệu đang xử lý
    if (D13_PartCode.replace(/[+\-*/]/g, '') === part_Code.replace(/[+\-*/]/g, '') && D13_ECN === ecnNo) {
        // Kiểm tra trùng mã vạch trong bảng
        let isExist = false;
        $("#DataGridView1 tr").each(function () {
            const existingLotCode = $(this).find("td:eq(1)").text().trim().toUpperCase();
            if (existingLotCode === lotCode) {
                isExist = true;
                return false;
            }
        });

        if (isExist) {
            ShowMessage(localizedText.Scanned, 'warning');
            return;
        }

        $("#BackGround").css("display", "block");

        const BaseParameter = {
            Barcode: lotCode.trim().toUpperCase(),
            PartNo: part_Code.toUpperCase(),
            PackingLotCode: $("#tab1_PackingLot").val().trim(),
            USER_ID: GetCookieValue("UserID")
        };

        const formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        const url = controller + "Buttonadd_Click";

        try {
            const response = await fetch(url, {
                method: "POST",
                body: formUpload
            });

            const data = await response.json();
            RenderDatatoView(data, lotCode, ecnNo);

        } catch (err) {
            ShowMessage(err.message || err, 'error');
        }

    } else {
        ShowMessage(localizedText.WrongProduct, 'error');
    }
}


/**
 * Lấy ra mã sản phẩm trong QR code
 * @param {string} barcodeText 
 * @returns {string}
 */
function getPartNo(barcodeText) {
    let trimmedText = barcodeText.trim().toUpperCase();
    let index = trimmedText.indexOf("0000");

    // Nếu không tìm thấy "0000", trả về lỗi
    if (index === -1) {
        return "";
    }

    // Lấy vị trí bắt đầu dữ liệu sau "0000"
    let startIndex = index + 4 + 1; // +4 ký tự "HDPP" +1 vì JavaScript index từ 0

    // Kiểm tra độ dài chuỗi
    if (startIndex > trimmedText.length) {
        return "";
    }

    // Lấy 10 ký tự từ vị trí startIndex
    let partSu = trimmedText.substring(0, 10).trim();

    if (partSu.length < 10) {
        return "";
    }

    return partSu;
}
/**
 * lấy mã ECN trong lot code
 * @param {any} barcodeText
 * @returns
 */
function getECN(barcodeText) {
    let trimmedText = barcodeText.trim().toUpperCase();
    return trimmedText.slice(-10).trim(); // cat 10 ký tự cuối cùng
}

function getCurrentDateTime() {
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}

async function RenderDatatoView(data, lotcode, ecn) {
    if (data.ErrorNumber === 0) {
        $("#tab1_PartCode").val(data.ListtspartTranfer[0].PART_NO);
        $("#tab1_SNP").val(data.ListtspartTranfer[0].PART_SNP);
        $("#tab1_Line").val(data.ListtspartTranfer[0].PART_CAR);
        $("#tab1_Family").val(data.ListtspartTranfer[0].PART_FML);
        $("#tab1_PartName").val(data.ListtspartTranfer[0].PART_NM);
        $("#tab1_PackingLot").val(data.PackingLotCode);
        prependRow({
            PartNo: data.ListtspartTranfer[0].PART_NO,
            LotCode: lotcode,
            Line: data.ListtspartTranfer[0].PART_CAR,
            Family: data.ListtspartTranfer[0].PART_FML,
            ECN: ecn,
            CreateDate: getCurrentDateTime(),
            CreatedBy: GetCookieValue("UserID"),
            DSCN_YN: data.ListtspartTranfer[0].DSCN_YN
        });
        ShowMessage(localizedText.Success, 'OK');

    }
    else if (data.ErrorNumber === 1) {
        showConfirmModal(localizedText.confirmScanMore, async function (result) {
            if (result) {
              await  LoadPackingList(lotcode);
            } else {
                $("#BackGround").css("display", "none");
            }
        });
    }
    else if (data.ErrorNumber === 2) {
        ShowMessage(localizedText.Shipped + " (Shipped)", 'error');
    }
    else if (data.ErrorNumber === -1) {
        ShowMessage(localizedText.Empty, 'error');
    }
    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
}

function prependRow(rowData) {
    const tbody = document.getElementById("DataGridView1");
    const newRow = document.createElement("tr");
    newRow.innerHTML = `
        <td>${rowData.PartNo}</td>
        <td class="preserve-space">${rowData.LotCode}</td>
        <td>${rowData.Line}</td>
        <td>${rowData.Family}</td>
        <td>${rowData.ECN}</td>
        <td>${FormatDateTime(rowData.CreateDate)}</td>
        <td>${rowData.CreatedBy}</td>
        <td>${rowData.DSCN_YN}</td>
    `;
    // Chèn dòng vào đầu tbody
    tbody.insertBefore(newRow, tbody.firstChild);
    //load tổng số đã Scan
    let total = $("#DataGridView1 tr").length;
    $("#tab1_TotalQty").val(total);

}

async function PrintLabel() {
    const partCode = $("#tab1_PartCode").val().trim();
    const SNP = $("#tab1_SNP").val().trim();
    const Line = $("#tab1_Line").val()?.trim() || " ";
    const date = $("#tab1_Date").val().trim();
    const Family = $("#tab1_Family").val().trim();
    const PartName = $("#tab1_PartName").val().trim();
    const TotalQty = $("#tab1_TotalQty").val().trim();
    const PackingLot = $("#tab1_PackingLot").val().trim();

    if (partCode && SNP && Line && Family && TotalQty && PackingLot) {
        let valuesSend = {
            PartNo: partCode,
            SerialNo: PackingLot,
            MfgDate: date, // yyyy-MM-dd
            Description: Family,
            Line: Line,
            Quantity: TotalQty,
            CustumerCode: customerCode
        };
        const url = `/html/DH_Lable.html?PartNo=${valuesSend.PartNo}&SerialNo=${PackingLot}&MfgDate=${valuesSend.MfgDate}&Description=${PartName}&Line=${Line}&Family=${Family}&Quantity=${TotalQty}`;
        window.open(url, "_blank");

        if (SNP === TotalQty) {
            Buttonfind_Click();
        }

    } else {
        ShowMessage(localizedText.Empty, 'error');
    }
}

function removeRowByLotCode(lotCode) {
    const tbody = document.getElementById("DataGridView1");
    const rows = tbody.getElementsByTagName("tr");

    for (let i = 0; i < rows.length; i++) {
        const cells = rows[i].getElementsByTagName("td");
        // Giả sử cột LotCode là cột thứ 2 (index 1)
        if (cells[1] && cells[1].textContent.trim() === lotCode) {
            tbody.removeChild(rows[i]);

            let total = parseInt($("#tab1_TotalQty").val(), 10); // chuyển về số nguyên
            if (!isNaN(total) && total > 0) {
                $("#tab1_TotalQty").val(total - 1);
            }

        }
    }
}

function showConfirmModal(message, callback) {
    $('#confirmMessage').text(message);

    var modalElem = document.getElementById('confirmModal');
    var instance = M.Modal.getInstance(modalElem);
    instance.open();

    $('#btnYes').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#btnCancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}

function showDeleteModal(callback) {
    const tbody = document.getElementById("DataGridView3");
    tbody.innerHTML = "";
    var modalElem = document.getElementById('deleteModal');
    var instance = M.Modal.getInstance(modalElem);
    instance.open();

    $('#delete_PrintLable').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#delete_Cancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}

//thục hiện truy vấn lên server vói các diều kiện tìm kiếm và xuất excel
async function ExportToExcelFull() {
    const fromDate = $("#tab2_fromDate").val()?.trim();
    const toDate = $("#tab2_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS"]:checked')?.value || "all";
    const PartCode = $("#tab2_PartCode").val()?.trim() || "";
    const partName = $("#tab2_PartName").val()?.trim() || "";
    const PackingLot = $("#tab2_PackingCode").val()?.trim() || "";
    const Stage = $("#tab2_Stage").val()?.trim() || "";

    //truyền các biến trước khi gửi lên server
    const BaseParameter = {
        FilterType: selectedValue,
        LeadData: Stage,
        PartNo: PartCode,
        PartName: partName,
        PackingLotCode: PackingLot,
        FromDate: fromDate,
        ToDate: toDate,
        Code: "LoadHistory"
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    //thục hiện hiên thị process loadding...
    $("#BackGround").fadeIn();

    try {
        //truyền và chờ server trả kết quả
        const response = await fetch(controller + "Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });
        //chuyển đổi dữ liệu nhận được
        const data = await response.json();

        if (data.ListtspartTranfer && data.ListtspartTranfer.length > 0) {
            //lọc các trường cần xuất excel
            const exportData = data.ListtspartTranfer.map(x => ({
                "Part No": x.PART_NO,
                "Part Name": x.PART_NM,
                "Car": x.PART_CAR,
                "FML": x.PART_FML,
                "SNP": x.SNP_QTY,
                "Group": x.PKG_GRP,
                "QTY": x.QTY,
                "Date": FormatDate(x.MTIN_DTM),
                "Scan Time": FormatDateTime(x.CREATE_DTM),
                "LOT CODE": x.LOT_CODE,
                "Remark": x.REMARK,
                "User": x.CREATE_USER,
                "DSCN_YN": x.DSCN_YN,
                "Delete Time": FormatDateTime(x.DeleteTime),
                "Delete By": x.DeleteBy
            }));
            ExportJSONToExcel(exportData, "Scan history", "D16_ScanHistory.xlsx");

            ShowMessage(localizedText.Success, "ok");
        } else {
            ShowMessage(localizedText.Empty, "error");
        }

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}

//xuất toàn bộ jsion kết quả truy vấn ra file excel
function ExportJSONToExcel(data, sheetMame, fileName = "Export.xlsx") {
    if (!data || data.length === 0) {
        alert("Không có dữ liệu để xuất.");
        return;
    }

    // Chuyển dữ liệu sang định dạng sheet
    const worksheet = XLSX.utils.json_to_sheet(data);

    // Tạo workbook
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, sheetMame);

    // Xuất file
    XLSX.writeFile(workbook, fileName);
}