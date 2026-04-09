namespace Service.Implement
{
    public class ProductionOrderSPSTOrderService : BaseService<ProductionOrderSPSTOrder, IProductionOrderSPSTOrderRepository>
    , IProductionOrderSPSTOrderService
    {
        private readonly IProductionOrderSPSTOrderRepository _ProductionOrderSPSTOrderRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderProductionPlanSemiRepository _ProductionOrderProductionPlanSemiRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IBOMTermRepository _BOMTermRepository;
        private readonly ICategoryToleranceRepository _CategoryToleranceRepository;
        private readonly ICategoryParentheseRepository _CategoryParentheseRepository;
        private readonly IMembershipRepository _MembershipRepository;

        public ProductionOrderSPSTOrderService(IProductionOrderSPSTOrderRepository ProductionOrderSPSTOrderRepository

            , IWebHostEnvironment webHostEnvironment
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderProductionPlanSemiRepository ProductionOrderProductionPlanSemiRepository
            , IMaterialRepository MaterialRepository
            , IBOMRepository BomRepository
            , IBOMDetailRepository BOMDetailRepository
            , IBOMTermRepository bOMTermRepository
            , ICategoryToleranceRepository CategoryToleranceRepository
            , ICategoryParentheseRepository CategoryParentheseRepository
            , IMembershipRepository MembershipRepository

            ) : base(ProductionOrderSPSTOrderRepository)
        {
            _ProductionOrderSPSTOrderRepository = ProductionOrderSPSTOrderRepository;
            _WebHostEnvironment = webHostEnvironment;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderProductionPlanSemiRepository = ProductionOrderProductionPlanSemiRepository;
            _MaterialRepository = MaterialRepository;
            _BOMRepository = BomRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _BOMTermRepository = bOMTermRepository;
            _CategoryToleranceRepository = CategoryToleranceRepository;
            _CategoryParentheseRepository = CategoryParentheseRepository;
            _MembershipRepository = MembershipRepository;
        }
        public override void Initialization(ProductionOrderSPSTOrder model)
        {
            BaseInitialization(model);
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.HookRack = model.HookRack ?? Material.CategoryLocationName;
                if (!string.IsNullOrEmpty(model.MaterialCode))
                {
                    model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                    try
                    {
                        if (model.MaterialCode.Split('.').Length > 1)
                        {
                            model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 2] + "." + model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            if (model.BOMID01 > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID01.Value);
                model.BOMECN01 = BOM.Code;
                model.BOMECNVersion01 = BOM.Version;
                model.BOMDate01 = BOM.Date;
            }
            if (model.BOMID > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.BOMECN = BOM.Code;
                model.BOMECNVersion = BOM.Version;
                model.BOMDate = BOM.Date;
                model.BundleSize = BOM.BundleSize;
            }
        }
        public virtual void InitializationSaveList(ProductionOrderSPSTOrder model, List<Material> ListMaterial, List<BOM> ListBOM)
        {
            BaseInitialization(model);

            if (model.MaterialID > 0)
            {
                var Material = ListMaterial.Where(o => o.ID == model.MaterialID.Value).FirstOrDefault();
                if (Material != null && Material.ID > 0)
                {
                    model.MaterialCode = Material.Code;
                    model.MaterialName = Material.Name;
                    model.HookRack = model.HookRack ?? Material.CategoryLocationName;
                    if (!string.IsNullOrEmpty(model.MaterialCode))
                    {
                        model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                        try
                        {
                            if (model.MaterialCode.Split('.').Length > 1)
                            {
                                model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 2] + "." + model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                            }
                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;
                        }
                    }
                }
            }
            if (model.BOMID01 > 0)
            {
                var BOM = ListBOM.Where(o => o.ID == model.BOMID01.Value).FirstOrDefault();
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMECN01 = BOM.Code;
                    model.BOMECNVersion01 = BOM.Version;
                    model.BOMDate01 = BOM.Date;
                }
            }
            if (model.BOMID > 0)
            {
                var BOM = ListBOM.Where(o => o.ID == model.BOMID.Value).FirstOrDefault();
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMECN = BOM.Code;
                    model.BOMECNVersion = BOM.Version;
                    model.BOMDate = BOM.Date;
                    model.BundleSize = BOM.BundleSize;
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderSPSTOrder>> SaveAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Date != null && BaseParameter.BaseModel.Date != null && o.Date.Value.Date == BaseParameter.BaseModel.Date.Value.Date).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> GetByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
            }
            return result;
        }

        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> SyncByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                long? CompanyID = 16;
                var ListProductionOrderSPSTOrder = GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToList();
                if (ListProductionOrderSPSTOrder.Count > 0)
                {
                    await _ProductionOrderSPSTOrderRepository.RemoveRangeAsync(ListProductionOrderSPSTOrder);
                }
                var ListProductionOrderSPSTOrderAdd = new List<ProductionOrderSPSTOrder>();
                var ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.ParentID && o.MaterialID01 > 0 && o.MaterialID > 0 && o.IsSPST == true).OrderBy(o => o.MaterialCode).ToListAsync();
                if (ListProductionOrderProductionPlanSemi.Count > 0)
                {
                    CompanyID = ListProductionOrderProductionPlanSemi[0].CompanyID;
                }
                List<Material> ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<BOM> ListBOM = await _BOMRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();

                foreach (var ProductionOrderProductionPlanSemi in ListProductionOrderProductionPlanSemi)
                {
                    var Index = 1;
                    foreach (PropertyInfo proDate in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                    {
                        var IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + IndexName;
                        }
                        var DateName = "Date" + IndexName;
                        var QuantityGAPName = "QuantityGAP" + IndexName;
                        if (proDate.Name == DateName)
                        {
                            if (proDate.GetValue(ProductionOrderProductionPlanSemi) != null)
                            {
                                var DateValue = (DateTime?)proDate.GetValue(ProductionOrderProductionPlanSemi);
                                if (DateValue != null && DateValue.Value.Date == BaseParameter.Date.Value.Date)
                                {
                                    foreach (PropertyInfo proQuantityGAP in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                                    {
                                        if (proQuantityGAP.Name == QuantityGAPName)
                                        {
                                            if (proQuantityGAP.GetValue(ProductionOrderProductionPlanSemi) != null)
                                            {
                                                var QuantityGAP = (int?)proQuantityGAP.GetValue(ProductionOrderProductionPlanSemi);
                                                if (QuantityGAP > 0)
                                                {
                                                    var ProductionOrderSPSTOrder = new ProductionOrderSPSTOrder();
                                                    ProductionOrderSPSTOrder.SortOrder = 10;
                                                    ProductionOrderSPSTOrder.ParentID = BaseParameter.ParentID;
                                                    ProductionOrderSPSTOrder.ParentID = BaseParameter.ParentID;
                                                    ProductionOrderSPSTOrder.Code = ProductionOrderProductionPlanSemi.Code;
                                                    ProductionOrderSPSTOrder.CompanyID = ProductionOrderProductionPlanSemi.CompanyID;
                                                    ProductionOrderSPSTOrder.CompanyName = ProductionOrderProductionPlanSemi.CompanyName;
                                                    ProductionOrderSPSTOrder.ProductionOrderProductionPlanSemiID = ProductionOrderProductionPlanSemi.ID;
                                                    ProductionOrderSPSTOrder.BOMID01 = ProductionOrderProductionPlanSemi.BOMID01;
                                                    ProductionOrderSPSTOrder.BOMID = ProductionOrderProductionPlanSemi.BOMID;
                                                    ProductionOrderSPSTOrder.MaterialID01 = ProductionOrderProductionPlanSemi.MaterialID01;
                                                    ProductionOrderSPSTOrder.MaterialCode01 = ProductionOrderProductionPlanSemi.MaterialCode01;
                                                    ProductionOrderSPSTOrder.MaterialName01 = ProductionOrderProductionPlanSemi.MaterialName01;
                                                    ProductionOrderSPSTOrder.MaterialID = ProductionOrderProductionPlanSemi.MaterialID;
                                                    ProductionOrderSPSTOrder.MaterialCode = ProductionOrderProductionPlanSemi.MaterialCode;
                                                    ProductionOrderSPSTOrder.MaterialName = ProductionOrderProductionPlanSemi.MaterialName;
                                                    ProductionOrderSPSTOrder.Date = BaseParameter.Date;
                                                    ProductionOrderSPSTOrder.QuantityTotal = QuantityGAP;
                                                    InitializationSaveList(ProductionOrderSPSTOrder, ListMaterial, ListBOM);
                                                    ListProductionOrderSPSTOrderAdd.Add(ProductionOrderSPSTOrder);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Index = Index + 1;
                        }
                    }
                }
                ListProductionOrderSPSTOrderAdd = ListProductionOrderSPSTOrderAdd.OrderBy(o => o.MaterialCode).ToList();
                await _ProductionOrderSPSTOrderRepository.AddRangeAsync(ListProductionOrderSPSTOrderAdd);
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> SyncMESByParentID_DateToListAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                Membership Membership = new Membership();
                string? UserName = "";
                if (BaseParameter.UpdateUserID > 0)
                {
                    Membership = await _MembershipRepository.GetByIDAsync(BaseParameter.UpdateUserID.Value);
                    UserName = Membership.UserName;
                }
                var ProductionOrder = _ProductionOrderRepository.GetByID(BaseParameter.ParentID.Value);
                if (ProductionOrder.ID > 0 && ProductionOrder.CompanyID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                    if (result.List.Count > 0)
                    {
                        string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(ProductionOrder.CompanyID.Value);
                        string FACTORY_NM = "Factory 1";
                        switch (ProductionOrder.CompanyID)
                        {
                            case 17:
                                FACTORY_NM = "Factory 2";
                                break;
                        }
                        string sql = @"INSERT INTO `TORDERLIST_SPST` (`OR_NO`, `WORK_WEEK`, `PO_DT`, `LEAD_NO`, `PO_QTY`, `SAFTY_QTY`, `MC`, `BUNDLE_SIZE`, `PERFORMN`, `CONDITION`, `LEAD_COUNT`, `PO_YN`, `DSCN_YN`, `ERROR_YN`, `CREATE_DTM`, `CREATE_USER`, `FCTRY_NM`, `REP`, `TORDER_FG`, `TOEXCEL_QTY`) VALUES ";
                        string VALUESSUM = "";
                        foreach (var item in result.List)
                        {
                            if (item.Date == null)
                            {
                                item.Date = GlobalHelper.InitializationDateTime;
                            }
                            string Date = item.Date.Value.ToString("yyyy-MM-dd");
                            int Week = GlobalHelper.GetWeekByDateTime(item.Date.Value);
                            item.MaterialCode = item.MaterialCode ?? "";
                            item.QuantityTotal = item.QuantityTotal ?? 0;
                            item.Quantity = item.Quantity ?? 0;
                            item.Machine = item.Machine ?? "";
                            item.BundleSize = item.BundleSize ?? 0;
                            item.SPST = item.SPST ?? "";
                            item.MaterialCode01 = item.MaterialCode01 ?? "";
                            string VALUES = "('EVENT'," + Week + ",'" + Date + "','" + item.MaterialCode + "'," + item.QuantityTotal + "," + item.Quantity + ",'" + item.Machine + "'," + item.BundleSize + " , '0', 'Stay', '0', 'N', 'N', 'N', NOW(),'" + UserName + "','" + FACTORY_NM + "','" + item.SPST + "','" + item.MaterialCode01 + "'," + item.QuantityTotal + ")";
                            if (VALUESSUM == "")
                            {
                                VALUESSUM = VALUES;
                            }
                            else
                            {
                                VALUESSUM = VALUESSUM + ", " + VALUES;
                            }
                        }
                        sql = sql + VALUESSUM;
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> ExportToExcelAsync(BaseParameter<ProductionOrderSPSTOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                if (result.List.Count > 0)
                {
                    var ProductionOrderSPSTOrder = result.List[0];
                    string fileName = ProductionOrderSPSTOrder.Code + "-SPSTOrder-" + BaseParameter.Date.Value.Date.ToString("yyyyMMdd") + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcel(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        private void InitializationExcel(List<ProductionOrderSPSTOrder> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "PO Code";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ECN";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ASSY NO (Finish Goods)";
                column = column + 1;                
                workSheet.Cells[row, column].Value = "SP/ST";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Total Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Machine";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Bundle Size";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "SPST";

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

                column = column + 1;



                row = row + 1;
                foreach (ProductionOrderSPSTOrder item in list)
                {
                    workSheet.Cells[row, 1].Value = item.Code;
                    workSheet.Cells[row, 2].Value = item.BOMECN01;
                    workSheet.Cells[row, 3].Value = item.MaterialCode01;                    
                    workSheet.Cells[row, 4].Value = item.MaterialCode;
                    workSheet.Cells[row, 6].Value = item.QuantityTotal;
                    workSheet.Cells[row, 7].Value = item.Quantity;
                    workSheet.Cells[row, 8].Value = item.Machine;
                    workSheet.Cells[row, 9].Value = item.BundleSize;
                    try
                    {
                        workSheet.Cells[row, 10].Value = item.Date.Value.ToString("dd/MM/yyyy");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        workSheet.Cells[row, 10].Value = item.Date;
                    }
                    workSheet.Cells[row, 11].Value = item.SPST;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 16;
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

