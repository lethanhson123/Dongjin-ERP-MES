namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInputFileController : BaseController<InvoiceInputFile, IInvoiceInputFileService>
    {
        private readonly IInvoiceInputFileService _InvoiceInputFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public InvoiceInputFileController(IInvoiceInputFileService InvoiceInputFileService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceInputFileService, WebHostEnvironment)
        {
            _InvoiceInputFileService = InvoiceInputFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

