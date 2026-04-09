

namespace MESService.Implement
{
    public class C09_COUNTService : BaseService<torderlist, ItorderlistRepository>
    , IC09_COUNTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C09_COUNTService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C09_COUNT_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT DATE_FORMAT(`PO_DT`,'%Y-%m-%d') AS `DATE`, COUNT(`CONDITION`) AS `SUM`, COUNT(IF(`CONDITION`='Stay', `CONDITION` , NULL)) AS `Stay`,COUNT(IF(`CONDITION`='Working', `CONDITION` , NULL)) AS `Working`,COUNT(IF(`CONDITION`='Complete', `CONDITION` , NULL)) AS `Complete`,COUNT(IF(`CONDITION`='Close', `CONDITION` , NULL)) AS `Close`
                FROM   TORDERLIST_SPST   WHERE `PO_DT` > DATE_FORMAT(date_add(now(), INTERVAL -8 DAY),'%Y-%m-%d') GROUP BY `PO_DT`";

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
        public virtual async Task<BaseResult> RadioButton2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                if (BaseParameter != null)
                {
                    string sql = @"";
                    if (BaseParameter.RadioButton1==true)
                    {
                        sql = @"SELECT DATE_FORMAT(`PO_DT`,'%Y-%m-%d') AS `DATE`, COUNT(`CONDITION`) AS `SUM`, COUNT(IF(`CONDITION`='Stay', `CONDITION` , NULL)) AS `Stay`,COUNT(IF(`CONDITION`='Working', `CONDITION` , NULL)) AS `Working`,COUNT(IF(`CONDITION`='Complete', `CONDITION` , NULL)) AS `Complete`,COUNT(IF(`CONDITION`='Close', `CONDITION` , NULL)) AS `Close`
                        FROM   TORDERLIST_SPST   WHERE `PO_DT` > DATE_FORMAT(date_add(now(), INTERVAL -8 DAY),'%Y-%m-%d') GROUP BY `PO_DT`";
                    }
                    if (BaseParameter.RadioButton2 == true)
                    {
                        sql = @"SELECT  `MC`, DATE_FORMAT(`PO_DT`,'%Y-%m-%d') AS `DATE`, COUNT(`CONDITION`) AS `SUM`, COUNT(IF(`CONDITION`='Stay', `CONDITION` , NULL)) AS `Stay`,COUNT(IF(`CONDITION`='Working', `CONDITION` , NULL)) AS `Working`,COUNT(IF(`CONDITION`='Complete', `CONDITION` , NULL)) AS `Complete`,COUNT(IF(`CONDITION`='Close', `CONDITION` , NULL)) AS `Close`
                        FROM   TORDERLIST_SPST   WHERE `PO_DT` > DATE_FORMAT(date_add(now(), INTERVAL -8 DAY),'%Y-%m-%d') GROUP BY `MC`, `PO_DT`";
                    }

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

