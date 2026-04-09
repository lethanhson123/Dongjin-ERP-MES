namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InvoiceInputController : BaseController<InvoiceInput, IInvoiceInputService>
    {
        private readonly IInvoiceInputService _InvoiceInputService;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IInvoiceInputFileService _InvoiceInputFileService;
        private readonly IInvoiceInputDetailService _InvoiceInputDetailService;
        public InvoiceInputController(IInvoiceInputService InvoiceInputService
            , IWebHostEnvironment WebHostEnvironment
            , IInvoiceInputFileService InvoiceInputFileService
            , IInvoiceInputDetailService InvoiceInputDetailService
            ) : base(InvoiceInputService, WebHostEnvironment)
        {
            _InvoiceInputService = InvoiceInputService;
            _WebHostEnvironment = WebHostEnvironment;
            _InvoiceInputFileService = InvoiceInputFileService;
            _InvoiceInputDetailService = InvoiceInputDetailService;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFilesAsync")]
        public override async Task<BaseResult<InvoiceInput>> SaveAndUploadFilesAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.SaveAsync(BaseParameter);
                if (result != null && result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    if (Request.Form.Files.Count > 0)
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
                                string fileName = controllerName + "_" + result.BaseModel.ID+ "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                    var InvoiceInputFile = new InvoiceInputFile();
                                    InvoiceInputFile.Code = result.BaseModel.Code;
                                    InvoiceInputFile.ParentID = result.BaseModel.ID;
                                    InvoiceInputFile.Description = physicalPath;
                                    InvoiceInputFile.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + controllerName + "/" + fileName;
                                    var BaseParameterInvoiceInputFile = new BaseParameter<InvoiceInputFile>();
                                    BaseParameterInvoiceInputFile.BaseModel = InvoiceInputFile;
                                    await _InvoiceInputFileService.SaveAsync(BaseParameterInvoiceInputFile);
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
                                                if (result.BaseModel != null)
                                                {
                                                    if (result.BaseModel.ID > 0)
                                                    {
                                                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                        if (workSheet != null)
                                                        {
                                                            int totalRows = workSheet.Dimension.Rows;
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                InvoiceInputDetail InvoiceInputDetail = new InvoiceInputDetail();
                                                                if (workSheet.Cells[j, 2].Value != null)
                                                                {
                                                                    InvoiceInputDetail.MaterialName = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                }
                                                                if (!string.IsNullOrEmpty(InvoiceInputDetail.MaterialName))
                                                                {
                                                                    if (workSheet.Cells[j, 1].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.Display = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 6].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.CategoryUnitName = workSheet.Cells[j, 6].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 3].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.Description = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 9].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.Note = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 4].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            InvoiceInputDetail.QuantitySNP = int.Parse(workSheet.Cells[j, 4].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 5].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            var QUANTITY = double.Parse(workSheet.Cells[j, 5].Value.ToString().Trim());
                                                                            var QUANTITYString = workSheet.Cells[j, 5].Value.ToString().Trim();
                                                                            QUANTITYString = QUANTITYString.Split(',')[0];
                                                                            QUANTITYString = QUANTITYString.Split('.')[0];
                                                                            var Part = double.Parse(QUANTITYString);
                                                                            var Remainder = QUANTITY - Part;
                                                                            if (Remainder >= 0.5)
                                                                            {
                                                                                Part = Part + 1;
                                                                            }
                                                                            InvoiceInputDetail.Quantity = (decimal)Part;
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 7].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            InvoiceInputDetail.Price = decimal.Parse(workSheet.Cells[j, 7].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 8].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            InvoiceInputDetail.Total = decimal.Parse(workSheet.Cells[j, 8].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    InvoiceInputDetail.ParentID = result.BaseModel.ID;
                                                                    await _InvoiceInputDetailService.SaveAsync(new BaseParameter<InvoiceInputDetail>
                                                                    {
                                                                        BaseModel = InvoiceInputDetail
                                                                    });
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
        [Route("SaveAndUploadFiles001Async")]
        public virtual async Task<BaseResult<InvoiceInput>> SaveAndUploadFiles001Async()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.SaveAsync(BaseParameter);
                if (result != null && result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        var BaseParameterInvoiceInputFile = new BaseParameter<InvoiceInputFile>();
                        BaseParameterInvoiceInputFile.BaseModel = new InvoiceInputFile();
                        BaseParameterInvoiceInputFile.BaseModel.Code = result.BaseModel.Code;
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
                                                if (result.BaseModel != null)
                                                {
                                                    if (result.BaseModel.ID > 0)
                                                    {
                                                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                        if (workSheet != null)
                                                        {
                                                            int totalRows = workSheet.Dimension.Rows;
                                                            for (int j = 2; j <= totalRows; j++)
                                                            {
                                                                InvoiceInputDetail InvoiceInputDetail = new InvoiceInputDetail();
                                                                if (workSheet.Cells[j, 2].Value != null)
                                                                {
                                                                    InvoiceInputDetail.MaterialName = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                                }
                                                                if (!string.IsNullOrEmpty(InvoiceInputDetail.MaterialName))
                                                                {
                                                                    if (workSheet.Cells[j, 3].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.CategoryUnitName = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 4].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.Description = workSheet.Cells[j, 4].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 8].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.PalletNo = workSheet.Cells[j, 8].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 9].Value != null)
                                                                    {
                                                                        InvoiceInputDetail.ShippedNo = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                                    }
                                                                    if (workSheet.Cells[j, 5].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            var QUANTITY = double.Parse(workSheet.Cells[j, 5].Value.ToString().Trim());
                                                                            var QUANTITYString = workSheet.Cells[j, 5].Value.ToString().Trim();
                                                                            QUANTITYString = QUANTITYString.Split(',')[0];
                                                                            QUANTITYString = QUANTITYString.Split('.')[0];
                                                                            var Part = double.Parse(QUANTITYString);
                                                                            var Remainder = QUANTITY - Part;
                                                                            if (Remainder >= 0.5)
                                                                            {
                                                                                Part = Part + 1;
                                                                            }
                                                                            InvoiceInputDetail.Quantity = (decimal)Part;
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 10].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            InvoiceInputDetail.Price = decimal.Parse(workSheet.Cells[j, 10].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    if (workSheet.Cells[j, 11].Value != null)
                                                                    {
                                                                        try
                                                                        {
                                                                            InvoiceInputDetail.Total = decimal.Parse(workSheet.Cells[j, 11].Value.ToString().Trim());
                                                                        }
                                                                        catch (Exception e)
                                                                        {
                                                                            string mes = e.Message;
                                                                        }
                                                                    }
                                                                    InvoiceInputDetail.ParentID = result.BaseModel.ID;
                                                                    await _InvoiceInputDetailService.SaveAsync(new BaseParameter<InvoiceInputDetail>
                                                                    {
                                                                        BaseModel = InvoiceInputDetail
                                                                    });
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
        [Route("GetByActive_IsFutureToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByActive_IsFutureToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByActive_IsFutureToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_ActiveToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByMembershipID_ActiveToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByMembershipID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_SearchStringToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_SearchStringToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByCompanyID_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_Year_Month_SearchStringToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_Year_Month_SearchStringToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByCompanyID_Year_Month_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<InvoiceInput>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InvoiceInput>>(Request.Form["BaseParameter"]);
                result = await _InvoiceInputService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

