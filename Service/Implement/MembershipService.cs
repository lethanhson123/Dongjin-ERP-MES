namespace Service.Implement
{
    public class MembershipService : BaseService<Membership, IMembershipRepository>
    , IMembershipService
    {
        private readonly IMembershipRepository _MembershipRepository;        
        private readonly IMembershipTokenRepository _MembershipTokenRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly ICategoryPositionRepository _CategoryPositionRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipService(IMembershipRepository MembershipRepository            
            , IMembershipTokenRepository MembershipTokenRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , ICategoryPositionRepository CategoryPositionRepository
            , IWebHostEnvironment WebHostEnvironment
            ) : base(MembershipRepository)
        {
            _MembershipRepository = MembershipRepository;            
            _MembershipTokenRepository = MembershipTokenRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _CategoryPositionRepository = CategoryPositionRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }

        public override void BaseInitialization(Membership model)
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
        public override void InitializationSave(Membership model)
        {
            BaseInitialization(model);
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                if (CategoryDepartment != null)
                {
                    model.CategoryDepartmentName = CategoryDepartment.Name;
                }
            }
            if (model.CategoryPositionID > 0)
            {
                var CategoryPosition = _CategoryPositionRepository.GetByID(model.CategoryPositionID.Value);
                if (CategoryPosition != null)
                {
                    model.CategoryPositionName = CategoryPosition.Name;
                }
            }
            model.GUID = model.GUID ?? GlobalHelper.InitializationGUICode;
            model.Password = model.Password ?? GlobalHelper.Password;
        }
        public override async Task<BaseResult<Membership>> SaveAsync(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                bool IsCheck = true;
                InitializationSave(BaseParameter.BaseModel);
                if (string.IsNullOrEmpty(BaseParameter.BaseModel.UserName))
                {
                    IsCheck = false;
                }
                Membership modelExist = await GetByCondition(item => item.UserName == BaseParameter.BaseModel.UserName).FirstOrDefaultAsync();
                if (modelExist != null)
                {
                    if (modelExist.ID != BaseParameter.BaseModel.ID)
                    {
                        IsCheck = false;
                    }
                }
                if (IsCheck == true)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.IsComplete == true)
                        {
                            BaseParameter.BaseModel.Password = GlobalHelper.Password;
                        }
                        if (BaseParameter.BaseModel.Password != modelExist.Password)
                        {
                            if (GlobalHelper.IsPasswordValidWithRegex(BaseParameter.BaseModel.Password) == false)
                            {
                                IsCheck = false;
                            }
                            BaseParameter.BaseModel.Password = SecurityHelper.Encrypt(BaseParameter.BaseModel.GUID, BaseParameter.BaseModel.Password);
                        }
                        if (BaseParameter.BaseModel.Active == null)
                        {
                            BaseParameter.BaseModel.Active = modelExist.Active;
                        }
                        if (IsCheck == true)
                        {
                            result = await UpdateAsync(BaseParameter);
                        }
                    }
                    else
                    {
                        if (GlobalHelper.IsPasswordValidWithRegex(BaseParameter.BaseModel.Password) == false)
                        {
                            IsCheck = false;
                        }
                        if (IsCheck == true)
                        {
                            BaseParameter.BaseModel.Password = SecurityHelper.Encrypt(BaseParameter.BaseModel.GUID, BaseParameter.BaseModel.Password);
                            result = await AddAsync(BaseParameter);
                        }
                    }

                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Membership>> AuthenticationAsync(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();
            result.BaseModel = await GetByCondition(item => item.Active == true && item.UserName == BaseParameter.UserName).FirstOrDefaultAsync();            
            bool IsCheck = false;
            if (result.BaseModel != null)
            {
                string passwordDecrypt = SecurityHelper.Decrypt(result.BaseModel.GUID, result.BaseModel.Password);
                if (passwordDecrypt.Equals(BaseParameter.Password))
                {
                    IsCheck = true;
                }
                if (IsCheck == true)
                {
                    var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, GlobalHelper.Subject),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("ID", result.BaseModel.ID.ToString())
                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalHelper.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        GlobalHelper.Issuer,
                        GlobalHelper.Audience,
                        claims,
                        expires: DateTime.UtcNow.AddDays(GlobalHelper.TokenExpired.Value),
                        signingCredentials: signIn
                    );

                    var MembershipToken = new MembershipToken();
                    MembershipToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    MembershipToken.Active = true;
                    MembershipToken.ParentID = result.BaseModel.ID;
                    MembershipToken.DateBegin = GlobalHelper.InitializationDateTime;
                    MembershipToken.DateEnd = MembershipToken.DateBegin.Value.AddDays(GlobalHelper.TokenExpired.Value);
                    await _MembershipTokenRepository.AddAsync(MembershipToken);
                    result.BaseModel.Code = MembershipToken.Token;

                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new Membership();
            }
            if (string.IsNullOrEmpty(result.BaseModel.Code))
            {
                result.BaseModel = new Membership();
            }
            result.BaseModel.GUID = string.Empty;
            result.BaseModel.Password = string.Empty;
            result.BaseModel.Note = GlobalHelper.ERPSite;
            return result;
        }
        public virtual async Task<BaseResult<Membership>> CreateAutoAsync(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();           
            return result;
        }
        public virtual async Task<BaseResult<Membership>> GetByCategoryDepartmentID_ActiveToListAsync(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();
            result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == BaseParameter.Active).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<Membership>> GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();
            result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.CategoryPositionID == BaseParameter.CategoryPositionID && o.Active == BaseParameter.Active).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<Membership>> IsPasswordValidWithRegex(BaseParameter<Membership> BaseParameter)
        {
            var result = new BaseResult<Membership>();
            result.Count = 0;
            var IsCheck = false;
            Membership modelExist = await GetByCondition(item => item.UserName == BaseParameter.BaseModel.UserName).FirstOrDefaultAsync();
            if (modelExist != null)
            {
                if (BaseParameter.BaseModel.Password == modelExist.Password)
                {
                    IsCheck = true;
                }
                else
                {
                    IsCheck = GlobalHelper.IsPasswordValidWithRegex(BaseParameter.BaseModel.Password);
                }
            }
            else
            {
                IsCheck = true;
            }
            if (IsCheck == true)
            {
                result.Count = 1;
            }
            return result;
        }
    }
}

