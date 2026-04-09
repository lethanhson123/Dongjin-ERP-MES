namespace MESService.Interface
{
    public interface IB08_REPRINTService : IBaseService<torderlist>
    {
        Task<BaseResult> Load(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


