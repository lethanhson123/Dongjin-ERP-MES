let IsTableSort = false;
let BaseResult;
let RowIndex = 0;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("Z04_1_Close", 1);
    window.close();
});

$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
$("#Button3").click(function () {
    Button3_Click();
});
$("#TextID").keydown(function (e) {
    if (e.keyCode == 13) {
        TextID_KeyDown();
    }
});
function TextID_KeyDown() {
    Button1_Click();
}

function Button1_Click() {
    $('#Label1').val("");
    $('#Label2').val("");
    $('#Label3').val("");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $('#TextID').val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z04_1/Button1_Click";

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
function Button2_Click() {
    localStorage.setItem("Z04_1_Close", 1);
    window.close();
}
function Button3_Click() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                if (RowIndex > -1) {
                    let AAA = BaseResult.DataGridView1[RowIndex].PART_IDX;
                    let BBB = BaseResult.DataGridView1[RowIndex].PART_NO;
                    let CCC = BaseResult.DataGridView1[RowIndex].PART_NM;
                    let DDD = BaseResult.DataGridView1[RowIndex].TNM;

                    localStorage.setItem("Z04_TextBox10", BBB);
                    localStorage.setItem("Z04_TextBox18", AAA);
                    localStorage.setItem("Z04_TextBox17", DDD);
                }
            }
        }
    }
    localStorage.setItem("Z04_1_Close", 1);
    window.close();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr ondblclick='DataGridView1_CellDoubleClick(" + i + ")' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TNM + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridViewSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    RowIndex = i;
    DGV_SELECT();
}
function DGV_SELECT() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                if (RowIndex > -1) {
                    let AAA = BaseResult.DataGridView1[RowIndex].PART_IDX;
                    let BBB = BaseResult.DataGridView1[RowIndex].PART_NO;
                    let CCC = BaseResult.DataGridView1[RowIndex].PART_NM;
                    let DDD = BaseResult.DataGridView1[RowIndex].TNM;

                    $('#Label1').val(AAA);
                    $('#Label2').val(BBB);
                    $('#Label3').val(CCC);
                }
            }
        }
    }
}
function DataGridView1_CellClick() {

}
function DataGridView1_CellDoubleClick() {
    Button3_Click();
}

