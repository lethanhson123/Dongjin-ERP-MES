let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;
let Timer;


$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    C09_COUNT_Load();
    document.getElementById("RadioButton1").checked = true;
});

function C09_COUNT_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_COUNT/C09_COUNT_Load";

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
$("#RadioButton1").click(function (e) {
    RadioButton2_Click();
});
$("#RadioButton2").click(function (e) {
    RadioButton2_Click();
});
function RadioButton2_Click() {    
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.RadioButton1 = document.getElementById("RadioButton1").checked;
    BaseParameter.RadioButton2 = document.getElementById("RadioButton2").checked;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_COUNT/RadioButton2_Click";

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
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";                  
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SUM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Stay + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Working + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Complete + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Close + "</td>";
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
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "OR_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});