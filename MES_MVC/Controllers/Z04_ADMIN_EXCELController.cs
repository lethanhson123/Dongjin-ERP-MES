namespace MES.Controllers
{
    public class Z04_ADMIN_EXCELController : Controller
    {
        private readonly IZ04_ADMIN_EXCELService _Z04_ADMIN_EXCELService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public Z04_ADMIN_EXCELController(IZ04_ADMIN_EXCELService Z04_ADMIN_EXCELService, IWebHostEnvironment webHostEnvironment)
        {
            _Z04_ADMIN_EXCELService = Z04_ADMIN_EXCELService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            BaseResult.ErrorNumber = GlobalHelper.InitializationNumber;
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

                                                        for (int j = 2; j <= totalRows; j++)
                                                        {
                                                            SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();

                                                            if (workSheet.Cells[j, 1].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_MESNO = int.Parse(workSheet.Cells[j, 1].Value.ToString().Trim());
                                                            }
                                                            if (workSheet.Cells[j, 2].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_DEPART = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 3].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_PKILOC = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 4].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_INPUTER = workSheet.Cells[j, 4].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 5].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_SERIAL_NO1 = int.Parse(workSheet.Cells[j, 5].Value.ToString().Trim());
                                                            }
                                                            if (workSheet.Cells[j, 6].Value != null)
                                                            {
                                                                SuperResultTranfer.TSYEAR_SERIAL_NO2 = int.Parse(workSheet.Cells[j, 6].Value.ToString().Trim());
                                                            }
                                                            BaseResult.DataGridView1.Add(SuperResultTranfer);
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
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _Z04_ADMIN_EXCELService.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> MES_CDD()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _Z04_ADMIN_EXCELService.MES_CDD(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

