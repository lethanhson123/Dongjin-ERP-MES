namespace MESService.Implement
{
    public class C11_SPC_LService : BaseService<torderlist, ItorderlistRepository>
    , IC11_SPC_LService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C11_SPC_LService(ItorderlistRepository torderlistRepository
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
                        var TextBox1 = BaseParameter.ListSearchString[0];
                        var TextBox2 = BaseParameter.ListSearchString[1];
                        var TextBox3 = BaseParameter.ListSearchString[2];
                        var TextBox4 = BaseParameter.ListSearchString[3];
                        var TextBox10 = BaseParameter.ListSearchString[4];
                        var Label6 = BaseParameter.ListSearchString[5];
                        var Label10 = BaseParameter.ListSearchString[6];
                        var MRUS = BaseParameter.ListSearchString[7];
                        var OR_IDX = BaseParameter.ListSearchString[8];
                        var MC2 = BaseParameter.ListSearchString[9];
                        var STRENGTH = BaseParameter.ListSearchString[10];
                        if (STRENGTH == null)
                        {
                            STRENGTH = "0";
                        }

                        string sql = @"INSERT INTO `torderinspection_sw` (`ORDER_IDX`, `CCH`, `CCW`, `ICH`, `ICW`, `WIRE_FORCE`, `IN_RESILT`, `COLSIP`, `LOC_LRJ`,`CREATE_DTM`,`CREATE_USER`,`MC2`,`STRENGTH`) VALUES ('" + OR_IDX + "', " + TextBox1 + ", " + TextBox2 + ",  " + TextBox3 + ", " + TextBox4 + ", " + TextBox10 + ", '" + MRUS + "', '" + Label10 + "', '" + Label6 + "', NOW(),'" + USER_ID + "','" + MC2 + "', "+ STRENGTH + ") ON DUPLICATE KEY UPDATE `CCH`= VALUES(`CCH`), `CCW`= VALUES(`CCW`), `ICH`= VALUES(`ICH`), `ICW`= VALUES(`ICW`), `WIRE_FORCE`= VALUES(`WIRE_FORCE`), `IN_RESILT`= VALUES(`IN_RESILT`)";
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

