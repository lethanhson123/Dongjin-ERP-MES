// C19.js

let BaseResult;
let IsTableSort = false;
let TagIndex = 1;

$(document).ready(function () {
    $("#ComboBox1").val("ALL");

    // Khởi tạo sự kiện click cho các tab
    $("#TabPage1").click(function () {
        TagIndex = 1;
    });

    $("#TabPage5").click(function () {
        TagIndex = 5;
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
$("#Button4").click(function () {
    Button4_Click();
});
function Button4_Click() {
    if (TagIndex === 5) {

        window.open('/Admin6User', 'Admin6User', 'width=1000,height=600,resizable=yes');
    }
}

$("#TextBox5").keydown(function (e) {
    if (e.keyCode === 13) {
        Buttonfind_Click();
    }
});

$("#TextBox19, #TextBox11, #TextBox16, #TextBox27, #TextBox29").keypress(function (e) {
    if (!isDigit(e.key) && !isControl(e.key)) {
        return false;
    }
});

$("#RadioButton6, #RadioButton7").click(function () {
    RadioButton_Click();
});

$("#Button5").click(function () {
    Button5_Click();
});

$("#Button6").click(function () {
    Button6_Click();
});

$("#Button3").click(function () {
    Button3_Click();
});

$("#TextBox8, #TextBox9, #TextBox10, #TextBox14, #TextBox15").blur(function () {
    const tspart_IDX = $(this).val();
    tspart_LOAD(tspart_IDX, $(this));
});

$("#TextBox30").keydown(function (e) {
    if (e.keyCode === 13) {
        TextBox30_KeyDown();
    }
});

// Xử lý sự kiện khi click vào hàng trong DataGridView6
$(document).on("click", "#Tab5_LeadListBody tr", function () {
    $(this).addClass("selected").siblings().removeClass("selected");
    DGV_LOAD();
});
// Xử lý sự kiện khi click vào hàng trong Tab5_SubPartBody
$(document).on("click", "#Tab5_SubPartBody tr", function () {
    $(this).addClass("selected").siblings().removeClass("selected");
});
// Xử lý khi giá trị trong DataGridView7 thay đổi
$(document).on("change", "#DataGridView7 td", function () {
    let row = $(this).closest("tr");
    let firstCell = row.find("td:first");

    if (firstCell.text() !== "NEW" && firstCell.text() !== "DEL") {
        firstCell.text("EDIT");
    }
});
$(document).on("change", "#Tab5_SubPartBody select", function () {
    let row = $(this).closest("tr");
    let firstCell = row.find("td:first");

    if (firstCell.text() !== "NEW" && firstCell.text() !== "DEL") {
        firstCell.text("EDIT");
    }
});

function Buttonfind_Click() {
    if (TagIndex === 1) {
        // Code cho TabPage1 (giữ nguyên)
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            Action: 1,
            ListSearchString: [
                $("#TextBox1").val() || "",
                $("#ComboBox1").val() || "ALL",
                $("#TextBox4").val() || "",
                $("#TextBox2").val() || "",
                $("#TextBox3").val() || ""
            ]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                BaseResult = { DataGridView1: data.DataGridView1 };
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert(localStorage.getItem("ERROR") || "An error occurred.");
                $("#BackGround").css("display", "none");
            });
    } else if (TagIndex === 5) {
        // Action 5 cho MAG_BACODE (TabPage5) - sử dụng chung với TabPage1
        $("#BackGround").css("display", "block");

        let BaseParameter = {
            Action: 5,
            ListSearchString: [
                $("#TextBox5").val() || ""
            ]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/Buttonfind_Click"; // Dùng chung endpoint

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Success || data.DataGridView6) {
                    DataGridView6Render(data.DataGridView6);

                    // Xóa dữ liệu chi tiết
                    $("#TextBox6").val("");
                    $("#Label36").text("");
                    $("#TextBox7").val("");
                    $("#TextBox19").val("");
                    $("#TextBox8").val("");
                    $("#TextBox9").val("");
                    $("#TextBox10").val("");
                    $("#TextBox11").val("");
                    $("#TextBox12").val("");
                    $("#TextBox13").val("");
                    $("#TextBox14").val("");
                    $("#TextBox15").val("");
                    $("#TextBox16").val("");
                    $("#TextBox17").val("");
                    $("#TextBox18").val("");
                    $("#TextBox22").val("");
                    $("#TextBox23").val("");
                    $("#TextBox24").val("");
                    $("#TextBox25").val("");
                    $("#TextBox26").val("");
                    $("#TextBox27").val("");
                    $("#TextBox20").val("");
                    $("#TextBox21").val("");
                    $("#TextBox29").val("");

                    $("#TextBox6").prop("readonly", true);
                    $("#Panel7").prop("disabled", false);
                } else {
                    alert(data.Error || "Không thể tải danh sách LEAD");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                console.error("Error in MAG_BACODE:", err);
                alert("Lỗi kết nối: " + err.message);
                $("#BackGround").css("display", "none");
            });
    }
}

function DataGridView6Render(data) {
    let html = "";
    if (data && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            html += "<tr>";
            html += "<td>" + (data[i].LEAD_PN || "") + "</td>";
            html += "</tr>";
        }
    } else {
        html = "<tr><td>Không tìm thấy dữ liệu</td></tr>";
    }

    $("#Tab5_LeadListBody").html(html);
}

function DGV_LOAD() {
    try {
        let selectedRow = $("#Tab5_LeadListBody tr.selected");
        if (selectedRow.length === 0) return;

        let LEAD_NO6 = selectedRow.find("td:first").text();

        $("#BackGround").css("display", "block");

        let BaseParameter = {
            ListSearchString: [LEAD_NO6]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/DGV_LOAD"; // Sử dụng endpoint trực tiếp

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {

                if (data.Success) {
                    // Điền dữ liệu vào form
                    $("#TextBox6").val(data.TextBox6 || "");

                    if (data.DSCN === "Y") {
                        $("#RadioButton4").prop("checked", true);
                    } else {
                        $("#RadioButton5").prop("checked", true);
                    }

                    $("#Label36").text(data.LEAD_INDEX || "");
                    $("#TextBox7").val(data.TextBox7 || "");
                    $("#TextBox19").val(data.TextBox19 || "");
                    $("#TextBox8").val(data.TextBox8 || "");
                    $("#TextBox9").val(data.TextBox9 || "");
                    $("#TextBox10").val(data.TextBox10 || "");
                    $("#TextBox11").val(data.TextBox11 || "");
                    $("#TextBox12").val(data.TextBox12 || "");
                    $("#TextBox13").val(data.TextBox13 || "");
                    $("#TextBox14").val(data.TextBox14 || "");
                    $("#TextBox15").val(data.TextBox15 || "");
                    $("#TextBox16").val(data.TextBox16 || "");
                    $("#TextBox17").val(data.TextBox17 || "");
                    $("#TextBox18").val(data.TextBox18 || "");
                    $("#TextBox22").val(data.TextBox22 || "");
                    $("#TextBox23").val(data.TextBox23 || "");
                    $("#TextBox24").val(data.TextBox24 || "");
                    $("#TextBox25").val(data.TextBox25 || "");
                    $("#TextBox26").val(data.TextBox26 || "");
                    $("#TextBox27").val(data.TextBox27 || "");
                    $("#TextBox20").val(data.TextBox20 || "");
                    $("#TextBox21").val(data.TextBox21 || "");
                    $("#TextBox29").val(data.TextBox29 || "");

                    if (data.LEAD_SCN === "LEAD") {
                        $("#RadioButton6").prop("checked", true);
                        $("#Panel7").prop("disabled", true);
                    } else {
                        $("#RadioButton7").prop("checked", true);
                        $("#Panel7").prop("disabled", false);
                    }

                    // Hiển thị danh sách subparts
                    DataGridView7Render(data.DataGridView7);

                    // Đặt trạng thái readonly cho TextBox6
                    $("#TextBox6").prop("readonly", true);
                } else {
                    alert(data.Error || "Không thể tải chi tiết LEAD");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert("Lỗi kết nối: " + err.message);
                $("#BackGround").css("display", "none");
            });
    } catch (ex) {
        $("#BackGround").css("display", "none");
    }
}


function DataGridView7Render(data) {
    // Xóa bảng hiện tại để chuẩn bị thêm dữ liệu mới
    $("#Tab5_SubPartBody").empty();

    if (data && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            let row = $("<tr>");
            row.append($("<td>").addClass("center-header-text").text(data[i].NO || ""));
            row.append($("<td>").addClass("center-header-text").text(data[i].PARTNO || data[i].LEAD_NO || ""));

            // Create dropdown for LH/RH column
            let currentValue = data[i].PART_CODE || data[i].S_LR || "L";
            let selectHtml = '<select class="browser-default">' +
                '<option value="L" ' + (currentValue === "L" ? 'selected' : '') + '>L</option>' +
                '<option value="R" ' + (currentValue === "R" ? 'selected' : '') + '>R</option>' +
                '</select>';

            row.append($("<td>").addClass("center-header-text").html(selectHtml));
            $("#Tab5_SubPartBody").append(row);
        }
    } else {
        let row = $("<tr>");
        row.append($("<td colspan='3' class='text-center'>").text("Không có dữ liệu"));
        $("#Tab5_SubPartBody").append(row);
    }
}

function Buttonadd_Click() {
    if (TagIndex === 5) {
        $("#TextBox6").val("");
        $("#Label36").text("");
        $("#TextBox7").val("");
        $("#TextBox19").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        $("#TextBox10").val("");
        $("#TextBox11").val("");
        $("#TextBox12").val("");
        $("#TextBox13").val("");
        $("#TextBox14").val("");
        $("#TextBox15").val("");
        $("#TextBox16").val("");
        $("#TextBox17").val("");
        $("#TextBox18").val("");
        $("#TextBox22").val("");
        $("#TextBox23").val("");
        $("#TextBox24").val("");
        $("#TextBox25").val("");
        $("#TextBox26").val("");
        $("#TextBox27").val("");
        $("#TextBox20").val("");
        $("#TextBox21").val("");
        $("#TextBox29").val("");
        $("#Label36").text("NEW");

        $("#RadioButton6").prop("checked", true);

        $("#TextBox6").prop("readonly", false);
    }
}

function Buttoncancel_Click() {
    if (TagIndex === 5) {
        $("#TextBox6").val("");
        $("#Label36").text("");
        $("#TextBox7").val("");
        $("#TextBox19").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        $("#TextBox10").val("");
        $("#TextBox11").val("");
        $("#TextBox12").val("");
        $("#TextBox13").val("");
        $("#TextBox14").val("");
        $("#TextBox15").val("");
        $("#TextBox16").val("");
        $("#TextBox17").val("");
        $("#TextBox18").val("");
        $("#TextBox22").val("");
        $("#TextBox23").val("");
        $("#TextBox24").val("");
        $("#TextBox25").val("");
        $("#TextBox26").val("");
        $("#TextBox27").val("");
        $("#TextBox20").val("");
        $("#TextBox21").val("");
        $("#TextBox29").val("");

        $("#TextBox6").prop("readonly", true);
    }
}

function Buttonsave_Click() {
    if (TagIndex === 5) {
        if ($("#TextBox6").val() === "") {
            alert("LEAD NO: There is no value entered. Please Check Again.");
            return;
        }

        if ($("#TextBox7").val() === "") {
            alert("Hook rack location: There is no value entered. Please Check Again.");
            return;
        }

        if ($("#TextBox19").val() === "") {
            alert("Safety stock: There is no value entered. Please Check Again.");
            return;
        }

        $("#BackGround").css("display", "block");

        let DS_YN = $("#RadioButton5").prop("checked") ? "N" : "Y";
        let LEAD_YN = $("#RadioButton6").prop("checked") ? "LEAD" : "SPST";

        let BaseParameter = {
            ListSearchString: [
                $("#TextBox6").val().trim(),  
                $("#TextBox7").val().trim(),
                $("#TextBox19").val().trim(),
                $("#TextBox8").val().trim(),
                $("#TextBox9").val().trim(),
                $("#TextBox10").val().trim(),
                $("#TextBox11").val().trim() || "0",
                $("#TextBox12").val().trim(),
                $("#TextBox13").val().trim(),
                $("#TextBox14").val().trim(),
                $("#TextBox15").val().trim(),
                $("#TextBox16").val().trim() || "0",
                $("#TextBox17").val().trim(),
                $("#TextBox18").val().trim(),
                $("#TextBox22").val().trim(),
                $("#TextBox23").val().trim(),
                $("#TextBox24").val().trim(),
                $("#TextBox25").val().trim(),
                $("#TextBox26").val().trim(),
                $("#TextBox27").val().trim() || "0",
                $("#TextBox20").val().trim(),
                $("#TextBox21").val().trim(),
                $("#TextBox29").val().trim() || "0",
                DS_YN,
                LEAD_YN
            ]
        };
        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/SaveLead"; // Sử dụng endpoint trực tiếp

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Success) {
                    alert("Save Data Completed.");
                    $("#TextBox6").prop("readonly", true);
                } else {
                    alert(data.Error || "Unable to connect to database.");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert("Error: " + err.message);
                $("#BackGround").css("display", "none");
            });
    }
}


function Button5_Click() {
    try {
        if ($("#TextBox28").val() === "") return;

        $("#BackGround").css("display", "block");

        let BaseParameter = {
            Action: 10,
            ListSearchString: [
                $("#TextBox28").val()
            ]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Success) {
                    if (data.DataGridView7 && data.DataGridView7.length > 0) {
                        let newRow = $("<tr>");
                        newRow.append($("<td>").text("NEW"));
                        newRow.append($("<td>").text($("#TextBox28").val()));

                        // Add dropdown with L selected by default
                        let selectHtml = '<select class="browser-default">' +
                            '<option value="L" selected>L</option>' +
                            '<option value="R">R</option>' +
                            '</select>';

                        newRow.append($("<td>").html(selectHtml));
                        $("#Tab5_SubPartBody").append(newRow);
                    } else {
                        alert("MES LEAD không tồn tại");
                    }
                } else {
                    alert(data.Error || "Không thể thêm LEAD");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert("Lỗi: " + err.message);
                $("#BackGround").css("display", "none");
            });
    } catch (ex) {
        $("#BackGround").css("display", "none");
    }
}

function Button6_Click() {
    try {
        let selectedRow = $("#Tab5_SubPartBody tr.selected");
        if (selectedRow.length === 0) return;

        selectedRow.find("td:first").text("DEL");
    } catch (ex) {
        console.error("Error in Button6_Click:", ex);
    }
}

function Button3_Click() {
    let rows = $("#Tab5_SubPartBody tr");
    if (rows.length <= 0) return;

    $("#BackGround").css("display", "block");

    let subPartItems = [];
    rows.each(function () {
        let item = {
            Coln1: $(this).find("td:eq(0)").text(),
            Coln2: $(this).find("td:eq(1)").text(),
            Coln3: $(this).find("td:eq(2) select").val() // Get value from dropdown
        };
        subPartItems.push(item);
    });

    console.log("Saving SubParts:", subPartItems);
    let BaseParameter = {
        ListSearchString: [
            $("#TextBox6").val()
        ],
        SubPartItems: subPartItems
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/C19/SaveSubPart";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            console.log("SaveSubPart response:", data);
            if (data.Success) {
                alert("Đã được lưu.");
                DGV_LOAD();
            } else {
                alert(data.Error || "Không thể lưu danh sách subpart");
            }
            $("#BackGround").css("display", "none");
        }).catch(err => {
            console.error("Error in SaveSubPart:", err);
            alert("Lỗi: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function RadioButton_Click() {
    if ($("#RadioButton6").prop("checked")) {
        $("#Panel7").prop("disabled", true);
    }

    if ($("#RadioButton7").prop("checked")) {
        $("#Panel7").prop("disabled", false);
    }
}

function tspart_LOAD(tspart_IDX, textBox) {
    let BaseParameter = {
        ListSearchString: [tspart_IDX]
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/C19/CheckPart"; // Sử dụng endpoint trực tiếp

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (!data.Success || data.Message !== "true") {
                alert("MES에 자재 품번이 없습니다.\nKhông có dữ liệu. MES Material PART NO.");
                textBox.val("");
            }
        }).catch(err => {
            alert("Lỗi: " + err.message);
        });
}

function Button5_Click() {
    try {
        if ($("#TextBox28").val() === "") return;

        $("#BackGround").css("display", "block");

        let BaseParameter = {
            ListSearchString: [
                $("#TextBox28").val()
            ]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/AddSubPart"; // Sử dụng endpoint trực tiếp

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Success) {
                    if (data.DataGridView7 && data.DataGridView7.length > 0) {
                        let newRow = $("<tr>");
                        newRow.append($("<td>").text("NEW"));
                        newRow.append($("<td>").text($("#TextBox28").val()));
                        newRow.append($("<td>").text("L"));
                        $("#DataGridView7").append(newRow);
                    } else {
                        alert("MES LEAD không tồn tại");
                    }
                } else {
                    alert(data.Error || "Không thể thêm LEAD");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert("Lỗi: " + err.message);
                $("#BackGround").css("display", "none");
            });
    } catch (ex) {
        $("#BackGround").css("display", "none");
    }
}

function isDigit(key) {
    return /^\d$/.test(key);
}

function isControl(key) {
    return key === "Backspace" || key === "Delete" || key === "ArrowLeft" || key === "ArrowRight" || key === "Tab";
}

function TextBox30_KeyDown() {
    try {
        let AAAA = $("#TextBox30").val();
        $("#TextBox30").val("");

        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //A
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //B
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //C

        let firstPart = AAAA.trim().substring(0, AAAA.indexOf("\t") - 1);

        if (firstPart === "LEAD") {
            $("#RadioButton6").prop("checked", true);
        }
        if (firstPart === "SPST") {
            $("#RadioButton7").prop("checked", true);
        }

        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //D LEAD_SCN

        if (!$("#TextBox6").prop("readonly")) {
            $("#TextBox6").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //LEAD
        }

        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //E LEAD

        $("#TextBox29").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //번들 사이즈
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //F BUNDEL_SIZE

        $("#TextBox8").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //WIRE 메인
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //G W.PART.NO

        $("#TextBox9").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //T1
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //H TERM1

        $("#TextBox10").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //S1
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //I SEAL1

        $("#TextBox14").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //T2
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //J TERM2

        $("#TextBox15").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //S2
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //K SELA2

        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //L
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //M
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //N
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //O
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //P

        $("#TextBox11").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //STRIP1
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //Q STRIP1

        $("#TextBox16").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //STRIP2
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //R STRIP2

        $("#TextBox12").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //CCH_1
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //S CCH_1

        $("#TextBox13").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //ICH_1
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //T ICH_1

        $("#TextBox17").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //CCH_2
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //U CCH_2

        $("#TextBox18").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //ICH_2
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //V ICH_2

        $("#TextBox20").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //T1.NO
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //W T1.NO

        $("#TextBox21").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //T2.NO
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //X T2.NO

        $("#TextBox22").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //WIRE/LINK
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //Y W/LINK

        $("#TextBox23").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //WIRE R / NO
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //Z W.R/NO

        $("#TextBox24").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //WIRE
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //AA WIRE

        $("#TextBox25").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //Diameter
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //AB Diameter

        $("#TextBox26").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //Color
        AAAA = AAAA.trim().substring(AAAA.indexOf("\t") + 1);   //AC Color

        $("#TextBox27").val(AAAA.trim().substring(0, AAAA.indexOf("\t") - 1));  //Length
    } catch (ex) {
        $("#TextBox6").val("");
        $("#TextBox7").val("");
        $("#TextBox19").val("");
        $("#TextBox29").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        $("#TextBox10").val("");
        $("#TextBox11").val("");
        $("#TextBox20").val("");
        $("#TextBox12").val("");
        $("#TextBox13").val("");
        $("#TextBox14").val("");
        $("#TextBox15").val("");
        $("#TextBox16").val("");
        $("#TextBox21").val("");
        $("#TextBox17").val("");
        $("#TextBox18").val("");
        $("#TextBox22").val("");
        $("#TextBox23").val("");
        $("#TextBox24").val("");
        $("#TextBox25").val("");
        $("#TextBox26").val("");
        $("#TextBox27").val("");
    }
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {

                    HTML += "<tr>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].LEAD_INDEX + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].LEAD_SCN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].LEAD_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].W_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].T1_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].S1_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].T2_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].S2_PN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].STRIP1 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].STRIP2 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].T1NO + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].T2NO + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].WR_NO + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].WIRE_NM + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].W_Diameter + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].W_LINK + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].W_Color + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].W_Length + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].DSCN_YN + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].UPDATE_DTM + "</td>";
                    HTML += "<td>" + BaseResult.DataGridView1[i].UPDATE_USER + "</td>";
                    HTML += "</tr>";
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

function DataGridViewSort(dataArray, ascending) {
    if (!dataArray || dataArray.length === 0) return;

    dataArray.sort(function (a, b) {
        let valueA = a.LEAD_INDEX;
        let valueB = b.LEAD_INDEX;

        if (valueA < valueB) {
            return ascending ? -1 : 1;
        }
        if (valueA > valueB) {
            return ascending ? 1 : -1;
        }
        return 0;
    });
}

function Buttoninport_Click() {
    if (TagIndex === 5) {
      
        window.open('/C19_1', 'C19_1', 'width=1000,height=600,resizable=yes');
    }
}


function Buttonexport_Click() {
    if (TagIndex === 1) {
        let dgvcount = $("#DataGridView1 tr").length;
        if (dgvcount <= 0) {
            return;
        }

        $("#BackGround").css("display", "block");

        let D1 = new Date().toISOString().split('T')[0].replace(/-/g, '');
        let fileName = "LEAD_NO_" + D1 + ".xls";

        // Sử dụng BaseParameter từ Buttonfind_Click để lấy lại dữ liệu
        let BaseParameter = {
            Action: 1,
            ListSearchString: [
                $("#TextBox1").val() || "",
                $("#ComboBox1").val() || "ALL",
                $("#TextBox4").val() || "",
                $("#TextBox2").val() || "",
                $("#TextBox3").val() || ""
            ]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/C19/Buttonexport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                if (data.Code) {
                    window.open(data.Code, '_blank');
                } else {
                    alert(data.Error || "Excel open failure. Please Check Again.");
                }
                $("#BackGround").css("display", "none");
            }).catch(err => {
                alert("Error: " + err.message);
                $("#BackGround").css("display", "none");
            });
    }
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", "width=" + width + ",height=" + height + ",toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no");
}

// Xử lý các sự kiện khác
$("#TextBox1, #TextBox2, #TextBox3, #TextBox4").keydown(function (e) {
    if (e.keyCode === 13) {
        Buttonfind_Click();
    }
});

// Khởi tạo trạng thái mặc định khi trang tải xong
$(document).ready(function () {
    // Các xử lý ban đầu khác (nếu cần)

    // Kiểm tra xem URL có chứa fragment identifier không
    if (window.location.hash === "#TabPage5") {
        TagIndex = 5;
        $("#TabPage5").click();
    }
});