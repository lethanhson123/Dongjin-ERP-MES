namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInputDetailController : BaseController<InvoiceInputDetail, IInvoiceInputDetailService>
    {
    private readonly IInvoiceInputDetailService _InvoiceInputDetailService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public InvoiceInputDetailController(IInvoiceInputDetailService InvoiceInputDetailService, IWebHostEnvironment WebHostEnvironment) : base(InvoiceInputDetailService, WebHostEnvironment)
    {
    _InvoiceInputDetailService = InvoiceInputDetailService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

