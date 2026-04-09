namespace Service.Interface
{
    public interface IReportService : IBaseService<Report>
    {
        Task<BaseResult<Report>> GetWarehouse001_001ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_002ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_003ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_004ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_005ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_006ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetWarehouse001_007ToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetByProductionTrackingToListAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetByProductionTracking2026Async(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> SyncByProductionTracking2026Async(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> GetByWarehouseStockLongTermAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> SyncByWarehouseStockLongTermAsync(BaseParameter<Report> BaseParameter);
        Task<BaseResult<Report>> HookRackGetByCompanyID_Begin_End_SearchStringAsync(BaseParameter<Report> BaseParameter);
    }
}

