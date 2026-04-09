namespace Service.Implement
{
    public class MaterialService : BaseService<Material, IMaterialRepository>
    , IMaterialService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IMaterialRepository _MaterialRepository;

        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        

        private readonly ICategoryMaterialRepository _CategoryMaterialRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly ICategoryFamilyRepository _CategoryFamilyRepository;

        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseOutputDetailBarcodeService _WarehouseOutputDetailBarcodeService;

        private readonly IMembershipRepository _MembershipRepository;


        public MaterialService(IMaterialRepository MaterialRepository
            , IWebHostEnvironment WebHostEnvironment
            , IMaterialConvertRepository MaterialConvertRepository
            

            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
            , IWarehouseOutputDetailBarcodeService WarehouseOutputDetailBarcodeService

            , ICategoryMaterialRepository CategoryMaterialRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , ICategoryFamilyRepository CategoryFamilyRepository

            , IMembershipRepository MembershipRepository

            ) : base(MaterialRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _MaterialRepository = MaterialRepository;
            

            _MaterialConvertRepository = MaterialConvertRepository;

            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseOutputDetailBarcodeService = WarehouseOutputDetailBarcodeService;

            _CategoryMaterialRepository = CategoryMaterialRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _CategoryFamilyRepository = CategoryFamilyRepository;
            _MembershipRepository = MembershipRepository;

        }
        public override void BaseInitialization(Material model)
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
        public override void InitializationSave(Material model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Material.Code;
            }
            model.CategoryMaterialID = model.CategoryMaterialID ?? GlobalHelper.CategoryMaterialID;
            if (model.CategoryMaterialID > 0)
            {
                var CategoryMaterial = _CategoryMaterialRepository.GetByID(model.CategoryMaterialID.Value);
                model.CategoryMaterialName = CategoryMaterial.Name;
            }
            if (model.CategoryFamilyID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryFamilyName))
                {
                    model.CategoryFamilyName = model.CategoryFamilyName.Trim();
                    model.CategoryFamilyName = model.CategoryFamilyName.Replace(@"Enter Family(PC)", @"");
                    var CategoryFamily = _CategoryFamilyRepository.GetByName(model.CategoryFamilyName);
                    if (CategoryFamily.ID == 0)
                    {
                        CategoryFamily.Active = true;
                        CategoryFamily.Name = model.CategoryFamilyName;
                        _CategoryFamilyRepository.Add(CategoryFamily);
                    }
                    model.CategoryFamilyID = CategoryFamily.ID;
                }
            }
            if (model.CategoryFamilyID > 0)
            {
                var CategoryFamily = _CategoryFamilyRepository.GetByID(model.CategoryFamilyID.Value);
                model.CategoryFamilyName = CategoryFamily.Name;
            }
            if (model.CategoryLocationID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryLocationName))
                {
                    model.CategoryLocationName = model.CategoryLocationName.Trim();
                    var CategoryLocation = _CategoryLocationRepository.GetByName(model.CategoryLocationName);
                    if (CategoryLocation.ID == 0)
                    {
                        CategoryLocation.Active = true;
                        CategoryLocation.Name = model.CategoryLocationName;
                        _CategoryLocationRepository.Add(CategoryLocation);
                    }
                    model.CategoryLocationID = CategoryLocation.ID;
                }
            }
            if (model.CategoryLocationID > 0)
            {
                var CategoryLocation = _CategoryLocationRepository.GetByID(model.CategoryLocationID.Value);
                model.CategoryLocationName = CategoryLocation.Name;
            }
            //if (model.CategoryLineID > 0)
            //{
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(model.CategoryLineName))
            //    {
            //        model.CategoryLineName = model.CategoryLineName.Trim();
            //        var CategoryLine = _CategoryLineRepository.GetByName(model.CategoryLineName);
            //        if (CategoryLine.ID == 0)
            //        {
            //            CategoryLine.Active = true;
            //            CategoryLine.Name = model.CategoryLineName;
            //            _CategoryLineRepository.Add(CategoryLine);
            //        }
            //        model.CategoryLineID = CategoryLine.ID;
            //    }
            //}
            //if (model.CategoryLineID > 0)
            //{
            //    var CategoryLine = _CategoryLineRepository.GetByID(model.CategoryLineID.Value);
            //    model.CategoryLineName = CategoryLine.Name;
            //}
            if (!string.IsNullOrEmpty(model.Code))
            {
                model.FileName = QRCodeHelper.CreateQRCodeViaString(model.Code);
            }
            model.QuantitySNP = model.QuantitySNP ?? 1;
            model.QuantityInput = model.QuantityInput ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput = model.QuantityOutput ?? GlobalHelper.InitializationNumber;

            model.Quantity = model.QuantityInput - model.QuantityOutput;

            model.Code = model.Code ?? GlobalHelper.InitializationString;
            model.CategoryLocationName01 = model.CategoryLocationName01 ?? model.CategoryLocationName;
            var ListMaterialConvert = _MaterialConvertRepository.GetByParentIDToList(model.ID);
            if (ListMaterialConvert.Count > 0)
            {
                var ListMaterialConvertCode = ListMaterialConvert.Select(x => x.Code).ToList();
                model.Description = string.Join(";", ListMaterialConvertCode);
            }
            model.Description = model.Code + ";" + model.Description;
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;

            if (string.IsNullOrEmpty(model.OriginalEquipmentManufacturer))
            {
                if (model.CompanyID == 17)
                {
                    model.OriginalEquipmentManufacturer = "ZDA";
                }
            }
            if (string.IsNullOrEmpty(model.CarMaker))
            {
                if (model.CompanyID == 17)
                {
                    model.CarMaker = "KG모빌리티 (SsangYong)";
                }
            }
            if (string.IsNullOrEmpty(model.DevelopmentStage))
            {
                if (model.CompanyID == 17)
                {
                    model.DevelopmentStage = "Mass production";
                }
            }
        }
        public override async Task<BaseResult<Material>> SaveAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Code == BaseParameter.BaseModel.Code).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CompanyID == null)
            {
                IsSave = false;
            }
            if (IsSave == true)
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
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> CreateAutoAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();   
            
            return result;
        }
        public virtual async Task<BaseResult<Material>> SyncParentChildAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            var resultList = await GetAllToListAsync();
            var ListMaterial = resultList.List.OrderBy(item => item.Code).ToList();
            foreach (var Material in ListMaterial)
            {
                var ListMaterialSub = ListMaterial.Where(item => item.Code.Contains(Material.Code)).ToList();
                if (ListMaterialSub.Count > 0)
                {
                    foreach (var MaterialSub in ListMaterialSub)
                    {
                        if (MaterialSub.ID != Material.ID)
                        {
                            var MaterialConvert = await _MaterialConvertRepository.GetByCondition(item => item.ParentID == Material.ID && item.MaterialID == MaterialSub.ID).FirstOrDefaultAsync();
                            if ((MaterialConvert == null) || (MaterialConvert.ID == 0))
                            {
                                MaterialConvert = new MaterialConvert();
                                MaterialConvert.Active = true;
                                MaterialConvert.ParentID = Material.ID;
                                MaterialConvert.ParentName = Material.Code;
                                MaterialConvert.Note = Material.Name;
                                MaterialConvert.MaterialID = MaterialSub.ID;
                                MaterialConvert.Code = MaterialSub.Code;
                                MaterialConvert.Name = MaterialSub.Name;
                                await _MaterialConvertRepository.AddAsync(MaterialConvert);
                            }
                            MaterialSub.ParentID = Material.ID;
                            MaterialSub.ParentName = Material.Code;
                            await _MaterialRepository.UpdateAsync(MaterialSub);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> SyncByWarehouseAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            var Material = await _MaterialRepository.GetByIDAsync(BaseParameter.ID);
            if (Material != null)
            {
                var ListWarehouseInput = await _WarehouseInputDetailRepository.GetByCondition(item => item.MaterialID == Material.ID).ToListAsync();
                if (ListWarehouseInput != null && ListWarehouseInput.Count > 0)
                {
                    Material.QuantityInput = ListWarehouseInput.Sum(x => x.Quantity);
                }

                var ListWarehouseOutput = await _WarehouseOutputDetailRepository.GetByCondition(item => item.MaterialID == Material.ID).ToListAsync();
                if (ListWarehouseOutput != null && ListWarehouseOutput.Count > 0)
                {
                    Material.QuantityOutput = ListWarehouseOutput.Sum(x => x.Quantity);
                }

                if (Material.QuantityInput == null)
                {
                    Material.QuantityInput = GlobalHelper.InitializationNumber;
                }
                if (Material.QuantityOutput == null)
                {
                    Material.QuantityOutput = GlobalHelper.InitializationNumber;
                }
                Material.Quantity = Material.QuantityInput - Material.QuantityOutput;
                await _MaterialRepository.UpdateAsync(Material);
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByWarehouseInputIDToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            result.List = new List<Material>();
            if (BaseParameter.GeneralID > 0)
            {
                var List = await _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.GeneralID).ToListAsync();
                if ((List != null) && (List.Count > 0))
                {
                    var ListID = List.Select(o => o.MaterialID).ToList();
                    result.List = await GetByCondition(o => ListID.Contains(o.ID)).ToListAsync();
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderBy(o => o.Code).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByWarehouseOutputIDToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            result.List = new List<Material>();
            if (BaseParameter.GeneralID > 0)
            {
                var List = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.GeneralID).ToListAsync();
                if ((List != null) && (List.Count > 0))
                {
                    var ListID = List.Select(o => o.MaterialID).ToList();
                    result.List = await GetByCondition(o => ListID.Contains(o.ID)).ToListAsync();
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderBy(o => o.Code).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByCategoryMaterialIDToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            if (BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.CategoryMaterialID == BaseParameter.GeneralID).ToListAsync();
            }
            else
            {
                result = await GetAllToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByParentID_ActiveToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == BaseParameter.Active).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByCategoryMaterialID_ActiveToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            if (BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.CategoryMaterialID == BaseParameter.GeneralID && o.Active == BaseParameter.Active).ToListAsync();
            }
            else
            {
                result = await GetAllToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> PrintAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.BaseModel != null)
                {
                    string HTMLContent = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Material400.html");
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
        public virtual async Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_SearchStringToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            result.List = new List<Material>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString))).ToListAsync();
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.OriginalEquipmentManufacturer) && o.OriginalEquipmentManufacturer.Contains(BaseParameter.SearchString))).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.CarMaker) && o.CarMaker.Contains(BaseParameter.SearchString))).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.CarType) && o.CarType.Contains(BaseParameter.SearchString))).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.Item) && o.Item.Contains(BaseParameter.SearchString))).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.DevelopmentStage) && o.DevelopmentStage.Contains(BaseParameter.SearchString))).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.ID.ToString().Contains(BaseParameter.SearchString)).ToListAsync();
                    }
                }
                else
                {
                    if (BaseParameter.GeneralID > 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryMaterialID == BaseParameter.GeneralID).OrderByDescending(o => o.UpdateDate).Take(100).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).Take(100).ToListAsync();
                    }
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.UpdateDate).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_ActiveToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            result.List = new List<Material>();
            if (BaseParameter.CompanyID > 0)
            {
                if (BaseParameter.GeneralID > 0)
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryMaterialID == BaseParameter.GeneralID && o.Active == BaseParameter.Active).OrderByDescending(o => o.UpdateDate).ToListAsync();
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.UpdateDate).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<Material>> GetFromtmmtinByIDToListAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();            
            return result;
        }
        public virtual async Task<BaseResult<Material>> ExportToExcelAsync(BaseParameter<Material> BaseParameter)
        {
            var result = new BaseResult<Material>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.CategoryMaterialID == BaseParameter.GeneralID).ToListAsync();
                var ListMaterial = new List<Material>();
                foreach (var Material in result.List)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Material.MESUpdateUserCode))
                        {
                            var UserID = long.Parse(Material.MESUpdateUserCode);
                        }
                        ListMaterial.Add(Material);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                    }
                }

                string fileName = @"Material-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationExcel(ListMaterial, streamExport);
                var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
            }
            return result;
        }
        private void InitializationExcel(List<Material> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Company";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Category";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NAME";
                column = column + 1;               
                workSheet.Cells[row, column].Value = "Family";
                column = column + 1;
                workSheet.Cells[row, column].Value = "OEM";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Car maker";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Car type";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Item";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Development Stage";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity SNP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Line";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Description";
                


                row = row + 1;
                foreach (Material item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.CompanyName;
                    workSheet.Cells[row, 3].Value = item.CategoryMaterialName;
                    workSheet.Cells[row, 4].Value = item.Code;                    
                    workSheet.Cells[row, 5].Value = item.Name;
                    workSheet.Cells[row, 6].Value = item.CategoryFamilyName;
                    workSheet.Cells[row, 7].Value = item.OriginalEquipmentManufacturer;
                    workSheet.Cells[row, 8].Value = item.CarMaker;
                    workSheet.Cells[row, 9].Value = item.CarType;
                    workSheet.Cells[row, 10].Value = item.Item;
                    workSheet.Cells[row, 11].Value = item.DevelopmentStage;
                    workSheet.Cells[row, 12].Value = item.QuantitySNP;
                    workSheet.Cells[row, 13].Value = item.CategoryLineName;
                    workSheet.Cells[row, 14].Value = item.CategoryLocationName;
                    workSheet.Cells[row, 15].Value = item.Description;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 11;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    row = row + 1;
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 12;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;

        }
    }
}

