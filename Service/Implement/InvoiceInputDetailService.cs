namespace Service.Implement
{
    public class InvoiceInputDetailService : BaseService<InvoiceInputDetail, IInvoiceInputDetailRepository>
    , IInvoiceInputDetailService
    {
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        public InvoiceInputDetailService(IInvoiceInputDetailRepository InvoiceInputDetailRepository
            , IInvoiceInputRepository InvoiceInputRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            ) : base(InvoiceInputDetailRepository)
        {
            _InvoiceInputDetailRepository = InvoiceInputDetailRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
        }
        public override void InitializationSave(InvoiceInputDetail model)
        {
            BaseInitialization(model);
            long? CompanyID = 0;
            if (model.ParentID > 0)
            {
                var Parent = _InvoiceInputRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                CompanyID = Parent.SupplierID;
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
            var Material = new Material();
            switch (CompanyID)
            {
                case 19:
                    Material = _MaterialRepository.GetByCondition(o => o.Active == true && o.CompanyID == model.CompanyID && o.Code == model.MaterialName).FirstOrDefault();
                    if (Material == null)
                    {
                        Material = new Material();
                        Material.Active = true;
                        Material.Code = model.MaterialName;
                        Material.CompanyID = model.CompanyID;
                        _MaterialRepository.Add(Material);
                    }
                    break;
                default:
                    Material = _MaterialRepository.GetByDescription_CompanyID_QuantitySNP_Name(model.MaterialName, model.CompanyID, model.QuantitySNP, model.Display);
                    break;
            }
            if (Material == null)
            {
                Material = new Material();
            }
            model.MaterialID = Material.ID;
            if (model.MaterialID > 0)
            {
                Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialID = Material.ID;
                model.MaterialName = Material.Code;
                model.Display = Material.Name;
                model.QuantitySNP = model.QuantitySNP ?? Material.QuantitySNP;
                if (Material.CreateDate != null && Material.CreateDate.Value == GlobalHelper.InitializationDateTime.Date)
                {
                    model.IsNew = true;
                }
                model.CategoryFamilyID = Material.CategoryFamilyID;
                model.CategoryFamilyName = Material.CategoryFamilyName;                
                model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;

            }
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
                if (model.Total == null)
                {
                    model.Total = model.Quantity * model.Price;
                }
            }
            model.TotalTax = model.Total * (model.Tax / 100);
            model.TotalDiscount = model.Total * (model.Discount / 100);
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override InvoiceInputDetail SetModelByModelCheck(InvoiceInputDetail Model, InvoiceInputDetail ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                    Model.Quantity = Model.Quantity + ModelCheck.Quantity;
                    Model.QuantityInvoice = Model.QuantityInvoice + ModelCheck.QuantityInvoice;
                    Model.Total = Model.Total + ModelCheck.Total;
                }
            }
            return Model;
        }
        public override async Task<BaseResult<InvoiceInputDetail>> SaveAsync(BaseParameter<InvoiceInputDetail> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputDetail>();
            if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.ParentID > 0)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.Description == BaseParameter.BaseModel.Description).FirstOrDefaultAsync();
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
                    await SyncAsync(result);
                }
            }
            return result;
        }
        public override async Task<BaseResult<InvoiceInputDetail>> RemoveAsync(BaseParameter<InvoiceInputDetail> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputDetail>();
            result.BaseModel = await _InvoiceInputDetailRepository.GetByIDAsync(BaseParameter.ID);
            result.Count = await _InvoiceInputDetailRepository.RemoveAsync(BaseParameter.ID);
            if (result.Count > 0)
            {
                await SyncAsync(result);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInputDetail>> SyncAsync(BaseResult<InvoiceInputDetail> BaseResult)
        {
            var result = new BaseResult<InvoiceInputDetail>();
            if (BaseResult.BaseModel != null)
            {
                if (BaseResult.BaseModel.ParentID > 0)
                {
                    var Parent = await _InvoiceInputRepository.GetByIDAsync(BaseResult.BaseModel.ParentID.Value);
                    if (Parent != null)
                    {
                        var List = await _InvoiceInputDetailRepository.GetByParentIDToListAsync(Parent.ID);
                        if (List != null && List.Count > 0)
                        {
                            Parent.Total = List.Sum(x => x.Total);
                            Parent.TotalQuantity = List.Sum(x => x.Quantity);
                            await _InvoiceInputRepository.UpdateAsync(Parent);
                        }
                    }
                }
            }
            return result;
        }
    }
}

