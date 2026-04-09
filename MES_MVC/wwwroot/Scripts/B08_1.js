let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;
let CHK_NO;
$(window).focus(function () {
}).blur(function () {
    Button3_Click();
});

$(document).ready(function () {
    CHK_NO = localStorage.getItem("B08_1_CHK_NO");
    $("#PART_NO_LB").val(localStorage.getItem("B08_1_PART_NO_LB"));
    $("#STAY_LB").val(localStorage.getItem("B08_1_STAY_LB"));
});
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    localStorage.setItem("B08_1_Close", 1);
    window.close();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    $("#BackGround").css("display", "block");    
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: CHK_NO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08_1/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            Button3_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: CHK_NO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08_1/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            Button3_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button5").click(function () {
    Button5_Click();
});
function Button5_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: CHK_NO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08_1/Button5_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            Button3_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
