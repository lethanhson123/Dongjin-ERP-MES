
namespace MES.Controllers
{
    public class B01Controller : Controller
    {
        private readonly IB01Service _B01Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B01Controller(IB01Service B01Service, IWebHostEnvironment webHostEnvironment)
        {
            _B01Service = B01Service;
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
                BaseResult = await _B01Service.Buttonfind_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttonsave_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttondelete_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttoncancel_Click(BaseParameter);
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
            try
            {

                BaseResult.ListB01 = new List<B01>();
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
                                                        string CollumName = GlobalHelper.InitializationString;
                                                        if (workSheet.Cells[1, 2].Value != null)
                                                        {
                                                            CollumName = workSheet.Cells[1, 2].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(CollumName))
                                                        {
                                                            if (CollumName == "PART NO")
                                                            {
                                                                for (int j = 2; j <= totalRows; j++)
                                                                {
                                                                    B01 B01 = new B01();
                                                                    if (workSheet.Cells[j, 2].Value != null)
                                                                    {
                                                                        B01.PARTNO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                    }
                                                                    if (!string.IsNullOrEmpty(B01.PARTNO))
                                                                    {
                                                                        B01.NO = "" + (j - 1);
                                                                        B01.dgvcheck = true;
                                                                        B01.inputdate = BaseParameter.SearchString;
                                                                        if (workSheet.Cells[j, 1].Value != null)
                                                                        {
                                                                            B01.NO = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 2].Value != null)
                                                                        {
                                                                            B01.PARTNO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 3].Value != null)
                                                                        {
                                                                            B01.UNIT = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 4].Value != null)
                                                                        {
                                                                            B01.GOODS = workSheet.Cells[j, 4].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 5].Value != null)
                                                                        {
                                                                            var QUANTITY = double.Parse(workSheet.Cells[j, 5].Value.ToString().Trim());
                                                                            var QUANTITYString = workSheet.Cells[j, 5].Value.ToString().Trim();
                                                                            QUANTITYString = QUANTITYString.Split(',')[0];
                                                                            QUANTITYString = QUANTITYString.Split('.')[0];
                                                                            var Part = double.Parse(QUANTITYString);
                                                                            var Remainder = QUANTITY - Part;
                                                                            if (Remainder >= 0.5)
                                                                            {
                                                                                Part = Part + 1;
                                                                            }
                                                                            B01.QUANTITY = (int)Part;
                                                                        }
                                                                        if (workSheet.Cells[j, 6].Value != null)
                                                                        {
                                                                            B01.NWEIGHT = workSheet.Cells[j, 6].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 7].Value != null)
                                                                        {
                                                                            B01.GWEIGHT = workSheet.Cells[j, 7].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 8].Value != null)
                                                                        {
                                                                            B01.PalletNo = workSheet.Cells[j, 8].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 9].Value != null)
                                                                        {
                                                                            B01.ShippedNo = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                                        }
                                                                        BaseResult.ListB01.Add(B01);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                BaseResult.ErrorNumber = 1;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            BaseResult.ErrorNumber = 1;
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
                BaseResult = await _B01Service.Buttonexport_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttonprint_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttonhelp_Click(BaseParameter);
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
                BaseResult = await _B01Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

