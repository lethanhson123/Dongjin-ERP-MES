namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryLocationMaterialController : BaseController<CategoryLocationMaterial, ICategoryLocationMaterialService>
    {
        private readonly ICategoryLocationMaterialService _CategoryLocationMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryLocationMaterialController(ICategoryLocationMaterialService CategoryLocationMaterialService, IWebHostEnvironment WebHostEnvironment) : base(CategoryLocationMaterialService, WebHostEnvironment)
        {
            _CategoryLocationMaterialService = CategoryLocationMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }       
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<CategoryLocationMaterial>> CreateAutoAsync()
        {
            var result = new BaseResult<CategoryLocationMaterial>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocationMaterial>>(Request.Form["BaseParameter"]);                
                result = await _CategoryLocationMaterialService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
                
    }
}

