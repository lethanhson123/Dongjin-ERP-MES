namespace MESService.Implement
{
    public class D11Service : BaseService<torderlist, ItorderlistRepository>
    , ID11Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public D11Service(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> FIND_NO(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT tdpdotpl_LABEL.LABEL_NO, tdpdotpl_LABEL.LABEL_TXT FROM tdpdotpl_LABEL  ORDER BY tdpdotpl_LABEL.LABEL_NO DESC LIMIT 1 ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DGV_D11_NO = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DGV_D11_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string SheetName = this.GetType().Name;
                    var M_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var ADD_NO = int.Parse(BaseParameter.ListSearchString[0]);
                        var LBN03 = int.Parse(BaseParameter.ListSearchString[1]);
                        var LBN02 = int.Parse(BaseParameter.ListSearchString[2]);

                        var II = 0;
                        var JJ = LBN03 - LBN02;
                        var SUM_VAL = "";
                        var VALSE = "";
                        var SN = LBN02;
                        StringBuilder HTMLContent = new StringBuilder();
                        for (II = 0; II <= JJ; II++)
                        {
                            var SNII = SN + II;

                            VALSE = "(DATE_FORMAT(NOW(), '%Y-%m-%d'), '" + SNII + "', 'PALL" + SNII.ToString().PadLeft(6, '0') + "', NOW(), '" + M_ID + "')";
                            if (SUM_VAL.Length > 0)
                            {
                                SUM_VAL = SUM_VAL + ", " + VALSE;
                            }
                            else
                            {
                                SUM_VAL = VALSE;
                            }

                            var QRCode = SNII.ToString().PadLeft(6, '0');
                            var Barcode = "PALL" + QRCode;

                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD11(_WebHostEnvironment.WebRootPath, QRCode, Barcode));

                        }

                        string sql = @"INSERT INTO `tdpdotpl_LABEL` (`PDLB_DATE`, `LABEL_NO`, `LABEL_TXT`, `CREATE_DTM`, `CREATE_USER`) VALUES   " + SUM_VAL;
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


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
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

    }
}

