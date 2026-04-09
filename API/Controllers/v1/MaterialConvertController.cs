namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MaterialConvertController : BaseController<MaterialConvert, IMaterialConvertService>
    {
        private readonly IMaterialConvertService _MaterialConvertService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MaterialConvertController(IMaterialConvertService MaterialConvertService, IWebHostEnvironment WebHostEnvironment) : base(MaterialConvertService, WebHostEnvironment)
        {
            _MaterialConvertService = MaterialConvertService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<MaterialConvert>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<MaterialConvert>();
            result.BaseModel = new MaterialConvert();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MaterialConvert>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null && BaseParameter.CompanyID > 0)
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
                                                var MaterialConvert = new MaterialConvert();

                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    MaterialConvert.ParentName = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                if (!string.IsNullOrEmpty(MaterialConvert.ParentName))
                                                {
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        MaterialConvert.Code = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(MaterialConvert.Code))
                                                    {
                                                        if (workSheet.Cells["C" + j].Value != null)
                                                        {
                                                            MaterialConvert.Description = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                        }
                                                        MaterialConvert.Active = true;
                                                        MaterialConvert.UpdateUserID = BaseParameter.MembershipID;
                                                        MaterialConvert.CompanyID = BaseParameter.CompanyID;
                                                        var BaseParameterMaterialConvert = new BaseParameter<MaterialConvert>();
                                                        BaseParameterMaterialConvert.BaseModel = MaterialConvert;
                                                        result = await _MaterialConvertService.SaveAsync(BaseParameterMaterialConvert);
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
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<MaterialConvert>> CreateAutoAsync()
        {
            var result = new BaseResult<MaterialConvert>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MaterialConvert>>(Request.Form["BaseParameter"]);
                result = await _MaterialConvertService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

