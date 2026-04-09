namespace MESService.Implement
{
    public class FA_M2Service : BaseService<LineAssignment, ILineAssignmentRepository>, IFA_M2Service
    {
        private readonly ILineAssignmentRepository _lineAssignmentRepository;
        private readonly IEmployeeFARepository _employeeFARepository;
        private readonly ILineListRepository _lineListRepository;
        private readonly IShiftTimeRepository _shiftTimeRepository;

        public FA_M2Service(
            ILineAssignmentRepository lineAssignmentRepository,
            IEmployeeFARepository employeeFARepository,
            ILineListRepository lineListRepository,
            IShiftTimeRepository shiftTimeRepository
        ) : base(lineAssignmentRepository)
        {
            _lineAssignmentRepository = lineAssignmentRepository;
            _employeeFARepository = employeeFARepository;
            _lineListRepository = lineListRepository;
            _shiftTimeRepository = shiftTimeRepository;
        }

        public override void Initialization(LineAssignment model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await AutoAssignEmployeesToLines();

                result.EmployeeFAList = await _employeeFARepository
                    .GetByCondition(e => e.Dept == "FA" && e.Active == true)
                    .ToListAsync();

                result.LineList = await _lineListRepository
                    .GetByCondition(l => l.Active == true)
                    .OrderBy(l => l.LineName)
                    .ToListAsync();

                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(s => s.Active == true)
                    .ToListAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }

        private async Task AutoAssignEmployeesToLines()
        {
            try
            {
                var allEmployees = await _employeeFARepository
                    .GetByCondition(e => e.Dept == "FA" && e.Active == true)
                    .ToListAsync();

                var allAssignments = await _lineAssignmentRepository
                    .GetByCondition(la => la.Active == true)
                    .ToListAsync();

                var allShifts = await _shiftTimeRepository
                    .GetByCondition(s => s.Active == true)
                    .ToListAsync();

                var morningShift = allShifts.FirstOrDefault(s =>
                    s.ShiftName.ToLower().Contains("ca sáng") ||
                    s.ShiftName.ToLower().Contains("ca sang") ||
                    s.ShiftName.ToLower() == "sáng" ||
                    s.ShiftName.ToLower() == "sang"
                );

                if (morningShift == null) return;

                var allLines = await _lineListRepository
                    .GetByCondition(l => l.Active == true)
                    .ToListAsync();

                DateTime today = DateTime.Now.Date;

                foreach (var employee in allEmployees)
                {
                    var empAssignments = allAssignments.Where(a => a.EmployeeID == employee.ID).ToList();

                    bool hasPrimaryShift = empAssignments.Any(assignment =>
                    {
                        var shift = allShifts.FirstOrDefault(s => s.ID == assignment.ShiftID);
                        if (shift == null) return false;

                        string shiftName = shift.ShiftName.ToLower();
                        bool isOvertimeShift = shiftName.Contains("tăng ca") || shiftName.Contains("tc");
                        return !isOvertimeShift;
                    });

                    if (hasPrimaryShift) continue;
                    if (string.IsNullOrEmpty(employee.Line)) continue;

                    var matchedLine = allLines.FirstOrDefault(line =>
                        line.LineName == employee.Line ||
                        (line.LineGroup != null && line.Family != null &&
                         $"{line.LineGroup} - {line.LineName} - {line.Family}" == employee.Line) ||
                        (line.LineGroup != null &&
                         $"{line.LineGroup} - {line.LineName}" == employee.Line)
                    );

                    if (matchedLine == null) continue;

                    bool alreadyAssignedToday = allAssignments.Any(a =>
                        a.EmployeeID == employee.ID &&
                        a.LineID == matchedLine.ID &&
                        a.ShiftID == morningShift.ID &&
                        a.StartDate <= today &&
                        (a.EndDate == null || a.EndDate >= today)
                    );

                    if (alreadyAssignedToday) continue;

                    var newAssignment = new LineAssignment
                    {
                        EmployeeID = employee.ID,
                        LineID = matchedLine.ID,
                        ShiftID = morningShift.ID,
                        StartDate = today,
                        EndDate = today,
                        Description = "Tự động phân công",
                        Active = true,
                        CreateDate = DateTime.Now,
                        CreateUser = "SYSTEM"
                    };

                    await _lineAssignmentRepository.AddAsync(newAssignment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Auto-assign error: {ex.Message}");
            }
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _employeeFARepository.GetByCondition(e => e.Dept == "FA" && e.Active == true);

                if (!string.IsNullOrEmpty(parameter.SearchString))
                {
                    string searchText = parameter.SearchString.ToLower();
                    query = query.Where(e =>
                        e.Name.ToLower().Contains(searchText) ||
                        e.EmpCode.ToLower().Contains(searchText) ||
                        (e.Line != null && e.Line.ToLower().Contains(searchText)));
                }

                result.EmployeeFAList = await query.ToListAsync();

                var employeeIds = result.EmployeeFAList.Select(e => e.ID).ToList();

                result.LineList = await _lineListRepository
                    .GetByCondition(l => l.Active == true)
                    .OrderBy(l => l.LineName)
                    .ToListAsync();

                var lineAssignments = await _lineAssignmentRepository
                    .GetByCondition(la => employeeIds.Contains(la.EmployeeID) && la.Active == true)
                    .ToListAsync();

                var shiftIds = lineAssignments.Select(la => la.ShiftID).Distinct().ToList();

                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(s => shiftIds.Contains(s.ID))
                    .ToListAsync();

                result.LineAssignments = lineAssignments;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                long employeeId = parameter.ID ?? 0;

                var employee = await _employeeFARepository
                    .GetByCondition(e => e.ID == employeeId)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Không tìm thấy nhân viên";
                    result.Success = false;
                    return result;
                }

                result.LineList = await _lineListRepository
                    .GetByCondition(l => l.Active == true)
                    .OrderBy(l => l.LineName)
                    .ToListAsync();

                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(s => s.Active == true)
                    .ToListAsync();

                result.Data = new
                {
                    ID = employee.ID,
                    EmpCode = employee.EmpCode,
                    Name = employee.Name,
                    Line = employee.Line ?? ""
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                long employeeId = long.Parse(parameter.UserID);
                long lineId = long.Parse(parameter.Code);
                long shiftId = long.Parse(parameter.GroupCode);
                DateTime startDate = DateTime.Parse(parameter.StartDate);
                DateTime? endDate = !string.IsNullOrEmpty(parameter.EndDate) ?
                    DateTime.Parse(parameter.EndDate) : (DateTime?)null;
                string description = parameter.Remark ?? "";

                long assignmentId = parameter.ID ?? 0;

                var existingAssignments = await _lineAssignmentRepository
                    .GetByCondition(la =>
                        la.EmployeeID == employeeId &&
                        la.Active == true &&
                        la.ID != assignmentId)
                    .ToListAsync();

                foreach (var existing in existingAssignments)
                {
                    if (existing.LineID == lineId && existing.ShiftID == shiftId)
                    {
                        DateTime existingStart = existing.StartDate;
                        DateTime existingEnd = existing.EndDate ?? DateTime.MaxValue;
                        DateTime newStart = startDate;
                        DateTime newEnd = endDate ?? DateTime.MaxValue;

                        if (newStart <= existingEnd && newEnd >= existingStart)
                        {
                            result.Error = "Nhân viên đã được phân công Line và Ca này trong khoảng thời gian trùng lặp";
                            result.Success = false;
                            return result;
                        }
                    }
                }

                if (assignmentId > 0)
                {
                    var assignment = await _lineAssignmentRepository
                        .GetByCondition(la => la.ID == assignmentId)
                        .FirstOrDefaultAsync();

                    if (assignment == null)
                    {
                        result.Error = "Không tìm thấy phân công";
                        result.Success = false;
                        return result;
                    }

                    assignment.LineID = lineId;
                    assignment.ShiftID = shiftId;
                    assignment.StartDate = startDate;
                    assignment.EndDate = endDate;
                    assignment.Description = description;
                    assignment.UpdateDate = DateTime.Now;
                    assignment.UpdateUser = parameter.USER_IDX;

                    await _lineAssignmentRepository.UpdateAsync(assignment);
                    result.Message = "Cập nhật phân công thành công";
                }
                else
                {
                    var assignment = new LineAssignment
                    {
                        EmployeeID = employeeId,
                        LineID = lineId,
                        ShiftID = shiftId,
                        StartDate = startDate,
                        EndDate = endDate,
                        Description = description,
                        Active = true,
                        CreateDate = DateTime.Now,
                        CreateUser = parameter.USER_IDX
                    };

                    await _lineAssignmentRepository.AddAsync(assignment);
                    result.Message = "Thêm mới phân công thành công";
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                long assignmentId = parameter.ID ?? 0;

                if (assignmentId <= 0)
                {
                    result.Error = "ID phân công không hợp lệ";
                    result.Success = false;
                    return result;
                }

                var assignment = await _lineAssignmentRepository
                    .GetByCondition(la => la.ID == assignmentId)
                    .FirstOrDefaultAsync();

                if (assignment == null)
                {
                    result.Error = "Không tìm thấy phân công";
                    result.Success = false;
                    return result;
                }

                assignment.Active = false;
                assignment.UpdateDate = DateTime.Now;
                assignment.UpdateUser = parameter.USER_IDX;

                await _lineAssignmentRepository.UpdateAsync(assignment);

                result.Message = "Xóa phân công thành công";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
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

        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
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

        public virtual async Task<BaseResult> GetAssignment(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                long assignmentId = parameter.ID ?? 0;

                var assignment = await _lineAssignmentRepository
                    .GetByCondition(la => la.ID == assignmentId && la.Active == true)
                    .FirstOrDefaultAsync();

                if (assignment == null)
                {
                    result.Error = "Không tìm thấy phân công";
                    result.Success = false;
                    return result;
                }

                var employee = await _employeeFARepository
                    .GetByCondition(e => e.ID == assignment.EmployeeID)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Không tìm thấy thông tin nhân viên";
                    result.Success = false;
                    return result;
                }

                result.Data = new
                {
                    ID = assignment.ID,
                    EmployeeID = assignment.EmployeeID,
                    LineID = assignment.LineID,
                    ShiftID = assignment.ShiftID,
                    StartDate = assignment.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = assignment.EndDate?.ToString("yyyy-MM-dd"),
                    Description = assignment.Description ?? ""
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetLineHistory(BaseParameter parameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                long lineId = parameter.ID ?? 0;

                if (lineId <= 0)
                {
                    result.Error = "ID dây chuyền không hợp lệ";
                    result.Success = false;
                    return result;
                }

                var lineAssignments = await _lineAssignmentRepository
                    .GetByCondition(la => la.LineID == lineId)
                    .OrderByDescending(la => la.Active)
                    .ThenByDescending(la => la.StartDate)
                    .ToListAsync();

                if (lineAssignments.Count == 0)
                {
                    result.Success = true;
                    return result;
                }

                var employeeIds = lineAssignments.Select(la => la.EmployeeID).Distinct().ToList();
                var shiftIds = lineAssignments.Select(la => la.ShiftID).Distinct().ToList();

                result.EmployeeFAList = await _employeeFARepository
                    .GetByCondition(e => employeeIds.Contains(e.ID))
                    .ToListAsync();

                result.ShiftTimes = await _shiftTimeRepository
                    .GetByCondition(s => shiftIds.Contains(s.ID))
                    .ToListAsync();

                result.LineAssignments = lineAssignments;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }
    }
}