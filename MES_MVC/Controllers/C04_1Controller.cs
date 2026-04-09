namespace MES.Controllers
{
    public class C04_1Controller : Controller
    {
        private readonly IC04_1Service _C04_1Service;
        public C04_1Controller(IC04_1Service C04_1Service)
        {
            _C04_1Service = C04_1Service;
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
                BaseResult = await _C04_1Service.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

