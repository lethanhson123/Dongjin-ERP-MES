let IsTableSort = false;
let BaseResult = {};
let TagIndex = 1;
let SQLCHK = 0;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView0RowIndex = 0;
let IntervalTimer;

$(document).ready(function () {
    $('ul.tabs').tabs();
    $('#excelModal').modal();

    Buttonfind_Click();

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
$("#ATag004").click(function (e) {
    TagIndex = 4;
});

$("#suchTB1").keydown(function (e) {
    if (e.keyCode === 13) { SuchTB1_KeyDown() }
});
function SuchTB1_KeyDown() {
    Buttonfind_Click();
}
$("#suchTB2").keydown(function (e) {
    if (e.keyCode === 13) { SuchTB2_KeyDown(); }
});
function SuchTB2_KeyDown() {
    Buttonfind_Click();
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
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        Action: TagIndex,
        ListSearchString: []
    };

    if (TagIndex === 1) {
        BaseParameter.ListSearchString.push($("#suchTB1").val());
        BaseParameter.ListSearchString.push($("#suchTB2").val());
        BaseParameter.CheckBox1 = true;
    } else if (TagIndex === 2) {
        BaseParameter.ListSearchString.push($("#SUCHA1").val());
        BaseParameter.ListSearchString.push($("#SUCHA2").val());
        BaseParameter.ListSearchString.push($("#SUCHA3").val());
    } else if (TagIndex === 3) {
        BaseParameter.ListSearchString.push($("#SUCHB1").val());
        BaseParameter.ListSearchString.push($("#SUCHB2").val());
        BaseParameter.ListSearchString.push($("#SUCHB3").val());
    } else if (TagIndex === 4) {
        BaseParameter.ListSearchString.push($("#SUCHC1").val());
        BaseParameter.ListSearchString.push($("#SUCHC2").val());
        BaseParameter.ListSearchString.push($("#SUCHC3").val());
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G01/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultButtonfind = data;
            BaseResult = BaseResultButtonfind;

            if (TagIndex === 1) {
                DataGridView0Render();
            } else if (TagIndex === 2) {
                DataGridView1Render();
                $("#GOOD1").text(BaseResult.G01RawMaterialGood || 0);
                $("#BAD1").text(BaseResult.G01RawMaterialBad || 0);
            } else if (TagIndex === 3) {
                DataGridView2Render();
                $("#GOOD2").text(BaseResult.G01FinishedGoodGood || 0);
                $("#BAD2").text(BaseResult.G01FinishedGoodBad || 0);
            } else if (TagIndex === 4) {
                DataGridView3Render();
                $("#GOOD3").text(BaseResult.G01HookRackGood || 0);
                $("#BAD3").text(BaseResult.G01HookRackBad || 0);
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(BaseResult.Error || localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}
function Buttonadd_Click() {

}

function Buttonsave_Click() {
}

function Buttondelete_Click() {

}

function Buttoncancel_Click() {

}

function Buttoninport_Click() {
    if (TagIndex == 2) {
        localStorage.setItem("G01_1_LOC_INDEX", "1");
        let url = "/G01_1";
        OpenWindowByURL(url, 400, 400);
    }
    if (TagIndex == 3) {
        localStorage.setItem("G01_1_LOC_INDEX", "2");
        let url = "/G01_1";
        OpenWindowByURL(url, 400, 400);
    }
}

function Buttonexport_Click() {
    if (TagIndex == 1) {
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            Action: TagIndex,
            ListSearchString: []
        };

        if (TagIndex === 1) {
            BaseParameter.ListSearchString.push($("#suchTB1").val());
            BaseParameter.ListSearchString.push($("#suchTB2").val());
            BaseParameter.CheckBox1 = false;
        } else if (TagIndex === 2) {
            BaseParameter.ListSearchString.push($("#SUCHA1").val());
            BaseParameter.ListSearchString.push($("#SUCHA2").val());
            BaseParameter.ListSearchString.push($("#SUCHA3").val());
        } else if (TagIndex === 3) {
            BaseParameter.ListSearchString.push($("#SUCHB1").val());
            BaseParameter.ListSearchString.push($("#SUCHB2").val());
            BaseParameter.ListSearchString.push($("#SUCHB3").val());
        } else if (TagIndex === 4) {
            BaseParameter.ListSearchString.push($("#SUCHC1").val());
            BaseParameter.ListSearchString.push($("#SUCHC2").val());
            BaseParameter.ListSearchString.push($("#SUCHC3").val());
        }

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G01/Buttonexport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult = BaseResultButtonfind;

                if (TagIndex === 1) {
                    DataGridView0Render();
                    TableHTMLToExcel("DataGridView0Table", "G01GoodsInStock", "G01GoodsInStock")
                } else if (TagIndex === 2) {
                    DataGridView1Render();
                    $("#GOOD1").text(BaseResult.G01RawMaterialGood || 0);
                    $("#BAD1").text(BaseResult.G01RawMaterialBad || 0);
                } else if (TagIndex === 3) {
                    DataGridView2Render();
                    $("#GOOD2").text(BaseResult.G01FinishedGoodGood || 0);
                    $("#BAD2").text(BaseResult.G01FinishedGoodBad || 0);
                } else if (TagIndex === 4) {
                    DataGridView3Render();
                    $("#GOOD3").text(BaseResult.G01HookRackGood || 0);
                    $("#BAD3").text(BaseResult.G01HookRackBad || 0);
                }

                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(BaseResult.Error || localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });

    }
    if (TagIndex == 2) {
        TableHTMLToExcel("DataGridView1Table", "G01Warehouse", "G01Warehouse")
    }
    if (TagIndex == 3) {
        TableHTMLToExcel("DataGridView2Table", "G01FinishedGood", "G01FinishedGood")
    }
    if (TagIndex == 4) {
        $("#BackGround").css("display", "block");
        let BaseParameter = {
            Action: TagIndex,
            ListSearchString: []
        };

        if (TagIndex === 1) {
            BaseParameter.ListSearchString.push($("#suchTB1").val());
            BaseParameter.ListSearchString.push($("#suchTB2").val());
            BaseParameter.CheckBox1 = false;
        } else if (TagIndex === 2) {
            BaseParameter.ListSearchString.push($("#SUCHA1").val());
            BaseParameter.ListSearchString.push($("#SUCHA2").val());
            BaseParameter.ListSearchString.push($("#SUCHA3").val());
        } else if (TagIndex === 3) {
            BaseParameter.ListSearchString.push($("#SUCHB1").val());
            BaseParameter.ListSearchString.push($("#SUCHB2").val());
            BaseParameter.ListSearchString.push($("#SUCHB3").val());
        } else if (TagIndex === 4) {
            BaseParameter.ListSearchString.push($("#SUCHC1").val());
            BaseParameter.ListSearchString.push($("#SUCHC2").val());
            BaseParameter.ListSearchString.push($("#SUCHC3").val());
        }

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G01/Buttonexport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                window.location.href = data.Code;
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(BaseResult.Error || localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });        
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
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    let url = "/G01_Button2";
    OpenWindowByURL(url, 800, 800);
    Button2StartInterval();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    let url = "/G01_Button2";
    OpenWindowByURL(url, 800, 800);
    Button1StartInterval();
}
function Button2StartInterval() {
    IntervalTimer = setInterval(function () {
        let G01_Button2_Close = localStorage.getItem("G01_Button2_Close");
        if (G01_Button2_Close == "1") {
            Button2_ClickSub();
        }
    }, 100);
}
function Button1StartInterval() {
    IntervalTimer = setInterval(function () {
        let G01_Button2_Close = localStorage.getItem("G01_Button2_Close");
        if (G01_Button2_Close == "1") {
            clearInterval(IntervalTimer);
            Button1_ClickSub();
        }
    }, 100);
}
function Button2_ClickSub() {
    let G01_Button2_TextBox1 = localStorage.getItem("G01_Button2_TextBox1");
    if (G01_Button2_TextBox1.length > 0) {
        if (G01_Button2_TextBox1 == "1111") {
            $("#BackGround").css("display", "block");
            let BaseParameter = {
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/G01/Button2_Click";
            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    //$("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                });
            });
        }
        else {
            alert(localStorage.getItem("ERROR"));
        }
    }
}
function Button1_ClickSub() {
    let G01_Button2_TextBox1 = localStorage.getItem("G01_Button2_TextBox1");    
    if (G01_Button2_TextBox1.length > 0) {     
        if (G01_Button2_TextBox1 == "0000") {            
            $("#BackGround").css("display", "block");
            let BaseParameter = {
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/G01/Button1_Click";
            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {}
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    //$("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                });
            });
        }
        else {
            alert(localStorage.getItem("ERROR"));
        }
    }
}


function ButtonsaveExcel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G01/ButtonsaveExcel_Click";
    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}
function DataGridView0Render() {
    let HTML = "";
    if (BaseResult && BaseResult.G01Overview && BaseResult.G01Overview.length > 0) {
        DataGridView0_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.G01Overview.length; i++) {
            let row = BaseResult.G01Overview[i];
            HTML += "<tr>";
            HTML += `<td>${row.PART_CODE || ""}</td>`;
            HTML += `<td>${row.PART_NO || ""}</td>`;
            HTML += `<td>${row.PART_NAME || ""}</td>`;
            HTML += `<td>${row.Raw_Material || 0}</td>`;
            HTML += `<td>${row.Finish_Goods || 0}</td>`;
            HTML += `<td>${row.WIP || 0}</td>`;
            HTML += `<td>${row.SUM || 0}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    } else {
        HTML = "<tbody><tr><td colspan='7'>No data available</td></tr></tbody>";
    }
    document.getElementById("DataGridView0").innerHTML = HTML;
}
function DataGridView0Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.G01Overview, IsTableSort);
    DataGridView0Render();
}
function DataGridView0_SelectionChanged(i) {
    DataGridView0RowIndex = i;
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.G01RawMaterial && BaseResult.G01RawMaterial.length > 0) {
        DataGridView1_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.G01RawMaterial.length; i++) {
            HTML += `<tr${BaseResult.G01RawMaterial[i].Verification === "Bad" ? ' style=" background-color: pink;"' : ""}>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].PART_IDX || ""}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].PART_NO || ""}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].PART_NM || ""}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.MT_CM07 || ""}">${BaseResult.G01RawMaterial[i].Change_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].Incoming_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].OUT_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].Difference_QTY || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.MT_CM08 || ""}">${BaseResult.G01RawMaterial[i].STOCK_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].MT_EXCEL || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.MT_CM10 || ""}">${BaseResult.G01RawMaterial[i].Verification || ""}</td>`;
            HTML += `<td>${BaseResult.G01RawMaterial[i].Location || ""}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    } else {
        HTML = "<tbody><tr><td colspan='11'>No data available</td></tr></tbody>";
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.G01RawMaterial, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.G01FinishedGood && BaseResult.G01FinishedGood.length > 0) {
        DataGridView2_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.G01FinishedGood.length; i++) {
            HTML += `<tr${BaseResult.G01FinishedGood[i].Verification === "Bad" ? ' style=" background-color: pink;"' : ""}>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].PART_IDX || ""}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].PART_NO || ""}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].PART_NM || ""}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.FG_CM07 || ""}">${BaseResult.G01FinishedGood[i].Change_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].Incoming_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].OUT_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].Difference_QTY || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.FG_CM08 || ""}">${BaseResult.G01FinishedGood[i].MES_STOCK || 0}</td>`;
            HTML += `<td style="background-color: LightYellow">${BaseResult.G01FinishedGood[i].REWORK_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].FG_EXCEL || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.FG_CM10 || ""}">${BaseResult.G01FinishedGood[i].Verification || ""}</td>`;
            HTML += `<td>${BaseResult.G01FinishedGood[i].Location || ""}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    } else {
        HTML = "<tbody><tr><td colspan='12'>No data available</td></tr></tbody>";
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.G01FinishedGood, IsTableSort);
    DataGridView2Render();
}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult && BaseResult.G01HookRack && BaseResult.G01HookRack.length > 0) {
        DataGridView3_SelectionChanged(0);
        HTML += "<tbody>";
        for (let i = 0; i < BaseResult.G01HookRack.length; i++) {
            HTML += `<tr${BaseResult.G01HookRack[i].Verification === "Bad" ? ' style=" background-color: pink;"' : ""}>`;
            HTML += `<td>${BaseResult.G01HookRack[i].PART_CODE || ""}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].PART_NO || ""}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.HR_CM07 || ""}">${BaseResult.G01HookRack[i].Change_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].Incoming_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].OUT_QTY || 0}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].MES_STOCK || 0}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].Difference_QTY || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.HR_CM08 || ""}">${BaseResult.G01HookRack[i].STOCK_QTY || 0}</td>`;
            HTML += `<td style="background-color: ${BaseResult.Data?.HR_CM09 || ""}">${BaseResult.G01HookRack[i].Verification || ""}</td>`;
            HTML += `<td>${BaseResult.G01HookRack[i].Location || ""}</td>`;
            HTML += "</tr>";
        }
        HTML += "</tbody>";
    } else {
        HTML = "<tbody><tr><td colspan='10'>No data available</td></tr></tbody>";
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.G01HookRack, IsTableSort);
    DataGridView3Render();
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
}
