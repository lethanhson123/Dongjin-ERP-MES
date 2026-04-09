namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputDetailBarcodeController : BaseController<WarehouseInputDetailBarcode, IWarehouseInputDetailBarcodeService>
    {
        private readonly IWarehouseInputDetailBarcodeService _WarehouseInputDetailBarcodeService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputDetailBarcodeController(IWarehouseInputDetailBarcodeService WarehouseInputDetailBarcodeService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputDetailBarcodeService, WebHostEnvironment)
        {
            _WarehouseInputDetailBarcodeService = WarehouseInputDetailBarcodeService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetByBarcodeFromtrackmtimAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtrackmtimAsync(long CompanyID, string Barcode)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInputDetailBarcode>();
                BaseParameter.CompanyID = CompanyID;
                BaseParameter.Barcode = Barcode;
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeFromtrackmtimAsync(BaseParameter);
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
        [Route("GetSyncByParrentIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetSyncByParrentIDAsync(long ParentID)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInputDetailBarcode>();
                BaseParameter.ParentID = ParentID;                
                result = await _WarehouseInputDetailBarcodeService.SyncByParrentIDAsync(BaseParameter);
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
        [Route("GetFGByBarcodeFromtdpdmtimAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetFGByBarcodeFromtdpdmtimAsync(string Barcode)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInputDetailBarcode>();
                BaseParameter.Barcode = Barcode;              
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeFromtdpdmtimAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseInputDetailIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByWarehouseInputDetailIDToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByWarehouseInputDetailIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentID_MaterialIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentID_MaterialIDToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByParentID_MaterialIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_BarcodeAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_BarcodeAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_BarcodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeFromtmbrcdAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtmbrcdAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeFromtmbrcdAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeFromtdpdmtimAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtdpdmtimAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeFromtdpdmtimAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeFromtrackmtimAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtrackmtimAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByBarcodeFromtrackmtimAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMaterialID_CategoryLocationIDFromDiagramToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByMaterialID_CategoryLocationIDFromDiagramToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByMaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndEmpty_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentIDAndEmpty_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByBarcodeAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByBarcodeAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByBarcodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByParentIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByParentIDAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByParentIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByListWarehouseInputDetailID2025Async")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailID2025Async()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByListWarehouseInputDetailID2025Async(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByListWarehouseInputDetailIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailIDAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByListWarehouseInputDetailIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByWarehouseInputDetailIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByWarehouseInputDetailIDAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByWarehouseInputDetailIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintBarcode_WarehouseOutputIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintBarcode_WarehouseOutputIDAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintBarcode_WarehouseOutputIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.PrintByListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> CreateAutoAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetCompareMESAndERPToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetCompareMESAndERPToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetCompareMESAndERPToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetPARTNOCompareMESAndERPToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetPARTNOCompareMESAndERPToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetPARTNOCompareMESAndERPToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByParrentIDAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncByParrentIDAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.SyncByParrentIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByHOOKRACK_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByHOOKRACK_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByHOOKRACK_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByKOMAX_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByKOMAX_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByKOMAX_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySHIELDWIRE_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetBySHIELDWIRE_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetBySHIELDWIRE_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByLP_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByLP_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetByLP_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySPST_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetBySPST_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.GetBySPST_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> ExportToExcelAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.ExportToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncStockAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncStockAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.SyncStockAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("AutoSyncAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> AutoSyncAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeService.AutoSyncAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

