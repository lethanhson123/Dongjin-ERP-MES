namespace Service.Implement
{
    public class MembershipMenuService : BaseService<MembershipMenu, IMembershipMenuRepository>
    , IMembershipMenuService
    {
        private readonly IMembershipMenuRepository _MembershipMenuRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly ICategoryMenuRepository _CategoryMenuRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipMenuService(IMembershipMenuRepository MembershipMenuRepository
            , IMembershipRepository MembershipRepository
            , ICategoryMenuRepository CategoryMenuRepository
            , IWebHostEnvironment WebHostEnvironment

            ) : base(MembershipMenuRepository)
        {
            _MembershipMenuRepository = MembershipMenuRepository;
            _MembershipRepository = MembershipRepository;
            _CategoryMenuRepository = CategoryMenuRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(MembershipMenu model)
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
        public override void Initialization(MembershipMenu model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.ParentID.Value);
                model.ParentName = Membership.Code + "-" + Membership.Name;
            }
            if (model.CategoryMenuID > 0)
            {
                var CategoryMenu = _CategoryMenuRepository.GetByID(model.CategoryMenuID.Value);
                model.CategoryMenuName = CategoryMenu.NameEnglish;
                model.Display = CategoryMenu.ParentName;
            }
        }
        public override async Task<BaseResult<MembershipMenu>> SaveAsync(BaseParameter<MembershipMenu> BaseParameter)
        {
            var result = new BaseResult<MembershipMenu>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryMenuID == BaseParameter.BaseModel.CategoryMenuID).FirstOrDefaultAsync();
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
        public override async Task<BaseResult<MembershipMenu>> GetByParentIDToListAsync(BaseParameter<MembershipMenu> BaseParameter)
        {
            var result = new BaseResult<MembershipMenu>();
            var ListCategoryMenu = await _CategoryMenuRepository.GetByActiveToListAsync(true);
            foreach (var CategoryMenu in ListCategoryMenu)
            {
                var MembershipMenu = new MembershipMenu();
                MembershipMenu.CategoryMenuID = CategoryMenu.ID;
                MembershipMenu.SortOrder = CategoryMenu.SortOrder;
                MembershipMenu.ParentID = BaseParameter.ParentID;
                BaseParameter.BaseModel = MembershipMenu;
                await SaveAsync(BaseParameter);
            }
            result.List = await _MembershipMenuRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            return result;
        }
    }
}

