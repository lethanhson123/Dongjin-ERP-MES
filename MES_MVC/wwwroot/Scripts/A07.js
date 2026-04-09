let BaseResult = new Object();

$(document).ready(function () {
    Load();
});

function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        if (!response.ok) {
            throw new Error("Server error: " + response.status);
        }
        return response.json();
    }).then((data) => {
        BaseResult = data;
        if (data.Error) {
            alert(data.Error);
        } else {
            TreeView1Render();
        }
        $("#BackGround").css("display", "none");
    }).catch((err) => {
        alert("Error: " + err.message);
        $("#BackGround").css("display", "none");
    });
}

function TreeView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DGV_A07_01 && BaseResult.DGV_A07_01.length > 0) {
        HTML += "<ul>";
        for (let i = 0; i < BaseResult.DGV_A07_01.length; i++) {
            let nodeText = BaseResult.DGV_A07_01[i].CDGR_NM_HAN + " (" + BaseResult.DGV_A07_01[i].CDGR_NM_EN + ")";
            HTML += "<li><a href='#' onclick='TreeView1_Click(" + i + ")'>" + nodeText + "</a></li>";
        }
        HTML += "</ul>";
    }
    document.getElementById("TreeView1").innerHTML = HTML;
}

function TreeView1_Click(index) {
    if (BaseResult && BaseResult.DGV_A07_01 && BaseResult.DGV_A07_01.length > 0) {
        let cdgr_idx = BaseResult.DGV_A07_01[index].CDGR_IDX;
        console.log("Nhấp vào CDGR_IDX:", cdgr_idx); // Gỡ lỗi
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            SearchString: cdgr_idx
        };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/A07/Buttonfind_Click";
        fetch(url, {
            method: "POST",
            body: formUpload
        }).then((response) => {
            if (!response.ok) {
                throw new Error("Lỗi server: " + response.status);
            }
            return response.json();
        }).then((data) => {
            console.log("Phản hồi server:", data); // Gỡ lỗi
            BaseResult.DataGridView1 = data.DataGridView1;
            if (data.Data) {
                $("#TextBox4").val(data.Data.CDGR_NM_HAN || "ERROR");
                $("#TextBox5").val(data.Data.CDGR_NM_EN || "ERROR");
                $("#Labelcode").text(data.Data.CDGR_SYSNOTE || "ERROR");
                $("#Label6").val(data.Data.CDGR_IDX || "ERROR");
            } else {
                console.warn("Không có data.Data trong phản hồi"); // Gỡ lỗi
                $("#TextBox4").val("ERROR");
                $("#TextBox5").val("ERROR");
                $("#Labelcode").text("ERROR");
                $("#Label6").val("ERROR");
            }
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Lỗi fetch:", err.message); // Gỡ lỗi
            $("#TextBox4").val("ERROR");
            $("#TextBox5").val("ERROR");
            $("#Labelcode").text("ERROR");
            $("#Label6").val("ERROR");
            alert("Lỗi: " + err.message);
            $("#BackGround").css("display", "none");
        });
    } else {
        console.error("BaseResult hoặc DGV_A07_01 không hợp lệ"); // Gỡ lỗi
    }
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
            HTML += "<td><label><input type='checkbox' class='filled-in' id='DataGridView1CHK" + i + "' /><span></span></label></td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].CD_IDX || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].CD_SYS_NOTE || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].CD_NM_HAN || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].CD_NM_EN || "") + "</td>";
            HTML += "</tr>";
        }
    }
    document.querySelector("#DataGridView1 tbody").innerHTML = HTML;
}

// Placeholder cho các hàm nút khác
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        if (!response.ok) {
            throw new Error("Server error: " + response.status);
        }
        return response.json();
    }).then((data) => {
        if (data.Error) {
            alert(data.Error);
        } else {
            $("#TextBox1").val("");
            $("#TextBox2").val("");
            $("#Label3").val("");
            alert(data.Data || "Ready to add new record");
        }
        $("#BackGround").css("display", "none");
    }).catch((err) => {
        alert("Error: " + err.message);
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    BaseParameter.ListSearchString.push($("#Label6").val());
    BaseParameter.ListSearchString.push($("#Label3").val());
    BaseParameter.ListSearchString.push($("#TextBox1").val());
    BaseParameter.ListSearchString.push($("#TextBox2").val());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        if (!response.ok) {
            throw new Error("Server error: " + response.status);
        }
        return response.json();
    }).then((data) => {
        if (data.Error) {
            alert(data.Error);
        } else {
            alert(data.Data || "Save database complete");
            Load(); // Làm mới dữ liệu
        }
        $("#BackGround").css("display", "none");
    }).catch((err) => {
        alert("Error: " + err.message);
        $("#BackGround").css("display", "none");
    });
}

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A07/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}
function DataGridView1_SelectionChanged(index) {
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        $("#TextBox1").val(BaseResult.DataGridView1[index].CD_NM_HAN || "ERROR");
        $("#TextBox2").val(BaseResult.DataGridView1[index].CD_NM_EN || "ERROR");
        $("#Label3").val(BaseResult.DataGridView1[index].CD_IDX || "ERROR");
    } else {
        $("#TextBox1").val("ERROR");
        $("#TextBox2").val("ERROR");
        $("#Label3").val("ERROR");
    }
}