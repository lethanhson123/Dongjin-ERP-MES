namespace MESService.Implement
{
    public class FA_M1Service : BaseService<AttendanceSession, IAttendanceSessionRepository>, IFA_M1Service
    {
        private readonly IAttendanceSessionRepository _attendanceSessionRepository;
        private readonly IEmployeeFARepository _employeeFARepository;
        private readonly IShiftTimeRepository _shiftTimeRepository;
        private readonly IAttendanceRepository _attendanceRepository;

        public FA_M1Service(
            IAttendanceSessionRepository attendanceSessionRepository,
            IEmployeeFARepository employeeFARepository,
            IShiftTimeRepository shiftTimeRepository,
            IAttendanceRepository attendanceRepository
        ) : base(attendanceSessionRepository)
        {
            _attendanceSessionRepository = attendanceSessionRepository;
            _employeeFARepository = employeeFARepository;
            _shiftTimeRepository = shiftTimeRepository;
            _attendanceRepository = attendanceRepository;
        }

        public override void Initialization(AttendanceSession model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var employees = await _employeeFARepository
                    .GetByCondition(e => e.Active == true)
                    .OrderBy(e => e.EmpCode)
                    .ToListAsync();

                var shifts = await _shiftTimeRepository
                    .GetByCondition(s => s.Active == true)
                    .OrderBy(s => s.ShiftName)
                    .ToListAsync();

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

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                DateTime startDate = DateTime.Today.AddDays(-7);
                DateTime endDate = DateTime.Today;

                if (!string.IsNullOrEmpty(BaseParameter.StartDate))
                {
                    if (DateTime.TryParse(BaseParameter.StartDate, out var sd))
                        startDate = sd.Date;
                }

                if (!string.IsNullOrEmpty(BaseParameter.EndDate))
                {
                    if (DateTime.TryParse(BaseParameter.EndDate, out var ed))
                        endDate = ed.Date;
                }

                var attendanceRecords = await _attendanceRepository
                    .GetByCondition(a =>
                        a.CheckTime >= startDate &&
                        a.CheckTime <= endDate.AddDays(1).AddSeconds(-1))
                    .OrderBy(a => a.CheckTime)
                    .ToListAsync();

                var employees = await _employeeFARepository
                    .GetByCondition(e => e.Active == true)
                    .ToListAsync();

                var shifts = await _shiftTimeRepository
                    .GetByCondition(s => s.Active == true)
                    .ToListAsync();

                var sessions = new List<AttendanceSession>();

                var grouped = attendanceRecords
                    .GroupBy(a => new
                    {
                        EmployeeCode = a.EmployeeCode,
                        WorkDate = a.CheckTime.Date
                    });

                foreach (var group in grouped)
                {
                    var employee = employees.FirstOrDefault(e => e.EmpCode == group.Key.EmployeeCode);
                    if (employee == null) continue;

                    var shift = shifts.FirstOrDefault(s => s.ID == employee.DefaultShiftID);
                    if (shift == null) continue;

                    var employeeSessions = ProcessAttendanceWithLineChanges(
                        employee,
                        shift,
                        group.Key.WorkDate,
                        group.OrderBy(a => a.CheckTime).ToList()
                    );

                    if (employeeSessions != null && employeeSessions.Any())
                        sessions.AddRange(employeeSessions);
                }

                var query = sessions.AsQueryable();

                if (!string.IsNullOrEmpty(BaseParameter.EmployeeCode))
                {
                    var search = BaseParameter.EmployeeCode.Trim().ToLower();
                    var employeeIds = employees
                        .Where(e => e.EmpCode.ToLower().Contains(search) ||
                                    e.Name.ToLower().Contains(search))
                        .Select(e => e.ID)
                        .ToList();

                    query = query.Where(s => employeeIds.Contains(s.EmployeeID));
                }

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString.Trim();
                    query = query.Where(s => s.Line.Contains(search));
                }

                var finalSessions = query
                    .OrderByDescending(s => s.WorkDate)
                    .ThenByDescending(s => s.CheckInTime)
                    .ToList();

                if (sessions.Any())
                {
                    await SaveSessionsToDB(sessions);
                }

                result.AttendanceSessionList = finalSessions;
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

        private List<AttendanceSession> ProcessAttendanceWithLineChanges(
            EmployeeFA employee,
            ShiftTime shift,
            DateTime workDate,
            List<Attendance> records)
        {
            var sessions = new List<AttendanceSession>();
            if (!records.Any()) return sessions;

            AttendanceSession currentSession = null;

            foreach (var record in records)
            {
                if (record.CheckType == 0)
                {
                    if (currentSession != null && currentSession.CheckOutTime == null)
                    {
                        currentSession.CheckOutID = record.ID;
                        currentSession.CheckOutTime = record.CheckTime;
                        currentSession.Status = "Complete";
                        CalculateSessionMetrics(currentSession, shift, workDate);
                    }

                    currentSession = new AttendanceSession
                    {
                        EmployeeID = employee.ID,
                        ShiftID = shift.ID,
                        WorkDate = workDate.Date,
                        Line = record.MachineName,
                        CheckInID = record.ID,
                        CheckInTime = record.CheckTime,
                        CheckOutID = null,
                        CheckOutTime = null,
                        Status = "Incomplete",
                        Active = true
                    };

                    sessions.Add(currentSession);
                }
                else if (record.CheckType == 1)
                {
                    var openSession = sessions.LastOrDefault(s => s.CheckOutTime == null);

                    if (openSession != null)
                    {
                        openSession.CheckOutID = record.ID;
                        openSession.CheckOutTime = record.CheckTime;
                        openSession.Status = "Complete";
                        CalculateSessionMetrics(openSession, shift, workDate);
                    }
                }
            }

            return sessions;
        }

        private void CalculateSessionMetrics(AttendanceSession session, ShiftTime shift, DateTime workDate)
        {
            if (session.CheckInTime == null) return;

            DateTime shiftStartDateTime = workDate.Date.Add(shift.StartTime);
            DateTime shiftEndDateTime = workDate.Date.Add(shift.EndTime);
            if (shift.EndTime < shift.StartTime)
                shiftEndDateTime = shiftEndDateTime.AddDays(1);

            if (session.CheckOutTime.HasValue)
            {
                var totalMinutes = (int)(session.CheckOutTime.Value - session.CheckInTime.Value).TotalMinutes;
                session.WorkingMinutes = totalMinutes - (shift.BreakTime ?? 0);
                if (session.WorkingMinutes < 0) session.WorkingMinutes = 0;
            }

            if (session.CheckInTime.Value > shiftStartDateTime)
            {
                session.LateMinutes = (int)(session.CheckInTime.Value - shiftStartDateTime).TotalMinutes;
                session.IsLate = session.LateMinutes > 0;
            }
            else
            {
                session.LateMinutes = 0;
                session.IsLate = false;
            }

            if (session.CheckOutTime.HasValue && session.CheckOutTime.Value > shiftEndDateTime)
            {
                session.OvertimeMinutes = (int)(session.CheckOutTime.Value - shiftEndDateTime).TotalMinutes;
            }
            else
            {
                session.OvertimeMinutes = 0;
            }
        }

        private async Task SaveSessionsToDB(List<AttendanceSession> sessions)
        {
            try
            {
                foreach (var session in sessions)
                {
                    var existing = await _attendanceSessionRepository
                        .GetByCondition(s =>
                            s.EmployeeID == session.EmployeeID &&
                            s.WorkDate == session.WorkDate &&
                            s.ShiftID == session.ShiftID &&
                            s.Line == session.Line)
                        .FirstOrDefaultAsync();

                    if (existing != null)
                    {
                        bool hasChanges =
                            existing.CheckInID != session.CheckInID ||
                            existing.CheckOutID != session.CheckOutID ||
                            existing.WorkingMinutes != session.WorkingMinutes ||
                            existing.LateMinutes != session.LateMinutes ||
                            existing.OvertimeMinutes != session.OvertimeMinutes ||
                            existing.Status != session.Status;

                        if (hasChanges)
                        {
                            existing.CheckInID = session.CheckInID;
                            existing.CheckInTime = session.CheckInTime;
                            existing.CheckOutID = session.CheckOutID;
                            existing.CheckOutTime = session.CheckOutTime;
                            existing.WorkingMinutes = session.WorkingMinutes;
                            existing.LateMinutes = session.LateMinutes;
                            existing.IsLate = session.IsLate;
                            existing.OvertimeMinutes = session.OvertimeMinutes;
                            existing.Status = session.Status;
                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateUser = "AUTO_SYSTEM";

                            await _attendanceSessionRepository.UpdateAsync(existing);
                        }
                    }
                    else
                    {
                        session.CreateDate = DateTime.Now;
                        session.CreateUser = "AUTO_SYSTEM";
                        await _attendanceSessionRepository.AddAsync(session);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving sessions: {ex.Message}");
            }
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            return new BaseResult { Success = true };
        }

        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }

        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            return new BaseResult();
        }
    }
}