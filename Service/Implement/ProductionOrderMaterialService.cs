namespace Service.Implement
{
    public class ProductionOrderMaterialService : BaseService<ProductionOrderMaterial, IProductionOrderMaterialRepository>
    , IProductionOrderMaterialService
    {
        private readonly IProductionOrderMaterialRepository _ProductionOrderMaterialRepository;
        private readonly IProductionOrderProductionPlanRepository _ProductionOrderProductionPlanRepository;
        private readonly IProductionOrderDetailRepository _ProductionOrderDetailRepository;
        private readonly IProductionOrderBOMDetailRepository _ProductionOrderBOMDetailRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IInvoiceInputInventoryRepository _InvoiceInputInventoryRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;
        public ProductionOrderMaterialService(IProductionOrderMaterialRepository ProductionOrderMaterialRepository
            , IProductionOrderProductionPlanRepository ProductionOrderProductionPlanRepository
            , IProductionOrderDetailRepository ProductionOrderDetailRepository
            , IProductionOrderBOMDetailRepository ProductionOrderBOMDetailRepository
            , IProductionOrderRepository ProductionOrderRepository
            , IMaterialRepository materialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryFamilyRepository categoryFamilyRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IBOMRepository bOMRepository
            , IWarehouseInventoryRepository warehouseInventoryRepository
            , IInvoiceInputInventoryRepository InvoiceInputInventoryRepository
            , IInvoiceInputRepository InvoiceInputRepository
            , IInvoiceInputDetailRepository InvoiceInputDetailRepository

            ) : base(ProductionOrderMaterialRepository)
        {
            _ProductionOrderMaterialRepository = ProductionOrderMaterialRepository;
            _ProductionOrderProductionPlanRepository = ProductionOrderProductionPlanRepository;
            _ProductionOrderDetailRepository = ProductionOrderDetailRepository;
            _ProductionOrderBOMDetailRepository = ProductionOrderBOMDetailRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _MaterialRepository = materialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryFamilyRepository = categoryFamilyRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _BOMRepository = bOMRepository;
            _WarehouseInventoryRepository = warehouseInventoryRepository;
            _InvoiceInputInventoryRepository = InvoiceInputInventoryRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
            _InvoiceInputDetailRepository = InvoiceInputDetailRepository;
        }
        public override void Initialization(ProductionOrderMaterial model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.Name = Parent.Name;
                model.Active = Parent.Active;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (model.Priority == GlobalHelper.SortOrder)
            {
                model.SortOrder = 2;
                model.MaterialID01 = GlobalHelper.InitializationNumber;
                model.MaterialCode01 = GlobalHelper.InitializationString;
                model.MaterialName01 = GlobalHelper.InitializationString;
            }
            if (model.CategoryFamilyID > 0)
            {
                var CategoryFamily = _CategoryFamilyRepository.GetByID(model.CategoryFamilyID.Value);
                model.CategoryFamilyName = CategoryFamily.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryFamilyName))
                {
                    var CategoryFamily = _CategoryFamilyRepository.GetByName(model.CategoryFamilyName);
                    if (CategoryFamily.ID == 0)
                    {
                        CategoryFamily.Active = true;
                        CategoryFamily.Name = model.CategoryUnitName;
                        _CategoryFamilyRepository.Add(CategoryFamily);
                    }
                    model.CategoryFamilyID = CategoryFamily.ID;
                }
            }
           
            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Active = true;
                        CategoryUnit.Name = model.CategoryUnitName;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID = CategoryUnit.ID;
                }
            }
            
            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialID = Material.ID;
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.QuantitySNP = Material.QuantitySNP;
                model.IsSNP = Material.IsSNP;
            }

            if (model.MaterialID01 > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode01, model.CompanyID);
                model.MaterialID01 = Material.ID;
            }
            if (model.MaterialID01 > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID01.Value);
                model.MaterialCode01 = Material.Code;
                model.MaterialName01 = Material.Name;
            }            
            
            if (model.BOMID > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.ECN = BOM.Code;
            }
            SQLHelper.InitializationNumber<ProductionOrderMaterial>(model);

            if (model.ParentID > 0 && model.MaterialID > 0)
            {
                var QuantityIndex = 1;
                foreach (PropertyInfo proQuantity in model.GetType().GetProperties())
                {
                    var QuantityIndexString = QuantityIndex.ToString();
                    if (QuantityIndex < 10)
                    {
                        QuantityIndexString = "0" + QuantityIndexString;
                    }
                    var QuantityName = "Quantity" + QuantityIndexString;
                    if (proQuantity.Name == QuantityName)
                    {
                        proQuantity.SetValue(model, 0, null);
                        var QuantityActualName = "QuantityActual" + QuantityIndexString;
                        foreach (PropertyInfo proQuantityActual in model.GetType().GetProperties())
                        {
                            if (proQuantityActual.Name == QuantityActualName)
                            {
                                proQuantityActual.SetValue(model, 0, null);
                                break;
                            }
                        }
                        var QuantityGAPName = "QuantityGAP" + QuantityIndexString;
                        foreach (PropertyInfo proQuantityGAP in model.GetType().GetProperties())
                        {
                            if (proQuantityGAP.Name == QuantityGAPName)
                            {
                                proQuantityGAP.SetValue(model, 0, null);
                                break;
                            }
                        }
                        var QuantityPOName = "QuantityPO" + QuantityIndexString;
                        foreach (PropertyInfo proQuantityPO in model.GetType().GetProperties())
                        {
                            if (proQuantityPO.Name == QuantityPOName)
                            {
                                proQuantityPO.SetValue(model, 0, null);
                                break;
                            }
                        }
                        QuantityIndex = QuantityIndex + 1;
                    }
                }
                var No = 0;
                var ListProductionOrderMaterial = _ProductionOrderMaterialRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID > 0 && o.MaterialID == model.MaterialID && o.Priority > GlobalHelper.SortOrder).OrderBy(o => o.Priority).ToList();
                if (ListProductionOrderMaterial.Count > 0)
                {
                    for (int i = 0; i < ListProductionOrderMaterial.Count; i++)
                    {
                        if (ListProductionOrderMaterial[i].ID == model.ID)
                        {
                            No = i;
                            i = ListProductionOrderMaterial.Count;
                        }
                    }
                    if (model.ID == 0 && No == 0)
                    {
                        No = ListProductionOrderMaterial.Count;
                    }
                }
                decimal? QuantityInventory = 0;

                model.QuantityWIP = model.QuantityWIP ?? GlobalHelper.InitializationNumber;
                model.QuantityStock = model.QuantityStock ?? GlobalHelper.InitializationNumber;
                model.QuantityInvoice = GlobalHelper.InitializationNumber;
                model.QuantityInventory = model.QuantityInventory ?? GlobalHelper.InitializationNumber;

                var ListInvoiceInput = _InvoiceInputRepository.GetByCondition(o => o.DateETA != null && o.Active == true && o.IsFuture == true && o.ProductionOrderID == model.ParentID).ToList();
                var ListInvoiceInputDetail = new List<InvoiceInputDetail>();
                if (ListInvoiceInput.Count > 0)
                {
                    var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).ToList();
                    ListInvoiceInputDetail = _InvoiceInputDetailRepository.GetByCondition(o => o.Active == true && o.MaterialID == model.MaterialID && o.ParentID != null && ListInvoiceInputID.Contains(o.ParentID.Value)).ToList();
                }

                var ListWarehouseInventory = new List<WarehouseInventory>();
                var ProductionOrder = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                if (ProductionOrder.ID > 0)
                {
                    var ListCategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == ProductionOrder.CustomerID && o.IsSync == true).ToList();
                    if (ListCategoryDepartment.Count > 0)
                    {
                        var ListCategoryDepartmentID = ListCategoryDepartment.Select(o => o.ID).ToList();
                        ListWarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => o.ParentID == model.MaterialID && o.CategoryDepartmentID != null && ListCategoryDepartmentID.Contains(o.CategoryDepartmentID.Value) && o.Year == 0 && o.Month == 0).ToList();
                    }
                }

                if (model.Priority == GlobalHelper.SortOrder)
                {
                    model.QuantityInventory = ListWarehouseInventory.Sum(o => o.Quantity00) ?? GlobalHelper.InitializationNumber;

                    if (model.QuantityStock > 0)
                    {
                        QuantityInventory = model.QuantityWIP + model.QuantityStock;
                    }
                    else
                    {
                        QuantityInventory = model.QuantityWIP + model.QuantityInventory;
                    }
                }
                else
                {
                    if (No == 0)
                    {
                        model.QuantityInventory = ListWarehouseInventory.Sum(o => o.Quantity00) ?? GlobalHelper.InitializationNumber;

                        if (model.QuantityStock > 0)
                        {
                            QuantityInventory = model.QuantityWIP + model.QuantityStock;
                        }
                        else
                        {
                            QuantityInventory = model.QuantityWIP + model.QuantityInventory;
                        }
                    }
                    else
                    {
                        try
                        {
                            var ProductionOrderMaterialPrevious = ListProductionOrderMaterial[No - 1];
                            var ProductionOrderMaterial = _ProductionOrderMaterialRepository.GetByID(ProductionOrderMaterialPrevious.ID);
                            if (ProductionOrderMaterial.ID > 0)
                            {
                                QuantityInventory = ProductionOrderMaterial.QuantityGAP00 ?? 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                    }
                }


                model.QuantityInventory00 = QuantityInventory;
                model.Quantity00 = GlobalHelper.InitializationNumber;
                model.Date00 = null;

                var ListProductionOrderDetail = new List<ProductionOrderDetail>();
                if (model.Priority == GlobalHelper.SortOrder)
                {
                    ListProductionOrderDetail = _ProductionOrderDetailRepository.GetByCondition(o => o.ParentID == model.ParentID.Value && o.MaterialID > 0).ToList();
                }
                else
                {
                    if (model.ProductionOrderDetailID > 0)
                    {
                        ListProductionOrderDetail = _ProductionOrderDetailRepository.GetByIDToList(model.ProductionOrderDetailID.Value);
                    }
                }
                foreach (var ProductionOrderDetail in ListProductionOrderDetail)
                {
                    var Index001 = 1;
                    foreach (PropertyInfo proProductionOrderDetail in ProductionOrderDetail.GetType().GetProperties())
                    {
                        var Index001String = Index001.ToString();
                        if (Index001 < 10)
                        {
                            Index001String = "0" + Index001String;
                        }
                        var ProductionOrderProductionPlanDateName = "Date" + Index001String;
                        if (proProductionOrderDetail.Name == ProductionOrderProductionPlanDateName)
                        {
                            foreach (PropertyInfo proProductionOrderMaterialDate in model.GetType().GetProperties())
                            {
                                var ProductionOrderMaterialDateName = "Date" + Index001String;
                                if (proProductionOrderMaterialDate.Name == ProductionOrderMaterialDateName)
                                {
                                    proProductionOrderMaterialDate.SetValue(model, proProductionOrderDetail.GetValue(ProductionOrderDetail), null);
                                    //Index001 = Index001 + 1;
                                }
                            }
                        }

                        var ProductionOrderProductionPlanQuantityName = "Quantity" + Index001String;
                        if (proProductionOrderDetail.Name == ProductionOrderProductionPlanQuantityName && proProductionOrderDetail.GetValue(ProductionOrderDetail) != null)
                        {
                            var ProductionOrderProductionPlanQuantity = (int)proProductionOrderDetail.GetValue(ProductionOrderDetail);
                            if (model.Priority == GlobalHelper.SortOrder)
                            {
                                ProductionOrderProductionPlanQuantity = 0;
                                foreach (var ProductionOrderProductionPlanSUB in ListProductionOrderDetail)
                                {
                                    try
                                    {
                                        foreach (PropertyInfo proQuantity in ProductionOrderProductionPlanSUB.GetType().GetProperties())
                                        {
                                            if (proQuantity.Name == ProductionOrderProductionPlanQuantityName)
                                            {
                                                ProductionOrderProductionPlanQuantity = ProductionOrderProductionPlanQuantity + (int)proQuantity.GetValue(ProductionOrderProductionPlanSUB);
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string mes = ex.Message;
                                    }
                                }
                            }
                            foreach (PropertyInfo proProductionOrderMaterial in model.GetType().GetProperties())
                            {
                                var Quantity = GlobalHelper.InitializationNumber;
                                var ProductionOrderMaterialQuantityPOName = "QuantityPO" + Index001String;
                                if (proProductionOrderMaterial.Name == ProductionOrderMaterialQuantityPOName)
                                {
                                    proProductionOrderMaterial.SetValue(model, ProductionOrderProductionPlanQuantity, null);
                                }
                                var ProductionOrderMaterialQuantityName = "Quantity" + Index001String;
                                if (proProductionOrderMaterial.Name == ProductionOrderMaterialQuantityName)
                                {
                                    var QuantityBOM = (decimal)(ProductionOrderProductionPlanQuantity * model.QuantityBOM);
                                    Quantity = (int)QuantityBOM;
                                    var DecimalPart = QuantityBOM - Quantity;
                                    if (DecimalPart > 0)
                                    {
                                        Quantity = Quantity + 1;
                                    }
                                    proProductionOrderMaterial.SetValue(model, Quantity, null);
                                    foreach (PropertyInfo proProductionOrderMaterialQuantityActual in model.GetType().GetProperties())
                                    {
                                        var ProductionOrderMaterialQuantityActualName = "QuantityActual" + Index001String;
                                        if (proProductionOrderMaterialQuantityActual.Name == ProductionOrderMaterialQuantityActualName)
                                        {
                                            try
                                            {
                                                if (No == 0)
                                                {
                                                    var DateName = "Date" + Index001String;
                                                    foreach (PropertyInfo proProductionOrderMaterialDate in model.GetType().GetProperties())
                                                    {
                                                        if (proProductionOrderMaterialDate.Name == DateName)
                                                        {
                                                            if (proProductionOrderMaterialDate.GetValue(model) != null)
                                                            {
                                                                var DateValue = (DateTime)proProductionOrderMaterialDate.GetValue(model);
                                                                if (DateValue != null)
                                                                {
                                                                    var ListInvoiceInputSub = ListInvoiceInput.Where(o => o.DateETA != null && o.DateETA.Value.Date == DateValue.Date).ToList();
                                                                    if (ListInvoiceInputSub.Count > 0)
                                                                    {
                                                                        var ListInvoiceInputSubID = ListInvoiceInputSub.Select(o => o.ID).ToList();
                                                                        var ListInvoiceInputDetailSub = ListInvoiceInputDetail.Where(o => o.ParentID != null && ListInvoiceInputSubID.Contains(o.ParentID.Value)).ToList();
                                                                        var QuantityInvoiceInput = ListInvoiceInputDetailSub.Sum(o => o.Quantity) ?? 0;
                                                                        model.QuantityInvoice = QuantityInvoiceInput;
                                                                        QuantityInventory = QuantityInventory + model.QuantityInvoice;
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                string mes = ex.Message;
                                            }
                                            if (model.Priority == GlobalHelper.SortOrder)
                                            {
                                                if (Index001String == "01")
                                                {
                                                    proProductionOrderMaterialQuantityActual.SetValue(model, (int)QuantityInventory, null);
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        var Index001SUB = Index001 - 1;
                                                        var Index001StringSub = Index001SUB.ToString();
                                                        if (Index001SUB < 10)
                                                        {
                                                            Index001StringSub = "0" + Index001StringSub;
                                                        }
                                                        var QuantityActual = 0;
                                                        foreach (PropertyInfo proProductionOrderProductionPlanMaterialQuantityActualSUB in model.GetType().GetProperties())
                                                        {
                                                            var ProductionOrderProductionPlanMaterialQuantityActualSUBName = "QuantityActual" + Index001StringSub;
                                                            if (proProductionOrderProductionPlanMaterialQuantityActualSUB.Name == ProductionOrderProductionPlanMaterialQuantityActualSUBName)
                                                            {
                                                                QuantityActual = (int)proProductionOrderProductionPlanMaterialQuantityActualSUB.GetValue(model);
                                                                break;
                                                            }
                                                        }
                                                        QuantityActual = QuantityActual - Quantity;
                                                        proProductionOrderMaterialQuantityActual.SetValue(model, (int)QuantityActual, null);
                                                        if (QuantityActual < Quantity && model.Date00 == null)
                                                        {
                                                            foreach (PropertyInfo proProductionOrderMaterialDateEnd in model.GetType().GetProperties())
                                                            {
                                                                var ProductionOrderMaterialDateName = "Date" + Index001String;
                                                                if (proProductionOrderMaterialDateEnd.Name == ProductionOrderMaterialDateName)
                                                                {
                                                                    model.Date00 = (DateTime)proProductionOrderMaterialDateEnd.GetValue(model);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        string mes = ex.Message;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                proProductionOrderMaterialQuantityActual.SetValue(model, (int)QuantityInventory, null);
                                                QuantityInventory = QuantityInventory - Quantity;
                                                if (QuantityInventory < 0 && model.Date00 == null)
                                                {
                                                    foreach (PropertyInfo proProductionOrderMaterialDateEnd in model.GetType().GetProperties())
                                                    {
                                                        var ProductionOrderMaterialDateName = "Date" + Index001String;
                                                        if (proProductionOrderMaterialDateEnd.Name == ProductionOrderMaterialDateName)
                                                        {
                                                            model.Date00 = (DateTime)proProductionOrderMaterialDateEnd.GetValue(model);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Index001 = Index001 + 1;
                        }
                    }
                }

            }

            if (model.Date00 != null)
            {
                var Index001 = 1;
                foreach (PropertyInfo proDate in model.GetType().GetProperties())
                {
                    var IndexString = Index001.ToString();
                    if (Index001 < 10)
                    {
                        IndexString = "0" + Index001;
                    }
                    var DateName = "Date" + IndexString;
                    if (proDate.Name == DateName)
                    {
                        try
                        {
                            if (proDate.GetValue(model) != null)
                            {
                                var DateValue = (DateTime)proDate.GetValue(model);
                                if (DateValue.Date >= model.Date00.Value.Date)
                                {
                                    foreach (PropertyInfo proActive in model.GetType().GetProperties())
                                    {
                                        var ActiveName = "Active" + IndexString;
                                        if (proActive.Name == ActiveName)
                                        {
                                            proActive.SetValue(model, true, null);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                        Index001 = Index001 + 1;
                    }
                }
            }
            else
            {
                var Index001 = 1;
                foreach (PropertyInfo proActive in model.GetType().GetProperties())
                {
                    var IndexString = Index001.ToString();
                    if (Index001 < 10)
                    {
                        IndexString = "0" + Index001;
                    }
                    var DateName = "Date" + IndexString;
                    var ActiveName = "Active" + IndexString;
                    if (proActive.Name == ActiveName)
                    {
                        proActive.SetValue(model, false, null);
                        Index001 = Index001 + 1;
                    }
                }
            }

            SQLHelper.InitializationDateTimeName<ProductionOrderMaterial>(model);
            SQLHelper.InitializationQuantityGAP<ProductionOrderMaterial>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderMaterial>(model);
            //SQLHelper.InitializationQuantityActual00<ProductionOrderMaterial>(model);

            model.QuantityInventory00 = model.QuantityInventory00 + model.QuantityInvoice;
            model.QuantityInventory00 = model.QuantityInventory00 ?? 0;
            model.QuantityGAP00 = (int)model.QuantityInventory00 - model.Quantity00;
        }
        public override async Task<BaseResult<ProductionOrderMaterial>> SaveAsync(BaseParameter<ProductionOrderMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderMaterial>();

            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ProductionOrderDetailID == BaseParameter.BaseModel.ProductionOrderDetailID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority == BaseParameter.BaseModel.Priority).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
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
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderMaterial>> SyncAsync(BaseParameter<ProductionOrderMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderMaterial>();
            await SyncProductionOrderMaterialAsync(BaseParameter);
            await SyncProductionOrderMaterialSumAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderMaterial>> SyncProductionOrderMaterialAsync(BaseParameter<ProductionOrderMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderMaterial>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.SortOrder > 1)
                            {
                                var ProductionOrderMaterial = new ProductionOrderMaterial();
                                ProductionOrderMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                ProductionOrderMaterial.MaterialID = GlobalHelper.InitializationNumber;
                                ProductionOrderMaterial.SortOrder = 1;
                                var ModelCheck = await GetByCondition(o => o.ParentID == ProductionOrderMaterial.ParentID && o.MaterialID == ProductionOrderMaterial.MaterialID && o.SortOrder == ProductionOrderMaterial.SortOrder).FirstOrDefaultAsync();
                                if (ModelCheck == null)
                                {
                                    foreach (PropertyInfo pro in BaseParameter.BaseModel.GetType().GetProperties())
                                    {
                                        if (pro.Name.Contains("Date") && !pro.Name.Contains("DatePO"))
                                        {
                                            foreach (PropertyInfo proDate in ProductionOrderMaterial.GetType().GetProperties())
                                            {
                                                if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO"))
                                                {
                                                    if (proDate.Name == pro.Name)
                                                    {
                                                        try
                                                        {
                                                            proDate.SetValue(ProductionOrderMaterial, pro.GetValue(BaseParameter.BaseModel), null);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            string mes = ex.Message;
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    await _ProductionOrderMaterialRepository.AddAsync(ProductionOrderMaterial);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderMaterial>> SyncProductionOrderMaterialSumAsync(BaseParameter<ProductionOrderMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderMaterial>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                if (BaseParameter.BaseModel.Priority > GlobalHelper.SortOrder)
                                {
                                    var ListProductionOrderMaterial = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority > GlobalHelper.SortOrder).OrderBy(o => o.Priority).ToListAsync();
                                    if (ListProductionOrderMaterial.Count > 0)
                                    {
                                        var ProductionOrderMaterialSUM = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority == GlobalHelper.SortOrder).FirstOrDefaultAsync();
                                        if (ProductionOrderMaterialSUM == null)
                                        {
                                            ProductionOrderMaterialSUM = ListProductionOrderMaterial[0];
                                            ProductionOrderMaterialSUM.ID = 0;
                                            ProductionOrderMaterialSUM.Priority = GlobalHelper.SortOrder;
                                        }
                                        BaseParameter.BaseModel = ProductionOrderMaterialSUM;
                                        await SaveAsync(BaseParameter);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

