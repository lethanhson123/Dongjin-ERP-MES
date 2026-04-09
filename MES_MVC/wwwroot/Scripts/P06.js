let BaseResult;
let importData = [];

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
    $("#P06_ExcelFile").click();
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

$("#P06_ExcelFile").change(function (e) {
    if (e.target.files.length > 0) {
        $("#BackGround").css("display", "block");
        importExcelFile(e.target.files[0]);
    }
});

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        DateTimePicker1: $("#DateTimepicker1").val(),
        TextBox1: $("#Search").val()
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#DataGridViewTop").empty();
            if (data.DataGridView1 && data.DataGridView1.length > 0) {
                for (let i = 0; i < data.DataGridView1.length; i++) {
                    let item = data.DataGridView1[i];
                    let row = `<tr>
                        <td>${item.Coln1 || ""}</td>
                        <td>${item.Coln2 || ""}</td>
                        <td>${item.Coln3 || ""}</td>
                        <td>${item.Coln4 || ""}</td>
                        <td>${item.Coln5 || ""}</td>
                        <td>${item.Coln6 || ""}</td>
                        <td>${item.Coln7 || ""}</td>
                        <td>${item.DATEString || ""}</td>
                        <td>${item.CreateTime || ""}</td>
                        <td>${item.CreateBy || ""}</td>
                        <td>${item.UpdateTime || ""}</td>
                        <td>${item.UpdateBy || ""}</td>
                        <td>${item.COUNT || 0}</td>
                    </tr>`;
                    $("#DataGridViewTop").append(row);
                }
            }

            if (data.DataGridView && data.DataGridView.length > 0) {
                let headerRow = data.DataGridView[0];
                let headerHtml = "<tr>";
                headerHtml += "<th>PO Code</th>";
                headerHtml += "<th>Vehicle</th>";
                headerHtml += "<th>Family</th>";
                headerHtml += "<th>Product Code</th>";
                headerHtml += "<th>ECN No</th>";
                headerHtml += "<th>SNP</th>";
                headerHtml += "<th>Total</th>";

                for (let i = 1; i <= 30; i++) {
                    let colName = "D" + (i < 10 ? "0" + i : i);
                    let dateValue = headerRow[colName] || "";
                    let displayDate = "";
                    if (dateValue) {
                        let dateParts = dateValue.split("-");
                        if (dateParts.length === 3) {
                            displayDate = dateParts[2] + "/" + dateParts[1];
                        } else {
                            displayDate = dateValue;
                        }
                    }
                    headerHtml += "<th>" + displayDate + "</th>";
                }
                headerHtml += "</tr>";
                $("#DataGridViewTableBottom thead").html(headerHtml);

                $("#DataGridViewBottom").empty();
                let dataRows = data.DataGridView.slice(1);
                if (dataRows.length > 0) {
                    for (let i = 0; i < dataRows.length; i++) {
                        let row = dataRows[i];
                        let total = 0;

                        let html = "<tr>";
                        html += "<td>" + (row.Coln1 || "") + "</td>";
                        html += "<td>" + (row.Coln2 || "") + "</td>";
                        html += "<td>" + (row.Coln3 || "") + "</td>";
                        html += "<td>" + (row.Coln4 || "") + "</td>";
                        html += "<td>" + (row.Coln5 || "") + "</td>";
                        html += "<td>" + (row.Coln6 || "") + "</td>";

                        for (let j = 1; j <= 30; j++) {
                            let colName = "D" + (j < 10 ? "0" + j : j);
                            let value = parseInt(row[colName] || 0);
                            total += value;
                        }
                        html += "<td>" + total + "</td>";

                        for (let j = 1; j <= 30; j++) {
                            let colName = "D" + (j < 10 ? "0" + j : j);
                            let value = row[colName] > 0 ? row[colName] : "";
                            html += "<td>" + value + "</td>";
                        }
                        html += "</tr>";
                        $("#DataGridViewBottom").append(html);
                    }
                }
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi: " + err.message);
        })
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttonadd_Click";

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

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

function Buttonsave_Click() {
    if (importData.length === 0) {
        alert("Không có dữ liệu để lưu!");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ImportData: importData,
        ListSearchString: [
            getCurrentUser()
        ]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (data.Error) {
                alert("Lỗi khi lưu dữ liệu: " + data.Error);
            } else {
                alert(data.Message || "Lưu dữ liệu thành công!");
                importData = [];
                $("#DataGridViewTop").empty();
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi khi lưu dữ liệu: " + err.message);
        })
    });
}

function loadDetailGrid() {
    $("#BackGround").css("display", "block");

    fetch("/P06/GetDetailGridData", {
        method: "GET"
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Error: " + data.Error);
                $("#BackGround").css("display", "none");
                return;
            }

            if (data.DataGridView && data.DataGridView.length > 0) {
                processDetailGridData(data.DataGridView);
            } else {
                $("#DataGridViewBottom").empty();
                alert("No data found for detail grid.");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Error: " + err.message);
        });
    });
}

function renderDetailGrid(headerRow, tableData) {
    $("#DataGridViewBottom").empty();
    updateColumnHeaders(headerRow);
    var html = "";

    for (var i = 0; i < tableData.length; i++) {
        var row = tableData[i];

        html += "<tr>";
        html += "<td>" + row.POCode + "</td>";
        html += "<td>" + row.Vehicle + "</td>";
        html += "<td>" + row.Family + "</td>";
        html += "<td>" + row.ProductCode + "</td>";
        html += "<td>" + row.ECN + "</td>";
        html += "<td>" + row.SNP + "</td>";
        html += "<td>" + row.Total + "</td>";

        for (var j = 1; j <= 30; j++) {
            var colName = "D" + (j < 10 ? "0" + j : j);
            var value = row[colName] > 0 ? row[colName] : "";
            html += "<td>" + value + "</td>";
        }

        html += "</tr>";
    }

    $("#DataGridViewBottom").append(html);
}

function updateColumnHeaders(headerRow) {
    var headerHtml = "<tr>";
    headerHtml += "<th>PO Code</th>";
    headerHtml += "<th>Vehicle</th>";
    headerHtml += "<th>Family</th>";
    headerHtml += "<th>Product Code</th>";
    headerHtml += "<th>ECN No</th>";
    headerHtml += "<th>SNP</th>";
    headerHtml += "<th>Total</th>";

    for (var i = 1; i <= 30; i++) {
        var colName = "D" + (i < 10 ? "0" + i : i);
        var dateValue = headerRow[colName] || "";

        var displayDate = "";
        if (dateValue) {
            var dateParts = dateValue.split("-");
            if (dateParts.length === 3) {
                displayDate = dateParts[2] + "/" + dateParts[1];
            } else {
                displayDate = dateValue;
            }
        }

        headerHtml += "<th>" + displayDate + "</th>";
    }

    headerHtml += "</tr>";
    $("#DataGridViewTableBottom thead").html(headerHtml);
}

function processDetailGridData(gridData) {
    if (!gridData || gridData.length === 0) return;
    var headerRow = gridData[0];
    var tableData = [];
    for (var i = 1; i < gridData.length; i++) {
        var row = gridData[i];

        var rowData = {
            POCode: row.Coln1,
            Vehicle: row.Coln2,
            Family: row.Coln3,
            ProductCode: row.Coln4,
            ECN: row.Coln5,
            SNP: row.Coln6
        };
        var total = 0;
        for (var j = 1; j <= 30; j++) {
            var colName = "D" + (j < 10 ? "0" + j : j);
            var value = parseInt(row[colName] || 0);

            rowData[colName] = value;
            total += value;
        }
        rowData["Total"] = total;

        tableData.push(rowData);
    }
    renderDetailGrid(headerRow, tableData);
}

$(document).ready(function () {
    var today = new Date();
    var formattedDate = today.getFullYear() + "-" +
        ("0" + (today.getMonth() + 1)).slice(-2) + "-" +
        ("0" + today.getDate()).slice(-2);
    $("#DateTimepicker1").val(formattedDate);

    var twoWeeksLater = new Date(today);
    twoWeeksLater.setDate(today.getDate() + 14);
    var formattedDateTwoWeeks = twoWeeksLater.getFullYear() + "-" +
        ("0" + (twoWeeksLater.getMonth() + 1)).slice(-2) + "-" +
        ("0" + twoWeeksLater.getDate()).slice(-2);
    $("#DateTimepicker2").val(formattedDateTwoWeeks);

    $("#DateTimepicker1").change(function () {
        var fromDate = new Date($(this).val());
        var toDate = new Date(fromDate);
        toDate.setDate(fromDate.getDate() + 14);
        var toDateFormatted = toDate.getFullYear() + "-" +
            ("0" + (toDate.getMonth() + 1)).slice(-2) + "-" +
            ("0" + toDate.getDate()).slice(-2);
        $("#DateTimepicker2").val(toDateFormatted);
        Buttonfind_Click();
    });

    Buttonfind_Click();
});

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttondelete_Click";

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

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttoncancel_Click";

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

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttonexport_Click";

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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/P06/Buttonprint_Click";

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

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function importExcelFile(file) {
    let reader = new FileReader();

    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheetName = workbook.SheetNames[0];
            let worksheet = workbook.Sheets[firstSheetName];
            let rows = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
            processExcelData(rows);
        } catch (error) {
            console.error("Lỗi khi đọc file Excel:", error);
            alert("Có lỗi khi đọc file Excel: " + error.message);
            $("#BackGround").css("display", "none");
        }
    };

    reader.onerror = function () {
        alert("Không thể đọc file!");
        $("#BackGround").css("display", "none");
    };

    reader.readAsArrayBuffer(file);
}

function processExcelData(rows) {
    importData = [];

    try {
        let startRow = -1;
        for (let i = 0; i < rows.length; i++) {
            if (rows[i] && rows[i][1] && typeof rows[i][1] === 'string' &&
                !rows[i][1].toLowerCase().includes('po code') &&
                !rows[i][1].toLowerCase().includes('pocode')) {
                startRow = i;
                break;
            }
        }

        if (startRow === -1) startRow = 3;

        let dateRow = -1;
        for (let i = 0; i < startRow; i++) {
            if (rows[i] && rows[i][8] && isValidDateValue(rows[i][8])) {
                dateRow = i;
                break;
            }
        }

        for (let i = startRow; i < rows.length; i++) {
            if (!rows[i] || rows[i].length === 0) continue;

            if (rows[i].length >= 7) {
                let poCode = rows[i][1] || "";

                if (typeof poCode === 'string' &&
                    (poCode.toLowerCase().includes('po code') ||
                        poCode.toLowerCase().includes('pocode'))) {
                    continue;
                }

                let ecnValue = rows[i][5] || "";
                if (typeof ecnValue === 'string') {
                    ecnValue = ecnValue.replace(/ECN/gi, "").trim();
                }

                for (let j = 8; j < rows[i].length; j++) {
                    if (rows[i][j] && rows[i][j] > 0) {
                        let woDate = new Date();
                        let woDateString = formatDate(woDate);

                        if (dateRow !== -1 && rows[dateRow][j]) {
                            let dateValue = rows[dateRow][j];

                            if (isValidDateValue(dateValue)) {
                                if (typeof dateValue === 'number') {
                                    woDate = convertExcelDateToJSDate(dateValue);
                                }
                                else if (typeof dateValue === 'string') {
                                    let parsedDate = parseDate(dateValue);
                                    if (parsedDate) {
                                        woDate = parsedDate;
                                    }
                                }

                                woDateString = formatDate(woDate);
                            }
                        }

                        let item = {
                            INDEX: 'temp_' + i + '_' + j,
                            Coln1: poCode,
                            Coln2: rows[i][2] || "",
                            Coln3: rows[i][3] || "",
                            Coln4: rows[i][4] || "",
                            Coln5: ecnValue,
                            Coln6: rows[i][6] || 0,
                            Coln7: parseInt(rows[i][j]) || 0,
                            DATEString: woDateString,
                            CreateTime: formatDateTime(new Date()),
                            CreateBy: "",
                            COUNT: 0
                        };

                        if (item.Coln1 && item.Coln7 > 0) {
                            importData.push(item);
                        }
                    }
                }
            }
        }

        displayDataOnGrid();
        $("#BackGround").css("display", "none");
    } catch (error) {
        console.error("Lỗi khi xử lý dữ liệu:", error);
        alert("Có lỗi khi xử lý dữ liệu: " + error.message);
        $("#BackGround").css("display", "none");
    }
}

function isValidDateValue(value) {
    if (typeof value === 'number') {
        return value > 1;
    }

    if (typeof value === 'string') {
        return parseDate(value) !== null;
    }

    return false;
}

function convertExcelDateToJSDate(excelDate) {
    return new Date((excelDate - 25569) * 86400 * 1000);
}

function parseDate(dateStr) {
    if (!dateStr) return null;

    const formats = [
        { regex: /^(\d{4})-(\d{1,2})-(\d{1,2})$/, fn: (m) => new Date(parseInt(m[1]), parseInt(m[2]) - 1, parseInt(m[3])) },
        { regex: /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/, fn: (m) => new Date(parseInt(m[3]), parseInt(m[2]) - 1, parseInt(m[1])) },
        { regex: /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/, fn: (m) => new Date(parseInt(m[3]), parseInt(m[1]) - 1, parseInt(m[2])) },
        { regex: /^(\d{4})\/(\d{1,2})\/(\d{1,2})$/, fn: (m) => new Date(parseInt(m[1]), parseInt(m[2]) - 1, parseInt(m[3])) }
    ];

    for (const format of formats) {
        const match = dateStr.match(format.regex);
        if (match) {
            const date = format.fn(match);
            if (!isNaN(date.getTime())) {
                return date;
            }
        }
    }

    const date = new Date(dateStr);
    if (!isNaN(date.getTime())) {
        return date;
    }

    return null;
}

function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function formatDateTime(date) {
    const formattedDate = formatDate(date);
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    return `${formattedDate} ${hours}:${minutes}:${seconds}`;
}

function displayDataOnGrid() {
    $("#DataGridViewTop").empty();

    importData.forEach(item => {
        let row = `<tr>
            <td>${item.Coln1}</td>
            <td>${item.Coln2}</td>
            <td>${item.Coln3}</td>
            <td>${item.Coln4}</td>
            <td>${item.Coln5}</td>
            <td>${item.Coln6}</td>
            <td>${item.Coln7}</td>
            <td>${item.DATEString}</td>
            <td>${item.CreateTime}</td>
            <td>${item.CreateBy}</td>
            <td></td>
            <td></td>
            <td>${item.COUNT}</td>
        </tr>`;
        $("#DataGridViewTop").append(row);
    });

    if (importData.length > 0) {
        alert(`Đã nhập ${importData.length} bản ghi`);
    } else {
        alert("Không có dữ liệu hợp lệ để nhập!");
    }
}

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    let features = `width=${width},height=${height},left=${left},top=${top},resizable=yes,scrollbars=yes`;
    window.open(url, '_blank', features);
}
