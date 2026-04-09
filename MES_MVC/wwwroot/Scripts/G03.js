let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let Now;
let DataGridView1RowIndex = 0;
let DataGridView3RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
});

$("#ATag001").click(function (e) {
    TagIndex = 1;
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
function Buttonfind_Click() {
    if (TagIndex == 1) {
        DGV3_LOAD();
        $("#BackGround").css("display", "block");
        let TextBox4 = $("#TextBox4").val();
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: TextBox4,
        }
        BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G03/Buttonfind_Click";

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
    if (TagIndex == 2) {
    }
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.DataGridView3 = BaseResult.DataGridView3;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G03/Buttonsave_Click";

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
                alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {
}
function Buttoncancel_Click() {
}
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G03/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            DGV3_LOAD();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
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
function DGV3_LOAD() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }  
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G03/DGV3_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultDGV3_LOAD = data;
            BaseResult.DataGridView3 = BaseResultDGV3_LOAD.DataGridView3;
            DataGridView3Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {           
            $("#BackGround").css("display", "none");
        })
    });
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    let LV = BaseResult.DataGridView1[i].LEVEL;
                    switch (LV) {
                        case "0":
                            HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' style='background-color: greenyellow;'>";
                            break;
                        case "1":
                            HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' style='background-color: lightyellow;'>";
                            break;
                    }
                    
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].FG_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEVEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].S_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOM_GRUP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].B_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].EONO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MOQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STOCK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].GAP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DSYN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].USED + "</td>";                    
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
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_ENCNO + "</td>";
                    HTML = HTML + "<td><input id='txtEdit' type='number' class='form-control' style='width: 100px;' value='" + BaseResult.DataGridView3[i].PO_QTY + "'></td>";
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
    let P_CHK = BaseResult.DataGridView3[DataGridView3RowIndex].PART_NO;
    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
        let PP_CHK = BaseResult.DataGridView1[i].B_PART_NO;
        if (PP_CHK == P_CHK) {
            DataGridView1_SelectionChanged(i);
        }
    }
}
