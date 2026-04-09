namespace MESService.Implement
{
    public class B08_REPRINTService : BaseService<torderlist, ItorderlistRepository>
    , IB08_REPRINTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B08_REPRINTService(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string Label4 = BaseParameter.SearchString;

                    string sql = @"SELECT TTC_BARCODE.TTC_BARCODENM, TTC_BARCODE.Barcode_SEQ, TTC_PART.TC_PART_NM, TTC_PART.TC_DESC, SUBSTRING_INDEX(SUBSTRING_INDEX(TTC_BARCODE.`TTC_BARCODENM`, '$$', 2), '$$', -1) AS `QTY`, TTC_PART.TC_LOC, TTC_ORDER.TTC_PO_DT

                    FROM TTC_ORDER, TTC_BARCODE, TTC_PART WHERE TTC_BARCODE.TTC_ORDER_IDX = TTC_ORDER.TTC_PO_INX AND TTC_ORDER.TTC_PN_IDX = TTC_PART.TTC_PART_IDX AND TTC_ORDER.TTC_PO_INX ='" + Label4 + "'";

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
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.SuperResultTranfer != null)
                    {
                        StringBuilder SearchString = new StringBuilder();
                        string SheetName = this.GetType().Name;
                        string contentHTML = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "B08_REPRINT.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                contentHTML = r.ReadToEnd();
                            }
                        }
                        QRCodeModel QRCodeModel = GlobalHelper.CreateQRCode(BaseParameter.SuperResultTranfer.TTC_BARCODENM, SheetName, _WebHostEnvironment.WebRootPath);
                        string QRCodeFileName = QRCodeModel.URL;

                        contentHTML = contentHTML.Replace(@"[TC_PART_NM]", BaseParameter.SuperResultTranfer.TC_PART_NM);
                        contentHTML = contentHTML.Replace(@"[TC_DESC]", BaseParameter.SuperResultTranfer.TC_DESC);
                        contentHTML = contentHTML.Replace(@"[QTY]", BaseParameter.SuperResultTranfer.QTY.ToString());
                        contentHTML = contentHTML.Replace(@"[Barcode_SEQ]", BaseParameter.SuperResultTranfer.Barcode_SEQ.ToString());
                        contentHTML = contentHTML.Replace(@"[TC_LOC]", BaseParameter.SuperResultTranfer.TC_LOC);
                        contentHTML = contentHTML.Replace(@"[TTC_PO_DT]", BaseParameter.SuperResultTranfer.TTC_PO_DT.Value.ToString("yyyy-MM-dd"));                        
                        contentHTML = contentHTML.Replace(@"[TTC_BARCODENM]", BaseParameter.SuperResultTranfer.TTC_BARCODENM);                        
                        contentHTML = contentHTML.Replace(@"[QRCodeFileName]", QRCodeFileName);
                        
                        int TTC_PO_DT2 = System.Globalization.ISOWeek.GetWeekOfYear(BaseParameter.SuperResultTranfer.TTC_PO_DT.Value);
                        contentHTML = contentHTML.Replace(@"[TTC_PO_DT2]", TTC_PO_DT2.ToString());

                        string fileName = "B08_REPRINT_" + GlobalHelper.InitializationDateTimeCode + ".html";
                        string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        string filePath = Path.Combine(physicalPathCreate, fileName);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                await w.WriteLineAsync(contentHTML);
                            }
                        }
                        result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
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

