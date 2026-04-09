namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputDetailController : BaseController<WarehouseOutputDetail, IWarehouseOutputDetailService>
    {
        private readonly IWarehouseOutputDetailService _WarehouseOutputDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputDetailController(IWarehouseOutputDetailService WarehouseOutputDetailService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputDetailService, WebHostEnvironment)
        {
            _WarehouseOutputDetailService = WarehouseOutputDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByActive_IsComplete_MembershipAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetail>> GetByActive_IsComplete_MembershipAsync()
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailService.GetByActive_IsComplete_MembershipAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

