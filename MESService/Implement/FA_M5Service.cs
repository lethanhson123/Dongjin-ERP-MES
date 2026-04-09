namespace MESService.Implement
{
    public class FA_M5Service : BaseService<AttendanceRecords, IAttendanceRecordsRepository>, IFA_M5Service
    {
        private readonly IAttendanceRecordsRepository _attendanceRepository;
        private readonly IEmployeeFARepository _employeeRepository;
        private readonly ILineListRepository _lineRepository;
        private readonly IShiftTimeRepository _shiftRepository;
        private readonly IDowntimeRecordsRepository _downtimeRepository;

        public FA_M5Service(
            IAttendanceRecordsRepository attendanceRepository,
            IEmployeeFARepository employeeRepository,
            ILineListRepository lineRepository,
            IShiftTimeRepository shiftRepository,
            IDowntimeRecordsRepository downtimeRepository
        ) : base(attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _lineRepository = lineRepository;
            _shiftRepository = shiftRepository;
            _downtimeRepository = downtimeRepository;
        }

        public override void Initialization(AttendanceRecords model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            var result = new BaseResult();
            try
            {
                result.LineList = await _lineRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.LineName)
                    .ToListAsync();

                result.ShiftTimes = await _shiftRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.StartTime)
                    .ToListAsync();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> ScanIn_Click(BaseParameter param)
        {
            var result = new BaseResult();
            try
            {
                var empCode = param.SearchString?.Trim();
                var line = param.Line?.Trim();
                var entryMethod = param.EntryMethod ?? "QR";

                var employee = await _employeeRepository
                    .GetByCondition(x => x.EmpCode == empCode && x.Active == true)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Mã nhân viên không tồn tại!";
                    return result;
                }

                if (!employee.DefaultShiftID.HasValue)
                {
                    result.Error = $"Nhân viên '{employee.Name}' chưa được xếp ca làm việc!";
                    return result;
                }

                var today = DateTime.Today;
                var existingAttendance = await _attendanceRepository
                    .GetByCondition(x =>
                        x.EmployeeFAID == employee.ID &&
                        x.AttendanceDate == today &&
                        x.Status.StartsWith("ScanIn") &&
                        x.Active == true)
                    .FirstOrDefaultAsync();

                if (existingAttendance != null)
                {
                    if (existingAttendance.Line == line)
                    {
                        result.Error = $"Nhân viên '{employee.Name}' đã quét vào Line '{line}' rồi!";
                        return result;
                    }
                    else
                    {
                        result.Error = $"Nhân viên '{employee.Name}' đang làm việc tại Line '{existingAttendance.Line}'!\nVui lòng quét ra trước khi chuyển Line.";
                        return result;
                    }
                }

                var status = entryMethod == "Manual" ? "ScanIn_Manual" : "ScanIn";
                var attendance = new AttendanceRecords
                {
                    EmployeeFAID = employee.ID,
                    Line = line,
                    ShiftID = employee.DefaultShiftID,
                    ScanIn = DateTime.Now,
                    AttendanceDate = today,
                    Status = status,
                    TotalDownTime = 0,
                    Active = true,
                    CreateDate = DateTime.Now,
                    CreateUser = param.USER_IDX
                };

                await _attendanceRepository.AddAsync(attendance);

                var shift = await _shiftRepository
                    .GetByCondition(x => x.ID == employee.DefaultShiftID.Value)
                    .FirstOrDefaultAsync();

                var shiftInfo = shift != null ? $" - Ca: {shift.ShiftName}" : "";
                var methodText = entryMethod == "Manual" ? " (Nhập tay)" : "";

                result.Success = true;
                result.Message = $"Quét vào thành công{methodText}!\n{employee.Name} - Line: {line}{shiftInfo}";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> ScanOut_Click(BaseParameter param)
        {
            var result = new BaseResult();
            try
            {
                var empCode = param.SearchString?.Trim();
                var entryMethod = param.EntryMethod ?? "QR";

                var employee = await _employeeRepository
                    .GetByCondition(x => x.EmpCode == empCode && x.Active == true)
                    .FirstOrDefaultAsync();

                if (employee == null)
                {
                    result.Error = "Mã nhân viên không tồn tại!";
                    return result;
                }

                var today = DateTime.Today;
                var attendance = await _attendanceRepository
                    .GetByCondition(x =>
                        x.EmployeeFAID == employee.ID &&
                        x.AttendanceDate == today &&
                        x.Status.StartsWith("ScanIn") &&
                        x.Active == true)
                    .FirstOrDefaultAsync();

                if (attendance == null)
                {
                    result.Error = "Nhân viên chưa quét vào hôm nay!";
                    return result;
                }

                attendance.ScanOut = DateTime.Now;

                var breakTime = 40m;
                DateTime? shiftStartTime = null;
                DateTime? shiftEndTime = null;

                if (attendance.ShiftID.HasValue)
                {
                    var shift = await _shiftRepository
                        .GetByCondition(x => x.ID == attendance.ShiftID.Value)
                        .FirstOrDefaultAsync();

                    if (shift != null)
                    {
                        if (shift.BreakTime.HasValue)
                            breakTime = shift.BreakTime.Value;

                        if (shift.StartTime != default(TimeSpan))
                            shiftStartTime = attendance.AttendanceDate.Value.Add(shift.StartTime);

                        if (shift.EndTime != default(TimeSpan))
                            shiftEndTime = attendance.AttendanceDate.Value.Add(shift.EndTime);
                    }
                }

                var effectiveScanIn = attendance.ScanIn.Value;
                if (shiftStartTime.HasValue && attendance.ScanIn.Value < shiftStartTime.Value)
                    effectiveScanIn = shiftStartTime.Value;

                var effectiveScanOut = attendance.ScanOut.Value;
                if (shiftEndTime.HasValue && attendance.ScanOut.Value > shiftEndTime.Value)
                    effectiveScanOut = shiftEndTime.Value;

                var totalMinutes = (decimal)(effectiveScanOut - effectiveScanIn).TotalMinutes;

                var downtimes = await _downtimeRepository
                    .GetByCondition(x =>
                        x.Line == attendance.Line &&
                        x.EndTime != null &&
                        x.StartTime < effectiveScanOut &&
                        x.EndTime > effectiveScanIn)
                    .ToListAsync();

                decimal totalDownTime = 0;
                foreach (var dt in downtimes)
                {
                    var overlapStart = dt.StartTime > effectiveScanIn ? dt.StartTime.Value : effectiveScanIn;
                    var overlapEnd = dt.EndTime < effectiveScanOut ? dt.EndTime.Value : effectiveScanOut;
                    if (overlapEnd > overlapStart)
                    {
                        totalDownTime += (decimal)(overlapEnd - overlapStart).TotalMinutes;
                    }
                }

                attendance.TotalDownTime = totalDownTime;
                attendance.WorkingTime = totalMinutes - breakTime - totalDownTime;

                if (attendance.WorkingTime < 0)
                    attendance.WorkingTime = 0;

                // ✅ ÁP DỤNG HỆ SỐ NHÂN
                if (employee.HireDate.HasValue)
                {
                    int workingDays = (today - employee.HireDate.Value.Date).Days + 1;

                    if (workingDays <= 3)
                    {
                        attendance.WorkingTime = 0;
                    }
                    else if (workingDays <= 6)
                    {
                        attendance.WorkingTime *= 0.5m;
                    }
                    else if (workingDays <= 9)
                    {
                        attendance.WorkingTime *= 0.75m;
                    }
                }

                attendance.Status = entryMethod == "Manual" ? "ScanOut_Manual" : "ScanOut";
                attendance.UpdateDate = DateTime.Now;
                attendance.UpdateUser = param.USER_IDX;

                await _attendanceRepository.UpdateAsync(attendance);

                // ✅ THÔNG BÁO RÚT GỌN
                var hours = (int)(attendance.WorkingTime / 60);
                var minutes = (int)(attendance.WorkingTime % 60);
                var timeDisplay = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";

                result.Success = true;
                result.Message = $"✓ {employee.Name} - {timeDisplay}";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> GetStaffList(BaseParameter param)
        {
            var result = new BaseResult();
            try
            {
                DateTime targetDate = DateTime.Today;

                if (!string.IsNullOrEmpty(param.SelectedDate))
                {
                    if (DateTime.TryParse(param.SelectedDate, out DateTime parsedDate))
                    {
                        targetDate = parsedDate.Date;
                    }
                }

                var line = param.Line;

                var query = _attendanceRepository
                    .GetByCondition(x => x.AttendanceDate == targetDate && x.Active == true);

                if (!string.IsNullOrEmpty(line))
                    query = query.Where(x => x.Line == line);

                var attendanceList = await query.OrderBy(x => x.ScanIn).ToListAsync();

                var employeeIds = attendanceList.Select(x => x.EmployeeFAID).Distinct().ToList();
                var employees = await _employeeRepository
                    .GetByCondition(x => employeeIds.Contains(x.ID))
                    .ToListAsync();

                var shiftIds = attendanceList.Where(x => x.ShiftID.HasValue)
                    .Select(x => x.ShiftID.Value).Distinct().ToList();
                var shifts = await _shiftRepository
                    .GetByCondition(x => shiftIds.Contains(x.ID))
                    .ToListAsync();

                result.AttendanceRecordsList = attendanceList;
                result.EmployeeFAList = employees;
                result.ShiftTimes = shifts;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> StartDowntime(BaseParameter param)
        {
            var result = new BaseResult();
            try
            {
                var line = param.Line?.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    result.Error = "Vui lòng chọn Line!";
                    return result;
                }

                var openDowntime = await _downtimeRepository
                    .GetByCondition(x => x.Line == line && x.EndTime == null)
                    .FirstOrDefaultAsync();

                if (openDowntime != null)
                {
                    result.Error = $"Line '{line}' đang có downtime mở, vui lòng kết thúc trước!";
                    return result;
                }

                var downtime = new DowntimeRecords
                {
                    Line = line,
                    StartTime = DateTime.Now,
                    EndTime = null,
                    Duration = null
                };

                await _downtimeRepository.AddAsync(downtime);

                result.Success = true;
                result.Message = $"Bắt đầu downtime cho Line '{line}' thành công!";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> EndDowntime(BaseParameter param)
        {
            var result = new BaseResult();
            try
            {
                var line = param.Line?.Trim();

                if (string.IsNullOrEmpty(line))
                {
                    result.Error = "Vui lòng chọn Line!";
                    return result;
                }

                var downtime = await _downtimeRepository
                    .GetByCondition(x => x.Line == line && x.EndTime == null)
                    .FirstOrDefaultAsync();

                if (downtime == null)
                {
                    result.Error = $"Line '{line}' không có downtime nào đang mở!";
                    return result;
                }

                downtime.EndTime = DateTime.Now;
                downtime.Duration = (decimal)(downtime.EndTime.Value - downtime.StartTime.Value).TotalMinutes;

                await _downtimeRepository.UpdateAsync(downtime);

                var hours = (int)(downtime.Duration / 60);
                var minutes = (int)(downtime.Duration % 60);
                var timeDisplay = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";

                result.Success = true;
                result.Message = $"Kết thúc downtime Line '{line}' thành công!\nThời gian: {timeDisplay}";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi: {ex.Message}";
            }
            return result;
        }
        public virtual async Task<BaseResult> EditShift(BaseParameter param)
        {
            BaseResult result = new BaseResult();
            try
            {
                var attendance = await _attendanceRepository
                    .GetByCondition(x => x.ID == param.ID && x.Active == true)
                    .FirstOrDefaultAsync();

                var newShift = await _shiftRepository
                    .GetByCondition(x => x.ID == param.ShiftID.Value && x.Active == true)
                    .FirstOrDefaultAsync();

                if (attendance.ScanOut.HasValue)
                {
                    var breakTime = newShift.BreakTime ?? 40m;
                    DateTime? shiftStartTime = newShift.StartTime != default(TimeSpan)
                        ? attendance.AttendanceDate.Value.Add(newShift.StartTime)
                        : null;
                    DateTime? shiftEndTime = newShift.EndTime != default(TimeSpan)
                        ? attendance.AttendanceDate.Value.Add(newShift.EndTime)
                        : null;

                    var effectiveScanIn = attendance.ScanIn.Value;
                    if (shiftStartTime.HasValue && attendance.ScanIn.Value < shiftStartTime.Value)
                        effectiveScanIn = shiftStartTime.Value;

                    var effectiveScanOut = attendance.ScanOut.Value;
                    if (shiftEndTime.HasValue && attendance.ScanOut.Value > shiftEndTime.Value)
                        effectiveScanOut = shiftEndTime.Value;

                    var totalMinutes = (decimal)(effectiveScanOut - effectiveScanIn).TotalMinutes;

                    var downtimes = await _downtimeRepository
                        .GetByCondition(x =>
                            x.Line == attendance.Line &&
                            x.EndTime != null &&
                            x.StartTime < effectiveScanOut &&
                            x.EndTime > effectiveScanIn)
                        .ToListAsync();

                    decimal totalDownTime = 0;
                    foreach (var dt in downtimes)
                    {
                        var overlapStart = dt.StartTime > effectiveScanIn ? dt.StartTime.Value : effectiveScanIn;
                        var overlapEnd = dt.EndTime < effectiveScanOut ? dt.EndTime.Value : effectiveScanOut;
                        if (overlapEnd > overlapStart)
                            totalDownTime += (decimal)(overlapEnd - overlapStart).TotalMinutes;
                    }

                    var workingTime = totalMinutes - breakTime - totalDownTime;
                    if (workingTime < 0) workingTime = 0;

                    var employee = await _employeeRepository
                        .GetByCondition(x => x.ID == attendance.EmployeeFAID)
                        .FirstOrDefaultAsync();

                    if (employee?.HireDate.HasValue == true)
                    {
                        int workingDays = (attendance.AttendanceDate.Value.Date - employee.HireDate.Value.Date).Days + 1;
                        workingTime = workingDays switch
                        {
                            <= 3 => 0,
                            <= 6 => workingTime * 0.5m,
                            <= 9 => workingTime * 0.75m,
                            _ => workingTime
                        };
                    }

                    attendance.WorkingTime = workingTime;
                    attendance.TotalDownTime = totalDownTime;
                }

                attendance.ShiftID = param.ShiftID;
                attendance.UpdateDate = DateTime.Now;
                attendance.UpdateUser = param.USER_IDX;

                await _attendanceRepository.UpdateAsync(attendance);

                var hours = (int)(attendance.WorkingTime ?? 0 / 60);
                var minutes = (int)(attendance.WorkingTime ?? 0 % 60);
                var timeDisplay = hours > 0 ? $"{hours}h {minutes}m" : $"{minutes}m";

                result.Success = true;
                result.Message = $"✓ Cập nhật ca: {newShift.ShiftName}\nGiờ làm: {timeDisplay}";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi: {ex.Message}";
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter param)
        {
            var result = new BaseResult();
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
    }
}