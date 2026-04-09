namespace MESService.Implement
{
    public class V01_1Service : BaseService<torderlist, ItorderlistRepository>
    , IV01_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_1Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.ListSearchString != null)
                    {
                        string Label3 = BaseParameter.ListSearchString[0];                      
                        string sql = "UPDATE `pdpart`   SET   `PN_PHOTO` = '" + Label3 + ".jpg" + "' WHERE  `PN_NM` = '" + Label3 + "'  ";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

