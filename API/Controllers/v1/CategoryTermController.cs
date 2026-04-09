namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryTermController : BaseController<CategoryTerm, ICategoryTermService>
    {
        private readonly ICategoryTermService _CategoryTermService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryTermController(ICategoryTermService CategoryTermService, IWebHostEnvironment WebHostEnvironment) : base(CategoryTermService, WebHostEnvironment)
        {
            _CategoryTermService = CategoryTermService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<CategoryTerm>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<CategoryTerm>();
            result.BaseModel = new CategoryTerm();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryTerm>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.CompanyID > 0)
                    {
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
                                                for (int j = 2; j <= totalRows; j++)
                                                {
                                                    var CategoryTerm = new CategoryTerm();

                                                    if (workSheet.Cells["A" + j].Value != null)
                                                    {
                                                        CategoryTerm.Code = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        CategoryTerm.Name = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["C" + j].Value != null)
                                                    {
                                                        CategoryTerm.Display = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["D" + j].Value != null)
                                                    {
                                                        CategoryTerm.Description = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                        try
                                                        {
                                                            CategoryTerm.SQ = decimal.Parse(CategoryTerm.Description);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["E" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            CategoryTerm.CCH1 = decimal.Parse(workSheet.Cells["E" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["F" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            CategoryTerm.CCW1 = decimal.Parse(workSheet.Cells["F" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["G" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            CategoryTerm.ICH1 = decimal.Parse(workSheet.Cells["G" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["H" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            CategoryTerm.ICW1 = decimal.Parse(workSheet.Cells["H" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string message = ex.Message;
                                                        }
                                                    }
                                                    CategoryTerm.Active = true;
                                                    CategoryTerm.CompanyID = BaseParameter.CompanyID;
                                                    var BaseParameterCategoryTerm = new BaseParameter<CategoryTerm>();
                                                    BaseParameterCategoryTerm.BaseModel = CategoryTerm;
                                                    result = await _CategoryTermService.SaveAsync(BaseParameterCategoryTerm);
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

