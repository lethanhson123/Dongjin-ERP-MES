namespace Service.Interface
{
    public interface ICategoryLocationMaterialService : IBaseService<CategoryLocationMaterial>
    {
        Task<BaseResult<CategoryLocationMaterial>> CreateAutoAsync(BaseParameter<CategoryLocationMaterial> BaseParameter);
    }
}

