
namespace MESService.Implement
{
    public class C01Service : BaseService<torderlist, ItorderlistRepository>
    , IC01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C01Service(ItorderlistRepository torderlistRepository

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
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AA = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                            var BB = BaseParameter.ListSearchString[1];

                            string sql = @"SELECT
                            IF(`AAA`.`CHK` = 'NG' , 'NG', 'OK') AS `Description`,
                            `AAA`.`OR_NO`, `AAA`.`WORK_WEEK`, `AAA`.`FCTRY_NM`, `AAA`.`TORDER_FG`,  `AAA`.`LEAD_NO`, `AAA`.`PROJECT`, 
                            `AAA`.`TOT_QTY`, `AAA`.`ADJ_AF_QTY`,

                            `AAA`.`TOT_QTY` - IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`TOT_QTY`-`AAA`.`STOCK`), 
		                            IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`TOT_QTY`, 0, 
		                            `AAA`.`TOT_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) AS `USED_STOCK`,

                            IF(`AAA`.`OR_NO` = 'EVENT', `AAA`.`TOEXCEL_QTY`, 

                            IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`TOT_QTY`-`AAA`.`STOCK`), 
		                            IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`TOT_QTY`, 0, 
		                            `AAA`.`TOT_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))))  ) AS `MES_ORDER`,

                            `AAA`.`CUR_LEADS`, `AAA`.`CT_LEADS`, `AAA`.`CT_LEADS_PR`, `AAA`.`GRP`, `AAA`.`PRT`, `AAA`.`DT`, 
                            `AAA`.`MC`, `AAA`.`BUNDLE_SIZE`, `AAA`.`HOOK_RACK`, `AAA`.`WIRE`, 
                            `AAA`.`T1_DIR`, `AAA`.`TERM1`, `AAA`.`STRIP1`, `AAA`.`SEAL1`, `AAA`.`CCH_W1`, `AAA`.`ICH_W1`, 
                            `AAA`.`T2_DIR`, `AAA`.`TERM2`, `AAA`.`STRIP2`, `AAA`.`SEAL2`, `AAA`.`CCH_W2`, `AAA`.`ICH_W2`, 
                            `AAA`.`SP_ST`, `AAA`.`REP`, `AAA`.`ORDER_IDX`

                            FROM (
                            SELECT 
                            `AA`.`CHK`, `AA`.`OR_NO`, `AA`.`WORK_WEEK`, `AA`.`FCTRY_NM`, `AA`.`TORDER_FG`,  `AA`.`LEAD_NO`, `AA`.`PROJECT`, 
                            `AA`.`TOT_QTY`, `AA`.`ADJ_AF_QTY`, 
                            `AA`.`STCK_CHK`, 
                            `AA`.`STOCK`, 
                            `AA`.`ACT_STOCK`,
                            IF(`AA`.`ROWNO` = 1, IF((`AA`.`ACT_STOCK`-`AA`.`TOT_QTY`) > 0, `AA`.`ACT_STOCK`-`AA`.`TOT_QTY`, 0), 
                                IF((IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`TOT_QTY`) > 0, 
                                       (IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`TOT_QTY`), 0)) AS `BE_DATE`,

                            `AA`.`ROWNO`, `AA`.`CUR_LEADS`, `AA`.`CT_LEADS`, `AA`.`CT_LEADS_PR`, `AA`.`GRP`, `AA`.`PRT`, `AA`.`DT`, 
                            `AA`.`MC`, `AA`.`BUNDLE_SIZE`, `AA`.`HOOK_RACK`, `AA`.`WIRE`, 
                            `AA`.`T1_DIR`, `AA`.`TERM1`, `AA`.`STRIP1`, `AA`.`SEAL1`, `AA`.`CCH_W1`, `AA`.`ICH_W1`, 
                            `AA`.`T2_DIR`, `AA`.`TERM2`, `AA`.`STRIP2`, `AA`.`SEAL2`, `AA`.`CCH_W2`, `AA`.`ICH_W2`, 
                            `AA`.`SP_ST`, `AA`.`REP`, `AA`.`ORDER_IDX`, `AA`.`TOEXCEL_QTY`

                            FROM(
                            SELECT 
                            `A`.`CHK`, `A`.`OR_NO`, `A`.`WORK_WEEK`, `A`.`FCTRY_NM`, `A`.`TORDER_FG`,  `A`.`LEAD_NO`, `A`.`PROJECT`, `A`.`TOT_QTY`, `A`.`ADJ_AF_QTY`, 
                            `A`.`STCK_CHK`, 
                            IFNULL(`A`.`STOCK`, 0) AS `STOCK`, 
                            IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) AS `ACT_STOCK`,
                            IFNULL(IF((IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`TOT_QTY`) > 0, (IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`TOT_QTY`), 0), 0) AS `STOCK_DTE`,
                            `A`.`ROWNO`, `A`.`CUR_LEADS`, `A`.`CT_LEADS`, `A`.`CT_LEADS_PR`, `A`.`GRP`, `A`.`PRT`, `A`.`DT`, `A`.`MC`, `A`.`BUNDLE_SIZE`, `A`.`HOOK_RACK`, `A`.`WIRE`, 
                            `A`.`T1_DIR`, `A`.`TERM1`, `A`.`STRIP1`, `A`.`SEAL1`, `A`.`CCH_W1`, `A`.`ICH_W1`, 
                            `A`.`T2_DIR`, `A`.`TERM2`, `A`.`STRIP2`, `A`.`SEAL2`, `A`.`CCH_W2`, `A`.`ICH_W2`, `A`.`SP_ST`, `A`.`REP`, `A`.`ORDER_IDX`,  `A`.`TOEXCEL_QTY`

                            FROM (
                            SELECT 
                            IF((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = `TORDER_FG` AND `PART_SCN` = '6') IS NULL, 'NG', `TORDER_FG`) AS `CHK`, 
                            `OR_NO`, `WORK_WEEK`, `TORDER_FG`,  `FCTRY_NM`, `LEAD_NO`, `PROJECT`, `TOT_QTY`, `ADJ_AF_QTY`, 
                            IF((SELECT tiivtr_lead_fg.`QTY` FROM tiivtr_lead_fg
                            WHERE tiivtr_lead_fg.`PART_IDX` = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `LEAD_NO`)) > 0, 'Y', 'N') AS `STCK_CHK`,

                            (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead  WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3') AS `STOCK`,

                            RANK() OVER (PARTITION BY `LEAD_NO` ORDER BY `LEAD_NO`, `TORDER_FG`) AS `ROWNO`,
	 
                            `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `PRT`, `DT`, IFNULL(`MC2`, `MC`) AS `MC`, `BUNDLE_SIZE`, `HOOK_RACK`, `WIRE`, 
                            `T1_DIR`, `TERM1`, `STRIP1`, `SEAL1`, `CCH_W1`, `ICH_W1`, `T2_DIR`, `TERM2`, `STRIP2`, `SEAL2`, `CCH_W2`, `ICH_W2`, `SP_ST`, `REP`, `ORDER_IDX`, `TOEXCEL_QTY`
                            FROM     TORDERLIST

                            WHERE  (`DT` = '" + AA + " 00:00:00')  AND  `CREATE_DTM` = '" + BB + "'  AND TORDERLIST.`DSCN_YN` = 'N' ) `A` ORDER BY `A`.`LEAD_NO`, `A`.`TORDER_FG`, `A`.`ROWNO`  ) `AA`   ) `AAA`   ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }

                    if (BaseParameter.Action == 2)
                    {
                        var AA = BaseParameter.SearchString;

                        string sql = @"SELECT
                        `AAA`.`CHK`, `AAA`.`OR_NO`, `AAA`.`WORK_WEEK`, `AAA`.`FCTRY_NM`, `AAA`.`TORDER_FG`,  `AAA`.`LEAD_NO`, `AAA`.`PROJECT`, 
                        `AAA`.`TOT_QTY`, `AAA`.`ADJ_AF_QTY`,

                        `AAA`.`TOT_QTY` - IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`TOT_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`TOT_QTY`, 0, 
		                        `AAA`.`TOT_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) AS `USED_STOCK`,

                        IF(`AAA`.`OR_NO` = 'EVENT', `AAA`.`TOEXCEL_QTY`, 

                        IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`TOT_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`TOT_QTY`, 0, 
		                        `AAA`.`TOT_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))))  ) AS `MES_ORDER`,

                        `AAA`.`CUR_LEADS`, `AAA`.`CT_LEADS`, `AAA`.`CT_LEADS_PR`, `AAA`.`GRP`, `AAA`.`PRT`, `AAA`.`DT`, 
                        `AAA`.`MC`, `AAA`.`BUNDLE_SIZE`, `AAA`.`HOOK_RACK`, `AAA`.`WIRE`, 
                        `AAA`.`T1_DIR`, `AAA`.`TERM1`, `AAA`.`STRIP1`, `AAA`.`SEAL1`, `AAA`.`CCH_W1`, `AAA`.`ICH_W1`, 
                        `AAA`.`T2_DIR`, `AAA`.`TERM2`, `AAA`.`STRIP2`, `AAA`.`SEAL2`, `AAA`.`CCH_W2`, `AAA`.`ICH_W2`, 
                        `AAA`.`SP_ST`, `AAA`.`REP`, `AAA`.`ORDER_IDX`

                        FROM (
                        SELECT 
                        `AA`.`CHK`, `AA`.`OR_NO`, `AA`.`WORK_WEEK`, `AA`.`FCTRY_NM`, `AA`.`TORDER_FG`,  `AA`.`LEAD_NO`, `AA`.`PROJECT`, 
                        `AA`.`TOT_QTY`, `AA`.`ADJ_AF_QTY`, 
                        `AA`.`STCK_CHK`, 
                        `AA`.`STOCK`, 
                        `AA`.`ACT_STOCK`,
                        IF(`AA`.`ROWNO` = 1, IF((`AA`.`ACT_STOCK`-`AA`.`TOT_QTY`) > 0, `AA`.`ACT_STOCK`-`AA`.`TOT_QTY`, 0), 
                            IF((IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`TOT_QTY`) > 0, 
                                   (IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`TOT_QTY`), 0)) AS `BE_DATE`,

                        `AA`.`ROWNO`, `AA`.`CUR_LEADS`, `AA`.`CT_LEADS`, `AA`.`CT_LEADS_PR`, `AA`.`GRP`, `AA`.`PRT`, `AA`.`DT`, 
                        `AA`.`MC`, `AA`.`BUNDLE_SIZE`, `AA`.`HOOK_RACK`, `AA`.`WIRE`, 
                        `AA`.`T1_DIR`, `AA`.`TERM1`, `AA`.`STRIP1`, `AA`.`SEAL1`, `AA`.`CCH_W1`, `AA`.`ICH_W1`, 
                        `AA`.`T2_DIR`, `AA`.`TERM2`, `AA`.`STRIP2`, `AA`.`SEAL2`, `AA`.`CCH_W2`, `AA`.`ICH_W2`, 
                        `AA`.`SP_ST`, `AA`.`REP`, `AA`.`ORDER_IDX`, `AA`.`TOEXCEL_QTY`

                        FROM(
                        SELECT 
                        `A`.`CHK`, `A`.`OR_NO`, `A`.`WORK_WEEK`, `A`.`FCTRY_NM`, `A`.`TORDER_FG`,  `A`.`LEAD_NO`, `A`.`PROJECT`, `A`.`TOT_QTY`, `A`.`ADJ_AF_QTY`, 
                        `A`.`STCK_CHK`, 
                        IFNULL(`A`.`STOCK`, 0) AS `STOCK`, 
                        IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) AS `ACT_STOCK`,
                        IFNULL(IF((IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`TOT_QTY`) > 0, (IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`TOT_QTY`), 0), 0) AS `STOCK_DTE`,
                        `A`.`ROWNO`, `A`.`CUR_LEADS`, `A`.`CT_LEADS`, `A`.`CT_LEADS_PR`, `A`.`GRP`, `A`.`PRT`, `A`.`DT`, `A`.`MC`, `A`.`BUNDLE_SIZE`, `A`.`HOOK_RACK`, `A`.`WIRE`, 
                        `A`.`T1_DIR`, `A`.`TERM1`, `A`.`STRIP1`, `A`.`SEAL1`, `A`.`CCH_W1`, `A`.`ICH_W1`, 
                        `A`.`T2_DIR`, `A`.`TERM2`, `A`.`STRIP2`, `A`.`SEAL2`, `A`.`CCH_W2`, `A`.`ICH_W2`, `A`.`SP_ST`, `A`.`REP`, `A`.`ORDER_IDX`,  `A`.`TOEXCEL_QTY`

                        FROM (
                        SELECT  'FALSE' AS `CHK`, `OR_NO`, `WORK_WEEK`, `TORDER_FG`,  `FCTRY_NM`, `LEAD_NO`, `PROJECT`, `TOT_QTY`, `ADJ_AF_QTY`, 

                        IF((SELECT tiivtr_lead_fg.`QTY` FROM tiivtr_lead_fg
                        WHERE tiivtr_lead_fg.`PART_IDX` = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `LEAD_NO`)) > 0, 'Y', 'N') AS `STCK_CHK`,

                        (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead  WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`)) AS `STOCK`,

                        RANK() OVER (PARTITION BY `LEAD_NO` ORDER BY `LEAD_NO`, `TORDER_FG`) AS `ROWNO`,
	 
                        `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `PRT`, `DT`, IFNULL(`MC2`, `MC`) AS `MC`, `BUNDLE_SIZE`, `HOOK_RACK`, `WIRE`, 
                        `T1_DIR`, `TERM1`, `STRIP1`, `SEAL1`, `CCH_W1`, `ICH_W1`, `T2_DIR`, `TERM2`, `STRIP2`, `SEAL2`, `CCH_W2`, `ICH_W2`, `SP_ST`, `REP`, `ORDER_IDX`, `TOEXCEL_QTY`
                        FROM     TORDERLIST

                        WHERE  `CREATE_DTM` = '" + AA + "'  AND TORDERLIST.`DSCN_YN` = 'N' ) `A` ORDER BY `A`.`LEAD_NO`, `A`.`TORDER_FG`, `A`.`ROWNO`  ) `AA`   ) `AAA`  ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    if (BaseParameter.Action == 1)
                    {
                        var USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (BaseParameter.DataGridView1 != null)
                            {
                                if (BaseParameter.DataGridView1.Count > 0)
                                {
                                    var ComboBox2 = BaseParameter.ListSearchString[0];
                                    var Label5 = BaseParameter.ListSearchString[1];

                                    var AA = new List<string>();
                                    for (int i = 0; i < 35; i++)
                                    {
                                        AA.Add("");
                                    }
                                    var VALUES = "";
                                    var VALUESSUM = "";
                                    var CREATE_USER = USER_ID;
                                    var FACTORY_NM = ComboBox2;

                                    foreach (var item in BaseParameter.DataGridView1)
                                    {
                                        var LL = 1;
                                        AA[LL] = item.TORDER_FG != null ? item.TORDER_FG : "";
                                        LL = LL + 1;
                                        AA[LL] = item.LEAD_NO != null ? item.LEAD_NO : "";
                                        LL = LL + 1;
                                        AA[LL] = item.PROJECT != null ? item.PROJECT : "";
                                        LL = LL + 1;
                                        try
                                        {
                                            AA[LL] = item.TOT_QTY != null ? item.TOT_QTY.Value.ToString() : "";
                                        }
                                        catch (Exception ex)
                                        {                                            
                                            string Message = ex.Message;
                                        }
                                        LL = LL + 1;

                                       
                                        try
                                        {
                                            AA[LL] = item.ADJ_AF_QTY != null ? item.ADJ_AF_QTY.Value.ToString() : "";
                                        }
                                        catch (Exception ex)
                                        {
                                            string Message = ex.Message;
                                        }
                                        LL = LL + 1;
                                        AA[LL] = item.CUR_LEADS != null ? item.CUR_LEADS : "";
                                        LL = LL + 1;
                                        AA[LL] = item.CT_LEADS != null ? item.CT_LEADS : "";
                                        LL = LL + 1;
                                        AA[LL] = item.CT_LEADS_PR != null ? item.CT_LEADS_PR : "";
                                        LL = LL + 1;
                                        AA[LL] = item.GRP != null ? item.GRP : "";
                                        LL = LL + 1;
                                        AA[LL] = item.PRT != null ? item.PRT : "";
                                        LL = LL + 1;

                                        if (item.DT != null)
                                        {
                                            if(item.DT.Value >= new DateTime( DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day))
                                            {
                                                AA[LL] = item.DT.Value.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                AA[LL] = DateTime.Now.ToString("yyyy-MM-dd");
                                            }
                                        }
                                        else
                                        {
                                            AA[LL] = DateTime.Now.ToString("yyyy-MM-dd");
                                        }
                                        //try
                                        //{
                                        //    AA[LL] = item.DT != null ? item.DT.Value.ToString() : "";
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    string Message = ex.Message;
                                        //}
                                        LL = LL + 1;
                                        AA[LL] = item.Machine != null ? item.Machine : "";
                                        LL = LL + 1;
                                        try
                                        {
                                            AA[LL] = item.BUNDLE_SIZE != null ? item.BUNDLE_SIZE.Value.ToString() : "";
                                        }
                                        catch (Exception ex)
                                        {
                                            string Message = ex.Message;
                                        }
                                        LL = LL + 1;
                                        AA[LL] = item.HOOK_RACK != null ? item.HOOK_RACK : "";
                                        LL = LL + 1;
                                        AA[LL] = item.WIRE != null ? item.WIRE : "";
                                        LL = LL + 1;
                                        AA[LL] = item.T1_DIR != null ? item.T1_DIR : "";
                                        LL = LL + 1;
                                        AA[LL] = item.TERM1 != null ? item.TERM1 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.STRIP1 != null ? item.STRIP1 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.SEAL1 != null ? item.SEAL1 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.CCH_W1 != null ? item.CCH_W1 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.ICH_W1 != null ? item.ICH_W1 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.T2_DIR != null ? item.T2_DIR : "";
                                        LL = LL + 1;
                                        AA[LL] = item.TERM2 != null ? item.TERM2 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.STRIP2 != null ? item.STRIP2 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.SEAL2 != null ? item.SEAL2 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.CCH_W2 != null ? item.CCH_W2 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.ICH_W2 != null ? item.ICH_W2 : "";
                                        LL = LL + 1;
                                        AA[LL] = item.SP_ST != null ? item.SP_ST : "";
                                        LL = LL + 1;
                                        AA[LL] = item.REP != null ? item.REP : "";                             
                                        LL = LL + 1;
                                        AA[LL] = item.PO_CODE != null ? item.PO_CODE : "";
                                        LL = LL + 1;
                                        AA[LL] = item.ECNNo != null ? item.ECNNo : "";
                                        LL = LL + 1;

                                        if (AA[1].Length > 0)
                                        {
                                            if (AA[4] == "")
                                            {
                                                AA[4] = "0";
                                            }
                                            if (AA[5] == "")
                                            {
                                                AA[5] = "0";
                                            }
                                            if (AA[13] == "")
                                            {
                                                AA[13] = "0";
                                            }
                                            var SP_LEN = AA[28].Length;
                                            var SPST_TEXT = "";
                                            if (SP_LEN >= 501)
                                            {
                                                SPST_TEXT = AA[28].Substring(0, 498);
                                            }
                                            else
                                            {
                                                SPST_TEXT = AA[28];
                                            }
                                            if (BaseParameter.CheckBox1 == true)
                                            {
                                                VALUES = "('" + AA[2] + "', '" + AA[3] + "', " + AA[4] + ", " + AA[5] + ", '" + AA[6] + "', '" + AA[7] + "', '" + AA[8] + "', '" + AA[9] + "', '" + AA[10] + "', '" + DateTime.Parse(AA[11]).ToString("yyyy-MM-dd") + "', '" + AA[12] + "', " + AA[13] + ", '" + AA[14] + "', '" + AA[15] + "', '" + AA[16] + "', '" + AA[17] + "', '" + AA[18] + "', '" + AA[19] + "', '" + AA[20] + "', '" + AA[21] + "', '" + AA[22] + "', '" + AA[23] + "', '" + AA[24] + "', '" + AA[25] + "', '" + AA[26] + "', '" + AA[27] + "', '" + SPST_TEXT + "', '" + AA[29] + "', 'N', 'Stay', NOW(), '" + CREATE_USER + "'," + Label5 + ", 'EVENT', '" + FACTORY_NM + "', IF((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO ='" + AA[1] + "') IS NULL, 'NG', '" + AA[1] + "'), " + AA[4] + ", '"+AA[30]+ "','"+AA[31]+"')";
                                            }
                                            else
                                            {
                                                VALUES = "('" + AA[2] + "', '" + AA[3] + "', " + AA[4] + ", " + AA[5] + ", '" + AA[6] + "', '" + AA[7] + "', '" + AA[8] + "', '" + AA[9] + "', '" + AA[10] + "', '" + DateTime.Parse(AA[11]).ToString("yyyy-MM-dd") + "', '" + AA[12] + "', " + AA[13] + ", '" + AA[14] + "', '" + AA[15] + "', '" + AA[16] + "', '" + AA[17] + "', '" + AA[18] + "', '" + AA[19] + "', '" + AA[20] + "', '" + AA[21] + "', '" + AA[22] + "', '" + AA[23] + "', '" + AA[24] + "', '" + AA[25] + "', '" + AA[26] + "', '" + AA[27] + "', '" + SPST_TEXT + "', '" + AA[29] + "', 'N', 'Stay', NOW(), '" + CREATE_USER + "'," + Label5 + ", 'NORMAL', '" + FACTORY_NM + "', IF((SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO ='" + AA[1] + "') IS NULL, 'NG', '" + AA[1] + "'), " + AA[4] + ", '"+AA[30]+ "','"+AA[31]+"')";
                                            }

                                            if (VALUESSUM == "")
                                            {
                                                VALUESSUM = VALUES;
                                            }
                                            else
                                            {
                                                VALUESSUM = VALUESSUM + ", " + VALUES;
                                            }
                                        }
                                                                               
                                    }

                                    string sql = @"INSERT INTO TORDERLIST(`LEAD_NO`, `PROJECT`, `TOT_QTY`, `ADJ_AF_QTY`, `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `PRT`, `DT`, `MC`, `BUNDLE_SIZE`, `HOOK_RACK`, `WIRE`, `T1_DIR`, `TERM1`, `STRIP1`, `SEAL1`, `CCH_W1`, `ICH_W1`, `T2_DIR`, `TERM2`, `STRIP2`, `SEAL2`, `CCH_W2`, `ICH_W2`, `SP_ST`, `REP`, `DSCN_YN`, `CONDITION`, `CREATE_DTM`, `CREATE_USER`, `WORK_WEEK`, `OR_NO`, `FCTRY_NM`, `TORDER_FG`, `TOEXCEL_QTY`, `PO_ID`, `BOM_ID`) VALUES ";
                                    result.ErrorNumber = VALUESSUM.Length;
                                    if (result.ErrorNumber > 0)
                                    {
                                        sql = sql + VALUESSUM;
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                            }
                        }
                    }

                    if (BaseParameter.Action == 2)
                    {
                        var USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.DataGridView2 != null)
                        {
                            if (BaseParameter.DataGridView2.Count > 0)
                            {
                                foreach (var item in BaseParameter.DataGridView2)
                                {
                                    if (item.CHK == true)
                                    {
                                        var AAA = item.MES_ORDER;
                                        var BBB = item.ADJ_AF_QTY;
                                        var CCC = item.ORDER_IDX;

                                        string sql = @"UPDATE TORDERLIST SET TORDERLIST.TOT_QTY = '" + AAA + "' , TORDERLIST.ADJ_AF_QTY ='" + BBB + "',  TORDERLIST.UPDATE_DTM = NOW(), TORDERLIST.UPDATE_USER = '" + USER_ID + "' WHERE TORDERLIST.ORDER_IDX = " + CCC;
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        var USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var CR_DATE = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd HH:mm:ss");
                            var DateTimePicker1 = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd 00:00:00");

                            if (BaseParameter.DataGridView3 != null)
                            {
                                if (BaseParameter.DataGridView3.Count > 0)
                                {
                                    var VALLIST = "";
                                    var VAL = "";
                                    var SUMVAL = "";
                                    foreach (var item in BaseParameter.DataGridView3)
                                    {
                                        if (item.Description == null)
                                        {
                                            item.Description = "NG";
                                        }
                                        if (item.Description == "OK")
                                        {
                                            var PART1 = "";
                                            var PART2 = "";
                                            var PART3 = "";
                                            var PART4 = "";
                                            var PART5 = "";

                                            var CHK1 = false;
                                            var CHK2 = false;
                                            var CHK3 = false;
                                            var CHK4 = false;
                                            var CHK5 = false;

                                            var AA = item.ORDER_IDX;
                                            var GG = decimal.Parse(item.MES_ORDER);
                                            var HH = item.OR_NO;
                                            var KK = 0;
                                            if (HH == "NORMAL")
                                            {
                                                KK = item.BUNDLE_SIZE.Value;
                                                GG = Math.Ceiling(decimal.Parse(item.MES_ORDER) / KK) * KK;
                                            }
                                            VAL = "(" + AA + ", 'N', NOW(), '" + USER_ID + "')";
                                            if (SUMVAL == "")
                                            {
                                                SUMVAL = VAL;
                                            }
                                            else
                                            {
                                                SUMVAL = SUMVAL + ", " + VAL;
                                            }

                                            string sql1 = @"UPDATE TORDERLIST   SET   `TOT_QTY` = '" + GG + "',  `DSCN_YN`='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "', TORDERLIST.`HOOK_RACK` = (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = TORDERLIST.LEAD_NO) WHERE `ORDER_IDX`= '" + AA + "'   ";
                                            string sqlResult1 = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);
                                        }
                                    }
                                    string sql = @"INSERT INTO torder_bom (`ORDER_IDX`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUE " + SUMVAL + "ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"UPDATE TORDERLIST   SET   TORDERLIST.`CONDITION` = 'Close'   WHERE  (TORDERLIST.`TOT_QTY`= 0 OR TORDERLIST.`DSCN_YN`= 'N')  AND TORDERLIST.`CREATE_DTM` = '" + CR_DATE + "'  ";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"INSERT INTO trackmaster (trackmaster.`LEAD_NO`, trackmaster.`SFTY_STK`, trackmaster.`CREATE_DTM`, trackmaster.`CREATE_USER`)
                                    SELECT TORDERLIST.`LEAD_NO`, TORDERLIST.`ADJ_AF_QTY`,'" + CR_DATE + "', '" + USER_ID + "'  FROM TORDERLIST WHERE TORDERLIST.`CREATE_DTM` = (SELECT max(TORDERLIST.`CREATE_DTM`) FROM TORDERLIST ) AND TORDERLIST.`DSCN_YN`= 'Y' GROUP by TORDERLIST.`LEAD_NO` ON DUPLICATE KEY UPDATE trackmaster.`SFTY_STK` = (SELECT TORDERLIST.`ADJ_AF_QTY` FROM TORDERLIST WHERE TORDERLIST.`CREATE_DTM` = (SELECT max(TORDERLIST.`CREATE_DTM`) FROM TORDERLIST ) AND TORDERLIST.`LEAD_NO` = trackmaster.`LEAD_NO`GROUP by trackmaster.`LEAD_NO`), trackmaster.`UPDATE_DTM` = '" + CR_DATE + "', trackmaster.`UPDATE_USER` = '" + USER_ID + "'";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"INSERT INTO TORDERLIST_LP (`ORDER_IDX`, `MC`, `TOT_QTY`, `PERFORMN_L`, `PERFORMN_R`, `CONDITION`, `CREATE_DTM`, `CREATE_USER`)
                                    SELECT `A`.`ORDER_IDX`, 'C00' AS `MC`, `A`.`TOT_QTY`, 0, 0, 'Stay', '" + CR_DATE + "', 'MES' FROM TORDERLIST `A` LEFT JOIN TORDERLIST_LP `B` ON `A`.`ORDER_IDX` = `B`.`ORDER_IDX` WHERE(`A`.DSCN_YN = 'Y') AND NOT(`A`.`CONDITION` = 'Close') AND(`A`.`TERM1` LIKE '(%' OR `A`.`TERM2` LIKE '(%') AND `B`.`ORDER_IDX` IS NULL  AND `A`.`TOT_QTY` > 0 AND (IF(IF((`A`.`TERM1` = '(899997)') = 1, 1, 0) + IF((`A`.`TERM1` = '(899998)') = 1, 1, 0) + IF((`A`.`TERM1` = '(899999)') = 1, 1, 0) > 0 , 0, IF(INSTR(`A`.`TERM1`, ')') > 0, 10, 0))   + IF(IF((`A`.`TERM2` = '(899997)') = 1, 1, 0) + IF((`A`.`TERM2` = '(899998)') = 1, 1, 0) + IF((`A`.`TERM2` = '(899999)') = 1, 1, 0) > 0 , 0, IF(INSTR(`A`.`TERM2`, ')') > 0, 10, 0))) > 8 ";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"INSERT INTO torder_bom_LP (`ORDER_IDX`,  `ERROR_CHK`,  `CREATE_DTM`, `CREATE_USER`) SELECT `ORDER_IDX`,  'N', `CREATE_DTM`, '" + USER_ID + "'   FROM  TORDERLIST_LP  WHERE  `CREATE_DTM` = '" + CR_DATE + "'";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"ALTER TABLE     `torder_bom`     AUTO_INCREMENT= 1";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"ALTER TABLE     `trackmaster`     AUTO_INCREMENT= 1";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


                                    sql = @"SELECT  TORDERLIST.ORDER_IDX, TORDERLIST.LEAD_NO,
                                    IF(INSTR(TORDERLIST.TERM1, '(') = 0,   IF(LENGTH(TORDERLIST.TERM1)>0,  CONCAT('(', TORDERLIST.TERM1, ')'), ''), TORDERLIST.TERM1)  AS `TERM1`,
                                    IF(INSTR(TORDERLIST.SEAL1, '(') = 0,   IF(LENGTH(TORDERLIST.SEAL1)>0,  CONCAT('(', TORDERLIST.SEAL1, ')'), ''), TORDERLIST.SEAL1)  AS `SEAL1`,
                                    IF(INSTR(TORDERLIST.TERM2, '(') = 0,   IF(LENGTH(TORDERLIST.TERM2)>0,  CONCAT('(', TORDERLIST.TERM2, ')'), ''), TORDERLIST.TERM2)  AS `TERM2`,
                                    IF(INSTR(TORDERLIST.SEAL2, '(') = 0,   IF(LENGTH(TORDERLIST.SEAL2)>0,  CONCAT('(', TORDERLIST.SEAL2, ')'), ''), TORDERLIST.SEAL2)  AS `SEAL2`
                                    FROM TORDERLIST JOIN TORDERLIST_LPLIST
                                    ON TORDERLIST.LEAD_NO = TORDERLIST_LPLIST.LPLIST_LEADNO
                                    WHERE TORDERLIST.CREATE_DTM ='" + CR_DATE + "'  AND  `A`.DSCN_YN = 'Y'  ";

                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    var DGV_C01_CPLIST = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        DGV_C01_CPLIST.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }

                                    if (DGV_C01_CPLIST.Count > 0)
                                    {
                                        foreach (var item in DGV_C01_CPLIST)
                                        {
                                            var AAA = item.ORDER_IDX;
                                            var BBB = item.LEAD_NO;
                                            var CCC = item.TERM1;
                                            var DDD = item.SEAL1;
                                            var EEE = item.TERM2;
                                            var FFF = item.SEAL2;

                                            sql = @"UPDATE TORDERLIST SET `TERM1` = '" + CCC + "', `SEAL1` = '" + DDD + "', `TERM2` ='" + EEE + "', `SEAL2` ='" + FFF + "'  WHERE `ORDER_IDX` ='" + AAA + "' AND `LEAD_NO` = '" + BBB + "'   ";
                                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        }
                                    }

                                    sql = @"SELECT  
                                        IF(IFNULL((SELECT LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `LEAD_NO`), 0) = 0, 'Not LEAD_NO', 'Y') AS `CHK`,
                                        `OR_NO`, `WORK_WEEK`, `FCTRY_NM`, `TORDER_FG`, `LEAD_NO`, `PROJECT`, `TOT_QTY`, `ADJ_AF_QTY`, 
                                        (SELECT `QTY` FROM tiivtr_lead WHERE `PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3') AS `Stock`,
                                        IF(`OR_NO` = 'NORMAL', IF(`TOT_QTY` + `ADJ_AF_QTY`  > (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3'), 
                                        IF(CEIL((`TOT_QTY` + `ADJ_AF_QTY`  - (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3'))/`BUNDLE_SIZE`) * `BUNDLE_SIZE` < 500, 
                                        500, CEIL((`TOT_QTY` + `ADJ_AF_QTY`  - (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3'))/`BUNDLE_SIZE`) * `BUNDLE_SIZE`), 0), `TOT_QTY`)  AS `MES_Order`,
                                        `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `PRT`, `DT`, IFNULL(`MC2`, `MC`) AS `MC`, `BUNDLE_SIZE`, `HOOK_RACK`, `WIRE`, `T1_DIR`, `TERM1`,
                                        `STRIP1`, `SEAL1`, `CCH_W1`, `ICH_W1`, `T2_DIR`, `TERM2`, `STRIP2`, `SEAL2`, `CCH_W2`, `ICH_W2`, `SP_ST`, `REP`, `CONDITION`, `ORDER_IDX`
                                        FROM     TORDERLIST
                                        WHERE  `DT` = '" + DateTimePicker1 + "'  AND `TORDERLIST`.`DSCN_YN` = 'N'  AND  `CREATE_DTM` ='" + CR_DATE + "' AND IF(IFNULL((SELECT LEAD_INDEX FROM  torder_lead_bom  WHERE  torder_lead_bom.`LEAD_PN` = `LEAD_NO`), 0) = 0, 'Not LEAD_NO', 'Y') = 'Not LEAD_NO' ";

                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView3 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                }
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 3)
                    {
                        var DEL_AA = BaseParameter.SearchString;
                        string sql = @"DELETE   FROM   TORDERLIST  WHERE   `CREATE_DTM` = '" + DEL_AA + "' AND `DSCN_YN` = 'N'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
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
        public virtual async Task<BaseResult> COM_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var ORDER_DT = DateTime.Parse(BaseParameter.SearchString).ToString("yyyy-MM-dd");

                    string sql = @"SELECT TORDERLIST.CREATE_DTM FROM TORDERLIST WHERE TORDERLIST.DT='" + ORDER_DT + "' AND TORDERLIST.DSCN_YN='N' GROUP BY CREATE_DTM";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    for (int i = 0; i < result.ComboBox1.Count; i++)
                    {
                        result.ComboBox1[i].Description = result.ComboBox1[i].CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COM_LIST2(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT TORDERLIST.`CREATE_DTM` FROM TORDERLIST   
                    WHERE TORDERLIST.`DSCN_YN` ='N' AND TORDERLIST.`CONDITION` ='STAY' AND TORDERLIST.`DT` > DATE_ADD(NOW(), INTERVAL -10 DAY)
                    GROUP BY  TORDERLIST.`CREATE_DTM`  ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox3 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                for (int i = 0; i < result.ComboBox3.Count; i++)
                {
                    result.ComboBox3[i].Description = result.ComboBox3[i].CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss");
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

