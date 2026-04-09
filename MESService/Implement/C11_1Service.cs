

namespace MESService.Implement
{
    public class C11_1Service : BaseService<torderlist, ItorderlistRepository>
    , IC11_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C11_1Service(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var ORDER = BaseParameter.ListSearchString[0];

                        string sql = @"SELECT 
                        `ORDER_IDX`, 
                        `LOC_LRJ`, 
                        `PERFORMN`, 
                        `DSCN_YN`, 
                        IFNULL(`T1_TOOL_IDX`, 0) AS `T1_TOOL_IDX`, 
                        `ERROR_CHK`,
                        IFNULL((SELECT (SELECT `APPLICATOR` FROM TTOOLMASTER WHERE TTOOLMASTER.`TOOL_IDX`  = ttoolmaster2.`TOOL_IDX`) FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` =  `T1_TOOL_IDX`), '') AS `APP`,
                        IFNULL((SELECT `SEQ` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` =  `T1_TOOL_IDX`), '') AS `SEQ`,
                        IFNULL((SELECT `WK_CNT` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` =  `T1_TOOL_IDX`),0) AS `COUNT`,
                        IFNULL((SELECT (SELECT `MAX_CNT` FROM TTOOLMASTER WHERE TTOOLMASTER.`TOOL_IDX`  = ttoolmaster2.`TOOL_IDX`) FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` =  `T1_TOOL_IDX`),0) AS `MAX`,
                        (SELECT `TOT_QTY` FROM TORDERLIST WHERE `ORDER_IDX` = `A`.ORDER_IDX) AS `TOT_COUNT` 
                        FROM torder_bom_sw `A` WHERE `A`.`ORDER_IDX` = '" + ORDER + "' AND `A`.`LOC_LRJ` = 'L'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count <= 0)
                        {

                        }
                        else
                        {
                            if (result.Search[0].PERFORMN == result.Search[0].TOT_COUNT)
                            {
                                sql = @"UPDATE `torder_bom_sw` SET `DSCN_YN`='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `ORDER_IDX`= '" + ORDER + "'  AND `LOC_LRJ` = 'L'";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var ORDER = BaseParameter.ListSearchString[0];

                        string sql = @"SELECT `COLSIP` FROM torderinspection_sw WHERE `ORDER_IDX` = '" + ORDER + "' AND `LOC_LRJ` = 'L'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var C11_COUNT3 = BaseParameter.ListSearchString[0];
                        var ORDER = BaseParameter.ListSearchString[1];
                        var TM_QTY = BaseParameter.ListSearchString[2];
                        var VLA1 = BaseParameter.ListSearchString[3];
                        var VLA11 = BaseParameter.ListSearchString[4];
                        var C11_D01 = BaseParameter.ListSearchString[5];
                        var TOOL_CONT = BaseParameter.ListSearchString[6];
                        var C11_D02 = BaseParameter.ListSearchString[7];
                        var VLT1 = BaseParameter.ListSearchString[8];

                        double A_CONT = 0;
                        try
                        {
                            A_CONT = double.Parse(TOOL_CONT.Replace(",", ""));
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                            A_CONT = double.Parse(TOOL_CONT);
                        }


                        string sql = @"UPDATE `torder_bom_sw` SET `PERFORMN`= `PERFORMN` + " + C11_COUNT3 + ", `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + C_USER + "' WHERE  `ORDER_IDX`= '" + ORDER + "' AND `LOC_LRJ` = 'L'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"UPDATE ttoolmaster2 SET `TOT_WK_CNT`= `TOT_WK_CNT` + " + TM_QTY + ", `WK_CNT` = `WK_CNT` + " + TM_QTY + ", `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + C_USER + "' WHERE `TOOLMASTER_IDX`= (SELECT ttoolmaster2.`TOOLMASTER_IDX`  FROM ttoolmaster2 WHERE ttoolmaster2.`TOOL_IDX` =(SELECT TTOOLMASTER.`TOOL_IDX` FROM TTOOLMASTER WHERE TTOOLMASTER.`APPLICATOR`  = '" + VLA1 + "') AND ttoolmaster2.SEQ = '" + VLA11 + "')";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO TWTOOL (`TOOL_IDX`, `TOOL_WORK`, `WK_QTY`, `TOT_WK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ((SELECT ttoolmaster2.`TOOLMASTER_IDX`  FROM ttoolmaster2 WHERE ttoolmaster2.`TOOL_IDX` = (SELECT TTOOLMASTER.`TOOL_IDX` FROM TTOOLMASTER WHERE TTOOLMASTER.`APPLICATOR`  = '" + VLA1 + "') AND ttoolmaster2.SEQ = '" + VLA11 + "'), '" + C11_D01 + "', " + C11_COUNT3 + ", " + A_CONT + ", NOW(), '" + C_USER + "')";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO TWWKAR_LP (`PART_IDX`, `WK_QTY`, `CREATE_DTM`, `CREATE_USER`, `MC_NO`, `TORDER_IDX`, `WK_TERM`) VALUES ('" + C11_D01 + "', " + C11_COUNT3 + ", NOW(), '" + C_USER + "', '" + C11_D02 + "', '" + ORDER + "', '" + VLT1 + "')";
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
    }
}

