let BaseResult;
let Tab1_table;
let Tab2_table;
let Tab3_table;
let Tab4_table;
let Tab5_table;

$(document).ready(function () {
    Tab1_table = $("#CuttingScrapTable").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
    });

    Tab2_table = $("#Tab2_Table").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
    });
    Tab3_table = $("#Tab3_Table").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
    });
    Tab4_table = $("#Tab4_Table").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
    });
    Tab5_table = $("#Tab5_Table").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
    });

    // Thêm min-height sau khi bảng đã khởi tạo, tùy vào kích thước mà điều chỉnh cho phù hợp màn hình
    $(".dataTables_scrollBody").css("min-height", "50vh");

    // Chọn tất cả các dòng
    $('#checkAll').on('click', function () {
        const isChecked = $(this).is(':checked');
        $('.row-check').prop('checked', isChecked);
    });

    // Nếu bỏ chọn từng dòng → cập nhật lại checkbox tổng
    $('#CuttingScrapTable tbody').on('change', '.row-check', function () {
        const allChecked = $('.row-check').length === $('.row-check:checked').length;
        $('#checkAll').prop('checked', allChecked);
    });

    LoadInput();
    LoadCompanyAndMachineSelectors();
});

document.querySelectorAll('.tabs .tab a').forEach(tab => {
    tab.addEventListener("click", function () {
        $("#BackGround").css("display", "block");
        clearTimeout(findTimeout);
        findTimeout = setTimeout(() => RunSearch(), 1000); 
    });
});

function formatDate(dateStr) {
    if (!dateStr) return "";
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return dateStr;
    return d.toISOString().split("T")[0];
}

function formatDateTime(dateStr) {
    if (!dateStr) return "";
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return dateStr;
    return d.getFullYear() + "-"
        + String(d.getMonth() + 1).padStart(2, "0") + "-"
        + String(d.getDate()).padStart(2, "0") + " "
        + String(d.getHours()).padStart(2, "0") + ":"
        + String(d.getMinutes()).padStart(2, "0") + ":"
        + String(d.getSeconds()).padStart(2, "0");
}

//load thong tin chọn công ty và line tương ứng
function LoadCompanyAndMachineSelectors() {

    const companySelect = document.getElementById("Tab1_select_Company");
    const machineSelect = document.getElementById("Tab1_select_Machine");

    // 1. Load danh sách công ty
    const companies = [
        { value: "DJG", text: "DJG" },
        { value: "DJM", text: "DJM" }
    ];

    // Xoá toàn bộ option cũ
    companySelect.innerHTML = "";

    // Thêm placeholder
    const defaultOption = document.createElement("option");
    defaultOption.value = "";
    defaultOption.text = "-- Select Company --";
    companySelect.appendChild(defaultOption);

    // Load danh sách công ty
    companies.forEach(c => {
        let op = document.createElement("option");
        op.value = c.value;
        op.text = c.text;
        companySelect.appendChild(op);
    });


    // 2. Bắt sự kiện khi thay đổi công ty
    companySelect.addEventListener("change", function () {
        const company = this.value;

        // Xoá tất cả machine, giữ lại option All
        while (machineSelect.options.length > 1) {
            machineSelect.remove(1);
        }

        let machines = [];

        if (company === "DJM") {
            machines = ["ZA801", "ZA802", "ZA803", "ZA804", "ZA805", "ZA806", "ZA807", "ZA808", "ZA809", "ZA810"];
        }

        // DHM → A801 → A815
        else if (company === "DJG") {
            machines.push("A501");
            machines.push("A505");  
            for (let i = 1; i <= 16; i++) {
                const num = String(i).padStart(2, '0');
                machines.push("A8" + num);  // A801 → A815
            }
                   
        }

        // Load máy tương ứng
        machines.forEach(m => {
            let op = document.createElement("option");
            op.value = m;
            op.text = m;
            machineSelect.appendChild(op);
        });

        // Reset về All
        machineSelect.value = "0";
    });
}

//load input ngay tháng
function LoadInput() {
;
    const fromDateInput = document.getElementById("Tab1_FromDate");
    const toDateInput = document.getElementById("Tab1_ToDate");          

    // Lấy ngày hiện tại theo định dạng yyyy-mm-dd
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0'); // Tháng từ 0->11
    const dd = String(today.getDate()).padStart(2, '0');
    const formattedDate = `${yyyy}-${mm}-${dd}`;

    // Gán giá trị ngày hiện tại cho input
    fromDateInput.value = formattedDate;
    toDateInput.value = formattedDate;
};

//load chi tiết phe liệu vào table 1
function RenderCuttingScrapTable(data) {
    Tab1_table.clear();  // Xóa dữ liệu cũ nhanh

    if (!data || data.length === 0) {
        Tab1_table.draw();
        return;
    }

    let rows = [];
    let stt = 0;

    for (let row of data) {
        stt++;
        rows.push([
            stt,
            row.TORDER_FG ?? "",
            row.LEAD_NO ?? "",
            row.MC ?? "",
            formatDate(row.DT),
            row.TOT_QTY ?? "",
            row.PERFORMN ?? "",
            row.W_Length ?? "",
            row.CONDITION ?? "",
            row.WireNumber ?? "",
            row.TERM1 ?? "",
            row.SEAL1 ?? "",
            row.WireCode ?? "",
            row.WIRE_NM ?? "",
            row.WIRE_LENGTH ?? "",
            row.TERM2 ?? "",
            row.SEAL2 ?? "",
            formatDateTime(row.CREATE_DTM),
            row.CREATE_USER ?? ""
        ]);

    }

    // Tối ưu: add 1 lần duy nhất
    Tab1_table.rows.add(rows).draw(false);
};

//load bao cáo tab2
function RenderReport2(data) {
    Tab2_table.clear();  // Xóa dữ liệu cũ nhanh

    if (!data || data.length === 0) {
        Tab2_table.draw();
        return;
    }

    let rows = [];
    let stt = 0;
    const fmt = (num) => Number(num ?? 0).toLocaleString("en-US");

    for (let row of data) {
        stt++;
        rows.push([
            stt,
            row.MC ?? "",      
            row.MType ?? "",
            row.Unit ?? "",
            fmt(row.Total_Plan),
            fmt(row.Total_Actual),
            fmt(row.Total_Using),
            fmt(row.Total_Scrap),
            fmt(row.Percentage),              
        ]);

    }

    // Tối ưu: add 1 lần duy nhất
    Tab2_table.rows.add(rows).draw(false);
};

//load bao cao tab3
function RenderReport3(data) {
    Tab3_table.clear();  // Xóa dữ liệu cũ nhanh

    if (!data || data.length === 0) {
        Tab3_table.draw();
        return;
    }

    let rows = [];
    let stt = 0;
    const fmt = (num) => Number(num ?? 0).toLocaleString("en-US");

    for (let row of data) {
        stt++;
        rows.push([
            stt,      
            row.PROJECT ?? "",
            row.LEAD_NO ?? "",
            row.MC ?? "",
            row.CREATE_USER ?? "",
            formatDate(row.Plan_Date),
            row.MaterialCode ?? "",
            row.MType ?? "",
            fmt(row.Total_Plan),
            fmt(row.Total_Actual),
            fmt(row.Total_BOM),
            fmt(row.Total_Using),
            fmt(row.Total_Scrap),
            fmt(row.Percentage),       
      
            row.Unit ?? "",
            row.CONDITION ?? "",
        ]);

    }

    // Tối ưu: add 1 lần duy nhất
    Tab3_table.rows.add(rows).draw(false);
};
//load bao cáo cho tab4
function RenderReport4(data) {
    Tab4_table.clear();  // Xóa dữ liệu cũ nhanh

    if (!data || data.length === 0) {
        Tab4_table.draw();
        return;
    }

    let rows = [];
    let stt = 0;
    const fmt = (num) => Number(num ?? 0).toLocaleString("en-US");

    for (let row of data) {
        stt++;
        rows.push([
            stt,
            row.MaterialCode ?? "",
            row.MType ?? "",
            row.Unit ?? "",
            fmt(row.Total_Plan),
            fmt(row.Total_Actual),
            fmt(row.Total_Using),
            fmt(row.Total_Scrap),
            fmt(row.Percentage),            
        ]);

    }

    // Tối ưu: add 1 lần duy nhất
    Tab4_table.rows.add(rows).draw(false);
};

//load bao cáo tab5
function RenderReport5(data) {
    Tab5_table.clear();  // Xóa dữ liệu cũ nhanh

    if (!data || data.length === 0) {
        Tab5_table.draw();
        return;
    }

    let rows = [];
    let stt = 0;

    // Hàm format chung
    const fmt = (num) => Number(num ?? 0).toLocaleString("en-US");

    for (let row of data) {
        stt++;
        rows.push([
            stt,
            row.MC ?? "",
            row.CREATE_USER ?? "",
            row.USER_NM ?? "",
            row.MType ?? "",
            row.Unit ?? "",
            fmt(row.Total_Plan),
            fmt(row.Total_Actual),
            fmt(row.Total_Using),
            fmt(row.Total_Scrap),
            fmt(row.Percentage),
        ]);
    }

    // Tối ưu: add 1 lần duy nhất
    Tab5_table.rows.add(rows).draw(false);
};

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

// Debounce để tránh spam server khi user click nhanh
let findTimeout;
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");

    clearTimeout(findTimeout);
    findTimeout = setTimeout(() => RunSearch(), 1000);       
}

async function RunSearch() {
    $("#BackGround").css("display", "block");

    try {
        let selectTab = 1;
        var activeTab = document.querySelector('.tabs .tab a.active');
        if (activeTab) {
            var currentTabId = activeTab.getAttribute('href').replace('#', '');

            if (currentTabId === 'Tag001') {
                selectTab = 1;
            } else if (currentTabId === 'Tag002') {
                selectTab = 2;
            } else if (currentTabId === 'Tag003') {
                selectTab = 3;

            }
            else if (currentTabId === 'Tag004') {
                selectTab = 4;

            }
            else if (currentTabId === 'Tag005') {
                selectTab = 5;

            }
        }


        const BaseParameter = {
            SearchString: $("#Tab1_search").val(),
            MC_NAME: $("#Tab1_select_Machine").val(),
            FromDate: $("#Tab1_FromDate").val(),
            ToDate: $("#Tab1_ToDate").val(),
            Action: selectTab
        };

        const formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

        const response = await fetch("/C18/Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();

        if (selectTab == 1) {
            RenderCuttingScrapTable(data.DataGridView);
           
        }
        else if (selectTab == 2) {
            RenderReport2(data.ScrapListView);
        }
        else if (selectTab == 3) {
            RenderReport3(data.ScrapListView);
        }
        else if (selectTab == 4) {
            RenderReport4(data.ScrapListView);
        }

        else if (selectTab == 5) {
            RenderReport5(data.ScrapListView);
        }

       
    }
    catch (err) {
        console.error(err);
    }
    finally {
        $("#BackGround").css("display", "none");
    }
}

async function RunSearch_ParallelOrdered() {
    $("#BackGround").css("display", "block");
    $("#btnSearch").prop("disabled", true);

    try {
        const base = {
            SearchString: $("#Tab1_search").val(),
            MC_NAME: $("#Tab1_select_Machine").val(),
            FromDate: $("#Tab1_FromDate").val(),
            ToDate: $("#Tab1_ToDate").val()
        };

        const actions = [1, 2, 3, 4, 5];

        // tạo mảng promise nhưng không await ngay
        const promises = actions.map(act => {
            const BaseParameter = { ...base, Action: act };
            const formUpload = new FormData();
            formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            return fetch("/C18/Buttonfind_Click", { method: "POST", body: formUpload })
                .then(resp => resp.json())
                .then(data => ({ act, data }))   // giữ act để biết tab nào
                .catch(err => ({ act, err }));
        });

        // chờ tất cả về
        const results = await Promise.all(promises);

        // sắp xếp theo act để render đúng thứ tự 1..5
        results.sort((a, b) => a.act - b.act);

        for (const r of results) {
            if (r.err) {
                console.error("Lỗi tab", r.act, r.err);
                continue;
            }
            const data = r.data;
            switch (r.act) {
                case 1: RenderCuttingScrapTable(data.DataGridView); break;
                case 2: RenderReport2(data.ScrapListView); break;
                case 3: RenderReport3(data.ScrapListView); break;
                case 4: RenderReport4(data.ScrapListView); break;
                case 5: RenderReport5(data.ScrapListView); break;
            }
        }

        // Sau khi render xong 5 tab → auto fix header
        setTimeout(() => {
            Tab1_table.columns.adjust();
            Tab2_table.columns.adjust();
            Tab3_table.columns.adjust();
            Tab4_table.columns.adjust();
            Tab5_table.columns.adjust();
        }, 200);
    
    } catch (err) {
        console.error(err);
    } finally {
        $("#BackGround").css("display", "none");
        $("#btnSearch").prop("disabled", false);
    }
}


function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttonadd_Click";

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
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttonsave_Click";

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
function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttondelete_Click";

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
function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttoncancel_Click";

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
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttoninport_Click";

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
function Buttonexport_Click() {
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            DataTableToExcel(Tab1_table, "Cutting_Scrap", "CuttingScrapTableDetail",);

        } else if (currentTabId === 'Tag002') {
            DataTableToExcel(Tab2_table, "Machine_Scrap", "Machine_Scrap");

        } else if (currentTabId === 'Tag003') {
            DataTableToExcel(Tab3_table, "LeadNo_Scrap", "LeadNo_Scrap");
        }
        else if (currentTabId === 'Tag004') {
            DataTableToExcel(Tab4_table, "Material_Scrap", "Material_Scrap");
        }
    }

    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = "/C18/Buttonexport_Click";

    //fetch(url, {
    //    method: "POST",
    //    body: formUpload,
    //    headers: {
    //    }
    //}).then((response) => {
    //    response.json().then((data) => {
    //        $("#BackGround").css("display", "none");
    //    }).catch((err) => {
    //        $("#BackGround").css("display", "none");
    //    })
    //});
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C18/Buttonprint_Click";

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
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}


