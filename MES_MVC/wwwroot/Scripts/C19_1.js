let BaseResult;
let currentImportType = "MASTER";

function loadXLSXLibrary() {
    return new Promise((resolve, reject) => {
        if (typeof XLSX !== 'undefined') {
            resolve();
            return;
        }

        const script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js';
        script.integrity = 'sha512-r22gChDnGvBylk90+2e/ycr3RVrDi8DIOkIGNhJlKfuyQM4tIRAI062MaV8sfjQKYVGjOBaZBOA87z+IhZE9DA==';
        script.crossOrigin = 'anonymous';
        script.referrerPolicy = 'no-referrer';

        script.onload = () => resolve();
        script.onerror = () => reject(new Error('Failed to load XLSX library'));

        document.head.appendChild(script);
    });
}

$(document).ready(function () {
    $("#RadioButton1").prop("checked", true);
    loadXLSXLibrary();

    try {
        $("#DataGridView1").empty();
    } catch (ex) { }

    $("#Button1").click(function () {
        Button1_Click();
    });

    $("#Buttoninport").click(function () {
        Buttoninport_Click();
    });

    $("#Buttonsave").click(function () {
        Buttonsave_Click();
    });

    $("#Buttonclose").click(function () {
        Buttonclose_Click();
    });

    // Xử lý sự kiện click cho tất cả radio buttons
    $("input[name='importType']").click(function () {
        currentImportType = $(this).attr("id") === "RadioButton1" ? "MASTER" : "MASTER_BOM";
    });

    $("#FileToUpload").change(function (e) {
        handleFileSelect(e);
    });
});

function Buttoninport_Click() {
    $("#FileToUpload").click();
}

// Hàm normalize tên cột từ Excel template gốc (VB) sang tên mới
function normalizeExcelColumns(data, importType) {
    if (importType === "MASTER") {
        // Mapping cho MASTER sheet
        const columnMapping = {
            'W_Partno': 'WIRE',      // VB column name -> New column name
            'Term1': 'TERM1',
            'SS1': 'SEAL1',
            'Term2': 'TERM2',
            'SS2': 'SEAL2',
            'Strip1': 'STRIP1',
            'Strip2': 'STRIP2',
            'Wire': 'WIRE_NM',
            'Diameter': 'W_Diameter',
            'Color': 'W_Color',
            'Length': 'W_Length',
            'SAFT_STOCK': 'SFTY_STK',
            'WRNo': 'WR_NO',
            'WLink': 'W_LINK',
            'T1_No': 'T1NO',
            'T2_No': 'T2NO'
        };

        return data.map(row => {
            let normalized = { ...row };

            // Map các cột từ tên cũ sang tên mới
            Object.keys(columnMapping).forEach(oldKey => {
                if (normalized[oldKey] !== undefined) {
                    normalized[columnMapping[oldKey]] = normalized[oldKey];
                    // Không xóa cột cũ để tránh mất dữ liệu nếu cả 2 cột đều tồn tại
                }
            });

            // Đảm bảo LEAD_SCN luôn có giá trị
            if (!normalized.LEAD_SCN && normalized.LEAD_SCN !== 0) {
                normalized.LEAD_SCN = "";
            }

            // Đảm bảo S_PART luôn có giá trị (không map sang LEAD_PN)
            if (!normalized.S_PART && normalized.S_PART !== 0) {
                normalized.S_PART = "";
            }

            return normalized;
        });
    } else if (importType === "MASTER_BOM") {
        // Mapping cho MASTER_BOM sheet
        const columnMapping = {
            'M_PART': 'M_PART',     // Map vào trường string
            'S_PART': 'S_PART',     // Map vào trường string
            'LR': 'S_LR'
        };

        return data.map(row => {
            let normalized = { ...row };

            Object.keys(columnMapping).forEach(oldKey => {
                if (normalized[oldKey] !== undefined) {
                    normalized[columnMapping[oldKey]] = normalized[oldKey];
                }
            });

            return normalized;
        });
    }

    return data;
}

function handleFileSelect(evt) {
    $("#BackGround").css("display", "block");

    const file = evt.target.files[0];
    if (!file) {
        $("#BackGround").css("display", "none");
        return;
    }

    loadXLSXLibrary()
        .then(() => {
            const reader = new FileReader();

            reader.onload = function (e) {
                try {
                    const data = new Uint8Array(e.target.result);
                    const workbook = XLSX.read(data, { type: 'array' });

                    let sheetName = currentImportType;

                    const worksheet = workbook.Sheets[sheetName];

                    if (!worksheet) {
                        alert(`Sheet "${sheetName}" không tồn tại trong file Excel.`);
                        $("#BackGround").css("display", "none");
                        return;
                    }

                    const jsonData = XLSX.utils.sheet_to_json(worksheet, { defval: "" });

                    if (jsonData.length === 0) {
                        alert("Không tìm thấy dữ liệu trong file Excel.");
                        $("#BackGround").css("display", "none");
                        return;
                    }

                    // Normalize column names từ Excel template gốc
                    const normalizedData = normalizeExcelColumns(jsonData, currentImportType);

                    renderDataGrid(normalizedData);

                    alert("Completed. Import to Excel");

                    $("#BackGround").css("display", "none");
                } catch (err) {
                    alert("Please Check Again. Error: " + (err.message || "Unknown error"));
                    $("#BackGround").css("display", "none");
                }
            };

            reader.onerror = function (err) {
                alert("Please Check Again. Error: File could not be read.");
                $("#BackGround").css("display", "none");
            };

            reader.readAsArrayBuffer(file);
        })
        .catch(error => {
            alert("Please Check Again. Error: " + error.message);
            $("#BackGround").css("display", "none");
        });
}

function renderDataGrid(data) {
    $("#DataGridView1").empty();

    if (!data || data.length === 0) return;

    let table = $("<table>").addClass("responsive-table");
    let thead = $("<thead>").addClass("sticky-header");
    let tbody = $("<tbody>");

    let headers = Object.keys(data[0]);

    // Filter headers để loại bỏ các cột không cần thiết
    headers = headers.filter(header => {
        if (header.includes("_EMPTY")) return false;

        // Với MASTER import, hiển thị các cột quan trọng
        if (currentImportType === "MASTER") {
            // Giữ lại các cột đã normalize
            const keepColumns = [
                'LEAD_SCN', 'S_PART', 'BUNDLE_SIZE', 'WIRE', 'TERM1', 'SEAL1',
                'TERM2', 'SEAL2', 'STRIP1', 'STRIP2', 'CCH_W1', 'ICH_W1',
                'CCH_W2', 'ICH_W2', 'T1NO', 'T2NO', 'W_LINK', 'WR_NO',
                'WIRE_NM', 'W_Diameter', 'W_Color', 'W_Length',
                'HOOK_RACK', 'SFTY_STK'
            ];
            return keepColumns.includes(header);
        }

        // Với MASTER_BOM import
        if (currentImportType === "MASTER_BOM") {
            const keepColumns = ['M_PART', 'S_PART', 'S_LR'];  // Thay đổi ở đây
            return keepColumns.includes(header);
        }
        return true;
    });

    let headerRow = $("<tr>");
    headers.forEach(header => {
        headerRow.append($("<th>").text(header));
    });

    thead.append(headerRow);
    table.append(thead);

    data.forEach((row, index) => {
        let tr = $("<tr>");

        headers.forEach(header => {
            let value = row[header];
            // Xử lý giá trị null/undefined
            if (value === null || value === undefined) {
                value = "";
            }
            tr.append($("<td>").text(value));
        });

        tbody.append(tr);
    });

    table.append(tbody);

    $("#DataGridView1").append(table);

    $("#DataGridView1 table").css({
        "width": "100%",
        "border-collapse": "collapse"
    });

    $("#DataGridView1 th").css({
        "background-color": "#f2f2f2",
        "text-align": "center",
        "padding": "8px",
        "border": "1px solid #ddd"
    });

    $("#DataGridView1 td").css({
        "text-align": "center",
        "padding": "8px",
        "border": "1px solid #ddd"
    });

    $("#DataGridView1 tr:nth-child(even)").css("background-color", "#f9f9f9");
}

function Buttonsave_Click() {
    if ($("#DataGridView1").find("tbody tr").length === 0) {
        alert("Không có dữ liệu để lưu. Vui lòng import dữ liệu trước.");
        return;
    }

    $("#BackGround").css("display", "block");

    let tableData = [];
    let headers = [];

    $("#DataGridView1 th").each(function () {
        headers.push($(this).text());
    });

    $("#DataGridView1 tbody tr").each(function () {
        let rowData = {};
        $(this).find("td").each(function (index) {
            let value = $(this).text().trim(); 
            rowData[headers[index]] = value === "" ? null : value;
        });
        tableData.push(rowData);
    });

    let BaseParameter = {
        RadioButton1: currentImportType === "MASTER",
        RadioButton2: currentImportType === "MASTER_BOM",
        DataGridView1: tableData
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C19_1/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");

            if (data.Success) {
                alert(data.Message || "정상처리 되었습니다. Đã được lưu.");
                $("#DataGridView1").empty();
            } else {
                alert(data.Error || "오류가 발생 하였습니다. Một lỗi đã xảy ra.");
            }
        }).catch(err => {
            $("#BackGround").css("display", "none");
            alert("오류가 발생 하였습니다. Một lỗi đã xảy ra. " + err.message);
        });
}

function Button1_Click() {
    $("#BackGround").css("display", "block");

    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C19_1/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");

            if (data.Success && data.DataGridView1) {
                displayServerData(data.DataGridView1);
            } else {
                alert(data.Error || "Không thể lấy dữ liệu từ server.");
            }
        }).catch(err => {
            $("#BackGround").css("display", "none");
            alert("Lỗi kết nối: " + err.message);
        });
}

function displayServerData(data) {
    $("#DataGridView1").empty();

    if (!data || data.length === 0) return;

    let table = $("<table>").addClass("responsive-table");
    let thead = $("<thead>").addClass("sticky-header");
    let tbody = $("<tbody>");

    let allProps = new Set();
    data.forEach(item => {
        Object.keys(item).forEach(key => {
            if (item[key] !== null && item[key] !== undefined && key !== "Error" && key !== "Success" && key !== "Message") {
                if (!key.includes("_EMPTY")) {
                    allProps.add(key);
                }
            }
        });
    });

    const columnsToShow = ["LEAD_SCN", "LEAD_PN", "WIRE", "TERM1", "SEAL1", "TERM2", "SEAL2"];

    const filteredProps = [...allProps].filter(prop => columnsToShow.includes(prop));

    let headerRow = $("<tr>");
    filteredProps.forEach(prop => {
        headerRow.append($("<th>").text(prop));
    });

    thead.append(headerRow);
    table.append(thead);

    data.forEach(item => {
        let tr = $("<tr>");

        filteredProps.forEach(prop => {
            let value = item[prop] !== null && item[prop] !== undefined ? item[prop] : "";
            tr.append($("<td>").text(value));
        });

        tbody.append(tr);
    });

    table.append(tbody);

    $("#DataGridView1").append(table);

    $("#DataGridView1 table").css({
        "width": "100%",
        "border-collapse": "collapse"
    });

    $("#DataGridView1 th").css({
        "background-color": "#f2f2f2",
        "text-align": "center",
        "padding": "8px",
        "border": "1px solid #ddd"
    });

    $("#DataGridView1 td").css({
        "text-align": "center",
        "padding": "8px",
        "border": "1px solid #ddd"
    });

    $("#DataGridView1 tr:nth-child(even)").css("background-color", "#f9f9f9");
}

function Buttonclose_Click() {
    window.close();
}