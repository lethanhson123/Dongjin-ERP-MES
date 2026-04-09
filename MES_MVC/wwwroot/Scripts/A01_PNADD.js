let IsTableSort = false;
let BaseResult = new Object();
let F_ECN_IDX = 0;
let DataGridView1RowIndex = 0;
let SAVE_BUTCHK = false;
let FULL_NM = "";
$(document).ready(function () {
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
});
$("#Buttoninport").click(function () {
    $("#FileToUpload").click();
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#Buttonhelp").click(function () {
    Buttonhelp_Click();
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
$("#FileToUpload").change(function () {
    Buttoninport_Click();
});
$("#ComboBox1").change(function () {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    let ComboBox1 = document.getElementById("ComboBox1");
    let ComboBox1Text = ComboBox1.options[ComboBox1.selectedIndex].text;    
    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
        BaseResult.DataGridView1[i].Item_TypeE = ComboBox1Text;
    }
    DataGridView1Render();
}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    var FileToUpload = $('#FileToUpload').prop('files');
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                formUpload.append('file[]', FileToUpload[i]);
            }
        }
    }
    let url = "/A01_PNADD/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            ComboBox1_SelectedIndexChanged();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonsave_Click() {
    let IsSave = true;
    if (BaseResult.DataGridView1.length == 0) {
        IsSave = false;
    }
    if (IsSave == true) {
        let ComboBox1 = $("#ComboBox1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: ComboBox1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        var FileToUpload = $('#FileToUpload').prop('files');
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/A01_PNADD/Buttonsave_Click";
        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                window.close();
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    window.close();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOM_GROUP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MODEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_FamilyPC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Packing_Unit + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Item_TypeE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_SUPL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Location + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseParameter.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}