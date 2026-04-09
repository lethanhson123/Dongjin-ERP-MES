namespace Service.Implement
{
    public class WarehouseRequestDetailService : BaseService<WarehouseRequestDetail, IWarehouseRequestDetailRepository>
    , IWarehouseRequestDetailService
    {
        private readonly IWarehouseRequestDetailRepository _WarehouseRequestDetailRepository;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IWarehouseStockRepository _WarehouseStockRepository;
        private readonly IWarehouseStockService _WarehouseStockService;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        public WarehouseRequestDetailService(IWarehouseRequestDetailRepository WarehouseRequestDetailRepository
            , IWarehouseRequestRepository WarehouseRequestRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , IWarehouseInventoryRepository WarehouseInventoryRepository
            , IWarehouseStockRepository WarehouseStockRepository
            , IWarehouseStockService WarehouseStockService
            , ICategoryDepartmentRepository categoryDepartmentRepository
            ) : base(WarehouseRequestDetailRepository)
        {
            _WarehouseRequestDetailRepository = WarehouseRequestDetailRepository;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _WarehouseInventoryRepository = WarehouseInventoryRepository;
            _WarehouseStockRepository = WarehouseStockRepository;
            _WarehouseStockService = WarehouseStockService;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
        }
        public override void InitializationSave(WarehouseRequestDetail model)
        {
            BaseInitialization(model);
            long? ProductionOrderID = 0;
            if (model.ParentID > 0)
            {
                var Parent = _WarehouseRequestRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.Date = Parent.Date;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                ProductionOrderID = Parent.ParentID;
            }
            if (model.CategoryUnitID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Name = model.CategoryUnitName;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID = CategoryUnit.ID;
                }
            }
            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }
            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
                model.MaterialID = Material.ID;
                model.MaterialName = Material.Code;
                model.QuantitySNP = model.QuantitySNP ?? Material.QuantitySNP;
                model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
                model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
                model.FileName = Material.CategoryLineName;                
                model.IsSNP = model.IsSNP ?? Material.IsSNP;
                if (model.QuantitySNP == null || model.QuantitySNP == 0)
                {
                    model.QuantitySNP = Material.QuantitySNP_TMMTIN_Last;
                }

                model.ProductName = model.ProductName ?? model.MaterialName;
                model.Date = model.Date ?? GlobalHelper.InitializationDateTime;




                model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
                model.QuantityInvoice = model.QuantityInvoice ?? model.Quantity;
                model.Price = model.Price ?? GlobalHelper.InitializationNumber;
                model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
                model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
                if (model.ID > 0)
                {
                    model.Total = model.Quantity * model.Price;
                }
                else
                {
                    model.Total = model.Total ?? model.Quantity * model.Price;
                }
                model.TotalTax = model.Total * model.Tax / 100;
                model.TotalDiscount = model.Total * model.Discount / 100;
                model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;

                int Year = model.Date.Value.Year;
                int Month = model.Date.Value.Month;
                //var CategoryDepartment = _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.IsSync == true).FirstOrDefault();
                //if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                //{

                //    var WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => o.Action == 1 && o.CompanyID == model.CompanyID && o.CategoryDepartmentID == CategoryDepartment.ID && o.ParentID == model.MaterialID && o.Year == Year && o.Month == Month).OrderByDescending(o => o.QuantityEnd).FirstOrDefault();
                //    if (WarehouseInventory == null)
                //    {
                //        Month = Month - 1;
                //        if (Month == 0)
                //        {
                //            Month = 12;
                //            Year = Year - 1;
                //        }
                //        WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => o.Action == 1 && o.CompanyID == model.CompanyID && o.CategoryDepartmentID == CategoryDepartment.ID && o.ParentID == model.MaterialID && o.Year == Year && o.Month == Month).OrderByDescending(o => o.QuantityEnd).FirstOrDefault();
                //    }
                //    if (WarehouseInventory != null && WarehouseInventory.ID > 0)
                //    {
                //        model.QuantityStock = WarehouseInventory.QuantityEnd;
                //    }
                //}
                Year = model.Date.Value.Year;
                Month = model.Date.Value.Month;
                //var WarehouseStock = _WarehouseStockRepository.GetByCondition(o => o.Action == 1 && o.CompanyID == model.CompanyID && o.ProductionOrderID == ProductionOrderID && o.ParentID == model.MaterialID && o.Year == Year && o.Month == Month).OrderByDescending(o => o.QuantityEnd).FirstOrDefault();
                //if (WarehouseStock == null)
                //{
                //    Month = Month - 1;
                //    if (Month == 0)
                //    {
                //        Month = 12;
                //        Year = Year - 1;
                //    }
                //    WarehouseStock = _WarehouseStockRepository.GetByCondition(o => o.Action == 1 && o.CompanyID == model.CompanyID && o.ProductionOrderID == ProductionOrderID && o.ParentID == model.MaterialID && o.Year == Year && o.Month == Month).OrderByDescending(o => o.QuantityEnd).FirstOrDefault();
                //}
                //if (WarehouseStock != null && WarehouseStock.ID > 0)
                //{
                //    model.QuantityBegin = WarehouseStock.QuantityEnd;
                //}
                model.QuantityStock = model.QuantityStock ?? GlobalHelper.InitializationNumber;
                model.QuantityBegin = model.QuantityBegin ?? GlobalHelper.InitializationNumber;
                model.QuantityEnd = model.QuantityBegin + model.Quantity - model.QuantityInvoice;
                model.QuantityGAP = model.QuantityInvoice - model.QuantityBegin;
            }
        }
        public override WarehouseRequestDetail SetModelByModelCheck(WarehouseRequestDetail Model, WarehouseRequestDetail ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    //if (Model.ID != ModelCheck.ID)
                    //{
                    //    Model.Quantity = Model.Quantity + ModelCheck.Quantity;
                    //}
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public override async Task<BaseResult<WarehouseRequestDetail>> SaveAsync(BaseParameter<WarehouseRequestDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequestDetail>();
            if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.ParentID > 0)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseRequestDetail>> SyncWarehouseStockAsync(BaseParameter<WarehouseRequestDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequestDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseRequestDetail>> GetByParentIDToListAsync(BaseParameter<WarehouseRequestDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseRequestDetail>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _WarehouseRequestDetailRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
                result.List = result.List.OrderByDescending(o => o.Quantity).ToList();
            }
            return result;
        }
    }
}

