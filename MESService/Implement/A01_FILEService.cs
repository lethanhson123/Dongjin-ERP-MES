namespace MESService.Implement
{
    public class A01_FILEService : BaseService<torderlist, ItorderlistRepository>
    , IA01_FILEService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01_FILEService(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.SearchString != null)
                    {
                        string PNO_TXT = BaseParameter.SearchString;


                        string sql = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ENCNO`)  AS  `NO`, 
                            tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ENCNO`, IFNULL(tspart_ecn.`DWG_NO`, '') AS `DWG_NO`, 
                            IFNULL(tspart_ecn.`DWG_FILE_GRP`, '') AS `DWG_FILE_GRP`,
                            'DOWN' AS `DN`,
                            IFNULL(tspart_ecn.`DWG_FILE_EXPOR`, '') AS `DWG_FILE_EXPOR`,
                            tspart_ecn.`PARTECN_IDX`
                            FROM tspart_ecn
                            WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' AND 
                            tspart_ecn.`PART_IDX` = (SELECT `PART_IDX` FROM tspart WHERE tspart.`PART_NO` =   '" + PNO_TXT + "') ORDER BY    tspart_ecn.`PART_ECN_DATE` DESC , tspart_ecn.`PART_ENCNO`   DESC     ";


                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                        string FN_NAME_TXT = BaseParameter.ListSearchString[0];
                        string FOLD = BaseParameter.ListSearchString[1];
                        string F_ECN_IDX = BaseParameter.ListSearchString[2];
                        string sql = @"UPDATE `tspart_ecn` SET `DWG_FILE_GRP`= '" + FN_NAME_TXT + "',  `DWG_FILE_EXPOR`= '" + FOLD + "', `PART_ECN_USENY`= 'Y', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_IDX + "' WHERE  `PARTECN_IDX`= '" + F_ECN_IDX + "'   ";

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

