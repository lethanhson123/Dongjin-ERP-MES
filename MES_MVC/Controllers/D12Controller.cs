namespace MES.Controllers
{
    public class D12Controller : Controller
    {
        private readonly ID12Service _D12Service;
        public D12Controller(ID12Service D12Service)
        {
            _D12Service = D12Service;
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
		BaseResult = await _D12Service.Buttonfind_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonadd_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonsave_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttondelete_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttoncancel_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttoninport_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonexport_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonprint_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonhelp_Click(BaseParameter);
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
        BaseResult = await _D12Service.Buttonclose_Click(BaseParameter);
    }
    catch (Exception ex)
    {
        BaseResult.Error = ex.Message;
    }
    return BaseResult;
}
    }
}

