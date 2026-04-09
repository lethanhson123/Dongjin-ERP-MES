

namespace MESService.Implement
{
    public class E01Service : BaseService<torderlist, ItorderlistRepository>
    , IE01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public E01Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment)
            : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                // Load customer dropdown data (like VB.NET ComboBox loading)
                string sql = "SELECT `CD_SYS_NOTE`, `CD_IDX`, `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '2'";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBoxData = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBoxData.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
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
                if (BaseParameter?.ListSearchString != null)
                {
                    var TabSelection = BaseParameter.ListSearchString[0];

                    if (TabSelection == "TabPage1")
                    {
                        await HandleTabPage1Search(result, BaseParameter);
                    }
                    else if (TabSelection == "TabPage2")
                    {
                        await HandleTabPage2Search(result, BaseParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private async Task HandleTabPage1Search(BaseResult result, BaseParameter BaseParameter)
        {
            // Extract TabPage1 parameters
            var STB1 = BaseParameter.ListSearchString[1]; // APP
            var STB2 = BaseParameter.ListSearchString[2]; // CUSTOMER
            var STB3 = BaseParameter.ListSearchString[3]; // TYPE
            var ComboBox2 = BaseParameter.ListSearchString[4]; // SEQ
                                                              
            var AAA = "%" + STB1 + "%";
            var BBB = "%" + STB2 + "%";
            var CCC = "%" + STB3 + "%";
            var DDD = "%" + ComboBox2 + "%";
            if (DDD == "%ALL%")
            {
                DDD = "%%";
            }
            if (BBB == "%ALL%")
            {
                BBB = "%%";
            }
            string sql = @"SELECT 
(ttoolmaster2.`Status`) AS `STATUS`,
TTOOLMASTER.`APPLICATOR`, 
IFNULL(ttoolmaster2.`SEQ`, '-') AS `SEQ`, 
TTOOLMASTER.`MAX_CNT`, 
(SELECT `CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = TTOOLMASTER.`TOO_SUPPLY` AND TSCODE.`CDGR_IDX` ='2') AS `CD_SYS_NOTE`, 
IFNULL(ttoolmaster2.`TOT_WK_CNT`, 0) AS `TOT_WK_CNT`, 
IFNULL(ttoolmaster2.`WK_CNT`, 0) AS `WK_CNT`,
TTOOLMASTER.`SPP_NO`, 
TTOOLMASTER.`TYPE`, 
IFNULL(TTOOLMASTER.`MAX_CNT` - ttoolmaster2.`WK_CNT`, 0) AS `LCount`, 
IFNULL(ttoolmaster2.`WK_CNT` / TTOOLMASTER.`MAX_CNT`, 0) AS `RAT`, 
ttoolmaster2.`TOOLMASTER_IDX`, 
TTOOLMASTER.`TOOL_IDX`
FROM TTOOLMASTER 
LEFT OUTER JOIN ttoolmaster2 ON TTOOLMASTER.`TOOL_IDX` = ttoolmaster2.`TOOL_IDX`
WHERE TTOOLMASTER.`APPLICATOR` LIKE '" + AAA + @"' 
  AND TTOOLMASTER.`TYPE` LIKE '" + CCC + @"' 
  AND ttoolmaster2.`SEQ` LIKE '" + DDD + @"'
HAVING `CD_SYS_NOTE` LIKE '" + BBB + @"'   
ORDER BY `RAT` DESC LIMIT 500";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView1 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
        }
        private async Task HandleTabPage2Search(BaseResult result, BaseParameter BaseParameter)
        {
            // Extract TabPage2 parameters
            var TextBox1 = BaseParameter.ListSearchString[1]; // APPLICATOR
            var ComboBox3 = BaseParameter.ListSearchString[2]; // SEQ

            var HH = "%" + TextBox1 + "%";
            var GG = "%" + ComboBox3 + "%";

            if (GG == "%ALL%")
            {
                GG = "%%";
            }

            string sql = @"SELECT 
TTOOLMASTER.`APPLICATOR`, 
ttoolmaster2.`SEQ`, 
TTOOLMASTER.`MAX_CNT`, 
ttoolmaster2.`TOT_WK_CNT`, 
ttoolmaster2.`WK_CNT`, 
TTOOLMASTER.`SPP_NO`, 
TTOOLMASTER.`TYPE`, 
TTOOLMASTER.`GAUGE`, 
TTOOLMASTER.`COPL_NOR`, 
TTOOLMASTER.`COPL_SPE`, 
TTOOLMASTER.`INSPL_SEALTYPE`, 
TTOOLMASTER.`INSPL_NONSEAL`, 
TTOOLMASTER.`INSPL_XTYPE`, 
TTOOLMASTER.`INSPL_KTYPE`, 
TTOOLMASTER.`INSPL_SPE`, 
TTOOLMASTER.`ANVIL_NOR`, 
TTOOLMASTER.`ANVIL_SPE`, 
TTOOLMASTER.`CMU_NOR`, 
TTOOLMASTER.`CMU_SPE`, 
TTOOLMASTER.`IMU_NOR`, 
TTOOLMASTER.`IMU_NONSEAL`, 
TTOOLMASTER.`IMU_SPE`, 
TTOOLMASTER.`CUTPL_ONE`, 
TTOOLMASTER.`CUTPL_DET`, 
TTOOLMASTER.`CUTAN_ONE`, 
TTOOLMASTER.`CUTAN_DET`, 
TTOOLMASTER.`CUTHO_ONE`, 
TTOOLMASTER.`CUTHO_DET`, 
TTOOLMASTER.`RRBLK_ONE`, 
TTOOLMASTER.`RRBLK_DET`, 
TTOOLMASTER.`RRCUTHO_ONE`, 
TTOOLMASTER.`RRCUTHO_DET`, 
TTOOLMASTER.`FRCUTHO_ONE`, 
TTOOLMASTER.`FRCUTHO_DET`, 
TTOOLMASTER.`RRCUTAN_ONE`, 
TTOOLMASTER.`RRCUTAN_DET`, 
TTOOLMASTER.`WRDN_ONE`, 
TTOOLMASTER.`WRDN_DET`, 
TTOOLMASTER.`DESC`, 
TTOOLMASTER.`COMB_CODE`, 
TTOOLMASTER.`TOOL_IDX`, 
ttoolmaster2.`TOOLMASTER_IDX`
FROM TTOOLMASTER 
JOIN ttoolmaster2 ON TTOOLMASTER.`TOOL_IDX` = ttoolmaster2.`TOOL_IDX` 
WHERE TTOOLMASTER.`APPLICATOR` LIKE '" + HH + @"' 
  AND ttoolmaster2.`SEQ` LIKE '" + GG + @"'  
ORDER BY TTOOLMASTER.`APPLICATOR` LIMIT 50";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView5 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
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
                if (BaseParameter?.ListSearchString != null)
                {
                    // Parse selected tools for reset (from JavaScript)
                    var selectedToolsJson = BaseParameter.ListSearchString[1];
                    var selectedTools = JsonConvert.DeserializeObject<List<SelectedToolReset>>(selectedToolsJson);

                    if (selectedTools?.Count > 0)
                    {
                        // Sử dụng USER_IDX từ BaseParameter thay vì GlobalHelper.CurrentUser
                        string currentUser = BaseParameter.USER_IDX ?? "SYSTEM";
                        await ProcessToolResets(selectedTools, currentUser);
                        result.Message = "Tool counters reset successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private async Task ProcessToolResets(List<SelectedToolReset> selectedTools, string userId)
        {
            string historyValues = "";
            List<int> toolIndices = new List<int>();

            // Build history insert values
            foreach (var tool in selectedTools)
            {
                var value = $"(NOW(), {tool.TOOLMASTER_IDX}, {tool.WK_CNT}, {tool.TOT_WK_CNT}, 'Manager check', NOW(), '{userId}')";

                if (string.IsNullOrEmpty(historyValues))
                {
                    historyValues = value;
                }
                else
                {
                    historyValues += ", " + value;
                }

                toolIndices.Add(tool.TOOLMASTER_IDX);
            }

            // Insert history records
            string historySql = "INSERT INTO `TTOOLHISTORY` (`WORK_DTM`, `TOOL_IDX`, `WK_QTY`, `TOT_QTY`, `CONTENT`, `CREATE_DTM`, `CREATE_USER`) VALUES " + historyValues;
            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, historySql);

            // Reset work counts to 0
            foreach (int toolIdx in toolIndices)
            {
                string resetSql = $"UPDATE `ttoolmaster2` SET `WK_CNT`='0' WHERE `TOOLMASTER_IDX`={toolIdx}";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, resetSql);
            }
        }
        public virtual async Task<BaseResult> Button13_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    var toolIdx = BaseParameter.ListSearchString[0];
                    var seq = BaseParameter.ListSearchString[1];
                    var count = BaseParameter.ListSearchString[2];

                    // Lấy currentUser từ BaseParameter
                    string currentUser = BaseParameter.USER_IDX ?? "SYSTEM";

                    if (int.TryParse(toolIdx, out int toolIdxInt) && int.TryParse(count, out int countInt) && countInt > 0)
                    {
                        // Insert/Update tool count like VB.NET Button13_Click
                        string sql1 = $@"INSERT INTO ttoolmaster2 (`TOOL_IDX`, `SEQ`, `TOT_WK_CNT`, `WK_CNT`, `CREATE_DTM`, `CREATE_USER`)
VALUES ('{toolIdxInt}', '{seq}', {countInt}, {countInt}, NOW(), '{currentUser}')
ON DUPLICATE KEY UPDATE 
`TOT_WK_CNT` = `TOT_WK_CNT` + VALUES(`TOT_WK_CNT`), 
`WK_CNT` = `WK_CNT` + VALUES(`WK_CNT`), 
`UPDATE_DTM` = VALUES(`CREATE_DTM`), 
`UPDATE_USER` = VALUES(`CREATE_USER`)";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                        // Insert history record
                        string sql2 = $@"INSERT INTO TTOOLHISTORY (WORK_DTM,TOOL_IDX,WK_QTY,TOT_QTY,CONTENT,CREATE_DTM,CREATE_USER)
VALUES (NOW(),(select `TOOLMASTER_IDX` from ttoolmaster2 where TOOL_IDX = '{toolIdxInt}'),{countInt},(SELECT TOT_WK_CNT FROM ttoolmaster2 WHERE TOOL_IDX ='{toolIdxInt}' ),'Add Manually', NOW(),'{currentUser}') 
ON DUPLICATE KEY UPDATE 
`WK_QTY` = VALUES(`WK_QTY`), 
`TOT_QTY` = VALUES(`TOT_QTY`), 
`UPDATE_DTM` = VALUES(`CREATE_DTM`), 
`UPDATE_USER` = VALUES(`CREATE_USER`)";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                        result.Message = "Manual count added successfully";
                    }
                    else
                    {
                        result.Error = "Invalid tool ID or count value";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> LoadToolHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    var toolMasterIdx = BaseParameter.ListSearchString[0];

                    // Load tool history like VB.NET DataGridView5_SelectionChanged
                    string sql = $@"SELECT 
TTOOLMASTER.`APPLICATOR`, 
ttoolmaster2.`SEQ`, 
TTOOLHISTORY.`WORK_DTM`, 
TTOOLHISTORY.`WK_QTY`, 
TTOOLHISTORY.`TOT_QTY`, 
TTOOLHISTORY.`CONTENT`, 
TTOOLHISTORY.`CREATE_DTM`, 
TTOOLHISTORY.`CREATE_USER`, 
TTOOLHISTORY.`TOOL_HIS_IDX`, 
TTOOLHISTORY.`TOOL_IDX`
FROM TTOOLHISTORY, ttoolmaster2, TTOOLMASTER
WHERE TTOOLHISTORY.`TOOL_IDX` = ttoolmaster2.`TOOLMASTER_IDX` 
  AND ttoolmaster2.`TOOL_IDX` = TTOOLMASTER.`TOOL_IDX` 
  AND ttoolmaster2.`TOOLMASTER_IDX` = '{toolMasterIdx}'        
ORDER BY TTOOLHISTORY.`TOOL_HIS_IDX` DESC";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView6 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
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
                if (BaseParameter?.ListSearchString != null)
                {
                    var toolMasterIdx = BaseParameter.ListSearchString[0];

                    if (!string.IsNullOrEmpty(toolMasterIdx))
                    {
                        string currentUser = BaseParameter.USER_IDX ?? "SYSTEM";

                        string querySql = $"SELECT `TOOL_IDX`, `WK_CNT`, `TOT_WK_CNT` FROM `ttoolmaster2` WHERE `TOOLMASTER_IDX` = {toolMasterIdx}";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, querySql);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = ds.Tables[0].Rows[0];
                            int toolIdx = Convert.ToInt32(row["TOOL_IDX"]);
                            int wkCnt = Convert.ToInt32(row["WK_CNT"]);
                            int totWkCnt = Convert.ToInt32(row["TOT_WK_CNT"]);

                            string historySql = $"INSERT INTO `TTOOLHISTORY` (`WORK_DTM`, `TOOL_IDX`, `WK_QTY`, `TOT_QTY`, `CONTENT`, `CREATE_DTM`, `CREATE_USER`) VALUES (NOW(), {toolMasterIdx}, {wkCnt}, {totWkCnt}, 'Tool Deleted', NOW(), '{currentUser}')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, historySql);

                            string deleteSql = $"DELETE FROM `ttoolmaster2` WHERE `TOOLMASTER_IDX` = {toolMasterIdx}";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, deleteSql);

                            result.Message = "Tool deleted successfully";
                        }
                        else
                        {
                            result.Error = "Tool not found";
                        }
                    }
                    else
                    {
                        result.Error = "No tool selected for deletion";
                    }
                }
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
                if (BaseParameter?.ListSearchString != null)
                {
                    var applicator = BaseParameter.ListSearchString[0];
                    var seq = BaseParameter.ListSearchString[1];

                    // Tạo QR code data như VB.NET: APPLICATOR + SEQ
                    string qrData = applicator + seq;

                    // Generate QR code
                    byte[] qrCodeBytes = GenerateQRCodeBytes(qrData);

                    result.QRCodeData = Convert.ToBase64String(qrCodeBytes);
                    result.QRCodeText = qrData;
                    result.Message = "QR Code generated successfully";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private byte[] GenerateQRCodeBytes(string data)
        {
            try
            {
                // Sử dụng QRCoder library (install-package QRCoder)
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
                    var qrCode = new QRCode(qrCodeData);

                    // Tương đương VB.NET settings
                    using (var qrCodeImage = qrCode.GetGraphic(2, Color.Black, Color.White, false))
                    {
                        using (var stream = new MemoryStream())
                        {
                            qrCodeImage.Save(stream, ImageFormat.Png);
                            return stream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"QR Code generation failed: {ex.Message}");
            }
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
        public virtual async Task<BaseResult> ScanIn(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    string qrCode = BaseParameter.ListSearchString[0];
                    string currentUser =BaseParameter.ListSearchString[1];

                    if (string.IsNullOrWhiteSpace(qrCode))
                    {
                        result.ErrorNumber = -1;
                        return result;
                    }

                    string applicator = qrCode.Substring(0, qrCode.Length - 1);
                    string seq = qrCode.Substring(qrCode.Length - 1);

                    string checkSql = @"SELECT 
                        TTOOLMASTER.`TOOL_IDX`,
                        ttoolmaster2.`TOOLMASTER_IDX`,
                        ttoolmaster2.`Status`,
                        TTOOLMASTER.`APPLICATOR`
                    FROM 
                        TTOOLMASTER 
                        JOIN ttoolmaster2 ON TTOOLMASTER.`TOOL_IDX` = ttoolmaster2.`TOOL_IDX`
                    WHERE 
                        TTOOLMASTER.`APPLICATOR` = '" + applicator + @"' 
                        AND ttoolmaster2.`SEQ` = '" + seq + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkSql);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        result.ErrorNumber = -2;
                        return result;
                    }

                    int toolIdx = Convert.ToInt32(ds.Tables[0].Rows[0]["TOOL_IDX"]);
                    int toolMasterIdx = Convert.ToInt32(ds.Tables[0].Rows[0]["TOOLMASTER_IDX"]);
                    string status = ds.Tables[0].Rows[0]["Status"] as string;

                    if (status == "Available")
                    {
                        result.ErrorNumber = 0;
                        return result;
                    }
                    else if (status != "Working" && status != null)
                    {
                        result.ErrorNumber = -3;
                        return result;
                    }

                    string insertSql = @"INSERT INTO ttoolscanin
                        (`TOOL_IDX`, `SEQ`, `APPLICATOR_LOT`, `CREATE_DTM`, `CREATE_USER`)
                    VALUES
                        ('" + toolIdx + "', '" + seq + "', '" + qrCode + "', NOW(), '" + currentUser + "')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);

                    string updateSql = @"UPDATE ttoolmaster2
                    SET 
                        `Status` = 'Available',
                        `UPDATE_DTM` = NOW(),
                        `UPDATE_USER` = '" + currentUser + @"'
                    WHERE 
                        `TOOLMASTER_IDX` = " + toolMasterIdx;

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                    result.ErrorNumber = 1;
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = -99;
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> ScanOut(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    string qrCode = BaseParameter.ListSearchString[0];
                    string deviceQR =BaseParameter.ListSearchString[1];
                    string deviceGroup =BaseParameter.ListSearchString[2];
                    string deviceCode =BaseParameter.ListSearchString[3];
                    string currentUser =BaseParameter.ListSearchString[4];

                    if (string.IsNullOrWhiteSpace(qrCode))
                    {
                        result.ErrorNumber = -1;
                        return result;
                    }

                    string applicator = qrCode.Substring(0, qrCode.Length - 1);
                    string seq = qrCode.Substring(qrCode.Length - 1);

                    string checkSql = @"SELECT 
                TTOOLMASTER.`TOOL_IDX`,
                ttoolmaster2.`TOOLMASTER_IDX`,
                ttoolmaster2.`Status`
            FROM 
                TTOOLMASTER 
                JOIN ttoolmaster2 ON TTOOLMASTER.`TOOL_IDX` = ttoolmaster2.`TOOL_IDX`
            WHERE 
                TTOOLMASTER.`APPLICATOR` = '" + applicator + @"' 
                AND ttoolmaster2.`SEQ` = '" + seq + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkSql);

                    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        result.ErrorNumber = -2;
                        return result;
                    }

                    int toolIdx = Convert.ToInt32(ds.Tables[0].Rows[0]["TOOL_IDX"]);
                    int toolMasterIdx = Convert.ToInt32(ds.Tables[0].Rows[0]["TOOLMASTER_IDX"]);
                    string status = ds.Tables[0].Rows[0]["Status"] as string;

                    if (status == "Working")
                    {
                        result.ErrorNumber = 0;
                        return result;
                    }
                    else if (status != "Available" && status != null)
                    {
                        result.ErrorNumber = -3;
                        return result;
                    }

                    // Thêm bản ghi vào ttoolscanout với thông tin thiết bị
                    string insertSql = @"INSERT INTO ttoolscanout
                (`TOOL_IDX`, `SEQ`, `APPLICATOR_LOT`, `DeviceCode`, `DeviceGroup`, `CREATE_DTM`, `CREATE_USER`)
            VALUES
                ('" + toolIdx + "', '" + seq + "', '" + qrCode + "', '" + deviceCode + "', '" + deviceGroup + "', NOW(), '" + currentUser + "')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);

                    // Cập nhật trạng thái trong ttoolmaster2
                    string updateSql = @"UPDATE ttoolmaster2
            SET 
                `Status` = 'Working',
                `UPDATE_DTM` = NOW(),
                `UPDATE_USER` = '" + currentUser + @"'
            WHERE 
                `TOOLMASTER_IDX` = " + toolMasterIdx;

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                    result.ErrorNumber = 1;
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = -99;
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetScanInHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    string fromDate = BaseParameter.ListSearchString[0];
                    string toDate = BaseParameter.ListSearchString[1];
                    string searchText = BaseParameter.ListSearchString.Count > 2 ? BaseParameter.ListSearchString[2] : "";

                    if (string.IsNullOrEmpty(fromDate))
                        fromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                    if (string.IsNullOrEmpty(toDate))
                        toDate = DateTime.Now.ToString("yyyy-MM-dd");

                    string searchCondition = "";
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        searchCondition = @" AND (
                            TTOOLMASTER.`APPLICATOR` LIKE '%" + searchText + @"%'
                            OR ttoolscanin.`APPLICATOR_LOT` LIKE '%" + searchText + @"%'
                        )";
                    }

                    string sql = @"
                        SELECT 
                            ttoolscanin.`SCAN_IN_IDX` as CODE,
                            TTOOLMASTER.`APPLICATOR`,
                            ttoolscanin.`SEQ`,
                            ttoolscanin.`APPLICATOR_LOT`,
                            ttoolscanin.`CREATE_DTM`,
                            ttoolscanin.`CREATE_USER`
                        FROM 
                            ttoolscanin
                            JOIN TTOOLMASTER ON ttoolscanin.`TOOL_IDX` = TTOOLMASTER.`TOOL_IDX`
                        WHERE 
                            ttoolscanin.`CREATE_DTM` BETWEEN '" + fromDate + @" 00:00:00' AND '" + toDate + @" 23:59:59'
                            " + searchCondition + @"
                        ORDER BY 
                            ttoolscanin.`CREATE_DTM` DESC";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                    result.DataGridView3 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    result.ErrorNumber = 1;
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = -99;
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetScanOutHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.ListSearchString != null)
                {
                    string fromDate = BaseParameter.ListSearchString[0];
                    string toDate = BaseParameter.ListSearchString[1];
                    string searchText = BaseParameter.ListSearchString.Count > 2 ? BaseParameter.ListSearchString[2] : "";

                    if (string.IsNullOrEmpty(fromDate))
                        fromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                    if (string.IsNullOrEmpty(toDate))
                        toDate = DateTime.Now.ToString("yyyy-MM-dd");

                    string searchCondition = "";
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        searchCondition = @" AND (
                    TTOOLMASTER.`APPLICATOR` LIKE '%" + searchText + @"%'
                    OR ttoolscanout.`APPLICATOR_LOT` LIKE '%" + searchText + @"%'
                    OR ttoolscanout.`DeviceGroup` LIKE '%" + searchText + @"%'
                    OR ttoolscanout.`DeviceCode` LIKE '%" + searchText + @"%'
                )";
                    }

                    string sql = @"
                SELECT 
                    ttoolscanout.`SCAN_OUT_IDX` as CODE,
                    TTOOLMASTER.`APPLICATOR`,
                    ttoolscanout.`SEQ`,
                    ttoolscanout.`APPLICATOR_LOT`,
                    ttoolscanout.`DeviceGroup`,
                    ttoolscanout.`DeviceCode`,
                    ttoolscanout.`CREATE_DTM`,
                    ttoolscanout.`CREATE_USER`
                FROM 
                    ttoolscanout
                    JOIN TTOOLMASTER ON ttoolscanout.`TOOL_IDX` = TTOOLMASTER.`TOOL_IDX`
                WHERE 
                    ttoolscanout.`CREATE_DTM` BETWEEN '" + fromDate + @" 00:00:00' AND '" + toDate + @" 23:59:59'
                    " + searchCondition + @"
                ORDER BY 
                    ttoolscanout.`CREATE_DTM` DESC";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                    result.DataGridView4 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    result.ErrorNumber = 1;
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = -99;
                result.Error = ex.Message;
            }
            return result;
        }

    }
}

