let BaseResult;
let curentTab = undefined;
let coustomerCode = "BV";
let controller = "/D14/";
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
function Buttonfind_Click() {

    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');
        curentTab = currentTabId;
        if (currentTabId === 'Tag001') {
            setDefault();
        } else if (currentTabId === 'Tag002') {
            SearchHistory();
        }
    }
}

function setDefault() {
    const select = document.getElementById("tab1_select");
    select.value = "Product1"; // đổi sang lựa chọn dau tiên
    $("#tab1_QRCodeScan").val("");
    $("#tab1_Date").val(new Date().toLocaleDateString('en-CA'));
    $("#tab1_PartCode").val("");
    $("#tab1_PartName").val("");
    $("#tab1_ECN").val("");
    $("#tab1_SNP").val("");
    $("#tab1_Line").val("");
    $("#tab1_Family").val("");
    $("#tab1_TotalQty").val("0");
    $("#tab1_PackingLot").val("");
    $("#tab1_partNoCheck1").val("");
    $("#tab1_PackingLotNoCheck1").val("");
    $("#tab1_CheckResult1").val("");
    $("#tab1_partNoCheck2").val("");
    $("#tab1_PackingLotNoCheck2").val("");
    $("#tab1_CheckResult2").val("");
    $("#tab1_CardQuantity").val("");
    LoadBoxStatus();
    $("#tab1_QRCodeScan").focus();
    $("#Table1 tbody").empty();

}
function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D14/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D14/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D14/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D14/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            TableHTMLToExcel("Table1", "ScanList", "D14_Scan_list");

        } else if (currentTabId === 'Tag002') {
            ExportToExcelFull()

        }
    }
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D14/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

document.addEventListener("DOMContentLoaded", function () {
    const radios = document.querySelectorAll('input[name="STATUS"]');
    radios.forEach(function (radio) {
        radio.addEventListener("change", function () {
            SearchHistory();
        });
    });
});

let isProcessing = false;

$("#tab1_QRCodeScan").on('keydown', async function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        const scanType = document.getElementById("tab1_select").value;
        const barcode = $("#tab1_QRCodeScan").val().trim().toUpperCase();
        const PartCode = $("#tab1_PartCode").val().trim().toUpperCase();
        const PackingLot = $("#tab1_PackingLot").val().trim().toUpperCase();
        const partNoCheck1 = $("#tab1_partNoCheck1").val().trim().toUpperCase();
        const PackingLotNoCheck1 = $("#tab1_PackingLotNoCheck1").val().trim().toUpperCase();
        const partNoCheck2 = $("#tab1_partNoCheck2").val().trim().toUpperCase();
        const PackingLotNoCheck2 = $("#tab1_PackingLotNoCheck2").val().trim().toUpperCase();
        const CardQuantity = $("#tab1_CardQuantity").val().trim().toUpperCase();
        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
        if (scanType && barcode) {
            //$("#BackGround").css("display", "block");

            if (scanType === "Product1" && barcode.substring(0, 1) === "P") {

                if (PartCode.length >= 4 && barcode.substring(1, barcode.length) === PartCode) {
                    // truong hợp đã scan idcard truoc do
                    ShowMessage("Đã Scan ID Card 1", "error");
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                }
                else if (PartCode.length >= 4 && barcode.substring(1, barcode.length) != PartCode) {
                    ShowMessage("ID Card không đúng", "error");
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                }
                else {
                    let pn = barcode.substring(1, barcode.length);
                    $("#tab1_PartCode").val(pn);
                    $("#tab1_partNoCheck1").val(pn);
                    ShowMessage("Bước 2: Scan Mã ID Card", "ok");
                    SetSelector("Card1");
                }
            }
            else if (scanType === "Card1" && barcode.substring(0, 2) === "3S") {
                if (PartCode.length > 4 && PartCode === partNoCheck1) {
                    if (barcode === PackingLotNoCheck1) {
                        ShowMessage("Đã Scan mã ID Card, thuc hiện bước tiếp theo", "error");
                        SetSelector("Product2"); // đổi sang bước scan partno 2
                    }
                    else {
                        $("#tab1_PackingLotNoCheck1").val(barcode);
                        ShowMessage("Bước 3: Scan Mã PartNo 2", "ok");
                        SetSelector("Product2");
                    }
                }
                else {
                    ShowMessage("Chưa scan part no, thao tác sai", "error");
                }
            }
            else if (scanType === "Product2" && barcode.substring(0, 1) === "P") {
                if (PartCode.length >= 4 && barcode.substring(1, barcode.length) === PartCode) {
                    if (partNoCheck2.length >= 4 && partNoCheck2 === barcode.substring(1, barcode.length)) {
                        ShowMessage("Đã Scan ID Card 2", "error");
                        SetSelector("Card2");
                    }
                    else if (partNoCheck2.length >= 4 && partNoCheck2 != barcode.substring(1, barcode.length)) {
                        ShowMessage("ID Card không đúng", "error");
                        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                    }
                    else {
                        let pn = barcode.substring(1, barcode.length);
                        $("#tab1_partNoCheck2").val(pn);
                        ShowMessage("Bước 4: Scan Mã ID Card 2", "ok");
                        SetSelector("Card2");
                    }

                }
                else if (PartCode.length >= 4 && barcode.substring(1, barcode.length) != PartCode) {
                    ShowMessage("ID Card không đúng", "error");
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                }
                else {
                    ShowMessage("ID Card không đúng kiểm tra lại", "error");
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                }
            }
            else if (scanType === "Card2" && barcode.substring(0, 2) === "3S") {
                if (PackingLotNoCheck1 === barcode) {
                    $("#tab1_PackingLotNoCheck2").val(barcode);
                    ShowMessage("Bước 5: Scan số lượng trên ID Card", "ok");
                    SetSelector("QtyCard");
                }
                else {
                    ShowMessage("ID Card không đúng", "error");
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                }
            }
            else if (scanType === "QtyCard" && barcode.substring(0, 1) === "Q") {
                if (PartCode === partNoCheck1 && partNoCheck1 === partNoCheck2 && PackingLotNoCheck1 === PackingLotNoCheck2) {
                    // thục hiện trủy vấn mã part no có hay chưa và đăng ký id Card vào databse
                    let snpQty = barcode.substring(1, barcode.lenght).trim();
                    $("#tab1_CardQuantity").val(snpQty);
                    $("#tab1_CheckResult1").val("OK");
                    $("#tab1_CheckResult2").val("OK");
                    $("#tab1_PackingLot").val(PackingLotNoCheck1);
                    $("#tab1_SNP").val(snpQty);
                    await UpdateIDCard(PartCode, PackingLotNoCheck1);
                }
                else {
                    ShowMessage("Các thông tin không khớp, lỗi thao tác", "error");
                    SetSelector("Product1");
                }
            }
            else if (scanType === "Lotcode") {
                if (!isProcessing) {
                    isProcessing = true;
                    //thuc hiện đẩy thông tin dong gói về server
                    //await NextScan(barcode);
                    queueScan(barcode);
                    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
                    isProcessing = false;
                } else {
                    ShowMessage("Đang xử lý chưa hoàn thành", "error");
                }

            }
            else {
                ShowMessage("KHÔNG ĐÚNG MÃ YÊU CẦU, KIỂM TRA LẠI", "error");
                $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
            }
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
            $("#BackGround").css("display", "block");
            await NextScan(barcode);
            $("#BackGround").css("display", "none");
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

async function UpdateIDCard(PartCode, PackingLotNo) {
    const BaseParameter = {
        PartNo: PartCode,
        PackingLotCode: PackingLotNo,
        USER_ID: GetCookieValue("UserID"),
        Code: "AddIDCard"
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    $("#BackGround").fadeIn();

    try {

        const response = await fetch(controller + "Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();


        if (data.ErrorNumber === -1) {
            ShowMessage(localizedText.WrongCode + " (" + data.Message + ")", 'error');
        }
        else {

            if (data.ListtspartTranfer && data.ListtspartTranfer.length >= 1) {
                $("#tab1_PartCode").val(data.ListtspartTranfer[0].PART_NO);
                $("#tab1_PartName").val(data.ListtspartTranfer[0].PART_NM);
                $("#tab1_Line").val(data.ListtspartTranfer[0].PART_CAR);
                $("#tab1_Family").val(data.ListtspartTranfer[0].PART_FML);
            }

            if (data.ListtdpdmtimTranfer && data.ListtdpdmtimTranfer.length > 0) {
                $("#tab1_ECN").val(getECN(data.ListtdpdmtimTranfer[0].VLID_BARCODE));
                $("#tab1_Shipped").val(data.ListtdpdmtimTranfer[0].VLID_DSCN_YN);

                for (let i = 0; i < data.ListtdpdmtimTranfer.length; i++) {
                    prependRow({
                        PartNo: data.ListtspartTranfer[0].PART_NO,
                        LotCode: data.ListtdpdmtimTranfer[i].VLID_BARCODE,
                        Line: data.ListtspartTranfer[0].PART_CAR,
                        Family: data.ListtspartTranfer[0].PART_FML,
                        ECN: getECN(data.ListtdpdmtimTranfer[i].VLID_BARCODE),
                        CreateDate: data.ListtdpdmtimTranfer[i].CREATE_DTM,
                        CreatedBy: data.ListtdpdmtimTranfer[i].CREATE_USER,
                        DSCN_YN: data.ListtdpdmtimTranfer[i].VLID_DSCN_YN,
                    });
                }
            }

            ShowMessage("Thông tin ID card đá chính xác, hãy thực hiện scan sản phẩm đóng gói", "ok");
            SetSelector("Lotcode");
        }

    } catch (err) {
        ShowMessage(err.message, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
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


async function RenderDatatoView(data) {
    if (data.ErrorNumber === 0) {
        $("#tab1_PartCode").val(data.ListtspartTranfer[0].PART_NO);
        $("#tab1_SNP").val(data.ListtspartTranfer[0].PART_SNP);
        $("#tab1_Line").val(data.ListtspartTranfer[0].PART_CAR);
        $("#tab1_Family").val(data.ListtspartTranfer[0].PART_FML);
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

async function LoadPackingList(lotcode) {
    if (!lotcode) return;

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
            const part = data.ListtspartTranfer[0];

            $("#tab1_PartCode").val(part.PART_NO);
            $("#tab1_SNP").val(part.PART_SNP);
            $("#tab1_Line").val(part.PART_CAR);
            $("#tab1_Family").val(part.PART_FML);
            $("#tab1_PartName").val(part.PART_NM);
            $("#tab1_PackingLot").val(data.PackingLotCode);
            $("#tab1_PackingLotNoCheck1").val(data.PackingLotCode);
            $("#tab1_PackingLotNoCheck2").val(data.PackingLotCode);

            if (data.ListtdpdmtimTranfer.length > 0) {
                const firstItem = data.ListtdpdmtimTranfer[0];
                $("#tab1_Date").val(formatToDateInput(firstItem.VLID_DTM));
                $("#tab1_ECN").val(getECN(firstItem.VLID_BARCODE));
                $("#tab1_Shipped").val(firstItem.VLID_DSCN_YN);
                $("#Table1 tbody").empty();

                for (let i = 0; i < data.ListtdpdmtimTranfer.length; i++) {
                    const item = data.ListtdpdmtimTranfer[i];
                    prependRow({
                        PartNo: part.PART_NO,
                        LotCode: item.VLID_BARCODE,
                        Line: part.PART_CAR,
                        Family: part.PART_FML,
                        ECN: getECN(item.VLID_BARCODE),
                        CreateDate: item.CREATE_DTM,
                        CreatedBy: item.CREATE_USER,
                        DSCN_YN: item.VLID_DSCN_YN,
                    });
                }
            }

            ShowMessage(localizedText.Success, 'OK');
        } else if (data.ErrorNumber === -1) {
            ShowMessage(localizedText.Empty, 'error');
        } 

    } catch (err) {
        ShowMessage(err.message || err, 'error');
    } finally {
        $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
        SetSelector("Lotcode"); // đổi sang lựa chọn đầu tiên
        $("#BackGround").css("display", "none");
    }
}


function formatToDateInput(dateString) {
    if (!dateString) return "";

    const date = new Date(dateString);
    if (isNaN(date)) return "";

    const yyyy = date.getFullYear();
    const mm = (date.getMonth() + 1).toString().padStart(2, '0'); // getMonth() từ 0–11
    const dd = date.getDate().toString().padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}

function SetSelector(value) {
    const select = document.getElementById("tab1_select");
    select.value = value; // đổi sang lựa chọn dau tiên
    $("#tab1_QRCodeScan").val("").focus(); // Xóa ô nhập
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



async function NextScan(lotCode) {

    if (!lotCode.trim().includes("DJG")) {
        ShowMessage('DJG string not found', 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    } 
    // Kiểm tra lấy được mã SP
    const part_Code = getPartNo(lotCode);
    if (!part_Code) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    let ecnNo = getECN(lotCode);
    if (!ecnNo) {
        ShowMessage(localizedText.WrongCode, 'error');
        $("#tab1_QRCodeScan").val("").focus();
        return;
    }

    const D13_PartCode = $("#tab1_PartCode").val()?.trim() || "";
    let D13_ECN = $("#tab1_ECN").val()?.trim() || "";
    const D13_SNP = $("#tab1_SNP").val()?.trim() || "";
    const Shipped = $("#tab1_Shipped").val()?.trim() || "N";
    const ScannedQty = $("#tab1_TotalQty").val()?.trim() || "0";

    // Gán ECN nếu scan đầu tiên
    if (ScannedQty === "0") {
        D13_ECN = ecnNo;
        $("#tab1_ECN").val(ecnNo);
    }

    if (D13_PartCode === part_Code && D13_ECN === ecnNo) {

        // Kiểm tra trùng mã vạch
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

        const scanQty = $("#tab1_TotalQty").val();
        const SNPQty = $("#tab1_CardQuantity").val();
        const PackingLot = $("#tab1_PackingLot").val().trim().toUpperCase();

        if (parseInt(scanQty) >= parseInt(SNPQty) && parseInt(scanQty) > 0) {
            ShowMessage(localizedText.FullBox, 'error');
            $("#tab1_QRCodeScan").val("").focus();
            return;
        }

        if (Shipped === "Y") {
            ShowMessage(localizedText.Shipped, 'error');
            $("#tab1_QRCodeScan").val("").focus();
            return;
        }

        $("#BackGround").css("display", "block");

        const BaseParameter = {
            Barcode: lotCode.trim().toUpperCase(),
            PartNo: part_Code.toUpperCase(),
            PackingLotCode: PackingLot,
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
function getPartNo(inputString) {
    try {
        inputString = inputString.trim();

        if (inputString.includes("DJG#")) {
            if (inputString.includes("@@$$")) {
                return inputString.substring(0, 15).trim();
            } else {
                if (inputString.substring(0, 5).toUpperCase() === "A 175") {
                    return inputString.substring(0, 15).trim();
                } else {
                    const index = inputString.indexOf("DJG#");
                    let pn = inputString.substring(0, index - 4).trim();
                    return pn;
                }
            }
        }
        else if (inputString.includes("INJEC")) {
            let cat = inputString.split(" ");
            return cat[0].trim();
        }
        else {
            if (inputString.includes("@@$$")) {
                return inputString.substring(0, 15).trim();
            } else {
                const length = inputString.length;

                if (length === 22) {
                    return inputString.substring(0, 6).trim();
                } else if (length === 30) {
                    return inputString.substring(5, 5 + 14).trim();
                } else {
                    const parts = inputString.trim().split(/\s+/);
                    if (parts.length === 2) {
                        return parts[0];
                    }
                    else {
                        const suTx1 = inputString.indexOf("#") + 1;
                        const aaa = inputString.substring(suTx1, suTx1 + 13).trim();
                        const suTx2 = aaa.indexOf("#");
                        return aaa.substring(0, suTx2).trim();
                    }
                  
                }
            }
        }
    } catch (ex) {
        return inputString.substring(0, 13).trim();
    }
}

/**
 * lấy mã ECN trong lot code
 * @param {any} barcodeText
 * @returns
 */
function getECN(barcodeText) {
    let trimmedText = barcodeText.trim().toUpperCase();
    return trimmedText.slice(-7).trim(); // cat 6 ký tự cuối cùng
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
        <td>${rowData.Line}</td>
        <td>${rowData.Family}</td>
        <td>${rowData.ECN}</td>
        <td>${FormatDateTime(rowData.CreateDate)}</td>
        <td>${rowData.CreatedBy}</td>
        <td>${rowData.DSCN_YN}</td>
    `;
    tbody.insertBefore(newRow, tbody.firstChild);
    //load tổng số đã Scan
    let total = $("#DataGridView1 tr").length;
    $("#tab1_TotalQty").val(total);
    // Chèn dòng vào đầu tbody

    LoadBoxStatus();
}

function LoadBoxStatus() {
    const snp = parseInt($("#tab1_CardQuantity").val(), 10);
    const qty = parseInt($("#tab1_TotalQty").val(), 10);
    const button = document.getElementById('Buttonfind');
    const buttonClose = document.getElementById('Buttonclose');

    if (!isNaN(snp) && !isNaN(qty)) {
        const statusText = `${qty}/${snp}`;
        if (qty === snp) {
            $("#tab1_packingStatus")
                .text(statusText + " (OK)");
            $("#tab1_packingStatusBK")
                .css("background-color", "green");

            button.disabled = false;
            button.classList.remove('disabled');
            buttonClose.disabled = false;
            buttonClose.classList.remove('disabled');

        } else if (qty < snp) {
            $("#tab1_packingStatus")
                .text(statusText + " (NG)");
            $("#tab1_packingStatusBK")
                .css("background-color", "red");
            button.disabled = true;
            button.classList.add('disabled'); // nếu dùng Materialize CSS         
            buttonClose.disabled = true;
            buttonClose.classList.add('disabled'); // nếu dùng Materialize CSS
        }
    } else {
        $("#tab1_packingStatus")
            .text("0/0 (NG)");
        $("#tab1_packingStatusBK")
            .css("background-color", "red");
        button.disabled = false;
        button.classList.remove('disabled');
        buttonClose.disabled = false;
        buttonClose.classList.remove('disabled');
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
                LoadBoxStatus();
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
            ExportJSONToExcel(exportData, "Scan history", "D14_ScanHistory.xlsx");

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
