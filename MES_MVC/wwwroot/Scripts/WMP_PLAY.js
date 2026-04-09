let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
});
function DataGridView1Sort() {
}
function DataGridView1_CellContentClick(WMPID) {
    for (let i = 1; i < 10; i++) {
        let WMPIDSub = "WMP" + i;
        $("#" + WMPIDSub).css("display", "none");
    }
    $("#" + WMPID).css("display", "block");
}