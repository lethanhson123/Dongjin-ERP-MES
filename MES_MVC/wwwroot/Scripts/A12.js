let CurrentID = null;
let BaseResult = {};

$(document).ready(function () {
    $('.modal').modal();
    Buttonfind_Click();

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    $("#DataGridView1").on("click", ".btn-edit", function () {
        let id = $(this).closest("tr").data("id");
        EditRow_Click(id);
    });
    $("#DataGridView1").on("click", ".btn-delete", function () {
        let id = $(this).closest("tr").data("id");
        ConfirmDelete_Click(id);
    });

    $("#ModalSaveBtn").click(Buttonsave_Click);
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/A12/${action}`, {
        method: "POST",
        body: formData
    })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function Buttonfind_Click() {
    let search = $("#ShiftTime_SearchBox").val() || "";
    ApiCall("Buttonfind_Click", { SearchString: search })
        .then(res => {
            BaseResult = res;
            console.log(res);
            DataGridView1Render(res.ShiftTimes || []);
        });
}

$("#DataGridView1").on("click", "tr", function () {
    // Bỏ chọn các dòng khác
    $("#DataGridView1 tr.selected").removeClass("selected");
    // Chọn dòng hiện tại
    $(this).addClass("selected");
});

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();
    if (!data.length) {
        $tbody.append('<tr><td colspan="12" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }
    data.forEach(item => {
        let startTime = new Date(`2000-01-01T${item.StartTime}`);
        let endTime = new Date(`2000-01-01T${item.EndTime}`);

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td>
                    <button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button>
                </td>
                <td>${item.ID ?? ""}</td>
                <td>${item.ShiftName ?? ""}</td>
                <td>${startTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</td>
                <td>${endTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</td>
                <td>${item.BreakTime ?? ""}</td>
                <td>${item.Description ?? ""}</td>
                <td>${item.Active ? "Hoạt động" : "Không hoạt động"}</td>
                <td>${item.CreateDate ? item.CreateDate.substring(0, 16).replace('T', ' ') : ""}</td>
                <td>${item.CreateUser ?? ""}</td>
                <td>${item.UpdateDate ? item.UpdateDate.substring(0, 16).replace('T', ' ') : ""}</td>
                <td>${item.UpdateUser ?? ""}</td>
            </tr>
        `);
    });
}

function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#active-container").hide();
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.ShiftTimes || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;
    $("#active-container").show();
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#TextBox_ID").val(data.ID ?? "");
    $("#TextBox_ShiftName").val(data.ShiftName ?? "");

    if (data.StartTime) {
        const [hours, minutes] = data.StartTime.split(':');
        $("#TextBox_StartTime").val(`${hours}:${minutes}`);
    } else {
        $("#TextBox_StartTime").val("");
    }

    if (data.EndTime) {
        const [hours, minutes] = data.EndTime.split(':');
        $("#TextBox_EndTime").val(`${hours}:${minutes}`);
    } else {
        $("#TextBox_EndTime").val("");
    }

    $("#TextBox_BreakTime").val(data.BreakTime ?? "");
    $("#TextBox_Description").val(data.Description ?? "");
    $("#CheckBox_Active").prop("checked", data.Active);
}

function ResetForm() {
    $("#ShiftTimeForm")[0].reset();
    $("#TextBox_ID").val("");
    $("#CheckBox_Active").prop("checked", true);
}

function GetFormData() {
    const currentUser = getCurrentUser();
    return {
        ID: $("#TextBox_ID").val(),
        TextBox2: $("#TextBox_ShiftName").val(),
        TextBox3: $("#TextBox_StartTime").val(),
        TextBox4: $("#TextBox_EndTime").val(),
        TextBox5: $("#TextBox_BreakTime").val(),
        TextBox6: $("#TextBox_Description").val(),
        TextBox7: $("#CheckBox_Active").is(":checked").toString(),
        USER_NM: currentUser
    };
}

function Buttonsave_Click() {
    let data = GetFormData();
    let action = data.ID ? "Buttonsave_Click" : "Buttonadd_Click";
    ApiCall(action, data)
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                $("#modal-add").modal("close");
                alert(res.Message || "Thành công");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        });
}

function ConfirmDelete_Click(id) {
    if (!confirm("Bạn chắc chắn muốn vô hiệu hóa ca làm việc này?")) return;
    ApiCall("Buttondelete_Click", { ID: id })
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                alert(res.Message || "Vô hiệu hóa thành công");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        });
}

function Buttondelete_Click() {
    let id = $("#DataGridView1 tr.selected").data("id");
    if (id) {
        ConfirmDelete_Click(id);
    } else {
        alert("Vui lòng chọn dòng muốn vô hiệu hóa!");
    }
}

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}