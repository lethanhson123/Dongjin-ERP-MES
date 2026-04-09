namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryConfigController : BaseController<CategoryConfig, ICategoryConfigService>
    {
        private readonly ICategoryConfigService _CategoryConfigService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryConfigController(ICategoryConfigService CategoryConfigService, IWebHostEnvironment WebHostEnvironment) : base(CategoryConfigService, WebHostEnvironment)
        {
            _CategoryConfigService = CategoryConfigService;
            _WebHostEnvironment = WebHostEnvironment;
        }       
    }
}