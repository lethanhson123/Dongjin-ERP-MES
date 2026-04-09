namespace MESService.Interface
{
    public interface IC09_START_V3Service : IBaseService<torderlist>
    {
        Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> OPER_TIME(BaseParameter BaseParameter);
        Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> Barcodebox_KeyDown_1(BaseParameter BaseParameter);
        Task<BaseResult> Barcodebox_KeyDown_1Sub01(BaseParameter BaseParameter);
        Task<BaseResult> Barcodebox_KeyDown_1Sub02(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


