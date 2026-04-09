let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let APIUpload;

$(document).ready(function () {
    APIUpload = decodeURIComponent(GetCookieValue("APIUpload"));
    var now = new Date();
    DateNow = DateToString(now);
    $("#DateTimePicker3").val(DateNow);
    $("#DateTimePicker1").val(DateNow);
    
    let Label5 = GetWeekNumberByDate(now) - 1;
    $("#Label5").val(Label5);
    document.getElementById("CheckBox1").checked = true;

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    BaseResult.DataGridView3 = new Object();
    BaseResult.DataGridView3 = [];

});

$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
    COM_LIST2();
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    COM_LIST();
});
function COM_LIST() {
    let ORDER_DT = $("#DateTimePicker1").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: ORDER_DT,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C06/COM_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.ComboBox1 = data.ComboBox1;
            $("#ComboBox1").empty();                        
            for (let i = 0; i < BaseResult.ComboBox1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox1[i].Description;
                option.value = BaseResult.ComboBox1[i].Description;

                var ComboBox1 = document.getElementById("ComboBox1");
                ComboBox1.add(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function COM_LIST2() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C06/COM_LIST2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.ComboBox3 = data.ComboBox3;
            $("#ComboBox3").empty();                        
            for (let i = 0; i < BaseResult.ComboBox3.length; i++) {                
                var option = document.createElement("option");
                option.text = BaseResult.ComboBox3[i].Description;
                option.value = BaseResult.ComboBox3[i].Description;

                var ComboBox3 = document.getElementById("ComboBox3");
                ComboBox3.add(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#DateTimePicker3").change(function (e) {
    DateTimePicker3_ValueChanged();
});
function DateTimePicker3_ValueChanged() {
    var date = new Date($("#DateTimePicker3").val());
    let Label5 = GetWeekNumberByDate(date);
    $("#Label5").val(Label5);
}
$("#DateTimePicker1").change(function (e) {
    DateTimePicker1_ValueChanged();
});
function DateTimePicker1_ValueChanged() {
    COM_LIST();
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
    }
    if (TagIndex == 2) {
        let AA = $("#ComboBox3").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: AA,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C06/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {                
                BaseResult.DataGridView3 = data.DataGridView3;
                DataGridView3Render();
                try {
                    $("#Label10").val(BaseResult.DataGridView3[0].MC);
                }
                catch (e) {
                    $("#Label10").val("-");
                }                               
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 3) {
        let AA = $("#ComboBox1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: AA,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C06/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView2 = BaseResultSub.DataGridView2;
                DataGridView2Render();
                $("#Label11").val(BaseResult.DataGridView2[0].FCTRY_NM);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = true;
        if (BaseResult.DataGridView1.length <= 0) {
            IsSave = false;
        }
        if (IsSave == true) {
            let DateTimePicker3 = $("#DateTimePicker3").val();
            let FACTORY_NM = $("#ComboBox2").val();
            let Label5 = $("#Label5").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView1 = BaseResult.DataGridView1;
            BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
            BaseParameter.ListSearchString.push(DateTimePicker3);
            BaseParameter.ListSearchString.push(FACTORY_NM);
            BaseParameter.ListSearchString.push(Label5);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C06_MES/Buttonsave_Click";
            let url = "/C06/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    BaseResult.DataGridView1 = [];
                    DataGridView1Render();
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 2) {
        let IsSave = true;
        if (BaseResult.DataGridView3.length <= 0) {
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
            BaseParameter.DataGridView3 = BaseResult.DataGridView3;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C06_MES/Buttonsave_Click";
            let url = "/C06/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    DataGridView3Render();                    
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 3) {
        let IsSave = true;
        if (BaseResult.DataGridView2.length <= 0) {
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
            BaseParameter.DataGridView2 = BaseResult.DataGridView2;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C06_MES/Buttonsave_Click";
            let url = "/C06/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttondelete_Click() {
    if (TagIndex == 3) {
        let IsCheck = true;
        let ComboBox1 = $("#ComboBox1").val();
        if (ComboBox1 == "") {
            IsCheck = false;
            alert("Select  CREATE DATE.");
        }
        if (BaseResult.DataGridView2.length <= 0) {
            IsCheck = false;
        }
        if (IsCheck == true) {
            let DEL_AA = $("#ComboBox1").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                SearchString: DEL_AA,
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C06/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    COM_LIST();
                    BaseResult.DataGridView2 = [];
                    DataGridView2Render();
                    alert("Completed. Delete data");
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }       
    }
}
function Buttoncancel_Click() {
    BaseResult.DataGridView2 = [];
    DataGridView2Render();
    document.getElementById("Buttonsave").disabled = false;
}
function Buttoninport_ClickSub() {
    if (TagIndex == 1) {
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
        let url = "/C06/Buttoninport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {                
                BaseResult.DataGridView1 = data.DataGridView1;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttoninport_Click() {
    if (TagIndex == 1) {
        $("#FileToUpload").click();
    }
}
$("#FileToUpload").change(function () {
    Buttoninport_ClickSub();
});
function Buttonexport_Click() {
    if (TagIndex == 3) {
        TableHTMLToExcel("DataGridView2Table", "C06_OrderConfirmation", "C06_OrderConfirmation");
    }
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
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<tr style='background-color: orangered;' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ASSY_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ORDER_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SAFETY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Machine + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REP + "</td>";
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
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "ASSY_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView2[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].OR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].FCTRY_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].USED_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MES_ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].REP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ORDER_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
function DataGridView2CHKChanged(i) {
    DataGridView2RowIndex = i;
    BaseResult.DataGridView2[DataGridView2RowIndex].CHK = !BaseResult.DataGridView2[DataGridView2RowIndex].CHK;
    DataGridView2Render();
}
let DataGridView2Table = document.getElementById("DataGridView2Table");
DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "ASSY_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
        DataGridView2Render();
    }
});
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                //DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView3[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView3CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView3CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].OR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].FCTRY_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PO_QTY + "</td>";
                    HTML = HTML + "<td><input onblur='DataGridView3_CellEndEdit(" + i + ", this)' id='DataGridView3SAFTY_QTY" + i + "' type='number' class='form-control' style='width: 100px; color: red;' value='" + BaseResult.DataGridView3[i].SAFTY_QTY + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].USED_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].MES_ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].REP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].ORDER_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;  
}
function DataGridView3_CellEndEdit(i, input) {    
    DataGridView3RowIndex = i;    
    BaseResult.DataGridView3[DataGridView3RowIndex].SAFTY_QTY = input.value;    
    if (BaseResult.DataGridView3[DataGridView3RowIndex].OR_NO == "NORMAL") {
        let AAII = (BaseResult.DataGridView3[DataGridView3RowIndex].PO_QTY + BaseResult.DataGridView3[DataGridView3RowIndex].SAFTY_QTY + BaseResult.DataGridView3[DataGridView3RowIndex].USED_STOCK) / BaseResult.DataGridView3[DataGridView3RowIndex].BUNDLE_SIZE;
        let MES_OD = AAII * BaseResult.DataGridView3[DataGridView3RowIndex].BUNDLE_SIZE;
        if (MES_OD <= 0) {
            MES_OD = 0;
        }
        else {
            MES_OD = MES_OD;
        }
        BaseResult.DataGridView3[DataGridView3RowIndex].MES_ORDER = MES_OD;
        BaseResult.DataGridView3[DataGridView3RowIndex].CHK = true;
    }
    else {
        BaseResult.DataGridView3[DataGridView3RowIndex].MES_ORDER = BaseResult.DataGridView3[DataGridView3RowIndex].PO_QTY;
        BaseResult.DataGridView3[DataGridView3RowIndex].CHK = true;
    }    
    DataGridView3Render();
}
function DataGridView3CHKChanged(i) {
    DataGridView3RowIndex = i;
    BaseResult.DataGridView3[DataGridView3RowIndex].CHK = !BaseResult.DataGridView3[DataGridView3RowIndex].CHK;
    DataGridView3Render();
}
let DataGridView3Table = document.getElementById("DataGridView3Table");
DataGridView3Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "ASSY_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView3, key, text, IsTableSort);
        DataGridView3Render();
    }
});
function DataGridView3_CellEndEdit_1() {

}