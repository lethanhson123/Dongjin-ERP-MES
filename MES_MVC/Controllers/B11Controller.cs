namespace MES.Controllers
{
	public class B11Controller : Controller
	{
		private readonly IB11Service _B11Service;
		public B11Controller(IB11Service B11Service)
		{
			_B11Service = B11Service;
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
				BaseResult = await _B11Service.Buttonfind_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonadd_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonsave_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttondelete_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttoncancel_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttoninport_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonexport_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonprint_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonhelp_Click(BaseParameter);
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
				BaseResult = await _B11Service.Buttonclose_Click(BaseParameter);
			}
			catch (Exception ex)
			{
				BaseResult.Error = ex.Message;
			}
			return BaseResult;
		}
	}
}

