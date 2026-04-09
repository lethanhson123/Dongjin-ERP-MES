namespace MESService.Implement
{
    public class H11Service : BaseService<torderlist, ItorderlistRepository>
    , IH11Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H11Service(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> GetMachineStatus()
        {
            BaseResult result = new BaseResult();
            try
            {

                string NOW_DATE_S = DateTime.Now.Hour < 6
                    ? DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
                    : DateTime.Now.ToString("yyyy-MM-dd");
                string sql = @"SELECT `A`.`MC_NO`, `A`.`SUM`, 
                    IFNULL(tsnon_oper_andon.`tsnon_oper_mitor_NOIC`, '') AS `tsnon_oper_mitor_NOIC`,
                    IFNULL(tsnon_oper_andon.`tsnon_oper_mitor_RUNYN`, 'N') AS `tsnon_oper_mitor_RUNYN`
                    FROM
                    (SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` FROM TWWKAR  
                        WHERE `CREATE_DTM` > DATE_FORMAT('" + NOW_DATE_S + @" 06:00:00', '%Y-%m-%d %T') AND 
                              `CREATE_DTM` < DATE_FORMAT('" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + @" 06:00:00', '%Y-%m-%d %T')
                        GROUP BY  `MC_NO`
                    UNION
                    SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` FROM TWWKAR_SPST  
                        WHERE `CREATE_DTM` > DATE_FORMAT('" + NOW_DATE_S + @" 06:00:00', '%Y-%m-%d %T') AND 
                              `CREATE_DTM` < DATE_FORMAT('" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + @" 06:00:00', '%Y-%m-%d %T')
                        GROUP BY  `MC_NO`) `A` 
                    LEFT JOIN tsnon_oper_andon 
                    ON `A`.`MC_NO` = tsnon_oper_andon.tsnon_oper_mitor_MCNM";

                DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_01.Tables[0]);

                sql = @"SELECT 
                    `A`.`tsnon_oper_mitor_IDX`,
                    `A`.`tsnon_oper_mitor_MCNM`,
                    `A`.`tsnon_oper_mitor_NOIC`,
                    `A`.`tsnon_oper_mitor_RUNYN`,
                    `B`.`CD_NM_HAN`
                    FROM
                    (SELECT 
                    tsnon_oper_andon_LIST.tsnon_oper_mitor_IDX,
                    tsnon_oper_andon_LIST.tsnon_oper_mitor_MCNM,
                    tsnon_oper_andon_LIST.tsnon_oper_mitor_NOIC,
                    tsnon_oper_andon_LIST.tsnon_oper_mitor_RUNYN
                    FROM tsnon_oper_andon_LIST
                    WHERE tsnon_oper_andon_LIST.CREATE_DTM >= DATE_FORMAT(NOW(), '%Y-%m-%d')) `A`
                    JOIN 
                    (SELECT  MAX(tsnon_oper_andon_LIST.tsnon_oper_mitor_IDX) AS `LAST_IDX`,
                    tsnon_oper_andon_LIST.tsnon_oper_mitor_MCNM, `C`.`CD_NM_HAN`
                    FROM tsnon_oper_andon_LIST LEFT JOIN
                    (SELECT TSCODE.CD_SYS_NOTE, TSCODE.CD_NM_HAN FROM TSCODE WHERE TSCODE.CDGR_IDX = '8') `C`
                    ON tsnon_oper_andon_LIST.tsnon_oper_mitor_MCNM = `C`.CD_SYS_NOTE
                    WHERE tsnon_oper_andon_LIST.CREATE_DTM >= DATE_FORMAT(NOW(), '%Y-%m-%d')
                    GROUP BY tsnon_oper_andon_LIST.tsnon_oper_mitor_MCNM  ) `B`
                    ON  `A`.`tsnon_oper_mitor_IDX` = `B`.`LAST_IDX`
                    WHERE NOT(ISNULL(`B`.`CD_NM_HAN`)) 
                    AND NOT(`A`.`tsnon_oper_mitor_RUNYN` ='N') 
                    AND CAST(RIGHT(`B`.`CD_NM_HAN`, 3) AS UNSIGNED) >= 501
                    ORDER BY `A`.`tsnon_oper_mitor_IDX` DESC LIMIT 8";

                DataSet dsDGV_02 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                result.DataGridView2 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_02.Tables[0]);
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

