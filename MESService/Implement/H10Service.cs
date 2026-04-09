namespace MESService.Implement
{
    public class H10Service : BaseService<torderlist, ItorderlistRepository>, IH10Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public H10Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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
                // Gọi Buttonfind_Click với ngày mặc định là hôm nay
                BaseParameter param = new BaseParameter
                {
                    DateTimePicker1 = DateTime.Now.ToString("yyyy-MM-dd")
                };
                result = await Buttonfind_Click(param);
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
SELECT `TB_A`.`Hour`,
IFNULL(`TB_A`.`Z101`, IF(IFNULL(`TB_B`.`W_Z101`, 0) =0, '', 'NO_WORKER')) AS `A801`,
IFNULL(`TB_A`.`Z102`, IF(IFNULL(`TB_B`.`W_Z102`, 0) =0, '', 'NO_WORKER')) AS `A802`,
IFNULL(`TB_A`.`Z103`, IF(IFNULL(`TB_B`.`W_Z103`, 0) =0, '', 'NO_WORKER')) AS `A803`,
IFNULL(`TB_A`.`Z104`, IF(IFNULL(`TB_B`.`W_Z104`, 0) =0, '', 'NO_WORKER')) AS `A804`,
IFNULL(`TB_A`.`Z105`, IF(IFNULL(`TB_B`.`W_Z105`, 0) =0, '', 'NO_WORKER')) AS `A805`,
IFNULL(`TB_A`.`Z106`, IF(IFNULL(`TB_B`.`W_Z106`, 0) =0, '', 'NO_WORKER')) AS `A806`,
IFNULL(`TB_A`.`Z107`, IF(IFNULL(`TB_B`.`W_Z107`, 0) =0, '', 'NO_WORKER')) AS `A807`,
IFNULL(`TB_A`.`Z108`, IF(IFNULL(`TB_B`.`W_Z108`, 0) =0, '', 'NO_WORKER')) AS `A808`,
IFNULL(`TB_A`.`Z109`, IF(IFNULL(`TB_B`.`W_Z109`, 0) =0, '', 'NO_WORKER')) AS `A809`,
IFNULL(`TB_A`.`Z110`, IF(IFNULL(`TB_B`.`W_Z110`, 0) =0, '', 'NO_WORKER')) AS `A810`,
IFNULL(`TB_A`.`Z111`, IF(IFNULL(`TB_B`.`W_Z111`, 0) =0, '', 'NO_WORKER')) AS `A811`,
IFNULL(`TB_A`.`Z112`, IF(IFNULL(`TB_B`.`W_Z112`, 0) =0, '', 'NO_WORKER')) AS `A812`,
IFNULL(`TB_A`.`TW_SUM`, IF(IFNULL(`TB_B`.`W_TW_SUM`, 0) =0, '', 'NO_WORKER')) AS `A813`,
IFNULL(`TB_A`.`S101`, IF(IFNULL(`TB_B`.`W_S101`, 0) =0, '', 'NO_WORKER')) AS `A501`,
IFNULL(`TB_A`.`S102`, IF(IFNULL(`TB_B`.`W_S102`, 0) =0, '', 'NO_WORKER')) AS `A502`,
IFNULL(`TB_A`.`S103`, IF(IFNULL(`TB_B`.`W_S103`, 0) =0, '', 'NO_WORKER')) AS `Coln1`,
IFNULL(`TB_A`.`S104`, IF(IFNULL(`TB_B`.`W_S104`, 0) =0, '', 'NO_WORKER')) AS `Coln2`,
IFNULL(`TB_A`.`S105`, IF(IFNULL(`TB_B`.`W_S105`, 0) =0, '', 'NO_WORKER')) AS `Coln3`,
IFNULL(`TB_A`.`S106`, IF(IFNULL(`TB_B`.`W_S106`, 0) =0, '', 'NO_WORKER')) AS `Coln4`,
IFNULL(`TB_A`.`S107`, IF(IFNULL(`TB_B`.`W_S107`, 0) =0, '', 'NO_WORKER')) AS `Coln5`,
IFNULL(`TB_A`.`S108`, IF(IFNULL(`TB_B`.`W_S108`, 0) =0, '', 'NO_WORKER')) AS `Coln6`,
IFNULL(`TB_A`.`S109`, IF(IFNULL(`TB_B`.`W_S109`, 0) =0, '', 'NO_WORKER')) AS `Coln7`,
IFNULL(`TB_A`.`S110`, IF(IFNULL(`TB_B`.`W_S110`, 0) =0, '', 'NO_WORKER')) AS `D01`,
IFNULL(`TB_A`.`S111`, IF(IFNULL(`TB_B`.`W_S111`, 0) =0, '', 'NO_WORKER')) AS `D02`,
IFNULL(`TB_A`.`S112`, IF(IFNULL(`TB_B`.`W_S112`, 0) =0, '', 'NO_WORKER')) AS `D03`,
IFNULL(`TB_A`.`WD_SUM`, IF(IFNULL(`TB_B`.`W_WD_SUM`, 0) =0, '', 'NO_WORKER')) AS `D04`,
IFNULL(`TB_A`.`B201`, IF(IFNULL(`TB_B`.`C_B201`, 0) =0, '', 'NO_WORKER')) AS `D05`,
IFNULL(`TB_A`.`B202`, IF(IFNULL(`TB_B`.`C_B202`, 0) =0, '', 'NO_WORKER')) AS `D06`,
IFNULL(`TB_A`.`B203`, IF(IFNULL(`TB_B`.`C_B203`, 0) =0, '', 'NO_WORKER')) AS `D07`,
IFNULL(`TB_A`.`B204`, IF(IFNULL(`TB_B`.`C_B204`, 0) =0, '', 'NO_WORKER')) AS `D08`,
IFNULL(`TB_A`.`B205`, IF(IFNULL(`TB_B`.`C_B205`, 0) =0, '', 'NO_WORKER')) AS `D09`,
IFNULL(`TB_A`.`B206`, IF(IFNULL(`TB_B`.`C_B206`, 0) =0, '', 'NO_WORKER')) AS `D10`,
IFNULL(`TB_A`.`B207`, IF(IFNULL(`TB_B`.`C_B207`, 0) =0, '', 'NO_WORKER')) AS `D11`,
IFNULL(`TB_A`.`B208`, IF(IFNULL(`TB_B`.`C_B208`, 0) =0, '', 'NO_WORKER')) AS `D12`,
IFNULL(`TB_A`.`B209`, IF(IFNULL(`TB_B`.`C_B209`, 0) =0, '', 'NO_WORKER')) AS `D13`,
IFNULL(`TB_A`.`B210`, IF(IFNULL(`TB_B`.`C_B210`, 0) =0, '', 'NO_WORKER')) AS `D14`,
IFNULL(`TB_A`.`B211`, IF(IFNULL(`TB_B`.`C_B211`, 0) =0, '', 'NO_WORKER')) AS `D15`,
IFNULL(`TB_A`.`B212`, IF(IFNULL(`TB_B`.`C_B212`, 0) =0, '', 'NO_WORKER')) AS `D16`,
IFNULL(`TB_A`.`B213`, IF(IFNULL(`TB_B`.`C_B213`, 0) =0, '', 'NO_WORKER')) AS `D17`,
IFNULL(`TB_A`.`B214`, IF(IFNULL(`TB_B`.`C_B214`, 0) =0, '', 'NO_WORKER')) AS `D18`,
IFNULL(`TB_A`.`B215`, IF(IFNULL(`TB_B`.`C_B215`, 0) =0, '', 'NO_WORKER')) AS `D19`,
IFNULL(`TB_A`.`B216`, IF(IFNULL(`TB_B`.`C_B216`, 0) =0, '', 'NO_WORKER')) AS `D20`,
IFNULL(`TB_A`.`CR_SUM`, IF(IFNULL(`TB_B`.`C_WD_SUM`, 0) =0, '', 'NO_WORKER')) AS `D21`,
IFNULL(TW_SUM, 0) + IFNULL(WD_SUM, 0) + IFNULL(CR_SUM, 0) AS `SUM`
FROM (
    SELECT STB_1.Hour, STB_2.HOUR_ST, STB_2.HOUR_ROW, 
        STB_1.B201, STB_1.B202, STB_1.B203, STB_1.B204, STB_1.B205, STB_1.B206,
        STB_1.B207, STB_1.B208, STB_1.B209, STB_1.B210, STB_1.B211, STB_1.B212,
        STB_1.B213, STB_1.B214, STB_1.B215, STB_1.B216, STB_1.CR_SUM,
        STB_2.Z101, STB_2.Z102, STB_2.Z103, STB_2.Z104, STB_2.Z105, STB_2.Z106,
        STB_2.Z107, STB_2.Z108, STB_2.Z109, STB_2.Z110, STB_2.Z111, STB_2.Z112, STB_2.TW_SUM,
        STB_2.S101, STB_2.S102, STB_2.S103, STB_2.S104, STB_2.S105, STB_2.S106,
        STB_2.S107, STB_2.S108, STB_2.S109, STB_2.S110, STB_2.S111, STB_2.S112, STB_2.WD_SUM,
        STB_1.CR_SUM + STB_2.TW_SUM + STB_2.WD_SUM AS `SUM`
    FROM (
        SELECT CONCAT(hour(TWWKAR_LP.`CREATE_DTM`), ' ~ ', hour(TWWKAR_LP.`CREATE_DTM`) + 1) AS `Hour`,
            SUM(CASE WHEN `MC_NO`='B201' THEN `WK_QTY` END) AS `B201`, 
            SUM(CASE WHEN `MC_NO`='B202' THEN `WK_QTY` END) AS `B202`, 
            SUM(CASE WHEN `MC_NO`='B203' THEN `WK_QTY` END) AS `B203`, 
            SUM(CASE WHEN `MC_NO`='B204' THEN `WK_QTY` END) AS `B204`, 
            SUM(CASE WHEN `MC_NO`='B205' THEN `WK_QTY` END) AS `B205`, 
            SUM(CASE WHEN `MC_NO`='B206' THEN `WK_QTY` END) AS `B206`, 
            SUM(CASE WHEN `MC_NO`='B207' THEN `WK_QTY` END) AS `B207`,
            SUM(CASE WHEN `MC_NO`='B208' THEN `WK_QTY` END) AS `B208`,
            SUM(CASE WHEN `MC_NO`='B209' THEN `WK_QTY` END) AS `B209`,
            SUM(CASE WHEN `MC_NO`='B210' THEN `WK_QTY` END) AS `B210`,
            SUM(CASE WHEN `MC_NO`='B211' THEN `WK_QTY` END) AS `B211`,
            SUM(CASE WHEN `MC_NO`='B212' THEN `WK_QTY` END) AS `B212`,
            SUM(CASE WHEN `MC_NO`='B213' THEN `WK_QTY` END) AS `B213`,
            SUM(CASE WHEN `MC_NO`='B214' THEN `WK_QTY` END) AS `B214`,
            SUM(CASE WHEN `MC_NO`='B215' THEN `WK_QTY` END) AS `B215`,
            SUM(CASE WHEN `MC_NO`='B216' THEN `WK_QTY` END) AS `B216`,
            SUM(CASE WHEN `MC_NO` LIKE 'B%' THEN `WK_QTY` END) AS `CR_SUM` 
        FROM TWWKAR_LP
        WHERE TWWKAR_LP.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR_LP.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
        GROUP BY hour(TWWKAR_LP.`CREATE_DTM`) ORDER BY TWWKAR_LP.`WK_IDX`
    ) AS STB_1
    JOIN (
        SELECT CONCAT(hour(TWWKAR_SPST.`CREATE_DTM`), ' ~ ', hour(TWWKAR_SPST.`CREATE_DTM`) + 1) AS `Hour`,
            SUM(CASE WHEN `MC_NO`='Z101' THEN `WK_QTY` END) AS `Z101`, 
            SUM(CASE WHEN `MC_NO`='Z102' THEN `WK_QTY` END) AS `Z102`, 
            SUM(CASE WHEN `MC_NO`='Z103' THEN `WK_QTY` END) AS `Z103`, 
            SUM(CASE WHEN `MC_NO`='Z104' THEN `WK_QTY` END) AS `Z104`, 
            SUM(CASE WHEN `MC_NO`='Z105' THEN `WK_QTY` END) AS `Z105`, 
            SUM(CASE WHEN `MC_NO`='Z106' THEN `WK_QTY` END) AS `Z106`, 
            SUM(CASE WHEN `MC_NO`='Z107' THEN `WK_QTY` END) AS `Z107`,
            SUM(CASE WHEN `MC_NO`='Z108' THEN `WK_QTY` END) AS `Z108`,
            SUM(CASE WHEN `MC_NO`='Z109' THEN `WK_QTY` END) AS `Z109`,
            SUM(CASE WHEN `MC_NO`='Z110' THEN `WK_QTY` END) AS `Z110`,
            SUM(CASE WHEN `MC_NO`='Z111' THEN `WK_QTY` END) AS `Z111`,
            SUM(CASE WHEN `MC_NO`='Z112' THEN `WK_QTY` END) AS `Z112`,
            SUM(CASE WHEN `MC_NO` LIKE 'Z1%' THEN `WK_QTY` END) AS `TW_SUM`, 
            SUM(CASE WHEN `MC_NO`='S101' THEN `WK_QTY` END) AS `S101`, 
            SUM(CASE WHEN `MC_NO`='S102' THEN `WK_QTY` END) AS `S102`, 
            SUM(CASE WHEN `MC_NO`='S103' THEN `WK_QTY` END) AS `S103`, 
            SUM(CASE WHEN `MC_NO`='S104' THEN `WK_QTY` END) AS `S104`,
            SUM(CASE WHEN `MC_NO`='S105' THEN `WK_QTY` END) AS `S105`,
            SUM(CASE WHEN `MC_NO`='S106' THEN `WK_QTY` END) AS `S106`,
            SUM(CASE WHEN `MC_NO`='S107' THEN `WK_QTY` END) AS `S107`,
            SUM(CASE WHEN `MC_NO`='S108' THEN `WK_QTY` END) AS `S108`,
            SUM(CASE WHEN `MC_NO`='S109' THEN `WK_QTY` END) AS `S109`,
            SUM(CASE WHEN `MC_NO`='S110' THEN `WK_QTY` END) AS `S110`,
            SUM(CASE WHEN `MC_NO`='S111' THEN `WK_QTY` END) AS `S111`,
            SUM(CASE WHEN `MC_NO`='S112' THEN `WK_QTY` END) AS `S112`,
            SUM(CASE WHEN `MC_NO` LIKE 'S1%' THEN `WK_QTY` END) AS `WD_SUM`,
            HOUR(TWWKAR_SPST.`CREATE_DTM`) AS `HOUR_ST`,
            IF(HOUR(TWWKAR_SPST.`CREATE_DTM`) < 6, HOUR(TWWKAR_SPST.`CREATE_DTM`) + 30, HOUR(TWWKAR_SPST.`CREATE_DTM`)) AS `HOUR_ROW`
        FROM TWWKAR_SPST
        WHERE TWWKAR_SPST.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR_SPST.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
        GROUP BY HOUR(TWWKAR_SPST.`CREATE_DTM`) ORDER BY TWWKAR_SPST.`WK_IDX`
    ) AS STB_2
    ON STB_1.Hour = STB_2.Hour
) `TB_A` 
LEFT JOIN (
    SELECT `TB_ZB`.`RNUM`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z101' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z101`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z102' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z102`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z103' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z103`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z104' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z104`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z105' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z105`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z106' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z106`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z107' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z107`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z108' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z108`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z109' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z109`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z110' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z110`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z111' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z111`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='Z112' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_Z112`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO` LIKE 'Z1%' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_TW_SUM`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S101' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S101`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S102' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S102`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S103' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S103`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S104' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S104`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S105' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S105`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S106' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S106`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S107' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S107`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S108' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S108`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S109' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S109`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S110' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S110`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S111' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S111`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='S112' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_S112`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO` LIKE 'S1%' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `W_WD_SUM`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B201' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B201`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B202' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B202`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B203' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B203`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B204' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B204`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B205' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B205`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B206' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B206`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B207' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B207`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B208' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B208`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B209' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B209`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B210' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B210`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B211' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B211`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B212' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B212`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B213' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B213`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B214' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B214`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B215' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B215`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO`='B216' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_B216`,
        SUM(CASE WHEN `TB_ZA`.`MC_NO` LIKE 'B2%' THEN (IF(`TB_ZB`.`RNUM` <= 5, 
            IF(`TB_ZB`.`RNUM` + 24 >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` + 24 <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0),
            IF(`TB_ZB`.`RNUM` >= `TB_ZA`.`S_TIME` AND `TB_ZB`.`RNUM` <= IF(`TB_ZA`.`E_TIME` <= 5, `TB_ZA`.`E_TIME` + 24, `TB_ZA`.`E_TIME`), 1, 0))) END) AS `C_WD_SUM`
    FROM (
        SELECT `TSNON_OPER_MCNM` AS `MC_NO`, `TSNON_OPER_TIME` AS `CREATE_DTM`,
            HOUR(`TSNON_OPER_TIME`) AS `S_TIME`,
            IF(`TSNON_OPER_COL` = 'S', 
                IF(LEAD(`TSNON_OPER_MCNM`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = `TSNON_OPER_MCNM` AND
                   LEAD(`TSNON_OPER_COL`) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`) = 'E', 
                   LEAD(HOUR(`TSNON_OPER_TIME`)) OVER(ORDER BY TSNON_OPER_MCNM, `TSNON_OPER_TIME`), 0), 0) AS `E_TIME`, 
            'NO_WORKER' AS `WK_QTY_S`
        FROM TSNON_OPER_WORKER
        WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= '{DATA_NOW} 06:00:00' AND TSNON_OPER_WORKER.`TSNON_OPER_TIME` < '{DATA_AFD} 06:00:00'
    ) `TB_ZA` 
    JOIN (
        WITH RECURSIVE `CTE` AS (
            SELECT 0 AS `RNUM` 
            UNION ALL
            SELECT `RNUM` + 1 FROM `CTE` WHERE `RNUM` < 23
        )
        SELECT `RNUM` FROM `CTE`
    ) AS `TB_ZB`
    WHERE `TB_ZA`.`E_TIME` > 0
    GROUP BY `TB_ZB`.`RNUM`
) `TB_B`
ON `TB_B`.`RNUM` = `TB_A`.`HOUR_ST` 
ORDER BY `TB_A`.`HOUR_ROW`";

                string DGV_DATA2 = $@"
SELECT 'Total' AS `Hour`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z101' THEN `WK_QTY` END), 0) AS `A801`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z102' THEN `WK_QTY` END), 0) AS `A802`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z103' THEN `WK_QTY` END), 0) AS `A803`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z104' THEN `WK_QTY` END), 0) AS `A804`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z105' THEN `WK_QTY` END), 0) AS `A805`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z106' THEN `WK_QTY` END), 0) AS `A806`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='Z107' THEN `WK_QTY` END), 0) AS `A807`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z108' THEN `WK_QTY` END), 0) AS `A808`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z109' THEN `WK_QTY` END), 0) AS `A809`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z110' THEN `WK_QTY` END), 0) AS `A810`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z111' THEN `WK_QTY` END), 0) AS `A811`,
    IFNULL(SUM(CASE WHEN `MC_NO`='Z112' THEN `WK_QTY` END), 0) AS `A812`,
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'Z1%' THEN `WK_QTY` END), 0) AS `A813`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='S101' THEN `WK_QTY` END), 0) AS `A501`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='S102' THEN `WK_QTY` END), 0) AS `A502`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='S103' THEN `WK_QTY` END), 0) AS `Coln1`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='S104' THEN `WK_QTY` END), 0) AS `Coln2`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S105' THEN `WK_QTY` END), 0) AS `Coln3`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S106' THEN `WK_QTY` END), 0) AS `Coln4`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S107' THEN `WK_QTY` END), 0) AS `Coln5`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S108' THEN `WK_QTY` END), 0) AS `Coln6`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S109' THEN `WK_QTY` END), 0) AS `Coln7`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S110' THEN `WK_QTY` END), 0) AS `D01`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S111' THEN `WK_QTY` END), 0) AS `D02`,
    IFNULL(SUM(CASE WHEN `MC_NO`='S112' THEN `WK_QTY` END), 0) AS `D03`,
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'S1%' THEN `WK_QTY` END), 0) AS `D04`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B201' THEN `WK_QTY` END), 0) AS `D05`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B202' THEN `WK_QTY` END), 0) AS `D06`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B203' THEN `WK_QTY` END), 0) AS `D07`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B204' THEN `WK_QTY` END), 0) AS `D08`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B205' THEN `WK_QTY` END), 0) AS `D09`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B206' THEN `WK_QTY` END), 0) AS `D10`, 
    IFNULL(SUM(CASE WHEN `MC_NO`='B207' THEN `WK_QTY` END), 0) AS `D11`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B208' THEN `WK_QTY` END), 0) AS `D12`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B209' THEN `WK_QTY` END), 0) AS `D13`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B210' THEN `WK_QTY` END), 0) AS `D14`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B211' THEN `WK_QTY` END), 0) AS `D15`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B212' THEN `WK_QTY` END), 0) AS `D16`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B213' THEN `WK_QTY` END), 0) AS `D17`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B214' THEN `WK_QTY` END), 0) AS `D18`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B215' THEN `WK_QTY` END), 0) AS `D19`,
    IFNULL(SUM(CASE WHEN `MC_NO`='B216' THEN `WK_QTY` END), 0) AS `D20`,
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'B%' THEN `WK_QTY` END), 0) AS `D21`,
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'Z1%' THEN `WK_QTY` END), 0) + 
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'S1%' THEN `WK_QTY` END), 0) + 
    IFNULL(SUM(CASE WHEN `MC_NO` LIKE 'B%' THEN `WK_QTY` END), 0) AS `SUM`
FROM (
    SELECT `MC_NO`, `WK_QTY`, `CREATE_DTM`
    FROM TWWKAR_SPST
    WHERE TWWKAR_SPST.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR_SPST.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
    UNION ALL
    SELECT `MC_NO`, `WK_QTY`, `CREATE_DTM`
    FROM TWWKAR_LP
    WHERE TWWKAR_LP.`CREATE_DTM` >= '{DATA_NOW} 06:00:00' AND TWWKAR_LP.`CREATE_DTM` < '{DATA_AFD} 06:00:00'
) AS combined";

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