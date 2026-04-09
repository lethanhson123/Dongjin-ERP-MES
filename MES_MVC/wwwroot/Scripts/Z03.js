let BaseResult = {};
let DataGridView1RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + month + "-" + day;
    $("#DateTimePicker1").val(today);
    $("#DateTimePicker4").val(today);
    COMLIST_LINE();
    updateCurrentTime();
});
function updateCurrentTime() {
    var now = new Date();
    var hours = ("0" + now.getHours()).slice(-2);
    var minutes = ("0" + now.getMinutes()).slice(-2);
    var currentTime = hours + ":" + minutes;

  
    $("#DateTimePicker2").val(currentTime);
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

$("#Buttonimport").click(function () {
    Buttonimport_Click();
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
    let BaseParameter = {
        ListSearchString: [$("#DateTimePicker4").val()]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z03/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DataGridView1 = data.DataGridView1;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonadd_Click() {
    $("#ComboBox1").val("");
    $("#ComboBox2").val("");
    $("#DateTimePicker1").val(new Date().toISOString().slice(0, 10));
    $("#DateTimePicker2").val("08:00");
    $("#NumericUpDown1").val("0");
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [
            $("#ComboBox1").val(),
            $("#ComboBox2").val(),
            $("#DateTimePicker1").val(),
            $("#DateTimePicker2").val(),
            $("#NumericUpDown1").val()
        ],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z03/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("Lưu thành công!");
                Buttonfind_Click();
            } else {
                alert(data.Error || "Lỗi khi lưu!");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttondelete_Click() {
    if (DataGridView1RowIndex >= 0 && BaseResult.DataGridView1?.length > 0) {
        if (confirm("Xác nhận xóa?")) {
            $("#BackGround").css("display", "block");
            let BaseParameter = {
                SearchString: BaseResult.DataGridView1[DataGridView1RowIndex].CODE
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/Z03/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload
            }).then((response) => {
                response.json().then((data) => {
                    if (data.Success) {
                        alert("Xóa thành công!");
                        Buttonfind_Click();
                    } else {
                        alert(data.Error || "Lỗi khi xóa!");
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                });
            });
        }
    }
}

function Buttoncancel_Click() {
    Buttonadd_Click();
}

function Buttonimport_Click() { }

function Buttonexport_Click() { }

function Buttonprint_Click() { }

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function COMLIST_LINE() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z03/Load";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.ComboBox1 = data.ComboBox1;
            ComboBox1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function ComboBox1Render() {
    $("#ComboBox1").empty();
    $("#ComboBox1").append('<option value="">Select...</option>');
    if (BaseResult?.ComboBox1?.length > 0) {
        BaseResult.ComboBox1.forEach(item => {
            $("#ComboBox1").append(`<option value="${item.Value}">${item.COMB_CODE}</option>`);
        });
    }
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult?.DataGridView1?.length > 0) {
        BaseResult.DataGridView1.forEach((item, i) => {
            HTML += `<tr onclick='DataGridView1_SelectionChanged(${i})'>`;
            HTML += `<td>${item.CODE}</td>`;
            HTML += `<td>${item.DATE}</td>`;
            HTML += `<td>${item.MC_NAME}</td>`;
            HTML += `<td>${item.SHIFT}</td>`;
            HTML += `<td>${item.START_TIME}</td>`;
            HTML += `<td>${item.END_TIME}</td>`;
            HTML += `<td>${item.WK_TIME_H}</td>`;
            HTML += `<td>${item.WK_TIME}</td>`;
            HTML += `</tr>`;
        });
    }
    $("#DataGridView1").html(HTML);
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    if (BaseResult?.DataGridView1?.length > i) {
        let row = BaseResult.DataGridView1[i];
        $("#ComboBox1").val(row.MC_NAME);
        $("#ComboBox2").val(row.SHIFT);
        $("#DateTimePicker1").val(row.DATE);
        $("#DateTimePicker2").val(row.START_TIME.split(" ")[1]);
        $("#NumericUpDown1").val(row.WK_TIME_H);
    }
}

function GetCookieValue(name) {
    let value = "; " + document.cookie;
    let parts = value.split("; " + name + "=");
    return parts.length == 2 ? parts.pop().split(";").shift() : "";
}