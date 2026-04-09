

namespace MESService.Implement
{
    public class C05_SPCService : BaseService<torderlist, ItorderlistRepository>
    , IC05_SPCService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C05_SPCService(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> C02_SPC_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var F_CO = BaseParameter.SearchString;
                    string sqlResult = "";
                    string sql = @"SELECT  FORCE_IDX, FORCE_NM, FORCE_MIN, FORCE_MAX, STRENGTH, CREATE_DTM, CREATE_USER, UPDATE_DTM, UPDATE_USER FROM  TTENSILFORCE WHERE  (FORCE_MIN <= '" + F_CO + "') AND (FORCE_MAX >= '" + F_CO + "')";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> ORDER_CHG(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var OR_IDX = BaseParameter.SearchString;
                    string sqlResult = "";
                    string sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Working' , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = " + OR_IDX + ")";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var TextBox1 = BaseParameter.ListSearchString[0] == "" ? "0" : BaseParameter.ListSearchString[0];
                        var TextBox2 = BaseParameter.ListSearchString[1] == "" ? "0" : BaseParameter.ListSearchString[1];
                        var TextBox3 = BaseParameter.ListSearchString[2] == "" ? "0" : BaseParameter.ListSearchString[2];
                        var TextBox4 = BaseParameter.ListSearchString[3] == "" ? "0" : BaseParameter.ListSearchString[3];
                        var TextBox5 = BaseParameter.ListSearchString[4] == "" ? "0" : BaseParameter.ListSearchString[4];
                        var TextBox6 = BaseParameter.ListSearchString[5] == "" ? "0" : BaseParameter.ListSearchString[5];
                        var TextBox7 = BaseParameter.ListSearchString[6] == "" ? "0" : BaseParameter.ListSearchString[6];
                        var TextBox8 = BaseParameter.ListSearchString[7] == "" ? "0" : BaseParameter.ListSearchString[7];
                        var TextBox10 = BaseParameter.ListSearchString[8] == "" ? "0" : BaseParameter.ListSearchString[8];
                        var Label8 = BaseParameter.ListSearchString[9];
                        var Label10 = BaseParameter.ListSearchString[10];
                        var Label11 = BaseParameter.ListSearchString[11];
                        var MRUS = BaseParameter.ListSearchString[12];
                        var MC2 = BaseParameter.ListSearchString[13];
                        var STRENGTH = BaseParameter.ListSearchString[14];
                        if (STRENGTH == null)
                        {
                            STRENGTH = "0";
                        }

                        string sql = @"INSERT INTO `torderinspection_lp` (`ORDER_IDX`, `CCH1`, `CCW1`, `CCH2`, `CCW2`, `ICH1`, `ICW1`, `ICH2`, `ICW2`, `WIRE_FORCE`, `IN_RESILT`, `COLSIP`, `CHK_LR`,`CREATE_DTM`,`CREATE_USER`,`MC2`,`STRENGTH`) VALUES (" + Label8 + ", " + TextBox1 + ", " + TextBox2 + ", " + TextBox5 + ", " + TextBox6 + ", " + TextBox3 + ", " + TextBox4 + ", " + TextBox7 + ", " + TextBox8 + ", " + TextBox10 + ", '" + MRUS + "', '" + Label10 + "', '" + Label11 + "', NOW(),'" + USER_ID + "','" + MC2 + "', " + STRENGTH + ") ON DUPLICATE KEY UPDATE `CCH1`= VALUES(`CCH1`), `CCW1`= VALUES(`CCW1`), `CCH2`= VALUES(`CCH2`), `CCW2`= VALUES(`CCW2`), `ICH1`= VALUES(`ICH1`), `ICW1`= VALUES(`ICW1`), `ICH2`= VALUES(`ICH2`), `ICW2`= VALUES(`ICW2`), `WIRE_FORCE`= VALUES(`WIRE_FORCE`), `IN_RESILT`= VALUES(`IN_RESILT`)";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torderinspection_lp`     AUTO_INCREMENT= 1";
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

