namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryMenuController : BaseController<CategoryMenu, ICategoryMenuService>
    {
        private readonly ICategoryMenuService _CategoryMenuService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryMenuController(ICategoryMenuService CategoryMenuService, IWebHostEnvironment WebHostEnvironment) : base(CategoryMenuService, WebHostEnvironment)
        {
            _CategoryMenuService = CategoryMenuService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByMembershipID_ActiveToListAsync")]
        public virtual async Task<BaseResult<CategoryMenu>> GetByMembershipID_ActiveToListAsync()
        {
            var result = new BaseResult<CategoryMenu>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryMenu>>(Request.Form["BaseParameter"]);
                result = await _CategoryMenuService.GetByMembershipID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

