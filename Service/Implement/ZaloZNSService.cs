namespace Service.Implement
{
    public class ZaloZNSService : BaseService<ZaloZNS, IZaloZNSRepository>
    , IZaloZNSService
    {
        private readonly IZaloZNSRepository _ZaloZNSRepository;
        private readonly IZaloTokenService _ZaloTokenService;
        public ZaloZNSService(IZaloZNSRepository ZaloZNSRepository, IZaloTokenService zaloTokenService) : base(ZaloZNSRepository)
        {
            _ZaloZNSRepository = ZaloZNSRepository;
            _ZaloTokenService = zaloTokenService;
        }

        public override void Initialization(ZaloZNS model)
        {
            BaseInitialization(model);

            model.Note = model.Note ?? GlobalHelper.ZaloZNSAPIURL;
        }
        public virtual async Task<BaseResult<ZaloZNS>> SendZaloTuyenDungCongNhan2026Async(BaseParameter<ZaloZNS> BaseParameter)
        {
            var result = new BaseResult<ZaloZNS>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                var BaseParameterZaloToken = new BaseParameter<ZaloToken>();
                var BaseResultZaloToken = await _ZaloTokenService.GetLatestAsync(BaseParameterZaloToken);
                if (BaseResultZaloToken.BaseModel != null && BaseResultZaloToken.BaseModel.ID > 0)
                {
                    ZaloToken ZaloToken = BaseResultZaloToken.BaseModel;
                    if (!string.IsNullOrEmpty(ZaloToken.OAAccessToken))
                    {
                        ZaloZNSDataRequest ZaloZNSDataRequest = new ZaloZNSDataRequest();
                        if (BaseParameter.SearchString.Length == 10)
                        {
                            ZaloZNSDataRequest.phone = "84" + BaseParameter.SearchString.Substring(1);
                        }
                        if (BaseParameter.SearchString.Length == 9)
                        {
                            ZaloZNSDataRequest.phone = "84" + BaseParameter.SearchString;
                        }
                        if (!string.IsNullOrEmpty(ZaloZNSDataRequest.phone))
                        {
                            ZaloZNSDataRequest.template_id = "556163";
                            ZaloZNSDataRequest.tracking_id = GlobalHelper.InitializationGUICode;
                            template_data template_data = new template_data();
                            ZaloZNSDataRequest.template_data = template_data;
                            await ZaloHelper.ZNSSendAsync(ZaloToken.OAAccessToken, ZaloZNSDataRequest);
                        }
                    }
                }
            }
            return result;
        }
    }
}

