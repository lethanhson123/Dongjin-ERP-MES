namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InventoryDetailController : BaseController<InventoryDetail, IInventoryDetailService>
    {
        private readonly IInventoryDetailService _InventoryDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public InventoryDetailController(IInventoryDetailService InventoryDetailService, IWebHostEnvironment WebHostEnvironment) : base(InventoryDetailService, WebHostEnvironment)
        {
            _InventoryDetailService = InventoryDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("SyncByParentIDToListAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> SyncByParentIDToListAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.SyncByParentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByParentIDCategoryLocationNameToListAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> SyncByParentIDCategoryLocationNameToListAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.SyncByParentIDCategoryLocationNameToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByIDAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> PrintByIDAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.PrintByIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("Print2025ByIDAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> Print2025ByIDAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.Print2025ByIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByListAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> PrintByListAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.PrintByListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("Print2025ByListAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> Print2025ByListAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.Print2025ByListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<InventoryDetail>> ExportToExcelAsync()
        {
            var result = new BaseResult<InventoryDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetail>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailService.ExportToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

