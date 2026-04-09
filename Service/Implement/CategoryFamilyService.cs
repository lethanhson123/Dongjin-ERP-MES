namespace Service.Implement
{
    public class CategoryFamilyService : BaseService<CategoryFamily, ICategoryFamilyRepository>
    , ICategoryFamilyService
    {
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;
        public CategoryFamilyService(ICategoryFamilyRepository CategoryFamilyRepository) : base(CategoryFamilyRepository)
        {
            _CategoryFamilyRepository = CategoryFamilyRepository;
        }
        public override async Task<BaseResult<CategoryFamily>> SaveAsync(BaseParameter<CategoryFamily> BaseParameter)
        {
            var result = new BaseResult<CategoryFamily>();
            if (BaseParameter.BaseModel != null)
            {
                var ModelCheck = await GetByCondition(o => o.Name == BaseParameter.BaseModel.Name).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
    }
}

