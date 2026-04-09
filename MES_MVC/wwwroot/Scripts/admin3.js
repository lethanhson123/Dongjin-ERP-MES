let BaseResult;
let controller = "/admin3/";

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
    let txt_employeeID = $("#txt_employeeID").val();
    let txt_employeeName = $("#txt_employeeName").val();
    let txt_department = $("#txt_department").val();
    let using_selector = $("#using_selector").val();

    let BaseParameter = new Object();
    BaseParameter = {
        USER_ID: txt_employeeID,
        USER_NM: txt_employeeName,
        GroupName: txt_department,
        IsUsing: using_selector
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data && data.ListtsuserTranfer) {
                renderUserTable(data.ListtsuserTranfer);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonadd_Click() {
    RecoverySelectedUsers();
}
function Buttonsave_Click() {
   
}
function Buttondelete_Click() {
    DeleteSelectedUsers();
}
function Buttoncancel_Click() {
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = controller + "Buttoncancel_Click";

    //fetch(url, {
    //    method: "POST",
    //    body: formUpload,
    //    headers: {
    //    }
    //}).then((response) => {
    //    response.json().then((data) => {
    //        $("#BackGround").css("display", "none");
    //    }).catch((err) => {
    //        $("#BackGround").css("display", "none");
    //    })
    //});
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


$(document).ready(function () {
    const table = $("#list_User").DataTable({
        scrollX: true,
        scrollY: "65vh",
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

    Buttonfind_Click();

    $('.modal').modal();

    $("#using_selector").on("change", function () {
        //const selectedValue = $(this).val(); // 'Y' hoặc 'N'
        //console.log("Giá trị được chọn:", selectedValue);

        //// Gọi hàm lọc dữ liệu hoặc xử lý gì đó tại đây
        //// Ví dụ: Buttonfind_Click(); hoặc filterData(selectedValue);
        Buttonfind_Click();
    });

    $("#txt_employeeID, #txt_employeeName, #txt_department").on("keydown", function (e) {
        if (e.key === "Enter" || e.keyCode === 13) {
            e.preventDefault(); // Ngăn form bị submit nếu có
            Buttonfind_Click(); // Gọi hàm tìm kiếm của bạn
        }
    });
       
});


function renderUserTable(dataList) {
    const tableId = "#list_User";

    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().clear().destroy();
    }

    $(tableId).DataTable({
        data: dataList,
        scrollX: true,
        scrollY: "65vh",
        scrollCollapse: true,
        paging: false,
        autoWidth: false,
        ordering: false,
        searching: false,
        fixedHeader: false,
        language: {
            emptyTable: "No data available"
        },
        columns: [
            { data: null, render: (data, type, row, meta) => meta.row + 1 },
            {
                data: "USER_IDX",
                render: (data) => `
                    <button class="btn btn-edit btn-small btn-secondary" data-user-idx="${data}" style="padding: 4px; height:20px;width:20px; margin:2px;" title="Edit">
                        <i class="material-icons">edit</i>
                    </button>`
            },
            { data: null, render: () => `<label><input type="checkbox" class="checkbox_item"><span></span></label>` },
            { data: "USER_ID" },
            { data: "USER_NM" },
            { data: "Dept" },
            { data: "Note" },
            { data: "AUTH_NM" },
            { data: "DESC_YN" },
            {
                data: "CREATE_DTM",
                render: (data) => FormatDateTime(data)
            },
            { data: "CREATE_USER" },
            {
                data: "UPDATE_DTM",
                render: (data) => FormatDateTime(data)
            },
            { data: "UPDATE_USER" }
        ],
        createdRow: function (row, data, dataIndex) {
            $(row).attr("data-auth-idx", data.AUTH_IDX ?? "");
            $(row).attr("data-user-idx", data.USER_IDX ?? "");
        }
    });

    // Gắn sự kiện chọn tất cả sau khi DataTable khởi tạo
    $(document).off('change', '#check_all').on('change', '#check_all', function () {
        const isChecked = $(this).is(":checked");
        $(".checkbox_item").prop("checked", isChecked);
    });
}


$(document).on("click", ".btn-edit", function () {
    const row = $(this).closest("tr");
    showEditModal(row, function (result) {
        if (result) {
            UpdateUser();
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

function showEditModal(data, callback) {
    // Lấy dữ liệu từ dòng hiện tại
    const userId = data.find("td:eq(3)").text().trim();
    const userName = data.find("td:eq(4)").text().trim();
    const dept = data.find("td:eq(5)").text().trim();
    const note = data.find("td:eq(6)").text().trim();
    const authidx = data.data("auth-idx"); // <-- đúng cách lấy user-idx
    const userIdx = data.data("user-idx"); // <-- đúng cách lấy user-idx

    // Đổ dữ liệu vào modal
    $("#txt_user_ID").val(userId);
    $("#txt_user_NM").val(userName);
    $("#txt_Dept").val(dept);
    $("#txt_Note").val(note);
    $("#Edit_IDX").val(userIdx);
    $("#txt_Password").val("");
    // Gán giá trị selector
    $("#selector_PermissionGroup").val(String(authidx));



    // Cập nhật tiêu đề
    $("#confirmTitle").text(HeaderText.Edit);

    var modalElem = document.getElementById('editModal');
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

function UpdateUser() {
    const Edit_IDX = $("#Edit_IDX").val().trim();
    const userId = $("#txt_user_ID").val().trim();
    const userName = $("#txt_user_NM").val().trim();
    const note = $("#txt_Note").val().trim();
    const password = $("#txt_Password").val().trim(); // input
    const authidx = $("#selector_PermissionGroup").val();
    const updateUser = GetCookieValue("UserID");

    // Kiểm tra bắt buộc
    if (!Edit_IDX || !authidx || !updateUser) {
        ShowMessage("Thông tin không đầy đủ để cập nhật.", "error");
        return;
    }

    // Khai báo đối tượng userinfor
    const userinfor = {
        AUTH_IDX: parseInt(authidx),
        USER_IDX: parseInt(Edit_IDX),
        USER_ID: userId,
        USER_NM: userName,
        Note: note,
        PW: password,
        UPDATE_USER: updateUser
    };

    const BaseParameter = {
        tsuserTranfer: userinfor
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    const url = controller + "Buttonsave_Click";

    $("#BackGround").css("display", "block");

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");

            if (data?.ErrorNumber === 0) {
                ShowMessage("Cập nhật thành công.", "success");
                //load lại thông tin sau khi cập nhật
                Buttonfind_Click();
            } else {
                ShowMessage("Lỗi: " + (data?.Error || "Không rõ lỗi"), "error");
            }
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            ShowMessage("Lỗi hệ thống: " + err.message, "error");
        });
}

function DeleteSelectedUsers() {
    const selectedRows = $(".checkbox_item:checked").closest("tr");
    if (selectedRows.length === 0) {
        ShowMessage("Vui lòng chọn ít nhất một người dùng để xóa.", "warning");
        return;
    }

    const userList = [];

    selectedRows.each(function () {
        const row = $(this);
        const userIdx = row.data("user-idx");
        const userId = row.find("td:eq(3)").text().trim();
        const userName = row.find("td:eq(4)").text().trim();

        userList.push({
            USER_IDX: userIdx,
            USER_ID: userId,
            USER_NM: userName,
            UPDATE_USER: GetCookieValue("UserID")
        });
    });

    // Hiện modal xác nhận xóa
    showConfirmDeleteModal(userList, () => {
        const baseParameter = {
            ListtsuserTranfer: userList
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(baseParameter));

        $("#BackGround").css("display", "block");

        fetch(controller + "Buttondelete_Click", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                $("#BackGround").css("display", "none");

                if (data?.ErrorNumber === 0) {
                    ShowMessage(MessageText.Success, "ok");
                    Buttonfind_Click(); // Làm mới bảng
                } else {
                    ShowMessage("Lỗi: " + data.Error, "error");
                }
            })
            .catch(error => {
                $("#BackGround").css("display", "none");
                ShowMessage("Lỗi hệ thống: " + error, "error");
            });
    });
}

function RecoverySelectedUsers() {
    const selectedRows = $(".checkbox_item:checked").closest("tr");
    if (selectedRows.length === 0) {
        ShowMessage(MessageText.Empty, "warning");
        return;
    }

    const userList = [];

    selectedRows.each(function () {
        const row = $(this);
        const userIdx = row.data("user-idx");
        const userId = row.find("td:eq(3)").text().trim();
        const userName = row.find("td:eq(4)").text().trim();

        userList.push({
            USER_IDX: userIdx,
            USER_ID: userId,
            USER_NM: userName,
            UPDATE_USER: GetCookieValue("UserID")
        });
    });

    // Hiện modal xác nhận xóa
    showConfirmRecoveryModal(userList, () => {
        const baseParameter = {
            ListtsuserTranfer: userList
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(baseParameter));

        $("#BackGround").css("display", "block");

        fetch(controller + "Buttonadd_Click", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                $("#BackGround").css("display", "none");

                if (data?.ErrorNumber === 0) {
                    ShowMessage(MessageText.Success, "ok");
                    Buttonfind_Click(); // Làm mới bảng
                } else {
                    ShowMessage("Lỗi: " + data.Error, "error");
                }
            })
            .catch(error => {
                $("#BackGround").css("display", "none");
                ShowMessage("Lỗi hệ thống: " + error, "error");
            });
    });
}

function showConfirmDeleteModal(userList, onConfirm) {
    const modalElem = document.getElementById("confirmModal");
    let instance = M.Modal.getInstance(modalElem);
    if (!instance) {
        instance = M.Modal.init(modalElem);
    }

    // Tạo danh sách tên người dùng để hiển thị
    const message = userList.map(u => `• [${u.USER_ID}] ${u.USER_NM}`).join("<br>");
    $("#confirmMessage").html(MessageText.DeleteData + "? <br><br>" + message);

    instance.open();

    $('#btn_confirmYes').off('click').on('click', function () {
        instance.close();
        if (typeof onConfirm === "function") onConfirm();
    });

    $('#btn_confirmCancel').off('click').on('click', function () {
        instance.close();
    });
}

function showConfirmRecoveryModal(userList, onConfirm) {
    const modalElem = document.getElementById("confirmModal");
    let instance = M.Modal.getInstance(modalElem);
    if (!instance) {
        instance = M.Modal.init(modalElem);
    }

    // Tạo danh sách tên người dùng để hiển thị
    const message = userList.map(u => `• [${u.USER_ID}] ${u.USER_NM}`).join("<br>");
    $("#confirmMessage").html(MessageText.RecoveryData + " ?<br><br>" + message);

    instance.open();

    $('#btn_confirmYes').off('click').on('click', function () {
        instance.close();
        if (typeof onConfirm === "function") onConfirm();
    });

    $('#btn_confirmCancel').off('click').on('click', function () {
        instance.close();
    });
}


$("#ButtonPassRandom").on("click", function () {
    const newPass = generateRandomPassword();
    $("#txt_Password").val(newPass);
});

function generateRandomPassword(length = 10) {
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()';
    let pass = '';
    for (let i = 0; i < length; i++) {
        pass += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return pass;
}

