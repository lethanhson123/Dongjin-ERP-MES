

namespace MESService.Implement
{
    public class G03Service : BaseService<torderlist, ItorderlistRepository>
    , IG03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public G03Service(ItorderlistRepository torderlistRepository

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
                if (BaseParameter != null)
                {
                    var CCC = BaseParameter.SearchString;
                    var HAVING_SQL = "";
                    if (BaseParameter.CheckBox1 == true)
                    {
                        HAVING_SQL = " AND NOT(`BOM_GRUP` ='MT' OR `ST` = 'WIRE' OR `ST` = 'T1' OR `ST` = 'S1' OR `ST` = 'T2' OR `ST` = 'S2') ";
                    }
                    string sql = @"SELECT
                        (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` = `BOM_DB`.`PAREN_PART_IDX`) AS `FG_PART_NO`,
                        `BOM_DB`.`LEVEL`,
                        `BOM_DB`.`S_PART_NO`,
                        `BOM_DB`.`ST`,
                        `BOM_DB`.`BOM_GRUP`,
                        `BOM_DB`.`B_PART_NO`,
                        IF(`BOM_DB`.`BOM_GRUP` ='ASSY', (SELECT tspart_ecn.PART_ENCNO FROM tspart_ecn WHERE tspart_ecn.PARTECN_IDX = `BOM_DB`.`PAREN_EO_IDX`), '') AS `EONO`,
                        `BOM_DB`.`PART_NM`,
                        (ROUND((IF(`BOM_DB`.`LEVEL` ='0', 1, IF(`BOM_DB`.`LEVEL` ='1', `BOM_DB`.`LEAD_MOQ`, 
                        (`BOM_DB`.`LEAD_MOQ` * IF(`BOM_DB`.`ST` = 'WIRE', `BOM_DB`.`MT_MOQ` /1000, `BOM_DB`.`MT_MOQ`))))), 3)) AS `MOQ`,
                        `BOM_DB`.`D0` AS `PO`,
                        IFNULL(ROUND((IF(`BOM_DB`.`LEVEL` ='0', 1, IF(`BOM_DB`.`LEVEL` ='1', `BOM_DB`.`LEAD_MOQ`, 
                        (`BOM_DB`.`LEAD_MOQ` * IF(`BOM_DB`.`ST` = 'WIRE', `BOM_DB`.`MT_MOQ` /1000, `BOM_DB`.`MT_MOQ`)))) * (`BOM_DB`.`D0`)), 3), 0)  AS `REQ`,

                        IFNULL(IF(`BOM_DB`.`LEVEL` = 0, (SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = `BOM_DB`.`PAREN_PART_IDX` AND `LOC_IDX` = '2'),
	                        IF(`BOM_DB`.`ST` ='LEAD', (SELECT tiivtr_lead.QTY FROM tiivtr_lead WHERE tiivtr_lead.PART_IDX = `BOM_DB`.`S_PARTIDX` AND tiivtr_lead.`LOC_IDX` = '3'),
	                        (SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = `BOM_DB`.`S_PARTIDX` AND `LOC_IDX` = '1'))), 0) AS `STOCK_QTY`,

                        (IFNULL(IF(`BOM_DB`.`LEVEL` = 0, (SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = `BOM_DB`.`PAREN_PART_IDX` AND `LOC_IDX` = '2'),
	                        IF(`BOM_DB`.`ST` ='LEAD', (SELECT tiivtr_lead.QTY FROM tiivtr_lead WHERE tiivtr_lead.PART_IDX = `BOM_DB`.`S_PARTIDX` AND tiivtr_lead.`LOC_IDX` = '3'),
	                        (SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = `BOM_DB`.`S_PARTIDX` AND `LOC_IDX` = '1'))), 0) -
	                        IFNULL(ROUND((IF(`BOM_DB`.`LEVEL` ='0', 1, IF(`BOM_DB`.`LEVEL` ='1', `BOM_DB`.`LEAD_MOQ`, 
                           (`BOM_DB`.`LEAD_MOQ` * IF(`BOM_DB`.`ST` = 'WIRE', `BOM_DB`.`MT_MOQ` /1000, `BOM_DB`.`MT_MOQ`)))) * (`BOM_DB`.`D0`)), 3), 0) ) AS `GAP`,

                        `BOM_DB`.`DSYN`,
                        IF(`BOM_DB`.`LEVEL` = 0, 'FG', IFNULL((SELECT tsbom_ver02.`BOM_RMK` FROM tsbom_ver02 WHERE tsbom_ver02.`BOM_IDX` = `BOM_DB`.`IDX_FULL`), '')) AS `USED`
                        FROM(
                        WITH RECURSIVE CTE AS (
                        SELECT
                        `TA`.`LEV`,
                        `TA`.`PAREN_PART_IDX`,
                        `TA`.`PART_IDX1`,
                        `TA`.`PART_IDX2`,
                        `TA`.`DES`,
                        `TA`.`PATH`,
                        `TA`.`BOM_GRUP`,
                        `TA`.`BOM_IDX`,
                        `TA`.`PAREN_EO_IDX`,
                        `TA`.`DSYN`,
                        `TA`.`D0`
                        FROM 
                        (SELECT  
                        0 AS `LEV`, tsbom_ver02.PAREN_PART_IDX, tsbom_ver02.PAREN_PART_IDX AS `PART_IDX1`, 0 AS `PART_IDX2`, 1 AS `DES`,
                        CONCAT('A', tsbom_ver02.PAREN_PART_IDX, '-', LPAD(0, 5, '0')) AS `PATH`,
                        'ASSY' AS BOM_GRUP,  tsbom_ver02.`BOM_IDX`, tsbom_ver02.PAREN_EO_IDX, tsbom_ver02.`DSYN`, `PODB`.`D0`
                        FROM tsbom_ver02 JOIN 
                        (SELECT TSBOM_PO_LIST.BOM_PO_LIST_IDX,   
                        (SELECT tspart.`PART_IDX` FROM tspart WHERE  tspart.`PART_NO` = TSBOM_PO_LIST.PART_NO) AS `PNIDX`,
                        TSBOM_PO_LIST.PART_NO,  TSBOM_PO_LIST.PO_QTY AS `D0`, `TM_A`.`PART_ENCNO`, `TM_A`.`PARTECN_IDX`  
                        FROM TSBOM_PO_LIST  LEFT JOIN
                        (SELECT  `TB_A`.`NO`, `TB_A`.`PART_IDX`, `TB_A`.`PART_ENCNO`, `TB_A`.`PART_ECN_DATE`, `TB_A`.`PARTECN_IDX`  
                        FROM (SELECT ROW_NUMBER() OVER (PARTITION BY tspart_ecn.`PART_IDX` ORDER BY  tspart_ecn.`PART_ECN_DATE` DESC) AS `NO`,
                        tspart_ecn.`PART_IDX`, tspart_ecn.`PART_ENCNO`, tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ECN_USENY`, tspart_ecn.PARTECN_IDX
                        FROM tspart_ecn
                        WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' ) `TB_A`
                        WHERE `TB_A`.`NO` = '1') `TM_A`
                        ON (SELECT tspart.`PART_IDX` FROM tspart WHERE  tspart.`PART_NO` = TSBOM_PO_LIST.PART_NO) = `TM_A`.`PART_IDX`
                        WHERE TSBOM_PO_LIST.DSYN = 'Y') `PODB`
                        ON tsbom_ver02.`PAREN_PART_IDX` = `PODB`.`PNIDX` AND tsbom_ver02.PAREN_EO_IDX = `PODB`.`PARTECN_IDX`
                        GROUP BY tsbom_ver02.`PAREN_PART_IDX`, tsbom_ver02.`PAREN_EO_IDX`

                        UNION ALL
                        SELECT
                        1 AS `LEV`, tsbom_ver02.PAREN_PART_IDX, tsbom_ver02.PAREN_PART_IDX AS `PART_IDX1`, tsbom_ver02.PART_IDX AS `PART_IDX2`, tsbom_ver02.BOM_DES,
                        CONCAT('A', tsbom_ver02.PAREN_PART_IDX, '-', LPAD(ROW_NUMBER() OVER (ORDER BY (SELECT tspart.`PART_FML` FROM tspart WHERE  tspart.`PART_IDX` = `PART_IDX2`)), 5, '0'))  AS `PATH`,
                        tsbom_ver02.`BOM_GRUP`, tsbom_ver02.`BOM_IDX`, tsbom_ver02.PAREN_EO_IDX, tsbom_ver02.`DSYN`,
                        `PODB`.`D0`
                        FROM tsbom_ver02 JOIN 
                        (SELECT TSBOM_PO_LIST.BOM_PO_LIST_IDX,   
                        (SELECT tspart.`PART_IDX` FROM tspart WHERE  tspart.`PART_NO` = TSBOM_PO_LIST.PART_NO) AS `PNIDX`,
                        TSBOM_PO_LIST.PART_NO,  TSBOM_PO_LIST.PO_QTY AS `D0`, `TM_A`.`PART_ENCNO`, `TM_A`.`PARTECN_IDX`  
                        FROM TSBOM_PO_LIST  LEFT JOIN
                        (SELECT  `TB_A`.`NO`, `TB_A`.`PART_IDX`, `TB_A`.`PART_ENCNO`, `TB_A`.`PART_ECN_DATE`, `TB_A`.`PARTECN_IDX`  
                        FROM (SELECT ROW_NUMBER() OVER (PARTITION BY tspart_ecn.`PART_IDX` ORDER BY  tspart_ecn.`PART_ECN_DATE` DESC) AS `NO`,
                        tspart_ecn.`PART_IDX`, tspart_ecn.`PART_ENCNO`, tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ECN_USENY`, tspart_ecn.PARTECN_IDX
                        FROM tspart_ecn
                        WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' ) `TB_A`
                        WHERE `TB_A`.`NO` = '1') `TM_A`
                        ON (SELECT tspart.`PART_IDX` FROM tspart WHERE  tspart.`PART_NO` = TSBOM_PO_LIST.PART_NO) = `TM_A`.`PART_IDX`
                        WHERE TSBOM_PO_LIST.DSYN = 'Y') `PODB`
                        ON tsbom_ver02.`PAREN_PART_IDX` = `PODB`.`PNIDX` AND tsbom_ver02.PAREN_EO_IDX = `PODB`.`PARTECN_IDX`
                        ) `TA`  

                        UNION ALL
                        SELECT
                        `TA`.`LEV` + 1 AS `LEV`, 
                        `TA`.`PAREN_PART_IDX`,  `TC`.M_PART_IDX AS `PART_IDX1`, `TC`.`S_PART_IDX` AS `PART_IDX2`, `TC`.`RQR_MENT`,
                        CONCAT(`TA`.`PATH`, '-', LPAD(@rownum := @rownum + 1, 5, '0')) AS `PATH`, 'LEAD' AS `BOM_GRUP`, '' AS `BOM_IDX`, `TA`.`PAREN_EO_IDX` AS `PAREN_EO_IDX`, ''  AS `DSYN`,
                        `TA`.`D0`
                        FROM CTE `TA`
                        JOIN torder_lead_bom_spst `TC` ON `TC`.`M_PART_IDX` = `TA`.`PART_IDX2`, (SELECT @rownum := 0) R  )

                        SELECT 
                        (`CTE`.`LEV` + IF(IFNULL(`BOM_CUT`.`ST`, '') = '', 0, IF(`BOM_CUT`.`ST` = 'LEAD', 0, 1))) AS `LEVEL`,

                        IF(`CTE`.`BOM_GRUP` = 'MT', 
                           CONCAT(REPEAT(' ', (`CTE`.`LEV` + IF(IFNULL(`BOM_CUT`.`ST`, '') = '', 0, IF(`BOM_CUT`.`ST` = 'LEAD', 0, 1))) * 8), 
	                         (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PART_IDX2`)), 
                           IF(`CTE`.`LEV` = 0, (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PAREN_PART_IDX`), 
                           CONCAT(REPEAT(' ', (`CTE`.`LEV` + IF(IFNULL(`BOM_CUT`.`ST`, '') = '', 0, IF(`BOM_CUT`.`ST` = 'LEAD', 0, 1))) * 8),
                           IFNULL(`BOM_CUT`.`PART_NO`, IFNULL((SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX2`),
                           (SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX1`)))))) AS `S_PART_NO`,

                        IF(`CTE`.`BOM_GRUP` = 'MT', (SELECT tspart.`PART_FML` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PART_IDX2`),
                           IF(`CTE`.`LEV` = 0, (SELECT tspart.`PART_NM` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PAREN_PART_IDX`), 
                           IFNULL(`BOM_CUT`.`ST`, (SELECT torder_lead_bom.LEAD_SCN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX2`)))) AS `ST`,

                        `CTE`.`BOM_GRUP`,

                        IF(`CTE`.`BOM_GRUP` = 'MT',   
                           (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PART_IDX2`),
                           IF(`CTE`.`LEV` = 0, (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PAREN_PART_IDX`), 
                           IFNULL(`BOM_CUT`.`PART_NO`, IFNULL((SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX2`),
                           (SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX1`))))) AS `B_PART_NO`,

                        IFNULL(IF(`CTE`.`BOM_GRUP` = 'MT', (SELECT tspart.`PART_NM` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PART_IDX2`),
                           IF(`CTE`.`LEV` = 0, (SELECT tspart.`PART_NM` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PAREN_PART_IDX`), 
                           (SELECT tspart.`PART_NM` FROM tspart WHERE  tspart.`PART_IDX` = `BOM_CUT`.`S_PARTIDX`))), '') AS `PART_NM`,

                        CAST(IFNULL(`CTE`.`DES`, 0) AS DOUBLE) AS `LEAD_MOQ`,
                        CAST(IFNULL(`BOM_CUT`.`MOQ`, 0) AS DOUBLE) AS `MT_MOQ`,

                        `CTE`.`PAREN_PART_IDX`,
                        `CTE`.`PART_IDX1`,
                        `CTE`.`PART_IDX2`,
                        IFNULL(`BOM_CUT`.`PART_IDX`, `CTE`.`PART_IDX2`) AS `PART_IDX`,
                        IFNULL(`BOM_CUT`.`S_PARTIDX`, `CTE`.`PART_IDX2`) AS `S_PARTIDX`,

                        IFNULL(IF(`CTE`.`BOM_GRUP` = 'MT', (SELECT tspart.`PART_FML` FROM tspart WHERE  tspart.`PART_IDX` = `CTE`.`PART_IDX2`),
                        IFNULL((SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX2`),
                        (SELECT torder_lead_bom.LEAD_PN FROM torder_lead_bom WHERE torder_lead_bom.LEAD_INDEX = `CTE`.`PART_IDX1`))), '') AS `GRP_LEAD_NM`,
                        `CTE`.`PATH`,
                        `CTE`.`BOM_IDX` AS `IDX_FULL`, 
                        IF((`CTE`.`LEV` + IF(IFNULL(`BOM_CUT`.`ST`, '') = '', 0, IF(`BOM_CUT`.`ST` = 'LEAD', 0, 1))) = 1, `CTE`.`BOM_IDX`, 0) AS `IDX`,
                        `CTE`.`PAREN_EO_IDX` AS `PAREN_EO_IDX`, `CTE`.`DSYN`,
                        `CTE`.`D0`
                        FROM CTE

                        LEFT JOIN                    

                        (SELECT
                        `M_PIDX`.`ST`,
                        `M_PIDX`.`PART_IDX`,
                        `M_PIDX`.`S_PARTIDX`,
                        IF(`M_PIDX`.`ST` ='LEAD', (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = `M_PIDX`.`S_PARTIDX` ), 
                                (SELECT tspart.`PART_NO` FROM tspart WHERE  tspart.`PART_IDX` =`M_PIDX`.`S_PARTIDX`)) AS `PART_NO`,
                        `M_PIDX`.`MOQ`,
                        `M_PIDX`.`X`
                        FROM(
                        SELECT
                        (CASE WHEN `MT_LIST`.`X` = 0 THEN 'LEAD' WHEN `MT_LIST`.`X` = 1 THEN 'WIRE' WHEN `MT_LIST`.`X` = 2 THEN 'T1' 
                              WHEN `MT_LIST`.`X` = 3 THEN 'S1'   WHEN `MT_LIST`.`X` = 4 THEN 'T2'   WHEN `MT_LIST`.`X` = 5 THEN 'S2' END) AS `ST`,    
                        `LD_LIST`.`LEAD_INDEX` AS `PART_IDX`,  
                        (CASE WHEN `MT_LIST`.`X` = 0 THEN `LD_LIST`.`LEAD_INDEX`
                              WHEN `MT_LIST`.`X` = 1 THEN `LD_LIST`.`W_PN_IDX` 
                              WHEN `MT_LIST`.`X` = 2 THEN `LD_LIST`.`T1_PN_IDX`
                              WHEN `MT_LIST`.`X` = 3 THEN `LD_LIST`.`S1_PN_IDX`   
		                        WHEN `MT_LIST`.`X` = 4 THEN `LD_LIST`.`T2_PN_IDX`   
		                        WHEN `MT_LIST`.`X` = 5 THEN `LD_LIST`.`S2_PN_IDX` END) AS `S_PARTIDX`,
                        (CASE WHEN `MT_LIST`.`X` = 0 THEN 1   WHEN `MT_LIST`.`X` = 1 THEN `LD_LIST`.`W_Length`   WHEN `MT_LIST`.`X` = 2 THEN 1 WHEN `MT_LIST`.`X` = 3 THEN 1
                        WHEN `MT_LIST`.`X` = 4 THEN 1 WHEN `MT_LIST`.`X` = 5 THEN 1 END) AS `MOQ`,  `MT_LIST`.`X`
                        FROM (
                        (SELECT torder_lead_bom.`LEAD_INDEX`,
                        torder_lead_bom.`LEAD_SCN`, torder_lead_bom.`LEAD_PN`, torder_lead_bom.`W_PN_IDX`, torder_lead_bom.`T1_PN_IDX`,
                        torder_lead_bom.`S1_PN_IDX`, torder_lead_bom.`T2_PN_IDX`, torder_lead_bom.`S2_PN_IDX`, torder_lead_bom.`W_Length`
                        FROM torder_lead_bom
                        WHERE torder_lead_bom.`LEAD_SCN` ='LEAD') `LD_LIST`, 
                        (SELECT 0 AS `X`
                        UNION ALL SELECT 1 AS `X`
                        UNION ALL SELECT 2 AS `X`
                        UNION ALL SELECT 3 AS `X`
                        UNION ALL SELECT 4 AS `X`
                        UNION ALL SELECT 5 AS `X`) `MT_LIST`)
                        ) `M_PIDX`
                        WHERE NOT(`M_PIDX`.`S_PARTIDX` IS NULL) )`BOM_CUT`
                        ON `BOM_CUT`.`PART_IDX` = `CTE`.`PART_IDX2` AND `CTE`.`BOM_GRUP` = 'LEAD'
                        ORDER BY `PATH`, `BOM_GRUP`, `LEV`    
                         )  `BOM_DB` 
                        HAVING `B_PART_NO` LIKE '%" + CCC + "%'  " + HAVING_SQL + " ORDER BY `PATH`, `BOM_GRUP`, `LEVEL`   ";




                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView3 != null)
                    {
                        if (BaseParameter.DataGridView3.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView3)
                            {
                                var IDX = item.BOM_PO_LIST_IDX;
                                var QTY = item.PO_QTY;

                                string sql = @"UPDATE  `TSBOM_PO_LIST` SET `PO_QTY`= " + QTY + " WHERE  `BOM_PO_LIST_IDX`= " + IDX;
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
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
        public virtual async Task<BaseResult> DGV3_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 
                (SELECT tspart.`BOM_GRP` FROM tspart WHERE tspart.`PART_NO` = TSBOM_PO_LIST.`PART_NO`) AS `GRP`, 
                TSBOM_PO_LIST.`PART_NO`, `TM_A`.`PART_ENCNO`,  TSBOM_PO_LIST.`PO_QTY`, `TM_A`.`PART_ECN_DATE`, TSBOM_PO_LIST.BOM_PO_LIST_IDX
                FROM TSBOM_PO_LIST JOIN
                (SELECT 
                `TB_A`.`NO`, `TB_A`.`PART_IDX`, `TB_A`.`PART_ENCNO`, `TB_A`.`PART_ECN_DATE` 
                FROM ( SELECT 
                ROW_NUMBER() OVER (PARTITION BY tspart_ecn.`PART_IDX` ORDER BY  tspart_ecn.`PART_ECN_DATE` DESC) AS `NO`,
                tspart_ecn.`PART_IDX`, tspart_ecn.`PART_ENCNO`, tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ECN_USENY`  FROM tspart_ecn
                WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' ) `TB_A`
                WHERE `TB_A`.`NO` = '1') `TM_A`
                ON TSBOM_PO_LIST.`PART_NO` = (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `TM_A`.`PART_IDX`)
                WHERE TSBOM_PO_LIST.`DSYN` = 'Y' AND
                NOT((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = TSBOM_PO_LIST.`PART_NO`) IS NULL)
                ORDER BY  `GRP`, `PART_NO` ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView3 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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


