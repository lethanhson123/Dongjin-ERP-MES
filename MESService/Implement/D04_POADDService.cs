namespace MESService.Implement
{
    public class D04_POADDService : BaseService<torderlist, ItorderlistRepository>
    , ID04_POADDService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_POADDService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> TextBoxA2_KeyDown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        result.PART_IDX = BaseParameter.ListSearchString[0].ToUpper();
                        var PO_CODE = BaseParameter.ListSearchString[1];
                        if (BaseParameter.CheckBox1 == true)
                        {
                            var chk_P = result.PART_IDX.Substring(0, 1);
                            if (chk_P == "P")
                            {
                                result.PART_IDX = result.PART_IDX.Substring(1, result.PART_IDX.Length - 1);
                            }
                            else
                            {
                                result.PART_IDX = result.PART_IDX;
                            }
                        }
                        string sql = @"SELECT `PART_IDX`, `PART_SNP` FROM tspart WHERE PART_NO ='" + result.PART_IDX + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count > 0)
                        {
                            var PART_IDXK = result.Search[0].PART_IDX;
                            sql = @"SELECT  tdpdotpl.PDOTPL_IDX, tdpdotpl.PO_CODE, tdpdotpl.PART_IDX, tdpdotpl.PO_QTY, tdpdotpl.PACK_QTY FROM tdpdotpl WHERE tdpdotpl.PO_CODE = '" + PO_CODE + "'  AND  tdpdotpl.PART_IDX = '" + PART_IDXK + "'";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_D04_PO01 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_D04_PO01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var PO_CODE = BaseParameter.ListSearchString[0];
                        var PO_QTY = BaseParameter.ListSearchString[1];
                        var PART_IDXK = BaseParameter.ListSearchString[2];
                        var PART_SNP = BaseParameter.ListSearchString[3];
                        var TextBoxA0 = BaseParameter.ListSearchString[4];

                        if (TextBoxA0 == "NEW")
                        {
                            string sql = @"INSERT `tdpdotpl` (`PO_CODE`, `PART_IDX`, `PO_QTY`, `PART_IDX_SNP`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + PO_CODE + "', '" + PART_IDXK + "', '" + PO_QTY + "', '" + PART_SNP + "', NOW(), '" + USER_ID + "');";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        else
                        {
                            string sql = @"UPDATE tdpdotpl SET tdpdotpl.PO_QTY = '" + PO_QTY + "' WHERE tdpdotpl.PO_CODE = '" + PO_CODE + "' AND tdpdotpl.PART_IDX = '" + PART_IDXK + " '";
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

