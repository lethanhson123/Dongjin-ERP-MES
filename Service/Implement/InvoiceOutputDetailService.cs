namespace Service.Implement
{
    public class InvoiceOutputDetailService : BaseService<InvoiceOutputDetail, IInvoiceOutputDetailRepository>
    , IInvoiceOutputDetailService
    {
        private readonly IInvoiceOutputDetailRepository _InvoiceOutputDetailRepository;
        private readonly IInvoiceOutputRepository _InvoiceOutputRepository;
        private readonly IMaterialRepository _MaterialRepository;

        public InvoiceOutputDetailService(IInvoiceOutputDetailRepository InvoiceOutputDetailRepository
            , IInvoiceOutputRepository InvoiceOutputRepository
            , IMaterialRepository MaterialRepository

            ) : base(InvoiceOutputDetailRepository)
        {
            _InvoiceOutputDetailRepository = InvoiceOutputDetailRepository;
            _InvoiceOutputRepository = InvoiceOutputRepository;
            _MaterialRepository = MaterialRepository;

        }
        public override void Initialization(InvoiceOutputDetail model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _InvoiceOutputRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
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
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialName = Material.Code;
                model.Display = Material.Name;
            }
            model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
            model.Price = model.Price ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.Total = model.Quantity * model.Price;
            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override async Task<BaseResult<InvoiceOutputDetail>> SaveAsync(BaseParameter<InvoiceOutputDetail> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutputDetail>();
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            if (result.Count > 0)
            {
                await SyncAsync(BaseParameter);
            }
            return result;
        }
        public override async Task<BaseResult<InvoiceOutputDetail>> RemoveAsync(BaseParameter<InvoiceOutputDetail> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutputDetail>();
            result.Count = await _InvoiceOutputDetailRepository.RemoveAsync(BaseParameter.ID);
            if (result.Count > 0)
            {
                await SyncAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceOutputDetail>> SyncAsync(BaseParameter<InvoiceOutputDetail> BaseParameter)
        {
            var result = new BaseResult<InvoiceOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ParentID > 0)
                {
                    var Parent = await _InvoiceOutputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                    if (Parent != null)
                    {
                        var List = await _InvoiceOutputDetailRepository.GetByParentIDToListAsync(Parent.ID);
                        if (List != null && List.Count > 0)
                        {
                            Parent.Total = List.Sum(x => x.Total);
                            Parent.TotalTax = List.Sum(x => x.TotalTax);
                            Parent.TotalDiscount = List.Sum(x => x.TotalDiscount);
                            Parent.TotalFinal = List.Sum(x => x.TotalFinal);
                            await _InvoiceOutputRepository.UpdateAsync(Parent);
                        }
                    }
                }
            }
            return result;
        }
    }
}

