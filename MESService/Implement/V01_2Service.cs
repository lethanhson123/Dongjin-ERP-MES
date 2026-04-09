namespace MESService.Implement
{
    public class V01_2Service : BaseService<torderlist, ItorderlistRepository>
    , IV01_2Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_2Service(ItorderlistRepository torderlistRepository
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
                    var AAA = BaseParameter.SearchString;
                    AAA = "%" + AAA + "%";
                    string sql = @"SELECT `D`.`CMPNY_IDX`, `D`.`CMPNY_NM`,`D`.`CMPNY_DVS`,`D`.`CMPNY_NO`, 
                    IFNULL(`D`.`CMPNY_ADDR`, '') AS `CMPNY_ADDR`,
                    IFNULL(`D`.`CMPNY_TEL`, '') AS `CMPNY_TEL`,
                    IFNULL(`D`.`CMPNY_FAX`, '') AS `CMPNY_FAX`,
                    IFNULL(`D`.`CMPNY_MNGR`,  '') AS `CMPNY_MNGR`,
                    `D`.`CMPNY_RMK` FROM PDCMPNY `D`
                    WHERE `D`.`CMPNY_NM` LIKE '" + AAA + "' OR `D`.`CMPNY_ADDR` LIKE '" + AAA + "' OR `D`.`CMPNY_MNGR`  LIKE '" + AAA + "' ";
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

