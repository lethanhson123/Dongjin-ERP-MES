namespace MESService.Implement
{
    public class D17Service : BaseService<tfg_inventory, Itfg_inventoryRepository>, ID17Service
    {
        private readonly Itfg_inventoryRepository _tfg_inventoryRepository;
        private readonly Itfg_packing_detailRepository _tfg_packing_detailRepository;
        private readonly Itfg_historyRepository _tfg_historyRepository;
        private readonly ItdpdmtimRepository _tdpdmtimRepository;
        private readonly ItspartRepository _tspartRepository;

        public D17Service(
            Itfg_inventoryRepository tfg_inventoryRepository,
            Itfg_packing_detailRepository tfg_packing_detailRepository,
            Itfg_historyRepository tfg_historyRepository,
            ItdpdmtimRepository tdpdmtimRepository,
            ItspartRepository tspartRepository
        ) : base(tfg_inventoryRepository)
        {
            _tfg_inventoryRepository = tfg_inventoryRepository;
            _tfg_packing_detailRepository = tfg_packing_detailRepository;
            _tfg_historyRepository = tfg_historyRepository;
            _tdpdmtimRepository = tdpdmtimRepository;
            _tspartRepository = tspartRepository;
        }

        public override void Initialization(tfg_inventory model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
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
                if (!string.IsNullOrEmpty(BaseParameter.Barcode))
                {
                    string packingLot = CleanStringProperties(BaseParameter.Barcode.ToUpper().Trim());

                    if (BaseParameter.Code == "LoadPackingDetail")
                    {
                        result = await LoadPackingDetail(packingLot);
                    }
                    else if (BaseParameter.Code == "ScanReceive")
                    {
                        result = await FG_ReceivePackingLot(packingLot, BaseParameter.USER_ID, BaseParameter.SkipSNPCheck);
                    }
                }
                else
                {
                    if (BaseParameter.Code == "SearchInventory")
                    {
                        result = await SearchInventory(BaseParameter);
                    }
                    else if (BaseParameter.Code == "SearchHistory")
                    {
                        result = await SearchHistory(BaseParameter);
                    }
                    else if (BaseParameter.Code == "SearchReport")
                    {
                        result = await SearchReport(BaseParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> FG_ReceivePackingLot(string packingLot, string userID, bool skipSNPCheck = false)
        {
            BaseResult result = new BaseResult();
            try
            {
                var existInv = await _tfg_inventoryRepository
                    .GetByCondition(x => x.PACKING_LOT == packingLot)
                    .FirstOrDefaultAsync();

                if (existInv != null)
                {
                    result.ErrorNumber = -2;
                    result.Message = "Packing Lot đã nhập kho FG";
                    return result;
                }

                string sql = @"
                    SELECT
                        t.VLID_GRP AS PACKING_LOT,
                        t.VLID_PART_IDX AS PART_IDX,
                        p.PART_SNP AS SNP_QTY,
                        p.PART_NO,
                        p.PART_NM,
                        COUNT(*) AS ACTUAL_QTY,
                        SUM(CASE WHEN t.REWORK_YN = 'Y' THEN 1 ELSE 0 END) AS REWORK_QTY,
                        MAX(t.VLID_DSCN_YN) AS DSCN_YN,
                        MAX(t.CREATE_DTM) AS QC_DATE,
                        MAX(t.CREATE_USER) AS QC_USER,
                        SUBSTRING(t.VLID_GRP, LENGTH(t.VLID_GRP) - 2, 3) AS CUSTOMER_CODE
                    FROM tdpdmtim t
                    JOIN tspart p ON t.VLID_PART_IDX = p.PART_IDX
                    WHERE t.VLID_GRP = @PackingLot
                    GROUP BY t.VLID_GRP, t.VLID_PART_IDX";

                var parameters = new[] {
                    new MySqlParameter("@PackingLot", MySqlDbType.VarChar) { Value = packingLot }
                };

                var ds = await MySQLHelper.FillDataSetBySQLAsync(
                    GlobalHelper.MariaDBConectionString, sql, parameters);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    result.ErrorNumber = -3;
                    result.Message = "Packing Lot không tồn tại";
                    return result;
                }

                var row = ds.Tables[0].Rows[0];
                int partIdx = Convert.ToInt32(row["PART_IDX"]);
                int snpQty = Convert.ToInt32(row["SNP_QTY"]);
                string partNo = row["PART_NO"].ToString();
                string partNm = row["PART_NM"].ToString();
                int actualQty = Convert.ToInt32(row["ACTUAL_QTY"]);
                int reworkQty = Convert.ToInt32(row["REWORK_QTY"]);
                string dscnYN = row["DSCN_YN"].ToString();
                DateTime qcDate = Convert.ToDateTime(row["QC_DATE"]);
                string qcUser = row["QC_USER"].ToString();
                string customerCode = row["CUSTOMER_CODE"].ToString();

                if (!skipSNPCheck && actualQty != snpQty)
                {
                    result.ErrorNumber = -4;
                    result.Message = $"Số lượng không khớp: SNP={snpQty}, Actual={actualQty}";
                    return result;
                }

                if (dscnYN == "Y")
                {
                    result.ErrorNumber = -5;
                    result.Message = "Packing Lot đã ship";
                    return result;
                }

                var newInv = new tfg_inventory
                {
                    PACKING_LOT = packingLot,
                    PART_IDX = partIdx,
                    PART_NO = partNo,
                    PART_NM = partNm,
                    SNP_QTY = snpQty,
                    ACTUAL_QTY = actualQty,
                    REWORK_QTY = reworkQty,
                    FG_IN_DATE = DateTime.Now,
                    FG_IN_USER = userID,
                    STATUS = "IN",
                    QC_DATE = qcDate,
                    QC_USER = qcUser,
                    CUSTOMER_CODE = customerCode,
                    CREATE_DTM = DateTime.Now
                };
                await _tfg_inventoryRepository.AddAsync(newInv);

                var newHist = new tfg_history
                {
                    PACKING_LOT = packingLot,
                    PART_IDX = partIdx,
                    PART_NO = partNo,
                    PART_NM = partNm,
                    SNP_QTY = snpQty,
                    ACTUAL_QTY = actualQty,
                    REWORK_QTY = reworkQty,
                    TRANS_TYPE = "IN",
                    TRANS_DATE = DateTime.Now,
                    TRANS_USER = userID,
                    CUSTOMER_CODE = customerCode,
                    REMARK = "Nhập kho từ QC",
                    CREATE_DTM = DateTime.Now
                };
                await _tfg_historyRepository.AddAsync(newHist);

                var lotCodes = await _tdpdmtimRepository
                    .GetByCondition(x => x.VLID_GRP == packingLot)
                    .ToListAsync();

                foreach (var item in lotCodes)
                {
                    var detail = new tfg_packing_detail
                    {
                        PACKING_LOT = packingLot,
                        LOT_CODE = item.VLID_BARCODE,
                        PART_IDX = item.VLID_PART_IDX.Value,
                        PART_NO = partNo,
                        ECN_NO = item.VLID_BARCODE.Substring(item.VLID_BARCODE.Length - 6).Trim(),
                        REWORK_YN = item.REWORK_YN,
                        QC_SCAN_DATE = item.CREATE_DTM,
                        QC_USER = item.CREATE_USER,
                        FG_IN_DATE = DateTime.Now,
                        FG_IN_USER = userID,
                        CREATE_DTM = DateTime.Now
                    };
                    await _tfg_packing_detailRepository.AddAsync(detail);
                }

                result.ErrorNumber = 0;
                result.Message = $"Nhập kho thành công: {packingLot} ({actualQty} EA)";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorNumber = -999;
            }
            return result;
        }

        private async Task<BaseResult> LoadPackingDetail(string packingLot)
        {
            BaseResult result = new BaseResult();
            try
            {
                var inventory = await _tfg_inventoryRepository
                    .GetByCondition(x => x.PACKING_LOT == packingLot)
                    .FirstOrDefaultAsync();

                if (inventory == null)
                {
                    result.ErrorNumber = -1;
                    result.Message = "Packing Lot không tồn tại trong FG Inventory";
                    return result;
                }

                var details = await _tfg_packing_detailRepository
                    .GetByCondition(x => x.PACKING_LOT == packingLot)
                    .OrderBy(x => x.FG_DETAIL_IDX)
                    .ToListAsync();

                result.tfg_inventory = inventory;
                result.tfg_packing_detailList = details;
                result.ErrorNumber = 0;
                result.Success = true;
                result.Message = $"Loaded {details.Count} lot codes";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorNumber = -999;
            }
            return result;
        }

        private async Task<BaseResult> SearchInventory(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _tfg_inventoryRepository.GetByCondition(x => x.STATUS == "IN");

                if (!string.IsNullOrEmpty(baseParameter.PartNo))
                    query = query.Where(x => x.PART_NO.Contains(baseParameter.PartNo));

                if (!string.IsNullOrEmpty(baseParameter.PackingLotCode))
                    query = query.Where(x => x.PACKING_LOT.Contains(baseParameter.PackingLotCode));

                if (!string.IsNullOrEmpty(baseParameter.CustomerCode))
                    query = query.Where(x => x.CUSTOMER_CODE.Contains(baseParameter.CustomerCode));

                result.tfg_inventoryList = await query
                    .OrderByDescending(x => x.FG_IN_DATE)
                    .ToListAsync();

                result.ErrorNumber = 0;
                result.Success = true;
                result.Message = $"Found {result.tfg_inventoryList.Count} boxes";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> SearchHistory(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _tfg_historyRepository.GetByCondition(x =>
                    x.TRANS_DATE >= baseParameter.FromDate &&
                    x.TRANS_DATE <= baseParameter.ToDate);

                if (!string.IsNullOrEmpty(baseParameter.FilterType) && baseParameter.FilterType != "ALL")
                    query = query.Where(x => x.TRANS_TYPE == baseParameter.FilterType);

                if (!string.IsNullOrEmpty(baseParameter.PartNo))
                    query = query.Where(x => x.PART_NO.Contains(baseParameter.PartNo));

                if (!string.IsNullOrEmpty(baseParameter.PackingLotCode))
                    query = query.Where(x => x.PACKING_LOT.Contains(baseParameter.PackingLotCode));

                result.tfg_historyList = await query
                    .OrderByDescending(x => x.TRANS_DATE)
                    .Take(1000)
                    .ToListAsync();

                result.ErrorNumber = 0;
                result.Success = true;
                result.Message = $"Found {result.tfg_historyList.Count} records";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private async Task<BaseResult> SearchReport(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                int year = baseParameter.Year ?? DateTime.Now.Year;
                int month = baseParameter.Month ?? DateTime.Now.Month;

                // Calculate date range for the month
                DateTime fromDate = new DateTime(year, month, 1);
                DateTime toDate = fromDate.AddMonths(1).AddSeconds(-1);

                // Query history data for the month
                var historyData = await _tfg_historyRepository
                    .GetByCondition(x =>
                        x.TRANS_DATE >= fromDate &&
                        x.TRANS_DATE <= toDate &&
                        x.TRANS_TYPE == "IN"
                    )
                    .OrderBy(x => x.PACKING_LOT)
                    .ThenBy(x => x.TRANS_DATE)
                    .ToListAsync();

                if (historyData == null || historyData.Count == 0)
                {
                    result.Success = true;
                    result.ReportData = new List<Dictionary<string, object>>();
                    result.Message = "No data found";
                    return result;
                }

                // Group by PART_NO and aggregate by day
                var grouped = historyData
                    .GroupBy(x => new { x.PART_NO, x.PART_NM, x.SNP_QTY })
                    .Select(g => new
                    {
                        g.Key.PART_NO,
                        g.Key.PART_NM,
                        g.Key.SNP_QTY,
                        Items = g.ToList()
                    })
                    .ToList();

                // Build report data
                var reportData = new List<Dictionary<string, object>>();

                foreach (var group in grouped)
                {
                    var row = new Dictionary<string, object>
            {
                { "PART_NO", group.PART_NO },
                { "PART_NM", group.PART_NM },
                { "SNP_QTY", group.SNP_QTY }
            };

                    // Initialize all 31 days with 0
                    for (int day = 1; day <= 31; day++)
                    {
                        string dayKey = $"DAY_{day:D2}";
                        row[dayKey] = 0;
                    }

                    // Fill actual data
                    foreach (var item in group.Items)
                    {
                        int day = item.TRANS_DATE.Day;
                        string dayKey = $"DAY_{day:D2}";
                        row[dayKey] = (int)row[dayKey] + item.ACTUAL_QTY;
                    }

                    reportData.Add(row);
                }

                result.ReportData = reportData;
                result.Success = true;
                result.Message = $"Found {reportData.Count} records";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private static string CleanStringProperties(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return new string(input.Where(c => !char.IsControl(c)).ToArray()).Trim();
        }

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
    }
}