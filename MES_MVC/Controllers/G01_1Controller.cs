namespace MES.Controllers
{
    public class G01_1Controller : Controller
    {
        private readonly IG01_1Service _G01_1Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public G01_1Controller(IG01_1Service G01_1Service, IWebHostEnvironment webHostEnvironment)
        {
            _G01_1Service = G01_1Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01_1Service.Buttonsave_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
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
                if (BaseParameter != null)
                {
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
                                    string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode + fileExtension;
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
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                try
                                                                {
                                                                    var item = new SuperResultTranfer();
                                                                    string PART_NO = GlobalHelper.InitializationString;
                                                                    if (workSheet.Cells[j, 2].Value != null)
                                                                    {
                                                                        PART_NO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                    }
                                                                    if (!string.IsNullOrEmpty(PART_NO))
                                                                    {
                                                                        item.Description = "ERROR";
                                                                        if (workSheet.Cells[j, 1].Value != null)
                                                                        {
                                                                            item.GROUP = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 2].Value != null)
                                                                        {
                                                                            item.PART_NO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 3].Value != null)
                                                                        {
                                                                            item.QTY = double.Parse(workSheet.Cells[j, 3].Value.ToString().Trim());
                                                                        }
                                                                        BaseResult.DataGridView1.Add(item);
                                                                    }
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
                    BaseParameter.DataGridView1 = BaseResult.DataGridView1;
                    await _G01_1Service.Buttoninport_Click(BaseParameter);
                }
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.ErrorNumber = 2;
            }
            return BaseResult;
        }
    }
}

