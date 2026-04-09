let CurrentID = null;
let BaseResult = {};
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

$(document).ready(function () {
    $('.modal').modal();
    Buttonfind_Click();

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
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

    $("#F04_SearchBox").keypress(function (e) {
        if (e.which == 13) {
            Buttonfind_Click();
        }
    });
    $("#selectAll").change(function () {
        $("input[id^='DataGridView1CHK']").prop("checked", this.checked);
    });
    $("#Buttonprint").off("click").on("click", function () {
        PrintQRCodes();
    });
    $("#TextBox_ErrorCode").on("blur", function () {
        if (!$("#ComboBox_ErrorType").val()) {
            let errorCode = $(this).val().trim();

            if (errorCode.startsWith('CTLP-')) {
                $("#ComboBox_ErrorType").val('CTLP');
            } else if (errorCode.startsWith('FA-')) {
                $("#ComboBox_ErrorType").val('FA');
            }
        }
    });
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);

    if (!param.USER_IDX) {
        param.USER_IDX = getCurrentUser();
    }

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/F04/${action}`, {
        method: "POST",
        body: formData
    })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function Buttonfind_Click() {
    let search = $("#F04_SearchBox").val() || "";
    ApiCall("Buttonfind_Click", { SearchString: search })
        .then(res => {
            BaseResult = res;
            DataGridView1Render(res.NGLists || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
        });
}

$("#DataGridView1").on("click", "tr", function () {
    $("#DataGridView1 tr.selected").removeClass("selected");
    $(this).addClass("selected");
});

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();
    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="11" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < data.length; i++) {
        let item = data[i];
        $tbody.append(`
            <tr data-id="${item.ID}">
                <td>
                    <label>
                        <input id="DataGridView1CHK${i}" class="form-check-input" type="checkbox" data-error-code="${item.ErrorCode}">
                        <span></span>
                    </label>
                </td>
                <td>
                    <button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button>
                </td>
                <td>${item.ID ?? ""}</td>
                <td>${item.ErrorCode ?? ""}</td>
                <td>${item.ErrorType ?? ""}</td>
                <td>${item.ErrorDescription ?? ""}</td>
                <td>${item.KoreanDescription ?? ""}</td>
                <td>${item.CreateDate ? formatDateTime(item.CreateDate) : ""}</td>
                <td>${item.CreateUser ?? ""}</td>
                <td>${item.UpdateDate ? formatDateTime(item.UpdateDate) : ""}</td>
                <td>${item.UpdateUser ?? ""}</td>
            </tr>
        `);
    }
}
function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    try {
        const date = new Date(dateTimeStr);

        if (isNaN(date.getTime())) {
            return dateTimeStr;
        }

        return date.toLocaleString('vi-VN');
    } catch (e) {
        return dateTimeStr;
    }
}
function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.NGLists || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#TextBox_ID").val(data.ID ?? "");
    $("#TextBox_ErrorCode").val(data.ErrorCode ?? "");
    $("#ComboBox_ErrorType").val(data.ErrorType ?? "");
    $("#TextBox_ErrorDescription").val(data.ErrorDescription ?? "");
    $("#TextBox_KoreanDescription").val(data.KoreanDescription ?? "");
}

function ResetForm() {
    $("#F04Form")[0].reset();
    $("#TextBox_ID").val("");
}

function ValidateForm() {
    let isValid = true;
    let errors = [];

    if (!$("#TextBox_ErrorCode").val().trim()) {
        errors.push("Mã lỗi không được để trống");
        isValid = false;
    }

    if (!$("#ComboBox_ErrorType").val().trim()) {
        errors.push("Loại lỗi không được để trống");
        isValid = false;
    }

    if (!$("#TextBox_ErrorDescription").val().trim()) {
        errors.push("Mô tả lỗi không được để trống");
        isValid = false;
    }

    let errorCode = $("#TextBox_ErrorCode").val().trim();
    let errorType = $("#ComboBox_ErrorType").val().trim();

    if (errorType === "CTLP" && !errorCode.startsWith("CTLP-")) {
        errors.push("Mã lỗi của loại CTLP phải bắt đầu bằng 'CTLP-'");
        isValid = false;
    } else if (errorType === "FA" && !errorCode.startsWith("FA-")) {
        errors.push("Mã lỗi của loại FA phải bắt đầu bằng 'FA-'");
        isValid = false;
    }

    if (!isValid) {
        alert(errors.join("\n"));
    }

    return isValid;
}

function GetFormData() {
    let ngItem = {
        ID: $("#TextBox_ID").val() ? parseInt($("#TextBox_ID").val()) : 0,
        ErrorCode: $("#TextBox_ErrorCode").val(),
        ErrorType: $("#ComboBox_ErrorType").val(),
        ErrorDescription: $("#TextBox_ErrorDescription").val(),
        KoreanDescription: $("#TextBox_KoreanDescription").val()
    };

    return {
        ID: ngItem.ID,
        NGList: ngItem,
        USER_IDX: getCurrentUser()
    };
}

function Buttonsave_Click() {
    if (!ValidateForm()) return;

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
        })
        .catch(err => {
            console.error("Error in save:", err);
            alert("Có lỗi xảy ra khi lưu: " + err.message);
        });
}

function ConfirmDelete_Click(id) {
    if (!confirm("Bạn chắc chắn muốn xóa?")) return;

    ApiCall("Buttondelete_Click", { ID: id })
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                alert(res.Message || "Xóa thành công");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error in delete:", err);
            alert("Có lỗi xảy ra khi xóa: " + err.message);
        });
}

function Buttondelete_Click() {
    let id = $("#DataGridView1 tr.selected").data("id");
    if (id) {
        ConfirmDelete_Click(id);
    } else {
        alert("Vui lòng chọn dòng muốn xóa!");
    }
}

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

function Buttonexport_Click() {
    ApiCall("Buttonexport_Click")
        .then(res => {
            if (res.Success && res.NGLists && res.NGLists.length > 0) {
                TableHTMLToExcel('DataGridView1', 'NGList.xls', 'NGList');
            } else {
                alert("Không có dữ liệu để xuất");
            }
        })
        .catch(err => {
            console.error("Error in export:", err);
            alert("Có lỗi xảy ra khi xuất file: " + err.message);
        });
}

function Buttoninport_Click() {
    $("#FileImportF04").val("");
    $(".file-path").val("");
    $("#ImportPreviewTableF04").empty();
    $("#ModalImportF04").modal("open");
}
$("#FileImportF04").change(function (e) {
    let file = e.target.files[0];
    if (!file) return;

    $("#importProgressF04").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let sheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(sheet, { header: 1 });

          
            if (jsonData.length > 0) jsonData.shift();

            let html = '';
            jsonData.forEach(row => {
                if (!row[0]) return;

             
                let errorCode = row[0] || "";
                let errorType = "";

             
                if (errorCode.startsWith('CTLP-')) {
                    errorType = 'CTLP';
                } else if (errorCode.startsWith('FA-')) {
                    errorType = 'FA';
                }

                html += '<tr>';
                html += `<td>${errorCode}</td>`;
                html += `<td>${errorType}</td>`; 
                html += `<td>${row[1] || ""}</td>`; 
                html += `<td>${row[2] || ""}</td>`; 
                html += '</tr>';
            });

            $("#ImportPreviewTableF04").html(html);
        } catch (error) {
            alert("Lỗi khi đọc file Excel: " + error.message);
        } finally {
            $("#importProgressF04").hide();
        }
    };

    reader.readAsArrayBuffer(file);
});
$("#BtnSaveImportF04").click(function () {
    let importData = [];
    let currentUser = getCurrentUser();

    $("#ImportPreviewTableF04 tr").each(function () {
        let $tds = $(this).find("td");
        if ($tds.length >= 4) {
            importData.push({
                ErrorCode: $tds.eq(0).text(),
                ErrorType: $tds.eq(1).text(), 
                ErrorDescription: $tds.eq(2).text(),
                KoreanDescription: $tds.eq(3).text(),
                Active: true,
                CreateUser: currentUser
            });
        }
    });

    if (!importData.length) {
        alert("Không có dữ liệu để import");
        return;
    }

    ApiCall("Buttoninport_Click", { NGLists: importData })
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImportF04").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Import thất bại!");
            }
        })
        .catch(err => {
            console.error("Error in import:", err);
            alert("Có lỗi xảy ra khi import dữ liệu!");
        });
});
function PrintQRCodes() {
    let selectedCodes = [];
    $("input[id^='DataGridView1CHK']:checked").each(function () {
        selectedCodes.push($(this).data("error-code"));
    });

    if (selectedCodes.length === 0) {
        alert("Vui lòng chọn ít nhất một mã lỗi để in QR code!");
        return;
    }
    let url = "/F04/PrintQRCode?codes=" + selectedCodes.join(',');
    window.open(url, "_blank");
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
