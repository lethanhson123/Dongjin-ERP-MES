namespace Service.Implement
{
    public class BOMTermService : BaseService<BOMTerm, IBOMTermRepository>
    , IBOMTermService
    {
        private readonly IBOMTermRepository _BOMTermRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailService _BOMDetailService;

        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMTermService(IBOMTermRepository BOMTermRepository
            , IBOMRepository BOMRepository
            , IBOMDetailService BOMDetailService
            , IWebHostEnvironment webHostEnvironment
            ) : base(BOMTermRepository)
        {
            _BOMTermRepository = BOMTermRepository;
            _BOMRepository = BOMRepository;
            _BOMDetailService = BOMDetailService;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(BOMTerm model)
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
        public override void Initialization(BOMTerm model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            model.CCH1 = model.CCH1 ?? 0;
            model.CCW1 = model.CCW1 ?? 0;
            model.ICH1 = model.ICH1 ?? 0;
            model.ICW1 = model.ICW1 ?? 0;
        }
        public override async Task<BaseResult<BOMTerm>> SaveAsync(BaseParameter<BOMTerm> BaseParameter)
        {
            var result = new BaseResult<BOMTerm>();
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
            try
            {
                await SyncAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<BOMTerm>> SyncAsync(BaseParameter<BOMTerm> BaseParameter)
        {
            var result = new BaseResult<BOMTerm>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    BOMDetail BOMDetail = new BOMDetail();
                    BOMDetail.Active = BaseParameter.BaseModel.Active;
                    BOMDetail.ParentID = BaseParameter.BaseModel.ParentID;
                    BOMDetail.Note = "Term";
                    BOMDetail.MaterialCode02 = BaseParameter.BaseModel.Code;
                    BaseParameter<BOMDetail> BaseParameterBOMDetail = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetail.BaseModel = BOMDetail;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetail);
                }
            }
            return result;
        }
    }
}

