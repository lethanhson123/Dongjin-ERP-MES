namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryDepartmentController : BaseController<CategoryDepartment, ICategoryDepartmentService>
    {
        private readonly ICategoryDepartmentService _CategoryDepartmentService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryDepartmentController(ICategoryDepartmentService CategoryDepartmentService, IWebHostEnvironment WebHostEnvironment) : base(CategoryDepartmentService, WebHostEnvironment)
        {
            _CategoryDepartmentService = CategoryDepartmentService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<CategoryDepartment>> CreateAutoAsync()
        {
            var result = new BaseResult<CategoryDepartment>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryDepartment>>(Request.Form["BaseParameter"]);
                result = await _CategoryDepartmentService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_ActiveToListAsync")]
        public virtual async Task<BaseResult<CategoryDepartment>> GetByMembershipID_ActiveToListAsync()
        {
            var result = new BaseResult<CategoryDepartment>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryDepartment>>(Request.Form["BaseParameter"]);
                result = await _CategoryDepartmentService.GetByMembershipID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_CompanyID_ActiveToListAsync")]
        public virtual async Task<BaseResult<CategoryDepartment>> GetByMembershipID_CompanyID_ActiveToListAsync()
        {
            var result = new BaseResult<CategoryDepartment>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryDepartment>>(Request.Form["BaseParameter"]);
                result = await _CategoryDepartmentService.GetByMembershipID_CompanyID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

