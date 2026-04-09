
namespace MESService.Implement
{
    public class C02_MTService : BaseService<torderlist, ItorderlistRepository>
    , IC02_MTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C02_MTService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C02_MT_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        string sqlResult = "";
                        var ORDR = BaseParameter.ListSearchString[0];

                        string sql = @"SELECT  `ORDER_IDX`, `LEAD_NO`, `TOT_QTY`,
`TERM1`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`TERM1`)), 0) > 0, 1, 0) * `TOT_QTY` AS `T1_QTY`, 
`TERM2`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.TERM2)), 0) > 0, 1, 0) * `TOT_QTY`  AS `T2_QTY`, 
`SEAL1`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`SEAL1`)), 0) > 0, 1, 0) * `TOT_QTY` AS `S1_QTY`, 
`SEAL2`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.SEAL2)), 0) > 0, 1, 0) * `TOT_QTY`  AS `S2_QTY`, 
`WIRE`, (SELECT `PART_NO` FROM tspart WHERE `PART_IDX`=(SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`))) AS `WIRE_NM`,
(SELECT `W_Length`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)) AS `W_Length`,
(IF(IFNULL((SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)), 0) > 0, (SELECT `W_Length`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)), 0) *  `TOT_QTY` / 1000)  AS `W_Length1`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_CNF_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`TERM1`)), '-') AS `T1_ORDER`,
IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_DSCN_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`TERM1`)), '-') AS `T1_COMP`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_CNF_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`TERM2`)), '-') AS `T2_ORDER`,
IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_DSCN_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`TERM2`)), '-') AS `T2_COMP`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_CNF_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`SEAL1`)), '-') AS `S1_ORDER`,
IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_DSCN_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`SEAL1`)), '-') AS `S1_COMP`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_CNF_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`SEAL2`)), '-') AS `S2_ORDER`,
IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_DSCN_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = TORDERLIST.`SEAL2`)), '-') AS `S2_COMP`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_CNF_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE `LEAD_PN` = TORDERLIST.`LEAD_NO`)), '-') AS `WR_ORDER`,

IFNULL((SELECT TMMTIN_DMM_CUT.`TMMTIN_DSCN_YN` FROM TMMTIN_DMM_CUT WHERE TMMTIN_DMM_CUT.`TMMTIN_ORDERNO` = `ORDER_IDX` AND 	  
TMMTIN_DMM_CUT.`TMMTIN_PART` = (SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE `LEAD_PN` = TORDERLIST.`LEAD_NO`)), '-') AS `WR_COMP`
FROM  TORDERLIST WHERE `ORDER_IDX` = '" + ORDR + "'   ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        string sqlResult = "";
                        var ORDR = BaseParameter.ListSearchString[0];
                        var SettingsMC_NM = BaseParameter.ListSearchString[1];

                        string sql = @"SELECT  
(CASE WHEN `K`.`TYPE` = 'T1' THEN IFNULL(`M`.`T1_IDX`, 0) 
     WHEN `K`.`TYPE` = 'T2' THEN IFNULL(`M`.`T2_IDX` , 0) 
     WHEN `K`.`TYPE` = 'S1' THEN IFNULL(`M`.`S1_IDX` , 0) 
	  WHEN `K`.`TYPE` = 'S2' THEN IFNULL(`M`.`S2_IDX` , 0) 
	  WHEN `K`.`TYPE` = 'WIRE' THEN `M`.`WIRE_IDX`   END) AS `IDX`,
	  
(CASE WHEN `K`.`TYPE` = 'T1' THEN `M`.`TERM1` 
     WHEN `K`.`TYPE` = 'T2' THEN `M`.`TERM2` 
     WHEN `K`.`TYPE` = 'S1' THEN `M`.`SEAL1` 
	  WHEN `K`.`TYPE` = 'S2' THEN `M`.`SEAL2` 
	  WHEN `K`.`TYPE` = 'WIRE' THEN `M`.`WIRE_NM`   END) AS `NAME`,
	  
IFNULL((CASE WHEN `K`.`TYPE` = 'T1' THEN (SELECT `PART_SNP` FROM tspart WHERE `PART_IDX`=`M`.`T1_IDX`) 
     WHEN `K`.`TYPE` = 'T2' THEN  (SELECT `PART_SNP` FROM tspart WHERE `PART_IDX`=`M`.`T2_IDX`) 
     WHEN `K`.`TYPE` = 'S1' THEN  (SELECT `PART_SNP` FROM tspart WHERE `PART_IDX`=`M`.`S1_IDX`) 
	  WHEN `K`.`TYPE` = 'S2' THEN  (SELECT `PART_SNP` FROM tspart WHERE `PART_IDX`=`M`.`S2_IDX`) 
	  WHEN `K`.`TYPE` = 'WIRE' THEN (SELECT `PART_SNP` FROM tspart WHERE `PART_IDX`=`M`.`WIRE_IDX`)    END), 0) AS `SNP`,
	  
IFNULL(SUM(CASE WHEN `K`.`TYPE` = 'T1' THEN `M`.`T1_QTY` 
     WHEN `K`.`TYPE` = 'T2' THEN `M`.`T2_QTY` 
     WHEN `K`.`TYPE` = 'S1' THEN `M`.`S1_QTY` 
	  WHEN `K`.`TYPE` = 'S2' THEN `M`.`S2_QTY` 
	  WHEN `K`.`TYPE` = 'WIRE' THEN `M`.`W_QTY`   END), 0) AS `QTY`, `K`.`TYPE` 
	  	  
FROM (
SELECT  `ORDER_IDX`, `LEAD_NO`, `TOT_QTY`,
`TERM1`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`TERM1`)), 0) > 0, 1, 0) * `TOT_QTY` AS `T1_QTY`,
(SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`TERM1`)) AS `T1_IDX`, 
`TERM2`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.TERM2)), 0) > 0, 1, 0) * `TOT_QTY`  AS `T2_QTY`, 
(SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`TERM2`)) AS `T2_IDX`, 
`SEAL1`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`SEAL1`)), 0) > 0, 1, 0) * `TOT_QTY` AS `S1_QTY`,
(SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`SEAL1`)) AS `S1_IDX`, 
`SEAL2`, IF(IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.SEAL2)), 0) > 0, 1, 0) * `TOT_QTY`  AS `S2_QTY`, 
(SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`SEAL2`)) AS `S2_IDX`, 
`WIRE`, (SELECT `PART_NO` FROM tspart WHERE `PART_IDX`=(SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`))) AS `WIRE_NM`,
(SELECT `W_Length`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)) AS `W_Length`,
(IF(IFNULL((SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)), 0) > 0, (SELECT `W_Length`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)), 0) *  `TOT_QTY` / 1000)  AS `W_QTY`,
(SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`)) AS `WIRE_IDX`
FROM  TORDERLIST WHERE `ORDER_IDX` = '" + ORDR + "') `M` JOIN(SELECT * FROM(SELECT 'T1' AS `TYPE` UNION SELECT 'T2' AS `TYPE` UNION SELECT 'S1' AS `TYPE` UNION SELECT 'S2' AS `TYPE` UNION SELECT 'WIRE') `L` ) `K` GROUP BY `IDX`    ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count == 0)
                        {
                            IsCheck = false;
                        }
                        if (IsCheck == true)
                        {
                            var VAL_DT = "";
                            var VAL_SUM = "";

                            foreach (var item in result.Search)
                            {
                                var AAA = SettingsMC_NM;
                                var CCC = item.IDX;
                                var DDD = item.SNP;
                                var EEE = item.QTY;
                                var GGG = "Material";
                                var HHH = item.TYPE;

                                if (EEE > 0)
                                {
                                    var CB_TYPE = HHH;
                                    switch (CB_TYPE)
                                    {
                                        case "T1":
                                            if (BaseParameter.TM_CB1 == true)
                                            {
                                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), NOW(),'" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + C_USER + "', '" + ORDR + "')";
                                            }
                                            break;
                                        case "T2":
                                            if (BaseParameter.TM_CB2 == true)
                                            {
                                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), NOW(),'" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + C_USER + "', '" + ORDR + "')";
                                            }
                                            break;
                                        case "S1":
                                            if (BaseParameter.SL_CB1 == true)
                                            {
                                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), NOW(),'" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + C_USER + "', '" + ORDR + "')";
                                            }
                                            break;
                                        case "S2":
                                            if (BaseParameter.SL_CB1 == true)
                                            {
                                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), NOW(),'" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + C_USER + "', '" + ORDR + "')";
                                            }
                                            break;
                                        case "WIRE":
                                            if (BaseParameter.WR_CB == true)
                                            {
                                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), NOW(),'" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + C_USER + "', '" + ORDR + "')";
                                            }
                                            break;
                                    }
                                    if (VAL_SUM.Length == 0)
                                    {
                                        VAL_SUM = VAL_DT;
                                    }
                                    else
                                    {
                                        VAL_SUM = VAL_SUM + ", " + VAL_DT;
                                    }
                                }
                            }
                            if (VAL_SUM.Length > 0)
                            {
                                sql = @"INSERT INTO `TMMTIN_DMM_CUT` (`TMMTIN_DMM_STGC`, `TMMTIN_DATE`, `TMMTIN_PART`, `TMMTIN_PART_SNP`, `TMMTIN_QTY`, `TMMTIN_CODE`, `TMMTIN_REC_YN`, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `TMMTIN_ORDERNO`) VALUES  " + VAL_SUM;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            sql = @"UPDATE `TORDERLIST` SET `MTRL_RQUST`='Y' WHERE  `ORDER_IDX`= '" + ORDR + "'  ";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
    }
}

