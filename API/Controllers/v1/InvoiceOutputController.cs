namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceOutputController : BaseController<InvoiceOutput, IInvoiceOutputService>
    {
        private readonly IInvoiceOutputService _InvoiceOutputService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public InvoiceOutputController(IInvoiceOutputService InvoiceOutputService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceOutputService, WebHostEnvironment)
        {
            _InvoiceOutputService = InvoiceOutputService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<InvoiceOutput>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<InvoiceOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceOutput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceOutputService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_ActiveToListAsync")]
        public virtual async Task<BaseResult<InvoiceOutput>> GetByMembershipID_ActiveToListAsync()
        {
            var result = new BaseResult<InvoiceOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceOutput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceOutputService.GetByMembershipID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

