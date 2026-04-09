namespace Repository.Interface
{
    public interface IWarehouseRequestDetailRepository : IBaseRepository<WarehouseRequestDetail>
    {
        Task<int> AddFromMESAsync(WarehouseRequestDetail model);
    }
}

