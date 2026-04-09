namespace MESService.Implement
{
    public class SETUP_FORMService : BaseService<torderlist, ItorderlistRepository>
    , ISETUP_FORMService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public SETUP_FORMService(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> DB_MC_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string MC_LIST_GRP = BaseParameter.SearchString;
                    string WHERE_CHK = "";
                    switch (MC_LIST_GRP)
                    {
                        case "A_01":
                            break;
                        case "A_02":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'ADMIN%'";
                            break;
                        case "B_01":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8' AND TSCODE.`CD_SYS_NOTE` LIKE 'A8%'";
                            break;
                        case "B_02":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'LP%'";
                            break;
                        case "B_03":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '10'";
                            break;
                        case "B_04":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'Z1%' AND  CAST(RIGHT(`CD_NM_HAN`, 3) AS UNSIGNED) <  500  ";
                            break;
                        case "B_05":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'S1%' AND  CAST(RIGHT(`CD_NM_HAN`, 3) AS UNSIGNED) <  500  ";
                            break;
                        case "C_01":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'ZA%'";
                            break;
                        case "C_02":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'ZB%'";
                            break;
                        case "C_03":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'ZZ%' AND  CAST(RIGHT(`CD_NM_HAN`, 3) AS UNSIGNED) >  500  ";
                            break;
                        case "C_04":
                            WHERE_CHK = " TSCODE.CDGR_IDX = '8'  AND TSCODE.`CD_SYS_NOTE` LIKE 'ZS%' ";
                            break;
                    }

                    string sql = @"SELECT TSCODE.CD_NM_EN FROM TSCODE WHERE " + WHERE_CHK;

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
    }
}

