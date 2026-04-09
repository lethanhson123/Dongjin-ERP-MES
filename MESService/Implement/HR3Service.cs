namespace MESService.Implement
{
    public class HR3Service : BaseService<Attendance, IAttendanceRepository>, IHR3Service
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public HR3Service(IAttendanceRepository attendanceRepository) : base(attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public override void Initialization(Attendance model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                // Load danh sách máy để filter
                var machines = await _attendanceRepository
                    .GetByCondition(x => true)
                    .Select(x => x.MachineName)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

                result.MachineList = machines;
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
                var query = _attendanceRepository.GetByCondition(x => true);

                // Filter theo MachineName
                if (!string.IsNullOrEmpty(BaseParameter.MachineName))
                {
                    query = query.Where(x => x.MachineName == BaseParameter.MachineName);
                }

                // Filter theo EmployeeCode
                if (!string.IsNullOrEmpty(BaseParameter.EmployeeCode))
                {
                    query = query.Where(x => x.EmployeeCode.Contains(BaseParameter.EmployeeCode));
                }

                // Filter theo ngày
                if (BaseParameter.FromDate.HasValue)
                {
                    query = query.Where(x => x.CheckTime >= BaseParameter.FromDate.Value);
                }

                if (BaseParameter.ToDate.HasValue)
                {
                    var toDateEnd = BaseParameter.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CheckTime <= toDateEnd);
                }

                // Search text (EmployeeCode)
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString.Trim();
                    query = query.Where(x => x.EmployeeCode.Contains(search));
                }

                // Lấy dữ liệu
                var rawData = await query
                    .OrderBy(x => x.EmployeeCode)
                    .ThenBy(x => x.CheckTime)
                    .ToListAsync();

                // ✅ TỰ ĐỘNG PHÂN LOẠI VÀO/RA
                result.AttendanceList = AutoClassifyCheckInOut(rawData);

                // Load danh sách máy
                var machines = await _attendanceRepository
                    .GetByCondition(x => true)
                    .Select(x => x.MachineName)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToListAsync();

                result.MachineList = machines;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // ✅ HÀM TỰ ĐỘNG PHÂN LOẠI VÀO/RA
        private List<Attendance> AutoClassifyCheckInOut(List<Attendance> records)
        {
            if (records == null || records.Count == 0)
                return records;

            // Group theo EmployeeCode và Ngày
            var grouped = records
                .GroupBy(x => new { x.EmployeeCode, Date = x.CheckTime.Date })
                .ToList();

            foreach (var group in grouped)
            {
                var dailyRecords = group.OrderBy(x => x.CheckTime).ToList();

                for (int i = 0; i < dailyRecords.Count; i++)
                {
                    // Lần chấm công lẻ (0, 2, 4...) = VÀO
                    // Lần chấm công chẵn (1, 3, 5...) = RA
                    dailyRecords[i].CheckType = (byte)(i % 2);
                }
            }

            return records;
        }

        // ❌ KHÔNG CẦN - Bỏ trống
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
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

        // ❌ KHÔNG CẦN - Bỏ trống
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
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

        // ❌ KHÔNG CẦN - Bỏ trống
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
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

        // ❌ KHÔNG CẦN - Bỏ trống
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

        // ❌ KHÔNG CẦN - Bỏ trống
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

        // ✅ CẦN - Export Excel
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Export sẽ xử lý ở JavaScript
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // ❌ KHÔNG CẦN - Bỏ trống
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

        // ❌ KHÔNG CẦN - Bỏ trống
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

        // ❌ KHÔNG CẦN - Bỏ trống
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
    }
}