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
    FNM = localStorage.getItem("FNM_C16");
    if (FNM == null) FNM = 0;
    if (FNM == 0) document.getElementById("RadioButton1").checked = true;
    else document.getElementById("RadioButton2").checked = true;
    localStorage.setItem("FNM_C16", FNM);
    COMLIST_LINE();

    BaseResult.DataGridView2 = [];
    $("#DateTimePicker1").prop("min", today);
    $("#DateTimePicker1").prop("max", new Date(now.setDate(now.getDate() + 3)).toISOString().split("T")[0]);
    $("#ComboBox2").val("ALL");
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
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
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

function Buttonfind_Click() {
    if (TagIndex == 1) {
        let IsFind = true;
        let ComboBox1 = $("#ComboBox1").val();
        if (ComboBox1 == "Select Stage...") {
            IsFind = false;
        }
        if (IsFind == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: []
            };
            BaseParameter.ListSearchString.push($("#TextBox1").val());
            BaseParameter.ListSearchString.push($("#TextBox2").val());
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C16/Buttonfind_Click";

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
                    alert("Error: Internet connection issue or server error. Please try again.");
                });
            });
        }
    }
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: []
        };
        BaseParameter.ListSearchString.push($("#ComboBox2").val());
        BaseParameter.ListSearchString.push($("#T2_S2").val());
        BaseParameter.ListSearchString.push($("#T2_S3").val());
        BaseParameter.ListSearchString.push($("#T2_S4").val());
        BaseParameter.ListSearchString.push($("#T2_S1").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C16/Buttonfind_Click";

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
                alert("Error: Internet connection issue or server error. Please try again.");
            });
        });
    }
}

function Buttonadd_Click() { }

function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = false;
        if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
            IsSave = true;
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                USER_IDX: GetCookieValue("USER_IDX"),
                DataGridView2: BaseResult.DataGridView2
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C16/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    if (data.Success) {
                        alert(localStorage.getItem("SaveSuccess") || "Saved successfully.");
                        BaseResult.DataGridView2 = [];
                        DataGridView2Render();
                    } else {
                        alert(data.Error || "Error: Failed to save data.");
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                    alert("Error: Internet connection issue or server error. Please try again.");
                });
            });
        } else {
            alert("No data to save.");
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
                alert(localStorage.getItem("Notification_C16_001") || "Cannot delete. Please check again.");
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
                let url = "/C16/Buttondelete_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {}
                }).then((response) => {
                    response.json().then((data) => {
                        if (data.Success) {
                            Buttonfind_Click();
                            alert(localStorage.getItem("SaveSuccess") || "Deleted successfully.");
                        } else {
                            alert(data.Error || "Error: Failed to delete data.");
                        }
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                        alert("Error: Internet connection issue or server error. Please try again.");
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

function Buttoninport_Click() { }

function Buttonexport_Click() { }

function Buttonprint_Click() { }

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function RadioButton1_Click() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    if (RadioButton1 == true) {
        COMLIST_LINE();
        FNM = 0;
    } else {
        COMLIST_LINE();
        FNM = 1;
    }
    localStorage.setItem("FNM_C16", FNM);
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
            HTML += "<td onclick='DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].ORDER + "</button></td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_IDX + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TMMTIN_CODE + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_FML + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_SNP + "</td>";
            HTML += "<td><input id='DataGridView1QTY" + i + "' type='number' class='form-control' value='0' style='width: 100px;'></td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].STOCK + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}

function DataGridView1_CellClick(i) {
    let IsCellClick = true;
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].QTY = $("#DataGridView1QTY" + DataGridView1RowIndex).val();
    let ComboBox1 = $("#ComboBox1").val();
    if (ComboBox1 == "Select Stage...") {
        IsCellClick = false;
    }

    if (IsCellClick) {
        let AA = ComboBox1;
        let DT = $("#DateTimePicker1").val();
        let BB = BaseResult.DataGridView1[DataGridView1RowIndex].PART_IDX;
        let CC = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NO;
        let DD = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NM;
        let EE = BaseResult.DataGridView1[DataGridView1RowIndex].PART_FML;
        let FF = BaseResult.DataGridView1[DataGridView1RowIndex].PART_SNP;
        let GG = BaseResult.DataGridView1[DataGridView1RowIndex].QTY;
        let HH = BaseResult.DataGridView1[DataGridView1RowIndex].TMMTIN_CODE;

        if (GG == 0) {
            if (FF == 0) {
                IsCellClick = false;
            } else {
                GG = FF;
            }
        }

        if (IsCellClick) {
            let PART_CHK = false;
            if (BaseResult.DataGridView2.length > 0) {
                for (let j = 0; j < BaseResult.DataGridView2.length; j++) {
                    if (CC == BaseResult.DataGridView2[j].PART_NO) {
                        BaseResult.DataGridView2[j].QTY = parseInt(BaseResult.DataGridView2[j].QTY) + parseInt(GG);
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
                    PART_NO: CC,
                    PART_NAME: DD,
                    FAMILY: EE,
                    PART_SNP: FF,
                    QTY: GG,
                    TMMTIN_CODE: HH
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
            HTML += "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView2[i].STAGE + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].DATE + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].DJG_CODE + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].TMMTIN_CODE + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NAME + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].FAMILY + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_SNP + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].QTY + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    DataGridView2Render();
}

function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}

function T2_DGV1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.T2_DGV1 && BaseResult.T2_DGV1.length > 0) {
        for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
            HTML += "<tr onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
            if (BaseResult.T2_DGV1[i].TMMTIN_CNF_YN == "Y") {
                HTML += "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
            } else {
                HTML += "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
            }
            if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "Y") {
                HTML += "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
            } else if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "C") {
                HTML += "<td style='background-color: yellow;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
            } else {
                HTML += "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
            }
            HTML += "<td>" + BaseResult.T2_DGV1[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.T2_DGV1[i].TMMTIN_CODE + "</td>";
            HTML += "<td>" + BaseResult.T2_DGV1[i].SNP + "</td>";
            HTML += "<td>" + BaseResult.T2_DGV1[i].QTY + "</td>";
            HTML += "<td>" + BaseResult.T2_DGV1[i].PART_NAME + "</td>";
            HTML += "<td>" + BaseResult.T2_DGV1[i].CREATE_DTM + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("T2_DGV1").innerHTML = HTML;
}

function T2_DGV1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.T2_DGV1, IsTableSort);
    T2_DGV1Render();
}

function T2_DGV1_SelectionChanged(i) {
    T2_DGV1RowIndex = i;
}

function T2_DGV1_DataBindingComplete() {
    D1_SORT();
}

function D1_SORT() {
    if (BaseResult && BaseResult.T2_DGV1 && BaseResult.T2_DGV1.length > 0) {
        for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
        }
    }
}

function COMLIST_LINE() {
    $("#BackGround").css("display", "block");
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let BaseParameter = {
        RadioButton1: RadioButton1
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C16/COMLIST_LINE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE = data;
            BaseResult.cb_Stage1 = BaseResultCOMLIST_LINE.cb_Stage1;
            ComboBox1Render();
            T2_S1Render();
            $("#ComboBox1").prepend("<option value='Select Stage...'>Select Stage...</option>");
            $("#ComboBox1").val("Select Stage...");
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Error: Internet connection issue or server error. Please try again.");
        });
    });
}

function ComboBox1Render() {
    $("#ComboBox1").empty();
    if (BaseResult && BaseResult.cb_Stage1 && BaseResult.cb_Stage1.length > 0) {
        for (let i = 0; i < BaseResult.cb_Stage1.length; i++) {
            var option = document.createElement("option");
            option.text = BaseResult.cb_Stage1[i].CD_NM_EN;
            option.value = BaseResult.cb_Stage1[i].CD_NM_EN;
            document.getElementById("ComboBox1").add(option);
        }
    }
}

function T2_S1Render() {
    $("#T2_S1").empty();
    if (BaseResult && BaseResult.cb_Stage1 && BaseResult.cb_Stage1.length > 0) {
        for (let i = 0; i < BaseResult.cb_Stage1.length; i++) {
            var option = document.createElement("option");
            option.text = BaseResult.cb_Stage1[i].CD_NM_EN;
            option.value = BaseResult.cb_Stage1[i].CD_NM_EN;
            document.getElementById("T2_S1").add(option);
        }
    }
}

function GetCookieValue(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return "";
}

function DataGridViewSort(data, isAsc) {
    if (!data) return;
    data.sort((a, b) => {
        let key = Object.keys(a)[0];
        let valA = a[key];
        let valB = b[key];
        if (typeof valA === 'string') {
            return isAsc ? valA.localeCompare(valB) : valB.localeCompare(valA);
        } else {
            return isAsc ? valA - valB : valB - valA;
        }
    });
}