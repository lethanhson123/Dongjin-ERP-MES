namespace MESService.Implement
{
    public class Admin6UserService : BaseService<torderlist, ItorderlistRepository>, IAdmin6UserService
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public Admin6UserService(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }

        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }

        // Lấy thông tin stock - Giống STK_BACODE() trong VB
        public virtual async Task<BaseResult> STK_BACODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox1 = BaseParameter.ListSearchString[0];

                string DGV_DATA16 = @"SELECT 
`DB_AA`.`HOOK_RACK`,  `DB_AA`.`LEAD_NM`,  IFNULL(`STOCKDB`.`STOCK_QTY`, 0) AS `STOCK_QTY`,
`DB_AA`.`IN_QTY`,  `DB_AA`.`OUT_QTY`,
IF(((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) =0, 'Good', 'Bad') AS `Stock_status`, 
((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `CHK_QTY`
FROM (SELECT 
(SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `HOOK_RACK`, 
trackmtim.`LEAD_NM`, (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = trackmtim.`LEAD_NM`) AS `LD_IDX`,
SUM(IF(trackmtim.`RACKIN_YN`='Y', trackmtim.QTY, 0)) AS `IN_QTY`, 
SUM(IF(trackmtim.`RACKOUT_YN`='Y', trackmtim.QTY, 0)) AS `OUT_QTY`
FROM trackmtim 
GROUP BY trackmtim.`LEAD_NM`) `DB_AA` 
LEFT JOIN  (SELECT  
tiivtr_lead.`PART_IDX`, IFNULL(tiivtr_lead.`QTY`, 0) AS `STOCK_QTY`, IFNULL(tiivaj_LEAD.`ADJ_QTY`, 0) AS `ADJ_QTY`
FROM tiivtr_lead LEFT JOIN tiivaj_LEAD
ON tiivtr_lead.`PART_IDX` = tiivaj_LEAD.`PART_IDX` AND tiivtr_lead.`LOC_IDX` = tiivaj_LEAD.`ADJ_SCN`
WHERE tiivtr_lead.`LOC_IDX` = '3') `STOCKDB`
ON  `DB_AA`.`LD_IDX` = `STOCKDB`.`PART_IDX`
WHERE LENGTH(`DB_AA`.`LEAD_NM`) > 0 AND `DB_AA`.`LEAD_NM` = '" + TextBox1 + @"'
ORDER BY `Stock_status` ASC,  `CHK_QTY` ASC , `DB_AA`.`LEAD_NM`";

                DataSet dsDGV_016 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA16);
                result.DataGridView1 = new List<SuperResultTranfer>();

                if (dsDGV_016.Tables.Count > 0)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsDGV_016.Tables[0]));
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Lấy transaction history - Giống STK_BACODE_DATA() trong VB
        public virtual async Task<BaseResult> STK_BACODE_DATA(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox1 = BaseParameter.ListSearchString[0];

                string DGV_DATA16 = "SELECT `TABLE_NM`, `RACKCODE`, `QTY`, `RACKDTM`, `RACKOUT_DTM`, `BARCODE_NM` FROM trackmtim WHERE trackmtim.LEAD_NM = '" + TextBox1 + "' ORDER BY trackmtim.RACKDTM DESC";

                DataSet dsDGV_016 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA16);
                result.DataGridView2 = new List<SuperResultTranfer>();

                if (dsDGV_016.Tables.Count > 0)
                {
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsDGV_016.Tables[0]));
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Tìm kiếm LEAD - Giống Button1_Click() trong VB
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox1 = BaseParameter.ListSearchString[0];

                string DGV_DATA1 = @"SELECT torder_lead_bom.LEAD_PN, torder_lead_bom.`BUNDLE_SIZE`, 
(SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = torder_lead_bom.LEAD_PN) AS `HOOK_RACK` 
FROM torder_lead_bom 
WHERE torder_lead_bom.LEAD_PN LIKE '%" + TextBox1 + @"%' 
AND torder_lead_bom.DSCN_YN = 'Y'
LIMIT 1000";

                DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                result.DataGridView1 = new List<SuperResultTranfer>();

                if (dsDGV_01.Tables.Count > 0)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsDGV_01.Tables[0]));
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Điều chỉnh stock - Giống Button3_Click() trong VB
        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string AAA = BaseParameter.ListSearchString[0]; // LEAD_NO
                string BBB = BaseParameter.ListSearchString[1]; // DateTime
                string CCC = BaseParameter.ListSearchString[2]; // QTY

                string sql1 = "UPDATE trackmtim SET trackmtim.RACKIN_YN = 'Y', trackmtim.RACKOUT_YN= 'Y', trackmtim.RACKCODE = 'OUTPUT', trackmtim.RACKOUT_DTM = NOW() WHERE trackmtim.LEAD_NM = '" + AAA + "' AND trackmtim.RACKDTM < '" + BBB + "' AND trackmtim.RACKOUT_YN= 'N'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                string sql2 = "UPDATE tiivtr_lead SET tiivtr_lead.QTY = '" + CCC + "' WHERE tiivtr_lead.PART_IDX = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + AAA + "') AND tiivtr_lead.`LOC_IDX` = '3'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // Xử lý barcode - Giống BC_KeyDown() trong VB
        public virtual async Task<BaseResult> BC_KeyDown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string BC = BaseParameter.ListSearchString[0];
                string LEAD_NO = BaseParameter.ListSearchString[1];
                string QTY = BaseParameter.ListSearchString[2];
                string user = BaseParameter.USER_IDX ?? "SYSTEM";

                // Check barcode đã tồn tại và trạng thái
                string checkSQL = "SELECT trackmtim.`RACKDTM`, trackmtim.`RACKOUT_YN` FROM trackmtim WHERE trackmtim.`BARCODE_NM` = '" + BC + "'";
                DataSet dsCheck = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkSQL);

                if (dsCheck.Tables.Count > 0 && dsCheck.Tables[0].Rows.Count > 0)
                {
                    string RACKOUT_YN = dsCheck.Tables[0].Rows[0]["RACKOUT_YN"].ToString();

                    if (RACKOUT_YN == "Y")
                    {
                        // Tái nhập kho - UPDATE
                        string updateSQL = "UPDATE trackmtim SET `RACKCODE`='INPUT', `RACKDTM` = NOW(), `RACKOUT_DTM`= NULL ,`QTY` = '" + QTY + "', `RACKOUT_YN`='N', `CREATE_USER`='" + user + "' WHERE `BARCODE_NM`= '" + BC + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSQL);

                        string updateStockSQL = @"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`)
SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '" + user + @"'
FROM trackmtim WHERE trackmtim.BARCODE_NM = '" + BC + @"'
ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` =(tiivtr_lead.`QTY` + " + QTY + "), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '" + user + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateStockSQL);

                        try
                        {
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                        }
                        catch { }

                        result.Success = true;
                        result.Message = "RE-INPUT";
                    }
                    else
                    {
                        // Đã nhập trước đó
                        result.Error = "Đã xử lý Barcode trước đó";
                    }
                }
                else
                {
                    // Barcode mới - INSERT
                    string insertSQL = @"INSERT INTO trackmtim (trackmtim.RACK_IDX, trackmtim.RACKCODE, trackmtim.TABLE_IDX, trackmtim.TABLE_NM, trackmtim.LEAD_NM, trackmtim.BARCODE_NM, trackmtim.RACKDTM, 
trackmtim.QTY, trackmtim.RACKIN_YN, trackmtim.RACKOUT_YN, trackmtim.CREATE_DTM, trackmtim.CREATE_USER) 
SELECT (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = '" + LEAD_NO + "') AS `RACK_IDX`, 'INPUT', 0, 'USER', '" + LEAD_NO + "', '" + BC + "', NOW(), '" + QTY + "', 'Y', 'N', NOW(), '" + user + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSQL);

                    string insertStockSQL = @"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`)
SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + LEAD_NO + "') AS `LEAD_IDX`, '3', '" + QTY + "', NOW(), '" + user + @"'
ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` = tiivtr_lead.`QTY` + " + QTY + ", tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '" + user + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertStockSQL);

                    try
                    {
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                    }
                    catch { }

                    result.Success = true;
                    result.Message = "INPUT";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}