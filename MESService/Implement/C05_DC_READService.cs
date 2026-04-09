
namespace MESService.Implement
{
    public class C05_DC_READService : BaseService<torderlist, ItorderlistRepository>
    , IC05_DC_READService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C05_DC_READService(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> TB_BARCODE_KeyDown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var BARNM = BaseParameter.SearchString;
                    var TB_BARCODE = BaseParameter.SearchString;
                    string sqlResult = "";
                    string sql = @"SELECT  `TORDER_BARCODE_IDX`, `TORDER_BARCODENM`, `ORDER_IDX` FROM torder_barcode_lp WHERE `TORDER_BARCODENM` = '" + BARNM + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    if (result.Search.Count <= 0)
                    {
                        sql = @"INSERT INTO torder_barcode_lp (`TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, `TORDER_BC_PRNT`, `TORDER_BC_WORK`, `DSCN_YN`,`CREATE_DTM`, `CREATE_USER`, `WORK_START`) SELECT `TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, 'Y', 'Y', 'N', NOW(), '" + USER_ID + "', NOW() FROM TORDER_BARCODE WHERE `TORDER_BARCODENM` = '" + BARNM + "' AND TORDER_BARCODE.DSCN_YN = 'Y' ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torder_barcode_lp`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }

                    var LIST_CHK = true;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (TB_BARCODE == item.TORDER_BARCODENM)
                                {
                                    LIST_CHK = false;
                                }
                            }
                        }
                    }
                    result.Search1 = new List<SuperResultTranfer>();
                    if (LIST_CHK == true)
                    {
                        sql = @"SELECT torder_barcode_lp.`TORDER_BARCODE_IDX`, torder_barcode_lp.`ORDER_IDX`, torder_barcode_lp.`DSCN_YN`, TORDERLIST.`TERM1`, TORDERLIST.`TERM2`, TORDERLIST.`LEAD_NO`,
                        (SELECT `CONDITION` FROM  TORDERLIST_LP WHERE TORDERLIST_LP.ORDER_IDX = torder_barcode_lp.`ORDER_IDX`) AS `DC_CHK`,
                        (SELECT `CONDITION` FROM  TORDERLIST_LP WHERE TORDERLIST_LP.ORDER_IDX = torder_barcode_lp.`ORDER_IDX`) AS `DC_CHK1`
                        FROM torder_barcode_lp JOIN TORDERLIST ON torder_barcode_lp.`ORDER_IDX` = TORDERLIST.`ORDER_IDX`  
                        WHERE `TORDER_BARCODENM` = '" + TB_BARCODE + "'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> BARCODE_LOAD2(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        result.DataGridView = new List<SuperResultTranfer>();
                        bool IsCheck = true;
                        var BARCODE_TEXT = BaseParameter.ListSearchString[0];
                        var Label2 = BaseParameter.ListSearchString[1];
                        string sqlResult = "";
                        string sql = @"SELECT torder_barcode_lp.`TORDER_BARCODE_IDX`, torder_barcode_lp.`ORDER_IDX`, torder_barcode_lp.`DSCN_YN`, TORDERLIST_LP.`PERFORMN_L`, TORDERLIST_LP.`PERFORMN_R`
                        FROM torder_barcode_lp JOIN TORDERLIST_LP ON torder_barcode_lp.`ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX` 
                        WHERE torder_barcode_lp.`TORDER_BARCODENM` = '" + BARCODE_TEXT + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.DataGridView1.Count <= 0)
                        {
                            IsCheck = false;
                        }
                        if (IsCheck == true)
                        {                            
                            if (result.DataGridView1[0].DSCN_YN == "Y")
                            {
                                IsCheck = false;
                            }
                        }
                        if (IsCheck == true)
                        {
                            var ORDER_NO = result.DataGridView1[0].ORDER_IDX;
                            sql = @"Update TORDERLIST_LP SET  `PROJECT` = 'DC',  `CONDITION` = 'Working' , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = " + ORDER_NO + ")";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            var Tcount = BARCODE_TEXT;
                            if (Label2 == "LH")
                            {
                                var SAA = result.DataGridView1[0].TORDER_BARCODE_IDX;

                                sql = @"UPDATE `torder_barcode_lp` SET `TORDER_BC_PRNT`='L', `WORK_END`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                //var AAT = Tcount.Substring(Tcount.IndexOf("$$") + 2, 12);
                                //var WORK_QTY = int.Parse(AAT.Substring(0, AAT.IndexOf("$$") - 1));
                                var WORK_QTY = int.Parse(Tcount.Split("$$")[1]);
                                var bbt_sum = result.DataGridView1[0].PERFORMN_L + WORK_QTY;

                                sql = @"UPDATE `TORDERLIST_LP` SET `PERFORMN_L`='" + bbt_sum + "' WHERE `ORDER_IDX`=" + ORDER_NO;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }

                            if (Label2 == "RH")
                            {
                                var SAA = result.DataGridView1[0].TORDER_BARCODE_IDX;

                                sql = @"UPDATE `torder_barcode_lp` SET `TORDER_BC_WORK`='R',`WORK_END`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                //var AAT = Tcount.Substring(Tcount.IndexOf("$$") + 2, 12);
                                //var WORK_QTY = int.Parse(AAT.Substring(0, AAT.IndexOf("$$") - 1));
                                var WORK_QTY = int.Parse(Tcount.Split("$$")[1]);
                                var bbt_sum = result.DataGridView1[0].PERFORMN_R + WORK_QTY;

                                sql = @"UPDATE `TORDERLIST_LP` SET `PERFORMN_R`='" + bbt_sum + "' WHERE `ORDER_IDX`=" + ORDER_NO;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT `TORDER_BARCODENM`, `WORK_END` FROM torder_barcode_lp WHERE `DSCN_YN` = 'N' AND `ORDER_IDX` = '" + ORDER_NO + "'";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView2 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }

                                var COM_COUNT = result.DataGridView2.Count;
                                if (COM_COUNT == 0)
                                {
                                    IsCheck = false;
                                    sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = '" + ORDER_NO + "')";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }                          

                            if (IsCheck == true)
                            {
                                sql = @"UPDATE `torder_barcode_lp` SET `DSCN_YN`= IF(`TORDER_BC_PRNT` ='L' AND `TORDER_BC_WORK`= 'R', 'Y', 'N'), `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT `TORDER_BARCODENM`, `WORK_END` FROM torder_barcode_lp WHERE `DSCN_YN` = 'N' AND `ORDER_IDX` = '" + ORDER_NO + "'";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                var COM_COUNT = result.DataGridView.Count;
                                if (COM_COUNT == 0)
                                {
                                    sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = '" + ORDER_NO + "')";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
    }
}

