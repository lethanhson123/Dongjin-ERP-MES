namespace MES.Controllers
{
    public class C06Controller : Controller
    {
        private readonly IC06Service _C06Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C06Controller(IC06Service C06Service, IWebHostEnvironment webHostEnvironment)
        {
            _C06Service = C06Service;
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
                BaseResult = await _C06Service.Buttonfind_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttonsave_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttondelete_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttoncancel_Click(BaseParameter);
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
            try
            {
                BaseResult.DataGridView1 = new List<SuperResultTranfer>();
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                            string fileExtension = Path.GetExtension(file.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                            {
                                string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                                FileInfo fileLocation = new FileInfo(filePath);
                                try
                                {
                                    if (fileLocation.Length > 0)
                                    {
                                        if (fileExtension == ".xlsx" || fileExtension == ".xls")
                                        {
                                            using (ExcelPackage package = new ExcelPackage(fileLocation))
                                            {
                                                if (package.Workbook.Worksheets.Count > 0)
                                                {
                                                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                    if (workSheet != null)
                                                    {
                                                        int totalRows = workSheet.Dimension.Rows;
                                                        int collumnRowIndex = 0;
                                                        bool IsDate = true;
                                                        for (int j = 2; j <= totalRows; j++)
                                                        {
                                                            SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();
                                                            string HeaderText = "";
                                                            if (workSheet.Cells[j - 1, 1].Value != null)
                                                            {
                                                                HeaderText = workSheet.Cells[j - 1, 1].Value.ToString().Trim();
                                                            }
                                                            if (HeaderText.Length > 0)
                                                            {
                                                                collumnRowIndex = 0;
                                                                IsDate = true;
                                                            }
                                                            else
                                                            {
                                                                collumnRowIndex = 1;
                                                                IsDate = false;
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 1].Value != null)
                                                            {
                                                                SuperResultTranfer.ASSY_NO = workSheet.Cells[j, collumnRowIndex + 1].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 2].Value != null)
                                                            {
                                                                SuperResultTranfer.LEAD_NO = workSheet.Cells[j, collumnRowIndex + 2].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 3].Value != null)
                                                            {
                                                                SuperResultTranfer.ORDER_QTY = workSheet.Cells[j, collumnRowIndex + 3].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 4].Value != null)
                                                            {
                                                                SuperResultTranfer.SAFETY_QTY = workSheet.Cells[j, collumnRowIndex + 4].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 5].Value != null)
                                                            {
                                                                SuperResultTranfer.Machine = workSheet.Cells[j, collumnRowIndex + 5].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, collumnRowIndex + 6].Value != null)
                                                            {
                                                                SuperResultTranfer.BUNDLE_SIZE = int.Parse(workSheet.Cells[j, collumnRowIndex + 6].Value.ToString().Trim());
                                                            }
                                                            if (IsDate == true)
                                                            {
                                                                if (workSheet.Cells[j, collumnRowIndex + 7].Value != null)
                                                                {
                                                                    SuperResultTranfer.DATE = DateTime.Parse(workSheet.Cells[j, collumnRowIndex + 7].Value.ToString().Trim());
                                                                }
                                                                if (workSheet.Cells[j, collumnRowIndex + 8].Value != null)
                                                                {
                                                                    SuperResultTranfer.REP = workSheet.Cells[j, collumnRowIndex + 8].Value.ToString().Trim();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (workSheet.Cells[j, collumnRowIndex + 7].Value != null)
                                                                {
                                                                    SuperResultTranfer.REP = workSheet.Cells[j, collumnRowIndex + 7].Value.ToString().Trim();
                                                                }
                                                            }
                                                            try
                                                            {
                                                                SuperResultTranfer.CHK = true;
                                                                if (int.Parse(SuperResultTranfer.ORDER_QTY) <= 0)
                                                                {
                                                                    SuperResultTranfer.CHK = false;
                                                                }
                                                                if (SuperResultTranfer.LEAD_NO.Length <= 5)
                                                                {
                                                                    SuperResultTranfer.CHK = false;
                                                                }
                                                                BaseResult.DataGridView1.Add(SuperResultTranfer);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                BaseResult.Error = ex.Message;
                                                                BaseResult.ErrorNumber = 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    BaseResult.Error = ex.Message;
                                    BaseResult.ErrorNumber = 1;
                                }
                                fileLocation.Delete();
                            }
                            else
                            {
                                BaseResult.ErrorNumber = 1;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _C06Service.Buttonexport_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttonprint_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttonhelp_Click(BaseParameter);
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
                BaseResult = await _C06Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COM_LIST()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C06Service.COM_LIST(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COM_LIST2()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C06Service.COM_LIST2(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

