namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderProductionPlanMaterialController : BaseController<ProductionOrderProductionPlanMaterial, IProductionOrderProductionPlanMaterialService>
    {
        private readonly IProductionOrderProductionPlanMaterialService _ProductionOrderProductionPlanMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderProductionPlanMaterialController(IProductionOrderProductionPlanMaterialService ProductionOrderProductionPlanMaterialService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderProductionPlanMaterialService, WebHostEnvironment)
        {
            _ProductionOrderProductionPlanMaterialService = ProductionOrderProductionPlanMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<ProductionOrderProductionPlanMaterial>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
            result.BaseModel = new ProductionOrderProductionPlanMaterial();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlanMaterial>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        BaseParameter.BaseModel = new ProductionOrderProductionPlanMaterial();
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            if (file == null || file.Length == 0)
                            {
                            }
                            if (file != null)
                            {
                                string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                                bool isFolderExists = System.IO.Directory.Exists(folderPath);
                                if (!isFolderExists)
                                {
                                    System.IO.Directory.CreateDirectory(folderPath);
                                }
                                BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "-" + GlobalHelper.InitializationDateTimeCode0001 + "_" + file.FileName;
                                var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                                using (var stream = new FileStream(physicalPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                    BaseParameter.BaseModel.FileName = physicalPath;
                                }
                                FileInfo fileLocation = new FileInfo(physicalPath);
                                if (fileLocation.Length > 0)
                                {
                                    using (ExcelPackage package = new ExcelPackage(fileLocation))
                                    {
                                        if (package.Workbook.Worksheets.Count > 0)
                                        {
                                            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                            if (workSheet != null)
                                            {
                                                int totalRows = workSheet.Dimension.Rows;
                                                var ListProductionOrderProductionPlanMaterial = await _ProductionOrderProductionPlanMaterialService.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.MaterialID > 0).ToListAsync();
                                                if (ListProductionOrderProductionPlanMaterial.Count > 0)
                                                {
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        var ProductionOrderProductionPlanMaterial = new ProductionOrderProductionPlanMaterial();
                                                        ProductionOrderProductionPlanMaterial.ParentID = BaseParameter.ParentID;
                                                        if (workSheet.Cells["A" + j].Value != null)
                                                        {
                                                            ProductionOrderProductionPlanMaterial.MaterialCode = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(ProductionOrderProductionPlanMaterial.MaterialCode))
                                                        {
                                                            var ListProductionOrderProductionPlanMaterialSub = ListProductionOrderProductionPlanMaterial.Where(o => o.MaterialCode == ProductionOrderProductionPlanMaterial.MaterialCode).OrderBy(o => o.Priority).ToList();
                                                            if (ListProductionOrderProductionPlanMaterialSub.Count > 0)
                                                            {
                                                                for (int i = 0; i < ListProductionOrderProductionPlanMaterialSub.Count; i++)
                                                                {
                                                                    ProductionOrderProductionPlanMaterial = ListProductionOrderProductionPlanMaterialSub[i];
                                                                    if (i == 0)
                                                                    {
                                                                        if (ProductionOrderProductionPlanMaterial != null && ProductionOrderProductionPlanMaterial.ID > 0)
                                                                        {
                                                                            if (workSheet.Cells["B" + j].Value != null)
                                                                            {
                                                                                try
                                                                                {
                                                                                    ProductionOrderProductionPlanMaterial.QuantityWIP = decimal.Parse(workSheet.Cells["B" + j].Value.ToString().Trim());
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    string mes = ex.Message;
                                                                                }
                                                                            }
                                                                            if (workSheet.Cells["C" + j].Value != null)
                                                                            {
                                                                                try
                                                                                {
                                                                                    ProductionOrderProductionPlanMaterial.QuantityStock = decimal.Parse(workSheet.Cells["C" + j].Value.ToString().Trim());
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    string mes = ex.Message;
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                    if (ProductionOrderProductionPlanMaterial != null && ProductionOrderProductionPlanMaterial.ID > 0)
                                                                    {
                                                                        BaseParameter.BaseModel = ProductionOrderProductionPlanMaterial;
                                                                        await _ProductionOrderProductionPlanMaterialService.SaveAsync(BaseParameter);
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

