namespace Service.Implement
{
    public class CategoryDepartmentService : BaseService<CategoryDepartment, ICategoryDepartmentRepository>
    , ICategoryDepartmentService
    {
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;        
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryDepartmentService(ICategoryDepartmentRepository CategoryDepartmentRepository            
            , IMembershipDepartmentRepository MembershipDepartmentRepository
            , IWebHostEnvironment WebHostEnvironment

            ) : base(CategoryDepartmentRepository)
        {
            _CategoryDepartmentRepository = CategoryDepartmentRepository;            
            _MembershipDepartmentRepository = MembershipDepartmentRepository;
            _WebHostEnvironment = WebHostEnvironment;
        }
        public override void BaseInitialization(CategoryDepartment model)
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
        public override void Initialization(CategoryDepartment model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _CategoryDepartmentRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
            }
            model.Code = model.Code ?? GlobalHelper.InitializationString;
            model.Name = model.Name ?? model.Code;
            model.Note = model.Note ?? GlobalHelper.InitializationString;
            if (model.Note.Contains(model.Code) == false)
            {
                model.Note = model.Code + ";" + model.Note;
            }
        }
        
        public virtual async Task<BaseResult<CategoryDepartment>> CreateAutoAsync(BaseParameter<CategoryDepartment> BaseParameter)
        {
            var result = new BaseResult<CategoryDepartment>();
            

            return result;
        }

        public virtual async Task<BaseResult<CategoryDepartment>> GetByMembershipID_ActiveToListAsync(BaseParameter<CategoryDepartment> BaseParameter)
        {
            var result = new BaseResult<CategoryDepartment>();
            if (BaseParameter.MembershipID > 0)
            {
                List<long> ListMembershipDepartmentID = await _MembershipDepartmentRepository.GetByCondition(o => o.ParentID == BaseParameter.MembershipID && o.Active == BaseParameter.Active).Select(o => o.CategoryDepartmentID.Value).ToListAsync();
                if (ListMembershipDepartmentID.Count > 0)
                {
                    result.List = await _CategoryDepartmentRepository.GetByCondition(o => ListMembershipDepartmentID.Contains(o.ID) && o.Active == true).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<CategoryDepartment>> GetByMembershipID_CompanyID_ActiveToListAsync(BaseParameter<CategoryDepartment> BaseParameter)
        {
            var result = new BaseResult<CategoryDepartment>();
            if (BaseParameter.MembershipID > 0 && BaseParameter.CompanyID > 0)
            {
                List<long> ListMembershipDepartmentID = await _MembershipDepartmentRepository.GetByCondition(o => o.ParentID == BaseParameter.MembershipID && o.Active == BaseParameter.Active).Select(o => o.CategoryDepartmentID.Value).ToListAsync();
                if (ListMembershipDepartmentID.Count > 0)
                {
                    result.List = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && ListMembershipDepartmentID.Contains(o.ID) && o.Active == true).ToListAsync();
                }
            }
            return result;
        }
    }
}

