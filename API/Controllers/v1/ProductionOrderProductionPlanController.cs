namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderProductionPlanController : BaseController<ProductionOrderProductionPlan, IProductionOrderProductionPlanService>
    {
        private readonly IProductionOrderProductionPlanService _ProductionOrderProductionPlanService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IProductionOrderFileService _ProductionOrderFileService;
        private readonly IMembershipRepository _MembershipRepository;
        public ProductionOrderProductionPlanController(IProductionOrderProductionPlanService ProductionOrderProductionPlanService
            , IWebHostEnvironment WebHostEnvironment
            , IProductionOrderFileService productionOrderFileService
            , IMembershipRepository membershipRepository

            ) : base(ProductionOrderProductionPlanService, WebHostEnvironment)
        {
            _ProductionOrderProductionPlanService = ProductionOrderProductionPlanService;
            _WebHostEnvironment = WebHostEnvironment;
            _ProductionOrderFileService = productionOrderFileService;
            _MembershipRepository = membershipRepository;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public override async Task<BaseResult<ProductionOrderProductionPlan>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            result.BaseModel = new ProductionOrderProductionPlan();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlan>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        BaseParameter.BaseModel = new ProductionOrderProductionPlan();
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            if (file == null || file.Length == 0)
                            {
                            }
                            if (file != null)
                            {
                                var BaseParameterProductionOrderFile = new BaseParameter<ProductionOrderFile>();
                                BaseParameterProductionOrderFile.BaseModel = new ProductionOrderFile();
                                BaseParameterProductionOrderFile.BaseModel.ParentID = BaseParameter.ParentID;
                                BaseParameterProductionOrderFile.BaseModel.UpdateUserID = BaseParameter.UpdateUserID;
                                BaseParameterProductionOrderFile.BaseModel.Display = Path.GetExtension(file.FileName);
                                BaseParameterProductionOrderFile.BaseModel.FileName = BaseParameter.ParentID + "_" + BaseParameter.BaseModel.GetType().Name + "_" + GlobalHelper.InitializationDateTimeCode0001 + "_" + BaseParameterProductionOrderFile.BaseModel.Display;

                                string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                                bool isFolderExists = System.IO.Directory.Exists(folderPath);
                                if (!isFolderExists)
                                {
                                    System.IO.Directory.CreateDirectory(folderPath);
                                }
                                var physicalPath = Path.Combine(folderPath, BaseParameterProductionOrderFile.BaseModel.FileName);
                                using (var stream = new FileStream(physicalPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);

                                    BaseParameterProductionOrderFile.BaseModel.Description = physicalPath;
                                    BaseParameterProductionOrderFile.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameterProductionOrderFile.BaseModel.FileName;
                                    await _ProductionOrderFileService.SaveAsync(BaseParameterProductionOrderFile);
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
                                                var DateFlag = new DateTime(1900, 1, 1);
                                                var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanService.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.MaterialID > 0).ToListAsync();
                                                if (ListProductionOrderProductionPlan.Count > 0)
                                                {
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        var ProductionOrderProductionPlan = new ProductionOrderProductionPlan();
                                                        ProductionOrderProductionPlan.ParentID = BaseParameter.ParentID;

                                                        if (workSheet.Cells["B" + j].Value != null)
                                                        {
                                                            ProductionOrderProductionPlan.MaterialCode = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(ProductionOrderProductionPlan.MaterialCode))
                                                        {
                                                            string ProductionOrderProductionPlanQuantityName = "Quantity";
                                                            ProductionOrderProductionPlan = ListProductionOrderProductionPlan.Where(o => o.MaterialCode == ProductionOrderProductionPlan.MaterialCode).FirstOrDefault();
                                                            if (ProductionOrderProductionPlan != null && ProductionOrderProductionPlan.ID > 0)
                                                            {
                                                                if (workSheet.Cells["C" + j].Value != null)
                                                                {
                                                                    ProductionOrderProductionPlan.BOMECN = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["A" + j].Value != null)
                                                                {
                                                                    ProductionOrderProductionPlan.CategoryLineName = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                                }
                                                                var STT = 1;
                                                                foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                {
                                                                    var Name = STT.ToString();
                                                                    if (STT < 10)
                                                                    {
                                                                        Name = "0" + Name;
                                                                    }
                                                                    Name = ProductionOrderProductionPlanQuantityName + Name;
                                                                    if (proQuantity.Name == Name)
                                                                    {
                                                                        proQuantity.SetValue(ProductionOrderProductionPlan, 0, null);
                                                                        STT = STT + 1;
                                                                    }
                                                                }

                                                                for (int i = 4; i <= 108; i++)
                                                                {
                                                                    if (workSheet.Cells[1, i].Value != null)
                                                                    {
                                                                        var DateTime = workSheet.Cells[1, i].Value.ToString().Trim();
                                                                        if (!string.IsNullOrEmpty(DateTime))
                                                                        {
                                                                            try
                                                                            {
                                                                                var DateExcel = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                                var no = i - 2;
                                                                                var DateName = no.ToString();
                                                                                var QuantityName = no.ToString();
                                                                                if (no < 10)
                                                                                {
                                                                                    DateName = "0" + DateName;
                                                                                    QuantityName = "0" + QuantityName;
                                                                                }
                                                                                DateName = "Date" + DateName;
                                                                                QuantityName = ProductionOrderProductionPlanQuantityName + QuantityName;
                                                                                foreach (PropertyInfo proDate in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                {
                                                                                    if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO") && !proDate.Name.Contains("Name") && (proDate.GetValue(ProductionOrderProductionPlan) != null))
                                                                                    {
                                                                                        QuantityName = proDate.Name;
                                                                                        QuantityName = QuantityName.Replace(@"Date", ProductionOrderProductionPlanQuantityName);
                                                                                        var ProductionOrderProductionPlanDate = (DateTime)proDate.GetValue(ProductionOrderProductionPlan);
                                                                                        if (DateExcel.Date == ProductionOrderProductionPlanDate.Date)
                                                                                        {
                                                                                            if (workSheet.Cells[j, i].Value != null)
                                                                                            {
                                                                                                try
                                                                                                {
                                                                                                    var Quantity = int.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                                                    foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                                    {
                                                                                                        if (proQuantity.Name == QuantityName)
                                                                                                        {
                                                                                                            proQuantity.SetValue(ProductionOrderProductionPlan, Quantity, null);
                                                                                                            //var QuantityCutName = QuantityName.Replace(@"Quantity", @"QuantityCut");
                                                                                                            //foreach (PropertyInfo proQuantityCut in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                                            //{
                                                                                                            //    if (proQuantityCut.Name == QuantityCutName)
                                                                                                            //    {
                                                                                                            //        proQuantityCut.SetValue(ProductionOrderProductionPlan, Quantity, null);
                                                                                                            //        break;
                                                                                                            //    }
                                                                                                            //}
                                                                                                            break;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    string msg = ex.Message;
                                                                                                }
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                string msg = ex.Message;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                ProductionOrderProductionPlan.IsUpload = true;
                                                                BaseParameter.BaseModel = ProductionOrderProductionPlan;
                                                                await _ProductionOrderProductionPlanService.SaveAsync(BaseParameter);
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
                        await SendMailAsync(BaseParameter.ParentID.Value);
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
        [Route("SaveQuantityCutAndUploadFileAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SaveQuantityCutAndUploadFileAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            result.BaseModel = new ProductionOrderProductionPlan();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlan>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        BaseParameter.BaseModel = new ProductionOrderProductionPlan();
                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            if (file == null || file.Length == 0)
                            {
                            }
                            if (file != null)
                            {
                                var BaseParameterProductionOrderFile = new BaseParameter<ProductionOrderFile>();
                                BaseParameterProductionOrderFile.BaseModel = new ProductionOrderFile();
                                BaseParameterProductionOrderFile.BaseModel.ParentID = BaseParameter.ParentID;
                                BaseParameterProductionOrderFile.BaseModel.UpdateUserID = BaseParameter.UpdateUserID;
                                BaseParameterProductionOrderFile.BaseModel.Display = Path.GetExtension(file.FileName);
                                BaseParameterProductionOrderFile.BaseModel.FileName = BaseParameter.ParentID + "_" + BaseParameter.BaseModel.GetType().Name + "_" + GlobalHelper.InitializationDateTimeCode0001 + "_" + BaseParameterProductionOrderFile.BaseModel.Display;

                                string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                                bool isFolderExists = System.IO.Directory.Exists(folderPath);
                                if (!isFolderExists)
                                {
                                    System.IO.Directory.CreateDirectory(folderPath);
                                }
                                var physicalPath = Path.Combine(folderPath, BaseParameterProductionOrderFile.BaseModel.FileName);
                                using (var stream = new FileStream(physicalPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);

                                    BaseParameterProductionOrderFile.BaseModel.Description = physicalPath;
                                    BaseParameterProductionOrderFile.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameterProductionOrderFile.BaseModel.FileName;
                                    await _ProductionOrderFileService.SaveAsync(BaseParameterProductionOrderFile);
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
                                                var DateFlag = new DateTime(1900, 1, 1);
                                                var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanService.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.MaterialID > 0).ToListAsync();
                                                if (ListProductionOrderProductionPlan.Count > 0)
                                                {
                                                    for (int j = 2; j <= totalRows; j++)
                                                    {
                                                        var ProductionOrderProductionPlan = new ProductionOrderProductionPlan();
                                                        ProductionOrderProductionPlan.ParentID = BaseParameter.ParentID;
                                                        if (workSheet.Cells["A" + j].Value != null)
                                                        {
                                                            ProductionOrderProductionPlan.CategoryLineName = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                        }
                                                        if (workSheet.Cells["B" + j].Value != null)
                                                        {
                                                            ProductionOrderProductionPlan.MaterialCode = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                        }
                                                        if (workSheet.Cells["C" + j].Value != null)
                                                        {
                                                            ProductionOrderProductionPlan.BOMECN = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                        }
                                                        if (!string.IsNullOrEmpty(ProductionOrderProductionPlan.MaterialCode))
                                                        {
                                                            string ProductionOrderProductionPlanQuantityName = "QuantityCut";
                                                            ProductionOrderProductionPlan = ListProductionOrderProductionPlan.Where(o => o.MaterialCode == ProductionOrderProductionPlan.MaterialCode).FirstOrDefault();
                                                            if (ProductionOrderProductionPlan != null && ProductionOrderProductionPlan.ID > 0)
                                                            {
                                                                var STT = 1;
                                                                foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                {
                                                                    var Name = STT.ToString();
                                                                    if (STT < 10)
                                                                    {
                                                                        Name = "0" + Name;
                                                                    }
                                                                    Name = ProductionOrderProductionPlanQuantityName + Name;
                                                                    if (proQuantity.Name == Name)
                                                                    {
                                                                        proQuantity.SetValue(ProductionOrderProductionPlan, 0, null);
                                                                        STT = STT + 1;
                                                                    }
                                                                }

                                                                for (int i = 4; i <= 108; i++)
                                                                {
                                                                    if (workSheet.Cells[1, i].Value != null)
                                                                    {
                                                                        var DateTime = workSheet.Cells[1, i].Value.ToString().Trim();
                                                                        if (!string.IsNullOrEmpty(DateTime))
                                                                        {
                                                                            try
                                                                            {
                                                                                var DateExcel = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                                var no = i - 2;
                                                                                var DateName = no.ToString();
                                                                                var QuantityName = no.ToString();
                                                                                if (no < 10)
                                                                                {
                                                                                    DateName = "0" + DateName;
                                                                                    QuantityName = "0" + QuantityName;
                                                                                }
                                                                                DateName = "Date" + DateName;
                                                                                QuantityName = ProductionOrderProductionPlanQuantityName + QuantityName;
                                                                                foreach (PropertyInfo proDate in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                {
                                                                                    if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO") && !proDate.Name.Contains("Name") && (proDate.GetValue(ProductionOrderProductionPlan) != null))
                                                                                    {
                                                                                        QuantityName = proDate.Name;
                                                                                        QuantityName = QuantityName.Replace(@"Date", ProductionOrderProductionPlanQuantityName);
                                                                                        var ProductionOrderProductionPlanDate = (DateTime)proDate.GetValue(ProductionOrderProductionPlan);
                                                                                        if (DateExcel.Date == ProductionOrderProductionPlanDate.Date)
                                                                                        {
                                                                                            if (workSheet.Cells[j, i].Value != null)
                                                                                            {
                                                                                                try
                                                                                                {
                                                                                                    var Quantity = int.Parse(workSheet.Cells[j, i].Value.ToString().Trim());
                                                                                                    foreach (PropertyInfo proQuantity in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                                                    {
                                                                                                        if (proQuantity.Name == QuantityName)
                                                                                                        {
                                                                                                            proQuantity.SetValue(ProductionOrderProductionPlan, Quantity, null);
                                                                                                            break;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                catch (Exception ex)
                                                                                                {
                                                                                                    string msg = ex.Message;
                                                                                                }
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                                string msg = ex.Message;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                ProductionOrderProductionPlan.IsUpload = true;
                                                                BaseParameter.BaseModel = ProductionOrderProductionPlan;
                                                                await _ProductionOrderProductionPlanService.SaveAsync(BaseParameter);
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
                        await SendMailAsync(BaseParameter.ParentID.Value);
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

        private async Task<string> SendMailAsync(long ID)
        {
            var result = GlobalHelper.InitializationString;
            var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanService.GetByCondition(o => o.ParentID == ID && o.Active == false && o.SortOrder > 1).ToListAsync();
            if (ListProductionOrderProductionPlan.Count > 0)
            {
                var ListMembership = await _MembershipRepository.GetByCondition(o => o.Active == true && !string.IsNullOrEmpty(o.Email) && o.CategoryDepartmentID == 191).ToListAsync();
                if (ListMembership.Count > 0)
                {
                    string Email = string.Join(",", ListMembership.Select(o => o.Email).ToList());
                    string Code = string.Join(",", ListProductionOrderProductionPlan.Select(o => o.ParentName).Distinct().ToList());
                    string link = GlobalHelper.ERPSite + "/#/ProductionOrderInfo/" + ID;
                    string URL = @"<a title='" + link + "' href='" + link + "'><h2>" + Code + "</h2></a>";
                    string MaterialCode = string.Join(",", ListProductionOrderProductionPlan.Select(o => o.MaterialCode).ToList());
                    string BOMECN = string.Join(",", ListProductionOrderProductionPlan.Select(o => o.BOMECN).ToList());
                    string HTMLContent = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "ProductionOrder.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            HTMLContent = r.ReadToEnd();
                        }
                    }
                    HTMLContent = HTMLContent.Replace(@"[ID]", ID.ToString());
                    HTMLContent = HTMLContent.Replace(@"[Code]", Code.ToString());
                    HTMLContent = HTMLContent.Replace(@"[MaterialCode]", MaterialCode.ToString());
                    HTMLContent = HTMLContent.Replace(@"[BOMECN]", BOMECN.ToString());
                    HTMLContent = HTMLContent.Replace(@"[URL]", URL.ToString());

                    if (!string.IsNullOrEmpty(HTMLContent))
                    {
                        Helper.Model.Mail mail = new Helper.Model.Mail();
                        mail.MailFrom = GlobalHelper.MasterEmailUser;
                        mail.UserName = GlobalHelper.MasterEmailUser;
                        mail.Password = GlobalHelper.MasterEmailPassword;
                        mail.SMTPPort = GlobalHelper.SMTPPort;
                        mail.SMTPServer = GlobalHelper.SMTPServer;
                        mail.IsMailBodyHtml = GlobalHelper.IsMailBodyHtml;
                        mail.IsMailUsingSSL = GlobalHelper.IsMailUsingSSL;
                        mail.Display = GlobalHelper.MasterEmailDisplay;
                        mail.Subject = "BOM: " + MaterialCode + " , ECN: " + BOMECN + " not found at PO: " + Code;
                        mail.Content = HTMLContent;
                        mail.MailTo = Email;
                        MailHelper.SendMail(mail);
                    }
                }
            }
            return result;
        }

        [HttpPost]
        [Route("SyncQuantityToQuantityCutAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> SyncQuantityToQuantityCutAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlan>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanService.SyncQuantityToQuantityCutAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportByParentIDToExcelAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> ExportByParentIDToExcelAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlan>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanService.ExportByParentIDToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndSearchStringToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlan>> GetByParentIDAndSearchStringToListAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlan>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlan>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanService.GetByParentIDAndSearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

