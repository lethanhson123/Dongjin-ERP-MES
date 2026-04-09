let TagIndex = 1;
let DateNow;
let currentData = [];
let spcTables = {};

/* ===============================
   HELPER
================================= */

function getSuffix() {
    switch (TagIndex) {
        case 4: return "_Crim";
        case 5: return "_USW";
        case 6: return "_Twist";
        default: return "";
    }
}

function getTableSelector() {
    switch (TagIndex) {
        case 3: return "#spcListDetailTable";
        case 4: return "#spcListDetailTable_Crim";
        case 5: return "#spcListDetailTable_USW";
        case 6: return "#spcListDetailTable_Twist";
        default: return null;
    }
}

function getTableKey() {
    return "T" + TagIndex;
}

/* ===============================
   INIT DATATABLE (1 lần)
================================= */

function initSpcTable() {

    const selector = getTableSelector();
    if (!selector) return null;

    const key = getTableKey();
    if (spcTables[key]) return spcTables[key];

    spcTables[key] = $(selector).DataTable({
        paging: true,
        searching: false,
        ordering: true,
        info: true,
        scrollX: true,
        scrollY: "55vh",
        deferRender: true,
        autoWidth: false,
        pageLength: 100,
        lengthMenu: [50, 100, 200, 500],
        language: {
            emptyTable: "No data"
        }
    });

    return spcTables[key];
}

/* ===============================
   RENDER DATA (FAST)
================================= */

function renderSpcListDetail() {

    const table = initSpcTable();
    if (!table) return;

    table.clear();

    if (!currentData || currentData.length === 0) {
        table.draw();
        return;
    }

    const rows = currentData.map(r => ([
        r.TORDER_FG ?? "",
        r.COLSIP ?? "",
        r.WORK_WEEK ?? "",
        r.ORDER_IDX ?? "",
        r.PROJECT ?? "",
        r.TestPos ?? "",
        r.LEAD_NO ?? "",
        r.BUNDLE_SIZE ?? "",
        r.STAGE ?? "",
        r.QtyPlan ?? 0,
        r.QtyActual ?? 0,
        r.HOOK_RACK ?? "",
        r.WIRE ?? "",
        r.WIRE_NM ?? "",
        r.W_Diameter ?? "",
        r.W_Color ?? "",
        r.W_Length ?? "",
        r.TERM1 ?? "",
        r.STRIP1 ?? "",
        r.SEAL1 ?? "",
        r.CCH_W1 ?? "",
        r.ICH_W1 ?? "",
        r.TERM2 ?? "",
        r.STRIP2 ?? "",
        r.SEAL2 ?? "",
        r.CCH_W2 ?? "",
        r.ICH_W2 ?? "",
        r.CONDITION ?? "",
        r.FCTRY_NM ?? "",
        r.DatePlan ?? "",
        r.CCH1 ?? "",
        r.CCW1 ?? "",
        r.CCH2 ?? "",
        r.CCW2 ?? "",
        r.ICH1 ?? "",
        r.ICW1 ?? "",
        r.ICH2 ?? "",
        r.ICW2 ?? "",
        r.WIRE_FORCE ?? "",
        r.WIRE_LENGTH ?? "",
        r.IN_RESILT ?? "",
        r.TestTime ?? "",
        r.DATE ?? "",
        r.MCPlan ?? "",
        r.MC2 ?? "",
        r.FIRST_TIME ?? "",
        r.END_TIME ?? "",
        r.CREATE_USER ?? "",
        r.FullName ?? ""
    ]));

    table.rows.add(rows).draw(false);
}

/* ===============================
   BUTTON FIND
================================= */

async function Buttonfind_Click() {

    if (TagIndex < 3) return;

    const suffix = getSuffix();

    try {
        $("#BackGround").show();
        currentData = [];
        renderSpcListDetail();

        const param = {
            Action: TagIndex,
            USER_ID: GetCookieValue("UserID"),
            USER_IDX: GetCookieValue("USER_IDX"),
            rb_Fac1: document.getElementById(`rb_Fac1${suffix}`).checked,
            rb_Fac2: document.getElementById(`rb_Fac2${suffix}`).checked,
            ListSearchString: [
                $(`#txt_FromDate${suffix}`).val(),
                $(`#txt_toDate${suffix}`).val(),
                $(`#txt_MC${suffix}`).val(),
                $(`#txt_EmployeeID${suffix}`).val(),
                $(`#txt_Search${suffix}`).val()
            ]
        };

        const form = new FormData();
        form.append("BaseParameter", JSON.stringify(param));

        const res = await fetch("/C20/Buttonfind_Click", {
            method: "POST",
            body: form
        });

        const json = await res.json();
        currentData = json?.spcListDetail || [];

        renderSpcListDetail();

    } catch (err) {
        console.error("Buttonfind_Click", err);
    } finally {
        $("#BackGround").hide();
    }
}

/* ===============================
   INIT PAGE
================================= */

$(document).ready(function () {

    // Date default
    DateNow = DateToString(new Date());
    $("input[id^='txt_FromDate'], input[id^='txt_toDate']").val(DateNow);

    // Default radio
    $("input[id^='rb_Fac1']").prop("checked", true);

    // Tabs
    $(".tabs").tabs();

    // Button
    $("#Buttonfind").on("click", Buttonfind_Click);

    // Change tab
    $("[id^='ATag']").on("click", function () {
        TagIndex = Number(this.id.replace("ATag", ""));
    });
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

function Buttonadd_Click() {

}
function Buttonsave_Click() {

}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}

function Buttonexport_Click() {
    if (TagIndex >= 3) {
        let suffix = "";
        if (TagIndex === 3) suffix = "";
        else if (TagIndex === 4) suffix = "_Crim";
        else if (TagIndex === 5) suffix = "_USW";
        else if (TagIndex === 6) suffix = "_Twist";

        let txt_FromDate = $(`#txt_FromDate${suffix}`).val();
        let txt_toDate = $(`#txt_toDate${suffix}`).val();
        let txt_MC = $(`#txt_MC${suffix}`).val();
        let txt_EmployeeID = $(`#txt_EmployeeID${suffix}`).val();
        let txt_Search = $(`#txt_Search${suffix}`).val();

        if (!txt_FromDate || !txt_toDate) {
            alert("Vui lòng nhập ngày bắt đầu và ngày kết thúc");
            return;
        }

        $("#BackGround").css("display", "block");
        ShowExportProgress("Đang khởi tạo...", 0);

        let BaseParameter = {
            Action: TagIndex,
            ListSearchString: [txt_FromDate, txt_toDate, txt_MC, txt_EmployeeID, txt_Search],
            USER_ID: GetCookieValue("UserID"),
            USER_IDX: GetCookieValue("USER_IDX"),
            rb_Fac1: document.getElementById(`rb_Fac1${suffix}`)?.checked || false,
            rb_Fac2: document.getElementById(`rb_Fac2${suffix}`)?.checked || false
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

        fetch("/C20/Buttonexport_Click", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                if (data && data.JobId) {
                    let jobId = data.JobId;
                    console.log("Job ID:", jobId);
                    checkExportStatus(jobId);
                } else {
                    alert(data.Error || "Không thể xuất file");
                    $("#BackGround").hide();
                    HideExportProgress();
                }
            })
            .catch(err => {
                console.error("Lỗi:", err);
                alert("Đã xảy ra lỗi khi kết nối server.");
                $("#BackGround").hide();
                HideExportProgress();
            });
    }
}

function checkExportStatus(jobId) {
    let pollCount = 0;
    const maxPolls = 180; // 30 phút (mỗi 10 giây)
    let formData = new FormData();
    formData.append("jobId", jobId);

    const interval = setInterval(() => {
        pollCount++;

        fetch("/C20/CheckExportStatus", {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(status => {
                console.log("Status:", status);

                if (status.Code === "Completed") {
                    clearInterval(interval);

                    let totalRows = status.TotalRows || 0;
                    ShowExportProgress(`Hoàn tất! Đã xử lý ${totalRows} dòng. Đang tải file...`, 100);

                    if (status.DownloadUrl) {
                        window.location.href = status.DownloadUrl;
                    }

                    setTimeout(() => {
                        $("#BackGround").hide();
                        HideExportProgress();
                    }, 3000);

                } else if (status.Code === "Failed") {
                    clearInterval(interval);
                    alert(status.Error || "Xuất file thất bại");
                    $("#BackGround").hide();
                    HideExportProgress();

                } else {
                    let progress = status.Progress || 0;
                    let currentRow = status.CurrentRow || 0;
                    let totalRows = status.TotalRows || 0;
                    let action = status.CurrentAction || "Đang xử lý";

                    let message = status.Message || action;
                    if (totalRows > 0 && currentRow > 0) {
                        message = `${action} (${progress}%)`;
                    }

                    ShowExportProgress(message, progress);
                }

                if (pollCount >= maxPolls) {
                    clearInterval(interval);
                    alert("Quá thời gian xử lý. Vui lòng thử lại.");
                    $("#BackGround").hide();
                    HideExportProgress();
                }
            })
            .catch(err => {
                console.error("Polling error:", err);
            });
    }, 3000);
}

function ShowExportProgress(message, percent) {
    let progressDiv = document.getElementById("exportProgress");
    if (!progressDiv) {
        progressDiv = document.createElement("div");
        progressDiv.id = "exportProgress";
        progressDiv.style.position = "fixed";
        progressDiv.style.top = "50%";
        progressDiv.style.left = "50%";
        progressDiv.style.transform = "translate(-50%, -50%)";
        progressDiv.style.backgroundColor = "#333";
        progressDiv.style.color = "#fff";
        progressDiv.style.padding = "20px";
        progressDiv.style.borderRadius = "5px";
        progressDiv.style.zIndex = "9999";
        progressDiv.style.minWidth = "350px";
        progressDiv.style.textAlign = "center";
        progressDiv.style.boxShadow = "0 4px 8px rgba(0,0,0,0.2)";

        progressDiv.innerHTML = `
            <div id="progressMessage" style="margin-bottom: 15px; font-weight: bold; font-size: 14px;"></div>
            <div style="width: 100%; height: 25px; background-color: #555; border-radius: 12px; overflow: hidden; margin-bottom: 10px;">
                <div id="progressBar" style="width: 0%; height: 100%; background: linear-gradient(90deg, #4CAF50, #8BC34A); transition: width 0.3s;"></div>
            </div>
            <div id="progressPercent" style="margin-top: 5px; font-size: 13px; color: #ddd;"></div>
            <div id="progressDetail" style="margin-top: 8px; font-size: 12px; color: #aaa;"></div>
        `;

        document.body.appendChild(progressDiv);
    }

    document.getElementById("progressMessage").innerHTML = message;
    document.getElementById("progressBar").style.width = percent + "%";
    document.getElementById("progressPercent").innerHTML = percent + "%";

    let detailDiv = document.getElementById("progressDetail");
    if (detailDiv) {
        let match = message.match(/(\d+)\/(\d+)/);
        if (match) {
            detailDiv.innerHTML = `${match[1]} / ${match[2]} dòng`;
        }
    }

    progressDiv.style.display = "block";
}

function HideExportProgress() {
    let progressDiv = document.getElementById("exportProgress");
    if (progressDiv) {
        progressDiv.style.display = "none";
    }
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