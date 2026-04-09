namespace MESService.Implement
{
    public class Z04_1Service : BaseService<torderlist, ItorderlistRepository>
    , IZ04_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z04_1Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
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
                    string AAA = "%" + BaseParameter.SearchString + "%";

                    string sql = @"SELECT `PART_IDX`, `PART_NO`, `PART_NM`, `TNM`  FROM (
                    SELECT  `PART_NO`, `PART_NM`, `PART_IDX`, 'tspart' AS `TNM` FROM tspart WHERE `PART_USENY` = 'Y'
                    UNION
                    SELECT  `LEAD_PN` AS `PART_NO`, 'LEAD' AS `PART_NM`, `LEAD_INDEX` AS `PART_IDX`, 'torder_lead_bom' AS `TNM` FROM torder_lead_bom) `MM`
                    WHERE `MM`.`PART_NO` LIKE'" + AAA + "'  ";

                    sql = sql + " LIMIT " + GlobalHelper.ListCount;

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

