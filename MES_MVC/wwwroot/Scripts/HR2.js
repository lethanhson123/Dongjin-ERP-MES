let BaseResult;
let currentEmployeeID = null;
let excelData = [];
let selectedEmployeeID = null;
const OFFICE_DEPARTMENTS = [
    "OFFICE ACC", "OFFICE ADMIN", "OFFICE CUT/LP", "OFFICE IE", "OFFICE IM-EX",
    "OFFICE IT", "OFFICE MAINTENANCE", "OFFICE PRODUCTION", "OFFICE QA",
    "OFFICE TRANSLATE", "OFFICE WAREHOUSE", "OFFICE PURCHASE"
];

const DEFAULT_DEPARTMENTS = [
    "FA", "HR", "CTLP", "QC", "PC", "ME", "ADMIN", "QA", "Tổng vụ"
];

$("#Buttonfind").click(function () { Buttonfind_Click(); });
$("#Buttonadd").click(function () { Buttonadd_Click(); });
$("#Buttonsave").click(function () { Buttonsave_Click(); });
$("#Buttondelete").click(function () { Buttondelete_Click(); });
$("#Buttoncancel").click(function () { Buttoncancel_Click(); });
$("#Buttoninport").click(function () { Buttoninport_Click(); });
$("#Buttonexport").click(function () { Buttonexport_Click(); });
$("#Buttonprint").click(function () { Buttonprint_Click(); });
$("#Buttonhelp").click(function () { Buttonhelp_Click(); });
$("#Buttonclose").click(function () { Buttonclose_Click(); });

$(document).ready(function () {
    $('.modal').modal();
    $('ul.tabs').tabs();

    $("#PersonalInfo_SearchBox, #EmployeeType_Filter, #Department_Filter").on("keyup change", function (e) {
        if (e.key === 'Enter' || e.type === 'change') {
            Buttonfind_Click();
        }
    });
    $("#EmployeeType_Filter").change(function () {
        updateDepartmentFilterOptions();
    });
    updateDepartmentFilterOptions();
    $(document).on("click", ".citizen-id", function (e) {
        e.preventDefault();
        let id = $(this).data("id");
        openEditModal(id);
    });

    $("#PersonalInfoSaveBtn").click(function () {
        savePersonalInfo();
    });

    $("#JobSaveBtn").click(function () {
        saveEmployeeJob();
    });

    $(document).on("change", ".job-employee-type", function () {
        updateRowDepartmentOptions($(this));
    });

    $("#EmployeeType").change(function () {
        updateDepartmentOptions();
    });

    $(document).on("click", ".job-delete-btn", function () {
        let jobId = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa thông tin công việc này?")) {
            deleteEmployeeJob(jobId);
        }
    });
    $("#ContractSaveBtn").click(function () {
        saveEmployeeContract();
    });
    $("#FinanceSaveBtn").click(function () {
        saveEmployeeFinance();
    });
    $("#FileSaveBtn").click(function () {
        console.log("FileSaveBtn clicked");
        saveEmployeeFile();
    });

    $(document).on("click", ".file-save-btn", function () {
        let row = $(this).closest("tr");
        let fileId = $(this).data("id");
        saveRowFileData(row, fileId);
    });

    $(document).on("click", ".file-delete-btn", function () {
        let fileId = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa tài liệu này?")) {
            deleteEmployeeFile(fileId);
        }
    });
    $(document).on("click", ".finance-save-btn", function () {
        let row = $(this).closest("tr");
        let financeId = $(this).data("id");
        saveRowFinanceData(row, financeId);
    });

    $(document).on("click", ".finance-delete-btn", function () {
        let financeId = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa thông tin tài chính này?")) {
            deleteEmployeeFinance(financeId);
        }
    });
    $(document).on("click", ".contract-save-btn", function () {
        let row = $(this).closest("tr");
        let contractId = $(this).data("id");
        saveRowContractData(row, contractId);
    });
    
    $(document).on("click", ".contract-delete-btn", function () {
        let contractId = $(this).data("id");
        if (confirm("Bạn có chắc chắn muốn xóa thông tin hợp đồng này?")) {
            deleteEmployeeContract(contractId);
        }
    });
    $(document).on("click", ".job-save-btn", function () {
        let row = $(this).closest("tr");
        let jobId = $(this).data("id");
        saveRowJobData(row, jobId);
    });
    $(document).on("click", "#DataGridView1 tbody tr", function () {
        $("#DataGridView1 tbody tr").removeClass("selected");
        $(this).addClass("selected");
        selectedEmployeeID = $(this).find("td:first").text();
    });
    loadPersonalInfoData();
});
function updateDepartmentFilterOptions() {
    let employeeType = $("#EmployeeType_Filter").val();
    let departmentSelect = $("#Department_Filter");
    let currentValue = departmentSelect.val();
    departmentSelect.find('option:not(:first)').remove();

    if (employeeType === "Office DJG") {
        OFFICE_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    } else {
        DEFAULT_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    }
    if (currentValue) {
        departmentSelect.val(currentValue);
        if (!departmentSelect.val()) {
            departmentSelect.val("");
        }
    }
    M.FormSelect.init(departmentSelect);
}
function updateDepartmentOptions() {
    let employeeType = $("#EmployeeType").val();
    let departmentSelect = $("#Department");
    let currentValue = departmentSelect.val();

    departmentSelect.find('option:not(:first)').remove();

    if (employeeType === "Office DJG") {
        OFFICE_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    } else {
        DEFAULT_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    }

    if (currentValue) {
        departmentSelect.val(currentValue);
    }

    M.FormSelect.init(departmentSelect);
}

function updateRowDepartmentOptions(employeeTypeSelect) {
    let employeeType = employeeTypeSelect.val();
    let row = employeeTypeSelect.closest('tr');
    let departmentSelect = row.find('select[name="Department"]');
    let currentValue = departmentSelect.val();

    departmentSelect.find('option:not(:first)').remove();

    if (employeeType === "Office DJG") {
        OFFICE_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    } else {
        DEFAULT_DEPARTMENTS.forEach(dept => {
            departmentSelect.append(`<option value="${dept}">${dept}</option>`);
        });
    }

    if (currentValue) {
        departmentSelect.val(currentValue);
    }

    M.FormSelect.init(departmentSelect);
}

function renderDataTable(result) {
    if (!result || !result.Data) return;

    let tbody = $("#DataGridView1 tbody");
    tbody.empty();

    result.Data.forEach(item => {
        // Xử lý null và undefined
        const id = item.ID ?? item.id ?? '';
        const employeeCode = item.EmployeeCode ?? item.employeeCode ?? '';
        const name = item.Name ?? item.name ?? '';
        const employeeType = item.EmployeeType ?? item.employeeType ?? '';
        const department = item.Department ?? item.department ?? '';
        const line = item.Line ?? item.line ?? '';
        const interviewDate = item.InterviewDate ?? item.interviewDate;
        const startDate = item.StartDate ?? item.startDate;
        const citizenID = item.CitizenID ?? item.citizenID ?? '';
        const phone = item.Phone ?? item.phone ?? '';
        const contractType = item.ContractType ?? item.contractType ?? '';
        const insuranceCode = item.InsuranceCode ?? item.insuranceCode ?? '';
        const timeUnit = item.TimeUnit ?? item.timeUnit ?? '';
        const active = (item.Active !== undefined) ? item.Active : ((item.active !== undefined) ? item.active : true);
        const createUser = item.CreateUser ?? item.createUser ?? '';
        const createDate = item.CreateDate ?? item.createDate;
        const updateUser = item.UpdateUser ?? item.updateUser ?? '';
        const updateDate = item.UpdateDate ?? item.updateDate;
       
     
     

        let statusText = active ? "Hoạt động" : "Không hoạt động";
        let statusClass = active ? "green-text" : "red-text";

        let row = `<tr>
    <td>${id}</td>
    <td>${employeeCode}</td>
    <td>${name}</td>
    <td>${employeeType}</td>
    <td>${department}</td>
    <td>${line}</td>
    <td>${interviewDate ? formatDate(interviewDate) : ''}</td>
    <td>${startDate ? formatDate(startDate) : ''}</td>
    <td><a href="#!" class="citizen-id" data-id="${id}">${citizenID}</a></td>
    <td>${phone}</td>
    <td>${contractType}</td>
    <td>${insuranceCode}</td>
    <td>${timeUnit}</td>
    <td class="${statusClass}">${statusText}</td>
    <td>${createUser}</td>
    <td>${createDate ? formatDateTime(createDate) : ''}</td>
    <td>${updateUser}</td>
    <td>${updateDate ? formatDateTime(updateDate) : ''}</td>
</tr>`;

        tbody.append(row);
    });
}

function renderEmployeeJobTable(jobsData) {
    $("#employeeJobGrid tbody tr:not(#employeeJobTemplate):not(#newJobRow)").remove();

    if (jobsData && jobsData.length > 0) {
        jobsData.forEach(job => {
            let newRow = $("#employeeJobTemplate").clone();

            newRow.attr("id", "").attr("data-id", job.ID).show();

            newRow.find('input[name="EmployeeCode"]').val(job.EmployeeCode || '');
            newRow.find('select[name="EmployeeType"]').val(job.EmployeeType || '');

            let employeeTypeSelect = newRow.find('select[name="EmployeeType"]');
            updateRowDepartmentOptions(employeeTypeSelect);
            newRow.find('select[name="Department"]').val(job.Department || '');

            newRow.find('select[name="Position"]').val(job.Position || '');
            newRow.find('select[name="Line"]').val(job.Line || '');
            newRow.find('select[name="Education"]').val(job.Education || '');
            newRow.find('input[name="Specialization"]').val(job.Specialization || '');
            newRow.find('input[name="CompanyEmail"]').val(job.CompanyEmail || '');
            newRow.find('input[name="InterviewDate"]').val(formatDateForInput(job.InterviewDate));
            newRow.find('input[name="StartDate"]').val(formatDateForInput(job.StartDate));
            newRow.find('select[name="TimeUnit"]').val(job.TimeUnit || '');

            newRow.find('input[name="JobActive"]').prop('checked', job.Active);

            if (!job.Active) {
                newRow.addClass('inactive-row');
            }

            newRow.find('.job-save-btn').attr('data-id', job.ID);
            newRow.find('.job-delete-btn').attr('data-id', job.ID);

            $("#newJobRow").after(newRow);

            M.FormSelect.init(newRow.find('select'));
        });
    }
}
function renderEmployeeContractTable(contractsData) {
    $("#employeeContractGrid tbody tr:not(#employeeContractTemplate):not(#newContractRow)").remove();

    if (contractsData && contractsData.length > 0) {
        contractsData.forEach(contract => {
            let newRow = $("#employeeContractTemplate").clone();

            newRow.attr("id", "").attr("data-id", contract.ID).show();

            newRow.find('select[name="ContractType"]').val(contract.ContractType || '');
            newRow.find('input[name="ContractDate"]').val(formatDateForInput(contract.ContractDate));
            newRow.find('input[name="StartDate"]').val(formatDateForInput(contract.StartDate));
            newRow.find('input[name="EndDate"]').val(formatDateForInput(contract.EndDate));
            newRow.find('input[name="ContractActive"]').prop('checked', contract.Active);
            if (!contract.Active) {
                newRow.addClass('inactive-row');
            }
            if (contract.ContractFile) {
                newRow.find('.contract-file-link').attr('href', contract.ContractFile);
                newRow.find('.contract-file-link').show();
            } else {
                newRow.find('.contract-file-link').hide();
            }
            newRow.find('.contract-save-btn').attr('data-id', contract.ID);
            newRow.find('.contract-delete-btn').attr('data-id', contract.ID);

            $("#newContractRow").after(newRow);

            M.FormSelect.init(newRow.find('select'));
        });
    }
}
function renderEmployeeFinanceTable(financeData) {
    $("#employeeFinanceGrid tbody tr:not(#employeeFinanceTemplate):not(#newFinanceRow)").remove();

    if (financeData && financeData.length > 0) {
        financeData.forEach((finance) => {
            if (!finance) return;

            let newRow = $("#employeeFinanceTemplate").clone();

            newRow.attr("id", "").attr("data-id", finance.ID).show();

            newRow.find('input[name="InsuranceCode"]').val(finance.InsuranceCode || '');
            newRow.find('input[name="TaxCode"]').val(finance.TaxCode || '');
            newRow.find('input[name="BankName"]').val(finance.BankName || '');
            newRow.find('input[name="BankAccount"]').val(finance.BankAccount || '');

            // Thiết lập trạng thái Active
            newRow.find('input[name="FinanceActive"]').prop('checked', finance.Active);

            // Thêm class cho hàng dựa trên trạng thái
            if (!finance.Active) {
                newRow.addClass('inactive-row');
            }

            newRow.find('.finance-save-btn').attr('data-id', finance.ID);
            newRow.find('.finance-delete-btn').attr('data-id', finance.ID);

            $("#newFinanceRow").after(newRow);
        });
    }
}
function renderEmployeeFileTable(filesData) {
    $("#employeeFileGrid tbody tr:not(#employeeFileTemplate):not(#newFileRow)").remove();

    if (filesData && filesData.length > 0) {
        filesData.forEach(file => {
            let newRow = $("#employeeFileTemplate").clone();
            newRow.attr("id", "").attr("data-id", file.ID).show();
            newRow.find('input[name="FileName"]').val(file.FileName || '');
            newRow.find('select[name="FileType"]').val(file.FileType || '');
            newRow.find('input[name="FileActive"]').prop('checked', file.Active);

            if (!file.Active) {
                newRow.addClass('inactive-row');
            }

            if (file.FilePath) {
                newRow.find('.file-download-link').attr('href', file.FilePath);
                newRow.find('.file-download-link').show();
                newRow.find('.file-name').text(file.OriginalFileName || 'Tải xuống');
            } else {
                newRow.find('.file-download-link').hide();
            }

            newRow.find('.create-date').text(file.CreateDate ? formatDateTime(file.CreateDate) : '');
            newRow.find('.file-save-btn').attr('data-id', file.ID);
            newRow.find('.file-delete-btn').attr('data-id', file.ID);

            $("#newFileRow").after(newRow);
            M.FormSelect.init(newRow.find('select'));
        });
    }
}
function saveRowJobData(row, jobId) {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin công việc' });
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: currentEmployeeID,
        TextBox1: row.find('input[name="EmployeeCode"]').val(),
        ComboBox1: row.find('select[name="EmployeeType"]').val(),
        ComboBox2: row.find('select[name="Department"]').val(),
        ComboBox3: row.find('select[name="Position"]').val(),
        ComboBox6: row.find('select[name="Line"]').val(),
        ComboBox4: row.find('select[name="Education"]').val(),
        TextBox3: row.find('input[name="Specialization"]').val(),
        TextBox4: row.find('input[name="CompanyEmail"]').val(),
        DateTimePicker1: row.find('input[name="InterviewDate"]').val(),
        DateTimePicker2: row.find('input[name="StartDate"]').val(),
        ComboBox5: row.find('select[name="TimeUnit"]').val(),
        TextBox10: jobId ? jobId.toString() : "0",
        Active: row.find('input[name="JobActive"]').is(':checked'),
        USER_IDX: getCurrentUser()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/HR2/SaveEmployeeJob";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin công việc' });

                let saveBtn = row.find('.job-save-btn');
                saveBtn.addClass('pulse');
                setTimeout(function () {
                    saveBtn.removeClass('pulse');
                }, 1000);
                if (BaseParameter.Active) {
                    row.removeClass('inactive-row');
                } else {
                    row.addClass('inactive-row');
                }

                if (!jobId || jobId == "0") {
                    loadEmployeeJobData(currentEmployeeID);
                }
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveRowContractData(row, contractId) {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin hợp đồng' });
        return;
    }

    $("#BackGround").css("display", "block");

    let formData = new FormData();
    let contractFileInput = row.find('input[name="ContractFile"]')[0];
    let contractFile = contractFileInput ? contractFileInput.files[0] : null;
    if (contractFile) {
        formData.append('file', contractFile);
    }

    let BaseParameter = {
        ID: currentEmployeeID,
        ComboBox1: row.find('select[name="ContractType"]').val(),
        DateTimePicker1: row.find('input[name="ContractDate"]').val(),
        DateTimePicker2: row.find('input[name="StartDate"]').val(),
        DateTimePicker3: row.find('input[name="EndDate"]').val(),
        TextBox10: contractId ? contractId.toString() : "0",
        Active: row.find('input[name="ContractActive"]').is(':checked') 
    };

    formData.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeContract", {
        method: "POST",
        body: formData
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin hợp đồng' });

                let saveBtn = row.find('.contract-save-btn');
                saveBtn.addClass('pulse');
                setTimeout(function () {
                    saveBtn.removeClass('pulse');
                }, 1000);
                if (BaseParameter.Active) {
                    row.removeClass('inactive-row');
                } else {
                    row.addClass('inactive-row');
                }

                if (!contractId || contractId == "0") {
                    loadEmployeeContractData(currentEmployeeID);
                }
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveRowFinanceData(row, financeId) {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin tài chính' });
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: currentEmployeeID,
        TextBox1: row.find('input[name="InsuranceCode"]').val(),
        TextBox2: row.find('input[name="TaxCode"]').val(),
        TextBox3: row.find('input[name="BankName"]').val(),
        TextBox4: row.find('input[name="BankAccount"]').val(),
        TextBox10: financeId ? financeId.toString() : "0",
        Active: row.find('input[name="FinanceActive"]').is(':checked') 
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeFinance", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin tài chính' });

                let saveBtn = row.find('.finance-save-btn');
                saveBtn.addClass('pulse');
                setTimeout(function () {
                    saveBtn.removeClass('pulse');
                }, 1000);
                if (BaseParameter.Active) {
                    row.removeClass('inactive-row');
                } else {
                    row.addClass('inactive-row');
                }

                loadEmployeeFinanceData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveRowFileData(row, fileId) {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu tài liệu' });
        return;
    }

    $("#BackGround").css("display", "block");

    let formData = new FormData();
    let fileInput = row.find('input[name="FileUpload"]')[0];
    let fileUpload = fileInput ? fileInput.files[0] : null;
    if (fileUpload) {
        formData.append('file', fileUpload);
    }

    let BaseParameter = {
        ID: currentEmployeeID,
        ComboBox1: row.find('input[name="FileName"]').val(),
        ComboBox2: row.find('select[name="FileType"]').val(),
        TextBox10: fileId ? fileId.toString() : "0",
        Active: row.find('input[name="FileActive"]').is(':checked')
    };

    formData.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeFile", {
        method: "POST",
        body: formData
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu tài liệu' });

                let saveBtn = row.find('.file-save-btn');
                saveBtn.addClass('pulse');
                setTimeout(function () {
                    saveBtn.removeClass('pulse');
                }, 1000);
                if (BaseParameter.Active) {
                    row.removeClass('inactive-row');
                } else {
                    row.addClass('inactive-row');
                }

                loadEmployeeFileData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function formatDate(dateString) {
    if (!dateString) return '';
    let date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

function formatDateTime(dateString) {
    if (!dateString) return '';
    let date = new Date(dateString);
    return date.toLocaleDateString('vi-VN') + ' ' + date.toLocaleTimeString('vi-VN');
}

function formatDateForInput(dateString) {
    if (!dateString) return '';
    let date = new Date(dateString);
    return date.toISOString().split('T')[0];
}

function openAddModal() {
    $("#PersonalInfoForm")[0].reset();
    $("#TextBox_ID").val("");
    $("#active-container").hide();
    $("#modal-title").text("Thêm Nhân Viên Mới");
    resetEmployeeJobForm();
    resetEmployeeContractForm();
    resetFinanceForm();
    $("#employeeJobGrid tbody tr:not(#employeeJobTemplate):not(#newJobRow)").remove();
    $("#employeeContractGrid tbody tr:not(#employeeContractTemplate):not(#newContractRow)").remove();
    $("#employeeFinanceGrid tbody tr:not(#employeeFinanceTemplate):not(#newFinanceRow)").remove();
    $("#employeeFileGrid tbody tr:not(#employeeFileTemplate):not(#newFileRow)").remove();
    currentEmployeeID = null;
    $('#modal-add').modal('open');
    $('ul.tabs').tabs('select', 'tab-personal');
}

function openEditModal(id) {
    $("#BackGround").css("display", "block");
    currentEmployeeID = id;

    let BaseParameter = { ID: id };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/GetPersonalInfo", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success && data.Data) {
                fillFormData(data.Data);
                $("#active-container").show();
                //$("#modal-title").text("Sửa Thông Tin Nhân Viên");
                $('#modal-add').modal('open');
                loadEmployeeJobData(id);
                loadEmployeeContractData(id);
                loadEmployeeFinanceData(id);
                loadEmployeeFileData(id); 
            } else {
                M.toast({ html: 'Không thể tải thông tin nhân viên: ' + (data.Error || 'Lỗi không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}

function fillFormData(employee) {
    $("#TextBox_ID").val(employee.ID);
    $("#TextBox_Name").val(employee.Name);
    $("#ComboBox_Gender").val(employee.Gender);
    $("#DatePicker_DOB").val(formatDateForInput(employee.DOB));
    $("#ComboBox_MaritalStatus").val(employee.MaritalStatus || '');
    $("#TextBox_Dependents").val(employee.Dependents || '');
    $("#TextBox_CitizenID").val(employee.CitizenID);
    $("#DatePicker_IDIssueDate").val(formatDateForInput(employee.IDIssueDate));
    $("#TextBox_IDIssuePlace").val(employee.IDIssuePlace);
    $("#TextBox_PermAddress").val(employee.PermAddress);
    $("#TextBox_CurrAddress").val(employee.CurrAddress);
    $("#TextBox_Phone").val(employee.Phone);
    $("#TextBox_Email").val(employee.Email || '');
    $("#CheckBox_Active").prop('checked', employee.Active);

    M.FormSelect.init(document.querySelectorAll('select'));
}

function loadEmployeeJobData(employeeID) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: employeeID }));

    fetch("/HR2/GetEmployeeJob", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                resetEmployeeJobForm();
                renderEmployeeJobTable(data.Data && data.Data.length > 0 ? data.Data : []);
            } else {
                M.toast({ html: 'Không thể tải thông tin công việc: ' + (data.Error || 'Lỗi không xác định') });
                resetEmployeeJobForm();
                renderEmployeeJobTable([]);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
            resetEmployeeJobForm();
            renderEmployeeJobTable([]);
        });
    });
}
function loadEmployeeContractData(employeeID) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: employeeID }));

    fetch("/HR2/GetEmployeeContract", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                resetEmployeeContractForm();
                renderEmployeeContractTable(data.Data && data.Data.length > 0 ? data.Data : []);
            } else {
                M.toast({ html: 'Không thể tải thông tin hợp đồng: ' + (data.Error || 'Lỗi không xác định') });
                resetEmployeeContractForm();
                renderEmployeeContractTable([]);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
            resetEmployeeContractForm();
            renderEmployeeContractTable([]);
        });
    });
}
function loadEmployeeFinanceData(employeeID) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: employeeID }));

    fetch("/HR2/GetEmployeeFinance", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                resetFinanceForm();
                // Đảm bảo data.Data là một mảng
                const financeArray = Array.isArray(data.Data) ? data.Data : [];
                renderEmployeeFinanceTable(financeArray);
            } else {
                M.toast({ html: 'Không thể tải thông tin tài chính: ' + (data.Error || 'Lỗi không xác định') });
                resetFinanceForm();
                renderEmployeeFinanceTable([]);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
            resetFinanceForm();
            renderEmployeeFinanceTable([]);
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        M.toast({ html: 'Đã xảy ra lỗi kết nối: ' + err });
        resetFinanceForm();
        renderEmployeeFinanceTable([]);
    });
}
function loadEmployeeFileData(employeeID) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: employeeID }));

    fetch("/HR2/GetEmployeeFiles", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                renderEmployeeFileTable(data.Data && data.Data.length > 0 ? data.Data : []);
            } else {
                M.toast({ html: 'Không thể tải thông tin tài liệu: ' + (data.Error || 'Lỗi không xác định') });
                renderEmployeeFileTable([]);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function deleteEmployeeFile(fileId) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: fileId }));

    fetch("/HR2/DeleteEmployeeFile", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã xóa tài liệu' });
                loadEmployeeFileData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function deleteEmployeeContract(contractId) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: contractId }));

    fetch("/HR2/DeleteEmployeeContract", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã xóa thông tin hợp đồng' });
                loadEmployeeContractData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function deleteEmployeeFinance(financeId) {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ID: financeId }));

    fetch("/HR2/DeleteEmployeeFinance", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã xóa thông tin tài chính' });
                loadEmployeeFinanceData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function resetEmployeeJobForm() {
    $("#EmployeeCode, #Specialization, #CompanyEmail, #InterviewDate, #StartDate").val('');
    $("#EmployeeType, #Department, #Position, #Line, #Education, #TimeUnit").val('');
    $("#JobActive").prop('checked', true); 

    updateDepartmentOptions();
    M.FormSelect.init(document.querySelectorAll('select'));
}
function resetFinanceForm() {
    $("#InsuranceCode, #TaxCode, #BankName, #BankAccount").val('');
    $("#FinanceActive").prop('checked', true); 
}
function resetEmployeeContractForm() {
    $("#ContractType").val('');
    $("#ContractDate, #StartDate, #EndDate").val('');
    $("#ContractFile").val('');
    $(".file-path").val('');

    M.FormSelect.init(document.querySelectorAll('select'));
}
function savePersonalInfo() {
    if (!validateForm()) return;

    let id = $("#TextBox_ID").val();
    id ? Buttonsave_Click() : Buttonadd_Click();
}

function saveEmployeeJob() {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin công việc' });
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: currentEmployeeID,
        TextBox1: $("#EmployeeCode").val(),
        ComboBox1: $("#EmployeeType").val(),
        ComboBox2: $("#Department").val(),
        ComboBox3: $("#Position").val(),
        ComboBox6: $("#Line").val(),
        ComboBox4: $("#Education").val(),
        TextBox3: $("#Specialization").val(),
        TextBox4: $("#CompanyEmail").val(),
        DateTimePicker1: $("#InterviewDate").val(),
        DateTimePicker2: $("#StartDate").val(),
        ComboBox5: $("#TimeUnit").val(),
        TextBox10: "0",
        Active: $("#JobActive").is(':checked'),
        USER_IDX: getCurrentUser()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/HR2/SaveEmployeeJob";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin công việc' });
                resetEmployeeJobForm();
                loadEmployeeJobData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveEmployeeContract() {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin hợp đồng' });
        return;
    }

    $("#BackGround").css("display", "block");

    let formData = new FormData();
    let contractFile = document.getElementById('ContractFile').files[0];
    if (contractFile) {
        formData.append('file', contractFile);
    }

    let BaseParameter = {
        ID: currentEmployeeID,
        ComboBox1: $("#ContractType").val(),
        DateTimePicker1: $("#ContractDate").val(),
        DateTimePicker2: $("#StartDate").val(),
        DateTimePicker3: $("#EndDate").val(),
        TextBox10: "0",
        Active: $("#ContractActive").is(':checked') 
    };

    formData.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeContract", {
        method: "POST",
        body: formData
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin hợp đồng' });
                resetEmployeeContractForm();
                loadEmployeeContractData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveEmployeeFinance() {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu thông tin tài chính' });
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: currentEmployeeID,
        TextBox1: $("#InsuranceCode").val(),
        TextBox2: $("#TaxCode").val(),
        TextBox3: $("#BankName").val(),
        TextBox4: $("#BankAccount").val(),
        TextBox10: "0",
        Active: $("#FinanceActive").is(':checked')
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeFinance", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu thông tin tài chính' });
                resetFinanceForm();
                loadEmployeeFinanceData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function saveEmployeeFile() {
    if (!currentEmployeeID) {
        M.toast({ html: 'Vui lòng lưu thông tin cá nhân trước khi lưu tài liệu' });
        return;
    }

    $("#BackGround").css("display", "block");

    let formData = new FormData();
    let fileUpload = document.getElementById('FileUpload').files[0];
    if (fileUpload) {
        formData.append('file', fileUpload);
    }

    let BaseParameter = {
        ID: currentEmployeeID,
        ComboBox1: $("#FileName").val(),
        ComboBox2: $("#FileType").val(),
        TextBox10: "0",
        Active: $("#FileActive").is(':checked') 
    };

    formData.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/SaveEmployeeFile", {
        method: "POST",
        body: formData
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Đã lưu tài liệu' });
                $("#FileName").val('');
                $("#FileType").val('');
                $("#FileUpload").val('');
                $(".file-path").val('');
                M.FormSelect.init(document.querySelectorAll('select'));
                loadEmployeeFileData(currentEmployeeID);
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function validateForm() {
    let isValid = true;
    let requiredFields = [
        { field: "#TextBox_Name", message: "Họ tên không được để trống" },
        { field: "#DatePicker_DOB", message: "Ngày sinh không được để trống" },
        { field: "#TextBox_CitizenID", message: "Số CCCD không được để trống" },
        { field: "#DatePicker_IDIssueDate", message: "Ngày cấp CCCD không được để trống" },
        { field: "#TextBox_IDIssuePlace", message: "Nơi cấp CCCD không được để trống" },
        { field: "#TextBox_PermAddress", message: "Địa chỉ thường trú không được để trống" },
        { field: "#TextBox_CurrAddress", message: "Địa chỉ hiện tại không được để trống" },
        { field: "#TextBox_Phone", message: "Số điện thoại không được để trống" }
    ];

    for (let item of requiredFields) {
        let value = $(item.field).val();
        if (!value) {
            M.toast({ html: item.message });
            $(item.field).focus();
            isValid = false;
            break;
        }
    }

    if (isValid) {
        let cccd = $("#TextBox_CitizenID").val();
        if (!/^\d{12}$/.test(cccd)) {
            M.toast({ html: "Số CCCD phải có đúng 12 chữ số" });
            $("#TextBox_CitizenID").focus();
            isValid = false;
        }
    }

    if (isValid) {
        let phone = $("#TextBox_Phone").val();
        if (!/^\d{10,11}$/.test(phone)) {
            M.toast({ html: "Số điện thoại phải có 10-11 chữ số" });
            $("#TextBox_Phone").focus();
            isValid = false;
        }
    }

    if (isValid) {
        let email = $("#TextBox_Email").val();
        if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            M.toast({ html: "Email không đúng định dạng" });
            $("#TextBox_Email").focus();
            isValid = false;
        }
    }

    return isValid;
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let searchText = $("#PersonalInfo_SearchBox").val();
    let employeeType = $("#EmployeeType_Filter").val();
    let department = $("#Department_Filter").val();

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({
        SearchString: searchText,
        ComboBox1: employeeType, 
        ComboBox2: department     
    }));

    fetch("/HR2/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                renderDataTable(data);
                if (data.Message) {
                    M.toast({ html: data.Message });
                }
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}
function Buttonadd_Click() {
    if ($("#modal-add").hasClass("open")) {
        let BaseParameter = {
            TextBox1: $("#TextBox_Name").val(),
            ComboBox1: $("#ComboBox_Gender").val(),
            DateTimePicker1: $("#DatePicker_DOB").val(),
            ComboBox2: $("#ComboBox_MaritalStatus").val(),
            TextBox3: $("#TextBox_Dependents").val(),
            TextBox2: $("#TextBox_CitizenID").val(),
            DateTimePicker2: $("#DatePicker_IDIssueDate").val(),
            TextBox4: $("#TextBox_IDIssuePlace").val(),
            TextBox5: $("#TextBox_PermAddress").val(),
            TextBox6: $("#TextBox_CurrAddress").val(),
            TextBox7: $("#TextBox_Phone").val(),
            TextBox8: $("#TextBox_Email").val(),
            USER_IDX: getCurrentUser()
        };

        $("#BackGround").css("display", "block");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/HR2/Buttonadd_Click", {
            method: "POST",
            body: formUpload
        }).then((response) => {
            response.json().then((data) => {
                if (data.Success) {
                    M.toast({ html: data.Message || 'Thêm nhân viên thành công' });
                    $('#modal-add').modal('close');
                    loadPersonalInfoData();
                } else {
                    M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
                M.toast({ html: 'Đã xảy ra lỗi: ' + err });
            });
        });
    } else {
        openAddModal();
    }
}

function Buttonsave_Click() {
    if (!$("#modal-add").hasClass("open")) return;

    let id = $("#TextBox_ID").val();
    if (!id) return;

    let BaseParameter = {
        ID: id,
        TextBox1: $("#TextBox_Name").val(),
        ComboBox1: $("#ComboBox_Gender").val(),
        DateTimePicker1: $("#DatePicker_DOB").val(),
        ComboBox2: $("#ComboBox_MaritalStatus").val(),
        TextBox3: $("#TextBox_Dependents").val(),
        TextBox2: $("#TextBox_CitizenID").val(),
        DateTimePicker2: $("#DatePicker_IDIssueDate").val(),
        TextBox4: $("#TextBox_IDIssuePlace").val(),
        TextBox5: $("#TextBox_PermAddress").val(),
        TextBox6: $("#TextBox_CurrAddress").val(),
        TextBox7: $("#TextBox_Phone").val(),
        TextBox8: $("#TextBox_Email").val(),
        Active: $("#CheckBox_Active").is(":checked"),
        USER_IDX: getCurrentUser()
    };

    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/HR2/Buttonsave_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                M.toast({ html: data.Message || 'Cập nhật thành công' });
                $('#modal-add').modal('close');
                loadPersonalInfoData();
            } else {
                M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}

function Buttondelete_Click() {
    if (!selectedEmployeeID) {
        M.toast({ html: 'Vui lòng chọn nhân viên cần xóa', classes: 'red' });
        return;
    }

    if (confirm("Bạn có chắc chắn muốn xóa nhân viên này?")) {
        $("#BackGround").css("display", "block");

        let formData = new FormData();
        formData.append('BaseParameter', JSON.stringify({
            ID: selectedEmployeeID
        }));

        fetch("/HR2/Buttondelete_Click", {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                $("#BackGround").css("display", "none");
                if (data.Success) {
                    M.toast({ html: data.Message || 'Xóa nhân viên thành công', classes: 'green' });
                    loadPersonalInfoData(); 
                    selectedEmployeeID = null;
                } else {
                    M.toast({ html: 'Lỗi: ' + (data.Error || 'Không thể xóa nhân viên'), classes: 'red' });
                }
            })
            .catch(err => {
                $("#BackGround").css("display", "none");
                M.toast({ html: 'Đã xảy ra lỗi: ' + err, classes: 'red' });
            });
    }
}

function Buttoncancel_Click() {
    if ($("#modal-add").hasClass("open")) {
        $('#modal-add').modal('close');
    } else {
        $("#BackGround").css("display", "block");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify({}));

        fetch("/HR2/Buttoncancel_Click", {
            method: "POST",
            body: formUpload
        }).then((response) => {
            response.json().then((data) => {
                if (data.Message) {
                    M.toast({ html: data.Message });
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
                M.toast({ html: 'Đã xảy ra lỗi: ' + err });
            });
        });
    }
}

function Buttoninport_Click() {
    $('#modal-import').modal('open');
    $('#FileImport').val('');
    $('.file-path').val('');
    $('#ImportPreviewTable').empty();
    excelData = [];
}
function formatExcelDate(value) {
    if (!value) return '';

    // If number (Excel serial date)
    if (typeof value === 'number') {
        let date = new Date((value - 25569) * 86400 * 1000);
        let year = date.getFullYear();
        let month = String(date.getMonth() + 1).padStart(2, '0');
        let day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    // If already string, return as is
    return value.toString();
}
$("#FileImport").change(function (e) {
    let file = e.target.files[0];
    if (!file) return;

    let reader = new FileReader();

    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let sheet = workbook.Sheets[workbook.SheetNames[0]];
            let rows = XLSX.utils.sheet_to_json(sheet, { header: 1 });

            // Parse data
            excelData = [];
            let tbody = $('#ImportPreviewTable');
            tbody.empty();

            // Bỏ header, đọc từ row 2
            for (let i = 1; i < rows.length; i++) {
                let row = rows[i];
                if (!row || row.length === 0) continue;

                let item = {
                    EmployeeCode: row[0] || '',
                    Name: row[1] || '',
                    EmployeeType: row[2] || '',
                    Department: row[3] || '',
                    Line: row[4] || '',
                    InterviewDate: formatExcelDate(row[5]),
                    StartDate: formatExcelDate(row[6]),
                    CitizenID: row[7] || '',
                    Phone: row[8] || '',
                    ContractType: row[9] || '',
                    InsuranceCode: row[10] || '',
                    TimeUnit: row[11] || ''
                };

                excelData.push(item);

                // Preview
                tbody.append(`<tr>
                    <td>${item.EmployeeCode}</td>
                    <td>${item.Name}</td>
                    <td>${item.EmployeeType}</td>
                    <td>${item.Department}</td>
                    <td>${item.Line}</td>
                    <td>${item.InterviewDate}</td>
                    <td>${item.StartDate}</td>
                    <td>${item.CitizenID}</td>
                    <td>${item.Phone}</td>
                    <td>${item.ContractType}</td>
                    <td>${item.InsuranceCode}</td>
                    <td>${item.TimeUnit}</td>
                </tr>`);
            }

            M.toast({ html: `Đọc được ${excelData.length} dòng` });

        } catch (err) {
            M.toast({ html: 'Lỗi đọc file: ' + err.message });
        }
    };

    reader.readAsArrayBuffer(file);
});

$("#BtnSaveImport").click(function () {
    if (excelData.length === 0) {
        M.toast({ html: 'Chưa có dữ liệu' });
        return;
    }

    if (!confirm(`Import ${excelData.length} nhân viên?`)) return;

    $("#importProgress").show();
    $(this).prop('disabled', true);

    let formData = new FormData();
    formData.append('BaseParameter', JSON.stringify({
        LeadData: excelData
    }));

    fetch("/HR2/Buttoninport_Click", {
        method: "POST",
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            $("#importProgress").hide();
            $("#BtnSaveImport").prop('disabled', false);

            if (data.Success) {
                M.toast({ html: data.Message, classes: 'green' });
                $('#modal-import').modal('close');
                loadPersonalInfoData();
            } else {
                M.toast({ html: 'Lỗi: ' + data.Error, classes: 'red' });
            }
        })
        .catch(err => {
            $("#importProgress").hide();
            $("#BtnSaveImport").prop('disabled', false);
            M.toast({ html: 'Lỗi: ' + err });
        });
});
function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));

    fetch("/HR2/Buttonexport_Click", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success && data.Data) {
                exportToExcel(data.Data);
            }
            if (data.Message) {
                M.toast({ html: data.Message });
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        });
    });
}

function exportToExcel(data) {
    console.log("Exporting data: ", data);
    M.toast({ html: 'Đang xuất file Excel...' });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));

    fetch("/HR2/Buttonprint_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");

            if (data.Success) {
                let fullLink = window.location.origin + data.Data;

                let modalContent = `
                <div id="link-modal" class="modal">
                    <div class="modal-content">
                        <h5>Link Đăng Ký Đã Tạo Thành Công</h5>
                        <p>Link dưới đây có hiệu lực trong 48 giờ:</p>
                        
                        <div class="card-panel grey lighten-4">
                            <div class="row valign-wrapper" style="margin-bottom: 0;">
                                <div class="col s10">
                                    <input id="registration-link" type="text" value="${fullLink}" readonly 
                                           style="border: none; background: transparent; font-family: monospace;">
                                </div>
                                <div class="col s2">
                                    <button class="btn-flat waves-effect waves-teal" onclick="copyLink()">
                                        <i class="material-icons">content_copy</i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        
                        <p class="grey-text">Gửi link này cho nhân viên mới để họ tự điền thông tin.</p>
                    </div>
                    <div class="modal-footer">
                        <a href="#!" class="modal-close waves-effect waves-green btn-flat">Đóng</a>
                    </div>
                </div>
            `;

                $('body').append(modalContent);

                let modal = M.Modal.init(document.getElementById('link-modal'), {
                    onCloseEnd: function () {
                        $('#link-modal').remove();
                    }
                });
                modal.open();

                M.toast({
                    html: '<i class="material-icons">check</i> ' + data.Message,
                    classes: 'green'
                });
            } else {
                M.toast({
                    html: '<i class="material-icons">error</i> ' + data.Error,
                    classes: 'red'
                });
            }
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            M.toast({
                html: '<i class="material-icons">error</i> Đã xảy ra lỗi: ' + err,
                classes: 'red'
            });
        });
}

function copyLink() {
    let linkInput = document.getElementById('registration-link');
    linkInput.select();
    linkInput.setSelectionRange(0, 99999);

    try {
        document.execCommand('copy');
        M.toast({
            html: '<i class="material-icons">check</i> Đã copy link!',
            classes: 'green',
            displayLength: 2000
        });
    } catch (err) {
        M.toast({
            html: '<i class="material-icons">error</i> Không thể copy!',
            classes: 'red'
        });
    }
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    window.open(url, "_blank", `width=${width},height=${height},left=${left},top=${top}`);
}

function loadPersonalInfoData() {
    $("#BackGround").css("display", "block");

    fetch("/HR2/Load", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => {
        if (!response.ok) {
            throw new Error(`HTTP error: ${response.status}`);
        }
        return response.json();
    }).then((data) => {
        if (data.Success) {
            renderDataTable(data);
            if (data.Message) {
                M.toast({ html: data.Message });
            }
        } else {
            M.toast({ html: 'Lỗi: ' + (data.Error || 'Không xác định') });
        }
        $("#BackGround").css("display", "none");
    }).catch((err) => {
        M.toast({ html: 'Đã xảy ra lỗi: ' + err });
        $("#BackGround").css("display", "none");
    });
}