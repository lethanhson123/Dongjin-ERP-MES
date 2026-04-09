namespace Service.Interface
{
    public interface IInvoiceOutputService : IBaseService<InvoiceOutput>
    {
        Task<BaseResult<InvoiceOutput>> GetByMembershipIDToListAsync(BaseParameter<InvoiceOutput> BaseParameter);
        Task<BaseResult<InvoiceOutput>> GetByMembershipID_ActiveToListAsync(BaseParameter<InvoiceOutput> BaseParameter);
    }
}

