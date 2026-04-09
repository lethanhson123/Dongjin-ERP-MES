namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderMaterialController : BaseController<ProductionOrderMaterial, IProductionOrderMaterialService>
    {
        private readonly IProductionOrderMaterialService _ProductionOrderMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderMaterialController(IProductionOrderMaterialService ProductionOrderMaterialService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderMaterialService, WebHostEnvironment)
        {
            _ProductionOrderMaterialService = ProductionOrderMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<ProductionOrderMaterial>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<ProductionOrderMaterial>();
            result.BaseModel = new ProductionOrderMaterial();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderMaterial>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        BaseParameter.BaseModel = new ProductionOrderMaterial();
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
                                                var ListProductionOrderMaterial = await _ProductionOrderMaterialService.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.MaterialID > 0).ToListAsync();
                                                if (ListProductionOrderMaterial.Count > 0)
                                                {
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        var ProductionOrderMaterial = new ProductionOrderMaterial();
                                                        ProductionOrderMaterial.ParentID = BaseParameter.ParentID;
                                                        if (workSheet.Cells["A" + j].Value != null)
                                                        {
                                                            ProductionOrderMaterial.MaterialCode = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(ProductionOrderMaterial.MaterialCode))
                                                        {
                                                            var ListProductionOrderMaterialSub = ListProductionOrderMaterial.Where(o => o.MaterialCode == ProductionOrderMaterial.MaterialCode).OrderBy(o => o.Priority).ToList();
                                                            if (ListProductionOrderMaterialSub.Count > 0)
                                                            {
                                                                for (int i = 0; i < ListProductionOrderMaterialSub.Count; i++)
                                                                {
                                                                    ProductionOrderMaterial = ListProductionOrderMaterialSub[i];
                                                                    if (i == 0)
                                                                    {
                                                                        if (ProductionOrderMaterial != null && ProductionOrderMaterial.ID > 0)
                                                                        {
                                                                            if (workSheet.Cells["B" + j].Value != null)
                                                                            {
                                                                                try
                                                                                {
                                                                                    ProductionOrderMaterial.QuantityWIP = decimal.Parse(workSheet.Cells["B" + j].Value.ToString().Trim());
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
                                                                                    ProductionOrderMaterial.QuantityStock = decimal.Parse(workSheet.Cells["C" + j].Value.ToString().Trim());
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    string mes = ex.Message;
                                                                                }
                                                                            }

                                                                        }
                                                                    }
                                                                    if (ProductionOrderMaterial != null && ProductionOrderMaterial.ID > 0)
                                                                    {
                                                                        BaseParameter.BaseModel = ProductionOrderMaterial;
                                                                        await _ProductionOrderMaterialService.SaveAsync(BaseParameter);
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

