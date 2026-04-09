namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMController : BaseController<BOM, IBOMService>
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IBOMService _BOMService;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailService _BOMDetailService;
        private readonly IBOMTermService _BOMTermService;
        private readonly IBOMFileService _BOMFileService;

        public BOMController(IBOMService BOMService
            , IBOMRepository BOMRepository
            , IWebHostEnvironment WebHostEnvironment
            , IBOMDetailService BOMDetailService
            , IBOMTermService BOMTermService
            , IBOMFileService BOMFileService

            ) : base(BOMService, WebHostEnvironment)
        {
            _BOMService = BOMService;
            _BOMRepository = BOMRepository;
            _WebHostEnvironment = WebHostEnvironment;
            _BOMDetailService = BOMDetailService;
            _BOMTermService = BOMTermService;
            _BOMFileService = BOMFileService;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("BOMCreateAutoAsync")]
        public virtual async Task<BaseResult<BOM>> BOMCreateAutoAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = new BaseParameter<BOM>();
                result = await _BOMService.CreateAutoAsync(BaseParameter);
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
        public override async Task<BaseResult<BOM>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.CompanyID > 0)
                    {
                        result = await _BOMService.SaveAsync(BaseParameter);
                        BaseParameter.BaseModel = result.BaseModel;

                        var BaseParameterBOMFile = new BaseParameter<BOMFile>();
                        BaseParameterBOMFile.BaseModel = new BOMFile();
                        BaseParameterBOMFile.BaseModel.ParentID = BaseParameter.BaseModel.ID;
                        BaseParameterBOMFile.BaseModel.Code = BaseParameter.BaseModel.Code;

                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            if (file == null || file.Length == 0)
                            {
                            }
                            if (file != null)
                            {
                                BaseParameterBOMFile.BaseModel.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                BaseParameterBOMFile.BaseModel.ParentID = BaseParameter.BaseModel.ID;
                                BaseParameterBOMFile.BaseModel.Code = BaseParameter.BaseModel.Code;
                                BaseParameterBOMFile.BaseModel.Display = Path.GetExtension(file.FileName);
                                BaseParameterBOMFile.BaseModel.FileName = BaseParameter.BaseModel.ID + "_" + BaseParameter.BaseModel.GetType().Name + "_" + GlobalHelper.InitializationDateTimeCode0001 + "_" + BaseParameterBOMFile.BaseModel.Display;

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

                                    BaseParameterBOMFile.BaseModel.Description = physicalPath;
                                    BaseParameterBOMFile.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameterBOMFile.BaseModel.GetType().Name + "/" + BaseParameterBOMFile.BaseModel.FileName;
                                    await _BOMFileService.SaveAsync(BaseParameterBOMFile);
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
                                                    var BOM = new BOM();

                                                    if (workSheet.Cells["C" + j].Value != null)
                                                    {
                                                        BOM.MaterialCode = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(BOM.MaterialCode))
                                                    {
                                                        if (workSheet.Cells["D" + j].Value != null)
                                                        {
                                                            BOM.Code = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(BOM.Code))
                                                        {
                                                            if (workSheet.Cells["A" + j].Value != null)
                                                            {
                                                                BOM.Description = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells["B" + j].Value != null)
                                                            {
                                                                BOM.Note = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                            }
                                                            BOM.Active = true;
                                                            BOM.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                                            BOM.CompanyID = BaseParameter.BaseModel.CompanyID;
                                                            var BaseParameterBOM = new BaseParameter<BOM>();
                                                            BaseParameterBOM.BaseModel = BOM;
                                                            await _BOMService.SaveAsync(BaseParameterBOM);
                                                            if (BaseParameterBOM.BaseModel.ID > 0)
                                                            {
                                                                //result.BaseModel = BaseParameterBOM.BaseModel;
                                                                var BOMDetail = new BOMDetail();
                                                                if (workSheet.Cells["E" + j].Value != null)
                                                                {
                                                                    BOMDetail.MaterialCode02 = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                                }
                                                                if (!string.IsNullOrEmpty(BOMDetail.MaterialCode02))
                                                                {
                                                                    if (workSheet.Cells["F" + j].Value != null)
                                                                    {
                                                                        BOMDetail.Description = workSheet.Cells["F" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells["G" + j].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            BOMDetail.Quantity02 = decimal.Parse(workSheet.Cells["G" + j].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string mes = ex.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells["H" + j].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            BOMDetail.QuantityActual = decimal.Parse(workSheet.Cells["H" + j].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string mes = ex.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells["I" + j].Value != null)
                                                                    {
                                                                        BOMDetail.CategoryUnitName02 = workSheet.Cells["I" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells["J" + j].Value != null)
                                                                    {
                                                                        BOMDetail.Display = workSheet.Cells["J" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells["K" + j].Value != null)
                                                                    {
                                                                        BOMDetail.Code = workSheet.Cells["K" + j].Value.ToString().Trim();
                                                                    }
                                                                    BOMDetail.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                                                    BOMDetail.Active = true;
                                                                    BOMDetail.ParentID = BaseParameterBOM.BaseModel.ID;
                                                                    var BaseParameterBOMDetail = new BaseParameter<BOMDetail>();
                                                                    BaseParameterBOMDetail.BaseModel = BOMDetail;
                                                                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetail);
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
        [Route("SaveListAndUploadFileAsync")]
        public virtual async Task<BaseResult<BOM>> SaveListAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
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
                                                    var BOM = new BOM();
                                                    if (workSheet.Cells["A" + j].Value != null)
                                                    {
                                                        BOM.Code = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(BOM.Code))
                                                    {
                                                        if (workSheet.Cells["B" + j].Value != null)
                                                        {
                                                            BOM.MaterialCode = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                        }
                                                        BOM.Active = true;
                                                        BOM.UpdateUserID = BaseParameter.MembershipID;
                                                        BOM.CompanyID = BaseParameter.CompanyID;
                                                        var BaseParameterBOM = new BaseParameter<BOM>();
                                                        BaseParameterBOM.BaseModel = BOM;
                                                        result = await _BOMService.SaveAsync(BaseParameterBOM);
                                                        if (BaseParameterBOM.BaseModel.ID > 0)
                                                        {
                                                            var BOMSub = new BOM();
                                                            if (workSheet.Cells["C" + j].Value != null)
                                                            {
                                                                BOMSub.MaterialCode = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                            }
                                                            if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                                                            {
                                                                BOMSub.Code = BOM.Code;
                                                                BOMSub.Active = true;
                                                                BOMSub.UpdateUserID = BaseParameter.MembershipID;
                                                                BOMSub.CompanyID = BaseParameter.CompanyID;
                                                                if (BaseParameterBOM.BaseModel.ParentID == null)
                                                                {
                                                                    BOMSub.ParentID = BaseParameterBOM.BaseModel.ID;
                                                                }
                                                                if (BaseParameterBOM.BaseModel.ParentID > 0)
                                                                {
                                                                    BOMSub.ParentID = BaseParameterBOM.BaseModel.ParentID;
                                                                    BOMSub.ParentID01 = BaseParameterBOM.BaseModel.ID;
                                                                }
                                                                var BaseParameterBOMSub = new BaseParameter<BOM>();
                                                                BaseParameterBOMSub.BaseModel = BOMSub;
                                                                await _BOMService.SaveAsync(BaseParameterBOMSub);
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
        [Route("SaveOnlyLeadNoAndUploadFileAsync")]
        public virtual async Task<BaseResult<BOM>> SaveOnlyLeadNoAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
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
                                                var ListBOM = await _BOMService.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                                                int totalRows = workSheet.Dimension.Rows;
                                                for (int j = 2; j <= totalRows; j++)
                                                {
                                                    var BOM = new BOM();
                                                    if (workSheet.Cells["A" + j].Value != null)
                                                    {
                                                        BOM.LeadNo = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(BOM.LeadNo))
                                                    {
                                                        BOM.LeadNo = BOM.LeadNo.Trim();
                                                        var ListBOMSub = ListBOM.Where(o => o.MaterialCode == BOM.LeadNo).ToList();
                                                        foreach (var BOMSave in ListBOMSub)
                                                        {
                                                            BOMSave.LeadNo = BOM.LeadNo;
                                                            BOMSave.IsLeadNo = true;
                                                            await _BOMRepository.UpdateAsync(BOMSave);
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
        [Route("SaveByLEADAndUploadFileAsync")]
        public virtual async Task<BaseResult<BOM>> SaveByLEADAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
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
                                                string ECN = GlobalHelper.InitializationString;
                                                if (workSheet.Cells[1, 44].Value != null)
                                                {
                                                    ECN = workSheet.Cells[1, 44].Value.ToString().Trim();
                                                }
                                                string Version = GlobalHelper.InitializationString;
                                                if (workSheet.Cells[1, 45].Value != null)
                                                {
                                                    Version = workSheet.Cells[1, 45].Value.ToString().Trim();
                                                }
                                                int CollumnIndex = 46;
                                                for (int i = CollumnIndex; i < 100; i++)
                                                {
                                                    BOM BOM = new BOM();
                                                    if (workSheet.Cells[1, i].Value != null)
                                                    {
                                                        BOM.MaterialCode = workSheet.Cells[1, i].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(BOM.MaterialCode))
                                                    {
                                                        BOM.Code = ECN;
                                                        BOM.Active = true;
                                                        BOM.UpdateUserID = BaseParameter.MembershipID;
                                                        BOM.CompanyID = BaseParameter.CompanyID;
                                                        var BaseParameterBOM = new BaseParameter<BOM>();
                                                        BaseParameterBOM.BaseModel = BOM;
                                                        await _BOMService.SaveAsync(BaseParameterBOM);
                                                        if (BOM.ID > 0)
                                                        {
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                BOM BOMSub01 = new BOM();
                                                                if (workSheet.Cells["D" + j].Value != null)
                                                                {
                                                                    BOMSub01.MaterialCode = workSheet.Cells["D" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells[j, i].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        BOMSub01.Quantity = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string mes = ex.Message;
                                                                    }
                                                                }
                                                                if (!string.IsNullOrEmpty(BOMSub01.MaterialCode) && BOMSub01.Quantity > 0)
                                                                {
                                                                    await SetParametersAsync(workSheet, j, i, BaseParameter.MembershipID, BaseParameter.CompanyID, BOM.ID, BOMSub01);

                                                                    BOM BOMSub02 = new BOM();
                                                                    if (workSheet.Cells["E" + j].Value != null)
                                                                    {
                                                                        BOMSub02.MaterialCode = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, i].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            BOMSub02.Quantity = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string mes = ex.Message;
                                                                        }
                                                                    }
                                                                    if (!string.IsNullOrEmpty(BOMSub02.MaterialCode) && BOMSub02.Quantity > 0)
                                                                    {
                                                                        BOMSub02.ParentID01 = BOMSub01.ID;
                                                                        await SetParametersAsync(workSheet, j, i, BaseParameter.MembershipID, BaseParameter.CompanyID, BOM.ID, BOMSub02);
                                                                    }

                                                                    BOM BOMSub03 = new BOM();
                                                                    if (workSheet.Cells["F" + j].Value != null)
                                                                    {
                                                                        BOMSub03.MaterialCode = workSheet.Cells["F" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, i].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            BOMSub03.Quantity = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string mes = ex.Message;
                                                                        }
                                                                    }
                                                                    if (!string.IsNullOrEmpty(BOMSub03.MaterialCode) && BOMSub03.Quantity > 0)
                                                                    {
                                                                        BOMSub03.ParentID01 = BOMSub01.ID;
                                                                        BOMSub03.ParentID02 = BOMSub02.ID;
                                                                        await SetParametersAsync(workSheet, j, i, BaseParameter.MembershipID, BaseParameter.CompanyID, BOM.ID, BOMSub03);
                                                                    }

                                                                    BOM BOMSub04 = new BOM();
                                                                    if (workSheet.Cells["G" + j].Value != null)
                                                                    {
                                                                        BOMSub04.MaterialCode = workSheet.Cells["G" + j].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, i].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            BOMSub04.Quantity = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string mes = ex.Message;
                                                                        }
                                                                    }
                                                                    if (!string.IsNullOrEmpty(BOMSub04.MaterialCode) && BOMSub04.Quantity > 0)
                                                                    {
                                                                        BOMSub04.ParentID01 = BOMSub01.ID;
                                                                        BOMSub04.ParentID02 = BOMSub02.ID;
                                                                        BOMSub04.ParentID03 = BOMSub03.ID;
                                                                        await SetParametersAsync(workSheet, j, i, BaseParameter.MembershipID, BaseParameter.CompanyID, BOM.ID, BOMSub04);
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
        private async Task<int> SetParametersAsync(ExcelWorksheet workSheet, int j, int i, long? MembershipID, long? CompanyID, long ID, BOM BOMSub)
        {
            int result = 0;
            if (workSheet.Cells["A" + j].Value != null)
            {
                BOMSub.Project = workSheet.Cells["A" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["B" + j].Value != null)
            {
                BOMSub.Item = workSheet.Cells["B" + j].Value.ToString().Trim();
            }
            //if (workSheet.Cells["C" + j].Value != null)
            //{
            //    BOMSub01.Group = workSheet.Cells["C" + j].Value.ToString().Trim();
            //}
            if (workSheet.Cells["C" + j].Value != null)
            {
                BOMSub.Stage = workSheet.Cells["C" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["H" + j].Value != null)
            {
                BOMSub.Display = workSheet.Cells["H" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["J" + j].Value != null)
            {
                BOMSub.LeadNo = workSheet.Cells["J" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["K" + j].Value != null)
            {
                BOMSub.Combination = workSheet.Cells["K" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["L" + j].Value != null)
            {
                BOMSub.DirT1 = workSheet.Cells["L" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["M" + j].Value != null)
            {
                BOMSub.NoT1 = workSheet.Cells["M" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["N" + j].Value != null)
            {
                BOMSub.AutoT1 = workSheet.Cells["N" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["O" + j].Value != null)
            {
                BOMSub.DirT2 = workSheet.Cells["O" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["P" + j].Value != null)
            {
                BOMSub.NoT2 = workSheet.Cells["P" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["Q" + j].Value != null)
            {
                BOMSub.AutoT2 = workSheet.Cells["Q" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells["R" + j].Value != null)
            {
                BOMSub.WLink = workSheet.Cells["R" + j].Value.ToString().Trim();
            }
            if (workSheet.Cells[j, i].Value != null)
            {
                try
                {
                    BOMSub.Quantity = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            if (workSheet.Cells["I" + j].Value != null)
            {
                try
                {
                    BOMSub.BundleSize = int.Parse(workSheet.Cells["I" + j].Value.ToString().Trim());
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            if (workSheet.Cells["AA" + j].Value != null)
            {
                try
                {
                    BOMSub.Strip1 = decimal.Parse(workSheet.Cells["AA" + j].Value.ToString().Trim());
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            if (workSheet.Cells["AQ" + j].Value != null)
            {
                try
                {
                    BOMSub.Strip2 = decimal.Parse(workSheet.Cells["AQ" + j].Value.ToString().Trim());
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            BOMSub.Active = true;
            BOMSub.UpdateUserID = MembershipID;
            BOMSub.CompanyID = CompanyID;
            BOMSub.ParentID = ID;
            var BaseParameterBOMSub01 = new BaseParameter<BOM>();
            BaseParameterBOMSub01.BaseModel = BOMSub;
            await _BOMService.SaveAsync(BaseParameterBOMSub01);
            if (BOMSub.ID > 0)
            {
                BOMDetail BOMDetailSS1 = new BOMDetail();
                if (workSheet.Cells["Y" + j].Value != null)
                {
                    BOMDetailSS1.MaterialCode02 = workSheet.Cells["Y" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailSS1.MaterialCode02))
                {
                    BOMDetailSS1.ParentID = BOMSub.ID;
                    BOMDetailSS1.Active = true;
                    BOMDetailSS1.Note = GlobalHelper.SS1;
                    var BaseParameterBOMDetailSS1 = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailSS1.BaseModel = BOMDetailSS1;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailSS1);
                }
                BOMDetail BOMDetailTube1 = new BOMDetail();
                if (workSheet.Cells["Z" + j].Value != null)
                {
                    BOMDetailTube1.MaterialCode02 = workSheet.Cells["Z" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailTube1.MaterialCode02))
                {
                    BOMDetailTube1.ParentID = BOMSub.ID;
                    BOMDetailTube1.Active = true;
                    BOMDetailTube1.Note = GlobalHelper.Tube1;
                    var BaseParameterBOMDetailTube1 = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailTube1.BaseModel = BOMDetailTube1;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailTube1);
                }

                BOMDetail BOMDetailSS2 = new BOMDetail();
                if (workSheet.Cells["AN" + j].Value != null)
                {
                    BOMDetailSS2.MaterialCode02 = workSheet.Cells["AN" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailSS2.MaterialCode02))
                {
                    BOMDetailSS2.ParentID = BOMSub.ID;
                    BOMDetailSS2.Active = true;
                    BOMDetailSS2.Note = GlobalHelper.SS2;
                    var BaseParameterBOMDetailSS2 = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailSS2.BaseModel = BOMDetailSS2;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailSS2);
                }
                BOMDetail BOMDetailTube2 = new BOMDetail();
                if (workSheet.Cells["AO" + j].Value != null)
                {
                    BOMDetailTube2.MaterialCode02 = workSheet.Cells["AO" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailTube2.MaterialCode02))
                {
                    BOMDetailTube2.ParentID = BOMSub.ID;
                    BOMDetailTube2.Active = true;
                    BOMDetailTube2.Note = GlobalHelper.Tube2;
                    var BaseParameterBOMDetailTube2 = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailTube2.BaseModel = BOMDetailTube2;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailTube2);
                }
                BOMDetail BOMDetailTape = new BOMDetail();
                if (workSheet.Cells["AP" + j].Value != null)
                {
                    BOMDetailTape.MaterialCode02 = workSheet.Cells["AP" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailTape.MaterialCode02))
                {
                    BOMDetailTape.ParentID = BOMSub.ID;
                    BOMDetailTape.Active = true;
                    BOMDetailTape.Note = GlobalHelper.Tape;
                    var BaseParameterBOMDetailTape = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailTape.BaseModel = BOMDetailTape;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailTape);
                }

                BOMDetail BOMDetailWire = new BOMDetail();
                if (workSheet.Cells["AB" + j].Value != null)
                {
                    BOMDetailWire.MaterialCode02 = workSheet.Cells["AB" + j].Value.ToString().Trim();
                }
                if (!string.IsNullOrEmpty(BOMDetailWire.MaterialCode02))
                {
                    if (workSheet.Cells["AC" + j].Value != null)
                    {
                        BOMDetailWire.WRNo = workSheet.Cells["AC" + j].Value.ToString().Trim();
                    }
                    if (workSheet.Cells["AD" + j].Value != null)
                    {
                        BOMDetailWire.Wire = workSheet.Cells["AD" + j].Value.ToString().Trim();
                    }
                    if (workSheet.Cells["AE" + j].Value != null)
                    {
                        try
                        {
                            BOMDetailWire.Diameter = decimal.Parse(workSheet.Cells["AE" + j].Value.ToString().Trim());
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                    }
                    if (workSheet.Cells["AF" + j].Value != null)
                    {
                        BOMDetailWire.Color = workSheet.Cells["AF" + j].Value.ToString().Trim();
                    }
                    if (workSheet.Cells["AG" + j].Value != null)
                    {
                        try
                        {
                            BOMDetailWire.Quantity02 = decimal.Parse(workSheet.Cells["AG" + j].Value.ToString().Trim());
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                    }
                    BOMDetailWire.ParentID = BOMSub.ID;
                    BOMDetailWire.Active = true;
                    BOMDetailWire.Note = GlobalHelper.WIRE;
                    var BaseParameterBOMDetailWire = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetailWire.BaseModel = BOMDetailWire;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetailWire);
                }

                BOMTerm BOMTerm1 = new BOMTerm();
                if (workSheet.Cells["S" + j].Value != null)
                {
                    BOMTerm1.Code = workSheet.Cells["S" + j].Value.ToString().Trim();
                }
                if (workSheet.Cells["U" + j].Value != null)
                {
                    try
                    {
                        BOMTerm1.CCH1 = decimal.Parse(workSheet.Cells["U" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["V" + j].Value != null)
                {
                    try
                    {
                        BOMTerm1.CCW1 = decimal.Parse(workSheet.Cells["V" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["W" + j].Value != null)
                {
                    try
                    {
                        BOMTerm1.ICH1 = decimal.Parse(workSheet.Cells["W" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["X" + j].Value != null)
                {
                    try
                    {
                        BOMTerm1.ICW1 = decimal.Parse(workSheet.Cells["X" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                BOMTerm1.ParentID = BOMSub.ID;
                BOMTerm1.Active = true;
                var BaseParameterBOMTerm1 = new BaseParameter<BOMTerm>();
                BaseParameterBOMTerm1.BaseModel = BOMTerm1;
                await _BOMTermService.SaveAsync(BaseParameterBOMTerm1);

                BOMTerm BOMTerm2 = new BOMTerm();
                if (workSheet.Cells["AH" + j].Value != null)
                {
                    BOMTerm2.Code = workSheet.Cells["AH" + j].Value.ToString().Trim();
                }
                if (workSheet.Cells["AJ" + j].Value != null)
                {
                    try
                    {
                        BOMTerm2.CCH2 = decimal.Parse(workSheet.Cells["AJ" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["AK" + j].Value != null)
                {
                    try
                    {
                        BOMTerm2.CCW2 = decimal.Parse(workSheet.Cells["AK" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["AL" + j].Value != null)
                {
                    try
                    {
                        BOMTerm2.ICH2 = decimal.Parse(workSheet.Cells["AL" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                if (workSheet.Cells["AM" + j].Value != null)
                {
                    try
                    {
                        BOMTerm2.ICW2 = decimal.Parse(workSheet.Cells["AM" + j].Value.ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                BOMTerm2.ParentID = BOMSub.ID;
                BOMTerm2.Active = true;
                var BaseParameterBOMTerm2 = new BaseParameter<BOMTerm>();
                BaseParameterBOMTerm2.BaseModel = BOMTerm2;
                await _BOMTermService.SaveAsync(BaseParameterBOMTerm2);
            }
            return result;
        }

        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveByBOMRawMaterialAndUploadFileAsync")]
        public virtual async Task<BaseResult<BOM>> SaveByBOMRawMaterialAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
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
                                                for (int i = 3; i < 100; i++)
                                                {
                                                    BOM BOM = new BOM();
                                                    if (workSheet.Cells[1, i].Value != null)
                                                    {
                                                        BOM.MaterialCode = workSheet.Cells[1, i].Value.ToString().Trim();
                                                    }
                                                    if (!string.IsNullOrEmpty(BOM.MaterialCode))
                                                    {
                                                        BOM.Active = true;
                                                        BOM.UpdateUserID = BaseParameter.MembershipID;
                                                        BOM.CompanyID = BaseParameter.CompanyID;
                                                        var BaseParameterBOM = new BaseParameter<BOM>();
                                                        BaseParameterBOM.BaseModel = BOM;
                                                        await _BOMService.SaveAsync(BaseParameterBOM);
                                                        if (BOM.ID > 0)
                                                        {
                                                            for (int j = 2; j < totalRows; j++)
                                                            {
                                                                BOMDetail BOMDetail = new BOMDetail();
                                                                if (workSheet.Cells[j, i].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        BOMDetail.Quantity02 = decimal.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                        if (BOMDetail.Quantity02 > 0)
                                                                        {
                                                                            BOMDetail.MaterialCode02 = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                            if (!string.IsNullOrEmpty(BOMDetail.MaterialCode02))
                                                                            {
                                                                                BOMDetail.CategoryUnitName02 = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                                BOMDetail.Active = true;
                                                                                BOMDetail.ParentID = BOM.ID;
                                                                                var BaseParameterBOMDetail = new BaseParameter<BOMDetail>();
                                                                                BaseParameterBOMDetail.BaseModel = BOMDetail;
                                                                                await _BOMDetailService.SaveAsync(BaseParameterBOMDetail);
                                                                            }
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string mes = ex.Message;
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
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveByHookRackAndUploadFileAsync")]
        public virtual async Task<BaseResult<BOM>> SaveByHookRackAndUploadFileAsync()
        {
            var result = new BaseResult<BOM>();
            result.BaseModel = new BOM();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
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
                                            List<trackmtim> Listtrackmtim = new List<trackmtim>();
                                            for (int i = 2; i < totalRows; i++)
                                            {
                                                var trackmtim = new trackmtim();
                                                if (workSheet.Cells["A" + i].Value != null)
                                                {
                                                    trackmtim.BARCODE_NM = workSheet.Cells["A" + i].Value.ToString().Trim();
                                                }
                                                Listtrackmtim.Add(trackmtim);
                                            }
                                            if (Listtrackmtim.Count > 0)
                                            {
                                                var ListtrackmtimBARCODE_NM = Listtrackmtim.Select(o => o.BARCODE_NM).ToList();
                                                var Parameter = string.Join("','", ListtrackmtimBARCODE_NM);
                                                Parameter = "'" + Parameter + "'";
                                                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(16);
                                                List<trackmtim> ListtrackmtimF1 = new List<trackmtim>();
                                                string sql = @"select * from trackmtim WHERE BARCODE_NM in (" + Parameter + ")";
                                                DataSet ds = await HelperMySQL.MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                                for (int i = 0; i < ds.Tables.Count; i++)
                                                {
                                                    DataTable dt = ds.Tables[i];
                                                    ListtrackmtimF1.AddRange(SQLHelper.ToList<trackmtim>(dt));
                                                }

                                                var ListtrackmtimLEAD_NM = ListtrackmtimF1.Select(o => o.LEAD_NM).ToList();
                                                Parameter = string.Join("','", ListtrackmtimLEAD_NM);
                                                Parameter = "'" + Parameter + "'";
                                                var DateRACKDTM = new DateTime(2025, 10, 24);
                                                var RACKDTM = DateRACKDTM.ToString("yyyy-MM-dd 23:59:59");
                                                List<trackmtim> ListtrackmtimF11 = new List<trackmtim>();
                                                sql = @"select * from trackmtim WHERE LEAD_NM in (" + Parameter + ") AND RACKCODE not in ('OUTPUT') AND RACKDTM <= '" + RACKDTM + "'";
                                                ds = await HelperMySQL.MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                                                for (int i = 0; i < ds.Tables.Count; i++)
                                                {
                                                    DataTable dt = ds.Tables[i];
                                                    ListtrackmtimF11.AddRange(SQLHelper.ToList<trackmtim>(dt));
                                                }

                                                var ListtrackmtimTRACK_IDX = ListtrackmtimF11.Select(o => o.TRACK_IDX).ToList();
                                                Parameter = string.Join(",", ListtrackmtimTRACK_IDX);
                                                sql = @"DELETE FROM trackmtim WHERE TRACK_IDX in (" + Parameter + ")";
                                                string sqlResult = await HelperMySQL.MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);

                                                //foreach (var trackmtim in ListtrackmtimF11)
                                                //{
                                                //    sql = @"DELETE FROM trackmtim WHERE TRACK_IDX='" + trackmtim.TRACK_IDX + "'";
                                                //    string sqlResult = await HelperMySQL.MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                                                //}


                                                //var DateRACKDTM = new DateTime(2026, 4, 10);
                                                //var RACKDTM = DateRACKDTM.ToString("yyyy-MM-dd HH:mm:ss");
                                                //foreach (var trackmtim in ListtrackmtimF1)
                                                //{
                                                //    sql = @"UPDATE trackmtim SET RACKDTM = '" + RACKDTM + "' WHERE BARCODE_NM='" + trackmtim.BARCODE_NM + "'";
                                                //    string sqlResult = await HelperMySQL.MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                                                //}
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
        public virtual async Task<BaseResult<BOM>> CreateAutoAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCode_MaterialCodeToListAsync")]
        public virtual async Task<BaseResult<BOM>> GetByCode_MaterialCodeToListAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.GetByCode_MaterialCodeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_Code_MaterialCodeToListAsync")]
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_Code_MaterialCodeToListAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.GetByCompanyID_Code_MaterialCodeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_SearchStringToListAsync")]
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_SearchStringToListAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.GetByCompanyID_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_PageAndPageSizeToListAsync")]
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_PageAndPageSizeToListAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.GetByCompanyID_PageAndPageSizeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportBOMLeadByIDToExcelAsync")]
        public virtual async Task<BaseResult<BOM>> ExportBOMLeadByIDToExcelAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.ExportBOMLeadByIDToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportBOMLeadByECNToExcelAsync")]
        public virtual async Task<BaseResult<BOM>> ExportBOMLeadByECNToExcelAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.ExportBOMLeadByECNToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync")]
        public virtual async Task<BaseResult<BOM>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncFinishGoodsListOftrackmtimAsync")]
        public virtual async Task<BaseResult<BOM>> SyncFinishGoodsListOftrackmtimAsync()
        {
            var result = new BaseResult<BOM>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOM>>(Request.Form["BaseParameter"]);
                result = await _BOMService.SyncFinishGoodsListOftrackmtimAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

