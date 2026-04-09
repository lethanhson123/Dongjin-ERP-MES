namespace MES.Controllers
{
    public class Z02Controller : Controller
    {
        private readonly IZ02Service _Z02Service;

        public Z02Controller(IZ02Service Z02Service)
        {
            _Z02Service = Z02Service;
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
                BaseParameter BaseParameter = new BaseParameter
                {
                    USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty
                };

                BaseResult = await _Z02Service.Load();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonfind_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonadd_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
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
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
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
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttoncancel_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttoninport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonexport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonhelp_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseParameter.USER_IDX = HttpContext.Session.GetString("USER_IDX") ?? string.Empty;

                BaseResult = await _Z02Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.Success = false;
            }
            return BaseResult;
        }
    }
}