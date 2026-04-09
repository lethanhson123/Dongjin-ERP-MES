namespace MESService.Implement
{
    public class H15Service : IH15Service
    {
        private readonly IROTestLogRepository _ROTestLogRepository;
        private readonly IFAWorkOrderRepository _faWorkOrderRepository; 
        private readonly ILineListRepository _lineListRepository;
        private readonly ITaskTimeFARepository _taskTimeFARepository;
        private readonly IAttendanceRecordsRepository _attendanceRecordsRepository;
        public H15Service(
    IROTestLogRepository ROTestLogRepository,
    IFAWorkOrderRepository faWorkOrderRepository,
    ILineListRepository lineListRepository,
    ITaskTimeFARepository taskTimeFARepository,
    IAttendanceRecordsRepository attendanceRecordsRepository  
)
        {
            _ROTestLogRepository = ROTestLogRepository;
            _faWorkOrderRepository = faWorkOrderRepository;
            _lineListRepository = lineListRepository;
            _taskTimeFARepository = taskTimeFARepository;
            _attendanceRecordsRepository = attendanceRecordsRepository;  
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var allLines = await _ROTestLogRepository
                    .GetByCondition(p => p.Active == true)
                    .Select(p => new { p.LineNumber, p.LineName })
                    .Distinct()
                    .OrderBy(p => p.LineNumber)
                    .ToListAsync();

                result.Data = new
                {
                    Lines = allLines.Select(l => new
                    {
                        Text = !string.IsNullOrEmpty(l.LineName) ? $"{l.LineNumber} - {l.LineName}" : l.LineNumber,
                        Value = l.LineNumber
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetDailyLineProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = BaseParameter.ToDate ?? DateTime.Today;

                // ✅ FIX
                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddSeconds(-1);

                var data = await _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate)
                    .ToListAsync();

                var lineData = data
                    .GroupBy(p => new
                    {
                        LineNumber = p.LineNumber,
                        LineName = p.LineName
                    })
                    .Select(g => new
                    {
                        LineNumber = g.Key.LineNumber,
                        LineName = g.Key.LineName,
                        TotalTests = g.Count()
                    })
                    .OrderBy(x => x.LineNumber)
                    .ToList();

                var lines = lineData.Select(x =>
                    !string.IsNullOrEmpty(x.LineName) ? x.LineName : $"Line {x.LineNumber}"
                ).ToList();

                result.Data = new
                {
                    Lines = lines,
                    LineNumbers = lineData.Select(x => x.LineNumber).ToList(),
                    TestCounts = lineData.Select(x => x.TotalTests).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetDashboardSummary(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = BaseParameter.ToDate ?? DateTime.Today;
                var lineNumber = BaseParameter.ComboBox1;
                var viewType = BaseParameter.ComboBox2;

                DateTime actualFromDate;
                DateTime actualToDate;

                switch (viewType)
                {
                    case "hour":
                        actualFromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
                        actualToDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 23, 59, 59);
                        break;

                    case "day":
                        actualFromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
                        actualToDate = actualFromDate.AddMonths(1).AddSeconds(-1);
                        break;

                    case "week":
                        var calendar = CultureInfo.CurrentCulture.Calendar;
                        var currentWeek = calendar.GetWeekOfYear(fromDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                        var startWeek = currentWeek - 6;
                        if (startWeek < 1) startWeek = 1;
                        var tempDate = new DateTime(fromDate.Year, 1, 1);
                        while (calendar.GetWeekOfYear(tempDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) < startWeek)
                        {
                            tempDate = tempDate.AddDays(7);
                        }
                        while (tempDate.DayOfWeek != DayOfWeek.Monday)
                        {
                            tempDate = tempDate.AddDays(-1);
                        }
                        actualFromDate = tempDate;
                        actualToDate = fromDate;
                        while (actualToDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            actualToDate = actualToDate.AddDays(1);
                        }
                        actualToDate = actualToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                        break;

                    case "month":
                        actualFromDate = new DateTime(fromDate.Year, 1, 1);
                        actualToDate = new DateTime(fromDate.Year, 12, 31, 23, 59, 59);
                        break;

                    case "year":
                        var fromYear = fromDate.Year - 5;
                        actualFromDate = new DateTime(fromYear, 1, 1);
                        actualToDate = new DateTime(fromDate.Year, 12, 31, 23, 59, 59);
                        break;

                    default:
                        actualFromDate = fromDate;
                        actualToDate = toDate.AddDays(1);
                        break;
                }

                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= actualFromDate && p.DateTime <= actualToDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.ToListAsync();

                if (!data.Any())
                {
                    result.Data = new
                    {
                        TotalTests = 0,
                        AverageSpeed = 0,
                        AvgCycleTime = 0,
                        ActiveLines = 0
                    };
                    return result;
                }

                var totalTests = data.Count;
                var activeLines = data.Select(p => p.LineNumber).Distinct().Count();

                var activeHours = data
                    .Select(p => new DateTime(p.DateTime.Year, p.DateTime.Month, p.DateTime.Day, p.DateTime.Hour, 0, 0))
                    .Distinct()
                    .Count();

                if (activeHours < 1) activeHours = 1;

                var avgSpeed = (int)(totalTests / (double)activeHours);
                var avgCycleTime = activeHours > 0 ? (int)(3600 / (totalTests / (double)activeHours)) : 0;

                result.Data = new
                {
                    TotalTests = totalTests,
                    AverageSpeed = avgSpeed,
                    AvgCycleTime = avgCycleTime,
                    ActiveLines = activeLines
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetHourlyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = BaseParameter.ToDate ?? DateTime.Today;
                var lineNumber = BaseParameter.ComboBox1;

                // ✅ FIX
                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddSeconds(-1);

                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.OrderBy(p => p.DateTime).ToListAsync();

                var hourlyData = data
                    .GroupBy(p => p.DateTime.Hour)
                    .Select(g => new
                    {
                        Hour = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Hour)
                    .ToList();

                var allHours = Enumerable.Range(0, 24).Select(h => new
                {
                    Hour = h,
                    HourLabel = $"{h:D2}:00",
                    Count = hourlyData.FirstOrDefault(x => x.Hour == h)?.Count ?? 0
                }).ToList();

                result.Data = new
                {
                    Labels = allHours.Select(x => x.HourLabel).ToList(),
                    Values = allHours.Select(x => x.Count).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetDailyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var selectedDate = BaseParameter.FromDate ?? DateTime.Today;
                var year = selectedDate.Year;
                var month = selectedDate.Month;
                var lineNumber = BaseParameter.ComboBox1;

                var fromDate = new DateTime(year, month, 1);
                var toDate = fromDate.AddMonths(1);

                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime < toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.ToListAsync();

                var dailyData = data
                    .GroupBy(p => p.DateTime.Day)
                    .Select(g => new
                    {
                        Day = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Day)
                    .ToList();

                var daysInMonth = DateTime.DaysInMonth(year, month);
                var allDays = Enumerable.Range(1, daysInMonth).Select(d => new
                {
                    Day = d,
                    DayLabel = d.ToString(),
                    Count = dailyData.FirstOrDefault(x => x.Day == d)?.Count ?? 0
                }).ToList();

                result.Data = new
                {
                    Labels = allDays.Select(x => x.DayLabel).ToList(),
                    Values = allDays.Select(x => x.Count).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetWeeklyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var selectedDate = BaseParameter.FromDate ?? DateTime.Today;
                var lineNumber = BaseParameter.ComboBox1;

                var calendar = CultureInfo.CurrentCulture.Calendar;
                var currentWeek = calendar.GetWeekOfYear(
                    selectedDate,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday
                );

                var startWeek = currentWeek - 6;
                if (startWeek < 1) startWeek = 1;

                var tempDate = new DateTime(selectedDate.Year, 1, 1);
                while (calendar.GetWeekOfYear(tempDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) < startWeek)
                {
                    tempDate = tempDate.AddDays(7);
                }

                while (tempDate.DayOfWeek != DayOfWeek.Monday)
                {
                    tempDate = tempDate.AddDays(-1);
                }
                var fromDate = tempDate;

                var toDate = selectedDate;
                while (toDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    toDate = toDate.AddDays(1);
                }
                toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59);

                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.ToListAsync();

                var weeklyData = data
                    .GroupBy(p => calendar.GetWeekOfYear(
                        p.DateTime,
                        CalendarWeekRule.FirstFourDayWeek,
                        DayOfWeek.Monday
                    ))
                    .Select(g => new
                    {
                        Week = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Week)
                    .ToList();

                var allWeeks = Enumerable.Range(startWeek, 7).Select(w => new
                {
                    Week = w,
                    WeekLabel = $"Week {w}",
                    Count = weeklyData.FirstOrDefault(x => x.Week == w)?.Count ?? 0
                }).ToList();

                result.Data = new
                {
                    Labels = allWeeks.Select(x => x.WeekLabel).ToList(),
                    Values = allWeeks.Select(x => x.Count).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetMonthlyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var year = BaseParameter.FromDate?.Year ?? DateTime.Today.Year;
                var fromDate = new DateTime(year, 1, 1);
                var toDate = new DateTime(year, 12, 31, 23, 59, 59);
                var lineNumber = BaseParameter.ComboBox1;

                // ✅ FIX: Không cần AddDays(1) vì đã set 23:59:59
                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.ToListAsync();

                var monthlyData = data
                    .GroupBy(p => p.DateTime.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Month)
                    .ToList();

                var allMonths = Enumerable.Range(1, 12).Select(m => new
                {
                    Month = m,
                    MonthLabel = new DateTime(year, m, 1).ToString("MMM"),
                    Count = monthlyData.FirstOrDefault(x => x.Month == m)?.Count ?? 0
                }).ToList();

                result.Data = new
                {
                    Labels = allMonths.Select(x => x.MonthLabel).ToList(),
                    Values = allMonths.Select(x => x.Count).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetYearlyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromYear = BaseParameter.FromDate?.Year ?? DateTime.Today.Year - 5;
                var toYear = BaseParameter.ToDate?.Year ?? DateTime.Today.Year;
                var fromDate = new DateTime(fromYear, 1, 1);
                var toDate = new DateTime(toYear, 12, 31, 23, 59, 59);
                var lineNumber = BaseParameter.ComboBox1;

                // ✅ FIX: Không cần AddDays(1)
                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "all")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                var data = await query.ToListAsync();

                var yearlyData = data
                    .GroupBy(p => p.DateTime.Year)
                    .Select(g => new
                    {
                        Year = g.Key,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Year)
                    .ToList();

                result.Data = new
                {
                    Labels = yearlyData.Select(x => x.Year.ToString()).ToList(),
                    Values = yearlyData.Select(x => x.Count).ToList()
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetLatestRecords(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                int count = BaseParameter.PageSize ?? 20;

                var data = await _ROTestLogRepository
                    .GetByCondition(p => p.Active == true)
                    .OrderByDescending(p => p.DateTime)
                    .Take(count)
                    .ToListAsync();

                result.ROTestLogList = data;
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
                var lineNumber = BaseParameter.ComboBox1;
                var search = BaseParameter.SearchString;
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = BaseParameter.ToDate ?? DateTime.Today;

                // ✅ FIX
                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddSeconds(-1);

                var query = _ROTestLogRepository
                    .GetByCondition(p => p.Active == true && p.DateTime >= fromDate && p.DateTime <= toDate);

                if (!string.IsNullOrEmpty(lineNumber) && lineNumber != "0")
                {
                    query = query.Where(p => p.LineNumber == lineNumber);
                }

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(p =>
                        (p.LineName != null && p.LineName.Contains(search)) ||
                        (p.LineNumber != null && p.LineNumber.Contains(search)) ||
                        (p.LotNum != null && p.LotNum.Contains(search)) ||
                        (p.PartName != null && p.PartName.Contains(search)) ||
                        (p.PartNumber != null && p.PartNumber.Contains(search)) ||
                        (p.PassCount != null && p.PassCount.Contains(search)) ||
                        (p.ProgramVersion != null && p.ProgramVersion.Contains(search)) ||
                        (p.Remark != null && p.Remark.Contains(search)) ||
                        (p.Retest != null && p.Retest.Contains(search)) ||
                        (p.ScanBarcode != null && p.ScanBarcode.Contains(search)) ||
                        (p.ALC != null && p.ALC.Contains(search)) ||
                        (p.ECO != null && p.ECO.Contains(search)) ||
                        (p.ETC != null && p.ETC.Contains(search)) ||
                        (p.VER != null && p.VER.Contains(search))
                    );
                }

                var data = await query
                    .OrderByDescending(p => p.DateTime)
                    .ThenByDescending(p => p.PartName)
                    .ThenByDescending(p => p.PassCount)
                    .ToListAsync();

                result.ROTestLogList = data;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetLinePlanByDate(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var date = BaseParameter.FromDate ?? DateTime.Today;
                var lineFilter = BaseParameter.ComboBox1;

                // Get work orders for date
                var workOrders = await _faWorkOrderRepository
                    .GetByCondition(x => x.Date.Date == date.Date && x.Active == true)
                    .ToListAsync();

                if (!workOrders.Any())
                {
                    result.Data = new Dictionary<string, int>();
                    result.Success = true;
                    return result;
                }

                // Get related lines
                var lineIds = workOrders.Select(x => x.LineID).Distinct().ToList();
                var lines = await _lineListRepository
                    .GetByCondition(x => lineIds.Contains(x.ID) && x.Active == true)
                    .ToListAsync();

                // Parse lineFilter to long
                long? filterLineId = null;
                if (!string.IsNullOrEmpty(lineFilter) && lineFilter != "all")
                {
                    if (long.TryParse(lineFilter, out long parsedId))
                    {
                        filterLineId = parsedId;
                    }
                }

                // Aggregate plans by LineID
                var planData = (from wo in workOrders
                                join line in lines on wo.LineID equals line.ID
                                where !filterLineId.HasValue || line.ID == filterLineId.Value
                                group wo by line.ID.ToString() into g
                                select new
                                {
                                    LineID = g.Key,
                                    TotalPlan = g.Sum(x => x.Quantity)
                                })
                               .ToDictionary(x => x.LineID, x => x.TotalPlan);

                result.Data = planData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetLineProductivityMetrics(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = BaseParameter.ToDate ?? DateTime.Today;

                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddSeconds(-1);

                var lineNumber = BaseParameter.ComboBox1;

                if (string.IsNullOrEmpty(lineNumber) || lineNumber == "all")
                {
                    result.Error = "LineNumber is required";
                    return result;
                }

                // 1. Get ROTestLog data (Actual production)
                var roQuery = _ROTestLogRepository
                    .GetByCondition(p =>
                        p.Active == true &&
                        p.DateTime >= fromDate &&
                        p.DateTime <= toDate &&
                        p.LineNumber == lineNumber);

                var roData = await roQuery.ToListAsync();

                var productionByPart = roData
                    .GroupBy(p => new { p.PartNumber, p.ECO })
                    .Select(g => new {
                        PartNumber = g.Key.PartNumber,
                        ECO = g.Key.ECO,
                        Actual = g.Count()
                    })
                    .ToList();

                var totalActual = productionByPart.Sum(p => p.Actual);

                // 2. Get TaskTime
                long? lineId = long.TryParse(lineNumber, out long id) ? id : null;

                var productionDetails = new Dictionary<string, object>();
                decimal totalTheoreticalMinutes = 0;

                foreach (var prod in productionByPart)
                {
                    if (string.IsNullOrEmpty(prod.PartNumber)) continue;

                    var taskQuery = _taskTimeFARepository
                        .GetByCondition(t =>
                            t.PartNo == prod.PartNumber &&
                            t.Active == true);

                    if (!string.IsNullOrEmpty(prod.ECO))
                    {
                        taskQuery = taskQuery.Where(t => t.ECN == prod.ECO);
                    }

                    if (lineId.HasValue)
                    {
                        taskQuery = taskQuery.Where(t => t.LineID == lineId.Value);
                    }

                    var taskTime = await taskQuery.FirstOrDefaultAsync();

                    if (taskTime != null)
                    {
                        decimal finalTaskTime = taskTime.TaskTimeIE4 ?? taskTime.TaskTimeIE3 ??
                                               taskTime.TaskTimeIE2 ?? taskTime.TaskTimeIE ?? 0;

                        if (finalTaskTime > 0)
                        {
                            decimal theoreticalMinutes = prod.Actual * finalTaskTime;
                            totalTheoreticalMinutes += theoreticalMinutes;

                            string key = $"{prod.PartNumber}_{prod.ECO}";
                            productionDetails[key] = new
                            {
                                Actual = prod.Actual,
                                TaskTimeIE = finalTaskTime,
                                TheoreticalTimeMinutes = Math.Round(theoreticalMinutes, 2)
                            };
                        }
                    }
                }

                // 3. Get Attendance (Working Time)
                var lineEntity = await _lineListRepository
                    .GetByCondition(l => l.ID == lineId.Value && l.Active == true)
                    .FirstOrDefaultAsync();

                decimal totalWorkingTimeMinutes = 0;
                int totalWorkers = 0;

                if (lineEntity != null)
                {
                    var fullLineName = string.Join(" - ", new[] {
                lineEntity.LineGroup,
                lineEntity.LineName,
                lineEntity.Family
            }.Where(s => !string.IsNullOrEmpty(s)));

                    var attendances = await _attendanceRecordsRepository
                        .GetByCondition(a =>
                            a.Active == true &&
                            a.AttendanceDate >= fromDate.Date &&
                            a.AttendanceDate <= toDate.Date &&
                            a.Line == fullLineName &&
                            a.ScanIn.HasValue)
                        .ToListAsync();

                    var now = DateTime.Now;

                    totalWorkingTimeMinutes = attendances.Sum(a =>
                        a.WorkingTime ?? (decimal)(now - a.ScanIn.Value).TotalMinutes
                    );

                    totalWorkers = attendances.Count;
                }

                // 4. Calculate Productivity
                decimal productivity = 0;
                if (totalWorkingTimeMinutes > 0)
                {
                    productivity = Math.Round((totalTheoreticalMinutes / totalWorkingTimeMinutes) * 100, 2);
                }

                // 5. Get Plan
                var planResult = await GetLinePlanByDate(BaseParameter);
                int planQty = 0;
                if (planResult.Success && planResult.Data is Dictionary<string, int> plans)
                {
                    plans.TryGetValue(lineNumber, out planQty);
                }

                result.Data = new
                {
                    LineNumber = lineNumber,
                    Actual = totalActual,
                    PlanQty = planQty,
                    TotalWorkers = totalWorkers,
                    TotalWorkingTimeMinutes = Math.Round(totalWorkingTimeMinutes, 2),
                    TotalTheoreticalMinutes = Math.Round(totalTheoreticalMinutes, 2),
                    Productivity = productivity,
                    ProductionDetails = productionDetails
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}