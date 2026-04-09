using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryLocationController : BaseController<CategoryLocation, ICategoryLocationService>
    {
        private readonly ICategoryLocationService _CategoryLocationService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryLocationController(ICategoryLocationService CategoryLocationService, IWebHostEnvironment WebHostEnvironment) : base(CategoryLocationService, WebHostEnvironment)
        {
            _CategoryLocationService = CategoryLocationService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> CreateAutoAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = new BaseParameter<CategoryLocation>();
                result = await _CategoryLocationService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDToListAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> GetByCategoryDepartmentIDToListAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                result = await _CategoryLocationService.GetByCategoryDepartmentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentID_CategoryLayerIDToListAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> GetByParentID_CategoryLayerIDToListAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                result = await _CategoryLocationService.GetByParentID_CategoryLayerIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> PrintAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                result = await _CategoryLocationService.PrintAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByCategoryDepartmentIDAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> PrintByCategoryDepartmentIDAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                result = await _CategoryLocationService.PrintByCategoryDepartmentIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintByParentIDAsync")]
        public virtual async Task<BaseResult<CategoryLocation>> PrintByParentIDAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                result = await _CategoryLocationService.PrintByParentIDAsync(BaseParameter);
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
        public override async Task<BaseResult<CategoryLocation>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<CategoryLocation>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<CategoryLocation>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null && BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0)
                {
                    BaseParameter.BaseModel = new CategoryLocation();
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string fileExtension = Path.GetExtension(file.FileName);
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "-" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                            string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                            bool isFolderExists = System.IO.Directory.Exists(folderPath);
                            if (!isFolderExists)
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
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
                                                CategoryLocation CategoryLocation = new CategoryLocation();
                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    CategoryLocation.Name = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                if (workSheet.Cells["B" + j].Value != null)
                                                {
                                                    CategoryLocation.ParentName = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                }
                                                //if (workSheet.Cells["C" + j].Value != null)
                                                //{
                                                //    CategoryLocation.CategoryLayerName = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                //}
                                                CategoryLocation.CompanyID = BaseParameter.CompanyID;
                                                CategoryLocation.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                                                CategoryLocation.Active = true;
                                                CategoryLocation.UpdateUserID = BaseParameter.UpdateUserID;                                                
                                                var BaseParameterCategoryLocation = new BaseParameter<CategoryLocation>();
                                                BaseParameterCategoryLocation.BaseModel = CategoryLocation;
                                                result = await _CategoryLocationService.SaveAsync(BaseParameterCategoryLocation);
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

