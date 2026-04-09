namespace MESService.Implement
{
    public class V02Service : BaseService<torderlist, ItorderlistRepository>
    , IV02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public V02Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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

        public virtual async Task<BaseResult> COMBO_LIST1()
        {
            BaseResult result = new BaseResult();
            try
            {
                string DGV_DATA_CB1 = "SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_VN`, `CD_NM_HAN` FROM PDCDNM WHERE PDCDNM.CDGR_IDX = '2'";
                DataSet dsDGV_CB1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_CB1);
                result.ComboBox1 = new List<SuperResultTranfer>();
                for (int i = 0; i < dsDGV_CB1.Tables.Count; i++)
                {
                    DataTable dt = dsDGV_CB1.Tables[i];
                    result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> COMBO_LIST2()
        {
            BaseResult result = new BaseResult();
            try
            {
                string SQL_ORDERS = @"SELECT `A`.`PDP_NO` FROM PDPUSCH `A`
                            WHERE `A`.`PDP_REC_YN` ='N' AND `A`.`PDP_CONF` = 'Apply' GROUP BY `A`.PDP_NO";

                DataSet dsOrders = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_ORDERS);
                result.ComboBox2 = new List<SuperResultTranfer>();
                for (int i = 0; i < dsOrders.Tables.Count; i++)
                {
                    DataTable dt = dsOrders.Tables[i];
                    result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // TabPage1
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string S_AAA = BaseParameter.ListSearchString[0];

                            string DGV_DATA1 = @"SELECT 'ORDER', `PDPART_IDX`, `PN_V`, `PSPEC_V`, 
                                                (SELECT CD_NM_VN FROM PDCDNM WHERE PDCDNM.CD_IDX = pdpart.PUNIT_IDX) AS `UNIT_VN`,
                                                `PN_K`, `PSPEC_K`, 
                                                (SELECT CD_NM_HAN FROM PDCDNM WHERE PDCDNM.CD_IDX = pdpart.PUNIT_IDX) AS `UNIT_KR`,
                                                `PQTY`, `PN_NM`, IFNULL(`S`.`STOCK`, 0) AS `STOCK`
                                                FROM pdpart 
                                                LEFT JOIN (SELECT `PART_IDX`, SUM(`QTY`) AS `STOCK` FROM pd_tiivtr GROUP BY `PART_IDX` ) `S` ON  `PDPART_IDX` = `S`.`PART_IDX`
                                                WHERE `PN_GROUP` = 'Normal' 
                                                    AND `PN_DSCN_YN` = 'Y'
                                                    AND (`PN_V` LIKE '%" + S_AAA + @"%' 
                                                         OR `PN_K` LIKE '%" + S_AAA + @"%' 
                                                         OR `PN_V` LIKE '%" + S_AAA + @"%' 
                                                         OR `PN_K` LIKE '%" + S_AAA + @"%' 
                                                         OR `PN_NM` LIKE '%" + S_AAA + @"%' 
                                                         OR `PSPEC_V` LIKE '%" + S_AAA + @"%' 
                                                         OR `PSPEC_K` LIKE '%" + S_AAA + @"%')";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2) // TabPage2
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string orderNo = BaseParameter.ListSearchString[0];

                            string DGV_DATA2 = @"SELECT 
                                                `A`.`PDP_CONF`,`A`.`PDP_NO`, `A`.`PDP_DATE1`, (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEPARTMENT`,
                                                `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                                                `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, `B`.`PQTY`, `A`.`PDP_QTY`,
                                                `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `NAME`, IFNULL(`PDP_REMARK`, '') AS `PDP_REMARK`
                                                FROM PDPUSCH `A` LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                                                WHERE `A`.`PDP_REC_YN` ='N' AND `A`.`PDP_CONF` = 'Apply' AND `A`.PDP_NO = '" + orderNo + "'";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA2);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        string S_AAA = BaseParameter.ListSearchString?.FirstOrDefault() ?? "";
                        string DGV_DATA1 = @"SELECT 'ORDER', `pdpart_IDX`, `PN_V`, `PSPEC_V`, 
                    (SELECT CD_NM_VN FROM PDCDNM WHERE PDCDNM.CD_IDX = pdpart.PUNIT_IDX) AS `UNIT_VN`,
                    `PN_K`, `PSPEC_K`, (SELECT CD_NM_HAN FROM PDCDNM WHERE PDCDNM.CD_IDX = pdpart.PUNIT_IDX) AS `UNIT_KR`,
                    `PQTY`, `PN_NM`, IFNULL(`S`.`STOCK`, 0) AS `STOCK`, `PN_PHOTO`
                    FROM pdpart
                    LEFT JOIN (SELECT `PART_IDX`, SUM(`QTY`) AS `STOCK` FROM pd_tiivtr GROUP BY `PART_IDX` ) `S` 
                        ON  `pdpart_IDX` = `S`.`PART_IDX`
                    WHERE `PN_GROUP` = 'ME' AND `PN_DSCN_YN` = 'Y' 
                        AND (`PN_V` LIKE '%" + S_AAA + @"%' 
                             OR `PN_K` LIKE '%" + S_AAA + @"%' 
                             OR `PN_NM` LIKE '%" + S_AAA + @"%' 
                             OR `PSPEC_V` LIKE '%" + S_AAA + @"%' 
                             OR `PSPEC_K` LIKE '%" + S_AAA + @"%')";

                        DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                        result.DataGridView5 = new List<SuperResultTranfer>();
                        for (int i = 0; i < dsDGV_01.Tables.Count; i++)
                        {
                            DataTable dt = dsDGV_01.Tables[i];
                            result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }

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
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;

                    if (BaseParameter.Action == 1) // TabPage1
                    {
                        if (BaseParameter.DataGridView2 != null && BaseParameter.DataGridView2.Count > 0)
                        {
                            string VAL_DT = "";
                            string VAL_SUM = "";

                            string CODE = BaseParameter.ListSearchString[0] + DateTime.Now.ToString("yyMMddHHmmss");

                            for (int i = 0; i < BaseParameter.DataGridView2.Count; i++)
                            {
                                if (BaseParameter.DataGridView2[i].ADD_3 == "Stay")
                                {
                                    string AAA = "Apply";
                                    string FACT = BaseParameter.DataGridView2[i].ADD_1;
                                    string BBB = CODE;
                                    string CCC = BaseParameter.DataGridView2[i].DEP_CODE;
                                    string DDD = BaseParameter.DataGridView2[i].DJG_CODE;
                                    string EEE = BaseParameter.DataGridView2[i].QTY.ToString().Replace("'", "");
                                    string FFF = BaseParameter.DataGridView2[i].ADD_2?.Replace("'", "");

                                    if (string.IsNullOrEmpty(FFF) || FFF.Length < 2)
                                    {
                                        result.ErrorNumber = 1001;
                                        return result;
                                    }

                                    VAL_DT = "('" + AAA + "', '" + FACT + "', '" + BBB + "', NOW(), '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + FFF + "', 'N', 'N', NOW(), '" + USER_IDX + "')";

                                    if (VAL_SUM.Length == 0)
                                    {
                                        VAL_SUM = VAL_DT;
                                    }
                                    else
                                    {
                                        VAL_SUM = VAL_SUM + ", " + VAL_DT;
                                    }
                                }
                            }

                            if (VAL_SUM.Length > 0)
                            {
                                string sql = "INSERT INTO `PDPUSCH` (`PDP_CONF`, `PDP_FACT`, `PDP_NO`, `PDP_DATE1`, `PDP_DEPA`, `PDP_PART`, `PDP_QTY`, `PDP_MEMO`, `PDP_REC_YN`, `PDP_CNF_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES " + VAL_SUM;
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                result.ErrorNumber = 0;
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2) 
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.ListSearchString[0]))
                        {
                            string orderNo = BaseParameter.ListSearchString[0];

                            string sql = "UPDATE PDPUSCH `A` SET `A`.`PDP_CONF` = 'Report', `A`.`PDP_REC_YN` ='Y', `A`.`UPDATE_DTM` = NOW(), `A`.`UPDATE_USER` = '" + USER_IDX + "', `A`.`PDP_PRIENT` = 'N' " +
                                         "WHERE `A`.`PDP_REC_YN` ='N' AND `A`.`PDP_CONF` = 'Apply' AND `A`.PDP_NO = '" + orderNo + "'";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            result.ErrorNumber = 0;
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.DataGridView6 != null && BaseParameter.DataGridView6.Count > 0)
                        {
                            string VAL2_DT = "";
                            string VAL2_SUM = "";
                        
                            for (int II = 0; II < BaseParameter.DataGridView6.Count; II++)
                            {
                                var row = BaseParameter.DataGridView6[II];
                                if (row.ADD_3 == "Stay")
                                {
                                    string A_DATE = DateTime.Now.ToString("yyyy-MM-dd");  
                                    string B_PARTIDX = row.DJG_CODE;     
                                    string C_MCNO = row.DEP_CODE;      
                                    string D_QTY = row.QTY?.ToString() ?? "0";    

                                 
                                    VAL2_DT = "('" + A_DATE + "', '" + B_PARTIDX + "', '" + C_MCNO + "', '" + D_QTY + "', 'Stay', 'Y', NOW(), NOW(), '" + USER_IDX + "', '')";
                                    if (string.IsNullOrEmpty(VAL2_SUM))
                                    {
                                        VAL2_SUM = VAL2_DT;
                                    }
                                    else
                                    {
                                        VAL2_SUM = VAL2_SUM + ", " + VAL2_DT;
                                    }
                                 
                                    row.ADD_3 = "Done";
                                }
                            }

                            if (!string.IsNullOrEmpty(VAL2_SUM))
                            {
                                string sql = "INSERT INTO `PD_MC_ORDERLIST` (`PD_MC_ODDATE`, `PD_PART_IDX`, `PD_MC_NO`, `PD_MC_QTY`, `PD_MC_STAY`, `PD_MC_DSN_YN`, `PD_PURDATE`, `CREATE_DTM`, `CREATE_USER`, `PD_ORDER_NO`) VALUES " + VAL2_SUM;
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.ErrorNumber = 0;
                            }
                            else
                            {
                                result.ErrorNumber = 1234;
                            }
                        }
                    }
                    if (BaseParameter.Action == 4) 
                    {
                        string orderNo = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0
                            ? BaseParameter.ListSearchString[0]
                            : "";

                        string sql = "";
                        if (!string.IsNullOrEmpty(orderNo))
                        {
                         
                            if (BaseParameter.OR_NO_CHK == true)
                            {
                                sql = @"SELECT 
                            `A`.`PDP_CONF`,`A`.`PDP_NO`, `A`.`PDP_DATE1`, 
                            (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEP`,
                            `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                            `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, `B`.`PQTY`, `A`.`PDP_QTY`,
                            `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, 
                            (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`PDPUSCH_IDX`
                        FROM PDPUSCH `A` 
                        LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                        WHERE NOT(`A`.`PDP_CONF` = 'Order') 
                            AND NOT(`A`.`PDP_CONF` = 'DEL') 
                            AND `A`.`PDP_NO` = '" + orderNo + "'";
                            }
                            else if (BaseParameter.CMPY_NO_CHK == true)
                            {
                                sql = @"SELECT 
                            `A`.`PDP_CONF`,`A`.`PDP_NO`, `A`.`PDP_DATE1`, 
                            (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEP`,
                            `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                            `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, `B`.`PQTY`, `A`.`PDP_QTY`,
                            `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, 
                            (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`PDPUSCH_IDX`
                        FROM PDPUSCH `A` 
                        LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                        WHERE NOT(`A`.`PDP_CONF` = 'Order') 
                            AND NOT(`A`.`PDP_CONF` = 'DEL') 
                            AND `A`.`PDP_NO` = '" + orderNo + "'";
                            }

                            if (!string.IsNullOrEmpty(sql))
                            {
                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView4 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorNumber = 9999;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;

                    if (BaseParameter.Action == 2) 
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.ListSearchString[0]))
                        {
                            string orderNo = BaseParameter.ListSearchString[0];

                            string sql = "UPDATE PDPUSCH `A` SET `A`.`PDP_CONF` = 'DEL', `A`.PDP_REC_YN ='N', `A`.UPDATE_DTM = NOW(), `A`.UPDATE_USER = '" + USER_IDX + "' " +
                                         "WHERE `A`.`PDP_CONF` = 'Apply' AND `A`.PDP_NO = '" + orderNo + "'";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            result.ErrorNumber = 0;
                        }
                    }
                    if (BaseParameter.Action == 4) 
                    {
                        string orderNo = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0
                            ? BaseParameter.ListSearchString[0]
                            : "";

                        if (!string.IsNullOrEmpty(orderNo))
                        {
                            string sql = "";
                           
                            if (BaseParameter.OR_NO_CHK == true)
                            {
                                sql = @"UPDATE PDPUSCH `A` 
                                SET `A`.`PDP_CONF` = 'DEL', `A`.PDP_REC_YN ='N', `A`.UPDATE_DTM = NOW(), `A`.UPDATE_USER = '" + USER_IDX + @"' 
                                WHERE `A`.`PDP_NO` = '" + orderNo + "'";
                            }
                        
                            else if (BaseParameter.CMPY_NO_CHK == true)
                            {
                                string pdpuschIdx = BaseParameter.PDPUSCH_IDX;
                                if (!string.IsNullOrEmpty(pdpuschIdx))
                                {
                                    sql = @"UPDATE PDPUSCH `A` 
                                    SET `A`.`PDP_CONF` = 'DEL', `A`.PDP_REC_YN ='N', `A`.UPDATE_DTM = NOW(), `A`.UPDATE_USER = '" + USER_IDX + @"' 
                                    WHERE `A`.`PDP_NO` = '" + orderNo + @"'  
                                    AND  `A`.`PDPUSCH_IDX` = '" + pdpuschIdx + "'";
                                }
                            }

                            if (!string.IsNullOrEmpty(sql))
                            {
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.ErrorNumber = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ErrorNumber = 9999;
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