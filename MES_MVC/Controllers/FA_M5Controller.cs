namespace MES.Controllers
{
    public class FA_M5Controller : Controller
    {
        private readonly IFA_M5Service _service;
        public FA_M5Controller(IFA_M5Service service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Load()
        {
            var result = new BaseResult();
            try
            {
                result = await _service.Load();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> ScanIn_Click()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.ScanIn_Click(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> ScanOut_Click()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.ScanOut_Click(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> GetStaffList()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.GetStaffList(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> StartDowntime()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.StartDowntime(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> EndDowntime()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.EndDowntime(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.Buttonexport_Click(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        [HttpPost]
        public async Task<BaseResult> EditShift()
        {
            var result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _service.EditShift(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}
