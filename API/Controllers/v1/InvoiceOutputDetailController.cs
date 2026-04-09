namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceOutputDetailController : BaseController<InvoiceOutputDetail, IInvoiceOutputDetailService>
    {
    private readonly IInvoiceOutputDetailService _InvoiceOutputDetailService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public InvoiceOutputDetailController(IInvoiceOutputDetailService InvoiceOutputDetailService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceOutputDetailService, WebHostEnvironment)
    {
    _InvoiceOutputDetailService = InvoiceOutputDetailService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

