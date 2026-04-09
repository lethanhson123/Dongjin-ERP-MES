namespace Service.Interface
{
    public interface IZaloZNSService : IBaseService<ZaloZNS>
    {
        Task<BaseResult<ZaloZNS>> SendZaloTuyenDungCongNhan2026Async(BaseParameter<ZaloZNS> BaseParameter);
    }
}

