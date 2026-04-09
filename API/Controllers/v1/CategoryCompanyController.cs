namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryCompanyController : BaseController<CategoryCompany, ICategoryCompanyService>
    {
        private readonly ICategoryCompanyService _CategoryCompanyService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryCompanyController(ICategoryCompanyService CategoryCompanyService, IWebHostEnvironment WebHostEnvironment) : base(CategoryCompanyService, WebHostEnvironment)
        {
            _CategoryCompanyService = CategoryCompanyService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("CategoryCompanyCreateAutoAsync")]
        public virtual async Task<BaseResult<CategoryCompany>> CategoryCompanyCreateAutoAsync()
        {
            var result = new BaseResult<CategoryCompany>();
            try
            {
                var BaseParameter = new BaseParameter<CategoryCompany>();
                result = await _CategoryCompanyService.GetAllToListAsync();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}