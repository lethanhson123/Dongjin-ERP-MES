

namespace MESService.Implement
{
    public class C19_1Service : BaseService<torderlist, ItorderlistRepository>, IC19_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public C19_1Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
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

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
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
                // Lấy loại import từ parameter
                bool isMasterImport = BaseParameter.RadioButton1 ?? true;
                string userName = BaseParameter.USER_IDX;
                string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (isMasterImport) // MASTER import
                {
                    if (BaseParameter.DataGridView1 != null && BaseParameter.DataGridView1.Count > 0)
                    {
                        // Tạo chuỗi SQL values cho nhiều hàng
                        string SQL_T = "";

                        foreach (var item in BaseParameter.DataGridView1)
                        {
                            string AAA = Replace(item.LEAD_SCN ?? "", "'", "");      // LEAD_SCN
                            string BBB = Replace(item.S_PART ?? "", "'", "");        // S_PART -> LEAD_PN
                            string CCC = item.BUNDLE_SIZE?.ToString() ?? "0";        // BUNDLE_SIZE
                            string DDD = Replace(item.WIRE ?? "", "'", "");          // W_Partno -> WIRE
                            string EEE = Replace(item.TERM1 ?? "", "'", "");         // Term1
                            string FFF = Replace(item.SEAL1 ?? "", "'", "");         // SS1 -> SEAL1
                            string GGG = Replace(item.TERM2 ?? "", "'", "");         // Term2
                            string HHH = Replace(item.SEAL2 ?? "", "'", "");         // SS2 -> SEAL2
                            string III = Replace(item.STRIP1 ?? "0", "'", "");       // Strip1
                            string JJJ = Replace(item.STRIP2 ?? "0", "'", "");       // Strip2
                            string KKK = Replace(item.CCH_W1 ?? "", "'", "");        // CCH_W1
                            string LLL = Replace(item.ICH_W1 ?? "", "'", "");        // ICH_W1
                            string MMM = Replace(item.CCH_W2 ?? "", "'", "");        // CCH_W2
                            string NNN = Replace(item.ICH_W2 ?? "", "'", "");        // ICH_W2
                            string OOO = Replace(item.T1NO ?? "", "'", "");          // T1_No
                            string PPP = Replace(item.T2NO ?? "", "'", "");          // T2_No
                            string QQQ = Replace(item.W_LINK ?? "", "'", "");        // WLink
                            string RRR = Replace(item.WR_NO ?? "", "'", "");         // WRNo
                            string SSS = Replace(item.WIRE_NM ?? "", "'", "");       // Wire -> WIRE_NM
                            string TTT = Replace(item.W_Diameter ?? "", "'", "");    // Diameter
                            string UUU = Replace(item.W_Color ?? "", "'", "");       // Color
                            string VVV = item.W_Length?.ToString() ?? "0";           // Length
                            string WWW = Replace(item.HOOK_RACK ?? "", "'", "");     // HOOK_RACK
                            string XXX = Replace(item.SFTY_STK ?? "0", "'", "");     // SAFT_STOCK -> SFTY_STK

                            string SQL_D = $"('{AAA}', '{BBB}', '{CCC}', '{DDD}', '{EEE}', '{FFF}', '{GGG}', '{HHH}', '{III}', '{JJJ}', '{KKK}', '{LLL}', '{MMM}', '{NNN}', '{OOO}', '{PPP}', '{QQQ}', '{RRR}', '{SSS}', '{TTT}', '{UUU}', '{VVV}', 'Y', '{currentDateTime}', '{userName}', '{WWW}', '{XXX}')";

                            if (string.IsNullOrEmpty(SQL_T))
                            {
                                SQL_T = SQL_D;
                            }
                            else
                            {
                                SQL_T = SQL_T + ", " + SQL_D;
                            }
                        }

                        if (!string.IsNullOrEmpty(SQL_T))
                        {
                            string sqlInsertTemp = $@"INSERT INTO `torder_lead_bom_EXCL` 
                        (`LEAD_SCN`, `LEAD_PN`, `BUNDLE_SIZE`, `W_PN_IDX`, `T1_PN_IDX`, `S1_PN_IDX`, `T2_PN_IDX`, `S2_PN_IDX`, 
                        `STRIP1`, `STRIP2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `T1NO`, `T2NO`, 
                        `W_LINK`, `WR_NO`, `WIRE_NM`, `W_Diameter`, `W_Color`, `W_Length`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `HOOK_RACK`, `SFTY_STK`) 
                        VALUES {SQL_T}";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsertTemp);

                            string sqlInsertMain = $@"
                        INSERT INTO `torder_lead_bom` 
                        (`LEAD_SCN`, `LEAD_PN`, `BUNDLE_SIZE`, `W_PN_IDX`, `T1_PN_IDX`, `S1_PN_IDX`, `T2_PN_IDX`, `S2_PN_IDX`, 
                        `STRIP1`, `STRIP2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `T1NO`, `T2NO`, 
                        `W_LINK`, `WR_NO`, `WIRE_NM`, `W_Diameter`, `W_Color`, `W_Length`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) 
                        SELECT 
                            `LEAD_SCN`, `LEAD_PN`, CAST(`BUNDLE_SIZE` AS UNSIGNED) AS `BUNDLE_SIZE`, 
                            IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `W_PN_IDX`), NULL) AS `W_PN_IDX`,
                            IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `T1_PN_IDX`), NULL) AS `T1_PN_IDX`,
                            IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `S1_PN_IDX`), NULL) AS `S1_PN_IDX`,
                            IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `T2_PN_IDX`), NULL) AS `T2_PN_IDX`,
                            IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `S2_PN_IDX`), NULL) AS `S2_PN_IDX`,
                            `STRIP1`, `STRIP2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `T1NO`, `T2NO`, 
                            `W_LINK`, `WR_NO`, `WIRE_NM`, `W_Diameter`, `W_Color`, 
                            CAST(`W_Length` AS UNSIGNED) AS `W_Length`, 
                            `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`
                        FROM torder_lead_bom_EXCL `EXCL`
                        WHERE `EXCL`.`CREATE_DTM` = '{currentDateTime}'
                        ON DUPLICATE KEY UPDATE  
                            `LEAD_SCN` = `EXCL`.`LEAD_SCN`, 
                            `BUNDLE_SIZE` = CAST(`EXCL`.`BUNDLE_SIZE` AS UNSIGNED), 
                            `W_PN_IDX` = IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `EXCL`.`W_PN_IDX`), NULL),
                            `T1_PN_IDX` = IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `EXCL`.`T1_PN_IDX`), NULL),
                            `S1_PN_IDX` = IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `EXCL`.`S1_PN_IDX`), NULL),  
                            `T2_PN_IDX` = IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `EXCL`.`T2_PN_IDX`), NULL),
                            `S2_PN_IDX` = IFNULL((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `EXCL`.`S2_PN_IDX`), NULL),
                            `STRIP1` = `EXCL`.`STRIP1`, 
                            `STRIP2` = `EXCL`.`STRIP2`, 
                            `CCH_W1` = `EXCL`.`CCH_W1`,
                            `ICH_W1` = `EXCL`.`ICH_W1`, 
                            `CCH_W2` = `EXCL`.`CCH_W2`, 
                            `ICH_W2` = `EXCL`.`ICH_W2`, 
                            `T1NO` = `EXCL`.`T1NO`,
                            `T2NO` = `EXCL`.`T2NO`,
                            `W_LINK` = `EXCL`.`W_LINK`,
                            `WR_NO` = `EXCL`.`WR_NO`, 
                            `WIRE_NM` = `EXCL`.`WIRE_NM`, 
                            `W_Diameter` = `EXCL`.`W_Diameter`, 
                            `W_Color` = `EXCL`.`W_Color`, 
                            `W_Length` = CAST(`EXCL`.`W_Length` AS UNSIGNED), 
                            `DSCN_YN` = `EXCL`.`DSCN_YN`, 
                            `UPDATE_DTM` = `EXCL`.`CREATE_DTM`, 
                            `UPDATE_USER` = `EXCL`.`CREATE_USER`";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsertMain);

                            string sqlInsertTrackmaster = $@"
                        INSERT INTO `trackmaster` (`LEAD_NO`, `HOOK_RACK`, `SFTY_STK`, `CREATE_DTM`, `CREATE_USER`) 
                        SELECT `LEAD_PN`, `HOOK_RACK`, `SFTY_STK`, `CREATE_DTM`, `CREATE_USER`
                        FROM torder_lead_bom_EXCL `EXCL`     
                        WHERE `EXCL`.`CREATE_DTM` = '{currentDateTime}'
                        ON DUPLICATE KEY UPDATE   
                            `HOOK_RACK` = `EXCL`.`HOOK_RACK`, 
                            `SFTY_STK` = `EXCL`.`SFTY_STK`, 
                            `UPDATE_DTM` = `EXCL`.`CREATE_DTM`, 
                            `UPDATE_USER` = `EXCL`.`CREATE_USER`";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsertTrackmaster);

                            try
                            {
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `torder_lead_bom` AUTO_INCREMENT= 1");
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `trackmaster` AUTO_INCREMENT= 1");
                            }
                            catch
                            {
                                // Bỏ qua lỗi reset AUTO_INCREMENT
                            }
                        }
                    }
                }
                else // MASTER_BOM import
                {
                    if (BaseParameter.DataGridView1 != null && BaseParameter.DataGridView1.Count > 0)
                    {
                        // Tạo chuỗi SQL values
                        string SQL_T = "";

                        foreach (var item in BaseParameter.DataGridView1)
                        {
                            // Lấy giá trị từ các trường string
                            string mPart = Replace(item.M_PART ?? "", "'", "");
                            string sPart = Replace(item.S_PART ?? "", "'", "");
                            string sLR = Replace(item.S_LR ?? "", "'", "");

                            // Tạo SQL với các trường string
                            string SQL_D = $"('{mPart}', '{sPart}', '1', '{sLR}', '{currentDateTime}', '{userName}')";

                            if (string.IsNullOrEmpty(SQL_T))
                            {
                                SQL_T = SQL_D;
                            }
                            else
                            {
                                SQL_T = SQL_T + ", " + SQL_D;
                            }
                        }

                        // Insert vào bảng tạm với các trường string
                        string sqlInsertTemp = $@"INSERT INTO `torder_lead_bom_spst_EXCL` 
                    (`M_PART_IDX`, `S_PART_IDX`, `RQR_MENT`, `S_LR`, `CREATE_DTM`, `CREATE_USER`) 
                    VALUES {SQL_T}";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsertTemp);

                        // Insert từ bảng tạm vào bảng chính, thực hiện lookup để chuyển đổi sang ID
                        string sqlInsertMain = $@"
                    INSERT INTO `torder_lead_bom_spst` 
                    (`M_PART_IDX`, `S_PART_IDX`, `RQR_MENT`, `S_LR`, `CREATE_DTM`, `CREATE_USER`) 
                    SELECT 
                        IFNULL((SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `M_PART_IDX`), NULL) AS `M_PART_IDX`,
                        IFNULL((SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `S_PART_IDX`), NULL) AS `S_PART_IDX`,
                        `RQR_MENT`, `S_LR`, `CREATE_DTM`, `CREATE_USER`
                    FROM torder_lead_bom_spst_EXCL `EXCL`
                    WHERE `EXCL`.`CREATE_DTM` = '{currentDateTime}'
                    ON DUPLICATE KEY UPDATE 
                        `S_LR` = `EXCL`.`S_LR`,
                        `UPDATE_DTM` = `EXCL`.`CREATE_DTM`,  
                        `UPDATE_USER` = `EXCL`.`CREATE_USER`";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsertMain);

                        // Reset AUTO_INCREMENT nếu cần
                        try
                        {
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `torder_lead_bom_spst` AUTO_INCREMENT= 1");
                        }
                        catch
                        {
                            // Bỏ qua lỗi reset AUTO_INCREMENT
                        }
                    }
                }

                result.Success = true;
                result.Message = "정상처리 되었습니다. Đã được lưu.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Helper method để thay thế ký tự trong chuỗi
        private string Replace(string original, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(original))
                return "";

            return original.Replace(oldValue, newValue).Trim();  
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

        // Thêm phương thức này vào C19_1Service:

        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string query = @"SELECT `LEAD_SCN`, `LEAD_PN`, 
            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `W_PN_IDX`)) AS `WIRE`,
            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `T1_PN_IDX`)) AS `TERM1`,
            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `S1_PN_IDX`)) AS `SEAL1`,
            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `T2_PN_IDX`)) AS `TERM2`,
            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `S2_PN_IDX`)) AS `SEAL2`
            FROM torder_lead_bom   
            WHERE `W_PN_IDX` IS NULL AND NOT(`T1_PN_IDX` IS NULL) OR (`STRIP1` > 0 AND `T1_PN_IDX` IS NULL)
            OR (`STRIP2` > 0 AND `T2_PN_IDX` IS NULL)";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                if (ds.Tables.Count > 0)
                {
                    result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                }

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