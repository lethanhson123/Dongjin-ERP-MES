using Service.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Implement
{
    public class BOMDetailService : BaseService<BOMDetail, IBOMDetailRepository>
    , IBOMDetailService
    {
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMDetailService(IBOMDetailRepository BOMDetailRepository
            , IBOMRepository BOMRepository
            , IMaterialRepository materialRepository
            , ICategoryUnitRepository categoryUnitRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IWebHostEnvironment webHostEnvironment
            ) : base(BOMDetailRepository)
        {
            _BOMDetailRepository = BOMDetailRepository;
            _BOMRepository = BOMRepository;
            _MaterialRepository = materialRepository;
            _CategoryUnitRepository = categoryUnitRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void BaseInitialization(BOMDetail model)
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
        public override void InitializationSave(BOMDetail model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _BOMRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Version = Parent.Version;
                model.MaterialID01 = Parent.MaterialID;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (string.IsNullOrEmpty(model.Code))
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(GlobalHelper.DepartmentIDCutting.Value);
                model.Code = CategoryDepartment.Code;
            }
            if (model.MaterialID01 > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode01, model.CompanyID);
                model.MaterialID01 = Material.ID;
            }
            if (model.MaterialID01 > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID01.Value);
                model.MaterialCode01 = Material.Code;
                model.MaterialName01 = Material.Name;
                model.CategoryMaterialID01 = Material.CategoryMaterialID;
                model.CategoryMaterialName01 = Material.CategoryMaterialName;

            }
            if (model.MaterialID02 > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode02, model.CompanyID);
                model.MaterialID02 = Material.ID;
            }
            if (model.MaterialID02 > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID02.Value);
                model.MaterialCode02 = Material.Code;
                model.MaterialName02 = Material.Name;
                model.CategoryMaterialID02 = Material.CategoryMaterialID;
                model.CategoryMaterialName02 = Material.CategoryMaterialName;
            }
            if (model.CategoryUnitID01 > 0)
            {
                if (string.IsNullOrEmpty(model.CategoryUnitName01))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID01.Value);
                    model.CategoryUnitName01 = CategoryUnit.Name;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName01))
                {
                    model.CategoryUnitName01 = model.CategoryUnitName01.Trim();
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName01);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Active = true;
                        CategoryUnit.Name = model.CategoryUnitName01;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID01 = CategoryUnit.ID;
                }
            }
            if (model.CategoryUnitID02 > 0)
            {
                if (string.IsNullOrEmpty(model.CategoryUnitName02))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID02.Value);
                    model.CategoryUnitName02 = CategoryUnit.Name;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName02))
                {
                    model.CategoryUnitName02 = model.CategoryUnitName02.Trim();
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName02);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Active = true;
                        CategoryUnit.Name = model.CategoryUnitName02;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID02 = CategoryUnit.ID;
                }
            }
            model.Quantity01 = model.Quantity01 ?? 1;
            model.Quantity02 = model.Quantity02 ?? 1;
            model.QuantityActual = model.QuantityActual ?? model.Quantity02;
            model.QuantityCompare = model.QuantityActual - model.Quantity02;
            model.Percent = 0;
            if (model.Quantity02 > 0)
            {
                model.Percent = (model.QuantityCompare / model.Quantity02) * 100;
            }

            if (string.IsNullOrEmpty(model.Note))
            {
                if (!string.IsNullOrEmpty(model.Wire))
                {
                    model.Note = "WIRE";
                }
            }
        }
        public override async Task<BaseResult<BOMDetail>> SaveAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID02 == BaseParameter.BaseModel.MaterialID02).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
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
            return result;
        }
        public virtual async Task<BaseResult<BOMDetail>> SyncAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
            await SyncBOMDetailAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<BOMDetail>> SyncBOMDetailAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID02 > 0)
                            {
                                var BOM = await _BOMRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                if (BOM != null && BOM.ID > 0)
                                {
                                    var ListBOM = new List<BOM>();
                                    if (BOM.ParentID > 0)
                                    {
                                        ListBOM = await _BOMRepository.GetByParentIDAndActiveToListAsync(BOM.ParentID.Value, true);
                                    }
                                    else
                                    {
                                        ListBOM = await _BOMRepository.GetByParentIDAndActiveToListAsync(BOM.ID, true);
                                    }
                                    if (ListBOM.Count > 0)
                                    {
                                        var ListBOMID = ListBOM.Select(o => o.ID).Distinct().ToList();
                                        var ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => o.Active == true && ListBOMID.Contains(o.ParentID ?? 0) && o.MaterialID02 == BaseParameter.BaseModel.MaterialID02).ToListAsync();
                                        if (ListBOMDetail.Count > 0)
                                        {
                                            BaseParameter.BaseModel.QuantitySumActual = ListBOMDetail.Sum(o => o.Quantity02 ?? 0);
                                            BaseParameter.BaseModel.Quantity02 = BaseParameter.BaseModel.Quantity02 ?? 0;
                                            BaseParameter.BaseModel.QuantitySumActual = BaseParameter.BaseModel.QuantitySumActual ?? 0;
                                            BaseParameter.BaseModel.QuantitySumCompare = BaseParameter.BaseModel.QuantitySumActual - BaseParameter.BaseModel.Quantity02;
                                            BaseParameter.BaseModel.QuantitySumCompare = BaseParameter.BaseModel.QuantitySumCompare ?? 0;
                                            BaseParameter.BaseModel.PercentSum = 0;
                                            if (BaseParameter.BaseModel.QuantitySumActual > 0)
                                            {
                                                BaseParameter.BaseModel.PercentSum = BaseParameter.BaseModel.QuantitySumCompare / BaseParameter.BaseModel.QuantitySumActual * 100;
                                            }
                                            await _BOMDetailRepository.UpdateAsync(BaseParameter.BaseModel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public override async Task<BaseResult<BOMDetail>> GetBySearchStringToListAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
            if (string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await _BOMDetailRepository.GetAllToListAsync();
            }
            else
            {
                result.List = await _BOMDetailRepository.GetByCodeToListAsync(BaseParameter.SearchString);
                if (result.List.Count == 0)
                {
                    result.List = await _BOMDetailRepository.GetBySearchStringToListAsync(BaseParameter.SearchString);
                }
            }
            if (BaseParameter.Active == true)
            {
                result.List = result.List.Where(o => o.Code == null).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOMDetail>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
            result.List = new List<BOMDetail>();
            BaseParameter.Quantity = BaseParameter.Quantity ?? 0;
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Name))
            {
                var ListBOMCompany = await _BOMRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true).ToListAsync();
                if (BaseParameter.Active == true)
                {
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
                    if (BOM != null && BOM.ID > 0)
                    {
                        var ListBOM = await _BOMRepository.GetByParentIDAndActiveToListAsync(BOM.ID, true);
                        if (ListBOM.Count > 0)
                        {
                            var ListBOMMaterialCode = ListBOM.Where(o => !string.IsNullOrEmpty(o.MaterialCode)).Select(o => o.MaterialCode).Distinct().ToList();
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
                            foreach (var MaterialCode in ListBOMMaterialCode)
                            {
                                BOMDetail BOMDetail = new BOMDetail();
                                BOMDetail.ParentID = BOM.ID;
                                BOMDetail.Active = true;
                                BOMDetail.RowVersion = 0;
                                var BOMSub = ListBOM.Where(o => o.MaterialCode == MaterialCode).FirstOrDefault();
                                if (BOMSub != null && BOMSub.ID > 0)
                                {
                                    if (BOMSub.IsSPST == true)
                                    {
                                        BOMDetail.Active = false;
                                    }
                                    if (BOMSub.IsLeadNo == true)
                                    {
                                        if (!string.IsNullOrEmpty(BOMSub.ParentName01))
                                        {
                                            var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == BOMSub.ParentName01).ToList();
                                            BOMDetail.RowVersion = BOMDetail.RowVersion + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                        }
                                        if (!string.IsNullOrEmpty(BOMSub.ParentName02))
                                        {
                                            var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == BOMSub.ParentName02).ToList();
                                            BOMDetail.RowVersion = BOMDetail.RowVersion + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                        }
                                        if (!string.IsNullOrEmpty(BOMSub.ParentName03))
                                        {
                                            var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == BOMSub.ParentName03).ToList();
                                            BOMDetail.RowVersion = BOMDetail.RowVersion + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                        }
                                        if (!string.IsNullOrEmpty(BOMSub.ParentName04))
                                        {
                                            var ListtrackmtimParentName = Listtrackmtim.Where(o => o.LEAD_NM == BOMSub.ParentName04).ToList();
                                            BOMDetail.RowVersion = BOMDetail.RowVersion + ListtrackmtimParentName.Sum(o => (int?)o.QTY);
                                        }
                                    }
                                }
                                BOMDetail.CompanyID = BaseParameter.CompanyID;
                                BOMDetail.Code = BaseParameter.Code;
                                BOMDetail.Name = BaseParameter.Name;
                                BOMDetail.Display = MaterialCode;
                                var ListBOMSub = ListBOMCompany.Where(o => o.MaterialCode == MaterialCode).ToList();
                                BOMDetail.Description = string.Join(" | ", ListBOMSub.Select(o => o.ParentName).Distinct().ToList());
                                var ListtrackmtimSub = Listtrackmtim.Where(o => o.LEAD_NM == MaterialCode).ToList();
                                BOMDetail.SortOrder = ListtrackmtimSub.Sum(o => (int?)o.QTY);
                                BOMDetail.QuantityActual = BOMDetail.SortOrder + BOMDetail.RowVersion;
                                BOMDetail.Quantity01 = BaseParameter.Quantity;
                                BOMDetail.Quantity02 = BOMDetail.Quantity01 - BOMDetail.QuantityActual;
                                result.List.Add(BOMDetail);
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
                    BOMDetail BOMDetail = new BOMDetail();
                    BOMDetail.CompanyID = BaseParameter.CompanyID;
                    BOMDetail.Code = BaseParameter.Code;
                    BOMDetail.Name = BaseParameter.Name;
                    BOMDetail.Display = string.Join(",", Listtrackmtim.Select(o => o.LEAD_NM).Distinct().ToList());
                    var ListBOMSub = ListBOMCompany.Where(o => o.MaterialCode == BOMDetail.Display).ToList();
                    BOMDetail.Description = string.Join(",", ListBOMSub.Select(o => o.ParentName).Distinct().ToList());
                    BOMDetail.QuantityActual = Listtrackmtim.Sum(o => (decimal?)o.QTY);
                    BOMDetail.Quantity01 = BaseParameter.Quantity;
                    BOMDetail.Quantity02 = BaseParameter.Quantity - BOMDetail.QuantityActual;
                    result.List.Add(BOMDetail);
                }

            }
            result.List = result.List.OrderByDescending(o => o.Active).ThenBy(o => o.Display).ToList();
            return result;
        }

        public virtual async Task<BaseResult<BOMDetail>> SyncFinishGoodsListOftrackmtimAsync(BaseParameter<BOMDetail> BaseParameter)
        {
            var result = new BaseResult<BOMDetail>();
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
    }
}

