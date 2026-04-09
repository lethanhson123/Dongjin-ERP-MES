namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseRequestController : BaseController<WarehouseRequest, IWarehouseRequestService>
    {
        private readonly IWarehouseRequestService _WarehouseRequestService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseRequestDetailService _WarehouseRequestDetailService;
        private readonly IWarehouseRequestFileService _WarehouseRequestFileService;
        public WarehouseRequestController(IWarehouseRequestService WarehouseRequestService
            , IWebHostEnvironment WebHostEnvironment
            , IWarehouseRequestDetailService warehouseRequestDetailService
            , IWarehouseRequestFileService warehouseRequestFileService) : base(WarehouseRequestService, WebHostEnvironment)
        {
            _WarehouseRequestService = WarehouseRequestService;
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseRequestDetailService = warehouseRequestDetailService;
            _WarehouseRequestFileService = warehouseRequestFileService;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFilesAsync")]
        public override async Task<BaseResult<WarehouseRequest>> SaveAndUploadFilesAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.SaveAsync(BaseParameter);
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
                            string controllerName = this.GetType().Name;
                            string fileExtension = Path.GetExtension(file.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                            {
                                string fileName = controllerName + "_" + result.BaseModel.ID + '_' + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                    var WarehouseRequestFile = new WarehouseRequestFile();
                                    WarehouseRequestFile.Code = result.BaseModel.Code;
                                    WarehouseRequestFile.ParentID = result.BaseModel.ID;
                                    WarehouseRequestFile.Description = physicalPath;
                                    WarehouseRequestFile.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + controllerName + "/" + fileName;
                                    var BaseParameterWarehouseRequestFile = new BaseParameter<WarehouseRequestFile>();
                                    BaseParameterWarehouseRequestFile.BaseModel = WarehouseRequestFile;
                                    await _WarehouseRequestFileService.SaveAsync(BaseParameterWarehouseRequestFile);
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
                                                if (result.BaseModel != null)
                                                {
                                                    if (result.BaseModel.ID > 0)
                                                    {
                                                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                        if (workSheet != null)
                                                        {
                                                            int totalRows = workSheet.Dimension.Rows;
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                WarehouseRequestDetail WarehouseRequestDetail = new WarehouseRequestDetail();
                                                                if (workSheet.Cells[j, 1].Value != null)
                                                                {
                                                                    WarehouseRequestDetail.MaterialName = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                }
                                                                if (!string.IsNullOrEmpty(WarehouseRequestDetail.MaterialName))
                                                                {
                                                                    if (workSheet.Cells[j, 2].Value != null)
                                                                    {
                                                                        WarehouseRequestDetail.CategoryUnitName = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 3].Value != null)
                                                                    {
                                                                        WarehouseRequestDetail.Description = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                        if (!string.IsNullOrEmpty(WarehouseRequestDetail.Description) && WarehouseRequestDetail.Description.Length > 0)
                                                                        {
                                                                            WarehouseRequestDetail.IsSNP = true;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 4].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            WarehouseRequestDetail.QuantitySNP = decimal.Parse(workSheet.Cells[j, 4].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 5].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            WarehouseRequestDetail.QuantityInvoice = decimal.Parse(workSheet.Cells[j, 5].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 6].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            WarehouseRequestDetail.Quantity = decimal.Parse(workSheet.Cells[j, 6].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    WarehouseRequestDetail.ParentID = result.BaseModel.ID;
                                                                    await _WarehouseRequestDetailService.SaveAsync(new BaseParameter<WarehouseRequestDetail>
                                                                    {
                                                                        BaseModel = WarehouseRequestDetail
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
        [Route("SaveAndUploadFilesByCategoryDepartmentAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> SaveAndUploadFilesByCategoryDepartmentAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.SaveAsync(BaseParameter);
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
                            string controllerName = this.GetType().Name;
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
                                                if (result.BaseModel != null)
                                                {
                                                    if (result.BaseModel.ID > 0)
                                                    {
                                                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                        if (workSheet != null)
                                                        {
                                                            int totalRows = workSheet.Dimension.Rows;
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                WarehouseRequestDetail WarehouseRequestDetail = new WarehouseRequestDetail();
                                                                if (workSheet.Cells[j, 1].Value != null)
                                                                {
                                                                    WarehouseRequestDetail.MaterialName = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                }
                                                                if (!string.IsNullOrEmpty(WarehouseRequestDetail.MaterialName))
                                                                {
                                                                    if (workSheet.Cells[j, 2].Value != null)
                                                                    {
                                                                        WarehouseRequestDetail.CategoryUnitName = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 3].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            WarehouseRequestDetail.Quantity = decimal.Parse(workSheet.Cells[j, 3].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    WarehouseRequestDetail.ParentID = result.BaseModel.ID;
                                                                    await _WarehouseRequestDetailService.SaveAsync(new BaseParameter<WarehouseRequestDetail>
                                                                    {
                                                                        BaseModel = WarehouseRequestDetail
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
        [Route("GetByConfirmToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByConfirmToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByConfirmToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_DateBegin_DateEndToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipID_DateBegin_DateEndToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_ConfirmToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipID_ConfirmToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByMembershipID_ConfirmToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> GetByCategoryDepartmentID_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.GetByCategoryDepartmentID_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<WarehouseRequest>> ExportToExcelAsync()
        {
            var result = new BaseResult<WarehouseRequest>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseRequest>>(Request.Form["BaseParameter"]);
                result = await _WarehouseRequestService.ExportToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

