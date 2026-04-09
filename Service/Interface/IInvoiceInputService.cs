namespace Service.Interface
{
    public interface IInvoiceInputService : IBaseService<InvoiceInput>
    {
        Task<BaseResult<InvoiceInput>> GetByActive_IsFutureToListAsync(BaseParameter<InvoiceInput> BaseParameter);
        Task<BaseResult<InvoiceInput>> GetByMembershipIDToListAsync(BaseParameter<InvoiceInput> BaseParameter);
        Task<BaseResult<InvoiceInput>> GetByMembershipID_ActiveToListAsync(BaseParameter<InvoiceInput> BaseParameter);
        Task<BaseResult<InvoiceInput>> GetByCompanyID_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter);
        Task<BaseResult<InvoiceInput>> GetByCompanyID_Year_Month_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter);
        Task<BaseResult<InvoiceInput>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter);
    }
}

