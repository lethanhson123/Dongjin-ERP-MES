using Microsoft.Extensions.Logging;

namespace MESService.Implement
{
    public class C20Service : BaseService<torderlist, ItorderlistRepository>, IC20Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly ILogger<C20Service> _logger;

        public C20Service(
            ItorderlistRepository torderlistRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<C20Service> logger) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
            _logger = logger;
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
                _logger.LogError(ex, "Error in Load");
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
                    if (BaseParameter.Action == 3)
                    {
                        result.spcListDetail = await GetCuttingSPC(BaseParameter, false);
                    }
                    else if (BaseParameter.Action == 4)
                    {
                        result.spcListDetail = await GetCrimpingSPC(BaseParameter, false);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                _logger.LogError(ex, "Error in Buttonfind_Click");
            }
            return result;
        }

        private async Task<List<SuperResultTranfer>?> GetCrimpingSPC(BaseParameter BaseParameter, bool full)
        {
            BaseResult result = new BaseResult();

            if (BaseParameter.ListSearchString != null)
            {
                var txt_FromDate = BaseParameter.ListSearchString[0].Trim();
                var txt_toDate = BaseParameter.ListSearchString[1].Trim();
                var txt_MC = BaseParameter.ListSearchString[2].Trim();
                var txt_EmployeeID = BaseParameter.ListSearchString[3].Trim();
                var txt_Search = BaseParameter.ListSearchString[4].Trim();

                var Fac = "Factory 1";
                var line = "  AND (`Main`.`MC_NO` LIKE '" + txt_MC + "%' OR `Main`.`MCPlan` LIKE '" + txt_MC + "%') ";
                var user = " AND Main.CREATE_USER = '" + txt_EmployeeID + "' ";
                var search = " AND Main.PART_IDX Like '%" + txt_Search + "%' ";

                if (BaseParameter.rb_Fac2 == true)
                {
                    Fac = "Factory 2";
                }
                if (string.IsNullOrEmpty(txt_MC))
                {
                    line = " ";
                }
                if (string.IsNullOrEmpty(txt_EmployeeID))
                {
                    user = " ";
                }
                if (string.IsNullOrEmpty(txt_Search))
                {
                    search = " ";
                }

                var fromDate = DateTime.Parse(txt_FromDate).ToString("yyyy-MM-dd") + " 06:00:00";
                var toDate = DateTime.Parse(txt_toDate).AddDays(1).ToString("yyyy-MM-dd") + " 06:00:00";

                string sql = @" SELECT `Main`.COLSIP,  `Main`.TestPos, `Main`.`STAGE`, `Main`.TORDER_IDX As ORDER_IDX,  DATE_FORMAT(`Main`.DatePlan,'%Y-%m-%d') AS `DatePlan`, 
 `Main`.`DATE`, `Main`.`PART_IDX` AS LEAD_NO, `Main`.MCPlan,  `Main`.`MC_NO` AS MC2, `Main`.QtyPlan, Main.WK_TERM,
`Main`.WK_QTY AS QtyActual, `Main`.FIRST_TIME, `Main`.END_TIME,  `Main`.CREATE_USER,  `Main`.`NAME` AS FullName, `Main`.TORDER_FG,
 `Main`.WORK_WEEK, `Main`.PROJECT, `Main`.BUNDLE_SIZE, `Main`.HOOK_RACK, `Main`.WIRE, `Main`.WIRE_NM, `Main`.W_Diameter,
  `Main`.W_Color, `Main`.W_Length, `Main`.TERM1,  `Main`.STRIP1, `Main`.SEAL1, `Main`.CCH_W1, `Main`.ICH_W1, `Main`.TERM2, `Main`.STRIP2,
`Main`.SEAL2, `Main`.CCH_W2,`Main`.ICH_W2, `Main`.`CONDITION`,  `Main`.FCTRY_NM,  `Main`.CCH1,  `Main`.CCW1,  `Main`.CCH2, `Main`.CCW2,
 `Main`.ICH1, `Main`.ICW1, `Main`.ICH2,   `Main`.ICW2, `Main`.STRENGTH,  `Main`.WIRE_FORCE,  `Main`.CHK_LR, `Main`.IN_RESILT, `Main`.TestTime
 FROM (SELECT L.*, T.* FROM  
(SELECT 'First' AS COLSIP, '1' AS TestPos, m.`STAGE`,m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM, M.WK_TERM
 , H.TORDER_FG, H.WORK_WEEK,H.PROJECT,H.MC AS MCPlan, H.QtyPlan, H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK,
 H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2,H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM    
 From
( SELECT 'Crimp' AS `STAGE`,A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`, 
  A.WK_TERM , SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
 (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM` 
   FROM TWWKAR_LP `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + @"' GROUP BY `A`.`PART_IDX`,
	 DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M JOIN
	 (SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, IFNULL(b.PERFORMN, 0) AS QtyActual, 
	 b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, IFNULL(c.W_Length, 0) AS W_Length,
	  b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM
	   from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` 
		LEFT JOIN ( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.STRENGTH, a.WIRE_FORCE, a.CHK_LR, a.IN_RESILT, a.CREATE_DTM AS TestTime 
		FROM torderinspection_LP a WHERE a.COLSIP = 'First' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX UNION 
		SELECT L.*, t.* 
		FROM (SELECT 'Middle' AS COLSIP, '2' AS TestPos, m.`STAGE`, m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, 
		M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM ,M.WK_TERM, H.TORDER_FG, H.WORK_WEEK, H.PROJECT, H.MC AS MCPlan, H.QtyPlan, 
		H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK, H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, 
		H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2, H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM 
		From   (SELECT 'Crimp' AS `STAGE`, A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`,
		 A.WK_TERM, SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`, 
		(SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM`
		FROM TWWKAR_LP `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + @"' GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M 
		 JOIN( SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, IFNULL(b.PERFORMN, 0) AS QtyActual, b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, IFNULL(c.W_Length, 0) AS W_Length, b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM 
		 from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` LEFT JOIN ( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.STRENGTH, a.WIRE_FORCE,  a.CHK_LR, a.IN_RESILT, a.CREATE_DTM AS TestTime 
		 FROM torderinspection_LP a WHERE a.COLSIP = 'Middle' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX UNION SELECT L.*, t.* FROM (SELECT 'End' AS COLSIP, '3' AS TestPos, m.`STAGE`, m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM , M.WK_TERM, H.TORDER_FG, H.WORK_WEEK, H.PROJECT, H.MC AS MCPlan, H.QtyPlan, H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK, H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2, H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM 
		 From  (SELECT 'Crimp' AS `STAGE`, A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`,                                
		  A.WK_TERM, SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
		 (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM` 
		  FROM TWWKAR_LP `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + @"' GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M JOIN
			( SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, 
			IFNULL(b.PERFORMN, 0) AS QtyActual, b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, 
			IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, 
			IFNULL(c.W_Length, 0) AS W_Length, b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, 
			b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM 
			from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` LEFT JOIN 
			( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.STRENGTH, a.WIRE_FORCE,  a.CHK_LR,  a.IN_RESILT, a.CREATE_DTM AS TestTime 
			FROM torderinspection_LP a WHERE a.COLSIP ='End' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX ) `Main` 
			WHERE Main.FCTRY_NM = '" + Fac + "'   " + line + user + search + "  ORDER BY `Main`.TORDER_IDX, `Main`.TestPos, `Main`.TestTime ";

                if (!full)
                    sql = sql + " LIMIT 450";

                result.spcListDetail = await MySQLHelperV2.QueryToListAsync<SuperResultTranfer>(GlobalHelper.MariaDBConectionString, sql);
            }

            return result.spcListDetail;
        }

        private async Task<List<SuperResultTranfer>?> GetCuttingSPC(BaseParameter BaseParameter, bool full)
        {
            BaseResult result = new BaseResult();

            if (BaseParameter.ListSearchString != null)
            {
                var txt_FromDate = BaseParameter.ListSearchString[0].Trim();
                var txt_toDate = BaseParameter.ListSearchString[1].Trim();
                var txt_MC = BaseParameter.ListSearchString[2].Trim();
                var txt_EmployeeID = BaseParameter.ListSearchString[3].Trim();
                var txt_Search = BaseParameter.ListSearchString[4].Trim();

                var Fac = "Factory 1";
                var line = " AND ( Main.MC_NO LIKE '" + txt_MC + "%' or `Main`.MCPlan like '" + txt_MC + "%')";
                var user = " AND Main.CREATE_USER = '" + txt_EmployeeID + "' ";
                var search = " AND Main.PART_IDX Like '%" + txt_Search + "%' ";

                if (BaseParameter.rb_Fac2 == true)
                {
                    Fac = "Factory 2";
                }
                if (string.IsNullOrEmpty(txt_MC))
                {
                    line = " ";
                }
                if (string.IsNullOrEmpty(txt_EmployeeID))
                {
                    user = " ";
                }
                if (string.IsNullOrEmpty(txt_Search))
                {
                    search = " ";
                }

                var fromDate = DateTime.Parse(txt_FromDate).ToString("yyyy-MM-dd") + " 06:00:00";
                var toDate = DateTime.Parse(txt_toDate).AddDays(1).ToString("yyyy-MM-dd") + " 06:00:00";

                string sql = @" SELECT 
                        `Main`.COLSIP,
                        `Main`.TestPos,
                        `Main`.`STAGE`,
                        `Main`.TORDER_IDX As ORDER_IDX,
                        DATE_FORMAT(`Main`.DatePlan,'%Y-%m-%d') AS `DatePlan`, 
                        `Main`.`DATE`,
                        `Main`.`PART_IDX` AS LEAD_NO,
                        `Main`.MCPlan,
                        `Main`.`MC_NO` AS MC2,
                        `Main`.QtyPlan,
                        `Main`.WK_QTY AS QtyActual,
                        `Main`.FIRST_TIME,
                        `Main`.END_TIME,
                        `Main`.CREATE_USER,
                        `Main`.`NAME` AS FullName,
                        `Main`.TORDER_FG,
                        `Main`.WORK_WEEK,
                        `Main`.PROJECT,
                        `Main`.BUNDLE_SIZE,
                        `Main`.HOOK_RACK,
                        `Main`.WIRE,
                        `Main`.WIRE_NM,
                        `Main`.W_Diameter,
                        `Main`.W_Color,
                        `Main`.W_Length,
                        `Main`.TERM1,
                        `Main`.STRIP1,
                        `Main`.SEAL1,
                        `Main`.CCH_W1,
                        `Main`.ICH_W1,
                        `Main`.TERM2,
                        `Main`.STRIP2,
                        `Main`.SEAL2,
                        `Main`.CCH_W2,
                        `Main`.ICH_W2,
                        `Main`.`CONDITION`,
                        `Main`.FCTRY_NM,
                        `Main`.CCH1,
                        `Main`.CCW1,
                        `Main`.CCH2,
                        `Main`.CCW2,
                        `Main`.ICH1,
                        `Main`.ICW1,
                        `Main`.ICH2,
                        `Main`.ICW2,
                        `Main`.WIRE_FORCE,
                        `Main`.WIRE_LENGTH,
                        `Main`.IN_RESILT,
                        `Main`.TestTime
                                                     FROM 
                                                     (SELECT L.*, T.* FROM  
                                                     (SELECT 'First' AS COLSIP, '1' AS TestPos, m.`STAGE`,m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM
                                                     , H.TORDER_FG, H.WORK_WEEK,H.PROJECT,H.MC AS MCPlan, H.QtyPlan, H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK,
                                                     H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2,H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM    
                                                     From
                                                       ( SELECT 'KOMAX' AS `STAGE`,A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`, 
                                                        '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
                                                        (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM` 
                                                        FROM TWWKAR `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + "' GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M JOIN(SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, IFNULL(b.PERFORMN, 0) AS QtyActual, b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, IFNULL(c.W_Length, 0) AS W_Length, b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` LEFT JOIN ( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.WIRE_FORCE, a.WIRE_LENGTH, a.IN_RESILT, a.CREATE_DTM AS TestTime FROM torderinspection a WHERE a.COLSIP LIKE '%First%' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX UNION SELECT L.*, t.* FROM (SELECT 'Middle' AS COLSIP, '2' AS TestPos, m.`STAGE`, m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM , H.TORDER_FG, H.WORK_WEEK, H.PROJECT, H.MC AS MCPlan, H.QtyPlan, H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK, H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2, H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM From   (SELECT 'KOMAX' AS `STAGE`, A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`,                                '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM`                                 FROM TWWKAR `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + "'                               GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M JOIN( SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, IFNULL(b.PERFORMN, 0) AS QtyActual, b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, IFNULL(c.W_Length, 0) AS W_Length, b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` LEFT JOIN ( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.WIRE_FORCE, a.WIRE_LENGTH, a.IN_RESILT, a.CREATE_DTM AS TestTime FROM torderinspection a WHERE a.COLSIP LIKE '%Middle%' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX UNION SELECT L.*, t.* FROM (SELECT 'End' AS COLSIP, '3' AS TestPos, m.`STAGE`, m.TORDER_IDX, m.`DATE`, M.PART_IDX, M.MC_NO, m.WK_QTY, M.FIRST_TIME, M.END_TIME, M.CREATE_USER, M.`NAME`, m.CREATE_DTM , H.TORDER_FG, H.WORK_WEEK, H.PROJECT, H.MC AS MCPlan, H.QtyPlan, H.QtyActual, H.BUNDLE_SIZE, H.dt AS DatePlan, H.HOOK_RACK, H.WIRE, H.WIRE_NM, H.W_Diameter, h.W_Color, H.W_Length, H.TERM1, H.STRIP1, H.SEAL1, H.CCH_W1, H.ICH_W1, H.TERM2, H.STRIP2, H.SEAL2, H.CCH_W2, H.ICH_W2, H.`CONDITION`, H.FCTRY_NM From   (SELECT 'KOMAX' AS `STAGE`, A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `A`.`PART_IDX`, `A`.`MC_NO`,                                '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`CREATE_DTM`                                 FROM TWWKAR `A` Where A.CREATE_DTM Between '" + fromDate + "' AND '" + toDate + "'                                GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d')) AS M JOIN( SELECT b.TORDER_FG, b.ORDER_IDX, b.WORK_WEEK, b.PROJECT, b.MC, b.Tot_qty AS QtyPlan, IFNULL(b.PERFORMN, 0) AS QtyActual, b.BUNDLE_SIZE, b.dt, b.HOOK_RACK, b.WIRE, c.WIRE_NM, IFNULL(c.W_Diameter, 0) AS W_Diameter, c.W_Color, IFNULL(c.W_Length, 0) AS W_Length, b.TERM1, b.STRIP1, b.SEAL1, b.CCH_W1, b.ICH_W1, b.TERM2, b.STRIP2, b.SEAL2, b.CCH_W2, b.ICH_W2, b.`CONDITION`, b.FCTRY_NM from TORDERLIST b JOIN torder_lead_bom c ON c.LEAD_PN = b.LEAD_NO) AS H ON H.ORDER_IDX = M.TORDER_IDX) `L` LEFT JOIN ( SELECT a.ORDER_IDX, a.CCH1, a.CCW1, a.CCH2, a.CCW2, a.ICH1, a.ICW1, a.ICH2, a.ICW2, a.WIRE_FORCE, a.WIRE_LENGTH, a.IN_RESILT, a.CREATE_DTM AS TestTime FROM torderinspection a WHERE a.COLSIP LIKE '%End%' ) AS `T` ON L.TORDER_IDX = T.ORDER_IDX ) `Main` WHERE Main.FCTRY_NM = '" + Fac + "' " + line + user + search + " ORDER BY `Main`.TORDER_IDX, `Main`.TestPos ";

                if (!full)
                    sql = sql + " LIMIT " + GlobalHelper.ListCount;

                sql = sql.Replace(@"[fromDate]", fromDate)
                         .Replace(@"[toDate]", toDate)
                         .Replace(@"[Fac]", Fac)
                         .Replace(@"[line]", line)
                         .Replace(@"[user]", user)
                         .Replace(@"[search]", search);

                result.spcListDetail = await MySQLHelperV2.QueryToListAsync<SuperResultTranfer>(GlobalHelper.MariaDBConectionString, sql);
            }
            return result.spcListDetail;
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
                _logger.LogError(ex, "Error in Buttonadd_Click");
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
                _logger.LogError(ex, "Error in Buttonsave_Click");
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
                _logger.LogError(ex, "Error in Buttondelete_Click");
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
                _logger.LogError(ex, "Error in Buttoncancel_Click");
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
                _logger.LogError(ex, "Error in Buttoninport_Click");
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
                _logger.LogError(ex, "Error in Buttonprint_Click");
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
                _logger.LogError(ex, "Error in Buttonhelp_Click");
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
                _logger.LogError(ex, "Error in Buttonclose_Click");
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Tạo Job ID
                string jobId = Guid.NewGuid().ToString();

                // Tạo thông tin job
                var jobInfo = new ExportJobInfo
                {
                    JobId = jobId,
                    Status = "Processing",
                    Progress = 0,
                    CreatedAt = DateTime.Now,
                    Parameters = BaseParameter
                };

                // Lưu vào cache với thời gian 30 phút
                CacheHelper.Set(jobId, jobInfo, TimeSpan.FromMinutes(30));

                _logger.LogInformation($"Export job started: {jobId}");

                // Khởi chạy background task - không await để trả về ngay
                _ = Task.Run(async () => await ProcessExportInBackground(jobId, BaseParameter));

                // Trả về job ID ngay lập tức
                result.JobId = jobId;
                result.Code = jobId;
                result.Message = "Đang xử lý xuất file...";
                result.Progress = 0;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                _logger.LogError(ex, "Error starting export job");
            }
            return result;
        }

        //cập nhất tiến độ trong quá trình xử lý
        private void UpdateJobStatus(string jobId, string action, int currentRow, int totalRows, int progress, string error = null)
        {
            try
            {
                var jobInfo = CacheHelper.Get<ExportJobInfo>(jobId);
                if (jobInfo != null)
                {
                    jobInfo.CurrentAction = action;
                    jobInfo.CurrentRow = currentRow;
                    jobInfo.TotalRows = totalRows;
                    jobInfo.Progress = progress;
                    jobInfo.LastUpdated = DateTime.Now;

                    if (!string.IsNullOrEmpty(error))
                    {
                        jobInfo.Error = error;
                        jobInfo.Status = "Failed";
                    }

                    CacheHelper.Set(jobId, jobInfo, TimeSpan.FromMinutes(30));

                    _logger.LogDebug($"Export job {jobId}: {action} - {currentRow}/{totalRows} - {progress}%");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating job status for {jobId}");
            }
        }

        private async Task ProcessExportInBackground(string jobId, BaseParameter BaseParameter)
        {
            try
            {
                _logger.LogInformation($"Processing export job {jobId} started");

                // Cập nhật trạng thái - Đang lấy dữ liệu
                UpdateJobStatus(jobId, "Đang lấy dữ liệu...", 0, 0, 0);

                // Lấy dữ liệu
                List<SuperResultTranfer> data = null;
                string reportTitle = "";
                string fileName = "";
                string SheetName = this.GetType().Name;

                string txt_FromDate = BaseParameter.ListSearchString[0];
                string txt_toDate = BaseParameter.ListSearchString[1];
                string txt_MC = BaseParameter.ListSearchString[2];

                string DateFrom = DateTime.Parse(txt_FromDate).ToString("yyyy-MM-dd");
                string DateTo = DateTime.Parse(txt_toDate).AddDays(1).ToString("yyyy-MM-dd");
                string MC = string.IsNullOrEmpty(txt_MC) ? "All" : txt_MC.ToUpper();

                UpdateJobStatus(jobId, "Đang truy vấn database...", 0, 0, 5);

                switch (BaseParameter.Action)
                {
                    case 3:
                        reportTitle = "BÁO CÁO CÔNG ĐOẠN CUTTING";
                        fileName = $"{SheetName}_SPC_Cutting_Report_{DateFrom}_{DateTo}.xlsx";
                        data = await GetCuttingSPC(BaseParameter, true);
                        break;
                    case 4:
                        reportTitle = "BÁO CÁO CÔNG ĐOẠN CRIMPING";
                        fileName = $"{SheetName}_SPC_Crimping_Report_{DateFrom}_{DateTo}.xlsx";
                        data = await GetCrimpingSPC(BaseParameter, true);
                        break;
                }

                if (data == null || data.Count == 0)
                {
                    UpdateJobStatus(jobId, "Failed", 0, 0, 0, "Không có dữ liệu để xuất");
                    return;
                }

                int totalRows = data.Count;
                _logger.LogInformation($"Export job {jobId}: Retrieved {totalRows} records");

                UpdateJobStatus(jobId, $"Đã lấy {totalRows} dòng dữ liệu", 0, totalRows, 8);

                // Tạo file Excel với callback cập nhật tiến độ
                using (var streamExport = new MemoryStream())
                {
                    // Truyền jobId để cập nhật tiến độ chi tiết
                    await InitializationToExcelWithProgressAsync(
                        data,
                        streamExport,
                        SheetName,
                        DateFrom,
                        DateTo,
                        MC,
                        reportTitle,
                        jobId,
                        totalRows,
                        (currentRow) => {
                            // Callback nếu cần xử lý thêm
                        }
                    );

                    UpdateJobStatus(jobId, "Đang lưu file...", totalRows, totalRows, 90);

                    // Lưu file
                    string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPath);

                    // Xóa file cũ hơn 1 giờ
                    CleanupOldFiles(physicalPath, TimeSpan.FromHours(1));

                    string filePath = Path.Combine(physicalPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        streamExport.Position = 0;
                        await streamExport.CopyToAsync(fileStream);
                    }

                    UpdateJobStatus(jobId, "Đang hoàn tất...", totalRows, totalRows, 95);

                    // Kiểm tra file đã được tạo
                    if (System.IO.File.Exists(filePath))
                    {
                        string downloadPath = Path.Combine(GlobalHelper.Download, SheetName, fileName).Replace("\\", "/");
                        string downloadUrl = $"/{downloadPath}";

                        _logger.LogInformation($"Export job {jobId}: File created at {filePath}");

                        // Cập nhật kết quả hoàn thành
                        var jobInfo = CacheHelper.Get<ExportJobInfo>(jobId);
                        if (jobInfo != null)
                        {
                            jobInfo.Status = "Completed";
                            jobInfo.DownloadUrl = downloadUrl;
                            jobInfo.FilePath = filePath;
                            jobInfo.Progress = 100;
                            jobInfo.CurrentRow = totalRows;
                            jobInfo.TotalRows = totalRows;
                            jobInfo.CurrentAction = "Hoàn tất";
                            jobInfo.CompletedAt = DateTime.Now;
                            jobInfo.LastUpdated = DateTime.Now;
                            CacheHelper.Set(jobId, jobInfo, TimeSpan.FromMinutes(30));
                        }

                        _logger.LogInformation($"Export job {jobId} completed successfully");
                    }
                    else
                    {
                        throw new Exception("Không thể tạo file trên server");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing export job {jobId}");

                var jobInfo = CacheHelper.Get<ExportJobInfo>(jobId);
                if (jobInfo != null)
                {
                    jobInfo.Status = "Failed";
                    jobInfo.Error = ex.Message;
                    jobInfo.LastUpdated = DateTime.Now;
                    CacheHelper.Set(jobId, jobInfo, TimeSpan.FromMinutes(30));
                }
            }
        }

        private async Task InitializationToExcelWithProgressAsync(
            List<SuperResultTranfer> list,
            MemoryStream streamExport,
            string sheetName,
            string fromDate,
            string toDate,
            string MC,
            string reportTile,
            string jobId,
            int totalOverallRows,
            Action<int> progressCallback)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                // TẮT AUTO CALCULATE và EVENT
                package.Workbook.CalcMode = ExcelCalcMode.Manual;

                // Sheet 1: Chi tiết
                var workSheet = package.Workbook.Worksheets.Add("SPC Detail");

                // Định nghĩa headers
                string[] headers = new string[]
                {
            "Part No", "Week", "Lead No", "Project", "QtyPlan", "QtyActual", "Bundle size", "Date",
            "MC Plan", "MC Actual", "Hook Rack", "wire", "WIRE_NM", "W_Diameter", "W_Color", "W_Length",
            "Term1", "Strip1", "SEAL1", "cch_w1", "ich_w1", "Term2", "Strip2", "seal2", "cch_w2", "ich_w2",
            "CONDITION", "Fctry_nm", "COLSIP", "CCH1", "CCW1", "CCH2", "CCW2", "ICH1", "ICW1", "ICH2", "ICW2",
            "WIRE_FORCE", "WIRE_LENGTH", "INS_RESILT", "Check Time", "Check By", "Full Name"
                };

                // Ghi headers
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = workSheet.Cells[1, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Font.Name = "Arial";
                    cell.Style.Font.Size = 14;
                    cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Chuẩn bị dữ liệu để ghi hàng loạt
                int totalRows = list?.Count ?? 0;
                if (totalRows == 0) return;

                var dataRange = workSheet.Cells[2, 1, totalRows + 1, headers.Length];

                // Tạo mảng 2 chiều để ghi nhanh
                object[,] values = new object[totalRows, headers.Length];

                // Cập nhật tiến độ
                UpdateJobStatus(jobId, $"Đang chuẩn bị dữ liệu: 0/{totalRows} dòng", 0, totalRows, 10);

                int currentRow = 0;
                foreach (var item in list)
                {
                    if (item == null)
                    {
                        currentRow++;
                        continue;
                    }

                    int col = 0;

                    // Sử dụng SafeGet để xử lý null
                    values[currentRow, col++] = SafeGetString(item.TORDER_FG);
                    values[currentRow, col++] = SafeGetNumber(item.WORK_WEEK);
                    values[currentRow, col++] = SafeGetString(item.LEAD_NO);
                    values[currentRow, col++] = SafeGetString(item.PROJECT);
                    values[currentRow, col++] = SafeGetNumber(item.QtyPlan);
                    values[currentRow, col++] = SafeGetNumber(item.QtyActual);
                    values[currentRow, col++] = SafeGetNumber(item.BUNDLE_SIZE);
                    values[currentRow, col++] = SafeGetDateString(item.DATE, "yyyy-MM-dd");
                    values[currentRow, col++] = SafeGetString(item.MCPlan);
                    values[currentRow, col++] = SafeGetString(item.MC2);
                    values[currentRow, col++] = SafeGetString(item.HOOK_RACK);
                    values[currentRow, col++] = SafeGetString(item.WIRE);
                    values[currentRow, col++] = SafeGetString(item.WIRE_NM);
                    values[currentRow, col++] = SafeGetString(item.W_Diameter);
                    values[currentRow, col++] = SafeGetString(item.W_Color);
                    values[currentRow, col++] = SafeGetNumber(item.W_Length);
                    values[currentRow, col++] = SafeGetString(item.TERM1);
                    values[currentRow, col++] = SafeGetString(item.STRIP1);
                    values[currentRow, col++] = SafeGetString(item.SEAL1);
                    values[currentRow, col++] = SafeGetString(item.CCH_W1);
                    values[currentRow, col++] = SafeGetString(item.ICH_W1);
                    values[currentRow, col++] = SafeGetString(item.TERM2);
                    values[currentRow, col++] = SafeGetString(item.STRIP2);
                    values[currentRow, col++] = SafeGetString(item.SEAL2);
                    values[currentRow, col++] = SafeGetString(item.CCH_W2);
                    values[currentRow, col++] = SafeGetString(item.ICH_W2);
                    values[currentRow, col++] = SafeGetString(item.CONDITION);
                    values[currentRow, col++] = SafeGetString(item.FCTRY_NM);
                    values[currentRow, col++] = SafeGetString(item.COLSIP);
                    values[currentRow, col++] = SafeGetString(item.CCH1);
                    values[currentRow, col++] = SafeGetString(item.CCW1);
                    values[currentRow, col++] = SafeGetString(item.CCH2);
                    values[currentRow, col++] = SafeGetString(item.CCW2);
                    values[currentRow, col++] = SafeGetString(item.ICH1);
                    values[currentRow, col++] = SafeGetString(item.ICW1);
                    values[currentRow, col++] = SafeGetString(item.ICH2);
                    values[currentRow, col++] = SafeGetString(item.ICW2);
                    values[currentRow, col++] = SafeGetString(item.WIRE_FORCE);
                    values[currentRow, col++] = SafeGetString(item.WIRE_LENGTH);
                    values[currentRow, col++] = SafeGetString(item.IN_RESILT);
                    values[currentRow, col++] = item.TestTime;
                    values[currentRow, col++] = SafeGetString(item.CREATE_USER);
                    values[currentRow, col++] = SafeGetString(item.FullName);

                    currentRow++;

                    // Cập nhật tiến độ sau mỗi 100 dòng
                    if (currentRow % 100 == 0)
                    {
                        int detailProgress = 10 + (int)((double)currentRow / totalRows * 40);
                        UpdateJobStatus(jobId, $"Đang chuẩn bị dữ liệu: {currentRow}/{totalRows} dòng", currentRow, totalRows, detailProgress);
                    }

                    progressCallback?.Invoke(currentRow);
                }

                // Ghi tất cả dữ liệu cùng lúc
                dataRange.Value = values;

                // Format hàng loạt
                var dataRows = workSheet.Cells[2, 1, totalRows + 1, headers.Length];
                dataRows.Style.Font.Name = "Arial";
                dataRows.Style.Font.Size = 11;
                dataRows.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRows.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRows.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                dataRows.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                // Auto fit columns
                workSheet.Cells[1, 1, totalRows + 1, headers.Length].AutoFitColumns();

                // Cập nhật tiến độ
                UpdateJobStatus(jobId, "Đã ghi xong dữ liệu chi tiết", totalRows, totalRows, 50);

                // Sheet 2: Tổng hợp SPC
                try
                {
                    await CreateSPCReportSheetOptimized(
                        package,
                        list,
                        fromDate,
                        toDate,
                        MC,
                        reportTile,
                        jobId,
                        totalRows,
                        50,
                        85);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not create SPC report sheet");
                }

                // Bật lại AutoCalculate nếu cần
                package.Workbook.CalcMode = ExcelCalcMode.Automatic;
                package.Save();

                UpdateJobStatus(jobId, "Đã tạo xong file Excel", totalRows, totalRows, 90);
            }
            streamExport.Position = 0;
        }

        // Các helper method xử lý null an toàn
        private string SafeGetString(object value)
        {
            if (value == null) return "";
            if (value == DBNull.Value) return "";
            return value.ToString()?.Trim() ?? "";
        }

        private double SafeGetNumber(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        private int SafeGetInt(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        private string SafeGetDateString(DateTime? date, string format)
        {
            if (date == null || !date.HasValue) return "";
            try
            {
                return date.Value.ToString(format);
            }
            catch
            {
                return "";
            }
        }

        private string SafeGetDateTimeString(DateTime? dateTime, string format)
        {
            if (dateTime == null || !dateTime.HasValue) return "";
            try
            {
                return dateTime.Value.ToString(format);
            }
            catch
            {
                return "";
            }
        }

        private string SafeGetDecimalString(decimal? value)
        {
            if (value == null || !value.HasValue) return "";
            return value.Value.ToString();
        }

        private async Task CreateSPCReportSheetOptimized(
     ExcelPackage package,
     List<SuperResultTranfer> list,
     string fromDate,
     string toDate,
     string MC,
     string reportTile,
     string jobId,
     int totalOverallRows,
     int baseProgress,
     int maxProgress)
        {
            var fileMau = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "SPC_Report.xlsx");
            if (!File.Exists(fileMau))
            {
                _logger.LogWarning($"Template file not found: {fileMau}");
                return;
            }

            try
            {
                using (var openPackage = new ExcelPackage(new FileInfo(fileMau)))
                {
                    if (openPackage.Workbook.Worksheets.Count == 0) return;

                    var templateSheet = openPackage.Workbook.Worksheets.First();
                    if (templateSheet == null) return;

                    UpdateJobStatus(jobId, "Đang xử lý dữ liệu SPC...", 0, 0, baseProgress);

                    // Xử lý dữ liệu SPC
                    int processStartProgress = baseProgress;
                    int processEndProgress = baseProgress + 15;
                    var spcData = ProcessSPCDataOptimized(list, jobId, processStartProgress, processEndProgress);

                    int totalSpcRows = spcData?.Count ?? 0;
                    if (totalSpcRows == 0)
                    {
                        _logger.LogWarning("No SPC data to process");
                        return;
                    }

                    UpdateJobStatus(jobId, "Đang tạo báo cáo SPC...", 0, totalSpcRows, processEndProgress);

                    var workSheet = package.Workbook.Worksheets.Add("SPC Total Report", templateSheet);

                    // Điền thông tin header - kiểm tra null
                    try
                    {
                        workSheet.Cells["E6"].Value = SafeGetString($"{fromDate} to {toDate}");
                        workSheet.Cells["AB6"].Value = SafeGetString(MC);
                        workSheet.Cells["E2"].Value = SafeGetString(reportTile);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error setting header values");
                    }

                    // Insert rows
                    int startRow = 10;
                    if (spcData.Count > 3)
                    {
                        try
                        {
                            workSheet.InsertRow(startRow + 2, spcData.Count - 3);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error inserting rows");
                        }
                    }

                    // Chuẩn bị dữ liệu để ghi hàng loạt
                    int columns = 38; // Số cột dữ liệu
                    object[,] values = new object[spcData.Count, columns];

                    int writeStartProgress = processEndProgress + 5;
                    int writeEndProgress = maxProgress - 5;
                    int writeRange = writeEndProgress - writeStartProgress;

                    int rowIdx = 0;
                    foreach (var item in spcData)
                    {
                        if (item == null)
                        {
                            rowIdx++;
                            continue;
                        }

                        int col = 0;
                        values[rowIdx, col++] = rowIdx + 1; // STT
                        values[rowIdx, col++] = SafeGetString(item.LEAD_NO);
                        values[rowIdx, col++] = SafeGetString(item.WIRE);
                        values[rowIdx, col++] = SafeGetString(item.TERM1);
                        values[rowIdx, col++] = SafeGetString(item.TERM2);
                        values[rowIdx, col++] = SafeGetString(item.SEAL1);
                        values[rowIdx, col++] = SafeGetString(item.SEAL2);
                        values[rowIdx, col++] = SafeGetNumber(item.WIRE_FORCEStandard ?? 10);
                        values[rowIdx, col++] = SafeGetString(item.WIRE_FORCEFirst);
                        values[rowIdx, col++] = SafeGetString(item.WIRE_FORCEMid);
                        values[rowIdx, col++] = SafeGetString(item.WIRE_FORCEEnd);
                        values[rowIdx, col++] = SafeGetNumber(item.LengthStandard);
                        values[rowIdx, col++] = SafeGetNumber(item.LengthFirst);
                        values[rowIdx, col++] = SafeGetNumber(item.LengthMid);
                        values[rowIdx, col++] = SafeGetNumber(item.LengthEnd);
                        values[rowIdx, col++] = SafeGetString(item.CCH1Standard);
                        values[rowIdx, col++] = SafeGetString(item.CCH1Firt);
                        values[rowIdx, col++] = SafeGetString(item.CCH1Mid);
                        values[rowIdx, col++] = SafeGetString(item.CCH1End);
                        values[rowIdx, col++] = SafeGetString(item.CCW1Standard);
                        values[rowIdx, col++] = SafeGetString(item.CCW1Firt);
                        values[rowIdx, col++] = SafeGetString(item.CCW1Mid);
                        values[rowIdx, col++] = SafeGetString(item.CCW1End);
                        values[rowIdx, col++] = SafeGetString(item.CCH2Standard);
                        values[rowIdx, col++] = SafeGetString(item.CCH2Firt);
                        values[rowIdx, col++] = SafeGetString(item.CCH2Mid);
                        values[rowIdx, col++] = SafeGetString(item.CCH2End);
                        values[rowIdx, col++] = SafeGetString(item.CCW2Standard);
                        values[rowIdx, col++] = SafeGetString(item.CCW2Firt);
                        values[rowIdx, col++] = SafeGetString(item.CCW2Mid);
                        values[rowIdx, col++] = SafeGetString(item.CCW2End);
                        values[rowIdx, col++] = ""; // Empty column
                        values[rowIdx, col++] = ""; // Empty column
                        values[rowIdx, col++] = ""; // Empty column
                        values[rowIdx, col++] = SafeGetNumber(item.QtyActual);
                        values[rowIdx, col++] = SafeGetNumber(item.QtyActual);
                        values[rowIdx, col++] = ""; // Empty column
                        values[rowIdx, col++] = SafeGetString(item.MC);

                        rowIdx++;

                        // Cập nhật tiến độ
                        if (rowIdx % 20 == 0 || rowIdx == spcData.Count)
                        {
                            int writeProgress = writeStartProgress + (int)((double)rowIdx / spcData.Count * writeRange);
                            UpdateJobStatus(jobId, $"Đang ghi báo cáo SPC: {rowIdx}/{spcData.Count} dòng", rowIdx, spcData.Count, writeProgress);
                        }
                    }

                    // Ghi dữ liệu hàng loạt
                    var dataRange = workSheet.Cells[startRow, 2, startRow + spcData.Count - 1, 2 + columns - 1];
                    dataRange.Value = values;

                    // Format hàng loạt
                    dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    dataRange.Style.Font.Name = "Arial";
                    dataRange.Style.Font.Size = 11;
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    UpdateJobStatus(jobId, "Hoàn tất báo cáo SPC", spcData.Count, spcData.Count, maxProgress - 5);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SPC report sheet");
                UpdateJobStatus(jobId, "Lỗi khi tạo báo cáo SPC", 0, 0, baseProgress, SafeGetString(ex.Message));
            }

            await Task.CompletedTask;
        }

        private List<SuperResultTranfer> ProcessSPCDataOptimized(
            List<SuperResultTranfer> list,
            string jobId,
            int baseProgress,
            int maxProgress)
        {
            if (list == null || list.Count == 0)
            {
                return new List<SuperResultTranfer>();
            }

            // Sử dụng Dictionary để tăng tốc tìm kiếm
            var resultDict = new Dictionary<string, SuperResultTranfer>();
            int totalItems = list.Count;
            int currentItem = 0;

            UpdateJobStatus(jobId, $"Đang xử lý dữ liệu SPC: 0/{totalItems} items", 0, totalItems, baseProgress);

            foreach (var item in list)
            {
                if (item == null)
                {
                    currentItem++;
                    continue;
                }

                string key = $"{item.ORDER_IDX ?? 0}_{item.DATE?.Date:yyyyMMdd}";

                if (!resultDict.TryGetValue(key, out var existing))
                {
                    existing = new SuperResultTranfer
                    {
                        ORDER_IDX = item.ORDER_IDX,
                        DATE = item.DATE,
                        LEAD_NO = SafeGetString(item.LEAD_NO),
                        WIRE = SafeGetString(item.WIRE),
                        TERM1 = SafeGetString(item.TERM1),
                        TERM2 = SafeGetString(item.TERM2),
                        SEAL1 = SafeGetString(item.SEAL1),
                        SEAL2 = SafeGetString(item.SEAL2),
                        QtyActual = item.QtyActual,
                        MC = SafeGetString(item.MC2 ?? item.MC),
                        WIRE_FORCEStandard = item.STRENGTH,
                        LengthStandard = item.W_Length,
                        CCH1Standard = ParseStandard(item.CCH_W1, 0),
                        CCW1Standard = ParseStandard(item.CCH_W1, 1),
                        CCH2Standard = ParseStandard(item.CCH_W2, 0),
                        CCW2Standard = ParseStandard(item.CCH_W2, 1)
                    };
                    resultDict[key] = existing;
                }

                // Update values
                UpdateSPCValues(existing, item);

                currentItem++;

                if (currentItem % 100 == 0 || currentItem == totalItems)
                {
                    int progress = baseProgress + (int)((double)currentItem / totalItems * (maxProgress - baseProgress));
                    UpdateJobStatus(jobId, $"Đang xử lý dữ liệu SPC: {currentItem}/{totalItems} items", currentItem, totalItems, progress);
                }
            }

            var result = resultDict.Values.ToList();
            UpdateJobStatus(jobId, $"Hoàn tất xử lý dữ liệu SPC: {totalItems} items", totalItems, totalItems, maxProgress);

            return result;
        }
        private void CleanupOldFiles(string path, TimeSpan olderThan)
        {
            try
            {
                if (!Directory.Exists(path)) return;

                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        if (DateTime.Now - fileInfo.CreationTime > olderThan)
                        {
                            File.Delete(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Could not delete file: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error cleaning up old files");
            }
        }

        private void SetCellValue(ExcelWorksheet worksheet, int row, int column, object value, string format = null)
        {
            if (value != null)
            {
                if (value is DateTime dateTime)
                {
                    worksheet.Cells[row, column].Value = dateTime;
                    if (!string.IsNullOrEmpty(format))
                    {
                        worksheet.Cells[row, column].Style.Numberformat.Format = format;
                    }
                }
                else
                {
                    worksheet.Cells[row, column].Value = value;
                }
            }
            else
            {
                worksheet.Cells[row, column].Value = "";
            }
        }

        private string ParseStandard(string value, int index)
        {
            if (string.IsNullOrEmpty(value)) return "0";
            try
            {
                var parts = value.Split('/');
                return parts != null && parts.Length > index ? (parts[index] ?? "0") : "0";
            }
            catch
            {
                return "0";
            }
        }

        private void UpdateSPCValues(SuperResultTranfer target, SuperResultTranfer source)
        {
            if (target == null || source == null) return;

            switch (source.COLSIP?.ToLower())
            {
                case "first":
                    target.WIRE_FORCEFirst = source.WIRE_FORCE;
                    target.LengthFirst = source.WIRE_LENGTH;
                    target.CCH1Firt = SafeGetString(source.CCH1?.ToString() ?? "-");
                    target.CCW1Firt = SafeGetString(source.CCW1?.ToString() ?? "-");
                    target.CCH2Firt = SafeGetString(source.CCH2?.ToString() ?? "-");
                    target.CCW2Firt = SafeGetString(source.CCW2?.ToString() ?? "-");
                    break;
                case "middle":
                    target.WIRE_FORCEMid = source.WIRE_FORCE;
                    target.LengthMid = source.WIRE_LENGTH;
                    target.CCH1Mid = SafeGetString(source.CCH1?.ToString() ?? "-");
                    target.CCW1Mid = SafeGetString(source.CCW1?.ToString() ?? "-");
                    target.CCH2Mid = SafeGetString(source.CCH2?.ToString() ?? "-");
                    target.CCW2Mid = SafeGetString(source.CCW2?.ToString() ?? "-");
                    break;
                case "end":
                    target.WIRE_FORCEEnd = source.WIRE_FORCE;
                    target.LengthEnd = source.WIRE_LENGTH;
                    target.CCH1End = SafeGetString(source.CCH1?.ToString() ?? "-");
                    target.CCW1End = SafeGetString(source.CCW1?.ToString() ?? "-");
                    target.CCH2End = SafeGetString(source.CCH2?.ToString() ?? "-");
                    target.CCW2End = SafeGetString(source.CCW2?.ToString() ?? "-");
                    break;
            }
        }
    }
}