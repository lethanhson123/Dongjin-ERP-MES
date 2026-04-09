namespace Service.Implement
{
    public class NotificationService : BaseService<CategoryCompany, ICategoryCompanyRepository>
    , INotificationService
    {
        private readonly ICategoryCompanyRepository _CategoryCompanyRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public NotificationService(ICategoryCompanyRepository CategoryCompanyRepository, IWebHostEnvironment webHostEnvironment) : base(CategoryCompanyRepository)
        {
            _CategoryCompanyRepository = CategoryCompanyRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public virtual async Task<BaseResult<Notification>> CreateWarehouseRequestAsync(BaseParameter<Notification> BaseParameter)
        {
            var result = new BaseResult<Notification>();
            try
            {
                if (BaseParameter != null && BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0 && BaseParameter.BaseModel.ParentID > 0)
                    {
                        string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, BaseParameter.BaseModel.GetType().Name);
                        bool isFolderExists = System.IO.Directory.Exists(folderPath);
                        if (!isFolderExists)
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        string fileName = BaseParameter.BaseModel.ParentID + ".json";
                        string filePath = Path.Combine(_WebHostEnvironment.WebRootPath, BaseParameter.BaseModel.GetType().Name, fileName);
                        bool isFileExists = System.IO.File.Exists(filePath);
                        if (!isFileExists)
                        {
                            List<Notification> ListNotificationNew = new List<Notification>();
                            string contentNew = JsonConvert.SerializeObject(ListNotificationNew);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    w.WriteLine(contentNew);
                                }
                            }
                        }
                        string Content = GlobalHelper.InitializationString;
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                Content = r.ReadToEnd();
                            }
                        }
                        List<Notification> ListNotification = new List<Notification>();
                        ListNotification = JsonConvert.DeserializeObject<List<Notification>>(Content);

                        BaseParameter.BaseModel.Display = "WarehouseRequestInfo";
                        BaseParameter.BaseModel.Description = GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                        ListNotification.Insert(0, BaseParameter.BaseModel);
                        if (ListNotification.Count > GlobalHelper.NotificationCount)
                        {
                            ListNotification.RemoveAt(GlobalHelper.NotificationCount.Value);
                        }
                        Content = JsonConvert.SerializeObject(ListNotification);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.WriteLine(Content);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult<Notification>> CreateWarehouseOutputDetailBarcodeFindAsync(BaseParameter<Notification> BaseParameter)
        {
            var result = new BaseResult<Notification>();
            try
            {
                if (BaseParameter != null && BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0 && BaseParameter.BaseModel.ParentID > 0)
                    {
                        string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, BaseParameter.BaseModel.GetType().Name);
                        bool isFolderExists = System.IO.Directory.Exists(folderPath);
                        if (!isFolderExists)
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        string fileName = BaseParameter.BaseModel.ParentID + ".json";
                        string filePath = Path.Combine(_WebHostEnvironment.WebRootPath, BaseParameter.BaseModel.GetType().Name, fileName);
                        bool isFileExists = System.IO.File.Exists(filePath);
                        if (!isFileExists)
                        {
                            List<Notification> ListNotificationNew = new List<Notification>();
                            string contentNew = JsonConvert.SerializeObject(ListNotificationNew);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    w.WriteLine(contentNew);
                                }
                            }
                        }
                        string Content = GlobalHelper.InitializationString;
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                Content = r.ReadToEnd();
                            }
                        }
                        List<Notification> ListNotification = new List<Notification>();
                        ListNotification = JsonConvert.DeserializeObject<List<Notification>>(Content);

                        BaseParameter.BaseModel.Display = "WarehouseOutputDetailBarcodeFind";
                        BaseParameter.BaseModel.Description = GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                        ListNotification.Insert(0, BaseParameter.BaseModel);
                        if (ListNotification.Count > GlobalHelper.NotificationCount)
                        {
                            ListNotification.RemoveAt(GlobalHelper.NotificationCount.Value);
                        }
                        Content = JsonConvert.SerializeObject(ListNotification);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.WriteLine(Content);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
    }
}

