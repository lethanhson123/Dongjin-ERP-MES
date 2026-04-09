namespace MESService.Implement
{
    public class HR1Service : BaseService<EmployeeFA, IEmployeeFARepository>, IHR1Service
    {
        private readonly IEmployeeFARepository _employeeFARepository;
        private readonly ILineListRepository _lineListRepository;
        private readonly IShiftTimeRepository _shiftTimeRepository;
        private readonly IAttendanceRecordsRepository _attendanceRepository; 

        public HR1Service(
            IEmployeeFARepository employeeFARepository,
            ILineListRepository lineListRepository,
            IShiftTimeRepository shiftTimeRepository,
            IAttendanceRecordsRepository attendanceRepository 
        ) : base(employeeFARepository)
        {
            _employeeFARepository = employeeFARepository;
            _lineListRepository = lineListRepository;
            _shiftTimeRepository = shiftTimeRepository;
            _attendanceRepository = attendanceRepository; 
        }

        public override void Initialization(EmployeeFA model)
        {
            BaseInitialization(model);
        }

        // ========== SỬA METHOD Load ==========
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result.LineList = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.LineName)
                    .ToListAsync();

              
                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.ID)
                    .ToListAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

       
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _employeeFARepository.GetByCondition(x => x.Active == true);

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString.ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(search) ||
                        x.EmpCode.ToLower().Contains(search) ||
                        x.Dept.ToLower().Contains(search) ||
                        x.Line.ToLower().Contains(search) ||
                        (x.Process != null && x.Process.ToLower().Contains(search))
                    );
                }

                result.EmployeeFAList = await query
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.LineList = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.LineName)
                    .ToListAsync();

              
                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.ID)
                    .ToListAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }


        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var existingEmpCode = await _employeeFARepository
                    .GetByCondition(x => x.EmpCode == BaseParameter.EmployeeFA.EmpCode)
                    .FirstOrDefaultAsync();

                if (existingEmpCode != null)
                {
                    if (existingEmpCode.Active == false)
                    {
                        // Tự động khôi phục và cập nhật thông tin
                        existingEmpCode.Active = true;
                        existingEmpCode.Name = BaseParameter.EmployeeFA.Name;
                        existingEmpCode.Dept = BaseParameter.EmployeeFA.Dept;
                        existingEmpCode.Line = BaseParameter.EmployeeFA.Line;
                        existingEmpCode.Process = BaseParameter.EmployeeFA.Process;
                        existingEmpCode.DefaultShiftID = BaseParameter.EmployeeFA.DefaultShiftID;
                        existingEmpCode.HireDate = BaseParameter.EmployeeFA.HireDate;
                        existingEmpCode.UpdateDate = DateTime.Now;
                        existingEmpCode.UpdateUser = BaseParameter.USER_IDX;

                        await _employeeFARepository.UpdateAsync(existingEmpCode);

                        result.EmployeeFAList = new List<EmployeeFA> { existingEmpCode };
                        result.Success = true;
                        result.Message = "Khôi phục và cập nhật nhân viên thành công.";
                        return result;
                    }
                    else
                    {
                        result.Error = "Mã nhân viên đã tồn tại!";
                        return result;
                    }
                }

                var employee = BaseParameter.EmployeeFA;
                employee.Active = true;
                employee.CreateDate = DateTime.Now;
                employee.CreateUser = BaseParameter.USER_IDX;

                if (!employee.HireDate.HasValue && !string.IsNullOrWhiteSpace(employee.EmpCode))
                {
                    employee.HireDate = ParseHireDateFromEmpCode(employee.EmpCode);
                }

                await _employeeFARepository.AddAsync(employee);

                result.EmployeeFAList = new List<EmployeeFA> { employee };
                result.Success = true;
                result.Message = "Thêm nhân viên thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var existingEmployee = await _employeeFARepository
                    .GetByCondition(x => x.ID == BaseParameter.EmployeeFA.ID)
                    .FirstOrDefaultAsync();

                if (existingEmployee == null)
                {
                    result.Error = "Không tìm thấy nhân viên.";
                    return result;
                }

                var duplicateEmpCode = await _employeeFARepository
                    .GetByCondition(x =>
                        x.EmpCode == BaseParameter.EmployeeFA.EmpCode &&
                        x.ID != BaseParameter.EmployeeFA.ID &&
                        x.Active == true)
                    .FirstOrDefaultAsync();

                if (duplicateEmpCode != null)
                {
                    result.Error = "Mã nhân viên đã tồn tại!";
                    return result;
                }

                var oldShiftID = existingEmployee.DefaultShiftID;
                var newShiftID = BaseParameter.EmployeeFA.DefaultShiftID;

                existingEmployee.Name = BaseParameter.EmployeeFA.Name;
                existingEmployee.Dept = BaseParameter.EmployeeFA.Dept;
                existingEmployee.Line = BaseParameter.EmployeeFA.Line;
                existingEmployee.Process = BaseParameter.EmployeeFA.Process;
                existingEmployee.EmpCode = BaseParameter.EmployeeFA.EmpCode;
                existingEmployee.DefaultShiftID = newShiftID;
                existingEmployee.HireDate = BaseParameter.EmployeeFA.HireDate; 
                existingEmployee.UpdateDate = DateTime.Now;
                existingEmployee.UpdateUser = BaseParameter.USER_IDX;

                await _employeeFARepository.UpdateAsync(existingEmployee);

                if (oldShiftID != newShiftID && newShiftID.HasValue)
                {
                    var pendingAttendances = await _attendanceRepository
                        .GetByCondition(x =>
                            x.EmployeeFAID == existingEmployee.ID &&
                            x.ScanOut == null &&
                            x.Active == true)
                        .ToListAsync();

                    foreach (var attendance in pendingAttendances)
                    {
                        attendance.ShiftID = newShiftID;
                        attendance.UpdateDate = DateTime.Now;
                        attendance.UpdateUser = BaseParameter.USER_IDX;
                        await _attendanceRepository.UpdateAsync(attendance);
                    }

                    if (pendingAttendances.Any())
                    {
                        result.Message = $"Cập nhật nhân viên thành công. Đã tự động cập nhật ca cho {pendingAttendances.Count} lần chấm công chưa hoàn thành.";
                    }
                    else
                    {
                        result.Message = "Cập nhật nhân viên thành công.";
                    }
                }
                else
                {
                    result.Message = "Cập nhật nhân viên thành công.";
                }

                result.EmployeeFAList = new List<EmployeeFA> { existingEmployee };
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                int successCount = 0;
                int skipCount = 0;
                int updateCount = 0;

                foreach (var item in BaseParameter.EmployeeFAList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Name) ||
                            string.IsNullOrWhiteSpace(item.EmpCode) ||
                            string.IsNullOrWhiteSpace(item.Dept) ||
                            string.IsNullOrWhiteSpace(item.Line))
                        {
                            skipCount++;
                            continue;
                        }

                        var existing = await _employeeFARepository
                            .GetByCondition(x => x.EmpCode == item.EmpCode && x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            existing.Name = item.Name;
                            existing.Dept = item.Dept;
                            existing.Line = item.Line;
                            existing.Process = item.Process;
                            existing.DefaultShiftID = item.DefaultShiftID;
                            existing.HireDate = item.HireDate;

                            if (!existing.HireDate.HasValue)
                            {
                                existing.HireDate = ParseHireDateFromEmpCode(existing.EmpCode);
                            }

                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateUser = BaseParameter.USER_IDX;
                            await _employeeFARepository.UpdateAsync(existing);
                            updateCount++;
                        }
                        else
                        {
                            item.Active = true;
                            item.CreateDate = DateTime.Now;
                            item.CreateUser = BaseParameter.USER_IDX ?? "IMPORT";

                            if (!item.HireDate.HasValue)
                            {
                                item.HireDate = ParseHireDateFromEmpCode(item.EmpCode);
                            }

                            await _employeeFARepository.AddAsync(item);
                            successCount++;
                        }
                    }
                    catch (Exception)
                    {
                        skipCount++;
                        continue;
                    }
                }

                result.EmployeeFAList = await _employeeFARepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.Success = (successCount + updateCount) > 0;
                result.Message = $"Import thành công! Thêm mới: {successCount}, Cập nhật: {updateCount}, Bỏ qua: {skipCount}";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }


        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var employee = await _employeeFARepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Không tìm thấy nhân viên.";
                    return result;
                }

                employee.Active = false;
                employee.UpdateDate = DateTime.Now;
                employee.UpdateUser = BaseParameter.USER_IDX;

                await _employeeFARepository.UpdateAsync(employee);

                result.EmployeeFAList = await _employeeFARepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.Success = true;
                result.Message = "Xóa nhân viên thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> BulkAssignShift(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (string.IsNullOrEmpty(BaseParameter.LineToAssign))
                {
                    result.Error = "Vui lòng chọn Line!";
                    return result;
                }

                if (BaseParameter.ShiftID == null)
                {
                    result.Error = "Vui lòng chọn ca!";
                    return result;
                }

                // Lấy tất cả nhân viên trong Line
                var employees = await _employeeFARepository
                    .GetByCondition(x => x.Line == BaseParameter.LineToAssign && x.Active == true)
                    .ToListAsync();

                if (!employees.Any())
                {
                    result.Error = "Không có nhân viên nào trong Line này!";
                    return result;
                }

                // Update ca cho tất cả
                foreach (var emp in employees)
                {
                    emp.DefaultShiftID = BaseParameter.ShiftID;
                    emp.UpdateDate = DateTime.Now;
                    emp.UpdateUser = BaseParameter.USER_IDX;
                    await _employeeFARepository.UpdateAsync(emp);
                }

                result.Success = true;
                result.Message = $"Đã phân ca cho {employees.Count} nhân viên trong Line '{BaseParameter.LineToAssign}'";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter) 
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private DateTime? ParseHireDateFromEmpCode(string empCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empCode) || empCode.Length < 8)
                    return null;

                string yy = empCode.Substring(0, 2);
                string mm = empCode.Substring(2, 2);
                string dd = empCode.Substring(4, 2);

                int year = 2000 + int.Parse(yy);
                int month = int.Parse(mm);
                int day = int.Parse(dd);

                if (month < 1 || month > 12 || day < 1 || day > 31)
                    return null;

                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }
        }
    }
}