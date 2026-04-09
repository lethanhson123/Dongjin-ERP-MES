namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputController : BaseController<WarehouseOutput, IWarehouseOutputService>
    {
        private readonly IWarehouseOutputService _WarehouseOutputService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputController(IWarehouseOutputService WarehouseOutputService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputService, WebHostEnvironment)
        {
            _WarehouseOutputService = WarehouseOutputService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync(long? CompanyID = 0, long? CategoryDepartmentID = 0, long ID = 0, int Action = 0)
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.CompanyID = CompanyID;
                BaseParameter.CategoryDepartmentID = CategoryDepartmentID;
                BaseParameter.ID = ID;
                BaseParameter.Action = Action;
                result = await _WarehouseOutputService.SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync(long? CompanyID = 0, long? CategoryDepartmentID = 0, int Action = 0, string SearchString = "")
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.CompanyID = CompanyID;
                BaseParameter.CategoryDepartmentID = CategoryDepartmentID;
                BaseParameter.Action = Action;
                BaseParameter.SearchString = SearchString;                
                result = await _WarehouseOutputService.SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(long ID = 0, string UserName = "")
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.ID = ID;
                BaseParameter.UserName = UserName;
                result = await _WarehouseOutputService.SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncOutputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncOutputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(long ID = 0, string UserName = "")
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.ID = ID;
                BaseParameter.UserName = UserName;
                result = await _WarehouseOutputService.SyncOutputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncOutputInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncOutputInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(long ID = 0, string UserName = "", string SearchString = "")
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.ID = ID;
                BaseParameter.UserName = UserName;
                BaseParameter.SearchString = SearchString;
                result = await _WarehouseOutputService.SyncOutputInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("WarehouseOutputCreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> WarehouseOutputCreateAutoAsync(int year = 2025, int month = 12, int day = 1)
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseOutput>();
                BaseParameter.Year = year;
                BaseParameter.Month = month;
                BaseParameter.Day = day;
                result = await _WarehouseOutputService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySupplierID_ActiveToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_ActiveToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetBySupplierID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySupplierID_Active_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsCompleteToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetBySupplierID_Active_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySupplierID_Active_IsComplete_ActionToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsComplete_ActionToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetBySupplierID_Active_IsComplete_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_DateBegin_DateEndToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_DateBegin_DateEndToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByMembershipID_Active_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsCompleteToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_IsComplete_ActionToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsComplete_ActionToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeFreedomNoFIFOAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomNoFIFOAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByBarcodeFreedomNoFIFOAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeFreedomAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByBarcodeFreedomAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByBarcodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeNoFIFOAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByBarcodeNoFIFOAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByBarcodeNoFIFOAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintGroupAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroupAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintGroupAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintGroup2025Async")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroup2025Async()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintGroup2025Async(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintGroup2026Async")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroup2026Async()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintGroup2026Async(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintGroupMobileAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintGroupMobileAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintGroupMobileAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintSumAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> PrintSumAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.PrintSumAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_MembershipToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_MembershipToListAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_MembershipToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncFromMESByCompanyID_CategoryDepartmentIDAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseOutput>> CreateAutoAsync()
        {
            var result = new BaseResult<WarehouseOutput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputService.CreateAutoAsync(BaseParameter);
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

