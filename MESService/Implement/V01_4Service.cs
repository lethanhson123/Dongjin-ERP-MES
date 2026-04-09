namespace MESService.Implement
{
    public class V01_4Service : BaseService<torderlist, ItorderlistRepository>
    , IV01_4Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_4Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.ListSearchString != null)
                    {
                        string CMP_CODE = BaseParameter.ListSearchString[0];                        
                        int MONTH_I = int.Parse(BaseParameter.ListSearchString[1]);
                        string MONTH_SQL = "";
                        switch (MONTH_I)
                        {
                            case 0:
                                MONTH_SQL = "366";
                                break;
                            case 1:
                                MONTH_SQL = "30";
                                break;
                            case 2:
                                MONTH_SQL = "60";
                                break;
                            case 3:
                                MONTH_SQL = "90";
                                break;
                        }
                        
                        string sql = @"SELECT  PD_CMPNY_COSTFILE.`PD_CMPNY_PART_IDX`,  PD_CMPNY_COSTFILE.`CMPNY_IDX`,  PD_CMPNY_COSTFILE.`COST_DATE`,  PD_CMPNY_COSTFILE.`COST_FILENM`,  PD_CMPNY_COSTFILE.`COST_FILETYPE`,  PD_CMPNY_COSTFILE.`REMARK`, 'DownLoad' AS `FILE`  
                                        FROM PD_CMPNY_COSTFILE   WHERE PD_CMPNY_COSTFILE.`CMPNY_IDX` = '" + CMP_CODE + "' AND PD_CMPNY_COSTFILE.`DSN_YN` = 'Y'  AND TIMESTAMPDIFF(DAY, PD_CMPNY_COSTFILE.`COST_DATE`, NOW()) <= '" + MONTH_SQL + "'";

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
                        string CMP_CODE = BaseParameter.ListSearchString[0];
                        string F_DATE = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                        string Label12 = BaseParameter.ListSearchString[2];
                        string TextBox2 = BaseParameter.ListSearchString[3];

                        string FILE_NM = "";
                        string sql = @"SELECT   MAX(PD_CMPNY_COSTFILE.`CMPNY_IDX`) + 1 AS `NO`  FROM  PD_CMPNY_COSTFILE";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var DGV_V01_42 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            DGV_V01_42.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (DGV_V01_42.Count > 0)
                        {
                            string FILE_SEQ = DGV_V01_42[0].NO.ToString();
                            FILE_NM = "PUR_COST_" + CMP_CODE + "_" + FILE_SEQ;
                            sql = @"INSERT INTO PD_CMPNY_COSTFILE (`CMPNY_IDX`, `COST_DATE`, `COST_FILENM`, `COST_FILETYPE`, `REMARK`, `DSN_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + CMP_CODE + "', '" + F_DATE + "', '" + FILE_NM + "', '" + Label12 + "', '" + TextBox2 + "', 'Y', NOW(), '" + USER_IDX + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }

                        string NEW_FILE = FILE_NM + Label12;
                        FileInfo fileLocation = new FileInfo(BaseParameter.FilePath);
                        BaseParameter.FilePath = Path.Combine(BaseParameter.PhysicalPath, NEW_FILE);
                        fileLocation.MoveTo(BaseParameter.FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var DGV_CODE = BaseParameter.SearchString;
                    string sql = @"UPDATE `PD_CMPNY_COSTFILE` SET `DSN_YN` = 'N' WHERE  `PD_CMPNY_PART_IDX` = '" + DGV_CODE + "'";
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

