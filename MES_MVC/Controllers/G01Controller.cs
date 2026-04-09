namespace MES.Controllers
{
    public class G01Controller : Controller
    {
        private readonly IG01Service _G01Service;

        public G01Controller(IG01Service G01Service)
        {
            _G01Service = G01Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonfind_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonadd_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonsave_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttondelete_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttoncancel_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttoninport_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonexport_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonprint_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonhelp_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Buttonclose_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Button1_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Button1_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Button2_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _G01Service.Button2_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }
    }
}