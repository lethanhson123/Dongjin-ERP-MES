namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategorySealKitController : BaseController<CategorySealKit, ICategorySealKitService>
    {
        private readonly ICategorySealKitService _CategorySealKitService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategorySealKitController(ICategorySealKitService CategorySealKitService, IWebHostEnvironment WebHostEnvironment) : base(CategorySealKitService, WebHostEnvironment)
        {
            _CategorySealKitService = CategorySealKitService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

