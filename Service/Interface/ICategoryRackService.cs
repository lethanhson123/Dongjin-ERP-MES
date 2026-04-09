namespace Service.Interface
{
    public interface ICategoryRackService : IBaseService<CategoryRack>
    {
        Task<BaseResult<CategoryRack>> GetByParentIDAndCompanyIDAndActiveToListAsync(BaseParameter<CategoryRack> BaseParameter);
    }
}

