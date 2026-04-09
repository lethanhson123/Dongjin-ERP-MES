namespace MES.Controllers
{
    public class E03Controller : Controller
    {
        private readonly IE03Service _toolShopService;

        public E03Controller(IE03Service toolShopService)
        {
            _toolShopService = toolShopService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<BaseResult> Load()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                baseResult = await _toolShopService.Load();
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _toolShopService.Buttonfind_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonadd_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonsave_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttondelete_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }

        // ============================================
        // NEW ENDPOINT: Upload Image
        // ============================================
        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var toolShopId = int.Parse(Request.Form["toolShopId"]);

                var result = await _toolShopService.UploadImage(file, toolShopId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new BaseResult { Error = ex.Message });
            }
        }

        // ============================================
        // NEW ENDPOINT: Delete Image
        // ============================================
        [HttpPost]
        public async Task<IActionResult> DeleteImage()
        {
            try
            {
                var toolShopId = int.Parse(Request.Form["toolShopId"]);
                var result = await _toolShopService.DeleteImage(toolShopId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new BaseResult { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult baseResult = new BaseResult();
            try
            {
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                baseResult = await _toolShopService.Buttoncancel_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttoninport_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonexport_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonprint_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonhelp_Click(baseParameter);
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
                baseResult = await _toolShopService.Buttonclose_Click(baseParameter);
            }
            catch (Exception ex)
            {
                baseResult.Error = ex.Message;
            }
            return baseResult;
        }
    }
}