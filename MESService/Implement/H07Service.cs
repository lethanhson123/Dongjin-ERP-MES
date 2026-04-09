namespace MESService.Implement
{
    public class H07Service : BaseService<torderlist, ItorderlistRepository>, IH07Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public H07Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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

        public virtual async Task<BaseResult> Buttonreload_Click(BaseParameter param)
        {
            return await Buttonfind_Click(param);
        }

        public virtual async Task<BaseResult> Buttonsearch_Click(BaseParameter param)
        {
            return await Buttonfind_Click(param);
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter param)
        {
            BaseResult result = new BaseResult();
            try
            {
                string DATA_NOW = param.DateTimePicker1 ?? DateTime.Now.ToString("yyyy-MM-dd");
                string DATA_AFD = DateTime.Parse(DATA_NOW).AddDays(1).ToString("yyyy-MM-dd");


                string DGV_DATA1 = $@"
SELECT 
`TB_A`.`Hour`,
IFNULL(`TB_A`.`ZA801`, IF(IFNULL(`TB_B`.`W_A801`, 0) = 0, '', 'NO_WORKER')) AS `ZA801`,
IFNULL(`TB_A`.`ZA802`, IF(IFNULL(`TB_B`.`W_A802`, 0) = 0, '', 'NO_WORKER')) AS `ZA802`,
IFNULL(`TB_A`.`ZA803`, IF(IFNULL(`TB_B`.`W_A803`, 0) = 0, '', 'NO_WORKER')) AS `ZA803`,
IFNULL(`TB_A`.`ZA804`, IF(IFNULL(`TB_B`.`W_A804`, 0) = 0, '', 'NO_WORKER')) AS `ZA804`,
IFNULL(`TB_A`.`ZA805`, IF(IFNULL(`TB_B`.`W_A805`, 0) = 0, '', 'NO_WORKER')) AS `ZA805`,
IFNULL(`TB_A`.`ZA806`, IF(IFNULL(`TB_B`.`W_A806`, 0) = 0, '', 'NO_WORKER')) AS `ZA806`,
IFNULL(`TB_A`.`ZA807`, IF(IFNULL(`TB_B`.`W_A807`, 0) = 0, '', 'NO_WORKER')) AS `ZA807`,
IFNULL(`TB_A`.`ZA808`, IF(IFNULL(`TB_B`.`W_A808`, 0) = 0, '', 'NO_WORKER')) AS `ZA808`,
IFNULL(`TB_A`.`ZA809`, IF(IFNULL(`TB_B`.`W_A809`, 0) = 0, '', 'NO_WORKER')) AS `ZA809`,
IFNULL(`TB_A`.`ZA810`, IF(IFNULL(`TB_B`.`W_A810`, 0) = 0, '', 'NO_WORKER')) AS `ZA810`,
`TB_A`.`SUM`
FROM (
    SELECT CONCAT(hour(TWWKAR.`CREATE_DTM`), ' ~ ', hour(TWWKAR.`CREATE_DTM`) + 1) AS `Hour`,
    sum(case when `MC_NO`='ZA801' then `WK_QTY` END) AS `ZA801`, 
    sum(case when `MC_NO`='ZA802' then `WK_QTY` end) AS `ZA802`, 
    sum(case when `MC_NO`='ZA803' then `WK_QTY` end) AS `ZA803`, 
    sum(case when `MC_NO`='ZA804' then `WK_QTY` end) AS `ZA804`, 
    sum(case when `MC_NO`='ZA805' then `WK_QTY` end) AS `ZA805`, 
    sum(case when `MC_NO`='ZA806' then `WK_QTY` end) AS `ZA806`, 
    sum(case when `MC_NO`='ZA807' then `WK_QTY` end) AS `ZA807`, 
    sum(case when `MC_NO`='ZA808' then `WK_QTY` end) AS `ZA808`, 
    sum(case when `MC_NO`='ZA809' then `WK_QTY` end) AS `ZA809`, 
    sum(case when `MC_NO`='ZA810' then `WK_QTY` end) AS `ZA810`, 
    sum(case when `MC_NO` IN ('ZA801','ZA802','ZA803','ZA804','ZA805','ZA806','ZA807','ZA808','ZA809','ZA810') then `WK_QTY` end) AS `SUM`, 
    hour(TWWKAR.`CREATE_DTM`) AS `HOUR_ST`,
    IF(hour(TWWKAR.`CREATE_DTM`) < 6, hour(TWWKAR.`CREATE_DTM`) + 30, hour(TWWKAR.`CREATE_DTM`)) AS `HOUR_ROW`
    FROM TWWKAR
    WHERE TWWKAR.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
    GROUP BY hour(TWWKAR.`CREATE_DTM`) 
    ORDER BY TWWKAR.`WK_IDX`
) `TB_A` 
LEFT JOIN (
    SELECT 
    `TB_ZB`.`RNUM`,
    sum(case when `TB_ZA`.`MC_NO`='ZA801' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A801`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA802' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A802`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA803' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A803`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA804' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A804`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA805' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A805`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA806' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A806`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA807' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A807`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA808' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A808`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA809' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A809`,
    
    sum(case when `TB_ZA`.`MC_NO`='ZA810' then (IF(`TB_ZB`.`RNUM` <= 5, 
    IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`) , 1, 0),
    IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A810`
    
    FROM (
        SELECT `TSNON_OPER_MCNM` AS `MC_NO`, `TSNON_OPER_TIME` AS `CREATE_DTM`,
        HOUR(`TSNON_OPER_TIME`) AS `S_TIME`,
        IF(`TSNON_OPER_COL` = 'S', 
        IF(LEAD(`TSNON_OPER_MCNM`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = `TSNON_OPER_MCNM` AND
        LEAD(`TSNON_OPER_COL`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = 'E', LEAD(HOUR(`TSNON_OPER_TIME`)) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`), 0), 0) AS `E_TIME`, 
        'NO_WORKER' AS `WK_QTY_S`
        FROM TSNON_OPER_WORKER
        WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= '{DATA_NOW} 06:00:00' AND TSNON_OPER_WORKER.`TSNON_OPER_TIME` < '{DATA_AFD} 06:00:00'
    ) `TB_ZA` 
    JOIN (WITH RECURSIVE `CTE` AS (SELECT 0 AS `RNUM` UNION ALL
    SELECT `RNUM` + 1 FROM `CTE` WHERE `RNUM` < 23)
    SELECT `RNUM` FROM `CTE`) AS `TB_ZB`
    WHERE `TB_ZA`.`E_TIME` > 0
    GROUP BY `TB_ZB`.`RNUM`
) `TB_B`
ON `TB_B`.`RNUM` = `TB_A`.`HOUR_ST` 
ORDER BY `TB_A`.`HOUR_ROW`";


                string DGV_DATA2 = $@"
SELECT 'Hour',
sum(case when MC_NO='ZA801' then WK_QTY end) as 'ZA801', 
sum(case when MC_NO='ZA802' then WK_QTY end) as 'ZA802', 
sum(case when MC_NO='ZA803' then WK_QTY end) as 'ZA803', 
sum(case when MC_NO='ZA804' then WK_QTY end) as 'ZA804', 
sum(case when MC_NO='ZA805' then WK_QTY end) as 'ZA805', 
sum(case when MC_NO='ZA806' then WK_QTY end) as 'ZA806', 
sum(case when MC_NO='ZA807' then WK_QTY end) as 'ZA807', 
sum(case when MC_NO='ZA808' then WK_QTY end) as 'ZA808', 
sum(case when MC_NO='ZA809' then WK_QTY end) as 'ZA809', 
sum(case when MC_NO='ZA810' then WK_QTY end) as 'ZA810', 
sum(case when MC_NO IN ('ZA801','ZA802','ZA803','ZA804','ZA805','ZA806','ZA807','ZA808','ZA809','ZA810') then WK_QTY end) as 'SUM'
FROM TWWKAR
WHERE TWWKAR.CREATE_DTM >= '{DATA_NOW} 06:00:00' AND TWWKAR.CREATE_DTM < '{DATA_AFD} 06:00:00'";


                DataSet ds1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                result.DataGridView1 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds1.Tables)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }


                DataSet ds2 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA2);
                result.DataGridView2 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds2.Tables)
                {
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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