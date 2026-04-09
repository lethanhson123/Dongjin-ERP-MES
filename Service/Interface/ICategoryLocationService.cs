namespace Service.Interface
{
    public interface ICategoryLocationService : IBaseService<CategoryLocation>
    {
        Task<BaseResult<CategoryLocation>> GetByCategoryDepartmentIDToListAsync(BaseParameter<CategoryLocation> BaseParameter);
        Task<BaseResult<CategoryLocation>> GetByParentID_CategoryLayerIDToListAsync(BaseParameter<CategoryLocation> BaseParameter);
        Task<BaseResult<CategoryLocation>> PrintAsync(BaseParameter<CategoryLocation> BaseParameter);
        Task<BaseResult<CategoryLocation>> PrintByCategoryDepartmentIDAsync(BaseParameter<CategoryLocation> BaseParameter);
        Task<BaseResult<CategoryLocation>> PrintByParentIDAsync(BaseParameter<CategoryLocation> BaseParameter);
        Task<BaseResult<CategoryLocation>> CreateAutoAsync(BaseParameter<CategoryLocation> BaseParameter);
    }
}

