namespace MESService.Implement
{
    public class C11_APPLICATIONService : BaseService<torderlist, ItorderlistRepository>
    , IC11_APPLICATIONService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C11_APPLICATIONService(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string USER_ID = BaseParameter.USER_ID;
                        var AAA = BaseParameter.ListSearchString[0];
                        var BBB = BaseParameter.ListSearchString[1];
                        string sql = @"SELECT ttoolmaster2.TOOLMASTER_IDX, (SELECT TTOOLMASTER.APPLICATOR FROM TTOOLMASTER WHERE TTOOLMASTER.TOOL_IDX = ttoolmaster2.TOOL_IDX) AS `APP_NAME`, ttoolmaster2.SEQ, ttoolmaster2.WK_CNT FROM ttoolmaster2 HAVING   `APP_NAME` = '" + AAA + "' AND `SEQ` = '" + BBB + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string USER_ID = BaseParameter.USER_ID;
                        var Label3 = BaseParameter.ListSearchString[0];
                        var Label10 = BaseParameter.ListSearchString[1];
                        var ORDER_IDX = BaseParameter.ListSearchString[2];
                        if (Label3 == "APPLICATION #R")
                        {
                            string sql = @"UPDATE `torder_bom_sw` SET `T1_TOOL_IDX`='" + Label10 + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "' WHERE  `ORDER_IDX`= '" + ORDER_IDX + "' AND `LOC_LRJ` = 'R'";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        if (Label3 == "APPLICATION #L")
                        {
                            string sql = @"UPDATE `torder_bom_sw` SET `T1_TOOL_IDX`='" + Label10 + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "' WHERE  `ORDER_IDX`= '" + ORDER_IDX + "' AND `LOC_LRJ` = 'L'";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        if (Label3 == "APPLICATION #J")
                        {
                            string sql = @"UPDATE `torder_bom_sw` SET `ERROR_CHK`='Y', `T1_TOOL_IDX`='" + Label10 + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "' WHERE  `ORDER_IDX`= '" + ORDER_IDX + "' AND `LOC_LRJ` = 'J'";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        if (Label3 == "APPLICATION #J2")
                        {
                            string sql = @"UPDATE `torder_bom_sw` SET `ERROR_CHK`='Y', `T1_TOOL_IDX`='" + Label10 + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "' WHERE  `ORDER_IDX`= '" + ORDER_IDX + "' AND `LOC_LRJ` = 'J2'";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

