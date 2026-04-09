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
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    DateNow = today;
    $("#DateTimePicker3").val(DateNow);
    $("#DateTimePicker1").val(DateNow);

    let DateTimePicker3 = $("#DateTimePicker3").val();
    let DateTimePicker3Date = new Date(DateTimePicker3);
    let WeekNumber = GetWeekNumberByDate(DateTimePicker3Date);
    WeekNumber = WeekNumber - 1;
    $("#Label5").val(WeekNumber);

    document.getElementById("CheckBox1").checked = true;

    $("#ComboBox2").val(SettingsFNMName);

    DateTimePicker1_ValueChanged();

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
function COM_LIST2() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {

    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C01/COM_LIST2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#ComboBox3").empty();
            let BaseResultCOM_LIST2 = data;
            BaseResult.ComboBox3 = BaseResultCOM_LIST2.ComboBox3;
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C01/COM_LIST";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#ComboBox1").empty();
            let BaseResultCOM_LIST = data;
            BaseResult.ComboBox1 = BaseResultCOM_LIST.ComboBox1;
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
$("#ComboBox2").change(function (e) {
    ComboBox2_SelectedIndexChanged();
});
function ComboBox2_SelectedIndexChanged() {
    SettingsFNMName = $("#ComboBox2").val();
    switch (SettingsFNMName) {
        case "Factory 1":
            SettingsFNM = 0;
            break;
        case "Factory 2":
            SettingsFNM = 1;
            break;
    }
}
$("#DateTimePicker3").change(function (e) {
    DateTimePicker3_ValueChanged();
});
function DateTimePicker3_ValueChanged() {
    let DateTimePicker3 = $("#DateTimePicker3").val();
    let DateTimePicker3Date = new Date(DateTimePicker3);
    let WeekNumber = GetWeekNumberByDate(DateTimePicker3Date);
    WeekNumber = WeekNumber - 1;
    $("#Label5").val(WeekNumber);
}

$("#DateTimePicker1").change(function (e) {
    DateTimePicker1_ValueChanged();
});
function DateTimePicker1_ValueChanged() {
    COM_LIST();
}
$("#Buttonfind").click(function () {
    Buttonfind_Click(0);
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
function Buttonfind_Click(Flag) {
    if (TagIndex == 3) {
        let AA = $("#DateTimePicker1").val();
        let BB = $("#ComboBox1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(AA);
        BaseParameter.ListSearchString.push(BB);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                if (Flag == 0) {
                    let BaseResultButtonfind = data;
                    BaseResult.DataGridView3 = BaseResultButtonfind.DataGridView3;
                    DataGridView3Render();
                    $("#Label11").val(BaseResult.DataGridView3[0].FCTRY_NM);
                    $("#BackGround").css("display", "none");
                }
                if (Flag == 1) {
                    Buttonsave_ClickSub();
                }
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 2) {
        let AA = $("#ComboBox3").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: AA,
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView2;
                DataGridView2Render();
                $("#Label10").val(BaseResult.DataGridView2[0].FCTRY_NM);
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
        let ComboBox2 = $("#ComboBox2").val();
        if (ComboBox2 == "") {
            IsSave = false;
            alert("Select Factory Name.");
        }
        if (IsSave == true) {
            let Label5 = $("#Label5").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.ListSearchString.push(ComboBox2);
            BaseParameter.ListSearchString.push(Label5);
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
            BaseParameter.DataGridView1 = BaseResult.DataGridView1;            
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C01_MES/Buttonsave_Click";
            let url = "/C01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonsave = data;
                    if (BaseResultButtonsave.ErrorNumber <= 0) {
                        alert("Can't save. (Check EXCEL). Không lưu được.(Check EXCEL)");
                    }
                    else {
                        alert(localStorage.getItem("SaveSuccess"));
                        BaseResult.DataGridView1 = [];
                        DataGridView1Render();
                    }
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
        if (BaseResult.DataGridView2.length <= 0) {
            IsSave = false;
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView2 = BaseResult.DataGridView2;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C01_MES/Buttonsave_Click";
            let url = "/C01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonsave = data;
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click(0);
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 3) {
        Buttonfind_Click(1);
    }
}
function Buttonsave_ClickSub() {    
    if (TagIndex == 3) {
        let IsSave = true;
        if (BaseResult.DataGridView3.length <= 0) {
            IsSave = false;
        }
        document.getElementById("Buttonsave").disabled = true;
        if (IsSave == true) {
            let CR_DATE = $("#ComboBox1").val();
            let DateTimePicker1 = $("#DateTimePicker1").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(CR_DATE);
            BaseParameter.ListSearchString.push(DateTimePicker1);
            BaseParameter.DataGridView3 = BaseResult.DataGridView3;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            //let url = APIUpload + "/C01_MES/Buttonsave_Click";
            let url = "/C01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonsave = data;
                    alert(localStorage.getItem("SaveSuccess"));
                    BaseResult.DataGridView2 = [];
                    DataGridView2Render();
                    BaseResult.DataGridView1 = [];
                    DataGridView1Render();
                    BaseResult.DataGridView3 = BaseResultButtonsave.DataGridView3;
                    DataGridView3Render();
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
    if (TagIndex == 3) {
        let IsDelete = true;
        let ComboBox1 = $("#ComboBox1").val();
        if (ComboBox1 == true) {
            IsDelete = false;
            alert("Select  CREATE DATE.");
        }
        if (BaseResult.DataGridView3.length <= 0) {
            IsDelete = false;
        }
        if (IsDelete == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                SearchString: ComboBox1,
            }
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C01/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("DeleteSuccess"));
                    COM_LIST();
                    BaseResult.DataGridView3 = [];
                    DataGridView3Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 3) {
        BaseResult.DataGridView3 = [];
        DataGridView3Render();
        document.getElementById("Buttonsave").disabled = false;
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
function Buttoninport_ClickSub() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
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
    let url = "/C01/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultButtoninport = data;
            BaseResult.DataGridView1 = BaseResultButtoninport.DataGridView1;
            DataGridView1Render();
            alert("Completed. Inport to Excel");
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    if (TagIndex == 3) {
        TableHTMLToExcel("DataGridView3Table", "C01_OrderConfirmation", "C01_OrderConfirmation");
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
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView1CHKChanged(" + i + ")'><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ECNNo + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CUR_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CT_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CT_LEADS_PR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PRT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";

                    let SP_ST = BaseResult.DataGridView1[i].SP_ST;
                    if (SP_ST) {
                        if (SP_ST.length > 50) {
                            SP_ST = SP_ST.substring(0, 40) + "...";
                        }
                    }

                    HTML = HTML + "<td>" + SP_ST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REP + "</td>";
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
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = !BaseResult.DataGridView1[DataGridView1RowIndex].CHK;
    DataGridView1Render();
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView2[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input id='DataGridView2CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input id='DataGridView2CHK" + i + "'' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].OR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].FCTRY_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PROJECT + "</td>";
                    HTML = HTML + "<td></td>";
                    HTML = HTML + "<td></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].USED_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MES_ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CUR_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CT_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CT_LEADS_PR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PRT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView2[i].SP_ST) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].REP + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STCK_QTY1 + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STCK_QTY2 + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BF_USE_PO1 + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView2[i].USED + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ORDER_IDX + "</td>";
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
}
function DataGridView2CHKChanged(i) {
    DataGridView2RowIndex = i;
    BaseResult.DataGridView2[DataGridView2RowIndex].CHK = !BaseResult.DataGridView2[DataGridView2RowIndex].CHK;
    DataGridView2Render();
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Description + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].OR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].FCTRY_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TORDER_FG + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TOT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].USED_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].MES_ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CUR_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CT_LEADS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CT_LEADS_PR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PRT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView3[i].SP_ST) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].REP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].ORDER_IDX + "</td>";
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
function DataGridView3CHKChanged(i) {
    DataGridView3RowIndex = i;
    BaseResult.DataGridView3[DataGridView3RowIndex].CHK = !BaseResult.DataGridView3[DataGridView3RowIndex].CHK;
    DataGridView3Render();
}

