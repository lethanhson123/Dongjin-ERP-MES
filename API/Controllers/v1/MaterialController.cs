namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MaterialController : BaseController<Material, IMaterialService>
    {
        private readonly IMaterialService _MaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IMaterialRepository _MaterialRepository;
        public MaterialController(IMaterialService MaterialService
            , IWebHostEnvironment WebHostEnvironment
            , IMaterialRepository MaterialRepository
            ) : base(MaterialService, WebHostEnvironment)
        {
            _MaterialService = MaterialService;
            _WebHostEnvironment = WebHostEnvironment;
            _MaterialRepository = MaterialRepository;

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("MaterialCreateAutoAsync")]
        public virtual async Task<BaseResult<Material>> MaterialCreateAutoAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = new BaseParameter<Material>();
                result = await _MaterialService.CreateAutoAsync(BaseParameter);
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
        public virtual async Task<BaseResult<Material>> CreateAutoAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncParentChildAsync")]
        public virtual async Task<BaseResult<Material>> SyncParentChildAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.SyncParentChildAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseInputIDToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByWarehouseInputIDToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByWarehouseInputIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseOutputIDToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByWarehouseOutputIDToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByWarehouseOutputIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetByCategoryMaterialIDToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByCategoryMaterialIDToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByCategoryMaterialIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryMaterialID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByCategoryMaterialID_ActiveToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByCategoryMaterialID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByParentID_ActiveToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByParentID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("PrintAsync")]
        public virtual async Task<BaseResult<Material>> PrintAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.PrintAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryMaterialID_SearchStringToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_SearchStringToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByCompanyID_CategoryMaterialID_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_CategoryMaterialID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_ActiveToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetByCompanyID_CategoryMaterialID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetFromtmmtinByIDToListAsync")]
        public virtual async Task<BaseResult<Material>> GetFromtmmtinByIDToListAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.GetFromtmmtinByIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<Material>> ExportToExcelAsync()
        {
            var result = new BaseResult<Material>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
                result = await _MaterialService.ExportToExcelAsync(BaseParameter);
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
        public override async Task<BaseResult<Material>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<Material>();
            result.BaseModel = new Material();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
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
                                                var Material = new Material();

                                                if (workSheet.Cells["F" + j].Value != null)
                                                {
                                                    Material.Code = workSheet.Cells["F" + j].Value.ToString().Trim();
                                                }
                                                if (!string.IsNullOrEmpty(Material.Code))
                                                {
                                                    if (workSheet.Cells["A" + j].Value != null)
                                                    {
                                                        Material.OriginalEquipmentManufacturer = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        Material.CarMaker = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["C" + j].Value != null)
                                                    {
                                                        Material.CarType = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["D" + j].Value != null)
                                                    {
                                                        Material.Item = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["E" + j].Value != null)
                                                    {
                                                        Material.DevelopmentStage = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                    }
                                                    Material.UpdateUserID = BaseParameter.MembershipID;
                                                    Material.CompanyID = BaseParameter.CompanyID;
                                                    Material.CategoryMaterialID = 10;
                                                    var BaseParameterMaterial = new BaseParameter<Material>();
                                                    BaseParameterMaterial.BaseModel = Material;
                                                    result = await _MaterialService.SaveAsync(BaseParameterMaterial);
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
        [Route("SaveLocaltionAndUploadFileAsync")]
        public virtual async Task<BaseResult<Material>> SaveLocaltionAndUploadFileAsync()
        {
            var result = new BaseResult<Material>();
            result.BaseModel = new Material();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
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
                                            var ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                                            for (int j = 2; j <= totalRows; j++)
                                            {
                                                var Material = new Material();

                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    Material.Code = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                if (!string.IsNullOrEmpty(Material.Code))
                                                {
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        Material.CategoryFamilyName = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["C" + j].Value != null)
                                                    {
                                                        Material.CategoryLocationName = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(Material.CategoryLocationName))
                                                    {
                                                        var MaterialSave = ListMaterial.Where(o => o.Code == Material.Code).FirstOrDefault();
                                                        if (MaterialSave != null && MaterialSave.ID > 0)
                                                        {
                                                            MaterialSave.UpdateUserID = BaseParameter.MembershipID;
                                                            MaterialSave.CategoryFamilyName = Material.CategoryFamilyName;
                                                            MaterialSave.CategoryLocationName = Material.CategoryLocationName;
                                                            await _MaterialRepository.UpdateAsync(MaterialSave);
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
        [Route("SaveLineAndUploadFileAsync")]
        public virtual async Task<BaseResult<Material>> SaveLineAndUploadFileAsync()
        {
            var result = new BaseResult<Material>();
            result.BaseModel = new Material();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
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
                                                var Material = new Material();

                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    Material.Code = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                if (!string.IsNullOrEmpty(Material.Code))
                                                {
                                                    if (workSheet.Cells["B" + j].Value != null)
                                                    {
                                                        Material.CategoryLineName = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(Material.CategoryLineName))
                                                    {
                                                        Material.UpdateUserID = BaseParameter.MembershipID;
                                                        Material.CompanyID = BaseParameter.CompanyID;
                                                        Material.CategoryMaterialID = 9;
                                                        await _MaterialRepository.UpdateAsync(Material);
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
        [Route("SaveFamilyAndUploadFileAsync")]
        public virtual async Task<BaseResult<Material>> SaveFamilyAndUploadFileAsync()
        {
            var result = new BaseResult<Material>();
            result.BaseModel = new Material();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
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
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "-" + GlobalHelper.InitializationDateTimeCode0001 + "-" + file.FileName;
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
                                            var ListMaterial = new List<Material>();
                                            for (int j = 2; j <= totalRows; j++)
                                            {
                                                var Material = new Material();
                                                if (workSheet.Cells["C" + j].Value != null)
                                                {
                                                    try
                                                    {
                                                        Material.ID = long.Parse(workSheet.Cells["C" + j].Value.ToString().Trim());
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        string mes = ex.Message;
                                                    }
                                                }
                                                if (Material.ID > 0)
                                                {
                                                    if (workSheet.Cells["D" + j].Value != null)
                                                    {
                                                        Material.CategoryLineName = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["D" + j].Value != null)
                                                    {
                                                        Material.CategoryLineName = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["E" + j].Value != null)
                                                    {
                                                        Material.Code = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                    }
                                                    if (workSheet.Cells["F" + j].Value != null)
                                                    {
                                                        Material.CategoryFamilyName = workSheet.Cells["F" + j].Value.ToString().Trim();
                                                    }
                                                }
                                                ListMaterial.Add(Material);
                                            }
                                            var ListMaterialID = ListMaterial.Select(o => o.ID).Distinct().ToList();
                                            var ListMaterialDB = await _MaterialRepository.GetByCondition(o => ListMaterialID.Contains(o.ID)).ToListAsync();
                                            var ListMaterialDBID = ListMaterialDB.Select(o => o.ID).Distinct().ToList();
                                            for (int i = 0; i < ListMaterialDB.Count; i++)
                                            {
                                                var Material = ListMaterial.Where(o => o.ID == ListMaterialDB[i].ID).FirstOrDefault();
                                                if (Material != null && Material.ID > 0)
                                                {
                                                    ListMaterialDB[i].CompanyID = 16;
                                                    ListMaterialDB[i].CategoryLineName = Material.CategoryLineName;
                                                    ListMaterialDB[i].OriginalEquipmentManufacturer = Material.OriginalEquipmentManufacturer;
                                                    ListMaterialDB[i].CategoryFamilyName = Material.CategoryFamilyName;
                                                }
                                            }
                                            await _MaterialRepository.UpdateRangeAsync(ListMaterialDB);
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
        [Route("SaveLeadNoAndTerm1AndTerm2AndUploadFileAsync")]
        public virtual async Task<BaseResult<Material>> SaveLeadNoAndTerm1AndTerm2AndUploadFileAsync()
        {
            var result = new BaseResult<Material>();
            result.BaseModel = new Material();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Material>>(Request.Form["BaseParameter"]);
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
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "-" + GlobalHelper.InitializationDateTimeCode0001 + "-" + file.FileName;
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
                                        ExcelWorksheet workSheet1 = package.Workbook.Worksheets[1];
                                        if (workSheet1 != null)
                                        {
                                            int totalRows = workSheet1.Dimension.Rows;
                                            var ListMaterial = new List<Material>();
                                            for (int j = 2; j <= totalRows; j++)
                                            {
                                                var Material = new Material();
                                                if (workSheet1.Cells["B" + j].Value != null)
                                                {
                                                    var Term1 = workSheet1.Cells["B" + j].Value.ToString().Trim();
                                                    if (Term1.Contains("("))
                                                    {
                                                        if (workSheet1.Cells["A" + j].Value != null)
                                                        {
                                                            Material.Code = workSheet1.Cells["A" + j].Value.ToString().Trim();
                                                            ListMaterial.Add(Material);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (workSheet1.Cells["C" + j].Value != null)
                                                        {
                                                            var Term2 = workSheet1.Cells["C" + j].Value.ToString().Trim();
                                                            if (Term2.Contains("("))
                                                            {
                                                                if (workSheet1.Cells["A" + j].Value != null)
                                                                {
                                                                    Material.Code = workSheet1.Cells["A" + j].Value.ToString().Trim();
                                                                    ListMaterial.Add(Material);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            string fileName = @"DJM-LeadNo-Term1-Term2" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                                            var streamExport = new MemoryStream();
                                            using (var package1 = new ExcelPackage(streamExport))
                                            {
                                                var workSheet = package1.Workbook.Worksheets.Add("Sheet1");
                                                int row = 1;
                                                int column = 1;
                                                workSheet.Cells[row, column].Value = "PART NO";

                                                for (int i = 1; i <= column; i++)
                                                {
                                                    workSheet.Cells[1, i].Style.Font.Bold = true;
                                                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                                                    workSheet.Cells[1, i].Style.Font.Size = 16;
                                                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                                }
                                                row = row + 1;
                                                ListMaterial = ListMaterial.OrderBy(o => o.Code).ToList();
                                                foreach (Material item in ListMaterial)
                                                {
                                                    workSheet.Cells[row, 1].Value = item.Code;
                                                    row = row + 1;
                                                }
                                                for (int i = 1; i <= column; i++)
                                                {
                                                    workSheet.Column(i).AutoFit();
                                                }
                                                package1.Save();
                                            }
                                            streamExport.Position = 0;
                                            var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                                            using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                                            {
                                                streamExport.CopyTo(stream);
                                            }
                                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
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
        private void InitializationExcel(List<Material> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "PART NO";



                row = row + 1;
                foreach (Material item in list)
                {
                    workSheet.Cells[row, 1].Value = item.Code;
                    workSheet.Cells[row, 2].Value = item.CategoryLocationName;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 11;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    row = row + 1;
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;

        }
    }
}

