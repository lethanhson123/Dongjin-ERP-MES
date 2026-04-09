namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInputHistoryController : BaseController<InvoiceInputHistory, IInvoiceInputHistoryService>
    {
        private readonly IInvoiceInputHistoryService _InvoiceInputHistoryService;
        private readonly IWebHostEnvironment _WebHostEnvironment;


        public InvoiceInputHistoryController(IInvoiceInputHistoryService InvoiceInputHistoryService
            , IWebHostEnvironment WebHostEnvironment
            ) : base(InvoiceInputHistoryService, WebHostEnvironment)
        {
            _InvoiceInputHistoryService = InvoiceInputHistoryService;
            _WebHostEnvironment = WebHostEnvironment;
        }

    }
}

