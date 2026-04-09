namespace MESService.Implement
{
    public class H12Service : BaseService<torderlist, ItorderlistRepository>
    , IH12Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H12Service(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string DATA_NOW = BaseParameter.DateTimePicker1 ?? DateTime.Now.ToString("yyyy-MM-dd");
                string DATA_AFD = DateTime.Parse(DATA_NOW).AddDays(1).ToString("yyyy-MM-dd");

                string DGV_DATA1 = $@"
SELECT 
    `TB_A`.`Hour`,
    IFNULL(`TB_A`.`ZZ101`, IF(IFNULL(`TB_B`.`W_ZZ101`, 0) = 0, '', 'NO_WORKER')) AS `A801`,
    IFNULL(`TB_A`.`ZZ102`, IF(IFNULL(`TB_B`.`W_ZZ102`, 0) = 0, '', 'NO_WORKER')) AS `A802`,
    IFNULL(`TB_A`.`ZZ103`, IF(IFNULL(`TB_B`.`W_ZZ103`, 0) = 0, '', 'NO_WORKER')) AS `A803`,
    IFNULL(`TB_A`.`ZZ104`, IF(IFNULL(`TB_B`.`W_ZZ104`, 0) = 0, '', 'NO_WORKER')) AS `A804`,
    IFNULL(`TB_A`.`ZZ105`, IF(IFNULL(`TB_B`.`W_ZZ105`, 0) = 0, '', 'NO_WORKER')) AS `A805`,
    IFNULL(`TB_A`.`ZZ106`, IF(IFNULL(`TB_B`.`W_ZZ106`, 0) = 0, '', 'NO_WORKER')) AS `A806`,
    IFNULL(`TB_A`.`TW_SUM`, IF(IFNULL(`TB_B`.`W_TW_SUM`, 0) = 0, '', 'NO_WORKER')) AS `A807`,
    IFNULL(`TB_A`.`ZS101`, IF(IFNULL(`TB_B`.`W_ZS101`, 0) = 0, '', 'NO_WORKER')) AS `A808`,
    IFNULL(`TB_A`.`ZS102`, IF(IFNULL(`TB_B`.`W_ZS102`, 0) = 0, '', 'NO_WORKER')) AS `A809`,
    IFNULL(`TB_A`.`ZS103`, IF(IFNULL(`TB_B`.`W_ZS103`, 0) = 0, '', 'NO_WORKER')) AS `A810`,
    IFNULL(`TB_A`.`ZS104`, IF(IFNULL(`TB_B`.`W_ZS104`, 0) = 0, '', 'NO_WORKER')) AS `A811`,
    IFNULL(`TB_A`.`ZS105`, IF(IFNULL(`TB_B`.`W_ZS105`, 0) = 0, '', 'NO_WORKER')) AS `A812`,
    IFNULL(`TB_A`.`ZS106`, IF(IFNULL(`TB_B`.`W_ZS106`, 0) = 0, '', 'NO_WORKER')) AS `A813`,
    IFNULL(`TB_A`.`ZS107`, IF(IFNULL(`TB_B`.`W_ZS107`, 0) = 0, '', 'NO_WORKER')) AS `A814`,
    IFNULL(`TB_A`.`WD_SUM`, IF(IFNULL(`TB_B`.`W_WD_SUM`, 0) = 0, '', 'NO_WORKER')) AS `A815`,
    `TB_A`.`SUM` AS `SUM`
FROM (
    SELECT 
        CONCAT(hour(TWWKAR_SPST.`CREATE_DTM`), ' ~ ', hour(TWWKAR_SPST.`CREATE_DTM`) + 1) AS `Hour`,
        sum(case when `MC_NO`='ZZ101' then `WK_QTY` END) AS `ZZ101`, 
        sum(case when `MC_NO`='ZZ102' then `WK_QTY` end) AS `ZZ102`, 
        sum(case when `MC_NO`='ZZ103' then `WK_QTY` end) AS `ZZ103`, 
        sum(case when `MC_NO`='ZZ104' then `WK_QTY` end) AS `ZZ104`, 
        sum(case when `MC_NO`='ZZ105' then `WK_QTY` end) AS `ZZ105`, 
        sum(case when `MC_NO`='ZZ106' then `WK_QTY` end) AS `ZZ106`, 
        sum(case when `MC_NO` LIKE 'ZZ1%' then `WK_QTY` end) AS `TW_SUM`, 
        sum(case when `MC_NO`='ZS101' then `WK_QTY` end) AS `ZS101`, 
        sum(case when `MC_NO`='ZS102' then `WK_QTY` end) AS `ZS102`, 
        sum(case when `MC_NO`='ZS103' then `WK_QTY` end) AS `ZS103`, 
        sum(case when `MC_NO`='ZS104' then `WK_QTY` end) AS `ZS104`,
        sum(case when `MC_NO`='ZS105' then `WK_QTY` end) AS `ZS105`,
        sum(case when `MC_NO`='ZS106' then `WK_QTY` end) AS `ZS106`,
        sum(case when `MC_NO`='ZS107' then `WK_QTY` end) AS `ZS107`,
        sum(case when `MC_NO`LIKE 'ZS1%' then `WK_QTY` end) AS `WD_SUM`,
        sum(case when `MC_NO` LIKE 'ZZ1%' OR `MC_NO` LIKE 'ZS1%' then `WK_QTY` end) AS `SUM`, 
        hour(TWWKAR_SPST.`CREATE_DTM`) AS `HOUR_ST`,
        IF(hour(TWWKAR_SPST.`CREATE_DTM`)<6, hour(TWWKAR_SPST.`CREATE_DTM`)+30, hour(TWWKAR_SPST.`CREATE_DTM`)) AS `HOUR_ROW`
    FROM TWWKAR_SPST
    WHERE TWWKAR_SPST.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR_SPST.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
    GROUP BY hour(TWWKAR_SPST.`CREATE_DTM`) ORDER BY TWWKAR_SPST.`WK_IDX`
) `TB_A`
LEFT JOIN (
    -- Subquery W_xxx giống như bạn đã có ở H07, chỉ cần copy đổi MC_NO cho đúng tên ZZ/ZS.
    SELECT 
        `TB_ZB`.`RNUM`,
        sum(case when `TB_ZA`.`MC_NO`='ZZ101' then (IF(`TB_ZB`.`RNUM` <=5, 
            IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_ZZ101`,
        -- ... (các dòng khác giữ nguyên như bên trên, chỉ đổi tên W_ZZxxx/W_ZSxxx/W_TW_SUM/W_WD_SUM)
        sum(case when `TB_ZA`.`MC_NO` LIKE 'ZZ1%'  then (IF(`TB_ZB`.`RNUM` <=5, 
            IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_TW_SUM`,
        -- ... các trường còn lại
        sum(case when `TB_ZA`.`MC_NO` LIKE 'ZS1%'  then (IF(`TB_ZB`.`RNUM` <=5, 
            IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_WD_SUM`
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
sum(case when MC_NO='ZZ101' then WK_QTY end) as 'A801',
sum(case when MC_NO='ZZ102' then WK_QTY end) as 'A802',
sum(case when MC_NO='ZZ103' then WK_QTY end) as 'A803',
sum(case when MC_NO='ZZ104' then WK_QTY end) as 'A804',
sum(case when MC_NO='ZZ105' then WK_QTY end) as 'A805',
sum(case when MC_NO='ZZ106' then WK_QTY end) as 'A806',
sum(case when MC_NO LIKE 'ZZ1%' then WK_QTY end) as 'A807',
sum(case when MC_NO='ZS101' then WK_QTY end) as 'A808',
sum(case when MC_NO='ZS102' then WK_QTY end) as 'A809',
sum(case when MC_NO='ZS103' then WK_QTY end) as 'A810',
sum(case when MC_NO='ZS104' then WK_QTY end) as 'A811',
sum(case when MC_NO='ZS105' then WK_QTY end) as 'A812',
sum(case when MC_NO='ZS106' then WK_QTY end) as 'A813',
sum(case when MC_NO='ZS107' then WK_QTY end) as 'A814',
sum(case when MC_NO LIKE 'ZS1%' then WK_QTY end) as 'A815',
sum(case when MC_NO LIKE 'ZZ1%' OR MC_NO LIKE 'ZS1%' then WK_QTY end) as 'SUM'
FROM TWWKAR_SPST
WHERE TWWKAR_SPST.CREATE_DTM >= '{DATA_NOW} 06:00:00' AND TWWKAR_SPST.CREATE_DTM < '{DATA_AFD} 06:00:00'";

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

