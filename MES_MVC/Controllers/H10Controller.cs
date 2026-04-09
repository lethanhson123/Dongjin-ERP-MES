namespace MES.Controllers
{
    public class H10Controller : Controller
    {
        private readonly IH10Service _H10Service;
        public H10Controller(IH10Service H10Service)
        {
            _H10Service = H10Service;
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
		BaseResult = await _H10Service.Buttonfind_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonadd_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonsave_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttondelete_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttoncancel_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttoninport_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonexport_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonprint_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonhelp_Click(BaseParameter);
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
        BaseResult = await _H10Service.Buttonclose_Click(BaseParameter);
    }
    catch (Exception ex)
    {
        BaseResult.Error = ex.Message;
    }
    return BaseResult;
}
    }
}

