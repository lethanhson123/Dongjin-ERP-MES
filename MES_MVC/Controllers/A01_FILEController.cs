namespace MES.Controllers
{
    public class A01_FILEController : Controller
    {
        private readonly IA01_FILEService _A01_FILEService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01_FILEController(IA01_FILEService A01_FILEService, IWebHostEnvironment webHostEnvironment)
        {
            _A01_FILEService = A01_FILEService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> DGV_LOAD()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _A01_FILEService.DGV_LOAD(BaseParameter);
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
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
                                    if (fileExtension == ".pdf")
                                    {
                                        string PNO_TXT = BaseParameter.ListSearchString[0];
                                        string ECN_TXT = BaseParameter.ListSearchString[1];
                                        string ECN_DATE = BaseParameter.ListSearchString[2];
                                        string F_ECN_IDX = BaseParameter.ListSearchString[3];
                                        string FN_NAME_TXT = PNO_TXT.Trim() + "_" + ECN_TXT.Trim().Replace(".", "") + "_" + ECN_DATE.Trim().Replace(".", "").Replace("-", "");
                                        string fileName = FN_NAME_TXT + fileExtension;
                                        string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                                        Directory.CreateDirectory(physicalPath);
                                        string filePath = Path.Combine(physicalPath, fileName);
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            file.CopyTo(stream);
                                        }
                                        FileInfo fileLocation = new FileInfo(filePath);

                                        BaseParameter.ListSearchString = new List<string>();
                                        BaseParameter.ListSearchString.Add(FN_NAME_TXT);
                                        string FOLD = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + controllerName + "/" + fileName;
                                        BaseParameter.ListSearchString.Add(FOLD);
                                        BaseParameter.ListSearchString.Add(F_ECN_IDX);
                                        BaseResult = await _A01_FILEService.Buttonsave_Click(BaseParameter);

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
            }
            return BaseResult;
        }
    }
}

