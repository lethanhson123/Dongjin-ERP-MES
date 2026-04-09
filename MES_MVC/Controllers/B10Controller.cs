using MES_MVC;

namespace MES.Controllers
{
    public class B10Controller : Controller
    {
        private readonly IB10Service _B10Service;
        private readonly CookieHelper _cookieHelper;
        public B10Controller(IB10Service B10Service,CookieHelper CookieHelper)
        {
            _B10Service = B10Service;
            _cookieHelper = CookieHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.Buttonfind_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttonsave_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttondelete_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttoncancel_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttoninport_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttonexport_Click(BaseParameter);
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
                BaseParameter.USER_ID= HttpContext.Session.GetString(GlobalHelper.UserID);
                BaseParameter.USER_NM = HttpContext.Session.GetString(GlobalHelper.USER_NM);
                BaseResult = await _B10Service.Buttonprint_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttonhelp_Click(BaseParameter);
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
                BaseResult = await _B10Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> CB_FCTRY_LINE()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.CB_FCTRY_LINE(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COMLIST_LINE_1()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.COMLIST_LINE_1(BaseParameter);

                if (BaseResult != null)
                {
                    if (BaseResult.T2_S1 != null)
                    {
                        string Language = _cookieHelper.GetCookie(GlobalHelper.Language) ?? "EN";

                        for (int i = 0; i < BaseResult.T2_S1.Count; i++)
                        {
                            BaseResult.T2_S1[i].CD_SYS_NOTE = BaseResult.T2_S1[i].CD_NM_EN;
                            if (Language == GlobalHelper.LanguageCodeKR)
                            {
                                BaseResult.T2_S1[i].CD_SYS_NOTE = BaseResult.T2_S1[i].CD_NM_HAN;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COMLIST_LINE_2()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.COMLIST_LINE_2(BaseParameter);

                if (BaseResult != null)
                {
                    if (BaseResult.ComboBox2 != null)
                    {
                        string Language = _cookieHelper.GetCookie(GlobalHelper.Language) ?? "EN"; 

                        for (int i = 0; i < BaseResult.ComboBox2.Count; i++)
                        {
                            BaseResult.ComboBox2[i].CD_SYS_NOTE = BaseResult.ComboBox2[i].CD_NM_EN;
                            if (Language == GlobalHelper.LanguageCodeKR)
                            {
                                BaseResult.ComboBox2[i].CD_SYS_NOTE = BaseResult.ComboBox2[i].CD_NM_HAN;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COMLIST_LINE_3()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.COMLIST_LINE_3(BaseParameter);

                if (BaseResult != null)
                {
                    if (BaseResult.T3_S1 != null)
                    {
                        string Language = _cookieHelper.GetCookie(GlobalHelper.Language) ?? "EN"; 

                        for (int i = 0; i < BaseResult.T3_S1.Count; i++)
                        {
                            BaseResult.T3_S1[i].CD_SYS_NOTE = BaseResult.T3_S1[i].CD_NM_EN;
                            if (Language == GlobalHelper.LanguageCodeKR)
                            {
                                BaseResult.T3_S1[i].CD_SYS_NOTE = BaseResult.T3_S1[i].CD_NM_HAN;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> DataT2DGV_LOAD()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B10Service.DataT2DGV_LOAD(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

