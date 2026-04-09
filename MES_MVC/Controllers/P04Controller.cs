namespace MES.Controllers
{
    public class P04Controller : Controller
    {
        private readonly IP04Service _P04Service;

        public P04Controller(IP04Service P04Service)
        {
            _P04Service = P04Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Load()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                baseResult = await _P04Service.Load();
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _P04Service.Buttonfind_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _P04Service.Buttonsave_Click(BaseParameter);
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
                BaseResult = await _P04Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttoncancel_Click(BaseParameter);
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
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _P04Service.Buttoninport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttonexport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttonhelp_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
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
                BaseResult = await _P04Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}