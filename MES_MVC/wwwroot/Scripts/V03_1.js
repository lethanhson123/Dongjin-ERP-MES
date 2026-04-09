let BaseResult;
let DataGridView1RowIndex = -1;

$(document).ready(function () {
    $("#TextID").focus();

    // Lấy tên phần từ sessionStorage (từ V03)
    let partName = sessionStorage.getItem("currentPartName");
    if (partName) {
        $("#Label7").text(partName);
        sessionStorage.removeItem("currentPartName");
    }

    // ⭐ Load đầy đủ thông tin supplier đã lưu
    let savedSupplierJSON = localStorage.getItem("selectedSupplier_V03_1");

    if (savedSupplierJSON) {
        try {
            let savedSupplier = JSON.parse(savedSupplierJSON);
            $("#TextID").val(savedSupplier.CMPNY_NM || "");

            if (partName) {
                Button1_Click(savedSupplier);
            }
        } catch (e) {
            console.error("Error parsing saved supplier:", e);
            localStorage.removeItem("selectedSupplier_V03_1");
        }
    } else if (partName) {
        Button1_Click();
    }
});

$("#Button1").click(function () {
    Button1_Click();
});

$("#Button2").click(function () {
    Button2_Click();
});

$("#Button3").click(function () {
    Button3_Click();
});

$("#TextID").keypress(function (e) {
    if (e.which === 13) {
        Button1_Click();
    }
});

function Button1_Click(savedSupplier = null) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: []
    }
    BaseParameter.ListSearchString.push($("#TextID").val());
    BaseParameter.ListSearchString.push($("#Label7").text());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V03_1/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();

            // ⭐ Tự động chọn dòng theo CMPNY_IDX
            if (savedSupplier && BaseResult.DataGridView1) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    if (BaseResult.DataGridView1[i].CMPNY_IDX === savedSupplier.CMPNY_IDX) {
                        DataGridView1_SelectionChanged(i);
                        break;
                    }
                }
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        })
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Button2_Click() {
    history.back();
}

function Button3_Click() {
    DGV_APPLY();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' ondblclick='DataGridView1_CellDoubleClick()'>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_NM || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PN_NM || "") + "</td>";

                    let dayValue = BaseResult.DataGridView1[i].DAY || 0;
                    let costValue = BaseResult.DataGridView1[i].COST || 0;
                    let bgColor = "";

                    if (dayValue >= 90) {
                        bgColor = "background-color: OrangeRed;";
                    } else if (dayValue >= 60) {
                        bgColor = "background-color: Moccasin;";
                    } else if (dayValue >= 30) {
                        bgColor = "background-color: LightGreen;";
                    }

                    HTML = HTML + "<td style='" + bgColor + "'>" + costValue + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PD_COST_DATE || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_DVS || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_NO || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_ADDR || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_TEL || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_FAX || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_MNGR || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CMPNY_RMK || "") + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;

    // ⭐ Highlight dòng được chọn
    $("#DataGridView1 tr").removeClass("selected");
    $("#DataGridView1 tr").eq(i).addClass("selected");

    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        $("#Label1").text(BaseResult.DataGridView1[i].CMPNY_IDX || "");
        $("#Label2").text(BaseResult.DataGridView1[i].CMPNY_NM || "");
        $("#Label3").text(BaseResult.DataGridView1[i].CMPNY_DVS || "");
        $("#Label9").text(BaseResult.DataGridView1[i].COST || "0");
    }
}

function DataGridView1_CellDoubleClick() {
    DGV_APPLY();
}

function DGV_APPLY() {
    if (DataGridView1RowIndex >= 0 && BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        const selectedRow = BaseResult.DataGridView1[DataGridView1RowIndex];

        // ⭐ Lưu ĐẦY ĐỦ thông tin dưới dạng JSON
        let supplierData = {
            CMPNY_IDX: selectedRow.CMPNY_IDX || "",
            CMPNY_NM: selectedRow.CMPNY_NM || "",
            CMPNY_DVS: selectedRow.CMPNY_DVS || "",
            COST: selectedRow.COST || "0",
            PN_NM: selectedRow.PN_NM || "",
            PD_COST_DATE: selectedRow.PD_COST_DATE || ""
        };

        localStorage.setItem("selectedSupplier_V03_1", JSON.stringify(supplierData));

        try {
            if (window.opener && !window.opener.closed) {
                window.opener.$("#TextBox7").val(selectedRow.CMPNY_IDX || "");
                window.opener.$("#TextBox8").val(selectedRow.CMPNY_NM || "");
                window.opener.$("#TextBox_C1").val(selectedRow.COST || "0");
                window.opener.CAL_COST();
            }
        } catch (e) {
            console.error("Error applying values to parent window:", e);
        }

        window.close();
    } else {
        alert("Please select a supplier!");
    }
}