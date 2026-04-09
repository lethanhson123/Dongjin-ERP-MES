using DocumentFormat.OpenXml.ExtendedProperties;

namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderController : BaseController<ProductionOrder, IProductionOrderService>
    {
        private readonly IProductionOrderService _ProductionOrderService;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IProductionOrderFileService _ProductionOrderFileService;
        private readonly IProductionOrderDetailService _ProductionOrderDetailService;

        private readonly ICompanyService _CompanyService;
        private readonly IMembershipRepository _MembershipRepository;
        public ProductionOrderController(IProductionOrderService ProductionOrderService

            , IWebHostEnvironment WebHostEnvironment

            , IProductionOrderFileService ProductionOrderFileService
            , IProductionOrderDetailService ProductionOrderDetailService
            , ICompanyService CompanyService
            , IMembershipRepository MembershipRepository

            ) : base(ProductionOrderService, WebHostEnvironment)
        {
            _ProductionOrderService = ProductionOrderService;
            _WebHostEnvironment = WebHostEnvironment;

            _ProductionOrderFileService = ProductionOrderFileService;
            _ProductionOrderDetailService = ProductionOrderDetailService;
            _CompanyService = CompanyService;
            _MembershipRepository = MembershipRepository;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFilesAsync")]
        public override async Task<BaseResult<ProductionOrder>> SaveAndUploadFilesAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.BaseModel != null)
                    {
                        result = await _ProductionOrderService.SaveAsync(BaseParameter);
                        BaseParameter.BaseModel = result.BaseModel;

                        var BaseParameterProductionOrderFile = new BaseParameter<ProductionOrderFile>();
                        BaseParameterProductionOrderFile.BaseModel = new ProductionOrderFile();
                        BaseParameterProductionOrderFile.BaseModel.ParentID = BaseParameter.BaseModel.ID;
                        BaseParameterProductionOrderFile.BaseModel.Code = BaseParameter.BaseModel.Code;

                        if (Request.Form.Files.Count > 0)
                        {
                            var file = Request.Form.Files[0];
                            if (file == null || file.Length == 0)
                            {
                            }
                            if (file != null)
                            {
                                BaseParameterProductionOrderFile.BaseModel.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                BaseParameterProductionOrderFile.BaseModel.ParentID = BaseParameter.BaseModel.ID;
                                BaseParameterProductionOrderFile.BaseModel.Code = BaseParameter.BaseModel.Code;
                                BaseParameterProductionOrderFile.BaseModel.Display = Path.GetExtension(file.FileName);
                                BaseParameterProductionOrderFile.BaseModel.FileName = BaseParameter.BaseModel.ID + "_" + BaseParameter.BaseModel.GetType().Name + "_" + GlobalHelper.InitializationDateTimeCode0001 + "_" + BaseParameterProductionOrderFile.BaseModel.Display;

                                string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameterProductionOrderFile.BaseModel.GetType().Name);
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
                                    BaseParameterProductionOrderFile.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameterProductionOrderFile.BaseModel.GetType().Name + "/" + BaseParameterProductionOrderFile.BaseModel.FileName;
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

                                                if (BaseParameter.BaseModel != null)
                                                {
                                                    if (BaseParameter.BaseModel.ID > 0)
                                                    {
                                                        var ListProductionOrderDetail = new List<ProductionOrderDetail>();
                                                        var DateFlag = new DateTime(1900, 1, 1);

                                                        for (int j = 2; j <= totalRows; j++)
                                                        {
                                                            ProductionOrderDetail ProductionOrderDetail = new ProductionOrderDetail();
                                                            if (workSheet.Cells["B" + j].Value != null)
                                                            {
                                                                ProductionOrderDetail.MaterialCode = workSheet.Cells["B" + j].Value.ToString().Trim();
                                                            }
                                                            if (!string.IsNullOrEmpty(ProductionOrderDetail.MaterialCode))
                                                            {
                                                                if (workSheet.Cells["A" + j].Value != null)
                                                                {
                                                                    ProductionOrderDetail.CategoryFamilyName = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["C" + j].Value != null)
                                                                {
                                                                    ProductionOrderDetail.BOMECN = workSheet.Cells["C" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["D" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.QuantitySNP = int.Parse(workSheet.Cells["D" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["E" + j].Value != null)
                                                                {
                                                                    ProductionOrderDetail.BOMECNVersion = workSheet.Cells["E" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["G" + j].Value != null)
                                                                {
                                                                    ProductionOrderDetail.BOMECNVersion = workSheet.Cells["G" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["F" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Priority = int.Parse(workSheet.Cells["F" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["G" + j].Value != null)
                                                                {
                                                                    ProductionOrderDetail.CategoryUnitName = workSheet.Cells["G" + j].Value.ToString().Trim();
                                                                }
                                                                if (workSheet.Cells["H" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity01 = int.Parse(workSheet.Cells["H" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["I" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity02 = int.Parse(workSheet.Cells["I" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["J" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity03 = int.Parse(workSheet.Cells["J" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["K" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity04 = int.Parse(workSheet.Cells["K" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["L" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity05 = int.Parse(workSheet.Cells["L" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["M" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity06 = int.Parse(workSheet.Cells["M" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["N" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity07 = int.Parse(workSheet.Cells["N" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["O" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity08 = int.Parse(workSheet.Cells["O" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["P" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity09 = int.Parse(workSheet.Cells["P" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["Q" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity10 = int.Parse(workSheet.Cells["Q" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["R" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity11 = int.Parse(workSheet.Cells["R" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["S" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity12 = int.Parse(workSheet.Cells["S" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["T" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity13 = int.Parse(workSheet.Cells["T" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["U" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity14 = int.Parse(workSheet.Cells["U" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                if (workSheet.Cells["V" + j].Value != null)
                                                                {
                                                                    try
                                                                    {
                                                                        ProductionOrderDetail.Quantity15 = int.Parse(workSheet.Cells["V" + j].Value.ToString().Trim());
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        string msg = ex.Message;
                                                                    }
                                                                }
                                                                int jSub = 1;
                                                                string column = "H";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date01 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "I";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date02 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "J";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date03 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "K";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date04 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "L";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date05 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "M";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date06 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "N";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date07 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "O";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date08 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "P";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date09 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "Q";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date10 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "R";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date11 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "S";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date12 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "T";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date13 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "U";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date14 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                column = "V";
                                                                if (workSheet.Cells[column + jSub].Value != null)
                                                                {
                                                                    var DateTime = workSheet.Cells[column + jSub].Value.ToString().Trim();
                                                                    ProductionOrderDetail.Date15 = GlobalHelper.CovertStringToDateTime(DateTime);
                                                                }
                                                                ProductionOrderDetail.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                                                ProductionOrderDetail.ParentID = BaseParameter.BaseModel.ID;
                                                                ProductionOrderDetail.Code = BaseParameter.BaseModel.Code;
                                                                ListProductionOrderDetail.Add(ProductionOrderDetail);
                                                            }
                                                        }

                                                        ListProductionOrderDetail = ListProductionOrderDetail.OrderBy(o => o.Priority).ToList();
                                                        foreach (var ProductionOrderDetail in ListProductionOrderDetail)
                                                        {
                                                            var BaseParameterProductionOrderDetail = new BaseParameter<ProductionOrderDetail>();
                                                            BaseParameterProductionOrderDetail.IsUpload = true;
                                                            BaseParameterProductionOrderDetail.BaseModel = ProductionOrderDetail;
                                                            await _ProductionOrderDetailService.SaveAsync(BaseParameterProductionOrderDetail);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        await SendMailAsync(BaseParameter.BaseModel.ID);
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
            var ListProductionOrderDetail = await _ProductionOrderDetailService.GetByCondition(o => o.ParentID == ID && o.Active == false && o.SortOrder > 1).ToListAsync();
            if (ListProductionOrderDetail.Count > 0)
            {
                var ListMembership = await _MembershipRepository.GetByCondition(o => o.Active == true && !string.IsNullOrEmpty(o.Email) && o.CategoryDepartmentID == 191).ToListAsync();
                if (ListMembership.Count > 0)
                {
                    string Email = string.Join(",", ListMembership.Select(o => o.Email).ToList());
                    string Code = string.Join(",", ListProductionOrderDetail.Select(o => o.ParentName).Distinct().ToList());
                    string link = GlobalHelper.ERPSite + "/#/ProductionOrderInfo/" + ID;
                    string URL = @"<a title='" + link + "' href='" + link + "'><h2>" + Code + "</h2></a>";
                    string MaterialCode = string.Join(",", ListProductionOrderDetail.Select(o => o.MaterialCode).ToList());
                    string BOMECN = string.Join(",", ListProductionOrderDetail.Select(o => o.BOMECN).ToList());
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
        [Route("GetByActive_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<ProductionOrder>> GetByActive_IsCompleteToListAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderService.GetByActive_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipIDToListAsync")]
        public virtual async Task<BaseResult<ProductionOrder>> GetByMembershipIDToListAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderService.GetByMembershipIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByMembershipID_Active_IsCompleteToListAsync")]
        public virtual async Task<BaseResult<ProductionOrder>> GetByMembershipID_Active_IsCompleteToListAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderService.GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_Year_Month_ActionToListAsync")]
        public virtual async Task<BaseResult<ProductionOrder>> GetByCompanyID_Year_Month_ActionToListAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderService.GetByCompanyID_Year_Month_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync")]
        public virtual async Task<BaseResult<ProductionOrder>> GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync()
        {
            var result = new BaseResult<ProductionOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderService.GetByCompanyID_DateBegin_DateEnd_SearchString_ActionToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

