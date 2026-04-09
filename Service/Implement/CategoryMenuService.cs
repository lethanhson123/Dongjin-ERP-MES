namespace Service.Implement
{
    public class CategoryMenuService : BaseService<CategoryMenu, ICategoryMenuRepository>
    , ICategoryMenuService
    {
        private readonly ICategoryMenuRepository _CategoryMenuRepository;
        private readonly IMembershipMenuRepository _MembershipMenuRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryMenuService(ICategoryMenuRepository CategoryMenuRepository
            , IMembershipMenuRepository MembershipMenuRepository
            , IWebHostEnvironment WebHostEnvironment
            ) : base(CategoryMenuRepository)
        {
            _CategoryMenuRepository = CategoryMenuRepository;
            _MembershipMenuRepository = MembershipMenuRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(CategoryMenu model)
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
        public override void Initialization(CategoryMenu model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _CategoryMenuRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.NameEnglish;
            }
        }

        public virtual async Task<BaseResult<CategoryMenu>> GetByMembershipID_ActiveToListAsync(BaseParameter<CategoryMenu> BaseParameter)
        {
            var result = new BaseResult<CategoryMenu>();
            if (BaseParameter.MembershipID > 0)
            {
                List<long> ListMembershipMenuID = await _MembershipMenuRepository.GetByCondition(item => item.ParentID == BaseParameter.MembershipID && item.Active == BaseParameter.Active).Select(item => item.CategoryMenuID.Value).ToListAsync();
                if (ListMembershipMenuID.Count > 0)
                {
                    result.List = await _CategoryMenuRepository.GetByCondition(o => ListMembershipMenuID.Contains(o.ID) && o.Active == true && o.SortOrder < 100000).ToListAsync();
                }
            }
            return result;
        }
    }
}

