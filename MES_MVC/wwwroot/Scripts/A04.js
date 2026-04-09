let BaseResult = { DataGridView1: [], DataGridView6: [] };
let sortState = {
    DataGridView1: { column: null, direction: 'asc' },
    DataGridView6: { column: null, direction: 'asc' }
};

$(document).ready(function () {
    $('.tabs').tabs();
    $('select').formSelect();
    LoadComboboxes();

    // Giới hạn nhập số cho TB5 và app03
    $("#TB5, #app03").on("keypress", function (e) {
        if (!/[0-9]/.test(e.key) && e.key !== "\b") {
            e.preventDefault();
        }
    });

    // Gọi Buttonfind_Click khi nhấn Enter trên STB1, STB3
    $("#STB1, #STB3").on("keydown", function (e) {
        if (e.key === "Enter") {
            Buttonfind_Click();
        }
    });

    // Sự kiện nhấp chuột cho tiêu đề DataGridView1
    $("#DataGridView1Table thead th").on("click", function () {
        const columnIndex = $(this).index();
        const columns = [
            'CHK', 'APPLICATOR', 'SEQ', 'CD_SYS_NOTE', 'MAX_CNT', 'TOT_WK_CNT', 'WK_CNT', 'SPP_NO', 'TYPE', 'GAUGE',
            'COPL_NOR', 'COPL_SPE', 'INSPL_SEALTYPE', 'INSPL_NONSEAL', 'INSPL_XTYPE', 'INSPL_KTYPE', 'INSPL_SPE',
            'ANVIL_NOR', 'ANVIL_SPE', 'CMU_NOR', 'CMU_SPE', 'IMU_NOR', 'IMU_NONSEAL', 'IMU_SPE',
            'CUTPL_ONE', 'CUTPL_DET', 'CUTAN_ONE', 'CUTAN_DET', 'CUTHO_ONE', 'CUTHO_DET',
            'RRBLK_ONE', 'RRBLK_DET', 'RRCUTHO_ONE', 'RRCUTHO_DET', 'FRCUTHO_ONE', 'FRCUTHO_DET',
            'RRCUTAN_ONE', 'RRCUTAN_DET', 'WRDN_ONE', 'WRDN_DET', 'COMB_CODE', 'DESC'
        ];
        const column = columns[columnIndex];
        if (column !== 'CHK') {
            sortTable('DataGridView1', column, BaseResult.DataGridView1, RenderDataGridView1);
        }
    });

    // Sự kiện nhấp chuột cho tiêu đề DataGridView6
    $("#DataGridView6Table thead th").on("click", function () {
        const columnIndex = $(this).index();
        const columns = [
            'APPLICATOR', 'SEQ', 'WORK_DTM', 'WK_QTY', 'TOT_QTY', 'CONTENT', 'CREATE_DTM', 'CREATE_USER'
        ];
        const column = columns[columnIndex];
        sortTable('DataGridView6', column, BaseResult.DataGridView6, RenderDataGridView6);
    });

    // Gắn sự kiện cho các nút
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
    $("#Button1").click(Button1_Click);
    $("#Button2").click(Button2_Click);
    $("#Button3").click(Button3_Click);
});

function sortTable(tableId, column, dataArray, renderFunction) {
    if (!dataArray || !Array.isArray(dataArray)) {
        console.warn(`Không có dữ liệu để sắp xếp cho ${tableId}`);
        return;
    }

    // Cập nhật trạng thái sắp xếp
    if (sortState[tableId].column === column) {
        sortState[tableId].direction = sortState[tableId].direction === 'asc' ? 'desc' : 'asc';
    } else {
        sortState[tableId].column = column;
        sortState[tableId].direction = 'asc';
    }

    // Sắp xếp mảng dữ liệu
    dataArray.sort((a, b) => {
        let valA = a[column] || '';
        let valB = b[column] || '';

        // Xử lý kiểu số cho các cột số
        if (['MAX_CNT', 'TOT_WK_CNT', 'WK_CNT', 'WK_QTY', 'TOT_QTY'].includes(column)) {
            valA = parseFloat(valA) || 0;
            valB = parseFloat(valB) || 0;
        } else {
            valA = valA.toString().toLowerCase();
            valB = valB.toString().toLowerCase();
        }

        if (valA < valB) return sortState[tableId].direction === 'asc' ? -1 : 1;
        if (valA > valB) return sortState[tableId].direction === 'asc' ? 1 : -1;
        return 0;
    });

    // Gọi hàm render tương ứng
    if (tableId === 'DataGridView1') {
        renderFunction();
    } else if (tableId === 'DataGridView6') {
        renderFunction({ DataGridView6: dataArray });
    }
}

function GetCookieValue(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}

function LoadComboboxes() {
    $("#BackGround").css("display", "block");
    let url = "/A04/Load";

    fetch(url, {
        method: "POST",
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        BaseResult = data;
        console.log("BaseResult:", BaseResult);
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#STB2").empty().append('<option value="ALL">ALL</option>');
            $("#TB4").empty();
            if (Array.isArray(BaseResult.Listtscode)) {
                BaseResult.Listtscode.forEach(item => {
                    $("#STB2").append(`<option value="${item.CD_SYS_NOTE}">${item.CD_SYS_NOTE}</option>`);
                    $("#TB4").append(`<option value="${item.CD_SYS_NOTE}">${item.CD_SYS_NOTE}</option>`);
                });
                $("#STB2").val("ALL").formSelect();
                $("#TB4").val(BaseResult.Listtscode[0]?.CD_SYS_NOTE || "").formSelect();
            } else {
                console.error("Listtscode không phải mảng:", BaseResult.Listtscode);
                alert("Không thể tải danh sách khách hàng. Dữ liệu không hợp lệ.");
            }
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        console.error("Lỗi fetch:", err);
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    $("#app02").prop("disabled", true);
    $("#app03").prop("readonly", true);
    $("#TB2").prop("readonly", true);

    let BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    if (!BaseParameter.USER_IDX) {
        alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
        $("#BackGround").css("display", "none");
        return;
    }
    BaseParameter.ListSearchString.push($('#STB1').val() || "");
    BaseParameter.ListSearchString.push($('#STB2').val() || "");
    BaseParameter.ListSearchString.push($('#STB3').val() || "");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        BaseResult = data;
        if (data.Error) {
            alert(data.Error);
        } else {
            RenderDataGridView1();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    if (!BaseParameter.USER_IDX) {
        alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
        $("#BackGround").css("display", "none");
        return;
    }

    BaseParameter.ListSearchString.push($('#TB1').val() || "");
    BaseParameter.ListSearchString.push($('#TB2').val() || "");
    BaseParameter.ListSearchString.push($('#TB4').val() || "");
    BaseParameter.ListSearchString.push($('#TB5').val() || "0");
    BaseParameter.ListSearchString.push($('#TB8').val() || "");
    BaseParameter.ListSearchString.push($('#TB9').val() || "");
    BaseParameter.ListSearchString.push($('#TB10').val() || "");
    BaseParameter.ListSearchString.push($('#TB11').val() || "");
    BaseParameter.ListSearchString.push($('#TB12').val() || "");
    BaseParameter.ListSearchString.push($('#TB13').val() || "");
    BaseParameter.ListSearchString.push($('#TB14').val() || "");
    BaseParameter.ListSearchString.push($('#TB15').val() || "");
    BaseParameter.ListSearchString.push($('#TB16').val() || "");
    BaseParameter.ListSearchString.push($('#TB17').val() || "");
    BaseParameter.ListSearchString.push($('#TB18').val() || "");
    BaseParameter.ListSearchString.push($('#TB19').val() || "");
    BaseParameter.ListSearchString.push($('#TB20').val() || "");
    BaseParameter.ListSearchString.push($('#TB21').val() || "");
    BaseParameter.ListSearchString.push($('#TB22').val() || "");
    BaseParameter.ListSearchString.push($('#TB23').val() || "");
    BaseParameter.ListSearchString.push($('#TB24').val() || "");
    BaseParameter.ListSearchString.push($('#TB25').val() || "");
    BaseParameter.ListSearchString.push($('#TB26').val() || "");
    BaseParameter.ListSearchString.push($('#TB27').val() || "");
    BaseParameter.ListSearchString.push($('#TB28').val() || "");
    BaseParameter.ListSearchString.push($('#TB29').val() || "");
    BaseParameter.ListSearchString.push($('#TB30').val() || "");
    BaseParameter.ListSearchString.push($('#TB31').val() || "");
    BaseParameter.ListSearchString.push($('#TB32').val() || "");
    BaseParameter.ListSearchString.push($('#TB33').val() || "");
    BaseParameter.ListSearchString.push($('#TB34').val() || "");
    BaseParameter.ListSearchString.push($('#TB35').val() || "");
    BaseParameter.ListSearchString.push($('#TB36').val() || "");
    BaseParameter.ListSearchString.push($('#TB37').val() || "");
    BaseParameter.ListSearchString.push($('#TB38').val() || "");
    BaseParameter.ListSearchString.push($('#TB39').val() || "");
    BaseParameter.ListSearchString.push($('#TB40').val() || "");
    BaseParameter.ListSearchString.push($('#TB41').val() || "");
    BaseParameter.ListSearchString.push($('#TB42').val() || "");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
            $("#TB2").prop("readonly", true);
        } else {
            alert(data.Message || "Lưu dữ liệu thành công.");
            $("#app02").prop("disabled", true);
            $("#app03").prop("readonly", true);
            $("#TB2").prop("readonly", true);
            Buttonfind_Click();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#TB2").prop("readonly", true);
        $("#BackGround").css("display", "none");
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#TB1").val("New");
            $("#TB2").prop("readonly", false);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#TB1, #TB2, #TB4, #TB5, #TB8, #TB9, #TB10, #TB11, #TB12, #TB13, #TB14, #TB15, #TB16, #TB17, #TB18, #TB19, #TB20, #TB21, #TB22, #TB23, #TB24, #TB25, #TB26, #TB27, #TB28, #TB29, #TB30, #TB31, #TB32, #TB33, #TB34, #TB35, #TB36, #TB37, #TB38, #TB39, #TB40, #TB41, #TB42, #app01, #app04").val("");
            $("#TB2").prop("readonly", true);
            $("#app02").prop("disabled", true);
            $("#app03").prop("readonly", true);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    if (!BaseParameter.USER_IDX) {
        alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
        $("#BackGround").css("display", "none");
        return;
    }
    BaseParameter.ListSearchString.push($('#STB1').val() || "");
    BaseParameter.ListSearchString.push($('#STB2').val() || "");
    BaseParameter.ListSearchString.push($('#STB3').val() || "");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            window.open(data.Code, "_blank");
            alert(data.Message || "Xuất Excel thành công.");
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonhelp_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonhelp_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            OpenWindowByURL(data.Code, 800, 460);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Buttonclose_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Buttonclose_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            history.back();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Button1_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    if (!BaseParameter.USER_IDX) {
        alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
        $("#BackGround").css("display", "none");
        return;
    }
    BaseParameter.ListSearchString.push($('#app01').val() || "");
    BaseParameter.ListSearchString.push($('#TB2').val() || "");
    BaseParameter.ListSearchString.push($('#app02').val() || "");
    BaseParameter.ListSearchString.push($('#app03').val() || "0");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
            Button3_Click();
        } else {
            alert(data.Message || "Lưu dữ liệu thành công.");
            Button3_Click();
            Buttonfind_Click();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Button2_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    if (!BaseParameter.USER_IDX) {
        alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
        $("#BackGround").css("display", "none");
        return;
    }
    BaseParameter.ListSearchString.push($('#TB1').val() || "");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#app01").val("New");
            $("#app02").val("A").prop("disabled", false);
            $("#app03").val("").prop("readonly", false);
            $("#app04").val("");
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function Button3_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = { ListSearchString: [] };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A04/Button3_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Lỗi server: ${response.status} - ${text}`);
            });
        }
        return response.json();
    }).then(data => {
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#app01, #app04").val("");
            $("#app02").prop("disabled", true);
            $("#app03").prop("readonly", true);
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert(`Lỗi: ${err.message}`);
        $("#BackGround").css("display", "none");
    });
}

function RenderDataGridView1() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        DataGridView1_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let item = BaseResult.DataGridView1[i];
            HTML += `<tr onclick='DataGridView1_SelectionChanged(${i})'>`;
            HTML += `<td><input type="checkbox" id="DataGridView1CHK${i}" class="form-check-input" ${item.CHK ? 'checked' : ''}></td>`;
            HTML += `<td>${item.APPLICATOR || ''}</td>`;
            HTML += `<td>${item.SEQ || ''}</td>`;
            HTML += `<td>${item.CD_SYS_NOTE || ''}</td>`;
            HTML += `<td>${item.MAX_CNT || ''}</td>`;
            HTML += `<td>${item.TOT_WK_CNT || ''}</td>`;
            HTML += `<td>${item.WK_CNT || ''}</td>`;
            HTML += `<td>${item.SPP_NO || ''}</td>`;
            HTML += `<td>${item.TYPE || ''}</td>`;
            HTML += `<td>${item.GAUGE || ''}</td>`;
            HTML += `<td>${item.COPL_NOR || ''}</td>`;
            HTML += `<td>${item.COPL_SPE || ''}</td>`;
            HTML += `<td>${item.INSPL_SEALTYPE || ''}</td>`;
            HTML += `<td>${item.INSPL_NONSEAL || ''}</td>`;
            HTML += `<td>${item.INSPL_XTYPE || ''}</td>`;
            HTML += `<td>${item.INSPL_KTYPE || ''}</td>`;
            HTML += `<td>${item.INSPL_SPE || ''}</td>`;
            HTML += `<td>${item.ANVIL_NOR || ''}</td>`;
            HTML += `<td>${item.ANVIL_SPE || ''}</td>`;
            HTML += `<td>${item.CMU_NOR || ''}</td>`;
            HTML += `<td>${item.CMU_SPE || ''}</td>`;
            HTML += `<td>${item.IMU_NOR || ''}</td>`;
            HTML += `<td>${item.IMU_NONSEAL || ''}</td>`;
            HTML += `<td>${item.IMU_SPE || ''}</td>`;
            HTML += `<td>${item.CUTPL_ONE || ''}</td>`;
            HTML += `<td>${item.CUTPL_DET || ''}</td>`;
            HTML += `<td>${item.CUTAN_ONE || ''}</td>`;
            HTML += `<td>${item.CUTAN_DET || ''}</td>`;
            HTML += `<td>${item.CUTHO_ONE || ''}</td>`;
            HTML += `<td>${item.CUTHO_DET || ''}</td>`;
            HTML += `<td>${item.RRBLK_ONE || ''}</td>`;
            HTML += `<td>${item.RRBLK_DET || ''}</td>`;
            HTML += `<td>${item.RRCUTHO_ONE || ''}</td>`;
            HTML += `<td>${item.RRCUTHO_DET || ''}</td>`;
            HTML += `<td>${item.FRCUTHO_ONE || ''}</td>`;
            HTML += `<td>${item.FRCUTHO_DET || ''}</td>`;
            HTML += `<td>${item.RRCUTAN_ONE || ''}</td>`;
            HTML += `<td>${item.RRCUTAN_DET || ''}</td>`;
            HTML += `<td>${item.WRDN_ONE || ''}</td>`;
            HTML += `<td>${item.WRDN_DET || ''}</td>`;
            HTML += `<td>${item.COMB_CODE || ''}</td>`;
            HTML += `<td>${item.DESC || ''}</td>`;
            HTML += `</tr>`;
        }
        HTML += "</tbody>";
    } else {
        HTML += "<tbody></tbody>";
    }
    document.getElementById("DataGridView1").innerHTML = HTML;

    // Cập nhật tiêu đề để hiển thị hướng sắp xếp
    $("#DataGridView1Table thead th").each(function () {
        const columnIndex = $(this).index();
        const columns = [
            'CHK', 'APPLICATOR', 'SEQ', 'CD_SYS_NOTE', 'MAX_CNT', 'TOT_WK_CNT', 'WK_CNT', 'SPP_NO', 'TYPE', 'GAUGE',
            'COPL_NOR', 'COPL_SPE', 'INSPL_SEALTYPE', 'INSPL_NONSEAL', 'INSPL_XTYPE', 'INSPL_KTYPE', 'INSPL_SPE',
            'ANVIL_NOR', 'ANVIL_SPE', 'CMU_NOR', 'CMU_SPE', 'IMU_NOR', 'IMU_NONSEAL', 'IMU_SPE',
            'CUTPL_ONE', 'CUTPL_DET', 'CUTAN_ONE', 'CUTAN_DET', 'CUTHO_ONE', 'CUTHO_DET',
            'RRBLK_ONE', 'RRBLK_DET', 'RRCUTHO_ONE', 'RRCUTHO_DET', 'FRCUTHO_ONE', 'FRCUTHO_DET',
            'RRCUTAN_ONE', 'RRCUTAN_DET', 'WRDN_ONE', 'WRDN_DET', 'COMB_CODE', 'DESC'
        ];
        const column = columns[columnIndex];
        let text = $(this).text().replace(/ [↑↓]$/, '');
        if (column === sortState.DataGridView1.column) {
            text += sortState.DataGridView1.direction === 'asc' ? ' ↑' : ' ↓';
        }
        $(this).text(text);
    });
}

function DataGridView1_SelectionChanged(index) {
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > index) {
        let item = BaseResult.DataGridView1[index];
        $("#TB1").val(item.TOOL_IDX || '');
        $("#TB2").val(item.APPLICATOR || '');
        $("#TB4").val(item.CD_SYS_NOTE || '').formSelect();
        $("#TB5").val(item.MAX_CNT || '');
        $("#TB8").val(item.SPP_NO || '');
        $("#TB9").val(item.TYPE || '');
        $("#TB10").val(item.GAUGE || '');
        $("#TB11").val(item.COPL_NOR || '');
        $("#TB12").val(item.COPL_SPE || '');
        $("#TB13").val(item.INSPL_SEALTYPE || '');
        $("#TB14").val(item.INSPL_NONSEAL || '');
        $("#TB15").val(item.INSPL_XTYPE || '');
        $("#TB16").val(item.INSPL_KTYPE || '');
        $("#TB17").val(item.INSPL_SPE || '');
        $("#TB18").val(item.ANVIL_NOR || '');
        $("#TB19").val(item.ANVIL_SPE || '');
        $("#TB20").val(item.CMU_NOR || '');
        $("#TB21").val(item.CMU_SPE || '');
        $("#TB22").val(item.IMU_NOR || '');
        $("#TB23").val(item.IMU_NONSEAL || '');
        $("#TB24").val(item.IMU_SPE || '');
        $("#TB25").val(item.CUTPL_ONE || '');
        $("#TB26").val(item.CUTPL_DET || '');
        $("#TB27").val(item.CUTAN_ONE || '');
        $("#TB28").val(item.CUTAN_DET || '');
        $("#TB29").val(item.CUTHO_ONE || '');
        $("#TB30").val(item.CUTHO_DET || '');
        $("#TB31").val(item.RRBLK_ONE || '');
        $("#TB32").val(item.RRBLK_DET || '');
        $("#TB33").val(item.RRCUTHO_ONE || '');
        $("#TB34").val(item.RRCUTHO_DET || '');
        $("#TB35").val(item.FRCUTHO_ONE || '');
        $("#TB36").val(item.FRCUTHO_DET || '');
        $("#TB37").val(item.RRCUTAN_ONE || '');
        $("#TB38").val(item.RRCUTAN_DET || '');
        $("#TB39").val(item.WRDN_ONE || '');
        $("#TB40").val(item.WRDN_DET || '');
        $("#TB41").val(item.COMB_CODE || '');
        $("#TB42").val(item.DESC || '');
        $("#app01").val(item.APPLICATOR || '');

        // Load TTOOLHISTORY cho DataGridView6
        let BaseParameter = {
            ListSearchString: [item.TOOL_IDX || ""],
            USER_IDX: GetCookieValue("USER_IDX")
        };
        if (!BaseParameter.USER_IDX) {
            alert("Không tìm thấy USER_IDX. Vui lòng đăng nhập lại.");
            return;
        }

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/A04/LoadToolHistory";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then(response => {
            if (!response.ok) {
                return response.text().then(text => {
                    throw new Error(`Lỗi server: ${response.status} - ${text}`);
                });
            }
            return response.json();
        }).then(data => {
            if (data.Error) {
                alert(data.Error);
            } else {
                RenderDataGridView6(data);
            }
        }).catch(err => {
            alert(`Lỗi: ${err.message}`);
        });
    } else {
        Buttoncancel_Click();
    }
}

function RenderDataGridView6(data) {
    BaseResult.DataGridView6 = data.DataGridView6 || [];

    let HTML = "";
    if (data && data.DataGridView6 && Array.isArray(data.DataGridView6)) {
        HTML += "<tbody>";
        for (let i = 0; i < data.DataGridView6.length; i++) {
            let item = data.DataGridView6[i];
            HTML += "<tr>";
            HTML += `<td>${item.APPLICATOR || ''}</td>`;
            HTML += `<td>${item.SEQ || ''}</td>`;
            HTML += `<td>${item.WORK_DTM || ''}</td>`;
            HTML += `<td>${item.WK_QTY || ''}</td>`;
            HTML += `<td>${item.TOT_QTY || ''}</td>`;
            HTML += `<td>${item.CONTENT || ''}</td>`;
            HTML += `<td>${item.CREATE_DTM || ''}</td>`;
            HTML += `<td>${item.CREATE_USER || ''}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    } else {
        HTML += "<tbody></tbody>";
    }
    document.getElementById("DataGridView6Body").innerHTML = HTML;

    // Cập nhật tiêu đề để hiển thị hướng sắp xếp
    $("#DataGridView6Table thead th").each(function () {
        const columnIndex = $(this).index();
        const columns = [
            'APPLICATOR', 'SEQ', 'WORK_DTM', 'WK_QTY', 'TOT_QTY', 'CONTENT', 'CREATE_DTM', 'CREATE_USER'
        ];
        const column = columns[columnIndex];
        let text = $(this).text().replace(/ [↑↓]$/, '');
        if (column === sortState.DataGridView6.column) {
            text += sortState.DataGridView6.direction === 'asc' ? ' ↑' : ' ↓';
        }
        $(this).text(text);
    });
}

function OpenWindowByURL(url, width, height) {
    window.open(url, '_blank', `width=${width},height=${height}`);
}