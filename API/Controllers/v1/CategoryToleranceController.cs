namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryToleranceController : BaseController<CategoryTolerance, ICategoryToleranceService>
    {
        private readonly ICategoryToleranceService _CategoryToleranceService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryToleranceController(ICategoryToleranceService CategoryToleranceService, IWebHostEnvironment WebHostEnvironment) : base(CategoryToleranceService, WebHostEnvironment)
        {
            _CategoryToleranceService = CategoryToleranceService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

