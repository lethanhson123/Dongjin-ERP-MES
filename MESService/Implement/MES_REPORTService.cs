namespace MESService.Implement
{
    public class MES_REPORTService : BaseService<torderlist, ItorderlistRepository>
    , IMES_REPORTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public MES_REPORTService(ItorderlistRepository torderlistRepository

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
        public async Task<BaseResult> T1_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 
                            'ALL' AS `FCTRY_NM`,
                            `B`.`WEEK`,
                            DATE_FORMAT(MIN(`B`.`MIN_DT`), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(`B`.`MAX_DT`), '%Y-%m-%d') AS `MAX_DT`,
                            SUM(`B`.`LEAD_COUNT`) AS `LEAD_COUNT`,
                            SUM(`B`.`COUNT`) AS `COUNT`,
                            SUM(`B`.`NOT_WORK`) AS `NOT_WORK`,
                            CEILING((SUM(`B`.`COUNT`) / SUM(`B`.`LEAD_COUNT`))  * 100) AS `LEAD_RAT`,
                            SUM(`B`.`PO_SUM`) AS `PO_SUM`,
                            SUM(`B`.`ACT_SUM`) AS `ACT_SUM`,
                            CEILING((SUM(`B`.`ACT_SUM`) / SUM(`B`.`PO_SUM`)  * 100 ))AS `ACT_RAT`

                            FROM(
                            SELECT 'ALL'AS `FCTRY_NM`,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST.PERFORMN) / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM

                            UNION
                            SELECT 'ALL'AS `FCTRY_NM`,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R))/2 AS `ACT_SUM`,
                            CEILING((SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R)) / 2 / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST JOIN TORDERLIST_LP
                            ON TORDERLIST.ORDER_IDX = TORDERLIST_LP.ORDER_IDX
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM

                            UNION
                            SELECT 'ALL'AS `FCTRY_NM`,
                            WEEK(TORDERLIST_SPST.PO_DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST_SPST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST_SPST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST_SPST.PO_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST_SPST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST_SPST.PERFORMN) / SUM(TORDERLIST_SPST.PO_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST_SPST
                            WHERE WEEK(TORDERLIST_SPST.PO_DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST_SPST.PO_DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST_SPST.DSCN_YN = 'Y' AND TORDERLIST_SPST.PO_QTY > 0
                            GROUP BY WEEK(TORDERLIST_SPST.PO_DT), TORDERLIST_SPST.FCTRY_NM) `B`

                            GROUP BY `B`.`WEEK`, `B`.`FCTRY_NM`

                            UNION
                            SELECT 
                            `B`.`FCTRY_NM`,
                            `B`.`WEEK`,
                            DATE_FORMAT(MIN(`B`.`MIN_DT`), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(`B`.`MAX_DT`), '%Y-%m-%d') AS `MAX_DT`,
                            SUM(`B`.`LEAD_COUNT`) AS `LEAD_COUNT`,
                            SUM(`B`.`COUNT`) AS `COUNT`,
                            SUM(`B`.`NOT_WORK`) AS `NOT_WORK`,
                            CEILING((SUM(`B`.`COUNT`) / SUM(`B`.`LEAD_COUNT`))  * 100) AS `LEAD_RAT`,
                            SUM(`B`.`PO_SUM`) AS `PO_SUM`,
                            SUM(`B`.`ACT_SUM`) AS `ACT_SUM`,
                            CEILING((SUM(`B`.`ACT_SUM`) / SUM(`B`.`PO_SUM`)  * 100 ))AS `ACT_RAT`

                            FROM(
                            SELECT TORDERLIST.FCTRY_NM,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST.PERFORMN) / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM
                            UNION
                            SELECT TORDERLIST.FCTRY_NM,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R))/2 AS `ACT_SUM`,
                            CEILING((SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R)) / 2 / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST JOIN TORDERLIST_LP
                            ON TORDERLIST.ORDER_IDX = TORDERLIST_LP.ORDER_IDX
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM

                            UNION
                            SELECT TORDERLIST_SPST.FCTRY_NM,
                            WEEK(TORDERLIST_SPST.PO_DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST_SPST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST_SPST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST_SPST.PO_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST_SPST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST_SPST.PERFORMN) / SUM(TORDERLIST_SPST.PO_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST_SPST
                            WHERE WEEK(TORDERLIST_SPST.PO_DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST_SPST.PO_DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST_SPST.DSCN_YN = 'Y' AND TORDERLIST_SPST.PO_QTY > 0
                            GROUP BY WEEK(TORDERLIST_SPST.PO_DT), TORDERLIST_SPST.FCTRY_NM) `B`

                            GROUP BY `B`.`WEEK`, `B`.`FCTRY_NM`

                            ORDER BY  `WEEK`  DESC, `FCTRY_NM`  ";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView0 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds.Tables)
                {
                    result.DataGridView0.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<BaseResult> T2_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 'ALL KOMAX' AS `FCTRY_NM`,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST.PERFORMN) / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT)

                            UNION
                            SELECT CONCAT(TORDERLIST.FCTRY_NM, ' KOMAX') AS `FCTRY_NM`,
                            WEEK(TORDERLIST.DT) AS `WEEK`,
                            DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                            DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                            COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                            IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                            CEILING(IFNULL(SUM(CASE WHEN TORDERLIST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                            SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                            SUM(TORDERLIST.PERFORMN) AS `ACT_SUM`,
                            CEILING((SUM(TORDERLIST.PERFORMN) / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                            FROM TORDERLIST
                            WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                            AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                            GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM

                            ORDER BY  `WEEK`  DESC, `FCTRY_NM`";
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
        public async Task<BaseResult> T3_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 'ALL LP' AS `FCTRY_NM`,
                                WEEK(TORDERLIST.DT) AS `WEEK`,
                                DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                                DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                                COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                                IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) AS `COUNT`,
                                IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) = 0 THEN 1 END), 0) AS `NOT_WORK`,
                                CEILING(IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                                SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                                SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R))/2 AS `ACT_SUM`,
                                CEILING((SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R)) / 2 / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                                FROM TORDERLIST JOIN TORDERLIST_LP
                                ON TORDERLIST.ORDER_IDX = TORDERLIST_LP.ORDER_IDX
                                WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                                AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                                GROUP BY WEEK(TORDERLIST.DT)

                                UNION
                                SELECT CONCAT(TORDERLIST.FCTRY_NM, ' LP') AS `FCTRY_NM`,
                                WEEK(TORDERLIST.DT) AS `WEEK`,
                                DATE_FORMAT(MIN(TORDERLIST.DT), '%Y-%m-%d') AS `MIN_DT`,
                                DATE_FORMAT(MAX(TORDERLIST.DT), '%Y-%m-%d') AS `MAX_DT`,
                                COUNT(TORDERLIST.LEAD_NO) AS `LEAD_COUNT`,
                                IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) AS `COUNT`,
                                IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) = 0 THEN 1 END), 0) AS `NOT_WORK`,
                                CEILING(IFNULL(SUM(CASE WHEN (TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R) > 0 THEN 1 END), 0) / COUNT(TORDERLIST.LEAD_NO) * 100) AS `LEAD_RAT`,
                                SUM(TORDERLIST.TOT_QTY) AS `PO_SUM`,
                                SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R))/2 AS `ACT_SUM`,
                                CEILING((SUM((TORDERLIST_LP.PERFORMN_L + TORDERLIST_LP.PERFORMN_R)) / 2 / SUM(TORDERLIST.TOT_QTY)  * 100 ))AS `ACT_RAT`
                                FROM TORDERLIST JOIN TORDERLIST_LP
                                ON TORDERLIST.ORDER_IDX = TORDERLIST_LP.ORDER_IDX
                                WHERE WEEK(TORDERLIST.DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST.DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                                AND TORDERLIST.DSCN_YN = 'Y' AND TORDERLIST.TOT_QTY > 0
                                GROUP BY WEEK(TORDERLIST.DT), TORDERLIST.FCTRY_NM

                                ORDER BY  `WEEK`  DESC, `FCTRY_NM` ";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView3 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds.Tables)
                {
                    result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<BaseResult> T4_LOAD(BaseParameter BaseParameter)
{
    BaseResult result = new BaseResult();
    try
    {
        string sql = @"SELECT 'ALL SPST' AS `FCTRY_NM`,
                        WEEK(TORDERLIST_SPST.PO_DT) AS `WEEK`,
                        DATE_FORMAT(MIN(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MIN_DT`,
                        DATE_FORMAT(MAX(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MAX_DT`,
                        COUNT(TORDERLIST_SPST.LEAD_NO) AS `LEAD_COUNT`,
                        IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                        IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                        CEILING(IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST_SPST.LEAD_NO) * 100) AS `LEAD_RAT`,
                        SUM(TORDERLIST_SPST.PO_QTY) AS `PO_SUM`,
                        SUM(TORDERLIST_SPST.PERFORMN) AS `ACT_SUM`,
                        CEILING((SUM(TORDERLIST_SPST.PERFORMN) / SUM(TORDERLIST_SPST.PO_QTY)  * 100 ))AS `ACT_RAT`
                        FROM TORDERLIST_SPST
                        WHERE WEEK(TORDERLIST_SPST.PO_DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST_SPST.PO_DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                        AND TORDERLIST_SPST.DSCN_YN = 'Y' AND TORDERLIST_SPST.PO_QTY > 0
                        GROUP BY WEEK(TORDERLIST_SPST.PO_DT)

                        UNION
                        SELECT CONCAT(TORDERLIST_SPST.FCTRY_NM, ' SPST') AS `FCTRY_NM`,
                        WEEK(TORDERLIST_SPST.PO_DT) AS `WEEK`,
                        DATE_FORMAT(MIN(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MIN_DT`,
                        DATE_FORMAT(MAX(TORDERLIST_SPST.PO_DT), '%Y-%m-%d') AS `MAX_DT`,
                        COUNT(TORDERLIST_SPST.LEAD_NO) AS `LEAD_COUNT`,
                        IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) AS `COUNT`,
                        IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN = 0 THEN 1 END), 0) AS `NOT_WORK`,
                        CEILING(IFNULL(SUM(CASE WHEN TORDERLIST_SPST.PERFORMN > 0 THEN 1 END), 0) / COUNT(TORDERLIST_SPST.LEAD_NO) * 100) AS `LEAD_RAT`,
                        SUM(TORDERLIST_SPST.PO_QTY) AS `PO_SUM`,
                        SUM(TORDERLIST_SPST.PERFORMN) AS `ACT_SUM`,
                        CEILING((SUM(TORDERLIST_SPST.PERFORMN) / SUM(TORDERLIST_SPST.PO_QTY)  * 100 ))AS `ACT_RAT`
                        FROM TORDERLIST_SPST
                        WHERE WEEK(TORDERLIST_SPST.PO_DT) > (WEEK(NOW()) -5) AND DATE_FORMAT(TORDERLIST_SPST.PO_DT, '%Y') >= DATE_FORMAT(NOW(), '%Y')
                        AND TORDERLIST_SPST.DSCN_YN = 'Y' AND TORDERLIST_SPST.PO_QTY > 0
                        GROUP BY WEEK(TORDERLIST_SPST.PO_DT), TORDERLIST_SPST.FCTRY_NM

                        ORDER BY  `WEEK`  DESC, `FCTRY_NM`";
        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
        result.DataGridView2 = new List<SuperResultTranfer>();
        foreach (DataTable dt in ds.Tables)
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

