let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let FNM;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let T2_DGV1RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    document.getElementById("DateTimePicker1").readOnly = true;
    $("#DateTimePicker1").val(today);
    $("#T2_S2").val(today);
    FNM = localStorage.getItem("FNM_C13");
    if (FNM == null) {
        FNM = 0;
    }
    if (FNM == 0) {
        document.getElementById("RadioButton1").checked = true;
    }
    else {
        document.getElementById("RadioButton2").checked = true;
    }
    localStorage.setItem("FNM_C13", FNM);
    COMLIST_LINE();

    BaseResult.DataGridView2 = [];
    $("#ComboBox2").val("ALL");
    $("#ComboBox3").val("ALL");
    $("#ComboBox4").val("ALL");
});

$("#TabPage1").click(function (e) {
    TagIndex = 1;
});

$("#TabPage2").click(function (e) {
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

$("#RadioButton1").click(function () {
    RadioButton1_Click();
});

$("#RadioButton2").click(function () {
    RadioButton1_Click();
});

$("#ComboBox1").change(function () {
    ComboBox1_SelectedIndexChanged();
});

function COMLIST_LINE() {
    $("#BackGround").css("display", "block");
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let BaseParameter = new Object();
    BaseParameter = {
        RadioButton1: RadioButton1
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C13/COMLIST_LINE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE = data;
            BaseResult.ComboBox1 = BaseResultCOMLIST_LINE.ComboBox1;
            BaseResult.T2_S1 = BaseResultCOMLIST_LINE.T2_S1;
            ComboBox1Render();
            T2_S1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function ComboBox1Render() {
    $("#ComboBox1").empty();
  
    if (BaseResult && BaseResult.ComboBox1 && BaseResult.ComboBox1.length > 0) {
        for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
            var option = document.createElement("option");
            option.text = BaseResult.ComboBox1[i].CD_NM_EN;
            option.value = BaseResult.ComboBox1[i].CD_NM_EN;
            document.getElementById("ComboBox1").add(option);
        }
    }
}

function T2_S1Render() {
    $("#T2_S1").empty();
   
    if (BaseResult && BaseResult.T2_S1 && BaseResult.T2_S1.length > 0) {
        for (let i = 0; i < BaseResult.T2_S1.length; i++) {
            var option = document.createElement("option");
            option.text = BaseResult.T2_S1[i].CD_NM_EN;
            option.value = BaseResult.T2_S1[i].CD_NM_EN;
            document.getElementById("T2_S1").add(option);
        }
    }
}

function Buttonfind_Click() {
    if (TagIndex == 1) {
        let IsFind = true;
        let ComboBox1 = $("#ComboBox1").val();
        if (ComboBox1 == "" || ComboBox1 == "--Select Stage--") {
            IsFind = false;
        }
        if (IsFind) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [
                    $("#ComboBox1").val(),
                    $("#TextBox1").val(),
                    $("#ComboBox3").val()
                ]
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C13/Buttonfind_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonfind = data;
                    BaseResult.DataGridView1 = BaseResultButtonfind.DataGridView1;
                    DataGridView1Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                    document.getElementById("DataGridView1").innerHTML = "";
                });
            });
        }
    }
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [
                $("#ComboBox2").val(),
                $("#ComboBox4").val(),
                $("#T2_S1").val(),
                $("#T2_S2").val(),
                $("#T2_S3").val()
            ]
        };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C13/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.T2_DGV1 = BaseResultButtonfind.T2_DGV1;
                T2_DGV1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
                document.getElementById("T2_DGV1").innerHTML = "";
            });
        });
    }
}

function Buttonadd_Click() {
    // Placeholder for Buttonadd_Click
}

function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0;
        if (IsSave) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                USER_IDX: GetCookieValue("USER_IDX"),
                DataGridView2: BaseResult.DataGridView2
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C13/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess") || "Saved successfully.");
                    BaseResult.DataGridView2 = [];
                    DataGridView2Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                    alert("Error occurred: " + err.message);
                });
            });
        }
    }
}

function Buttondelete_Click() {
    if (TagIndex == 1) {
        if (confirm(localStorage.getItem("DeleteConfirm") || "Confirm Delete?")) {
            BaseResult.DataGridView2.splice(DataGridView2RowIndex, 1);
            DataGridView2Render();
        }
    }
    if (TagIndex == 2) {
        let IsDelete = true;
        if (BaseResult.T2_DGV1 && BaseResult.T2_DGV1[T2_DGV1RowIndex]) {
            let D3_D01 = BaseResult.T2_DGV1[T2_DGV1RowIndex].CODE;
            let D3_D02 = BaseResult.T2_DGV1[T2_DGV1RowIndex].TMMTIN_CNF_YN;
            if (D3_D02 == "Y") {
                alert(localStorage.getItem("Notification_C13_001") || "Can Not Delete. Please Check Again.");
                IsDelete = false;
            }
            if (IsDelete && confirm(localStorage.getItem("DeleteConfirm") || "Confirm Delete?")) {
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                    Action: TagIndex,
                    SearchString: D3_D01
                };
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/C13/Buttondelete_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {}
                }).then((response) => {
                    response.json().then((data) => {
                        Buttonfind_Click();
                        alert(localStorage.getItem("SaveSuccess") || "Deleted successfully.");
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                        alert("Error occurred: " + err.message);
                    });
                });
            }
        }
    }
}

function Buttoncancel_Click() {
    if (TagIndex == 1) {
        BaseResult.DataGridView2 = [];
        DataGridView2Render();
    }
}

function Buttoninport_Click() {
    // Placeholder for Buttoninport_Click
}

function Buttonexport_Click() {
    // Placeholder for Buttonexport_Click
}

function Buttonprint_Click() {
    // Placeholder for Buttonprint_Click
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function RadioButton1_Click() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    FNM = RadioButton1 ? 0 : 1;
    localStorage.setItem("FNM_C13", FNM);
    COMLIST_LINE();
}

function ComboBox1_SelectedIndexChanged() {
    BaseResult.DataGridView2 = [];
    DataGridView2Render();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr>";
            HTML += `<td onclick='DataGridView1_CellClick(${i})'><button class='btn waves-effect waves-light grey darken-1'>${BaseResult.DataGridView1[i].ORDER}</button></td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].DJG_CODE}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].TYPE}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].LEAD_NO}</td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].BUNDLE_SIZE}</td>`;
            HTML += `<td><input id='DataGridView1QTY${i}' type='number' class='form-control' style='width: 100px;' value='${BaseResult.DataGridView1[i].QTY || 0}'></td>`;
            HTML += `<td>${BaseResult.DataGridView1[i].STOCK}</td>`;
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_CellClick(i) {
    let IsCellClick = true;
    DataGridView1RowIndex = i;
    let ComboBox1 = $("#ComboBox1").val();
    BaseResult.DataGridView1[i].QTY = parseFloat($(`#DataGridView1QTY${i}`).val()) || 0;

    if (ComboBox1 == "" || ComboBox1 == "--Select Stage--") {
        IsCellClick = false;
    }

    if (IsCellClick) {
        let AA = ComboBox1;
        let DT = $("#DateTimePicker1").val();
        let BB = BaseResult.DataGridView1[i].DJG_CODE;
        let CC = BaseResult.DataGridView1[i].LEAD_NO;
        let DD = BaseResult.DataGridView1[i].TYPE;
        let FF = BaseResult.DataGridView1[i].BUNDLE_SIZE || 0;
        let GG = BaseResult.DataGridView1[i].QTY || FF;

        if (GG == 0 && FF == 0) {
            IsCellClick = false;
        }

        if (IsCellClick) {
            let PART_CHK = false;
            if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
                for (let j = 0; j < BaseResult.DataGridView2.length; j++) {
                    if (CC == BaseResult.DataGridView2[j].LEAD_NO) {
                        BaseResult.DataGridView2[j].QTY = (parseFloat(BaseResult.DataGridView2[j].QTY) || 0) + (parseFloat(GG) || 0);
                        PART_CHK = true;
                        break;
                    }
                }
            }
            if (!PART_CHK) {
                let DataGridView2Item = {
                    STAGE: AA,
                    DATE: DT,
                    DJG_CODE: BB,
                    TYPE: DD,
                    LEAD_NO: CC,
                    BUNDLE_SIZE: FF,
                    QTY: parseFloat(GG)
                };
                BaseResult.DataGridView2.push(DataGridView2Item);
            }
            DataGridView2Render();
        }
    }
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            HTML += `<tr onclick='DataGridView2_SelectionChanged(${i})'>`;
            HTML += `<td>${BaseResult.DataGridView2[i].STAGE}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].DATE}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].DJG_CODE}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].TYPE}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].LEAD_NO}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].BUNDLE_SIZE}</td>`;
            HTML += `<td>${BaseResult.DataGridView2[i].QTY}</td>`;
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}

function T2_DGV1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.T2_DGV1 && BaseResult.T2_DGV1.length > 0) {
        for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
            HTML += `<tr onclick='T2_DGV1_SelectionChanged(${i})'>`;
            HTML += `<td style='background-color: ${BaseResult.T2_DGV1[i].TMMTIN_CNF_YN == "Y" ? "lightgreen" : "palevioletred"}'>${BaseResult.T2_DGV1[i].TMMTIN_CNF_YN}</td>`;
            HTML += `<td style='background-color: ${BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "Y" ? "lightgreen" : BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "C" ? "yellow" : "palevioletred"}'>${BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN}</td>`;
            HTML += `<td>${BaseResult.T2_DGV1[i].TYPE}</td>`;
            HTML += `<td>${BaseResult.T2_DGV1[i].LEAD_NO}</td>`;
            HTML += `<td>${BaseResult.T2_DGV1[i].BUNDLE_SIZE}</td>`;
            HTML += `<td>${BaseResult.T2_DGV1[i].QTY}</td>`;
            HTML += `<td>${BaseResult.T2_DGV1[i].CREATE_DTM}</td>`;
            HTML += "</tr>";
        }
    }
    document.getElementById("T2_DGV1").innerHTML = HTML;
}

function T2_DGV1_SelectionChanged(i) {
    T2_DGV1RowIndex = i;
}

function T2_DGV1_DataBindingComplete() {
    T2_DGV1Render();
}

function GetCookieValue(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return "";
}