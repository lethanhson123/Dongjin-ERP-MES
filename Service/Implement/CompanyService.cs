namespace Service.Implement
{
    public class CompanyService : BaseService<Company, ICompanyRepository>
    , ICompanyService
    {
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IMembershipCompanyRepository _MembershipCompanyRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CompanyService(ICompanyRepository CompanyRepository
            , IMembershipCompanyRepository MembershipCompanyRepository
            , IWebHostEnvironment WebHostEnvironment

            ) : base(CompanyRepository)
        {
            _CompanyRepository = CompanyRepository;
            _MembershipCompanyRepository = MembershipCompanyRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(Company model)
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
        public override void Initialization(Company model)
        {
            BaseInitialization(model);
            if (string.IsNullOrEmpty(model.Code))
            {
                model.Code = GlobalHelper.SetName(model.Name);
                if (!string.IsNullOrEmpty(model.Code))
                {
                    model.Code = model.Code.ToUpper();
                }
            }            
        }
        public virtual async Task<BaseResult<Company>> GetByMembershipID_ActiveToListAsync(BaseParameter<Company> BaseParameter)
        {
            var result = new BaseResult<Company>();
            if (BaseParameter.MembershipID > 0)
            {
                List<long> ListMembershipCompanyID = await _MembershipCompanyRepository.GetByCondition(o => o.ParentID == BaseParameter.MembershipID && o.Active == BaseParameter.Active).Select(o => o.CompanyID.Value).ToListAsync();
                if (ListMembershipCompanyID.Count > 0)
                {
                    result.List = await _CompanyRepository.GetByCondition(o => ListMembershipCompanyID.Contains(o.ID) && o.Active == true).ToListAsync();
                }
            }
            return result;
        }
    }
}

