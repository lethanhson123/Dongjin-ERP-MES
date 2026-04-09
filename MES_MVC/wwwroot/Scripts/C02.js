let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let SHIELDWIRE_CHK = false;
let MCLIST_CHK = false;
let CHK_MC;
let APIUpload;

let mainGridView;

//thiết lập hiên thị cho main table trên view
function installMainGridView() {
    mainGridView = $("#C02_MainTable").DataTable({
        scrollX: true,
        scrollY: "42vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 500,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: true,
        fixedColumns: {
            leftColumns: 8 // số cột muốn đóng khung
        },

        columnDefs: [
            { targets: 1, orderable: false, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" },
            { targets: 36, width: "auto", className: "dt-nowrap dt-spst" } // SPST auto-fit
        ],
    });

    // Thêm min-height sau khi bảng đã khởi tạo, tùy vào kích thước mà điều chỉnh cho phù hợp màn hình
    $(".dataTables_scrollBody").css("min-height", "60vh");

    // Chọn tất cả các dòng
    $('#mainTableCheckAll').on('click', function () {
        const isChecked = $(this).is(':checked');
        $('.row-check').prop('checked', isChecked);
    });

    // Nếu bỏ chọn từng dòng → cập nhật lại checkbox tổng
    $('#C02_MainTable tbody').on('change', '.row-check', function () {
        const allChecked = $('.row-check').length === $('.row-check:checked').length;
        $('#mainTableCheckAll').prop('checked', allChecked);
    });
}


window.addEventListener('beforeunload', function (event) {
    C02_STOP_Button1_Click();
});

window.addEventListener('load', function () {
    const navigationEntry = performance.getEntriesByType('navigation')[0];
    if (navigationEntry && navigationEntry.type === 'reload') {
        C02_STOP_Button1_Click();
    }
});


$(document).ready(function () {
    installMainGridView();
    APIUpload = decodeURIComponent(GetCookieValue("APIUpload"));

    $(".modal").modal({
        dismissible: false
    });
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    DateNow = today;
    $("#DateTimePicker1").val(DateNow);

    Lan_Change();

    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MCbox").val(SettingsMC_NM);
    C02_STOP_PageLoad
    localStorage.setItem("C02_MCbox", $("#MCbox").val());

    MC_LIST(0);

    DB_LISECHK();

    TS_USER(0);

    SHIELDWIRE_CHK = false;

    PageLoad();

    MCLIST_CHK = false

    SetMCLIST_CHK();

    document.getElementById("rbchk2").checked = true;

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];

    $('#C02_STOP_MaintenanceModal').modal({
        dismissible: false
    });

    $("#C02_STOP_MaintenanceSaveBtn").click(function () {
        C02_STOP_SaveMaintenanceHistory();
    });
    let trolleyTimer = null;

    $("#C02_TROLLEY_INPUT").on("input", function () {
        clearTimeout(trolleyTimer);
        let val = $(this).val();
        trolleyTimer = setTimeout(function () {
            C02_SCAN_TROLLEY_Validate(val);
        }, 500);
    });

    $("#C02_TROLLEY_INPUT").on("keypress", function (e) {
        if (e.which === 13) {
            clearTimeout(trolleyTimer);
            C02_SCAN_TROLLEY_Validate($(this).val());
        }
    });

    $("#C02_TROLLEY_BTN_CONFIRM").click(C02_SCAN_TROLLEY_Confirm);
    $("#C02_TROLLEY_BTN_SKIP").click(C02_SCAN_TROLLEY_Skip);
});
function Lan_Change() {
    Form_Label2();
}
function Form_Label2() {
    let CODE_S1 = "S";
    let CODE_I1 = "I";
    let CODE_Q1 = "Q";
    let CODE_M1 = "M"; 
    let CODE_T1 = "T";
    let CODE_L1 = "L";
    let CODE_E1 = "E";

    let CODE_S0 = $("#CODE_S").val() + " (" + CODE_S1 + ")";
    let CODE_I0 = $("#CODE_I").val() + " (" + CODE_I1 + ")";
    let CODE_Q0 = $("#CODE_Q").val() + " (" + CODE_Q1 + ")";
    let CODE_M0 = $("#CODE_M").val() + " (" + CODE_M1 + ")";
    let CODE_T0 = $("#CODE_T").val() + " (" + CODE_T1 + ")";
    let CODE_L0 = $("#CODE_L").val() + " (" + CODE_L1 + ")";
    let CODE_E0 = $("#CODE_E").val() + " (" + CODE_E1 + ")";

    $("#ComboBox3").empty();

    var ComboBox3 = document.getElementById("ComboBox3");

    var option = document.createElement("option");
    option.text = CODE_S0;
    option.value = CODE_S0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_I0;
    option.value = CODE_I0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_Q0;
    option.value = CODE_Q0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_M0;
    option.value = CODE_M0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_T0;
    option.value = CODE_T0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_L0;
    option.value = CODE_L0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_E0;
    option.value = CODE_E0;
    ComboBox3.add(option);

}

//cập nhật thông tin máy cắt và thời gian àm việc
function MC_LIST(Flag) {
    let MC_CHKE = $("#MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MC_CHKE,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02/MC_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultMC_LIST = data;
            BaseResult.ComboBox1 = BaseResultMC_LIST.ComboBox1;

            $("#ComboBox1").empty();
            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
                var option = document.createElement("option");
                option.text = BaseResult.ComboBox1[i].MC;
                option.value = BaseResult.ComboBox1[i].MC;

                var ComboBox1 = document.getElementById("ComboBox1");
                ComboBox1.add(option);
            }

            if (Flag == 1) {
                $("#ComboBox1").val(CHK_MC);
                Buttonfind_Click();
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//cập nhật và đong đơn vượt quá 11 ngày cho Cutting
function DB_LISECHK() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02/DB_LISECHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//cập nhật thông tin người dùng và thời gian vận hành ghi log đăng nhập của người dùng
function TS_USER(Flag) {
    let MCbox = $("#MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02/TS_USER";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {

            BaseResult.USER_TIME = data.USER_TIME;
            if (BaseResult.USER_TIME) {
                if (BaseResult.USER_TIME.length > 0) {
                    ToolStripLOGIDX = BaseResult.USER_TIME[0].TUSER_IDX;
                    localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
                }
            }
            BaseResult.USER_TIME1 = data.USER_TIME1;
            if (BaseResult.USER_TIME1) {
                if (BaseResult.USER_TIME1.length > 0) {

                    $("#C02_START_V2_BI_STIME").val(BaseResult.USER_TIME1[0].Name);
                    $("#C02_START_V2_Label70").val(BaseResult.USER_TIME1[0].Description);
                }
            }
            if (Flag == 1) {
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

/// load thong tin trang khi giao diện hiển thị lần đầu tiên
function PageLoad() {
    let MCbox = $("#MCbox").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02/PageLoad";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.Listtsnon_oper) {
                if (data.Listtsnon_oper.length > 0) {
                    let dtIDX = data.Listtsnon_oper[0].TSNON_OPER_IDX;
                    let dtCode = data.Listtsnon_oper[0].TSNON_OPER_CODE;
                    let MC = data.Listtsnon_oper[0].TSNON_OPER_MCNM;
                    let startTime = FormatDateTime(data.Listtsnon_oper[0].TSNON_OPER_STIME);

                    C02_STOP_ShowDowntime(dtIDX, dtCode, MC, startTime);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function SetMCLIST_CHK() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02/SetMCLIST_CHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSetMCLIST_CHK = data;
            if (BaseResultSetMCLIST_CHK.DGV_C02_ML.length > 0) {
                MCLIST_CHK = true;
            }
            else {
                MCLIST_CHK = false;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button2").click(function () {
    Button2_Click();
});


function Button2_Click() {
    TS_USER(1);
    let CODE_MC = "";
    let ComboBox3 = document.getElementById("ComboBox3").selectedIndex;
    switch (ComboBox3) {
        case 0:
            CODE_MC = "S";
            break;
        case 1:
            CODE_MC = "I";
            break;
        case 2:
            CODE_MC = "Q";
            break;
        case 3:
            CODE_MC = "M";
            break;    
        case 4:
            CODE_MC = "T";
            break;
        case 5:
            CODE_MC = "L";
            break;
        case 6:
            CODE_MC = "E";
            break;
    }
    //localStorage.setItem("C02_STOP_Close", 0);
    //localStorage.setItem("C02_STOP_Label5", CODE_MC);
    //localStorage.setItem("C02_STOP_STOP_MC", $("#MCbox").val());
    //localStorage.setItem("C02_STOP_Label2", $("#ComboBox3").val());

    //localStorage.setItem("C02_MCbox", $("#MCbox").val());
    //localStorage.setItem("C02_START_V2_Label8", "");
    //localStorage.setItem("SHIELDWIRE_CHK", SHIELDWIRE_CHK);
    //let url = "/C02_STOP";
    //OpenWindowByURL(url, 800, 460);

    $("#C02_STOP_Label5").val(CODE_MC);
    $("#C02_STOP_STOP_MC").val($("#MCbox").val());
    $("#C02_STOP_Label2").val($("#ComboBox3").val());
    $("#C02_STOP").modal("open");
    C02_STOP_PageLoad();
}

$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Buttonfind_Click();
}
$("#rbchk1").click(function () {
    rbchk1_Click();
});
$("#rbchk2").click(function () {
    rbchk1_Click();
});
function rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = true;
        }
        DataGridView1Render();
    }
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
        }
        DataGridView1Render();
    }
}
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
    Buttoninport_Click();
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
function Buttonfind_Click() {
    let IsFind = true;
    let AA = $("#DateTimePicker1").val();
    let BB = $("#ComboBox1").val();
    let CC = $("#TextBox1").val();
    let DD = $("#ComboBox2").val();
    let EEE = $("#TextBox2").val();
    let FFF = $("#TextBox3").val();
    let MCbox = $("#MCbox").val();
    if (MCbox == "") {
        IsFind = false;
        alert("Machine NO. Please Check Again. Máy KHÔNG. Vui lòng kiểm tra lại.");
    }
    if (IsFind == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(AA);
        BaseParameter.ListSearchString.push(BB);
        BaseParameter.ListSearchString.push(CC);
        BaseParameter.ListSearchString.push(DD);
        BaseParameter.ListSearchString.push(EEE);
        BaseParameter.ListSearchString.push(FFF);
        BaseParameter.ListSearchString.push(MCbox);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView1 = BaseResultSub.DataGridView1;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {
}
function Buttonsave_Click() {
    let IsSave = true;
    if (BaseResult.DataGridView1.length < 0) {
        IsSave = false;
    }
    if (IsSave == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert("Edit data Completed.");
                CHK_MC = $("#ComboBox1").val();
                MC_LIST(1);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}

//xóa đơn đã upload, chỉ xóa các đơn có trạng thái là Stay
function Buttondelete_Click() {
    if (confirm(localStorage.getItem("DeleteConfirm"))) {
        $("#BackGround").css("display", "block");

        deleteSelectedRowsOnDataGridView1();
    }
}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    TableHTMLToExcel("mainTableCheckAll", "C02", "C02");
}
function Buttonprint_Click() {

}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

//load dũ liệu lên lưới chính cua C02
function DataGridView1Render() {
    if (!BaseResult || !BaseResult.DataGridView1 || BaseResult.DataGridView1.length === 0) {
        if (mainGridView) {
            mainGridView.clear().draw();
        }
        return;
    }

    const rows = [];

    BaseResult.DataGridView1.forEach((row, i) => {
        const orNoStyle = row.OR_NO === "" ? "background-color: green;" : "background-color: darkorange;";

        const addCheckbox = `
            <label><input type="checkbox" class="row-check form-check-input"
                   id="DataGridView1CHK${i}" ${row.CHK ? "checked" : ""}><span></span>
            </label>
        `;

        const conditionBtn = `
            <button class="btn waves-effect waves-light grey darken-1"
                    onclick="DataGridView1_CellClick(${i})">
                ${row.CONDITION}
            </button>
        `;

        // Ô MC có input -> thêm data-row, data-col
        const mcInput = `
            <input
                type="text"
                class="form-control mc-cell"
                value="${row.MC ?? ''}"
                style="width:100px;"
                data-row-index="${i}"
                data-col="MC"
                onblur="DataGridView1_CellBeginEdit(${i}, this)"
            >
        `;

        const rowData = [
            i + 1,
            addCheckbox,
            `<span style="${orNoStyle} padding-left:20px; padding-right:20px;">${row.OR_NO}</span>`,
            row.WORK_WEEK,
            conditionBtn,
            FormatDateTime(row.CREATE_DTM),
            row.LEAD_NO,
            row.WIRE,
            row.QTY_STOCK,
            row.TOEXCEL_QTY,
            row.SUM_QTY,     
            row.ACT,
            mcInput,
            row.ADJ_AF_QTY,
            row.TERM1,
            row.SEAL1,
            row.TERM2,
            row.SEAL2,
            row.CCH_W1,
            row.ICH_W1,
            row.CCH_W2,
            row.ICH_W2,
            row.DT,
            row.LS_DATE,
            row.PROJECT,
            row.CUR_LEADS,
            row.CT_LEADS,
            row.CT_LEADS_PR,
            row.GRP,
            row.BUNDLE_SIZE,
            row.HOOK_RACK,
            row.T1_DIR,
            row.STRIP1,
            row.T2_DIR,
            row.STRIP2,
            row.SP_ST,
            row.REP
        ];

        rows.push(rowData);
    });

    if (mainGridView) {
        mainGridView.clear();
        mainGridView.rows.add(rows);
        mainGridView.draw(false);
    }

    $('#mainTableCheckAll').prop('checked', false);
}

// commit giá trị MC
function DataGridView1_CellBeginEdit(i, input) {
    BaseResult.DataGridView1[i].CHK = true;
    BaseResult.DataGridView1[i].MC = input.value;
}

// xử lý Enter để nhảy sang MC dòng kế tiếp
$(document).on('keydown', 'input.mc-cell', function (e) {
    if (e.key === "Enter") {
        e.preventDefault();

        const i = parseInt(this.dataset.rowIndex, 10);

        // lưu giá trị trước
        DataGridView1_CellBeginEdit(i, this);

        const nextRow = i + 1;

        // render lại
        DataGridView1Render();

        // focus vào ô MC dòng kế tiếp
        setTimeout(() => {
            const nextInput = document.querySelector(
                `input.mc-cell[data-row-index="${nextRow}"][data-col="MC"]`
            );
            if (nextInput) {
                nextInput.focus();
                nextInput.select();
            }
        }, 50);
    }
});


function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
let DataGridView1Table = document.getElementById("mainTableCheckAll");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}

//sư kiện clich vao đơn đang là Working hoạc Stay, nếu trạng thái là Complete thì không hiện đơn lên làm việc
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
    let IsCheck = true;

    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    if (CHKA == "Complete") {
        IsCheck = false;
    }
    if (IsCheck == true) {

        $("#C02_LIST_MCbox").val($("#MCbox").val());
        C02_LIST_LEAD_NO_SK = BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO;
        C02_LIST_CRT_DATE_SK = BaseResult.DataGridView1[DataGridView1RowIndex].CREATE_DTM;
        $("#C02_LIST").modal("open");
        C02_LIST_PageLoad();
    }
}
//lấy các dong đã chọn
function getSelectedRowsOnDataGridView1() {
    const selectedIndexes = [];
    const selectedRows = [];

    $(".row-check").each(function (i) {
        if ($(this).is(":checked")) {
            selectedIndexes.push(i);
            selectedRows.push(BaseResult.DataGridView1[i]);
        }
    });

    return { selectedIndexes, selectedRows };
}

//xóa các dòng đã chọn trên lưới dữ liệu chính
function deleteSelectedRowsOnDataGridView1() {
    const { selectedIndexes, selectedRows } = getSelectedRowsOnDataGridView1();

    if (selectedIndexes.length === 0) {
        ShowMessage("No data.", "error");
        return;
    }

    // Chuẩn bị dữ liệu gửi về server
    let BaseParameter = {};
    BaseParameter.DataGridView1 = [];
    BaseParameter.USER_ID = GetCookieValue("UserID");

    // Xóa khỏi BaseResult và lưu thông tin cần gửi
    for (let i = selectedIndexes.length - 1; i >= 0; i--) {
        // Nếu bạn muốn gửi ID về server thay vì chỉ số
        BaseParameter.DataGridView1.push(selectedRows[i]);
        BaseResult.DataGridView1.splice(selectedIndexes[i], 1);
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    let url = "/C02/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then((response) => response.json())
        .then((data) => {
            // Nếu server đã xóa thành công thì mới reload
            if (data.Success == true) {
                Buttonfind_Click();
                ShowMessage("Delete Order Data Completed. ( " + data.Message + " )", "ok");
            }

        })
        .catch((err) => {
            console.error(err);
        })
        .finally(() => {
            $("#BackGround").css("display", "none");
        });

    // Render lại Grid sau khi xóa local
    // DataGridView1Render();
}

function DataGridView1_CellPainting() {
    DGV_PNT();
}
function DGV_PNT() {

}

let C02_STOP_Timer1;
let C02_STOP_EW_TIME_Timer1;
let C02_STOP_StartTime
let C02_STOP_STOPW_ORING_IDX = 0;

function C02_STOP_PageLoad() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    C02_STOP_StartTime = now;
    C02_STOP_Timer1StartInterval();
    let STOP_MC = $("#C02_STOP_STOP_MC").val();
    let Label5 = $("#C02_STOP_Label5").val();
    let Label2 = $("#C02_STOP_Label2").val();

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label2);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_STOP/PageLoad";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    if (BaseResult.DataGridView1.length > 0) {
                        $("#C02_STOP_Label6").val(BaseResult.DataGridView1[0].TSNON_OPER_IDX);
                        SettingsNON_OPER_CHK = true;
                        SettingsNON_OPER_IDX = BaseResult.DataGridView1[0].TSNON_OPER_IDX;
                        localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
                        localStorage.setItem("SettingsNON_OPER_IDX", SettingsNON_OPER_IDX);
                    }
                }
            }
            C02_STOP_SW_TIME();
            C02_STOP_EW_TIME_Timer1StartInterval();

            // ====== THAY ĐỔI: Logic button enable/disable ======
            document.getElementById("C02_STOP_Button1").disabled = true;  // Khởi động lại
            document.getElementById("C02_STOP_Button2").disabled = true;  // Đang kiểm tra
            document.getElementById("C02_STOP_Button3").disabled = true;  // Hoàn thành sửa chữa

            if ($("#C02_STOP_Label5").val() == "M") {
                document.getElementById("C02_STOP_Button2").disabled = false;  // Enable "Đang kiểm tra"
            } else {
                document.getElementById("C02_STOP_Button1").disabled = false;  // Enable "Khởi động lại" cho loại khác
            }
            // ====================================================
            C02_STOP_ToggleButton3();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//kiểm tra máy có báo downtime trước đó hay không, nếu có mà chưa được đóng lại thì hiển thị lại giao diện Downtime
function C02_STOP_ShowDowntime(ID, downtimeCode, MC_Name, startTime) {
    $("#C02_STOP_Label6").val(ID);
    $("#C02_STOP_Label5").val(downtimeCode);
    $("#C02_STOP_STOP_MC").val(MC_Name);
    let stopIfor = "Chuan bi Thiet Bi";

    // ====== THAY ĐỔI: Logic button enable/disable ======
    document.getElementById("C02_STOP_Button1").disabled = true;
    document.getElementById("C02_STOP_Button2").disabled = true;
    document.getElementById("C02_STOP_Button3").disabled = true;

    if (downtimeCode === 'S') {
        stopIfor = "Chuẩn bị thiết bị (S)";
    } else if (downtimeCode === 'E') {
        stopIfor = "Khác (E)";
    } else if (downtimeCode === 'I') {
        stopIfor = "Thiếu nguyên vật liệu (I)";
    } else if (downtimeCode === 'L') {
        stopIfor = "Giờ ăn (L)";
    } else if (downtimeCode === 'M') {
        stopIfor = "Máy hư (M)";
        document.getElementById("C02_STOP_Button2").disabled = false;  // Enable "Đang kiểm tra"
    } else if (downtimeCode === 'N') {
        stopIfor = "Không có NV (N)";
    } else if (downtimeCode === 'Q') {
        stopIfor = "Vấn đề chất lượng (Q)";
    } else if (downtimeCode === 'T') {
        stopIfor = "Đào tạo/ họp (T)";
    }

    if (downtimeCode !== 'M') {
        document.getElementById("C02_STOP_Button1").disabled = false;  // Enable "Khởi động lại" cho loại khác
    }
    // ====================================================

    $("#C02_STOP_Label2").val(stopIfor);

    C02_STOP_StartTime = new Date(startTime);
    $("#C02_STOP").modal("open");
    C02_STOP_Timer1StartInterval();
    C02_STOP_ToggleButton3();
}

function C02_STOP_Timer1StartInterval() {
    C02_STOP_Timer1 = setInterval(function () {
        C02_STOP_Timer1_Tick();
    }, 1000);
}

function C02_STOP_Timer1_Tick() {
    let End = new Date();
    $("#C02_STOP_Label4").val(CounterByBegin_EndToString(C02_STOP_StartTime, End));
}

function C02_STOP_FormClosed() {
    let Label6 = $("#C02_STOP_Label6").val();
    let STOP_MC = $("#C02_STOP_STOP_MC").val();
    let Label5 = $("#C02_STOP_Label5").val();

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C02_STOP_StartTime);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_STOP/C02_STOP_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            clearInterval(C02_STOP_Timer1);
            C02_STOP_OPER_TIME();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function C02_STOP_OPER_TIME() {
    let STOP_MC = $("#C02_STOP_STOP_MC").val();
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            try {
                BaseResult.C02_STOP_Search = data.Search;
                let TOT_SUM = BaseResult.C02_STOP_Search[0].SUM_TIME;

                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C02_START_V2_BI_STOPTIME = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C02_START_V2_BI_STOPTIME.substr(0, 2);
                let M_TIME1 = C02_START_V2_BI_STOPTIME.substr(2, 2);
                let S_TIME1 = C02_START_V2_BI_STOPTIME.substr(4, 2);
                C02_START_V2_BI_STOPTIME = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                $("#C02_START_V2_BI_STOPTIME").val(C02_START_V2_BI_STOPTIME);
            }
            catch (e) {
            }
            C02_STOP_EW_TIME(1);
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

// ====== THAY ĐỔI: Button2 chỉ update trạng thái, KHÔNG mở modal ======
$("#C02_STOP_Button2").click(function () {
    C02_STOP_Button2_Click();
});

function C02_STOP_Button2_Click() {
    let STOP_MC = $("#C02_STOP_STOP_MC").val();
    let Label6 = $("#C02_STOP_Label6").val();

    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_STOP/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#C02_STOP_TSNON_OPER_IDX").val(Label6);
            $("#C02_STOP_Label2").val("점검 중 / Đang kiểm tra");

            // Disable "Đang kiểm tra", Enable "Hoàn thành sửa chữa" và "Khởi động lại"
            document.getElementById("C02_STOP_Button2").disabled = true;
            document.getElementById("C02_STOP_Button3").disabled = false;
            document.getElementById("C02_STOP_Button1").disabled = false;

            M.toast({
                html: 'Đang kiểm tra',
                classes: 'yellow darken-2',
                displayLength: 3000
            });
        }).catch((err) => {
            alert("Lỗi: " + err.message);
        })
    });
}

// ====== NÚT MỚI: Button3 - Hoàn thành sửa chữa (Mở form) ======
$("#C02_STOP_Button3").click(function () {
    C02_STOP_Button3_Click();
});

function C02_STOP_Button3_Click() {
    let formElement = $("#C02_STOP_MaintenanceForm");

    if (formElement.length === 0) {
        alert("Form không tồn tại! Kiểm tra HTML.");
        return;
    }

    // Reset form
    if (formElement.length > 0) {
        formElement[0].reset();
    }

    let modalElement = $("#C02_STOP_MaintenanceModal");

    if (modalElement.length === 0) {
        alert("Modal không tồn tại! Kiểm tra HTML.");
        return;
    }

    modalElement.modal("open");
}
// =================================================================

$("#C02_STOP_Button1").click(function () {
    C02_STOP_Button1_Click();
});

function C02_STOP_Button1_Click() {
    let Label5 = $("#C02_STOP_Label5").val();

    // ====== THAY ĐỔI: Bỏ check bắt buộc phải nhấn "Đang kiểm tra" ======
    // Cho phép khởi động lại ngay cả khi chưa nhập form
    // (Backend sẽ tự động đóng MaintenanceHistory)

    C02_START_V2_ORDER_LOAD(1);

    if (Label5 == "M") {
        let STOP_MC = $("#C02_STOP_STOP_MC").val();
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: STOP_MC,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_STOP/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                C02_STOP_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C02_STOP_FormClosed();
    }
}

function C02_STOP_EW_TIME_Timer1StartInterval() {
    C02_STOP_EW_TIME_Timer1 = setInterval(function () {
        C02_STOP_EW_TIME_Timer1_Tick();
    }, 60000);
}

function C02_STOP_EW_TIME_Timer1_Tick() {
    C02_STOP_EW_TIME(0);
}

function C02_STOP_EW_TIME(Flag) {
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C02_STOP_STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_STOP/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (Flag == 1) {
                $("#C02_STOP").modal("close");
            }
        }).catch((err) => {
            ShowMessage("C02_STOP_EW_TIME:" + err.message, "error");
        })
    });
}

function C02_STOP_SW_TIME() {
    try {
        let USER_MC = localStorage.getItem("C02_MCbox");
        let USER_ORIDX = localStorage.getItem("C02_START_V2_Label8");
        let Label5 = $("#C02_STOP_Label5").val();;
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(USER_MC);
        BaseParameter.ListSearchString.push(USER_ORIDX);
        BaseParameter.ListSearchString.push(Label5);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_STOP/SW_TIME";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C02_STOP_DGV_WT = data.DGV_WT;
                try {
                    C02_STOP_STOPW_ORING_IDX = BaseResult.C02_STOP_DGV_WT[0].TOWT_INDX;
                }
                catch (e) {
                    C02_STOP_STOPW_ORING_IDX = 0;
                }
            }).catch((err) => {
                ShowMessage("C02_STOP_SW_TIME:" + err.message, "error");
            })
        });
    }
    catch (e) {
        C02_STOP_STOPW_ORING_IDX = 0;
    }
}
function C02_STOP_ToggleButton3() {
    if ($("#C02_STOP_Label5").val() === "M") {
        $("#C02_STOP_Button3").show();
    } else {
        $("#C02_STOP_Button3").hide();
    }
}
function C02_STOP_SaveMaintenanceHistory() {
    let currentStatus = $("#C02_STOP_CurrentStatus").val();
    let solution = $("#C02_STOP_Solution").val();
    let maintenedBy = $("#C02_STOP_MaintenedBy").val();
    let sparePartsUsed = $("#C02_STOP_SparePartsUsed").val().trim();

    // Validate required fields
    if (!currentStatus) {
        M.toast({ html: 'Vui lòng chọn Nội dung lỗi!', classes: 'red' });
        return;
    }
    if (!solution) {
        M.toast({ html: 'Vui lòng chọn Biện pháp!', classes: 'red' });
        return;
    }
    if (!sparePartsUsed) {
        M.toast({ html: 'Vui lòng nhập Phụ tùng sửa chữa!', classes: 'red' });
        return;
    }
    if (!maintenedBy) {
        M.toast({ html: 'Vui lòng chọn Người sửa!', classes: 'red' });
        return;
    }

    let STOP_MC = $("#C02_STOP_STOP_MC").val();
    let BaseParameter = {
        STOP_MC: STOP_MC,
        CurrentStatus: currentStatus,
        Solution: solution,
        SparePartsUsed: sparePartsUsed,
        MaintenedBy: maintenedBy,
        Notes: $("#C02_STOP_Notes").val().trim(),
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C02_STOP/SaveMaintenanceHistory", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                M.toast({ html: 'Lưu chi tiết bảo trì thành công!', classes: 'green' });
                $("#C02_STOP_MaintenanceModal").modal("close");
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không thể lưu dữ liệu'), classes: 'red' });
            }
        })
        .catch(err => {
            M.toast({ html: 'Có lỗi xảy ra: ' + err.message, classes: 'red' });
        });
}

let C02_LIST_DataGridView1RowIndex = 0;
let C02_LIST_SHIELDWIRE_CHK;
let C02_LIST_MCLIST_CHK;
let C02_LIST_LEAD_NO_SK;
let C02_LIST_CRT_DATE_SK;

//load danh sách chi tiết đơn cắt theo đơn đã chọn
function C02_LIST_PageLoad() {
    BaseResult.C02_LIST_DataGridView1 = new Object();
    BaseResult.C02_LIST_DataGridView1 = [];
    //load thông tin mã dừng line
    C02_LIST_Lan_Change();
    //đóng đơn quá ngày
    C02_LIST_DB_LISECHK();

    C02_LIST_TS_USER(0);

    C02_LIST_SHIELDWIRE_CHK = false;

    C02_LIST_PageLoadSub();

    document.getElementById("C02_LIST_rbchk2").checked = true;

    C02_LIST_MCLIST_CHK = false;

    C02_LIST_SetMCLIST_CHK();

    C02_LIST_Buttonfind_Click();
}

function C02_LIST_DB_LISECHK() {
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/DB_LISECHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_TS_USER(Flag) {
    let MCbox = $("#C02_LIST_MCbox").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/TS_USER";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_LIST_USER_TIME = data.USER_TIME;
            if (BaseResult.C02_LIST_USER_TIME) {
                if (BaseResult.C02_LIST_USER_TIME.length > 0) {
                    ToolStripLOGIDX = BaseResult.C02_LIST_USER_TIME[0].TUSER_IDX;
                    localStorage.setItem("ToolStripLOGIDX", ToolStripLOGIDX);
                }
            }
            BaseResult.C02_LIST_USER_TIME1 = data.USER_TIME1;
            if (BaseResult.C02_LIST_USER_TIME1) {
                if (BaseResult.C02_LIST_USER_TIME1.length > 0) {
                    let S_DATE = BaseResult.C02_LIST_USER_TIME1[0].TS_DATE;
                    $("#C02_START_V2_BI_STIME").val(BaseResult.C02_LIST_USER_TIME1[0].Name);
                    $("#C02_START_V2_Label70").val(BaseResult.C02_LIST_USER_TIME1[0].Description);
                }
            }
            if (Flag == 1) {
                let CODE_MC = "";
                let ComboBox3 = document.getElementById("C02_LIST_ComboBox3").selectedIndex;
                switch (ComboBox3) {
                    case 0:
                        CODE_MC = "S";
                        break;
                    case 1:
                        CODE_MC = "I";
                        break;
                    case 2:
                        CODE_MC = "Q";
                        break;
                    case 3:
                        CODE_MC = "M";
                        break;
                    case 4:
                        CODE_MC = "T";
                        break;
                    case 5:
                        CODE_MC = "L";
                        break;
                    case 6:
                        CODE_MC = "E";
                        break;
                }

                $("#C02_STOP_Label5").val(CODE_MC);
                $("#C02_STOP_STOP_MC").val($("#C02_LIST_MCbox").val());
                $("#C02_STOP_Label2").val($("#C02_LIST_ComboBox3").val());
                $("#C02_START_V2_Label8").val("");

                $("#C02_STOP").modal("open");
                C02_STOP_PageLoad();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_PageLoadSub() {
    let MCbox = $("#C02_LIST_MCbox").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/PageLoad";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_SetMCLIST_CHK() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SettingsMC_NM,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/SetMCLIST_CHK";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_LIST_DGV_C02_ML = data.DGV_C02_ML;
            if (BaseResult.C02_LIST_DGV_C02_ML.length > 0) {
                C02_LIST_MCLIST_CHK = true;
            }
            else {
                C02_LIST_MCLIST_CHK = false;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_Lan_Change() {
    C02_LIST_Form_Label2();
}
function C02_LIST_Form_Label2() {
    let CODE_S1 = "S";
    let CODE_I1 = "I";
    let CODE_Q1 = "Q";
    let CODE_M1 = "M";
    let CODE_T1 = "T";
    let CODE_L1 = "L";
    let CODE_E1 = "E";

    let CODE_S0 = $("#CODE_S").val() + " (" + CODE_S1 + ")";
    let CODE_I0 = $("#CODE_I").val() + " (" + CODE_I1 + ")";
    let CODE_Q0 = $("#CODE_Q").val() + " (" + CODE_Q1 + ")";
    let CODE_M0 = $("#CODE_M").val() + " (" + CODE_M1 + ")";
    let CODE_T0 = $("#CODE_T").val() + " (" + CODE_T1 + ")";
    let CODE_L0 = $("#CODE_L").val() + " (" + CODE_L1 + ")";
    let CODE_E0 = $("#CODE_E").val() + " (" + CODE_E1 + ")";

    $("#C02_LIST_ComboBox3").empty();

    var ComboBox3 = document.getElementById("C02_LIST_ComboBox3");

    var option = document.createElement("option");
    option.text = CODE_S0;
    option.value = CODE_S0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_I0;
    option.value = CODE_I0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_Q0;
    option.value = CODE_Q0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_M0;
    option.value = CODE_M0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_T0;
    option.value = CODE_T0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_L0;
    option.value = CODE_L0;
    ComboBox3.add(option);

    option = document.createElement("option");
    option.text = CODE_E0;
    option.value = CODE_E0;
    ComboBox3.add(option);
}

$("#C02_LIST_rbchk1").change(function () {
    C02_LIST_rbchk1_CheckedChanged();
});

function C02_LIST_rbchk1_CheckedChanged() {
    let rbchk1 = document.getElementById("C02_LIST_rbchk1").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.C02_LIST_DataGridView1.length; i++) {
            BaseResult.C02_LIST_DataGridView1[i].CHK = true;
        }
        C02_LIST_DataGridView1Render();
    }
}
$("#C02_LIST_rbchk2").change(function () {
    C02_LIST_rbchk2_CheckedChanged();
});
function C02_LIST_rbchk2_CheckedChanged() {
    let rbchk2 = document.getElementById("C02_LIST_rbchk2").checked;
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.C02_LIST_DataGridView1.length; i++) {
            BaseResult.DataGridView1[i].CHK = !BaseResult.C02_LIST_DataGridView1[i].CHK;
        }
        C02_LIST_DataGridView1Render();
    }
}
$("#C02_LIST_Button1").click(function () {
    C02_LIST_Button1_Click();
});
function C02_LIST_Button1_Click() {
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data) {
                if (data.Code) {
                    let url = data.Code;
                    window.location.href = url;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_LIST_Button2").click(function () {
    C02_LIST_Button2_Click();
});
function C02_LIST_Button2_Click() {
    C02_LIST_TS_USER(1);
}
$("#C02_LIST_Button3").click(function () {
    C02_LIST_Button3_Click();
});
function C02_LIST_Button3_Click() {
    $("#C02_COUNT").modal("open");
    C02_COUNT_PageLoad();
}
$("#C02_LIST_Buttonfind").click(function () {
    C02_LIST_Buttonfind_Click();
});
$("#C02_LIST_Buttonadd").click(function () {
    C02_LIST_Buttonadd_Click();
});
$("#C02_LIST_Buttonsave").click(function () {
    C02_LIST_Buttonsave_Click();
});
$("#C02_LIST_Buttondelete").click(function () {
    C02_LIST_Buttondelete_Click();
});
$("#C02_LIST_Buttoncancel").click(function () {
    C02_LIST_Buttoncancel_Click();
});
$("#C02_LIST_Buttoninport").click(function () {
    C02_LIST_Buttoninport_Click();
});
$("#C02_LIST_Buttonexport").click(function () {
    C02_LIST_Buttonexport_Click();
});
$("#C02_LIST_Buttonprint").click(function () {
    C02_LIST_Buttonprint_Click();
});
$("#C02_LIST_Buttonhelp").click(function () {
    C02_LIST_Buttonhelp_Click();
});
$("#C02_LIST_Buttonclose").click(function () {
    C02_LIST_Buttonclose_Click();
});
function C02_LIST_Buttonfind_Click() {

    $("#BackGround").css("display", "block");
    let MCbox = $("#C02_LIST_MCbox").val();
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C02_LIST_LEAD_NO_SK);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_LIST_DataGridView1 = data.DataGridView1;
            C02_LIST_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_Buttonadd_Click() {
}
function C02_LIST_Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.C02_LIST_DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            alert("Edit data Completed.");
            C02_LIST_Buttonfind_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_LIST_Buttondelete_Click() {
    if (confirm("Delete the selected Data?.")) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView1 = BaseResult.C02_LIST_DataGridView1;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                alert("Delete Order Data Completed. ");
                C02_LIST_Buttonfind_Click();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C02_LIST_Buttoncancel_Click() {
    if (confirm("Cancel MT Request(Huỷ  Yêu Cầu vật NVL)?")) {
        let IDX_CHK = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].ORDER_IDX;
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(IDX_CHK);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/Buttoncancel_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C02_LIST_Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.DataGridView1 = BaseResult.DataGridView1;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            alert("Edit data Completed.");
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function C02_LIST_Buttonexport_Click() {
    TableHTMLToExcel("C02_LIST_DataGridView1Table", "C02_LIST", "C02_LIST");
}

function C02_LIST_Buttonprint_Click() {
    let CHKA = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CONDITION;
    if (CHKA == "Stay") {

    }
    else {
        if (BaseResult.C02_LIST_DataGridView1.length > 0) {

            $("#C02_REPRINT_Label4").val(BaseResult.DataGridView1[DataGridView1RowIndex].ORDER_IDX);
            C02_REPRINT_WHERE_TEXT = "";
            $("#C02_REPRINT").modal("open");
            C02_REPRINT_PageLoad();
        }
    }
}

function C02_LIST_Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function C02_LIST_Buttonclose_Click() {
    $("#C02_LIST").modal("close");
    Buttonfind_Click();
}

function C02_LIST_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C02_LIST_DataGridView1) {
            if (BaseResult.C02_LIST_DataGridView1.length > 0) {
                C02_LIST_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C02_LIST_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.C02_LIST_DataGridView1[i].CHK == true) {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "'' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    if (BaseResult.C02_LIST_DataGridView1[i].OR_NO == "") {
                        HTML = HTML + "<td style='background-color: green; padding-left: 20px; padding-right: 20px;'>" + BaseResult.C02_LIST_DataGridView1[i].OR_NO + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: darkorange; padding-left: 20px; padding-right: 20px;'>" + BaseResult.C02_LIST_DataGridView1[i].OR_NO + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td onclick='C02_LIST_DataGridView1_CellClick3(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.C02_LIST_DataGridView1[i].CONDITION + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td onclick='C02_LIST_DataGridView1_CellClick5(" + i + ")'>" + BaseResult.C02_LIST_DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td onclick='C02_LIST_DataGridView1_CellClick5(" + i + ")'><button class='btn waves-effect waves-light grey darken-1' style='width: 50px;'>" + BaseResult.C02_LIST_DataGridView1[i].MTRL_RQUST + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].QTY_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].TOEXCEL_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td><input onblur='C02_LIST_DataGridView1_CellBeginEdit(" + i + ", this)' type='text' class='form-control' value='" + BaseResult.C02_LIST_DataGridView1[i].MC + "' style='width: 100px;'></td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].LS_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].CUR_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].CT_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].CT_LEADS_PR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.C02_LIST_DataGridView1[i].SP_ST) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].REP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_LIST_DataGridView1[i].ORDER_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C02_LIST_DataGridView1").innerHTML = HTML;
}

function C02_LIST_DataGridView1_SelectionChanged(i) {
    C02_LIST_DataGridView1RowIndex = i;
    C02_LIST_TS_USER(0);
    C02_LIST_RE_TIME();
    C02_LIST_SHIELDWIRE_CHK = false;
}
function C02_LIST_RE_TIME() {
    let MCbox = $("#C02_LIST_MCbox").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_LIST/RE_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
let C02_LIST_DataGridView1Table = document.getElementById("C02_LIST_DataGridView1Table");
C02_LIST_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C02_LIST_DataGridView1, key, text, IsTableSort);
        C02_LIST_DataGridView1Render();
    }
});
function C02_LIST_DataGridView1_CellBeginEdit(i, input) {
    C02_LIST_DataGridView1RowIndex = i;
    if (BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CONDITION == "Stay") {
        BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CHK = true;
        BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].MC = input.value;
        C02_LIST_DataGridView1Render();
    }
}
function C02_LIST_DataGridView1CHKChanged(i) {
    C02_LIST_DataGridView1RowIndex = i;
    BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CHK = !BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CHK;
    C02_LIST_DataGridView1Render();
}
function C02_LIST_DataGridView1_CellClick5(i) {
    C02_LIST_DataGridView1RowIndex = i;
    let IsCheck = true;
    if (C02_LIST_MCLIST_CHK == true) {
        CHKA = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].MTRL_RQUST;
        if (CHKA == "N") {

            C02_MT_ORDR = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].ORDER_IDX;
            document.getElementById("C02_MT_Buttonsave").disabled = false;
            $("#C02_MT").modal("open");
            C02_MT_PageLoad();
        }
        else {

            C02_MT_ORDR = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].ORDER_IDX;
            document.getElementById("C02_MT_Buttonsave").disabled = true;
            $("#C02_MT").modal("open");
            C02_MT_PageLoad();
        }
    }
}

//button trạng thái của đơn trong dánh sách C02_List : Stay, Working hoạc Complete
function C02_LIST_DataGridView1_CellClick3(i) {
    C02_LIST_DataGridView1RowIndex = i;
    let IsCheck = true;
    let CHKA = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].CONDITION;
    if (CHKA == "Complete") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let XXX = 0;
        if (CHKA == "Working") {
        }
        else {
            for (let i = 0; i < BaseResult.C02_LIST_DataGridView1.length; i++) {
                if (BaseResult.C02_LIST_DataGridView1[i].CONDITION == "Working") {
                    XXX = XXX + 1;
                }
            }
            if (XXX >= 3) {
                IsCheck = false;
                alert("이전 작업이 있습니다. Có một công việc trước đây.");
            }
        }
    }
    if (IsCheck == true) {
        if (BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].MC == "SHIELD WIRE") {
            C02_LIST_SHIELDWIRE_CHK = true;
        }
        let AAA = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].LEAD_NO;
        let BBB = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].BUNDLE_SIZE;
        let CCC = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].DT;
        let DDD = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].TOT_QTY;
        let FFF = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].ORDER_IDX;
        let GGG = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].SP_ST;
        let HHH = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].PROJECT;
        let LLL = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].MC;
        let MMM = BaseResult.C02_LIST_DataGridView1[C02_LIST_DataGridView1RowIndex].ADJ_AF_QTY;

        SettingsMC_NM = localStorage.getItem("SettingsMC_NM");

        $("#C02_START_V2_Label4").val(AAA);
        $("#C02_START_V2_Label8").val(FFF);
        $("#C02_START_V2_Label42").val(GGG);
        $("#C02_START_V2_Label43").val(HHH);
        $("#C02_START_V2_L_MCNM").val(SettingsMC_NM);
        $("#C02_START_V2_Label48").val(DDD);
        $("#C02_START_V2_Label50").val(BBB);
        $("#C02_START_V2_Label77").val(DateNow);
        $("#C02_ERROR_Label1").val(FFF);

        let ERROR_CHK = "";

        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(FFF);
        BaseParameter.ListSearchString.push(GGG);
        BaseParameter.ListSearchString.push(HHH);
        BaseParameter.ListSearchString.push(LLL);
        BaseParameter.ListSearchString.push(MMM);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_LIST/DataGridView1_CellClick3";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {

                BaseResult.C02_LIST_Search = data.Search;
                if (BaseResult.C02_LIST_Search.length <= 0) {
                    ERROR_CHK = "N";
                    C02_START_V2_C_EXIT = false;
                }
                else {
                    ERROR_CHK = BaseResult.C02_LIST_Search[0].ERROR_CHK;
                    C02_START_V2_C_EXIT = false;
                }
                if (CHKA == "Complete") {

                }
                else {
                    if (ERROR_CHK == "Y") {
                        C02_START_V2_C_EXIT = false;
                        $("#C02_START_V2").modal("open");
                        C02_START_V2_PageLoad();
                    }
                    else {
                        C02_START_V2_C_EXIT = false;
                        $("#C02_ERROR").modal("open");
                        C02_ERROR_PageLoad();
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}

let C02_ERROR_LAST_ID;
let C02_ERROR_StartTime;
let C02_ERROR_RunTime;
let C02_ERROR_EW_TIME_Timer1;
function C02_ERROR_PageLoad() {
    WORING_IDX = localStorage.getItem("WORING_IDX");
    if (WORING_IDX == null) {
        WORING_IDX = 0;
        localStorage.setItem("WORING_IDX", WORING_IDX);
    }
    C02_ERROR_StartTime = new Date();

    $("#C02_ERROR_TextBox1").val("");
    $("#C02_ERROR_LBT1R").val("");
    $("#C02_ERROR_LBT2R").val("");
    $("#C02_ERROR_LBA1R").val("");
    $("#C02_ERROR_LBA2R").val("");
    $("#C02_ERROR_LBS1R").val("");
    $("#C02_ERROR_LBS2R").val("");
    $("#C02_ERROR_LBWR").val("");
    $("#C02_ERROR_LBT1V").val("");
    $("#C02_ERROR_LBT2V").val("");
    $("#C02_ERROR_LBA1V").val("");
    $("#C02_ERROR_LBA2V").val("");
    $("#C02_ERROR_LBS1V").val("");
    $("#C02_ERROR_LBS2V").val("");
    $("#C02_ERROR_LBWV").val("");
    $("#C02_ERROR_LBA1S").val("");
    $("#C02_ERROR_LBA2S").val("");
    $("#C02_ERROR_LBA1SEQ").val("");
    $("#C02_ERROR_LBA2SEQ").val("");
    $("#C02_ERROR_LBA1IDX").val("");
    $("#C02_ERROR_LBA2IDX").val("");
    $("#C02_ERROR_LBR1").val("");
    $("#C02_ERROR_LBR2").val("");
    $("#C02_ERROR_LBR3").val("");
    $("#C02_ERROR_LBR4").val("");
    $("#C02_ERROR_LBR5").val("");
    $("#C02_ERROR_LBR11").val("");
    $("#C02_ERROR_LBR22").val("");

    document.getElementById("C02_ERROR_LBR1").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR2").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR3").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR4").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR5").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR11").style.backgroundColor = "white";
    document.getElementById("C02_ERROR_LBR22").style.backgroundColor = "white";

    document.getElementById("C02_ERROR_CHBT1").checked = false;
    document.getElementById("C02_ERROR_CHBA1").checked = false;
    document.getElementById("C02_ERROR_CHBT2").checked = false;
    document.getElementById("C02_ERROR_CHBA2").checked = false;
    document.getElementById("C02_ERROR_CHBS1").checked = false;
    document.getElementById("C02_ERROR_CHBS2").checked = false;
    document.getElementById("C02_ERROR_CHBW").checked = false;
    C02_ERROR_Load();
}
function C02_ERROR_Load() {
    let IsCheck = true;
    let Label1 = $("#C02_ERROR_Label1").val();
    let MCbox = $("#MCbox").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label1);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/C02_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_ERROR_DataGridView1 = data.DataGridView1;
            BaseResult.C02_ERROR_DataGridView2 = data.DataGridView2;
            BaseResult.C02_ERROR_DataGridView3 = data.DataGridView3;

            if (BaseResult.C02_ERROR_DataGridView1.length > 0) {
                $("#C02_ERROR_LBT1S").val(BaseResult.C02_ERROR_DataGridView1[0].TERM1);
                $("#C02_ERROR_LBA1S").val(BaseResult.C02_ERROR_DataGridView1[0].TERM1);
                $("#C02_ERROR_LBS1S").val(BaseResult.C02_ERROR_DataGridView1[0].SEAL1);
                $("#C02_ERROR_LBT2S").val(BaseResult.C02_ERROR_DataGridView1[0].TERM2);
                $("#C02_ERROR_LBA2S").val(BaseResult.C02_ERROR_DataGridView1[0].TERM2);
                $("#C02_ERROR_LBS2S").val(BaseResult.C02_ERROR_DataGridView1[0].SEAL2);
                $("#C02_ERROR_Label2").val(BaseResult.C02_ERROR_DataGridView1[0].WIRE);
            }

            $("#C02_ERROR_ComboBox1").empty();

            let PP = BaseResult.C02_ERROR_DataGridView1.length - 1;
            if (PP != 0) {
                IsCheck = false;
                C02_START_V2_C_EXIT = true;
                C02_ERROR_Buttonclose_Click();
            }
            if (IsCheck == true) {
                if (BaseResult.C02_ERROR_DataGridView2.length > 0) {
                    if ((BaseResult.C02_ERROR_DataGridView2[0].WIRE_IDX == null) || (BaseResult.C02_ERROR_DataGridView2[0].WIRE_IDX == 0)) {
                        document.getElementById("C02_ERROR_CHBW").checked = false;
                    }
                    else {
                        document.getElementById("C02_ERROR_CHBW").checked = true;
                        var ComboBox1 = document.getElementById("C02_ERROR_ComboBox1");

                        var option = document.createElement("option");
                        option.text = "WIRE";
                        option.value = "WIRE";
                        ComboBox1.add(option);
                    }
                    if ((BaseResult.C02_ERROR_DataGridView2[0].T1_IDX == 0) || (BaseResult.C02_ERROR_DataGridView2[0].T1_IDX == null)) {
                        document.getElementById("C02_ERROR_CHBT1").checked = false;
                        document.getElementById("C02_ERROR_CHBA1").checked = false;
                    }
                    else {
                        document.getElementById("C02_ERROR_CHBT1").checked = true;
                        document.getElementById("C02_ERROR_CHBA1").checked = true;

                        var ComboBox1 = document.getElementById("C02_ERROR_ComboBox1");

                        var option = document.createElement("option");
                        option.text = "TERM1";
                        option.value = "TERM1";
                        ComboBox1.add(option);

                        option = document.createElement("option");
                        option.text = "APPLICATOR1";
                        option.value = "APPLICATOR1";
                        ComboBox1.add(option);
                    }
                    if ((BaseResult.C02_ERROR_DataGridView2[0].S1_IDX == 0) || (BaseResult.C02_ERROR_DataGridView2[0].S1_IDX == null)) {
                        document.getElementById("C02_ERROR_CHBS1").checked = false;
                    }
                    else {
                        document.getElementById("C02_ERROR_CHBS1").checked = true;
                        var ComboBox1 = document.getElementById("C02_ERROR_ComboBox1");

                        var option = document.createElement("option");
                        option.text = "SEAL1";
                        option.value = "SEAL1";
                        ComboBox1.add(option);
                    }
                    if ((BaseResult.C02_ERROR_DataGridView2[0].T2_IDX == 0) || (BaseResult.C02_ERROR_DataGridView2[0].T2_IDX == null)) {
                        document.getElementById("C02_ERROR_CHBT2").checked = false;
                        document.getElementById("C02_ERROR_CHBA2").checked = false;
                    }
                    else {
                        document.getElementById("C02_ERROR_CHBT2").checked = true;
                        document.getElementById("C02_ERROR_CHBA2").checked = true;
                        var ComboBox1 = document.getElementById("C02_ERROR_ComboBox1");

                        var option = document.createElement("option");
                        option.text = "TERM2";
                        option.value = "TERM2";
                        ComboBox1.add(option);

                        option = document.createElement("option");
                        option.text = "APPLICATOR2";
                        option.value = "APPLICATOR2";
                        ComboBox1.add(option);
                    }
                    if ((BaseResult.C02_ERROR_DataGridView2[0].S2_IDX == 0) || (BaseResult.C02_ERROR_DataGridView2[0].S2_IDX == null)) {
                        document.getElementById("C02_ERROR_CHBS2").checked = false;
                    }
                    else {
                        document.getElementById("C02_ERROR_CHBS2").checked = true;
                        var ComboBox1 = document.getElementById("C02_ERROR_ComboBox1");

                        var option = document.createElement("option");
                        option.text = "SEAL2";
                        option.value = "SEAL2";
                        ComboBox1.add(option);
                    }
                    $("#C02_ERROR_LBWS").val(BaseResult.C02_ERROR_DataGridView2[0].WIRE_NM);
                    if (SHIELDWIRE_CHK == true) {
                        document.getElementById("C02_ERROR_CHBT1").checked = false;
                        document.getElementById("C02_ERROR_CHBA1").checked = false;
                        document.getElementById("C02_ERROR_CHBT2").checked = false;
                        document.getElementById("C02_ERROR_CHBA2").checked = false;
                        document.getElementById("C02_ERROR_CHBS1").checked = false;
                        document.getElementById("C02_ERROR_CHBS2").checked = false;
                    }

                }
                if (BaseResult.C02_ERROR_DataGridView3.length > 0) {
                    C02_ERROR_LAST_ID = BaseResult.C02_ERROR_DataGridView3[0].TSNON_OPER_IDX;
                }
                C02_ERROR_SW_TIME();
                C02_ERROR_EW_TIME_Timer1StartInterval();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_ERROR_SW_TIME() {
    let KOMAX_ERROR = "KOMAX_ERROR";
    let MCbox = $("#MCbox").val();
    let Label1 = $("#C02_ERROR_Label1").val();
    //  $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    BaseParameter.ListSearchString.push(Label1);
    BaseParameter.ListSearchString.push(KOMAX_ERROR);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_ERROR_Search = data.Search;
            if (BaseResult.C02_ERROR_Search.length > 0) {
                WORING_IDX = BaseResult.C02_ERROR_Search[0].TOWT_INDX;
                localStorage.setItem("WORING_IDX", WORING_IDX);
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_ERROR_EW_TIME_Timer1StartInterval() {
    C02_ERROR_EW_TIME_Timer1 = setInterval(function () {
        C02_ERROR_Timer1_Tick();
    }, 60000);
}
function C02_ERROR_Timer1_Tick() {
    C02_ERROR_EW_TIME(0);
}
function C02_ERROR_EW_TIME(Flag) {
    WORING_IDX = localStorage.getItem("WORING_IDX");
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(WORING_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            //if (Flag == 1) {
            //    C02_ERROR_Buttonclose_Click();
            //}
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_ERROR_Button1").click(function (e) {
    C02_ERROR_Button1_Click();
});


function C02_ERROR_Button1_Click() {
    let IsCheck = true;
    let chuoiNhap = "";
    let ComboBox1 = $("#C02_ERROR_ComboBox1").val();
    let TextBox1 = $("#C02_ERROR_TextBox1").val();
    TextBox1 = TextBox1.trim();
    if (ComboBox1 == "WIRE") {
        chuoiNhap = TextBox1.toUpperCase();
    }
    else {
        chuoiNhap = TextBox1.toUpperCase();
    }

    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;

    let BAR_TEXT = false;
    if (ComboBox1 == "APPLICATOR1") {
        BAR_TEXT = true;
    }
    if (ComboBox1 == "APPLICATOR2") {
        BAR_TEXT = true;
    }
    if (chuoiNhap == BC_MAST) {
        BAR_TEXT = true;
    }

    if (BAR_TEXT == false) {
        // $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(chuoiNhap);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_ERROR/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C02_ERROR_DataGridView = data.DataGridView;
                if (BaseResult.C02_ERROR_DataGridView.length <= 0) {
                    IsCheck == false;
                   // ShowMessage("오류가 발생 하였습니다.(바코드 이력 없음). BARCODE Một lỗi đã xảy ra.","error");
                }
                if (IsCheck == true) {
                    C02_ERROR_Button1_ClickSub001();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C02_ERROR_Button1_ClickSub001();
    }
}
function C02_ERROR_Button1_ClickSub001() {
    let IsCheck = true;
    let chuoiNhap = "";
    let ComboBox1 = $("#C02_ERROR_ComboBox1").val();
    let TextBox1 = $("#C02_ERROR_TextBox1").val();
    if (ComboBox1 == "WIRE") {
        chuoiNhap = TextBox1.toUpperCase();
    }
    else {
        chuoiNhap = TextBox1.toUpperCase();
    }
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC.toUpperCase();
    switch (ComboBox1) {
        case "TERM1":
            try {
                let T1 = "";
                if (chuoiNhap == BC_MAST) {
                    T1 = $("#LBT1S").val();
                }
                else {
                    T1 = chuoiNhap.substr(0, chuoiNhap.indexOf("$$"));
                }
                if ($("#C02_ERROR_LBT1S").val() == T1) {
                    $("#C02_ERROR_LBT1R").val("OK");
                    $("#C02_ERROR_LBR1").val("OK");
                    document.getElementById("C02_ERROR_LBR1").style.color = "green";
                    try {
                        let selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex;
                        document.getElementById("C02_ERROR_ComboBox1").selectedIndex = selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C02_ERROR_LBT1R").val("NG");
                    $("#C02_ERROR_LBR1").val("NG");
                    document.getElementById("C02_ERROR_LBR1").style.color = "red";
                }
                $("#C02_ERROR_LBT1V").val(T1);
                $("#C02_ERROR_TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "TERM2":
            try {
                let T2 = "";
                if (chuoiNhap == BC_MAST) {
                    T2 = $("#C02_ERROR_LBT2S").val();
                }
                else {
                    T2 = chuoiNhap.substr(0, chuoiNhap.indexOf("$$"));
                }
                if ($("#C02_ERROR_LBT2S").val() == T2) {
                    $("#C02_ERROR_LBT2R").val("OK");
                    $("#C02_ERROR_LBR2").val("OK");
                    document.getElementById("C02_ERROR_LBR2").style.color = "green";
                    try {
                        let selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex;
                        document.getElementById("C02_ERROR_ComboBox1").selectedIndex = selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C02_ERROR_LBT2R").val("NG");
                    $("#C02_ERROR_LBR2").val("NG");
                    document.getElementById("C02_ERROR_LBR2").style.color = "red";
                }
                $("#C02_ERROR_LBT2V").val(T2);
                $("#C02_ERROR_TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "APPLICATOR1":
            let A1 = chuoiNhap.substr(0, chuoiNhap.length - 1);
            let LBA1S = $("#C02_ERROR_LBA1S").val();
            if (LBA1S == A1) {
                $("#C02_ERROR_LBA1R").val("OK");
                let LBA1SEQ = chuoiNhap.substr(chuoiNhap.length - 1, 1);
                $("#C02_ERROR_LBA1SEQ").val(LBA1SEQ);
                let ASC_SEQ = LBA1SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'red', displayLength: 6000 });
                }
                if (ASC_SEQ >= 90) {
                    IsCheck == false;
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'red', displayLength: 6000 });
                }
                if (IsCheck == true) {
                    LBA1SEQ = $("#C02_ERROR_LBA1SEQ").val();
                    let Label1 = $("#C02_ERROR_Label1").val();
                    // $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A1);
                    BaseParameter.ListSearchString.push(LBA1SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C02_ERROR/Button1_ClickSub001";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {

                            BaseResult.C02_ERROR_DataGridView4 = data.DataGridView4;
                            BaseResult.C02_ERROR_DataGridView5 = data.DataGridView5;
                            BaseResult.C02_ERROR_DataGridView6 = data.DataGridView6;
                            if (BaseResult.C02_ERROR_DataGridView4.length <= 0) {
                                if (BaseResult.C02_ERROR_DataGridView5.length <= 0) {
                                    IsCheck == false;
                                    M.toast({ html: "Barcode Please Check Again. & NOT add APPLICATOR data", classes: 'red', displayLength: 6000 });
                                    $("#C02_ERROR_LBA1R").val("Not data");
                                    $("#C02_ERROR_LBA1V").val(A1);
                                    $("#C02_ERROR_TextBox1").val("");
                                }
                                else {
                                    if (BaseResult.C02_ERROR_DataGridView6.length > 0) {
                                        $("#C02_ERROR_LBR11").val(BaseResult.C02_ERROR_DataGridView6[0].WK_CNT);
                                        document.getElementById("C02_ERROR_LBR11").style.color = "green";
                                    }
                                }
                            }
                            else {
                                $("#C02_ERROR_LBR11").val(BaseResult.C02_ERROR_DataGridView4[0].WK_CNT);
                                document.getElementById("C02_ERROR_LBR11").style.color = "green";
                                $("#C02_ERROR_LBA1IDX").val(BaseResult.C02_ERROR_DataGridView4[0].TOOL_IDX);
                            }
                            if (IsCheck == true) {
                                document.getElementById("C02_ERROR_ComboBox1").selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex + 1;
                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
            else {
                $("#C02_ERROR_LBA1R").val("NG");
                document.getElementById("C02_ERROR_LBR11").style.color = "red";
            }
            $("#C02_ERROR_LBA1V").val(A1);
            $("#C02_ERROR_TextBox1").val("");
            break;
        case "APPLICATOR2":
            let A2 = chuoiNhap.substr(0, chuoiNhap.length - 1);
            let LBA2S = $("#C02_ERROR_LBA2S").val();
            if (LBA2S == A2) {
                $("#C02_ERROR_LBA2R").val("OK");
                let LBA2SEQ = chuoiNhap.substr(chuoiNhap.length - 1, 1);
                $("#C02_ERROR_LBA2SEQ").val(LBA2SEQ);
                let ASC_SEQ = LBA2SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'red', displayLength: 6000 });
                }
                if (ASC_SEQ >= 90) {
                    IsCheck == false;
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'red', displayLength: 6000 });
                }
                if (IsCheck == true) {
                    LBA2SEQ = $("#C02_ERROR_LBA2SEQ").val();
                    let Label1 = $("#C02_ERROR_Label1").val();
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A2);
                    BaseParameter.ListSearchString.push(LBA2SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C02_ERROR/Button1_ClickSub002";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            BaseResult.C02_ERROR_DataGridView7 = data.DataGridView7;
                            BaseResult.C02_ERROR_DataGridView8 = data.DataGridView8;
                            BaseResult.C02_ERROR_DataGridView9 = data.DataGridView9;
                            if (BaseResult.C02_ERROR_DataGridView7.length <= 0) {
                                if (BaseResult.C02_ERROR_DataGridView8.length <= 0) {
                                    IsCheck == false;
                                    M.toast({ html: "Barcode Please Check Again. & NOT add APPLICATOR data", classes: 'red', displayLength: 6000 });
                                    $("#C02_ERROR_LBA2R").val("Not data");
                                    $("#C02_ERROR_LBA2V").val(A2);
                                    $("#C02_ERROR_TextBox1").val("");
                                }
                                else {
                                    if (BaseResult.C02_ERROR_DataGridView9.length > 0) {
                                        $("#C02_ERROR_LBR22").val(BaseResult.C02_ERROR_DataGridView9[0].WK_CNT);
                                        document.getElementById("C02_ERROR_LBR22").style.color = "green";
                                    }
                                }
                            }
                            else {
                                $("#C02_ERROR_LBR22").val(BaseResult.C02_ERROR_DataGridView7[0].WK_CNT);
                                document.getElementById("C02_ERROR_LBR22").style.color = "green";
                                $("#C02_ERROR_LBA2IDX").val(BaseResult.C02_ERROR_DataGridView7[0].TOOL_IDX);
                            }
                            if (IsCheck == true) {
                                document.getElementById("C02_ERROR_ComboBox1").selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex + 1;
                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
            else {
                $("#C02_ERROR_LBA2R").val("NG");
                document.getElementById("C02_ERROR_LBR22").style.color = "red";
            }
            $("#C02_ERROR_LBA2V").val(A2);
            $("#C02_ERROR_TextBox1").val("");
            break;
        case "SEAL1":
            try {
                let S1 = "";
                if (chuoiNhap == BC_MAST) {
                    S1 = $("#C02_ERROR_LBS1S").val();
                }
                else {
                    S1 = chuoiNhap.substr(0, chuoiNhap.indexOf("$$"));
                }
                if ($("#C02_ERROR_LBS1S").val() == S1) {
                    $("#C02_ERROR_LBS1R").val("OK");
                    $("#C02_ERROR_LBR3").val("OK");
                    document.getElementById("C02_ERROR_LBR3").style.color = "green";
                    try {
                        let selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex;
                        document.getElementById("C02_ERROR_ComboBox1").selectedIndex = selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C02_ERROR_LBS1R").val("NG");
                    $("#C02_ERROR_LBR3").val("NG");
                    document.getElementById("C02_ERROR_LBR3").style.color = "red";
                }
                $("#C02_ERROR_LBS1V").val(S1);
                $("#C02_ERROR_TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again." + e);
            }
            break;
        case "SEAL2":
            try {
                let S2 = "";
                if (chuoiNhap == BC_MAST) {
                    S2 = $("#C02_ERROR_LBS2S").val();
                }
                else {
                    S2 = chuoiNhap.substr(0, chuoiNhap.indexOf("$$"));
                }
                if ($("#C02_ERROR_LBS2S").val() == S2) {
                    $("#C02_ERROR_LBS2R").val("OK");
                    $("#C02_ERROR_LBR4").val("OK");
                    document.getElementById("C02_ERROR_LBR4").style.color = "green";
                    try {
                        let selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex;
                        document.getElementById("C02_ERROR_ComboBox1").selectedIndex = selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C02_ERROR_LBS2R").val("NG");
                    $("#C02_ERROR_LBR4").val("NG");
                    document.getElementById("C02_ERROR_LBR4").style.color = "red";
                }
                $("#C02_ERROR_LBS2V").val(S2);
                $("#C02_ERROR_TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "WIRE":
            try {
                let WW = "";
                if (chuoiNhap == BC_MAST) {
                    WW = $("#C02_ERROR_LBWS").val().toUpperCase();
                }
                else {
                    WW = chuoiNhap.substr(0, chuoiNhap.indexOf("$$")).toUpperCase();
                }
                if ($("#C02_ERROR_LBWS").val().toUpperCase() == WW) {
                    $("#C02_ERROR_LBWR").val("OK");
                    $("#C02_ERROR_LBR5").val("OK");
                    document.getElementById("C02_ERROR_LBR5").style.color = "green";
                    document.getElementById("C02_ERROR_LBWR").style.color = "green";
                    try {
                        let selectedIndex = document.getElementById("C02_ERROR_ComboBox1").selectedIndex;
                        document.getElementById("C02_ERROR_ComboBox1").selectedIndex = selectedIndex + 1;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#C02_ERROR_LBWR").val("NG");
                    $("#C02_ERROR_LBR5").val("NG");
                    document.getElementById("C02_ERROR_LBR5").style.color = "red";
                    document.getElementById("C02_ERROR_LBWR").style.color = "red";
                }
                $("#C02_ERROR_LBWV").val(WW);
                $("#C02_ERROR_TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
    }
    $("#C02_ERROR_TextBox1").focus();
}
$("#C02_ERROR_Button2").click(function (e) {
    C02_ERROR_Button2_Click();
});
function C02_ERROR_Button2_Click() {
    let AA1 = document.getElementById("C02_ERROR_CHBT1").checked;
    let AA2 = document.getElementById("C02_ERROR_CHBT2").checked;
    let AA3 = document.getElementById("C02_ERROR_CHBA1").checked;
    let AA4 = document.getElementById("C02_ERROR_CHBA2").checked;
    let AA5 = document.getElementById("C02_ERROR_CHBS1").checked;
    let AA6 = document.getElementById("C02_ERROR_CHBS2").checked;
    let AA7 = document.getElementById("C02_ERROR_CHBW").checked;

    let BB1 = false;
    let BB2 = false;
    let BB3 = false;
    let BB4 = false;
    let BB5 = false;
    let BB6 = false;
    let BB7 = false;

    if (AA1 == true) {
        if ($("#C02_ERROR_LBT1R").val() == "OK") {
            BB1 = true;
        }
    }
    else {
        AA1 = true;
        BB1 = true;
    }
    if (AA2 == true) {
        if ($("#C02_ERROR_LBT2R").val() == "OK") {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA3 == true) {
        if ($("#C02_ERROR_LBA1R").val() == "OK") {
            BB3 = true;
        }
    }
    else {
        AA3 = true;
        BB3 = true;
    }
    if (AA4 == true) {
        if ($("#C02_ERROR_LBA2R").val() == "OK") {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if (AA5 == true) {
        if ($("#C02_ERROR_LBS1R").val() == "OK") {
            BB5 = true;
        }
    }
    else {
        AA5 = true;
        BB5 = true;
    }
    if (AA6 == true) {
        if ($("#C02_ERROR_LBS2R").val() == "OK") {
            BB6 = true;
        }
    }
    else {
        AA6 = true;
        BB6 = true;
    }
    if (AA7 == true) {
        if ($("#C02_ERROR_LBWR").val() == "OK") {
            BB7 = true;
        }
    }
    else {
        AA7 = true;
        BB7 = true;
    }
    if ((AA1 == true) && (BB1 == true) && (AA2 == true) && (BB2 == true) && (AA3 == true) && (BB3 == true) && (AA4 == true) && (BB4 == true) && (AA5 == true) && (BB5 == true) && (AA6 == true) && (BB6 == true) && (AA7 == true) && (BB7 == true)) {
        let Label1 = $("#C02_ERROR_Label1").val();
        let MCbox = $("#C02_LIST_MCbox").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(Label1);
        BaseParameter.ListSearchString.push(MCbox);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_ERROR/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                C02_START_V2_C_EXIT = false;
                C02_ERROR_Buttonclose_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#C02_ERROR_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_ERROR_TextBox1_KeyDown();
    }
});
function C02_ERROR_TextBox1_KeyDown() {
    C02_ERROR_Button1_Click();
}
$("#C02_ERROR_Buttonclose").click(function (e) {
    C02_ERROR_Buttonclose_Click();
});
function C02_ERROR_Buttonclose_Click() {
    C02_START_V2_C_EXIT = false;
    C02_ERROR_FormClosed();
}
function C02_ERROR_FormClosed() {
    let MCbox = $("#MCbox").val();
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [],
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };
    BaseParameter.ListSearchString.push(C02_ERROR_LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C02_ERROR/C02_ERROR_FormClosed", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            localStorage.setItem("SettingsNON_OPER_CHK", false);
            C02_ERROR_EW_TIME(1);
            $("#C02_ERROR").modal("close");
            $("#BackGround").css("display", "none");

            $("#C02_START_V2").modal("open");
            C02_START_V2_PageLoad();
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
        });
}

let C02_COUNT_DataGridView1RowIndex;
function C02_COUNT_PageLoad() {
    BaseResult.C02_COUNT_DataGridView1 = new Object();
    BaseResult.C02_COUNT_DataGridView1 = [];
    C02_COUNT_Load();
}
function C02_COUNT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_COUNT/C02_COUNT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_COUNT_DataGridView1 = data.DataGridView1;
            C02_COUNT_DataGridView1Render();
            document.getElementById("C02_COUNT_RadioButton1").checked = true;
            C02_COUNT_RadioButton2_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_COUNT_Buttonclose").click(function (e) {
    C02_COUNT_Buttonclose_Click();
});
function C02_COUNT_Buttonclose_Click() {
    $("#C02_COUNT").modal("close");
}
$("#C02_COUNT_RadioButton1").click(function (e) {
    C02_COUNT_RadioButton2_Click();
});
$("#C02_COUNT_RadioButton2").click(function (e) {
    C02_COUNT_RadioButton2_Click();
});
function C02_COUNT_RadioButton2_Click() {
    BaseResult.C02_COUNT_DataGridView1 = [];
    C02_COUNT_DataGridView1Render();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.RadioButton1 = document.getElementById("C02_COUNT_RadioButton1").checked;
    BaseParameter.RadioButton2 = document.getElementById("C02_COUNT_RadioButton2").checked;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_COUNT/RadioButton2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_COUNT_DataGridView1 = data.DataGridView1;
            C02_COUNT_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_COUNT_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C02_COUNT_DataGridView1) {
            if (BaseResult.C02_COUNT_DataGridView1.length > 0) {
                C02_COUNT_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C02_COUNT_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C02_COUNT_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].SUM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].Stay + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].Working + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].Complete + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_COUNT_DataGridView1[i].Close + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C02_COUNT_DataGridView1").innerHTML = HTML;
}
function C02_COUNT_DataGridView1_SelectionChanged(i) {
    C02_COUNT_DataGridView1RowIndex = i;
}
let C02_COUNT_DataGridView1Table = document.getElementById("C02_COUNT_DataGridView1Table");
C02_COUNT_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "MC";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C02_COUNT_DataGridView1, key, text, IsTableSort);
        C02_COUNT_DataGridView1Render();
    }
});


let C02_REPRINT_DataGridView1RowIndex;
function C02_REPRINT_PageLoad() {
    BaseResult.C02_REPRINT_DataGridView1 = new Object();
    BaseResult.C02_REPRINT_DataGridView1 = [];

    C02_REPRINT_Load();
}

//in lại tem nhãn cutting
function C02_REPRINT_Load() {
    let Label4 = $("#C02_REPRINT_Label4").val();
    let WHERE_TEXT = $("#C02_REPRINT_WHERE_TEXT").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label4);
    BaseParameter.ListSearchString.push(WHERE_TEXT);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_REPRINT/C02_REPRINT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_REPRINT_DataGridView1 = data.DataGridView1;
            C02_REPRINT_DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_REPRINT_Buttonprint").click(function (e) {
    C02_REPRINT_Buttonprint_Click();
});
function C02_REPRINT_Buttonprint_Click() {
    let IsCheck = true;
    if (BaseResult.C02_REPRINT_DataGridView1.length <= 0) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    let PRT_1 = $("#C02_REPRINT_PRT_1").val();
    if (PRT_1.length < 8) {
        IsCheck = false;
        alert("NOT Barcode.");
    }
    if (IsCheck == true) {
        C02_REPRINT_PrintDocument1_PrintPage();
    }
}
function C02_REPRINT_PrintDocument1_PrintPage() {
    let BARCODE_QR = $("#C02_REPRINT_PRT_1").val();
    let PR1 = $("#C02_REPRINT_PRT_20").val();
    let PR4 = $("#C02_REPRINT_PRT_6").val();
    let PR5 = $("#C02_REPRINT_PRT_3").val();
    let PR6 = $("#C02_REPRINT_PRT_9").val();
    let PR7 = $("#C02_REPRINT_PRT_7").val();
    let PR8 = $("#C02_REPRINT_PRT_8").val();
    let PR9 = $("#C02_REPRINT_PRT_10").val();
    let PR10 = $("#C02_REPRINT_PRT_15").val();
    let PR11 = $("#C02_REPRINT_PRT_12").val();
    let PR12 = $("#C02_REPRINT_PRT_17").val();
    let PR13 = $("#C02_REPRINT_PRT_13").val();
    let PR14 = $("#C02_REPRINT_PRT_18").val();
    let PR15 = $("#C02_REPRINT_PRT_14").val();
    let PR16 = $("#C02_REPRINT_PRT_19").val();
    let PR17 = $("#C02_REPRINT_PRT_21").val();
    let PR20 = $("#C02_REPRINT_PRT_2").val();
    let PR21 = $("#C02_REPRINT_PRT_11").val();
    let PR22 = $("#C02_REPRINT_PRT_16").val();
    let PR25 = $("#C02_REPRINT_PRT_22").val();
    let PRT_3 = $("#C02_REPRINT_PRT_3").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(BARCODE_QR);
    BaseParameter.ListSearchString.push(PR1);
    BaseParameter.ListSearchString.push(PR4);
    BaseParameter.ListSearchString.push(PR5);
    BaseParameter.ListSearchString.push(PR6);
    BaseParameter.ListSearchString.push(PR7);
    BaseParameter.ListSearchString.push(PR8);
    BaseParameter.ListSearchString.push(PR9);
    BaseParameter.ListSearchString.push(PR10);
    BaseParameter.ListSearchString.push(PR11);
    BaseParameter.ListSearchString.push(PR12);
    BaseParameter.ListSearchString.push(PR13);
    BaseParameter.ListSearchString.push(PR14);
    BaseParameter.ListSearchString.push(PR15);
    BaseParameter.ListSearchString.push(PR16);
    BaseParameter.ListSearchString.push(PR17);
    BaseParameter.ListSearchString.push(PR20);
    BaseParameter.ListSearchString.push(PR21);
    BaseParameter.ListSearchString.push(PR22);
    BaseParameter.ListSearchString.push(PR25);
    BaseParameter.ListSearchString.push(PRT_3);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_REPRINT/PrintDocument1_PrintPage";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data) {
                if (data.Code) {
                    let url = data.Code;
                    OpenWindowByURL(url, 200, 200);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

$("#C02_REPRINT_Buttonclose").click(function (e) {
    C02_REPRINT_Buttonclose_Click();
});
function C02_REPRINT_Buttonclose_Click() {
    $("#C02_REPRINT").modal("close");
}
function C02_REPRINT_DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.C02_REPRINT_DataGridView1) {
            if (BaseResult.C02_REPRINT_DataGridView1.length > 0) {
                C02_REPRINT_DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.C02_REPRINT_DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='C02_REPRINT_DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].TORDER_BARCODENM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].SP_ST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.C02_REPRINT_DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("C02_REPRINT_DataGridView1").innerHTML = HTML;
}
function C02_REPRINT_DataGridView1_SelectionChanged(i) {
    C02_REPRINT_DataGridView1RowIndex = i;
    $("#C02_REPRINT_PRT_1").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].TORDER_BARCODENM);
    $("#C02_REPRINT_PRT_2").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].Barcode_SEQ);
    $("#C02_REPRINT_PRT_3").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].LEAD_NO);
    $("#C02_REPRINT_PRT_4").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].DT);
    $("#C02_REPRINT_PRT_5").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].MC);
    $("#C02_REPRINT_PRT_6").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].HOOK_RACK);
    $("#C02_REPRINT_PRT_7").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].TOT_QTY);
    $("#C02_REPRINT_PRT_8").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].BUNDLE_SIZE);
    $("#C02_REPRINT_PRT_9").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].WIRE);
    $("#C02_REPRINT_PRT_10").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].TERM1);
    $("#C02_REPRINT_PRT_11").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].STRIP1);
    $("#C02_REPRINT_PRT_12").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].SEAL1);
    $("#C02_REPRINT_PRT_13").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].CCH_W1);
    $("#C02_REPRINT_PRT_14").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].ICH_W1);
    $("#C02_REPRINT_PRT_15").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].TERM2);
    $("#C02_REPRINT_PRT_16").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].STRIP2);
    $("#C02_REPRINT_PRT_17").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].SEAL2);
    $("#C02_REPRINT_PRT_18").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].CCH_W2);
    $("#C02_REPRINT_PRT_19").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].ICH_W2);
    $("#C02_REPRINT_PRT_20").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].PROJECT);
    $("#C02_REPRINT_PRT_21").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].SP_ST);
    $("#C02_REPRINT_PRT_22").val(BaseResult.C02_REPRINT_DataGridView1[C02_REPRINT_DataGridView1RowIndex].ADJ_AF_QTY);
}

let C02_REPRINT_DataGridView1Table = document.getElementById("C02_REPRINT_DataGridView1Table");
C02_REPRINT_DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "TORDER_BARCODENM";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.C02_REPRINT_DataGridView1, key, text, IsTableSort);
        C02_REPRINT_DataGridView1Render();
    }
});

let C02_MT_ORDR = 0;
function C02_MT_PageLoad() {
    C02_MT_Load();
}
function C02_MT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C02_MT_ORDR);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_MT/C02_MT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_MT_Search = data.Search;
            $("#C02_MT_OR_IDX").val(C02_MT_ORDR);
            if (BaseResult.C02_MT_Search.length > 0) {
                $("#C02_MT_Label18").val("LEAD NAME : " + BaseResult.C02_MT_Search[0].LEAD_NO);
                $("#C02_MT_Label19").val("WORK QTY : " + BaseResult.C02_MT_Search[0].TOT_QTY);
                $("#C02_MT_Label9").val(BaseResult.C02_MT_Search[0].TERM1);
                $("#C02_MT_Label17").val(BaseResult.C02_MT_Search[0].T1_QTY);
                $("#C02_MT_Label8").val(BaseResult.C02_MT_Search[0].TERM2);
                $("#C02_MT_Label16").val(BaseResult.C02_MT_Search[0].T2_QTY);
                $("#C02_MT_Label7").val(BaseResult.C02_MT_Search[0].SEAL1);
                $("#C02_MT_Label15").val(BaseResult.C02_MT_Search[0].S1_QTY);
                $("#C02_MT_Label6").val(BaseResult.C02_MT_Search[0].SEAL2);
                $("#C02_MT_Label14").val(BaseResult.C02_MT_Search[0].S2_QTY);
                $("#C02_MT_Label5").val(BaseResult.C02_MT_Search[0].WIRE);
                $("#C02_MT_Label20").val(BaseResult.C02_MT_Search[0].WIRE_NM + " " + BaseResult.C02_MT_Search[0].W_Length + " mm");
                $("#C02_MT_Label13").val(BaseResult.C02_MT_Search[0].W_Length1);
                $("#C02_MT_T1_OR").val(BaseResult.C02_MT_Search[0].T1_ORDER);
                $("#C02_MT_T1_CP").val(BaseResult.C02_MT_Search[0].T1_COMP);
                $("#C02_MT_T2_OR").val(BaseResult.C02_MT_Search[0].T2_ORDER);
                $("#C02_MT_T2_CP").val(BaseResult.C02_MT_Search[0].T2_COMP);
                $("#C02_MT_S1_OR").val(BaseResult.C02_MT_Search[0].S1_ORDER);
                $("#C02_MT_S1_CP").val(BaseResult.C02_MT_Search[0].S1_COMP);
                $("#C02_MT_S2_OR").val(BaseResult.C02_MT_Search[0].S2_ORDER);
                $("#C02_MT_S2_CP").val(BaseResult.C02_MT_Search[0].S2_COMP);
                $("#C02_MT_WR_OR").val(BaseResult.C02_MT_Search[0].WR_ORDER);
                $("#C02_MT_WR_CP").val(BaseResult.C02_MT_Search[0].WR_COMP);

                try {
                    let T1_TXT = BaseResult.C02_MT_Search[0].TERM1;
                    if (T1_TXT.indexOf("(") > -1) {
                        document.getElementById("C02_MT_TM_CB1").checked = false;
                    }
                    else {
                        if (T1_TXT.length > 0) {
                            document.getElementById("C02_MT_TM_CB1").checked = true;
                        }
                        else {
                            document.getElementById("C02_MT_TM_CB1").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_MT_TM_CB1").checked = false;
                }
                try {
                    let T2_TXT = BaseResult.C02_MT_Search[0].TERM2;
                    if (T2_TXT.indexOf("(") > -1) {
                        document.getElementById("C02_MT_TM_CB2").checked = false;
                    }
                    else {
                        if (T2_TXT.length > 0) {
                            document.getElementById("C02_MT_TM_CB2").checked = true;
                        }
                        else {
                            document.getElementById("C02_MT_TM_CB2").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_MT_TM_CB2").checked = false;
                }
                try {
                    let S1_TXT = BaseResult.C02_MT_Search[0].SEAL1;
                    if (S1_TXT.indexOf("(") > -1) {
                        document.getElementById("C02_MT_SL_CB1").checked = false;
                    }
                    else {
                        if (S1_TXT.length > 0) {
                            document.getElementById("C02_MT_SL_CB1").checked = true;
                        }
                        else {
                            document.getElementById("C02_MT_SL_CB1").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_MT_SL_CB1").checked = false;
                }
                try {
                    let S2_TXT = BaseResult.C02_MT_Search[0].SEAL2;
                    if (S2_TXT.indexOf("(") > -1) {
                        document.getElementById("C02_MT_SL_CB2").checked = false;
                    }
                    else {
                        if (S2_TXT.length > 0) {
                            document.getElementById("C02_MT_SL_CB2").checked = true;
                        }
                        else {
                            document.getElementById("C02_MT_SL_CB2").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_MT_SL_CB2").checked = false;
                }
                try {
                    let WR_TXT = BaseResult.C02_MT_Search[0].WIRE;
                    if (WR_TXT.indexOf("(") > -1) {
                        document.getElementById("C02_MT_WR_CB").checked = false;
                    }
                    else {
                        if (WR_TXT.length > 0) {
                            document.getElementById("C02_MT_WR_CB").checked = true;
                        }
                        else {
                            document.getElementById("C02_MT_WR_CB").checked = false;
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_MT_WR_CB").checked = false;
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_MT_Buttonsave").click(function (e) {
    C02_MT_Buttonsave_Click();
});

function C02_MT_Buttonsave_Click() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(C02_MT_ORDR);
    BaseParameter.ListSearchString.push(SettingsMC_NM);
    BaseParameter.TM_CB1 = document.getElementById("C02_MT_TM_CB1").checked;
    BaseParameter.TM_CB2 = document.getElementById("C02_MT_TM_CB2").checked;
    BaseParameter.SL_CB1 = document.getElementById("C02_MT_SL_CB1").checked;
    BaseParameter.SL_CB1 = document.getElementById("C02_MT_SL_CB1").checked;
    BaseParameter.WR_CB = document.getElementById("C02_MT_WR_CB").checked;

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_MT/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_MT_Search = data.Search;
            $("#BackGround").css("display", "none");
            alert("정상처리 되었습니다. Đã được lưu.");
            C02_MT_Buttonclose_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_MT_TM_CB1").click(function (e) {
    C02_MT_TM_CB1_Click();
});
function C02_MT_TM_CB1_Click() {
    let TM_CB1 = document.getElementById("C02_MT_TM_CB1").checked;
    if (TM_CB1 == true) {
        try {
            let T1_TXT = $("#C02_MT_Label9").val();
            if (T1_TXT.indexOf("(") > -1) {
                document.getElementById("C02_MT_TM_CB1").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("C02_MT_TM_CB1").checked = true;
                }
                else {
                    document.getElementById("C02_MT_TM_CB1").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#C02_MT_TM_CB2").click(function (e) {
    C02_MT_TM_CB2_Click();
});
function C02_MT_TM_CB2_Click() {
    let TM_CB2 = document.getElementById("C02_MT_TM_CB2").checked;
    if (TM_CB2 == true) {
        try {
            let T1_TXT = $("#C02_MT_Label8").val();
            if (T1_TXT.indexOf("(") > -1) {
                document.getElementById("C02_MT_TM_CB2").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("C02_MT_TM_CB2").checked = true;
                }
                else {
                    document.getElementById("C02_MT_TM_CB2").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#C02_MT_SL_CB1").click(function (e) {
    C02_MT_SL_CB1_Click();
});
function C02_MT_SL_CB1_Click() {
    let SL_CB1 = document.getElementById("C02_MT_SL_CB1").checked;
    if (SL_CB1 == true) {
        try {
            let T1_TXT = $("#C02_MT_Label7").val();
            if (T1_TXT.indexOf("(") > -1) {
                document.getElementById("C02_MT_SL_CB1").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("C02_MT_SL_CB1").checked = true;
                }
                else {
                    document.getElementById("C02_MT_SL_CB1").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#C02_MT_SL_CB2").click(function (e) {
    C02_MT_SL_CB2_Click();
});
function C02_MT_SL_CB2_Click() {
    let SL_CB2 = document.getElementById("C02_MT_SL_CB2").checked;
    if (SL_CB2 == true) {
        try {
            let T1_TXT = $("#C02_MT_Label6").val();
            if (T1_TXT.indexOf("(") > -1) {
                document.getElementById("C02_MT_SL_CB2").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("C02_MT_SL_CB2").checked = true;
                }
                else {
                    document.getElementById("C02_MT_SL_CB2").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}
$("#C02_MT_Label5").click(function (e) {
    C02_MT_Label5_Click();
});
function C02_MT_Label5_Click() {
    let WR_CB = document.getElementById("C02_MT_WR_CB").checked;
    if (WR_CB == true) {
        try {
            let T1_TXT = $("#C02_MT_Label5").val();
            if (T1_TXT.indexOf("(") > -1) {
                document.getElementById("C02_MT_WR_CB").checked = false;
            }
            else {
                if (T1_TXT.length > 0) {
                    document.getElementById("C02_MT_WR_CB").checked = true;
                }
                else {
                    document.getElementById("C02_MT_WR_CB").checked = false;
                }
            }
        }
        catch (e) {
        }
    }
}

$("#C02_MT_Buttonclose").click(function (e) {
    C02_MT_Buttonclose_Click();
});
function C02_MT_Buttonclose_Click() {
    $("#C02_MT").modal("close");
    C02_LIST_Buttonfind_Click();
}


let C02_START_V2_C_EXIT;
let C02_START_V2_C_USER;
let C02_START_V2_Timer1;
let C02_START_V2_Timer2;
let C02_START_V2_Timer3;
let C02_START_V2_Timer4;
let C02_START_V2_EW_TIME_Timer1;
let C02_START_V2_MAIN_CLOSE;
let C02_START_V2_TI_CONT = 70;
let C02_START_V2_BARCODE_QR;
let C02_START_V2_SPC_EXIT;
let C02_START_V2_StartTime;
let C02_START_V2_RunTime;
let IsC02_SPC_PageLoad = false;
let C02_CURRENT_TROLLEY = localStorage.getItem("C02_CURRENT_TROLLEY") || "";
$("#C02_START_V2_BT_TROLLEY").click(function (e) {
    C02_START_V2_ChangeTrolley_Click();
});
function C02_START_V2_ChangeTrolley_Click() {
    C02_SCAN_TROLLEY_Open(function (trolleyCode) {
        if (trolleyCode) {
            // Có trolley code
            C02_CURRENT_TROLLEY = trolleyCode;
            localStorage.setItem("C02_CURRENT_TROLLEY", trolleyCode);
            $("#C02_START_V2_TROLLEY_CODE").val(trolleyCode);
            M.toast({ html: "Đã đổi Trolley: " + trolleyCode, classes: 'green', displayLength: 3000 });
        } else {
            // Bỏ qua - xóa trolley
            C02_CURRENT_TROLLEY = null;
            localStorage.removeItem("C02_CURRENT_TROLLEY");
            $("#C02_START_V2_TROLLEY_CODE").val("");
            M.toast({ html: "Đã bỏ qua Trolley", classes: 'orange', displayLength: 3000 });
        }
    });
}
function C02_SCAN_TROLLEY_Open(callback) {
    C02_CURRENT_TROLLEY = null;
    $("#C02_TROLLEY_INPUT").val("");
    $("#C02_TROLLEY_MSG").html("").removeClass("green-text red-text");
    $("#C02_TROLLEY_BTN_CONFIRM").prop("disabled", true);

    window.C02_TROLLEY_CALLBACK = callback;

    $("#C02_SCAN_TROLLEY").modal({
        dismissible: false,
        onOpenEnd: function () {
            $("#C02_TROLLEY_INPUT").focus();
        }
    });
    $("#C02_SCAN_TROLLEY").modal("open");
}
function C02_SCAN_TROLLEY_Validate(trolleyCode) {
    if (!trolleyCode || trolleyCode.trim() === "") {
        $("#C02_TROLLEY_MSG").html("").removeClass("green-text red-text");
        $("#C02_TROLLEY_BTN_CONFIRM").prop("disabled", true);
        return;
    }

    let formData = new FormData();
    formData.append("TrolleyCode", trolleyCode.trim());

    fetch("/C02_LIST/ValidateTrolley", {
        method: "POST",
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                C02_CURRENT_TROLLEY = trolleyCode.trim();
                $("#C02_TROLLEY_MSG")
                    .html('<i class="material-icons tiny" style="vertical-align:middle;">check_circle</i> ' + data.Location)
                    .removeClass("red-text")
                    .addClass("green-text");
                $("#C02_TROLLEY_BTN_CONFIRM").prop("disabled", false);
            } else {
                C02_CURRENT_TROLLEY = null;
                $("#C02_TROLLEY_MSG")
                    .html('<i class="material-icons tiny" style="vertical-align:middle;">error</i> ' + (data.Error || "Trolley không tồn tại!"))
                    .removeClass("green-text")
                    .addClass("red-text");
                $("#C02_TROLLEY_BTN_CONFIRM").prop("disabled", true);
            }
        })
        .catch(err => {
            C02_CURRENT_TROLLEY = null;
            $("#C02_TROLLEY_MSG")
                .html('<i class="material-icons tiny" style="vertical-align:middle;">error</i> Lỗi kết nối!')
                .removeClass("green-text")
                .addClass("red-text");
            $("#C02_TROLLEY_BTN_CONFIRM").prop("disabled", true);
        });
}
function C02_SCAN_TROLLEY_Confirm() {
    C02_CURRENT_TROLLEY = $("#C02_TROLLEY_INPUT").val().trim();
    localStorage.setItem("C02_CURRENT_TROLLEY", C02_CURRENT_TROLLEY);
    $("#C02_SCAN_TROLLEY").modal("close");

    if (typeof window.C02_TROLLEY_CALLBACK === "function") {
        window.C02_TROLLEY_CALLBACK(C02_CURRENT_TROLLEY);
    }
}

function C02_SCAN_TROLLEY_Skip() {
    C02_CURRENT_TROLLEY = null;
    $("#C02_SCAN_TROLLEY").modal("close");
    if (typeof window.C02_TROLLEY_CALLBACK === "function") {
        window.C02_TROLLEY_CALLBACK(null);
    }
}
function C02_START_V2_PageLoad() {
    let IsCheck = true;
    document.getElementById("C02_START_V2_SPC1").innerHTML = "First";
    document.getElementById("C02_START_V2_SPC2").innerHTML = "Middle";
    document.getElementById("C02_START_V2_SPC3").innerHTML = "End";
    document.getElementById("C02_START_V2_SPC1").disabled = true;
    document.getElementById("C02_START_V2_SPC2").disabled = true;
    document.getElementById("C02_START_V2_BT_APP1").disabled = true;
    document.getElementById("C02_START_V2_BT_APP2").disabled = true;
    document.getElementById("C02_START_V2_Buttonprint").disabled = true;

    if (!C02_CURRENT_TROLLEY) {
        C02_CURRENT_TROLLEY = localStorage.getItem("C02_CURRENT_TROLLEY") || "";
    }
    $("#C02_START_V2_TROLLEY_CODE").val(C02_CURRENT_TROLLEY || "");

    if (C02_START_V2_C_EXIT == true) {
        IsCheck = false;
        C02_START_V2_Buttonclose_Click();
    }
    if (IsCheck == true) {
        C02_START_V2_MAIN_CLOSE = false;
        C02_START_V2_C02_start_Load();
        C02_START_V2_ORDER_LOAD(0);
    }
}
function C02_START_V2_C02_start_Load() {
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    let Label8 = $("#C02_START_V2_Label8").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(SettingsMC_NM);
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/C02_start_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_START_V2_Label18").change(function (e) {
    C02_START_V2_Label18_TextChanged();
});
function C02_START_V2_Label18_TextChanged() {
    let aaw = $("#C02_START_V2_Label18").val();
    aaw = aaw.trim();
    let bbw = aaw.substr(aaw.indexOf("  "));
    bbw = bbw.trim();
    let ccw = bbw.substr(bbw.indexOf("  "));
    $("#C02_START_V2_WIRE_Length").val(ccw);
}
function C02_START_V2_Timer1StartInterval() {
    C02_START_V2_Timer1 = setInterval(function () {
        C02_START_V2_Timer1_Tick();
    }, 500);
}
function C02_START_V2_Timer1_Tick() {
    if (C02_START_V2_TI_CONT >= 30) {
        document.getElementById("C02_START_V2_Buttonprint").disabled = false;
        document.getElementById("C02_START_V2_Buttonprint").innerHTML = "Complete / Print";
        clearInterval(C02_START_V2_Timer1);
    }
    else {
        if (C02_START_V2_TI_CONT > 0) {
            C02_START_V2_TI_CONT = C02_START_V2_TI_CONT + 1;
            document.getElementById("C02_START_V2_Buttonprint").innerHTML =
                "Complete(Barcode Reading) (" + C02_START_V2_TI_CONT + ")";
            document.getElementById("C02_START_V2_Buttonprint").disabled = true; // 🚨 giữ khóa
        }
    }
}
function C02_START_V2_Timer2StartInterval() {
    C02_START_V2_Timer2 = setInterval(function () {
        C02_START_V2_Timer2_Tick();
    }, 1000);
}
function C02_START_V2_Timer2_Tick() {
    let now = new Date();
    let NOW_HH = now.getHours();
    let NOW_DD = now.getDate();
    let NOW_DATE_S;
    let NOW_DATE_E;
    if (NOW_DD == now.getDate()) {
        if (NOW_HH < 6) {
            let nowSub = now.setDate(now.getDate() - 1);
            NOW_DATE_S = new Date(nowSub.getFullYear(), nowSub.getMonth(), nowSub.getDate(), 6, 0, 0);
            NOW_DATE_E = new Date(nowSub.getFullYear(), nowSub.getMonth(), nowSub.getDate(), 6, 0, 0);
        }
        else {
            NOW_DATE_S = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 6, 0, 0);
            NOW_DATE_E = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 6, 0, 0);
        }
    }
    NOW_DATE_E.setDate(NOW_DATE_E.getDate() + 1);
    C02_START_V2_RunTime = new Date();
    let myTimeSpan;
    let Text_time = $("#C02_START_V2_BI_STOPTIME").val();
    let hh_time1 = Text_time.substr(0, 2);
    let mm_time1 = Text_time.substr(3, 2);
    let ss_time2 = Text_time.substr(6, 2);
    let hh_time = Number(Text_time.split(":")[0]) * 60 * 60;
    let mm_time = Number(Text_time.split(":")[1]) * 60;
    let ss_time = Number(Text_time.split(":")[2]);
    let tot_time = hh_time + mm_time + ss_time;
    myTimeSpan = C02_START_V2_RunTime - C02_START_V2_StartTime;
    myTimeSpan = Math.floor(myTimeSpan / 1000) - tot_time;
    let BI_RTIME = ConvertSecondToString(myTimeSpan, ":");
    $("#C02_START_V2_BI_RTIME").val(BI_RTIME);
    if (myTimeSpan < 0) {
        document.getElementById("C02_START_V2_BI_RTIME").style.color = "red";
        document.getElementById("C02_START_V2_BI_RCK").style.color = "red";
        $("#C02_START_V2_BI_RCK").val("-");
    }
    else {
        document.getElementById("C02_START_V2_BI_RTIME").style.color = "black";
        document.getElementById("C02_START_V2_BI_RCK").style.color = "black";
        $("#C02_START_V2_BI_RCK").val("+");
    }
    let M_STIME = new Date(NOW_DATE_S.getFullYear(), NOW_DATE_S.getMonth(), NOW_DATE_S.getDate(), 6, 0, 0);
    let M_SPAN = C02_START_V2_RunTime - M_STIME;
    M_SPAN = Math.floor(M_SPAN / 1000);

    let DTIME11 = M_SPAN;
    let DTIME15 = DTIME11;

    let DTIME21 = $("#C02_START_V2_BI_RTIME").val();
    let DTIME22 = Number(DTIME21.split(":")[0]) * 60 * 60;
    let DTIME23 = Number(DTIME21.split(":")[1]) * 60;
    let DTIME24 = Number(DTIME21.split(":")[2]);
    let DTIME25 = DTIME22 + DTIME23 + DTIME24;
    let BI_RVA = ((DTIME25 / DTIME15) * 100).toFixed(2) + " %";
    $("#C02_START_V2_BI_RVA").val(BI_RVA);
    try {
        if ($("#C02_START_V2_BI_RCK").val() == "+") {
            let UPH_T1 = $("#C02_START_V2_BI_RTIME").val();
            let UPH_H1 = Number(UPH_T1.split(":")[0]);
            let UPH_M1 = Number(UPH_T1.split(":")[1]);
            let BI_WQTY = Number($("#C02_START_V2_BI_WQTY").val());
            let BI_UPH = 0;
            try {
                let division = (UPH_H1 + Math.floor((UPH_M1 / 60)));
                if (division != 0) {
                    BI_UPH = Math.floor(BI_WQTY / division);
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#C02_START_V2_BI_UPH").val(BI_UPH);
        }
    }
    catch (e) {
        console.log(e);
    }
}

function C02_START_V2_Timer3StartInterval() {
    C02_START_V2_Timer3 = setInterval(function () {
        C02_START_V2_Timer3_Tick();
    }, 10000);
}
function C02_START_V2_Timer3_Tick() {
    C02_START_V2_OPER_TIME();
}


function C02_START_V2_EW_TIME_Timer1StartInterval() {
    C02_START_V2_EW_TIME_Timer1 = setInterval(function () {
        C02_START_V2_EW_TIME_Timer1_Tick();
    }, 60000);
}
function C02_START_V2_EW_TIME_Timer1_Tick() {
    C02_START_V2_EW_TIME();
}
function C02_START_V2_EW_TIME() {
    WORING_IDX = localStorage.getItem("WORING_IDX");
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(WORING_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_START_V2_BT_APP1").click(function (e) {
    C02_START_V2_Button4_Click();
});
function C02_START_V2_Button4_Click() {
    $("#C02_APPLICATION_Label3").val("APPLICATION #1");
    $("#C02_APPLICATION_Label1").val($("#C02_START_V2_VLA1").val());
    $("#C02_APPLICATION_Label2").val($("#C02_START_V2_VLA11").val());
    $("#C02_APPLICATION").modal("open");
    C02_APPLICATION_PageLoad();
}

$("#C02_START_V2_BT_APP2").click(function (e) {
    C02_START_V2_Button5_Click();
});
function C02_START_V2_Button5_Click() {

    $("#C02_APPLICATION_Label3").val("APPLICATION #2");
    $("#C02_APPLICATION_Label1").val($("#C02_START_V2_VLA2").val());
    $("#C02_APPLICATION_Label2").val($("#C02_START_V2_VLA22").val());
    $("#C02_APPLICATION").modal("open");
    C02_APPLICATION_PageLoad();
}
$("#C02_START_V2_SPC1").click(function (e) {
    C02_START_V2_SPC1_Click();
});
function C02_START_V2_SPC1_Click() {

    $("#C02_SPC_Label10").val("First");
    $("#C02_SPC").modal("open");
    C02_SPC_PageLoad();
}

$("#C02_START_V2_SPC2").click(function (e) {
    C02_START_V2_SPC2_Click();
});
function C02_START_V2_SPC2_Click() {
    let TOT_QTY = BaseResult.C02_START_V2_Search[0].TOT_QTY;
    let PERFORMN = BaseResult.C02_START_V2_Search[0].PERFORMN;

    if (TOT_QTY > 501) {
        if (Math.floor(TOT_QTY / 2) <= PERFORMN) {

            $("#C02_SPC_Label10").val("Middle");
            $("#C02_SPC").modal("open");
            C02_SPC_PageLoad();
        }
    }
}
$("#C02_START_V2_SPC3").click(function (e) {
    C02_START_V2_SPC3_Click();
});
function C02_START_V2_SPC3_Click() {
    let AA = BaseResult.C02_START_V2_Search[0].TOT_QTY - BaseResult.C02_START_V2_Search[0].PERFORMN;
    let BB = AA - BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE;
    if (BB <= 0) {

        $("#C02_SPC_Label10").val("End");
        $("#C02_SPC").modal("open");
        C02_SPC_PageLoad();
    }
}

$("#C02_START_V2_Buttonprint").click(function (e) {
    e.preventDefault();
    var $btn = $(this);

    if ($btn.prop("disabled")) return;

    $btn.prop("disabled", true); // disable ngay khi bấm

    C02_START_V2_Buttonprint_Click()
        .catch(err => {
            console.error("Lỗi khi in:", err);
            M.toast({ html: "Có lỗi xảy ra khi in. Vui lòng thử lại.", classes: "red", displayLength: 6000 });
        })
        .finally(() => {
            $btn.prop("disabled", false); // chỉ bật lại 1 lần ở đây
        });
});

// in tem nhãn co cutting
function C02_START_V2_Buttonprint_Click() {
    try {
        IsC02_SPC_PageLoad = false;
        let IsCheck = true;

        //let trolleyCode = C02_CURRENT_TROLLEY || localStorage.getItem("C02_CURRENT_TROLLEY") || "";
        //if (!trolleyCode) {
        //    M.toast({ html: "Vui lòng scan Trolley trước khi in tem!", classes: "red", displayLength: 4000 });
        //    return Promise.resolve();
        //}

        if ($("#C02_START_V2_BARCODE_TEXT").val() == "None") {
            IsCheck = false;
            M.toast({ html: "NOT New Barcode. Please Check Again.", classes: "red", displayLength: 6000 });
            return Promise.resolve();
        }

        C02_START_V2_BARCODE_QR = $("#C02_START_V2_BARCODE_TEXT").val();
        let ATOOL1 = true, ATOOL2 = true;

        if (IsCheck) {
            let TOOLA1 = Number($("#C02_START_V2_TOOL_CONT1").val());
            let TOOLA2 = Number($("#C02_START_V2_TOOL_CONT2").val());
            let TOOLC1 = BaseResult?.C02_START_V2_Search?.[0]?.TOOL_MAX1 ?? 1000000;
            let TOOLC2 = BaseResult?.C02_START_V2_Search?.[0]?.TOOL_MAX2 ?? 1000000;

            if (TOOLA1 >= TOOLC1) {
                M.toast({ html: "Application #1 check(Check counter). Please Check Again.", classes: "red", displayLength: 6000 });
                return Promise.resolve();
            }
            if (TOOLA2 >= TOOLC2) {
                M.toast({ html: "Application #2 check(Check counter). Please Check Again.", classes: "red", displayLength: 6000 });
                return Promise.resolve();
            }

            C02_START_V2_SPC_EXIT = true;

            if (ATOOL1 || ATOOL2) {
                if (document.getElementById("C02_START_V2_SPC1").innerHTML == "First") {
                    IsC02_SPC_PageLoad = true;
                    C02_START_V2_SPC_EXIT = false;
                    $("#C02_SPC_Label10").val("First");
                    $("#C02_SPC").modal("open");
                    C02_SPC_PageLoad();
                    return Promise.resolve();
                }

                if (
                    BaseResult.C02_START_V2_Search[0].TOT_QTY <= BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE &&
                    BaseResult.C02_START_V2_Search[0].WORK_QTY < BaseResult.C02_START_V2_Search[0].TOT_QTY &&
                    document.getElementById("C02_START_V2_SPC3").innerHTML == "End"
                ) {
                    IsC02_SPC_PageLoad = true;
                    C02_START_V2_SPC_EXIT = false;
                    $("#C02_SPC_Label10").val("End");
                    $("#C02_SPC").modal("open");
                    C02_SPC_PageLoad();
                    return Promise.resolve();
                }

                if (
                    BaseResult.C02_START_V2_Search[0].TOT_QTY > 500 &&
                    BaseResult.C02_START_V2_Search[0].TOT_QTY / 2 <= BaseResult.C02_START_V2_Search[0].PERFORMN &&
                    document.getElementById("C02_START_V2_SPC2").innerHTML == "Middle"
                ) {
                    IsC02_SPC_PageLoad = true;
                    C02_START_V2_SPC_EXIT = false;
                    $("#C02_SPC_Label10").val("Middle");
                    $("#C02_SPC").modal("open");
                    C02_SPC_PageLoad();
                    return Promise.resolve();
                }

                let A_CSPC = BaseResult.C02_START_V2_Search[0].TOT_QTY - BaseResult.C02_START_V2_Search[0].PERFORMN;
                if (
                    BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE >= A_CSPC &&
                    document.getElementById("C02_START_V2_SPC3").innerHTML == "End"
                ) {
                    IsC02_SPC_PageLoad = true;
                    C02_START_V2_SPC_EXIT = false;
                    $("#C02_SPC_Label10").val("End");
                    $("#C02_SPC").modal("open");
                    C02_SPC_PageLoad();
                    return Promise.resolve();
                }

                if (!IsC02_SPC_PageLoad) {
                    return C02_START_V2_Buttonprint_ClickSub(); // return promise
                }
            }
        }

        return Promise.resolve();
    } catch (err) {
        return Promise.reject(err);
    }
}

// in tem cho đơn cắt
function C02_START_V2_Buttonprint_ClickSub() {
    return new Promise((resolve, reject) => {
        if (!C02_START_V2_SPC_EXIT) {
            M.toast({ html: "검사 누락. Kiểm tra mất tích.", classes: "red", displayLength: 6000 });
            return resolve();
        }

        try {
            let BaseParameter = {
                ListSearchString: [],
                USER_ID: GetCookieValue("UserID"),
                USER_IDX: GetCookieValue("USER_IDX")
            };

            let barcodePrint = $("#C02_START_V2_BARCODE_TEXT").val();
            if (barcodePrint.length > 10) {
                BaseParameter.ListSearchString.push($("#C02_START_V2_Label5").val());
                BaseParameter.ListSearchString.push(barcodePrint);
                BaseParameter.ListSearchString.push(SHIELDWIRE_CHK);
                BaseParameter.ListSearchString.push($("#C02_START_V2_VLA1").val());
                BaseParameter.ListSearchString.push($("#C02_START_V2_TOOL1_IDX").val());
                BaseParameter.ListSearchString.push(BaseResult?.C02_START_V2_Search?.[0]?.T1_CONT ?? "");
                BaseParameter.ListSearchString.push($("#C02_START_V2_Label4").val());
                BaseParameter.ListSearchString.push($("#C02_START_V2_VLA2").val());
                BaseParameter.ListSearchString.push($("#C02_START_V2_TOOL2_IDX").val());
                BaseParameter.ListSearchString.push(BaseResult?.C02_START_V2_Search?.[0]?.T2_CONT ?? "");
                BaseParameter.ListSearchString.push($("#C02_START_V2_Label8").val());
                BaseParameter.ListSearchString.push(BaseResult?.C02_START_V2_Search?.[0]?.PERFORMN ?? "");
                BaseParameter.ListSearchString.push($("#MCbox").val());
                BaseParameter.ListSearchString.push($("#C02_START_V2_ProductCode").val());
                let trolleyToSave = C02_CURRENT_TROLLEY || localStorage.getItem("C02_CURRENT_TROLLEY") || "";
                BaseParameter.ListSearchString.push(trolleyToSave);
                let formUpload = new FormData();
                formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
                let url = "/C02_START_V2/Buttonprint_Click";

                fetch(url, { method: "POST", body: formUpload })
                    .then((response) => response.json())
                    .then((data) => {
                        C02_START_V2_PrintDocument1_PrintPage();

                        // Kiểm tra null/undefined trước khi gán
                        BaseResult.C02_START_V2_DataGridView5 = data?.DataGridView5 ?? [];

                        let COM_COUNT = BaseResult.C02_START_V2_DataGridView5.length;
                        if (COM_COUNT === 0) {
                            C02_START_V2_Buttonclose_Click();
                            return resolve();
                        }

                        C02_START_V2_TI_CONT = 1;
                        C02_START_V2_Timer1StartInterval();

                        M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: "green", displayLength: 6000 });

                        try {
                            C02_START_V2_ORDER_LOAD(1);
                        } catch {
                            M.toast({ html: "Please Check Again.", classes: "red", displayLength: 6000 });
                        }

                        $("#BackGround").css("display", "none");
                        resolve();
                    })
                    .catch((err) => {
                        $("#BackGround").css("display", "none");
                        reject(err);
                    });
            }

        } catch (e) {
            reject(e);
        }
    });
}


//lệnh in tem nhãn cho cutting
function C02_START_V2_PrintDocument1_PrintPage() {
    if (window.isPrintingNow) return;
    window.isPrintingNow = true;
    setTimeout(() => window.isPrintingNow = false, 8000);

    let BARCODE_QR = $("#C02_START_V2_BARCODE_TEXT").val();
    if (BARCODE_QR.length > 10) {

        let PR1 = $("#C02_START_V2_Label43").val();
        let PR4 = $("#C02_START_V2_Label54").val();
        let PR5 = $("#C02_START_V2_Label4").val();
        let PR6 = $("#C02_START_V2_Label18").val();
        let PR7 = $("#C02_START_V2_Label48").val();
        let PR8 = $("#C02_START_V2_Label50").val();
        let PR9 = $("#C02_START_V2_VLT1").val();
        let PR10 = $("#C02_START_V2_VLT2").val();
        let PR11 = $("#C02_START_V2_VLS1").val();
        let PR12 = $("#C02_START_V2_VLS2").val();
        let PR13 = $("#C02_START_V2_ST_DCC1").val();
        let PR14 = $("#C02_START_V2_ST_DCC2").val();
        let PR15 = $("#C02_START_V2_ST_DIC1").val();
        let PR16 = $("#C02_START_V2_ST_DIC2").val();
        let PR17 = $("#C02_START_V2_Label42").val();
        let PR20 = $("#C02_START_V2_barsq").val();
        let PR21 = $("#C02_START_V2_ST_DSTR1").val();
        let PR22 = $("#C02_START_V2_Label34").val();
        let PR23 = $("#C02_START_V2_Label77").val();
        let ProductCode = $("#C02_START_V2_ProductCode").val();
        let MC = $("#C02_START_V2_L_MCNM").val();
        let OR_NO = BaseResult.C02_START_V2_Search[0].OR_NO;
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(BARCODE_QR);
        BaseParameter.ListSearchString.push(PR1);
        BaseParameter.ListSearchString.push(PR4);
        BaseParameter.ListSearchString.push(PR5);
        BaseParameter.ListSearchString.push(PR6);
        BaseParameter.ListSearchString.push(PR7);
        BaseParameter.ListSearchString.push(PR8);
        BaseParameter.ListSearchString.push(PR9);
        BaseParameter.ListSearchString.push(PR10);
        BaseParameter.ListSearchString.push(PR11);
        BaseParameter.ListSearchString.push(PR12);
        BaseParameter.ListSearchString.push(PR13);
        BaseParameter.ListSearchString.push(PR14);
        BaseParameter.ListSearchString.push(PR15);
        BaseParameter.ListSearchString.push(PR16);
        BaseParameter.ListSearchString.push(PR17);
        BaseParameter.ListSearchString.push(PR20);
        BaseParameter.ListSearchString.push(PR21);
        BaseParameter.ListSearchString.push(PR22);
        BaseParameter.ListSearchString.push(PR23);
        BaseParameter.ListSearchString.push(OR_NO);
        BaseParameter.ListSearchString.push(ProductCode);
        BaseParameter.ListSearchString.push(MC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_START_V2/PrintDocument1_PrintPage";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        })
            .then((response) => response.json())
            .then((data) => {
                if (data.DataGridView1) {
                    let dt = data.DataGridView1[0];

                    // build URL encode an toàn vói các ký tự đạc biệt
                    const printUrl = `/html/C02Lable.html?` +
                        `QRCodeString=${encodeURIComponent(dt.Barcode)}&` +
                        `PR1=${encodeURIComponent(dt.D01)}&` +
                        `PR2=${encodeURIComponent(dt.D02)}&` +
                        `PR3=${encodeURIComponent(dt.D03)}&` +
                        `PR4=${encodeURIComponent(dt.D04)}&` +
                        `PR5=${encodeURIComponent(dt.D05)}&` +
                        `PR6=${encodeURIComponent(dt.D06)}&` +
                        `PR7=${encodeURIComponent(dt.D07)}&` +
                        `PR8=${encodeURIComponent(dt.D08)}&` +
                        `PR9=${encodeURIComponent(dt.D09)}&` +
                        `PR10=${encodeURIComponent(dt.D10)}&` +
                        `PR11=${encodeURIComponent(dt.D11)}&` +
                        `PR12=${encodeURIComponent(dt.D12)}&` +
                        `PR13=${encodeURIComponent(dt.D13)}&` +
                        `PR14=${encodeURIComponent(dt.D14)}&` +
                        `PR15=${encodeURIComponent(dt.D15)}&` +
                        `PR16=${encodeURIComponent(dt.D16)}&` +
                        `PR17=${encodeURIComponent(dt.D17)}&` +
                        `PR18=${encodeURIComponent(dt.D18)}&` +
                        `PR19=${encodeURIComponent(dt.D19)}&` +
                        `PR20=${encodeURIComponent(dt.D20)}&` +
                        `PR21=${encodeURIComponent(dt.D21)}&` +
                        `PR22=${encodeURIComponent(dt.D22)}&` +
                        `PR23=${encodeURIComponent(dt.D23)}&` +
                        `PR24=${encodeURIComponent(dt.D24)}&` +
                        `PR25=${encodeURIComponent(dt.D25)}&` +
                        `PR26=${encodeURIComponent(dt.D26)}&` +
                        `PR27=${encodeURIComponent(dt.D27)}` ;

                    // mở tab in tem
                    window.open(printUrl, "_blank");
                }
                $("#BackGround").css("display", "none");
            })
            .catch((err) => {
                console.error(err);
                $("#BackGround").css("display", "none");
            });
    }
}

$("#C02_START_V2_Button3").click(function (e) {
    C02_START_V2_Button3_Click();
});
function C02_START_V2_Button3_Click() {
    $("#C02_REPRINT_Label4").val($("#C02_START_V2_Label8").val());
    $("#C02_REPRINT_WHERE_TEXT").val(" ORDER BY TORDER_BARCODE.UPDATE_DTM DESC LIMIT 1");
    $("#C02_REPRINT").modal("open");
    C02_REPRINT_PageLoad();
}

$("#C02_START_V2_BI_10").click(function (e) {
    C02_START_V2_Button2_Click_1();
});

function C02_START_V2_Button2_Click_1() {
    let Non_text = "";
    let Non_text_NM = "";
    let Non_idx = 0;
    let BI_RadioButton1 = document.getElementById("C02_START_V2_BI_RadioButton1").checked;
    if (BI_RadioButton1 == true) {
        Non_idx = 1;
    }
    let BI_RadioButton2 = document.getElementById("C02_START_V2_BI_RadioButton2").checked;
    if (BI_RadioButton2 == true) {
        Non_idx = 2;
    }
    let BI_RadioButton3 = document.getElementById("C02_START_V2_BI_RadioButton3").checked;
    if (BI_RadioButton3 == true) {
        Non_idx = 3;
    }
    let BI_RadioButton4 = document.getElementById("C02_START_V2_BI_RadioButton4").checked;
    if (BI_RadioButton4 == true) {
        Non_idx = 4;
    }
    let BI_RadioButton5 = document.getElementById("C02_START_V2_BI_RadioButton5").checked;
    if (BI_RadioButton5 == true) {
        Non_idx = 5;
    }
    let BI_RadioButton6 = document.getElementById("C02_START_V2_BI_RadioButton6").checked;
    if (BI_RadioButton6 == true) {
        Non_idx = 6;
    }
    let BI_RadioButton7 = document.getElementById("C02_START_V2_BI_RadioButton7").checked;
    if (BI_RadioButton7 == true) {
        Non_idx = 7;
    }
    switch (Non_idx) {
        case 1:
            Non_text = "S";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton1Text").innerHTML;
            break;
        case 2:
            Non_text = "I";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton2Text").innerHTML;
            break;
        case 3:
            Non_text = "Q";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton3Text").innerHTML;
            break;
        case 4:
            Non_text = "M";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton4Text").innerHTML;
            break;
        case 5:
            Non_text = "T";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton5Text").innerHTML;
            break;
        case 6:
            Non_text = "L";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton6Text").innerHTML;
            break;
        case 7:
            Non_text = "E";
            Non_text_NM = document.getElementById("C02_START_V2_BI_RadioButton7Text").innerHTML;
            break;
    }


    $("#C02_STOP_Label5").val(Non_text);
    $("#C02_STOP_STOP_MC").val($("#MCbox").val());
    $("#C02_STOP_Label2").val(Non_text_NM + "(" + Non_text + ")");
    $("#C02_STOP").modal("open");
    C02_STOP_PageLoad();

}

//thuc hiện load thông tin kỹ thuật của đơn cắt cần hoàn thành
function C02_START_V2_ORDER_LOAD(Flag) {

    //C02_STOP_FormClosed();
    let IsCheck = true;
    let Label8 = $("#C02_START_V2_Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_START_V2_Search = data.Search;
            BaseResult.C02_START_V2_Search1 = data.Search1;
            if (BaseResult.C02_START_V2_Search.length <= 0) {
                let COM_COUNT = BaseResult.C02_START_V2_Search1.length;
                if (COM_COUNT == 0) {
                    IsCheck = false;
                    M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                    C02_START_V2_Buttonclose_Click();
                }
                else {
                    IsCheck = false;
                    M.toast({ html: "등록된 자료가 없습니다. Không có dữ liệu đăng ký MES.", classes: 'green', displayLength: 6000 });
                    C02_START_V2_Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                C02_START_V2_StartTime = new Date($("#C02_START_V2_Label70").val());
                
                $("#C02_START_V2_BARCODE_TEXT").val(BaseResult.C02_START_V2_Search[0].TORDER_BARCODENM).trigger("change");
                $("#C02_START_V2_Label10").val(BaseResult.C02_START_V2_Search[0].TERM1).trigger("change");
                $("#C02_START_V2_VLT1").val(BaseResult.C02_START_V2_Search[0].TERM1).trigger("change");
                $("#C02_START_V2_Label14").val(BaseResult.C02_START_V2_Search[0].TERM2).trigger("change");
                $("#C02_START_V2_VLT2").val(BaseResult.C02_START_V2_Search[0].TERM2).trigger("change");
                $("#C02_START_V2_Label12").val(BaseResult.C02_START_V2_Search[0].SEAL1).trigger("change");
                $("#C02_START_V2_VLS1").val(BaseResult.C02_START_V2_Search[0].SEAL1).trigger("change");
                $("#C02_START_V2_Label16").val(BaseResult.C02_START_V2_Search[0].SEAL2).trigger("change");
                $("#C02_START_V2_VLS2").val(BaseResult.C02_START_V2_Search[0].SEAL2).trigger("change");
                $("#C02_START_V2_Label18").val(BaseResult.C02_START_V2_Search[0].WIRE).trigger("change");
                $("#C02_START_V2_WireCode").val(BaseResult.C02_START_V2_Search[0].WIRE_PARTNO).trigger("change");
                $("#C02_START_V2_ProductCode").val(BaseResult.C02_START_V2_Search[0].TORDER_FG);
                C02_START_V2_Label18_TextChanged();
                $("#C02_START_V2_VLW").val(BaseResult.C02_START_V2_Search[0].WIRE);
                $("#C02_START_V2_ST_DSTR1").val(BaseResult.C02_START_V2_Search[0].STRIP1);
                $("#C02_START_V2_Label34").val(BaseResult.C02_START_V2_Search[0].STRIP2);
                $("#C02_START_V2_ST_DCC1").val(BaseResult.C02_START_V2_Search[0].CCH_W1);
                $("#C02_START_V2_ST_DIC1").val(BaseResult.C02_START_V2_Search[0].ICH_W1);
                $("#C02_START_V2_ST_DCC2").val(BaseResult.C02_START_V2_Search[0].CCH_W2);
                $("#C02_START_V2_ST_DIC2").val(BaseResult.C02_START_V2_Search[0].ICH_W2);
                $("#C02_START_V2_Label48").val(BaseResult.C02_START_V2_Search[0].TOT_QTY);
                $("#C02_START_V2_Label59").val(BaseResult.C02_START_V2_Search[0].PERFORMN);
                $("#C02_START_V2_Label42").val(BaseResult.C02_START_V2_Search[0].SP_ST);
                $("#C02_START_V2_Label43").val(BaseResult.C02_START_V2_Search[0].PROJECT);
                $("#C02_START_V2_Label54").val(BaseResult.C02_START_V2_Search[0].HOOK_RACK);
                $("#C02_START_V2_barsq").val(BaseResult.C02_START_V2_Search[0].Barcode_SEQ);
                $("#C02_START_V2_Label77").val(BaseResult.C02_START_V2_Search[0].ADJ_AF_QTY);
                $("#C02_START_V2_TOOL_CONT1").val(BaseResult.C02_START_V2_Search[0].T1_CONT);
                $("#C02_START_V2_TOOL_CONT2").val(BaseResult.C02_START_V2_Search[0].T2_CONT);
                $("#C02_START_V2_Label5").val(BaseResult.C02_START_V2_Search[0].TORDER_BARCODE_IDX);
                $("#C02_START_V2_Label50").val(BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE);
                $("#C02_START_V2_T1_IDX").val(BaseResult.C02_START_V2_Search[0].T1_PN_IDX);
                $("#C02_START_V2_T2_IDX").val(BaseResult.C02_START_V2_Search[0].T2_PN_IDX);
                $("#C02_START_V2_S1_IDX").val(BaseResult.C02_START_V2_Search[0].S1_PN_IDX);
                $("#C02_START_V2_S2_IDX").val(BaseResult.C02_START_V2_Search[0].S2_PN_IDX);
                $("#C02_START_V2_WIRE_IDX").val(BaseResult.C02_START_V2_Search[0].W_PN_IDX);
                $("#C02_START_V2_TOOL1_IDX").val(BaseResult.C02_START_V2_Search[0].T1_TOO_IDX);
                $("#C02_START_V2_TOOL2_IDX").val(BaseResult.C02_START_V2_Search[0].T2_TOOL_IDX);

                if (BaseResult.C02_START_V2_Search[0].T1_NM == "") {
                    $("#C02_START_V2_VLA1").val("");
                    $("#C02_START_V2_VLA11").val("");
                    $("#C02_START_V2_TOOL_CONT1").val("");
                    document.getElementById("C02_START_V2_BT_APP1").disabled = true;
                }
                else {
                    $("#C02_START_V2_VLA1").val(BaseResult.C02_START_V2_Search[0].TERM1);
                    $("#C02_START_V2_VLA11").val(BaseResult.C02_START_V2_Search[0].T1_NM);
                    document.getElementById("C02_START_V2_BT_APP1").disabled = false;
                }
                let BB0 = BaseResult.C02_START_V2_Search[0].WIRE;
                if (BB0 == "") {

                }
                else {
                    let PO1 = BB0.indexOf("  ") - BB0.indexOf(" ");
                    let TT2 = BB0.substr(BB0.indexOf(" ") + 1, PO1);
                    $("#C02_START_V2_ST_DWIRE1").val(TT2);

                }
                if (BaseResult.C02_START_V2_Search[0].T2_NM == "") {
                    $("#C02_START_V2_VLA2").val("");
                    $("#C02_START_V2_VLA22").val("");
                    $("#C02_START_V2_TOOL_CONT2").val("");
                    document.getElementById("C02_START_V2_BT_APP2").disabled = true;
                }
                else {
                    $("#C02_START_V2_VLA2").val(BaseResult.C02_START_V2_Search[0].TERM2);
                    $("#C02_START_V2_VLA22").val(BaseResult.C02_START_V2_Search[0].T2_NM);
                    document.getElementById("C02_START_V2_BT_APP2").disabled = false;
                }
                try {
                    if ($("#C02_START_V2_TOOL_CONT1").val() == "") {
                        document.getElementById("C02_START_V2_TOOL_CONT1").style.color = "black";
                    }
                    else {
                        let T1_CONT = Number(BaseResult.C02_START_V2_Search[0].T1_CONT);
                        let TOOL_MAX1 = Number(BaseResult.C02_START_V2_Search[0].TOOL_MAX1);
                        TOOL_MAX1 = TOOL_MAX1 * 0.9;
                        if (T1_CONT <= TOOL_MAX1) {
                            document.getElementById("C02_START_V2_TOOL_CONT1").style.color = "green";
                        }
                        if (T1_CONT > TOOL_MAX1) {
                            document.getElementById("C02_START_V2_TOOL_CONT1").style.color = "yellow";
                        }
                        TOOL_MAX1 = BaseResult.C02_START_V2_Search[0].TOOL_MAX1;
                        TOOL_MAX1 = TOOL_MAX1 * 1.0;
                        if (T1_CONT >= TOOL_MAX1) {
                            document.getElementById("C02_START_V2_TOOL_CONT1").style.color = "red";
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_START_V2_TOOL_CONT1").style.color = "red";
                    $("#C02_START_V2_TOOL_CONT1").val("ERROR");
                }
                try {
                    if ($("#C02_START_V2_TOOL_CONT2").val() == "") {
                        document.getElementById("C02_START_V2_TOOL_CONT2").style.color = "black";
                    }
                    else {
                        let T2_CONT = Number(BaseResult.C02_START_V2_Search[0].T2_CONT);
                        let TOOL_MAX2 = Number(BaseResult.C02_START_V2_Search[0].TOOL_MAX2);
                        TOOL_MAX2 = TOOL_MAX2 * 0.9;
                        if (T2_CONT <= TOOL_MAX2) {
                            document.getElementById("C02_START_V2_TOOL_CONT2").style.color = "green";
                        }
                        if (T2_CONT > TOOL_MAX2) {
                            document.getElementById("C02_START_V2_TOOL_CONT2").style.color = "yellow";
                        }
                        TOOL_MAX2 = BaseResult.C02_START_V2_Search[0].TOOL_MAX2;
                        TOOL_MAX2 = TOOL_MAX2 * 1.0;
                        if (T2_CONT >= TOOL_MAX2) {
                            document.getElementById("C02_START_V2_TOOL_CONT2").style.color = "red";
                        }
                    }
                }
                catch (e) {
                    document.getElementById("C02_START_V2_TOOL_CONT2").style.color = "red";
                    $("#C02_START_V2_TOOL_CONT2").val("ERROR");
                }
                let AAA = Number(BaseResult.C02_START_V2_Search[0].TOT_QTY) - Number(BaseResult.C02_START_V2_Search[0].PERFORMN);
                let BUNDLE_SIZE = Number(BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE);
                if (AAA >= BUNDLE_SIZE) {
                    $("#C02_START_V2_Label50").val(BUNDLE_SIZE);
                }
                else {
                    if (AAA > 0) {
                        $("#C02_START_V2_Label50").val(AAA);
                    }
                    else {
                        M.toast({ html: "작업을 종료 하였습니다. Đã dừng làm việc.", classes: 'green', displayLength: 6000 });
                        C02_START_V2_Buttonclose_Click();
                    }
                }

                if (Flag == 0) {
                    C02_START_V2_DB_COUTN(0);

                    document.getElementById("C02_START_V2_BI_RadioButton1").checked = true;

                    C02_START_V2_OPER_TIME(0);

                    C02_START_V2_SPC_LOAD(0);

                    C02_START_V2_Timer2StartInterval();

                    C02_START_V2_Timer1StartInterval();

                    C02_START_V2_SW_TIME(0);

                    C02_START_V2_EW_TIME_Timer1StartInterval();

                    //$("#C02_START_V2").modal("open");

                }
                if (Flag == 1) {
                    C02_START_V2_DB_COUTN();
                }
            }

            updateScrapCheckboxes();
            if (data.DataGridView && Array.isArray(data.DataGridView)) {
                loadScrapData(data.DataGridView);
            }     
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_START_V2_F_SPC() {
    if (BaseResult.C02_START_V2_Search[0].TOT_QTY == BaseResult.C02_START_V2_Search[0].BUNDLE_SIZE) {
        if (document.getElementById("C02_START_V2_SPC1").innerHTML == "First") {
            $("#C02_SPC_Label10").val("First");
            $("#C02_SPC").modal("open");
            C02_SPC_PageLoad();
        }
    }
}
function C02_START_V2_SW_TIME(Flag) {
    let MCbox = $("#MCbox").val();
    let Label8 = $("#C02_START_V2_Label8").val();
    let FormText = "KOMAX_WORK";
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    BaseParameter.ListSearchString.push(Label8);
    BaseParameter.ListSearchString.push(FormText);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_START_V2_DataGridView4 = data.DataGridView4;

            try {
                WORING_IDX = BaseResult.C02_START_V2_DataGridView4[0].TOWT_INDX;
            }
            catch (e) {
                WORING_IDX = 0;
            }
            localStorage.setItem("WORING_IDX", WORING_IDX);

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_START_V2_SPC_LOAD(Flag) {
    let Label8 = $("#C02_START_V2_Label8").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_START_V2_DataGridView3 = data.DataGridView3;
            let COUNT_SPC = 0;
            try {
                COUNT_SPC = Number(BaseResult.C02_START_V2_DataGridView3[0].WORK_QTY);
            }
            catch (e) {
                COUNT_SPC = Number($("#C02_START_V2_Label48").val());
            }
            try {
                if (COUNT_SPC < 1) {
                    document.getElementById("C02_START_V2_SPC1").innerHTML = "----";
                    document.getElementById("C02_START_V2_SPC1").disabled = true;

                    document.getElementById("C02_START_V2_SPC2").innerHTML = "----";
                    document.getElementById("C02_START_V2_SPC2").disabled = true;

                    document.getElementById("C02_START_V2_SPC3").innerHTML = "----";
                    document.getElementById("C02_START_V2_SPC3").disabled = true;
                }
                else {
                    document.getElementById("C02_START_V2_SPC1").innerHTML = "First";
                    document.getElementById("C02_START_V2_SPC1").disabled = false;

                    document.getElementById("C02_START_V2_SPC3").innerHTML = "End";
                    document.getElementById("C02_START_V2_SPC3").disabled = false;

                }
                if (COUNT_SPC < 501) {
                    document.getElementById("C02_START_V2_SPC2").innerHTML = "----";
                    document.getElementById("C02_START_V2_SPC2").disabled = true;
                }
                else {
                    document.getElementById("C02_START_V2_SPC2").innerHTML = "Middle";
                    document.getElementById("C02_START_V2_SPC2").disabled = false;
                }
            }
            catch (e) {
                document.getElementById("C02_START_V2_SPC1").innerHTML = "ERROR";
                document.getElementById("C02_START_V2_SPC2").innerHTML = "ERROR";
                document.getElementById("C02_START_V2_SPC3").innerHTML = "ERROR";
            }
            for (let i = 0; i < BaseResult.C02_START_V2_DataGridView3.length; i++) {
                if (BaseResult.C02_START_V2_DataGridView3[i].COLSIP == "First") {
                    document.getElementById("C02_START_V2_SPC1").innerHTML = "Complete";
                    document.getElementById("C02_START_V2_SPC1").disabled = true;
                }
                if (BaseResult.C02_START_V2_DataGridView3[i].COLSIP == "Middle") {
                    document.getElementById("C02_START_V2_SPC2").innerHTML = "Complete";
                    document.getElementById("C02_START_V2_SPC2").disabled = true;
                }
                if (BaseResult.C02_START_V2_DataGridView3[i].COLSIP == "End") {
                    document.getElementById("C02_START_V2_SPC3").innerHTML = "Complete";
                    document.getElementById("C02_START_V2_SPC3").disabled = true;
                }
            }
            if (SHIELDWIRE_CHK == false) {
                C02_START_V2_F_SPC();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_START_V2_OPER_TIME(Flag) {
    let MCbox = $("#MCbox").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_START_V2_DataGridView2 = data.DataGridView2;
            let TOT_SUM = BaseResult.C02_START_V2_DataGridView2[0].SUM_TIME;

            let H_TIME = Math.floor(TOT_SUM / 60 / 60);
            TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
            let M_TIME = Math.floor(TOT_SUM / 60);
            let S_TIME = TOT_SUM - (M_TIME * 60);
            let C02_START_V2_BI_STOPTIME = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
            let H_TIME1 = C02_START_V2_BI_STOPTIME.substr(0, 2);
            let M_TIME1 = C02_START_V2_BI_STOPTIME.substr(2, 2);
            let S_TIME1 = C02_START_V2_BI_STOPTIME.substr(4, 2);
            C02_START_V2_BI_STOPTIME = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
            $("#C02_START_V2_BI_STOPTIME").val(C02_START_V2_BI_STOPTIME);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//dem lai tong do da làm viec cua may
function C02_START_V2_DB_COUTN(Flag) {
    let MCbox = $("#MCbox").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/DB_COUTN";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.C02_START_V2_DataGridView1 = data.DataGridView1;
            $("#C02_START_V2_BI_WQTY").val(0);
            if (BaseResult.C02_START_V2_DataGridView1.length > 0) {
                let TOT_QTY = Number(BaseResult.C02_START_V2_DataGridView1[0].SUM);
                if (TOT_QTY <= 0) {
                    TOT_QTY = 0;
                }
                else {
                    TOT_QTY = Number(BaseResult.C02_START_V2_DataGridView1[0].SUM);
                }
                $("#C02_START_V2_BI_WQTY").val(TOT_QTY);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

$("#C02_START_V2_Buttonclose").click(function (e) {
    C02_START_V2_Buttonclose_Click();
});
function C02_START_V2_Buttonclose_Click() {
    C02_START_V2_C02_FormClosed();
}
function C02_START_V2_C02_FormClosed() {

    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C02_START_V2_BARCODE_TEXT").val("");

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(SettingsMC_NM);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_START_V2/C02_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
    $("#C02_START_V2").modal("close");
    C02_LIST_Buttonfind_Click();
    Buttonfind_Click();
}

function C02_APPLICATION_PageLoad() {
    $("#C02_APPLICATION_Label5").val("---");
    $("#C02_APPLICATION_Label4").val("---");

    $("#C02_APPLICATION_TextBox1").val("");
    $("#C02_APPLICATION_TextBox1").focus();
}
$("#C02_APPLICATION_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_APPLICATION_TextBox1_KeyDown();
    }
});
function C02_APPLICATION_TextBox1_KeyDown() {
    C02_APPLICATION_Button1_Click();
}
$("#C02_APPLICATION_Button1").click(function (e) {
    C02_APPLICATION_Button1_Click();
});
function C02_APPLICATION_Button1_Click() {
    let IsCheck = true;
    let APPT = $("#C02_APPLICATION_TextBox1").val();
    let Label1 = $("#C02_APPLICATION_Label1").val();
    Label1 = Label1.toUpperCase();
    let CO = APPT.length;
    let AAA = APPT.substr(0, CO - 1);
    if (AAA == Label1) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(APPT);
        BaseParameter.ListSearchString.push(Label1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_APPLICATION/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.C02_APPLICATION_Search = data.Search;
                if (BaseResult.C02_APPLICATION_Search.length <= 0) {
                    IsCheck = false;
                    M.toast({ html: "No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.", classes: 'red', displayLength: 6000 });
                }
                if (IsCheck == true) {
                    $("#C02_APPLICATION_Label5").val(BaseResult.C02_APPLICATION_Search[0].APP_NAME);
                    $("#C02_APPLICATION_Label4").val(BaseResult.C02_APPLICATION_Search[0].SEQ);
                    $("#C02_APPLICATION_Label9").val(BaseResult.C02_APPLICATION_Search[0].WK_CNT);
                    $("#C02_APPLICATION_Label10").val(BaseResult.C02_APPLICATION_Search[0].TOOLMASTER_IDX);
                }
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        IsCheck = false;
        M.toast({ html: "APPLICATOR을 잘못 등록했습니다. Đăng ký sai của ỨNG DỤNG.", classes: 'red', displayLength: 6000 });
    }
}
$("#C02_APPLICATION_Button2").click(function (e) {
    C02_APPLICATION_Button2_Click();
});
function C02_APPLICATION_Button2_Click() {
    let IsCheck = true;
    let Label5 = $("#C02_APPLICATION_Label5").val();
    if (Label5 == "---") {
        IsCheck = false;
    }
    let Label4 = $("#C02_APPLICATION_Label4").val();
    if (Label4 == "---") {
        IsCheck = false;
    }
    let Label3 = $("#C02_APPLICATION_Label3").val();
    let Label10 = $("#C02_APPLICATION_Label10").val();
    let Label8 = $("#C02_START_V2_Label8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label3);
    BaseParameter.ListSearchString.push(Label10);
    BaseParameter.ListSearchString.push(Label8);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_APPLICATION/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            C02_APPLICATION_Buttonclose_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#C02_APPLICATION_Buttonclose").click(function (e) {
    C02_APPLICATION_Buttonclose_Click();
});
function C02_APPLICATION_Buttonclose_Click() {
    C02_APPLICATION_FormClosed();
}
function C02_APPLICATION_FormClosed() {
    $("#C02_APPLICATION").modal("close");
    C02_START_V2_ORDER_LOAD(1);
}

let C02_SPC_OR_IDX;

//hàm gọi check SPC
function C02_SPC_PageLoad() {

    $("#C02_SPC_MRUS").val("-");

    C02_SPC_OR_IDX = $("#C02_START_V2_Label8").val();

    C02_START_V2_SPC_EXIT = false;

    $("#C02_SPC_STS1").val("");
    $("#C02_SPC_STS2").val("");
    $("#C02_SPC_STS3").val("");
    $("#C02_SPC_STS4").val("");
    $("#C02_SPC_STS5").val("");
    $("#C02_SPC_STS6").val("");
    $("#C02_SPC_STS7").val("");
    $("#C02_SPC_STS8").val("");

    $("#C02_SPC_STL1").val("");
    $("#C02_SPC_STL2").val("");
    $("#C02_SPC_STL3").val("");
    $("#C02_SPC_STL4").val("");
    $("#C02_SPC_STL5").val("");
    $("#C02_SPC_STL6").val("");
    $("#C02_SPC_STL7").val("");
    $("#C02_SPC_STL8").val("");

    $("#C02_SPC_STM1").val("");
    $("#C02_SPC_STM2").val("");
    $("#C02_SPC_STM3").val("");
    $("#C02_SPC_STM4").val("");
    $("#C02_SPC_STM5").val("");
    $("#C02_SPC_STM6").val("");
    $("#C02_SPC_STM7").val("");
    $("#C02_SPC_STM8").val("");

    document.getElementById("C02_SPC_TextBox1").disabled = false;
    document.getElementById("C02_SPC_TextBox2").disabled = false;
    document.getElementById("C02_SPC_TextBox3").disabled = false;
    document.getElementById("C02_SPC_TextBox4").disabled = false;
    document.getElementById("C02_SPC_TextBox5").disabled = false;
    document.getElementById("C02_SPC_TextBox6").disabled = false;
    document.getElementById("C02_SPC_TextBox7").disabled = false;
    document.getElementById("C02_SPC_TextBox8").disabled = false;
    document.getElementById("C02_SPC_TextBox9").disabled = false;


    $("#C02_SPC_ST11").val($("#C02_START_V2_ST_DCC1").val());
    $("#C02_SPC_ST12").val($("#C02_START_V2_ST_DIC1").val());
    $("#C02_SPC_ST21").val($("#C02_START_V2_ST_DCC2").val());
    $("#C02_SPC_ST22").val($("#C02_START_V2_ST_DIC2").val());
    $("#C02_SPC_Label8").val($("#C02_START_V2_VLW").val());
    $("#C02_SPC_Label18").val($("#C02_START_V2_WIRE_Length").val());


    let C02_START_V2_WIRE_Length = $("#C02_START_V2_WIRE_Length").val();
    C02_START_V2_WIRE_Length = C02_START_V2_WIRE_Length.replace(",", "");
    let WIRE_LENCK = Number(C02_START_V2_WIRE_Length);

    if ((WIRE_LENCK >= 0) && (WIRE_LENCK <= 500)) {
        $("#C02_SPC_STL10").val(WIRE_LENCK);
        $("#C02_SPC_STM10").val(WIRE_LENCK + 5);
    }

    if ((WIRE_LENCK >= 501) && (WIRE_LENCK <= 2000)) {
        $("#C02_SPC_STL10").val(WIRE_LENCK);
        $("#C02_SPC_STM10").val(WIRE_LENCK + 10);
    }

    if ((WIRE_LENCK >= 2001) && (WIRE_LENCK <= 90000)) {
        $("#C02_SPC_STL10").val(WIRE_LENCK);
        $("#C02_SPC_STM10").val(WIRE_LENCK + 15);
    }



    let C02_START_V2_ST_DWIRE1 = $("#C02_START_V2_ST_DWIRE1").val();
    C02_START_V2_ST_DWIRE1 = C02_START_V2_ST_DWIRE1.replace(",", "");
    $("#C02_SPC_Label19").val(C02_START_V2_ST_DWIRE1);

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#C02_SPC_ST11").val());
    BBBB.push($("#C02_SPC_ST12").val());
    BBBB.push($("#C02_SPC_ST21").val());
    BBBB.push($("#C02_SPC_ST22").val());

    let OO = 0;
    for (let i = 0; i < 4; i++) {
        if (BBBB[i].length > 0) {
            let AA = BBBB[i];
            let Flag = AA.indexOf("/");
            try {
                let BB = AA.substr(0, Flag);
                if (BB.length > 0) {
                    BB = BB.trim();
                    //let CC = BB.substr(0, 4);
                    let CC = BB.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC.length > 4) {
                        if (BB.split("-").length > 1) {
                            CC = BB.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            CC = BB.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC = "";
                    let DD = BB.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD.length > 4) {
                        if (BB.split("-").length > 1) {
                            DD = BB.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB.split("+").length > 1) {
                            DD = BB.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC = CC.split("(")[0].replace(/[^0-9.]/g, "");
                    }
                    catch (e) {
                        CCC = CC;
                    }
                    CCCC.push(CCC);
                    let CCCDD = Number(CCC) - Number(DD);
                    LLLL.push(CCCDD.toFixed(2));
                    CCCDD = Number(CCC) + Number(DD);
                    MMMM.push(CCCDD.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
            try {
                let BB1 = AA.substr(Flag + 1, AA.length - 1);
                if (BB1.length > 0) {
                    BB1 = BB1.trim();
                    //let CC1 = BB1.substr(0, 4);
                    let CC1 = BB1.split("±")[0].replace(/[^0-9.]/g, "");
                    if (CC1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            CC1 = BB1.split("-")[0].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            CC1 = BB1.split("+")[0].replace(/[^0-9.]/g, "");
                        }
                    }
                    let CCC1 = "";
                    let DD1 = BB1.split("±")[1].replace(/[^0-9.]/g, "");
                    if (DD1.length > 4) {
                        if (BB1.split("-").length > 1) {
                            DD1 = BB1.split("-")[1].replace(/[^0-9.]/g, "");
                        }
                        if (BB1.split("+").length > 1) {
                            DD1 = BB1.split("+")[1].replace(/[^0-9.]/g, "");
                        }
                    }
                    try {
                        CCC1 = CC1.split("(")[0].replace(/[^0-9.]/g, "");;
                    }
                    catch (e) {
                        CCC1 = CC1;
                    }
                    CCCC.push(CCC1);
                    let CCC1DD1 = Number(CCC1) - Number(DD1);
                    LLLL.push(CCC1DD1.toFixed(2));
                    CCC1DD1 = Number(CCC1) + Number(DD1);
                    MMMM.push(CCC1DD1.toFixed(2));
                }
                else {
                    CCCC.push("");
                    LLLL.push("");
                    MMMM.push("");
                }
            }
            catch (e) {
                CCCC.push("");
                LLLL.push("");
                MMMM.push("");
            }
        }
        else {
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
        }
    }
    $("#C02_SPC_STS1").val(CCCC[0]);
    $("#C02_SPC_STS2").val(CCCC[1]);
    $("#C02_SPC_STS3").val(CCCC[2]);
    $("#C02_SPC_STS4").val(CCCC[3]);
    $("#C02_SPC_STS5").val(CCCC[4]);
    $("#C02_SPC_STS6").val(CCCC[5]);
    $("#C02_SPC_STS7").val(CCCC[6]);
    $("#C02_SPC_STS8").val(CCCC[7]);

    if ($("#C02_SPC_STS1").val() == "NaN") {
        $("#C02_SPC_STS1").val("");
    }
    if ($("#C02_SPC_STS2").val() == "NaN") {
        $("#C02_SPC_STS2").val("");
    }
    if ($("#C02_SPC_STS3").val() == "NaN") {
        $("#C02_SPC_STS3").val("");
    }
    if ($("#C02_SPC_STS4").val() == "NaN") {
        $("#C02_SPC_STS4").val("");
    }
    if ($("#C02_SPC_STS5").val() == "NaN") {
        $("#C02_SPC_STS5").val("");
    }
    if ($("#C02_SPC_STS6").val() == "NaN") {
        $("#C02_SPC_STS6").val("");
    }
    if ($("#C02_SPC_STS7").val() == "NaN") {
        $("#C02_SPC_STS7").val("");
    }
    if ($("#C02_SPC_STS8").val() == "NaN") {
        $("#C02_SPC_STS8").val("");
    }

    $("#C02_SPC_STL1").val(LLLL[0]);
    $("#C02_SPC_STL2").val(LLLL[1]);
    $("#C02_SPC_STL3").val(LLLL[2]);
    $("#C02_SPC_STL4").val(LLLL[3]);
    $("#C02_SPC_STL5").val(LLLL[4]);
    $("#C02_SPC_STL6").val(LLLL[5]);
    $("#C02_SPC_STL7").val(LLLL[6]);
    $("#C02_SPC_STL8").val(LLLL[7]);

    if ($("#C02_SPC_STL1").val() == "NaN") {
        $("#C02_SPC_STL1").val("");
    }
    if ($("#C02_SPC_STL2").val() == "NaN") {
        $("#C02_SPC_STL2").val("");
    }

    if ($("#C02_SPC_STL3").val() == "NaN") {
        $("#C02_SPC_STL3").val("");
    }
    if ($("#C02_SPC_STL4").val() == "NaN") {
        $("#C02_SPC_STL4").val("");
    }

    if ($("#C02_SPC_STL5").val() == "NaN") {
        $("#C02_SPC_STL5").val("");
    }
    if ($("#C02_SPC_STL6").val() == "NaN") {
        $("#C02_SPC_STL6").val("");
    }
    if ($("#C02_SPC_STL7").val() == "NaN") {
        $("#C02_SPC_STL7").val("");
    }
    if ($("#C02_SPC_STL8").val() == "NaN") {
        $("#C02_SPC_STL8").val("");
    }

    $("#C02_SPC_STM1").val(MMMM[0]);
    $("#C02_SPC_STM2").val(MMMM[1]);
    $("#C02_SPC_STM3").val(MMMM[2]);
    $("#C02_SPC_STM4").val(MMMM[3]);
    $("#C02_SPC_STM5").val(MMMM[4]);
    $("#C02_SPC_STM6").val(MMMM[5]);
    $("#C02_SPC_STM7").val(MMMM[6]);
    $("#C02_SPC_STM8").val(MMMM[7]);

    if ($("#C02_SPC_STM1").val() == "NaN") {
        $("#C02_SPC_STM1").val("");
    }
    if ($("#C02_SPC_STM2").val() == "NaN") {
        $("#C02_SPC_STM2").val("");
    }
    if ($("#C02_SPC_STM3").val() == "NaN") {
        $("#C02_SPC_STM3").val("");
    }
    if ($("#C02_SPC_STM4").val() == "NaN") {
        $("#C02_SPC_STM4").val("");
    }
    if ($("#C02_SPC_STM5").val() == "NaN") {
        $("#C02_SPC_STM5").val("");
    }
    if ($("#C02_SPC_STM6").val() == "NaN") {
        $("#C02_SPC_STM6").val("");
    }
    if ($("#C02_SPC_STM7").val() == "NaN") {
        $("#C02_SPC_STM7").val("");
    }
    if ($("#C02_SPC_STM8").val() == "NaN") {
        $("#C02_SPC_STM8").val("");
    }

    if ($("#C02_SPC_STS1").val() == "") {
        document.getElementById("C02_SPC_TextBox1").disabled = true;
    }
    if ($("#C02_SPC_STS2").val() == "") {
        document.getElementById("C02_SPC_TextBox2").disabled = true;
    }
    if ($("#C02_SPC_STS3").val() == "") {
        document.getElementById("C02_SPC_TextBox3").disabled = true;
    }
    if ($("#C02_SPC_STS4").val() == "") {
        document.getElementById("C02_SPC_TextBox4").disabled = true;
    }
    if ($("#C02_SPC_STS5").val() == "") {
        document.getElementById("C02_SPC_TextBox5").disabled = true;
    }
    if ($("#C02_SPC_STS6").val() == "") {
        document.getElementById("C02_SPC_TextBox6").disabled = true;
    }
    if ($("#C02_SPC_STS7").val() == "") {
        document.getElementById("C02_SPC_TextBox7").disabled = true;
    }
    if ($("#C02_SPC_STS8").val() == "") {
        document.getElementById("C02_SPC_TextBox8").disabled = true;
    }

    $("#C02_SPC_Label4").val($("#C02_START_V2_VLT1").val());
    $("#C02_SPC_Label5").val($("#C02_START_V2_VLT1").val());
    $("#C02_SPC_Label6").val($("#C02_START_V2_VLT2").val());
    $("#C02_SPC_Label7").val($("#C02_START_V2_VLT2").val());

    $("#C02_SPC_Label4").val($("#C02_START_V2_VLT1").val());
    $("#C02_SPC_Label5").val($("#C02_START_V2_VLT1").val());
    $("#C02_SPC_Label6").val($("#C02_START_V2_VLT2").val());
    $("#C02_SPC_Label7").val($("#C02_START_V2_VLT2").val());

    let Label4 = $("#C02_SPC_Label4").val();
    let Label6 = $("#C02_SPC_Label6").val();

    let BBB = Label4.includes("(");
    let DDD = Label6.includes("(");

    if (BBB == true) {
        document.getElementById("C02_SPC_TextBox1").disabled = true;
        document.getElementById("C02_SPC_TextBox2").disabled = true;
        document.getElementById("C02_SPC_TextBox3").disabled = true;
        document.getElementById("C02_SPC_TextBox4").disabled = true;
    }

    if (DDD == true) {
        document.getElementById("C02_SPC_TextBox5").disabled = true;
        document.getElementById("C02_SPC_TextBox6").disabled = true;
        document.getElementById("C02_SPC_TextBox7").disabled = true;
        document.getElementById("C02_SPC_TextBox8").disabled = true;
    }

    $("#C02_SPC_TextBox1").val("");
    C02_SPC_TextBox1_TextChanged();
    $("#C02_SPC_TextBox2").val("");
    C02_SPC_TextBox2_TextChanged();
    $("#C02_SPC_TextBox3").val("");
    C02_SPC_TextBox3_TextChanged();
    $("#C02_SPC_TextBox4").val("");
    C02_SPC_TextBox4_TextChanged();
    $("#C02_SPC_TextBox5").val("");
    C02_SPC_TextBox5_TextChanged();
    $("#C02_SPC_TextBox6").val("");
    C02_SPC_TextBox6_TextChanged();
    $("#C02_SPC_TextBox7").val("");
    C02_SPC_TextBox7_TextChanged();
    $("#C02_SPC_TextBox8").val("");
    C02_SPC_TextBox8_TextChanged();
    $("#C02_SPC_TextBox9").val("");
    C02_SPC_TextBox9_TextChanged();
    $("#C02_SPC_TextBox10").val("");
    C02_SPC_TextBox10_TextChanged();

    $("#C02_SPC_LR1").val("");
    $("#C02_SPC_LR2").val("");
    $("#C02_SPC_LR3").val("");
    $("#C02_SPC_LR4").val("");
    $("#C02_SPC_LR5").val("");
    $("#C02_SPC_LR6").val("");
    $("#C02_SPC_LR7").val("");
    $("#C02_SPC_LR8").val("");
    $("#C02_SPC_LR9").val("");
    $("#C02_SPC_LR10").val("");

    document.getElementById("C02_SPC_MRUS").style.backgroundColor = "white";

    document.getElementById("C02_SPC_LR1").style.backgroundColor = "white";
    document.getElementById("C02_SPC_LR2").style.backgroundColor = "white";
    document.getElementById("C02_SPC_LR3").style.backgroundColor = "whitesmoke";
    document.getElementById("C02_SPC_LR4").style.backgroundColor = "whitesmoke";
    document.getElementById("C02_SPC_LR5").style.backgroundColor = "white";
    document.getElementById("C02_SPC_LR6").style.backgroundColor = "white";
    document.getElementById("C02_SPC_LR7").style.backgroundColor = "whitesmoke";
    document.getElementById("C02_SPC_LR8").style.backgroundColor = "whitesmoke";
    document.getElementById("C02_SPC_LR9").style.backgroundColor = "white";
    document.getElementById("C02_SPC_LR10").style.backgroundColor = "white";

    C02_SPC_ORDER_CHG();
    C02_SPC_Load();

    // bỏ đoạn check Lục kéo nếu đầu daay chỉ tuốt không có dập bất kỳ đầu Term nào
    if ((BBB == true) && (DDD == true)) {
        document.getElementById("C02_SPC_TextBox9").disabled = true;
    }

    if ($("#C02_SPC_STL1").val() == "") {
        document.getElementById("C02_SPC_TextBox1").disabled = true;
    }
    if ($("#C02_SPC_STL2").val() == "") {
        document.getElementById("C02_SPC_TextBox2").disabled = true;
    }
    if ($("#C02_SPC_STL3").val() == "") {
        document.getElementById("C02_SPC_TextBox3").disabled = true;
    }
    if ($("#C02_SPC_STL4").val() == "") {
        document.getElementById("C02_SPC_TextBox4").disabled = true;
    }
    if ($("#C02_SPC_STL5").val() == "") {
        document.getElementById("C02_SPC_TextBox5").disabled = true;
    }
    if ($("#C02_SPC_STL6").val() == "") {
        document.getElementById("C02_SPC_TextBox6").disabled = true;
    }
    if ($("#C02_SPC_STL7").val() == "") {
        document.getElementById("C02_SPC_TextBox7").disabled = true;
    }
    if ($("#C02_SPC_STL8").val() == "") {
        document.getElementById("C02_SPC_TextBox8").disabled = true;
    }
    if (document.getElementById("C02_SPC_TextBox1").disabled == false) {
        $("#C02_SPC_TextBox1").focus();
    }
    else {
        $("#C02_SPC_TextBox5").focus();
    }
}
function C02_SPC_Load() {
    let F_CO = $("#C02_SPC_Label19").val();
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_SPC/C02_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {

            BaseResult.C02_SPC_Search = data.Search;
            if (BaseResult.C02_SPC_Search.length == 0) {
                $("#C02_SPC_Label17").val(0);
            }
            $("#C02_SPC_Label17").val(BaseResult.C02_SPC_Search[0].STRENGTH);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_SPC_ORDER_CHG() {
    // $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: C02_SPC_OR_IDX,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_SPC/ORDER_CHG";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C02_SPC_Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("C02_SPC_TextBox1").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox2").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox3").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox4").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox5").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox6").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox7").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox8").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox9").disabled);
    CHK.push(document.getElementById("C02_SPC_TextBox10").disabled);

    let AA = [];
    AA.push($("#C02_SPC_LR1").val());
    AA.push($("#C02_SPC_LR2").val());
    AA.push($("#C02_SPC_LR3").val());
    AA.push($("#C02_SPC_LR4").val());
    AA.push($("#C02_SPC_LR5").val());
    AA.push($("#C02_SPC_LR6").val());
    AA.push($("#C02_SPC_LR7").val());
    AA.push($("#C02_SPC_LR8").val());
    AA.push($("#C02_SPC_LR9").val());
    AA.push($("#C02_SPC_LR10").val());

    for (let i = 0; i < CHK.length; i++) {
        if (CHK[i] == false) {
            if (AA[i] == "OK") {
                VAL.push(true);
            }
        }
        else {
            VAL.push(true);
        }
    }

    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true) && (VAL[5] == true) && (VAL[6] == true) && (VAL[7] == true) && (VAL[8] == true) && (VAL[9] == true)) {
        $("#C02_SPC_MRUS").val("OK");
        document.getElementById("C02_SPC_MRUS").style.color = "green";
    }
    else {
        $("#C02_SPC_MRUS").val("NG");
        document.getElementById("C02_SPC_MRUS").style.color = "red";
    }
}
$("#C02_SPC_Button1").click(function (e) {
    C02_SPC_Button1_Click();
});

//bấm nút bấm check SPC
function C02_SPC_Button1_Click() {
    C02_START_V2_SPC_EXIT = false;
    let IsCheck = true;
    let TextBox1 = $("#C02_SPC_TextBox1").val();
    let TextBox2 = $("#C02_SPC_TextBox2").val();
    let TextBox3 = $("#C02_SPC_TextBox3").val();
    let TextBox4 = $("#C02_SPC_TextBox4").val();
    let TextBox5 = $("#C02_SPC_TextBox5").val();
    let TextBox6 = $("#C02_SPC_TextBox6").val();
    let TextBox7 = $("#C02_SPC_TextBox7").val();
    let TextBox8 = $("#C02_SPC_TextBox8").val();
    let TextBox9 = $("#C02_SPC_TextBox9").val();
    let TextBox10 = $("#C02_SPC_TextBox10").val();

    let Label8 = $("#C02_START_V2_Label8").val();
    let Label10 = $("#C02_SPC_Label10").val();
    let Label11 = $("#C02_SPC_Label11").val();

    let MRUS = $("#C02_SPC_MRUS").val();

    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox1);
            BaseParameter.ListSearchString.push(TextBox2);
            BaseParameter.ListSearchString.push(TextBox3);
            BaseParameter.ListSearchString.push(TextBox4);
            BaseParameter.ListSearchString.push(TextBox5);
            BaseParameter.ListSearchString.push(TextBox6);
            BaseParameter.ListSearchString.push(TextBox7);
            BaseParameter.ListSearchString.push(TextBox8);
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label8);
            BaseParameter.ListSearchString.push(Label10);
            BaseParameter.ListSearchString.push(Label11);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(TextBox9);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C02_SPC/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    if (data.Success) {
                        C02_START_V2_SPC_EXIT = true;
                        C02_SPC_Buttonclose_Click();
                    } else {
                        ShowMessage(data.Error, "error");
                    }

                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
$("#C02_SPC_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox1_KeyDown();
    }
});
function C02_SPC_TextBox1_KeyDown() {
    $("#C02_SPC_TextBox2").focus();
}
$("#C02_SPC_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox2_KeyDown();
    }
});
function C02_SPC_TextBox2_KeyDown() {
    $("#C02_SPC_TextBox3").focus();
}
$("#C02_SPC_TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox3_KeyDown();
    }
});
function C02_SPC_TextBox3_KeyDown() {
    $("#C02_SPC_TextBox4").focus();
}
$("#C02_SPC_TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox4_KeyDown();
    }
});
function C02_SPC_TextBox4_KeyDown() {
    if (document.getElementById("C02_SPC_TextBox5").disabled == false) {
        $("#C02_SPC_TextBox5").focus();
    }
    else {
        $("#C02_SPC_TextBox9").focus();
    }
}
$("#C02_SPC_TextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox5_KeyDown();
    }
});
function C02_SPC_TextBox5_KeyDown() {
    $("#C02_SPC_TextBox6").focus();
}
$("#C02_SPC_TextBox6").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox6_KeyDown();
    }
});
function C02_SPC_TextBox6_KeyDown() {
    $("#C02_SPC_TextBox7").focus();
}
$("#C02_SPC_TextBox7").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox7_KeyDown();
    }
});
function C02_SPC_TextBox7_KeyDown() {
    $("#C02_SPC_TextBox8").focus();
}
$("#C02_SPC_TextBox8").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox8_KeyDown();
    }
});
function C02_SPC_TextBox8_KeyDown() {
    $("#C02_SPC_TextBox9").focus();
}
$("#C02_SPC_TextBox9").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox10_KeyDown();
    }
});
function C02_SPC_TextBox10_KeyDown() {
    $("#C02_SPC_TextBox10").focus();
}
$("#C02_SPC_TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        C02_SPC_TextBox10_KeyDown_1();
    }
});
function C02_SPC_TextBox10_KeyDown_1() {
    $("#C02_SPC_TextBox10").focus();
}
$("#C02_SPC_TextBox1").change(function (e) {
    C02_SPC_TextBox1_TextChanged()
});
function C02_SPC_TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#C02_SPC_TextBox1").val();
        let STL1 = $("#C02_SPC_STL1").val();
        let STM1 = $("#C02_SPC_STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#C02_SPC_LR1").val("OK");
            document.getElementById("C02_SPC_LR1").style.color = "green";
        }
        else {
            $("#C02_SPC_LR1").val("NG");
            document.getElementById("C02_SPC_LR1").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox2").change(function (e) {
    C02_SPC_TextBox2_TextChanged()
});
function C02_SPC_TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#C02_SPC_TextBox2").val();
        let STL2 = $("#C02_SPC_STL2").val();
        let STM2 = $("#C02_SPC_STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#C02_SPC_LR2").val("OK");
            document.getElementById("C02_SPC_LR2").style.color = "green";
        }
        else {
            $("#C02_SPC_LR2").val("NG");
            document.getElementById("C02_SPC_LR2").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox3").change(function (e) {
    C02_SPC_TextBox3_TextChanged()
});
function C02_SPC_TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#C02_SPC_TextBox3").val();
        let STL3 = $("#C02_SPC_STL3").val();
        let STM3 = $("#C02_SPC_STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#C02_SPC_LR3").val("OK");
            document.getElementById("C02_SPC_LR3").style.color = "green";
        }
        else {
            $("#C02_SPC_LR3").val("NG");
            document.getElementById("C02_SPC_LR3").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox4").change(function (e) {
    C02_SPC_TextBox4_TextChanged()
});
function C02_SPC_TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#C02_SPC_TextBox4").val();
        let STL4 = $("#C02_SPC_STL4").val();
        let STM4 = $("#C02_SPC_STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#C02_SPC_LR4").val("OK");
            document.getElementById("C02_SPC_LR4").style.color = "green";
        }
        else {
            $("#C02_SPC_LR4").val("NG");
            document.getElementById("C02_SPC_LR4").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox5").change(function (e) {
    C02_SPC_TextBox5_TextChanged()
});
function C02_SPC_TextBox5_TextChanged() {
    try {
        let TextBox5 = $("#C02_SPC_TextBox5").val();
        let STL5 = $("#C02_SPC_STL5").val();
        let STM5 = $("#C02_SPC_STM5").val();
        if (SPCValuesCheckValue(TextBox5, STL5, STM5)) {
            $("#C02_SPC_LR5").val("OK");
            document.getElementById("C02_SPC_LR5").style.color = "green";
        }
        else {
            $("#C02_SPC_LR5").val("NG");
            document.getElementById("C02_SPC_LR5").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox6").change(function (e) {
    C02_SPC_TextBox6_TextChanged()
});
function C02_SPC_TextBox6_TextChanged() {
    try {
        let TextBox6 = $("#C02_SPC_TextBox6").val();
        let STL6 = $("#C02_SPC_STL6").val();
        let STM6 = $("#C02_SPC_STM6").val();
        if (SPCValuesCheckValue(TextBox6, STL6, STM6)) {
            $("#C02_SPC_LR6").val("OK");
            document.getElementById("C02_SPC_LR6").style.color = "green";
        }
        else {
            $("#C02_SPC_LR6").val("NG");
            document.getElementById("C02_SPC_LR6").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox7").change(function (e) {
    C02_SPC_TextBox7_TextChanged()
});
function C02_SPC_TextBox7_TextChanged() {
    try {
        let TextBox7 = $("#C02_SPC_TextBox7").val();
        let STL7 = $("#C02_SPC_STL7").val();
        let STM7 = $("#C02_SPC_STM7").val();
        if (SPCValuesCheckValue(TextBox7, STL7, STM7)) {
            $("#C02_SPC_LR7").val("OK");
            document.getElementById("C02_SPC_LR7").style.color = "green";
        }
        else {
            $("#C02_SPC_LR7").val("NG");
            document.getElementById("C02_SPC_LR7").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox8").change(function (e) {
    C02_SPC_TextBox8_TextChanged()
});
function C02_SPC_TextBox8_TextChanged() {
    try {
        let TextBox8 = $("#C02_SPC_TextBox8").val();
        let STL8 = $("#C02_SPC_STL8").val();
        let STM8 = $("#C02_SPC_STM8").val();
        if (SPCValuesCheckValue(TextBox8, STL8, STM8)) {
            $("#C02_SPC_LR8").val("OK");
            document.getElementById("C02_SPC_LR8").style.color = "green";
        }
        else {
            $("#C02_SPC_LR8").val("NG");
            document.getElementById("C02_SPC_LR8").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox9").change(function (e) {
    C02_SPC_TextBox9_TextChanged()
});
function C02_SPC_TextBox9_TextChanged() {
    try {
        let AA = Number($("#C02_SPC_TextBox9").val());
        let CC = Number($("#C02_SPC_Label17").val());

        if (CC <= AA) {
            $("#C02_SPC_LR9").val("OK");
            document.getElementById("C02_SPC_LR9").style.color = "green";
        }
        else {
            $("#C02_SPC_LR9").val("NG");
            document.getElementById("C02_SPC_LR9").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}
$("#C02_SPC_TextBox10").change(function (e) {
    C02_SPC_TextBox10_TextChanged()
});
function C02_SPC_TextBox10_TextChanged() {
    try {
        let TextBox10 = $("#C02_SPC_TextBox10").val();
        let STL10 = $("#C02_SPC_STL10").val();
        let STM10 = $("#C02_SPC_STM10").val();
        if (SPCValuesCheckValue(TextBox10, STL10, STM10)) {
            $("#C02_SPC_LR10").val("OK");
            document.getElementById("C02_SPC_LR10").style.color = "green";
        }
        else {
            $("#C02_SPC_LR10").val("NG");
            document.getElementById("C02_SPC_LR10").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C02_SPC_Form_RUS();
}

function C02_SPC_Buttonclose_Click() {
    $("#C02_SPC").modal("close");
    C02_SPC_FormClosed();
}
function C02_SPC_FormClosed() {
    C02_START_V2_SPC_LOAD(0);
}