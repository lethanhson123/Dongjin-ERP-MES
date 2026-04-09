let BaseResult;
var ScanList = $('#scanlist');
let coustomerCode = "KGM";
let controller = "/D15/";
let table2;

$(document).ready(function () {
    table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "50vh",
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
    $(".dataTables_scrollBody").css("min-height", "50vh");

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

function LoadBoxNumber() {
    fetch('/D15/load')
        .then(response => {
            if (!response.ok) throw new Error("Lỗi khi gọi API");
            return response.json();
        })
        .then(result => {
            if (result.ErrorNumber === 0 && result.BoxNumber !== undefined) {
                document.getElementById("tab1_BoxNumber").value = result.BoxNumber;
            } else {
                document.getElementById("tab1_BoxNumber").value = "1"; // fallback
            }
        })
        .catch(error => {
            console.error("Lỗi:", error);
            document.getElementById("tab1_BoxNumber").value = "1";
        });
}


function Buttonfind_Click() {
    // Lấy tab đang được chọn
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            let boxnum = parseInt($("#tab1_BoxNumber").val()) + 1;
            if (boxnum <= 0 || boxnum >= 21)
                boxnum = 1;

            $("#tab1_QRCodeScan").val("");
            $("#tab1_BoxNumber").val(boxnum);
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

    $("#BackGround").css("display", "block");

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
    finally {
        $("#BackGround").css("display", "none");
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
            if (IDCard.includes("3S") && IDCard.includes(customerCode)) {
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

$("#tab1_QRCodeScan").on('keydown', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        const barcode = $("#tab1_QRCodeScan").val().trim().toUpperCase();
        if (barcode) {
            $('#tab1_TorqueBarcode').val("").focus();
        }
    }
});

let isProcessing = false;

$("#tab1_TorqueBarcode").on('keydown', async function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        const barcode = $("#tab1_QRCodeScan").val().trim().toUpperCase();
        const TorqueBarcode = $("#tab1_TorqueBarcode").val().trim().toUpperCase();
        const input1 = document.getElementById("tab1_QRCodeScan");
        const input2 = document.getElementById("tab1_TorqueBarcode");
        $("#tab1_QRCodeScan").val("").focus();
        $("#tab1_TorqueBarcode").val("");
        if (barcode && TorqueBarcode) {
            if (!isProcessing) {             
                isProcessing = true;
                input1.readOnly = true;
                input2.readOnly = true;
                if (barcode.length <= 15 && barcode.includes("3S") && barcode.includes(coustomerCode)) {
                    await LoadPackingList(barcode);
                }
                else {
                    await ScanQRCode(barcode, TorqueBarcode);
                }
                input1.readOnly = false;
                input2.readOnly = false;
                isProcessing = false;
            } else {
                ShowMessage("Đang xử lý chưa hoàn thành", "error");
            }
           
        }
    }
});

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
                    <td class="preserve-space">${BaseParameter.Barcode}</td>
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


async function ScanQRCode(lotCode, TorqueBarcode) {
    // Kiểm tra chiều dài hợp lệ
    if (!lotCode || lotCode.trim().length != 38) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_TorqueBarcode").val(""); // Xóa ô nhập
        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập và focus lại
        return;
    }
    if (!lotCode.trim().includes("DJG")) {
        ShowMessage('DJG string not found', 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    } 

    const PartCode = $("#tab1_PartCode").val()?.trim() || "";
    const ECN = $("#tab1_ECN").val()?.trim() || "";
    const SNP = $("#tab1_SNP").val()?.trim() || "0";
    const PackingLot = $("#tab1_PackingLot").val()?.trim() || "";
    const TotalQty = $("#tab1_TotalQty").val()?.trim() || "0";

    if (parseInt(TotalQty) >= parseInt(SNP) && parseInt(TotalQty) > 0) {
        ShowMessage(localizedText.FullBox, 'error');
        $("#tab1_TorqueBarcode").val(""); // Xóa ô nhập
        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập và focus lại
        return;
    }
    // Kiểm tra Scan lần đầu hay là đang đóng gói tiếp
    if (!PartCode || !ECN || !SNP || parseInt(TotalQty) === 0) {
        await FirstScan(lotCode, TorqueBarcode); // Scan lần đầu
    } else if (PartCode && ECN && SNP && parseInt(TotalQty) >= 1) {
        await NextScan(lotCode, TorqueBarcode); // Scan tiếp theo
    }
    $("#tab1_TorqueBarcode").val(""); // Xóa ô nhập
    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập  
}


async function FirstScan(lotCode, TorqueBarcode) {
    const validPatterns = [' 1F ', ' 2F ', ' 3F ', ' 4F ', ' 5F ', ' 6F ', ' 7F ', '8F', ' 9F '];
    const isValid = validPatterns.some(pattern => lotCode.includes(pattern));
    $("#BackGround").css("display", "block");
    if (isValid && TorqueBarcode.includes("KGM/")) {
        // Kiểm tra lấy được mã SP
        const part_Code = getPartNo(lotCode);
        if (!part_Code) {
            ShowMessage(localizedText.WrongCode, 'error');
            $("#tab1_TorqueBarcode").val("");
            $("#tab1_QRCodeScan").val("").focus();
            return;
        }

        const ecnNo = getECN(lotCode);
        if (!ecnNo) {
            ShowMessage(localizedText.WrongCode, 'error');
            $("#tab1_TorqueBarcode").val("");
            $("#tab1_QRCodeScan").val("").focus();
            return;
        }

        $("#tab1_PartCode").val(part_Code);
        $("#tab1_ECN").val(ecnNo);

        const BaseParameter = {
            Barcode: lotCode.trim().toUpperCase(),
            TorqueBarcode: TorqueBarcode.trim().toUpperCase(),
            PartNo: part_Code.toUpperCase(),
            PackingLotCode: " ",
            USER_ID: GetCookieValue("UserID"),
            CustomerCode: coustomerCode,
            CreateDate: $("#tab1_Date").val(),
            BoxNumber: $("#tab1_BoxNumber").val()
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
            RenderDatatoView(data, lotCode, TorqueBarcode.toUpperCase(), ecnNo);

        } catch (err) {
            ShowMessage(err.message || err, 'error');
        }

    } else {
        ShowMessage(localizedText.WrongProduct + " (" + coustomerCode + ")", "error");
    }
    $("#BackGround").css("display", "none");
}


async function NextScan(lotCode, TorqueBarcode) {
    // Kiểm tra lấy được mã SP
    const part_Code = getPartNo(lotCode);
    if (!part_Code) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_TorqueBarcode").val("");
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const ecnNo = getECN(lotCode);
    if (!ecnNo) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_TorqueBarcode").val("");
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const CHK_PartCode = $("#tab1_PartCode").val()?.trim() || "";
    const CHK_ECN = $("#tab1_ECN").val()?.trim() || "";
    const CHK_SNP = $("#tab1_SNP").val()?.trim() || "";
    $("#BackGround").css("display", "block");
    // Kiểm tra PartCode và ECN có khớp không
    if (CHK_PartCode === part_Code && CHK_ECN === ecnNo) {

        // Kiểm tra xem lotCode đã tồn tại trong bảng chưa
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

        const CHK_packingLot = $("#tab1_PackingLot").val()?.trim() || "";

        $("#BackGround").css("display", "block");

        const BaseParameter = {
            Barcode: lotCode.trim().toUpperCase(),
            TorqueBarcode: TorqueBarcode.trim().toUpperCase(),
            PartNo: part_Code.toUpperCase(),
            PackingLotCode: CHK_packingLot,
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
            RenderDatatoView(data, lotCode, TorqueBarcode.toUpperCase(), ecnNo);

        } catch (err) {
            ShowMessage(err.message || err, 'error');
        }

    } else {
        ShowMessage(localizedText.WrongProduct, 'error');
    }
    $("#BackGround").css("display", "none");
}


/**
 * Lấy ra mã sản phẩm trong QR code
 * @param {string} barcodeText 
 * @returns {string}
 */
function getPartNo(barcodeText) {
    let trimmedText = barcodeText.trim().toUpperCase();
    let index = trimmedText.indexOf("*A");
    // Nếu không tìm thấy "HDPP", trả về lỗi
    if (index === -1) {
        return "";
    }
    let part_su = trimmedText.substring(0, 11).replace(/\*/g, '').trim();
    return part_su;
}

/**
 * lấy mã ECN trong lot code
 * @param {any} barcodeText
 * @returns
 */
function getECN(barcodeText) {
    let trimmedText = barcodeText.trim().toUpperCase();
    return trimmedText.slice(-7).trim().replace(/\*/g, ''); // cat 6 ký tự cuối cùng
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

async function RenderDatatoView(data, lotcode, TorderBarcode, ecn) {
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
            REMARK: TorderBarcode,
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
                await LoadPackingList(lotcode);
            } else {
                $("#BackGround").css("display", "none");
            }
        });
    }
    else if (data.ErrorNumber === 2) {
        ShowMessage(localizedText.Scanned, 'error');
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
        <td>${rowData.REMARK}</td>
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
    const Line = $("#tab1_Line").val().trim();
    const date = $("#tab1_Date").val().trim();
    const partName = $("#tab1_PartName").val().trim();
    const TotalQty = $("#tab1_TotalQty").val().trim();
    const PackingLot = $("#tab1_PackingLot").val().trim();
    const boxNo = $("#tab1_BoxNumber").val().trim();
    const lotinPack = getLotInPacking();


    if (partCode && SNP && Line && partName && TotalQty && PackingLot) {

        const url = `/html/KGM_Lable.html?PartNo=${partCode}&SerialNo=${PackingLot}&LotNo=${lotinPack}&MfgDate=${date}&Description=${partName}&Line=${Line}&Quantity=${TotalQty}&Boxno=${boxNo}`;
        window.open(url, "_blank");

        if (SNP === TotalQty) {
            Buttonfind_Click();
        }

    } else {
        ShowMessage(localizedText.Empty, 'error');
    }
}


function getLotInPacking() {
    const tbody = document.getElementById("DataGridView1");
    const rows = tbody.getElementsByTagName("tr");
    let lotinPack = "";
    for (let i = 0; i < rows.length; i++) {
        const cells = rows[i].getElementsByTagName("td");
        if (cells[1] && cells[1].textContent.trim() !== null) {

            const input = cells[1].textContent.trim();
            // Tìm vị trí của "5F"
            const cleaned = input.trim().replace(/\s+/g, " ");
            const parts = cleaned.split(" ");

            let result = "";
            if (parts.length === 4) {
                // Bắt đầu từ sau "5F ", nên +3
                result = parts[2];
                lotinPack += result + " ";
            }

        }
    }

    return lotinPack;
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
            ExportJSONToExcel(exportData, "Scan history", "D15_ScanHistory.xlsx");

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