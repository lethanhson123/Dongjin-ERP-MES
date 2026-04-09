namespace MESService.Implement
{
    public class B07_3Service : BaseService<torderlist, ItorderlistRepository>
    , IB07_3Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B07_3Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
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
                            result.DataGridView1 = BaseParameter.DataGridView1;
                            result.TB_PART_CNT = GlobalHelper.InitializationNumber;
                            result.RAW_PART_CNT = GlobalHelper.InitializationNumber;
                            string sql = @"SELECT `TC_PART_NM` FROM TTC_PART";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_B07_31 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_B07_31.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            for (int i = 0; i < result.DataGridView1.Count; i++)
                            {
                                string TB_PART = result.DataGridView1[i].Tube_Cutting_Part_No;
                                result.DataGridView1[i].CHK = false;
                                result.DataGridView1[i].LOAD = "STAY";
                                for (int j = 0; j < result.DGV_B07_31.Count; j++)
                                {
                                    if (TB_PART == result.DGV_B07_31[j].TC_PART_NM)
                                    {
                                        result.DataGridView1[i].LOAD = "TB_PART_ERROR";
                                        result.DataGridView1[i].CHK = false;
                                        result.TB_PART_CNT = result.TB_PART_CNT + 1;
                                    }
                                }

                            }

                            sql = @"SELECT  `PART_NO`  FROM tspart   WHERE   `PART_SCN` ='5'";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_B07_32 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_B07_32.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            for (int i = 0; i < result.DataGridView1.Count; i++)
                            {
                                if (result.DataGridView1[i].LOAD == "STAY")
                                {
                                    string RAW_PART = result.DataGridView1[i].Raw_Material_Part;
                                    bool RAW_TF = true;
                                    for (int j = 0; j < result.DGV_B07_32.Count; j++)
                                    {
                                        if (RAW_PART == result.DGV_B07_32[j].PART_NO)
                                        {
                                            result.DataGridView1[i].CHK = true;
                                            if (result.DataGridView1[i].LOAD == "STAY")
                                            {
                                                result.DataGridView1[i].LOAD = "OK";
                                                result.DataGridView1[i].CHK = false;
                                            }
                                        }
                                        else
                                        {
                                            RAW_TF = false;
                                        }
                                    }
                                    if (RAW_TF == false)
                                    {
                                        if (result.DataGridView1[i].LOAD == "TB_PART_ERROR")
                                        {
                                            result.DataGridView1[i].LOAD = "BOTH_ERROR";
                                        }
                                        else
                                        {
                                            result.DataGridView1[i].LOAD = "RAW_PART_ERROR";
                                        }
                                        result.DataGridView1[i].CHK = false;
                                        result.TB_PART_CNT = result.TB_PART_CNT + 1;
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
                        string SUM_DATA = GlobalHelper.InitializationString;
                        string VAL_DATA = GlobalHelper.InitializationString;
                        foreach (var item in BaseParameter.DataGridView1)
                        {
                            if (item.CHK == true)
                            {
                                string AAA = item.Tube_Cutting_Part_No;
                                string BBB = item.Description;
                                string CCC = item.Raw_Material_Part;
                                string DDD = item.Size;
                                string EEE = item.Machine;
                                string FFF = item.Packing_Unit;
                                string GGG = item.Location;

                                VAL_DATA = "('" + AAA + "', '" + BBB + "', (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = '" + CCC + "'), '" + DDD + "', '" + EEE + "', '" + FFF + "', '" + GGG + "', NOW(), '" + USER_IDX + "')";
                                if (SUM_DATA.Length <= 0)
                                {
                                    SUM_DATA = VAL_DATA;
                                }
                                else
                                {
                                    SUM_DATA = SUM_DATA + ", " + VAL_DATA;
                                }
                            }
                        }
                        string sql = @"INSERT INTO `TTC_PART` (`TC_PART_NM`, `TC_DESC`, `RAW_PART_IDX`, `TC_SIZE`, `TC_MC`, `TC_PACKUNIT`, `TC_LOC`, `CREATE_DTM`, `CREATE_USER`) VALUES " + SUM_DATA;
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `TTC_PART`     AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

