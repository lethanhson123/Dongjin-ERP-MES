namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseStockController : BaseController<WarehouseStock, IWarehouseStockService>
    {
        private readonly IWarehouseStockService _WarehouseStockService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseStockController(IWarehouseStockService WarehouseStockService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseStockService, WebHostEnvironment)
        {
            _WarehouseStockService = WarehouseStockService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("WarehouseStockCreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseStock>> WarehouseStockCreateAutoAsync(long CompanyID = 16, long CategoryDepartmentID = 23)
        {
            var result = new BaseResult<WarehouseStock>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseStock>();
                BaseParameter.CompanyID = CompanyID;
                BaseParameter.CategoryDepartmentID = CategoryDepartmentID;
                BaseParameter.Action = 1;
                BaseParameter.Year = GlobalHelper.InitializationDateTime.Year;
                BaseParameter.Month = GlobalHelper.InitializationDateTime.Month;
                BaseParameter.Day = GlobalHelper.InitializationDateTime.Day;
                result = await _WarehouseStockService.GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync(BaseParameter);
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
        public override async Task<BaseResult<WarehouseStock>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<WarehouseStock>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseStock>>(Request.Form["BaseParameter"]);
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
                                                    WarehouseStock WarehouseStock = new WarehouseStock();
                                                    if (workSheet.Cells[j, 1].Value != null)
                                                    {
                                                        WarehouseStock.Code = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(WarehouseStock.Code))
                                                    {
                                                        WarehouseStock.CompanyID = BaseParameter.CompanyID;
                                                        WarehouseStock.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                                                        WarehouseStock.ProductionOrderID = BaseParameter.GeneralID;
                                                        WarehouseStock.Year = BaseParameter.Year;
                                                        WarehouseStock.Month = BaseParameter.Month;
                                                        //var WarehouseStockCheck = new WarehouseStock();
                                                        //if (WarehouseStock.CategoryDepartmentID > 0)
                                                        //{
                                                        //    WarehouseStockCheck = await _WarehouseStockService.GetByCondition(o => o.Code == WarehouseStock.Code && o.CategoryDepartmentID == WarehouseStock.CategoryDepartmentID && o.Year == WarehouseStock.Year && o.Month == WarehouseStock.Month).FirstOrDefaultAsync();
                                                        //}
                                                        //else
                                                        //{
                                                        //    WarehouseStockCheck = await _WarehouseStockService.GetByCondition(o => o.Code == WarehouseStock.Code && o.CompanyID == WarehouseStock.CompanyID && o.Year == WarehouseStock.Year && o.Month == WarehouseStock.Month).FirstOrDefaultAsync();
                                                        //}
                                                        if (workSheet.Cells[j, 2].Value != null)
                                                        {
                                                            try
                                                            {
                                                                WarehouseStock.QuantityStock = decimal.Parse(workSheet.Cells[j, 2].Value.ToString().Trim());
                                                                WarehouseStock.UpdateUserID = BaseParameter.MembershipID;
                                                                await _WarehouseStockService.SaveAsync(new BaseParameter<WarehouseStock>
                                                                {
                                                                    BaseModel = WarehouseStock
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
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync")]
        public virtual async Task<BaseResult<WarehouseStock>> GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync()
        {
            var result = new BaseResult<WarehouseStock>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseStock>>(Request.Form["BaseParameter"]);                
                result = await _WarehouseStockService.GetByCompanyIDAndCategoryDepartmentIDAndYearAndMonthAndDayAndActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncAsync")]
        public virtual async Task<BaseResult<WarehouseStock>> SyncAsync()
        {
            var result = new BaseResult<WarehouseStock>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseStock>>(Request.Form["BaseParameter"]);
                result = await _WarehouseStockService.SyncAsync(BaseParameter);
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

