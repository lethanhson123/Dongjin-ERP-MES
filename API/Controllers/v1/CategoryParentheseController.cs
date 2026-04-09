namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryParentheseController : BaseController<CategoryParenthese, ICategoryParentheseService>
    {
        private readonly ICategoryParentheseService _CategoryParentheseService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryParentheseController(ICategoryParentheseService CategoryParentheseService, IWebHostEnvironment WebHostEnvironment) : base(CategoryParentheseService, WebHostEnvironment)
        {
            _CategoryParentheseService = CategoryParentheseService;
            _WebHostEnvironment = WebHostEnvironment;
        }       
    }
}