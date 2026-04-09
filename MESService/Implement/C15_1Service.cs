

namespace MESService.Implement
{
    public class C15_1Service : BaseService<torderlist, ItorderlistRepository>
    , IC15_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C15_1Service(ItorderlistRepository torderlistRepository
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
                if (BaseParameter.ListSearchString != null)
                {
                    string TextBox1 = BaseParameter.ListSearchString[0];


                    string sql = @"SELECT torder_lead_bom.LEAD_PN, torder_lead_bom.BUNDLE_SIZE FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN LIKE  '%" + TextBox1 + "%' AND torder_lead_bom.DSCN_YN = 'Y'";

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

