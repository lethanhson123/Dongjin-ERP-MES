namespace MESService.Implement
{
    public class V06Service : BaseService<torderlist, ItorderlistRepository>
    , IV06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public V06Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
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
                // TabPage3: Chi tiết đặt hàng
                if (BaseParameter.TabName == "TabPage3" && BaseParameter.ListSearchString != null)
                {
                    // Mapping tham số giống VB
                    string SU01 = BaseParameter.ListSearchString[0];  // DateTimePicker1
                    string SU02 = BaseParameter.ListSearchString[1];  // DateTimePicker2
                    string SU03 = BaseParameter.ListSearchString[2];  // TextBox10 - Số sản phẩm
                    string SU04 = BaseParameter.ListSearchString[3];  // TextBox11 - Tên sản phẩm
                    string SU05 = BaseParameter.ListSearchString[4];  // TextBox12 - Quy cách sản phẩm
                    string SU06 = BaseParameter.ListSearchString[5];  // TextBox13 - Tên nhà cung cấp
                    string SU07 = BaseParameter.ListSearchString[6];  // TextBox14 - Tên bộ phận
                    string SU08 = BaseParameter.ListSearchString[7];  // TextBox15 - Số đơn hàng
                    string SU09 = BaseParameter.ListSearchString[8];  // ComboBox1 - State

                    if (SU09 == "0")
                        SU09 = "%%";
                    else if (SU09 == "6")
                        SU09 = "0";

                    string CHK_DATE = "";
                    if (BaseParameter.RadioButton1 == true)
                        CHK_DATE = $" AND `PDP_DATE1` >= '{SU01}' AND `PDP_DATE1` <= '{SU02}'";
                    else if (BaseParameter.RadioButton2 == true)
                        CHK_DATE = $" AND `PDP_CNF_DATE` >= '{SU01} 00:00:00' AND `PDP_CNF_DATE` <= '{SU02} 23:59:59'";

                    string PDP_CNF_YN = BaseParameter.RadioButton3 == true ? "Y" : (BaseParameter.RadioButton4 == true ? "N" : "");

                    string DGV_DATA2 = @"SELECT `A`.`PDP_CONF`,`A`.`PDP_NO`, `A`.`PDP_DATE1`, 
                                        (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEP`,
                                        IFNULL(`A`.`PDP_REMARK`, '') AS `PDP_REMARK`, `B`.`PN_NM`, `B`.`PQTY`, 
                                        IFNULL((SELECT `QTY` FROM pd_tiivtr WHERE `PART_IDX` = `A`.`PDP_PART` AND `A`.`PDPUSCH_IDX` = `ORDER_IDX`),0) AS `STOCK`, `A`.`PDP_BE_COST`,
                                        `A`.`PDP_QTY`, `A`.`PDP_COST`,(`A`.`PDP_COST` * `A`.`PDP_QTY`) AS `SUM_COST`, `A`.`PDP_ECTCOST`, `A`.`PDP_VAT`, `A`.`PDP_TOTCOST`, 
                                        `A`.`PDP_MEMO`, IFNULL(`A`.`PDP_CMPY`, '') AS `PDP_CMPY`, IFNULL((SELECT CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = `A`.`PDP_CMPY`),'') AS `COMP_NM`,  `A`.`CREATE_DTM`, 
                                        `A`.`CREATE_USER`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`PDP_PART`, `A`.`PDPUSCH_IDX`, 
                                        `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                                        `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, 
                                        IFNULL(`PDP_ORD_ST`, '0') AS `ORDER_ST`, IFNULL(`PDP_IN_QTY`, 0) AS `PDP_IN_QTY`, IFNULL(`PDP_FIFO`, 'N') AS `PDP_FIFO`, IFNULL(DATE_FORMAT(`A`.`PDP_CNF_DATE`, '%Y-%m-%d'), '----') AS `PDP_CNF_DATE`
                                        FROM PDPUSCH `A` 
                                        LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                                        WHERE
                                        `A`.`PDP_REC_YN` ='Y' AND (`A`.`PDP_CONF` = 'Order' OR `A`.`PDP_CONF` = 'Report') 
                                        AND `PDP_CNF_YN` = '" + PDP_CNF_YN + @"' 
                                        AND `B`.`PN_NM` LIKE '%" + SU03 + @"%' 
                                        AND (`B`.`PN_V` LIKE '%" + SU04 + @"%' OR `B`.`PN_K` LIKE '%" + SU04 + @"%') 
                                        AND (`B`.`PSPEC_V` LIKE '%" + SU05 + @"%' OR `B`.`PSPEC_K` LIKE '%" + SU05 + @"%') 
                                        AND (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) LIKE '%" + SU07 + @"%' 
                                        AND `A`.`PDP_NO` LIKE '%" + SU08 + @"%' 
                                        HAVING `COMP_NM` LIKE '%" + SU06 + @"%'   
                                        AND `ORDER_ST` LIKE '" + SU09 + @"' " + CHK_DATE;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA2);
                    result.DataGridView4 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    result.Success = true;
                }

                // TabPage4: Tổng hợp theo năm
                else if (BaseParameter.TabName == "TabPage4" && BaseParameter.ListSearchString != null)
                {
                    string ComboBox2 = BaseParameter.ListSearchString[0];
                    string DATE_D1 = ComboBox2 + "-01-01";
                    string DATE_D2 = ComboBox2 + "-12-31";

                    string DGV_DATA98 = @"SELECT 
                                        CONCAT(`MN`.`PDP_FACT`,'_SUM') AS `PDP_FACT`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '1' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D01`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '2' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D02`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '3' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D03`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '4' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D04`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '5' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D05`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '6' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D06`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '7' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D07`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '8' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D08`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '9' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D09`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '10' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D10`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '11' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D11`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '12' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D12`
                                        FROM 
                                        (SELECT PDCDNM.`CD_SYS_NOTE` FROM PDCDNM WHERE PDCDNM.`CDGR_IDX` = '2') `M_M`
                                        LEFT JOIN 
                                        (SELECT   
                                        IF(PDPUSCH.PDP_FACT = '', 'DJG', IFNULL(PDPUSCH.PDP_FACT, 'DJG')) AS `PDP_FACT`,
                                        (SELECT PDCDNM.CD_SYS_NOTE FROM PDCDNM WHERE PDCDNM.CD_IDX = PDPUSCH.`PDP_DEPA`) AS `PDP_DEPA`,
                                        MONTH(PDPUSCH.`PDP_DATE1`) AS `MONTH`,
                                        ROUND(SUM((IFNULL(PDPUSCH.PDP_QTY, 0) * IFNULL(PDPUSCH.PDP_COST, 0)) + IFNULL(PDPUSCH.PDP_VAT, 0) + IFNULL(PDPUSCH.PDP_ECTCOST, 0)), 2) AS `TOT_SUM`
                                        FROM PDPUSCH
                                        WHERE `PDP_DATE1` >= '" + DATE_D1 + @"' AND `PDP_DATE1` <= '" + DATE_D2 + @"' AND `PDP_CNF_YN` = 'Y' AND NOT(IFNULL(`PDP_ORD_ST`, 0) ='4')  
                                        GROUP BY `PDP_FACT`, PDPUSCH.`PDP_DEPA`, MONTH(PDPUSCH.`PDP_DATE1`)) `MN`
                                        ON `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA`
                                        GROUP BY `MN`.`PDP_FACT`
                                        UNION
                                        SELECT 
                                        `M_M`.`CD_SYS_NOTE`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '1' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D01`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '2' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D02`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '3' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D03`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '4' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D04`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '5' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D05`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '6' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D06`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '7' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D07`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '8' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D08`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '9' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D09`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '10' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D10`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '11' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D11`,
                                        IFNULL(SUM(CASE WHEN `MN`.`MONTH` = '12' AND `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA` THEN `MN`.`TOT_SUM` END), 0) AS `D12`
                                        FROM 
                                        (SELECT PDCDNM.`CD_SYS_NOTE` FROM PDCDNM WHERE PDCDNM.`CDGR_IDX` = '2') `M_M`
                                        LEFT JOIN 
                                        (SELECT   
                                        IF(PDPUSCH.PDP_FACT = '', 'DJG', IFNULL(PDPUSCH.PDP_FACT, 'DJG')) AS `PDP_FACT`,
                                        (SELECT PDCDNM.CD_SYS_NOTE FROM PDCDNM WHERE PDCDNM.CD_IDX = PDPUSCH.`PDP_DEPA`) AS `PDP_DEPA`,
                                        MONTH(PDPUSCH.`PDP_DATE1`) AS `MONTH`,
                                        ROUND(SUM((IFNULL(PDPUSCH.PDP_QTY, 0) * IFNULL(PDPUSCH.PDP_COST, 0)) + IFNULL(PDPUSCH.PDP_VAT, 0) + IFNULL(PDPUSCH.PDP_ECTCOST, 0)), 2) AS `TOT_SUM`
                                        FROM PDPUSCH
                                        WHERE `PDP_DATE1` >= '" + DATE_D1 + @"' AND `PDP_DATE1` <= '" + DATE_D2 + @"' AND `PDP_CNF_YN` = 'Y' AND NOT(IFNULL(`PDP_ORD_ST`, 0) ='4')  
                                        GROUP BY `PDP_FACT`, PDPUSCH.`PDP_DEPA`, MONTH(PDPUSCH.`PDP_DATE1`)) `MN`
                                        ON `M_M`.`CD_SYS_NOTE` = `MN`.`PDP_DEPA`
                                        GROUP BY `M_M`.`CD_SYS_NOTE` WITH ROLLUP";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA98);
                    result.DataGridView5 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
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
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 6)
                {
                    string PDP_QTY = BaseParameter.ListSearchString[0];
                    string PDP_COST = BaseParameter.ListSearchString[1];
                    string PDP_VAT = BaseParameter.ListSearchString[2];
                    string PDP_ECTCOST = BaseParameter.ListSearchString[3];
                    string PDP_TOTCOST = BaseParameter.ListSearchString[4];
                    string PDPUSCH_IDX = BaseParameter.ListSearchString[5];

                    string sql = $@"
                                    UPDATE `PDPUSCH` 
                                    SET 
                                        `PDP_QTY` = '{PDP_QTY}', 
                                        `PDP_COST` = '{PDP_COST}', 
                                        `PDP_VAT` = '{PDP_VAT}', 
                                        `PDP_ECTCOST` = '{PDP_ECTCOST}', 
                                        `PDP_TOTCOST` = '{PDP_TOTCOST}' 
                                    WHERE `PDPUSCH_IDX` = '{PDPUSCH_IDX}'";

                    string resultSQL = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
                }
               
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            var result = new BaseResult();
            try
            {
                // Kiểm tra đủ tham số cần thiết để xóa
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 1)
                {
                    var PDPUSCH_IDX = BaseParameter.ListSearchString[0];
                    var UPDATE_USER = BaseParameter.USER_ID ?? ""; // Nên truyền USER_ID từ FE

                    var sql = $@"
                                UPDATE `PDPUSCH` 
                                SET 
                                    `PDP_CONF`   = 'DEL', 
                                    `UPDATE_DTM` = NOW(), 
                                    `UPDATE_USER`= '{UPDATE_USER}'
                                WHERE `PDPUSCH_IDX` = '{PDPUSCH_IDX}'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
                }
               
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

