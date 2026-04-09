let IsTableSort = false;
let BaseResult = {};
let TagIndex = 1;

$(document).ready(function () {
   
    $('.modal').modal();
});
$("#TabPage1").click(function () {
    TagIndex = 1;
});
$("#TabPage2").click(function () {
    TagIndex = 2;
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
$("#Button1").click(function () {
    Button1_Click();
});
$("#Buttonsave_A09_1").click(function () {
    Buttonsave_Click_A09_1();
});
$("#Buttoncancel_A09_1").click(function () {
    Buttoncancel_Click_A09_1();
});
$("#Buttonclose_A09_1").click(function () {
    Buttonclose_Click_A09_1();
});
$("#Buttoninport_A09_2").click(function () {
    $("#ExcelFile_A09_2").click();
});
$("#Buttonsave_A09_2").click(Buttonsave_Click_A09_2);
$("#Buttonclose_A09_2").click(Buttonclose_Click_A09_2);
function Buttonsave_Click_A09_2() {
    let rowCount = $("#DataGridView1_A09_2 tbody tr").length;
    if (rowCount === 0) {
        alert("Lỗi: Không có dữ liệu để lưu.");
        return;
    }

    $("#BackGround").css("display", "block");

    let ListSearchString = [];
    $("#DataGridView1_A09_2 tbody tr").each(function () {
        let cells = $(this).find("td");
        ListSearchString.push(cells.eq(0).text()); 
        ListSearchString.push(cells.eq(1).text()); 
        ListSearchString.push(cells.eq(2).text()); 
    });

    let BaseParameter = {
        ListSearchString: ListSearchString,
        USER_IDX: getCurrentUser()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/A09/Buttonsave_Click_A09_2", {
        method: "POST",
        body: formUpload
    }).then(response => response.json().then(data => {
        if (data.Error) {
            alert("Lỗi: " + data.Error);
        } else {
            alert("Đã được lưu.");
            $('#modal_A09_2').modal('close');
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert("Lỗi kết nối: " + err);
        $("#BackGround").css("display", "none");
    }));
}

function Buttonclose_Click_A09_2() {
    $('#modal_A09_2').modal('close');
}
function Button1_Click() {
    if (!$("#TextBox1").val()) return;

    $("#TextBox1_A09_1").val($("#TextBox1").val());
    $("#TextBox2_A09_1").val($("#TextBox2").val());
    $("#TextBox3_A09_1").val($("#TextBox3").val());
    $("#TextBox4_A09_1").val($("#TextBox4").val());
    $("#TextBox5_A09_1").val($("#TextBox5").val());
    $("#TextBox6_A09_1").val($("#TextBox6").val());

    $("#DateTimePicker1_A09_1").val(new Date().toISOString().split('T')[0]);
    $("#TextBox7_A09_1").val("0");

    try {
        let minDate = new Date($("#TextBox5").val());
        $("#DateTimePicker1_A09_1").attr("min", minDate.toISOString().split('T')[0]);
    } catch (e) { }

    M.updateTextFields();
    $('#modal_A09_1').modal('open');
}
function Buttonsave_Click_A09_1() {
    if ($("#TextBox7_A09_1").val() == "0") {
        alert("Lỗi: Giá trị chi phí không thể là 0.");
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [
            $("#TextBox1_A09_1").val(),
            $("#DateTimePicker1_A09_1").val(),
            $("#TextBox7_A09_1").val() 
        ],
        USER_IDX: getCurrentUser() 
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/A09/Buttonsave_Click_A09_1", {
        method: "POST",
        body: formUpload
    }).then(response => response.json().then(data => {
        if (data.Error) {
            alert("Lỗi: " + data.Error);
        } else {
            alert("Đã được lưu.");
            $('#modal_A09_1').modal('close');
            Buttonfind_Click();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        alert("Lỗi kết nối: " + err);
        $("#BackGround").css("display", "none");
    }));
}

function Buttoncancel_Click_A09_1() {
    $("#DateTimePicker1_A09_1").val(new Date().toISOString().split('T')[0]);
    $("#TextBox7_A09_1").val("0");
}

function Buttonclose_Click_A09_1() {
    $('#modal_A09_1').modal('close');
}
function Buttoninport_Click() {
    $("#ExcelFile_A09_2").val("");
    $("#DataGridView1_A09_2 tbody").empty();
    M.updateTextFields();
    $('#modal_A09_2').modal('open');

    $("#ExcelFile_A09_2").on('change', function (e) {
        var file = e.target.files[0];
        if (!file) return;

        var reader = new FileReader();
        reader.onload = function (e) {
            try {
                var data = new Uint8Array(e.target.result);
                var workbook = XLSX.read(data, { type: 'array' });
                var sheet = workbook.Sheets[workbook.SheetNames[0]];
                var jsonData = XLSX.utils.sheet_to_json(sheet, { header: ['PART_NO', 'DATE', 'Unit_Price'] }).slice(1);

                jsonData = jsonData.filter(row => row.PART_NO && row.Unit_Price);

                let html = "";
                jsonData.forEach(row => {
                    html += `<tr><td>${row.PART_NO}</td><td>${row.DATE}</td><td>${row.Unit_Price}</td></tr>`;
                });
                $("#DataGridView1_A09_2 tbody").html(html);

                alert("Completed. Imported to Excel");
            } catch (ex) {
                alert("Error: " + ex.message);
            }
        };
        reader.readAsArrayBuffer(file);
    });

  
}
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [],
        Action: TagIndex 
    };

    if (TagIndex === 1) {
        BaseParameter.ListSearchString = [
            $('#suchTB1').val(),
            $('#suchTB2').val(),
            $('#suchTB3').val()
        ];
    } else if (TagIndex === 2) {
        BaseParameter.ListSearchString = [
            $('#suchTB21').val(),
            $('#suchTB22').val(),
            $('#suchTB23').val()
        ];
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A09/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json().then(data => {
        BaseResult = data;
        if (TagIndex === 1) {
            DataGridView1Render();
        } else if (TagIndex === 2) {
            DataGridView2Render();
        }
        $("#BackGround").css("display", "none");
    }).catch(err => {
        $("#BackGround").css("display", "none");
    }));
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            HTML += "<tr>";
            HTML += "<td>" + (i + 1) + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_CAR + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_FML + "</td>";
            HTML += "<td>" + new Date(BaseResult.DataGridView2[i].TSCOST_DT).toISOString().slice(0, 10) + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].TSCOST_VAL + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].TSCOST_IDX + "</td>";
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + (i + 1) + "</td>"; // STT                   
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_CAR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_FML + "</td>";
                    HTML = HTML + "<td>" + new Date(BaseResult.DataGridView1[i].TSCOST_DT).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSCOST_VAL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSCOST_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
$(document).on('click', '#DataGridView1 tr', function () {
    let index = $(this).index();

    try {
     
        $('#TextBox1').val(BaseResult.DataGridView1[index].PART_NO);
        $('#TextBox2').val(BaseResult.DataGridView1[index].PART_NM);
        $('#TextBox3').val(BaseResult.DataGridView1[index].PART_CAR);
        $('#TextBox4').val(BaseResult.DataGridView1[index].PART_FML);
        $('#TextBox5').val(new Date(BaseResult.DataGridView1[index].TSCOST_DT).toISOString().slice(0, 10));
        $('#TextBox6').val(BaseResult.DataGridView1[index].TSCOST_VAL);
    } catch (ex) {
      
        $('#TextBox1').val('');
        $('#TextBox2').val('');
        $('#TextBox3').val('');
        $('#TextBox4').val('');
        $('#TextBox5').val('');
        $('#TextBox6').val('');
    }
});
$(document).on('click', '#DataGridView2 tr', function () {
    let index = $(this).index();

    try {
        $('#TextBox21').val(BaseResult.DataGridView2[index].PART_NO);
        $('#TextBox22').val(BaseResult.DataGridView2[index].PART_NM);
        $('#TextBox23').val(BaseResult.DataGridView2[index].PART_CODE);
        $('#TextBox24').val(BaseResult.DataGridView2[index].PART_FML);
        $('#TextBox25').val(new Date(BaseResult.DataGridView2[index].TSCOST_DT).toISOString().slice(0, 10));
        $('#TextBox26').val(BaseResult.DataGridView2[index].TSCOST_VAL);

        let BaseParameter = {
            ListSearchString: [$('#TextBox21').val()]
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/A09/LoadDataGridView3", {
            method: "POST",
            body: formUpload
        }).then(response => response.json().then(data => {
            BaseResult.DataGridView3 = data.DataGridView3;
            DataGridView3Render();
            RenderChart();
        }).catch(err => {
            console.error("Lỗi kết nối: " + err);
        }));
    } catch (ex) {
        $('#TextBox21').val('');
        $('#TextBox22').val('');
        $('#TextBox23').val('');
        $('#TextBox24').val('');
        $('#TextBox25').val('');
        $('#TextBox26').val('');
        $("#DataGridView3").empty();
    }
});
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            HTML += "<tr>";
            HTML += "<td>" + new Date(BaseResult.DataGridView3[i].TSCOST_DT).toISOString().slice(0, 10) + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSCOST_VAL + "</td>";
           
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        Action: 1 
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A09/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Lỗi: " + data.Error);
            } else {
                alert("Đã được lưu.");
                Buttonfind_Click(); 
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("Lỗi kết nối: " + err);
            $("#BackGround").css("display", "none");
        });
    });
}
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A09/Buttonsave_Click";

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
    let url = "/A09/Buttondelete_Click";

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
    let url = "/A09/Buttoncancel_Click";

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
    if (TagIndex === 1) {
        const table = document.getElementById("DataGridView1Table");
        const rows = table.querySelectorAll("tbody tr");
        if (rows.length > 0) {
            const data = [
                ["", "PART_NO", "PART_NAME", "PART_MODEL", "PART_GROUP", "COST_DATE", "COST_VALUE", "COST_IDX"],
                ...Array.from(rows).map(row => Array.from(row.querySelectorAll("td")).map(td => td.innerText))
            ];
            const wb = XLSX.utils.book_new();
            const ws = XLSX.utils.aoa_to_sheet(data);
            XLSX.utils.book_append_sheet(wb, ws, "PARTCOST");
            XLSX.writeFile(wb, "COST.xlsx");
        } else {
            alert("Không có dữ liệu!");
        }
    }
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A09/Buttonprint_Click";

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
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}
function RenderChart() {
    let existingChart = Chart.getChart("Chart");
    if (existingChart) {
        existingChart.destroy();
    }

    let chartData = [];
    let maxRows = Math.min(BaseResult.DataGridView3.length, 10);
    for (let i = 0; i < maxRows; i++) {
        let date = new Date(BaseResult.DataGridView3[i].TSCOST_DT).toISOString().slice(5, 10);
        let cost = Math.round(BaseResult.DataGridView3[i].TSCOST_VAL);
        chartData.push({ x: date, y: cost });
    }

    new Chart(document.getElementById("Chart").getContext("2d"), {
        type: "line",
        data: {
            labels: chartData.map(d => d.x),
            datasets: [{
                label: "Cost",
                data: chartData.map(d => d.y),
                borderColor: "#2196F3",
                backgroundColor: "rgba(33, 150, 243, 0.2)",
                fill: true
            }]
        },
        options: {
            responsive: false,
            maintainAspectRatio: false,
            scales: {
                x: { title: { display: true, text: "Date" } },
                y: { title: { display: true, text: "Cost" } }
            }
        }
    });
}

