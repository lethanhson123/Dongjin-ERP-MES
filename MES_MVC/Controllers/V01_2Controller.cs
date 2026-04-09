namespace MES.Controllers
{
    public class V01_2Controller : Controller
    {
        private readonly IV01_2Service _V01_2Service;

        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_2Controller(IV01_2Service V01_2Service, IWebHostEnvironment webHostEnvironment)
        {
            _V01_2Service = V01_2Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Button1_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _V01_2Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

