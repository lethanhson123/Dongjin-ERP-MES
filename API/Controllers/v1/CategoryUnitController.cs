namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryUnitController : BaseController<CategoryUnit, ICategoryUnitService>
    {
        private readonly ICategoryUnitService _CategoryUnitService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryUnitController(ICategoryUnitService CategoryUnitService, IWebHostEnvironment WebHostEnvironment) : base(CategoryUnitService, WebHostEnvironment)
        {
            _CategoryUnitService = CategoryUnitService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

