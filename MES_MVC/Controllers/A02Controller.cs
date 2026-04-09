namespace MES.Controllers
{
    public class A02Controller : Controller
    {
        private readonly IA02Service _A02Service;
        public A02Controller(IA02Service A02Service)
        {
            _A02Service = A02Service;
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
		BaseResult = await _A02Service.Buttonfind_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonadd_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonsave_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttondelete_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttoncancel_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttoninport_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonexport_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonprint_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonhelp_Click(BaseParameter);
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
        BaseResult = await _A02Service.Buttonclose_Click(BaseParameter);
    }
    catch (Exception ex)
    {
        BaseResult.Error = ex.Message;
    }
    return BaseResult;
}
    }
}

