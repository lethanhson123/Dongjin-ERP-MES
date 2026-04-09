
namespace MESService.Implement
{
    public class C02_REPRINTService : BaseService<torderlist, ItorderlistRepository>
    , IC02_REPRINTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C02_REPRINTService(ItorderlistRepository torderlistRepository
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
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var Label4 = BaseParameter.ListSearchString[0];
                        var WHERE_TEXT = BaseParameter.ListSearchString[1];
                        string sql = @"SELECT  TORDER_BARCODE.`TORDER_BARCODENM`, TORDER_BARCODE.Barcode_SEQ, TORDER_BARCODE.WORK_END, TORDERLIST.LEAD_NO, TORDERLIST.DT, 
IFNULL(TORDERLIST.MC2, TORDERLIST.MC) AS `MC`, TORDERLIST.HOOK_RACK, TORDERLIST.TOT_QTY, 

IF(TORDERLIST.OR_NO= 'EVENT', 
   IF(TORDERLIST.TOT_QTY > TORDERLIST.BUNDLE_SIZE * TORDER_BARCODE.Barcode_SEQ, 
           TORDERLIST.BUNDLE_SIZE, TORDERLIST.TOT_QTY - TORDERLIST.BUNDLE_SIZE * ( TORDER_BARCODE.Barcode_SEQ -1)),
TORDERLIST.BUNDLE_SIZE) AS `BUNDLE_SIZE`,

TORDERLIST.WIRE, TORDERLIST.TERM1, TORDERLIST.STRIP1, TORDERLIST.SEAL1, TORDERLIST.CCH_W1, TORDERLIST.ICH_W1, 
TORDERLIST.TERM2, TORDERLIST.STRIP2, TORDERLIST.SEAL2, TORDERLIST.CCH_W2, TORDERLIST.ICH_W2, TORDERLIST.PROJECT,TORDERLIST.SP_ST, TORDERLIST.ADJ_AF_QTY
FROM     TORDER_BARCODE, TORDERLIST
WHERE  TORDER_BARCODE.ORDER_IDX = TORDERLIST.ORDER_IDX AND (TORDERLIST.DSCN_YN = 'Y') AND (TORDER_BARCODE.DSCN_YN  = 'Y') AND (TORDERLIST.ORDER_IDX = '" + Label4 + "')" + WHERE_TEXT;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> PrintDocument1_PrintPage(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var BARCODE_DATE = DateTime.Now.ToString();
                        var BARCODE_QR = BaseParameter.ListSearchString[0];
                        var PR1 = BaseParameter.ListSearchString[1];
                        var PR2 = DateTime.Now.ToString("yyyy-MM-dd");
                        var PR3 = C_USER;
                        var PR4 = BaseParameter.ListSearchString[2];
                        var PR5 = BaseParameter.ListSearchString[3];
                        var PR6 = BaseParameter.ListSearchString[4];
                        var PR7 = BaseParameter.ListSearchString[5];
                        var PR8 = BaseParameter.ListSearchString[6];
                        var PR9 = BaseParameter.ListSearchString[7];
                        var PR10 = BaseParameter.ListSearchString[8];
                        var PR11 = BaseParameter.ListSearchString[9];
                        var PR12 = BaseParameter.ListSearchString[10];
                        var PR13 = BaseParameter.ListSearchString[11];
                        var PR14 = BaseParameter.ListSearchString[12];
                        var PR15 = BaseParameter.ListSearchString[13];
                        var PR16 = BaseParameter.ListSearchString[14];
                        var PR17 = BaseParameter.ListSearchString[15];
                        var PR18 = "CCH.W";
                        var PR19 = "ICH.W";
                        var PR20 = BaseParameter.ListSearchString[16];
                        var PR21 = BaseParameter.ListSearchString[17];
                        var PR22 = BaseParameter.ListSearchString[18];
                        var PR23 = BaseParameter.ListSearchString[19];
                        var PR25 = "-";
                        var PRT_3 = BaseParameter.ListSearchString[20];

                        if (PR4.Length > 10)
                        {
                            PR4 = PR4.Substring(0, 10);
                        }

                        string sql = @"SELECT TTENSILBNDLST.STRENGTH FROM TTENSILBNDLST WHERE TTENSILBNDLST.BNDLST_MIN <= (SELECT IFNULL(torder_lead_bom.W_Length, '-') AS `BUND` FROM  torder_lead_bom  WHERE torder_lead_bom.LEAD_PN = '" + PRT_3 + "') AND TTENSILBNDLST.BNDLST_MAX >= (SELECT IFNULL(torder_lead_bom.W_Length, '-') AS `BUND` FROM torder_lead_bom  WHERE torder_lead_bom.LEAD_PN = '" + PRT_3 + "') ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.Search.Count > 0)
                        {
                            PR25 = result.Search[0].STRENGTH.ToString();
                        }
                        var PR24 = "RE";
                        StringBuilder HTMLContent = new StringBuilder();
                        HTMLContent.AppendLine(GlobalHelper.CreateHTMLC02(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, PR1, PR2, PR3, PR4, PR5, PR6, PR7.ToString(), PR8.ToString(), PR9, PR10, PR11, PR12, PR13, PR14, PR15, PR16, PR17, PR18, PR19, PR20.ToString(), PR21, PR22, PR23, PR24, PR25));
                        result.Code = GlobalHelper.CreateHTMLClose(SheetName, _WebHostEnvironment.WebRootPath, HTMLContent.ToString());
                                                
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

