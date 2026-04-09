let IsTableSort = false;
let BaseResult = new Object();
let F_ECN_IDX = 0;
let DataGridView1RowIndex = 0;
let SAVE_BUTCHK = false;
let FULL_NM = "";
$(document).ready(function () {
    $("#PNO_TXT").val(localStorage.getItem("A01_FILE_PNO_TXT"));
    $("#ECN_TXT").val(localStorage.getItem("A01_FILE_ECN_TXT"));
    $("#ECN_DATE").val(localStorage.getItem("A01_FILE_ECN_DATE"));
    F_ECN_IDX = localStorage.getItem("A01_FILE_F_ECN_IDX");

    DGV_LOAD();
});
$("#Buttonadd").click(function () {
    Buttonadd_Click();
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#FileToUpload").change(function () {
    var FileToUpload = $('#FileToUpload').prop('files');
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                let MB_SIZE = FileToUpload[i].size / 1000000;
                $("#F_NAME").val(FileToUpload[i].name);
                $("#F_EXE").val("pdf");
                $("#F_SIZE").val(MB_SIZE + " MB");
                if (FileToUpload[i].size > 100000000) {
                    $("#RUL_TXT").val("등록불가");
                    document.getElementById("RUL_TXT").style.backgroundColor = "red";
                    FULL_NM = "";
                    SAVE_BUTCHK = false;
                }
                else {
                    $("#RUL_TXT").val("동록가능");
                    document.getElementById("RUL_TXT").style.backgroundColor = "yellow";
                    FULL_NM = FileToUpload[i].name;
                    SAVE_BUTCHK = true;
                }
            }
        }
    }
});
function Buttonadd_Click() {
    FULL_NM = "";
    SAVE_BUTCHK = false;
    $("#FileToUpload").click();
}
function Buttonsave_Click() {
    if (SAVE_BUTCHK == true) {
        $("#BackGround").css("display", "block");
        let BaseResultDGV2_LOAD = new Object();
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($("#PNO_TXT").val());
        BaseParameter.ListSearchString.push($("#ECN_TXT").val());
        BaseParameter.ListSearchString.push($("#ECN_DATE").val());
        BaseParameter.ListSearchString.push($("#F_ECN_IDX").val());        
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
        let url = "/A01_FILE/Buttonsave_Click";

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
}
function DGV_LOAD() {
    $("#F_NAME").val("-");
    $("#F_EXE").val("-");
    $("#F_SIZE").val("-");
    $("#RUL_TXT").val("-");

    $("#BackGround").css("display", "block");
    let BaseResultDGV2_LOAD = new Object();
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#PNO_TXT").val(),
    }
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/A01_FILE/DGV_LOAD";

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
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_ECN_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_ENCNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DWG_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DWG_FILE_GRP + "</td>";
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'>" + BaseResult.DataGridView1[i].DN + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
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
function DataGridView1_CellClick(i) {
    DataGridView3RowIndex = i;
    let filePAthName = BaseResult.DataGridView1[DataGridView1RowIndex].DWG_FILE_GRP;
    let ftpAddress = BaseResult.DataGridView1[DataGridView1RowIndex].DWG_FILE_EXPOR + "/";
    let ftpUser = "ysj4947";
    let ftpPassword = "Kh0ngx0a";
    let fileToDownload = filePAthName;
    let sFtpFile = ftpAddress + fileToDownload;
    let url = sFtpFile;
    OpenWindowByURL(url, 800, 400);
}