namespace MES.Controllers
{
    public class Z04_1Controller : Controller
    {
        private readonly IZ04_1Service _Z04_1Service;
        public Z04_1Controller(IZ04_1Service Z04_1Service)
        {
            _Z04_1Service = Z04_1Service;
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
                BaseResult = await _Z04_1Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }       
    }
}

