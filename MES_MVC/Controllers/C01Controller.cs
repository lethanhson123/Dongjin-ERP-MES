using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace MES.Controllers
{
    public class C01Controller : Controller
    {
        private readonly IC01Service _C01Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C01Controller(IC01Service C01Service, IWebHostEnvironment webHostEnvironment)
        {
            _C01Service = C01Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonfind_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonadd_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonsave_Click(BaseParameter);

                //Call API Upload
                //var _httpClient = new HttpClient();
                //string jsonPayload = System.Text.Json.JsonSerializer.Serialize(BaseParameter);
                //StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                //MultipartFormDataContent formData = new MultipartFormDataContent();
                //formData.Add(content, "BaseParameter");
                //HttpResponseMessage response = await _httpClient.PostAsync(GlobalHelper.APIUploadSite + "/api/v1/C01_MES/Buttonsave_Click", formData);
                //if (response.IsSuccessStatusCode)
                //{
                //    string responseBody = await response.Content.ReadAsStringAsync();
                //    BaseResult = JsonConvert.DeserializeObject<BaseResult>(responseBody);
                //}
                //else
                //{
                //    Console.WriteLine($"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                //}
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttoncancel_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseResult.DataGridView1 = new List<SuperResultTranfer>();
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
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
                            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                            string fileExtension = Path.GetExtension(file.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                            {
                                string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                                Directory.CreateDirectory(physicalPath);
                                string filePath = Path.Combine(physicalPath, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }
                                FileInfo fileLocation = new FileInfo(filePath);
                                try
                                {
                                    if (fileLocation.Length > 0)
                                    {
                                        if (fileExtension == ".xlsx" || fileExtension == ".xls")
                                        {
                                            using (ExcelPackage package = new ExcelPackage(fileLocation))
                                            {
                                                if (package.Workbook.Worksheets.Count > 0)
                                                {
                                                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                    if (workSheet != null)
                                                    {
                                                        int totalRows = workSheet.Dimension.Rows;
                                                        if (totalRows > 5000)
                                                        {
                                                            totalRows = 5000;
                                                        }
                                                        for (int j = 2; j <= totalRows; j++)
                                                        {
                                                            SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();

                                                            if (workSheet.Cells[j, 1].Value != null)
                                                            {
                                                                SuperResultTranfer.PO_CODE = workSheet.Cells[j, 1].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 2].Value != null)
                                                            {
                                                                SuperResultTranfer.ECNNo = workSheet.Cells[j, 2].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 3].Value != null)
                                                            {
                                                                SuperResultTranfer.TORDER_FG = workSheet.Cells[j, 3].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 4].Value != null)
                                                            {
                                                                SuperResultTranfer.LEAD_NO = workSheet.Cells[j, 4].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 5].Value != null)
                                                            {
                                                                SuperResultTranfer.PROJECT = workSheet.Cells[j, 5].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 6].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    SuperResultTranfer.TOT_QTY = double.Parse(workSheet.Cells[j, 6].Value.ToString().Trim());
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    BaseResult.Error = ex.Message;
                                                                    BaseResult.ErrorNumber = 1;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 7].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    SuperResultTranfer.ADJ_AF_QTY = double.Parse(workSheet.Cells[j, 7].Value.ToString().Trim());
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    BaseResult.Error = ex.Message;
                                                                    BaseResult.ErrorNumber = 1;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 8].Value != null)
                                                            {
                                                                SuperResultTranfer.CUR_LEADS = workSheet.Cells[j, 8].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 9].Value != null)
                                                            {
                                                                SuperResultTranfer.CT_LEADS = workSheet.Cells[j, 9].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 10].Value != null)
                                                            {
                                                                SuperResultTranfer.CT_LEADS_PR = workSheet.Cells[j, 10].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 11].Value != null)
                                                            {
                                                                SuperResultTranfer.GRP = workSheet.Cells[j, 11].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 12].Value != null)
                                                            {
                                                                SuperResultTranfer.PRT = workSheet.Cells[j, 12].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 13].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    SuperResultTranfer.DT = DateTime.Parse(workSheet.Cells[j, 13].Value.ToString().Trim());
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    BaseResult.Error = ex.Message;
                                                                    BaseResult.ErrorNumber = 1;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 14].Value != null)
                                                            {
                                                                SuperResultTranfer.Machine = workSheet.Cells[j, 14].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 15].Value != null)
                                                            {
                                                                try
                                                                {
                                                                    SuperResultTranfer.BUNDLE_SIZE = int.Parse(workSheet.Cells[j, 15].Value.ToString().Trim());
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    BaseResult.Error = ex.Message;
                                                                    BaseResult.ErrorNumber = 1;
                                                                }
                                                            }
                                                            if (workSheet.Cells[j, 16].Value != null)
                                                            {
                                                                SuperResultTranfer.HOOK_RACK = workSheet.Cells[j, 16].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 17].Value != null)
                                                            {
                                                                SuperResultTranfer.WIRE = workSheet.Cells[j, 17].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 18].Value != null)
                                                            {
                                                                SuperResultTranfer.T1_DIR = workSheet.Cells[j, 18].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 19].Value != null)
                                                            {
                                                                SuperResultTranfer.TERM1 = workSheet.Cells[j, 19].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 20].Value != null)
                                                            {
                                                                SuperResultTranfer.STRIP1 = workSheet.Cells[j, 20].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 21].Value != null)
                                                            {
                                                                SuperResultTranfer.SEAL1 = workSheet.Cells[j, 21].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 22].Value != null)
                                                            {
                                                                SuperResultTranfer.CCH_W1 = workSheet.Cells[j, 22].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 23].Value != null)
                                                            {
                                                                SuperResultTranfer.ICH_W1 = workSheet.Cells[j, 23].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 24].Value != null)
                                                            {
                                                                SuperResultTranfer.T2_DIR = workSheet.Cells[j, 24].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 25].Value != null)
                                                            {
                                                                SuperResultTranfer.TERM2 = workSheet.Cells[j, 25].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 26].Value != null)
                                                            {
                                                                SuperResultTranfer.STRIP2 = workSheet.Cells[j, 26].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 27].Value != null)
                                                            {
                                                                SuperResultTranfer.SEAL2 = workSheet.Cells[j, 27].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 28].Value != null)
                                                            {
                                                                SuperResultTranfer.CCH_W2 = workSheet.Cells[j, 28].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 29].Value != null)
                                                            {
                                                                SuperResultTranfer.ICH_W2 = workSheet.Cells[j, 29].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 30].Value != null)
                                                            {
                                                                SuperResultTranfer.SP_ST = workSheet.Cells[j, 30].Value.ToString().Trim();
                                                            }
                                                            if (workSheet.Cells[j, 31].Value != null)
                                                            {
                                                                SuperResultTranfer.REP = workSheet.Cells[j, 31].Value.ToString().Trim();
                                                            }
                                                            try
                                                            {
                                                                var cc = SuperResultTranfer.TOT_QTY;
                                                                var FG = SuperResultTranfer.TORDER_FG;
                                                                SuperResultTranfer.CHK = true;
                                                                bool IsAdd = true;
                                                                if (FG.Length >= 5)
                                                                {
                                                                    if (cc <= 0)
                                                                    {
                                                                        IsAdd = false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    IsAdd = false;
                                                                }
                                                                if (IsAdd == true)
                                                                {
                                                                    BaseResult.DataGridView1.Add(SuperResultTranfer);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                BaseResult.Error = ex.Message;
                                                                BaseResult.ErrorNumber = 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    BaseResult.Error = ex.Message;
                                    BaseResult.ErrorNumber = 1;
                                }
                                fileLocation.Delete();
                            }
                            else
                            {
                                BaseResult.ErrorNumber = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonexport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonhelp_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COM_LIST()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.COM_LIST(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> COM_LIST2()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _C01Service.COM_LIST2(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

