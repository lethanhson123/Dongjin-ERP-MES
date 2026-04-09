namespace Service.Implement
{
    public class ProductionOrderProductionPlanMaterialService : BaseService<ProductionOrderProductionPlanMaterial, IProductionOrderProductionPlanMaterialRepository>
    , IProductionOrderProductionPlanMaterialService
    {
        private readonly IProductionOrderProductionPlanMaterialRepository _ProductionOrderProductionPlanMaterialRepository;
        private readonly IProductionOrderProductionPlanRepository _ProductionOrderProductionPlanRepository;
        private readonly IProductionOrderMaterialRepository _ProductionOrderMaterialRepository;
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
        public ProductionOrderProductionPlanMaterialService(IProductionOrderProductionPlanMaterialRepository ProductionOrderProductionPlanMaterialRepository
            , IProductionOrderProductionPlanRepository ProductionOrderProductionPlanRepository
            , IProductionOrderMaterialRepository ProductionOrderMaterialRepository
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

            ) : base(ProductionOrderProductionPlanMaterialRepository)
        {
            _ProductionOrderProductionPlanMaterialRepository = ProductionOrderProductionPlanMaterialRepository;
            _ProductionOrderProductionPlanRepository = ProductionOrderProductionPlanRepository;
            _ProductionOrderMaterialRepository = ProductionOrderMaterialRepository;
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
        public override void Initialization(ProductionOrderProductionPlanMaterial model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _ProductionOrderRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Code = Parent.Code;
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
            SQLHelper.InitializationNumber<ProductionOrderProductionPlanMaterial>(model);
            if (model.ParentID > 0 && model.ProductionOrderDetailID > 0 && model.MaterialID01 > 0 && model.MaterialID > 0)
            {
                var ProductionOrderMaterial = _ProductionOrderMaterialRepository.GetByCondition(o => o.ParentID == model.ParentID && o.ProductionOrderDetailID == model.ProductionOrderDetailID && o.MaterialID == model.MaterialID).FirstOrDefault();
                if (ProductionOrderMaterial != null && ProductionOrderMaterial.ID > 0)
                {
                    model.QuantityGAP00ProductionOrderDetail = ProductionOrderMaterial.QuantityGAP00;
                }
            }


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

                var ListProductionOrderProductionPlanMaterial = _ProductionOrderProductionPlanMaterialRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID > 0 && o.MaterialID == model.MaterialID && o.Priority > GlobalHelper.SortOrder).OrderBy(o => o.Priority).ToList();
                if (ListProductionOrderProductionPlanMaterial.Count > 0)
                {
                    for (int i = 0; i < ListProductionOrderProductionPlanMaterial.Count; i++)
                    {
                        if (ListProductionOrderProductionPlanMaterial[i].ID == model.ID)
                        {
                            No = i;
                            i = ListProductionOrderProductionPlanMaterial.Count;
                        }
                    }
                    if (No == 0 && model.ID == 0)
                    {
                        No = ListProductionOrderProductionPlanMaterial.Count;
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
                            var ProductionOrderProductionPlanPrevious = ListProductionOrderProductionPlanMaterial[No - 1];
                            var ProductionOrderProductionPlanMaterial = _ProductionOrderProductionPlanMaterialRepository.GetByID(ProductionOrderProductionPlanPrevious.ID);
                            if (ProductionOrderProductionPlanMaterial.ID > 0)
                            {
                                QuantityInventory = ProductionOrderProductionPlanMaterial.QuantityGAP00 ?? 0;
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
                model.QuantityActual00 = GlobalHelper.InitializationNumber;
                model.QuantityGAP00 = GlobalHelper.InitializationNumber;
                model.Date00 = null;

                var ListProductionOrderProductionPlan = new List<ProductionOrderProductionPlan>();
                if (model.Priority == GlobalHelper.SortOrder)
                {
                    ListProductionOrderProductionPlan = _ProductionOrderProductionPlanRepository.GetByCondition(o => o.ParentID == model.ParentID.Value && o.MaterialID > 0).ToList();
                }
                else
                {
                    if (model.ProductionOrderProductionPlanID > 0)
                    {
                        ListProductionOrderProductionPlan = _ProductionOrderProductionPlanRepository.GetByIDToList(model.ProductionOrderProductionPlanID.Value);
                    }
                }

                foreach (var ProductionOrderProductionPlan in ListProductionOrderProductionPlan)
                {
                    var Index001 = 1;
                    foreach (PropertyInfo proProductionOrderProductionPlan in ProductionOrderProductionPlan.GetType().GetProperties())
                    {
                        var Index001String = Index001.ToString();
                        if (Index001 < 10)
                        {
                            Index001String = "0" + Index001String;
                        }
                        var ProductionOrderProductionPlanDateName = "Date" + Index001String;
                        if (proProductionOrderProductionPlan.Name == ProductionOrderProductionPlanDateName)
                        {
                            foreach (PropertyInfo proProductionOrderProductionPlanMaterialDate in model.GetType().GetProperties())
                            {
                                var ProductionOrderProductionPlanMaterialDateName = "Date" + Index001String;
                                if (proProductionOrderProductionPlanMaterialDate.Name == ProductionOrderProductionPlanMaterialDateName)
                                {
                                    proProductionOrderProductionPlanMaterialDate.SetValue(model, proProductionOrderProductionPlan.GetValue(ProductionOrderProductionPlan), null);
                                    //Index001 = Index001 + 1;
                                }
                            }
                        }

                        var ProductionOrderProductionPlanQuantityName = "Quantity" + Index001String;
                        if (proProductionOrderProductionPlan.Name == ProductionOrderProductionPlanQuantityName && proProductionOrderProductionPlan.GetValue(ProductionOrderProductionPlan) != null)
                        {
                            var ProductionOrderProductionPlanQuantity = (int)proProductionOrderProductionPlan.GetValue(ProductionOrderProductionPlan);
                            if (model.Priority == GlobalHelper.SortOrder)
                            {
                                ProductionOrderProductionPlanQuantity = 0;
                                foreach (var ProductionOrderProductionPlanSUB in ListProductionOrderProductionPlan)
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
                            foreach (PropertyInfo proProductionOrderProductionPlanMaterial in model.GetType().GetProperties())
                            {
                                var Quantity = GlobalHelper.InitializationNumber;
                                var ProductionOrderProductionPlanMaterialQuantityPOName = "QuantityPO" + Index001String;
                                if (proProductionOrderProductionPlanMaterial.Name == ProductionOrderProductionPlanMaterialQuantityPOName)
                                {
                                    proProductionOrderProductionPlanMaterial.SetValue(model, ProductionOrderProductionPlanQuantity, null);
                                }
                                var ProductionOrderProductionPlanMaterialQuantityName = "Quantity" + Index001String;
                                if (proProductionOrderProductionPlanMaterial.Name == ProductionOrderProductionPlanMaterialQuantityName)
                                {
                                    var QuantityBOM = (decimal)(ProductionOrderProductionPlanQuantity * model.QuantityBOM);
                                    Quantity = (int)QuantityBOM;
                                    var DecimalPart = QuantityBOM - Quantity;
                                    if (DecimalPart > 0)
                                    {
                                        Quantity = Quantity + 1;
                                    }
                                    proProductionOrderProductionPlanMaterial.SetValue(model, Quantity, null);
                                    foreach (PropertyInfo proProductionOrderProductionPlanMaterialQuantityActual in model.GetType().GetProperties())
                                    {
                                        var ProductionOrderProductionPlanMaterialQuantityActualName = "QuantityActual" + Index001String;
                                        if (proProductionOrderProductionPlanMaterialQuantityActual.Name == ProductionOrderProductionPlanMaterialQuantityActualName)
                                        {
                                            try
                                            {
                                                if (No == 0)
                                                {
                                                    var DateName = "Date" + Index001String;
                                                    foreach (PropertyInfo proProductionOrderProductionPlanMaterialDate in model.GetType().GetProperties())
                                                    {
                                                        if (proProductionOrderProductionPlanMaterialDate.Name == DateName)
                                                        {
                                                            if (proProductionOrderProductionPlanMaterialDate.GetValue(model) != null)
                                                            {
                                                                var DateValue = (DateTime)proProductionOrderProductionPlanMaterialDate.GetValue(model);
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
                                                    proProductionOrderProductionPlanMaterialQuantityActual.SetValue(model, (int)QuantityInventory, null);
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
                                                        proProductionOrderProductionPlanMaterialQuantityActual.SetValue(model, (int)QuantityActual, null);
                                                        if (QuantityActual < Quantity && model.Date00 == null)
                                                        {
                                                            foreach (PropertyInfo proProductionOrderProductionPlanMaterialDateEnd in model.GetType().GetProperties())
                                                            {
                                                                var ProductionOrderProductionPlanMaterialDateName = "Date" + Index001String;
                                                                if (proProductionOrderProductionPlanMaterialDateEnd.Name == ProductionOrderProductionPlanMaterialDateName)
                                                                {
                                                                    model.Date00 = (DateTime)proProductionOrderProductionPlanMaterialDateEnd.GetValue(model);
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
                                                proProductionOrderProductionPlanMaterialQuantityActual.SetValue(model, (int)QuantityInventory, null);
                                                QuantityInventory = QuantityInventory - Quantity;
                                                if (QuantityInventory < 0 && model.Date00 == null)
                                                {
                                                    foreach (PropertyInfo proProductionOrderProductionPlanMaterialDateEnd in model.GetType().GetProperties())
                                                    {
                                                        var ProductionOrderProductionPlanMaterialDateName = "Date" + Index001String;
                                                        if (proProductionOrderProductionPlanMaterialDateEnd.Name == ProductionOrderProductionPlanMaterialDateName)
                                                        {
                                                            model.Date00 = (DateTime)proProductionOrderProductionPlanMaterialDateEnd.GetValue(model);
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

            SQLHelper.InitializationDateTimeName<ProductionOrderProductionPlanMaterial>(model);
            SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanMaterial>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanMaterial>(model);
            //SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanMaterial>(model);

            model.QuantityInventory00 = model.QuantityInventory00 + model.QuantityInvoice;
            model.QuantityInventory00 = model.QuantityInventory00 ?? 0;
            model.QuantityGAP00 = (int)model.QuantityInventory00 - model.Quantity00;
        }
        public override async Task<BaseResult<ProductionOrderProductionPlanMaterial>> SaveAsync(BaseParameter<ProductionOrderProductionPlanMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ProductionOrderDetailID == BaseParameter.BaseModel.ProductionOrderDetailID && o.ProductionOrderProductionPlanID == BaseParameter.BaseModel.ProductionOrderProductionPlanID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority == BaseParameter.BaseModel.Priority).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlanMaterial>> SyncAsync(BaseParameter<ProductionOrderProductionPlanMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
            await SyncProductionOrderProductionPlanMaterialAsync(BaseParameter);
            await SyncProductionOrderProductionPlanMaterialSumAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanMaterial>> SyncProductionOrderProductionPlanMaterialAsync(BaseParameter<ProductionOrderProductionPlanMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
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
                                var ProductionOrderProductionPlanMaterial = new ProductionOrderProductionPlanMaterial();
                                ProductionOrderProductionPlanMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                ProductionOrderProductionPlanMaterial.MaterialID = GlobalHelper.InitializationNumber;
                                ProductionOrderProductionPlanMaterial.SortOrder = 1;
                                var ModelCheck = await GetByCondition(o => o.ParentID == ProductionOrderProductionPlanMaterial.ParentID && o.MaterialID == ProductionOrderProductionPlanMaterial.MaterialID && o.SortOrder == ProductionOrderProductionPlanMaterial.SortOrder).FirstOrDefaultAsync();
                                if (ModelCheck == null)
                                {
                                    foreach (PropertyInfo pro in BaseParameter.BaseModel.GetType().GetProperties())
                                    {
                                        if (pro.Name.Contains("Date") && !pro.Name.Contains("DatePO"))
                                        {
                                            foreach (PropertyInfo proDate in ProductionOrderProductionPlanMaterial.GetType().GetProperties())
                                            {
                                                if (proDate.Name.Contains("Date") && !proDate.Name.Contains("DatePO"))
                                                {
                                                    if (proDate.Name == pro.Name)
                                                    {
                                                        try
                                                        {
                                                            proDate.SetValue(ProductionOrderProductionPlanMaterial, pro.GetValue(BaseParameter.BaseModel), null);
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
                                    await _ProductionOrderProductionPlanMaterialRepository.AddAsync(ProductionOrderProductionPlanMaterial);
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
        public virtual async Task<BaseResult<ProductionOrderProductionPlanMaterial>> SyncProductionOrderProductionPlanMaterialSumAsync(BaseParameter<ProductionOrderProductionPlanMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
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
                                    var ListProductionOrderProductionPlanMaterial = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority > GlobalHelper.SortOrder).OrderBy(o => o.Priority).ToListAsync();
                                    if (ListProductionOrderProductionPlanMaterial.Count > 0)
                                    {
                                        var ProductionOrderProductionPlanMaterialSUM = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Priority == GlobalHelper.SortOrder).FirstOrDefaultAsync();
                                        if (ProductionOrderProductionPlanMaterialSUM == null)
                                        {
                                            ProductionOrderProductionPlanMaterialSUM = ListProductionOrderProductionPlanMaterial[0];
                                            ProductionOrderProductionPlanMaterialSUM.ID = 0;
                                            ProductionOrderProductionPlanMaterialSUM.Priority = GlobalHelper.SortOrder;
                                        }
                                        BaseParameter.BaseModel = ProductionOrderProductionPlanMaterialSUM;
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
        public override async Task<BaseResult<ProductionOrderProductionPlanMaterial>> GetByParentIDToListAsync(BaseParameter<ProductionOrderProductionPlanMaterial> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanMaterial>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _ProductionOrderProductionPlanMaterialRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID.Value).OrderBy(o => o.MaterialCode).ThenBy(o => o.Priority).ToListAsync();
            }
            return result;
        }
    }
}

