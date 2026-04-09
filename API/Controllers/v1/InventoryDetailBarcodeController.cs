namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class InventoryDetailBarcodeController : BaseController<InventoryDetailBarcode, IInventoryDetailBarcodeService>
    {
        private readonly IInventoryDetailBarcodeService _InventoryDetailBarcodeService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public InventoryDetailBarcodeController(IInventoryDetailBarcodeService InventoryDetailBarcodeService, IWebHostEnvironment WebHostEnvironment) : base(InventoryDetailBarcodeService, WebHostEnvironment)
        {
            _InventoryDetailBarcodeService = InventoryDetailBarcodeService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("InventoryDetailBarcodeCreateAutoAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> InventoryDetailBarcodeCreateAutoAsync(string FileName = "")
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            result.List = new List<InventoryDetailBarcode>();
            result.BaseModel = new InventoryDetailBarcode();
            try
            {
                var BaseParameter = new BaseParameter<InventoryDetailBarcode>();
                //await _InventoryDetailBarcodeService.CreateAutoAsync(BaseParameter);
                string filePath = Path.Combine(_WebHostEnvironment.WebRootPath, result.BaseModel.GetType().Name, FileName);
                using (StreamReader r = new StreamReader(filePath))
                {
                    string json = r.ReadToEnd();
                    result.List = JsonConvert.DeserializeObject<List<InventoryDetailBarcode>>(json);
                }
                if (result.List != null && result.List.Count > 0)
                {
                    foreach (InventoryDetailBarcode InventoryDetailBarcode in result.List)
                    {
                        try
                        {
                            InventoryDetailBarcode.Active = true;
                            BaseParameter.BaseModel = InventoryDetailBarcode;
                            await _InventoryDetailBarcodeService.SaveAsync(BaseParameter);
                        }
                        catch (Exception ex)
                        {
                            result.StatusCode = 500;
                            result.Message = ex.Message;
                        }
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
        [Route("GetByCategoryDepartmentIDToListAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> GetByCategoryDepartmentIDToListAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailBarcodeService.GetByCategoryDepartmentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportToExcelAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailBarcodeService.ExportToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportWithCategoryLocationNameToExcelAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithCategoryLocationNameToExcelAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailBarcodeService.ExportWithCategoryLocationNameToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportWithNotExistToExcelAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithNotExistToExcelAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailBarcodeService.ExportWithNotExistToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportWithQuantityToExcelAsync")]
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithQuantityToExcelAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _InventoryDetailBarcodeService.ExportWithQuantityToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveListAsync")]
        public override async Task<BaseResult<InventoryDetailBarcode>> SaveListAsync()
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            result.List = new List<InventoryDetailBarcode>();
            result.BaseModel = new InventoryDetailBarcode();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<InventoryDetailBarcode>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null && BaseParameter.List != null && BaseParameter.List.Count > 0)
                {
                    long? ParentID = BaseParameter.List[0].ParentID;
                    string folderPathRoot = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, result.BaseModel.GetType().Name);
                    bool isFolderExists = System.IO.Directory.Exists(folderPathRoot);
                    if (!isFolderExists)
                    {
                        System.IO.Directory.CreateDirectory(folderPathRoot);
                    }
                    string fileName = result.BaseModel.GetType().Name + "-" + ParentID + "-" + GlobalHelper.InitializationDateTimeCode + ".json";
                    string path = Path.Combine(folderPathRoot, fileName);
                    bool isFileExists = System.IO.File.Exists(path);
                    if (!isFileExists)
                    {
                        var List = BaseParameter.List;
                        string json = JsonConvert.SerializeObject(List);
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.WriteLine(json);
                            }
                        }
                    }
                    BaseParameter.List = BaseParameter.List.Where(o => o.Active == true).ToList();
                    foreach (InventoryDetailBarcode item in BaseParameter.List)
                    {
                        try
                        {
                            if (item.Active == true)
                            {
                                BaseParameter.BaseModel = item;
                                await _InventoryDetailBarcodeService.SaveAsync(BaseParameter);
                                result.List.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.StatusCode = 500;
                            result.Message = ex.Message;
                        }
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

