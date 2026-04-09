namespace Service.Implement
{
    public class BOMStageService : BaseService<BOMStage, IBOMStageRepository>
    , IBOMStageService
    {
        private readonly IBOMStageRepository _BOMStageRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMStageService(IBOMStageRepository BOMStageRepository
            , IBOMRepository BOMRepository
            , IWebHostEnvironment webHostEnvironment
            ) : base(BOMStageRepository)
        {
            _BOMStageRepository = BOMStageRepository;
            _BOMRepository = BOMRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(BOMStage model)
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
        public override void Initialization(BOMStage model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (model.Percent == null)
            {
                var List = GetByCondition(o => o.Active == true && o.ParentID == model.ParentID).ToList();
                model.Percent = 100 - List.Sum(o => o.Percent ?? 0);
            }
        }
        public override async Task<BaseResult<BOMStage>> SaveAsync(BaseParameter<BOMStage> BaseParameter)
        {
            var result = new BaseResult<BOMStage>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            return result;
        }
    }
}

