namespace Service.Interface
{
    public interface ICategoryMenuService : IBaseService<CategoryMenu>
    {
        Task<BaseResult<CategoryMenu>> GetByMembershipID_ActiveToListAsync(BaseParameter<CategoryMenu> BaseParameter);
    }
}

