
namespace Service.Implement
{
    public class WarehouseRequestService : BaseService<WarehouseRequest, IWarehouseRequestRepository>
    , IWarehouseRequestService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseRequestDetailRepository _WarehouseRequestDetailRepository;
        private readonly IWarehouseOutputService _WarehouseOutputService;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly INotificationService _NotificationService;
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        public WarehouseRequestService(IWarehouseRequestRepository WarehouseRequestRepository
            , IWebHostEnvironment WebHostEnvironment
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IWarehouseRequestDetailRepository WarehouseRequestDetailRepository
            , IWarehouseOutputService WarehouseOutputService
            , IProductionOrderRepository ProductionOrderRepository
            , IMembershipRepository MembershipRepository
            , INotificationService NotificationService
            , IMembershipDepartmentRepository membershipDepartmentRepository

            ) : base(WarehouseRequestRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WarehouseRequestDetailRepository = WarehouseRequestDetailRepository;
            _WarehouseOutputService = WarehouseOutputService;
            _ProductionOrderRepository = ProductionOrderRepository;
            _MembershipRepository = MembershipRepository;
            _NotificationService = NotificationService;
            _MembershipDepartmentRepository = membershipDepartmentRepository;
        }
        public override void InitializationSave(WarehouseRequest model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Active = Parent.Active;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Date.Value.Year;
            model.Month = model.Date.Value.Month;
            model.Day = model.Date.Value.Day;
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            //model.SupplierID = model.SupplierID ?? GlobalHelper.DepartmentID;
            //model.CustomerID = model.CustomerID ?? GlobalHelper.DepartmentID;
            if (model.SupplierID == null || model.SupplierID == 0)
            {
                if (model.CompanyID > 0)
                {
                    var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.IsSync == true).FirstOrDefault();
                    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                    {
                        model.SupplierID = CategoryDepartment.ID;
                    }
                }
            }
            if (model.SupplierID > 0)
            {
                var Customer = _CategoryDepartmentRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = Customer.Code;
            }
            if (model.CustomerID > 0)
            {
                var Customer = _CategoryDepartmentRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = Customer.Code;
            }

            model.Code = model.Code ?? model.ParentName + "-" + model.Display + "-" + model.CustomerName + "-" + GlobalHelper.InitializationDateTimeCode0001;
            model.Name = model.Name ?? model.Code;
            model.Total = model.Total ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * (model.Tax / 100);
            model.TotalDiscount = model.Total * (model.Discount / 100);
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;

            model.IsManagerCustomer = model.IsManagerSupplier;
        }
        public override async Task<BaseResult<WarehouseRequest>> SaveAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            InitializationSave(BaseParameter.BaseModel);
            //var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code && o.Name == BaseParameter.BaseModel.Name && o.Year == BaseParameter.BaseModel.Year && o.Month == BaseParameter.BaseModel.Month && o.Day == BaseParameter.BaseModel.Day).FirstOrDefaultAsync();
            var ModelCheck = await GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.SupplierID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CustomerID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CompanyID == null)
            {
                IsSave = false;
            }
            if (IsSave == true)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
                if (result.BaseModel.ID > 0)
                {
                    try
                    {
                        BaseParameter.BaseModel = result.BaseModel;
                        await SyncAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> SyncAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            await SyncByWarehouseRequestAsync(BaseParameter);
            //await SendMailAsync(BaseParameter);
            //await SendNotificationAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> SyncByWarehouseRequestAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.IsManagerSupplier == true)
                        {
                            if (BaseParameter.BaseModel.IsManagerCustomer == true)
                            {
                                var BaseParameterWarehouseOutput = new BaseParameter<WarehouseOutput>();
                                BaseParameterWarehouseOutput.ID = BaseParameter.BaseModel.ID;
                                await _WarehouseOutputService.SyncByWarehouseRequestAsync(BaseParameterWarehouseOutput);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseRequest>> RemoveAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            //var IsCheck = true;
            //var WarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.Active == false && o.WarehouseRequestID == BaseParameter.ID).FirstOrDefaultAsync();
            //if (WarehouseOutput != null)
            //{
            //    if (WarehouseOutput.ID > 0)
            //    {
            //        IsCheck = false;
            //    }
            //}
            //if (IsCheck == true)
            //{
            //    result.BaseModel = await _WarehouseRequestRepository.GetByIDAsync(BaseParameter.ID);
            //    if (result.BaseModel.ID > 0 && result.BaseModel.Active != true)
            //    {
            //        result.Count = await _WarehouseRequestRepository.RemoveAsync(BaseParameter.ID);
            //    }
            //}
            result.Count = await _WarehouseRequestRepository.RemoveAsync(BaseParameter.ID);
            if (result.Count > 0)
            {
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByConfirmToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            result.List = await GetByCondition(o => o.Active == true && o.IsManagerSupplier == true && o.IsManagerCustomer == true).ToListAsync();
            return result;
        }
        public override async Task<BaseResult<WarehouseRequest>> GetByParentIDToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            result.List = await _WarehouseRequestRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            result.List = result.List.OrderByDescending(o => o.Date).ThenBy(o => o.CustomerID).ToList();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> SendMailAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            result.BaseModel = BaseParameter.BaseModel;
            if (result.BaseModel != null)
            {
                if (result.BaseModel.ID > 0)
                {
                    if (result.BaseModel.IsManagerSupplier == true && result.BaseModel.IsManagerCustomer == true)
                    {
                    }
                    else
                    {
                        var ListMembership = await _MembershipRepository.GetByCondition(o => o.Active == true && o.CategoryPositionID == GlobalHelper.PositionID && (o.CategoryDepartmentID == result.BaseModel.SupplierID || o.CategoryDepartmentID == result.BaseModel.CustomerID)).ToListAsync();
                        foreach (var Membership in ListMembership)
                        {
                            if (!string.IsNullOrEmpty(Membership.Email))
                            {
                                string HTMLContent = GlobalHelper.InitializationString;
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "WarehouseRequestEmail.html");
                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                {
                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        HTMLContent = r.ReadToEnd();
                                    }
                                }

                                HTMLContent = HTMLContent.Replace(@"[ERPSite]", GlobalHelper.ERPSite);
                                HTMLContent = HTMLContent.Replace(@"[ID]", result.BaseModel.ID.ToString());
                                HTMLContent = HTMLContent.Replace(@"[Code]", result.BaseModel.Code);
                                HTMLContent = HTMLContent.Replace(@"[ParentName]", result.BaseModel.ParentName);
                                HTMLContent = HTMLContent.Replace(@"[Date]", result.BaseModel.Date.Value.ToString("yyyy-MM-dd"));

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
                                    mail.Subject = result.BaseModel.Code + " - " + GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy HH:mm:ss") + " | Request";
                                    mail.Content = HTMLContent;
                                    mail.MailTo = Membership.Email;
                                    MailHelper.SendMail(mail);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> SendNotificationAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            result.BaseModel = BaseParameter.BaseModel;
            if (result.BaseModel != null)
            {
                if (result.BaseModel.ID > 0)
                {
                    if (result.BaseModel.IsManagerSupplier == true && result.BaseModel.IsManagerCustomer == true)
                    {
                    }
                    else
                    {
                        var ListMembership = await _MembershipRepository.GetByCondition(o => o.Active == true && o.CategoryPositionID == GlobalHelper.PositionID && (o.CategoryDepartmentID == result.BaseModel.SupplierID || o.CategoryDepartmentID == result.BaseModel.CustomerID)).ToListAsync();
                        foreach (var Membership in ListMembership)
                        {
                            var Notification = new Notification();
                            Notification.ID = result.BaseModel.ID;
                            Notification.Code = result.BaseModel.Code;
                            Notification.Name = result.BaseModel.GetType().Name;
                            Notification.ParentID = Membership.ID;
                            var BaseParameterNotification = new BaseParameter<Notification>();
                            BaseParameterNotification.BaseModel = Notification;
                            await _NotificationService.CreateWarehouseRequestAsync(BaseParameterNotification);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipIDToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();

            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    result.List = await GetByCondition(o => o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value)).ToListAsync();
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();

            if (BaseParameter.MembershipID > 0)
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                    if (ListMembershipDepartment.Count > 0)
                    {
                        var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                        result.List = await GetByCondition(o => o.SupplierID > 0 && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();

            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString))).ToListAsync();
            }
            else
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    result.List = await GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();

                    switch (BaseParameter.Action)
                    {
                        case 0:
                            break;
                        case 1:
                            result.List = result.List.Where(o => o.Active == true && o.IsManagerSupplier != true).ToList();
                            break;
                        case 2:
                            result.List = result.List.Where(o => o.Active == true && o.IsManagerSupplier == true).ToList();
                            break;
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByMembershipID_ConfirmToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipDepartment = await _MembershipDepartmentRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipDepartment.Count > 0)
                {
                    var ListMembershipDepartmentID = ListMembershipDepartment.Select(o => o.CategoryDepartmentID).ToList();
                    result.List = await GetByCondition(o => o.SupplierID != null && ListMembershipDepartmentID.Contains(o.SupplierID.Value) && o.Active == true && o.IsManagerSupplier == true && o.IsManagerCustomer == true).ToListAsync();
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequest>> GetByCategoryDepartmentID_SearchStringToListAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            result.List = new List<WarehouseRequest>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID).Take(100).ToListAsync();
                }
            }
            result.List = result.List.OrderByDescending(o => o.Date).ThenBy(o => o.Code).ToList();
            return result;
        }

        public virtual async Task<BaseResult<WarehouseRequest>> ExportToExcelAsync(BaseParameter<WarehouseRequest> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequest>();
            if (BaseParameter.ID > 0)
            {
                var ListWarehouseRequestDetail = await _WarehouseRequestDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
                string fileName = BaseParameter.ID + "-WarehouseRequest" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationExcel(ListWarehouseRequestDetail, streamExport);
                var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
            }
            return result;
        }
        private void InitializationExcel(List<WarehouseRequestDetail> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "PART NO";
                workSheet.Cells[row, column].Style.Font.Bold = true;
                workSheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[row, column].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, column].Style.Font.Size = 11;
                workSheet.Cells[row, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                workSheet.Cells[row, column].Style.Font.Bold = true;
                workSheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[row, column].Style.Font.Name = "Times New Roman";
                workSheet.Cells[row, column].Style.Font.Size = 11;
                workSheet.Cells[row, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[row, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                row = row + 1;
                foreach (WarehouseRequestDetail item in list)
                {
                    workSheet.Cells[row, 1].Value = item.MaterialName;
                    workSheet.Cells[row, 2].Value = item.Quantity;

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

