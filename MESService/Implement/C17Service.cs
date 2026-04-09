namespace MESService.Implement
{
    public class C17Service : BaseService<torderlist, ItorderlistRepository>
    , IC17Service
    {
    private readonly ItorderlistRepository _torderlistRepository;
    public C17Service(ItorderlistRepository torderlistRepository

    ) : base(torderlistRepository)
    {
    _torderlistRepository = torderlistRepository;
    }
    public override void Initialization(torderlist model)
    {
    BaseInitialization(model);
    }
    public virtual async Task<BaseResult> Load()
    {
        BaseResult result = new BaseResult();
         try
         {
            await Task.Run(() => { });
         }
         catch (Exception ex)
         {
             result.Error = ex.Message;
         }
        return result;
    }
     public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
    public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
    {
        BaseResult result = new BaseResult();
         try
         {
            await Task.Run(() => { });
         }
         catch (Exception ex)
         {
             result.Error = ex.Message;
         }
        return result;
    }
     public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
     public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
     {
         BaseResult result = new BaseResult();
          try
          {
            await Task.Run(() => { });
          }
          catch (Exception ex)
          {
              result.Error = ex.Message;
          }
         return result;
     }
    public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
    {
        BaseResult result = new BaseResult();
         try
         {
            await Task.Run(() => { });
         }
         catch (Exception ex)
         {
             result.Error = ex.Message;
         }
        return result;
    }
  }
}

