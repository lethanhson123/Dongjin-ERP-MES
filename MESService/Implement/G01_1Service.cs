

namespace MESService.Implement
{
    public class G01_1Service : BaseService<torderlist, ItorderlistRepository>, IG01_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public G01_1Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment)
            : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }

        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        var DGV_CNT = BaseParameter.DataGridView1.Count;
                        if (DGV_CNT > 0)
                        {
                            var II = 0;
                            var JJ = BaseParameter.DataGridView1.Count;
                            var SUM_VAL = "";
                            var VAL = "";
                            var SCN_CODE = 0;
                            var LOC_INDEX = BaseParameter.SearchString;
                            if (LOC_INDEX == "1")
                            {
                                SCN_CODE = 1;
                            }
                            if (LOC_INDEX == "2")
                            {
                                SCN_CODE = 2;
                            }

                            for (II = 0; II < JJ; II++)
                            {
                                var Description = BaseParameter.DataGridView1[II].Description;
                                if (Description == "OK")
                                {
                                    var P_01 = BaseParameter.DataGridView1[II].PART_NO;
                                    var P_02 = BaseParameter.DataGridView1[II].QTY;
                                    VAL = "('" + P_01 + "', '" + SCN_CODE + "', '" + P_02 + "', NOW(), '" + USER_IDX + "')";
                                    if (SUM_VAL.Length <= 0)
                                    {
                                        SUM_VAL = VAL;
                                    }
                                    else
                                    {
                                        SUM_VAL = SUM_VAL + ", " + VAL;
                                    }
                                }
                            }

                            string sql = @"UPDATE tiivtr_EXCEL  SET  tiivtr_EXCEL.`QTY` ='0' WHERE tiivtr_EXCEL.LOC_IDX = '" + SCN_CODE + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO `tiivtr_EXCEL` (`PART_NO`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES   " + SUM_VAL + " ON DUPLICATE KEY UPDATE `QTY` = VALUES(`QTY`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tiivtr_EXCEL`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tiivtr_EXCEL`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView1 != null)
                    {
                        result = await DB_DGV(BaseParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DB_DGV(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView1 != null)
                    {
                        string LOC_INDEX = BaseParameter.SearchString;
                        int SCN_CODE = 0;
                        if (LOC_INDEX == "1")
                        {
                            SCN_CODE = 5;
                        }
                        if (LOC_INDEX == "2")
                        {
                            SCN_CODE = 6;
                        }
                        string sql = @"SELECT  tspart.`PART_NO`  FROM  tspart  WHERE  tspart.`PART_SCN` = '" + SCN_CODE + "' AND  tspart.`PART_USENY` ='Y'      ";

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                        var PARTNO_MASTER = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            PARTNO_MASTER.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        var DB_II = 0;
                        var DB_JJ = PARTNO_MASTER.Count;

                        var ii = 0;
                        var jj = BaseParameter.DataGridView1.Count;

                        result.QTY_SUM = 0;
                        result.CHK_PM = 0;
                        for (ii = 0; ii < jj; ii++)
                        {
                            result.QTY_SUM = result.QTY_SUM + BaseParameter.DataGridView1[ii].QTY.Value;
                            for (DB_II = 0; DB_II < DB_JJ; DB_II++)
                            {
                                var DB_PN = PARTNO_MASTER[DB_II].PART_NO;
                                var DGV_PN = BaseParameter.DataGridView1[ii].PART_NO;
                                if (DB_PN == DGV_PN)
                                {
                                    BaseParameter.DataGridView1[ii].Description = "OK";
                                    result.CHK_PM = result.CHK_PM + 1;
                                    DB_II = DB_JJ;
                                }
                            }
                        }
                        result.DataGridView1 = BaseParameter.DataGridView1;
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