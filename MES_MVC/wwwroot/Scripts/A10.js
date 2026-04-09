let CurrentID = null;
let BaseResult = {};

// Thêm function để lấy giá trị cookie
function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

// Function lấy thông tin người dùng hiện tại
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
});

document.getElementById("Buttonexport").addEventListener("click", function () {
    TableHTMLToExcel('DataGridView1', 'LineList.xls', 'LineList');
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);

    // Thêm thông tin người dùng vào mỗi request
    if (!param.USER_IDX) {
        param.USER_IDX = getCurrentUser();
    }

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/A10/${action}`, {
        method: "POST",
        body: formData
    })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function Buttonfind_Click() {
    let search = $("#P07_SearchBox").val() || "";
    ApiCall("Buttonfind_Click", { SearchString: search })
        .then(res => {
            BaseResult = res;
            DataGridView1Render(res.LineList || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
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
    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="19" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }
    data.forEach(item => {
        $tbody.append(`
    <tr data-id="${item.ID}">
        <td>
            <button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button>
        </td>
        <td>${item.ID ?? ""}</td>
        <td>${item.LineGroup ?? ""}</td>
        <td>${item.LineName ?? ""}</td>
        <td>${item.LineType ?? ""}</td>
        <td>${item.Family ?? ""}</td>
        <td>${item.LineCapa ?? ""}</td>
        <td>${item.WorkerNumber ?? ""}</td>
        <td>${item.SUB_Worker ?? ""}</td>
        <td>${item.FA_Worker ?? ""}</td>
        <td>${item.RO_Worker ?? ""}</td>
        <td>${item.CLIP_Worker ?? ""}</td>
        <td>${item.Channel ?? ""}</td>
        <td>${item.Vision_LeakTest ?? ""}</td>
        <td>${item.Description ?? ""}</td>
        <td>${item.CreateDate ? formatDateTime(item.CreateDate) : ""}</td>
        <td>${item.CreateUser ?? ""}</td>
        <td>${item.UpdateDate ? formatDateTime(item.UpdateDate) : ""}</td>
        <td>${item.UpdateUser ?? ""}</td>
    </tr>
`);
    });
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    // Xử lý cả chuỗi ISO và đối tượng Date
    const date = typeof dateTimeStr === 'string'
        ? new Date(dateTimeStr)
        : dateTimeStr;

    if (isNaN(date.getTime())) {
        // Nếu là chuỗi định dạng khác, thử cắt 16 ký tự đầu
        return dateTimeStr.substring(0, 16).replace('T', ' ');
    }

    // Định dạng yyyy-MM-dd HH:mm
    return date.toISOString().substring(0, 16).replace('T', ' ');
}

function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.LineList || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#TextBox_ID").val(data.ID ?? "");
    $("#TextBox_LineGroup").val(data.LineGroup ?? "");
    $("#TextBox_LineName").val(data.LineName ?? "");
    $("#TextBox_LineType").val(data.LineType ?? "");
    $("#TextBox_Family").val(data.Family ?? "");
    $("#TextBox_LineCapa").val(data.LineCapa ?? "");
    $("#TextBox_WorkerNumber").val(data.WorkerNumber ?? "");
    $("#TextBox_SUB_Worker").val(data.SUB_Worker ?? "");
    $("#TextBox_FA_Worker").val(data.FA_Worker ?? "");
    $("#TextBox_RO_Worker").val(data.RO_Worker ?? "");
    $("#TextBox_CLIP_Worker").val(data.CLIP_Worker ?? "");
    $("#TextBox_Channel").val(data.Channel ?? "");
    $("#TextBox_Vision_LeakTest").val(data.Vision_LeakTest ?? "");
    $("#TextBox_Description").val(data.Description ?? "");
}

function ResetForm() {
    $("#P07Form")[0].reset();
    $("#TextBox_ID").val("");
}

function ValidateForm() {
    let isValid = true;
    let errors = [];

    // Kiểm tra các trường bắt buộc
    if (!$("#TextBox_LineGroup").val().trim()) {
        errors.push("Line Group không được để trống");
        isValid = false;
    }

    if (!$("#TextBox_LineName").val().trim()) {
        errors.push("Line Name không được để trống");
        isValid = false;
    }

    // Hiển thị lỗi nếu có
    if (!isValid) {
        alert(errors.join("\n"));
    }

    return isValid;
}

function GetFormData() {
    return {
        ID: $("#TextBox_ID").val(),
        TextBox0: $("#TextBox_LineGroup").val(),
        TextBox1: $("#TextBox_LineName").val(),
        TextBox2: $("#TextBox_LineType").val(),
        TextBox3: $("#TextBox_Family").val(),
        TextBox4: $("#TextBox_LineCapa").val(),
        ComboBox1: $("#TextBox_WorkerNumber").val(),
        ComboBox2: $("#TextBox_SUB_Worker").val(),
        ComboBox3: $("#TextBox_FA_Worker").val(),
        ComboBox4: $("#TextBox_RO_Worker").val(),
        ComboBox5: $("#TextBox_CLIP_Worker").val(),
        TextBox10: $("#TextBox_Channel").val(),
        TextBox11: $("#TextBox_Vision_LeakTest").val(),
        TextBox12: $("#TextBox_Description").val(),
        USER_IDX: getCurrentUser() // Thêm thông tin người dùng
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A10/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttoninport_Click() {
    $("#FileImportLineP07").val("");
    $(".file-path").val("");
    $("#ImportPreviewTableLineP07").empty();
    $("#ModalImportLineP07").modal("open");
}

$("#FileImportLineP07").change(function (e) {
    let file = e.target.files[0];
    if (!file) return;

    // Kiểm tra tên file
    let validName = ["LineList.xlsx", "LineList.xls"];
    if (validName.indexOf(file.name) === -1) {
        alert("File không đúng định dạng, vui lòng tải file mẫu!");
        $(this).val("");
        $("#ImportPreviewTableLineP07").html("");
        return;
    }

    $("#importProgressLineP07").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let sheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(sheet, { header: 1 });

            // Bỏ qua hàng tiêu đề
            if (jsonData.length > 0) jsonData.shift();

            let html = '';
            jsonData.forEach(row => {
                if (!row[0]) return;

                html += '<tr>';

                for (let i = 0; i < 13; i++) {
                    html += `<td>${row[i] !== undefined ? row[i] : ''}</td>`;
                }
                html += '</tr>';
            });

            $("#ImportPreviewTableLineP07").html(html);
        } catch (error) {
            alert("Lỗi khi đọc file Excel: " + error.message);
        } finally {
            $("#importProgressLineP07").hide();
        }
    };

    reader.readAsArrayBuffer(file);
});

$("#BtnSaveImportLineP07").click(function () {
    let importData = [];
    let currentUser = getCurrentUser();

    $("#ImportPreviewTableLineP07 tr").each(function () {
        let $tds = $(this).find("td");
        if ($tds.length >= 13) {
            importData.push({
                LineGroup: $tds.eq(0).text().trim(),
                LineName: $tds.eq(1).text().trim(),
                LineType: $tds.eq(2).text().trim(),
                Family: $tds.eq(3).text().trim(),
                LineCapa: $tds.eq(4).text().trim() ? parseFloat($tds.eq(4).text().trim()) : null,
                WorkerNumber: $tds.eq(5).text().trim() ? parseInt($tds.eq(5).text().trim()) : null,
                SUB_Worker: $tds.eq(6).text().trim() ? parseInt($tds.eq(6).text().trim()) : null,
                FA_Worker: $tds.eq(7).text().trim() ? parseInt($tds.eq(7).text().trim()) : null,
                RO_Worker: $tds.eq(8).text().trim() ? parseInt($tds.eq(8).text().trim()) : null,
                CLIP_Worker: $tds.eq(9).text().trim() ? parseInt($tds.eq(9).text().trim()) : null,
                Channel: $tds.eq(10).text().trim() ? parseInt($tds.eq(10).text().trim()) : null,
                Vision_LeakTest: $tds.eq(11).text().trim() ? parseInt($tds.eq(11).text().trim()) : null,
                Description: $tds.eq(12).text().trim(),
                CreateUser: currentUser // Thêm thông tin người dùng
            });
        }
    });

    if (!importData.length) {
        alert("Không có dữ liệu để import");
        return;
    }

    $("#importProgressLineP07").show();

    // Gửi dữ liệu dưới dạng JSON
    fetch('/A10/Buttoninport_Click', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(importData)
    })
        .then(response => response.json())
        .then(res => {
            $("#importProgressLineP07").hide();
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImportLineP07").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Import lỗi!");
            }
        })
        .catch(error => {
            $("#importProgressLineP07").hide();
            console.error("Error in import:", error);
            alert("Import thất bại! " + error.message);
        });
});

function Buttonprint_Click() {
    window.open(`/A10/Buttonprint_Click`, "_blank");
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}