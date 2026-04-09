namespace MES.Controllers
{
    public class A01_PNADDController : Controller
    {
        private readonly IA01_PNADDService _A01_PNADDService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01_PNADDController(IA01_PNADDService A01_PNADDService, IWebHostEnvironment webHostEnvironment)
        {
            _A01_PNADDService = A01_PNADDService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _A01_PNADDService.Buttonsave_Click(BaseParameter);
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
                                                        string PART_NO = GlobalHelper.InitializationString;
                                                        if (workSheet.Cells[1, 2].Value != null)
                                                        {
                                                            PART_NO = workSheet.Cells[1, 2].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(PART_NO))
                                                        {
                                                            if (PART_NO == "PART NO")
                                                            {
                                                                for (int j = 3; j <= totalRows; j++)
                                                                {
                                                                    SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();                                                                   
                                                                    if (workSheet.Cells[j, 2].Value != null)
                                                                    {
                                                                        SuperResultTranfer.PART_NO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                    }
                                                                    if (!string.IsNullOrEmpty(SuperResultTranfer.PART_NO))
                                                                    {                                                                      
                                                                        if (workSheet.Cells[j, 1].Value != null)
                                                                        {
                                                                            SuperResultTranfer.NO = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 2].Value != null)
                                                                        {
                                                                            SuperResultTranfer.PART_NO = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 3].Value != null)
                                                                        {
                                                                            SuperResultTranfer.PART_NAME = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 4].Value != null)
                                                                        {
                                                                            SuperResultTranfer.BOM_GROUP = workSheet.Cells[j, 4].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 5].Value != null)
                                                                        {
                                                                            SuperResultTranfer.MODEL = workSheet.Cells[j, 5].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 6].Value != null)
                                                                        {
                                                                            SuperResultTranfer.PART_FamilyPC = workSheet.Cells[j, 6].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 7].Value != null)
                                                                        {
                                                                            SuperResultTranfer.Packing_Unit = workSheet.Cells[j, 7].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 8].Value != null)
                                                                        {
                                                                            SuperResultTranfer.Item_TypeE = workSheet.Cells[j, 8].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 9].Value != null)
                                                                        {
                                                                            SuperResultTranfer.Location = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                                        }
                                                                        if (workSheet.Cells[j, 9].Value != null)
                                                                        {
                                                                            SuperResultTranfer.PART_SUPL = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                                        }
                                                                        BaseResult.DataGridView1.Add(SuperResultTranfer);
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
    }
}

