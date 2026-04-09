using Microsoft.AspNetCore.Mvc;

namespace MESService.Interface
{
    public interface IMAIN_LoginService : IBaseService<tsuser>
    {
        Task<BaseResult> ChangePassword_Click(ChangePasswordModel model);
        Task<BaseResult> OK_Click(BaseParameter BaseParameter);
    }
}

