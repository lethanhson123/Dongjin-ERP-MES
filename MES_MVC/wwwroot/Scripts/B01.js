let IsTableSort = false;

let BaseParameter = new Object();
BaseParameter = {
    ListB01: [],
}

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#Datelist").val(today);
    $("#rbchk1").prop("checked", true);
    localStorage.setItem("B01_1BBB", "");
    localStorage.setItem("B01_1CCC", "");
    localStorage.setItem("B01_1DDD", "");
});
function StartInterval() {
    setInterval(function () {
        $("#TextBox1").val(localStorage.getItem("B01_1DDD"));
        $("#TextBox2").val(localStorage.getItem("B01_1BBB"));
        $("#TextBox4").val(localStorage.getItem("B01_1CCC"));
    }, 100);
}
$("#FileToUpload").change(function () {
    Buttoninport_Click();
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
    $("#FileToUpload").click();

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
    BaseParameter = {
        ListB01: [],
    }
    DataGridViewRender();
}
function Buttonadd_Click() {
    if (BaseParameter.ListB01 == null) {
        BaseParameter = {
            ListB01: [],
        }
    }
    let IsAdd = true;
    let TextBox2 = $("#TextBox2").val();
    let TextBox5 = $("#TextBox5").val();
    if (TextBox2 == "") {
        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        IsAdd = false;
    }
    if (TextBox5 == "") {
        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        IsAdd = false;
    }
    else {
        if (TextBox5.includes("e")) {
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
            IsAdd = false;
        }
        let TextBox5Number = Number(TextBox5);
        if (TextBox5Number < 0) {
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
            IsAdd = false;
        }
    }
    if (IsAdd == true) {
        let B01 = {
            dgvcheck: true,
            NO: "" + BaseParameter.ListB01.length + 1,
            PARTNO: $("#TextBox2").val(),
            UNIT: $("#TextBox3").val(),
            GOODS: $("#TextBox4").val(),
            QUANTITY: $("#TextBox5").val(),
            NWEIGHT: $("#TextBox6").val(),
            GWEIGHT: $("#TextBox7").val(),
            PalletNo: $("#TextBox8").val(),
            ShippedNo: $("#TextBox9").val(),
            inputdate: $("#Datelist").val(),
            Remark: "",
        }
        BaseParameter.ListB01.push(B01);
        DataGridViewRender();
        $("#TextBox2").val("");
        $("#TextBox3").val("");
        $("#TextBox4").val("");
        $("#TextBox5").val("");
        $("#TextBox6").val("");
        $("#TextBox7").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        localStorage.setItem("B01_1BBB", "");
        localStorage.setItem("B01_1CCC", "");
        localStorage.setItem("B01_1DDD", "");
    }

}
function Buttonsave_Click() {
    let IsSave = true;
    let dgvcount = BaseParameter.ListB01.length;
    if (dgvcount <= 0) {
        IsSave = false;
    }
    if (IsSave == true) {
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B01/Buttonsave_Click";
        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResult = data;
                if (BaseResult) {
                    BaseParameter.ListB01 = BaseResult.ListB01;
                    alert(BaseResult.SaveSuccessNumber + " Save database complete / Error " + BaseResult.SaveNotSuccessNumber);
                    Buttondelete_Click();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });

    }
}
function Buttondelete_Click() {
    let i = 0;
    while (i < BaseParameter.ListB01.length) {
        if (BaseParameter.ListB01[i].dgvcheck == true) {
            BaseParameter.ListB01.splice(i, 1);
        }
        else {
            i++;
        }
    }
    DataGridViewRender();
}
function Buttoncancel_Click() {
    $("#TextBox2").val("");
    $("#TextBox3").val("");
    $("#TextBox4").val("");
    $("#TextBox5").val("");
    $("#TextBox6").val("");
    $("#TextBox7").val("");
    $("#TextBox8").val("");
    $("#TextBox9").val("");
    $("#TextBox10").val("");
}
function Buttoninport_Click() {

    $("#BackGround").css("display", "block");
    let BaseParameterImport = new Object();
    BaseParameterImport = {
        SearchString: $("#Datelist").val(),
    }
    var FileToUpload = $('#FileToUpload').prop('files');
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameterImport));
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                formUpload.append('file[]', FileToUpload[i]);
            }
        }
    }
    let url = "/B01/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResult = data;
            if (BaseResult) {
                if (BaseResult.ErrorNumber > 0) {
                    alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                }
                BaseParameter.ListB01 = BaseResult.ListB01;
                DataGridViewRender();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    TableHTMLToExcel("DataGridViewTable", "B01", "B01")
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B01/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
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
$("#TextBox2").click(function () {
    let url = "/B01_1";
    OpenWindowByURL(url, 800, 400);
    StartInterval();
});
$("#rbchk1").click(function () {
    for (let i = 0; i < BaseParameter.ListB01.length; i++) {
        BaseParameter.ListB01[i].dgvcheck = true;
    }
    DataGridViewRender();
});
$("#rbchk2").click(function () {
    for (let i = 0; i < BaseParameter.ListB01.length; i++) {
        BaseParameter.ListB01[i].dgvcheck = !BaseParameter.ListB01[i].dgvcheck;
    }
    DataGridViewRender();
});
function DataGridViewRender() {
    let HTML = "";
    if (BaseParameter) {
        if (BaseParameter.ListB01) {
            if (BaseParameter.ListB01.length > 0) {
                for (let i = 0; i < BaseParameter.ListB01.length; i++) {
                    if (BaseParameter.ListB01[i].dgvcheck) {
                        HTML = HTML + "<tr>";
                    }
                    else {
                        HTML = HTML + "<tr style='background-color: red;'>";
                    }
                    if (BaseParameter.ListB01[i].dgvcheck) {
                        HTML = HTML + "<td><label><input id='DataGridViewCHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridViewCHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridViewCHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridViewCHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].UNIT + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].GOODS + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].QUANTITY + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].NWEIGHT + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].GWEIGHT + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].PalletNo + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].ShippedNo + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].inputdate + "</td>";
                    HTML = HTML + "<td>" + BaseParameter.ListB01[i].Remark + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView").innerHTML = HTML;
}
function DataGridViewSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseParameter.ListB01, IsTableSort);
    DataGridViewRender();
}
function DataGridViewCHKChanged(index) {
    let id = "DataGridViewCHK" + index;
    for (let i = 0; i < BaseParameter.ListB01.length; i++) {
        if (i == index) {
            BaseParameter.ListB01[i].dgvcheck = $("#" + id).is(":checked");
        }
    }
}