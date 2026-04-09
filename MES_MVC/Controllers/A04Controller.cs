namespace MES.Controllers
{
    public class A04Controller : Controller
    {
        private readonly IA04Service _A04Service;

        public A04Controller(IA04Service A04Service)
        {
            _A04Service = A04Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Load()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                baseResult = await _A04Service.Load();
            }
            catch (Exception ex)
            {
                baseResult.Error = $"Error: {ex.Message}";
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> LoadToolHistory()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.LoadToolHistory(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonfind_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonfind_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonadd_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonadd_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonsave_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonsave_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }
       
        [HttpPost]
        public async Task<IActionResult> Buttondelete_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttondelete_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttoncancel_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttoncancel_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttoninport_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttoninport_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonexport_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonexport_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonprint_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonprint_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonhelp_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonhelp_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Buttonclose_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Buttonclose_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Button1_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Button1_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Button2_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Button2_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }

        [HttpPost]
        public async Task<IActionResult> Button3_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _A04Service.Button3_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return Json(baseResult);
        }
    }
}