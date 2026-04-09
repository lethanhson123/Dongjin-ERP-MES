let BaseResult;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DateTimePicker1").val(today);
    $('.modal').modal();
    $("#Label6").text(getCurrentUser());
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
$("#Button3").click(function () {
    Button3_Click();
});
$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
$("#Button4").click(function () {
    Button4_Click();
});
$("#TextBox4").keydown(function (e) {
    if (e.key === 'Enter') {
        TextBox4_KeyDown();
    }
});

$("#searchButton1").click(function () {
    SearchLead_Click();
});

$("#searchTextBox1").keydown(function (e) {
    if (e.key === 'Enter') {
        SearchLead_Click();
    }
});

$("#selectButton").click(function () {
    selectAndClose();
});

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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

function Button1_Click() {
    $("#BackGround").css("display", "block");

    let leadNo = $("#TextBox1").val();
    let quantity = $("#TextBox2").val();
    let numericUpDownValue = $("#NumericUpDown1").val();

    if (!leadNo) {
        M.toast({ html: 'LEAD NO Please Check Again.' });
        $("#BackGround").css("display", "none");
        return;
    }

    if (!quantity || parseInt(quantity) <= 0) {
        M.toast({ html: 'Quantity Please Check Again.' });
        $("#BackGround").css("display", "none");
        return;
    }

    let BaseParameter = {
        ListSearchString: [leadNo, quantity, numericUpDownValue]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Success && BaseResult.Barcodes && BaseResult.Barcodes.length > 0) {
                // Gọi hàm in barcode sau khi nhận được kết quả thành công
                printBarcodeTable(BaseResult.Barcodes);
                M.toast({ html: 'Barcode generated successfully' });
                $("#NumericUpDown1").val(1);
            } else if (BaseResult.Error) {
                M.toast({ html: BaseResult.Error });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            M.toast({ html: 'Error processing request' });
            $("#BackGround").css("display", "none");
        })
    });
}

function Button2_Click() {
    $("#TextBox1").val("");
    $("#TextBox2").val("");
    $("#TextBox3").val("");
    $("#TextBox5").val("");
    $("#NumericUpDown1").val(1);

    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DateTimePicker1").val(today);

    $("#Label37").text("0");
    $("#TextBox4").focus();

    if (document.getElementById("qrcode")) {
        document.getElementById("qrcode").innerHTML = "";
    }
}

function Button3_Click() {
    $("#searchTextBox1").val('');
    $("#selectedLabel2").val('');
    $("#selectedLabel3").val('');
    $("#selectedLabel4").val('');
    $("#searchDataGrid tbody").empty();
    $("#searchModal").modal('open');
    setTimeout(function () {
        $("#searchTextBox1").focus();
    }, 300);
}

function Button4_Click() {
    $("#BackGround").css("display", "block");

    let leadNo = $("#TextBox1").val();
    let quantity = $("#TextBox2").val();
    let numericUpDownValue = $("#NumericUpDown1").val();

    if (!leadNo) {
        M.toast({ html: 'LEAD NO Please Check Again.' });
        $("#BackGround").css("display", "none");
        return;
    }

    if (!quantity || parseInt(quantity) <= 0) {
        M.toast({ html: 'Quantity Please Check Again.' });
        $("#BackGround").css("display", "none");
        return;
    }

    let BaseParameter = {
        ListSearchString: [leadNo, quantity, numericUpDownValue]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/Button4_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Success && BaseResult.Barcodes && BaseResult.Barcodes.length > 0) {
                // Gọi hàm in barcode only sau khi nhận được kết quả thành công
                printBarcodeOnly(BaseResult.Barcodes);
                M.toast({ html: 'Barcode only generated successfully' });
                $("#NumericUpDown1").val(1);
            } else if (BaseResult.Error) {
                M.toast({ html: BaseResult.Error });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            M.toast({ html: 'Error processing request' });
            $("#BackGround").css("display", "none");
        })
    });
}

function TextBox4_KeyDown() {
    $("#BackGround").css("display", "block");

    let textBox4Value = $("#TextBox4").val().trim();
    let label37Value = $("#Label37").text() || "0";

    if (!textBox4Value) {
        $("#BackGround").css("display", "none");
        return;
    }

    let BaseParameter = {
        TextBox4: textBox4Value,
        Label37: label37Value
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/TextBox4_KeyDown";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Success) {
                if (BaseResult.Label37) {
                    $("#Label37").text(BaseResult.Label37);
                }
                $("#TextBox4").val("").focus();

                if (BaseResult.AudioCue === "success" && $("#successSound").length > 0) {
                    $("#successSound")[0].play().catch(e => { });
                }
            } else if (BaseResult.Error) {
                M.toast({ html: BaseResult.Error });

                if (BaseResult.AudioCue === "error" && $("#errorSound").length > 0) {
                    $("#errorSound")[0].play().catch(e => { });
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            M.toast({ html: 'Error processing request' });
            $("#BackGround").css("display", "none");
        })
    });
}

function SearchLead_Click() {
    var searchText = $("#searchTextBox1").val();

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: ["", searchText]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/SearchLead";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            SearchDataGridRender();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function SearchDataGridRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML += "<tr>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].LEAD_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML += "<td>" + (BaseResult.DataGridView1[i].HOOK_RACK || '') + "</td>";
                    HTML += "</tr>";
                }
            }
        }
    }
    document.getElementById("searchDataGrid").getElementsByTagName("tbody")[0].innerHTML = HTML;

    $("#searchDataGrid tbody tr").click(function () {
        $("#searchDataGrid tbody tr").removeClass("selected");
        $(this).addClass("selected");

        var leadPN = $(this).find("td:eq(0)").text();
        var bundleSize = $(this).find("td:eq(1)").text();
        var hookRack = $(this).find("td:eq(2)").text();

        $("#selectedLabel2").val(leadPN);
        $("#selectedLabel3").val(bundleSize);
        $("#selectedLabel4").val(hookRack);

        M.updateTextFields();
    });

    $("#searchDataGrid tbody tr").dblclick(function () {
        selectAndClose();
    });
}

function selectAndClose() {
    var leadPN = $("#selectedLabel2").val();
    var bundleSize = $("#selectedLabel3").val();
    var hookRack = $("#selectedLabel4").val();

    if (leadPN) {
        $("#TextBox1").val(leadPN);
        $("#TextBox2").val(bundleSize);
        $("#TextBox3").val(hookRack);

        $("#searchModal").modal('close');
        generateQRCode(leadPN);
    } else {
        M.toast({ html: 'Please select a lead record!' });
    }
}

function generateQRCode(text) {
    if (document.getElementById("qrcode")) {
        document.getElementById("qrcode").innerHTML = "";
    }

    new QRCode(document.getElementById("qrcode"), {
        text: text,
        width: 135,
        height: 111,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.L
    });
}

function printBarcodeTable(barcodes) {
    if (!barcodes || barcodes.length === 0) return;

    let BaseParameter = {
        ListSearchString: [],
        Barcodes: barcodes
    };

    BaseParameter.ListSearchString.push(barcodes[0]);
    BaseParameter.ListSearchString.push($("#TextBox5").val()); // Model (TextBox5)
    BaseParameter.ListSearchString.push($("#DateTimePicker1").val());
    BaseParameter.ListSearchString.push(getCurrentUser());
    BaseParameter.ListSearchString.push($("#TextBox3").val());
    BaseParameter.ListSearchString.push($("#TextBox1").val());
    BaseParameter.ListSearchString.push($("#TextBox2").val());
    BaseParameter.TimeS = BaseResult.TimeS;
    localStorage.setItem("C08HTML", "1");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/PrintBarcode";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            let printResult = data;
            if (printResult && printResult.Code) {
                OpenWindowByURL(printResult.Code, 600, 400);
            } else if (printResult.Error) {
                M.toast({ html: printResult.Error });
            }
            $("#NumericUpDown1").val(1);
        }).catch((err) => {
            M.toast({ html: 'Error generating print view' });
        });
    }).catch((err) => {
        M.toast({ html: 'Network error' });
    });
}

function printBarcodeOnly(barcodes) {
    if (!barcodes || barcodes.length === 0) return;

    let BaseParameter = {
        ListSearchString: [],
        Barcodes: barcodes
    };

    BaseParameter.ListSearchString.push(barcodes[0]);
    BaseParameter.ListSearchString.push($("#TextBox1").val());
    BaseParameter.ListSearchString.push($("#TextBox2").val());
    localStorage.setItem("C08HTML", "1");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C08/PrintBarcodeOnly";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            let printResult = data;
            console.log("Print response:", printResult);

            if (printResult && printResult.Code) {
                console.log("Opening URL:", printResult.Code);
                OpenWindowByURL(printResult.Code, 300, 200);
            } else if (printResult.Error) {
                M.toast({ html: printResult.Error });
            }

            $("#NumericUpDown1").val(1);
        }).catch((err) => {
            console.error("Error parsing response:", err);
            M.toast({ html: 'Error generating print view' });
        });
    }).catch((err) => {
        console.error("Fetch error:", err);
        M.toast({ html: 'Network error' });
    });
}



$(window).focus(function () {
}).blur(function () {
    let C08HTML = localStorage.getItem("C08HTML");
    if (C08HTML == "0") {
        window.close();
    }
});
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}