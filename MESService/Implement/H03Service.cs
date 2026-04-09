namespace MESService.Implement
{
    public class H03Service : BaseService<torderlist, ItorderlistRepository>
    , IH03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H03Service(ItorderlistRepository torderlistRepository

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
IFNULL(`TB_A`.`A801`, IF(IFNULL(`TB_B`.`W_A801`, 0) =0, '', 'NO_WORKER')) AS `A801`,
IFNULL(`TB_A`.`A802`, IF(IFNULL(`TB_B`.`W_A802`, 0) =0, '', 'NO_WORKER')) AS `A802`,
IFNULL(`TB_A`.`A803`, IF(IFNULL(`TB_B`.`W_A803`, 0) =0, '', 'NO_WORKER')) AS `A803`,
IFNULL(`TB_A`.`A804`, IF(IFNULL(`TB_B`.`W_A804`, 0) =0, '', 'NO_WORKER')) AS `A804`,
IFNULL(`TB_A`.`A805`, IF(IFNULL(`TB_B`.`W_A805`, 0) =0, '', 'NO_WORKER')) AS `A805`,
IFNULL(`TB_A`.`A806`, IF(IFNULL(`TB_B`.`W_A806`, 0) =0, '', 'NO_WORKER')) AS `A806`,
IFNULL(`TB_A`.`A807`, IF(IFNULL(`TB_B`.`W_A807`, 0) =0, '', 'NO_WORKER')) AS `A807`,
IFNULL(`TB_A`.`A808`, IF(IFNULL(`TB_B`.`W_A808`, 0) =0, '', 'NO_WORKER')) AS `A808`,
IFNULL(`TB_A`.`A809`, IF(IFNULL(`TB_B`.`W_A809`, 0) =0, '', 'NO_WORKER')) AS `A809`,
IFNULL(`TB_A`.`A810`, IF(IFNULL(`TB_B`.`W_A810`, 0) =0, '', 'NO_WORKER')) AS `A810`,
IFNULL(`TB_A`.`A811`, IF(IFNULL(`TB_B`.`W_A811`, 0) =0, '', 'NO_WORKER')) AS `A811`,
IFNULL(`TB_A`.`A812`, IF(IFNULL(`TB_B`.`W_A812`, 0) =0, '', 'NO_WORKER')) AS `A812`,
IFNULL(`TB_A`.`A813`, IF(IFNULL(`TB_B`.`W_A813`, 0) =0, '', 'NO_WORKER')) AS `A813`,
IFNULL(`TB_A`.`A814`, IF(IFNULL(`TB_B`.`W_A814`, 0) =0, '', 'NO_WORKER')) AS `A814`,
IFNULL(`TB_A`.`A815`, IF(IFNULL(`TB_B`.`W_A815`, 0) =0, '', 'NO_WORKER')) AS `A815`,
IFNULL(`TB_A`.`A816`, IF(IFNULL(`TB_B`.`W_A816`, 0) =0, '', 'NO_WORKER')) AS `A816`,
IFNULL(`TB_A`.`A830`, IF(IFNULL(`TB_B`.`W_A830`, 0) =0, '', 'NO_WORKER')) AS `A830`,
IFNULL(`TB_A`.`A831`, IF(IFNULL(`TB_B`.`W_A831`, 0) =0, '', 'NO_WORKER')) AS `A831`,
IFNULL(`TB_A`.`A832`, IF(IFNULL(`TB_B`.`W_A832`, 0) =0, '', 'NO_WORKER')) AS `A832`,
IFNULL(`TB_A`.`A833`, IF(IFNULL(`TB_B`.`W_A833`, 0) =0, '', 'NO_WORKER')) AS `A833`,
IFNULL(`TB_A`.`A501`, IF(IFNULL(`TB_B`.`W_A501`, 0) =0, '', 'NO_WORKER')) AS `A501`,
IFNULL(`TB_A`.`A502`, IF(IFNULL(`TB_B`.`W_A502`, 0) =0, '', 'NO_WORKER')) AS `A502`,
`TB_A`.`SUM`
 FROM(
SELECT CONCAT(hour(TWWKAR.`CREATE_DTM`), ' ~ ', hour(TWWKAR.`CREATE_DTM`) +1) AS `Hour`,
sum(case when `MC_NO`='A801' then `WK_QTY` END) AS `A801`, 
sum(case when `MC_NO`='A802' then `WK_QTY` end) AS `A802`, 
sum(case when `MC_NO`='A803' then `WK_QTY` end) AS `A803`, 
sum(case when `MC_NO`='A804' then `WK_QTY` end) AS `A804`, 
sum(case when `MC_NO`='A805' then `WK_QTY` end) AS `A805`, 
sum(case when `MC_NO`='A806' then `WK_QTY` end) AS `A806`, 
sum(case when `MC_NO`='A807' then `WK_QTY` end) AS `A807`, 
sum(case when `MC_NO`='A808' then `WK_QTY` end) AS `A808`, 
sum(case when `MC_NO`='A809' then `WK_QTY` end) AS `A809`, 
sum(case when `MC_NO`='A810' then `WK_QTY` end) AS `A810`, 
sum(case when `MC_NO`='A811' then `WK_QTY` end) AS `A811`,
sum(case when `MC_NO`='A812' then `WK_QTY` end) AS `A812`,
sum(case when `MC_NO`='A813' then `WK_QTY` end) AS `A813`,
sum(case when `MC_NO`='A814' then `WK_QTY` end) AS `A814`,
sum(case when `MC_NO`='A815' then `WK_QTY` end) AS `A815`,
sum(case when `MC_NO`='A816' then `WK_QTY` end) AS `A816`,
sum(case when `MC_NO`='A830' then `WK_QTY` end) AS `A830`,
sum(case when `MC_NO`='A831' then `WK_QTY` end) AS `A831`,
sum(case when `MC_NO`='A832' then `WK_QTY` end) AS `A832`,
sum(case when `MC_NO`='A833' then `WK_QTY` end) AS `A833`,
sum(case when `MC_NO`='A501' then `WK_QTY` end) AS `A501`,
sum(case when `MC_NO`='A502' then `WK_QTY` end) AS `A502`,
sum(case when `MC_NO` IN ('A502','A501','A816','A815','A814','A813','A812','A811','A810','A809','A808','A807','A806','A805','A804','A803','A802','A801','A830','A831','A832','A833') then `WK_QTY` end) AS `SUM`, 
hour(TWWKAR.`CREATE_DTM`) AS `HOUR_ST`,
IF(hour(TWWKAR.`CREATE_DTM`)<6, hour(TWWKAR.`CREATE_DTM`)+30, hour(TWWKAR.`CREATE_DTM`)) AS `HOUR_ROW`
FROM TWWKAR
WHERE TWWKAR.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
GROUP BY hour(TWWKAR.`CREATE_DTM`) ORDER BY TWWKAR.`WK_IDX`) `TB_A` 
LEFT JOIN
(SELECT 
`TB_ZB`.`RNUM`,
sum(case when `TB_ZA`.`MC_NO`='A801' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A801`,
sum(case when `TB_ZA`.`MC_NO`='A802' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A802`,

sum(case when `TB_ZA`.`MC_NO`='A803' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A803`,

sum(case when `TB_ZA`.`MC_NO`='A804' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A804`,

sum(case when `TB_ZA`.`MC_NO`='A805' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A805`,

sum(case when `TB_ZA`.`MC_NO`='A806' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A806`,

sum(case when `TB_ZA`.`MC_NO`='A807' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A807`,

sum(case when `TB_ZA`.`MC_NO`='A808' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A808`,

sum(case when `TB_ZA`.`MC_NO`='A809' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A809`,

sum(case when `TB_ZA`.`MC_NO`='A810' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A810`,

sum(case when `TB_ZA`.`MC_NO`='A811' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A811`,

sum(case when `TB_ZA`.`MC_NO`='A812' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A812`,

sum(case when `TB_ZA`.`MC_NO`='A813' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A813`,

sum(case when `TB_ZA`.`MC_NO`='A814' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A814`,

sum(case when `TB_ZA`.`MC_NO`='A815' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A815`,

sum(case when `TB_ZA`.`MC_NO`='A816' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A816`,

sum(case when `TB_ZA`.`MC_NO`='A830' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A830`,

sum(case when `TB_ZA`.`MC_NO`='A831' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A831`,

sum(case when `TB_ZA`.`MC_NO`='A832' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A832`,

sum(case when `TB_ZA`.`MC_NO`='A833' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A833`,

sum(case when `TB_ZA`.`MC_NO`='A501' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A501`,

sum(case when `TB_ZA`.`MC_NO`='A502' then (IF(`TB_ZB`.`RNUM` <=5, 
IF(`TB_ZB`.`RNUM`+24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM`+24 <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`) , 1, 0),
IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <=5,`TB_ZA`.`E_TIME`+24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_A502`
FROM(
SELECT `TSNON_OPER_MCNM` AS `MC_NO`, `TSNON_OPER_TIME` AS `CREATE_DTM`,
HOUR(`TSNON_OPER_TIME`) AS `S_TIME`,
IF(`TSNON_OPER_COL` = 'S', 
IF(LEAD(`TSNON_OPER_MCNM`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = `TSNON_OPER_MCNM` AND
LEAD(`TSNON_OPER_COL`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = 'E', LEAD(HOUR(`TSNON_OPER_TIME`)) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`), 0), 0) AS `E_TIME`, 
'NO_WORKER' AS `WK_QTY_S`
FROM TSNON_OPER_WORKER
WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= '{DATA_NOW} 06:00:00' AND TSNON_OPER_WORKER.`TSNON_OPER_TIME` < '{DATA_AFD} 06:00:00') `TB_ZA` 
JOIN (WITH RECURSIVE `CTE` AS (SELECT 0 AS `RNUM` UNION ALL
SELECT `RNUM` + 1 FROM `CTE` WHERE `RNUM` < 23)
SELECT `RNUM` FROM `CTE`) AS `TB_ZB`
WHERE `TB_ZA`.`E_TIME`> 0
GROUP BY `TB_ZB`.`RNUM`) `TB_B`
ON `TB_B`.`RNUM` = `TB_A`.`HOUR_ST` 
ORDER BY `TB_A`.`HOUR_ROW`";


                string DGV_DATA2 = $@"
SELECT 'Hour',
sum(case when MC_NO='A801' then WK_QTY end) as 'A801', 
sum(case when MC_NO='A802' then WK_QTY end) as 'A802', 
sum(case when MC_NO='A803' then WK_QTY end) as 'A803', 
sum(case when MC_NO='A804' then WK_QTY end) as 'A804', 
sum(case when MC_NO='A805' then WK_QTY end) as 'A805', 
sum(case when MC_NO='A806' then WK_QTY end) as 'A806', 
sum(case when MC_NO='A807' then WK_QTY end) as 'A807', 
sum(case when MC_NO='A808' then WK_QTY end) as 'A808', 
sum(case when MC_NO='A809' then WK_QTY end) as 'A809', 
sum(case when MC_NO='A810' then WK_QTY end) as 'A810', 
sum(case when MC_NO='A811' then WK_QTY end) as 'A811',
sum(case when MC_NO='A812' then WK_QTY end) as 'A812',
sum(case when MC_NO='A813' then WK_QTY end) as 'A813',
sum(case when MC_NO='A814' then WK_QTY end) as 'A814',
sum(case when MC_NO='A815' then WK_QTY end) as 'A815',
sum(case when MC_NO='A816' then WK_QTY end) as 'A816',
sum(case when MC_NO='A830' then WK_QTY end) as 'A830',
sum(case when MC_NO='A831' then WK_QTY end) as 'A831',
sum(case when MC_NO='A832' then WK_QTY end) as 'A832',
sum(case when MC_NO='A833' then WK_QTY end) as 'A833',
sum(case when MC_NO='A501' then WK_QTY end) as 'A501',
sum(case when MC_NO='A502' then WK_QTY end) as 'A502',
sum(case when MC_NO IN ('A502','A501','A816','A815','A814','A813','A812','A811','A810','A809','A808','A807','A806','A805','A804','A803','A802','A801','A830','A831','A832','A833') then WK_QTY end) as 'SUM'
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

