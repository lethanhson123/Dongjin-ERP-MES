namespace MESService.Interface
{
    public interface IC05_STARTService : IBaseService<torderlist>
    {
        Task<BaseResult> C05_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> C05_start_Load(BaseParameter BaseParameter);
        Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> DB_COUTN(BaseParameter BaseParameter);
        Task<BaseResult> OPER_TIME(BaseParameter BaseParameter);
        Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> BARCODE_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> BARCODE_LOADSub001(BaseParameter BaseParameter);
        Task<BaseResult> BARCODE_LOADSub002(BaseParameter BaseParameter);
        Task<BaseResult> BARCODE_LOADSub003(BaseParameter BaseParameter);
    }
}


