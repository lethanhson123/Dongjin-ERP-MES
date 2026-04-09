let BaseResult;
let isPanelVisible = false;
$(document).ready(function () {
    // Khởi tạo Materialize tabs
    if ($('.tabs').length) {
        $('.tabs').tabs();
        $('.tabs').tabs('select', 'TabPage2');
    } else {
        console.error("Tabs element not found");
    }

    // Thiết lập giá trị mặc định
    $("#ComboBox5").val("ALL");
    $("#ComboBox3").val("06:00");
    $("#ComboBox4").val("06:00");


    let today = new Date();
    let tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    $("#DateTimePicker1").val(today.toISOString().split('T')[0]);
    $("#DateTimePicker2").val(tomorrow.toISOString().split('T')[0]);



    // Load dữ liệu
    initializeData();

    // Gọi tìm kiếm ban đầu cho Tab2
    $("#CheckBox1").prop("checked", false);
    $("#CheckBox1").trigger("change");
    Buttonfind_Click();

    // Lắng nghe sự kiện click trên tab
    $('.tabs .tab a').on('click', function () {
        setTimeout(function () {
            Buttonfind_Click();
        }, 100);
    });
});


// Thêm xử lý nút Toggle Panel2
$("#Button1").click(function () {
    togglePanel2();
});

// Hàm Toggle Panel2
function togglePanel2() {
    isPanelVisible = !isPanelVisible;

    if (isPanelVisible) {
        $("#Panel2").slideDown(200);
        $("#Button1").html('<i class="material-icons">visibility_off</i> Manual');
    } else {
        $("#Panel2").slideUp(200);
        $("#Button1").html('<i class="material-icons">visibility</i> Manual');
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
    $("#BackGround").css("display", "block");
    let activeTab = $(".tabs .tab a.active").attr("href");
    let BaseParameter = {
        TabSelected: activeTab === "#TabPage1" ? "TabPage1" : "TabPage2",
        ComboBox5: $("#ComboBox5").val(),
        ComboBox1: $("#ComboBox1").val(),
        ComboBox2: $("#ComboBox2").val(),
        ComboBox3: $("#ComboBox3").val(),
        ComboBox4: $("#ComboBox4").val(),
        DateTimePicker1: $("#DateTimePicker1").val(),
        DateTimePicker2: $("#DateTimePicker2").val(),
        TextBox1: $("#TextBox1").val(),
        CheckBox1: $("#CheckBox1").is(":checked")
    };
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/H02/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json()).then(data => {
        BaseResult = data;


        if (activeTab === "#TabPage1") {
            DataGridView2Render();
            DataGridView3Render();
            RenderChart();
        } else {
            DataGridView1Render();
            DataGridView4Render();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {

        $("#BackGround").css("display", "none");
    });
}
$("#CheckBox1").change(function () {
    // Nếu checkbox được chọn, vô hiệu hóa các control ngày và giờ
    if ($(this).is(":checked")) {
        $("#DateTimePicker1").prop("disabled", true);
        $("#DateTimePicker2").prop("disabled", true);
        $("#ComboBox3").prop("disabled", true);
        $("#ComboBox4").prop("disabled", true);
    } else {
        // Nếu checkbox không được chọn, kích hoạt lại các control
        $("#DateTimePicker1").prop("disabled", false);
        $("#DateTimePicker2").prop("disabled", false);
        $("#ComboBox3").prop("disabled", false);
        $("#ComboBox4").prop("disabled", false);
    }
});

function RenderChart() {
    if (!BaseResult || !BaseResult.DataGridView2 || BaseResult.DataGridView2.length === 0) return;

    const row2 = BaseResult.DataGridView2[0];
    const ctx = document.getElementById("Chart").getContext("2d");

    if (window.myChart) window.myChart.destroy();

    window.myChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["A501", "A502", "A801", "A802", "A803", "A804", "A805", "A806", "A807", "A808", "A809", "A810", "A811", "A812", "A813", "A814", "A815", "A816"],
            datasets: [{
                label: "Non-Operation Time (Minutes)",
                data: [
                    Math.round(row2.A501 || 0), Math.round(row2.A502 || 0),
                    Math.round(row2.A801 || 0), Math.round(row2.A802 || 0),
                    Math.round(row2.A803 || 0), Math.round(row2.A804 || 0),
                    Math.round(row2.A805 || 0), Math.round(row2.A806 || 0),
                    Math.round(row2.A807 || 0), Math.round(row2.A808 || 0),
                    Math.round(row2.A809 || 0), Math.round(row2.A810 || 0),
                    Math.round(row2.A811 || 0), Math.round(row2.A812 || 0),
                    Math.round(row2.A813 || 0), Math.round(row2.A814 || 0),
                    Math.round(row2.A815 || 0), Math.round(row2.A816 || 0)
                ],
                backgroundColor: "#4caf50",
                borderColor: "#388e3c",
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
}
function DataGridView1Render() {
    let HTML = "";
    let Language = GetCookieValue("Language");
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    // Xác định mô tả dựa trên mã
                    let contentDesc = "";
                    if (BaseResult.DataGridView1[i].CODEH02) {
                        switch (BaseResult.DataGridView1[i].CODEH02) {
                            case "E": contentDesc = "Khác (기타)"; break;
                            case "I": contentDesc = "Thiếu NVL(자재결품)"; break;
                            case "L": contentDesc = "Giờ ăn (식사시간)"; break;
                            case "M": contentDesc = "Máy hư (설비고장)"; break;
                            case "N": contentDesc = "Đăng xuất (No Worker)"; break;
                            case "Q": contentDesc = "Vấn đề chết lượng (품질문제)"; break;
                            case "S": contentDesc = "Chuẩn bị TB (설비준비)"; break;
                            case "T": contentDesc = "Họp/đào tạo (교육 / 회의)"; break;
                            default: contentDesc = "";
                        }
                    }

                    // Định dạng lại DATE, S_TIME và E_TIME
                    let formattedDate = BaseResult.DataGridView1[i].DATE ? BaseResult.DataGridView1[i].DATE.split('T')[0] : "";
                    let formattedSTime = BaseResult.DataGridView1[i].S_TIME ? BaseResult.DataGridView1[i].S_TIME.replace('T', ' ') : "";
                    let formattedETime = BaseResult.DataGridView1[i].E_TIME ? BaseResult.DataGridView1[i].E_TIME.replace('T', ' ') : "";

                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CODEH02 || "") + "</td>";
                    HTML = HTML + "<td>" + formattedDate + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].S_USER || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].MC_NAME || "") + "</td>";
                    HTML = HTML + "<td>" + formattedSTime + "</td>";
                    HTML = HTML + "<td>" + formattedETime + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].TIME_S || "") + "</td>";
                    if (Language == "vi") {
                        HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].TIME_M / 100 || "") + "</td>";
                    }
                    else {
                        HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].TIME_M || "") + "</td>";
                    }
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].E_USER || "") + "</td>";
                    HTML = HTML + "<td>" + contentDesc + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView2[i].TIMEH02 || "") + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A501 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A502 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A801 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A802 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A803 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A804 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A805 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A806 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A807 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A808 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A809 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A810 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A811 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A812 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A813 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A814 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A815 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].A816 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView2[i].SUM || 0)) + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView3[i].CODEH02 || "ALL") + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A501 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A502 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A801 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A802 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A803 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A804 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A805 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A806 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A807 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A808 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A809 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A810 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A811 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A812 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A813 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A814 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A815 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].A816 || 0)) + "</td>";
                    HTML = HTML + "<td>" + (Math.round(BaseResult.DataGridView3[i].SUM || 0)) + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}

function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView4[i].MC_NAME || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView4[i].TIME_M || "") + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}
function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H02/Buttonadd_Click";

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
    let url = "/H02/Buttonsave_Click";

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
    let url = "/H02/Buttondelete_Click";

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
    let url = "/H02/Buttoncancel_Click";

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
function CB_LIST() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ComboBox5: $("#ComboBox5").val()
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/H02/CB_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json()).then(data => {
        if (data.ComboBox1 && data.ComboBox1.length > 0) {
            // Cập nhật ComboBox1
            let comboBox1 = $("#ComboBox1");
            comboBox1.empty();
            comboBox1.append(new Option("ALL", "ALL"));

            data.ComboBox1.forEach(item => {
                comboBox1.append(new Option(item.TSNON_OPER_MCNM, item.TSNON_OPER_MCNM));
            });

            comboBox1.val("ALL");
        }

        $("#BackGround").css("display", "none");
    }).catch(err => {
        console.error("Error:", err);
        $("#BackGround").css("display", "none");
    });
}
function initializeData() {
    $("#BackGround").css("display", "block");

    fetch("/H02/Load", {
        method: "POST"
    }).then(response => response.json()).then(data => {
        if (data.ComboBox1 && data.ComboBox1.length > 0) {
            // Cập nhật ComboBox1
            let comboBox1 = $("#ComboBox1");
            comboBox1.empty();
            comboBox1.append(new Option("ALL", "ALL"));

            data.ComboBox1.forEach(item => {
                comboBox1.append(new Option(item.TSNON_OPER_MCNM, item.TSNON_OPER_MCNM));
            });

            comboBox1.val("ALL");
        }

        if (data.ComboBox2 && data.ComboBox2.length > 0) {
            // Cập nhật ComboBox2
            let comboBox2 = $("#ComboBox2");
            comboBox2.empty();
            comboBox2.append(new Option("ALL", "ALL"));

            data.ComboBox2.forEach(item => {
                comboBox2.append(new Option(item.TSNON_OPER_CODE, item.TSNON_OPER_CODE));
            });

            comboBox2.val("ALL");
        }

        $("#BackGround").css("display", "none");
    }).catch(err => {
        console.error("Error:", err);
        $("#BackGround").css("display", "none");
    });
}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H02/Buttoninport_Click";

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
document.getElementById("Buttonexport").addEventListener("click", function () {

    const sheetName = "LIST";
    const fileName = "LIST.xlsx";
    TableHTMLToExcel('DataGridView1Table', fileName, sheetName);
});

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H02/Buttonexport_Click";

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
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H02/Buttonprint_Click";

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


