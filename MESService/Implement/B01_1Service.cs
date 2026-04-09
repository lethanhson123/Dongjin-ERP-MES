namespace MESService.Implement
{
    public class B01_1Service : BaseService<torderlist, ItorderlistRepository>
    , IB01_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public B01_1Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
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
                string AAA = "%" + BaseParameter.SearchString + "%";

                string sql = @"SELECT  `PART_IDX`, `PART_NO`, `PART_NM`, `PART_SNP`  FROM tspart  WHERE  `PART_NO` LIKE '" + AAA + "' AND `PART_SCN` = 5  AND `PART_USENY` = 'Y'   ";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.Listtspart = new List<tspart>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.Listtspart.AddRange(SQLHelper.ToList<tspart>(dt));
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

