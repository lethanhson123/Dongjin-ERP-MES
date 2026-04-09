namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotificationController : Controller
    {        
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public NotificationController(INotificationService NotificationService, IWebHostEnvironment WebHostEnvironment)
        {     
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByParentIDToListAsync")]
        public virtual async Task<BaseResult<Notification>> GetByParentIDToListAsync()
        {
            var result = new BaseResult<Notification>();
            result.List = new List<Notification>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Notification>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        BaseParameter.BaseModel = new Notification();
                        string fileName = BaseParameter.ParentID + ".json";
                        string filePath = Path.Combine(_WebHostEnvironment.WebRootPath, BaseParameter.BaseModel.GetType().Name, fileName);
                        string content = GlobalHelper.InitializationString;
                        try
                        {
                            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    content = await r.ReadToEndAsync();
                                }
                            }
                            result.List = JsonConvert.DeserializeObject<List<Notification>>(content);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}


