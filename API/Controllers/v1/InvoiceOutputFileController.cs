namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceOutputFileController : BaseController<InvoiceOutputFile, IInvoiceOutputFileService>
    {
    private readonly IInvoiceOutputFileService _InvoiceOutputFileService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public InvoiceOutputFileController(IInvoiceOutputFileService InvoiceOutputFileService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceOutputFileService, WebHostEnvironment)
    {
    _InvoiceOutputFileService = InvoiceOutputFileService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

