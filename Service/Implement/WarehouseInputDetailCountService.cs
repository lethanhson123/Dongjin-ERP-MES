namespace Service.Implement
{
    public class WarehouseInputDetailCountService : BaseService<WarehouseInputDetailCount, IWarehouseInputDetailCountRepository>
    , IWarehouseInputDetailCountService
    {
        private readonly IWarehouseInputDetailCountRepository _WarehouseInputDetailCountRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IMaterialRepository _MaterialRepository;
        public WarehouseInputDetailCountService(IWarehouseInputDetailCountRepository WarehouseInputDetailCountRepository
            , IWebHostEnvironment webHostEnvironment
            , IWarehouseInputRepository WarehouseInputRepository
            , IBOMRepository bomRepository
            , IBOMDetailRepository BOMDetailRepository
            , IWarehouseInputDetailBarcodeRepository warehouseInputDetailBarcodeRepository
            , IMaterialRepository MaterialRepository



            ) : base(WarehouseInputDetailCountRepository)
        {
            _WarehouseInputDetailCountRepository = WarehouseInputDetailCountRepository;
            _WebHostEnvironment = webHostEnvironment;
            _WarehouseInputRepository = WarehouseInputRepository;
            _BOMRepository = bomRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _WarehouseInputDetailBarcodeRepository = warehouseInputDetailBarcodeRepository;
            _MaterialRepository = MaterialRepository;
        }
        public override void InitializationSave(WarehouseInputDetailCount model)
        {
            model.Active = false;
            if (model.ParentID > 0)
            {
                var Parent = _WarehouseInputRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                if (Material.ID > 0)
                {
                    model.MaterialName = Material.Code;
                }
            }
            if (model.Date != null)
            {
                model.Year = model.Date.Value.Year;
                model.Month = model.Date.Value.Month;
                model.Day = model.Date.Value.Day;
                model.Week = GlobalHelper.GetWeekByDateTime(model.Date.Value);
                var DateTarget = DateTime.Now.AddMonths(-3);
                if (DateTarget > model.Date)
                {
                    model.Active = true;
                }
            }
            if (!string.IsNullOrEmpty(model.ECN) && model.CompanyID > 0)
            {
                var BOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialID == model.MaterialID && o.Code == model.ECN).OrderByDescending(o => o.Date).FirstOrDefault();
                if (BOM == null)
                {
                    _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialID == model.MaterialID).OrderByDescending(o => o.Date).FirstOrDefault();
                }
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMID = BOM.ID;
                    model.ECN = BOM.Code;
                    model.BOMDate = BOM.Date;
                }
            }
            var ListWarehouseInputDetailBarcode = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID == model.ParentID && o.MaterialID == model.MaterialID && o.BOMID == model.BOMID && o.DateScan != null && o.DateScan.Value.Date == model.Date.Value.Date).ToList();
            model.Count = ListWarehouseInputDetailBarcode.Count;

        }
        public override async Task<BaseResult<WarehouseInputDetailCount>> SaveAsync(BaseParameter<WarehouseInputDetailCount> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailCount>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.BOMID == BaseParameter.BaseModel.BOMID && o.Date != null && BaseParameter.BaseModel.Date != null && o.Date.Value.Date == BaseParameter.BaseModel.Date.Value.Date).FirstOrDefaultAsync();
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
            }
            return result;
        }
    }
}

