let BaseResult = {};

$(document).ready(function () {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    const todayStr = `${yyyy}-${mm}-${dd}`;

    $("#FilterFromDate").val(todayStr);
    $("#FilterToDate").val(todayStr);
    LoadData();

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);
    $("#SaveEditBtn").click(SaveEdit_Click);

    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });
    $("#FilterFromDate").on("change", Buttonfind_Click);
    $("#FilterToDate").on("change", Buttonfind_Click);
    $("#FilterMachine").on("change", Buttonfind_Click);
    $('.modal').modal();
});

function Buttonadd_Click() {
    // Reset form
    $("#EditID").val("");
    $("#EditMachine").prop("disabled", false).val("");

    // Set thời gian mặc định = hiện tại
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const nowStr = `${year}-${month}-${day}T${hours}:${minutes}`;

    $("#EditStartDate").val(nowStr).prop("disabled", false);
    $("#EditEndDate").val(nowStr).prop("disabled", false);

    $("#EditCurrentStatus").val("");
    $("#EditSolution").val("");
    $("#EditSparePartsUsed").val("");
    $("#EditMaintenedBy").val("");
    $("#EditNotes").val("");

    $("#EditModal h5").text("Thêm mới lịch sử bảo trì");
    $("#EditModal").modal("open");
}


function SaveEdit_Click() {
    let id = $("#EditID").val();
    let machine = $("#EditMachine").val().trim();
    let currentStatus = $("#EditCurrentStatus").val();
    let solution = $("#EditSolution").val();
    let maintenedBy = $("#EditMaintenedBy").val();
    let startDate = $("#EditStartDate").val();
    let endDate = $("#EditEndDate").val();

    // Validation
    if (!id && !machine) {
        alert("Vui lòng nhập Mã máy!");
        return;
    }
    if (!currentStatus) {
        alert("Vui lòng chọn Nội dung lỗi!");
        return;
    }
    if (!solution) {
        alert("Vui lòng chọn Biện pháp!");
        return;
    }
    if (!maintenedBy) {
        alert("Vui lòng chọn Người sửa!");
        return;
    }
    if (!id && (!startDate || !endDate)) {
        alert("Vui lòng chọn thời gian!");
        return;
    }
    if (!id && new Date(startDate) > new Date(endDate)) {
        alert("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc!");
        return;
    }

    ShowLoading(true);
    let formData = new FormData();
    let endpoint = id ? "/E04/Buttonsave_Click" : "/E04/Buttonadd_Click";
    let baseParam = {
        USER_ID: GetCookieValue("UserID")
    };

    if (id) {
        // Edit
        baseParam.ID = id;
        baseParam.CurrentStatus = currentStatus;
        baseParam.Solution = solution;
        baseParam.SparePartsUsed = $("#EditSparePartsUsed").val().trim();
        baseParam.MaintenedBy = maintenedBy;
        baseParam.Notes = $("#EditNotes").val().trim();
    } else {
        // Add - Thêm StartDate và EndDate
        baseParam.MaintenanceHistory = {
            ToolShopSubCode: machine,
            MaintenanceType: "M",
            CurrentStatus: currentStatus,
            Solution: solution,
            SparePartsUsed: $("#EditSparePartsUsed").val().trim(),
            MaintenedBy: maintenedBy,
            Notes: $("#EditNotes").val().trim(),
            StartDate: startDate,
            EndDate: endDate
        };
    }

    formData.append('BaseParameter', JSON.stringify(baseParam));

    fetch(endpoint, { method: "POST", body: formData })
        .then(r => r.json())
        .then(res => {
            if (res.Success) {
                alert(res.Message || (id ? "Cập nhật thành công" : "Thêm mới thành công"));
                $("#EditModal").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error:", err);
            alert("Có lỗi xảy ra: " + err.message);
        })
        .finally(() => ShowLoading(false));
}

function ShowLoading(show = true) {
    $("#BackGround").css("display", show ? "block" : "none");
}

function LoadData() {
    ShowLoading(true);
    fetch('/E04/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            BaseResult = res;
            LoadMachineList(res.MaintenanceHistoryList || []);
            Buttonfind_Click();
        })
        .catch(err => {
            console.error("Error:", err);
            alert("Có lỗi xảy ra: " + err.message);
        })
        .finally(() => ShowLoading(false));
}

function LoadMachineList(data) {
    let machines = [...new Set(data.map(item => item.ToolShopSubCode).filter(m => m))];
    machines.sort();

    let $select = $("#FilterMachine");
    $select.find("option:not(:first)").remove();

    machines.forEach(machine => {
        $select.append(`<option value="${machine}">${machine}</option>`);
    });
}

function Buttonfind_Click() {
    ShowLoading(true);
    let search = $("#SearchBox").val() || "";
    let machine = $("#FilterMachine").val() || "";
    let fromDate = $("#FilterFromDate").val() || "";
    let toDate = $("#FilterToDate").val() || "";

    let formData = new FormData();
    formData.append('BaseParameter', JSON.stringify({
        SearchString: search,
        ToolShopSubCode: machine,
        FilterFromDate: fromDate,
        FilterToDate: toDate
    }));

    fetch("/E04/Buttonfind_Click", { method: "POST", body: formData })
        .then(r => r.json())
        .then(res => {
            BaseResult = res;
            RenderTable(res.MaintenanceHistoryList || []);
        })
        .catch(err => {
            console.error("Error:", err);
            alert("Có lỗi xảy ra: " + err.message);
        })
        .finally(() => ShowLoading(false));
}

function RenderTable(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="14" class="center-align">Không có dữ liệu</td></tr>');
        $("#TotalRecords").text("0");
        return;
    }

    data.forEach(item => {
        let duration = "";
        if (item.StartDate && item.EndDate) {
            const start = new Date(item.StartDate.replace('Z', ''));
            const end = new Date(item.EndDate.replace('Z', ''));
            duration = Math.round((end - start) / (1000 * 60));
        }

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td class="center-align">
                    <button class="btn-small orange darken-1 btn-edit" data-id="${item.ID}" title="Chỉnh sửa">
                        <i class="material-icons">edit</i>
                    </button>
                </td>
                <td>${item.ID ?? ""}</td>
                <td>${item.ToolShopSubCode ?? ""}</td>
                <td>${item.MaintenanceType ?? ""}</td>
                <td>${item.CurrentStatus ?? ""}</td>
                <td>${item.Solution ?? ""}</td>
                <td>${item.SparePartsUsed ?? ""}</td>
                <td>${item.MaintenedBy ?? ""}</td>
                <td>${formatDateTime(item.StartDate)}</td>
                <td>${formatDateTime(item.EndDate)}</td>
                <td class="right-align">${duration ? duration + ' phút' : ''}</td>
                <td>${item.Notes ?? ""}</td>
                <td>${formatDateTime(item.UpdateDate)}</td>
                <td>${item.UpdateUser ?? ""}</td>
            </tr>
        `);
    });

    $("#TotalRecords").text(data.length);

    $("#DataGridView1 tbody tr").on("click", function (e) {
        if ($(e.target).closest('.btn-edit').length) return;
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });

    $(".btn-edit").on("click", function (e) {
        e.stopPropagation();
        let id = $(this).data("id");
        OpenEditModal(id);
    });
}

function OpenEditModal(id) {
    let item = BaseResult.MaintenanceHistoryList.find(x => x.ID == id);
    if (!item) {
        alert("Không tìm thấy dữ liệu!");
        return;
    }

    $("#EditModal h5").text("Chỉnh sửa chi tiết bảo trì");

    $("#EditID").val(item.ID);
    $("#EditMachine").prop("disabled", true).val(item.ToolShopSubCode || "");

    // Hiển thị thời gian nhưng không cho sửa
    if (item.StartDate) {
        const start = new Date(item.StartDate.replace('Z', ''));
        const startStr = formatDateTimeLocal(start);
        $("#EditStartDate").val(startStr).prop("disabled", true);
    }
    if (item.EndDate) {
        const end = new Date(item.EndDate.replace('Z', ''));
        const endStr = formatDateTimeLocal(end);
        $("#EditEndDate").val(endStr).prop("disabled", true);
    }

    $("#EditCurrentStatus").val(item.CurrentStatus || "");
    $("#EditSolution").val(item.Solution || "");
    $("#EditSparePartsUsed").val(item.SparePartsUsed || "");
    $("#EditMaintenedBy").val(item.MaintenedBy || "");
    $("#EditNotes").val(item.Notes || "");

    $("#EditModal").modal("open");
}
function formatDateTimeLocal(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
}
function formatDateTime(dateStr) {
    if (!dateStr) return "";
    const date = new Date(dateStr.replace('Z', ''));
    if (isNaN(date.getTime())) return "";

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}`;
}

function formatCurrency(value) {
    if (!value) return "";
    return parseFloat(value).toLocaleString('vi-VN');
}

function Buttondelete_Click() {
    let id = $("#DataGridView1 tr.selected").data("id");
    if (!id) {
        alert("Vui lòng chọn dòng muốn xóa!");
        return;
    }

    if (!confirm("Bạn chắc chắn muốn xóa?")) return;

    ShowLoading(true);
    let formData = new FormData();
    formData.append('BaseParameter', JSON.stringify({ ID: id }));

    fetch("/E04/Buttondelete_Click", { method: "POST", body: formData })
        .then(r => r.json())
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Xóa thành công");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error:", err);
            alert("Có lỗi xảy ra: " + err.message);
        })
        .finally(() => ShowLoading(false));
}

function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1', 'MaintenanceHistory.xls', 'MaintenanceHistory');
}

function Buttonprint_Click() {
    window.print();
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}