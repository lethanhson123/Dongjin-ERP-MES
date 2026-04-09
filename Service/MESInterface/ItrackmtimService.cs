namespace Service.Interface
{
    public interface ItrackmtimService : IBaseService<BOM>
    {
        Task<BaseResult<trackmtim>> GetByCompanyID_LeadNo_FinishGoodsToListAsync(BaseParameter<trackmtim> BaseParameter);
        Task<BaseResult<trackmtim>> SaveByListID_PO_FinishGoods_ECNAsync(BaseParameter<trackmtim> BaseParameter);
        Task<BaseResult<trackmtim>> GetByCompanyID_LEADNM_Begin_EndToListAsync(BaseParameter<trackmtim> BaseParameter);
        Task<BaseResult<trackmtim>> SaveAsync(BaseParameter<trackmtim> BaseParameter);
    }
}

