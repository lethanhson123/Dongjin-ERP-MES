let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let DataGridView5RowIndex = 0;
let DGV_OUTRowIndex = 0;


$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    DateNow = today;

    $("#DateTimePicker3").val(DateNow);
    PO_LIST_CB();
    document.getElementById("Button2").disabled = true;

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    BaseResult.DataGridView3 = new Object();
    BaseResult.DataGridView3 = [];
    BaseResult.DataGridView4 = new Object();
    BaseResult.DataGridView4 = [];
    BaseResult.DataGridView5 = new Object();
    BaseResult.DataGridView5 = [];
    BaseResult.DGV_OUT = new Object();
    BaseResult.DGV_OUT = [];

});
function PO_LIST_CB() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D07/PO_LIST_CB";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultPO_LIST_CB = data;
            BaseResult.ComboBox2 = BaseResultPO_LIST_CB.ComboBox2;
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].PO_CODE;
                option.value = BaseResult.ComboBox2[i].PO_CODE;

                var ComboBox2 = document.getElementById("ComboBox2");
                ComboBox2.add(option);

                option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].PO_CODE;
                option.value = BaseResult.ComboBox2[i].PO_CODE;

                var ComboBox4 = document.getElementById("ComboBox4");
                ComboBox4.add(option);
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#ATag001").click(function (e) {
    TagIndex = 1;
    PO_LIST_CB();
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
    PO_LIST_CB();
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    PO_LIST_CB();
    document.getElementById("Button2").disabled = true;
});

$("#ComboBox4").change(function (e) {
    ComboBox4_SelectedIndexChanged();
});
function ComboBox4_SelectedIndexChanged() {
    $("#TextBox7").val("");
    Buttonfind_Click();
}
$("#Button3").click(function (e) {
    Button3_Click();
});
function Button3_Click() {
    let IsCheck = true;
    let ComboBox4 = $("#ComboBox4").val();
    if (ComboBox4 == "") {
        alert("Phải nhập Customer PO(Need Customer PO).  Please Check Again.");
        IsCheck = false;
    }
    let TextBox7 = $("#TextBox7").val();
    if (TextBox7 == "") {
        alert("Ship NO Không thể.   Please Check Again.");
        IsCheck = false;
    }
    let TextBox5 = $("#TextBox5").val();
    if (TextBox5 == "") {
        alert("PO CODE Không thể.   Please Check Again.");
        IsCheck = false;
    }
    let TextBox2 = $("#TextBox2").val();
    if (TextBox2 == "") {
        alert("Part NO Không thể.   Please Check Again.");
        IsCheck = false;
    }
    let TextBox3 = $("#TextBox3").val();
    if (TextBox3 == "") {
        alert("Stage Không thể.   Please Check Again.");
        IsCheck = false;
    }
    if (BaseResult.DataGridView1.length <= 0) {
        alert("Not Data PO.  Please Check Again.");
        IsCheck = false;
    }
    if (IsCheck == true) {
        let PO_TXT = $("#ComboBox4").val();
        let POCNT = $("#TextBox7").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(PO_TXT);
        BaseParameter.ListSearchString.push(POCNT);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D07/Button3_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                $("#TextBox7").val("");
                Buttonfind_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}

$("#ComboBox1").change(function (e) {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    CB_CHENG();
}
$("#ComboBox3").change(function (e) {
    ComboBox3_SelectedIndexChanged();
});
function ComboBox3_SelectedIndexChanged() {
    CB_CHENG();
}
$("#DateTimePicker3").change(function (e) {
    DateTimePicker3_ValueChanged();
});
function DateTimePicker3_ValueChanged() {
    CB_CHENG();
}
function CB_CHENG() {
    let IsCheck = true;
    let AAAA = "";
    let BBBB = "";
    let ComboBox1 = document.getElementById("ComboBox1").selectedIndex;
    let ComboBox3 = document.getElementById("ComboBox3").selectedIndex;
    if (ComboBox1 == "0") {
        IsCheck = false;
    }
    if (ComboBox3 == "0") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let TEXT = $("#ComboBox1").val();
        let CUSM = $("#ComboBox3").val();
        let DateTimePicker3 = $("#DateTimePicker3").val();
        let DateTimePicker3Date = new Date(DateTimePicker3.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
        DateTimePicker3 = DateTimePicker3Date.toLocaleDateString('en-GB').split('/').reverse().join('');
        DateTimePicker3 = DateTimePicker3.slice(2);
        AAAA = TEXT.substring(0, 3).toUpperCase();
        BBBB = "-" + CUSM.substring(0, 1).toUpperCase();
        let PO_CODE = AAAA + BBBB + DateTimePicker3 + "-M";
        $("#PO_CODE").val(PO_CODE);
    }
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let PO_CODE = $("#PO_CODE").val();
    if (PO_CODE.length <= 5) {
        IsCheck = false;
        alert("PO_CODE is null. Please Check Again.");
    }
    if (IsCheck == true) {
        BaseResult.DataGridView5 = [];
        DataGridView5Render();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: PO_CODE,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView4 = BaseResult.DataGridView4;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D07/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButton1 = data
                BaseResult.DataGridView5 = BaseResultButton1.DataGridView5;
                DataGridView5Render();
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    if (BaseResult.DataGridView4[i].CHK == true) {
                        BaseResult.DataGridView4[i].CNF = "Complete";
                    }
                }
                DataGridView4Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#rbchk1").click(function () {
    Rbchk1_Click();
});
$("#rbchk2").click(function () {
    Rbchk1_Click();
});
function Rbchk1_Click() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            BaseResult.DataGridView4[i].CHK = true;
        }
        DataGridView4Render();
    }
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            BaseResult.DataGridView4[i].CHK = !BaseResult.DataGridView4[i].CHK;
        }
        DataGridView4Render();
    }
}
$("#ComboBox2").change(function (e) {
    ComboBox2_SelectedIndexChanged();
});
function ComboBox2_SelectedIndexChanged() {
    Buttonfind_Click();
}
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    let P_CHTEXT = $("#TextBox1").val();
    for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
        let PART_NO = BaseResult.DGV_OUT[i].PART_NO;
        if (P_CHTEXT == PART_NO) {
            i = BaseResult.DGV_OUT.length;
            DGV_OUT_SelectionChanged(i);
        }
    }
}
$("#RadioButton1").click(function () {
    RadioButton2_Click();
});
$("#RadioButton2").click(function () {
    RadioButton2_Click();
});
function RadioButton2_Click() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton2 == true) {
        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            BaseResult.DataGridView5[i].CHK = true;
        }
        DataGridView5Render();
    }
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            BaseResult.DataGridView5[i].CHK = !BaseResult.DataGridView5[i].CHK;
        }
        DataGridView5Render();
    }
}
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    if (confirm("Would you like to create the next PO? Sẽ thực hiện tạo ra PO sau không?")) {
        let IsCheck = true;
        if (BaseResult.DGV_OUT.length <= 0) {
            IsCheck = false;
        }
        if (IsCheck == true) {
            document.getElementById("Button2").disabled = true;
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                SearchString: BaseResult.DataGridView2[DataGridView2RowIndex].PO_CODE,
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView2 = BaseResult.DataGridView2;
            BaseParameter.DGV_OUT = BaseResult.DGV_OUT;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D07/Button2_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    document.getElementById("Button2").disabled = true;
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
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
    if (TagIndex == 1) {
        let AAA = $("#ComboBox4").val();
        let BBB = $("#TextBox5").val();
        let CCC = $("#TextBox2").val();
        let DDD = $("#TextBox3").val();
        let EEE = $("#TextBox4").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(EEE);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D07/Buttonfind_Click";

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
    if (TagIndex == 3) {
        SUB_POCODE();
    }
}
function SUB_POCODE() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#ComboBox2").val(),
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D07/SUB_POCODE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSUB_POCODE = data;
            BaseResult.DataGridView2 = BaseResultSUB_POCODE.DataGridView2;
            DataGridView2Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {
    if (confirm("Would you like to save? Bạn có lưu không?")) {
        if (TagIndex == 2) {
            document.getElementById("Buttonsave").disabled = true;
            let PO_COUNT = $("#TextBox6").val();
            let PO_CODE = $("#PO_CODE").val();
            let DateTimePicker3 = $("#DateTimePicker3").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView5 = BaseResult.DataGridView5;           
            BaseParameter.ListSearchString.push(PO_COUNT);
            BaseParameter.ListSearchString.push(PO_CODE);
            BaseParameter.ListSearchString.push(DateTimePicker3);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D07/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    BaseResult.DataGridView4 = [];
                    DataGridView4Render();
                    BaseResult.DataGridView5 = [];
                    DataGridView5Render();
                    document.getElementById("Buttonsave").disabled = false;
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    document.getElementById("Buttonsave").disabled = false;
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
        if (TagIndex == 3) {
            let IsSave = true;
            if (BaseResult.DGV_OUT.length <= 0) {
                IsSave = false;
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
                BaseParameter.DGV_OUT = BaseResult.DGV_OUT;
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/D07/Buttonsave_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        SUB_DATE();
                        alert(localStorage.getItem("SaveSuccess"));
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        document.getElementById("Buttonsave").disabled = false;
                        alert(localStorage.getItem("ERROR"));
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {

}

function Buttoninport_Click() {
    if (TagIndex == 2) {
        $("#FileToUpload").click();
    }
}
$("#FileToUpload").change(function () {
    Buttoninport_ClickSub();
});
function Buttoninport_ClickSub() {
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        var FileToUpload = $('#FileToUpload').prop('files');
        if (FileToUpload) {
            if (FileToUpload.length > 0) {
                for (var i = 0; i < FileToUpload.length; i++) {
                    formUpload.append('file[]', FileToUpload[i]);
                }
            }
        }
        let url = "/D07/Buttoninport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtoninport = data;
                BaseResult.DataGridView4 = BaseResultButtoninport.DataGridView4;
                DataGridView4Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonexport_Click() {

}
function Buttonprint_Click() {

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
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TDD_POCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_IDX_SNP + "</td>";
                    HTML = HTML + "<td>" + new Date(BaseResult.DataGridView1[i].TDD_PP_DT.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TDD_PP_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TDD_PP_NTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PACK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TDD_REMARK + "</td>";
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
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_CODE + "</td>";
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
    SUB_DATE();
}
function DataGridView2_CellClick() {
    SUB_DATE();
}
function SUB_DATE() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: BaseResult.DataGridView2[DataGridView2RowIndex].PO_CODE,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D07/SUB_DATE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSUB_DATE = data;
            BaseResult.DGV_OUT = BaseResultSUB_DATE.DGV_OUT;
            DGV_OUTRender();

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

    let NO_CHK = BaseResult.DataGridView2[DataGridView2RowIndex].NO;
    let DG2_CHK = BaseResult.DataGridView2.length;
    if (NO_CHK == DG2_CHK) {
        document.getElementById("Button2").disabled = false;
    }
    else {
        document.getElementById("Button2").disabled = true;
    }
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";

                    HTML = HTML + "</tr>";
                }
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
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
}
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                DataGridView4_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView4[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView4CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView4CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].NEXT_PO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].CNF + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}
function DataGridView4Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView4, IsTableSort);
    DataGridView4Render();
}
function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}
function DataGridView4CHKChanged(i) {
    DataGridView4RowIndex = i;
    BaseResult.DataGridView4[DataGridView4RowIndex].CHK = !BaseResult.DataGridView4[DataGridView4RowIndex].CHK;
    DataGridView4Render();
}
function DataGridView5Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView5) {
            if (BaseResult.DataGridView5.length > 0) {
                DataGridView5_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView5.length; i++) {        
                    if (BaseResult.DataGridView5[i].PART_IDX > 0) {
                        HTML = HTML + "<tr onclick='DataGridView5_SelectionChanged(" + i + ")'>";
                    }
                    else {
                        HTML = HTML + "<tr style='background-color: red;' onclick='DataGridView5_SelectionChanged(" + i + ")'>";
                    }
                    if (BaseResult.DataGridView5[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView5CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView5CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].NT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].Inventory + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PO_status + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PART_USENY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].MAX_PO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PACK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PDOTPL_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView5").innerHTML = HTML;
}
function DataGridView5Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView5, IsTableSort);
    DataGridView5Render();
}
function DataGridView5_SelectionChanged(i) {
    DataGridView5RowIndex = i;
}
function DataGridView5CHKChanged(i) {
    DataGridView5RowIndex = i;
    BaseResult.DataGridView5[DataGridView5RowIndex].CHK = !BaseResult.DataGridView5[DataGridView5RowIndex].CHK;
    DataGridView5Render();
}
function DGV_OUTRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_OUT) {
            if (BaseResult.DGV_OUT.length > 0) {
                DGV_OUT_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
                    HTML = HTML + "<tr onclick='DGV_OUT_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DGV_OUT[i].CHK) {
                        HTML = HTML + "<td onclick='DGV_OUTCHKChanged(" + i + ")'><input id='DGV_OUTCHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DGV_OUTCHKChanged(" + i + ")'><input id='DGV_OUTCHK" + i + "' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PART_SNP + "</td>";
                    HTML = HTML + "<td><input onblur='DGV_OUTPO_QTYChange(" + i + ", this)' id='DGV_OUTPO_QTY" + i + "' type='number' class='form-control' style='width: 100px;' value='" + BaseResult.DGV_OUT[i].PO_QTY + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].NT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_OUT[i].PACK_QTY + "</td>";
                    HTML = HTML + "<td><input onblur='DGV_OUTNEXT_POChange(" + i + ", this)' id='DGV_OUTNEXT_PO" + i + "' type='number' class='form-control' style='width: 100px; color: orange;' value='" + BaseResult.DGV_OUT[i].NEXT_PO + "'></td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_OUT").innerHTML = HTML;
}
function DGV_OUTSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DGV_OUT, IsTableSort);
    DGV_OUTRender();
}
function DGV_OUT_SelectionChanged(i) {
    DGV_OUTRowIndex = i;
}
function DGV_OUTCHKChanged(i) {
    DGV_OUTRowIndex = i;
    BaseResult.DGV_OUT[DGV_OUTRowIndex].CHK = !BaseResult.DGV_OUT[DGV_OUTRowIndex].CHK;
    DGV_OUTRender();
}
function DGV_OUTPO_QTYChange(i, input) {
    DGV_OUTRowIndex = i;
    BaseResult.DGV_OUT[DGV_OUTRowIndex].PO_QTY = input.value;
}
function DGV_OUTNEXT_POChange(i, input) {
    DGV_OUTRowIndex = i;
    BaseResult.DGV_OUT[DGV_OUTRowIndex].NEXT_PO = input.value;
    if (BaseResult.DGV_OUT[DGV_OUTRowIndex].PO_QTY == BaseResult.DGV_OUT[DGV_OUTRowIndex].NEXT_PO) {

    }
    else {
        BaseResult.DGV_OUT[DGV_OUTRowIndex].CHK = true;
        DGV_OUTRender();
    }
}
function DGV_OUT_CellBeginEdit() {

}
function DGV_OUT_CellEndEdit() {

}
function dgvPOFWH3ReceiveSub_EditingControlShowing() {

}
function DGV_OUT_txtEdit_Keypress() {

}