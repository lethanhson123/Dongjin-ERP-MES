namespace Service.Implement
{
    public class BOMCompareService : BaseService<BOMCompare, IBOMCompareRepository>
    , IBOMCompareService
    {

        private readonly IBOMCompareRepository _BOMCompareRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        public BOMCompareService(IBOMCompareRepository BOMCompareRepository
            , IBOMRepository BOMRepository
            , IBOMDetailRepository BOMDetailRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            ) : base(BOMCompareRepository)
        {
            _BOMCompareRepository = BOMCompareRepository;
            _BOMRepository = BOMRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
        }
        public override void Initialization(BOMCompare model)
        {
            BaseInitialization(model);

            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;

            if (model.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(model.ParentID.Value);
                model.Code = Parent.Code;
                model.ParentName = Parent.Name;
            }

            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialName = Material.Code;
                model.Display = Material.Name;
            }

            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }

            model.Quantity01 = model.Quantity01 ?? GlobalHelper.InitializationNumber;
            model.Quantity02 = model.Quantity02 ?? GlobalHelper.InitializationNumber;
            model.Quantity03 = model.Quantity03 ?? GlobalHelper.InitializationNumber;
            model.Quantity04 = model.Quantity04 ?? GlobalHelper.InitializationNumber;
            model.Quantity05 = model.Quantity05 ?? GlobalHelper.InitializationNumber;
            model.Quantity06 = model.Quantity06 ?? GlobalHelper.InitializationNumber;
            model.Quantity07 = model.Quantity07 ?? GlobalHelper.InitializationNumber;
            model.Quantity08 = model.Quantity08 ?? GlobalHelper.InitializationNumber;
            model.Quantity09 = model.Quantity09 ?? GlobalHelper.InitializationNumber;

            model.QuantityActual01 = model.Quantity02 > 0 ? model.Quantity02 : model.Quantity01;
            model.QuantityActual02 = model.Quantity03 > 0 ? model.Quantity03 : model.Quantity02;
            model.QuantityActual03 = model.Quantity04 > 0 ? model.Quantity04 : model.Quantity03;
            model.QuantityActual04 = model.Quantity05 > 0 ? model.Quantity05 : model.Quantity04;
            model.QuantityActual05 = model.Quantity06 > 0 ? model.Quantity06 : model.Quantity05;
            model.QuantityActual06 = model.Quantity07 > 0 ? model.Quantity07 : model.Quantity06;
            model.QuantityActual07 = model.Quantity08 > 0 ? model.Quantity08 : model.Quantity07;
            model.QuantityActual08 = model.Quantity09 > 0 ? model.Quantity09 : model.Quantity08;
            model.QuantityActual09 = model.Quantity09 > 0 ? model.Quantity09 : 0;


            model.QuantityGAP01 = model.Quantity01 - model.QuantityActual01;
            model.QuantityGAP02 = model.Quantity02 - model.QuantityActual02;
            model.QuantityGAP03 = model.Quantity03 - model.QuantityActual03;
            model.QuantityGAP04 = model.Quantity04 - model.QuantityActual04;
            model.QuantityGAP05 = model.Quantity05 - model.QuantityActual05;
            model.QuantityGAP06 = model.Quantity06 - model.QuantityActual06;
            model.QuantityGAP07 = model.Quantity07 - model.QuantityActual07;
            model.QuantityGAP08 = model.Quantity08 - model.QuantityActual08;
            model.QuantityGAP09 = model.Quantity09 - model.QuantityActual09;

            model.QuantityGAP = model.QuantityGAP01 + model.QuantityGAP02 + model.QuantityGAP03 + model.QuantityGAP04 + model.QuantityGAP05 + model.QuantityGAP06 + model.QuantityGAP07 + model.QuantityGAP08 + model.QuantityGAP09;
        }
        public override async Task<BaseResult<BOMCompare>> SaveAsync(BaseParameter<BOMCompare> BaseParameter)
        {
            var result = new BaseResult<BOMCompare>();
            //Initialization(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.CompanyID == BaseParameter.BaseModel.CompanyID && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<BOMCompare>> GetCompanyID_YearBegin_YearEndToListAsync(BaseParameter<BOMCompare> BaseParameter)
        {
            var result = new BaseResult<BOMCompare>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Year > 0 && BaseParameter.Month > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = await GetByCondition(o => o.QuantityGAP > 0 && o.CompanyID == BaseParameter.CompanyID && o.YearBegin == BaseParameter.Month && o.YearEnd == BaseParameter.Year).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.YearBegin == BaseParameter.Month && o.YearEnd == BaseParameter.Year).ToListAsync();
                }
                result.List = result.List.OrderBy(o => o.Code).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<BOMCompare>> SyncByCompanyID_YearBegin_YearEndToListAsync(BaseParameter<BOMCompare> BaseParameter)
        {
            var result = new BaseResult<BOMCompare>();
            result.List = new List<BOMCompare>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.Year > 0 && BaseParameter.Month > 0)
            {
                var ListBOMCompare = await _BOMCompareRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.YearBegin == BaseParameter.Month && o.YearEnd == BaseParameter.Year).ToListAsync();
                await _BOMCompareRepository.RemoveRangeAsync(ListBOMCompare);
                var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == null && o.CompanyID == BaseParameter.CompanyID && o.Date != null && o.Date.Value.Year >= BaseParameter.Month && o.Date.Value.Year <= BaseParameter.Year).OrderByDescending(o => o.Date.Value.Date).ThenByDescending(o => o.Code).ToListAsync();
                if (ListBOM.Count > 0)
                {
                    var ListBOMID = ListBOM.Select(o => o.ID).ToList();
                    var ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListBOMID.Contains(o.ParentID.Value)).ToListAsync();
                    if (ListBOMDetail.Count > 0)
                    {
                        var ListBOMMaterialCode = ListBOM.Select(o => o.MaterialCode).Distinct().ToList();
                        foreach (var MaterialCode in ListBOMMaterialCode)
                        {
                            var ListBOMDetailMaterialCode02 = ListBOMDetail.Where(o => o.MaterialCode01 == MaterialCode).Select(o => o.MaterialCode02).Distinct().ToList();
                            foreach (var MaterialCode02 in ListBOMDetailMaterialCode02)
                            {
                                var ListBOMDetailParentID = ListBOMDetail.Where(o => o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Select(o => o.ParentID).Distinct().ToList();
                                if (ListBOMDetailParentID.Count > 0)
                                {
                                    var ListBOMSub = ListBOM.Where(o => ListBOMDetailParentID.Contains(o.ID)).OrderByDescending(o => o.Date).ToList();
                                    var BOMCompare = new BOMCompare();
                                    BOMCompare.CompanyID = BaseParameter.CompanyID;
                                    BOMCompare.YearBegin = BaseParameter.Month;
                                    BOMCompare.YearEnd = BaseParameter.Year;
                                    BOMCompare.Code = MaterialCode;
                                    BOMCompare.MaterialName = MaterialCode02;

                                    var End = 9;
                                    if (ListBOMSub.Count < End)
                                    {
                                        End = ListBOMSub.Count;
                                    }
                                    for (int i = 0; i < End; i++)
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                BOMCompare.BOMID01 = ListBOMSub[i].ID;
                                                BOMCompare.ECN01 = ListBOMSub[i].Code;
                                                BOMCompare.Date01 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity01 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 1:
                                                BOMCompare.BOMID02 = ListBOMSub[i].ID;
                                                BOMCompare.ECN02 = ListBOMSub[i].Code;
                                                BOMCompare.Date02 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity02 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 2:
                                                BOMCompare.BOMID03 = ListBOMSub[i].ID;
                                                BOMCompare.ECN03 = ListBOMSub[i].Code;
                                                BOMCompare.Date03 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity03 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 3:
                                                BOMCompare.BOMID04 = ListBOMSub[i].ID;
                                                BOMCompare.ECN04 = ListBOMSub[i].Code;
                                                BOMCompare.Date04 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity04 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 4:
                                                BOMCompare.BOMID05 = ListBOMSub[i].ID;
                                                BOMCompare.ECN05 = ListBOMSub[i].Code;
                                                BOMCompare.Date05 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity05 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 5:
                                                BOMCompare.BOMID06 = ListBOMSub[i].ID;
                                                BOMCompare.ECN06 = ListBOMSub[i].Code;
                                                BOMCompare.Date06 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity06 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 6:
                                                BOMCompare.BOMID07 = ListBOMSub[i].ID;
                                                BOMCompare.ECN07 = ListBOMSub[i].Code;
                                                BOMCompare.Date07 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity07 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 7:
                                                BOMCompare.BOMID08 = ListBOMSub[i].ID;
                                                BOMCompare.ECN08 = ListBOMSub[i].Code;
                                                BOMCompare.Date08 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity08 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                            case 8:
                                                BOMCompare.BOMID09 = ListBOMSub[i].ID;
                                                BOMCompare.ECN09 = ListBOMSub[i].Code;
                                                BOMCompare.Date09 = ListBOMSub[i].Date;
                                                BOMCompare.Quantity09 = ListBOMDetail.Where(o => o.ParentID == ListBOMSub[i].ID && o.MaterialCode01 == MaterialCode && o.MaterialCode02 == MaterialCode02).Sum(o => o.Quantity02);
                                                break;
                                        }
                                    }
                                    Initialization(BOMCompare);
                                    result.List.Add(BOMCompare);
                                }
                            }
                        }
                        await _BOMCompareRepository.AddRangeAsync(result.List);
                    }
                }
            }
            return result;
        }
    }
}

