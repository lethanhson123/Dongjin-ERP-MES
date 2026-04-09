using OfficeOpenXml;
using OfficeOpenXml.Style;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.ComponentModel;

namespace MES.Controllers
{
    public class F02Controller : Controller
    {
        private readonly IF02Service _F02Service;
        public F02Controller(IF02Service F02Service)
        {
            _F02Service = F02Service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<BaseResult> Load()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseResult = await _F02Service.Load();
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _F02Service.Buttonfind_Click(BaseParameter);
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
                BaseResult = await _F02Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _F02Service.Buttonsave_Click(BaseParameter);
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
            BaseResult result = new BaseResult();

            try
            {
                if (!Request.Form.ContainsKey("BaseParameter"))
                {
                    result.Success = false;
                    result.Message = "Thiếu BaseParameter!";
                    return result;
                }

                var json = Request.Form["BaseParameter"].ToString();

                var baseParameter = JsonConvert.DeserializeObject<BaseParameter>(json);

                if (baseParameter?.Ids == null || baseParameter.Ids.Count == 0)
                {
                    result.Success = false;
                    result.Message = "Danh sách ID rỗng hoặc không hợp lệ!";
                    return result;
                }

                result = await _F02Service.Buttondelete_Click(baseParameter);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }

            return result;
        }


        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _F02Service.Buttoncancel_Click(BaseParameter);
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
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _F02Service.Buttoninport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<IActionResult> Buttonexport_Click()
        {
           
            
            try
            {
                BaseResult result = new BaseResult();
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await _F02Service.Buttonexport_Click(BaseParameter);           
                var data = result.DataGridView;
                if (data == null || !data.Any())
                    return BadRequest("Không có dữ liệu để xuất Excel");

                //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("OQC_Data");

                // 🔹 Header
                var headers = new[]
                {
            "ID", "PART_NO", "PART_SUPL", "PART_NM", "LineName", "LineType",
            "Barcode", "ErrorCode", "LOC", "Description", "ECN", "CREATE_DTM",
            "CREATE_USER", "REMARK"
        };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                    ws.Cells[1, i + 1].Style.Font.Bold = true;
                    ws.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                }

                // 🔹 Data
                int row = 2;
                foreach (var item in data)
                {
                    ws.Cells[row, 1].Value = item.ID;
                    ws.Cells[row, 2].Value = item.PART_NO;
                    ws.Cells[row, 3].Value = item.PART_SUPL;
                    ws.Cells[row, 4].Value = item.PART_NM;
                    ws.Cells[row, 5].Value = item.LineName;
                    ws.Cells[row, 6].Value = item.LineType;
                    ws.Cells[row, 7].Value = item.Barcode;
                    ws.Cells[row, 8].Value = item.ErrorCode;
                    ws.Cells[row, 9].Value = item.LOC;
                    ws.Cells[row, 10].Value = item.Description;
                    ws.Cells[row, 11].Value = item.ECN;
                    ws.Cells[row, 12].Value = item.CREATE_DTM?.ToString("yyyy-MM-dd HH:mm:ss");
                    ws.Cells[row, 13].Value = item.CREATE_USER;
                    ws.Cells[row, 14].Value = item.REMARK;
                    row++;
                }

                ws.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"OQC_Data_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(stream,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _F02Service.Buttonprint_Click(BaseParameter);
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
                BaseResult = await _F02Service.Buttonhelp_Click(BaseParameter);
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
                BaseResult = await _F02Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "File Upload not found" });

            // ✅ Kiểm tra định dạng hợp lệ
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest(new { success = false, message = "Invalid file format (format suport: .jpg .jpeg .png .gif)" });

            // ✅ Cấu trúc thư mục lưu theo năm / tháng / ngày
            var rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Picture", "OQCNG");
            var now = DateTime.Now;
            var dayFolder = Path.Combine(rootFolder, now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
            Directory.CreateDirectory(dayFolder);

            // ✅ Tên file duy nhất
            var fileName = $"OQCNG_{now:yyyyMMdd_HHmmssfff}.jpg";
            var filePath = Path.Combine(dayFolder, fileName);

            // ✅ Resize ảnh về 1280x720 mà KHÔNG CẮT
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1920, 1080),
                    Mode = ResizeMode.Stretch // 👉 Giữ nguyên toàn bộ hình, co giãn để khớp 16:9
                }));

                // Lưu với chất lượng 90% để tối ưu dung lượng
                var encoder = new JpegEncoder { Quality = 100 };
                await image.SaveAsJpegAsync(filePath, encoder);
            }

            // ✅ Đường dẫn trả về cho client
            var relativePath = $"/Picture/OQCNG/{now:yyyy}/{now:MM}/{now:dd}/{fileName}";

            return Ok(new { success = true, path = relativePath });
        }

    }
}

