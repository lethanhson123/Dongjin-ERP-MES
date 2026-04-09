namespace Service.Implement
{
    public class ZaloTokenService : BaseService<ZaloToken, IZaloTokenRepository>
    , IZaloTokenService
    {
        private readonly IZaloTokenRepository _ZaloTokenRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ZaloTokenService(IZaloTokenRepository ZaloTokenRepository
            , IMembershipRepository membershipRepository
            , IWebHostEnvironment webHostEnvironment
            
            ) : base(ZaloTokenRepository)
        {
            _ZaloTokenRepository = ZaloTokenRepository;
            _MembershipRepository = membershipRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(ZaloToken model)
        {
            string folderPathRoot = Path.Combine(_WebHostEnvironment.WebRootPath, model.GetType().Name);
            bool isFolderExists = System.IO.Directory.Exists(folderPathRoot);
            if (!isFolderExists)
            {
                System.IO.Directory.CreateDirectory(folderPathRoot);
            }
            string fileName = model.GetType().Name + ".json";
            string path = Path.Combine(folderPathRoot, fileName);
            bool isFileExists = System.IO.File.Exists(path);
            if (!isFileExists)
            {
                var List = GetAllToList();
                string json = JsonConvert.SerializeObject(List);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(json);
                    }
                }
            }
        }
        public override void Initialization(ZaloToken model)
        {
            BaseInitialization(model);

            model.Note = model.Note ?? GlobalHelper.ZaloZNSAPIURL;
            model.URL = model.URL ?? GlobalHelper.ZaloRefreshTokenAPIURL;
        }
        public virtual async Task<BaseResult<ZaloToken>> SendTemplateAsync(BaseParameter<ZaloToken> BaseParameter)
        {
            var result = new BaseResult<ZaloToken>();
            if (BaseParameter.ID > 0)
            {
                var Membership = await _MembershipRepository.GetByIDAsync(BaseParameter.ID);
                if (Membership.ID > 0 && !string.IsNullOrEmpty(Membership.Phone))
                {
                    var BaseParameterZaloToken = new BaseParameter<ZaloToken>();
                    var BaseResultZaloToken = await GetLatestAsync(BaseParameterZaloToken);
                    if (BaseResultZaloToken.BaseModel != null && BaseResultZaloToken.BaseModel.ID > 0)
                    {
                        ZaloToken ZaloToken = BaseResultZaloToken.BaseModel;
                        if (!string.IsNullOrEmpty(ZaloToken.OAAccessToken))
                        {
                            ZaloZNSDataRequest ZaloZNSDataRequest = new ZaloZNSDataRequest();
                            if (Membership.Phone.Length == 10)
                            {
                                ZaloZNSDataRequest.phone = "84" + Membership.Phone.Substring(1);
                            }
                            if (Membership.Phone.Length == 9)
                            {
                                ZaloZNSDataRequest.phone = "84" + Membership.Phone;
                            }
                            if (!string.IsNullOrEmpty(ZaloZNSDataRequest.phone))
                            {
                                ZaloZNSDataRequest.template_id = GlobalHelper.ZaloTemplateID;
                                ZaloZNSDataRequest.tracking_id = GlobalHelper.InitializationGUICode;
                                template_data template_data = new template_data();
                                template_data.membership_code = Membership.UserName;
                                template_data.membership_name = Membership.Name;
                                template_data.output_id = "80";
                                template_data.output_code = "KGM-162025 a-CUT-02";
                                template_data.warehouse_id = template_data.output_id;
                                result.BaseModel = new ZaloToken();
                                result.BaseModel.Date = DateTime.Now;
                                if (result.BaseModel.Date == null)
                                {
                                    template_data.output_date = GlobalHelper.InitializationString;
                                }
                                else
                                {
                                    template_data.output_date = result.BaseModel.Date.Value.ToString("HH:mm:ss dd/MM/yyyy");
                                }
                                if (!string.IsNullOrEmpty(template_data.membership_code) && template_data.membership_code.Length > 30)
                                {
                                    template_data.membership_code = template_data.membership_code.Substring(0, 30);
                                }
                                if (!string.IsNullOrEmpty(template_data.membership_name) && template_data.membership_name.Length > 30)
                                {
                                    template_data.membership_name = template_data.membership_name.Substring(0, 30);
                                }
                                if (!string.IsNullOrEmpty(template_data.output_id) && template_data.output_id.Length > 30)
                                {
                                    template_data.output_id = template_data.output_id.Substring(0, 30);
                                }
                                if (!string.IsNullOrEmpty(template_data.output_code) && template_data.output_code.Length > 30)
                                {
                                    template_data.output_code = template_data.output_code.Substring(0, 30);
                                }
                                if (!string.IsNullOrEmpty(template_data.warehouse_id) && template_data.warehouse_id.Length > 30)
                                {
                                    template_data.warehouse_id = template_data.warehouse_id.Substring(0, 30);
                                }
                                if (!string.IsNullOrEmpty(template_data.output_date) && template_data.output_date.Length > 30)
                                {
                                    template_data.output_date = template_data.output_date.Substring(0, 30);
                                }
                                ZaloZNSDataRequest.template_data = template_data;
                                await ZaloHelper.ZNSSendAsync(ZaloToken.OAAccessToken, ZaloZNSDataRequest);
                            }
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<ZaloToken>> GetLatestAsync(BaseParameter<ZaloToken> BaseParameter)
        {
            var result = new BaseResult<ZaloToken>();
            try
            {
                DateTime Now = GlobalHelper.InitializationDateTime;
                result.BaseModel = await GetByCondition(item => item.Active == true && item.Date.Value.Year == Now.Year && item.Date.Value.Month == Now.Month && item.Date.Value.Day == Now.Day).FirstOrDefaultAsync();
                if (result.BaseModel == null)
                {
                    result.BaseModel = await GetByCondition(item => item.Active == true).OrderByDescending(item => item.ID).FirstOrDefaultAsync();
                    if (result.BaseModel != null)
                    {
                        string url = result.BaseModel.URL;
                        string secret_key = result.BaseModel.SecretKey;
                        string refresh_token = result.BaseModel.OARefreshToken;
                        string app_id = result.BaseModel.AppID;

                        HttpClient HttpClient = new HttpClient();
                        HttpClient.BaseAddress = new Uri(url);

                        HttpClient.DefaultRequestHeaders.Accept.Clear();
                        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpClient.DefaultRequestHeaders.Add("secret_key", secret_key);

                        var data = new List<KeyValuePair<string, string>>();
                        data.Add(new KeyValuePair<string, string>("refresh_token", refresh_token));
                        data.Add(new KeyValuePair<string, string>("app_id", app_id));
                        data.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));


                        var task = HttpClient.PostAsync(url, new FormUrlEncodedContent(data));
                        var ContentResult = await task.Result.Content.ReadAsStringAsync();
                        ZaloRefreshTokenDataRespond ZaloRefreshTokenDataRespond = JsonConvert.DeserializeObject<ZaloRefreshTokenDataRespond>(ContentResult);
                        result.BaseModel = new ZaloToken();
                        result.BaseModel.AppID = app_id;
                        result.BaseModel.URL = url;
                        result.BaseModel.SecretKey = secret_key;

                        result.BaseModel.Active = true;
                        result.BaseModel.OAAccessToken = ZaloRefreshTokenDataRespond.access_token;
                        result.BaseModel.OARefreshToken = ZaloRefreshTokenDataRespond.refresh_token;
                        BaseParameter.BaseModel = result.BaseModel;
                        result = await SaveAsync(BaseParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new ZaloToken();
            }
            return result;
        }
    }
}

