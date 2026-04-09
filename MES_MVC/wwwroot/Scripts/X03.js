let BaseResult;
let excelData = [];

$(document).ready(function () {
    $('.modal').modal();
    if (!BaseResult) {
        BaseResult = {};
    }
    Load(); // Gọi hàm Load để tải dữ liệu ban đầu
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
    $("#ModalImportTaskTime").modal("open");
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Load";

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
            alert("Lỗi khi tải dữ liệu: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].ID || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].Coln1 || "") + "</td>"; // ECNNo
            HTML += "<td>" + (BaseResult.DataGridView1[i].Coln2 || "") + "</td>"; // ProductionLineName
            HTML += "<td>" + (BaseResult.DataGridView1[i].Coln3 || "") + "</td>"; // ProcessName
            HTML += "<td>" + (BaseResult.DataGridView1[i].PART_CODE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].PART_NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].FAMILY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].COUNT || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].MIN || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].UPH || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].MT || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].Description || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].Active === "1" ? "Y" : "N") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView").innerHTML = HTML;
}

$("#FileImportTaskTime").change(function () {
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

            if (rawData.length > 0) rawData = rawData.slice(1); // Bỏ dòng tiêu đề

            excelData = rawData.map(row => {
                if (row.length < 11) return null; // Đảm bảo đủ 11 cột
                return {
                    Coln1: row[0] || "",          // ECNNo
                    Coln2: row[1] || "",          // ProductionLineName
                    Coln3: row[2] || "",          // ProcessName
                    PART_CODE: row[3] || "",      // Product Code
                    PART_NAME: row[4] || "",      // Product Name
                    FAMILY: row[5] || "",         // Family
                    COUNT: parseInt(row[6] || 0), // Circuit Number
                    MIN: parseFloat(row[7] || 0), // Task Time
                    UPH: parseFloat(row[8] || 0), // Customer Task Time
                    MT: parseFloat(row[9] || 0),  // Gap
                    Description: row[10] || ""    // Note
                };
            }).filter(item => item && item.Coln1);

            console.log("Dữ liệu Excel:", excelData);
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
        row += "<td>" + (item.Coln1 || "") + "</td>";
        row += "<td>" + (item.Coln2 || "") + "</td>";
        row += "<td>" + (item.Coln3 || "") + "</td>";
        row += "<td>" + (item.PART_CODE || "") + "</td>";
        row += "<td>" + (item.PART_NAME || "") + "</td>";
        row += "<td>" + (item.FAMILY || "") + "</td>";
        row += "<td>" + (item.COUNT || "0") + "</td>";
        row += "<td>" + (item.MIN || "0") + "</td>";
        row += "<td>" + (item.UPH || "0") + "</td>";
        row += "<td>" + (item.MT || "0") + "</td>";
        row += "</tr>";
        $("#ImportPreviewTable").append(row);
    }

    if (excelData.length > limit) {
        $("#ImportPreviewTable").append("<tr><td colspan='10' class='center-align'>...và " +
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
        USER_IDX: GetCookieValue('USER_IDX') || 'SYSTEM',
        USER_NM: GetCookieValue('USER_NM') || 'SYSTEM'
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/X03/Buttoninport_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            $("#importProgress").hide();
            if (data.Error) {
                alert("Lỗi: " + data.Error);
            } else {
                alert("Import thành công! " + (data.TotalCount || 0) + " bản ghi đã được nhập.");
                $("#ModalImportTaskTime").modal("close");
                $("#FileImportTaskTime").val("");
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

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttoninport_Click() {
    $("#ModalImportTaskTime").modal("open");
    $("#ImportPreviewTable").empty();
    excelData = [];
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/X03/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            alert("Lỗi: " + err);
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        alert("Lỗi khi gửi yêu cầu: " + err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    window.open(url, '', 'width=' + width + ',height=' + height + ',top=' + top + ',left=' + left);
}

function GetCookieValue(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return '';
}