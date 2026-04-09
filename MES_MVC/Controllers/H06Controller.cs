namespace MES.Controllers
{
    public class H06Controller : Controller
    {
        private readonly IH06Service _H06Service;

        public H06Controller(IH06Service H06Service)
        {
            _H06Service = H06Service;
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
                BaseResult = await _H06Service.Load();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click([FromForm] string BaseParameter)
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter parameter = JsonConvert.DeserializeObject<BaseParameter>(BaseParameter);
                BaseResult = await _H06Service.Buttonfind_Click(parameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click([FromForm] string BaseParameter)
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter parameter = JsonConvert.DeserializeObject<BaseParameter>(BaseParameter);
                BaseResult = await _H06Service.Buttonsave_Click(parameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Dgv_reload()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseResult = await _H06Service.Dgv_reload();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> DB_STOP()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseResult = await _H06Service.DB_STOP();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> MC_STOP_RUN([FromForm] string BaseParameter)
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter parameter = JsonConvert.DeserializeObject<BaseParameter>(BaseParameter);
                BaseResult = await _H06Service.MC_STOP_RUN(parameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> MC_STOPN([FromForm] string BaseParameter)
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter parameter = JsonConvert.DeserializeObject<BaseParameter>(BaseParameter);
                BaseResult = await _H06Service.MC_STOPN(parameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> GetMonthlyChart([FromForm] string BaseParameter)
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter parameter = JsonConvert.DeserializeObject<BaseParameter>(BaseParameter);
                BaseResult = await _H06Service.GetMonthlyChart(parameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}