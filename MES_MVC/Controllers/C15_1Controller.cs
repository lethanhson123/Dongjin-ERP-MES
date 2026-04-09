namespace MES.Controllers
{
    public class C15_1Controller : Controller
    {
        private readonly IC15_1Service _C15_1Service;
        public C15_1Controller(IC15_1Service C15_1Service)
        {
            _C15_1Service = C15_1Service;
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
                BaseResult = await _C15_1Service.Button1_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
       
    }
}

