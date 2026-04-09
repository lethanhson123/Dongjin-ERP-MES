let BaseResult;
let controller = "/admin4/";


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
    LoadMenuDetail();
}
function Buttonadd_Click() {
    showEditModal(null, function (result) {
        if (result) {
            AddNewMenu();
        } else {
            $("#BackGround").css("display", "none");
        }
    });
}
function Buttonsave_Click() {
   
}
function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
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
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
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


$(document).ready(function () {
    const table = $("#Menu_Table").DataTable({
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

    LoadMenuDetail();       

    // Gọi lần đầu
    toggleFields();

    // Gán sự kiện khi đổi radio
    $('input[name="STATUS"]').on('change', function () {
        toggleFields();
    });

    // Nếu selector thay đổi trong khi chọn RadioButton2, thì cập nhật txt_GroupMenuCode theo
    $('#selector_menuGroup').on('change', function () {
        if ($('#RadioButton2').is(':checked')) {
            $('#txt_GroupMenuCode').val($(this).val());
        }
    });
});

function toggleFields() {
    const isPrimary = $('#RadioButton1').is(':checked');
    const $groupInput = $('#txt_GroupMenuCode');
    const $groupSelector = $('#selector_menuGroup');

    if (isPrimary) {
        // Nếu chọn Primary Menu
        $groupSelector.prop('disabled', true).css('background-color', '#e0e0e0');
        $groupInput.prop('readonly', false).css('background-color', 'white').val('');
    } else {
        // Nếu chọn Secondary Menu
        $groupSelector.prop('disabled', false).css('background-color', 'burlywood');
        const selectedVal = $groupSelector.val();
        $groupInput.prop('readonly', true).css('background-color', '#f2f2f2').val(selectedVal);
    }
}

function LoadMenuDetail() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
       Code: "Load Detail",
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
            if (data && data.ListtsmenuTranfer) {
                renderUserTable(data.ListtsmenuTranfer);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function renderUserTable(dataList) {
    const tableId = "#Menu_Table";

    if ($.fn.DataTable.isDataTable(tableId)) {
        $(tableId).DataTable().clear().destroy();
    }

    $(tableId).DataTable({
        data: dataList,
        scrollX: true,
        scrollY: "72vh",
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
            
            { data: null, render: () => `<label><input type="checkbox" class="checkbox_item"><span></span></label>` },
            { data: "MENU_NM_EN" },
            { data: "MENU_LVL" },
            { data: "MENU_NM_HAN" },
            { data: "GroupCode" },
            { data: "SCRN_PATH" },      
            {
                data: "CREATE_DTM",
                render: (data) => FormatDateTime(data)
            },
            { data: "CREATE_USER" },            
        ],
        createdRow: function (row, data, dataIndex) {
            $(row).attr("data-menu_idx", data.MENU_IDX ?? "");  

            //to màu xa lá cây cho cấp menu 1
            if (data.MENU_LVL === 1) {
                $(row).css("background-color", "#d4edd1"); // Xanh lá cây nhạt
            }
        }
    });

    // Gắn sự kiện chọn tất cả sau khi DataTable khởi tạo
    $(document).off('change', '#check_all').on('change', '#check_all', function () {
        const isChecked = $(this).is(":checked");
        $(".checkbox_item").prop("checked", isChecked);
    });
}
function showEditModal(data, callback) {
    //// Lấy dữ liệu từ dòng hiện tại
    //const userId = data.find("td:eq(3)").text().trim();
    //const userName = data.find("td:eq(4)").text().trim();
    //const dept = data.find("td:eq(5)").text().trim();
    //const note = data.find("td:eq(6)").text().trim();
    //const authidx = data.data("auth-idx"); // <-- đúng cách lấy user-idx
    //const userIdx = data.data("user-idx"); // <-- đúng cách lấy user-idx

    //// Đổ dữ liệu vào modal
    //$("#txt_user_ID").val(userId);
    //$("#txt_user_NM").val(userName);
    //$("#txt_Dept").val(dept);
    //$("#txt_Note").val(note);
    //$("#Edit_IDX").val(userIdx);
    //$("#txt_Password").val("");
    //// Gán giá trị selector
    //$("#selector_PermissionGroup").val(String(authidx));



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

async function AddNewMenu() {
    const isPrimary = $('#RadioButton1').is(':checked');
    const menuCode = $("#txt_menuCode").val();
    const menuEng = $("#txt_MenuEng").val();
    const menuKo = $("#txt_MenuKo").val();
    const groupMenuCode = $("#txt_GroupMenuCode").val();

    if (!menuCode || !menuEng) {
        ShowMessage("Vui lòng nhập SCRN_PATH và tên tiếng Anh", "error");
        return;
    }

    $("#BackGround").show();

    const payload = {
        USER_ID: GetCookieValue("UserID"),
        tsmenuTranfer: {
            MENU_LVL: isPrimary ? 1 : 2,
            MENU_NM_EN: menuEng,
            MENU_NM_HAN: menuKo,
            MENU_NM_VIE: groupMenuCode,
            SCRN_PATH: menuCode
        }
    };

    const formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(payload));

    try {
        const response = await fetch(controller + "Buttonadd_Click", {
            method: "POST",
            body: formData
        });
        const data = await response.json();

        ShowMessage(data.Message, data.ErrorNumber === -1 ? "error" : "ok");
        LoadMenuDetail();
    } catch (err) {
        ShowMessage(err.message || "Lỗi không xác định", "error");
    } finally {
        $("#BackGround").hide();
    }
}
