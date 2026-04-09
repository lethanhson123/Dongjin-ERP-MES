namespace Service.Implement
{
    public class CategoryTermService : BaseService<CategoryTerm, ICategoryTermRepository>
    , ICategoryTermService
    {
        private readonly ICategoryTermRepository _CategoryTermRepository;
        public CategoryTermService(ICategoryTermRepository CategoryTermRepository) : base(CategoryTermRepository)
        {
            _CategoryTermRepository = CategoryTermRepository;
        }
    }
}

