namespace MESService.Implement
{
    public class D04_POLISTService : BaseService<torderlist, ItorderlistRepository>
    , ID04_POLISTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_POLISTService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> D04_POLIST_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                string sql = @"SELECT tdpdotpl.PO_CODE, tdpdotpl.CREATE_DTM
                FROM tdpdotpl
                GROUP BY tdpdotpl.PO_CODE
                ORDER BY  tdpdotpl.PDOTPL_IDX DESC
                LIMIT 10";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

