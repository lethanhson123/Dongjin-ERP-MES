namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MaterialUnitCovertController : BaseController<MaterialUnitCovert, IMaterialUnitCovertService>
    {
        private readonly IMaterialUnitCovertService _MaterialUnitCovertService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MaterialUnitCovertController(IMaterialUnitCovertService MaterialUnitCovertService, IWebHostEnvironment WebHostEnvironment) : base(MaterialUnitCovertService, WebHostEnvironment)
        {
            _MaterialUnitCovertService = MaterialUnitCovertService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<MaterialUnitCovert>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<MaterialUnitCovert>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MaterialUnitCovert>>(Request.Form["BaseParameter"]);
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
                                                var MaterialUnitCovert = new MaterialUnitCovert();

                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    MaterialUnitCovert.ParentName = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                if (!string.IsNullOrEmpty(MaterialUnitCovert.ParentName))
                                                {
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            MaterialUnitCovert.Quantity01 = decimal.Parse(workSheet.Cells["B" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string mes = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["C" + j].Value != null)
                                                    {
                                                        MaterialUnitCovert.CategoryUnitName01 = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["D" + j].Value != null)
                                                    {
                                                        try
                                                        {
                                                            MaterialUnitCovert.Quantity02 = decimal.Parse(workSheet.Cells["D" + j].Value.ToString().Trim());
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string mes = ex.Message;
                                                        }
                                                    }
                                                    if (workSheet.Cells["E" + j].Value != null)
                                                    {
                                                        MaterialUnitCovert.CategoryUnitName02 = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                    }
                                                    MaterialUnitCovert.Active = true;
                                                    MaterialUnitCovert.UpdateUserID = BaseParameter.MembershipID;
                                                    MaterialUnitCovert.CompanyID = BaseParameter.CompanyID;
                                                    var BaseParameterMaterialUnitCovert = new BaseParameter<MaterialUnitCovert>();
                                                    BaseParameterMaterialUnitCovert.BaseModel = MaterialUnitCovert;
                                                    result = await _MaterialUnitCovertService.SaveAsync(BaseParameterMaterialUnitCovert);
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