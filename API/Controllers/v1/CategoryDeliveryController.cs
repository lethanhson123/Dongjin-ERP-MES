namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryDeliveryController : BaseController<CategoryDelivery, ICategoryDeliveryService>
    {
    private readonly ICategoryDeliveryService _CategoryDeliveryService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryDeliveryController(ICategoryDeliveryService CategoryDeliveryService, IWebHostEnvironment WebHostEnvironment) : base(CategoryDeliveryService, WebHostEnvironment)
    {
    _CategoryDeliveryService = CategoryDeliveryService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

