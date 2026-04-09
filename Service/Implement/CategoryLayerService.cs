namespace Service.Implement
{
    public class CategoryLayerService : BaseService<CategoryLayer, ICategoryLayerRepository>
    , ICategoryLayerService
    {
        private readonly ICategoryLayerRepository _CategoryLayerRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryLayerService(ICategoryLayerRepository CategoryLayerRepository, IWebHostEnvironment webHostEnvironment) : base(CategoryLayerRepository)
        {
            _CategoryLayerRepository = CategoryLayerRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(CategoryLayer model)
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
        public override void Initialization(CategoryLayer model)
        {
            BaseInitialization(model);            
            model.Code = model.Code ?? model.Name;           
        }
    }
}

