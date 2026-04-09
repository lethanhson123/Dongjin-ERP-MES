let BaseResult;

$(document).ready(function () {
    var now = new Date();
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var firstDay = now.getFullYear() + "-" + month + "-01";
    var lastDay = now.getFullYear() + "-" + month + "-" + new Date(now.getFullYear(), now.getMonth() + 1, 0).getDate();
    $("#DateTimePicker1").val(firstDay);
    $("#DateTimePicker2").val(lastDay);

    $("#TexT1").keydown(function (e) {
        if (e.keyCode == 13) {
            Buttonfind_Click();
        }
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

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [
            $("#TexT1").val(),
            $("#DateTimePicker1").val(),
            $("#DateTimePicker2").val()
        ]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A03/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (data.Error) {
                alert("Error: " + data.Error);
            } else {
                RenderDataGridView1();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("Error: " + err.message);
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonadd_Click() {
    let today = new Date().toISOString().split('T')[0];
    $("#TextBox1").val("0");
    $("#TextBox2").val(today);
    $("#TextBox3").val("");
    $("#TextBox4").val("");
}

function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [
            $("#TextBox1").val(),
            $("#TextBox3").val(),
            $("#TextBox4").val(),
            $("#TextBox2").val()
        ],
        USER_IDX: GetCookieValue("USER_IDX")
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A03/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Error: " + data.Error);
            } else {
                alert("Saving completed.");
                Buttonfind_Click();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("Error: " + err.message);
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttondelete_Click() {
    let noticeIdx = $("#TextBox1").val();
    if (noticeIdx === "0" || !noticeIdx) {
        Buttoncancel_Click();
        return;
    }
    if (confirm("Delete the selected Notice data?")) {
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            ListSearchString: [noticeIdx]
        };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/A03/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then((response) => {
            response.json().then((data) => {
                if (data.Error) {
                    alert("Error: " + data.Error);
                } else {
                    alert("Done. The selected Notice data.");
                    Buttonfind_Click();
                    Buttoncancel_Click();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert("Error: " + err.message);
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Buttoncancel_Click() {
    $("#TextBox1").val("");
    $("#TextBox2").val("");
    $("#TextBox3").val("");
    $("#TextBox4").val("");
}

function Buttoninport_Click() {
    alert("Excel import not implemented yet.");
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [
            $("#TexT1").val(),
            $("#DateTimePicker1").val(),
            $("#DateTimePicker2").val()
        ]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A03/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Error: " + data.Error);
            } else {
                window.location.href = data.Code;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("Error: " + err.message);
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [
            $("#TexT1").val(),
            $("#DateTimePicker1").val(),
            $("#DateTimePicker2").val()
        ]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/A03/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Error: " + data.Error);
            } else {
                window.open(data.Code, "_blank");
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("Error: " + err.message);
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

function RenderDataGridView1() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        DataGridView1_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView1[i].NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].Title + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView1[i].DATE ? new Date(BaseResult.DataGridView1[i].DATE).toISOString().split('T')[0] : "") + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].Contents + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        $("#TextBox1").val(BaseResult.DataGridView1[i].NO);
        $("#TextBox2").val(BaseResult.DataGridView1[i].DATE ? new Date(BaseResult.DataGridView1[i].DATE).toISOString().split('T')[0] : "");
        $("#TextBox3").val(BaseResult.DataGridView1[i].Title);
        $("#TextBox4").val(BaseResult.DataGridView1[i].Contents);
    }
}