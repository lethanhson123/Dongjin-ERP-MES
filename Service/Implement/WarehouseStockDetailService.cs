namespace Service.Implement
{
    public class WarehouseStockDetailService : BaseService<WarehouseStockDetail, IWarehouseStockDetailRepository>
    , IWarehouseStockDetailService
    {
        private readonly IWarehouseStockDetailRepository _WarehouseStockDetailRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseStockDetailService(IWarehouseStockDetailRepository WarehouseStockDetailRepository, IWebHostEnvironment webHostEnvironment) : base(WarehouseStockDetailRepository)
        {
            _WarehouseStockDetailRepository = WarehouseStockDetailRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override async Task<BaseResult<WarehouseStockDetail>> SaveAsync(BaseParameter<WarehouseStockDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseStockDetail>();
            Initialization(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.ParentID == BaseParameter.BaseModel.ParentID).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
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
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            return result;
        }
    }
}

