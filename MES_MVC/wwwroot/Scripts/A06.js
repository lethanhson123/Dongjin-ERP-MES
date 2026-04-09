let BaseResult;
let DataGridView1RowIndex = 0;

$(document).ready(function () {
    $('.modal').modal();
    // Xóa giá trị tất cả ô nhập khi load trang
    $('#SU01, #SU02, #SU03, #TextBox1, #TextBox2, #TextBox3, #TextBox4, #TextBox5, #TextBox6').val("");
    // Tắt autocomplete để tránh trình duyệt tự điền
    $('#SU02, #TextBox4').attr('autocomplete', 'off');
    // Đảm bảo không ghi đè giá trị sau khi load
    setTimeout(function () {
        $('#SU02, #TextBox4').val("");
    }, 1000); // Chạy lại sau 1 giây để ghi đè logic tự động điền
});
$("#SU01, #SU02, #SU03").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
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
$("#A06_1Button1").click(function () {
    A06_1Button1_Click();
});

$("#A06_1Button3").click(function () {
    A06_1Button3_Click();
});

$("#A06_1Button2").click(function () {
    $('#A06_1Modal').modal('close');
});

$("#A06_2Button1").click(function () {
    A06_2Button1_Click();
});

$("#A06_2Button2").click(function () {
    $('#A06_2Modal').modal('close');
});
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: []
    };
    BaseParameter.ListSearchString.push($('#SU01').val() || "");
    BaseParameter.ListSearchString.push($('#SU02').val() || "");
    BaseParameter.ListSearchString.push($('#SU03').val() || "");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A06/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            BaseResult = data;
            RenderDataGridView1();
            $("#BackGround").css("display", "none");
        }).catch(err => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
}
function RenderDataGridView1() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView && BaseResult.DataGridView.length > 0) {
        DataGridView1_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView.length; i++) {
            HTML += `<tr onclick='DataGridView1_SelectionChanged(${i})'>`;
            HTML += `<td>${BaseResult.DataGridView[i].USER_ID}</td>`;
            HTML += `<td>${BaseResult.DataGridView[i].USER_NM}</td>`;
            HTML += `<td>****</td>`; // Che mật khẩu
            HTML += `<td>${BaseResult.DataGridView[i].Dept}</td>`;
            HTML += `<td>${BaseResult.DataGridView[i].Note}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    if (BaseResult && BaseResult.DataGridView && BaseResult.DataGridView.length > 0) {
        $("#TextBox1").val(BaseResult.DataGridView[i].USER_IDX);
        $("#TextBox2").val(BaseResult.DataGridView[i].USER_ID);
        $("#TextBox3").val(BaseResult.DataGridView[i].USER_NM);
        $("#TextBox4").val(""); // Không hiển thị mật khẩu
        $("#TextBox5").val(BaseResult.DataGridView[i].Dept);
        $("#TextBox6").val(BaseResult.DataGridView[i].Note);
        DataGridView1RowIndex = i;
    }
}
function Buttonadd_Click() {
    $('#TextBox1, #TextBox2, #TextBox3, #TextBox4, #TextBox5, #TextBox6').val("");
    $('#A06_1Modal').modal('open');
}
function Buttonsave_Click() {
    if ($('#TextBox2').val() === "") {
        alert("User ID không được để trống.");
        return;
    }
    $('#Label_ID').text($('#TextBox2').val());
    $('#Label_NM').text($('#TextBox3').val());
    $('#Label_DP').text($('#TextBox5').val());
    $('#A06_2Modal').modal('open');
}
function A06_1Button1_Click() {
    let textID = $('#TextID').val().trim();
    if (!textID) {
        $('#LB_CHK').text("-").css('color', 'black');
        $('#A06_1DataGridView1').empty();
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [textID]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A06/CheckUserID";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            $('#A06_1DataGridView1').empty();
            if (data.Success && data.DataGridView && data.DataGridView.length > 0) {
                data.DataGridView.forEach((item, index) => {
                    $('#A06_1DataGridView1').append(`
                        <tr>
                            <td>${item.USER_ID}</td>
                            <td>${item.USER_NM}</td>
                            <td>${item.DT}</td>
                        </tr>
                    `);
                });
                $('#LB_CHK').text("ID in use: NG").css('color', 'red');
            } else {
                $('#LB_CHK').text("Available ID: OK").css('color', 'blue');
            }
            $("#BackGround").css("display", "none");
        }).catch(err => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
}
function A06_1DataGridView1_SelectionChanged(i) {
    if (BaseResult && BaseResult.DataGridView && BaseResult.DataGridView.length > 0) {
        $('#Label1').text(BaseResult.DataGridView[i].USER_ID);
        $('#Label2').text(BaseResult.DataGridView[i].USER_NM);
        $('#Label3').text(BaseResult.DataGridView[i].DT);
    }
}

function A06_1Button3_Click() {
    if ($('#LB_CHK').text() !== "Available ID: OK") {
        alert("Vui lòng chọn User ID hợp lệ.");
        return;
    }
    $('#TextBox2').val($('#TextID').val());
    $('#A06_1Modal').modal('close');
}

function A06_2Button1_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: []
    };
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push($('#TextBox1').val());
    BaseParameter.ListSearchString.push($('#TextBox2').val());
    BaseParameter.ListSearchString.push($('#TextBox3').val());
    BaseParameter.ListSearchString.push($('#TextBox4').val());
    BaseParameter.ListSearchString.push($('#TextBox5').val());
    BaseParameter.ListSearchString.push($('#TextBox6').val());
    BaseParameter.ListSearchString.push($('#ComboBox2').val());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A06/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Success) {
                alert(data.Message);
                $('#SU01').val($('#TextBox2').val());
                Buttonfind_Click();
                $('#TextBox2').prop('readonly', true);
                $('#A06_2Modal').modal('close');
            } else {
                alert(data.Error);
            }
            $("#BackGround").css("display", "none");
        }).catch(err => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
}


function Buttondelete_Click() {
    if ($('#TextBox1').val() === "") {
        alert("Vui lòng chọn người dùng để xóa.");
        return;
    }
    if (confirm("Xóa dữ liệu người dùng đã chọn?")) {
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            ListSearchString: []
        };
        BaseParameter.ListSearchString.push($('#TextBox1').val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/A06/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Success) {
                    alert(data.Message);
                    Buttonfind_Click();
                } else {
                    alert(data.Error);
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
    }
}
function Buttoncancel_Click() {
    // Xóa tất cả ô nhập
    $('#SU01, #SU02, #SU03, #TextBox1, #TextBox2, #TextBox3, #TextBox4, #TextBox5, #TextBox6').val("");
    // Đặt lại ComboBox2 về trạng thái mặc định (nếu cần)
    $("#ComboBox2").prop("selectedIndex", 0);
    // Hủy thao tác bằng cách không làm mới lưới
}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A06/Buttoninport_Click";

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
    $("#BackGround").css("display", "block");
    try {
        // Xuất bảng DataGridView1Table thành Excel
        TableHTMLToExcel("DataGridView1Table", "A06", "A06_" + new Date().toISOString().replace(/[-:T]/g, '').slice(0, 14));
    } catch (err) {
        alert("Lỗi khi xuất Excel: " + err.message);
    }
    $("#BackGround").css("display", "none");
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A06/Buttonprint_Click";

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


