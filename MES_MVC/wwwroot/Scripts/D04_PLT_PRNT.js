let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("D04_PLT_PRNT_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("D04_PLT_PRNT_Close", 0);
    $("#MaskedTextBox1").val(localStorage.getItem("D04_PLT_PRNT_MaskedTextBox1"));
    D04_PLT_PRNT_Load();
});
function D04_PLT_PRNT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_PLT_PRNT/D04_PLT_PRNT_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#Label3").val(0);
            $("#NumericUpDown1").val(1);
            try {
                let S_QTY = Number($("#MaskedTextBox2").val());
                let E_QTY = Number($("#NumericUpDown1").val()) * 2;
                $("#Label3").val(S_QTY + E_QTY - 1);                
            }
            catch (err) {
            }
            $("#MaskedTextBox2").focus();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("D04_PLT_PRNT_Close", 1);
    window.close();
}
$("#NumericUpDown1").change(function () {
    NumericUpDown1_ValueChanged();
});
function NumericUpDown1_ValueChanged() {
    try {
        let S_QTY = Number($("#MaskedTextBox2").val());
        let E_QTY = Number($("#NumericUpDown1").val()) * 2;
        $("#Label3").val(S_QTY + E_QTY - 1);        
    }
    catch (err) {

    }
}
$("#MaskedTextBox2").change(function () {
    MaskedTextBox2_TextChanged();
});
function MaskedTextBox2_TextChanged() {    
    try {
        let S_QTY = Number($("#MaskedTextBox2").val());
        if (S_QTY > 999) {
            S_QTY = 999;
            $("#MaskedTextBox2").val("999");          
        }
        let E_QTY = Number($("#NumericUpDown1").val()) * 2;
        $("#Label3").val(S_QTY + E_QTY - 1);        
    }
    catch (err) {

    }
}
$("#Buttonprint").click(function () {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    let IsCheck = true;
    let MaskedTextBox1 = $("#MaskedTextBox1").val();
    if (MaskedTextBox1 == "") {
        IsCheck = false;
        alert("Not PO CODE. Please Check Again.");
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
    }
    let MaskedTextBox2 = Number($("#MaskedTextBox2").val());
    if (MaskedTextBox1 == 0) {
        IsCheck = false;
        alert("Not Start NO. Please Check Again.");
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
    }
    if (IsCheck == true) {
        let NumericUpDown1 = $("#NumericUpDown1").val();
        let MaskedTextBox1 = $("#MaskedTextBox1").val();
        let MaskedTextBox2 = $("#MaskedTextBox2").val();
        let SU03 = $("#TextBox3").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {            
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(NumericUpDown1);
        BaseParameter.ListSearchString.push(MaskedTextBox1);
        BaseParameter.ListSearchString.push(MaskedTextBox2);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04_PLT_PRNT/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultPrint = data;
                if (BaseResultPrint) {
                    if (BaseResultPrint.Code) {
                        let url = BaseResultPrint.Code;
                        OpenWindowByURL(url, 200, 200);
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                //DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PO_CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    let PO_CD = BaseResult.DataGridView1[DataGridView1RowIndex].PO_CODE;
    $("#MaskedTextBox1").val(PO_CD);
    $("#Label3").val(0);
    $("#NumericUpDown1").val(1);
    try {
        let S_QTY = Number($("#MaskedTextBox2").val());
        let E_QTY = Number($("#NumericUpDown1").val()) * 2;
        $("#Label3").val(S_QTY + E_QTY - 1);
    }
    catch (err) {

    }
    $("#MaskedTextBox2").focus();
}
