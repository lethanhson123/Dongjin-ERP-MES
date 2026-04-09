namespace Service.Interface
{
    public interface IMaterialConvertService : IBaseService<MaterialConvert>
    {
        Task<BaseResult<MaterialConvert>> CreateAutoAsync(BaseParameter<MaterialConvert> BaseParameter);
    }
}

