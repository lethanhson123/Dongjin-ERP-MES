namespace Service.Interface
{
    public interface IInvoiceInputInventoryService : IBaseService<InvoiceInputInventory>
    {
        Task<BaseResult<InvoiceInputInventory>> CreateAutoAsync(BaseParameter<InvoiceInputInventory> BaseParameter);
        Task<BaseResult<InvoiceInputInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter<InvoiceInputInventory> BaseParameter);        
    }
}

