namespace Service.Implement
{
    public class BOMService : BaseService<BOM, IBOMRepository>
    , IBOMService
    {

        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailService _BOMDetailService;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IBOMTermService _BOMTermService;
        private readonly IBOMStageService _BOMStageService;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IMaterialService _MaterialService;

        private readonly IWebHostEnvironment _WebHostEnvironment;

        public BOMService(IBOMRepository BOMRepository
            , IMaterialRepository materialRepository
            , IBOMDetailService BOMDetailService
            , IBOMDetailRepository BOMDetailRepository
            , IBOMTermService BOMTermService
            , IBOMStageService BOMStageService
            , IMaterialRepository MaterialRepository
            , IMaterialService materialService
            , IWebHostEnvironment WebHostEnvironment

            ) : base(BOMRepository)
        {
            _BOMRepository = BOMRepository;
            _BOMDetailService = BOMDetailService;
            _BOMDetailRepository = BOMDetailRepository;
            _BOMTermService = BOMTermService;
            _BOMStageService = BOMStageService;
            _MaterialRepository = MaterialRepository;
            _MaterialService = materialService;
            //_ECNDetailRepository = ECNDetailRepository;
            //_BOMRawDetailRepository = BOMRawDetailRepository;
            //_MaterialInfoRepository = MaterialInfoRepository;            
            //_BOMComponentRepository = BOMComponentRepository;
            //_BOMParentRepository = BOMParentRepository;
            //_TechInfoRepository = TechInfoRepository;
            _WebHostEnvironment = WebHostEnvironment;

        }
        public override void BaseInitialization(BOM model)
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
        public override void InitializationSave(BOM model)
        {
            BaseInitialization(model);
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            if (model.ParentID > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.MaterialCode;
                model.Code = Parent.Code;
                model.Version = Parent.Version;


            }
            if (model.ParentID01 > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID01.Value);
                model.ParentName01 = Parent.MaterialCode;
            }
            if (model.ParentID02 > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID02.Value);
                model.ParentName02 = Parent.MaterialCode;
            }
            if (model.ParentID03 > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID03.Value);
                model.ParentName03 = Parent.MaterialCode;
            }
            if (model.ParentID04 > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID04.Value);
                model.ParentName04 = Parent.MaterialCode;
            }
            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialCode = material.Code;
                model.MaterialName = material.Name;
                model.CategoryFamilyID = material.CategoryFamilyID;
                model.CategoryFamilyName = material.CategoryFamilyName;
            }
            if (model.ParentID > 0 && !string.IsNullOrEmpty(model.MaterialCode) && model.IsSPST == null)
            {
                if (model.MaterialCode.Contains("SP") || model.MaterialCode.Contains("ST"))
                {
                    model.IsSPST = true;
                }
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Version = model.Version ?? "1.0";
            //model.Code = model.Code ?? "ECN-" + model.Date.Value.ToString("yyyyMMddHHmmss");
            model.Quantity = model.Quantity ?? 1;

            model.Level = 0;
            if (model.ParentID > 0)
            {
                model.Level = 1;
                if (model.ParentID01 > 0)
                {
                    model.Level = 2;
                    if (model.ParentID02 > 0)
                    {
                        model.Level = 3;
                        if (model.ParentID03 > 0)
                        {
                            model.Level = 4;
                            if (model.ParentID04 > 0)
                            {
                                model.Level = 5;
                            }
                        }
                    }
                }
            }
            model.RawMaterialCount = _BOMDetailRepository.GetByCondition(o => o.ParentID == model.ID).Count();
            model.BOMCount = _BOMRepository.GetByCondition(o => o.ParentID == model.ID).Count();
            model.IsLeadNo = false;
            if (!string.IsNullOrEmpty(model.LeadNo))
            {
                if (!model.LeadNo.Contains("SP") || !model.LeadNo.Contains("ST"))
                {
                    if (model.ParentID > 0)
                    {
                        model.IsLeadNo = true;
                        model.Level = 5;
                    }
                }
            }
            if (model.IsSPST == true)
            {
                model.IsLeadNo = false;
            }
        }
        public override async Task<BaseResult<BOM>> SaveAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter != null && BaseParameter.BaseModel != null && BaseParameter.BaseModel.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.BaseModel.MaterialCode))
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.ParentID01 == BaseParameter.BaseModel.ParentID01 && o.ParentID02 == BaseParameter.BaseModel.ParentID02 && o.ParentID03 == BaseParameter.BaseModel.ParentID03 && o.ParentID04 == BaseParameter.BaseModel.ParentID04 && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Code == BaseParameter.BaseModel.Code && o.Version == BaseParameter.BaseModel.Version).OrderByDescending(o => o.Date).ThenByDescending(o => o.UpdateDate).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                bool IsSave = true;
                if (string.IsNullOrEmpty(BaseParameter.BaseModel.MaterialCode))
                {
                    IsSave = false;
                }
                if (BaseParameter.BaseModel.MaterialID == null)
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
                    if (result.BaseModel.ID > 0)
                    {
                        try
                        {
                            BaseParameter.BaseModel = result.BaseModel;
                            await SyncAsync(BaseParameter);
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> SyncAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            await SyncBOMAsync(BaseParameter);
            await SyncBOMStageAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<BOM>> SyncBOMAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    var List = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ID).ToListAsync();
                    foreach (var BOM in List)
                    {
                        BOM.Code = BaseParameter.BaseModel.Code;
                        await _BOMRepository.UpdateAsync(BOM);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> SyncBOMStageAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Stage))
                    {
                        BaseParameter.BaseModel.Stage = BaseParameter.BaseModel.Stage.Replace(",", ";");
                        foreach (var stage in BaseParameter.BaseModel.Stage.Split(';'))
                        {
                            var BaseParameterBOMStage = new BaseParameter<BOMStage>();
                            BOMStage BOMStage = new BOMStage();
                            BOMStage.ParentID = BaseParameter.BaseModel.ID;
                            BOMStage.Code = stage;
                            BaseParameterBOMStage.BaseModel = BOMStage;
                            await _BOMStageService.SaveAsync(BaseParameterBOMStage);
                        }
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<BOM>> CopyAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            long IDOld = BaseParameter.BaseModel.ID;
            BaseParameter.BaseModel.ID = 0;
            BaseParameter.BaseModel.Code = BaseParameter.BaseModel.Code + "-Copy";
            result = await AddAsync(BaseParameter);
            if (result.BaseModel.ID > 0)
            {
                BaseParameter.ID = IDOld;
                BaseParameter.BaseModel = result.BaseModel;
                await SyncCopyAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> SyncCopyAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter != null && BaseParameter.BaseModel != null && BaseParameter.BaseModel.ID > 0)
            {
                var ListBOMDetail = await _BOMDetailRepository.GetByParentIDToListAsync(BaseParameter.ID);
                foreach (var item in ListBOMDetail)
                {
                    item.ID = 0;
                    item.ParentID = BaseParameter.BaseModel.ID;
                    var BaseParameterBOMDetail = new BaseParameter<BOMDetail>();
                    BaseParameterBOMDetail.BaseModel = item;
                    await _BOMDetailService.SaveAsync(BaseParameterBOMDetail);
                }
                var ListBOM = await _BOMRepository.GetByParentIDToListAsync(BaseParameter.ID);
                foreach (var item in ListBOM)
                {
                    item.ID = 0;
                    item.ParentID = BaseParameter.BaseModel.ID;
                    var BaseParameterBOM = new BaseParameter<BOM>();
                    BaseParameterBOM.BaseModel = item;
                    await SaveAsync(BaseParameterBOM);
                }
                var ListBOMTerm = await _BOMTermService.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
                foreach (var item in ListBOMTerm)
                {
                    item.ID = 0;
                    item.ParentID = BaseParameter.BaseModel.ID;
                    var BaseParameterBOMTerm = new BaseParameter<BOMTerm>();
                    BaseParameterBOMTerm.BaseModel = item;
                    await _BOMTermService.SaveAsync(BaseParameterBOMTerm);
                }
                var ListBOMStage = await _BOMStageService.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
                foreach (var item in ListBOMStage)
                {
                    item.ID = 0;
                    item.ParentID = BaseParameter.BaseModel.ID;
                    var BaseParameterBOMStage = new BaseParameter<BOMStage>();
                    BaseParameterBOMStage.BaseModel = item;
                    await _BOMStageService.SaveAsync(BaseParameterBOMStage);
                }
            }
            return result;
        }
        public override async Task<BaseResult<BOM>> GetByParentIDToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            result.List = await GetByCondition(o => o.ParentID01 == BaseParameter.ParentID).ToListAsync();
            if (result.List.Count == 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
            }
            if (result.List.Count == 0)
            {
                result.List = await GetByCondition(o => o.ParentID02 == BaseParameter.ParentID).ToListAsync();
            }
            if (result.List.Count == 0)
            {
                result.List = await GetByCondition(o => o.ParentID03 == BaseParameter.ParentID).ToListAsync();
            }
            if (result.List.Count == 0)
            {
                result.List = await GetByCondition(o => o.ParentID04 == BaseParameter.ParentID).ToListAsync();
            }

            result.List = ListSort(result.List);
            return result;
        }
        public override async Task<BaseResult<BOM>> GetBySearchStringToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => (!string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)) || (!string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode.Contains(BaseParameter.SearchString)) || (o.MaterialID > 0 && o.MaterialID.ToString().Contains(BaseParameter.SearchString))).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> CreateAutoAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();

            var ListBOM = await GetByCondition(o => o.Active == true && (o.ParentID == null || o.ParentID == 0)).ToListAsync();
            var ListBOMSub = await GetByCondition(o => o.Active == true && o.ParentID > 0).ToListAsync();
            var ListBOMDetail = await _BOMDetailRepository.GetByActiveToListAsync(true);
            var ListBOMDetailUpdate = new List<BOMDetail>();
            foreach (var item in ListBOM)
            {
                var ListBOMSubSub = ListBOMSub.Where(o => o.ParentID == item.ID).ToList();
                if (ListBOMSubSub.Count > 0)
                {
                    var ListBOMSubSubID = ListBOMSubSub.Select(o => o.ID).ToList();
                    var ListBOMDetailSub = ListBOMDetail.Where(o => o.ParentID == item.ID).ToList();
                    foreach (var BOMDetail in ListBOMDetailSub)
                    {
                        var ListBOMDetailSubSub = ListBOMDetail.Where(o => ListBOMSubSubID.Contains(o.ParentID ?? 0) && o.MaterialID02 == BOMDetail.MaterialID02).ToList();
                        if (ListBOMDetailSubSub.Count > 0)
                        {
                            var ListBOMSubSubSubID = ListBOMDetailSubSub.Select(o => o.ParentID ?? 0).Distinct().ToList();
                            var ListBOMSubSubSub = ListBOMSubSub.Where(o => ListBOMSubSubSubID.Contains(o.ID)).ToList();
                            BOMDetail.FileName = string.Join(",", ListBOMSubSubSub.Select(o => o.MaterialCode).Distinct().ToList());
                            BOMDetail.QuantitySumActual = ListBOMDetailSubSub.Sum(o => o.Quantity02 ?? 0);
                            BOMDetail.Quantity02 = BOMDetail.Quantity02 ?? 0;
                            BOMDetail.QuantitySumActual = BOMDetail.QuantitySumActual ?? 0;
                            BOMDetail.QuantitySumCompare = BOMDetail.QuantitySumActual - BOMDetail.Quantity02;
                            BOMDetail.QuantitySumCompare = BOMDetail.QuantitySumCompare ?? 0;
                            BOMDetail.PercentSum = 0;
                            if (BOMDetail.QuantitySumActual > 0)
                            {
                                BOMDetail.PercentSum = BOMDetail.QuantitySumCompare / BOMDetail.QuantitySumActual * 100;
                            }
                            ListBOMDetailUpdate.Add(BOMDetail);
                        }
                    }
                }
            }
            await _BOMDetailRepository.UpdateRangeAsync(ListBOMDetailUpdate);
            return result;
        }
        public virtual async Task<BaseResult<BOM>> GetByCode_MaterialCodeToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (!string.IsNullOrEmpty(BaseParameter.Code) && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.Code = BaseParameter.Code.Trim();
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                result.BaseModel = await GetByCondition(o => o.Code == BaseParameter.SearchString && o.MaterialCode == BaseParameter.Code).FirstOrDefaultAsync();
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new BOM();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_Code_MaterialCodeToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (!string.IsNullOrEmpty(BaseParameter.Code) && !string.IsNullOrEmpty(BaseParameter.SearchString) && BaseParameter.CompanyID > 0)
            {
                BaseParameter.Code = BaseParameter.Code.Trim();
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                result.BaseModel = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Code == BaseParameter.SearchString && o.MaterialCode == BaseParameter.Code).FirstOrDefaultAsync();
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new BOM();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_SearchStringToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString) && BaseParameter.CompanyID > 0)
            {
                if (BaseParameter.SearchString.Split(';').Length > 1)
                {
                    var MaterialCode = BaseParameter.SearchString.Split(';')[0];
                    var Code = BaseParameter.SearchString.Split(';')[1];
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Code == Code && o.MaterialCode == MaterialCode).OrderByDescending(o => o.UpdateDate).ThenByDescending(o => o.Date).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && ((!string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)))).OrderByDescending(o => o.UpdateDate).ThenByDescending(o => o.Date).ToListAsync();
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && (!string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode.Contains(BaseParameter.SearchString))).OrderByDescending(o => o.UpdateDate).ThenByDescending(o => o.Date).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> GetByCompanyID_PageAndPageSizeToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Page > -1 && BaseParameter.PageSize > -1)
            {
                result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).OrderByDescending(o => o.UpdateDate).ThenByDescending(o => o.Date).Skip(BaseParameter.Page.Value * BaseParameter.PageSize.Value).Take(BaseParameter.PageSize.Value).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> ExportBOMLeadByIDToExcelAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter.ID > 0)
            {
                result.BaseModel = await GetByCondition(o => o.ID == BaseParameter.ID).FirstOrDefaultAsync();
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ID && o.Active == true).ToListAsync();
                    if (result.List.Count > 0)
                    {
                        var ListBOMID = result.List.Select(o => o.ID).ToList();
                        var ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => ListBOMID.Contains(o.ParentID ?? 0)).ToListAsync();
                        var ListBOMTerm = await _BOMTermService.GetByCondition(o => ListBOMID.Contains(o.ParentID ?? 0)).ToListAsync();
                        string fileName = BaseParameter.ID + "-" + result.BaseModel.MaterialCode + "-" + result.BaseModel.Code + "-BOMLead-" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
                        var streamExport = new MemoryStream();
                        InitializationExcel(result.List, ListBOMDetail, ListBOMTerm, streamExport);
                        var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                        using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                        {
                            streamExport.CopyTo(stream);
                        }
                        result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            result.List = new List<BOM>();
            BaseParameter.Quantity = BaseParameter.Quantity ?? 0;
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Name))
            {
                var ListBOMCompany = await _BOMRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true).ToListAsync();
                BaseParameter.Name = BaseParameter.Name.Trim();
                var BOM = new BOM();
                if (!string.IsNullOrEmpty(BaseParameter.Code))
                {
                    BaseParameter.Code = BaseParameter.Code.Trim();
                    BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.Code == BaseParameter.Code && o.MaterialCode == BaseParameter.Name).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                }
                else
                {
                    BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.MaterialCode == BaseParameter.Name).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                }

                if (BaseParameter.Active == true)
                {
                    if (BOM != null && BOM.ID > 0)
                    {
                        result.List = await _BOMRepository.GetByParentIDAndActiveToListAsync(BOM.ID, true);
                        if (result.List.Count > 0)
                        {
                            result.List = ListSort(result.List);
                            var ListBOMMaterialCode = result.List.Where(o => !string.IsNullOrEmpty(o.MaterialCode)).Select(o => o.MaterialCode).Distinct().ToList();
                            var ListMaterialCode = string.Join(",", ListBOMMaterialCode);
                            ListMaterialCode = ListMaterialCode.Replace(",", "','");
                            ListMaterialCode = "'" + ListMaterialCode + "'";
                            string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                            string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM IN (" + ListMaterialCode + ")";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                            var Listtrackmtim = new List<trackmtim>();
                            if (ds.Tables.Count > 0)
                            {
                                Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                            }
                            for (int i = 0; i < result.List.Count; i++)
                            {
                                result.List[i].UpdateDate = GlobalHelper.InitializationDateTime;
                                result.List[i].Description = string.Join(",", ListBOMCompany.Where(o => o.MaterialCode == result.List[i].MaterialCode).Select(o => o.ParentName).Distinct().ToList());
                                result.List[i].Quantity = BaseParameter.Quantity;
                                result.List[i].RowVersion = Listtrackmtim.Where(o => o.LEAD_NM == result.List[i].MaterialCode).Sum(o => (int?)o.QTY);
                                result.List[i].SortOrder = 0;
                                if (result.List[i].IsLeadNo == true)
                                {
                                    if (!string.IsNullOrEmpty(result.List[i].ParentName01))
                                    {
                                        var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == result.List[i].ParentName01).ToList();
                                        result.List[i].SortOrder = result.List[i].SortOrder + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                    }
                                    if (!string.IsNullOrEmpty(result.List[i].ParentName02))
                                    {
                                        var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == result.List[i].ParentName02).ToList();
                                        result.List[i].SortOrder = result.List[i].SortOrder + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                    }
                                    if (!string.IsNullOrEmpty(result.List[i].ParentName03))
                                    {
                                        var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == result.List[i].ParentName03).ToList();
                                        result.List[i].SortOrder = result.List[i].SortOrder + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                    }
                                    if (!string.IsNullOrEmpty(result.List[i].ParentName04))
                                    {
                                        var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == result.List[i].ParentName04).ToList();
                                        result.List[i].SortOrder = result.List[i].SortOrder + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                    }
                                }
                                result.List[i].RawMaterialCount = result.List[i].RowVersion + result.List[i].SortOrder;
                                result.List[i].BOMCount = BaseParameter.Quantity - result.List[i].RawMaterialCount;
                            }
                        }
                    }
                }
                else
                {
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM IN ('" + BaseParameter.Name + "')";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    var Listtrackmtim = new List<trackmtim>();
                    if (ds.Tables.Count > 0)
                    {
                        Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                    }
                    if (BOM == null)
                    {
                        BOM = new BOM();
                    }
                    BOM.MaterialCode = BaseParameter.Name;
                    BOM.UpdateDate = GlobalHelper.InitializationDateTime;
                    BOM.Description = string.Join(",", ListBOMCompany.Where(o => o.MaterialCode == BOM.MaterialCode).Select(o => o.ParentName).Distinct().ToList());
                    BOM.Quantity = BaseParameter.Quantity;
                    BOM.RowVersion = Listtrackmtim.Sum(o => (int?)o.QTY);
                    BOM.SortOrder = 0;
                    BOM.RawMaterialCount = BOM.RowVersion + BOM.SortOrder;
                    BOM.BOMCount = BaseParameter.Quantity - BOM.RawMaterialCount;
                    result.List.Add(BOM);
                }

            }
            return result;
        }
        public virtual async Task<BaseResult<BOM>> SyncFinishGoodsListOftrackmtimAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            if (BaseParameter.CompanyID > 0)
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND FinishGoodsList IS NULL";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtrackmtim = new List<trackmtim>();
                if (ds.Tables.Count > 0)
                {
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var ListBOM = await _BOMRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true).ToListAsync();
                    for (int i = 0; i < Listtrackmtim.Count; i++)
                    {
                        var ListBOMSub = ListBOM.Where(o => o.MaterialCode == Listtrackmtim[i].LEAD_NM).ToList();
                        if (ListBOMSub.Count > 0)
                        {
                            var ListBOMSubParentName = ListBOMSub.Select(o => o.ParentName).Distinct().ToList();
                            Listtrackmtim[i].FinishGoodsList = string.Join(",", ListBOMSubParentName);
                            sql = @"update trackmtim set FinishGoodsList='" + Listtrackmtim[i].FinishGoodsList + "' where TRACK_IDX=" + Listtrackmtim[i].TRACK_IDX;
                            await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                        }
                    }
                }
            }
            return result;
        }
        private List<BOM> ListSort(List<BOM> list)
        {
            List<BOM> ListBOMSub = new List<BOM>();
            var ListBOMLeadNo = list.Where(o => o.IsLeadNo == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode).ToList();
            foreach (BOM BOMLeadNo in ListBOMLeadNo)
            {
                ListBOMSub.Add(BOMLeadNo);
            }
            var ListBOMSPST = list.Where(o => o.IsSPST == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode).ToList();
            foreach (BOM BOMSPST in ListBOMSPST)
            {
                ListBOMSub.Add(BOMSPST);
                var ListBOMLeadNo01 = list.Where(o => o.IsLeadNo == true && o.ParentID01 == BOMSPST.ID && o.ParentID02 == null && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                foreach (BOM BOMLeadNo01 in ListBOMLeadNo01)
                {
                    ListBOMSub.Add(BOMLeadNo01);
                }
                var ListBOMSPSTParentID01 = list.Where(o => o.IsSPST == true && o.ParentID01 == BOMSPST.ID).OrderBy(o => o.MaterialCode).ToList();
                foreach (BOM BOMSPSTParentID01 in ListBOMSPSTParentID01)
                {
                    ListBOMSub.Add(BOMSPSTParentID01);
                    var ListBOMLeadNo02 = list.Where(o => o.IsLeadNo == true && o.ParentID02 == BOMSPSTParentID01.ID && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                    foreach (BOM BOMLeadNo02 in ListBOMLeadNo02)
                    {
                        ListBOMSub.Add(BOMLeadNo02);
                    }
                    var ListBOMSPSTParentID02 = list.Where(o => o.IsSPST == true && o.ParentID02 == BOMSPSTParentID01.ID).OrderBy(o => o.MaterialCode).ToList();
                    foreach (BOM BOMSPSTParentID02 in ListBOMSPSTParentID02)
                    {
                        ListBOMSub.Add(BOMSPSTParentID02);
                        var ListBOMLeadNo03 = list.Where(o => o.IsLeadNo == true && o.ParentID03 == BOMSPSTParentID02.ID && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                        foreach (BOM BOMLeadNo03 in ListBOMLeadNo03)
                        {
                            ListBOMSub.Add(BOMLeadNo03);
                        }
                        var ListBOMSPSTParentID03 = list.Where(o => o.IsSPST == true && o.ParentID03 == BOMSPSTParentID02.ID).OrderBy(o => o.MaterialCode).ToList();
                        foreach (BOM BOMSPSTParentID03 in ListBOMSPSTParentID03)
                        {
                            ListBOMSub.Add(BOMSPSTParentID03);
                            var ListBOMLeadNo04 = list.Where(o => o.IsLeadNo == true && o.ParentID04 == BOMSPSTParentID03.ID).OrderBy(o => o.MaterialCode).ToList();
                            foreach (BOM BOMLeadNo04 in ListBOMLeadNo04)
                            {
                                ListBOMSub.Add(BOMLeadNo04);
                            }
                            var ListBOMSPSTParentID04 = list.Where(o => o.IsSPST == true && o.ParentID04 == BOMSPSTParentID03.ID).OrderBy(o => o.MaterialCode).ToList();
                            foreach (BOM BOMSPSTParentID04 in ListBOMSPSTParentID04)
                            {
                                ListBOMSub.Add(BOMSPSTParentID04);
                            }
                        }
                    }
                }
            }
            return ListBOMSub;
        }
        public virtual async Task<BaseResult<BOM>> ExportBOMLeadByECNToExcelAsync(BaseParameter<BOM> BaseParameter)
        {
            var result = new BaseResult<BOM>();
            result.List = new List<BOM>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                var ListBOM = await GetByCondition(o => o.Code == BaseParameter.SearchString && o.Active == true && o.ParentID == null).ToListAsync();
                if (ListBOM.Count > 0)
                {
                    foreach (var item in ListBOM)
                    {
                        var ListBOMSub = await GetByCondition(o => o.ParentID == item.ID && o.Active == true).ToListAsync();
                        if (ListBOMSub.Count > 0)
                        {
                            var ListBOMID = ListBOMSub.Select(o => o.ID).ToList();
                            var ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => ListBOMID.Contains(o.ParentID ?? 0)).ToListAsync();
                            var ListBOMTerm = await _BOMTermService.GetByCondition(o => ListBOMID.Contains(o.ParentID ?? 0)).ToListAsync();
                            string fileName = item.ID + "-" + item.MaterialCode + "-" + item.Code + "-BOMLead-" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
                            var streamExport = new MemoryStream();
                            InitializationExcel(ListBOMSub, ListBOMDetail, ListBOMTerm, streamExport);
                            var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                            using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                            {
                                streamExport.CopyTo(stream);
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                            BOM BOM = new BOM();
                            BOM.Code = result.Message;
                            result.List.Add(BOM);
                        }
                    }
                }
            }
            return result;
        }
        private void InitializationExcel(List<BOM> list, List<BOMDetail> ListBOMDetail, List<BOMTerm> ListBOMTerm, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "Project";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Item";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Group";
                column = column + 1;
                workSheet.Cells[row, column].Value = "1st_Step";
                column = column + 1;
                workSheet.Cells[row, column].Value = "2nd_Step";
                column = column + 1;
                workSheet.Cells[row, column].Value = "3rd_Step";
                column = column + 1;
                workSheet.Cells[row, column].Value = "4th_Step";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Bundle Size";
                column = column + 1;
                workSheet.Cells[row, column].Value = "LEAD NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Combination Lead NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T1.Dir";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T1.No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T1.Auto";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T2.Dir";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T2.No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T2.Auto";
                column = column + 1;
                workSheet.Cells[row, column].Value = "W/Link";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Term1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "REF1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCH1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCW1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICH1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICW1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "SS1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tube1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Strip1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "W.Part no";
                column = column + 1;
                workSheet.Cells[row, column].Value = "W.R/No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Wire";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Diameter";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Color";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Length";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Term2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "REF2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCH2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCW2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICH2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICW2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "SS2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tube2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tape";
                column = column + 1;
                workSheet.Cells[row, column].Value = " Strip2";

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 16;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                row = row + 1;
                var ListBOMSub = ListSort(list);
                //var ListBOMLeadNo = list.Where(o => o.IsLeadNo == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode).ToList();
                //foreach (BOM BOMLeadNo in ListBOMLeadNo)
                //{
                //    ListBOMSub.Add(BOMLeadNo);
                //}
                //var ListBOMSPST = list.Where(o => o.IsSPST == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode).ToList();
                //foreach (BOM BOMSPST in ListBOMSPST)
                //{
                //    ListBOMSub.Add(BOMSPST);
                //    var ListBOMLeadNo01 = list.Where(o => o.IsLeadNo == true && o.ParentID01 == BOMSPST.ID && o.ParentID02 == null && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                //    foreach (BOM BOMLeadNo01 in ListBOMLeadNo01)
                //    {
                //        ListBOMSub.Add(BOMLeadNo01);
                //    }
                //    var ListBOMSPSTParentID01 = list.Where(o => o.IsSPST == true && o.ParentID01 == BOMSPST.ID).OrderBy(o => o.MaterialCode).ToList();
                //    foreach (BOM BOMSPSTParentID01 in ListBOMSPSTParentID01)
                //    {
                //        ListBOMSub.Add(BOMSPSTParentID01);
                //        var ListBOMLeadNo02 = list.Where(o => o.IsLeadNo == true && o.ParentID02 == BOMSPSTParentID01.ID && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                //        foreach (BOM BOMLeadNo02 in ListBOMLeadNo02)
                //        {
                //            ListBOMSub.Add(BOMLeadNo02);
                //        }
                //        var ListBOMSPSTParentID02 = list.Where(o => o.IsSPST == true && o.ParentID02 == BOMSPSTParentID01.ID).OrderBy(o => o.MaterialCode).ToList();
                //        foreach (BOM BOMSPSTParentID02 in ListBOMSPSTParentID02)
                //        {
                //            ListBOMSub.Add(BOMSPSTParentID02);
                //            var ListBOMLeadNo03 = list.Where(o => o.IsLeadNo == true && o.ParentID03 == BOMSPSTParentID02.ID && o.ParentID04 == null).OrderBy(o => o.MaterialCode).ToList();
                //            foreach (BOM BOMLeadNo03 in ListBOMLeadNo03)
                //            {
                //                ListBOMSub.Add(BOMLeadNo03);
                //            }
                //            var ListBOMSPSTParentID03 = list.Where(o => o.IsSPST == true && o.ParentID03 == BOMSPSTParentID02.ID).OrderBy(o => o.MaterialCode).ToList();
                //            foreach (BOM BOMSPSTParentID03 in ListBOMSPSTParentID03)
                //            {
                //                ListBOMSub.Add(BOMSPSTParentID03);
                //                var ListBOMLeadNo04 = list.Where(o => o.IsLeadNo == true && o.ParentID04 == BOMSPSTParentID03.ID).OrderBy(o => o.MaterialCode).ToList();
                //                foreach (BOM BOMLeadNo04 in ListBOMLeadNo04)
                //                {
                //                    ListBOMSub.Add(BOMLeadNo04);
                //                }
                //                var ListBOMSPSTParentID04 = list.Where(o => o.IsSPST == true && o.ParentID04 == BOMSPSTParentID03.ID).OrderBy(o => o.MaterialCode).ToList();
                //                foreach (BOM BOMSPSTParentID04 in ListBOMSPSTParentID04)
                //                {
                //                    ListBOMSub.Add(BOMSPSTParentID04);
                //                }
                //            }
                //        }
                //    }
                //}
                foreach (BOM item in ListBOMSub)
                {
                    var ListBOMDetailSub = ListBOMDetail.Where(o => o.ParentID == item.ID).ToList();
                    var ListBOMTermSub = ListBOMTerm.Where(o => o.ParentID == item.ID).ToList();
                    var BOMTerm1 = ListBOMTermSub.Where(o => o.CCH1 > 0).FirstOrDefault();
                    var BOMTerm2 = ListBOMTermSub.Where(o => o.CCH2 > 0).FirstOrDefault();
                    workSheet.Cells[row, 1].Value = item.Project;
                    workSheet.Cells[row, 2].Value = item.Item;
                    workSheet.Cells[row, 3].Value = item.Stage;
                    bool IsMaterialCode = false;
                    if (!string.IsNullOrEmpty(item.ParentName01))
                    {
                        workSheet.Cells[row, 4].Value = item.ParentName01;
                    }
                    else
                    {
                        if (IsMaterialCode == false)
                        {
                            workSheet.Cells[row, 4].Value = item.MaterialCode;
                            IsMaterialCode = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ParentName02))
                    {
                        workSheet.Cells[row, 5].Value = item.ParentName02;
                    }
                    else
                    {
                        if (IsMaterialCode == false)
                        {
                            workSheet.Cells[row, 5].Value = item.MaterialCode;
                            IsMaterialCode = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ParentName03))
                    {
                        workSheet.Cells[row, 6].Value = item.ParentName03;
                    }
                    else
                    {
                        if (IsMaterialCode == false)
                        {
                            workSheet.Cells[row, 6].Value = item.MaterialCode;
                            IsMaterialCode = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.ParentName04))
                    {
                        workSheet.Cells[row, 7].Value = item.ParentName04;
                    }
                    else
                    {
                        if (IsMaterialCode == false)
                        {
                            workSheet.Cells[row, 7].Value = item.MaterialCode;
                            IsMaterialCode = true;
                        }
                    }
                    workSheet.Cells[row, 8].Value = item.Display;
                    workSheet.Cells[row, 9].Value = item.BundleSize;
                    if (item.IsLeadNo == true)
                    {
                        workSheet.Cells[row, 10].Value = item.LeadNo;
                    }
                    else
                    {
                        workSheet.Cells[row, 10].Value = GlobalHelper.InitializationString;
                    }
                    workSheet.Cells[row, 11].Value = item.Combination;
                    workSheet.Cells[row, 12].Value = item.DirT1;
                    workSheet.Cells[row, 13].Value = item.NoT1;
                    workSheet.Cells[row, 14].Value = item.AutoT1;
                    workSheet.Cells[row, 15].Value = item.DirT2;
                    workSheet.Cells[row, 16].Value = item.NoT2;
                    workSheet.Cells[row, 17].Value = item.AutoT2;
                    workSheet.Cells[row, 18].Value = item.WLink;

                    workSheet.Cells[row, 19].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 20].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 21].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 22].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 23].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 24].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 25].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 26].Value = GlobalHelper.InitializationString;

                    if (BOMTerm1 != null && BOMTerm1.ID > 0)
                    {
                        workSheet.Cells[row, 19].Value = BOMTerm1.Code;
                        workSheet.Cells[row, 20].Value = GlobalHelper.InitializationString;
                        workSheet.Cells[row, 21].Value = BOMTerm1.CCH1;
                        workSheet.Cells[row, 22].Value = BOMTerm1.CCW1;
                        workSheet.Cells[row, 23].Value = BOMTerm1.ICH1;
                        workSheet.Cells[row, 24].Value = BOMTerm1.ICW1;
                        workSheet.Cells[row, 25].Value = GlobalHelper.InitializationString;
                        workSheet.Cells[row, 26].Value = GlobalHelper.InitializationString;
                    }
                    var BOMDetailSS1 = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.SS1).FirstOrDefault();
                    if (BOMDetailSS1 != null && BOMDetailSS1.ID > 0)
                    {
                        workSheet.Cells[row, 25].Value = BOMDetailSS1.MaterialCode02;
                    }
                    var BOMDetailTube1 = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.Tube1).FirstOrDefault();
                    if (BOMDetailTube1 != null && BOMDetailTube1.ID > 0)
                    {
                        workSheet.Cells[row, 25].Value = BOMDetailTube1.MaterialCode02;
                    }
                    workSheet.Cells[row, 27].Value = item.Strip1;

                    workSheet.Cells[row, 28].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 29].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 30].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 31].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 32].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 33].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 34].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 35].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 36].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 37].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 38].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 39].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 40].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 41].Value = GlobalHelper.InitializationString;
                    workSheet.Cells[row, 42].Value = GlobalHelper.InitializationString;
                    var BOMDetailWIRE = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.WIRE).FirstOrDefault();
                    if (BOMDetailWIRE != null && BOMDetailWIRE.ID > 0)
                    {
                        workSheet.Cells[row, 28].Value = BOMDetailWIRE.MaterialCode02;
                        workSheet.Cells[row, 29].Value = BOMDetailWIRE.WRNo;
                        workSheet.Cells[row, 30].Value = BOMDetailWIRE.Wire;
                        workSheet.Cells[row, 31].Value = BOMDetailWIRE.Diameter;
                        workSheet.Cells[row, 32].Value = BOMDetailWIRE.Color;
                        workSheet.Cells[row, 33].Value = BOMDetailWIRE.Quantity02;
                    }
                    if (BOMTerm2 != null && BOMTerm2.ID > 0)
                    {
                        workSheet.Cells[row, 34].Value = BOMTerm2.Code;
                        workSheet.Cells[row, 35].Value = GlobalHelper.InitializationString;
                        workSheet.Cells[row, 36].Value = BOMTerm2.CCH2;
                        workSheet.Cells[row, 37].Value = BOMTerm2.CCW2;
                        workSheet.Cells[row, 38].Value = BOMTerm2.ICH2;
                        workSheet.Cells[row, 39].Value = BOMTerm2.ICW2;
                    }
                    var BOMDetailSS2 = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.SS2).FirstOrDefault();
                    if (BOMDetailSS2 != null && BOMDetailSS2.ID > 0)
                    {
                        workSheet.Cells[row, 40].Value = BOMDetailSS2.MaterialCode02;
                    }
                    var BOMDetailTube2 = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.Tube2).FirstOrDefault();
                    if (BOMDetailTube2 != null && BOMDetailTube2.ID > 0)
                    {
                        workSheet.Cells[row, 41].Value = BOMDetailTube2.MaterialCode02;
                    }
                    var BOMDetailTape = ListBOMDetailSub.Where(o => o.Note == GlobalHelper.Tape).FirstOrDefault();
                    if (BOMDetailTape != null && BOMDetailTape.ID > 0)
                    {
                        workSheet.Cells[row, 42].Value = BOMDetailTape.MaterialCode02;
                    }
                    workSheet.Cells[row, 43].Value = item.Strip2;


                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 16;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    row = row + 1;
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
    }
}

