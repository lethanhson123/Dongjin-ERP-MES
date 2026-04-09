namespace MES.Controllers
{
    public class H12Controller : Controller
    {
        private readonly IH12Service _H12Service;
        public H12Controller(IH12Service H12Service)
        {
            _H12Service = H12Service;
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
		BaseResult = await _H12Service.Buttonfind_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonadd_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonsave_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttondelete_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttoncancel_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttoninport_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonexport_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonprint_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonhelp_Click(BaseParameter);
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
        BaseResult = await _H12Service.Buttonclose_Click(BaseParameter);
    }
    catch (Exception ex)
    {
        BaseResult.Error = ex.Message;
    }
    return BaseResult;
}
    }
}

