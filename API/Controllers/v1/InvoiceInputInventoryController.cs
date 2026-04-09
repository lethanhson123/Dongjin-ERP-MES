namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInputInventoryController : BaseController<InvoiceInputInventory, IInvoiceInputInventoryService>
    {
        private readonly IInvoiceInputInventoryService _InvoiceInputInventoryService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public InvoiceInputInventoryController(IInvoiceInputInventoryService InvoiceInputInventoryService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceInputInventoryService, WebHostEnvironment)
        {
            _InvoiceInputInventoryService = InvoiceInputInventoryService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthToListAsync")]
        public virtual async Task<BaseResult<InvoiceInputInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync()
        {
            var result = new BaseResult<InvoiceInputInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInputInventory>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputInventoryService.GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<InvoiceInputInventory>> CreateAutoAsync()
        {
            var result = new BaseResult<InvoiceInputInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInputInventory>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputInventoryService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

