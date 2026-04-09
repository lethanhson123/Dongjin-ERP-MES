namespace MES.Controllers
{
    public class D04_RANKController : Controller
    {
        private readonly ID04_RANKService _D04_RANKService;
        public D04_RANKController(ID04_RANKService D04_RANKService)
        {
            _D04_RANKService = D04_RANKService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> SELECT_SAVE()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D04_RANKService.SELECT_SAVE(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

