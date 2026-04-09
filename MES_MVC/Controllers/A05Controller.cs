namespace MES.Controllers
{
    public class A05Controller : Controller
    {
        private readonly IA05Service _A05Service;
        public A05Controller(IA05Service A05Service)
        {
            _A05Service = A05Service;
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
		BaseResult = await _A05Service.Buttonfind_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonadd_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonsave_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttondelete_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttoncancel_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttoninport_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonexport_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonprint_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonhelp_Click(BaseParameter);
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
        BaseResult = await _A05Service.Buttonclose_Click(BaseParameter);
    }
    catch (Exception ex)
    {
        BaseResult.Error = ex.Message;
    }
    return BaseResult;
}
    }
}

