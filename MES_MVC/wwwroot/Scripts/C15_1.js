let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("C15_1_Close", 1);
    window.close();
});

$(document).ready(function () {
    localStorage.setItem("C15_1_Close", 0);

});

$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    BaseResult.DataGridView1 = [];
    DataGridView1Render();
    let TextBox1 = $("#TextBox1").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(TextBox1);

    let formUpload = new FormData();formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C15_1/Button1_Click";

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
                    HTML = HTML + "<tr ondblclick='DataGridView1_CellContentDoubleClick(" + i + ")' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_PN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    $("#Label2").val(BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_PN);
    $("#Label3").val(BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE);
}
function DataGridView1_CellContentDoubleClick(i) {
    DataGridView1RowIndex = i;
    $("#Label2").val(BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_PN);
    $("#Label3").val(BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE);

    localStorage.setItem("C15_1_LEAD_PN", BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_PN);
    localStorage.setItem("C15_1_BUNDLE_SIZE", BaseResult.DataGridView1[DataGridView1RowIndex].BUNDLE_SIZE);

    localStorage.setItem("C15_1_Close", 1);
    window.close();
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "DATE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});