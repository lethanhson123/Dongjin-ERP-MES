namespace MESService.Implement
{
    public class D04_RANKService : BaseService<torderlist, ItorderlistRepository>
    , ID04_RANKService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_RANKService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> SELECT_SAVE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var Label5= BaseParameter.SearchString;
                    var CHK_SV = BaseParameter.CHK_SV;
                    string sql = @"UPDATE `tdpdotpl` SET `SORTNO` = '" + CHK_SV + " ' WHERE  `PDOTPL_IDX`= '" + Label5 + "'  ";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

