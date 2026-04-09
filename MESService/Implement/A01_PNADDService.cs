namespace MESService.Implement
{
    public class A01_PNADDService : BaseService<torderlist, ItorderlistRepository>
    , IA01_PNADDService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01_PNADDService(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();

            string sql = @"SELECT  `CD_IDX`, `CD_NM_HAN`, `CD_NM_EN`, `CDGR_IDX`, `CD_SYS_NOTE` FROM   TSCODE  WHERE  (`CDGR_IDX` = 4)";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ComboBox1 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            var ComboBox1 = BaseParameter.SearchString;
                            var CREATE_USER = BaseParameter.USER_ID;
                            var UPDATE_USER = BaseParameter.USER_ID;
                            foreach (SuperResultTranfer item in BaseParameter.DataGridView1)
                            {
                                var AAA = item.PART_NO == null ? "" : item.PART_NO.Trim();
                                var BBB = item.PART_NAME == null ? "" : item.PART_NAME.Trim();
                                BBB = BBB.Replace(",", " ");
                                BBB = BBB.Replace("'", "");
                                var CCC = item.MODEL == null ? "" : item.MODEL.Trim();
                                var DDD = item.PART_FamilyPC == null ? "" : item.PART_FamilyPC.Trim();
                                var EEE = item.Packing_Unit == null ? "0" : item.Packing_Unit.Trim();
                                var FFF = ComboBox1 == null ? "0" : ComboBox1;
                                var GGG = item.Location == null ? "" : item.Location.Trim();
                                var HHH = item.BOM_GROUP == null ? "" : item.BOM_GROUP.Trim();
                                var MMM = item.PART_SUPL == null ? "" : item.PART_SUPL.Trim();
                                var LOC_IDX_CODE = 0;
                                if (AAA == "")
                                {

                                }
                                else
                                {
                                    if (FFF == "5")
                                    {
                                        LOC_IDX_CODE = 1;
                                    }
                                    if (FFF == "6")
                                    {
                                        LOC_IDX_CODE = 1;
                                    }

                                    string sql = @"INSERT INTO tspart (`PART_NO`, `PART_NM`, `PART_CAR`, `PART_FML`, `PART_SNP`, `PART_SCN`, `PART_LOC`, `BOM_GRP`, `PART_SUPL`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                                    ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', " + EEE + ", " + FFF + ", '" + GGG + "', '" + HHH + "', '" + MMM + "', NOW(), '" + CREATE_USER + "') ON DUPLICATE KEY UPDATE `PART_NM`= '" + BBB + "', `PART_CAR`= '" + CCC + "', `PART_FML`= '" + DDD + "', `PART_SNP`= " + EEE + ", `PART_SCN`= " + FFF + ", `PART_LOC`= '" + GGG + "', `BOM_GRP`= '" + HHH + "', `PART_SUPL`= '" + MMM + "', `UPDATE_DTM` = NOW(), `UPDATE_USER`= '" + UPDATE_USER + "'";
                                    string sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"ALTER TABLE     `tspart`     AUTO_INCREMENT= 1";
                                    sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"SELECT tiivtr.`PART_IDX`, tspart.`PART_NO`, tiivtr.`LOC_IDX` FROM tspart,tiivtr WHERE tiivtr.`PART_IDX` = tspart.`PART_IDX` AND tspart.`PART_NO` = ' " + AAA + "' AND tiivtr.`LOC_IDX`='" + FFF + "'";
                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    var DB_GROUP_CODE = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        DB_GROUP_CODE.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    if (DB_GROUP_CODE.Count <= 0)
                                    {
                                        sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + AAA + "'), '" + LOC_IDX_CODE + "', 0, NOW(), '" + CREATE_USER + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
    }
}

