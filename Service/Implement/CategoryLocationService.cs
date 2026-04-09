namespace Service.Implement
{
    public class CategoryLocationService : BaseService<CategoryLocation, ICategoryLocationRepository>
    , ICategoryLocationService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly ICategoryLayerRepository _CategoryLayerRepository;
        private readonly ICategoryRackRepository _CategoryRackRepository;
        private readonly IMaterialRepository _MaterialRepository;
        public CategoryLocationService(ICategoryLocationRepository CategoryLocationRepository
            , IWebHostEnvironment WebHostEnvironment
            , ICategoryLayerRepository CategoryLayerRepository
            , ICategoryRackRepository CategoryRackRepository
            , IMaterialRepository MaterialRepository


            ) : base(CategoryLocationRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _CategoryLocationRepository = CategoryLocationRepository;
            _CategoryLayerRepository = CategoryLayerRepository;
            _CategoryRackRepository = CategoryRackRepository;
            _MaterialRepository = MaterialRepository;
        }
        public override void BaseInitialization(CategoryLocation model)
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
        public override void Initialization(CategoryLocation model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
                var Parent = _CategoryRackRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Name;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                model.CategoryDepartmentID = Parent.ParentID;
                model.CategoryDepartmentName = Parent.ParentName;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ParentName))
                {
                    var Parent = _CategoryRackRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.ParentID == model.CategoryDepartmentID && o.Code == model.ParentName).FirstOrDefault();
                    if (Parent == null)
                    {
                        Parent = new CategoryRack();
                        Parent.Active = true;
                        Parent.CompanyID = model.CompanyID;
                        Parent.ParentID = model.CategoryDepartmentID;
                        Parent.Code = model.ParentName;
                        Parent.Name = Parent.Code;
                        _CategoryRackRepository.Add(Parent);
                    }
                    model.ParentID = Parent.ID;
                }
            }
            if (model.CategoryLayerID > 0)
            {
                var CategoryLayer = _CategoryLayerRepository.GetByID(model.CategoryLayerID.Value);
                model.CategoryLayerName = CategoryLayer.Name;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryLayerName))
                {
                    if (model.CategoryLayerName.Length == 1)
                    {
                        model.CategoryLayerName = "0" + model.CategoryLayerName;
                    }
                    var CategoryLayer = _CategoryLayerRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.CategoryLayerName).FirstOrDefault();
                    if (CategoryLayer == null)
                    {
                        CategoryLayer = new CategoryLayer();
                        CategoryLayer.Active = true;
                        CategoryLayer.CompanyID = model.CompanyID;
                        CategoryLayer.Code = model.CategoryLayerName;
                        CategoryLayer.Name = CategoryLayer.Code;
                        _CategoryLayerRepository.Add(CategoryLayer);
                    }
                    model.CategoryLayerID = CategoryLayer.ID;
                }
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                model.FileName = QRCodeHelper.CreateQRCodeViaString(model.Name);
            }
            model.Code = model.Code ?? model.Name;
            model.Height = model.Height ?? 1;
            model.Width = model.Width ?? 2;
            model.Length = model.Length ?? 3;
            model.Weight = model.Weight ?? 100;
        }
        public override async Task<BaseResult<CategoryLocation>> SaveAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            if (BaseParameter.BaseModel != null)
            {
                var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.Name == BaseParameter.BaseModel.Name).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> GetByParentID_CategoryLayerIDToListAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            if (BaseParameter.ParentID > 0 && BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.CategoryLayerID == BaseParameter.GeneralID).ToListAsync();
                if (result.List.Count == 0)
                {
                    var CategoryLayer = await _CategoryLayerRepository.GetByIDAsync(BaseParameter.GeneralID.Value);
                    if (CategoryLayer.ID > 0)
                    {
                        var CategoryRack = await _CategoryRackRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                        if (CategoryRack.ID > 0)
                        {
                            for (var i = 1; i <= CategoryRack.Count; i++)
                            {
                                var RackName = i.ToString();
                                if (i < 10)
                                {
                                    RackName = "0" + RackName;
                                }
                                var CategoryLocation = new CategoryLocation();
                                CategoryLocation.Active = true;
                                CategoryLocation.ParentID = BaseParameter.ParentID;
                                CategoryLocation.CategoryLayerID = BaseParameter.GeneralID;
                                CategoryLocation.Name = CategoryLocation.Name + "-" + RackName + "-" + CategoryLayer.Code;
                                var BaseParameterCategoryLocation = new BaseParameter<CategoryLocation>();
                                BaseParameterCategoryLocation.BaseModel = CategoryLocation;
                                await SaveAsync(BaseParameterCategoryLocation);
                            }
                        }
                    }
                }
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.CategoryLayerID == BaseParameter.GeneralID).ToListAsync();
            }
            return result;
        }
        public override async Task<BaseResult<CategoryLocation>> GetByActiveToListAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            result.List = await GetByCondition(o => o.Active == BaseParameter.Active).ToListAsync();
            if (result.List.Count == 0)
            {
                var ListCategoryLayer = await _CategoryLayerRepository.GetByActiveToListAsync(true);
                var ListCategoryRack = await _CategoryRackRepository.GetByActiveToListAsync(true);
                foreach (var CategoryLayer in ListCategoryLayer)
                {
                    foreach (var CategoryRack in ListCategoryRack)
                    {
                        for (var i = 1; i <= CategoryRack.Count; i++)
                        {
                            var RackName = i.ToString();
                            if (i < 10)
                            {
                                RackName = "0" + RackName;
                            }
                            var CategoryLocation = new CategoryLocation();
                            CategoryLocation.Active = true;
                            CategoryLocation.ParentID = CategoryRack.ID;
                            CategoryLocation.CategoryLayerID = CategoryLayer.ID;
                            CategoryLocation.Name = CategoryRack.Name + "-" + RackName + "-" + CategoryLayer.Code;
                            var BaseParameterCategoryLocation = new BaseParameter<CategoryLocation>();
                            BaseParameterCategoryLocation.BaseModel = CategoryLocation;
                            await SaveAsync(BaseParameterCategoryLocation);
                        }
                    }
                }
                result.List = await GetByCondition(o => o.Active == BaseParameter.Active).ToListAsync();
            }
            return result;
        }
        public override async Task<BaseResult<CategoryLocation>> GetByCompanyIDAndActiveToListAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == BaseParameter.Active).ToListAsync();
                if (result.List.Count == 0)
                {
                    var ListCategoryLayer = await _CategoryLayerRepository.GetByCompanyIDAndActiveToListAsync(BaseParameter.CompanyID.Value, true);
                    var ListCategoryRack = await _CategoryRackRepository.GetByCompanyIDAndActiveToListAsync(BaseParameter.CompanyID.Value, true);
                    foreach (var CategoryLayer in ListCategoryLayer)
                    {
                        foreach (var CategoryRack in ListCategoryRack)
                        {
                            for (var i = 1; i <= CategoryRack.Count; i++)
                            {
                                var RackName = i.ToString();
                                if (i < 10)
                                {
                                    RackName = "0" + RackName;
                                }
                                var CategoryLocation = new CategoryLocation();
                                CategoryLocation.Active = true;
                                CategoryLocation.ParentID = CategoryRack.ID;
                                CategoryLocation.CategoryLayerID = CategoryLayer.ID;
                                CategoryLocation.Name = CategoryRack.Name + "-" + RackName + "-" + CategoryLayer.Code;
                                var BaseParameterCategoryLocation = new BaseParameter<CategoryLocation>();
                                BaseParameterCategoryLocation.BaseModel = CategoryLocation;
                                await SaveAsync(BaseParameterCategoryLocation);
                            }
                        }
                    }
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == BaseParameter.Active).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> GetByCategoryDepartmentIDToListAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> PrintAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.BaseModel != null)
                {
                    string HTMLContent = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "CategoryLocation400.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            HTMLContent = r.ReadToEnd();
                        }
                    }
                    HTMLContent = HTMLContent.Replace(@"[FileName]", BaseParameter.BaseModel.FileName);
                    HTMLContent = HTMLContent.Replace(@"[Name]", BaseParameter.BaseModel.Name);

                    string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            await w.WriteLineAsync(HTMLContent);
                        }
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> PrintByCategoryDepartmentIDAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.CategoryDepartmentID > 0)
                {
                    StringBuilder HTMLContent = new StringBuilder();
                    result.List = await GetByCondition(o => o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Name).ToListAsync();
                    foreach (var CategoryLocation in result.List)
                    {
                        string HTMLContentSub = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "CategoryLocation400.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLContentSub = r.ReadToEnd();
                            }
                        }
                        HTMLContentSub = HTMLContentSub.Replace(@"[FileName]", CategoryLocation.FileName);
                        HTMLContentSub = HTMLContentSub.Replace(@"[Name]", CategoryLocation.Name);

                        HTMLContent.AppendLine(HTMLContentSub);
                    }
                    string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            await w.WriteLineAsync(HTMLContent.ToString());
                        }
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> PrintByParentIDAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ParentID > 0)
                {
                    StringBuilder HTMLContent = new StringBuilder();
                    result.List = await GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.ParentID).OrderBy(o => o.Name).ToListAsync();
                    foreach (var CategoryLocation in result.List)
                    {
                        string HTMLContentSub = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "CategoryLocation400.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLContentSub = r.ReadToEnd();
                            }
                        }
                        HTMLContentSub = HTMLContentSub.Replace(@"[FileName]", CategoryLocation.FileName);
                        HTMLContentSub = HTMLContentSub.Replace(@"[Name]", CategoryLocation.Name);

                        HTMLContent.AppendLine(HTMLContentSub);
                    }
                    string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            await w.WriteLineAsync(HTMLContent.ToString());
                        }
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<CategoryLocation>> CreateAutoAsync(BaseParameter<CategoryLocation> BaseParameter)
        {
            var result = new BaseResult<CategoryLocation>();

            var ListCategoryRack = await _CategoryRackRepository.GetByCondition(o => o.ID == 68 || o.ID == 69 || o.ID == 79).ToListAsync();
            var ListCategoryLayer = await _CategoryLayerRepository.GetByCondition(o => o.CompanyID == 17).ToListAsync();
            foreach (var CategoryRack in ListCategoryRack)
            {
                foreach (var CategoryLayer in ListCategoryLayer)
                {
                    for (int i = 1; i < 10; i++)
                    {
                        var CategoryLocation = new CategoryLocation();
                        CategoryLocation.Active = false;
                        CategoryLocation.CompanyID = 17;
                        CategoryLocation.CategoryLayerID = CategoryLayer.ID;
                        CategoryLocation.ParentID = CategoryRack.ID;
                        CategoryLocation.CategoryDepartmentID = CategoryRack.ParentID;
                        CategoryLocation.CategoryDepartmentName = CategoryRack.ParentName;
                        var index = "0" + i.ToString();
                        CategoryLocation.Code = CategoryRack.Code + "-" + index + "-" + CategoryLayer.Code;
                        CategoryLocation.Name = CategoryLocation.Code;
                        BaseParameter.BaseModel = CategoryLocation;
                        await SaveAsync(BaseParameter);
                    }
                }
            }
            return result;
        }
    }
}

