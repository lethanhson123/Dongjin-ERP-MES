

namespace MESService.Implement
{
    public class D01Service : BaseService<torderlist, ItorderlistRepository>
    , ID01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D01Service(ItorderlistRepository torderlistRepository

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
                if (BaseParameter.SearchString == "1")
                {
                    result = await SearcByDate(BaseParameter);
                }
                else if (BaseParameter.SearchString == "2")
                {
                    result = await SearcByYear(BaseParameter);
                }



            }
            catch (Exception ex)
            {
                result.ErrorNumber = -1;
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> SearcByYear(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            int YEAR_D = baseParameter.Year.Value;

            string SUB_SQL = @"
    SELECT    
        ZZ.MODEL,
        COUNT(ZZ.PART_NO) AS P_COUNT,
        SUM(ZZ.PO_QTY) AS PO_QTY,
        SUM(ZZ.Sales_QTY) AS ACT_QTY,
        (SUM(ZZ.Sales_QTY) / SUM(ZZ.PO_QTY) * 100) AS RAT,
        SUM(ZZ.Sales) AS Sales,
        -- Từng tháng từ 01 đến 12
        " + string.Join(",\n", Enumerable.Range(1, 12).Select(m => $@"
        IFNULL(SUM(CASE WHEN DATE_FORMAT(PO_DATE, '%m') = '{m:00}' THEN ZZ.PO_QTY END), 0) AS PO_{m:00},
        IFNULL(SUM(CASE WHEN DATE_FORMAT(PO_DATE, '%m') = '{m:00}' THEN ZZ.Sales_QTY END), 0) AS ACT_{m:00},
        IFNULL(SUM(CASE WHEN DATE_FORMAT(PO_DATE, '%m') = '{m:00}' THEN ZZ.Sales END), 0) AS Sales_{m:00}
        ")) + @"
    FROM (
        SELECT 
            (SELECT PO_CODE FROM tdpdotpl WHERE PDOTPL_IDX = tdpdmtim.PDOTPL_IDX) AS PO_CODE,
            DATE_FORMAT(tdpdmtim.UPDATE_DTM, '%Y-%m-%d') AS PO_DATE,
            (SELECT PART_NO FROM tspart WHERE PART_IDX = tdpdmtim.VLID_PART_IDX) AS PART_NO,
            (SELECT PART_CAR FROM tspart WHERE PART_IDX = tdpdmtim.VLID_PART_IDX) AS MODEL,
            (SELECT PO_QTY FROM tdpdotpl WHERE PDOTPL_IDX = tdpdmtim.PDOTPL_IDX) AS PO_QTY,
            COUNT(VLID_BARCODE) AS Sales_QTY,
            -- Cost
            IFNULL((
                SELECT CT_DATA.TDD_CT_QTY 
                FROM (
                    SELECT 
                        TSPSRT_IDX, TSCOST_DT AS TDD_CT_DT, TSCOST_VAL AS TDD_CT_QTY
                    FROM TSCOST
                ) CT_DATA
                WHERE CT_DATA.TSPSRT_IDX = tdpdmtim.VLID_PART_IDX
                AND CT_DATA.TDD_CT_DT <= tdpdmtim.UPDATE_DTM
                ORDER BY CT_DATA.TDD_CT_DT DESC
                LIMIT 1
            ), 0) AS COST,
            -- Sales = COST * QTY
            (IFNULL((
                SELECT CT_DATA.TDD_CT_QTY 
                FROM (
                    SELECT 
                        TSPSRT_IDX, TSCOST_DT AS TDD_CT_DT, TSCOST_VAL AS TDD_CT_QTY
                    FROM TSCOST
                ) CT_DATA
                WHERE CT_DATA.TSPSRT_IDX = tdpdmtim.VLID_PART_IDX
                AND CT_DATA.TDD_CT_DT <= tdpdmtim.UPDATE_DTM
                ORDER BY CT_DATA.TDD_CT_DT DESC
                LIMIT 1
            ), 0) * COUNT(VLID_BARCODE)) AS Sales
        FROM tdpdmtim
        WHERE 
            VLID_DSCN_YN = 'Y'
            AND UPDATE_DTM >= @StartDate
            AND UPDATE_DTM <= @EndDate
            AND NOT (tdpdotplmu_IDX IS NULL)
            AND NOT (PDOTPL_IDX IS NULL)
        GROUP BY PDOTPL_IDX, VLID_PART_IDX, DATE_FORMAT(UPDATE_DTM, '%Y-%m-%d')
    ) ZZ
    GROUP BY ZZ.MODEL WITH ROLLUP;
    ";

            var parameters = new[] {
        new MySqlParameter("@StartDate", MySqlDbType.Date) { Value = new DateTime(YEAR_D, 1, 1) },
        new MySqlParameter("@EndDate", MySqlDbType.Date) { Value = new DateTime(YEAR_D, 12, 31) },
    };

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SUB_SQL, parameters);

            result.List_D01 = new List<D01Model>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                {
                    result.List_D01.AddRange(SQLHelper.ToList<D01Model>(dt));
                }
            }

            result.ErrorNumber = 0;
            result.Error = "";
            return result;
        }


        private async Task<BaseResult> SearcByDate(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();

            DateTime aDate = DateTime.ParseExact(baseParameter.StartDate.Trim(), "yyyy-MM-dd", null);
            DateTime bDate = DateTime.ParseExact(baseParameter.EndDate.Trim(), "yyyy-MM-dd", null);

            string sql = @"
SELECT 
  po.PO_CODE,
  DATE_FORMAT(t.UPDATE_DTM, '%Y-%m-%d') AS PO_DATE,
  p.PART_NO,
  p.PART_NM AS PART_NAME,
  p.PART_CAR AS MODEL,
  p.PART_FML AS `GROUP`,
  p.PART_SNP,
  (SELECT QTY FROM tiivtr WHERE PART_IDX = p.PART_IDX AND LOC_IDX = 2) AS STOCK,
  po.PO_QTY,
  COUNT(t.VLID_BARCODE) AS Sales_QTY,

  IFNULL((
    SELECT TSCOST.TSCOST_VAL
    FROM TSCOST
    WHERE TSCOST.TSPSRT_IDX = p.PART_IDX AND TSCOST.TSCOST_DT <= t.UPDATE_DTM
    ORDER BY TSCOST.TSCOST_DT DESC
    LIMIT 1
  ), 0) AS COST,

  (
    IFNULL((
      SELECT TSCOST.TSCOST_VAL
      FROM TSCOST
      WHERE TSCOST.TSPSRT_IDX = p.PART_IDX AND TSCOST.TSCOST_DT <= t.UPDATE_DTM
      ORDER BY TSCOST.TSCOST_DT DESC
      LIMIT 1
    ), 0) * COUNT(t.VLID_BARCODE)
  ) AS Sales

FROM tdpdmtim t
JOIN tspart p ON p.PART_IDX = t.VLID_PART_IDX
JOIN tdpdotpl po ON po.PDOTPL_IDX = t.PDOTPL_IDX

WHERE 
  po.PO_CODE LIKE CONCAT('%', @PO_CODE, '%') AND
  p.PART_NO LIKE CONCAT('%', @PART_NO, '%') AND
  p.PART_CAR LIKE CONCAT('%', @PART_CAR, '%') AND
  t.VLID_DSCN_YN = 'Y' AND
  t.UPDATE_DTM BETWEEN @ADATE AND @BDATE

GROUP BY t.PDOTPL_IDX, t.VLID_PART_IDX, DATE_FORMAT(t.UPDATE_DTM, '%Y-%m-%d')
ORDER BY t.VLID_DTM DESC, p.PART_NO;
";

            var parameters = new[] {
        new MySqlParameter("@PO_CODE", MySqlDbType.VarChar) { Value = baseParameter.PartEncno.Trim() },
        new MySqlParameter("@PART_NO", MySqlDbType.VarChar) { Value = baseParameter.PartNo.Trim() },
        new MySqlParameter("@PART_CAR", MySqlDbType.VarChar) { Value = baseParameter.LeadNo.Trim() },
        new MySqlParameter("@ADATE", MySqlDbType.Date) { Value = aDate },
        new MySqlParameter("@BDATE", MySqlDbType.Date) { Value = bDate },
    };

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);

            result.List_D01 = new List<D01Model>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                    result.List_D01.AddRange(SQLHelper.ToList<D01Model>(dt));
            }

            result.ErrorNumber = 0;
            result.Error = "";

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

