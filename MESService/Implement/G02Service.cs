namespace MESService.Implement
{
    public class G02Service : BaseService<torderlist, ItorderlistRepository>
    , IG02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public G02Service(ItorderlistRepository torderlistRepository

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
                if (BaseParameter != null)
                {
                    var suchTB1 = BaseParameter.SearchString;
                    var AA = "" + suchTB1 + "";

                    string sql = @"SELECT  
                    tspart.`PART_IDX`, 
                    tspart.`PART_NO`, 
                    tspart.`PART_NM`, 
                    tspart.`PART_CAR`, 
                    tspart.`PART_FML`, 
                    IFNULL(`derivedtbl_1`.`CD_SYS_NOTE`, '-') AS `CD_SYS_NOTE`, 
                    IFNULL(`derivedtbl_1`.`QTY`, 0) AS `QTY`, 
                    `derivedtbl_1`.`LOC_IDX`, 
                    `derivedtbl_1`.`CD_IDX`, 
                    `derivedtbl_1`.`CDGR_IDX`, 
                    `derivedtbl_1`.`CD_NM_HAN`, 
                    derivedtbl_1.`CD_NM_EN`, 
                    tspart.PART_SCN
                    FROM  tspart LEFT OUTER JOIN
                    (SELECT  tiivtr.`IV_IDX`, tiivtr.`PART_IDX`, tiivtr.`LOC_IDX`, tiivtr.`QTY`, TSCODE.`CD_IDX`, TSCODE.`CDGR_IDX`, TSCODE.`CD_NM_HAN`,
                     TSCODE.`CD_NM_EN`, TSCODE.`CD_SYS_NOTE`    FROM     tiivtr, TSCODE   
                     WHERE  tiivtr.`LOC_IDX` = TSCODE.`CD_IDX`  AND TSCODE.`CDGR_IDX` = '1' ) `derivedtbl_1` ON tspart.`PART_IDX` = `derivedtbl_1`.`PART_IDX`

                    WHERE  tspart.`PART_NO` = '" + AA + "' ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_G02_01 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_G02_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        string LBPIDX = BaseParameter.ListSearchString[0];
                        string LBQTY = BaseParameter.ListSearchString[1];
                        string ComboBox1 = BaseParameter.ListSearchString[2];
                        string TextBox1 = BaseParameter.ListSearchString[3];
                        string TextBox3 = BaseParameter.ListSearchString[4];
                        string TextBox4 = BaseParameter.ListSearchString[5];

                        string sql = @"INSERT INTO tiivaj (`PART_IDX`, `ADJ_SCN`, `ADJ_DTM`, `ADJ_QTY`, `ADJ_BF_QTY`, `ADJ_AF_QTY`, `ADJ_RSON`, `CREATE_DTM`, `CREATE_USER`) VALUES	 
                            (" + LBPIDX + ", (SELECT `CD_IDX` FROM TSCODE WHERE TSCODE.`CDGR_IDX` = '1' AND TSCODE.`CD_SYS_NOTE` = '" + ComboBox1 + "'), NOW(), " + TextBox1 + ", " + LBQTY + ", " + TextBox3 + ", '" + TextBox4 + "', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `ADJ_RSON` = '" + TextBox4 + "', `ADJ_QTY`= " + TextBox1 + ", `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + USER_ID + "'  ";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO tiivaj_HISTORY (`PART_IDX`, `ADJ_SCN`, `ADJ_DTM`, `ADJ_QTY`, `ADJ_BF_QTY`, `ADJ_AF_QTY`, `ADJ_RSON`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + LBPIDX + ", (SELECT `CD_IDX` FROM TSCODE WHERE TSCODE.`CDGR_IDX` = '1' AND TSCODE.`CD_SYS_NOTE` = '" + ComboBox1 + "'), NOW(), " + TextBox1 + ", " + LBQTY + ", " + TextBox3 + ", '" + TextBox4 + "', NOW(), '" + USER_ID + "')  ";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + LBPIDX + ", (SELECT `CD_IDX` FROM TSCODE WHERE TSCODE.`CD_SYS_NOTE` = '" + ComboBox1 + "'), " + TextBox3 + ", NOW(), '" + USER_ID + "' ) ON DUPLICATE KEY UPDATE `QTY`= " + TextBox3 + ", `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + USER_ID + "'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `tiivaj`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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
        public virtual async Task<BaseResult> STOCK_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string PART_LOC = BaseParameter.ListSearchString[0];
                        string LBNO = BaseParameter.ListSearchString[1];

                        string WHR_CD = "";
                        if (PART_LOC == "5")
                        {
                            WHR_CD = " WHERE `CD_IDX` = '1'  ";
                        }
                        if (PART_LOC == "6")
                        {
                            WHR_CD = " WHERE `CD_IDX` = '2'  ";
                        }
                        if (WHR_CD == "")
                        {
                        }

                        string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE " + WHR_CD;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_G02_CB = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_G02_CB.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        result.DataGridView2 = new List<SuperResultTranfer>();

                        if (BaseParameter.DGV_G02_01 != null)
                        {
                            if (BaseParameter.DGV_G02_01.Count > 0)
                            {
                                if (PART_LOC == "5")
                                {
                                    sql = @"SELECT 0 AS `CHK`,
                                tiivtr.PART_IDX, 
                                (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX =tiivtr.PART_IDX) AS `PART_NO`, 
                                (SELECT tspart.PART_NM FROM tspart WHERE tspart.PART_IDX =tiivtr.PART_IDX) AS `PART_NM`, 
                                IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) AS `Change_QTY`, 
                                IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, 
                                IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,

                                IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) -IFNULL(tiivtr.QTY,0) AS `Difference_QTY`,

                                IFNULL(tiivtr.QTY,0) AS `MES_STOCK`, 

                                IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX =tiivtr.PART_IDX)), 0) AS `EXCEL`,

                                IF((IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) -IFNULL(tiivtr.QTY,0)) = 0, 'Good', 'Bad') AS `Verification`,

                                (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `Location` 

                                FROM tiivtr LEFT JOIN 
                                (SELECT `A`.`PART_CODE`, SUM(`A`.`Incoming_QTY`) AS `Incoming_QTY`, 
                                SUM(`A`.`OUT_QTY`) AS `OUT_QTY` 
                                FROM (
                                SELECT 
                                (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) AS `PART_CODE`, 
                                IF(TMBRCD.BBCO = 'Y', TMBRCD.`PKG_QTY`, 0) AS `Incoming_QTY`, 
                                IFNULL(TMBRCD.`PKG_OUTQTY`, 0) AS `OUT_QTY`
                                FROM TMBRCD     ) AS `A`
                                GROUP BY  `A`.`PART_CODE` ) `TB_B`
                                ON tiivtr.PART_IDX = `TB_B`.`PART_CODE`

                                WHERE tiivtr.LOC_IDX = '1' AND
                                (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) = '" + LBNO + "' ORDER BY  `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC   ";

                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView2 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                }

                                if (PART_LOC == "6")
                                {
                                    sql = @"SELECT 0 AS `CHK`,
                            `A`.`PART_IDX`, `A`.`PART_NO`, `A`.`PART_NM`, `A`.`Change_QTY`, 
                            `A`.`Incoming_QTY`, `A`.`OUT_QTY`,  (IFNULL(`A`.`Incoming_QTY`, 0) - IFNULL(`A`.`OUT_QTY`, 0) +  IFNULL(`A`.`Change_QTY`,0) - IFNULL(`A`.`STOCK_QTY`, 0)) AS `Difference_QTY`,
                            `A`.`STOCK_QTY` AS `MES_STOCK`,  
                            IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = `A`.`PART_NO`), 0) AS `EXCEL`,

                             IF((IFNULL(`A`.`Incoming_QTY`, 0) - IFNULL(`A`.`OUT_QTY`, 0) +  IFNULL(`A`.`Change_QTY`,0) - IFNULL(`A`.`STOCK_QTY`, 0)) = 0, 'Good', 'Bad') AS `Verification`,  
                            `A`.`Location`
                            /* ,  IFNULL(`SUB_A`.`QTY`, 0) AS `BE_QTY`, 
                            IFNULL(`SUB_B`.`IN`, 0) AS `IN`, IFNULL(`SUB_B`.`OUT`, 0) AS `OUT`,
                            IF((IFNULL(`A`.`STOCK_QTY`, 0) - (IFNULL(`SUB_A`.`QTY`, 0) + IFNULL(`SUB_B`.`IN`, 0) - IFNULL(`SUB_B`.`OUT`, 0))) = 0 ,'OK', 'NG') AS `CHK`
                            */
                            FROM 
                            (SELECT tiivtr.`PART_IDX`,
                            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NO`,
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NM`,
                            IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,
                            IFNULL( tiivtr.`QTY`, 0) AS `MES_STOCK`,
                            IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '2' AND tiivaj.PART_IDX = tiivtr.`PART_IDX`), 0) AS `Change_QTY`,
                            IFNULL( tiivtr.QTY, 0) AS `STOCK_QTY`,
                            (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.`PART_IDX`) AS `Location`

                            FROM tiivtr LEFT JOIN  (SELECT tdpdmtim.VLID_PART_IDX AS `PART_CODE`,  
                            COUNT(tdpdmtim.VLID_PART_IDX) AS `Incoming_QTY`, SUM(IF(tdpdmtim.VLID_DSCN_YN = 'Y', 1, 0)) AS `OUT_QTY`
                            FROM tdpdmtim GROUP BY `PART_CODE`) `TB_B`

                            ON tiivtr.`PART_IDX` = `TB_B`.`PART_CODE`

                            WHERE tiivtr.LOC_IDX = '2')  `A`
                            /* 
                            LEFT JOIN  (SELECT *
                            FROM (
                            SELECT tiivtr_HISTORY.`IV_IDX`, tiivtr_HISTORY.`PART_IDX`, tiivtr_HISTORY.`LOC_IDX`, tiivtr_HISTORY.`QTY`, tiivtr_HISTORY.`CREATE_DTM`
                            FROM tiivtr_HISTORY
                            WHERE tiivtr_HISTORY.`STOCK_DATE` = DATE_FORMAT(NOW(), '%Y-%m-%d')  AND tiivtr_HISTORY.LOC_IDX ='2'
                            ORDER BY tiivtr_HISTORY.`CREATE_DTM` DESC) `T_H`
                            GROUP BY `T_H`.`PART_IDX`
                            ) `SUB_A`
                            ON `A`.`PART_IDX` = `SUB_A`.`PART_IDX`

                            LEFT JOIN 
                            (SELECT
                            `MMDB`.`VLID_PART_IDX`,
                            IFNULL(SUM(CASE WHEN   `MMDB`.`VLID_DSCN_YN` = 'N' THEN  `MMDB`.`QTY` END), 0)  AS `IN`,
                            IFNULL(SUM(CASE WHEN   `MMDB`.`VLID_DSCN_YN` = 'Y' THEN  `MMDB`.`QTY` END), 0)  AS `OUT`
                            FROM( SELECT  tdpdmtim.`VLID_PART_IDX`, 1 AS `QTY`, tdpdmtim.`VLID_DSCN_YN`   FROM tdpdmtim
                            WHERE    tdpdmtim.`VLID_DTM` <= DATE_FORMAT(NOW(), '%Y-%m-%d') AND  tdpdmtim.`VLID_DTM` >= DATE_FORMAT(NOW(), '%Y-%m-%d') ) `MMDB`
                            GROUP BY `VLID_PART_IDX`) `SUB_B`

                            ON `A`.`PART_IDX` = `SUB_B`.`VLID_PART_IDX` */

                            WHERE `PART_NO`= '" + LBNO + "' ORDER BY  `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC  ";

                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView2 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                }
                            }
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
        public virtual async Task<BaseResult> tiivaj_HISTORY(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var LBNO = BaseParameter.SearchString;

                    string sql = @"SELECT `ADJ_DTM`, `ADJ_QTY`, `ADJ_BF_QTY`, `ADJ_AF_QTY`, `ADJ_RSON`, `CREATE_DTM`, `CREATE_USER` FROM   tiivaj_HISTORY
                        WHERE tiivaj_HISTORY.PART_IDX = (SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + LBNO + "') ORDER BY `CREATE_DTM` DESC LIMIT 100 ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
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

