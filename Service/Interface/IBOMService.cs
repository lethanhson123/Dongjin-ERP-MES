namespace Service.Interface
{
    public interface IBOMService : IBaseService<BOM>
    {
        Task<BaseResult<BOM>> CreateAutoAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> GetByCode_MaterialCodeToListAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> GetByCompanyID_Code_MaterialCodeToListAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> GetByCompanyID_SearchStringToListAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> GetByCompanyID_PageAndPageSizeToListAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> ExportBOMLeadByIDToExcelAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> ExportBOMLeadByECNToExcelAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter<BOM> BaseParameter);
        Task<BaseResult<BOM>> SyncFinishGoodsListOftrackmtimAsync(BaseParameter<BOM> BaseParameter);
    }
}

