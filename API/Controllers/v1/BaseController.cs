namespace API.Controllers.v1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController<T, TBaseService> : Controller
        where T : Data.Model.BaseModel
        where TBaseService : Service.Interface.IBaseService<T>
    {
        private readonly TBaseService _BaseService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BaseController(TBaseService baseService
            , IWebHostEnvironment WebHostEnvironment)
        {
            _BaseService = baseService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetYearList")]
        public virtual BaseResult<T> GetYearList()
        {
            var result = new BaseResult<T>();
            try
            {
                result.List = new List<T>();
                var item = (T)Activator.CreateInstance(typeof(T));
                item.Name = "All";
                item.SortOrder = 0;
                result.List.Add(item);
                for (var i = GlobalHelper.YearBegin; i < GlobalHelper.YearEnd; i++)
                {
                    item = (T)Activator.CreateInstance(typeof(T));
                    item.Name = i.ToString();
                    item.SortOrder = i;
                    result.List.Add(item);
                }
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetMonthList")]
        public virtual BaseResult<T> GetMonthList()
        {
            var result = new BaseResult<T>();
            try
            {
                result.List = new List<T>();
                for (var i = 0; i < 13; i++)
                {
                    var item = (T)Activator.CreateInstance(typeof(T));
                    if (i == 0)
                    {
                        item.Name = "All";
                    }
                    else
                    {
                        item.Name = i.ToString();
                        if (i < 10)
                        {
                            item.Name = "0" + item.Name;
                        }
                    }
                    item.SortOrder = i;
                    result.List.Add(item);
                }
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetDayList")]
        public virtual BaseResult<T> GetDayList()
        {
            var result = new BaseResult<T>();
            try
            {
                result.List = new List<T>();
                for (var i = 0; i < 32; i++)
                {
                    var item = (T)Activator.CreateInstance(typeof(T));
                    if (i == 0)
                    {
                        item.Name = "All";
                    }
                    else
                    {
                        item.Name = i.ToString();
                        if (i < 10)
                        {
                            item.Name = "0" + item.Name;
                        }
                    }
                    item.SortOrder = i;
                    result.List.Add(item);
                }
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("Save")]
        public virtual BaseResult<T> Save()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.Save(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveAsync")]
        public virtual async Task<BaseResult<T>> SaveAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.SaveAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("Copy")]
        public virtual BaseResult<T> Copy()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.Copy(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CopyAsync")]
        public virtual async Task<BaseResult<T>> CopyAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.CopyAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("Remove")]
        public virtual BaseResult<T> Remove()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.Remove(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("RemoveAsync")]
        public virtual async Task<BaseResult<T>> RemoveAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.RemoveAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByID")]
        public virtual BaseResult<T> GetByID()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByID(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByIDAsync")]
        public virtual async Task<BaseResult<T>> GetByIDAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByIDAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByName")]
        public virtual BaseResult<T> GetByName()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByName(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByNameAsync")]
        public virtual async Task<BaseResult<T>> GetByNameAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByNameAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCode")]
        public virtual BaseResult<T> GetByCode()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByCode(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCodeAsync")]
        public virtual async Task<BaseResult<T>> GetByCodeAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByCodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetAllToList")]
        public virtual BaseResult<T> GetAllToList()
        {
            var result = new BaseResult<T>();
            try
            {
                result = _BaseService.GetAllToList();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetAllToListAsync")]
        public virtual async Task<BaseResult<T>> GetAllToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                result = await _BaseService.GetAllToListAsync();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByIDToList")]
        public virtual BaseResult<T> GetByIDToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByIDToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByIDToListAsync")]
        public virtual async Task<BaseResult<T>> GetByIDToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByListIDToList")]
        public virtual BaseResult<T> GetByListIDToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByListIDToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByListIDToListAsync")]
        public virtual async Task<BaseResult<T>> GetByListIDToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByListIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCodeToList")]
        public virtual BaseResult<T> GetByCodeToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByCodeToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCodeToListAsync")]
        public virtual async Task<BaseResult<T>> GetByCodeToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByCodeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByActiveToList")]
        public virtual BaseResult<T> GetByActiveToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByActiveToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByActiveToListAsync")]
        public virtual async Task<BaseResult<T>> GetByActiveToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetByParentIDToList")]
        public virtual BaseResult<T> GetByParentIDToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByParentIDToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDToListAsync")]
        public virtual async Task<BaseResult<T>> GetByParentIDToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByParentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndActiveToList")]
        public virtual BaseResult<T> GetByParentIDAndActiveToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByParentIDAndActiveToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndActiveToListAsync")]
        public virtual async Task<BaseResult<T>> GetByParentIDAndActiveToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByParentIDAndActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDToList")]
        public virtual BaseResult<T> GetByCompanyIDToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByCompanyIDToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDToListAsync")]
        public virtual async Task<BaseResult<T>> GetByCompanyIDToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByCompanyIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndActiveToList")]
        public virtual BaseResult<T> GetByCompanyIDAndActiveToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByCompanyIDAndActiveToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndActiveToListAsync")]
        public virtual async Task<BaseResult<T>> GetByCompanyIDAndActiveToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByCompanyIDAndActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySearchStringToList")]
        public virtual BaseResult<T> GetBySearchStringToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetBySearchStringToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetBySearchStringToListAsync")]
        public virtual async Task<BaseResult<T>> GetBySearchStringToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetBySearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByPageAndPageSizeToList")]
        public virtual BaseResult<T> GetByPageAndPageSizeToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByPageAndPageSizeToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByPageAndPageSizeToListAsync")]
        public virtual async Task<BaseResult<T>> GetByPageAndPageSizeToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByPageAndPageSizeToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetAllAndEmptyToList")]
        public virtual BaseResult<T> GetAllAndEmptyToList()
        {
            var result = new BaseResult<T>();
            try
            {
                result = _BaseService.GetAllAndEmptyToList();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetAllAndEmptyToListAsync")]
        public virtual async Task<BaseResult<T>> GetAllAndEmptyToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                result = await _BaseService.GetAllAndEmptyToListAsync();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndEmptyToList")]
        public virtual BaseResult<T> GetByParentIDAndEmptyToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByParentIDAndEmptyToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndEmptyToListAsync")]
        public virtual async Task<BaseResult<T>> GetByParentIDAndEmptyToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByParentIDAndEmptyToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndEmptyToList")]
        public virtual BaseResult<T> GetByCompanyIDAndEmptyToList()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = _BaseService.GetByCompanyIDAndEmptyToList(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyIDAndEmptyToListAsync")]
        public virtual async Task<BaseResult<T>> GetByCompanyIDAndEmptyToListAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                result = await _BaseService.GetByCompanyIDAndEmptyToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFile")]
        public virtual BaseResult<T> SaveAndUploadFile()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file == null || file.Length == 0)
                    {
                    }
                    if (file != null)
                    {
                        string fileExtension = Path.GetExtension(file.FileName);
                        BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "_" + BaseParameter.BaseModel.ID + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                        if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Name))
                        {
                            BaseParameter.BaseModel.FileName = GlobalHelper.SetName(BaseParameter.BaseModel.Name) + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                        }
                        string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                        bool isFolderExists = System.IO.Directory.Exists(folderPath);
                        if (!isFolderExists)
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            BaseParameter.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameter.BaseModel.FileName;
                        }
                    }
                }
                result = _BaseService.Save(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFileAsync")]
        public virtual async Task<BaseResult<T>> SaveAndUploadFileAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file == null || file.Length == 0)
                    {
                    }
                    if (file != null)
                    {
                        string fileExtension = Path.GetExtension(file.FileName);
                        BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "_" + BaseParameter.BaseModel.ID + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                        if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Name))
                        {
                            BaseParameter.BaseModel.FileName = GlobalHelper.SetName(BaseParameter.BaseModel.Name) + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                        }
                        string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                        bool isFolderExists = System.IO.Directory.Exists(folderPath);
                        if (!isFolderExists)
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }
                        var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            BaseParameter.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameter.BaseModel.FileName;
                        }
                    }
                }
                result = await _BaseService.SaveAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFiles")]
        public virtual BaseResult<T> SaveAndUploadFiles()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string fileExtension = Path.GetExtension(file.FileName);
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "_" + BaseParameter.BaseModel.ID + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                            string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                            bool isFolderExists = System.IO.Directory.Exists(folderPath);
                            if (!isFolderExists)
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
                            var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                            using (var stream = new FileStream(physicalPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                BaseParameter.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameter.BaseModel.FileName;
                                BaseParameter.BaseModel.ID = GlobalHelper.InitializationNumber;
                                result = _BaseService.Save(BaseParameter);
                            }
                        }
                    }
                }
                else
                {
                    result = _BaseService.Save(BaseParameter);
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFilesAsync")]
        public virtual async Task<BaseResult<T>> SaveAndUploadFilesAsync()
        {
            var result = new BaseResult<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            string fileExtension = Path.GetExtension(file.FileName);
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.GetType().Name + "_" + BaseParameter.BaseModel.ParentID + "_" + i + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                            string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                            bool isFolderExists = System.IO.Directory.Exists(folderPath);
                            if (!isFolderExists)
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
                            var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                            using (var stream = new FileStream(physicalPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                                BaseParameter.BaseModel.FileName = GlobalHelper.URLSite + "/" + GlobalHelper.Upload + "/" + BaseParameter.BaseModel.GetType().Name + "/" + BaseParameter.BaseModel.FileName;
                                BaseParameter.BaseModel.ID = GlobalHelper.InitializationNumber;
                                result = await _BaseService.SaveAsync(BaseParameter);
                            }
                        }
                    }
                }
                else
                {
                    result = _BaseService.Save(BaseParameter);
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("SaveList")]
        public virtual BaseResult<T> SaveList()
        {
            var result = new BaseResult<T>();
            result.List = new List<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                foreach (T item in BaseParameter.List)
                {
                    try
                    {
                        BaseParameter.BaseModel = item;
                        _BaseService.Save(BaseParameter);
                        result.List.Add(item);
                    }
                    catch (Exception ex)
                    {
                        result.StatusCode = 500;
                        result.Message = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveListAsync")]
        public virtual async Task<BaseResult<T>> SaveListAsync()
        {
            var result = new BaseResult<T>();
            result.List = new List<T>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<T>>(Request.Form["BaseParameter"]);
                foreach (T item in BaseParameter.List)
                {
                    try
                    {
                        BaseParameter.BaseModel = item;
                        await _BaseService.SaveAsync(BaseParameter);
                        result.List.Add(item);
                    }
                    catch (Exception ex)
                    {
                        result.StatusCode = 500;
                        result.Message = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
