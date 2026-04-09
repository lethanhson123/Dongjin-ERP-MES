namespace Service.Interface
{
    public interface IReportDetailService : IBaseService<ReportDetail>
    {
        Task<BaseResult<ReportDetail>> GetProductionTracking2026ByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter);
        Task<BaseResult<ReportDetail>> GetWarehouseStockLongTermByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter);
        Task<BaseResult<ReportDetail>> GetWarehouseStockLongTerm1000ByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter);
        Task<BaseResult<ReportDetail>> HookRackByParentIDExportToExcelAsync(BaseParameter<ReportDetail> BaseParameter);
    }
}

