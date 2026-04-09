namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryInvoiceController : BaseController<CategoryInvoice, ICategoryInvoiceService>
    {
    private readonly ICategoryInvoiceService _CategoryInvoiceService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryInvoiceController(ICategoryInvoiceService CategoryInvoiceService, IWebHostEnvironment WebHostEnvironment) : base(CategoryInvoiceService, WebHostEnvironment)
    {
    _CategoryInvoiceService = CategoryInvoiceService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

