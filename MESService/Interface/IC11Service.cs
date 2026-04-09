namespace MESService.Interface
{
    public interface IC11Service : IBaseService<torderlist>
    {       
        Task<BaseResult> TS_USER(BaseParameter BaseParameter);
        Task<BaseResult> DB_COUTN(BaseParameter BaseParameter);
        Task<BaseResult> OPER_TIME(BaseParameter BaseParameter);
        Task<BaseResult> Barcod_read(BaseParameter BaseParameter);
        Task<BaseResult> Barcod_readSub(BaseParameter BaseParameter);
        Task<BaseResult> BOM_CHK(BaseParameter BaseParameter);

    }
}


