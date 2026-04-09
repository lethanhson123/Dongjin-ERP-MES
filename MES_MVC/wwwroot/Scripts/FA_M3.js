
// Biến toàn cục
let BaseResult;
let barcodeLengthByLine = {};
let table2;
//thiết lập giao diện cho table2
function setTableView() {
    table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "62vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: 1, orderable: false, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ],
        createdRow: function (row, data) {
            if (data[17] === "false") {
                $(row).addClass("red-text");
            }
        }
    });

    // Thêm min-height sau khi bảng đã khởi tạo, tùy vào kích thước mà điều chỉnh cho phù hợp màn hình
    $(".dataTables_scrollBody").css("min-height", "60vh");

    // Chọn tất cả các dòng
    $('#checkAll').on('click', function () {
        const isChecked = $(this).is(':checked');
        $('.row-check').prop('checked', isChecked);
    });

    // Nếu bỏ chọn từng dòng → cập nhật lại checkbox tổng
    $('#Table2 tbody').on('change', '.row-check', function () {
        const allChecked = $('.row-check').length === $('.row-check:checked').length;
        $('#checkAll').prop('checked', allChecked);
    });
}

// Gắn sự kiện khi trang tải xong
$(document).ready(function () {
    loadLines();
    setTableView();
    $("#txtScanCode").keypress(function (e) {
        if (e.which == 13) {
            e.preventDefault();
            Buttonsave_Click();
        }
    });
});

// Gắn sự kiện cho các button
$("#Buttonfind").click(function () { Buttonfind_Click(); });
$("#Buttonadd").click(function () { Buttonadd_Click(); });
$("#Buttonsave").click(function () { Buttonsave_Click(); });
$("#Buttondelete").click(function () { Buttondelete_Click(); });
$("#Buttoncancel").click(function () { Buttoncancel_Click(); });
$("#Buttoninport").click(function () { Buttoninport_Click(); });
$("#Buttonexport").click(function () { Buttonexport_Click(); });
$("#Buttonprint").click(function () { Buttonprint_Click(); });
$("#Buttonhelp").click(function () { Buttonhelp_Click(); });
$("#Buttonclose").click(function () { Buttonclose_Click(); });

// Sự kiện khi nhập mã barcode
$("#txtScanCode").on('input', function () {
    const barcode = $(this).val();
    if (barcode && checkBarcodeExistsInGrid(barcode)) {
        showToast("Barcode đã tồn tại trong lưới", true);
        $(this).val("").focus();
    }
});

// Sự kiện khi thay đổi Line
$("#cbLine").change(function () {
    $("#txtScanCode").val('').focus();
});

// Hàm load danh sách Line
function loadLines() {
    $("#BackGround").css("display", "block");
    fetch("/FA_M3/Load", { method: "POST" })
        .then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                if (data.Error) {
                    showToast(data.Error, true);
                    return;
                }
                $("#cbLine").empty();
                $("#ComboBox_Line option:not(:first)").remove();

                $("#cbLine").append('<option value="">All</option>');
                if (data.Data && data.Data.Lines) {
                    data.Data.Lines.forEach(function (line) {
                        $("#cbLine").append(`<option value="${line.Value}">${line.Text}</option>`);
                        $("#ComboBox_Line").append(`<option value="${line.Value}">${line.Text}</option>`);
                    });
                }
            }).catch((err) => {
                $("#BackGround").css("display", "none");
                showToast("Lỗi: " + err, true);
            });
        });
}

// Các hàm xử lý button
function Buttonfind_Click() {
    let BaseParameter = {};

    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            if (!validateInput(false)) return;

            BaseParameter = {
                ID: $("#cbLine").val(),
                Barcode: $("#txtScanCode").val(),
                TabSelected: "1"
            };
        } else if (currentTabId === 'Tag002') {
            BaseParameter = {
                ComboBox1: $("#ComboBox_Line").val(),
                FromDate: $("#tab2_fromDate").val(),
                ToDate: $("#tab2_toDate").val(),
                SearchString: $("#tab2_SearchText").val(),
                TabSelected: "2"
            };
        } else if (currentTabId === 'Tag003') {
            loadHourlyReport();
            return;
        }
    }

    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M3/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        return response.json();
    }).then((data) => {
        $("#BackGround").css("display", "none");
        if (data.Error) {
            showToast(data.Error, true);
            return;
        }
        if (currentTabId === 'Tag001') {
            displayDataInTable(data.FAProductionList);
        }
        else if (currentTabId === 'Tag002') {
            renderDataToTable2(data.ROTestLogList);
        }

    }).catch((err) => {
        $("#BackGround").css("display", "none");
        showToast("Lỗi: " + err, true);
    });
}

function renderDataToTable2(data) {

    $("#tab2_Total").text("0");
    $("#tab2_TotalRT").text("0");
    $("#tab2_TotalSP").text("0");
    $("#tab2_TotalActual").text("0");

    if (data && data.length > 0) {

        //khai báo biến lưu trữ duex liệu nhận về
        let rows = [];
        let stats = {
            total: 0,
            acttual: 0,
            RT: 0,
            SP:0
        };
        //thục hiện đẩy dữ liệu vào bảng
        data.forEach((rowData, i) => {

            //stats.uniqueSet.add(rowData.ID);
           stats.total++;
            if (rowData.Active) stats.acttual++;
            if (rowData.Retest === "RT") stats.RT++;
            if (rowData.Retest === "SP") stats.SP++;

            let addcheckbox = `<label><input type="checkbox" class="row-check" data-part="${rowData.ID}"><span></span></label>`;

            //khai báo 1 dong dữ liệu cần đẩy vào table
            const row = [
                i + 1,
                addcheckbox,
                FormatDateTime(rowData.DateTime),
                rowData.PartNumber || "",
                rowData.PartName || "",        
                rowData.LotNum || "",
                rowData.LineNumber ?? "",
                rowData.LineName || "",
                rowData.Remark || "",
                rowData.ALC || "",
                rowData.Retest || "",
                rowData.ECO || "",
                rowData.VER || "",
                rowData.ETC || "",
                rowData.ProgramVersion || "",
                rowData.PassCount || "",
                `<span class="preserve-space">${rowData.ScanBarcode || ""}</span>`,
                rowData.Active || "false",
                rowData.Note || "",
                rowData.Update_DTM || "",
                rowData.Update_User || "",

            ];
            //đẩy dòng dữ liệu vào danh sách giúp cải thiện hiệu năng
            rows.push(row);
        });

        //load dữ liệu lên table vói thư viện DataTable 
        if (table2) {
            table2.clear();
            table2.rows.add(rows);
            table2.draw();
        }

        $('#checkAll').prop('checked', false);
        // Gán thống kê
        $("#tab2_Total").text(stats.total);
        $("#tab2_TotalRT").text(stats.RT);
        $("#tab2_TotalSP").text(stats.SP);
        $("#tab2_TotalActual").text(stats.acttual);

      //  ShowMessage(localizedText.Success, "ok");
    } else {
       // ShowMessage(localizedText.Empty, "error");
        if (table2) {
            table2.clear();
            table2.draw();
        }
    }
}

function Buttonadd_Click() {
    $("#cbLine").val("");
    $("#txtScanCode").val("").focus();
    resetBarcodeValidation();
}

function Buttonsave_Click() {
    if (!validateInput(true)) return;

    const barcode = $("#txtScanCode").val();
    if (checkBarcodeExistsInGrid(barcode)) {
        showToast("Barcode đã tồn tại trong lưới", true);
        $("#txtScanCode").val("").focus();
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ComboBox1: $("#cbLine").val(),
        Barcode: barcode,
        USER_IDX: getCurrentUser()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M3/Buttonsave_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (data.Error) {
                showToast(data.Error, true);
                return;
            }
            showToast(data.Message || "Lưu dữ liệu thành công");
            $("#txtScanCode").val("").focus();
            Buttonfind_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            showToast("Lỗi: " + err, true);
        });
    });
}

function Buttondelete_Click() {

    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            if (!validateInput(false)) return;
            DeleteSelectedDatagrid1();
            
        } else if (currentTabId === 'Tag002') {
            DeleteHistoryItem();
        } else if (currentTabId === 'Tag003') {
           
        }
    }
       
}

// xóa dong dữ liệu đã chọn trong lưới Tab1
function DeleteSelectedDatagrid1() {
    let selectedRow = $("#DataGridView tr.selected");
    if (selectedRow.length === 0) {
        showToast("Vui lòng chọn một dòng để xóa", true);
        return;
    }

    let id = selectedRow.find("td:first").text();
    if (!confirm("Bạn có chắc chắn muốn xóa dòng này?")) {
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ID: id,
        USER_IDX: getCurrentUser(),
        TabName: '1'
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M3/Buttondelete_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (data.Error) {
                showToast(data.Error, true);
                return;
            }
            showToast(data.Message || "Xóa thành công");
            Buttonfind_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            showToast("Lỗi: " + err, true);
        });
    });
}

function getSelectedTable2IDs() {
    const selectedRows = [];

    $('.row-check:checked').each(function () {
        const row = $(this).closest('tr'); // Lấy dòng chứa checkbox
        const cells = row.find('td');      // Lấy tất cả ô trong dòng
        const id = $(this).data('part');   // Lấy ID thực từ thuộc tính data-part

        const rowData = {
            ID: id,
            DateTime: $(cells[2]).text().trim(),
            PartNumber: $(cells[3]).text().trim(),
            PartName: $(cells[4]).text().trim(),
            LotNum: $(cells[5]).text().trim(),
            LineNumber: $(cells[6]).text().trim(),
            LineName: $(cells[7]).text().trim(),
            Remark: $(cells[8]).text().trim(),
            ALC: $(cells[9]).text().trim(),
            Retest: $(cells[10]).text().trim(),
            ECO: $(cells[11]).text().trim(),
            VER: $(cells[12]).text().trim(),
            ETC: $(cells[13]).text().trim(),
            ProgramVersion: $(cells[14]).text().trim(),
            PassCount: $(cells[15]).text().trim()
        };

        selectedRows.push(rowData);
    });

    return selectedRows;
}

//xóa khỏi sản lượng tính toán của lịch sử Test RO
async function DeleteHistoryItem() {
    const selectedParts = getSelectedTable2IDs();

    if (selectedParts.length === 0) {
        M.toast({ html: "Chưa chọn dòng nào.", classes: "red" });
        return;
    }

    const message = `${localizedText.TotalDelete} = ${selectedParts.length} (EA)`;

    showConfirmDeleteAllModal(message, async function (confirmed) {
        if (!confirmed) {
            $("#BackGround").hide();
            return;
        }

        const remark = $("#inputConfirmDelete").val()?.trim() || "";
        if (!remark) {
            ShowMessage(`${localizedText.Empty}: ${localizedText.Remark}`, "error");
            return;
        }

        // Chuẩn hóa dữ liệu gửi đi
        const baseParameter = {
            DataGridView: selectedParts, 
            USER_ID: GetCookieValue("UserID"),
            REMARK: remark,
            Code: "DeleteAll",
            TabName: '2'
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(baseParameter));

        $("#BackGround").fadeIn();

        try {
            const response = await fetch("/FA_M3/Buttondelete_Click", {
                method: "POST",
                body: formUpload
            });

            const data = await response.json();

            if (data.ErrorNumber === 0) {
                ShowMessage(data.Message || "Đã xóa thành công", "ok");
                Buttonfind_Click();
            } else {
                ShowMessage(data.Error || "Xóa thất bại", "error");
            }
        } catch (err) {
            ShowMessage(err.message || err, "error");
        } finally {
            $("#BackGround").fadeOut();
        }
    });
}

function showConfirmDeleteAllModal(message, callback) {
    $('#confirmDeleteMessage').text(message);

    var modalElem = document.getElementById('confirmDeleteModal');
    var instance = M.Modal.getInstance(modalElem);
    instance.open();

    $('#btn_confirmDeleteAllYes').off('click').on('click', function () {
        instance.close();
        callback(true);
    });

    $('#btn_confirmDeleteAllCancel').off('click').on('click', function () {
        instance.close();
        callback(false);
    });
}


function Buttoncancel_Click() {
    Buttonadd_Click();
}

function Buttoninport_Click() {
    alert("Chức năng nhập dữ liệu chưa được triển khai");
}

function Buttonexport_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (!activeTab) return;

    var currentTabId = activeTab.getAttribute('href').replace('#', '');

    if (currentTabId === 'Tag001') {
        TableHTMLToExcel('DataGridViewTable', 'FAProduction_' + getTimestamp() + '.xls', 'FAProduction');
        showToast("Đã xuất file thành công");

    } else if (currentTabId === 'Tag002') {
        exportTab2ToExcel();

    } else if (currentTabId === 'Tag003') {
        TableHTMLToExcel('HourlyTable', 'HourlyProduction_' + getTimestamp() + '.xls', 'HourlyProduction');
        showToast("Đã xuất file thành công");
    } else {
        showToast("Chức năng xuất chưa được hỗ trợ cho tab này", true);
    }
}
function exportTab2ToExcel() {
    if (!table2 || table2.data().count() === 0) {
        showToast("Không có dữ liệu để xuất", true);
        return;
    }

    try {
        const allData = table2.rows().data().toArray();
        let html = '<table border="1">';
        html += '<thead><tr>';
        html += '<th>No</th>';
        html += '<th>DateTime</th>';
        html += '<th>PartNumber</th>';
        html += '<th>PartName</th>';
        html += '<th>LotNum</th>';
        html += '<th>LineNumber</th>';
        html += '<th>LineName</th>';
        html += '<th>Remark</th>';
        html += '<th>ALC</th>';
        html += '<th>Retest</th>';
        html += '<th>ECO</th>';
        html += '<th>VER</th>';
        html += '<th>ETC</th>';
        html += '<th>ProgramVersion</th>';
        html += '<th>PassCount</th>';
        html += '<th>ScanBarcode</th>';
        html += '<th>Active</th>';
        html += '<th>Note</th>';
        html += '<th>Update_DTM</th>';
        html += '<th>Update_User</th>';
        html += '</tr></thead>';

        // Body - Lặp qua TẤT CẢ dữ liệu
        html += '<tbody>';
        allData.forEach((row, index) => {
            html += '<tr>';
            html += '<td>' + (index + 1) + '</td>';                        
            html += '<td>' + (row[2] || '') + '</td>';                    
            html += '<td>' + escapeHtml(row[3] || '') + '</td>';         
            html += '<td>' + escapeHtml(row[4] || '') + '</td>';           
            html += '<td>' + escapeHtml(row[5] || '') + '</td>';           
            html += '<td>' + escapeHtml(row[6] || '') + '</td>';           
            html += '<td>' + escapeHtml(row[7] || '') + '</td>';         
            html += '<td>' + escapeHtml(row[8] || '') + '</td>';            
            html += '<td>' + escapeHtml(row[9] || '') + '</td>';           
            html += '<td>' + escapeHtml(row[10] || '') + '</td>';          
            html += '<td>' + escapeHtml(row[11] || '') + '</td>';          
            html += '<td>' + escapeHtml(row[12] || '') + '</td>';         
            html += '<td>' + escapeHtml(row[13] || '') + '</td>';         
            html += '<td>' + escapeHtml(row[14] || '') + '</td>';           
            html += '<td>' + escapeHtml(row[15] || '') + '</td>';      
            html += '<td>' + extractTextFromHTML(row[16]) + '</td>';       
            html += '<td>' + (row[17] || '') + '</td>';                  
            html += '<td>' + escapeHtml(row[18] || '') + '</td>';          
            html += '<td>' + (row[19] || '') + '</td>';                      
            html += '<td>' + escapeHtml(row[20] || '') + '</td>';         
            html += '</tr>';
        });
        html += '</tbody>';
        html += '</table>';

        // Export với Excel format
        const filename = 'ROTestLog_History_' + getTimestamp() + '.xls';
        exportToExcel(html, filename);

        showToast("Đã xuất " + allData.length + " dòng dữ liệu thành công");

    } catch (err) {
        console.error('Export error:', err);
        showToast("Lỗi khi xuất file: " + err.message, true);
    }
}
function exportToExcel(tableHtml, filename) {
    const uri = 'data:application/vnd.ms-excel;base64,';
    const template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" ' +
        'xmlns:x="urn:schemas-microsoft-com:office:excel" ' +
        'xmlns="http://www.w3.org/TR/REC-html40">' +
        '<head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>' +
        '<x:ExcelWorksheet><x:Name>ROTestLog</x:Name><x:WorksheetOptions>' +
        '<x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>' +
        '</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->' +
        '<meta http-equiv="content-type" content="text/plain; charset=UTF-8"/>' +
        '</head><body>{table}</body></html>';

    const base64 = function (s) {
        return window.btoa(unescape(encodeURIComponent(s)));
    };

    const format = function (s, c) {
        return s.replace(/{(\w+)}/g, function (m, p) {
            return c[p];
        });
    };

    const ctx = { table: tableHtml };
    const link = document.createElement('a');
    link.download = filename;
    link.href = uri + base64(format(template, ctx));
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function extractTextFromHTML(html) {
    if (!html) return '';
    const temp = document.createElement('div');
    temp.innerHTML = html;
    return escapeHtml(temp.textContent || temp.innerText || '');
}

function escapeHtml(text) {
    if (!text) return '';
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return String(text).replace(/[&<>"']/g, m => map[m]);
}

function getTimestamp() {
    const now = new Date();
    const pad = (n) => String(n).padStart(2, '0');
    return now.getFullYear() +
        pad(now.getMonth() + 1) +
        pad(now.getDate()) + '_' +
        pad(now.getHours()) +
        pad(now.getMinutes()) +
        pad(now.getSeconds());
}
function Buttonprint_Click() {
    alert("Chức năng in chưa được triển khai");
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

// Các hàm tiện ích
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

function showToast(message, isError = false) {
    let classes = isError ? 'red' : 'green';
    M.toast({ html: message, classes: classes, displayLength: 6000 });
    if (isError) {
        PlayErrorSound();
    } else {
        PlaySuccessSound();
    }
}

function displayDataInTable(data) {
    $("#DataGridView").empty();

    let recordCount = 0;
    if (data && data.length > 0) {
        recordCount = data.length;
    }
    $("#recordCount").text(`Tổng: ${recordCount}`);

    if (!data || data.length === 0) {
        $("#DataGridView").append('<tr><td colspan="7" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    data.forEach(function (item) {
        let createDate = item.CreateDate ? new Date(item.CreateDate).toLocaleString() : '';
        let updateDate = item.UpdateDate ? new Date(item.UpdateDate).toLocaleString() : '';

        $("#DataGridView").append(`
            <tr>
                <td>${item.ID}</td>
                <td>${item.Line || ''}</td>
                <td>${item.Barcode || ''}</td>
                <td>${createDate}</td>
                <td>${item.CreateUser || ''}</td>
                <td>${updateDate}</td>
                <td>${item.UpdateUser || ''}</td>
            </tr>
        `);
    });

    $("#DataGridView tr").click(function () {
        $("#DataGridView tr").removeClass("selected");
        $(this).addClass("selected");
    });
}

function validateInput(requireBoth) {
    let line = $("#cbLine").val();
    let barcode = $("#txtScanCode").val();

    if (requireBoth) {
        if (!line) {
            showToast("Vui lòng chọn Line", true);
            $("#cbLine").focus();
            return false;
        }

        if (!barcode) {
            showToast("Vui lòng nhập Barcode", true);
            $("#txtScanCode").focus();
            return false;
        }

        if (!validateBarcode(line, barcode)) {
            showToast(`Barcode không đúng độ dài cho Line ${line}. Yêu cầu ${barcodeLengthByLine[line]} ký tự.`, true);
            $("#txtScanCode").val('').focus();
            return false;
        }
    } else {
        if (!line && !barcode) {
            showToast("Vui lòng nhập ít nhất một điều kiện tìm kiếm", true);
            $("#cbLine").focus();
            return false;
        }
    }

    return true;
}

function validateBarcode(line, barcode) {
    if (!barcodeLengthByLine[line]) {
        barcodeLengthByLine[line] = barcode.length;
        console.log(`Đã lưu độ dài chuẩn cho Line ${line}: ${barcode.length} ký tự`);
        return true;
    }

    const isValid = barcode.length === barcodeLengthByLine[line];
    if (!isValid) {
        console.log(`Barcode không hợp lệ: có ${barcode.length} ký tự, cần ${barcodeLengthByLine[line]} ký tự`);
    }
    return isValid;
}

function checkBarcodeExistsInGrid(barcode) {
    let rows = $("#DataGridView tr");
    for (let i = 0; i < rows.length; i++) {
        let cellBarcode = $(rows[i]).find("td:eq(2)").text();
        if (cellBarcode.trim() === barcode.trim()) {
            return true;
        }
    }
    return false;
}

function resetBarcodeValidation() {
    barcodeLengthByLine = {};
}
function PlaySuccessSound() {
    try {
        let audio = new Audio('/audio/HOOK_S.mp3');
        audio.play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}
function PlayErrorSound() {
    try {
        let audio = new Audio('/audio/Sash_brk.mp3');
        audio.play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
}
function loadHourlyReport() {
    let BaseParameter = {
        FromDate: $("#tab3_fromDate").val(),
        ToDate: $("#tab3_toDate").val()
    };

    $("#BackGround").show();

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M3/GetHourlyProduction", {
        method: "POST",
        body: formUpload
    })
        .then(res => res.json())
        .then(data => {
            $("#BackGround").hide();
            if (data.Error) {
                showToast(data.Error, true);
                return;
            }
            renderHourlyTable(data.Data);
        })
        .catch(err => {
            $("#BackGround").hide();
            showToast("Lỗi: " + err, true);
        });
}
function renderHourlyTable(data) {
    if (!data || data.length === 0) {
        $("#hourlyBody").html('<tr><td colspan="100">Không có dữ liệu</td></tr>');
        return;
    }

    // Lấy danh sách Line unique
    let lines = [...new Set(data.map(x => x.LineName))].sort();

    // Tạo header
    let headerHtml = '<th>HOUR</th>';
    lines.forEach(line => {
        headerHtml += `<th>${line}</th>`;
    });
    headerHtml += '<th class="blue white-text">Total</th>';
    $("#hourlyHeader").html(headerHtml);

    // Khởi tạo mảng tổng cho mỗi Line
    let lineTotals = {};
    lines.forEach(line => {
        lineTotals[line] = 0;
    });
    let grandTotal = 0;

    // Tạo body
    let bodyHtml = '';
    for (let h = 6; h <= 21; h++) {
        let rowClass = (h >= 14 && h <= 16) ? 'green lighten-4' : '';
        bodyHtml += `<tr class="${rowClass}">`;
        bodyHtml += `<td><b>${h}~${h + 1}</b></td>`;

        let hourTotal = 0;
        lines.forEach(line => {
            let item = data.find(x => x.Hour === h && x.LineName === line);
            let count = item ? item.Count : 0;
            hourTotal += count;
            lineTotals[line] += count; // Cộng dồn cho mỗi Line
            bodyHtml += `<td>${count || ''}</td>`;
        });

        grandTotal += hourTotal;
        bodyHtml += `<td class="blue white-text"><b>${hourTotal}</b></td>`;
        bodyHtml += '</tr>';
    }

    // ⭐ Thêm dòng SUM ở cuối
    bodyHtml += '<tr style="font-weight: bold; background: #ffeb3b;">';
    bodyHtml += '<td>SUM</td>';

    lines.forEach(line => {
        bodyHtml += `<td>${lineTotals[line]}</td>`;
    });

    bodyHtml += `<td class="blue white-text">${grandTotal}</td>`;
    bodyHtml += '</tr>';

    $("#hourlyBody").html(bodyHtml);
    showToast("Đã load dữ liệu");
}