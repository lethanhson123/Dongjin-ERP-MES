using ZXing;

namespace MESService.Implement
{
    public class C18Service : BaseService<torderlist, ItorderlistRepository>
    , IC18Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C18Service(ItorderlistRepository torderlistRepository

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
                string Search = BaseParameter.SearchString?.Trim().ToLower() ?? "";
                string MC = BaseParameter.MC_NAME?.Trim() ?? "0";
                DateTime fromDate = BaseParameter.FromDate.Value;
                DateTime toDate = BaseParameter.ToDate.Value;
                var selectTab = BaseParameter.Action;

                if (selectTab == 1)
                {
                    result = await SearchCrapDetail(MC, fromDate, toDate, Search);
                }
                else if (selectTab == 2) {
                    result = await GetCrapReportByLine(MC, fromDate, toDate, Search);
                }
                else if (selectTab == 3)
                {
                    result = await GetCrapReportByLeadNo(MC, fromDate, toDate, Search);
                }
                else if (selectTab == 4)
                {
                    result = await GetCrapReportByMaterialCode(MC, fromDate, toDate, Search);
                }

                else if (selectTab == 5)
                {
                    result = await GetCrapReportByPersonal(MC, fromDate, toDate, Search);
                }

            }
            catch (Exception ex)
            {
                result.Error = "Có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại.";
                Console.WriteLine("[ERROR] Buttonfind_Click: " + ex.ToString());
            }

            return result;
        }

        private async Task<BaseResult> GetCrapReportByPersonal(string MC, DateTime fromDate, DateTime toDate, string SearchString)
        {
            BaseResult result = new BaseResult();


            string sql = @"
select mTB.MC, mTB.CREATE_USER, b.USER_NM, mTB.MType, mTB.Unit, mTB.Total_Plan,mTB.Total_Actual, mTB.Total_Using, mTB.Total_Scrap, mTB.Percentage from
(SELECT
S.MC,
    S.CREATE_USER,
    S.MType,
    S.Unit,
    SUM(S.Total_Plan) AS Total_Plan,
    SUM(S.Total_Actual) AS Total_Actual,
    SUM(S.Total_Using) AS Total_Using,
    SUM(S.Total_Scrap) AS Total_Scrap,
    ROUND(
        CASE 
            WHEN S.MType = 'Wire' THEN 
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Using), 0)) * 100
            ELSE
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Actual), 0)) * 100
        END, 2
    ) AS Percentage

FROM (

    SELECT 
        th.Order_IDX, 
        b.PROJECT, 
        b.LEAD_NO, 
        th.MC,                 
        th.CREATE_USER,    
        b.DT AS Plan_Date,
        
        th.MaterialCode,
        th.MType, 
        (b.TOT_QTY *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END)) AS Total_Plan, 
        (b.PERFORMN *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END) ) AS Total_Actual, 

        CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END AS Total_BOM,
            
        ((CASE 
            WHEN th.MType = 'Wire' THEN 
                (b.PERFORMN * IFNULL(c.W_Length, 0))
            ELSE 
                b.PERFORMN
        END) + th.TotalQty) AS Total_Using,

        th.TotalQty AS Total_Scrap, 

        ROUND(
            CASE 
                WHEN th.MType = 'Wire' THEN 
                    (th.TotalQty / NULLIF((b.PERFORMN * IFNULL(c.W_Length, 0)), 0)) * 100
                ELSE 
                    (th.TotalQty / NULLIF(b.PERFORMN, 0)) * 100
            END
        ,2) AS Percentage,

        th.DVT AS Unit, 
        b.`CONDITION`

    FROM (
        WITH T AS (
            SELECT
                a.ORDER_IDX,
                a.MC,
                a.CREATE_USER,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN NULL ELSE a.Term1 END),''),'') AS Term1,
                SUM(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN 0 ELSE 1 END) AS Term1_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN NULL ELSE a.Seal1 END),''),'') AS Seal1,
                SUM(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN 0 ELSE 1 END) AS Seal1_Qty,

                a.WireCode,
                SUM(a.WireLenght) AS WireLenght_Total,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN NULL ELSE a.Term2 END),''),'') AS Term2,
                SUM(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN 0 ELSE 1 END) AS Term2_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN NULL ELSE a.Seal2 END),''),'') AS Seal2,
                SUM(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN 0 ELSE 1 END) AS Seal2_Qty

            FROM ScrapCutting a
            WHERE a.Date BETWEEN @FromDate AND @ToDate
            GROUP BY a.ORDER_IDX, a.CREATE_USER, a.MC
        )

        SELECT 
            Order_IDX,
            MC,
            MaterialCode,
            DVT,
            SUM(Qty) AS TotalQty,
            MType,
            CREATE_USER
        FROM (
            SELECT ORDER_IDX, MC, Term1 AS MaterialCode, 'EA' AS DVT, Term1_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal1 AS MaterialCode, 'EA' AS DVT, Seal1_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, WireCode AS MaterialCode, 'mm' AS DVT, WireLenght_Total AS Qty, 'Wire' AS MType, CREATE_USER
            FROM T WHERE WireCode <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Term2 AS MaterialCode, 'EA' AS DVT, Term2_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term2 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal2 AS MaterialCode, 'EA' AS DVT, Seal2_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal2 <> ''
        ) X
        GROUP BY Order_IDX, MC, MaterialCode, DVT, MType, CREATE_USER

    ) AS th

    LEFT JOIN torderlist b ON th.Order_IDX = b.Order_IDX
    LEFT JOIN torder_lead_bom c ON b.LEAD_NO = c.LEAD_PN
) AS S

GROUP BY 
S.MC,
    S.CREATE_USER,
    S.MType,
    S.Unit
ORDER BY S.CREATE_USER,S.MC) as mTB LEFT JOIN tsuser b on mTB.CREATE_USER = b.USER_ID
WHERE 
    (@MC = '0' OR TRIM(mTB.MC) = @MC)
    AND (
        @Search = '' OR (
                LOWER(mTB.MType) LIKE CONCAT('%', @Search, '%') OR
                LOWER(b.USER_NM) LIKE CONCAT('%', @Search, '%') OR
                LOWER(mTB.CREATE_USER) LIKE CONCAT('%', @Search, '%') OR
		        LOWER(mTB.Unit) LIKE CONCAT('%', @Search, '%')
        )
    )

";

            var parameters = new[]
           {
                    new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC },
                    new MySqlParameter("@FromDate", MySqlDbType.DateTime) { Value = fromDate },
                    new MySqlParameter("@ToDate", MySqlDbType.DateTime) { Value = toDate },
                    new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = SearchString }
                };

            //Sử dụng QueryToListAsync Tối ưu RAM cho server
            var list = await MySQLHelperV2.QueryToListAsync<ScrapListTranfer>(GlobalHelper.MariaDBConectionString, sql, parameters);
            result.ScrapListView = list;

            return result;
        }

        /// <summary>
        /// tổng hợp báo cáo phe liệu theo từng mã nvl thô
        /// </summary>
        /// <param name="MC"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="SearchString"></param>
        /// <returns></returns>
        private async Task<BaseResult> GetCrapReportByMaterialCode(string MC, DateTime fromDate, DateTime toDate, string SearchString)
        {
            BaseResult result = new BaseResult();

            string sql = @"
SELECT
    S.MaterialCode,
    S.MType,
    S.Unit,
    SUM(S.Total_Plan) AS Total_Plan,
    SUM(S.Total_Actual) AS Total_Actual,
    SUM(S.Total_Using) AS Total_Using,
    SUM(S.Total_Scrap) AS Total_Scrap,
    ROUND(
        CASE 
            WHEN S.MType = 'Wire' THEN 
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Using), 0)) * 100
            ELSE
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Actual), 0)) * 100
        END, 2
    ) AS Percentage

FROM (
    SELECT 
        th.Order_IDX, 
        b.PROJECT, 
        b.LEAD_NO, 
        th.MC,                 
        th.CREATE_USER,    
        b.DT AS Plan_Date,
        
        th.MaterialCode,
        th.MType, 
        (b.TOT_QTY *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END)) AS Total_Plan, 
        (b.PERFORMN *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END) ) AS Total_Actual, 

        CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END AS Total_BOM,
            
        ((CASE 
            WHEN th.MType = 'Wire' THEN 
                (b.PERFORMN * IFNULL(c.W_Length, 0))
            ELSE 
                b.PERFORMN
        END) + th.TotalQty) AS Total_Using,

        th.TotalQty AS Total_Scrap, 
 

        ROUND(
            CASE 
                WHEN th.MType = 'Wire' THEN 
                    (th.TotalQty / NULLIF((b.PERFORMN * IFNULL(c.W_Length, 0)), 0)) * 100
                ELSE 
                    (th.TotalQty / NULLIF(b.PERFORMN, 0)) * 100
            END
        ,2) AS Percentage,

        th.DVT AS Unit, 
        b.`CONDITION`

    FROM (
        WITH T AS (
            SELECT
                a.ORDER_IDX,
                a.MC,
                a.CREATE_USER,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN NULL ELSE a.Term1 END),''),'') AS Term1,
                SUM(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN 0 ELSE 1 END) AS Term1_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN NULL ELSE a.Seal1 END),''),'') AS Seal1,
                SUM(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN 0 ELSE 1 END) AS Seal1_Qty,

                a.WireCode,
                SUM(a.WireLenght) AS WireLenght_Total,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN NULL ELSE a.Term2 END),''),'') AS Term2,
                SUM(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN 0 ELSE 1 END) AS Term2_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN NULL ELSE a.Seal2 END),''),'') AS Seal2,
                SUM(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN 0 ELSE 1 END) AS Seal2_Qty

            FROM ScrapCutting a
            WHERE a.Date BETWEEN @FromDate AND @ToDate
            GROUP BY a.ORDER_IDX, a.CREATE_USER, a.MC
        )

        SELECT 
            Order_IDX,
            MC,
            MaterialCode,
            DVT,
            SUM(Qty) AS TotalQty,
            MType,
            CREATE_USER
        FROM (
            SELECT ORDER_IDX, MC, Term1 AS MaterialCode, 'EA' AS DVT, Term1_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal1 AS MaterialCode, 'EA' AS DVT, Seal1_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, WireCode AS MaterialCode, 'mm' AS DVT, WireLenght_Total AS Qty, 'Wire' AS MType, CREATE_USER
            FROM T WHERE WireCode <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Term2 AS MaterialCode, 'EA' AS DVT, Term2_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term2 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal2 AS MaterialCode, 'EA' AS DVT, Seal2_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal2 <> ''
        ) X
        GROUP BY Order_IDX, MC, MaterialCode, DVT, MType, CREATE_USER

    ) AS th

    LEFT JOIN torderlist b ON th.Order_IDX = b.Order_IDX
    LEFT JOIN torder_lead_bom c ON b.LEAD_NO = c.LEAD_PN
) AS S
WHERE 
    (@MC = '0' OR TRIM(S.MC) = @MC)
    AND (
        @Search = '' OR (
                LOWER(S.MType) LIKE CONCAT('%', @Search, '%') OR
                LOWER(S.MaterialCode) LIKE CONCAT('%', @Search, '%') OR
		        LOWER(S.Unit) LIKE CONCAT('%', @Search, '%')
        )
    )
GROUP BY 
    S.MaterialCode,
    S.MType,
    S.Unit

ORDER BY S.MaterialCode;

";

            var parameters = new[]
           {
                    new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC },
                    new MySqlParameter("@FromDate", MySqlDbType.DateTime) { Value = fromDate },
                    new MySqlParameter("@ToDate", MySqlDbType.DateTime) { Value = toDate },
                    new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = SearchString }
                };

            //Sử dụng QueryToListAsync Tối ưu RAM cho server
            var list = await MySQLHelperV2.QueryToListAsync<ScrapListTranfer>(GlobalHelper.MariaDBConectionString, sql, parameters);
            result.ScrapListView = list;

            return result;

        }

        /// <summary>
        /// load báo cáo tổng hợp phê liệu theo LEAD dây
        /// </summary>
        /// <param name="MC"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="SearchString"></param>
        /// <returns></returns>
        private async Task<BaseResult> GetCrapReportByLeadNo(string MC, DateTime fromDate, DateTime toDate, string SearchString)
        {
            BaseResult result = new BaseResult();

            string sql = @"
SELECT 
    th.Order_IDX, 
    b.PROJECT, 
    b.LEAD_NO, 
    th.MC,                  
    th.CREATE_USER,         
    b.DT AS Plan_Date,
    
    th.MaterialCode,
    th.MType, 
    b.TOT_QTY AS Total_Plan, 
    b.PERFORMN AS Total_Actual, 

    CASE 
        WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
        ELSE 1
    END AS Total_BOM,
		
    CASE 
        WHEN th.MType = 'Wire' THEN 
            (b.PERFORMN * IFNULL(c.W_Length, 0))
        ELSE 
            b.PERFORMN
    END AS Total_Using,

    th.TotalQty AS Total_Scrap, 

    ROUND(
        CASE 
            WHEN th.MType = 'Wire' THEN 
                (th.TotalQty / NULLIF((b.PERFORMN * IFNULL(c.W_Length, 0)), 0)) * 100
            ELSE 
                (th.TotalQty / NULLIF(b.PERFORMN, 0)) * 100
        END
    ,2) AS Percentage,

    th.DVT AS Unit, 
    b.`CONDITION`

FROM (

    WITH T AS (
        SELECT
            a.ORDER_IDX,
            a.MC,
            a.CREATE_USER,

            COALESCE(NULLIF(MIN(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN NULL ELSE a.Term1 END),''),'') AS Term1,
            SUM(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN 0 ELSE 1 END) AS Term1_Qty,

            COALESCE(NULLIF(MIN(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN NULL ELSE a.Seal1 END),''),'') AS Seal1,
            SUM(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN 0 ELSE 1 END) AS Seal1_Qty,

            a.WireCode,
            SUM(a.WireLenght) AS WireLenght_Total,

            COALESCE(NULLIF(MIN(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN NULL ELSE a.Term2 END),''),'') AS Term2,
            SUM(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN 0 ELSE 1 END) AS Term2_Qty,

            COALESCE(NULLIF(MIN(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN NULL ELSE a.Seal2 END),''),'') AS Seal2,
            SUM(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN 0 ELSE 1 END) AS Seal2_Qty

        FROM ScrapCutting a
        WHERE a.Date BETWEEN @FromDate AND @ToDate
        GROUP BY a.ORDER_IDX, a.CREATE_USER, a.MC
    )

    SELECT 
        Order_IDX,
        MC,
        MaterialCode,
        DVT,
        SUM(Qty) AS TotalQty,
        MType,
        CREATE_USER
    FROM (

        SELECT ORDER_IDX, MC, Term1 AS MaterialCode, 'EA' AS DVT, Term1_Qty AS Qty, 'Term' AS MType, CREATE_USER
        FROM T WHERE Term1 <> ''

        UNION ALL
        SELECT ORDER_IDX, MC, Seal1 AS MaterialCode, 'EA' AS DVT, Seal1_Qty AS Qty, 'Seal' AS MType, CREATE_USER
        FROM T WHERE Seal1 <> ''

        UNION ALL
        SELECT ORDER_IDX, MC, WireCode AS MaterialCode, 'mm' AS DVT, WireLenght_Total AS Qty, 'Wire' AS MType, CREATE_USER
        FROM T WHERE WireCode <> ''

        UNION ALL
        SELECT ORDER_IDX, MC, Term2 AS MaterialCode, 'EA' AS DVT, Term2_Qty AS Qty, 'Term' AS MType, CREATE_USER
        FROM T WHERE Term2 <> ''

        UNION ALL
        SELECT ORDER_IDX, MC, Seal2 AS MaterialCode, 'EA' AS DVT, Seal2_Qty AS Qty, 'Seal' AS MType, CREATE_USER
        FROM T WHERE Seal2 <> ''

    ) AS X

    GROUP BY 
        Order_IDX, 
        MC,
        MaterialCode, 
        DVT, 
        MType,
        CREATE_USER

) AS th

LEFT JOIN torderlist b ON th.Order_IDX = b.Order_IDX
LEFT JOIN torder_lead_bom c ON b.LEAD_NO = c.LEAD_PN

WHERE 
    (@MC = '0' OR TRIM(th.MC) = @MC)
    AND (
        @Search = '' OR (
                LOWER(th.MaterialCode) LIKE CONCAT('%', @Search, '%') OR
                LOWER(th.MC) LIKE CONCAT('%', @Search, '%') OR
		        LOWER(b.LEAD_NO) LIKE CONCAT('%', @Search, '%') OR
				LOWER(th.CREATE_USER) LIKE CONCAT('%', @Search, '%') OR
                LOWER(b.LEAD_NO) LIKE CONCAT('%', @Search, '%') OR
                LOWER(th.MType) LIKE CONCAT('%', @Search, '%') OR
				LOWER(b.PROJECT) LIKE CONCAT('%', @Search, '%')
        )
    );";

            var parameters = new[]
            {
                    new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC },
                    new MySqlParameter("@FromDate", MySqlDbType.DateTime) { Value = fromDate },
                    new MySqlParameter("@ToDate", MySqlDbType.DateTime) { Value = toDate },
                    new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = SearchString }
                };

            //Sử dụng QueryToListAsync Tối ưu RAM cho server
            var list = await MySQLHelperV2.QueryToListAsync<ScrapListTranfer>(GlobalHelper.MariaDBConectionString, sql, parameters);
            result.ScrapListView = list;

            return result;
        }

        //tính tỷ lệ theo từng máy cắt
        private async Task<BaseResult> GetCrapReportByLine(string MC, DateTime fromDate, DateTime toDate, string SearchString)
        {
            BaseResult result = new BaseResult();

            string sql = @"
SELECT
    S.MC,
    S.MType,
    S.Unit,
    SUM(S.Total_Plan) AS Total_Plan,
    SUM(S.Total_Actual) AS Total_Actual,
    SUM(S.Total_Using) AS Total_Using,
    SUM(S.Total_Scrap) AS Total_Scrap,
    ROUND(
        CASE 
            WHEN S.MType = 'Wire' THEN 
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Using), 0)) * 100
            ELSE
                (SUM(S.Total_Scrap) / NULLIF(SUM(S.Total_Actual), 0)) * 100
        END, 2
    ) AS Percentage

FROM (

    SELECT 
        th.Order_IDX, 
        b.PROJECT, 
        b.LEAD_NO, 
        th.MC,                 
        th.CREATE_USER,    
        b.DT AS Plan_Date,
        
        th.MaterialCode,
        th.MType, 
        (b.TOT_QTY *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END)) AS Total_Plan, 
        (b.PERFORMN *(CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END) ) AS Total_Actual, 

        CASE 
            WHEN th.MType = 'Wire' THEN IFNULL(c.W_Length, 0)
            ELSE 1
        END AS Total_BOM,
            
        ((CASE 
            WHEN th.MType = 'Wire' THEN 
                (b.PERFORMN * IFNULL(c.W_Length, 0))
            ELSE 
                b.PERFORMN
        END) + th.TotalQty) AS Total_Using,

        th.TotalQty AS Total_Scrap, 

        ROUND(
            CASE 
                WHEN th.MType = 'Wire' THEN 
                    (th.TotalQty / NULLIF((b.PERFORMN * IFNULL(c.W_Length, 0)), 0)) * 100
                ELSE 
                    (th.TotalQty / NULLIF(b.PERFORMN, 0)) * 100
            END
        ,2) AS Percentage,

        th.DVT AS Unit, 
        b.`CONDITION`

    FROM (
        WITH T AS (
            SELECT
                a.ORDER_IDX,
                a.MC,
                a.CREATE_USER,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN NULL ELSE a.Term1 END),''),'') AS Term1,
                SUM(CASE WHEN a.Term1 IS NULL OR a.Term1 = '' THEN 0 ELSE 1 END) AS Term1_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN NULL ELSE a.Seal1 END),''),'') AS Seal1,
                SUM(CASE WHEN a.Seal1 IS NULL OR a.Seal1 = '' THEN 0 ELSE 1 END) AS Seal1_Qty,

                a.WireCode,
                SUM(a.WireLenght) AS WireLenght_Total,

                COALESCE(NULLIF(MIN(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN NULL ELSE a.Term2 END),''),'') AS Term2,
                SUM(CASE WHEN a.Term2 IS NULL OR a.Term2 = '' THEN 0 ELSE 1 END) AS Term2_Qty,

                COALESCE(NULLIF(MIN(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN NULL ELSE a.Seal2 END),''),'') AS Seal2,
                SUM(CASE WHEN a.Seal2 IS NULL OR a.Seal2 = '' THEN 0 ELSE 1 END) AS Seal2_Qty

            FROM ScrapCutting a
            WHERE a.Date BETWEEN @FromDate AND @ToDate
            GROUP BY a.ORDER_IDX, a.CREATE_USER, a.MC
        )

        SELECT 
            Order_IDX,
            MC,
            MaterialCode,
            DVT,
            SUM(Qty) AS TotalQty,
            MType,
            CREATE_USER
        FROM (
            SELECT ORDER_IDX, MC, Term1 AS MaterialCode, 'EA' AS DVT, Term1_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal1 AS MaterialCode, 'EA' AS DVT, Seal1_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal1 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, WireCode AS MaterialCode, 'mm' AS DVT, WireLenght_Total AS Qty, 'Wire' AS MType, CREATE_USER
            FROM T WHERE WireCode <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Term2 AS MaterialCode, 'EA' AS DVT, Term2_Qty AS Qty, 'Term' AS MType, CREATE_USER
            FROM T WHERE Term2 <> ''

            UNION ALL
            SELECT ORDER_IDX, MC, Seal2 AS MaterialCode, 'EA' AS DVT, Seal2_Qty AS Qty, 'Seal' AS MType, CREATE_USER
            FROM T WHERE Seal2 <> ''
        ) X
        GROUP BY Order_IDX, MC, MaterialCode, DVT, MType, CREATE_USER

    ) AS th

    LEFT JOIN torderlist b ON th.Order_IDX = b.Order_IDX
    LEFT JOIN torder_lead_bom c ON b.LEAD_NO = c.LEAD_PN
) AS S

WHERE 
    (@MC = '0' OR TRIM(S.MC) = @MC)
    AND (
        @Search = '' OR (
                LOWER(S.MType) LIKE CONCAT('%', @Search, '%') OR
                LOWER(S.MC) LIKE CONCAT('%', @Search, '%') OR
		        LOWER(S.Unit) LIKE CONCAT('%', @Search, '%')
        )
    )

GROUP BY 
    S.MC,
    S.MType,
    S.Unit

ORDER BY S.MC;

                       ";

            var parameters = new[]
            {
                    new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC },
                    new MySqlParameter("@FromDate", MySqlDbType.DateTime) { Value = fromDate },
                    new MySqlParameter("@ToDate", MySqlDbType.DateTime) { Value = toDate },
                    new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = SearchString }
                };

            //Sử dụng QueryToListAsync Tối ưu RAM cho server
            var list = await MySQLHelperV2.QueryToListAsync<ScrapListTranfer>(GlobalHelper.MariaDBConectionString, sql, parameters);
            result.ScrapListView = list;

            return result;
        }

        /// <summary>
        /// tìm kiếm chi tiết thông tin Scrap
        /// </summary>
        /// <param name="MC">máy cắt</param>
        /// <param name="fromDate"> chuỗi DateTime</param>
        /// <param name="toDate"> chuỗi Datetime</param>
        /// <param name="SearchString">chuỗi tìm kiếm</param>
        /// <returns></returns>
        private async Task<BaseResult> SearchCrapDetail(string MC, DateTime fromDate, DateTime toDate, string SearchString)
        {
            BaseResult result = new BaseResult();

            string sql = @"
                        SELECT 
                            a.ORDER_IDX, 
                            IFNULL(a.WireNumber, 0) AS WireNumber,
                            a.Date AS DT, 
                            IFNULL(a.MC, '') AS MC, 
                            IFNULL(a.TERM1, '') AS TERM1, 
                            IFNULL(a.PriceTerm1, 0) AS PriceTerm1,
                            IFNULL(a.SEAL1, '') AS SEAL1, 
                            IFNULL(a.SealPrice1, 0) AS SealPrice1, 
                            IFNULL(a.WireCode, '') AS WireCode, 
                            IFNULL(a.WirePrice, 0) AS WirePrice,
                            IFNULL(a.WireLenght, 0) AS WIRE_LENGTH,

                            IFNULL(a.TERM2, '') AS TERM2, 
                            IFNULL(a.TermPrice2, 0) AS TermPrice2,
                            IFNULL(a.SEAL2, '') AS SEAL2, 
                            IFNULL(a.SealPrice2, 0) AS SealPrice2,

                            a.CREATE_DTM, 
                            IFNULL(a.CREATE_USER, '') AS CREATE_USER,

                            IFNULL(b.`CONDITION`, '') AS `CONDITION`,
                            b.DT AS DatePlan, 
                            IFNULL(b.FCTRY_NM, '') AS FCTRY_NM, 
                            IFNULL(b.LEAD_NO, '') AS LEAD_NO, 
                            IFNULL(b.WIRE, '') AS WIRE_NM,

                            IFNULL(b.TORDER_FG, '') AS TORDER_FG, 
                            IFNULL(b.TOT_QTY, 0) AS TOT_QTY, 
                            IFNULL(b.PERFORMN, 0) AS PERFORMN,

                            IFNULL(c.BUNDLE_SIZE, 0) AS BUNDLE_SIZE, 
                            IFNULL(c.W_Length, 0) AS W_Length

                        FROM ScrapCutting a 
                        JOIN TORDERLIST b ON a.ORDER_IDX = b.ORDER_IDX 
                        LEFT JOIN torder_lead_bom c ON b.LEAD_NO = c.LEAD_PN

                        WHERE 
                            (@MC = '0' OR TRIM(a.MC) = @MC)
                            AND a.Date BETWEEN @FromDate AND @ToDate
                            AND (
                                @Search = '' OR (
                                    LOWER(a.MC) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(a.TERM1) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(a.SEAL1) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(a.WireCode) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(a.TERM2) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(a.CREATE_USER) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(b.`CONDITION`) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(b.LEAD_NO) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(b.WIRE) LIKE CONCAT('%', @Search, '%') OR
                                    LOWER(b.TORDER_FG) LIKE CONCAT('%', @Search, '%')
                                )
                            )
                        ORDER BY a.ORDER_IDX, a.WireNumber DESC;";

            var parameters = new[]
            {
                    new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC },
                    new MySqlParameter("@FromDate", MySqlDbType.DateTime) { Value = fromDate },
                    new MySqlParameter("@ToDate", MySqlDbType.DateTime) { Value = toDate },
                    new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = SearchString }
                };

            //Sử dụng QueryToListAsync Tối ưu RAM cho server
            var list = await MySQLHelperV2.QueryToListAsync<SuperResultTranfer>(GlobalHelper.MariaDBConectionString, sql, parameters);
            result.DataGridView = list;

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

