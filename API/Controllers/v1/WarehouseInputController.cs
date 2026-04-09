namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputController : BaseController<WarehouseInput, IWarehouseInputService>
    {
        private readonly IWarehouseInputService _WarehouseInputService;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IWarehouseInputDetailService _WarehouseInputDetailService;
        private readonly IWarehouseInputFileService _WarehouseInputFileService;
        public WarehouseInputController(IWarehouseInputService WarehouseInputService, IWebHostEnvironment WebHostEnvironment
            , IWarehouseInputDetailService warehouseInputDetailService
            , IWarehouseInputFileService WarehouseInputFileService

            ) : base(WarehouseInputService, WebHostEnvironment)
        {
            _WarehouseInputService = WarehouseInputService;
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseInputDetailService = warehouseInputDetailService;
            _WarehouseInputFileService = WarehouseInputFileService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncFromMES_C03ByCompanyID_Action_IDAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> SyncFromMES_C03ByCompanyID_Action_IDAsync(long? CompanyID, int? Action = 0, long ID = 0)
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInput>();
                BaseParameter.CompanyID = CompanyID;
                BaseParameter.Action = Action;
                BaseParameter.ID = ID;
                result = await _WarehouseInputService.SyncFromMES_C03ByCompanyID_Action_IDAsync(BaseParameter);
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
        [Route("SyncInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> SyncInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(long ID = 0, string UserName = "")
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInput>();
                BaseParameter.ID = ID;
                BaseParameter.UserName = UserName;
                result = await _WarehouseInputService.SyncInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter);
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
        public virtual async Task<BaseResult<WarehouseInput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(long ID = 0, string UserName = "")
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInput>();
                BaseParameter.ID = ID;
                BaseParameter.UserName = UserName;
                result = await _WarehouseInputService.SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter);
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
        [Route("WarehouseInputCreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> WarehouseInputCreateAutoAsync(int Year = 0)
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInput>();
                BaseParameter.Year = Year;
                result = await _WarehouseInputService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CreateAutoByPageIndexAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> CreateAutoByPageIndexAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<WarehouseInput>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.SaveAsync(BaseParameter);
                if (result != null && result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string controllerName = result.BaseModel.GetType().Name;
                            string fileExtension = Path.GetExtension(file.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                            {
                                string fileName = controllerName + "_" + result.BaseModel.ID + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                    var WarehouseInputFile = new WarehouseInputFile();
                                    WarehouseInputFile.Code = result.BaseModel.Code;
                                    WarehouseInputFile.ParentID = result.BaseModel.ID;
                                    WarehouseInputFile.Description = physicalPath;
                                    WarehouseInputFile.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + controllerName + "/" + fileName;
                                    var BaseParameterWarehouseInputFile = new BaseParameter<WarehouseInputFile>();
                                    BaseParameterWarehouseInputFile.BaseModel = WarehouseInputFile;
                                    await _WarehouseInputFileService.SaveAsync(BaseParameterWarehouseInputFile);
                                }
                                FileInfo fileLocation = new FileInfo(filePath);
                                if (fileLocation.Length > 0)
                                {
                                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                                    {
                                        using (ExcelPackage package = new ExcelPackage(fileLocation))
                                        {
                                            if (package.Workbook.Worksheets.Count > 0)
                                            {
                                                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                if (workSheet != null)
                                                {
                                                    int totalRows = workSheet.Dimension.Rows;
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        WarehouseInputDetail WarehouseInputDetail = new WarehouseInputDetail();
                                                        if (workSheet.Cells[j, 1].Value != null)
                                                        {
                                                            WarehouseInputDetail.MaterialName = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(WarehouseInputDetail.MaterialName))
                                                        {
                                                            if (workSheet.Cells[j, 2].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    WarehouseInputDetail.QuantitySNP = decimal.Parse(workSheet.Cells[j, 2].Value.ToString().Trim());
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    string mes = e.Message;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 3].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    WarehouseInputDetail.Quantity = decimal.Parse(workSheet.Cells[j, 3].Value.ToString().Trim());
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    string mes = e.Message;
                                                                }
                                                            }
                                                            WarehouseInputDetail.ParentID = result.BaseModel.ID;
                                                            await _WarehouseInputDetailService.SaveAsync(new BaseParameter<WarehouseInputDetail>
                                                            {
                                                                BaseModel = WarehouseInputDetail
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileStockAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> SaveAndUploadFileStockAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.SaveAsync(BaseParameter);
                if (result != null && result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string controllerName = result.BaseModel.GetType().Name;
                            string fileExtension = Path.GetExtension(file.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                            {
                                string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                                FileInfo fileLocation = new FileInfo(filePath);
                                if (fileLocation.Length > 0)
                                {
                                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                                    {
                                        using (ExcelPackage package = new ExcelPackage(fileLocation))
                                        {
                                            if (package.Workbook.Worksheets.Count > 0)
                                            {
                                                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                if (workSheet != null)
                                                {
                                                    int totalRows = workSheet.Dimension.Rows;
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        WarehouseInputDetail WarehouseInputDetail = new WarehouseInputDetail();
                                                        if (workSheet.Cells[j, 1].Value != null)
                                                        {
                                                            WarehouseInputDetail.MaterialName = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(WarehouseInputDetail.MaterialName))
                                                        {
                                                            if (workSheet.Cells[j, 2].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    WarehouseInputDetail.QuantityStock = decimal.Parse(workSheet.Cells[j, 2].Value.ToString().Trim());
                                                                    WarehouseInputDetail.QuantitySNP = WarehouseInputDetail.QuantityStock;
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    string mes = e.Message;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 3].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    string DateBegin = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                    WarehouseInputDetail.DateBegin = GlobalHelper.CovertStringToDateTime(DateBegin);
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    string mes = e.Message;
                                                                }
                                                            }
                                                            WarehouseInputDetail.ParentID = result.BaseModel.ID;
                                                            WarehouseInputDetail.UpdateUserID = result.BaseModel.UpdateUserID;
                                                            await _WarehouseInputDetailService.SaveAsync(new BaseParameter<WarehouseInputDetail>
                                                            {
                                                                BaseModel = WarehouseInputDetail
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetByCustomerID_Active_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsCompleteToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByCustomerID_Active_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCustomerID_Active_IsComplete_ActionToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsComplete_ActionToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByCustomerID_Active_IsComplete_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsCompleteToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_IsComplete_ActionToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsComplete_ActionToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcodeAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> GetByBarcodeAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.GetByBarcodeAsync(BaseParameter);
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
        public virtual async Task<BaseResult<WarehouseInput>> CreateAutoAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                //var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                var BaseParameter = new BaseParameter<WarehouseInput>();
                result = await _WarehouseInputService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncFromMESByCompanyID_CategoryDepartmentIDAsync")]
        public virtual async Task<BaseResult<WarehouseInput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync()
        {
            var result = new BaseResult<WarehouseInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInput>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputService.SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter);
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

