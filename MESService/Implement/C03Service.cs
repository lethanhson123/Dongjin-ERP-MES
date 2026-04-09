namespace MESService.Implement
{
    public class C03Service : BaseService<torderlist, ItorderlistRepository>, IC03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;


        // Variables from VB form
        public string sql;
        private int SQLCHK = 0;
        private string StrCulture = "";
        private string C_USER = "";
        private bool SPST_CHK = false;
        private int LEAD_CONT_IN = 0;
        private int LEAD_CONT_OUT = 0;
        private string LOG_DB07_TEXT;

        private string LOG_DB08;

        public C03Service(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment
            , ICategoryDepartmentRepository categoryDepartmentRepository
            ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
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
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Equivalent to Buttonfind_Click in VB (Reset function)
        public async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string selectedTab = BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count > 0
                    ? BaseParameter.ListSearchString[0]
                    : "";

                switch (selectedTab)
                {
                    case "TabPage1":
                        result = await IN_BACODE(BaseParameter);
                        break;

                    case "TabPage2":
                        result = await OUT_BACODE(BaseParameter);
                        break;

                    case "TabPage3":
                        result = await STK_BACODE(BaseParameter);
                        break;

                    case "TabPage4":
                        result = await INOUT_BACODE(BaseParameter);
                        break;
                    case "TabPage5":

                        result = await MAG_BACODE(BaseParameter);
                        break;


                    case "TabPage7":
                        result = await LONG_TERM(BaseParameter);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DATA_ADD_IN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string barcode = BaseParameter.ListSearchString[0].Trim();
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "";

                LOG_DB08 = barcode;

                string leadChk01 = barcode.Substring(0, barcode.LastIndexOf("$$") + 2);
                string leadNo = barcode.Substring(0, barcode.IndexOf("$$"));
                string query = $"SELECT `TBCTOTAL`.`TORDER_BARCODENM`, `TBCTOTAL`.`Barcode_SEQ`, " +
                              $"MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, " +
                              $"(SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`, " +
                              $"(SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`, " +
                              $"(SELECT COUNT(`TBCTOTAL`.`TORDER_BARCODENM`) AS `BC_COUNT` FROM ((SELECT * FROM (SELECT * FROM TORDER_BARCODE WHERE `DSCN_YN`='Y' UNION SELECT * FROM torder_barcode_lp WHERE `DSCN_YN`='Y') AS `TB1`) UNION SELECT * FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='Y') AS `TBCTOTAL` WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '{leadChk01}') AS `BC_count`, " +
                              $"(SELECT COUNT(trackmtim.`BARCODE_NM`) FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{leadChk01}') AS `IN_count`, " +
                              $"`TBCTOTAL`.`TORDER_BARCODE_IDX`, `TBCTOTAL`.`TB_NM` " +
                              $"FROM ((SELECT * FROM (SELECT *, 'KOMAX' AS `TB_NM` FROM TORDER_BARCODE WHERE `DSCN_YN`='Y') AS `TB1`) UNION SELECT *, 'SPST' AS `TB_NM` FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='Y') AS `TBCTOTAL` " +
                              $"WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '{barcode}' ORDER BY `UPDATE_DTM` DESC LIMIT 50";

                DataSet ds011 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                bool leadChk = false;
                SPST_CHK = false;

                if (ds011.Tables[0].Rows.Count <= 0)
                {
                    query = $"SELECT `TBCTOTAL`.`TORDER_BARCODENM`, `TBCTOTAL`.`Barcode_SEQ`, " +
                            $"MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, " +
                            $"(SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`, " +
                            $"(SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`, " +
                            $"(SELECT COUNT(`TBCTOTAL`.`TORDER_BARCODENM`) AS `BC_COUNT` FROM ((SELECT * FROM (SELECT * FROM TORDER_BARCODE WHERE `DSCN_YN`='N' UNION SELECT * FROM torder_barcode_lp WHERE `DSCN_YN`='N') AS `TB1`) UNION SELECT * FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='N') AS `TBCTOTAL` WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '{leadChk01}') AS `BC_count`, " +
                            $"(SELECT COUNT(trackmtim.`BARCODE_NM`) FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{leadChk01}') AS `IN_count`, " +
                            $"`TBCTOTAL`.`TORDER_BARCODE_IDX`, `TBCTOTAL`.`TB_NM` " +
                            $"FROM ((SELECT * FROM (SELECT *, 'KOMAX' AS `TB_NM` FROM TORDER_BARCODE WHERE `DSCN_YN`='N') AS `TB1`) UNION SELECT *, 'SPST' AS `TB_NM` FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='N') AS `TBCTOTAL` " +
                            $"WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '{barcode}' ORDER BY `UPDATE_DTM` DESC LIMIT 50";
                    DataSet ds022 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                    if (ds022.Tables[0].Rows.Count <= 0)
                    {
                        leadChk = true;
                        SPST_CHK = true;
                    }
                    else
                    {
                        result.Error = "Không có tài liệu đăng ký thực tế sản xuất\nNo production record registered. Please request registration.";
                        result.Code = "SOUND_ERROR";
                        return result;
                    }
                }
                else
                {
                    result.Label4 = ds011.Tables[0].Rows[0]["LEAD_NO"]?.ToString() ?? "";
                    result.Label5 = ds011.Tables[0].Rows[0]["HOOR_RACK"]?.ToString() ?? "";
                    result.Label12 = $"{ds011.Tables[0].Rows[0]["IN_count"]}/{ds011.Tables[0].Rows[0]["BC_count"]}";
                    leadChk = true;
                }

                query = $"SELECT * FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '{leadNo}'";
                DataSet ds023 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                if (ds023.Tables[0].Rows.Count <= 0)
                {
                    result.Error = "LEAD NO chưa được đăng ký\nUnregistered LEAD NO.";
                    result.Code = "SOUND_ERROR";
                    return result;
                }

                if (leadChk)
                {
                    query = $"SELECT trackmtim.`RACKDTM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{barcode}' AND `RACKOUT_YN` = 'Y'";
                    DataSet ds014 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                    if (ds014.Tables[0].Rows.Count > 0)
                    {
                        result.Error = $"Đã nhập kho ngày: {ds014.Tables[0].Rows[0]["RACKDTM"]}\nAlready processed input";
                        result.Code = "CONFIRM_REINPUT";
                        return result;
                    }
                    else
                    {
                        query = $"SELECT trackmtim.`RACKDTM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{barcode}'";
                        DataSet ds020 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                        if (ds020.Tables[0].Rows.Count > 0)
                        {
                            result.Error = "Barcode đã xử lý trước đó\nAlready processed barcode";
                            result.Code = "SOUND_ERROR";
                            return result;
                        }
                        else
                        {
                            var saveResult = await DATE_SAVE_IN(barcode, currentUser, BaseParameter.CompanyID, BaseParameter.CategoryDepartmentID);
                            if (!string.IsNullOrEmpty(saveResult.Error))
                            {
                                result.Error = saveResult.Error;
                                result.Code = "SOUND_ERROR";
                                return result;
                            }

                            LEAD_CONT_IN++;
                            result.Message = "Nhập kho thành công\nInput completed successfully";
                            result.Code = "SOUND_SUCCESS";
                            result.Label37 = LEAD_CONT_IN.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi xử lý: {ex.Message}\nPlease Check Again.";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }
        private async Task<BaseResult> DATE_SAVE_IN(string barcode, string currentUser, long? CompanyID, long? CategoryDepartmentID)
        {
            var result = new BaseResult();
            try
            {
                string bbb = barcode.Substring(barcode.IndexOf("$$") + 2);
                string bb = bbb.Substring(0, bbb.IndexOf("$$"));

                var NOW = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                string sql;
                if (SPST_CHK)
                {
                    sql = $"INSERT INTO trackmtim (trackmtim.RACK_IDX, trackmtim.RACKCODE, trackmtim.TABLE_IDX, trackmtim.TABLE_NM, trackmtim.LEAD_NM, trackmtim.BARCODE_NM, trackmtim.RACKDTM, " +
                           $"trackmtim.QTY, trackmtim.RACKIN_YN, trackmtim.RACKOUT_YN, trackmtim.CREATE_DTM, trackmtim.CREATE_USER, trackmtim.CompanyID, trackmtim.CategoryDepartmentID, trackmtim.UpdateDate, trackmtim.UpdateUserCode) " +
                           $"SELECT (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `RACK_IDX`, 'INPUT', " +
                           $"0, 'USER', MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, `TBCTOTAL`.`TORDER_BARCODENM`, NOW(), '{bb}', 'Y', 'N', NOW(), '{currentUser}', {CompanyID}, {CategoryDepartmentID}, '{NOW}', '{currentUser}' " +
                           $"FROM (SELECT '{barcode}' AS `TORDER_BARCODENM`) AS `TBCTOTAL`";
                }
                else
                {
                    sql = $"INSERT INTO trackmtim (trackmtim.RACK_IDX, trackmtim.RACKCODE, trackmtim.TABLE_IDX, trackmtim.TABLE_NM, trackmtim.LEAD_NM, trackmtim.BARCODE_NM, trackmtim.RACKDTM, " +
                          $"trackmtim.QTY, trackmtim.RACKIN_YN, trackmtim.RACKOUT_YN, trackmtim.CREATE_DTM, trackmtim.CREATE_USER, trackmtim.CompanyID, trackmtim.CategoryDepartmentID, trackmtim.UpdateDate, trackmtim.UpdateUserCode) " +
                          $"SELECT (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `RACK_IDX`, 'INPUT', " +
                          $"`TBCTOTAL`.`TORDER_BARCODE_IDX`, `TBCTOTAL`.`TB_NM`, MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, `TBCTOTAL`.`TORDER_BARCODENM`, NOW(), '{bb}', 'Y', 'N', NOW(), '{currentUser}', {CompanyID}, {CategoryDepartmentID}, '{NOW}', '{currentUser}' " +
                          $"FROM ((SELECT *, 'KOMAX' AS `TB_NM` FROM TORDER_BARCODE WHERE `DSCN_YN`='Y') UNION SELECT *, 'SPST' AS `TB_NM` FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='Y') AS `TBCTOTAL` " +
                          $"WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '{barcode}'";
                }
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                string sql2 = $"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) " +
                             $"(SELECT " +
                             $"`DB_M`.`S_PART_IDX`, '7' AS `LOC`, " +
                             $"ROUND((SUM(tiivtr.QTY) - (SUM(`DB_M`.`QTY`) * `DB_M`.`LD_BN`)), 3) AS `USER_QTY`, " +
                             $"NOW(), '{currentUser}' AS `CREATE_USER` " +
                             $"FROM " +
                             $"(SELECT " +
                             $"`DB_A`.`ST`, `DB_A`.`S_PART_IDX`, `DB_A`.`PART_NO`, " +
                             $"ROUND(IF(`DB_A`.`ST` = 'WIRE', `DB_A`.`MOQ` / 1000, `DB_A`.`MOQ`), 3) AS `QTY`, " +
                             $"(SELECT trackmtim.QTY FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}') AS `LD_BN` " +
                             $"FROM " +
                             $"(SELECT " +
                             $"`M_PIDX`.`ST`, `M_PIDX`.`PART_IDX`, `M_PIDX`.`S_PART_IDX`, " +
                             $"(SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` =`M_PIDX`.`S_PART_IDX`) AS `PART_NO`, " +
                             $"`M_PIDX`.`MOQ`, `M_PIDX`.`X` " +
                             $"FROM( " +
                             $"SELECT " +
                             $"(CASE WHEN `MT_LIST`.`X` = 1 THEN 'WIRE' WHEN `MT_LIST`.`X` = 2 THEN 'T1' " +
                             $"WHEN `MT_LIST`.`X` = 3 THEN 'S1' WHEN `MT_LIST`.`X` = 4 THEN 'T2' WHEN `MT_LIST`.`X` = 5 THEN 'S2' END) AS `ST`, " +
                             $"`LD_LIST`.`LEAD_INDEX` AS `PART_IDX`, " +
                             $"(CASE WHEN `MT_LIST`.`X` = 1 THEN `LD_LIST`.`W_PN_IDX` " +
                             $"WHEN `MT_LIST`.`X` = 2 THEN `LD_LIST`.`T1_PN_IDX` " +
                             $"WHEN `MT_LIST`.`X` = 3 THEN `LD_LIST`.`S1_PN_IDX` " +
                             $"WHEN `MT_LIST`.`X` = 4 THEN `LD_LIST`.`T2_PN_IDX` " +
                             $"WHEN `MT_LIST`.`X` = 5 THEN `LD_LIST`.`S2_PN_IDX` END) AS `S_PART_IDX`, " +
                             $"(CASE WHEN `MT_LIST`.`X` = 1 THEN `LD_LIST`.`W_Length` WHEN `MT_LIST`.`X` = 2 THEN 1 WHEN `MT_LIST`.`X` = 3 THEN 1 " +
                             $"WHEN `MT_LIST`.`X` = 4 THEN 1 WHEN `MT_LIST`.`X` = 5 THEN 1 END) AS `MOQ`, `MT_LIST`.`X` " +
                             $"FROM ( " +
                             $"(SELECT torder_lead_bom.`LEAD_INDEX`, torder_lead_bom.`LEAD_SCN`, torder_lead_bom.`LEAD_PN`, torder_lead_bom.`W_PN_IDX`, torder_lead_bom.`T1_PN_IDX`, " +
                             $"torder_lead_bom.`S1_PN_IDX`, torder_lead_bom.`T2_PN_IDX`, torder_lead_bom.`S2_PN_IDX`, torder_lead_bom.`W_Length` " +
                             $"FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_SCN` ='LEAD') `LD_LIST`, " +
                             $"(SELECT 1 AS `X` UNION ALL SELECT 2 AS `X` UNION ALL SELECT 3 AS `X` UNION ALL SELECT 4 AS `X` UNION ALL SELECT 5 AS `X`) `MT_LIST`) " +
                             $") `M_PIDX` WHERE NOT(`M_PIDX`.`S_PART_IDX` IS NULL)) `DB_A` " +
                             $"JOIN (SELECT torder_lead_bom_spst.M_PART_IDX, IFNULL(torder_lead_bom_spst.S_PART_IDX, torder_lead_bom.LEAD_INDEX) AS `S_PARTIDX` " +
                             $"FROM torder_lead_bom LEFT JOIN torder_lead_bom_spst ON torder_lead_bom.LEAD_INDEX = torder_lead_bom_spst.M_PART_IDX " +
                             $"WHERE torder_lead_bom.LEAD_PN = (SELECT trackmtim.LEAD_NM FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}')) `DB_B` " +
                             $"ON `DB_A`.`PART_IDX` = `DB_B`.`S_PARTIDX`) `DB_M` " +
                             $"JOIN tiivtr ON `DB_M`.`S_PART_IDX` = tiivtr.PART_IDX WHERE tiivtr.LOC_IDX = '7' GROUP BY `DB_M`.`S_PART_IDX`) " +
                             $"ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`), `QTY` = VALUES(`QTY`)";
                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);
                }
                catch (Exception ex)
                {
                    result.Error = $"MES tái xử lý lỗi: {ex.Message}\nInventory processing error MES.";
                    result.Code = "SOUND_ERROR";
                    return result;
                }

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                }

                string sql3 = $"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`) " +
                             $"SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '{currentUser}' " +
                             $"FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}' " +
                             $"ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` =(tiivtr_lead.`QTY` + trackmtim.`QTY`), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '{currentUser}'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql3);

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                }

                SPST_CHK = false;
                result.Code = "SOUND_SUCCESS";

                //ERP INPUT
                List<string> ListBarcode = new List<string>();
                ListBarcode.Add(barcode);
                await ERPSync(CompanyID, CategoryDepartmentID, NOW, ListBarcode, 1);
            }
            catch (Exception ex)
            {
                result.Error = $"Quá trình nhập kho đã hoàn tất, vui lòng kiểm tra lại: {ex.Message}\nThe receipt process has already been completed.";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }
        public virtual async Task<BaseResult> ReInputBarcode(BaseParameter BaseParameter)
        {
            var result = new BaseResult();
            try
            {
                string barcode = BaseParameter.ListSearchString[0].Trim();
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "";

                string sql = $"UPDATE trackmtim SET `RACKCODE`='INPUT', `RACKDTM` = NOW(), `RACKOUT_DTM`= NULL, `RACKOUT_YN`='N', `CREATE_USER`='{currentUser}' " +
                            $"WHERE `BARCODE_NM`= '{barcode}'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = $"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`) " +
                     $"SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '{currentUser}' " +
                     $"FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}' " +
                     $"ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` =(tiivtr_lead.`QTY` + trackmtim.`QTY`), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '{currentUser}'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                }

                LEAD_CONT_IN++;
                result.Message = "Tái nhập kho thành công\nRe-input completed successfully";
                result.Code = "SOUND_SUCCESS";
                result.Label37 = LEAD_CONT_IN.ToString();

                //ERP RE INPUT (Không thay đổi số lượng RE INPUT)

                //long ID = 0;
                //string UserName = currentUser.Trim();
                //string Barcode = barcode.Trim();
                //sql = @"select * from trackmtim where BARCODE_NM='" + Barcode + "' AND RACKCODE='INPUT'";
                //DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                //var Listtrackmtim = new List<trackmtim>();
                //for (int i = 0; i < ds.Tables.Count; i++)
                //{
                //    DataTable dt = ds.Tables[i];
                //    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                //}
                //if (Listtrackmtim.Count > 0)
                //{
                //    trackmtim trackmtim = Listtrackmtim[0];
                //    if (trackmtim != null && trackmtim.TRACK_IDX > 0 && !string.IsNullOrEmpty(trackmtim.BARCODE_NM))
                //    {
                //        ID = (long)trackmtim.TRACK_IDX;
                //        string url = GlobalHelper.APISite + "/api/v1/WarehouseInput/SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync?ID=" + ID + "&UserName=" + UserName;
                //        HttpClient client = new HttpClient();
                //        client.GetStringAsync(url);
                //        url = GlobalHelper.APISite + "/api/v1/WarehouseOutput/SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync?ID=" + ID + "&UserName=" + UserName;
                //        client = new HttpClient();
                //        client.GetStringAsync(url);
                //    }
                //}
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi tái nhập kho: {ex.Message}\nRe-input error";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }
        public virtual async Task<BaseResult> GetBarcodesByTrolley(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string trolleyCode = BaseParameter.ListSearchString[0]?.Trim() ?? "";

                if (string.IsNullOrEmpty(trolleyCode))
                {
                    result.Error = "Vui lòng nhập Trolley Code!";
                    return result;
                }

                string safeTrolleyCode = trolleyCode.Replace("'", "''");

                string sql = @"
            SELECT 
                TB.TORDER_BARCODENM AS BARCODE,
                SUBSTRING_INDEX(TB.TORDER_BARCODENM, '$$', 1) AS LEAD_NO,
                IFNULL(TM.HOOK_RACK, '') AS HOOK_RACK,
                CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(TB.TORDER_BARCODENM, '$$', 2), '$$', -1) AS SIGNED) AS QTY
            FROM TORDER_BARCODE TB
            LEFT JOIN trackmaster TM ON TM.LEAD_NO = SUBSTRING_INDEX(TB.TORDER_BARCODENM, '$$', 1)
            WHERE TB.TrolleyCode = '" + safeTrolleyCode + @"'
            AND TB.TORDER_BC_PRNT = 'Y'
            AND TB.DSCN_YN = 'Y'
            AND NOT EXISTS (SELECT 1 FROM trackmtim WHERE trackmtim.BARCODE_NM = TB.TORDER_BARCODENM)
            ORDER BY LEAD_NO, TB.Barcode_SEQ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    result.Error = "Không có barcode cần nhập kho!";
                    return result;
                }

                result.TrolleyBarcodes = ds.Tables[0].AsEnumerable().Select(row => new TrolleyBarcodeItem
                {
                    BARCODE = row["BARCODE"]?.ToString() ?? "",
                    LEAD_NO = row["LEAD_NO"]?.ToString() ?? "",
                    HOOK_RACK = row["HOOK_RACK"]?.ToString() ?? "",
                    QTY = Convert.ToInt32(row["QTY"] ?? 0)
                }).ToList();

                result.TrolleySummary = result.TrolleyBarcodes
                    .GroupBy(x => new { x.LEAD_NO, x.HOOK_RACK })
                    .Select(g => new TrolleySummaryItem
                    {
                        LEAD_NO = g.Key.LEAD_NO,
                        HOOK_RACK = g.Key.HOOK_RACK,
                        BarcodeCount = g.Count(),
                        TotalQty = g.Sum(x => x.QTY)
                    }).ToList();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> InputTrolleyBarcodes(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                BaseParameter.CompanyID = BaseParameter.CompanyID ?? 16;
                BaseParameter.CategoryDepartmentID = 217;
                if (BaseParameter.CompanyID == 17)
                {
                    BaseParameter.CategoryDepartmentID = 224;
                }
                var barcodes = BaseParameter.ListSearchString;
                string currentUser = BaseParameter.USER_ID ?? "SYSTEM";

                if (barcodes == null || barcodes.Count == 0)
                {
                    result.Error = "Không có barcode để nhập kho!";
                    return result;
                }

                int successCount = 0;
                int failCount = 0;

                var NOW = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (var barcode in barcodes)
                {
                    try
                    {
                        string safeBarcode = barcode.Replace("'", "''");
                        string safeUser = currentUser.Replace("'", "''");

                        // Kiểm tra đã nhập kho chưa
                        string checkSql = $"SELECT COUNT(*) AS CNT FROM trackmtim WHERE BARCODE_NM = '{safeBarcode}'";
                        DataSet dsCheck = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkSql);

                        if (Convert.ToInt32(dsCheck.Tables[0].Rows[0]["CNT"]) > 0)
                        {
                            failCount++;
                            continue;
                        }

                        // Lấy số lượng từ barcode
                        string bbb = barcode.Substring(barcode.IndexOf("$$") + 2);
                        string qty = bbb.Substring(0, bbb.IndexOf("$$"));

                        // Insert vào trackmtim
                        string sql = $@"INSERT INTO trackmtim (RACK_IDX, RACKCODE, TABLE_IDX, TABLE_NM, LEAD_NM, BARCODE_NM, RACKDTM, QTY, RACKIN_YN, RACKOUT_YN, CREATE_DTM, CREATE_USER, CompanyID, CategoryDepartmentID, UpdateDate, UpdateUserCode)
                    SELECT 
                        (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = SUBSTRING_INDEX('{safeBarcode}', '$$', 1)),
                        'INPUT',
                        TB.TORDER_BARCODE_IDX,
                        'KOMAX',
                        SUBSTRING_INDEX('{safeBarcode}', '$$', 1),
                        '{safeBarcode}',
                        NOW(),
                        '{qty}',
                        'Y',
                        'N',
                        NOW(),
                        '{safeUser}',
                        {BaseParameter.CompanyID},
                        {BaseParameter.CategoryDepartmentID},
                        '{NOW}',
                        '{safeUser}'
                    FROM TORDER_BARCODE TB
                    WHERE TB.TORDER_BARCODENM = '{safeBarcode}'
                    LIMIT 1";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        // Cập nhật tồn kho tiivtr_lead
                        string sqlLead = $@"INSERT INTO tiivtr_lead (PART_IDX, LOC_IDX, QTY, CREATE_DTM, CREATE_USER)
                    SELECT 
                        (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = SUBSTRING_INDEX('{safeBarcode}', '$$', 1)),
                        '3',
                        {qty},
                        NOW(),
                        '{safeUser}'
                    ON DUPLICATE KEY UPDATE 
                        QTY = QTY + {qty},
                        UPDATE_DTM = NOW(),
                        UPDATE_USER = '{safeUser}'";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlLead);

                        successCount++;
                    }
                    catch
                    {
                        failCount++;
                    }
                }

                result.Success = successCount > 0;
                result.SuccessCount = successCount;
                result.TotalCount = barcodes.Count;
                result.Message = $"Đã nhập kho {successCount}/{barcodes.Count} barcode";

                //ERP INPUT By Trolley
                List<string> ListBarcode = new List<string>();
                ListBarcode = barcodes;
                await ERPSync(BaseParameter.CompanyID, BaseParameter.CategoryDepartmentID, NOW, ListBarcode, 1);

                if (failCount > 0)
                {
                    result.Error = $"{failCount} barcode bị lỗi hoặc đã nhập trước đó";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click_IN(BaseParameter BaseParameter)
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

        public virtual async Task<BaseResult> DATA_ADD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string textBox4Text = BaseParameter.ListSearchString[0];
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "";

                // Truy vấn kiểm tra barcode trong kho chưa xuất
                string DGV_DATA18 = $"SELECT trackmtim.`TRACK_IDX`, trackmtim.`LEAD_NM`, trackmtim.`BARCODE_NM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{textBox4Text}' AND trackmtim.`RACKOUT_YN`= 'N'";
                DataSet dsDGV_018 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA18);

                if (dsDGV_018.Tables[0].Rows.Count <= 0)
                {
                    // Kiểm tra barcode đã xuất kho
                    string DGV_DATA19 = $"SELECT trackmtim.`TRACK_IDX`, trackmtim.`RACKOUT_DTM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{textBox4Text}' AND trackmtim.`RACKOUT_YN`= 'Y'";
                    DataSet dsDGV_019 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA19);

                    if (dsDGV_019.Tables[0].Rows.Count <= 0)
                    {
                        result.Error = "입고처리 하지 않는 바코드 사용\nKhông có dữ liệu LEAD. Mã vạch không được xử lý(INPUT)";
                        result.Code = "SOUND_ERROR";
                        return result;
                    }
                    else
                    {
                        // Kiểm tra barcode FA
                        string DGV_DATA28 = $"SELECT trackmtim.`TRACK_IDX`, trackmtim.`LEAD_NM`, trackmtim.`BARCODE_NM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{textBox4Text}FA' AND trackmtim.`RACKOUT_YN`= 'N'";
                        DataSet dsDGV_028 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA28);

                        if (dsDGV_028.Tables[0].Rows.Count <= 0)
                        {
                            string rackoutDate = dsDGV_019.Tables[0].Rows[0]["RACKOUT_DTM"]?.ToString() ?? "";
                            result.Error = $"출고 처리가 이미 되었습니다. (OUTPUT completed)\n Date : {rackoutDate}\nĐã xử lý xuất kho trước đó";
                            result.Code = "SOUND_ERROR";
                            return result;
                        }
                        else
                        {
                            // Lưu thông tin lead và barcode vào kết quả
                            result.Data = new
                            {
                                LeadNo = dsDGV_028.Tables[0].Rows[0]["LEAD_NM"]?.ToString() ?? "",
                                Barcode = dsDGV_028.Tables[0].Rows[0]["BARCODE_NM"]?.ToString() ?? ""
                            };

                            // Thực hiện xuất kho FA
                            var saveResult = await DATE_SAVE_FA(textBox4Text, currentUser);
                            if (!string.IsNullOrEmpty(saveResult.Error))
                            {
                                result.Error = saveResult.Error;
                                result.Code = "SOUND_ERROR";
                                return result;
                            }

                            result.Code = "SUCCESS_FA";
                            result.Message = "Xuất kho FA thành công\nFA output completed successfully";
                            return result;
                        }
                    }
                }
                else
                {
                    // Lưu thông tin lead và barcode vào kết quả
                    string leadName = dsDGV_018.Tables[0].Rows[0]["LEAD_NM"]?.ToString() ?? "";
                    string barcodeName = dsDGV_018.Tables[0].Rows[0]["BARCODE_NM"]?.ToString() ?? "";

                    result.Data = new
                    {
                        LeadNo = leadName,
                        Barcode = barcodeName
                    };

                    // Kiểm tra FIFO
                    string DGV_DATA_FIFO = "SELECT `A`.`ZADMIN_FUNCTION_CODE`, `A`.`ZADMIN_FUNCTION_NAME`, `A`.`ZADMIN_FUNCTION_YN`, `A`.`ZADMIN_FUNCTION_REMARK`, `A`.`ZADMIN_FUNCTION_DATE` FROM ZADMIN_FUNCTION `A` WHERE `A`.`ZADMIN_FUNCTION_CODE` = 'C03'";
                    DataSet dsDGV_FIFO = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_FIFO);

                    if (dsDGV_FIFO.Tables[0].Rows[0]["ZADMIN_FUNCTION_YN"]?.ToString() == "Y")
                    {
                        string dateFunction = dsDGV_FIFO.Tables[0].Rows[0]["ZADMIN_FUNCTION_DATE"]?.ToString() ?? "WEEK";

                        string DGV_DATA_FIFO_D1 = $@"SELECT `AA`.`LEAD_NM`, `AA`.`CREATE_DTM`, `BB`.`MIN_DATE`, ABS(TIMESTAMPDIFF({dateFunction}, `AA`.`CREATE_DTM`, `BB`.`MIN_DATE`)) AS `RUS`
FROM
( SELECT `A`.`LEAD_NM`, `A`.`CREATE_DTM` FROM trackmtim `A` WHERE `A`.`BARCODE_NM` = '{barcodeName}' ) `AA` JOIN  
(SELECT `B`.`LEAD_NM` AS `LEAD_NM_B`, MIN(`B`.`CREATE_DTM`) AS `MIN_DATE` FROM trackmtim `B` WHERE `B`.`LEAD_NM` = '{leadName}' AND `B`.`RACKOUT_YN` ='N') `BB`
ON `AA`.`LEAD_NM` = `BB`.`LEAD_NM_B`";

                        DataSet dsDGV_FIFO_D1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_FIFO_D1);

                        if (Convert.ToInt32(dsDGV_FIFO_D1.Tables[0].Rows[0]["RUS"]) <= 3)
                        {
                            var saveResult = await DATE_SAVE(textBox4Text, currentUser, BaseParameter.CompanyID, BaseParameter.CategoryDepartmentID);
                            if (!string.IsNullOrEmpty(saveResult.Error))
                            {
                                result.Error = saveResult.Error;
                                result.Code = "SOUND_ERROR";
                                return result;
                            }
                            result.Code = "SUCCESS";
                            result.Message = "Xuất kho thành công\nOutput completed successfully";
                        }
                        else
                        {
                            string minDate = dsDGV_FIFO_D1.Tables[0].Rows[0]["MIN_DATE"]?.ToString() ?? "";
                            result.Error = $"IN Date : {minDate}\n   선입선출(FIFO) 관리 바랍니다.\nFIFO Một lỗi đã xảy ra.";
                            result.Code = "SOUND_ERROR";
                            return result;
                        }
                    }
                    else
                    {
                        var saveResult = await DATE_SAVE(textBox4Text, currentUser, BaseParameter.CompanyID, BaseParameter.CategoryDepartmentID);
                        if (!string.IsNullOrEmpty(saveResult.Error))
                        {
                            result.Error = saveResult.Error;
                            result.Code = "SOUND_ERROR";
                            return result;
                        }
                        result.Code = "SUCCESS";
                        result.Message = "Xuất kho thành công\nOutput completed successfully";
                    }

                    LEAD_CONT_OUT = LEAD_CONT_OUT + 1;
                    result.Data = new
                    {
                        LeadNo = leadName,
                        Barcode = barcodeName,
                        OutCount = LEAD_CONT_OUT
                    };
                }
            }
            catch (Exception ex)
            {
                result.Error = $"DATA LOAD 오류가 발생 하였습니다.\nDATA LOAD Một lỗi đã xảy ra.\n{ex.Message}";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }


        private async Task<BaseResult> DATE_SAVE(string textBox4Text, string currentUser, long? CompanyID, long? CategoryDepartmentID)
        {
            BaseResult result = new BaseResult();
            try
            {
                var NOW = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                string updateSql = $@"UPDATE trackmtim SET trackmtim.`RACKCODE` ='OUTPUT' , trackmtim.`RACKOUT_DTM` = NOW(), trackmtim.`RACKOUT_YN` = 'Y', trackmtim.`CompanyID` = {CompanyID}, trackmtim.`CategoryDepartmentID` = {CategoryDepartmentID}, trackmtim.`UpdateDate` = '{NOW}', trackmtim.`UpdateUserCode` = '{currentUser}'
WHERE trackmtim.`TRACK_IDX` = (SELECT trackmtim.`TRACK_IDX` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{textBox4Text}')";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                string insertSql1 = $@"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`)
SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '{currentUser}'
FROM trackmtim WHERE trackmtim.BARCODE_NM = '{textBox4Text}'
ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` = (tiivtr_lead.`QTY` - trackmtim.`QTY`), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '{currentUser}'";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql1);

                string insertSql2 = $@"INSERT INTO tiivtr_lead_fg (tiivtr_lead_fg.`PART_IDX`, tiivtr_lead_fg.`LOC_IDX`, tiivtr_lead_fg.`QTY`, tiivtr_lead_fg.`CREATE_DTM`, tiivtr_lead_fg.`CREATE_USER`)
SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '5', trackmtim.`QTY`, NOW(), '{currentUser}'
FROM trackmtim WHERE trackmtim.BARCODE_NM = '{textBox4Text}'
ON DUPLICATE KEY UPDATE tiivtr_lead_fg.`QTY` = (tiivtr_lead_fg.`QTY` + trackmtim.`QTY`), tiivtr_lead_fg.`UPDATE_DTM` = NOW(), tiivtr_lead_fg.`UPDATE_USER` = '{currentUser}'";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql2);

                result.Code = "SOUND_SUCCESS";

                //ERP OUTPUT
                List<string> ListBarcode = new List<string>();
                ListBarcode.Add(textBox4Text);
                await ERPSync(CompanyID, CategoryDepartmentID, NOW, ListBarcode, 2);
            }
            catch (Exception ex)
            {
                result.Error = $"Unable to connect to database.\nKhông thể kết nối MES\n{ex.Message}";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }


        private async Task<BaseResult> DATE_SAVE_FA(string barcode, string currentUser)
        {
            var result = new BaseResult();
            try
            {
                string updateSql = $"UPDATE trackmtim SET trackmtim.`RACKCODE` ='OUTPUT' , trackmtim.`RACKOUT_DTM` = NOW(), trackmtim.`RACKOUT_YN` = 'Y' " +
                                         $"WHERE trackmtim.`TRACK_IDX` = (SELECT trackmtim.`TRACK_IDX` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{barcode}FA')";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                string insertSql1 = $"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`) " +
                                         $"SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '{currentUser}' " +
                                         $"FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}FA' " +
                                         $"ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` = (tiivtr_lead.`QTY` - trackmtim.`QTY`), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '{currentUser}'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql1);

                string insertSql2 = $"INSERT INTO tiivtr_lead_fg (tiivtr_lead_fg.`PART_IDX`, tiivtr_lead_fg.`LOC_IDX`, tiivtr_lead_fg.`QTY`, tiivtr_lead_fg.`CREATE_DTM`, tiivtr_lead_fg.`CREATE_USER`) " +
                                         $"SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '5', trackmtim.`QTY`, NOW(), '{currentUser}' " +
                                         $"FROM trackmtim WHERE trackmtim.BARCODE_NM = '{barcode}' " +
                                         $"ON DUPLICATE KEY UPDATE tiivtr_lead_fg.`QTY` = (tiivtr_lead_fg.`QTY` + trackmtim.`QTY`), tiivtr_lead_fg.`UPDATE_DTM` = NOW(), tiivtr_lead_fg.`UPDATE_USER` = '{currentUser}'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql2);

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                    // Bỏ qua lỗi
                }

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead_fg` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                    // Bỏ qua lỗi
                }

                LEAD_CONT_OUT++;
                result.Code = "SOUND_SUCCESS";
                result.Message = "Xuất kho FA thành công\nFA output completed successfully";
            }
            catch (Exception ex)
            {
                result.Error = $"Không thể kết nối cơ sở dữ liệu: {ex.Message}\nUnable to connect to database";
                result.Code = "SOUND_ERROR";
            }
            return result;
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
        public async Task<BaseResult> IN_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Truy vấn SQL điều chỉnh để khớp với tên trường trong SuperResultTranfer
                string query = @"SELECT trackmtim.`TABLE_NM` AS `TABLE_NM`, 
            (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`,    
            trackmtim.`LEAD_NM` AS `LEAD_NO`, 
            (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead WHERE (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = tiivtr_lead.`PART_IDX`) = trackmtim.`LEAD_NM` AND tiivtr_lead.`LOC_IDX` = '3') AS `STOCK_QTY`, 
            trackmtim.`BARCODE_NM` AS `Barcode`, 
            trackmtim.`RACKDTM` AS `inputdate`, 
            trackmtim.`QTY` AS `QUANTITY`, 
            trackmtim.`CREATE_DTM` AS `CREATE_DTM`, 
            trackmtim.`CREATE_USER` AS `CREATE_USER`, 
            trackmtim.`TRACK_IDX`, 
            trackmtim.`TABLE_IDX`  
            FROM trackmtim  
            WHERE trackmtim.`RACKIN_YN` = 'Y' 
            ORDER BY `RACKDTM` DESC, `TRACK_IDX` DESC 
            LIMIT 50";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> OUT_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Truy vấn SQL cho danh sách các mục đã xuất kho
                string query = @"SELECT trackmtim.`TABLE_NM` AS `TABLE_NM`, 
            (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`,
            trackmtim.`LEAD_NM` AS `LEAD_NO`, 
            (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead WHERE 
                (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE 
                    torder_lead_bom.`LEAD_INDEX` = tiivtr_lead.`PART_IDX` AND tiivtr_lead.`LOC_IDX` = '3') = trackmtim.`LEAD_NM`) AS `STOCK_QTY`, 
            trackmtim.`BARCODE_NM` AS `Barcode`, 
            trackmtim.`RACKDTM` AS `inputdate`, 
            trackmtim.`QTY` AS `QUANTITY`, 
            trackmtim.`CREATE_DTM` AS `CREATE_DTM`, 
            trackmtim.`CREATE_USER` AS `CREATE_USER`, 
            trackmtim.`TRACK_IDX`, 
            trackmtim.`TABLE_IDX`  
            FROM trackmtim  
            WHERE trackmtim.`RACKOUT_YN` = 'Y' 
            ORDER BY `RACKOUT_DTM` DESC, `TRACK_IDX` DESC 
            LIMIT 50";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> STK_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy giá trị tìm kiếm từ tham số (nếu có)
                string searchText = "";
                if (BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count > 1)
                {
                    searchText = BaseParameter.ListSearchString[1]; // Giả sử tham số thứ hai là văn bản tìm kiếm
                }

                // Truy vấn SQL với tên cột khớp với trường trong SuperResultTranfer
                string query = @"WITH BaseTrack AS (
    SELECT
        trackmtim.LEAD_NM,
        trackmtim.RACK_IDX,
        SUM(CASE WHEN trackmtim.RACKIN_YN = 'Y' THEN trackmtim.QTY ELSE 0 END) AS IN_QTY,
        SUM(CASE WHEN trackmtim.RACKOUT_YN = 'Y' THEN trackmtim.QTY ELSE 0 END) AS OUT_QTY
    FROM trackmtim
    GROUP BY trackmtim.LEAD_NM, trackmtim.RACK_IDX
),
DB_AA AS (
    SELECT
        trackmaster.HOOK_RACK,
        BaseTrack.LEAD_NM,
        torder_lead_bom.LEAD_INDEX AS LD_IDX,
        BaseTrack.IN_QTY,
        BaseTrack.OUT_QTY
    FROM BaseTrack
    LEFT JOIN trackmaster ON trackmaster.RACK_IDX = BaseTrack.RACK_IDX
    LEFT JOIN torder_lead_bom ON torder_lead_bom.LEAD_PN = BaseTrack.LEAD_NM
),
STOCKDB AS (
    SELECT
        tiivtr_lead.PART_IDX,
        IFNULL(tiivtr_lead.QTY, 0) AS STOCK_QTY,
        IFNULL(tiivaj_LEAD.ADJ_QTY, 0) AS ADJ_QTY
    FROM tiivtr_lead
    LEFT JOIN tiivaj_LEAD ON tiivtr_lead.PART_IDX = tiivaj_LEAD.PART_IDX AND tiivtr_lead.LOC_IDX = tiivaj_LEAD.ADJ_SCN
    WHERE tiivtr_lead.LOC_IDX = '3'
),
FinalTable AS (
    SELECT
        DB_AA.HOOK_RACK,
        DB_AA.LEAD_NM,
        IFNULL(STOCKDB.STOCK_QTY, 0) AS STOCK_QTY,
        DB_AA.IN_QTY,
        DB_AA.OUT_QTY,
        CASE WHEN ((DB_AA.IN_QTY - DB_AA.OUT_QTY) - (IFNULL(STOCKDB.STOCK_QTY, 0) - IFNULL(STOCKDB.ADJ_QTY, 0))) = 0 THEN 'Good' ELSE 'Bad' END AS STAGE,
        ((DB_AA.IN_QTY - DB_AA.OUT_QTY) - (IFNULL(STOCKDB.STOCK_QTY, 0) - IFNULL(STOCKDB.ADJ_QTY, 0))) AS CHK_QTY
    FROM DB_AA
    LEFT JOIN STOCKDB ON DB_AA.LD_IDX = STOCKDB.PART_IDX
    WHERE LENGTH(DB_AA.LEAD_NM) > 0
)
SELECT *
FROM FinalTable
WHERE LEAD_NM LIKE '%" + searchText + @"%'
ORDER BY STAGE ASC, CHK_QTY ASC, LEAD_NM 
LIMIT 1000;";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView3 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView3 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> DGV_CellSEL(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy tham số LEAD_NM từ BaseParameter
                if (BaseParameter?.ListSearchString == null || BaseParameter.ListSearchString.Count < 1)
                {
                    result.Error = "Không có tham số LEAD_NM";
                    return result;
                }

                string leadNm = BaseParameter.ListSearchString[0];

                // Truy vấn SQL đã điều chỉnh tên cột Status thành STAGE để khớp với SuperResultTranfer
                //    string query = $@"SELECT 
                //(SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`, 
                //trackmtim.`LEAD_NM` AS `LEAD_NO`,
                //trackmtim.`BARCODE_NM` AS `Barcode`, 
                //SUBSTRING_INDEX(trackmtim.`BARCODE_NM`, '$$',-1) AS `SEQ`,
                //trackmtim.`QTY` AS `QUANTITY`, 
                //trackmtim.`RACKCODE` AS `STAGE`,
                //trackmtim.`RACKDTM` AS `inputdate`, 
                //trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM`
                //FROM trackmtim 
                //WHERE trackmtim.`LEAD_NM` = '{leadNm}'
                //ORDER BY trackmtim.`TRACK_IDX` DESC 
                //LIMIT 1500";

                string query = $@"select  IFNULL(b.MC2,b.MC) as MC_NO, b.BOM_ID As ECNNo, b.PO_ID as PO_CODE,a.ORDER_IDX,b.TORDER_FG as FG_PART_NO, a.Barcode_SEQ as SEQ, mainTB.* FROM
(SELECT (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`, 
            trackmtim.`LEAD_NM` AS `LEAD_NO`,
            trackmtim.`BARCODE_NM` AS `Barcode`, 
            
            trackmtim.`QTY` AS `QUANTITY`, 
            trackmtim.`RACKCODE` AS `STAGE`,
            trackmtim.`RACKDTM` AS `inputdate`, 
            trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM`
            FROM trackmtim 
            WHERE trackmtim.`LEAD_NM` = '{leadNm}'
            ORDER BY trackmtim.`TRACK_IDX` DESC 
            LIMIT 1500) as mainTB 
          LEFT JOIN torder_barcode as a on a.TORDER_BARCODENM = mainTB.Barcode
          LEFT JOIN TORDERLIST b on a.ORDER_IDX = b.ORDER_IDX";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView4 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView4 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> INOUT_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy các tham số từ BaseParameter
                if (BaseParameter?.ListSearchString == null || BaseParameter.ListSearchString.Count < 4)
                {
                    result.Error = "Thiếu tham số tìm kiếm";
                    return result;
                }

                string searchText = BaseParameter.ListSearchString[1]; // TextBox3.Text
                string startDate = BaseParameter.ListSearchString[2]; // DateTimePicker1.Value
                string endDate = BaseParameter.ListSearchString[3]; // DateTimePicker2.Value
                string radioOption = BaseParameter.ListSearchString[4]; // RadioButton selection

                // Biến để lưu loại hiển thị
                string dvgDgz = "";
                string query = "";

                // Tạo điều kiện WHERE và truy vấn SQL dựa trên radioOption
                if (radioOption == "RadioButton1") // Tìm theo ngày nhập
                {
                    string val = $"WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' " +
                                 $"AND trackmtim.`RACKDTM` >= '{startDate} 06:00:00' AND trackmtim.`RACKDTM` <= '{endDate} 06:00:00' " +
                                 $"ORDER BY trackmtim.`TRACK_IDX` DESC LIMIT 30000";

                    query = $"SELECT (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`, " +
                            $"trackmtim.`LEAD_NM` AS `LEAD_NO`, " +
                            $"trackmtim.`BARCODE_NM` AS `Barcode`, " +
                            $"SUBSTRING_INDEX(trackmtim.`BARCODE_NM`, '$$',-1) AS `SEQ`, " +
                            $"trackmtim.`QTY` AS `QUANTITY`, " +
                            $"trackmtim.`RACKCODE` AS `STAGE`, " +
                            $"trackmtim.`RACKDTM` AS `inputdate`, " +
                            $"trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM` " +
                            $"FROM trackmtim {val}";

                    dvgDgz = "LIST";
                }
                else if (radioOption == "RadioButton2") // Tìm theo ngày xuất
                {
                    string val = $"WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' " +
                                 $"AND trackmtim.`RACKOUT_DTM` >= '{startDate} 06:00:00' AND trackmtim.`RACKOUT_DTM` <= '{endDate} 06:00:00' " +
                                 $"ORDER BY trackmtim.`TRACK_IDX` DESC LIMIT 30000";

                    query = $"SELECT (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `Location`, " +
                            $"trackmtim.`LEAD_NM` AS `LEAD_NO`, " +
                            $"trackmtim.`BARCODE_NM` AS `Barcode`, " +
                            $"SUBSTRING_INDEX(trackmtim.`BARCODE_NM`, '$$',-1) AS `SEQ`, " +
                            $"trackmtim.`QTY` AS `QUANTITY`, " +
                            $"trackmtim.`RACKCODE` AS `STAGE`, " +
                            $"trackmtim.`RACKDTM` AS `inputdate`, " +
                            $"trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM` " +
                            $"FROM trackmtim {val}";

                    dvgDgz = "LIST";
                }
                else if (radioOption == "RadioButton3") // Tìm theo LEAD
                {
                    query = $@"SELECT 
                `A`.`LEAD_NO`, `A`.`STAGE`, 
                IFNULL(MAX(`A`.`inputdate`), '') AS `inputdate`, 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='IN' THEN (`A`.`QUANTITY`) END), 0) AS `IN_QTY`,
                IFNULL(MAX(`A`.`RACKOUT_DTM`), '') AS `RACKOUT_DTM`, 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='OUT' THEN (`A`.`QUANTITY`) END), 0) AS `OUT_QTY`,
                IFNULL(`A`.`RACKOUT_DTM`, `A`.`inputdate`) AS `SEQ`
                FROM (
                SELECT
                trackmtim.`LEAD_NM` AS `LEAD_NO`, trackmtim.`QTY` AS `QUANTITY`, trackmtim.`RACKCODE` AS `STAGE`, 
                'IN' AS `TYPE`, trackmtim.`RACKDTM` AS `inputdate`, NULL AS `RACKOUT_DTM`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' 
                AND trackmtim.`RACKDTM`>= '{startDate} 06:00:00'  
                AND trackmtim.`RACKDTM` <= '{endDate} 06:00:00'
                 
                UNION
                SELECT 
                trackmtim.`LEAD_NM` AS `LEAD_NO`, trackmtim.`QTY` AS `QUANTITY`, trackmtim.`RACKCODE` AS `STAGE`,  
                'OUT' AS `TYPE`, NULL AS `inputdate`, trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' 
                AND trackmtim.`RACKOUT_DTM` >= '{startDate} 06:00:00' 
                AND trackmtim.`RACKOUT_DTM` <= '{endDate} 06:00:00') `A`
                GROUP BY `A`.`LEAD_NO`
                ORDER BY `SEQ`, `A`.`LEAD_NO`";

                    dvgDgz = "LEAD";
                }
                else if (radioOption == "RadioButton8") // Tìm theo nhập/xuất
                {
                    query = $@"SELECT 
                (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = `A`.`RACK_IDX`) AS `Location`,
                `A`.`LEAD_NO`, `A`.`STAGE`, `A`.`Barcode`, `A`.`QUANTITY`, 
                IFNULL(MAX(`A`.`inputdate`), '') AS `inputdate`, 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='IN' THEN IFNULL(`A`.`QUANTITY`, 0) END), 0) AS `IN_QTY`,
                IFNULL(MAX(`A`.`RACKOUT_DTM`), '') AS `RACKOUT_DTM`, 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='OUT' THEN IFNULL(`A`.`QUANTITY`, 0) END), 0) AS `OUT_QTY`,
                IFNULL(`A`.`RACKOUT_DTM`, `A`.`inputdate`) AS `SEQ`
                FROM (
                SELECT
                trackmtim.`LEAD_NM` AS `LEAD_NO`, trackmtim.`QTY` AS `QUANTITY`, trackmtim.`RACKCODE` AS `STAGE`, 
                trackmtim.`BARCODE_NM` AS `Barcode`, trackmtim.`RACK_IDX`,
                'IN' AS `TYPE`, trackmtim.`RACKDTM` AS `inputdate`, NULL AS `RACKOUT_DTM`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' 
                AND trackmtim.`RACKDTM`>= '{startDate} 06:00:00' AND trackmtim.`RACKDTM` <= '{endDate} 06:00:00'
                 
                UNION
                SELECT 
                trackmtim.`LEAD_NM` AS `LEAD_NO`, trackmtim.`QTY` AS `QUANTITY`, trackmtim.`RACKCODE` AS `STAGE`, 
                trackmtim.`BARCODE_NM` AS `Barcode`, trackmtim.`RACK_IDX`,
                'OUT' AS `TYPE`, NULL AS `inputdate`, trackmtim.`RACKOUT_DTM` AS `RACKOUT_DTM`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%' 
                AND trackmtim.`RACKOUT_DTM` >= '{startDate} 06:00:00' AND trackmtim.`RACKOUT_DTM` <= '{endDate} 06:00:00') `A`
                GROUP BY `A`.`Barcode`
                ORDER BY `SEQ`, `A`.`LEAD_NO`";

                    dvgDgz = "INOUT";
                }

                // Thực thi truy vấn chính
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView5 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView5 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }

                // Lưu loại hiển thị
                result.DATEString = dvgDgz;

                // Truy vấn tổng số lượng nhập/xuất
                try
                {
                    string summaryQuery = $@"SELECT 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='IN' THEN `A`.`QTY` END), 0) AS `IN_QTY`, 
                IFNULL(SUM(CASE WHEN `A`.`TYPE`='OUT' THEN `A`.`QTY` END), 0) AS `OUT_QTY`
                FROM (SELECT
                SUM(trackmtim.`QTY`) AS `QTY`, 
                'IN' AS `TYPE`, trackmtim.`RACKDTM` AS `DATE`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%'  
                AND trackmtim.`RACKDTM`>= '{startDate} 06:00:00' 
                AND trackmtim.`RACKDTM` <= '{endDate} 06:00:00'
                 
                UNION
                SELECT 
                SUM(trackmtim.`QTY`) AS `QTY`,
                'OUT' AS `TYPE`, trackmtim.`RACKOUT_DTM` AS `DATE`
                FROM trackmtim  
                WHERE LENGTH(trackmtim.`LEAD_NM`) > 0 AND trackmtim.`LEAD_NM` LIKE '%{searchText}%'  
                AND trackmtim.`RACKOUT_DTM`>= '{startDate} 06:00:00' 
                AND trackmtim.`RACKOUT_DTM` <= '{endDate} 06:00:00') `A`";

                    DataSet summaryDs = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, summaryQuery);

                    if (summaryDs != null && summaryDs.Tables.Count > 0 && summaryDs.Tables[0].Rows.Count > 0)
                    {
                        result.SUM_QTY = summaryDs.Tables[0].Rows[0]["IN_QTY"].ToString();
                        result.OUT_QTY = summaryDs.Tables[0].Rows[0]["OUT_QTY"].ToString();
                    }
                    else
                    {
                        result.SUM_QTY = "0";
                        result.OUT_QTY = "0";
                    }
                }
                catch (Exception)
                {
                    result.SUM_QTY = "0";
                    result.OUT_QTY = "0";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> LONG_TERM(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Truy vấn SQL để lấy dữ liệu hàng tồn kho dài hạn
                string query = @"SELECT 
            IFNULL(`TB`.`YEAR`, 9999) AS `YEAR`,
            IFNULL(`TB`.`MONTH`, 'SUM') AS `MONTH`,
            `TB`.`LEAD_COUNT`,
            `TB`.`ADD_1`,
            `TB`.`ADD_2`,
            `TB`.`ADD_3`,
            `TB`.`ADD_4`,
            `TB`.`ADD_5`,
            `TB`.`ADD_6`,
            `TB`.`OVER_9`,
            `TB`.`OVER_10`,
            `TB`.`SUM`,
            IFNULL(`TB`.`MONTH`, 13) AS `CNO` 
            FROM (
            SELECT 
            `MAIN`.`YEAR`,
            `MAIN`.`MONTH`,
            COUNT(`MAIN`.LEAD_NM) AS `LEAD_COUNT`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 1 THEN MAIN.`SUM` END), 0) AS `ADD_1`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 2 THEN MAIN.`SUM` END), 0) AS `ADD_2`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 3 THEN MAIN.`SUM` END), 0) AS `ADD_3`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 4 THEN MAIN.`SUM` END), 0) AS `ADD_4`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 5 THEN MAIN.`SUM` END), 0) AS `ADD_5`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 6 THEN MAIN.`SUM` END), 0) AS `ADD_6`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` >= 7 AND `MAIN`.`MONTHA` <= 9 THEN MAIN.`SUM` END), 0) AS `OVER_9`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` >= 10 THEN MAIN.`SUM` END), 0) AS `OVER_10`,
            IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` THEN MAIN.`SUM` END), 0) AS `SUM`
            FROM(
            SELECT 
            `A`.`LEAD_NM`, SUM(`A`.`QTY`) AS `SUM`, `A`.`YEAR`, `A`.`MONTH`, `A`.`MONTHA` 
            FROM(
            SELECT 
            `TB_A`.`TRACK_IDX`,
            `TB_A`.`LEAD_NM`,
            `TB_A`.`QTY`,
            YEAR(IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`)) AS `YEAR`,
            MONTH(IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`)) AS `MONTH`,
            TIMESTAMPDIFF(MONTH , IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`), NOW()) AS `MONTHA`
            FROM 
            trackmtim `TB_A` LEFT JOIN
            (SELECT 
            `TB_IN`.`LT_INSP_IDX`, `TB_IN`.`TRACKMTIN_IDX`, `TB_IN`.`INSP_DATE`, `TB_IN`.`INSP_RESULT` FROM (
            SELECT  ROW_NUMBER() OVER (PARTITION BY `TRACKMTIN_IDX` ORDER BY `INSP_DATE` DESC) AS `RUM`,
            `LT_INSP_IDX`, `TRACKMTIN_IDX`,  `INSP_DATE` AS `INSP_DATE`, 
            `INSP_RESULT` FROM TRACKMTIM_LT_INSP) `TB_IN`
            WHERE `TB_IN`.`RUM` = '1')
             `TB_B`
            ON `TB_A`.`TRACK_IDX` = `TB_B`.`TRACKMTIN_IDX`
            WHERE `TB_A`.`RACKOUT_YN` = 'N' AND TIMESTAMPDIFF(MONTH , IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`), NOW()) > 0) `A`
            GROUP BY `A`.`LEAD_NM`, `A`.`YEAR`, `A`.`MONTH` ) AS `MAIN`
            GROUP BY `MAIN`.`YEAR`, `MAIN`.`MONTH` WITH ROLLUP) `TB`
            ORDER BY `YEAR` DESC, `CNO` DESC";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView8 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView8 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> GET_LONG_TERM_DETAIL(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy các tham số từ BaseParameter
                if (BaseParameter?.ListSearchString == null || BaseParameter.ListSearchString.Count < 3)
                {
                    result.Error = "Thiếu tham số tìm kiếm";
                    return result;
                }

                string filterMonth = BaseParameter.ListSearchString[0]; // ComboBox1
                string leadNo = BaseParameter.ListSearchString[1]; // TextBox1
                string barcode = BaseParameter.ListSearchString[2]; // TextBox2

                // Xây dựng điều kiện WHERE cho MONTHA
                string monthCondition = "";
                if (filterMonth != "ALL")
                {
                    // Chuyển "OVER +X" thành số tháng
                    string numericPart = filterMonth.Replace("OVER +", "").Trim();
                    if (int.TryParse(numericPart, out int monthValue))
                    {
                        monthCondition = $"AND TIMESTAMPDIFF(MONTH, IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`), NOW()) = {monthValue}";
                    }
                }

                // Điều kiện tìm kiếm LeadNo và Barcode
                string leadCondition = string.IsNullOrEmpty(leadNo) ? "" : $"AND `TB_A`.`LEAD_NM` LIKE '%{leadNo}%'";
                string barcodeCondition = string.IsNullOrEmpty(barcode) ? "" : $"AND `TB_A`.`BARCODE_NM` LIKE '%{barcode}%'";

                // Truy vấn SQL để lấy chi tiết
                string query = $@"SELECT 
            `TB_A`.`LEAD_NM` AS `LEAD_NO`,
            `TB_A`.`TABLE_NM`,
            (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = `TB_A`.`RACK_IDX`) AS `COLN_LOC`,
            `TB_A`.`BARCODE_NM` AS `BARCODE_NM`,
            `TB_A`.`QTY`,
            IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`) AS `Coln1`,
            YEAR(IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`)) AS `YEAR`,
            MONTH(IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`)) AS `MONTH`,
            TIMESTAMPDIFF(MONTH, IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`), NOW()) AS `Coln7`
            FROM 
            trackmtim `TB_A` LEFT JOIN
            (SELECT 
            `TB_IN`.`LT_INSP_IDX`, `TB_IN`.`TRACKMTIN_IDX`, `TB_IN`.`INSP_DATE`, `TB_IN`.`INSP_RESULT` FROM (
            SELECT ROW_NUMBER() OVER (PARTITION BY `TRACKMTIN_IDX` ORDER BY `INSP_DATE` DESC) AS `RUM`,
            `LT_INSP_IDX`, `TRACKMTIN_IDX`, `INSP_DATE` AS `INSP_DATE`, 
            `INSP_RESULT` FROM TRACKMTIM_LT_INSP) `TB_IN`
            WHERE `TB_IN`.`RUM` = '1')
            `TB_B`
            ON `TB_A`.`TRACK_IDX` = `TB_B`.`TRACKMTIN_IDX`
            WHERE `TB_A`.`RACKOUT_YN` = 'N' 
            AND TIMESTAMPDIFF(MONTH, IFNULL(`TB_B`.`INSP_DATE`, `TB_A`.`CREATE_DTM`), NOW()) > 0
            {leadCondition}
            {barcodeCondition}
            {monthCondition}
            ORDER BY `Coln7` DESC, `TB_A`.`LEAD_NM`
            ";

                // Thực thi truy vấn

                result.DataGridView9 = await MySQLHelperV2.QueryToListAsync<SuperResultTranfer>(GlobalHelper.MariaDBConectionString, query);

                //DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                //// Chuyển DataSet thành danh sách đối tượng
                //result.DataGridView9 = new List<SuperResultTranfer>();
                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    DataTable dt = ds.Tables[0];
                //    result.DataGridView9 = SQLHelper.ToList<SuperResultTranfer>(dt);
                //}
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        // Thêm vào C03Service.cs
        public async Task<BaseResult> MAG_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string searchText = "";

                // Kiểm tra xem có tham số tìm kiếm không
                if (BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count > 1)
                {
                    searchText = BaseParameter.ListSearchString[1] ?? ""; // Tham số thứ hai là văn bản tìm kiếm
                }

                // Truy vấn SQL để lấy danh sách Lead PN
                // Sử dụng điều kiện WHERE 1=1 để luôn đúng khi searchText trống
                string query = $"SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom " +
                               $"WHERE 1=1 ";

                // Chỉ thêm điều kiện LIKE khi có searchText
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query += $"AND torder_lead_bom.`LEAD_PN` LIKE '%{searchText}%' ";
                }

                // Thêm ORDER BY và LIMIT để không trả về quá nhiều kết quả
                query += $"ORDER BY torder_lead_bom.`LEAD_PN` LIMIT 100";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Chuyển DataSet thành danh sách đối tượng
                result.DataGridView6 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView6 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }


            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết hơn

                result.Error = $"Lỗi kết nối hoặc truy vấn: {ex.Message}";
            }
            return result;
        }
        public async Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy giá trị LEAD_NO từ tham số
                if (BaseParameter?.ListSearchString == null || BaseParameter.ListSearchString.Count < 1)
                {
                    result.Error = "Không có tham số LEAD_NO";
                    return result;
                }

                string leadNo = BaseParameter.ListSearchString[0];

                // Truy vấn SQL để lấy chi tiết Lead
                string query = $@"SELECT `LEAD_PN`, `DSCN_YN`, `LEAD_INDEX`,
            IFNULL((SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`LEAD_NO`=`LEAD_PN`),'-') AS `HOOK_RACK`,
            IFNULL((SELECT trackmaster.`SFTY_STK` FROM trackmaster WHERE trackmaster.`LEAD_NO`=LEAD_PN),0) AS `Safety_Stock`,
            IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `W_PN_IDX`), '') AS `WIRE`,
            IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T1_PN_IDX`),'') AS `TERM1`,
            IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S1_PN_IDX`),'') AS `SEAL1`,
            IFNULL(`STRIP1`, 0) AS `STRIP1`, IFNULL(`CCH_W1`,'') AS `CCH_W1`,  IFNULL(`ICH_W1`, '') AS `ICH_W1`,
            IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T2_PN_IDX`),'') AS `TERM2`,
            IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S2_PN_IDX`),'') AS `SEAL2`,
            IFNULL(`STRIP2`, 0) AS `STRIP2`, IFNULL(`CCH_W2`,'') AS `CCH_W2`,  IFNULL(`ICH_W2`, '') AS `ICH_W2`,
            `LEAD_SCN`, IFNULL(`W_LINK`, '') AS `W_LINK`, IFNULL(`WR_NO`, '') AS `WR_NO`, IFNULL(`WIRE_NM`, '') AS `WIRE_NM`, 
            IFNULL(`W_Diameter`, '') AS `W_Diameter`, IFNULL(`W_Color`, '') AS `W_Color`, 
            IFNULL(`W_Length`, 0) AS `W_Length`, IFNULL(`T1NO`, '') AS `T1NO`, IFNULL(`T2NO`, '') AS `T2NO`, IFNULL(`BUNDLE_SIZE`, 0) AS `BUNDLE_QTY`
            FROM torder_lead_bom
            WHERE torder_lead_bom.`LEAD_PN` = '{leadNo}'";

                // Thực thi truy vấn
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Kiểm tra kết quả và lưu vào đối tượng kết quả
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    // Ánh xạ dữ liệu từ DataRow vào các thuộc tính của đối tượng kết quả
                    result.LeadDetail = new Dictionary<string, string>
                    {
                        ["LEAD_PN"] = row["LEAD_PN"].ToString(),
                        ["DSCN_YN"] = row["DSCN_YN"].ToString(),
                        ["LEAD_INDEX"] = row["LEAD_INDEX"].ToString(),
                        ["HOOK_RACK"] = row["HOOK_RACK"].ToString(),
                        ["Safety_Stock"] = row["Safety_Stock"].ToString(),
                        ["WIRE"] = row["WIRE"].ToString(),
                        ["TERM1"] = row["TERM1"].ToString(),
                        ["SEAL1"] = row["SEAL1"].ToString(),
                        ["STRIP1"] = row["STRIP1"].ToString(),
                        ["CCH_W1"] = row["CCH_W1"].ToString(),
                        ["ICH_W1"] = row["ICH_W1"].ToString(),
                        ["TERM2"] = row["TERM2"].ToString(),
                        ["SEAL2"] = row["SEAL2"].ToString(),
                        ["STRIP2"] = row["STRIP2"].ToString(),
                        ["CCH_W2"] = row["CCH_W2"].ToString(),
                        ["ICH_W2"] = row["ICH_W2"].ToString(),
                        ["LEAD_SCN"] = row["LEAD_SCN"].ToString(),
                        ["W_LINK"] = row["W_LINK"].ToString(),
                        ["WR_NO"] = row["WR_NO"].ToString(),
                        ["WIRE_NM"] = row["WIRE_NM"].ToString(),
                        ["W_Diameter"] = row["W_Diameter"].ToString(),
                        ["W_Color"] = row["W_Color"].ToString(),
                        ["W_Length"] = row["W_Length"].ToString(),
                        ["T1NO"] = row["T1NO"].ToString(),
                        ["T2NO"] = row["T2NO"].ToString(),
                        ["BUNDLE_QTY"] = row["BUNDLE_QTY"].ToString()
                    };
                }


                string subPartQuery = $@"SELECT 
    '1' AS `NO`,
    (SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = torder_lead_bom_spst.S_PART_IDX) AS `LEAD_NO`, 
    `S_LR`
    FROM torder_lead_bom_spst
    WHERE torder_lead_bom_spst.M_PART_IDX = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '{leadNo}')";

                DataSet subPartDs = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, subPartQuery);



                result.DataGridView7 = new List<SuperResultTranfer>();
                if (subPartDs != null && subPartDs.Tables.Count > 0)
                {
                    DataTable dt = subPartDs.Tables[0];
                    result.DataGridView7 = SQLHelper.ToList<SuperResultTranfer>(dt);



                }
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi khi tải dữ liệu Lead: {ex.Message}";
            }
            return result;
        }
        public async Task<BaseResult> CheckPartExists(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Lấy Part ID từ tham số
                if (BaseParameter?.ListSearchString == null || BaseParameter.ListSearchString.Count < 1)
                {
                    result.Error = "Không có tham số Part ID";
                    result.Code = "ERROR";
                    return result;
                }

                string partId = BaseParameter.ListSearchString[0];

                // Truy vấn SQL để kiểm tra Part ID tồn tại
                string query = $"SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '{partId}'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                // Kiểm tra kết quả
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result.Code = "SUCCESS";
                }
                else
                {
                    result.Error = "MES에 자재 품번이 없습니다.\nKhông có dữ liệu. MES Material PART NO.";
                    result.Code = "ERROR";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi kiểm tra Part: {ex.Message}";
                result.Code = "ERROR";
            }
            return result;
        }
        public virtual async Task<BaseResult> DATA_ADD_RE_IN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox1 = BaseParameter.ListSearchString[0].Trim();
                int NumericUpDown1 = int.Parse(BaseParameter.ListSearchString[1]);
                string C_USER = BaseParameter.ListSearchString[2];

                string SUCHK_LEAD = TextBox1.Substring(0, TextBox1.IndexOf("$$"));

                string DGV_DATA14 = $"SELECT trackmtim.`RACKDTM` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{TextBox1}' AND `RACKOUT_YN` = 'Y'";
                DataSet dsDGV_014 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA14);

                if (dsDGV_014.Tables[0].Rows.Count > 0)
                {
                    string NEW_BC = TextBox1 + "FA";
                    string checkExistingFA = $"SELECT * FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '{NEW_BC}'";
                    DataSet dsExistingFA = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkExistingFA);

                    if (dsExistingFA.Tables[0].Rows.Count > 0)
                    {
                        result.Error = "Barcode đã được tái nhập trước đó\nBarcode has already been re-entered";
                        result.Code = "SOUND_ERROR";
                        return result;
                    }
                    var NOW = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string da9_CommandText = $@"INSERT INTO `trackmtim` (`RACK_IDX`, `RACKCODE`, `TABLE_IDX`, `TABLE_NM`, `LEAD_NM`, `BARCODE_NM`, `RACKDTM`, `QTY`, `RACKIN_YN`, `RACKOUT_YN`, `CREATE_DTM`, `CREATE_USER`, `CompanyID`, `CategoryDepartmentID`, `UpdateDate`, `UpdateUserCode`) 
            VALUES ((SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = '{SUCHK_LEAD}'), 
            'RE_INPUT', '0', 'USER', '{SUCHK_LEAD}', '{NEW_BC}', NOW(), '{NumericUpDown1}', 'Y', 'N', NOW(), '{C_USER}', {BaseParameter.CompanyID}, {BaseParameter.CategoryDepartmentID}, '{NOW}', '{C_USER}')";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, da9_CommandText);

                    da9_CommandText = $@"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`)
            SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '{SUCHK_LEAD}') AS `LEAD_IDX`, '3', {NumericUpDown1}, NOW(), '{C_USER}'
            ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` = (tiivtr_lead.`QTY` + {NumericUpDown1}), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '{C_USER}'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, da9_CommandText);

                    da9_CommandText = $@"INSERT INTO tiivtr_lead_fg (tiivtr_lead_fg.`PART_IDX`, tiivtr_lead_fg.`LOC_IDX`, tiivtr_lead_fg.`QTY`, tiivtr_lead_fg.`CREATE_DTM`, tiivtr_lead_fg.`CREATE_USER`)
            SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '{SUCHK_LEAD}') AS `LEAD_IDX`, '5', {NumericUpDown1}, NOW(), '{C_USER}'
            ON DUPLICATE KEY UPDATE tiivtr_lead_fg.`QTY` = (tiivtr_lead_fg.`QTY` - {NumericUpDown1}), tiivtr_lead_fg.`UPDATE_DTM` = NOW(), tiivtr_lead_fg.`UPDATE_USER` = '{C_USER}'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, da9_CommandText);

                    try
                    {
                        da9_CommandText = "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, da9_CommandText);
                    }
                    catch (Exception) { }

                    try
                    {
                        da9_CommandText = "ALTER TABLE `tiivtr_lead_fg` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, da9_CommandText);
                    }
                    catch (Exception) { }

                    string DGV_DATA_HOOK = $"SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = '{SUCHK_LEAD}'";
                    DataSet dsDGV_HOOK = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_HOOK);
                    string HOOK_RACK = dsDGV_HOOK.Tables[0].Rows.Count > 0 ? dsDGV_HOOK.Tables[0].Rows[0]["HOOK_RACK"]?.ToString() ?? "" : "";

                    LEAD_CONT_IN++;
                    result.Code = "SOUND_SUCCESS";
                    result.Message = "Tái nhập kho thành công\nRe-input completed successfully";
                    result.Label4 = SUCHK_LEAD;
                    result.Label5 = HOOK_RACK;
                    result.Label37 = LEAD_CONT_IN.ToString();

                    //ERP RE INPUT (Thay đổi số lượng)
                    List<string> ListBarcode = new List<string>();
                    ListBarcode.Add(NEW_BC);
                    await ERPSync(BaseParameter.CompanyID, BaseParameter.CategoryDepartmentID, NOW, ListBarcode, 1);

                }
                else
                {
                    result.Error = "Barcode chưa được xuất kho trước đó\nBarcode has not been output before";
                    result.Code = "SOUND_ERROR";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Lỗi xử lý: {ex.Message}";
                result.Code = "SOUND_ERROR";
            }
            return result;
        }

        public async Task<BaseResult> CategoryDepartmentGetByCompanyID_ActiveToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.CompanyID > 0)
                {
                    BaseParameter.Active = true;
                    switch (BaseParameter.CompanyID)
                    {
                        case 16:
                            result.ListCategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == BaseParameter.Active && (o.ParentID == 86 || o.ParentID == 26)).OrderBy(o => o.SortOrder).ToListAsync();
                            break;
                        case 17:
                            result.ListCategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == BaseParameter.Active && (o.ParentID == 195 || o.ParentID == 196)).OrderBy(o => o.SortOrder).ToListAsync();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        private async Task<int> ERPSync(long? CompanyID, long? CategoryDepartmentID, string NOW, List<string> ListBarcode, int? Action)
        {
            int result = 0;
            try
            {
                //foreach (var Barcode in ListBarcode)
                //{
                //    List<trackmtim> Listtrackmtim = new List<trackmtim>();
                //    string sql = $"SELECT * FROM trackmtim WHERE BARCODE_NM = '{Barcode}' AND CompanyID = {CompanyID} AND CategoryDepartmentID = {CategoryDepartmentID} AND UpdateDate = '{NOW}'";
                //    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                //    for (int i = 0; i < ds.Tables.Count; i++)
                //    {
                //        DataTable dt = ds.Tables[i];
                //        Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                //    }
                //    if (Listtrackmtim.Count > 0)
                //    {
                //        var trackmtim = Listtrackmtim[0];
                //        if (trackmtim != null && trackmtim.TRACK_IDX > 0)
                //        {
                //            long ID = (long)trackmtim.TRACK_IDX;
                //            ERPAPICall2026(CompanyID, CategoryDepartmentID, ID, Action);
                //        }
                //    }
                //}
                if (CompanyID > 0 && CategoryDepartmentID > 0)
                {
                    string SearchString = GlobalHelper.InitializationString;
                    string Barcodes = string.Join(",", ListBarcode.Select(x => $"'{x}'"));
                    List<trackmtim> Listtrackmtim = new List<trackmtim>();
                    string sql = $"SELECT * FROM trackmtim WHERE BARCODE_NM in ({Barcodes}) AND CompanyID = {CompanyID} AND CategoryDepartmentID = {CategoryDepartmentID} AND UpdateDate = '{NOW}'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                    }
                    if (Listtrackmtim.Count > 0)
                    {
                        SearchString = string.Join(",", Listtrackmtim.Select(x => x.TRACK_IDX));
                        ERPAPICall2026(CompanyID, CategoryDepartmentID, SearchString, Action);
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        private async Task ERPAPICall(long? CompanyID, long? CategoryDepartmentID, long? ID, int? Action)
        {
            try
            {
                string url = GlobalHelper.APISite + "/WarehouseOutput/SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync?CompanyID=" + CompanyID + "&CategoryDepartmentID=" + CategoryDepartmentID + "&ID=" + ID + "&Action=" + Action;
                HttpClient client = new HttpClient();
                client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
        }
        private async Task ERPAPICall2026(long? CompanyID, long? CategoryDepartmentID, string? SearchString, int? Action)
        {
            try
            {
                string url = GlobalHelper.APISite + "/WarehouseOutput/SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync?CompanyID=" + CompanyID + "&CategoryDepartmentID=" + CategoryDepartmentID + "&Action=" + Action + "&SearchString=" + SearchString;
                HttpClient client = new HttpClient();
                client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
        }
    }
}