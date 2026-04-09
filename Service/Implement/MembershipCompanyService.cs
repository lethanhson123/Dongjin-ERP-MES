namespace Service.Implement
{
    public class MembershipCompanyService : BaseService<MembershipCompany, IMembershipCompanyRepository>
    , IMembershipCompanyService
    {
        private readonly IMembershipCompanyRepository _MembershipCompanyRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipCompanyService(IMembershipCompanyRepository MembershipCompanyRepository
            , IMembershipRepository MembershipRepository
            , ICompanyRepository CompanyRepository
            , IWebHostEnvironment WebHostEnvironment
            ) : base(MembershipCompanyRepository)
        {
            _MembershipCompanyRepository = MembershipCompanyRepository;
            _MembershipRepository = MembershipRepository;
            _CompanyRepository = CompanyRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(MembershipCompany model)
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
        public override void Initialization(MembershipCompany model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.ParentID.Value);
                model.ParentName = Membership.Code + "-" + Membership.Name;
            }
            if (model.CompanyID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CompanyID.Value);
                model.CompanyName = Company.Name;
            }
        }
        public override async Task<BaseResult<MembershipCompany>> SaveAsync(BaseParameter<MembershipCompany> BaseParameter)
        {
            var result = new BaseResult<MembershipCompany>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.CompanyID == BaseParameter.BaseModel.CompanyID).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            if (result.BaseModel.ID > 0)
            {
            }
            return result;
        }
        public override async Task<BaseResult<MembershipCompany>> GetByParentIDToListAsync(BaseParameter<MembershipCompany> BaseParameter)
        {
            var result = new BaseResult<MembershipCompany>();
            var ListCompany = await _CompanyRepository.GetByActiveToListAsync(true);
            foreach (var Company in ListCompany)
            {
                var MembershipCompany = new MembershipCompany();
                MembershipCompany.CompanyID = Company.ID;
                MembershipCompany.SortOrder = Company.SortOrder;
                MembershipCompany.ParentID = BaseParameter.ParentID;
                BaseParameter.BaseModel = MembershipCompany;
                await SaveAsync(BaseParameter);
            }
            result.List = await _MembershipCompanyRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            return result;
        }
    }
}

