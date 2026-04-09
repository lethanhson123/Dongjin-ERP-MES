let SAVE_BUTCHK = false;
let BaseResult;
let DataGridView1RowIndex;
let CHK_V;

$(window).focus(function () {
}).blur(function () {
    window.close();
});

$(document).ready(function () {
    document.getElementById("Label8").style.backgroundColor = "white";
    $("#Label3").val(localStorage.getItem("V01_1_Label3"));
    $("#Label2").val(localStorage.getItem("V01_1_Label2"));
    CHK_V = localStorage.getItem("V01_1_CHK_V");
    document.getElementById("PictureBox1").src = localStorage.getItem("V01_1_URL_PHOTO");
    document.getElementById("Button1").href = localStorage.getItem("V01_1_URL_PHOTO");
});
$("#Buttonadd").click(function () {
    Buttonadd_Click();
});
function Buttonadd_Click() {
    $("#FileToUpload").click();
}
$("#FileToUpload").change(function () {
    var FileToUpload = $('#FileToUpload').prop('files');
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                let F_SIZE = FileToUpload[i].size;
                let MB_SIZE = F_SIZE / 1000000;

                $("#Label4").val(F_SIZE);
                $("#Label8").val(MB_SIZE);

                if (F_SIZE >= 1700000) {
                    $("#Label2").val("-");
                    document.getElementById("Label8").style.backgroundColor = "red";
                    document.getElementById("PictureBox1").src = "#";
                    document.getElementById("Button1").href = "#";
                }
                else {
                    $("#Label2").val(FileToUpload[i].name);
                    document.getElementById("Label8").style.backgroundColor = "white";
                    document.getElementById("PictureBox1").src = URL.createObjectURL(FileToUpload[i]);
                    document.getElementById("Button1").href = URL.createObjectURL(FileToUpload[i]);
                    SAVE_BUTCHK = true;
                }
            }
        }
    }
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
function Buttonsave_Click() {
    let Label3 = $("#Label3").val();
    if (Label3.length < 3) {
        SAVE_BUTCHK = false;
    }
    let Label2 = $("#Label2").val();
    if (Label2.length < 3) {
        SAVE_BUTCHK = false;
    }
    if (SAVE_BUTCHK == true) {
        $("#BackGround").css("display", "block");        
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }       
        BaseParameter.ListSearchString.push($("#Label3").val());      
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
        let url = "/V01_1/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.setItem("SaveSuccess"));
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.setItem("ERRORAnErrorOccurred"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    window.close();
}