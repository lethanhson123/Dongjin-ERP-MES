using Newtonsoft.Json.Linq;

namespace MESService.Implement
{
    public class HR2Service : BaseService<PersonalInfo, IPersonalInfoRepository>, IHR2Service
    {
        // Các trường hiện có
        private readonly IPersonalInfoRepository _personalInfoRepository;
        private readonly IEmployeeJobRepository _employeeJobRepository;
        private readonly IEmployeeContractRepository _employeeContractRepository;
        private readonly IEmployeeFinanceRepository _employeeFinanceRepository;
        private readonly IEmployeeFileRepository _employeeFileRepository;

        public HR2Service(
            IPersonalInfoRepository personalInfoRepository,
            IEmployeeJobRepository employeeJobRepository,
            IEmployeeContractRepository employeeContractRepository,
            IEmployeeFinanceRepository employeeFinanceRepository,
            IEmployeeFileRepository employeeFileRepository) : base(personalInfoRepository)
        {
            _personalInfoRepository = personalInfoRepository;
            _employeeJobRepository = employeeJobRepository;
            _employeeContractRepository = employeeContractRepository;
            _employeeFinanceRepository = employeeFinanceRepository;
            _employeeFileRepository = employeeFileRepository;
        }

        public override void Initialization(PersonalInfo model)
        {
            BaseInitialization(model);
        }

        // Helper method để giảm code trùng lặp
        private async Task<BaseResult> ExecuteServiceOperation(Func<Task<BaseResult>> operation)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await operation();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Load()
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                try
                {
                    // Lấy dữ liệu cơ bản từ PersonalInfo
                    var employees = await _personalInfoRepository
                        .GetByCondition(o => o.Active)
                        .OrderBy(o => o.Name)
                        .ToListAsync();

                    // Lấy thông tin từ các bảng liên quan để join trong memory
                    var employeeIds = employees.Select(e => e.ID).ToList();

                    var jobs = await _employeeJobRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    var contracts = await _employeeContractRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    var finances = await _employeeFinanceRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    // Tạo list kết quả
                    var resultList = new List<dynamic>();

                    foreach (var emp in employees)
                    {
                        // Lấy bản ghi mới nhất từ mỗi bảng
                        var job = jobs
                            .Where(j => j.PersonalInfoID == emp.ID)
                            .OrderByDescending(j => j.Active)
                            .ThenByDescending(j => j.CreateDate)
                            .FirstOrDefault();

                        var contract = contracts
                            .Where(c => c.PersonalInfoID == emp.ID)
                            .OrderByDescending(c => c.Active)
                            .ThenByDescending(c => c.CreateDate)
                            .FirstOrDefault();

                        var finance = finances
                            .Where(f => f.PersonalInfoID == emp.ID)
                            .OrderByDescending(f => f.Active)
                            .ThenByDescending(f => f.CreateDate)
                            .FirstOrDefault();

                        // Tạo object chứa thông tin từ tất cả các bảng
                        var item = new
                        {
                            ID = emp.ID,
                            Name = emp.Name,
                            CitizenID = emp.CitizenID,
                            Phone = emp.Phone,
                            EmployeeCode = job?.EmployeeCode,
                            EmployeeType = job?.EmployeeType,
                            Department = job?.Department,
                            Line = job?.Line,
                            InterviewDate = job?.InterviewDate,
                            StartDate = job?.StartDate,
                            ContractType = contract?.ContractType,
                            InsuranceCode = finance?.InsuranceCode,
                            TimeUnit = job?.TimeUnit,
                            Active = emp.Active,
                            CreateUser = emp.CreateUser,
                            CreateDate = emp.CreateDate,
                            UpdateUser = emp.UpdateUser,
                            UpdateDate = emp.UpdateDate ?? emp.CreateDate




                        };

                        resultList.Add(item);
                    }

                    result.Data = resultList;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Error = ex.Message;
                }

                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                try
                {
                    // Lấy dữ liệu với filter
                    var query = _personalInfoRepository.GetByCondition(o => o.Active);

                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        string search = BaseParameter.SearchString.Trim().ToLower();
                        query = query.Where(o =>
                            o.Name.ToLower().Contains(search) ||
                            o.CitizenID.Contains(search) ||
                            o.Phone.Contains(search) ||
                            (o.Email != null && o.Email.ToLower().Contains(search))
                        );
                    }

                    var employees = await query.OrderBy(o => o.Name).ToListAsync();

                    // Lấy thông tin từ các bảng liên quan
                    var employeeIds = employees.Select(e => e.ID).ToList();

                    var jobs = await _employeeJobRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    // Áp dụng filter cho bộ phận và loại nhân viên
                    if (!string.IsNullOrEmpty(BaseParameter.ComboBox1)) // EmployeeType
                    {
                        string employeeType = BaseParameter.ComboBox1;
                        jobs = jobs.Where(j => j.EmployeeType == employeeType).ToList();
                    }

                    if (!string.IsNullOrEmpty(BaseParameter.ComboBox2)) // Department
                    {
                        string department = BaseParameter.ComboBox2;
                        jobs = jobs.Where(j => j.Department == department).ToList();
                    }

                    // Lọc lại danh sách employeeIds dựa trên jobs đã lọc
                    if (!string.IsNullOrEmpty(BaseParameter.ComboBox1) || !string.IsNullOrEmpty(BaseParameter.ComboBox2))
                    {
                        employeeIds = jobs.Select(j => j.PersonalInfoID).Distinct().ToList();
                        employees = employees.Where(e => employeeIds.Contains(e.ID)).ToList();
                    }

                    // Lấy thêm dữ liệu từ các bảng khác
                    var contracts = await _employeeContractRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    var finances = await _employeeFinanceRepository
                        .GetByCondition(o => employeeIds.Contains(o.PersonalInfoID))
                        .ToListAsync();

                    // Tạo list kết quả
                    var resultList = new List<dynamic>();

                    foreach (var emp in employees)
                    {
                        // Lấy bản ghi mới nhất từ mỗi bảng
                        var job = jobs
                            .Where(j => j.PersonalInfoID == emp.ID)
                            .OrderByDescending(j => j.Active)
                            .ThenByDescending(j => j.CreateDate)
                            .FirstOrDefault();

                        // Bỏ qua nếu không tìm thấy thông tin công việc phù hợp với filter
                        if ((!string.IsNullOrEmpty(BaseParameter.ComboBox1) || !string.IsNullOrEmpty(BaseParameter.ComboBox2)) && job == null)
                            continue;

                        var contract = contracts
                            .Where(c => c.PersonalInfoID == emp.ID)
                            .OrderByDescending(c => c.Active)
                            .ThenByDescending(c => c.CreateDate)
                            .FirstOrDefault();

                        var finance = finances
                            .Where(f => f.PersonalInfoID == emp.ID)
                            .OrderByDescending(f => f.Active)
                            .ThenByDescending(f => f.CreateDate)
                            .FirstOrDefault();

                        // Tạo object chứa thông tin từ tất cả các bảng
                        var item = new
                        {
                            ID = emp.ID,
                            Name = emp.Name,
                            CitizenID = emp.CitizenID,
                            Phone = emp.Phone,
                            EmployeeCode = job?.EmployeeCode,
                            EmployeeType = job?.EmployeeType,
                            Department = job?.Department,
                            Line = job?.Line,
                            InterviewDate = job?.InterviewDate,
                            StartDate = job?.StartDate,
                            ContractType = contract?.ContractType,
                            InsuranceCode = finance?.InsuranceCode,
                            TimeUnit = job?.TimeUnit,
                            Active = emp.Active,
                            CreateUser = emp.CreateUser,
                            CreateDate = emp.CreateDate,
                            UpdateUser = emp.UpdateUser,
                            UpdateDate = emp.UpdateDate ?? emp.CreateDate
                        };

                        resultList.Add(item);
                    }

                    result.Data = resultList;
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Error = ex.Message;
                }

                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var existingEmployee = await _personalInfoRepository
                    .GetByCondition(o => o.CitizenID == BaseParameter.TextBox2 && o.Active)
                    .FirstOrDefaultAsync();

                if (existingEmployee != null)
                {
                    result.Error = "CCCD đã tồn tại trong hệ thống";
                    return result;
                }

                var newEmployee = new PersonalInfo
                {
                    Name = BaseParameter.TextBox1,
                    Gender = BaseParameter.ComboBox1,
                    DOB = DateTime.TryParse(BaseParameter.DateTimePicker1, out DateTime dob) ? dob : DateTime.Now,
                    MaritalStatus = BaseParameter.ComboBox2,
                    Dependents = int.TryParse(BaseParameter.TextBox3, out int dep) ? dep : null,
                    CitizenID = BaseParameter.TextBox2,
                    IDIssueDate = DateTime.TryParse(BaseParameter.DateTimePicker2, out DateTime issueDate) ? issueDate : DateTime.Now,
                    IDIssuePlace = BaseParameter.TextBox4,
                    PermAddress = BaseParameter.TextBox5,
                    CurrAddress = BaseParameter.TextBox6,
                    Phone = BaseParameter.TextBox7,
                    Email = BaseParameter.TextBox8,
                    Active = true,
                    CreateDate = DateTime.Now,
                    CreateUser = BaseParameter.USER_IDX
                };

                await AddAsync(newEmployee);
                result.Success = true;
                result.Message = "Thêm nhân viên thành công";
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employee = await _personalInfoRepository
                    .GetByCondition(o => o.ID == BaseParameter.ID && o.Active)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Không tìm thấy nhân viên";
                    return result;
                }

                employee.Name = BaseParameter.TextBox1 ?? employee.Name;
                employee.Gender = BaseParameter.ComboBox1 ?? employee.Gender;
                if (DateTime.TryParse(BaseParameter.DateTimePicker1, out DateTime dob))
                    employee.DOB = dob;
                employee.MaritalStatus = BaseParameter.ComboBox2 ?? employee.MaritalStatus;
                if (int.TryParse(BaseParameter.TextBox3, out int dep))
                    employee.Dependents = dep;
                employee.IDIssuePlace = BaseParameter.TextBox4 ?? employee.IDIssuePlace;
                employee.PermAddress = BaseParameter.TextBox5 ?? employee.PermAddress;
                employee.CurrAddress = BaseParameter.TextBox6 ?? employee.CurrAddress;
                employee.Phone = BaseParameter.TextBox7 ?? employee.Phone;
                employee.Email = BaseParameter.TextBox8 ?? employee.Email;
                employee.Active = BaseParameter.Active.HasValue ? BaseParameter.Active.Value : employee.Active;
                employee.UpdateDate = DateTime.Now;
                employee.UpdateUser = BaseParameter.USER_IDX;

                await UpdateAsync(employee);
                result.Success = true;
                result.Message = "Cập nhật thành công";
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();
                if (BaseParameter.ID == null || BaseParameter.ID <= 0)
                {
                    result.Success = false;
                    result.Error = "Vui lòng chọn nhân viên cần xóa";
                    return result;
                }
                var personalInfo = await _personalInfoRepository
                    .GetByCondition(o => o.ID == (long)BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (personalInfo == null)
                {
                    result.Success = false;
                    result.Error = "Không tìm thấy thông tin nhân viên";
                    return result;
                }
                personalInfo.Active = false;
                personalInfo.UpdateDate = DateTime.Now;
                personalInfo.UpdateUser = BaseParameter.USER_IDX;

                await _personalInfoRepository.UpdateAsync(personalInfo);

                result.Success = true;
                result.Message = "Đã xóa nhân viên thành công";

                return result;
            });
        }

        public virtual async Task<BaseResult> GetPersonalInfo(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employee = await _personalInfoRepository
                    .GetByCondition(o => o.ID == BaseParameter.ID && o.Active)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Không tìm thấy thông tin nhân viên";
                    return result;
                }

                result.Data = employee;
                result.Success = true;
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult
                {
                    Success = true,
                    Message = "Đã hủy thao tác"
                };
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();
                int success = 0, error = 0;
                var errors = new List<string>();

                try
                {
                    var data = BaseParameter.LeadData as JArray;
                    if (data == null || data.Count == 0)
                    {
                        result.Error = "Không có dữ liệu";
                        return result;
                    }

                    foreach (var item in data)
                    {
                        try
                        {
                            var empCode = item["EmployeeCode"]?.ToString()?.Trim();
                            var name = item["Name"]?.ToString()?.Trim();
                            var empType = item["EmployeeType"]?.ToString()?.Trim();
                            var dept = item["Department"]?.ToString()?.Trim();
                            var line = item["Line"]?.ToString()?.Trim();
                            var interviewDate = item["InterviewDate"]?.ToString()?.Trim();
                            var startDate = item["StartDate"]?.ToString()?.Trim();
                            var cccd = item["CitizenID"]?.ToString()?.Trim();
                            var phone = item["Phone"]?.ToString()?.Trim();
                            var contractType = item["ContractType"]?.ToString()?.Trim();
                            var bhxh = item["InsuranceCode"]?.ToString()?.Trim();
                            var timeUnit = item["TimeUnit"]?.ToString()?.Trim();

                            // Validate
                            if (string.IsNullOrEmpty(empCode) || string.IsNullOrEmpty(name))
                            {
                                errors.Add($"Mã {empCode}: Thiếu mã NV hoặc họ tên");
                                error++;
                                continue;
                            }

                            // Check duplicate
                            var exists = await _employeeJobRepository
                                .GetByCondition(o => o.EmployeeCode == empCode)
                                .FirstOrDefaultAsync();

                            if (exists != null)
                            {
                                errors.Add($"Mã {empCode}: Đã tồn tại");
                                error++;
                                continue;
                            }

                            // Create PersonalInfo
                            var personal = new PersonalInfo
                            {
                                Name = name,
                                Gender = "Nam",
                                DOB = DateTime.Now.AddYears(-25),
                                CitizenID = string.IsNullOrEmpty(cccd) ? "TEMP_" + empCode : cccd,
                                IDIssueDate = DateTime.Now,
                                IDIssuePlace = "Cần cập nhật",
                                PermAddress = "Cần cập nhật",
                                CurrAddress = "Cần cập nhật",
                                Phone = string.IsNullOrEmpty(phone) ? "0000000000" : phone,
                                Active = true,
                                CreateDate = DateTime.Now,
                                CreateUser = "IMPORT"
                            };
                            await _personalInfoRepository.AddAsync(personal);

                            // Create EmployeeJob
                            var job = new EmployeeJob
                            {
                                PersonalInfoID = personal.ID,
                                EmployeeCode = empCode,
                                EmployeeType = empType,
                                Department = dept,
                                Line = line,
                                InterviewDate = ParseDate(interviewDate),
                                StartDate = ParseDate(startDate),
                                TimeUnit = timeUnit,
                                Active = true,
                                CreateDate = DateTime.Now,
                                CreateUser = "IMPORT"
                            };
                            await _employeeJobRepository.AddAsync(job);

                            // Create EmployeeContract (if has)
                            if (!string.IsNullOrEmpty(contractType))
                            {
                                var contract = new EmployeeContract
                                {
                                    PersonalInfoID = personal.ID,
                                    ContractType = contractType,
                                    ContractDate = DateTime.Now,
                                    StartDate = ParseDate(startDate) ?? DateTime.Now,
                                    Active = true,
                                    CreateDate = DateTime.Now,
                                    CreateUser = "IMPORT"
                                };
                                await _employeeContractRepository.AddAsync(contract);
                            }

                            // Create EmployeeFinance (if has BHXH)
                            if (!string.IsNullOrEmpty(bhxh))
                            {
                                var finance = new EmployeeFinance
                                {
                                    PersonalInfoID = personal.ID,
                                    InsuranceCode = bhxh,
                                    Active = true,
                                    CreateDate = DateTime.Now,
                                    CreateUser = "IMPORT"
                                };
                                await _employeeFinanceRepository.AddAsync(finance);
                            }

                            success++;
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Mã {item["EmployeeCode"]}: {ex.Message}");
                            error++;
                        }
                    }

                    result.Success = true;
                    result.Message = $"✅ Thành công: {success} | ❌ Lỗi: {error}";
                    result.Data = errors;
                }
                catch (Exception ex)
                {
                    result.Error = ex.Message;
                }

                return result;
            });
        }

        private DateTime? ParseDate(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;

            try
            {
                return DateTime.Parse(dateStr);
            }
            catch
            {
                return null;
            }
        }

        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employees = await _personalInfoRepository
                    .GetByCondition(o => o.Active)
                    .OrderBy(o => o.Name)
                    .ToListAsync();

                result.Data = employees;
                result.Success = true;
                result.Message = $"Xuất {employees.Count} bản ghi thành công";
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                string token = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

                var tempRecord = new PersonalInfo
                {
                    RegistrationToken = token,
                    TokenExpiry = DateTime.Now.AddDays(2),
                    Active = false,
                    Name = "TEMP_REGISTRATION",
                    Gender = "TEMP",
                    DOB = DateTime.Now,
                    CitizenID = "TEMP_" + token,
                    IDIssueDate = DateTime.Now,
                    IDIssuePlace = "TEMP",
                    PermAddress = "TEMP",
                    CurrAddress = "TEMP",
                    Phone = "TEMP",
                    CreateDate = DateTime.Now,
                    CreateUser = BaseParameter.USER_IDX
                };

                await AddAsync(tempRecord);

                string registrationLink = $"/HR2/SelfRegister?token={token}";

                result.Success = true;
                result.Data = registrationLink;
                result.Message = "Tạo link đăng ký thành công. Link có hiệu lực trong 48 giờ.";
                return result;
            });
        }

        public virtual async Task<BaseResult> SubmitSelfRegistration(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var tempRecord = await _personalInfoRepository
                    .GetByCondition(o => o.RegistrationToken == BaseParameter.SearchString
                                      && o.TokenExpiry > DateTime.Now
                                      && !o.Active)
                    .FirstOrDefaultAsync();

                if (tempRecord == null)
                {
                    result.Error = "Link không hợp lệ, đã hết hạn hoặc đã được sử dụng!";
                    return result;
                }

                var existingCCCD = await _personalInfoRepository
                    .GetByCondition(o => o.CitizenID == BaseParameter.TextBox2 && o.Active)
                    .FirstOrDefaultAsync();

                if (existingCCCD != null)
                {
                    result.Error = "Số CCCD này đã tồn tại trong hệ thống!";
                    return result;
                }

                tempRecord.Name = BaseParameter.TextBox1;
                tempRecord.Gender = BaseParameter.ComboBox1;
                tempRecord.DOB = DateTime.Parse(BaseParameter.DateTimePicker1);
                tempRecord.MaritalStatus = BaseParameter.ComboBox2;
                tempRecord.Dependents = string.IsNullOrEmpty(BaseParameter.TextBox3) ? null : int.Parse(BaseParameter.TextBox3);
                tempRecord.CitizenID = BaseParameter.TextBox2;
                tempRecord.IDIssueDate = DateTime.Parse(BaseParameter.DateTimePicker2);
                tempRecord.IDIssuePlace = BaseParameter.TextBox4;
                tempRecord.PermAddress = BaseParameter.TextBox5;
                tempRecord.CurrAddress = BaseParameter.TextBox6;
                tempRecord.Phone = BaseParameter.TextBox7;
                tempRecord.Email = BaseParameter.TextBox8;

                tempRecord.Active = true;
                tempRecord.RegistrationToken = null;
                tempRecord.TokenExpiry = null;
                tempRecord.UpdateDate = DateTime.Now;
                tempRecord.UpdateUser = "SELF_REGISTERED";

                await UpdateAsync(tempRecord);

                result.Success = true;
                result.Message = "Đăng ký thông tin thành công!";
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult
                {
                    Success = true,
                    Message = "Trợ giúp"
                };
                return result;
            });
        }

        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult
                {
                    Success = true,
                    Message = "Đóng form"
                };
                return result;
            });
        }

        public virtual async Task<BaseResult> GetEmployeeJob(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                // Bỏ điều kiện Active để lấy tất cả bản ghi
                var employeeJobs = await _employeeJobRepository
                    .GetByCondition(o => o.PersonalInfoID == (long)BaseParameter.ID)
                    .OrderBy(o => o.CreateDate)
                    .ToListAsync();

                result.Data = employeeJobs;
                result.Success = true;
                return result;
            });
        }

        public virtual async Task<BaseResult> SaveEmployeeJob(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                EmployeeJob employeeJob;
                long jobId = 0;

                if (long.TryParse(BaseParameter.TextBox10, out jobId) && jobId > 0)
                {
                    // Sửa: lấy bản ghi bất kể Active hay không
                    employeeJob = await _employeeJobRepository
                        .GetByCondition(o => o.ID == jobId)
                        .FirstOrDefaultAsync();
                    employeeJob.EmployeeCode = BaseParameter.TextBox1;
                    employeeJob.EmployeeType = BaseParameter.ComboBox1;
                    employeeJob.Department = BaseParameter.ComboBox2;
                    employeeJob.Position = BaseParameter.ComboBox3;
                    employeeJob.Line = BaseParameter.ComboBox6;
                    employeeJob.Education = BaseParameter.ComboBox4;
                    employeeJob.Specialization = BaseParameter.TextBox3;
                    employeeJob.CompanyEmail = BaseParameter.TextBox4;
                    employeeJob.InterviewDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker1) ? null : DateTime.Parse(BaseParameter.DateTimePicker1);
                    employeeJob.StartDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker2) ? null : DateTime.Parse(BaseParameter.DateTimePicker2);
                    employeeJob.TimeUnit = BaseParameter.ComboBox5;
                    employeeJob.Active = BaseParameter.Active ?? employeeJob.Active;

                    employeeJob.UpdateDate = DateTime.Now;
                    employeeJob.UpdateUser = BaseParameter.USER_IDX;

                    await _employeeJobRepository.UpdateAsync(employeeJob);
                    result.Message = "Cập nhật thông tin công việc thành công";
                }
                else
                {
                    employeeJob = new EmployeeJob
                    {
                        PersonalInfoID = (long)BaseParameter.ID,
                        EmployeeCode = BaseParameter.TextBox1,
                        EmployeeType = BaseParameter.ComboBox1,
                        Department = BaseParameter.ComboBox2,
                        Position = BaseParameter.ComboBox3,
                        Line = BaseParameter.ComboBox6,
                        Education = BaseParameter.ComboBox4,
                        Specialization = BaseParameter.TextBox3,
                        CompanyEmail = BaseParameter.TextBox4,
                        InterviewDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker1) ? null : DateTime.Parse(BaseParameter.DateTimePicker1),
                        StartDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker2) ? null : DateTime.Parse(BaseParameter.DateTimePicker2),
                        TimeUnit = BaseParameter.ComboBox5,
                        Active = BaseParameter.Active ?? true,

                        CreateDate = DateTime.Now,
                        CreateUser = BaseParameter.USER_IDX
                    };

                    await _employeeJobRepository.AddAsync(employeeJob);
                    result.Message = "Thêm thông tin công việc thành công";
                }

                result.Success = true;
                result.Data = employeeJob;
                return result;
            });
        }

        public virtual async Task<BaseResult> DeleteEmployeeJob(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employeeJob = await _employeeJobRepository
                    .GetByCondition(o => o.ID == (long)BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (employeeJob == null)
                {
                    result.Error = "Không tìm thấy thông tin công việc";
                    return result;
                }

                employeeJob.Active = false;
                employeeJob.UpdateDate = DateTime.Now;
                employeeJob.UpdateUser = BaseParameter.USER_IDX;

                await _employeeJobRepository.UpdateAsync(employeeJob);
                result.Success = true;
                result.Message = "Đã xóa thông tin công việc";
                return result;
            });
        }
        public virtual async Task<BaseResult> GetEmployeeContract(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employeeContracts = await _employeeContractRepository
                    .GetByCondition(o => o.PersonalInfoID == (long)BaseParameter.ID && o.Active)
                    .ToListAsync();

                result.Data = employeeContracts;
                result.Success = true;
                return result;
            });
        }
        public virtual async Task<BaseResult> SaveEmployeeContract(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                EmployeeContract employeeContract;
                long contractId = 0;

                if (long.TryParse(BaseParameter.TextBox10, out contractId) && contractId > 0)
                {

                    employeeContract = await _employeeContractRepository.GetByCondition(o => o.ID == contractId).FirstOrDefaultAsync();
                    employeeContract.ContractType = BaseParameter.ComboBox1;
                    employeeContract.ContractDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker1) ? DateTime.Now : DateTime.Parse(BaseParameter.DateTimePicker1);
                    employeeContract.StartDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker2) ? DateTime.Now : DateTime.Parse(BaseParameter.DateTimePicker2);
                    employeeContract.EndDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker3) ? null : (DateTime?)DateTime.Parse(BaseParameter.DateTimePicker3);
                    if (!string.IsNullOrEmpty(BaseParameter.TextBox1))
                    {
                        employeeContract.ContractFile = BaseParameter.TextBox1;
                    }
                    employeeContract.Active = BaseParameter.Active ?? employeeContract.Active;

                    employeeContract.UpdateDate = DateTime.Now;
                    employeeContract.UpdateUser = BaseParameter.USER_IDX;

                    await _employeeContractRepository.UpdateAsync(employeeContract);
                    result.Message = "Cập nhật thông tin hợp đồng thành công";
                }
                else
                {
                    employeeContract = new EmployeeContract
                    {
                        PersonalInfoID = (long)BaseParameter.ID,
                        ContractType = BaseParameter.ComboBox1,
                        ContractDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker1) ? DateTime.Now : DateTime.Parse(BaseParameter.DateTimePicker1),
                        StartDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker2) ? DateTime.Now : DateTime.Parse(BaseParameter.DateTimePicker2),
                        EndDate = string.IsNullOrEmpty(BaseParameter.DateTimePicker3) ? null : (DateTime?)DateTime.Parse(BaseParameter.DateTimePicker3),
                        ContractFile = BaseParameter.TextBox1,
                        Active = BaseParameter.Active ?? true,
                        CreateDate = DateTime.Now,
                        CreateUser = BaseParameter.USER_IDX
                    };

                    await _employeeContractRepository.AddAsync(employeeContract);
                    result.Message = "Thêm thông tin hợp đồng thành công";
                }

                result.Success = true;
                result.Data = employeeContract;
                return result;
            });
        }

        public virtual async Task<BaseResult> DeleteEmployeeContract(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employeeContract = await _employeeContractRepository
                    .GetByCondition(o => o.ID == (long)BaseParameter.ID)
                    .FirstOrDefaultAsync();
                employeeContract.Active = false;
                employeeContract.UpdateDate = DateTime.Now;
                employeeContract.UpdateUser = BaseParameter.USER_IDX;

                await _employeeContractRepository.UpdateAsync(employeeContract);
                result.Success = true;
                result.Message = "Đã xóa thông tin hợp đồng";
                return result;
            });
        }
        public virtual async Task<BaseResult> GetEmployeeFinance(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();
                var employeeFinances = await _employeeFinanceRepository
                    .GetByCondition(o => o.PersonalInfoID == (long)BaseParameter.ID)
                    .OrderByDescending(o => o.CreateDate)
                    .ToListAsync();

                result.Data = employeeFinances;
                result.Success = true;
                return result;
            });
        }
        public virtual async Task<BaseResult> SaveEmployeeFinance(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                EmployeeFinance employeeFinance;
                long financeId = 0;

                if (long.TryParse(BaseParameter.TextBox10, out financeId) && financeId > 0)
                {
                    employeeFinance = await _employeeFinanceRepository.GetByCondition(o => o.ID == financeId).FirstOrDefaultAsync();
                    employeeFinance.InsuranceCode = BaseParameter.TextBox1;
                    employeeFinance.TaxCode = BaseParameter.TextBox2;
                    employeeFinance.BankName = BaseParameter.TextBox3;
                    employeeFinance.BankAccount = BaseParameter.TextBox4;
                    employeeFinance.Active = BaseParameter.Active ?? employeeFinance.Active;

                    employeeFinance.UpdateDate = DateTime.Now;
                    employeeFinance.UpdateUser = BaseParameter.USER_IDX;

                    await _employeeFinanceRepository.UpdateAsync(employeeFinance);
                    result.Message = "Cập nhật thông tin tài chính thành công";
                }
                else
                {
                    employeeFinance = new EmployeeFinance
                    {
                        PersonalInfoID = (long)BaseParameter.ID,
                        InsuranceCode = BaseParameter.TextBox1,
                        TaxCode = BaseParameter.TextBox2,
                        BankName = BaseParameter.TextBox3,
                        BankAccount = BaseParameter.TextBox4,
                        Active = BaseParameter.Active ?? true,
                        CreateDate = DateTime.Now,
                        CreateUser = BaseParameter.USER_IDX
                    };

                    await _employeeFinanceRepository.AddAsync(employeeFinance);
                    result.Message = "Thêm thông tin tài chính thành công";
                }

                result.Success = true;
                result.Data = employeeFinance;
                return result;
            });
        }
        public virtual async Task<BaseResult> DeleteEmployeeFinance(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employeeFinance = await _employeeFinanceRepository
                    .GetByCondition(o => o.ID == (long)BaseParameter.ID)
                    .FirstOrDefaultAsync();
                employeeFinance.Active = false;
                employeeFinance.UpdateDate = DateTime.Now;
                employeeFinance.UpdateUser = BaseParameter.USER_IDX;

                await _employeeFinanceRepository.UpdateAsync(employeeFinance);
                result.Success = true;
                result.Message = "Đã xóa thông tin tài chính";
                return result;
            });
        }
        public virtual async Task<BaseResult> GetEmployeeFiles(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();
                var employeeFiles = await _employeeFileRepository
                    .GetByCondition(o => o.PersonalInfoID == (long)BaseParameter.ID)
                    .OrderByDescending(o => o.CreateDate)
                    .ToListAsync();

                result.Data = employeeFiles;
                result.Success = true;
                return result;
            });
        }
        public virtual async Task<BaseResult> SaveEmployeeFile(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                EmployeeFile employeeFile;
                long fileId = 0;

                if (long.TryParse(BaseParameter.TextBox10, out fileId) && fileId > 0)
                {
                    employeeFile = await _employeeFileRepository.GetByCondition(o => o.ID == fileId).FirstOrDefaultAsync();
                    employeeFile.FileName = BaseParameter.ComboBox1;
                    employeeFile.FileType = BaseParameter.ComboBox2;
                    if (!string.IsNullOrEmpty(BaseParameter.TextBox1))
                    {
                        employeeFile.FilePath = BaseParameter.TextBox1;
                        employeeFile.OriginalFileName = BaseParameter.TextBox2;
                    }
                    employeeFile.Active = BaseParameter.Active ?? employeeFile.Active;

                    employeeFile.UpdateDate = DateTime.Now;
                    employeeFile.UpdateUser = BaseParameter.USER_IDX;

                    await _employeeFileRepository.UpdateAsync(employeeFile);
                    result.Message = "Cập nhật thông tin tài liệu thành công";
                }
                else
                {
                    employeeFile = new EmployeeFile
                    {
                        PersonalInfoID = (long)BaseParameter.ID,
                        FileName = BaseParameter.ComboBox1,
                        FileType = BaseParameter.ComboBox2,
                        FilePath = BaseParameter.TextBox1,
                        OriginalFileName = BaseParameter.TextBox2,
                        Active = BaseParameter.Active ?? true,

                        CreateDate = DateTime.Now,
                        CreateUser = BaseParameter.USER_IDX
                    };

                    await _employeeFileRepository.AddAsync(employeeFile);
                    result.Message = "Thêm tài liệu thành công";
                }

                result.Success = true;
                result.Data = employeeFile;
                return result;
            });
        }
        public virtual async Task<BaseResult> DeleteEmployeeFile(BaseParameter BaseParameter)
        {
            return await ExecuteServiceOperation(async () =>
            {
                var result = new BaseResult();

                var employeeFile = await _employeeFileRepository
                    .GetByCondition(o => o.ID == (long)BaseParameter.ID)
                    .FirstOrDefaultAsync();
                employeeFile.Active = false;
                employeeFile.UpdateDate = DateTime.Now;
                employeeFile.UpdateUser = BaseParameter.USER_IDX;

                await _employeeFileRepository.UpdateAsync(employeeFile);
                result.Success = true;
                result.Message = "Đã xóa tài liệu";
                return result;
            });
        }
    }
}