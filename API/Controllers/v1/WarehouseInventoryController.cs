namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInventoryController : BaseController<WarehouseInventory, IWarehouseInventoryService>
    {
        private readonly IWarehouseInventoryService _WarehourseInventoryService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInventoryController(IWarehouseInventoryService WarehourseInventoryService, IWebHostEnvironment WebHostEnvironment) : base(WarehourseInventoryService, WebHostEnvironment)
        {
            _WarehourseInventoryService = WarehourseInventoryService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("WarehouseInventoryCreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> WarehouseInventoryCreateAutoAsync(long CategoryDepartmentID = 23, int Year = 0, int Month = 0, string SearchString = "")
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInventory>();
                BaseParameter.CategoryDepartmentID = CategoryDepartmentID;
                BaseParameter.Year = Year;
                BaseParameter.Month = Month;
                BaseParameter.SearchString = SearchString;
                result = await _WarehourseInventoryService.CreateAutoAsync(BaseParameter);
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
        public virtual async Task<BaseResult<WarehouseInventory>> CreateAutoAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<WarehouseInventory>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0 && BaseParameter != null)
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
                                            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                            if (workSheet != null)
                                            {
                                                int totalRows = workSheet.Dimension.Rows;
                                                for (int j = 2; j <= totalRows; j++)
                                                {
                                                    WarehouseInventory WarehouseInventory = new WarehouseInventory();
                                                    if (workSheet.Cells[j, 1].Value != null)
                                                    {
                                                        WarehouseInventory.Code = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(WarehouseInventory.Code))
                                                    {
                                                        WarehouseInventory.CompanyID = BaseParameter.CompanyID;
                                                        WarehouseInventory.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                                                        WarehouseInventory.Year = BaseParameter.Year;
                                                        WarehouseInventory.Month = BaseParameter.Month;
                                                        var WarehouseInventoryCheck = new WarehouseInventory();
                                                        if (WarehouseInventory.CategoryDepartmentID > 0)
                                                        {
                                                            WarehouseInventoryCheck = await _WarehourseInventoryService.GetByCondition(o => o.Code == WarehouseInventory.Code && o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                                        }
                                                        else
                                                        {
                                                            WarehouseInventoryCheck = await _WarehourseInventoryService.GetByCondition(o => o.Code == WarehouseInventory.Code && o.CompanyID == WarehouseInventory.CompanyID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                                        }
                                                        if (WarehouseInventoryCheck != null && WarehouseInventoryCheck.ID > 0)
                                                        {
                                                            if (workSheet.Cells[j, 2].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    WarehouseInventoryCheck.QuantityStock = decimal.Parse(workSheet.Cells[j, 2].Value.ToString().Trim());
                                                                    WarehouseInventoryCheck.UpdateUserID = BaseParameter.MembershipID;
                                                                    await _WarehourseInventoryService.SaveAsync(new BaseParameter<WarehouseInventory>
                                                                    {
                                                                        BaseModel = WarehouseInventoryCheck
                                                                    });
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    string mes = e.Message;
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
        [Route("GetByActionAndYearAndMonthAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByActionAndYearAndMonthAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByActionAndYearAndMonthAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndYearAndMonthToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCompanyIDAndYearAndMonthToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCompanyIDAndYearAndMonthToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByCategoryDepartmentIDAndYearAndMonthAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByCategoryDepartmentIDAndYearAndMonthAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.SyncByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncValueByCategoryDepartmentIDAndYearAndMonthAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> SyncValueByCategoryDepartmentIDAndYearAndMonthAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInventory>>(Request.Form["BaseParameter"]);
                result = await _WarehourseInventoryService.SyncValueByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncAutoAsync")]
        public virtual async Task<BaseResult<WarehouseInventory>> SyncAutoAsync()
        {
            var result = new BaseResult<WarehouseInventory>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInventory>();
                result = await _WarehourseInventoryService.SyncAutoAsync(BaseParameter);
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

