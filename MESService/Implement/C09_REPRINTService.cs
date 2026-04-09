
namespace MESService.Implement
{
    public class C09_REPRINTService : BaseService<torderlist, ItorderlistRepository>
    , IC09_REPRINTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C09_REPRINTService(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C02_REPRINT_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var ORDER_NO = BaseParameter.SearchString;
                    string sql = @"SELECT `B`.`TORDER_BARCODENM`, `B`.`Barcode_SEQ`,  `A`.`LEAD_NO`, `A`.`PO_DT`, `A`.`MC`,
                    (SELECT `HOOK_RACK` FROM trackmaster WHERE trackmaster.`LEAD_NO` = `A`.`LEAD_NO`) AS `HOOK_RACK`,
                    `A`.`PO_QTY`,  `A`.`BUNDLE_SIZE`, `A`.`SAFTY_QTY`,   `A`.`OR_NO`,  `A`.`ORDER_IDX`,`A`.`WORK_WEEK`,    
                    `A`.`PERFORMN`, `A`.`CONDITION`, `A`.`LEAD_COUNT`, `A`.`ERROR_YN`, 
                    (SELECT `LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `A`.`LEAD_NO`) AS `LEAD_IDX`, `B`.`WORK_END`

                    FROM TORDERLIST_SPST `A` INNER JOIN TORDER_BARCODE_SP `B` ON `A`.`ORDER_IDX` = `B`.`ORDER_IDX`
                    WHERE `A`.`DSCN_YN` = 'Y' AND `B`.`DSCN_YN` = 'Y' AND `A`.`ORDER_IDX` = '" + ORDER_NO + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    sql = @"SELECT   `A`.`ORDER_IDX`, `A`.`LEAD_NO` AS `M_LEAD`, `B`.LEAD_NO
                                   FROM TORDERLIST_SPST `A` LEFT JOIN 
                                    (SELECT  (SELECT `LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`) AS `MLEAD_NO`,
                                    (SELECT `LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `LEAD_NO`
                                   FROM   torder_lead_bom_spst)  `B` ON `A`.`LEAD_NO` = `B`.`MLEAD_NO`
                                   WHERE `A`.`ORDER_IDX` = '" + ORDER_NO + "'";

                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> PrintDocument1_PrintPage(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    string C_USER = BaseParameter.USER_IDX;
                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.DataGridView != null)
                        {
                            if (BaseParameter.DataGridView.Count > 0)
                            {
                                var BARCODE_QR = BaseParameter.ListSearchString[0];
                                var BARCODE_DATE = DateTime.Now;
                                var PR1 = "";
                                var PR2 = DateTime.Now.ToString("yyyy-MM-dd");
                                var PR3 = C_USER;
                                var PR4 = BaseParameter.ListSearchString[1];
                                if (PR4.Length > 10)
                                {
                                    PR4 = PR4.Substring(0, 10);
                                }
                                var PR5 = BaseParameter.ListSearchString[2];
                                var PR7 = BaseParameter.ListSearchString[3];
                                var PR8 = BaseParameter.ListSearchString[4];
                                var PR20 = BaseParameter.ListSearchString[5];
                                var PR23 = BaseParameter.ListSearchString[6];
                                var PR24 = "RE";
                                var LEAD_BOM = new List<string>();
                                for (int i = 0; i < 30; i++)
                                {
                                    if (i < BaseParameter.DataGridView.Count)
                                    {
                                        LEAD_BOM.Add(BaseParameter.DataGridView[i].LEAD_NO);
                                    }
                                    else
                                    {
                                        LEAD_BOM.Add("");
                                    }
                                }
                                string sql = @"SELECT 'After Length' AS `M_NAME`,
                                (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`) AS `M_PART`,
                                MAX((SELECT torder_lead_bom.`W_Length` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`)) AS `M_W_Length`,
                                'Before Length' AS `S_NAME`,
                                (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `S_PART`,
                                MAX((SELECT torder_lead_bom.`W_Length` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`)) AS `S_W_Length`
 
                                FROM  torder_lead_bom_spst
                                WHERE  
                                torder_lead_bom_spst.`M_PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + PR5 + "')   ";

                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView2 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                var M_LEN = "";
                                var S_LEN = "";
                                if (result.DataGridView2.Count > 0)
                                {
                                    M_LEN = result.DataGridView2[0].M_NAME + " :" + result.DataGridView2[0].M_W_Length;
                                    S_LEN = result.DataGridView2[0].S_NAME + " :" + result.DataGridView2[0].S_W_Length;
                                }
                                StringBuilder HTMLContent = new StringBuilder();
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLC09_REPRINT(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, PR1, PR2, PR3, PR4, PR5, PR7, PR8, PR20, PR23, PR24, M_LEN, S_LEN, LEAD_BOM));
                                result.Code = GlobalHelper.CreateHTMLClose(SheetName, _WebHostEnvironment.WebRootPath, HTMLContent.ToString());
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
    }
}

