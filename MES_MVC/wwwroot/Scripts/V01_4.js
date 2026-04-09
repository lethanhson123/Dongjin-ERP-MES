let SAVE_BUTCHK = false;
let BaseResult;
let DataGridView1RowIndex;
let CMP_CODE = "";

$(window).focus(function () {
}).blur(function () {
    window.close();
});

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $("#DateTimePicker1").val(today);

    $("#Label3").val("-");
    $("#Label1").val("-");
    $("#Label11").val("-");
    $("#Label12").val("-");
    $("#Label10").val("-");
    CMP_CODE = localStorage.getItem("V01_4_CMP_CODE");
    Buttonfind_Click();
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
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    $("#FileToUpload").click();
}
$("#FileToUpload").change(function () {
    var FileToUpload = $('#FileToUpload').prop('files');
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                let F_SIZE = FileToUpload[i].size;
                let MB_SIZE = F_SIZE / 1000000;
                
                $("#Label10").val(MB_SIZE);

                if (F_SIZE >= 2300000) {
                    $("#TextBox1").val("Over File Size");
                    document.getElementById("TextBox1").style.backgroundColor = "red";
                    $("#Label12").val("");
                }
                else {
                    $("#TextBox1").val(FileToUpload[i].name);
                    document.getElementById("TextBox1").style.backgroundColor = "white";
                    $("#Label12").val(FileToUpload[i].name);
                }
            }
        }
    }
});
function Buttonfind_Click() {
    document.getElementById("Button1").disabled = true;
    document.getElementById("TextBox2").readOnly = true;
    document.getElementById("DateTimePicker1").readOnly = false;
    $("#Label11").val("-");
    $("#BackGround").css("display", "block");
    let MONTH_I = $("#ComboBox1").val();
    let BaseParameter = new Object();
    BaseParameter = {        
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(CMP_CODE);
    BaseParameter.ListSearchString.push(MONTH_I);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V01_4/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("SaveNotSuccess"));
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonadd_Click() {
    document.getElementById("Button1").disabled = false;
    document.getElementById("TextBox2").readOnly = false;
    document.getElementById("DateTimePicker1").readOnly = true;
    $("#Label11").val("New");
}
function Buttonsave_Click() {
    let IsSave = false;
    let Label11 = $("#Label11").val();
    if (Label11 == "-") {
        IsSave = false;
    }
    if (Label11 == "New") {
        IsSave = true;
    }
    let TextBox1 = $("#TextBox1").val();
    if (TextBox1 == "Over File Size") {
        IsSave = false;
        alert(localStorage.getItem("Notification_V01_4_001"));
    }
    if (IsSave == true) {
        $("#BackGround").css("display", "block");
        let F_DATE = $("#DateTimePicker1").val();
        let Label12 = $("#Label12").val();
        let TextBox2 = $("#TextBox2").val();
        let BaseParameter = new Object();
        BaseParameter = {            
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(CMP_CODE);
        BaseParameter.ListSearchString.push(F_DATE);
        BaseParameter.ListSearchString.push(Label12);
        BaseParameter.ListSearchString.push(TextBox2);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01_4/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                Buttonfind_Click();
                alert(localStorage.getItem("SaveSuccess"));
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("SaveNotSuccess"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {
    if (confirm(localStorage.setItem("DeleteConfirm"))) {
        $("#BackGround").css("display", "block");
        let DGV_CODE = BaseResult.DataGridView1[DataGridView1RowIndex].PD_CMPNY_PART_IDX;
        let BaseParameter = new Object();
        BaseParameter = {            
            SearchString: DGV_CODE,
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01_4/Buttondelete_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.setItem("SaveSuccess"));
                Buttonfind_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.setItem("SaveNotSuccess"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonclose_Click() {
    window.close();
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
}
$("#TextBox2").keypress(function (e) {
    TextBox2_KeyPress();
});
function TextBox2_KeyPress() {

}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].COST_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].COST_FILENM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].COST_FILETYPE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REMARK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].FILE + "</td>";
                    HTML = HTML + "</tr>";
                }
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
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
}