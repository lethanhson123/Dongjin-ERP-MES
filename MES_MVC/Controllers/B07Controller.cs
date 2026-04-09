namespace MES.Controllers
{
    public class B07Controller : Controller
    {
        private readonly IB07Service _B07Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B07Controller(IB07Service B07Service, IWebHostEnvironment webHostEnvironment)
        {
            _B07Service = B07Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonfind_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonadd_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttoncancel_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            BaseResult.ErrorNumber = GlobalHelper.InitializationNumber;
            BaseResult.DataGridView1 = new List<SuperResultTranfer>();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                //if (BaseParameter != null)
                //{
                //    if (BaseParameter.Action == 1)
                //    {
                //        if (Request.Form.Files.Count > 0)
                //        {
                //            for (int i = 0; i < Request.Form.Files.Count; i++)
                //            {
                //                var file = Request.Form.Files[i];
                //                if (file == null || file.Length == 0)
                //                {
                //                }
                //                if (file != null)
                //                {
                //                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                //                    string fileExtension = Path.GetExtension(file.FileName);
                //                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                //                    {
                //                        string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                //                        string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                //                        Directory.CreateDirectory(physicalPath);
                //                        string filePath = Path.Combine(physicalPath, fileName);
                //                        using (var stream = new FileStream(filePath, FileMode.Create))
                //                        {
                //                            file.CopyTo(stream);
                //                        }
                //                        FileInfo fileLocation = new FileInfo(filePath);
                //                        try
                //                        {
                //                            if (fileLocation.Length > 0)
                //                            {
                //                                if (fileExtension == ".xlsx" || fileExtension == ".xls")
                //                                {
                //                                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                //                                    {
                //                                        if (package.Workbook.Worksheets.Count > 0)
                //                                        {
                //                                            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                //                                            if (workSheet != null)
                //                                            {
                //                                                int totalRows = workSheet.Dimension.Rows;
                //                                                for (int j = 2; j <= totalRows; j++)
                //                                                {
                //                                                    try
                //                                                    {
                //                                                        var item = new SuperResultTranfer();
                //                                                        int CUT_ORDER = GlobalHelper.InitializationNumber;
                //                                                        if (workSheet.Cells[j, 3].Value != null)
                //                                                        {
                //                                                            CUT_ORDER = int.Parse(workSheet.Cells[j, 3].Value.ToString().Trim());
                //                                                        }
                //                                                        if (CUT_ORDER > 0)
                //                                                        {
                //                                                            item.CHK = true;
                //                                                            if (workSheet.Cells[j, 1].Value != null)
                //                                                            {
                //                                                                item.DATEString = workSheet.Cells[j, 1].Value.ToString().Trim();
                //                                                            }
                //                                                            if (workSheet.Cells[j, 2].Value != null)
                //                                                            {
                //                                                                item.Tube_Cutting_Part_No = workSheet.Cells[j, 2].Value.ToString().Trim();
                //                                                            }
                //                                                            if (workSheet.Cells[j, 3].Value != null)
                //                                                            {
                //                                                                item.CUT_ORDER = workSheet.Cells[j, 3].Value.ToString().Trim();
                //                                                            }
                //                                                            BaseResult.DataGridView1.Add(item);
                //                                                        }
                //                                                    }
                //                                                    catch (Exception ex)
                //                                                    {
                //                                                        BaseResult.Error = ex.Message;
                //                                                        BaseResult.ErrorNumber = 1;
                //                                                    }
                //                                                }
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                        catch (Exception ex)
                //                        {
                //                            BaseResult.Error = ex.Message;
                //                            BaseResult.ErrorNumber = 1;
                //                        }
                //                        fileLocation.Delete();
                //                    }
                //                    else
                //                    {
                //                        BaseResult.ErrorNumber = 1;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.ErrorNumber = 2;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonexport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonhelp_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> DGV_BOM_LD()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.DGV_BOM_LD(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Button1_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

