namespace Service.Interface
{
    public interface IBOMDetailService : IBaseService<BOMDetail>
    {
        Task<BaseResult<BOMDetail>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter<BOMDetail> BaseParameter);
        Task<BaseResult<BOMDetail>> SyncFinishGoodsListOftrackmtimAsync(BaseParameter<BOMDetail> BaseParameter);
    }
}

