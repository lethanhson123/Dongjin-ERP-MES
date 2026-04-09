namespace Service.Interface
{
    public interface INotificationService : IBaseService<CategoryCompany>
    {
        Task<BaseResult<Notification>> CreateWarehouseRequestAsync(BaseParameter<Notification> BaseParameter);
        Task<BaseResult<Notification>> CreateWarehouseOutputDetailBarcodeFindAsync(BaseParameter<Notification> BaseParameter);
    }
}

