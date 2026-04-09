namespace MES.Controllers
{
    public class V01_4Controller : Controller
    {
        private readonly IV01_4Service _V01_4Service;

        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_4Controller(IV01_4Service V01_4Service, IWebHostEnvironment webHostEnvironment)
        {
            _V01_4Service = V01_4Service;
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
                BaseResult = await _V01_4Service.Buttonfind_Click(BaseParameter);
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
                                    string fileExtension = ".jpg";
                                    string Label3 = BaseParameter.ListSearchString[0];
                                    string fileName = Label3 + fileExtension;
                                    string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                                    Directory.CreateDirectory(physicalPath);
                                    string filePath = Path.Combine(physicalPath, fileName);
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }
                                    FileInfo fileLocation = new FileInfo(filePath);
                                    BaseParameter.FilePath = filePath;
                                    BaseParameter.PhysicalPath = physicalPath;
                                    BaseResult = await _V01_4Service.Buttonsave_Click(BaseParameter);

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
        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _V01_4Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

