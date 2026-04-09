let BaseResult = {};
let TagIndex = 1;
let DataGridView1RowIndex = -1;
let DataGridView2RowIndex = -1;
let DataGridView3RowIndex = -1;
let currentPartIndex = -1;
let isUploadMode = false;
let excelData = [];
let CurrentID = null;

// ============================================
// UTILITY FUNCTIONS
// ============================================
function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || GetCookieValue('USER_IDX') || 'SYSTEM';
}

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));

    return fetch(`/X01/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function formatDate(dateString) {
    if (!dateString) return "";
    let date = new Date(dateString);
    if (isNaN(date.getTime())) return dateString;

    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    let hours = ("0" + date.getHours()).slice(-2);
    let minutes = ("0" + date.getMinutes()).slice(-2);

    return year + "-" + month + "-" + day + " " + hours + ":" + minutes;
}

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    window.open(url, '', 'width=' + width + ',height=' + height + ',top=' + top + ',left=' + left);
}

// ============================================
// INITIALIZATION
// ============================================
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $("#txt_ScanIn_FromDate").val(today);
    $("#txt_ScanIn_ToDate").val(today);
    $("#txt_ScanOut_FromDate").val(today);
    $("#txt_ScanOut_ToDate").val(today);

    $(".modal").modal();
    Load();

    // Button Events
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    // Tab Events
    $("#ATag001").click(function (e) {
        TagIndex = 1;
    });
    $("#ATag002").click(function (e) {
        TagIndex = 2;
        LoadScanInData();
    });
    $("#ATag003").click(function (e) {
        TagIndex = 3;
        LoadScanOutData();
    });

    // Search on Enter key
    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });

    // Modal Save Button
    $("#ModalSaveBtn").click(Buttonsave_Click);

    // Table row click events
    $("#DataGridView1").on("click", ".btn-edit", function () {
        let id = $(this).closest("tr").data("id");
        EditRow_Click(id);
    });

    $("#DataGridView1").on("click", "tr", function () {
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });

    // Scan In Events
    $("#btnScanIn").click(OpenScanInModal);
    $("#modal_Code").on("change", LoadPartInfoByCode);
    $("#btnScanInSave").click(SaveScanIn);

    // Scan Out Events
    $("#btnScanOut").click(OpenScanOutModal);
    $("#modal_Code_Out").on("change", LoadPartInfoByCodeOut);
    $("#btnScanOutSave").click(SaveScanOut);

    // Image Upload Events
    $("#btnToggleUpload").click(toggleUploadMode);
    $("#partImageUpload").change(PreviewImage);
    $("#btnSaveImage").click(SavePartImage);

    // Import Events
    $("#FileImportParts").change(HandleFileSelect);
    $("#BtnSaveImport").click(SaveImport);
});

// ============================================
// MAIN CRUD OPERATIONS
// ============================================
function Load() {
    ApiCall("Load", {})
        .then(res => {
            BaseResult = res;
            DataGridView1Render();
        })
        .catch(err => {
            console.error("Error loading data:", err);
            alert("Có lỗi xảy ra khi tải dữ liệu: " + err.message);
        });
}

function Buttonfind_Click() {
    if (TagIndex == 1) {
        let search = $("#SearchBox").val() || "";
        let param = {
            SearchString: search
        };

        ApiCall("Buttonfind_Click", param)
            .then(res => {
                BaseResult = res;
                DataGridView1Render();
            })
            .catch(err => {
                console.error("Error in search:", err);
                alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
            });
    }
    else if (TagIndex == 2) {
        LoadScanInData();
    }
    else if (TagIndex == 3) {
        LoadScanOutData();
    }
}

function Buttonadd_Click() {
    if (TagIndex == 1) {
        ResetForm();
        CurrentID = null;
        $("#modal-title").text("Add Spare Part");
        $("#modal-add").modal("open");
    }
    else if (TagIndex == 2) {
        OpenScanInModal();
    }
}

function EditRow_Click(id) {
    let data = (BaseResult.PartSpareList || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;
    $("#modal-title").text("Edit Spare Part");
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#PartSpareID").val(data.ID ?? "");
    $("#PartCode").val(data.Code ?? "");
    $("#PartName").val(data.Name ?? "");
    $("#UnitPrice").val(data.Price ?? "");
    $("#Supplier").val(data.Display ?? "");
    $("#QuantityRequired").val(data.QuantityRequired ?? "");
    $("#SafetyStock").val(data.SafetyStock ?? "");
    $("#MachineNo").val(data.Description ?? "");
}

function ResetForm() {
    $("#SparePartForm")[0].reset();
    $("#PartSpareID").val("");
}

function ValidateForm() {
    let errors = [];
    if (!$("#PartCode").val().trim()) errors.push("Code không được để trống");
    if (!$("#PartName").val().trim()) errors.push("Name không được để trống");

    let price = $("#UnitPrice").val();
    if (!price || isNaN(price) || parseFloat(price) < 0) {
        errors.push("Unit Price phải là số dương");
    }

    let qty = $("#QuantityRequired").val();
    if (!qty || isNaN(qty) || parseInt(qty) < 0) {
        errors.push("Quantity Required phải là số nguyên dương");
    }

    let safety = $("#SafetyStock").val();
    if (!safety || isNaN(safety) || parseInt(safety) < 0) {
        errors.push("Safety Stock phải là số nguyên dương");
    }

    if (errors.length > 0) {
        alert(errors.join("\n"));
        return false;
    }
    return true;
}

function GetFormData() {
    return {
        PartSpare: {
            ID: parseInt($("#PartSpareID").val()) || 0,
            Code: $("#PartCode").val().trim(),
            Name: $("#PartName").val().trim(),
            Price: parseFloat($("#UnitPrice").val()) || 0,
            Display: $("#Supplier").val().trim(),
            QuantityRequired: parseInt($("#QuantityRequired").val()) || 0,
            SafetyStock: parseInt($("#SafetyStock").val()) || 0,
            Description: $("#MachineNo").val().trim()
        },
        USER_IDX: getCurrentUser()
    };
}

function Buttonsave_Click() {
    if (TagIndex != 1) return;

    if (!ValidateForm()) return;

    let data = GetFormData();
    console.log("📤 Gửi data:", data);  // ✅ Add this

    let action = data.PartSpare.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";

    ApiCall(action, data)
        .then(res => {
            console.log("📥 Response:", res);  // ✅ Add this
            console.log("❌ Error:", res.Error);  // ✅ Add this
            if (res.Success) {
                Load();
                $("#modal-add").modal("close");
                alert(res.Message || "Thành công!");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("❌ Catch error:", err);  // ✅ Add this
            alert("Lỗi: " + err.message);
        });
}

function Buttondelete_Click() {
    if (TagIndex != 1) return;

    let id = $("#DataGridView1 tr.selected").data("id");
    if (!id) {
        alert("Vui lòng chọn dòng muốn xóa!");
        return;
    }

    if (!confirm("Bạn chắc chắn muốn xóa?")) return;

    let param = {
        ID: id
    };

    ApiCall("Buttondelete_Click", param)
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                alert(res.Message || "Xóa thành công!");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error in delete:", err);
            alert("Có lỗi xảy ra khi xóa: " + err.message);
        });
}

function Buttoncancel_Click() {
    if (TagIndex == 1) {
        ResetForm();
        $("#modal-add").modal("close");
    }
    else if (TagIndex == 2) {
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);

        $("#txt_ScanIn_FromDate").val(today);
        $("#txt_ScanIn_ToDate").val(today);
        LoadScanInData();
    }
}

function Buttoninport_Click() {
    $("#ModalImportParts").modal("open");
    $("#ImportPreviewTable").empty().append('<tr><td colspan="7" class="center-align grey-text">Chưa có dữ liệu. Vui lòng chọn file Excel.</td></tr>');
    $("#FileImportParts").val("");
    excelData = [];
}

function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1Table', 'SpareParts.xlsx', 'SpareParts');
}

function Buttonprint_Click() {
    if (DataGridView1RowIndex < 0 || !BaseResult?.PartSpareList?.[DataGridView1RowIndex]) {
        alert("Vui lòng chọn 1 dòng để in.");
        return;
    }

    const code = BaseResult.PartSpareList[DataGridView1RowIndex].Code;
    if (!code) {
        alert("Dòng được chọn không có Code.");
        return;
    }

    let param = {
        SearchString: code
    };

    ApiCall("Buttonprint_Click", param)
        .then(res => {
            if (res.QRCodeData) {
                printQRCode(res.QRCodeData, res.QRCodeText);
            } else {
                alert(res.Error || "Không có dữ liệu in");
            }
        })
        .catch(err => {
            console.error("Error in print:", err);
            alert("Có lỗi xảy ra khi in: " + err.message);
        });
}

function printQRCode(qrCodeBase64, qrText) {
    const printWindow = window.open('', '', 'width=400,height=300');
    printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>Print QR Code</title>
            <style>
                @page { margin: 0; }
                body {
                    margin: 0;
                    padding: 0;
                    font-family: Arial;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    text-align: center;
                }
                img {
                    width: 200px;
                    height: 200px;
                    display: block;
                    margin: 0 auto;
                }
                .text {
                    font-size: 12pt;
                    font-weight: bold;
                    margin-top: 10px;
                    word-break: break-all;
                }
            </style>
        </head>
        <body>
            <img src="data:image/png;base64,${qrCodeBase64}">
            <div class="text">${qrText}</div>
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.focus();
    printWindow.onload = function () {
        printWindow.print();
        printWindow.close();
    };
}

function Buttonhelp_Click() {
    OpenWindowByURL("/WMP_PLAY", 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

// ============================================
// DATA GRID RENDER FUNCTIONS
// ============================================
function DataGridView1Render() {
    let HTML = "";
    let data = BaseResult.PartSpareList || [];

    if (data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            let item = data[i];
            HTML += `<tr data-id="${item.ID}">`;

            // Action column with Edit button
            HTML += '<td class="center-align">';
            HTML += `<button class="btn btn-small btn-edit cyan"><i class="material-icons">edit</i></button>`;
            HTML += '</td>';

            // Image column
            HTML += "<td class='center-align'>";
            HTML += "<a href='javascript:void(0)' onclick='openPartImage(" + i + ", event)'>";
            if (item.FileName) {
                HTML += "<i class='material-icons' style='color: #26a69a;'>photo</i>";
            } else {
                HTML += "<i class='material-icons'>add_a_photo</i>";
            }
            HTML += "</a></td>";

            HTML += "<td>" + (item.Code || "") + "</td>";
            HTML += "<td>" + (item.Name || "") + "</td>";
            HTML += "<td>" + (item.Price || "") + "</td>";
            HTML += "<td>" + (item.Display || "") + "</td>";
            HTML += "<td>" + (item.QuantityRequired || "") + "</td>";
            HTML += "<td>" + (item.SafetyStock || "") + "</td>";
            HTML += "<td>" + (item.Inventory || "") + "</td>";

            let inventoryWarning = item.InventoryWarning || 0;
            let warningClass = inventoryWarning < 0 ? "warning-negative" : "";
            HTML += "<td class='" + warningClass + "'>" + inventoryWarning + "</td>";

            HTML += "<td>" + (item.TotalAmount || "") + "</td>";
            HTML += "<td>" + (item.Description || "") + "</td>";
            HTML += "<td>" + (item.Active ? "Y" : "N") + "</td>";
            HTML += "<td>" + formatDate(item.CreateDate) + "</td>";
            HTML += "<td>" + (item.CreateUserName || "") + "</td>";
            HTML += "</tr>";
        }
    } else {
        HTML = '<tr><td colspan="15" class="center-align grey-text">Không có dữ liệu</td></tr>';
    }

    document.getElementById("DataGridView1").innerHTML = HTML;
    $("#TotalRecords").text(data.length);
}

function DataGridView2Render() {
    let HTML = "";
    let data = BaseResult.PartSpareScanInList || [];

    if (data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            let item = data[i];
            HTML += "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + (item.Code || "") + "</td>";
            HTML += "<td>" + (item.Name || "") + "</td>";
            HTML += "<td>" + (item.Quantity || "") + "</td>";
            HTML += "<td>" + (item.SafetyQty || "") + "</td>";
            HTML += "<td>" + (item.InventoryQty || "") + "</td>";
            HTML += "<td>" + formatDate(item.CreateDate) + "</td>";
            HTML += "<td>" + (item.CreateUserName || "") + "</td>";
            HTML += "<td>" + (item.Note || "") + "</td>";
            HTML += "<td>" + (item.Description || "") + "</td>";
            HTML += "</tr>";
        }
    } else {
        HTML = '<tr><td colspan="9" class="center-align grey-text">Không có dữ liệu</td></tr>';
    }

    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView3Render() {
    let HTML = "";
    let data = BaseResult.PartSpareScanOutList || [];

    if (data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            let item = data[i];
            HTML += "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + (item.Code || "") + "</td>";
            HTML += "<td>" + (item.Name || "") + "</td>";
            HTML += "<td>" + (item.Quantity || "") + "</td>";
            HTML += "<td>" + (item.SafetyQty || "") + "</td>";
            HTML += "<td>" + (item.InventoryQty || "") + "</td>";
            HTML += "<td>" + formatDate(item.CreateDate) + "</td>";
            HTML += "<td>" + (item.CreateUserName || "") + "</td>";
            HTML += "<td>" + (item.Note || "") + "</td>";
            HTML += "<td>" + (item.Description || "") + "</td>";
            HTML += "</tr>";
        }
    } else {
        HTML = '<tr><td colspan="9" class="center-align grey-text">Không có dữ liệu</td></tr>';
    }

    document.getElementById("DataGridView3").innerHTML = HTML;
}

function DataGridView2_SelectionChanged(index) {
    DataGridView2RowIndex = index;
}

function DataGridView3_SelectionChanged(index) {
    DataGridView3RowIndex = index;
}

// ============================================
// SCAN IN OPERATIONS
// ============================================
function OpenScanInModal() {
    $("#modal_Code").val("");
    $("#modal_Quantity").val("");
    $("#modal_SafetyStock").val("");
    $("#modal_QuantityToAdd").val("1");
    $("#modal_User").val(getCurrentUser());
    $("#modal_ReasonForUse").val("");
    $("#modal_Note").val("");
    $("#scanInModal").modal("open");
}

function LoadPartInfoByCode() {
    let code = $("#modal_Code").val().trim();
    if (code === "") return;

    let param = {
        SearchString: code
    };

    ApiCall("GetPartInfoByCode", param)
        .then(res => {
            if (res && !res.Error) {
                $("#modal_Quantity").val(res.QuantityRequired || "");
                $("#modal_SafetyStock").val(res.SafetyStock || "");
            }
        })
        .catch(err => {
            console.error("Error loading part info:", err);
        });
}

function SaveScanIn() {
    let code = $("#modal_Code").val().trim();
    let quantity = $("#modal_QuantityToAdd").val().trim();
    let safetyStock = $("#modal_SafetyStock").val().trim();
    let reasonForUse = $("#modal_ReasonForUse").val().trim();
    let note = $("#modal_Note").val().trim();

    // Validation
    if (code === "") {
        alert("Vui lòng nhập mã phụ tùng!");
        $("#modal_Code").focus();
        return;
    }

    if (quantity === "" || isNaN(quantity) || parseInt(quantity) <= 0) {
        alert("Số lượng thêm vào phải là số nguyên dương!");
        $("#modal_QuantityToAdd").focus();
        return;
    }

    let param = {
        PartSpareScanIn: {
            Code: code,
            Quantity: parseInt(quantity),
            SafetyQty: safetyStock ? parseInt(safetyStock) : null,
            Description: reasonForUse,
            Note: note
        }
    };

    ApiCall("ButtonScanIn_Click", param)
        .then(res => {
            if (res.Error) {
                if (res.Error === "NotFound") {
                    alert("Không tìm thấy phụ tùng với mã: " + code);
                } else {
                    alert(res.Error);
                }
            } else {
                alert("Scan In thành công!");
                $("#scanInModal").modal("close");
                LoadScanInData();
                Load();
            }
        })
        .catch(err => {
            console.error("Error in scan in:", err);
            alert("Có lỗi xảy ra: " + err.message);
        });
}

function LoadScanInData() {
    let fromDate = $("#txt_ScanIn_FromDate").val();
    let toDate = $("#txt_ScanIn_ToDate").val();
    let search = "";

    let param = {
        FromDate: fromDate ? new Date(fromDate) : null,
        ToDate: toDate ? new Date(toDate) : null,
        SearchString: search
    };

    ApiCall("LoadScanInData", param)
        .then(res => {
            BaseResult = res;
            DataGridView2Render();
        })
        .catch(err => {
            console.error("Error loading scan in data:", err);
            alert("Có lỗi xảy ra: " + err.message);
        });
}

// ============================================
// SCAN OUT OPERATIONS
// ============================================
function OpenScanOutModal() {
    $("#modal_Code_Out").val("");
    $("#modal_Quantity_Out").val("");
    $("#modal_SafetyStock_Out").val("");
    $("#modal_QuantityToRemove").val("1");
    $("#modal_User_Out").val(getCurrentUser());
    $("#modal_ReasonForUse_Out").val("");
    $("#modal_Note_Out").val("");
    $("#scanOutModal").modal("open");
}

function LoadPartInfoByCodeOut() {
    let code = $("#modal_Code_Out").val().trim();
    if (code === "") return;

    let param = {
        SearchString: code
    };

    ApiCall("GetPartInfoByCode", param)
        .then(res => {
            if (res && !res.Error) {
                $("#modal_Quantity_Out").val(res.QuantityRequired || "");
                $("#modal_SafetyStock_Out").val(res.SafetyStock || "");
            }
        })
        .catch(err => {
            console.error("Error loading part info:", err);
        });
}

function SaveScanOut() {
    let code = $("#modal_Code_Out").val().trim();
    let quantity = $("#modal_QuantityToRemove").val().trim();
    let safetyStock = $("#modal_SafetyStock_Out").val().trim();
    let reasonForUse = $("#modal_ReasonForUse_Out").val().trim();
    let note = $("#modal_Note_Out").val().trim();

    // Validation
    if (code === "") {
        alert("Vui lòng nhập mã phụ tùng!");
        $("#modal_Code_Out").focus();
        return;
    }

    if (quantity === "" || isNaN(quantity) || parseInt(quantity) <= 0) {
        alert("Số lượng lấy ra phải là số nguyên dương!");
        $("#modal_QuantityToRemove").focus();
        return;
    }

    let param = {
        PartSpareScanOut: {
            Code: code,
            Quantity: parseInt(quantity),
            SafetyQty: safetyStock ? parseInt(safetyStock) : null,
            Description: reasonForUse,
            Note: note
        }
    };

    ApiCall("ButtonScanOut_Click", param)
        .then(res => {
            if (res.Error) {
                if (res.Error === "NotFound") {
                    alert("Không tìm thấy phụ tùng với mã: " + code);
                } else if (res.Error === "NotEnoughInventory") {
                    alert("Số lượng tồn kho không đủ để xuất!");
                } else {
                    alert(res.Error);
                }
            } else {
                alert("Scan Out thành công!");
                $("#scanOutModal").modal("close");
                LoadScanOutData();
                Load();
            }
        })
        .catch(err => {
            console.error("Error in scan out:", err);
            alert("Có lỗi xảy ra: " + err.message);
        });
}

function LoadScanOutData() {
    let fromDate = $("#txt_ScanOut_FromDate").val();
    let toDate = $("#txt_ScanOut_ToDate").val();
    let search = "";

    let param = {
        FromDate: fromDate ? new Date(fromDate) : null,
        ToDate: toDate ? new Date(toDate) : null,
        SearchString: search
    };

    ApiCall("LoadScanOutData", param)
        .then(res => {
            BaseResult = res;
            DataGridView3Render();
        })
        .catch(err => {
            console.error("Error loading scan out data:", err);
            alert("Có lỗi xảy ra: " + err.message);
        });
}

// ============================================
// IMAGE OPERATIONS
// ============================================
function openPartImage(index, event) {
    event.stopPropagation();
    currentPartIndex = index;
    isUploadMode = false;
    $("#imageUploadSection").hide();
    $("#btnSaveImage").hide();
    $("#btnToggleText").text("Change Image");
    $("#imageModalTitle").text("Part Image");

    if (BaseResult.PartSpareList[index].FileName) {
        $("#partImage").attr("src", "/PartImages/" + BaseResult.PartSpareList[index].FileName + "?t=" + new Date().getTime());
    } else {
        $("#partImage").attr("src", "/Images/no-image.png");
        toggleUploadMode();
    }
    $("#imageModal").modal("open");
}

function toggleUploadMode() {
    isUploadMode = !isUploadMode;

    if (isUploadMode) {
        $("#imageUploadSection").show();
        $("#btnSaveImage").show();
        $("#btnToggleText").text("Cancel");
        $("#imageModalTitle").text("Upload Image");
    } else {
        $("#imageUploadSection").hide();
        $("#btnSaveImage").hide();
        $("#btnToggleText").text("Change Image");
        $("#imageModalTitle").text("Part Image");
        $("#partImageUpload").val("");
        if (BaseResult.PartSpareList[currentPartIndex].FileName) {
            $("#partImage").attr("src", "/PartImages/" + BaseResult.PartSpareList[currentPartIndex].FileName + "?t=" + new Date().getTime());
        } else {
            $("#partImage").attr("src", "/Images/no-image.png");
        }
    }
}

function PreviewImage() {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#partImage").attr("src", e.target.result);
        }
        reader.readAsDataURL(this.files[0]);
    }
}

function SavePartImage() {
    if (currentPartIndex < 0 || !$("#partImageUpload")[0].files[0]) {
        alert("Please select an image file");
        return;
    }

    let partID = BaseResult.PartSpareList[currentPartIndex].ID;
    let formUpload = new FormData();
    formUpload.append('file', $("#partImageUpload")[0].files[0]);

    let BaseParameter = {
        ID: partID,
        USER_IDX: getCurrentUser()
    };

    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    ShowLoading(true);

    fetch("/X01/UploadPartImage", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Error) {
                alert(data.Error);
            } else {
                alert("Image uploaded successfully!");
                toggleUploadMode();
                Load();
            }
            ShowLoading(false);
        })
        .catch(err => {
            alert("Error: " + err);
            ShowLoading(false);
        });
}

// ============================================
// IMPORT OPERATIONS
// ============================================
function HandleFileSelect(e) {
    let file = e.target.files[0];
    if (!file) return;

    $("#importProgress").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.SheetNames[0];
            let rawData = XLSX.utils.sheet_to_json(workbook.Sheets[firstSheet], { header: 1 });

            if (rawData.length > 0) rawData = rawData.slice(1); // Skip header

            excelData = rawData.map(row => {
                if (row.length < 7) return null;
                return {
                    Code: row[0] || "",
                    Name: row[1] || "",
                    Price: parseFloat(row[2] || 0),
                    Display: row[3] || "",
                    QuantityRequired: parseInt(row[4] || 0),
                    SafetyStock: parseInt(row[5] || 0),
                    Description: row[6] || ""
                };
            }).filter(item => item && item.Code);

            RenderImportPreview();
            $("#importProgress").hide();
        } catch (error) {
            alert("Lỗi đọc file: " + error.message);
            $("#importProgress").hide();
        }
    };

    reader.readAsArrayBuffer(file);
}

function RenderImportPreview() {
    $("#ImportPreviewTable").empty();
    let limit = Math.min(excelData.length, 50);

    for (let i = 0; i < limit; i++) {
        let item = excelData[i];
        let row = "<tr>";
        row += "<td>" + item.Code + "</td>";
        row += "<td>" + item.Name + "</td>";
        row += "<td>" + item.Price + "</td>";
        row += "<td>" + item.Display + "</td>";
        row += "<td>" + item.QuantityRequired + "</td>";
        row += "<td>" + item.SafetyStock + "</td>";
        row += "<td>" + item.Description + "</td>";
        row += "</tr>";
        $("#ImportPreviewTable").append(row);
    }

    if (excelData.length > limit) {
        $("#ImportPreviewTable").append("<tr><td colspan='7' class='center-align'>...và " +
            (excelData.length - limit) + " dòng khác</td></tr>");
    }
}

function SaveImport() {
    if (excelData.length === 0) {
        alert("Không có dữ liệu để import");
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn import ${excelData.length} records?`)) return;

    let param = {
        PartSpareList: excelData
    };

    ApiCall("Buttoninport_Click", param)
        .then(res => {
            if (res.Error) {
                alert("Lỗi: " + res.Error);
            } else {
                alert(res.Message || "Import thành công!");
                $("#ModalImportParts").modal("close");
                $("#FileImportParts").val("");
                $("#ImportPreviewTable").empty();
                excelData = [];
                Load();
            }
        })
        .catch(error => {
            console.error("Error in import:", error);
            alert("Lỗi khi gửi dữ liệu: " + error.message);
        });
}