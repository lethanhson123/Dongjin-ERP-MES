namespace MESService.Implement
{
    public class FA_M3Service : BaseService<FAProduction, IFAProductionRepository>, IFA_M3Service
    {
        private readonly IFAProductionRepository _faProductionRepository;
        private readonly ILineListRepository _lineListRepository;
        private readonly IROTestLogRepository _ROTestLogRepository;

        public FA_M3Service(IFAProductionRepository faProductionRepository, ILineListRepository lineListRepository, IROTestLogRepository ROTestLogRepository)
            : base(faProductionRepository)
        {
            _faProductionRepository = faProductionRepository;
            _lineListRepository = lineListRepository;
            _ROTestLogRepository = ROTestLogRepository;
        }

        public override void Initialization(FAProduction model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var lines = await _lineListRepository.GetByCondition(l => l.Active == true)
                    .OrderBy(l => l.LineName)
                    .Select(l => new
                    {
                        DisplayText = $"{l.LineGroup} - {l.LineName} - {l.Family}",
                        Value = l.ID,

                    })
                    .ToListAsync();

                result.Data = new
                {
                    Lines = lines.Select(l => new
                    {
                        Text = l.DisplayText,
                        Value = l.Value
                    }).ToList()
                };
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
                var tabID = BaseParameter.TabSelected;

                // ========================= TAB 1 ============================
                if (tabID == "1")
                {
                    int lineID = BaseParameter.ID ?? 0;
                    string barcode = BaseParameter.Barcode?.Trim();

                    IQueryable<FAProduction> query =
                        _faProductionRepository.GetByCondition(p => true)
                        .AsNoTracking();   // ⭐ GIẢM RAM

                    if (lineID > 0)
                        query = query.Where(p => p.ID == lineID);

                    if (!string.IsNullOrEmpty(barcode))
                        query = query.Where(p => EF.Functions.Like(p.Barcode, $"%{barcode}%")); // ⭐ Tối ưu LIKE

                    var data = await query
                        .OrderByDescending(p => p.CreateDate)
                        .Take(1000)                 // ⭐ GIỚI HẠN SỐ LƯỢNG
                        .ToListAsync();

                    result.FAProductionList = data;
                }

                // ========================= TAB 2 ============================
                else if (tabID == "2")
                {
                    string lineID = BaseParameter.ComboBox1;
                    string search = BaseParameter.SearchString?.Trim();
                    DateTime fromDate = BaseParameter.FromDate.Value;
                    DateTime toDate = BaseParameter.ToDate.Value.AddDays(1);

                    IQueryable<ROTestLog> query =
                        _ROTestLogRepository
                            .GetByCondition(p => p.DateTime >= fromDate && p.DateTime <= toDate)
                            .AsNoTracking();            // ⭐ KHÔNG tracking → GIẢM RAM

                    if (!string.IsNullOrEmpty(lineID) && lineID != "0")
                        query = query.Where(p => p.LineNumber == lineID);

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        string searchLike = $"%{search}%";

                        // ⭐ Tối ưu nhiều OR → SQL sẽ nhẹ hơn
                        query = query.Where(p =>
                            EF.Functions.Like(p.LineName, searchLike) ||
                            EF.Functions.Like(p.LineNumber, searchLike) ||
                            EF.Functions.Like(p.LotNum, searchLike) ||
                            EF.Functions.Like(p.PartName, searchLike) ||
                            EF.Functions.Like(p.PartNumber, searchLike) ||
                            EF.Functions.Like(p.PassCount, searchLike) ||
                            EF.Functions.Like(p.ProgramVersion, searchLike) ||
                            EF.Functions.Like(p.Remark, searchLike) ||
                            EF.Functions.Like(p.Retest, searchLike) ||
                            EF.Functions.Like(p.ScanBarcode, searchLike) ||
                            EF.Functions.Like(p.ALC, searchLike) ||
                            EF.Functions.Like(p.ECO, searchLike) ||
                            EF.Functions.Like(p.ETC, searchLike) ||
                            EF.Functions.Like(p.VER, searchLike) ||
                            EF.Functions.Like(p.Note, searchLike) ||
                            EF.Functions.Like(p.Update_User, searchLike)
                        );
                    }

                    var data = await query
                        .OrderByDescending(p => p.DateTime)
                        .ThenByDescending(p => p.PartName)
                        .ThenByDescending(p => p.PassCount)                
                        .ToListAsync();

                    result.ROTestLogList = data;
                }
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
                result.Message = "Sẵn sàng để thêm mới";
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
                string line = BaseParameter.ComboBox1;
                string barcode = BaseParameter.Barcode;
                string userId = BaseParameter.USER_IDX ?? getCurrentUser();

                bool exists = await _faProductionRepository.GetByCondition(p => p.Barcode == barcode)
                    .AnyAsync();

                if (exists)
                {
                    result.Error = "Barcode đã tồn tại trong hệ thống";
                    return result;
                }

                var production = new FAProduction
                {
                    Line = line,
                    Barcode = barcode,
                    CreateDate = DateTime.Now,
                    CreateUser = userId,
                    UpdateDate = DateTime.Now,
                    UpdateUser = userId
                };

                await AddAsync(production);

                result.Message = "Lưu dữ liệu thành công";
                result.Data = production;
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
                var tabselect = BaseParameter.TabName;

                if (tabselect == "1")
                {
                    int id = BaseParameter.ID ?? 0;
                    string userId = BaseParameter.USER_IDX ?? getCurrentUser();

                    if (id <= 0)
                    {
                        result.Error = "ID không hợp lệ";
                        return result;
                    }

                    var production = await _faProductionRepository.GetByCondition(p => p.ID == id)
                        .FirstOrDefaultAsync();

                    if (production == null)
                    {
                        result.Error = "Không tìm thấy dữ liệu cần xóa";
                        return result;
                    }

                    await RemoveAsync(production);
                    result.Message = "Xóa thành công";
                    result.ErrorNumber = 0;
                }
                else if (tabselect == "2")
                {

                    var note = BaseParameter.Remark;
                    var userID = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView != null && BaseParameter.DataGridView.Count > 0)
                    {

                        foreach (var item in BaseParameter.DataGridView)
                        {
                            string sql = @"UPDATE ROTestLog SET ROTestLog.Active = 0, ROTestLog.Note = '" + note + "', ROTestLog.Update_DTM = NOW(), ROTestLog.Update_User = '" + userID + "' WHERE ROTestLog.ID = '" + item.ID + "'; ";
                            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);
                        }
                        result.Message = "Deleted " + BaseParameter.DataGridView.Count.ToString() + " EA";
                        result.ErrorNumber = 0;
                    }
                    else
                    {
                        result.Message = "Deleted 0 EA";
                        result.ErrorNumber = 1;
                    }

                }

            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorNumber = 1;
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

        public virtual async Task<BaseResult> GetAllItemsAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    result.FAProductionList = await _faProductionRepository.GetAllToListAsync();
                }
                else
                {
                    result.FAProductionList = await _faProductionRepository.GetByCondition(
                        o => o.Line.Contains(BaseParameter.SearchString) ||
                             o.Barcode.Contains(BaseParameter.SearchString))
                        .OrderByDescending(o => o.CreateDate)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private string getCurrentUser()
        {
            return "SYSTEM";
        }
        public virtual async Task<BaseResult> GetHourlyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fromDate = BaseParameter.FromDate ?? DateTime.Today;
                var toDate = (BaseParameter.ToDate ?? DateTime.Today).AddDays(1);

                // Lấy data thô
                var data = await _ROTestLogRepository
                    .GetByCondition(r => r.DateTime >= fromDate && r.DateTime < toDate && r.Active == true)
                    .Select(r => new
                    {
                        Hour = r.DateTime.Hour,
                        LineName = r.LineName ?? r.LineNumber
                    })
                    .ToListAsync();

                // Group theo giờ và Line
                var grouped = data
                    .GroupBy(x => new { x.Hour, x.LineName })
                    .Select(g => new
                    {
                        g.Key.Hour,
                        g.Key.LineName,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Hour)
                    .ThenBy(x => x.LineName)
                    .ToList();

                result.Data = grouped;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}