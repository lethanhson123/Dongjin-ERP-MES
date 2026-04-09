let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {    
}).blur(function () {
    window.close();
});

$("#Button1").click(function () {
    Button1_Click();
});
$("#Button3").click(function () {
    Button3_Click();
});
$("#TextID").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
function Button1_Click() {
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $('#TextID').val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B01_1/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;            
            RenderDataGridView1();
        }).catch((err) => {
        })
    });    
}
function Button3_Click() {
    if (BaseResult) {
        if (BaseResult.Listtspart) {
            if (BaseResult.Listtspart.length > 0) {
                if (RowIndex > -1) {                    
                    let AAA = BaseResult.Listtspart[RowIndex].PART_IDX;
                    let BBB = BaseResult.Listtspart[RowIndex].PART_NO;
                    let CCC = BaseResult.Listtspart[RowIndex].PART_NM;
                    let DDD = BaseResult.Listtspart[RowIndex].PART_SNP;

                    localStorage.setItem("B01_1BBB", BBB);
                    localStorage.setItem("B01_1CCC", CCC);
                    localStorage.setItem("B01_1DDD", DDD);                    

                    //$('#Label1').val(AAA);
                    //$('#Label2').val(BBB);
                    //$('#Label3').val(CCC);
                }
            }
        }
    }
    window.close();
}
function RenderDataGridView1() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.Listtspart) {
            if (BaseResult.Listtspart.length > 0) {                
                let RowCount = 100;
                if (BaseResult.Listtspart.length < 100) {
                    RowCount = BaseResult.Listtspart.length;
                }
                for (let i = 0; i < RowCount; i++) {                    
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.Listtspart[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.Listtspart[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.Listtspart[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.Listtspart[i].PART_SNP + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridViewSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.Listtspart, IsTableSort);
    RenderDataGridView();
}
function DataGridView1_SelectionChanged(i) {
    RowIndex = i;
    if (BaseResult) {
        if (BaseResult.Listtspart) {
            if (BaseResult.Listtspart.length > 0) {
                if (RowIndex > -1) {
                    let AAA = BaseResult.Listtspart[RowIndex].PART_IDX;
                    let BBB = BaseResult.Listtspart[RowIndex].PART_NO;
                    let CCC = BaseResult.Listtspart[RowIndex].PART_NM;
                    let DDD = BaseResult.Listtspart[RowIndex].PART_SNP;
                    
                    $('#Label1').val(AAA);
                    $('#Label2').val(BBB);
                    $('#Label3').val(CCC);
                }
            }
        }
    }
}



