
namespace MESService.Implement
{
    public class A04Service : BaseService<torderlist, ItorderlistRepository>, IA04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public A04Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment) : base(torderlistRepository)
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
                string sql = @"SELECT CD_IDX, CD_NM_HAN, CD_NM_EN, CDGR_IDX, CD_SYS_NOTE 
                               FROM TSCODE 
                               WHERE CDGR_IDX = '2'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.Listtscode = new List<tscode>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.Listtscode.AddRange(SQLHelper.ToList<tscode>(dt));
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
                if (BaseParameter.ListSearchString == null || BaseParameter.ListSearchString.Count < 3)
                {
                    result.Error = "Thiếu tham số tìm kiếm.";
                    return result;
                }

                string applicator = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                string customer = BaseParameter.ListSearchString[1] ?? "";
                string type = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                string customerCondition = customer == "ALL" ? "" :
                    $" AND (SELECT CD_SYS_NOTE FROM TSCODE WHERE CD_IDX = TTOOLMASTER.TOO_SUPPLY) LIKE @Customer";
                string sql = @"
                    SELECT TTOOLMASTER.APPLICATOR, ttoolmaster2.SEQ, 
                           (SELECT CD_SYS_NOTE FROM TSCODE WHERE CD_IDX = TTOOLMASTER.TOO_SUPPLY) AS CD_SYS_NOTE,
                           TTOOLMASTER.MAX_CNT, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.SPP_NO, 
                           TTOOLMASTER.TYPE, TTOOLMASTER.GAUGE, TTOOLMASTER.COPL_NOR, TTOOLMASTER.COPL_SPE, 
                           TTOOLMASTER.INSPL_SEALTYPE, TTOOLMASTER.INSPL_NONSEAL, TTOOLMASTER.INSPL_XTYPE, 
                           TTOOLMASTER.INSPL_KTYPE, TTOOLMASTER.INSPL_SPE, TTOOLMASTER.ANVIL_NOR, TTOOLMASTER.ANVIL_SPE, 
                           TTOOLMASTER.CMU_NOR, TTOOLMASTER.CMU_SPE, TTOOLMASTER.IMU_NOR, TTOOLMASTER.IMU_NONSEAL, 
                           TTOOLMASTER.IMU_SPE, TTOOLMASTER.CUTPL_ONE, TTOOLMASTER.CUTPL_DET, TTOOLMASTER.CUTAN_ONE, 
                           TTOOLMASTER.CUTAN_DET, TTOOLMASTER.CUTHO_ONE, TTOOLMASTER.CUTHO_DET, TTOOLMASTER.RRBLK_ONE, 
                           TTOOLMASTER.RRBLK_DET, TTOOLMASTER.RRCUTHO_ONE, TTOOLMASTER.RRCUTHO_DET, TTOOLMASTER.FRCUTHO_ONE, 
                           TTOOLMASTER.FRCUTHO_DET, TTOOLMASTER.RRCUTAN_ONE, TTOOLMASTER.RRCUTAN_DET, TTOOLMASTER.WRDN_ONE, 
                           TTOOLMASTER.WRDN_DET, TTOOLMASTER.COMB_CODE, TTOOLMASTER.DESC, TTOOLMASTER.TOOL_IDX, 
                           ttoolmaster2.TOOLMASTER_IDX, TTOOLMASTER.TOO_SUPPLY,
                           TTOOLMASTER.CREATE_DTM, TTOOLMASTER.CREATE_USER, TTOOLMASTER.UPDATE_DTM, TTOOLMASTER.UPDATE_USER
                    FROM TTOOLMASTER 
                    LEFT OUTER JOIN ttoolmaster2 ON TTOOLMASTER.TOOL_IDX = ttoolmaster2.TOOL_IDX 
                    WHERE TTOOLMASTER.APPLICATOR LIKE @APPLICATOR AND TTOOLMASTER.TYPE LIKE @TYPE" + customerCondition + @"
                    ORDER BY TTOOLMASTER.APPLICATOR, ttoolmaster2.SEQ
                    LIMIT " + GlobalHelper.ListCount;

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@APPLICATOR", applicator),
                    new MySqlParameter("@TYPE", type),
                    new MySqlParameter("@Customer", $"%{customer}%")
                };

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                result.DataGridView1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Không thể kết nối MES: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadToolHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString == null || BaseParameter.ListSearchString.Count < 1)
                {
                    result.Error = "Thiếu TOOL_IDX để truy vấn lịch sử.";
                    return result;
                }

                string toolIdx = BaseParameter.ListSearchString[0] ?? "";
                if (string.IsNullOrEmpty(toolIdx))
                {
                    result.Error = "TOOL_IDX không hợp lệ.";
                    return result;
                }

                string sql = @"
                    SELECT TTOOLMASTER.APPLICATOR, ttoolmaster2.SEQ, TTOOLHISTORY.WORK_DTM, TTOOLHISTORY.WK_QTY, 
                           TTOOLHISTORY.TOT_QTY, TTOOLHISTORY.CONTENT, TTOOLHISTORY.CREATE_DTM, 
                           TTOOLHISTORY.CREATE_USER, TTOOLHISTORY.TOOL_HIS_IDX, TTOOLHISTORY.TOOL_IDX
                    FROM TTOOLHISTORY
                    INNER JOIN ttoolmaster2 ON TTOOLHISTORY.TOOL_IDX = ttoolmaster2.TOOLMASTER_IDX
                    INNER JOIN TTOOLMASTER ON ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX
                    WHERE ttoolmaster2.TOOL_IDX = @TOOL_IDX
                    ORDER BY TTOOLHISTORY.TOOL_HIS_IDX DESC";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@TOOL_IDX", toolIdx)
                };

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                result.DataGridView6 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99994, Lỗi khi truy vấn TTOOLHISTORY: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy giá trị từ ListSearchString
                string toolIdx = "";
                string applicator = "";
                string customer = "";
                string maxCnt = "0";
                string sppNo = "";
                string type = "";
                string gauge = "";
                string coplNor = "";
                string coplSpe = "";
                string insplSealtype = "";
                string insplNonseal = "";
                string insplXtype = "";
                string insplKtype = "";
                string insplSpe = "";
                string anvilNor = "";
                string anvilSpe = "";
                string cmuNor = "";
                string cmuSpe = "";
                string imuNor = "";
                string imuNonseal = "";
                string imuSpe = "";
                string cutplOne = "";
                string cutplDet = "";
                string cutanOne = "";
                string cutanDet = "";
                string cuthoOne = "";
                string cuthoDet = "";
                string rrblkOne = "";
                string rrblkDet = "";
                string rrcuthoOne = "";
                string rrcuthoDet = "";
                string frcuthoOne = "";
                string frcuthoDet = "";
                string rrcutanOne = "";
                string rrcutanDet = "";
                string wrdnOne = "";
                string wrdnDet = "";
                string combCode = "";
                string desc = "";

                // Gán giá trị từ ListSearchString
                if (BaseParameter.ListSearchString != null)
                {
                    if (BaseParameter.ListSearchString.Count > 0) toolIdx = BaseParameter.ListSearchString[0] ?? "";
                    if (BaseParameter.ListSearchString.Count > 1) applicator = BaseParameter.ListSearchString[1] ?? "";
                    if (BaseParameter.ListSearchString.Count > 2) customer = BaseParameter.ListSearchString[2] ?? "";
                    if (BaseParameter.ListSearchString.Count > 3) maxCnt = BaseParameter.ListSearchString[3] ?? "0";
                    if (BaseParameter.ListSearchString.Count > 4) sppNo = BaseParameter.ListSearchString[4] ?? "";
                    if (BaseParameter.ListSearchString.Count > 5) type = BaseParameter.ListSearchString[5] ?? "";
                    if (BaseParameter.ListSearchString.Count > 6) gauge = BaseParameter.ListSearchString[6] ?? "";
                    if (BaseParameter.ListSearchString.Count > 7) coplNor = BaseParameter.ListSearchString[7] ?? "";
                    if (BaseParameter.ListSearchString.Count > 8) coplSpe = BaseParameter.ListSearchString[8] ?? "";
                    if (BaseParameter.ListSearchString.Count > 9) insplSealtype = BaseParameter.ListSearchString[9] ?? "";
                    if (BaseParameter.ListSearchString.Count > 10) insplNonseal = BaseParameter.ListSearchString[10] ?? "";
                    if (BaseParameter.ListSearchString.Count > 11) insplXtype = BaseParameter.ListSearchString[11] ?? "";
                    if (BaseParameter.ListSearchString.Count > 12) insplKtype = BaseParameter.ListSearchString[12] ?? "";
                    if (BaseParameter.ListSearchString.Count > 13) insplSpe = BaseParameter.ListSearchString[13] ?? "";
                    if (BaseParameter.ListSearchString.Count > 14) anvilNor = BaseParameter.ListSearchString[14] ?? "";
                    if (BaseParameter.ListSearchString.Count > 15) anvilSpe = BaseParameter.ListSearchString[15] ?? "";
                    if (BaseParameter.ListSearchString.Count > 16) cmuNor = BaseParameter.ListSearchString[16] ?? "";
                    if (BaseParameter.ListSearchString.Count > 17) cmuSpe = BaseParameter.ListSearchString[17] ?? "";
                    if (BaseParameter.ListSearchString.Count > 18) imuNor = BaseParameter.ListSearchString[18] ?? "";
                    if (BaseParameter.ListSearchString.Count > 19) imuNonseal = BaseParameter.ListSearchString[19] ?? "";
                    if (BaseParameter.ListSearchString.Count > 20) imuSpe = BaseParameter.ListSearchString[20] ?? "";
                    if (BaseParameter.ListSearchString.Count > 21) cutplOne = BaseParameter.ListSearchString[21] ?? "";
                    if (BaseParameter.ListSearchString.Count > 22) cutplDet = BaseParameter.ListSearchString[22] ?? "";
                    if (BaseParameter.ListSearchString.Count > 23) cutanOne = BaseParameter.ListSearchString[23] ?? "";
                    if (BaseParameter.ListSearchString.Count > 24) cutanDet = BaseParameter.ListSearchString[24] ?? "";
                    if (BaseParameter.ListSearchString.Count > 25) cuthoOne = BaseParameter.ListSearchString[25] ?? "";
                    if (BaseParameter.ListSearchString.Count > 26) cuthoDet = BaseParameter.ListSearchString[26] ?? "";
                    if (BaseParameter.ListSearchString.Count > 27) rrblkOne = BaseParameter.ListSearchString[27] ?? "";
                    if (BaseParameter.ListSearchString.Count > 28) rrblkDet = BaseParameter.ListSearchString[28] ?? "";
                    if (BaseParameter.ListSearchString.Count > 29) rrcuthoOne = BaseParameter.ListSearchString[29] ?? "";
                    if (BaseParameter.ListSearchString.Count > 30) rrcuthoDet = BaseParameter.ListSearchString[30] ?? "";
                    if (BaseParameter.ListSearchString.Count > 31) frcuthoOne = BaseParameter.ListSearchString[31] ?? "";
                    if (BaseParameter.ListSearchString.Count > 32) frcuthoDet = BaseParameter.ListSearchString[32] ?? "";
                    if (BaseParameter.ListSearchString.Count > 33) rrcutanOne = BaseParameter.ListSearchString[33] ?? "";
                    if (BaseParameter.ListSearchString.Count > 34) rrcutanDet = BaseParameter.ListSearchString[34] ?? "";
                    if (BaseParameter.ListSearchString.Count > 35) wrdnOne = BaseParameter.ListSearchString[35] ?? "";
                    if (BaseParameter.ListSearchString.Count > 36) wrdnDet = BaseParameter.ListSearchString[36] ?? "";
                    if (BaseParameter.ListSearchString.Count > 37) combCode = BaseParameter.ListSearchString[37] ?? "";
                    if (BaseParameter.ListSearchString.Count > 38) desc = BaseParameter.ListSearchString[38] ?? "";
                }

                // Kiểm tra giống hệt VB.NET
                if (string.IsNullOrEmpty(toolIdx))
                {
                    result.Error = "Data lựa chọn lỗi. (Không có lựa chọn)";
                    return result;
                }

                if (string.IsNullOrEmpty(applicator))
                {
                    result.Error = "APP Data lựa chọn lỗi. (Không có lựa chọn)";
                    return result;
                }

                // Mở kết nối và thực hiện câu lệnh SQL
                using (var connection = new MySqlConnection(GlobalHelper.MariaDBConectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;

                        // Câu lệnh SQL giống hệt trong VB.NET
                        if (toolIdx == "New")
                        {
                            command.CommandText = @"INSERT INTO TTOOLMASTER (`APPLICATOR`, `SEQ_TOT`, `MAX_CNT`, `TOO_SUPPLY`, `TOT_WK_CNT`, `WK_CNT`, `SPP_NO`, `TYPE`, `GAUGE`, `COPL_NOR`, `COPL_SPE`, `INSPL_SEALTYPE`, 
                   `INSPL_NONSEAL`, `INSPL_XTYPE`, `INSPL_KTYPE`, `INSPL_SPE`, `ANVIL_NOR`, `ANVIL_SPE`, `CMU_NOR`, `CMU_SPE`, `IMU_NOR`, `IMU_NONSEAL`, `IMU_SPE`, `CUTPL_ONE`, `CUTPL_DET`, `CUTAN_ONE`, 
                   `CUTAN_DET`, `CUTHO_ONE`, `CUTHO_DET`, `RRBLK_ONE`, `RRBLK_DET`, `RRCUTHO_ONE`, `RRCUTHO_DET`, `FRCUTHO_ONE`, `FRCUTHO_DET`, `RRCUTAN_ONE`, `RRCUTAN_DET`, `WRDN_ONE`, `WRDN_DET`, `COMB_CODE`, 
                   `DESC`, `CREATE_DTM`, `CREATE_USER`)
                   VALUES (@APPLICATOR, '0', @MAX_CNT, (SELECT TSCODE.`CD_IDX` FROM TSCODE WHERE TSCODE.`CD_SYS_NOTE` = @Customer AND CDGR_IDX = '2'), '0', '0', @SPP_NO, @TYPE, @GAUGE, @COPL_NOR, @COPL_SPE, @INSPL_SEALTYPE, @INSPL_NONSEAL, @INSPL_XTYPE, @INSPL_KTYPE, 
                            @INSPL_SPE, @ANVIL_NOR, @ANVIL_SPE, @CMU_NOR, @CMU_SPE, @IMU_NOR, @IMU_NONSEAL, @IMU_SPE, @CUTPL_ONE, @CUTPL_DET, @CUTAN_ONE, @CUTAN_DET, @CUTHO_ONE, 
                            @CUTHO_DET, @RRBLK_ONE, @RRBLK_DET, @RRCUTHO_ONE, @RRCUTHO_DET, @FRCUTHO_ONE, @FRCUTHO_DET, @RRCUTAN_ONE, @RRCUTAN_DET, @WRDN_ONE, @WRDN_DET, @COMB_CODE, @DESC, NOW(), @USER_IDX)";
                        }
                        else
                        {
                            command.CommandText = @"UPDATE TTOOLMASTER SET `SEQ_TOT` = '0', `MAX_CNT` = @MAX_CNT, `TOO_SUPPLY` = (SELECT TSCODE.`CD_IDX` FROM TSCODE WHERE TSCODE.`CD_SYS_NOTE` = @Customer AND `CDGR_IDX` = '2'), `TOT_WK_CNT` = '0', `WK_CNT` = '0', `SPP_NO` = @SPP_NO, `TYPE` = @TYPE, 
                   `GAUGE` = @GAUGE, `COPL_NOR` = @COPL_NOR, `COPL_SPE` = @COPL_SPE, `INSPL_SEALTYPE` = @INSPL_SEALTYPE, `INSPL_NONSEAL` = @INSPL_NONSEAL, `INSPL_XTYPE` = @INSPL_XTYPE, `INSPL_KTYPE` = @INSPL_KTYPE, `INSPL_SPE` = @INSPL_SPE, `ANVIL_NOR` = @ANVIL_NOR, `ANVIL_SPE` = @ANVIL_SPE, 
                   `CMU_NOR` = @CMU_NOR, `CMU_SPE` = @CMU_SPE, `IMU_NOR` = @IMU_NOR, `IMU_NONSEAL` = @IMU_NONSEAL, `IMU_SPE` = @IMU_SPE, `CUTPL_ONE` = @CUTPL_ONE, `CUTPL_DET` = @CUTPL_DET, `CUTAN_ONE` = @CUTAN_ONE, `CUTAN_DET` = @CUTAN_DET, `CUTHO_ONE` = @CUTHO_ONE, 
                   `CUTHO_DET` = @CUTHO_DET, `RRBLK_ONE` = @RRBLK_ONE, `RRBLK_DET` = @RRBLK_DET, `RRCUTHO_ONE` = @RRCUTHO_ONE, `RRCUTHO_DET` = @RRCUTHO_DET, `FRCUTHO_ONE` = @FRCUTHO_ONE, `FRCUTHO_DET` = @FRCUTHO_DET, `RRCUTAN_ONE` = @RRCUTAN_ONE, `RRCUTAN_DET` = @RRCUTAN_DET, `WRDN_ONE` = @WRDN_ONE, 
                   `WRDN_DET` = @WRDN_DET, `COMB_CODE` = @COMB_CODE, `DESC` = @DESC, UPDATE_DTM = NOW(), UPDATE_USER = @USER_IDX
                     WHERE `TOOL_IDX` = @TOOL_IDX";
                        }

                        // Thêm tham số giống hệt trong VB.NET
                        command.Parameters.AddWithValue("@APPLICATOR", applicator);
                        command.Parameters.AddWithValue("@Customer", customer);
                        command.Parameters.AddWithValue("@MAX_CNT", maxCnt);
                        command.Parameters.AddWithValue("@SPP_NO", sppNo);
                        command.Parameters.AddWithValue("@TYPE", type);
                        command.Parameters.AddWithValue("@GAUGE", gauge);
                        command.Parameters.AddWithValue("@COPL_NOR", coplNor);
                        command.Parameters.AddWithValue("@COPL_SPE", coplSpe);
                        command.Parameters.AddWithValue("@INSPL_SEALTYPE", insplSealtype);
                        command.Parameters.AddWithValue("@INSPL_NONSEAL", insplNonseal);
                        command.Parameters.AddWithValue("@INSPL_XTYPE", insplXtype);
                        command.Parameters.AddWithValue("@INSPL_KTYPE", insplKtype);
                        command.Parameters.AddWithValue("@INSPL_SPE", insplSpe);
                        command.Parameters.AddWithValue("@ANVIL_NOR", anvilNor);
                        command.Parameters.AddWithValue("@ANVIL_SPE", anvilSpe);
                        command.Parameters.AddWithValue("@CMU_NOR", cmuNor);
                        command.Parameters.AddWithValue("@CMU_SPE", cmuSpe);
                        command.Parameters.AddWithValue("@IMU_NOR", imuNor);
                        command.Parameters.AddWithValue("@IMU_NONSEAL", imuNonseal);
                        command.Parameters.AddWithValue("@IMU_SPE", imuSpe);
                        command.Parameters.AddWithValue("@CUTPL_ONE", cutplOne);
                        command.Parameters.AddWithValue("@CUTPL_DET", cutplDet);
                        command.Parameters.AddWithValue("@CUTAN_ONE", cutanOne);
                        command.Parameters.AddWithValue("@CUTAN_DET", cutanDet);
                        command.Parameters.AddWithValue("@CUTHO_ONE", cuthoOne);
                        command.Parameters.AddWithValue("@CUTHO_DET", cuthoDet);
                        command.Parameters.AddWithValue("@RRBLK_ONE", rrblkOne);
                        command.Parameters.AddWithValue("@RRBLK_DET", rrblkDet);
                        command.Parameters.AddWithValue("@RRCUTHO_ONE", rrcuthoOne);
                        command.Parameters.AddWithValue("@RRCUTHO_DET", rrcuthoDet);
                        command.Parameters.AddWithValue("@FRCUTHO_ONE", frcuthoOne);
                        command.Parameters.AddWithValue("@FRCUTHO_DET", frcuthoDet);
                        command.Parameters.AddWithValue("@RRCUTAN_ONE", rrcutanOne);
                        command.Parameters.AddWithValue("@RRCUTAN_DET", rrcutanDet);
                        command.Parameters.AddWithValue("@WRDN_ONE", wrdnOne);
                        command.Parameters.AddWithValue("@WRDN_DET", wrdnDet);
                        command.Parameters.AddWithValue("@COMB_CODE", combCode);
                        command.Parameters.AddWithValue("@DESC", desc);
                        command.Parameters.AddWithValue("@USER_IDX", BaseParameter.USER_IDX);
                        command.Parameters.AddWithValue("@TOOL_IDX", toolIdx);

                        // Thực thi câu lệnh
                        await command.ExecuteNonQueryAsync();

                        // Thêm vào ttoolmaster2 - giống hệt trong VB.NET
                        command.CommandText = @"INSERT INTO ttoolmaster2 (`TOOL_IDX`, `SEQ`, `TOT_WK_CNT`, `WK_CNT`, `CREATE_DTM`, `CREATE_USER`)
VALUES ((SELECT TTOOLMASTER.TOOL_IDX FROM TTOOLMASTER WHERE TTOOLMASTER.APPLICATOR = @APPLICATOR), 'A', '0', '0', NOW(), @USER_IDX)
ON DUPLICATE KEY UPDATE 
`TOT_WK_CNT` = VALUES(`TOT_WK_CNT`), `WK_CNT` = VALUES(`WK_CNT`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@APPLICATOR", applicator);
                        command.Parameters.AddWithValue("@USER_IDX", BaseParameter.USER_IDX);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                result.Message = "Lưu dữ liệu thành công.";
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99991, Lỗi khi lưu dữ liệu: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Message = "New";
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
                result.Message = "Cancel";
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
                result = await Buttonfind_Click(BaseParameter);
                if (result.Error != null)
                {
                    return result;
                }

                if (result.DataGridView1 == null || result.DataGridView1.Count == 0)
                {
                    result.Error = "Không có dữ liệu để xuất Excel.";
                    return result;
                }

                string sheetName = "A04";
                string fileName = $"{sheetName}_{GlobalHelper.InitializationDateTimeCode}.xlsx";
                var streamExport = new MemoryStream();
                using (var package = new ExcelPackage(streamExport))
                {
                    var workSheet = package.Workbook.Worksheets.Add(sheetName);
                    int row = 1;
                    int column = 1;

                    workSheet.Cells[row, column++].Value = "APPLICATOR";
                    workSheet.Cells[row, column++].Value = "SEQ";
                    workSheet.Cells[row, column++].Value = "CUSTOMER";
                    workSheet.Cells[row, column++].Value = "MAX_CNT";
                    workSheet.Cells[row, column++].Value = "TOT_WK_CNT";
                    workSheet.Cells[row, column++].Value = "WK_CNT";
                    workSheet.Cells[row, column++].Value = "SPP_NO";
                    workSheet.Cells[row, column++].Value = "TYPE";
                    workSheet.Cells[row, column++].Value = "GAUGE";
                    workSheet.Cells[row, column++].Value = "COPL_NOR";
                    workSheet.Cells[row, column++].Value = "COPL_SPE";
                    workSheet.Cells[row, column++].Value = "INSPL_SEALTYPE";
                    workSheet.Cells[row, column++].Value = "INSPL_NONSEAL";
                    workSheet.Cells[row, column++].Value = "INSPL_XTYPE";
                    workSheet.Cells[row, column++].Value = "INSPL_KTYPE";
                    workSheet.Cells[row, column++].Value = "INSPL_SPE";
                    workSheet.Cells[row, column++].Value = "ANVIL_NOR";
                    workSheet.Cells[row, column++].Value = "ANVIL_SPE";
                    workSheet.Cells[row, column++].Value = "CMU_NOR";
                    workSheet.Cells[row, column++].Value = "CMU_SPE";
                    workSheet.Cells[row, column++].Value = "IMU_NOR";
                    workSheet.Cells[row, column++].Value = "IMU_NONSEAL";
                    workSheet.Cells[row, column++].Value = "IMU_SPE";
                    workSheet.Cells[row, column++].Value = "CUTPL_ONE";
                    workSheet.Cells[row, column++].Value = "CUTPL_DET";
                    workSheet.Cells[row, column++].Value = "CUTAN_ONE";
                    workSheet.Cells[row, column++].Value = "CUTAN_DET";
                    workSheet.Cells[row, column++].Value = "CUTHO_ONE";
                    workSheet.Cells[row, column++].Value = "CUTHO_DET";
                    workSheet.Cells[row, column++].Value = "RRBLK_ONE";
                    workSheet.Cells[row, column++].Value = "RRBLK_DET";
                    workSheet.Cells[row, column++].Value = "RRCUTHO_ONE";
                    workSheet.Cells[row, column++].Value = "RRCUTHO_DET";
                    workSheet.Cells[row, column++].Value = "FRCUTHO_ONE";
                    workSheet.Cells[row, column++].Value = "FRCUTHO_DET";
                    workSheet.Cells[row, column++].Value = "RRCUTAN_ONE";
                    workSheet.Cells[row, column++].Value = "RRCUTAN_DET";
                    workSheet.Cells[row, column++].Value = "WRDN_ONE";
                    workSheet.Cells[row, column++].Value = "WRDN_DET";
                    workSheet.Cells[row, column++].Value = "COMB_CODE";
                    workSheet.Cells[row, column++].Value = "DESC";
                    workSheet.Cells[row, column++].Value = "TOOL_IDX";
                    workSheet.Cells[row, column++].Value = "TOOLMASTER_IDX";
                    workSheet.Cells[row, column++].Value = "TOO_SUPPLY";

                    for (int i = 1; i < column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Bold = true;
                        workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 14;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    row++;
                    foreach (var item in result.DataGridView1)
                    {
                        column = 1;
                        workSheet.Cells[row, column++].Value = item.APPLICATOR;
                        workSheet.Cells[row, column++].Value = item.SEQ;
                        workSheet.Cells[row, column++].Value = item.CD_SYS_NOTE;
                        workSheet.Cells[row, column++].Value = item.MAX_CNT;
                        workSheet.Cells[row, column++].Value = item.TOT_WK_CNT;
                        workSheet.Cells[row, column++].Value = item.WK_CNT;
                        workSheet.Cells[row, column++].Value = item.SPP_NO;
                        workSheet.Cells[row, column++].Value = item.TYPE;
                        workSheet.Cells[row, column++].Value = item.GAUGE;
                        workSheet.Cells[row, column++].Value = item.COPL_NOR;
                        workSheet.Cells[row, column++].Value = item.COPL_SPE;
                        workSheet.Cells[row, column++].Value = item.INSPL_SEALTYPE;
                        workSheet.Cells[row, column++].Value = item.INSPL_NONSEAL;
                        workSheet.Cells[row, column++].Value = item.INSPL_XTYPE;
                        workSheet.Cells[row, column++].Value = item.INSPL_KTYPE;
                        workSheet.Cells[row, column++].Value = item.INSPL_SPE;
                        workSheet.Cells[row, column++].Value = item.ANVIL_NOR;
                        workSheet.Cells[row, column++].Value = item.ANVIL_SPE;
                        workSheet.Cells[row, column++].Value = item.CMU_NOR;
                        workSheet.Cells[row, column++].Value = item.CMU_SPE;
                        workSheet.Cells[row, column++].Value = item.IMU_NOR;
                        workSheet.Cells[row, column++].Value = item.IMU_NONSEAL;
                        workSheet.Cells[row, column++].Value = item.IMU_SPE;
                        workSheet.Cells[row, column++].Value = item.CUTPL_ONE;
                        workSheet.Cells[row, column++].Value = item.CUTPL_DET;
                        workSheet.Cells[row, column++].Value = item.CUTAN_ONE;
                        workSheet.Cells[row, column++].Value = item.CUTAN_DET;
                        workSheet.Cells[row, column++].Value = item.CUTHO_ONE;
                        workSheet.Cells[row, column++].Value = item.CUTHO_DET;
                        workSheet.Cells[row, column++].Value = item.RRBLK_ONE;
                        workSheet.Cells[row, column++].Value = item.RRBLK_DET;
                        workSheet.Cells[row, column++].Value = item.RRCUTHO_ONE;
                        workSheet.Cells[row, column++].Value = item.RRCUTHO_DET;
                        workSheet.Cells[row, column++].Value = item.FRCUTHO_ONE;
                        workSheet.Cells[row, column++].Value = item.FRCUTHO_DET;
                        workSheet.Cells[row, column++].Value = item.RRCUTAN_ONE;
                        workSheet.Cells[row, column++].Value = item.RRCUTAN_DET;
                        workSheet.Cells[row, column++].Value = item.WRDN_ONE;
                        workSheet.Cells[row, column++].Value = item.WRDN_DET;
                        workSheet.Cells[row, column++].Value = item.COMB_CODE;
                        workSheet.Cells[row, column++].Value = item.DESC;
                        workSheet.Cells[row, column++].Value = item.TOOL_IDX;
                        workSheet.Cells[row, column++].Value = item.TOOLMASTER_IDX;
                        workSheet.Cells[row, column++].Value = item.TOO_SUPPLY;

                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        row++;
                    }

                    for (int i = 1; i < column; i++)
                    {
                        workSheet.Column(i).AutoFit();
                    }
                    package.Save();
                }

                streamExport.Position = 0;
                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, sheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Code = $"{GlobalHelper.URLSite}/{GlobalHelper.Download}/{sheetName}/{fileName}";
                result.Message = "Xuất Excel thành công.";
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99992, Lỗi khi xuất Excel: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString == null || BaseParameter.ListSearchString.Count < 4)
                {
                    result.Error = "Thiếu dữ liệu để lưu.";
                    return result;
                }

                string toolIdx = BaseParameter.ListSearchString[0] ?? "";
                string applicator = BaseParameter.ListSearchString[1] ?? "";
                string seq = BaseParameter.ListSearchString[2] ?? "";
                string totWkCnt = BaseParameter.ListSearchString[3] ?? "0";

                if (string.IsNullOrEmpty(toolIdx) || toolIdx == "0")
                {
                    result.Error = "Vui lòng chọn APPLICATOR.";
                    return result;
                }
                if (toolIdx != "New")
                {
                    result.Error = "Chỉ có thể lưu dữ liệu mới.";
                    return result;
                }

                string sql = @"
                    INSERT INTO ttoolmaster2 (
                        TOOL_IDX, SEQ, TOT_WK_CNT, WK_CNT, CREATE_DTM, CREATE_USER
                    ) VALUES (
                        (SELECT TOOL_IDX FROM TTOOLMASTER WHERE APPLICATOR = @APPLICATOR LIMIT 1),
                        @SEQ, @TOT_WK_CNT, '0', NOW(), @USER_IDX
                    ) ON DUPLICATE KEY UPDATE 
                        TOT_WK_CNT = VALUES(TOT_WK_CNT), WK_CNT = VALUES(WK_CNT), 
                        UPDATE_DTM = NOW(), UPDATE_USER = @USER_IDX";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@APPLICATOR", applicator),
                    new MySqlParameter("@SEQ", seq),
                    new MySqlParameter("@TOT_WK_CNT", int.TryParse(totWkCnt, out int totWkCntVal) ? totWkCntVal : 0),
                    new MySqlParameter("@USER_IDX", BaseParameter.USER_IDX)
                };

                string executeResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                if (executeResult.Contains("Error"))
                {
                    result.Error = $"Lỗi khi lưu ttoolmaster2: {executeResult}";
                    return result;
                }

                result.Message = "Lưu dữ liệu thành công.";
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99993, Lỗi khi lưu dữ liệu: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString == null || BaseParameter.ListSearchString.Count < 1)
                {
                    result.Error = "Thiếu dữ liệu.";
                    return result;
                }

                string toolIdx = BaseParameter.ListSearchString[0] ?? "";
                if (string.IsNullOrEmpty(toolIdx) || toolIdx == "0")
                {
                    result.Error = "Vui lòng chọn APPLICATOR.";
                    return result;
                }

                result.Message = "New";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Message = "Cancel";
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
                // Rỗng trong VB.NET
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
                // Rỗng trong VB.NET
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
                // Rỗng trong VB.NET
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
                result.Code = "/WMP_PLAY?FLNM=A04";
                result.Message = "Mở form trợ giúp.";
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
                result.Message = "Đóng form.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}