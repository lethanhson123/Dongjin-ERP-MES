namespace MESService.Implement
{
    public class C15Service : BaseService<torderlist, ItorderlistRepository>
    , IC15Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C15Service(ItorderlistRepository torderlistRepository
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
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView1 == null)
                    {
                        BaseParameter.DataGridView1 = new List<SuperResultTranfer>();
                    }
                    result.DataGridView1 = BaseParameter.DataGridView1;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var TextBox1 = BaseParameter.ListSearchString[0];
                        var DateTimePicker1 = BaseParameter.ListSearchString[1];
                        var NumericUpDown1 = BaseParameter.ListSearchString[2];

                        string sql = @"SELECT torder_lead_bom.LEAD_PN, torder_lead_bom.BUNDLE_SIZE FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN LIKE  '" + TextBox1 + "' AND torder_lead_bom.DSCN_YN = 'Y'";
                        string sqlResult = "";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count > 0)
                        {
                            var LEAD_PART = result.Search[0].LEAD_PN;
                            var BNDQTY = result.Search[0].BUNDLE_SIZE;
                            DateTimePicker1 = DateTime.Parse(DateTimePicker1).ToString("yyyy-MM-dd");
                            var item = new SuperResultTranfer();
                            item.Description = DateTimePicker1;
                            item.LEAD_PN = result.Search[0].LEAD_PN;
                            item.BUNDLE_SIZE = result.Search[0].BUNDLE_SIZE;
                            item.QTY = int.Parse(NumericUpDown1);
                            item.Name = C_USER;
                            result.DataGridView1.Add(item);
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
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();

                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                var II = 1;
                                var JJ = item.QTY;
                                var D_LEAD = item.LEAD_PN;
                                var D_BNQTY = item.BUNDLE_SIZE;
                                for (II = 1; II <= JJ; II++)
                                {
                                    string sql = @"SELECT 
                                    'OUT' AS `CAR`,
                                    torder_lead_bom.`LEAD_PN`,
                                    torder_lead_bom.`BUNDLE_SIZE`,
                                    torder_lead_bom.`WIRE_NM`, torder_lead_bom.`W_Diameter`, torder_lead_bom.`W_Color`, torder_lead_bom.`W_Length`,
                                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = torder_lead_bom.`T1_PN_IDX`), '') AS `T1`,
                                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = torder_lead_bom.`T2_PN_IDX`), '')  AS `T2`,
                                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = torder_lead_bom.`S1_PN_IDX`), '')  AS `S1`,
                                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = torder_lead_bom.`S2_PN_IDX`), '')  AS `S2`,
                                    IFNULL(torder_lead_bom.`STRIP1`, '') AS `STRIP1`, IFNULL(torder_lead_bom.`STRIP2`, '') AS `STRIP2`,
                                    IFNULL(torder_lead_bom.`CCH_W1`, '') AS `CCH_W1`, IFNULL(torder_lead_bom.`ICH_W1`, '') AS `ICH_W1`,
                                    IFNULL(torder_lead_bom.`CCH_W2`, '') AS `CCH_W2`, IFNULL(torder_lead_bom.`ICH_W2`, '') AS `ICH_W2`,
                                    IFNULL((SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = '" + D_LEAD + "'), '') AS `HOOK` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + D_LEAD + "' ";
                                    string sqlResult = "";
                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.Search1 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    if (result.Search1.Count > 0)
                                    {
                                        var BARCODE_QR_SEQ = II;
                                        var PR1 = "OUT";
                                        var PR2 = DateTime.Now.ToString("yyyy-MM-dd");
                                        var PR3 = C_USER;
                                        var PR4 = result.Search1[0].HOOK;
                                        if (PR4.Length > 10)
                                        {
                                            PR4 = PR4.Substring(0, 10);
                                        }
                                        var PR5 = D_LEAD;
                                        var PR6 = result.Search1[0].WIRE_NM + " " + result.Search1[0].W_Diameter + " " + result.Search1[0].W_Color + " " + result.Search1[0].W_Length;
                                        var PR7 = D_BNQTY * JJ;
                                        var PR8 = D_BNQTY;
                                        var PR9 = result.Search1[0].T1;
                                        var PR10 = result.Search1[0].T2;
                                        var PR11 = result.Search1[0].S1;
                                        var PR12 = result.Search1[0].S2;
                                        var PR13 = result.Search1[0].CCH_W1;
                                        var PR14 = result.Search1[0].ICH_W1;
                                        var PR15 = result.Search1[0].CCH_W2;
                                        var PR16 = result.Search1[0].ICH_W2;
                                        var PR20 = BARCODE_QR_SEQ;
                                        var PR21 = result.Search1[0].STRIP1;
                                        var PR22 = result.Search1[0].STRIP2;
                                        var BARCODE_QR = D_LEAD.ToUpper() + "$$" + D_BNQTY + "$$" + PR3 + "$$OUT$$" + BARCODE_QR_SEQ + "@@";

                                        HTMLContent.AppendLine(GlobalHelper.CreateHTMLC15(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, PR1, PR2, PR3, PR4, PR5, PR6, PR7.ToString(), PR8.ToString(), PR9, PR10, PR11, PR12, PR13, PR14, PR15, PR16, PR20.ToString(), PR21, PR22));
                                    }
                                }
                            }

                            result.Code = GlobalHelper.CreateHTML(SheetName, _WebHostEnvironment.WebRootPath, HTMLContent.ToString());
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

