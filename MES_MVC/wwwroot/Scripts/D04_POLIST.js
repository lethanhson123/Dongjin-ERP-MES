let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("D04_POLIST_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("D04_POLIST_Close", 0);
    D04_POLIST_Load();
});
function D04_POLIST_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_POLIST/D04_POLIST_Load";

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
    let IsCheck = true;
    let PO_CD = BaseResult.DataGridView1[DataGridView1RowIndex].PO_CODE;
    let PO_CD_CHK = localStorage.getItem("D04_PO_CD_CHK");
    if (PO_CD_CHK == "") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        localStorage.setItem("D04_POLIST_PO_CD", PO_CD);
        localStorage.setItem("D04_POLIST_Close", 1);
        window.close();      
    }
}
