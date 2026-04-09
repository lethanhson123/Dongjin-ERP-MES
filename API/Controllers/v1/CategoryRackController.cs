namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryRackController : BaseController<CategoryRack, ICategoryRackService>
    {
        private readonly ICategoryRackService _CategoryRackService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryRackController(ICategoryRackService CategoryRackService, IWebHostEnvironment WebHostEnvironment) : base(CategoryRackService, WebHostEnvironment)
        {
            _CategoryRackService = CategoryRackService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByParentIDAndCompanyIDAndActiveToListAsync")]
        public virtual async Task<BaseResult<CategoryRack>> GetByParentIDAndCompanyIDAndActiveToListAsync()
        {
            var result = new BaseResult<CategoryRack>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryRack>>(Request.Form["BaseParameter"]);
                result = await _CategoryRackService.GetByParentIDAndCompanyIDAndActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

