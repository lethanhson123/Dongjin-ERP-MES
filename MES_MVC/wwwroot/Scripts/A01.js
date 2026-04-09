let IsTableSort = false;
let BaseResult = new Object();;
let TagIndex = 1;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let SQLCHK = 0;
let Now;
let IsButtonfind = true;
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $("#DateTimePicker1").val(Now);
    document.getElementById("TextBox1").readOnly = true;

});
$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
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
function SetUI(IsAdd) {
    if (IsAdd == 1) {
        document.getElementById("ComboBox1").style.display = "none";
        document.getElementById("ComboBox1TextBox").style.display = "block";

        document.getElementById("ComboBox2").style.display = "none";
        document.getElementById("ComboBox2TextBox").style.display = "block";
    }
    else {
        document.getElementById("ComboBox1").style.display = "block";
        document.getElementById("ComboBox1TextBox").style.display = "none";

        document.getElementById("ComboBox2").style.display = "block";
        document.getElementById("ComboBox2TextBox").style.display = "none";
    }
}
function Buttonfind_Click() {
    if (TagIndex == 1) {
        IsButtonfind = true;
        SetUI(0);
        document.getElementById("TextBox1").readOnly = true;
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($("#suchTB1").val());
        BaseParameter.ListSearchString.push($("#suchTB2").val());
        BaseParameter.ListSearchString.push($("#suchTB3").val());
        BaseParameter.ListSearchString.push($("#ComboBox5").val());
        BaseParameter.ListSearchString.push($("#TextBox8").val());
        BaseParameter.ListSearchString.push($("#TextBox9").val());
        BaseParameter.ListSearchString.push($("#ComboBox3").val());
        BaseParameter.ListSearchString.push($("#TextBox10").val());

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/A01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView1 = BaseResultButtonfind.DataGridView1;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });
    }
    if (TagIndex == 2) {
        let IsFind = true;
        $("#Label14").val("-");
        $("#TextBox3").val("");
        $("#TextBox4").val("");
        let ComboBox3SelectedIndex = $("#ComboBox3 option:selected").index();
        if (ComboBox3SelectedIndex == 0) {
            alert(localStorage.getItem("Notification_A01_001"));
            IsFind = false;
        }
        if (IsFind == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push($("#ComboBox3").val());
            BaseParameter.ListSearchString.push($("#TextBox10").val());

            let formUpload = new FormData();
            formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            let url = "/A01/Buttonfind_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonfind = data;
                    BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView1;
                    DataGridView2Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                });
            });
        }
    }
}
function Buttonadd_Click() {
    if (TagIndex == 1) {
        SQLCHK = 1;
        $("#TextBox1").val("");
        $("#TextBox2").val("");


        SetUI(1);

        $("#TextBox5").val("0");
        $("#MaskedTextBox1").val("");
        $("#TextBox21").val("");
        $("#RadioButton1").prop("checked", true);
        $("#ComboBox5").prop("selectedIndex", 0);
        $("#TextBox6").prop("selectedIndex", 0);

        document.getElementById("TextBox1").readOnly = false;
    }
    if (TagIndex == 2) {
        let IsAdd = true;
        let Label14 = $("#Label14").val();
        if (Label14.length < 2) {
            alert(localStorage.getItem("Notification_A01_001"));
            IsAdd = false;
        }
        if (IsAdd == true) {
            document.getElementById("TextBox3").readOnly = false;
            document.getElementById("DateTimePicker1").readOnly = false;
            document.getElementById("TextBox4").readOnly = false;
            $("#TextBox3").val("");
            $("#DateTimePicker1").val(Now);
            $("#TextBox4").val("");
            $("#TextBox3").focus();
        }
    }
}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = true;
        let TextBox1 = $("#TextBox1").val();
        if (TextBox1 == "") {
            IsSave = false;
            alert("Part No Error. Please Check Again.");
        }
        let TextBox6Display = document.getElementById("TextBox6").style.display;
        if (TextBox6Display != "none") {
            let TextBox6Index = document.getElementById("TextBox6").selectedIndex;
            if (TextBox6Index == "0") {
                IsSave = false;
                alert("Not select " + $("#TextBox6").val() + ". (Một lỗi đã xảy ra.)");
            }
        }
        let ComboBox1Display = document.getElementById("ComboBox1").style.display;
        if (ComboBox1Display != "none") {
            let ComboBox1Index = document.getElementById("ComboBox1").selectedIndex;
            if (ComboBox1Index == "0") {
                IsSave = false;
                alert("Not select " + $("#ComboBox1").val() + ". (Một lỗi đã xảy ra.)");
            }
        }
        let ComboBox2Display = document.getElementById("ComboBox2").style.display;
        if (ComboBox1Display != "none") {
            let ComboBox2Index = document.getElementById("ComboBox2").selectedIndex;
            if (ComboBox2Index == "0") {
                IsSave = false;
                alert("Not select " + $("#ComboBox2").val() + ". (Một lỗi đã xảy ra.)");
            }
        }
        if (IsSave == true) {
            let TextBox5 = $("#TextBox5").val();
            if (TextBox5 == "") {
                TextBox5 = "0";
            }
            let ComboBox1 = $("#ComboBox1").val();

            if (ComboBox1Display == "none") {
                ComboBox1 = $("#ComboBox1TextBox").val();
            }
            let ComboBox2 = $("#ComboBox2").val();

            if (ComboBox2Display == "none") {
                ComboBox2 = $("#ComboBox2TextBox").val();
            }

            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push($("#TextBox1").val());
            BaseParameter.ListSearchString.push($("#TextBox2").val());
            BaseParameter.ListSearchString.push(ComboBox1);
            BaseParameter.ListSearchString.push(ComboBox2);
            BaseParameter.ListSearchString.push(TextBox5);
            BaseParameter.ListSearchString.push($("#TextBox6").val());
            if ($("#RadioButton1").is(":checked") == true) {
                BaseParameter.ListSearchString.push("Y");
            }
            else {
                BaseParameter.ListSearchString.push("N");
            }
            BaseParameter.ListSearchString.push($("#MaskedTextBox1").val());
            BaseParameter.ListSearchString.push($("#ComboBox4").val());
            BaseParameter.ListSearchString.push($("#TextBox21").val());
            let formUpload = new FormData();
            formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            let url = "/A01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    Buttonfind_Click();
                    document.getElementById("TextBox1").readOnly = true;

                    document.getElementById("ComboBox1").style.display = "block";
                    document.getElementById("ComboBox1TextBox").style.display = "none";

                    document.getElementById("ComboBox2").style.display = "block";
                    document.getElementById("ComboBox2TextBox").style.display = "none";

                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }

    if (TagIndex == 2) {
        let IsSave = true;
        let Label14 = $("#Label14").val();
        if (Label14 == "") {
            IsSave = false;
            alert("Part No Error. Please Check Again.");
        }
        let Label25 = $("#Label25").val();
        if (Label25 == "") {
            IsSave = false;
            alert("Part No Error. Please Check Again.");
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            let AAA = $("#TextBox3").val();
            let BBB = $("#DateTimePicker1").val();
            let CCC = $("#TextBox4").val();

            BaseParameter.ListSearchString.push(Label25);
            BaseParameter.ListSearchString.push(AAA);
            BaseParameter.ListSearchString.push(BBB);
            BaseParameter.ListSearchString.push(CCC);
            let formUpload = new FormData();
            formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            let url = "/A01/Buttonsave_Click";
            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    DGV2_LOAD(DataGridView2RowIndex);
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        let i = 0;
        while (i < BaseResult.DataGridView1.length) {
            if (BaseResult.DataGridView1[i].CHK == true) {
                BaseResult.DataGridView1.splice(i, 1);
            }
            else {
                i++;
            }
        }
        DataGridView1Render();
    }
    if (TagIndex == 2) {
        if (confirm(localStorage.getItem("DeleteConfirm"))) {
            if (BaseResult) {
                if (BaseResult.DataGridView3) {
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        Action: TagIndex,
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    let P_INDEX = BaseResult.DataGridView3[DataGridView3RowIndex].PARTECN_IDX;
                    let FULL_FILE_PATH = BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_EXPOR;
                    let FULL_FILE = BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_GRP;
                    BaseParameter.ListSearchString.push(P_INDEX);
                    BaseParameter.ListSearchString.push(FULL_FILE_PATH);
                    BaseParameter.ListSearchString.push(FULL_FILE);
                    let formUpload = new FormData();
                    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
                    let url = "/A01/Buttondelete_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            DGV2_LOAD();
                            alert(localStorage.getItem("SaveSuccess"));
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            alert(localStorage.getItem("ERROR"));
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        SQLCHK = 0;
        $("#TextBox1").val("");
        $("#TextBox2").val("");
        document.getElementById("ComboBox1").options.length = 0;
        let ComboBox1Value = "Select " + $("#Label6").val();
        $("#ComboBox1").append(new Option(ComboBox1Value, ComboBox1Value));
        $("#TextBox5").val("");
        $("#MaskedTextBox1").val("");
        document.getElementById("RadioButton1").checked = true;
        document.getElementById("TextBox1").readOnly = true;
    }
    if (TagIndex == 2) {
        $("#Label14").val("");
        $("#TextBox3").val("");
        $("#TextBox4").val("");
        if (BaseResult) {
            if (BaseResult.DataGridView1) {
                BaseResult.DataGridView1 = [];
                DataGridView1Render();
            }
        }
    }
}

function Buttoninport_Click() {
    if (TagIndex == 1) {
        let url = "/A01_PNADD";
        OpenWindowByURL(url, 800, 400);
    }
}
function Buttonexport_Click() {
    if (TagIndex == 1) {
        IsButtonfind = false;
        SetUI(0);
        document.getElementById("TextBox1").readOnly = true;
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ID = 0;
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($("#suchTB1").val());
        BaseParameter.ListSearchString.push($("#suchTB2").val());
        BaseParameter.ListSearchString.push($("#suchTB3").val());
        BaseParameter.ListSearchString.push($("#ComboBox5").val());
        BaseParameter.ListSearchString.push($("#TextBox8").val());
        BaseParameter.ListSearchString.push($("#TextBox9").val());
        BaseParameter.ListSearchString.push($("#ComboBox3").val());
        BaseParameter.ListSearchString.push($("#TextBox10").val());

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/A01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView1 = BaseResultButtonfind.DataGridView1;
                DataGridView1Render();
                let filename = "A01_" + Now;
                TableHTMLToExcel("DataGridView1Table", "A01", filename);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });
    }
}
function Buttonprint_Click() {
    if (TagIndex == 1) {
        //let url = "/A01_PRINTV";
        //OpenWindowByURL(url, 800, 400);

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($("#suchTB1").val());
        BaseParameter.ListSearchString.push($("#suchTB2").val());
        BaseParameter.ListSearchString.push($("#suchTB3").val());
        BaseParameter.ListSearchString.push($("#ComboBox5").val());
        BaseParameter.ListSearchString.push($("#TextBox8").val());
        BaseParameter.ListSearchString.push($("#TextBox9").val());
        BaseParameter.ListSearchString.push($("#ComboBox3").val());
        BaseParameter.ListSearchString.push($("#TextBox10").val());
        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/A01/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonprint_Click = data
                if (BaseResultButtonprint_Click) {
                    if (BaseResultButtonprint_Click.Code) {
                        let url = BaseResultButtonprint_Click.Code;
                        OpenWindowByURL(url, 200, 200);
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
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

$("#suchTB1").keydown(function (e) {
    if (e.keyCode == 13) {
        SuchTB1_KeyDown();
    }
});
function SuchTB1_KeyDown() {
    Buttonfind_Click();
}
$("#suchTB2").keydown(function (e) {
    if (e.keyCode == 13) {
        SuchTB2_KeyDown();
    }
});
function SuchTB2_KeyDown() {
    Buttonfind_Click();
}
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
$("#TextBox1").keypress(function (e) {
    TextBox1_KeyPress();
});
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
$("#TextBox2").keypress(function (e) {
    TextBox1_KeyPress();
});
function TextBox1_KeyDown() {
    DGV_KeyPress();
}
function TextBox1_KeyPress() {
}
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        DGV_KeyPress();
    }
});
$("#TextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
$("#TextBox5").keypress(function (e) {
    TextBox5_KeyPress();
});
function TextBox5_KeyPress() {

}
$("#TextBox6").change(function (e) {
    TextBox6_SelectedIndexChanged();
});
function TextBox6_SelectedIndexChanged() {
    CB_ADD();
}
$("#MaskedTextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
$("#MaskedTextBox1").keypress(function (e) {
    MaskedTextBox1_KeyPress();
});
$("#MaskedTextBox1").focusout(function (e) {
    MaskedTextBox1_Leave();
});
function MaskedTextBox1_KeyPress() {

}
function MaskedTextBox1_Leave() {
    let MaskedTextBox1 = $("#MaskedTextBox1").val();
    if (MaskedTextBox1 == " -  -") {
        $("#MaskedTextBox1").val(MaskedTextBox1);
    }
    else {
        MaskedTextBox1 = MaskedTextBox1.trim();
        MaskedTextBox1 = MaskedTextBox1.replace(" ", "0");
        $("#MaskedTextBox1").val(MaskedTextBox1);
    }
}
$("#RadioButton1").click(function () {
    RadioButton1_CheckedChanged();
});
$("#RadioButton2").click(function () {
    RadioButton1_CheckedChanged();
});
function RadioButton1_CheckedChanged() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton1 == true) {
        $("#TextBox7").val("Y");
    }
    if (RadioButton2 == true) {
        $("#TextBox7").val("N");
    }
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                localStorage.setItem("A01_FILE_PNO_TXT", BaseResult.DataGridView3[DataGridView3RowIndex].PART_NO);
                localStorage.setItem("A01_FILE_ECN_TXT", BaseResult.DataGridView3[DataGridView3RowIndex].PART_ENCNO);
                localStorage.setItem("A01_FILE_ECN_DATE", BaseResult.DataGridView3[DataGridView3RowIndex].PART_ECN_DATE);
                localStorage.setItem("A01_FILE_F_ECN_IDX", BaseResult.DataGridView3[DataGridView3RowIndex].PARTECN_IDX);
                let url = "/A01_FILE";
                OpenWindowByURL(url, 800, 400);
                DGV2_LOAD();
            }
        }
    }
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                let RowCount = 50;
                if (IsButtonfind == false) {
                    RowCount = BaseResult.DataGridView1.length;
                }
                if (RowCount >= BaseResult.DataGridView1.length) {
                    RowCount = BaseResult.DataGridView1.length;
                }
                DataGridView1_SelectionChanged(0);
                HTML = HTML + "<tbody>";
                for (let i = 0; i < RowCount; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_SUPL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOM_GROUP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MODEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_FamilyPC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Packing_Unit + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Item_TypeK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Item_TypeE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Location + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_USE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_ENCNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_ECN_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Creation_date + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Creation_User + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Update_Date + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].UPDATE_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                $("#TextBox1").val(BaseResult.DataGridView1[DataGridView1RowIndex].PART_NO);
                $("#TextBox2").val(BaseResult.DataGridView1[DataGridView1RowIndex].PART_NAME);
                $("#TextBox5").val(BaseResult.DataGridView1[DataGridView1RowIndex].Packing_Unit);
                $("#MaskedTextBox1").val(BaseResult.DataGridView1[DataGridView1RowIndex].Location);
                $("#TextBox21").val(BaseResult.DataGridView1[DataGridView1RowIndex].PART_SUPL);
                $("#ComboBox4").val(BaseResult.DataGridView1[DataGridView1RowIndex].BOM_GROUP);


                const dd = document.getElementById('TextBox6');
                dd.selectedIndex = [...dd.options].findIndex(option => option.text === BaseResult.DataGridView1[DataGridView1RowIndex].Item_TypeE);
                let Language = GetCookieValue("Language");
                if (Language == "kr") {
                    dd.selectedIndex = [...dd.options].findIndex(option => option.text === BaseResult.DataGridView1[DataGridView1RowIndex].Item_TypeK);
                }


                if (BaseResult.DataGridView1[i].PART_USE == "Y") {
                    $("#RadioButton1").prop("checked", true);
                }
                else {
                    $("#RadioButton2").prop("checked", true);
                }
                CB_ADD();
            }
        }
    }
}
function CB_ADD() {

    $("#ComboBox1").find("option:not(:first)").remove();
    $("#ComboBox2").find("option:not(:first)").remove();

    let SELECT_V = $("#TextBox6").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: SELECT_V,
    }
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/A01/CB_ADD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCB_ADD = data;
            if (BaseResultCB_ADD.ComboBox1) {
                for (let i = 0; i < BaseResultCB_ADD.ComboBox1.length; i++) {
                    $("#ComboBox1").append("<option value='" + BaseResultCB_ADD.ComboBox1[i].PART_CAR + "'>" + BaseResultCB_ADD.ComboBox1[i].PART_CAR + "</option>");
                }
            }
            if (BaseResultCB_ADD.ComboBox2) {
                for (let i = 0; i < BaseResultCB_ADD.ComboBox2.length; i++) {
                    $("#ComboBox2").append("<option value='" + BaseResultCB_ADD.ComboBox2[i].PART_FML + "'>" + BaseResultCB_ADD.ComboBox2[i].PART_FML + "</option>");
                }
            }
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    if (BaseResult.DataGridView1.length > 0) {
                        $("#ComboBox1").val(BaseResult.DataGridView1[DataGridView1RowIndex].MODEL);
                        $("#ComboBox2").val(BaseResult.DataGridView1[DataGridView1RowIndex].PART_FamilyPC);
                        $("#ComboBox1TextBox").val(BaseResult.DataGridView1[DataGridView1RowIndex].MODEL);
                        $("#ComboBox2TextBox").val(BaseResult.DataGridView1[DataGridView1RowIndex].PART_FamilyPC);
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function DGV_KeyPress() {

}
function DataGridView1CHKChanged(i) {
    let id = "DataGridView1CHK" + i;
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = $("#" + id).is(":checked");
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
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
    DGV2_LOAD(DataGridView2RowIndex);
}

function DGV2_LOAD(i) {
    DataGridView2RowIndex = i;
    document.getElementById("TextBox3").readOnly = true;
    document.getElementById("DateTimePicker1").readOnly = false;
    document.getElementById("TextBox4").readOnly = true;
    $("#Label14").val("-");
    $("#TextBox3").val("");
    $("#TextBox4").val("");

    let P_IDX = BaseResult.DataGridView2[DataGridView2RowIndex].PART_IDX;
    let P_NO = BaseResult.DataGridView2[DataGridView2RowIndex].PART_NO;

    $("#Label14").val(P_NO);
    $("#Label25").val(P_IDX);

    $("#BackGround").css("display", "block");
    let BaseResultDGV2_LOAD = new Object();
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: P_IDX,
    }
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/A01/DGV2_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultDGV2_LOAD = data;
            BaseResult.DataGridView3 = BaseResultDGV2_LOAD.DataGridView3;
            DataGridView3Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DGV_CTRender() {
    let HTML = "";
    document.getElementById("DGV_CT").innerHTML = HTML;
}
function DGV_CTSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DGV_CT, IsTableSort);
    DGV_CTRender();
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_ECN_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_ENCNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].DWG_NO + "</td>";
                    HTML = HTML + "<td onclick='DataGridView3_CellClick(" + i + ")'>" + BaseResult.DataGridView3[i].FVWR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].DWG_FILE_GRP + "</td>";
                    HTML = HTML + "<td onclick='DataGridView3_CellClick(" + i + ")'>" + BaseResult.DataGridView3[i].ADDFILE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UPDATE_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    DataGridView3Render();
}
function DataGridView3_CellClick(i) {
    DataGridView3RowIndex = i;
    let filePAthName = BaseResult.DataGridView3[DataGridView3RowIndex].ADDFILE;
    let ftpAddress = BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_EXPOR + "/";
    let ftpUser = "ysj4947";
    let ftpPassword = "Kh0ngx0a";
    let fileToDownload = filePAthName;
    let sFtpFile = ftpAddress + fileToDownload;
    let url = sFtpFile;
    OpenWindowByURL(url, 800, 400);
    //if (DataGridView3Action == 1) {
    //    let DWG_FILE_EXPOR = BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_EXPOR;
    //    let DWG_FILE_GRP = BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_GRP;
    //    localStorage.setItem("A01_PDF_FULL_FILE_PATH", DWG_FILE_EXPOR);
    //    localStorage.setItem("A01_PDF_FULL_FILE", DWG_FILE_GRP);
    //    let EXFL = DWG_FILE_GRP.length - 2;
    //    DWG_FILE_GRP = DWG_FILE_GRP.toUpperCase();
    //    let check = DWG_FILE_GRP.includes("PDF");
    //    if (check == true) {
    //        let url = "/A01_PDF";
    //        OpenWindowByURL(url, 800, 400);
    //    }
    //}
    //if (DataGridView3Action == 2) {
    //}
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                $("#Label14").val(BaseResult.DataGridView3[DataGridView3RowIndex].PART_NO);
                $("#TextBox3").val(BaseResult.DataGridView3[DataGridView3RowIndex].PART_ENCNO);
                $("#DateTimePicker1").val(BaseResult.DataGridView3[DataGridView3RowIndex].PART_ECN_DATE);
                $("#TextBox4").val(BaseResult.DataGridView3[DataGridView3RowIndex].DWG_NO);
                $("#TextBox20").val(BaseResult.DataGridView3[DataGridView3RowIndex].DWG_FILE_GRP);
            }
        }
    }
}