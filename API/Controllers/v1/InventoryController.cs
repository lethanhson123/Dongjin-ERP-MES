namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InventoryController : BaseController<Inventory, IInventoryService>
    {
        private readonly IInventoryService _InventoryService;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IInventoryDetailService _InventoryDetailService;
        public InventoryController(IInventoryService InventoryService, IWebHostEnvironment WebHostEnvironment
            , IInventoryDetailService InventoryDetailService

            ) : base(InventoryService, WebHostEnvironment)
        {
            _InventoryService = InventoryService;
            _WebHostEnvironment = WebHostEnvironment;
            _InventoryDetailService = InventoryDetailService;
        }
        [HttpPost]
        [Route("SyncByIDAsync")]
        public virtual async Task<BaseResult<Inventory>> SyncByIDAsync()
        {
            var result = new BaseResult<Inventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Inventory>>(Request.Form["BaseParameter"]);
                result = await _InventoryService.SyncByIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAsync")]
        public virtual async Task<BaseResult<Inventory>> GetByCategoryDepartmentIDAsync()
        {
            var result = new BaseResult<Inventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Inventory>>(Request.Form["BaseParameter"]);
                result = await _InventoryService.GetByCategoryDepartmentIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDToListAsync")]
        public virtual async Task<BaseResult<Inventory>> GetByCategoryDepartmentIDToListAsync()
        {
            var result = new BaseResult<Inventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Inventory>>(Request.Form["BaseParameter"]);
                result = await _InventoryService.GetByCategoryDepartmentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<Inventory>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<Inventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Inventory>>(Request.Form["BaseParameter"]);
                result = await _InventoryService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<Inventory>> GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<Inventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Inventory>>(Request.Form["BaseParameter"]);
                result = await _InventoryService.GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

