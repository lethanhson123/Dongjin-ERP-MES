let BaseResult;
let controller = "/admin2/";
let selectedAuth = {}; // Biến toàn cục để lưu dòng được chọn

$(document).ready(function () {
    $("#group_table thead th").eq(0).text(HeaderText.No);
    $("#group_table thead th").eq(1).text(HeaderText.GroupCode);
    $("#group_table thead th").eq(2).text(HeaderText.GroupName);

    $("#group_table").DataTable({
        scrollX: true,
        scrollY: "68vh",
        scrollCollapse: true,
        paging: false,
        autoWidth: false,
        ordering: false,
        searching: false,
        fixedHeader: false,
        language: {
            emptyTable: "No data available"
        }
    });
});

$(document).on("click", ".btn-edit", function () {
    const row = $(this).closest("tr");
    // Hiển thị modal
    // $("#createModal").fadeIn();
    showEditModal(row ,function (result) {
        if (result) {
            UpdatePermissionGroup();
        } else {
            $("#BackGround").css("display", "none");
        }
    });
});

$(document).on("click", "#group_table tbody tr", function () {
    const cells = $(this).find("td");

    if (cells.length > 0) {
        selectedAuth = {
            AUTH_IDX: $(this).data("auth-idx"),
            AUTH_ID: cells.eq(1).text().trim(), // Sau khi ẩn IDX, AUTH_ID là .eq(1)
            AUTH_NM: cells.eq(2).text().trim()
        };

        Buttonfind_Click();
    }
});

document.getElementById("group_menu_selector").addEventListener("change", function () {
    Buttonfind_Click();
});

function showCreateModal(callback) {
    $("#group_InputCode").val("");
    $("#group_InputName").val("");
    $("#Edit_IDX").text("");
    $("#confirmTitle").text(HeaderText.Create);
    var modalElem = document.getElementById('createModal');
    var instance = M.Modal.getInstance(modalElem);
    if (!instance) {
        instance = M.Modal.init(modalElem); // Khởi tạo nếu chưa có instance
    }
    instance.open();


    $('#btnSave').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#btnCancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}

function showEditModal(data,callback) {
    // Lấy dữ liệu từ dòng hiện tại
    const authId = data.find("td:eq(1)").text().trim();
    const authName = data.find("td:eq(2)").text().trim();
    const authIdx = data.data("auth-idx"); // <-- đúng cách lấy AUTH_IDX

    // Đổ dữ liệu vào modal
    $("#group_InputCode").val(authId);
    $("#group_InputName").val(authName);
    $("#Edit_IDX").text(authIdx);

    // Cập nhật tiêu đề
    $("#confirmTitle").text(HeaderText.Edit);

    var modalElem = document.getElementById('createModal');
    var instance = M.Modal.getInstance(modalElem);
    if (!instance) {
        instance = M.Modal.init(modalElem); // Khởi tạo nếu chưa có instance
    }
    instance.open();


    $('#btnSave').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#btnCancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}

async function UpdatePermissionGroup() {
    let Edit_IDX = $("#Edit_IDX").text();
    let groupCode = $("#group_InputCode").val().trim();
    let groupName = $("#group_InputName").val().trim();

    if (!groupCode || !groupName) {
        ShowMessage(MessageText.Empty, "error");
        return;
    }

    const BaseParameter = {
        ID: Edit_IDX,
        GroupCode: groupCode,
        GroupName: groupName,
        USER_ID: GetCookieValue("UserID"),
        Code: Edit_IDX ? "edit" : "add"
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    $("#BackGround").fadeIn();

    try {
        const response = await fetch(controller + "Buttonadd_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();

        if (data.tsauthTranfer && data.tsauthTranfer.AUTH_ID) {
            const table = $("#group_table").DataTable();
            const authIDX = data.tsauthTranfer.AUTH_IDX;

            if (Edit_IDX) {
                // Cập nhật dòng đã tồn tại (edit)
                const rowToUpdate = $(`#group_table tbody tr[data-auth-idx="${authIDX}"]`);
                if (rowToUpdate.length > 0) {
                    const rowIndex = table.row(rowToUpdate).index();
                    table.row(rowIndex).data([
                        rowIndex + 1,
                        data.tsauthTranfer.AUTH_ID,
                        data.tsauthTranfer.AUTH_NM,
                        `<button class="btn btn-small btn-edit btn-secondary" data-index="${rowIndex}" style="padding: 4px; height:20px;width:20px; margin:2px;" title="Edit">
                            <i class="material-icons">edit</i>
                        </button>`
                    ]).draw(false);

                    ShowMessage(MessageText.Success, "ok");
                } else {
                    ShowMessage("Không tìm thấy dòng cần cập nhật", "error");
                }
            } else {
                // Thêm mới dòng (add)
                const newRow = table.row.add([
                    table.rows().count() + 1,
                    data.tsauthTranfer.AUTH_ID,
                    data.tsauthTranfer.AUTH_NM,
                    `<button class="btn btn-small btn-edit btn-secondary" data-index="${rowIndex}" style="padding: 4px; height:20px;width:20px; margin:2px;" title="Edit">
                            <i class="material-icons">edit</i>
                     </button>`
                ]).draw(false).node();

                $(newRow).attr("data-auth-idx", authIDX);
                ShowMessage(MessageText.Success, "ok");
            }

        } else {
            ShowMessage(MessageText.Empty, "error");
        }

    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}


function DeleteGroup() {
    $("#BackGround").css("display", "block");

    if (!selectedAuth.AUTH_IDX) {
        ShowMessage(MessageText.NotSelectOnGrid, "error");
        return;
    }

    const BaseParameter = {
        ID: selectedAuth.AUTH_IDX,
        GroupCode: selectedAuth.AUTH_ID,
        GroupName: selectedAuth.AUTH_NM,
        USER_ID: GetCookieValue("UserID"),
        Code: "delete"
    };
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
            deleteGroupInRow(data.tsauthTranfer.AUTH_IDX);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}

function deleteGroupInRow(authIdx) {
    const table = $("#group_table").DataTable();

    let found = false;

    table.rows().every(function () {
        const rowNode = this.node();
        const rowAuthIdx = $(rowNode).attr("data-auth-idx");

        if (rowAuthIdx === String(authIdx)) {
            this.remove();
            table.draw(false);
            ShowMessage(MessageText.Success, "ok");
            found = true;
            return false;
        }
    });

    if (!found) {
        ShowMessage(MessageText.NotSelectOnGrid, "error");
    }
}

function showConfirmModal(message, callback) {
    // Gán nội dung HTML vào phần message
    $("#confirmMessage").html(message);

    // Mở modal
    const modalElem = document.getElementById("confirmModal");
    const instance = M.Modal.getInstance(modalElem) || M.Modal.init(modalElem, { dismissible: false });
    instance.open();

    // Xử lý nút xác nhận
    $("#btn_confirmYes").off("click").on("click", function () {
        instance.close();
        if (typeof callback === "function") callback(true);
    });

    // Xử lý nút hủy
    $("#btn_confirmCancel").off("click").on("click", function () {
        instance.close();
        if (typeof callback === "function") callback(false);
    });
}


$("#btnCancel").on("click", function () {
    $("#createModal").fadeOut();
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
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");

    if (!selectedAuth.AUTH_IDX) {
        ShowMessage(MessageText.NotSelectOnGrid, "error");
        return;
    }

    const BaseParameter = {
        tsauthTranfer: {
            AUTH_IDX: parseInt(selectedAuth.AUTH_IDX),
            AUTH_ID: selectedAuth.AUTH_ID,
            AUTH_NM: selectedAuth.AUTH_NM
        },
        ComboBox1: document.getElementById("group_menu_selector")?.value || "ALL"
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    const url = controller + "Buttonfind_Click"; // hoặc controller + "/DGV_DATA_CHG" nếu bạn map rõ

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");
            if (data && data.ListSuperResultTranfer) {
                renderMenuTable(data.ListSuperResultTranfer);
            }
        })
        .catch(error => {
            $("#BackGround").css("display", "none");
            ShowMessage("error: " + error, "error");
        });
}

function renderMenuTable(dataList) {
    const tableId = "#menu_table";

    // Clear nếu đã khởi tạo DataTable
    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().clear().destroy();
    }

    // Tạo bảng mới với dữ liệu
    const dt = $(tableId).DataTable({
        data: dataList,
        scrollX: true,
        scrollY: "68vh",
        scrollCollapse: true,
        paging: false,
        autoWidth: false,
        ordering: false,
        searching: false,
        fixedHeader: true,
        language: {
            emptyTable: "No data available"
        },
        columns: [
            { data: null, render: (data, type, row, meta) => meta.row + 1 },  // No
            { data: "SCRN_PATH" },
            { data: "MENU_LVL" },
            { data: "MENU_NM_EN" },
            { data: "MENU_NM_HAN" },

            { data: "MENU_AUTH_YN", render: d => checkboxHTML("menu_checkbox", d) },
            { data: "IQ_AUTH_YN", render: d => checkboxHTML("search_checkbox", d) },
            { data: "RGST_AUTH_YN", render: d => checkboxHTML("add_checkbox", d) },
            { data: "MDFY_AUTH_YN", render: d => checkboxHTML("save_checkbox", d) },
            { data: "DEL_AUTH_YN", render: d => checkboxHTML("delete_checkbox", d) },
            { data: "CAN_AUTH_YN", render: d => checkboxHTML("cancel_checkbox", d) },
            { data: "EXCL_AUTH_YN", render: d => checkboxHTML("import_checkbox", d) },
            { data: "DNLD_AUTH_YN", render: d => checkboxHTML("export_checkbox", d) },
            { data: "PRNT_AUTH_YN", render: d => checkboxHTML("print_checkbox", d) },
            { data: "ETC1_AUTH_YN", render: d => checkboxHTML("ETC1_checkbox", d) },
            { data: "ETC2_AUTH_YN", render: d => checkboxHTML("ETC2_checkbox", d) },
            { data: "ETC3_AUTH_YN", render: d => checkboxHTML("ETC3_checkbox", d) },
        ],
        rowCallback: function (row, data) {
            // Gắn lại data-* attributes cho từng row
            $(row).attr("data-menu-idx", data.MENU_IDX || "");
            $(row).attr("data-menu-auth-idx", data.MENU_AUTH_IDX || "");
            $(row).attr("data-menu-cd", data.MENU_CD || "");

            // Tô màu nếu MENU_LVL = 1
            if (data.MENU_LVL === "1" || data.MENU_LVL === 1) {
                $(row).css("background-color", "lightgreen");
            }
        }
    });
}

// Hàm tạo checkbox HTML
function checkboxHTML(className, value) {
    const checked = value === "True" ? "checked" : "";
    return `<label><input type="checkbox" class="${className}" ${checked}><span></span></label>`;
}

function Buttonadd_Click() {
    showCreateModal(function (result) {
        if (result) {
            UpdatePermissionGroup();
        } else {
            $("#BackGround").css("display", "none");
        }
    });
}
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");

    const menuList = [];
    $("#menu_table tbody tr").each(function () {
        const cells = $(this).find("td");

        const row = {
            SCRN_PATH: cells.eq(1).text().trim(),
            MENU_LVL: cells.eq(2).text().trim(),
            MENU_NM_EN: cells.eq(3).text().trim(),
            MENU_NM_HAN: cells.eq(4).text().trim(),

            MENU_AUTH_YN: $(cells.eq(5)).find(".menu_checkbox").prop("checked") ? "True" : "False",
            IQ_AUTH_YN: $(cells.eq(6)).find(".search_checkbox").prop("checked") ? "True" : "False",
            RGST_AUTH_YN: $(cells.eq(7)).find(".add_checkbox").prop("checked") ? "True" : "False",
            MDFY_AUTH_YN: $(cells.eq(8)).find(".save_checkbox").prop("checked") ? "True" : "False",
            DEL_AUTH_YN: $(cells.eq(9)).find(".delete_checkbox").prop("checked") ? "True" : "False",
            CAN_AUTH_YN: $(cells.eq(10)).find(".cancel_checkbox").prop("checked") ? "True" : "False",
            EXCL_AUTH_YN: $(cells.eq(11)).find(".import_checkbox").prop("checked") ? "True" : "False",
            DNLD_AUTH_YN: $(cells.eq(12)).find(".export_checkbox").prop("checked") ? "True" : "False",
            PRNT_AUTH_YN: $(cells.eq(13)).find(".print_checkbox").prop("checked") ? "True" : "False",
            ETC1_AUTH_YN: $(cells.eq(14)).find(".ETC1_checkbox").prop("checked") ? "True" : "False",
            ETC2_AUTH_YN: $(cells.eq(15)).find(".ETC2_checkbox").prop("checked") ? "True" : "False",
            ETC3_AUTH_YN: $(cells.eq(16)).find(".ETC3_checkbox").prop("checked") ? "True" : "False",

            // Lấy từ data-* attribute
            MENU_IDX: $(this).data("menu-idx"),
            MENU_AUTH_IDX: $(this).data("menu-auth-idx"),
            MENU_CD: $(this).data("menu-cd")
        };

        menuList.push(row);
    });

    if (menuList.length <= 0) {
        ShowMessage("Không có dòng nào để lưu", "error");
        $("#BackGround").css("display", "none");
        return;
    }

    const BaseParameter = {
        DataGridView: menuList,
        USER_ID: GetCookieValue("UserID")
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    const url = controller + "Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => {
        return response.json();
    }).then(data => {
        $("#BackGround").css("display", "none");
        ShowMessage("Lưu dữ liệu thành công", "ok");
    }).catch(error => {
        $("#BackGround").css("display", "none");
        ShowMessage("Lỗi khi lưu dữ liệu: " + (error.message || error), "error");
    });
}


function Buttondelete_Click() {
    if (!selectedAuth.AUTH_IDX) {
        ShowMessage(MessageText.NotSelectOnGrid, "error");
        return;
    }

    const message = `${HeaderText.GroupCode}: ${selectedAuth.AUTH_ID}<br>${HeaderText.GroupName}: ${selectedAuth.AUTH_NM}`;

    showConfirmModal(message, function (confirmed) {
        if (confirmed) {
            // Thực hiện hành động xoá
            DeleteGroup();
        }
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttoncancel_Click";

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
    let url = controller + "Buttoninport_Click";

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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonexport_Click";

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
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonprint_Click";

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

document.getElementById("check_all_menu").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .menu_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});


document.getElementById("check_all_search").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .search_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_add").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .add_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_save").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .save_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_delete").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .delete_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});
document.getElementById("check_all_cancel").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .cancel_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_import").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .import_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_export").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .export_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});

document.getElementById("check_all_print").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .print_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});
document.getElementById("check_all_ETC1").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .ETC1_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});
document.getElementById("check_all_ETC2").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .ETC2_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});
document.getElementById("check_all_ETC3").addEventListener("change", function () {
    const checked = this.checked;
    document.querySelectorAll('#menu_table_view .ETC3_checkbox').forEach(cb => {
        cb.checked = checked;
    });
});
