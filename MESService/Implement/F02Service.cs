using MESRepository.Implement;
using MESRepository.Interface;
using System.Linq;
using ZXing;

namespace MESService.Implement
{
    public class F02Service : BaseService<OQC_NG, IOQC_NGRepository>, IF02Service
    {
        private readonly IOQC_NGRepository _OQC_NGRepository;
        private readonly ILineListRepository _lineListRepository;
        private readonly INGListRepository _ngListRepository;
        private readonly ItspartRepository _tspartListRepository;

        public F02Service(IOQC_NGRepository OQC_NGRepository, ILineListRepository lineListRepository, INGListRepository ngListRepository, ItspartRepository tspartListRepository) : base(OQC_NGRepository)
        {
            _OQC_NGRepository = OQC_NGRepository;
            _lineListRepository = lineListRepository;
            _ngListRepository = ngListRepository;
            _tspartListRepository = tspartListRepository;
        }

        public override void Initialization(OQC_NG model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var lines = await _lineListRepository.Query().Where(l => l.Active == true)
                    .AsNoTracking()
                    .OrderBy(l => l.LineName)
                    .ToListAsync();

                var errorLists = await _ngListRepository.Query().Where(s => s.Active == true && s.ErrorType == "FA")
                    .AsNoTracking()
                    .ToListAsync();

                // Gán cho Razor
                result.LineList = lines;
                result.NGLists = errorLists;
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

            var action = BaseParameter.Action;

            if(action == 1)
            {
                result = await GetCurentScanned(BaseParameter);
            }
            else if (action == 2)
            {
                result = await GetHistoryScanned(BaseParameter);
            }

            return result;
        }

        private async Task<BaseResult> GetHistoryScanned(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            var from_date = baseParameter.FromDate.Value;
            var to_date = baseParameter.ToDate.Value;
            var search = baseParameter.SearchString.Trim();
            var lineID = baseParameter.LineID;
            var ErrorID = baseParameter.ErrorID;


            var sql = @"select a.ID,
                        b.PART_NO, b.PART_SUPL, b.PART_NM,
                        a.LineListID,
                        CONCAT(c.LineGroup, '-', c.LineName, '-', c.Family) AS LineName,                     
                        c.LineType,
                        a.LotCode as Barcode,
                        a.NGList_ID, d.ErrorCode,a.GP12,
                        CASE 
						    WHEN a.GP12 = TRUE THEN 'GP12'
							ELSE 'OQC'
					    END AS LOC,
                       CONCAT( d.ErrorDescription, '(', d.KoreanDescription, ')' ) as Description,
                        a.Part_IDX, a.ECN, a.CREATE_DTM,a.CREATE_USER, a.REMARK,a.Picture, a.UPDATE_DTM,a.UPDATE_USER
                         from OQC_NG a LEFT JOIN tspart b on a.Part_IDX = b.PART_IDX
                         LEFT JOIN LineList c on a.LineListID = c.ID
                         LEFT JOIN NGList d on a.NGList_ID = d.ID
                        where (a.CREATE_DTM >= '" + from_date.ToString("yyyy-MM-dd") + "' and a.CREATE_DTM <= '" + to_date.ToString("yyyy-MM-dd") + " 23:59:59')";

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt, true)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            if (!string.IsNullOrEmpty(search))
            {
                string upperSearch = search.ToUpper();
                result.DataGridView = result.DataGridView
                    .Where(s => (s.PART_NO?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.PART_SUPL?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.PART_NM?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.LineName?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.Barcode?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.ErrorCode?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.Description?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.LOC?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.ECN?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.CREATE_USER?.ToUpper().Contains(upperSearch) ?? false) ||
                    (s.REMARK?.ToUpper().Contains(upperSearch) ?? false)
                    )
                    .ToList();
            }

            if(lineID > 0)
            {
                result.DataGridView = result.DataGridView.Where(s => s.LineListID == lineID).ToList();
            }

            if (ErrorID != "0")
            {
                result.DataGridView = result.DataGridView.Where(s => s.NGList_ID == long.Parse(ErrorID)).ToList();
            }
            return result;
        }


        /// <summary>
        /// load thông tin đã scan theo thời điểm hiện tại
        /// </summary>
        /// <param name="BaseParameter"></param>
        /// <returns></returns>
        private async Task<BaseResult> GetCurentScanned(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            var date = DateTime.Today;

            var sql = @"select a.ID,
                        b.PART_NO, b.PART_SUPL, b.PART_NM,
                        a.LineListID,
                        CONCAT(c.LineGroup, '-', c.LineName, '-', c.Family) AS LineName,                     
                        c.LineType,
                        a.LotCode as Barcode,
                        a.NGList_ID, d.ErrorCode,a.GP12,
                        CASE 
						    WHEN a.GP12 = TRUE THEN 'GP12'
							ELSE 'OQC'
					    END AS LOC,
                       CONCAT( d.ErrorDescription, '(', d.KoreanDescription, ')' ) as Description,
                        a.Part_IDX, a.ECN, a.CREATE_DTM,a.CREATE_USER, a.REMARK,a.Picture, a.UPDATE_DTM,a.UPDATE_USER
                         from OQC_NG a LEFT JOIN tspart b on a.Part_IDX = b.PART_IDX
                         LEFT JOIN LineList c on a.LineListID = c.ID
                         LEFT JOIN NGList d on a.NGList_ID = d.ID
                        where a.LineListID = '" + BaseParameter.LineID + "' and (a.CREATE_DTM >= '" + date.ToString("yyyy-MM-dd") + "' or a.UPDATE_DTM >= '" + date.ToString("yyyy-MM-dd") + "') order by a.ID DESC";

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt, true)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            return result;

        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() =>
                {

                    var lotCode = BaseParameter.LotCode.ToUpper().Trim();
                    var partNo = BaseParameter.PartNo.ToUpper().Trim();
                    var Ecn = BaseParameter.PartEncno.ToUpper().Trim();

                    var tsp = _tspartListRepository.Query().Where(s => s.PART_NO == partNo || s.PART_SUPL == partNo).ToList().FirstOrDefault();
                    if (tsp != null)
                        tsp.BOM_GRP = Ecn;

                    result.tspart = tsp;
                    result.QRCodeText = lotCode;
                });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            var result = new BaseResult();

            try
            {
                if (BaseParameter == null)
                {
                    result.Error = "Dữ liệu gửi lên không hợp lệ.";
                    return result;
                }

                string lotCode = BaseParameter.LotCode?.Trim().ToUpper() ?? "";
                string partNo = BaseParameter.PartNo?.Trim().ToUpper() ?? "";
                string ecn = BaseParameter.PartEncno?.Trim().ToUpper() ?? "";
                long ngCode = BaseParameter.NGList.ID;
                string userID = BaseParameter.USER_ID ?? "";
                long lineID = BaseParameter.LineID.Value;
                bool GP12 = BaseParameter.GP12.Value;
                string picture = BaseParameter.Picture;

                if (string.IsNullOrWhiteSpace(lotCode) ||
                    string.IsNullOrWhiteSpace(partNo) ||
                    ngCode <= 0)
                {
                    result.Error = "Thiếu dữ liệu bắt buộc (LotCode, PartNo hoặc NGCode).";
                    return result;
                }

                // Kiểm tra part có tồn tại
                var partList = _tspartListRepository
                    .Query().Where(s => s.PART_NO == partNo || s.PART_SUPL == partNo)
                    .FirstOrDefault();

                if (partList == null)
                {
                    result.Error = "Không tìm thấy PartNo trong danh sách linh kiện.";
                    return result;
                }

                // Kiểm tra NGList có tồn tại
                var ngItem = _ngListRepository
                    .Query().Where(s => s.ID == ngCode)
                    .FirstOrDefault();

                if (ngItem == null)
                {
                    result.Error = $"Không tìm thấy mã NG: {ngCode}";
                    return result;
                }

                // Kiểm tra LotCode đã tồn tại chưa
                var existed = _OQC_NGRepository
                    .Query().Where(s => s.LotCode == lotCode && s.NGList_ID == ngItem.ID)
                    .FirstOrDefault();

                if (existed != null)
                {
                    result.Error = "LotCode đã báo lỗi NG "+ ngCode + " (ngày " + existed.CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss") + ")";
                    return result;
                }

                // Thêm mới bản ghi
                var newRecord = new OQC_NG
                {
                    LotCode = lotCode,
                    NGList_ID = ngItem.ID,
                    Part_IDX = partList.PART_IDX ?? 0,
                    ECN = ecn,
                    CREATE_DTM = DateTime.Now,
                    CREATE_USER = userID,
                    LineListID = lineID,
                    GP12 = GP12,
                    Picture = picture

                };

                await _OQC_NGRepository.AddAsync(newRecord);

                result.Message = "Lưu dữ liệu thành công!";
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi hệ thống: {ex.Message}";
                // Optionally log exception
            }

            return result;
        }


        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();

            try
            {
                if (BaseParameter?.Ids == null || BaseParameter.Ids.Count == 0)
                {
                    result.Success = false;
                    result.Message = "Không có dữ liệu để xóa!";
                    return result;
                }

                // 1️⃣ Tạo danh sách parameter: @id0,@id1,@id2,...
                var paramNames = BaseParameter.Ids
                    .Select((x, i) => $"@id{i}")
                    .ToList();

                string sql = $@"
            DELETE FROM OQC_NG
            WHERE ID IN ({string.Join(",", paramNames)})
        ";

                // 2️⃣ Build MySqlParameter[]
                var parameters = BaseParameter.Ids
                    .Select((id, i) => new MySqlParameter(paramNames[i], id))
                    .ToArray();

                // 3️⃣ Execute
                string affectedRows = await MySQLHelper.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString,
                    sql,
                    parameters
                );

                result.Success = true;
                result.Message = $"Deleted: {affectedRows} row(s)!";
            }
            catch (Exception ex)
            {
                result.Success = false;
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
                result = await GetHistoryScanned(BaseParameter);

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
    }
}
