namespace Service.Implement
{
    public class MaterialConvertService : BaseService<MaterialConvert, IMaterialConvertRepository>
    , IMaterialConvertService
    {
        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MaterialConvertService(IMaterialConvertRepository MaterialConvertRepository
            , IMaterialRepository materialRepository
            , IWebHostEnvironment webHostEnvironment

            ) : base(MaterialConvertRepository)
        {
            _MaterialConvertRepository = MaterialConvertRepository;
            _MaterialRepository = materialRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(MaterialConvert model)
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
        public override void InitializationSave(MaterialConvert model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ParentName))
                {
                    Material Parent = _MaterialRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.ParentName).FirstOrDefault();
                    if (Parent == null)
                    {
                        Parent = new Material();
                    }
                    if (Parent.ID == 0)
                    {
                        Parent.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
                        Parent.Active = true;
                        Parent.Code = model.ParentName;
                        Parent.OriginalEquipmentManufacturer = model.Description;
                        _MaterialRepository.Add(Parent);
                    }
                    model.ParentID = Parent.ID;
                }
            }
            if (model.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Note = Parent.Name;
                model.CompanyID = Parent.CompanyID;

                Parent.OriginalEquipmentManufacturer = model.Description;
                _MaterialRepository.Update(Parent);
            }
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            model.Active = false;
            if (!string.IsNullOrEmpty(model.Code) && model.Code != model.ParentName)
            {
                var Material = _MaterialRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Code == model.Code).FirstOrDefault();
                if (Material == null)
                {
                    Material = new Material();
                }
                if (Material.ID == 0)
                {
                    Material.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
                    Material.Active = false;
                    Material.Code = model.Code;
                    _MaterialRepository.Add(Material);
                }
                Material.Active = false;
                Material.OriginalEquipmentManufacturer = model.Description;
                _MaterialRepository.Update(Material);
                model.MaterialID = Material.ID;
                model.Active = true;
            }
        }
        public override async Task<BaseResult<MaterialConvert>> SaveAsync(BaseParameter<MaterialConvert> BaseParameter)
        {
            var result = new BaseResult<MaterialConvert>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            result.IsCheck = false;
            if (ModelCheck == null)
            {
                result.IsCheck = true;
            }
            else
            {
                if (ModelCheck.ID == BaseParameter.BaseModel.ID)
                {
                    result.IsCheck = true;
                }
            }
            if (result.IsCheck == true)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<MaterialConvert>> SyncAsync(BaseParameter<MaterialConvert> BaseParameter)
        {
            var result = new BaseResult<MaterialConvert>();
            if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.ID > 0 && BaseParameter.BaseModel.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                if (Parent.ID > 0)
                {
                    Parent.Description = GlobalHelper.InitializationString;
                    var ListMaterialConvert = await _MaterialConvertRepository.GetByParentIDToListAsync(Parent.ID);
                    if (ListMaterialConvert.Count > 0)
                    {
                        var ListMaterialConvertCode = ListMaterialConvert.Select(x => x.Code).ToList();
                        Parent.Description = string.Join(";", ListMaterialConvertCode);
                    }
                    Parent.Description = Parent.Code + ";" + Parent.Description;
                    await _MaterialRepository.UpdateAsync(Parent);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<MaterialConvert>> CreateAutoAsync(BaseParameter<MaterialConvert> BaseParameter)
        {
            var result = new BaseResult<MaterialConvert>();
            if (BaseParameter.ID > 0)
            {
                if (BaseParameter.ListID != null)
                {
                    if (BaseParameter.ListID.Count > 0)
                    {
                        //foreach (var item in BaseParameter.ListID)
                        //{
                        //    BaseParameter.BaseModel = new MaterialConvert();
                        //    BaseParameter.BaseModel.Active = true;
                        //    BaseParameter.BaseModel.ParentID = BaseParameter.ID;
                        //    BaseParameter.BaseModel.MaterialID = item;
                        //    await SaveAsync(BaseParameter);
                        //}
                    }
                }
            }
            return result;
        }
    }
}

