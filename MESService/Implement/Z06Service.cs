namespace MESService.Implement
{
    public class Z06Service : BaseService<torderlist, ItorderlistRepository>
    , IZ06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z06Service(ItorderlistRepository torderlistRepository

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
               
                BaseParameter param = new BaseParameter();
                var longTermResult = await LONG_TERM(param);

                
                result.DataGridView8 = longTermResult.DataGridView8;

            
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> LONG_TERM(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
            
                string query = @"SELECT 
    IFNULL(`TB`.`YEAR`, 9999) AS `YEAR`,
    IFNULL(`TB`.`MONTH`, 'SUM') AS `MONTH`,
    `TB`.`LEAD_COUNT`,
    `TB`.`ADD_1`,
    `TB`.`ADD_2`,
    `TB`.`ADD_3`,
    `TB`.`ADD_4`,
    `TB`.`ADD_5`,
    `TB`.`ADD_6`,
    `TB`.`OVER_9`,
    `TB`.`OVER_10`,
    `TB`.`SUM`,
    IFNULL(`TB`.`MONTH`, 13) AS `CNO` 
    FROM (
    SELECT 
    `MAIN`.`YEAR`,
    `MAIN`.`MONTH`,
    COUNT(`MAIN`.LEAD_NM) AS `LEAD_COUNT`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 1 THEN MAIN.`SUM` END), 0) AS `ADD_1`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 2 THEN MAIN.`SUM` END), 0) AS `ADD_2`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 3 THEN MAIN.`SUM` END), 0) AS `ADD_3`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 4 THEN MAIN.`SUM` END), 0) AS `ADD_4`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 5 THEN MAIN.`SUM` END), 0) AS `ADD_5`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` = 6 THEN MAIN.`SUM` END), 0) AS `ADD_6`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` >= 7 AND `MAIN`.`MONTHA` <= 9 THEN MAIN.`SUM` END), 0) AS `OVER_9`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` >= 10 THEN MAIN.`SUM` END), 0) AS `OVER_10`,
    IFNULL(SUM(CASE WHEN `MAIN`.`MONTHA` THEN MAIN.`SUM` END), 0) AS `SUM`
    FROM(
    SELECT 
    `A`.`LEAD_NM`, SUM(`A`.`QTY`) AS `SUM`, `A`.`YEAR`, `A`.`MONTH`, `A`.`MONTHA` 
    FROM(
    SELECT 
    `TB_A`.`TRACK_IDX`,
    `TB_A`.`LEAD_NM`,
    `TB_A`.`QTY`,
    YEAR(IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`)) AS `YEAR`,
    MONTH(IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`)) AS `MONTH`,
    TIMESTAMPDIFF(MONTH , IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`), NOW()) AS `MONTHA`
    FROM 
    trackmtim `TB_A` LEFT JOIN
    (SELECT 
    `TB_IN`.`LT_INSP_IDX`, `TB_IN`.`TRACKMTIN_IDX`, `TB_IN`.`INSP_DATE`, `TB_IN`.`INSP_RESULT` FROM (
    SELECT  ROW_NUMBER() OVER (PARTITION BY `TRACKMTIN_IDX` ORDER BY `INSP_DATE` DESC) AS `RUM`,
    `LT_INSP_IDX`, `TRACKMTIN_IDX`,  `INSP_DATE` AS `INSP_DATE`, 
    `INSP_RESULT` FROM TRACKMTIM_LT_INSP) `TB_IN`
    WHERE `TB_IN`.`RUM` = '1')
     `TB_B`
    ON `TB_A`.`TRACK_IDX` = `TB_B`.`TRACKMTIN_IDX`
    WHERE `TB_A`.`RACKOUT_YN` = 'N' AND TIMESTAMPDIFF(MONTH , IFNULL(`TB_B`.`INSP_DATE`,  `TB_A`.`CREATE_DTM`), NOW()) > 0) `A`
    GROUP BY `A`.`LEAD_NM`, `A`.`YEAR`, `A`.`MONTH` ) AS `MAIN`
    GROUP BY `MAIN`.`YEAR`, `MAIN`.`MONTH` WITH ROLLUP) `TB`
    ORDER BY `YEAR` DESC, `CNO` DESC";
              
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
             
                result.DataGridView8 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView8 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
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

