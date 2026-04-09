let BaseResult;
let TagIndex = 1;
let DataGridView1RowIndex = -1;
let DataGridView2RowIndex = -1;
let DataGridView3RowIndex = -1;
let currentBladeIndex = -1;
let excelData = [];

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
});
document.getElementById("Buttonexport").addEventListener("click", function () {
    TableHTMLToExcel('DataGridView1Table', 'BladeList.xlsx', 'BladeList');
});
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
    $("#ModalImportBlades").modal("open");
    $("#ImportPreviewTable").empty();
    excelData = [];
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

function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {}
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X02/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        })
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonfind_Click() {
    if (TagIndex == 1) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: []
        }
        BaseParameter.ListSearchString.push($("#search_PartName").val());
        BaseParameter.ListSearchString.push($("#search_Spec").val());
        BaseParameter.ListSearchString.push($("#search_Location").val());

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/X02/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                console.error("Error parsing JSON:", err);
                $("#BackGround").css("display", "none");
            })
        }).catch((err) => {
            console.error("Fetch error:", err);
            $("#BackGround").css("display", "none");
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
        $("#txt_PartName").val("");
        $("#txt_Spec").val("");
        $("#txt_Location").val("");
        $("#txt_SafetyStock").val("");
        $("#txt_Description").val("");

        $("#txt_PartName").focus();
    }
    else if (TagIndex == 2) {
        $("#modal_Code").val("");
        $("#modal_Quantity").val("");
        $("#modal_SafetyStock").val("");
        $("#modal_Note").val("");
        $("#modal_User").val(GetCookieValue("USER_IDX"));

        $("#scanInModal").modal("open");
    }
}

function Buttonsave_Click() {
    if (TagIndex == 1) {
        let partName = $("#txt_PartName").val().trim();
        let spec = $("#txt_Spec").val().trim();
        let location = $("#txt_Location").val().trim();
        let safetyStock = $("#txt_SafetyStock").val().trim();
        let description = $("#txt_Description").val().trim();

        if (partName === "") {
            alert("Vui lòng nhập Tên lưỡi cắt!");
            $("#txt_PartName").focus();
            return;
        }

        if (spec === "") {
            alert("Vui lòng nhập Thông số kỹ thuật!");
            $("#txt_Spec").focus();
            return;
        }

        if (location === "") {
            alert("Vui lòng nhập Vị trí!");
            $("#txt_Location").focus();
            return;
        }

        if (safetyStock === "" || isNaN(safetyStock) || parseInt(safetyStock) < 0) {
            alert("Tồn kho an toàn phải là số nguyên dương!");
            $("#txt_SafetyStock").focus();
            return;
        }

        let currentUser = getCurrentUser();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [partName, spec, location, safetyStock, description],
            USER_IDX: currentUser
        }

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/X02/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                if (data.Error) {
                    alert(data.Error);
                } else {
                    alert("Lưu thành công!");
                    Buttonfind_Click();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert("Lỗi: " + err);
                $("#BackGround").css("display", "none");
            })
        });
    }
}

function Buttondelete_Click() {
    if (TagIndex == 1) {
        if (DataGridView1RowIndex < 0 || !BaseResult || !BaseResult.DataGridView1 || DataGridView1RowIndex >= BaseResult.DataGridView1.length) {
            alert("Vui lòng chọn lưỡi cắt cần xóa!");
            return;
        }

        if (confirm("Bạn có chắc chắn muốn xóa lưỡi cắt này?")) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                SearchString: BaseResult.DataGridView1[DataGridView1RowIndex].ID
            }

            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/X02/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    if (data.Error) {
                        alert(data.Error);
                    } else {
                        alert("Xóa thành công!");
                        Buttonfind_Click();
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert("Lỗi: " + err);
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}

function Buttoncancel_Click() {
    if (TagIndex == 1) {
        $("#txt_PartName").val("");
        $("#txt_Spec").val("");
        $("#txt_Location").val("");
        $("#txt_SafetyStock").val("");
        $("#txt_Description").val("");

        $("#search_PartName").val("");
        $("#search_Spec").val("");
        $("#search_Location").val("");
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
    $("#ModalImportBlades").modal("open");
    $("#ImportPreviewTable").empty();
    excelData = [];
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X02/Buttonexport_Click";

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

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X02/Buttonprint_Click";

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
function LoadScanInData() {
    $("#BackGround").css("display", "block");

    // Lấy giá trị từ date picker
    let fromDate = $("#txt_ScanIn_FromDate").val();
    let toDate = $("#txt_ScanIn_ToDate").val();

    console.log("Loading Scan In data from", fromDate, "to", toDate);

    let BaseParameter = {
        FromDate: fromDate,
        ToDate: toDate
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X02/LoadScanInData";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            BaseResult = data;
            DataGridView2Render();
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}
function LoadScanOutData() {
    $("#BackGround").css("display", "block");

    // Lấy giá trị từ date picker
    let fromDate = $("#txt_ScanOut_FromDate").val();
    let toDate = $("#txt_ScanOut_ToDate").val();

    console.log("Loading Scan Out data from", fromDate, "to", toDate);

    let BaseParameter = {
        FromDate: fromDate,
        ToDate: toDate
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X02/LoadScanOutData";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            BaseResult = data;
            DataGridView3Render();
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            HTML += "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView3[i].W1 || "") + "</td>"; // Code
            HTML += "<td>" + (BaseResult.DataGridView3[i].W2 || "") + "</td>"; // Name
            HTML += "<td>" + (BaseResult.DataGridView3[i].QTY || "0") + "</td>"; // Quantity
            HTML += "<td>" + (BaseResult.DataGridView3[i].RAT || "0") + "</td>"; // SafetyStock
            HTML += "<td>" + (BaseResult.DataGridView3[i].MT || "0") + "</td>"; // CurrentStock
            HTML += "<td>" + formatDate(BaseResult.DataGridView3[i].CREATE_DTM) + "</td>"; // CreateDate
            HTML += "<td>" + (BaseResult.DataGridView3[i].CREATE_USER || "") + "</td>"; // CreateUser
            HTML += "<td>" + (BaseResult.DataGridView3[i].W5 || "") + "</td>"; // ReasonForUse
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3_SelectionChanged(index) {
    DataGridView3RowIndex = index;
}

$("#btnScanOut").click(function () {
    $("#modal_PartName_Out").val("");
    $("#modal_Quantity_Out").val("1");
    $("#modal_User_Out").val(getCurrentUser());
    $("#modal_ReasonForUse_Out").val("");

    $("#scanOutModal").modal("open");
});
$("#btnScanOutSave").click(function () {
    let partName = $("#modal_PartName_Out").val().trim();
    let quantity = $("#modal_Quantity_Out").val().trim();
    let user = $("#modal_User_Out").val().trim();
    let reasonForUse = $("#modal_ReasonForUse_Out").val().trim();

    if (partName === "") {
        alert("Vui lòng nhập tên lưỡi cắt!");
        $("#modal_PartName_Out").focus();
        return;
    }

    if (quantity === "" || isNaN(quantity) || parseInt(quantity) <= 0) {
        alert("Số lượng phải là số nguyên dương!");
        $("#modal_Quantity_Out").focus();
        return;
    }

    let currentUser = getCurrentUser();

    let BaseParameter = {
        Action: 3,
        ListSearchString: [partName, quantity, currentUser, reasonForUse],
        USER_IDX: currentUser
    };

    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/X02/ButtonScanOut_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                if (data.Error === "Số lượng tồn kho không đủ") {
                    alert("Không đủ số lượng tồn kho để xuất!");
                } else {
                    alert(data.Error);
                }
            } else {
                alert("Scan Out thành công!");
                $("#scanOutModal").modal("close");
                LoadScanOutData();
                Load(); // Cập nhật lại DataGridView1
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
});
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            HTML += "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + (BaseResult.DataGridView2[i].W1 || "") + "</td>"; // Code
            HTML += "<td>" + (BaseResult.DataGridView2[i].W2 || "") + "</td>"; // Name
            HTML += "<td>" + (BaseResult.DataGridView2[i].QTY || "0") + "</td>"; // Quantity
            HTML += "<td>" + (BaseResult.DataGridView2[i].RAT || "0") + "</td>"; // SafetyStock
            HTML += "<td>" + (BaseResult.DataGridView2[i].MT || "0") + "</td>"; // CurrentStock
            HTML += "<td>" + formatDate(BaseResult.DataGridView2[i].CREATE_DTM) + "</td>"; // CreateDate
            HTML += "<td>" + (BaseResult.DataGridView2[i].CREATE_USER || "") + "</td>"; // CreateUser
            HTML += "<td>" + (BaseResult.DataGridView2[i].W5 || "") + "</td>"; // ReasonForUse
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2_SelectionChanged(index) {
    DataGridView2RowIndex = index;
}
$("#btnScanIn").click(function () {
    $("#modal_PartName").val("");
    $("#modal_Quantity").val("1");
    $("#modal_User").val(getCurrentUser());
    $("#modal_ReasonForUse").val("");

    $("#scanInModal").modal("open");
});
$("#btnScanInSave").click(function () {
    let partName = $("#modal_PartName").val().trim();
    let quantity = $("#modal_Quantity").val().trim();
    let user = $("#modal_User").val().trim();
    let reasonForUse = $("#modal_ReasonForUse").val().trim();

    if (partName === "") {
        alert("Vui lòng nhập tên lưỡi cắt!");
        $("#modal_PartName").focus();
        return;
    }

    if (quantity === "" || isNaN(quantity) || parseInt(quantity) <= 0) {
        alert("Số lượng phải là số nguyên dương!");
        $("#modal_Quantity").focus();
        return;
    }

    let currentUser = getCurrentUser();

    let BaseParameter = {
        Action: 2,
        ListSearchString: [partName, quantity, currentUser, reasonForUse],
        USER_IDX: currentUser
    };

    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/X02/ButtonScanIn_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                alert(data.Error);
            } else {
                alert("Scan In thành công!");
                $("#scanInModal").modal("close");
                LoadScanInData();
                Load(); // Cập nhật lại DataGridView1
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
});
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";

            HTML += "<td>" + (BaseResult.DataGridView1[i].PART_NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].SPECIFICATION || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].Location || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].MT || "0") + "</td>";

            // Hiển thị cảnh báo nếu tồn kho thấp hơn mức an toàn
            let stockWarning = BaseResult.DataGridView1[i].MIN;
            let warningClass = "";
            if (stockWarning < 0) {
                warningClass = "warning-negative";
            }
            HTML += "<td class='" + warningClass + "'>" + (stockWarning || "0") + "</td>";

            HTML += "<td>" + (BaseResult.DataGridView1[i].RAT || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].W5 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].D00 === "1" ? "Y" : "N") + "</td>";
            HTML += "<td>" + formatDate(BaseResult.DataGridView1[i].CREATE_DTM) + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].CREATE_USER || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(index) {
    DataGridView1RowIndex = index;

    if (BaseResult && BaseResult.DataGridView1 && index >= 0 && index < BaseResult.DataGridView1.length) {
        $("#txt_PartName").val(BaseResult.DataGridView1[index].PART_NAME || "");
        $("#txt_Spec").val(BaseResult.DataGridView1[index].SPECIFICATION || "");
        $("#txt_Location").val(BaseResult.DataGridView1[index].Location || "");
        $("#txt_SafetyStock").val(BaseResult.DataGridView1[index].RAT || "");
        $("#txt_Description").val(BaseResult.DataGridView1[index].W5 || "");
    }
}
$("#FileImportBlades").change(function () {
    let file = this.files[0];
    if (!file) return;
    $("#importProgress").show();
    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.SheetNames[0];
            let rawData = XLSX.utils.sheet_to_json(workbook.Sheets[firstSheet], { header: 1 });
            if (rawData.length > 0) rawData = rawData.slice(1);
            excelData = rawData.map(row => {
                if (row.length < 5) return null;
                return {
                    PART_NAME: row[0] || "",
                    SPECIFICATION: row[1] || "",
                    Location: row[2] || "",
                    RAT: parseInt(row[3] || 0),
                    W5: row[4] || ""
                };
            }).filter(item => item && item.PART_NAME);
            showPreview();
            $("#importProgress").hide();
        } catch (error) {
            alert("Lỗi đọc file: " + error);
            $("#importProgress").hide();
        }
    };
    reader.readAsArrayBuffer(file);
});


function showPreview() {
    $("#ImportPreviewTable").empty();
    let limit = Math.min(excelData.length, 50);
    for (let i = 0; i < limit; i++) {
        let item = excelData[i];
        let row = "<tr>";
        row += "<td>" + (item.PART_NAME || "") + "</td>";
        row += "<td>" + (item.SPECIFICATION || "") + "</td>";
        row += "<td>" + (item.Location || "") + "</td>";
        row += "<td>" + (item.RAT || "0") + "</td>";
        row += "<td>" + (item.W5 || "") + "</td>";
        row += "</tr>";
        $("#ImportPreviewTable").append(row);
    }
    if (excelData.length > limit) {
        $("#ImportPreviewTable").append("<tr><td colspan='5' class='center-align'>...và " +
            (excelData.length - limit) + " dòng khác</td></tr>");
    }
}

$("#BtnSaveImport").click(function () {
    if (excelData.length === 0) {
        alert("Không có dữ liệu để import");
        return;
    }
    $("#importProgress").show();
    let BaseParameter = {
        ImportData: excelData,
        USER_IDX: getCurrentUser()
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/X02/Buttoninport_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            $("#importProgress").hide();
            if (data.Error) {
                alert("Lỗi: " + data.Error);
            } else {
                alert("Import thành công! " + (data.TotalCount || 0) + " lưỡi cắt đã được nhập.");
                $("#ModalImportBlades").modal("close");
                $("#FileImportBlades").val("");
                $("#ImportPreviewTable").empty();
                excelData = [];
                Load();
            }
        })
        .catch(error => {
            $("#importProgress").hide();
            alert("Lỗi khi gửi dữ liệu: " + error);
        });
});

function getCurrentUser() {
    return GetCookieValue('UserID') || GetCookieValue('USER_IDX') || 'SYSTEM';
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

$("#FileImportBlades").change(function () {
    let file = this.files[0];
    if (!file) return;

    $("#importProgress").show();
    let reader = new FileReader();

    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.SheetNames[0];
            let rawData = XLSX.utils.sheet_to_json(workbook.Sheets[firstSheet], { header: 1 });

            if (rawData.length > 0) rawData = rawData.slice(1);

            excelData = rawData.map(row => {
                if (row.length < 5) return null;
                return {
                    PART_NAME: row[0] || "",      // Cột 1: Part Name
                    SPECIFICATION: row[1] || "",  // Cột 2: Specification - Đúng tên thuộc tính C#
                    Location: row[2] || "",       // Cột 3: Location
                    RAT: parseInt(row[3] || 0),   // Cột 4: Safety Stock
                    W5: row[4] || ""              // Cột 5: Description
                };
            }).filter(item => item && item.PART_NAME);

            // Log để kiểm tra
            console.log("Dữ liệu đã parse:", excelData);

            showPreview();
            $("#importProgress").hide();
        } catch (error) {
            alert("Lỗi đọc file: " + error);
            console.error("Lỗi đọc file Excel:", error);
            $("#importProgress").hide();
        }
    };

    reader.readAsArrayBuffer(file);
});

function showPreview() {
    $("#ImportPreviewTable").empty();
    let limit = Math.min(excelData.length, 50);

    for (let i = 0; i < limit; i++) {
        let item = excelData[i];
        let row = "<tr>";
        row += "<td>" + (item.PART_NAME || "") + "</td>";
        row += "<td>" + (item.SPECIFICATION || "") + "</td>";  // Đúng tên thuộc tính C#
        row += "<td>" + (item.Location || "") + "</td>";
        row += "<td>" + (item.RAT || "0") + "</td>";
        row += "<td>" + (item.W5 || "") + "</td>";
        row += "</tr>";
        $("#ImportPreviewTable").append(row);
    }

    if (excelData.length > limit) {
        $("#ImportPreviewTable").append("<tr><td colspan='5' class='center-align'>...và " +
            (excelData.length - limit) + " dòng khác</td></tr>");
    }
}

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    window.open(url, '', 'width=' + width + ',height=' + height + ',top=' + top + ',left=' + left);
}

