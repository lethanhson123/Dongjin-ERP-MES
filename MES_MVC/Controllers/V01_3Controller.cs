namespace MES.Controllers
{
    public class V01_3Controller : Controller
    {
        private readonly IV01_3Service _V01_3Service;

        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_3Controller(IV01_3Service V01_3Service, IWebHostEnvironment webHostEnvironment)
        {
            _V01_3Service = V01_3Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseResult = await _V01_3Service.Load();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> ComboBox1_SelectedIndexChanged()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _V01_3Service.ComboBox1_SelectedIndexChanged(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

