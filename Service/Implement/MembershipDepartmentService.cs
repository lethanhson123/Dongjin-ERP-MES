namespace Service.Implement
{
    public class MembershipDepartmentService : BaseService<MembershipDepartment, IMembershipDepartmentRepository>
    , IMembershipDepartmentService
    {
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipDepartmentService(IMembershipDepartmentRepository MembershipDepartmentRepository
            , IMembershipRepository MembershipRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IWebHostEnvironment WebHostEnvironment
            ) : base(MembershipDepartmentRepository)
        {
            _MembershipDepartmentRepository = MembershipDepartmentRepository;
            _MembershipRepository = MembershipRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(MembershipDepartment model)
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
        public override void Initialization(MembershipDepartment model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.ParentID.Value);
                model.ParentName = Membership.Code + "-" + Membership.Name;
            }
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.CompanyName + "-" + CategoryDepartment.Name;
            }
        }
        public override async Task<BaseResult<MembershipDepartment>> SaveAsync(BaseParameter<MembershipDepartment> BaseParameter)
        {
            var result = new BaseResult<MembershipDepartment>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID).FirstOrDefaultAsync();
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
        public override async Task<BaseResult<MembershipDepartment>> GetByParentIDToListAsync(BaseParameter<MembershipDepartment> BaseParameter)
        {
            var result = new BaseResult<MembershipDepartment>();
            var ListCategoryDepartment = await _CategoryDepartmentRepository.GetByActiveToListAsync(true);
            foreach (var CategoryDepartment in ListCategoryDepartment)
            {
                var MembershipDepartment = new MembershipDepartment();
                MembershipDepartment.CategoryDepartmentID = CategoryDepartment.ID;
                MembershipDepartment.SortOrder = CategoryDepartment.SortOrder;
                MembershipDepartment.ParentID = BaseParameter.ParentID;
                BaseParameter.BaseModel = MembershipDepartment;
                await SaveAsync(BaseParameter);
            }
            result.List = await _MembershipDepartmentRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            return result;
        }
    }
}

