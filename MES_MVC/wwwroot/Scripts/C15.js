let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;

$(document).ready(function () {

    var now = new Date();
    DateNow = DateToString(now);

    $("#DateTimePicker1").val(DateNow);
    $("#NumericUpDown1").val(1);   

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];

});
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Buttonadd_Click();
    $("#TextBox1").val("");
    $("#TextBox1").focus();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    localStorage.setItem("C15_1_Close", 0);
    let url = "/C15_1";
    OpenWindowByURL(url, 800, 460);
    C15_1TimerStartInterval();
}
function C15_1TimerStartInterval() {
    let C15_1Timer = setInterval(function () {
        let C15_1_Close = localStorage.getItem("C15_1_Close");
        if (C15_1_Close == "1") {
            clearInterval(C15_1Timer);
            C11_1_Close = 0;
            localStorage.setItem("C15_1_Close", C15_1_Close);

            let DataGridView1Item = {
                Description: $("#DateTimePicker1").val(),
                LEAD_PN: localStorage.getItem("C15_1_LEAD_PN"),
                BUNDLE_SIZE: localStorage.getItem("C15_1_BUNDLE_SIZE"),
                QTY: 1,
                Name: decodeURI(GetCookieValue("USER_NM")),
            }
            BaseResult.DataGridView1.push(DataGridView1Item);
            DataGridView1Render();
        }
    }, 100);
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
}
function Buttonadd_Click() {
    if (TagIndex == 1) {
        let TextBox1 = $("#TextBox1").val();
        let DateTimePicker1 = $("#DateTimePicker1").val();
        let NumericUpDown1 = $("#NumericUpDown1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        BaseParameter.ListSearchString.push(TextBox1);
        BaseParameter.ListSearchString.push(DateTimePicker1);
        BaseParameter.ListSearchString.push(NumericUpDown1);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C15/Buttonadd_Click";

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
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonsave_Click() {

}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        if (DataGridView1RowIndex > -1) {
            BaseResult.DataGridView1.splice(DataGridView1RowIndex, 1);
            DataGridView1Render();
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        BaseResult.DataGridView1 = [];
        DataGridView1Render();
    }
}
function Buttoninport_Click() {

}
function Buttonexport_Click() {

}
function Buttonprint_Click() {
    if (TagIndex == 1) {
        let IsCheck = true;
        if (BaseResult.DataGridView1.length <= 0) {
            IsCheck = false;
            alert("LEAD NO Please Check Again.");
        }
        if (IsCheck == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView1 = BaseResult.DataGridView1;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C15/Buttonprint_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {

                    let BaseResultSub = data;
                    if (BaseResultSub) {
                        if (BaseResultSub.Code) {
                            let url = BaseResultSub.Code;
                            OpenWindowByURL(url, 200, 200);
                        }                       
                    }
                    BaseResult.DataGridView1 = [];
                    DataGridView1Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }       
    }
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Description + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_PN + "</td>";
                    HTML = HTML + "<td><input onblur='DataGridView1BUNDLE_SIZEChange(" + i + ", this)' type='number' class='form-control' value='" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "' style='width: 200px;'></td>";
                    HTML = HTML + "<td><input onblur='DataGridView1QTYChange(" + i + ", this)' type='number' class='form-control' value='" + BaseResult.DataGridView1[i].QTY + "' style='width: 200px;'></td>";                    
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Name + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "DATE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});

function DataGridView1BUNDLE_SIZEChange(i, input) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE = input.value;
}
function DataGridView1QTYChange(i, input) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].QTY = input.value;
}
