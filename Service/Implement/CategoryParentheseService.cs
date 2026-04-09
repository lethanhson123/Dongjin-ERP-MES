namespace Service.Implement
{
    public class CategoryParentheseService : BaseService<CategoryParenthese, ICategoryParentheseRepository>
    , ICategoryParentheseService
    {
        private readonly ICategoryParentheseRepository _CategoryParentheseRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CategoryParentheseService(ICategoryParentheseRepository CategoryParentheseRepository, IWebHostEnvironment webHostEnvironment) : base(CategoryParentheseRepository)
        {
            _CategoryParentheseRepository = CategoryParentheseRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(CategoryParenthese model)
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
    }
}

