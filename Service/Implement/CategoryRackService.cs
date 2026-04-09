
namespace Service.Implement
{
    public class CategoryRackService : BaseService<CategoryRack, ICategoryRackRepository>
    , ICategoryRackService
    {
        private readonly ICategoryRackRepository _CategoryRackRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryRackService(ICategoryRackRepository CategoryRackRepository, IWebHostEnvironment webHostEnvironment) : base(CategoryRackRepository)
        {
            _CategoryRackRepository = CategoryRackRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(CategoryRack model)
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
        public override void Initialization(CategoryRack model)
        {
            BaseInitialization(model);
            if (string.IsNullOrEmpty(model.Code))
            {
                model.Code = model.Name;
            }
            if (string.IsNullOrEmpty(model.Display))
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    model.Display = model.Name.Split('.')[0];
                    model.Display = model.Display.Split('-')[0];
                }
            }
            model.Count = model.Count ?? 10;
        }
        public virtual async Task<BaseResult<CategoryRack>> GetByParentIDAndCompanyIDAndActiveToListAsync(BaseParameter<CategoryRack> BaseParameter)
        {
            var result = new BaseResult<CategoryRack>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.CompanyID == BaseParameter.CompanyID && o.Active == BaseParameter.Active).ToListAsync();
            }
            return result;
        }
    }
}

