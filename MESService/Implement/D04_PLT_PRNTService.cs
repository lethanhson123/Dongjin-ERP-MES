namespace MESService.Implement
{
    public class D04_PLT_PRNTService : BaseService<torderlist, ItorderlistRepository>
    , ID04_PLT_PRNTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public D04_PLT_PRNTService(ItorderlistRepository torderlistRepository
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

        public virtual async Task<BaseResult> D04_PLT_PRNT_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                string sql = @"SELECT tdpdotpl.PO_CODE, tdpdotpl.CREATE_DTM
                FROM tdpdotpl
                GROUP BY tdpdotpl.PO_CODE
                ORDER BY  tdpdotpl.PDOTPL_IDX DESC
                LIMIT 8";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var NumericUpDown1 = int.Parse(BaseParameter.ListSearchString[0]);
                        var MaskedTextBox1 = BaseParameter.ListSearchString[1];
                        var MaskedTextBox2 = BaseParameter.ListSearchString[2];
                        var MaskedTextBox2Number = int.Parse(MaskedTextBox2);
                        var II = 0;
                        var JJ = NumericUpDown1 * 2;
                        var TXN = 0;
                        var Num = "";
                        for (TXN = MaskedTextBox1.Length - 1; TXN >= 0; TXN--)
                        {
                            try
                            {
                                var Temp = MaskedTextBox1.Substring(TXN, 1);
                                Num = int.Parse(Temp) + Num;
                            }
                            catch (Exception ex)
                            {
                                string Message = ex.Message;
                            }
                        }
                        var AAA = MaskedTextBox1;
                        var ADD_PLUS = 0;
                        StringBuilder HTMLContent = new StringBuilder();
                        for (II = 0; II < JJ; II++)
                        {
                            var Temp = (MaskedTextBox2Number + ADD_PLUS).ToString().PadLeft(3, '0');
                            var BBB = "DJG-" + Num + "-" + Temp;
                            ADD_PLUS = ADD_PLUS + 1;
                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04_PLT_PRNT(SheetName, _WebHostEnvironment.WebRootPath, AAA, BBB));
                            //var CCC = "DJG-" + Num + "-" + Temp;
                            //ADD_PLUS = ADD_PLUS + 1;
                        }
                        if (!string.IsNullOrEmpty(HTMLContent.ToString()))
                        {
                            string contentHTML = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    contentHTML = r.ReadToEnd();
                                }
                            }
                            contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                            string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
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
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

