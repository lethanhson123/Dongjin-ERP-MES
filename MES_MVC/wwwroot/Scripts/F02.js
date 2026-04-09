let BaseResult;
let LineSelected = $("#lineSelected");
let errorSelected = $("#errorSelected");
let table1;
let table2;

//load thông tin hiên thị cho Table1
function initTable1() {
    table1 = $("#Table1").DataTable({
        scrollX: true,
        scrollY: "42vh",
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
    $(".dataTables_scrollBody").css("min-height", "42vh");

    // Chọn tất cả các dòng
    $('#checkAll').on('click', function () {
        const isChecked = $(this).is(':checked');
        $('.row-check').prop('checked', isChecked);
    });

    // Nếu bỏ chọn từng dòng → cập nhật lại checkbox tổng
    $('#Table1 tbody').on('change', '.row-check', function () {
        const allChecked = $('.row-check').length === $('.row-check:checked').length;
        $('#checkAll').prop('checked', allChecked);
    });
}

function initTable2() {
    table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "52vh",
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
    $(".dataTables_scrollBody").css("min-height", "42vh");

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
}

//ẩn hiện processBar
function showLoading(show) {
    $("#BackGround").css("display", show ? "block" : "none");
}

//hàm reset toàn bộ giá trị nhập liệu trên UI
function ClearValueUI() {
    $("#errorSelected").val("0");
    $("#txt_LotCode").text("-");
    $("#txt_ProductCode").text("-");
    $("#txt_ProductName").text("-");
    $("#txt_SNP").text("-");
    $("#txt_ECN").text("-");
    $("#txt_Picture").text("-");
    pictureUpload.src = undefined;
    $("#tab1_QRCodeScan").val("").focus();
}


async function callApi(url, param) {
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(param));

    const response = await fetch(url, {
        method: "POST",
        body: formUpload
    });

    return await response.json();
}



//ham load duw lieu hien tai
async function loadDataToTable1() {
    try {
        $("#BackGround").fadeIn();

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify({ LineID: $("#lineSelected").val(), Action:1 }));

        const response = await fetch("/F02/Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();

        if (!data.DataGridView || data.DataGridView.length === 0) {
            table1.clear().draw();
            ShowMessage("Không có dữ liệu!", "info");
            return;
        }

        const stats = {
            totalY: 0,
            totalN: 0,
            totalD: 0,
            uniqueSet: new Set(),
            deletedSet: new Set()
        };

        const rows = data.DataGridView.map((rowData, i) => {
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

            const checkbox = `<label><input type="checkbox" class="row-check" data-part="${rowData.ID}"><span></span></label>`;
           // const Gp12 = `<label><input type="checkbox" class="row-check" ${isChecked(rowData.GP12) ? 'checked' : ''} disabled> <span></span></label>`;

            const pictureUrl = (rowData.Picture || "").trim();
            const pictureBtn = (pictureUrl && pictureUrl.length > 15)
                ? `<button class="view-picture-btn" data-src="${pictureUrl}" style="padding:4px 8px;">View</button>`
                : "-";

            return [
                checkbox,
                i + 1,
                `<span class="preserve-space">${rowData.Barcode || ""}</span>`,
                rowData.PART_NO || "",
                rowData.PART_NM || "",
                rowData.PART_SUPL || "",
                rowData.LineName || "",
                rowData.ErrorCode ?? "",
                rowData.Description || "",
                rowData.ECN || "",
                rowData.LOC || "",     
                pictureBtn,
                rowData.CREATE_USER || "",
                FormatDateTime(rowData.CREATE_DTM)
            ];
        });

        table1.clear();
        table1.rows.add(rows);
        table1.draw();
        $('#checkAll').prop('checked', false);

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}

async function LoadHistory() {
    let lineID = $("#tab2_lineSelected").val();
    let tab2_fromDate = $("#tab2_fromDate").val();
    let tab2_toDate = $("#tab2_toDate").val();
    let tab2_errorSelected = $("#tab2_errorSelected").val();
    let tab2_Search = $("#tab2_Search").val();

    try {
        $("#BackGround").fadeIn();
        let BaseParameter = {
            SearchString: tab2_Search,
            LineID: lineID,
            FromDate: tab2_fromDate,
            ToDate: tab2_toDate,
            ErrorID: tab2_errorSelected,
            Action: 2
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

        const response = await fetch("/F02/Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();

        if (!data.DataGridView || data.DataGridView.length === 0) {
            table2.clear().draw();
            ShowMessage("Không có dữ liệu!", "info");
            return;
        }

        const stats = {
            totalY: 0,
            totalN: 0,
            totalD: 0,
            uniqueSet: new Set(),
            deletedSet: new Set()
        };

        const rows = data.DataGridView.map((rowData, i) => {
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

            const checkbox = `<label><input type="checkbox" class="row-check" data-part="${rowData.ID}"><span></span></label>`;
            //const Gp12 = `<label><input type="checkbox" class="row-check" ${isChecked(rowData.GP12) ? 'checked' : ''} disabled> <span></span></label>`;

            const pictureUrl = (rowData.Picture || "").trim();
            const pictureBtn = (pictureUrl && pictureUrl.length > 15)
                ? `<button class="view-picture-btn" data-src="${pictureUrl}" style="padding:4px 8px;">View</button>`
                : "-";


            return [
                checkbox,
                i + 1,
                `<span class="preserve-space">${rowData.Barcode || ""}</span>`,
                rowData.PART_NO || "",
                rowData.PART_NM || "",
                rowData.PART_SUPL || "",
                rowData.LineName || "",
                rowData.ErrorCode ?? "",
                rowData.Description || "",
                rowData.ECN || "",
                rowData.LOC || "",          
                pictureBtn,
                rowData.CREATE_USER || "",
                FormatDateTime(rowData.CREATE_DTM)
            ];
        });

        table2.clear();
        table2.rows.add(rows);
        table2.draw();
        $('#checkAll').prop('checked', false);

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }

}

//lấy các dong đã chọn cần xóa
function getCheckedFromDataTable(dt) {
    const ids = [];

    dt.rows().nodes().to$().find('.row-check:checked').each(function () {
        ids.push(parseInt($(this).data("part"), 10));
    });

    return ids;
}

//xuất file excel
async function ExportExxcel() {
    let lineID = $("#tab2_lineSelected").val();
    let tab2_fromDate = $("#tab2_fromDate").val();
    let tab2_toDate = $("#tab2_toDate").val();
    let tab2_errorSelected = $("#tab2_errorSelected").val();
    let tab2_Search = $("#tab2_Search").val();

    try {
        $("#BackGround").fadeIn();

        let BaseParameter = {
            SearchString: tab2_Search,
            LineID: lineID,
            FromDate: tab2_fromDate,
            ToDate: tab2_toDate,
            ErrorID: tab2_errorSelected,
            Action: 2
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

        const response = await fetch("/F02/Buttonexport_Click", {
            method: "POST",
            body: formUpload
        });

        if (!response.ok) {
            const msg = await response.text();
            ShowMessage(msg || "Lỗi khi xuất Excel", "error");
            return;
        }

        const blob = await response.blob();
        const link = document.createElement("a");
        const url = window.URL.createObjectURL(blob);
        link.href = url;
        const now = new Date();
        const timestamp = now.getFullYear() +
            ('0' + (now.getMonth() + 1)).slice(-2) +
            ('0' + now.getDate()).slice(-2) + "_" +
            ('0' + now.getHours()).slice(-2) +
            ('0' + now.getMinutes()).slice(-2) +
            ('0' + now.getSeconds()).slice(-2);

        link.download = `OQC_NG_Report_${timestamp}.xlsx`;
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.URL.revokeObjectURL(url);

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}



function isChecked(value) {
    if (value === true || value === "true" || value === 1 || value === "1") return true;
    return false;
}

// Mở modal khi click nút xem hình trên lưới
$(document).on("click", ".view-picture-btn", function () {
    const src = $(this).data("src");
    $("#modalImage").attr("src", src);
    $("#pictureModal").fadeIn();
});

// Đóng modal xem hình
$("#modalClose, #pictureModal").on("click", function (e) {
    if (e.target.id === "modalClose" || e.target.id === "pictureModal") {
        $("#pictureModal").fadeOut();
    }
});

function FormatDateTime(dt) {
    if (!dt) return "";
    const d = new Date(dt);
    return d.toLocaleString();
}

//nhấn phín Enter tại input LotCode
$('#tab1_QRCodeScan').on('keydown', async function (e) {
    if (e.key === "Enter") { // hoặc e.which === 13
        e.preventDefault();
        let ScanValue = $('#tab1_QRCodeScan').val().trim().toUpperCase();

        if (ScanValue) {
            await ScanLotCode(ScanValue);
            $('#tab1_QRCodeScan').val("").focus();
        }
    }
});

// Hàm check Scan QR code
async function ScanLotCode(Lotcode) {
    try {
        showLoading(true);

        let errorInfo = $("#errorSelected").val();

        // Kiểm tra dữ liệu đầu vào
        if (!Lotcode || Lotcode.trim() === "") {
            ShowMessage("Mã LotCode không hợp lệ!", "error");
            return;
        }

        if (!errorInfo || errorInfo.trim() === "0") {
            ShowMessage("Hãy chọn thông tin lỗi (NG Code) để tiếp tục!", "error");
            return;
        }

        let partNo = await GetPartNo(Lotcode);
        let ECN = await GetECN(Lotcode);

        // Tạo đối tượng tham số gửi lên server
        const BaseParameter = {
            LotCode: Lotcode,
            PartNo: partNo,
            PartEncno: ECN
        };

        const formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        const url = "/F02/Buttonadd_Click";
        //truyền và chờ server trả kết quả
        const response = await fetch(url, {
            method: "POST",
            body: formUpload
        });
        //chuyển đổi dữ liệu nhận được
        const data = await response.json();

        if (data?.Error) {
            ShowMessage(data.Error, "error");
        } else {
            if (data?.QRCodeText) {
                $("#txt_LotCode").text(data.QRCodeText);
            }
            if (data?.tspart?.PART_IDX) {
                $("#txt_ProductCode").text(data.tspart.PART_NO);
                $("#txt_ProductName").text(data.tspart.PART_NM);
                $("#txt_SNP").text(data.tspart.PART_SNP);
                $("#txt_ECN").text(data.tspart.BOM_GRP);
                ShowMessage("Scan thành công!", "ok");
            } else {
                $("#txt_ProductCode").text("-");
                $("#txt_ProductName").text("-");
                $("#txt_SNP").text("-");
                $("#txt_ECN").text("-");
                ShowMessage("Không tìn thấy Mã PartNo, kiểm tra lại Lotcode!", "error");
            }

        }
    } catch (err) {
        console.error(err);
        ShowMessage("Có lỗi xảy ra khi quét mã!", "error");
    } finally {
        showLoading(false);
    }
}


//lấy ra mã part no trong lotcode
async function GetPartNo(LotCode) {
    if (!LotCode || LotCode.trim() === "") {
        ShowMessage("Mã LotCode không hợp lệ!", "error");
        return "";
    }

    let partNum = "";

    if (LotCode.includes("DJG#")) {
        if (LotCode.substring(0, 2) === "A ") {
            partNum = LotCode.length >= 18 ? LotCode.substring(0, 15) : LotCode;
        }
        else
            partNum = LotCode.length >= 13 ? LotCode.substring(0, 13) : LotCode;
    } else {
        let cleanStr = LotCode.replace(/\*/g, '');

        if (cleanStr.includes("DJG") && !cleanStr.includes("[)>06VHDPPP")) {
            partNum = cleanStr.length >= 12 ? cleanStr.substring(0, 12) : cleanStr;
        }
        else {
            if (cleanStr.includes("[)>06VHDPPP")) {
                partNum = cleanStr.length >= 22 ? cleanStr.substring(11, 21) : cleanStr;
            }
            else
                // Ngược lại, cắt lấy 16 ký tự đầu và partnot 
                partNum = cleanStr.length >= 10 ? cleanStr.substring(0, 16).substring(5, 16) : cleanStr;
        }

    }

    return partNum.trim();
}

//lấy mã ECN trong lotcode
async function GetECN(LotCode) {
    if (!LotCode || LotCode.trim() === "") {
        ShowMessage("Mã LotCode không hợp lệ!", "error");
        return "";
    }
    let ECN = "";

    if (LotCode.includes("DJG#")) {
        if (LotCode.substring(0, 2) === "A ") {
            ECN = LotCode.slice(-7);
        }
        else
            ECN = LotCode.slice(-7);
    } else {
        let cleanStr = LotCode.replace(/\*/g, '');

        if (cleanStr.includes("DJG") && !cleanStr.includes("[)>06VHDPPP")) {
            ECN = cleanStr.slice(-7);
        }
        else {
            if (cleanStr.includes("[)>06VHDPPP")) {
                ECN = cleanStr.slice(-7);
            }
            else
                // Ngược lại, cắt lấy 16 ký tự đầu và partnot 
                ECN = "XX-XXXXX";
        }

    }

    return ECN.trim();
}

//hàm lưu thông tin NG vào hệ thống
async function SaveNG() {
    let LotCode = $('#txt_LotCode').text().trim().toUpperCase();
    let LineID = $('#lineSelected').val();
    let NGListID = $('#errorSelected').val();
    let ProductCode = $('#txt_ProductCode').text().trim().toUpperCase();
    let ECN = $('#txt_ECN').text().trim().toUpperCase();
    let Gp12 = $('#tab1_GP12').is(':checked');
    let pictrue = $('#txt_Picture').text().trim().toUpperCase();

    if (!LineID || LineID === "-") return ShowMessage("Chưa chọn line!", "error");
    if (!NGListID || NGListID === "0") return ShowMessage("Chưa chọn lỗi NG.", "error");
    if (!LotCode || LotCode === '-') return ShowMessage("Mã LotCode không hợp lệ!", "error");
    if (!ProductCode || ProductCode === '-') return ShowMessage("Mã PartNo không hợp lệ!", "error");

    try {
        showLoading(true);

        const BaseParameter = {
            LotCode,
            PartNo: ProductCode,
            PartEncno: ECN,
            LineID: LineID,
            GP12: Gp12,
            Picture: pictrue,
            USER_ID: GetCookieValue("UserID"),
            NGList: { ID: NGListID, ErrorCode: NGListID }
        };

        const url = "/F02/Buttonsave_Click";
        const data = await callApi(url, BaseParameter);

        if (data?.Error) ShowMessage(data.Error, "error");
        else if (data?.Message) ShowMessage(data.Message, "ok");
        else ShowMessage("Lưu thành công!", "ok");

    } catch (err) {
        ShowMessage("SaveNG error:" + err, "error");
    } finally {
        showLoading(false);
    }
}

//Xóa lỗi đã scan trên giao diện
async function DeleteDataFromTable1() {

    const idsTable1 = getCheckedFromDataTable(table1);

    if (idsTable1.length === 0) {
        ShowMessage("Vui lòng chọn ít nhất 1 dòng để xóa!", "warning");
        return;
    }

    if (!confirm(`Bạn có chắc muốn xóa ${idsTable1.length} dòng đã chọn?`)) {
        return;
    }

    try {
        showLoading(true);

        const data = await callApi("/F02/Buttondelete_Click", {
            Ids: idsTable1
        });   

        if (data.Success) {

            // ✅ Xóa trên giao diện Table1
            table1.rows(function (idx, data, node) {
                return $(node).find(".row-check").is(":checked");
            }).remove().draw(false);

            $('#checkAll_Table1').prop('checked', false);

            ShowMessage("Xóa dữ liệu thành công!", "success");
        } else {
            ShowMessage(data.Message || "Xóa thất bại!", "error");
        }

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        showLoading(false);
    }
}


// 🔹 Button find
async function Buttonfind_Click() {

    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            showLoading(true);
            await loadDataToTable1();
            showLoading(false);
            ShowMessage("Load dữ liệu thành công", "ok");

        } else if (currentTabId === 'Tag002') {
            LoadHistory();
        } else if (currentTabId === 'Tag003') {


        }
    }


}

// 🔹 Button add
async function Buttonadd_Click() {
    let ScanValue = $('#tab1_QRCodeScan').val().trim().toUpperCase();

    if (ScanValue) {
        await ScanLotCode(ScanValue);
        $('#tab1_QRCodeScan').val("").focus();
    }
}

// 🔹 Button save
async function Buttonsave_Click() {
    await UploadPicture();
    await SaveNG();
    ClearValueUI();
    loadDataToTable1();
}

// 🔹 Button delete
async function Buttondelete_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            await DeleteDataFromTable1();
        } else if (currentTabId === 'Tag002') {
            //LoadHistory();
        } else if (currentTabId === 'Tag003') {


        }
    }   
}

async function Buttoncancel_Click() {
    showLoading(true);
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    const data = await callApi("/F02/Buttoncancel_Click", formUpload);
    BaseResult = data;
    showLoading(false);
}

async function Buttoninport_Click() {
    showLoading(true);
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    const data = await callApi("/F02/Buttoninport_Click", formUpload);
    BaseResult = data;
    showLoading(false);
}

// 🔹 Button cancel
async function Buttonexport_Click() {
    ExportExxcel();
}


// 🔹 Button help
function Buttonhelp_Click() {
    OpenWindowByURL("/WMP_PLAY", 800, 460);
}

// 🔹 Button close
function Buttonclose_Click() {
    history.back();
}

// 🔹 Sự kiện khởi động
$(document).ready(function () {

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    autoCompleteSelector('#lineSelected', 'Nhập Line hoặc mô tả...');
    autoCompleteSelector('#errorSelected', 'Nhập mã lỗi hoặc mô tả...');       

    //load Table1
    initTable1();
    initTable2();
    loadDataToTable1();
});


// scrip chụp hình anh
let selectedFile = null; // Biến lưu file được chọn

const pictureUpload = document.getElementById("pictureUpload");
const uploadBtn = document.querySelector(".upload-btn");
const fileInput = document.getElementById("fileInput");
const saveBtn = document.getElementById("Buttonsave"); // Nút Save

// Click vào nút upload → mở file browser
uploadBtn.addEventListener("click", () => {
    fileInput.click();
});

// Khi chọn file → xem trước ảnh
fileInput.addEventListener("change", () => {
    const file = fileInput.files[0];
    if (!file) return;

    selectedFile = file; // Lưu file để upload sau
    const reader = new FileReader();
    reader.onload = (e) => {
        pictureUpload.src = e.target.result; // Xem trước
    };
    reader.readAsDataURL(file);
});

async function UploadPicture() {
    if (!selectedFile) {
        ShowMessage("Chưa chọn ảnh để lưu", "error");
        return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile, `OQCNG_${Date.now()}.jpg`);

    try {
        const response = await fetch("/F02/UploadPhoto", {
            method: "POST",
            body: formData
        });
        const result = await response.json();

        if (result.success) {
            ShowMessage("Lưu ảnh thành công", "ok");
            pictureUpload.src = result.path; // Cập nhật src từ server
            selectedFile = null; // Reset file đã chọn
            $("#txt_Picture").text(result.path);
            // TODO: Lưu metadata vào DB nếu cần
        } else {
            ShowMessage("Lưu ảnh thất bại", "error");
        }
    } catch (err) {
        alert("Lỗi upload: " + err.message);
    }
}

