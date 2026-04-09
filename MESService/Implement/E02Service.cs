namespace MESService.Implement
{
    public class E02Service : BaseService<torderlist, ItorderlistRepository>
    , IE02Service
    {
    private readonly ItorderlistRepository _torderlistRepository;
    public E02Service(ItorderlistRepository torderlistRepository

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
        public async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT ttoolmaster2.`TOOLMASTER_IDX` AS `CODE`, TTOOLMASTER.`APPLICATOR`, 
            ttoolmaster2.`SEQ` AS `SEQ`, TTOOLMASTER.`MAX_CNT` AS `MAX_COUNT`, 
            ttoolmaster2.`TOT_WK_CNT` AS `TOTAL_COUNT`, ttoolmaster2.`WK_CNT` AS `WORK_COUNT`, 
            IF(TTOOLMASTER.`MAX_CNT` - ttoolmaster2.`WK_CNT` < 0, 0, TTOOLMASTER.`MAX_CNT` - ttoolmaster2.`WK_CNT`) AS `FREE_COUNT`, 
            IF(TTOOLMASTER.`MAX_CNT` - ttoolmaster2.`WK_CNT` > 0, 0, ttoolmaster2.`WK_CNT` - TTOOLMASTER.`MAX_CNT`) AS `OVER_COUNT`,
            (ttoolmaster2.`WK_CNT` / TTOOLMASTER.`MAX_CNT` * 100) AS `RAT`
            FROM TTOOLMASTER, ttoolmaster2 
            WHERE TTOOLMASTER.`TOOL_IDX` = ttoolmaster2.`TOOL_IDX`
            ORDER BY `RAT` DESC, `OVER_COUNT` DESC";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView1 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds.Tables)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
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
    public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
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
     public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
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
    public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
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
  }
}

