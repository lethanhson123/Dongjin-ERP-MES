let IsTableSort = false;
let BaseResult;
let RowIndex;
let TB_PART_CNT;
let RAW_PART_CNT;

$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
    TB_PART_CNT = 0;
    RAW_PART_CNT = 0;
    $("#Label1").val(TB_PART_CNT);
    $("#Label2").val(RAW_PART_CNT);
    $("#Label6").val("-");
    $("#Label6").css("color", "black");
    $("#Label7").val("Cannot be Saved");
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
$("#Buttonfind").click(function () {
    Buttonfind_Click();
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#Buttoninport").click(function () {
    $("#FileToUpload").click();
});
$("#FileToUpload").change(function () {
    Buttoninport_Click();
});
function Buttonclose_Click() {
    window.close();
}
function Buttonfind_Click() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                $("#Label6").val("-");
                $("#Label6").css("color", "black");
                $("#Label7").val("Cannot be Saved");
                TB_PART_CNT = 0;
                RAW_PART_CNT = 0;
                $("#Label1").val(TB_PART_CNT);
                $("#Label2").val(RAW_PART_CNT);
                $("#Label6").val("NG");
                $("#Label6").css("color", "red");
                $("#BackGround").css("display", "block");
                let TextBox1 = $("#TextBox1").val();
                let BaseParameter = new Object();
                BaseParameter = {
                }
                BaseParameter.DataGridView1 = BaseResult.DataGridView1;
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B07_3/Buttonfind_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        BaseResult = data;
                        DataGridView1Render();
                        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                            if (BaseResult.DataGridView1[i].LOAD == "STAY") {
                                let RAW_PART = BaseResult.DataGridView1[i].Raw_Material_Part;
                                let RAW_TF = true;
                                for (let j = 0; j < BaseResult.DGV_B07_32.Count; j++) {
                                    if (RAW_PART == BaseResult.DGV_B07_32[j].PART_NO) {
                                        if (BaseResult.DataGridView1[i].LOAD == "STAY") {
                                            $("#Label7").val("Can be Saved");
                                            $("#Label6").val("Some OK");
                                            $("#Label6").css("color", "red");
                                        }
                                    }
                                }
                            }
                        }
                        TB_PART_CNT = BaseResult.TB_PART_CNT;
                        RAW_PART_CNT = BaseResult.RAW_PART_CNT;
                        $("#Label1").val(TB_PART_CNT);
                        $("#Label2").val(RAW_PART_CNT);
                        if ((TB_PART_CNT + RAW_PART_CNT) == 0) {
                            $("#Label6").val("OK");
                            $("#Label6").css("color", "green");
                        }
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
}
function Buttonsave_Click() {
    let IsSave = true;
    let Label7 = $("#Label7").val();
    if (Label7 == "Can be Saved") {
        let Label6 = $("#Label6").val();
        if (Label6 == "-") {
            IsSave = false;
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        }
        if (Label6 == "NG") {
            IsSave = false;
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
            }
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView1 = BaseResult.DataGridView1;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B07_3/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    BaseResult = data;
                    DataGridView1Render();
                    $("#Label6").val("-");
                    $("#Label6").css("color", "black");
                    $("#Label7").val("Cannot be Saved");
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("SaveNotSuccess"));
                    $("#BackGround").css("display", "none");
                })
            });

        }
    }
}
function Buttoninport_Click() {
    $("#Label6").val("-");
    $("#Label7").val("Cannot be Saved");
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
    let url = "/B07_3/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            alert(localStorage.getItem("ExcelImportSuccess"));
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("SaveNotSuccess"));
            $("#BackGround").css("display", "none");
        })
    });
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LOAD + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Tube_Cutting_Part_No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Tube_Cutting_Part_No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Description + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Raw_Material_Part + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Size + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Machine + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Packing_Unit + "</td>";
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
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
