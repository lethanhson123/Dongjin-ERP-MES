using SixLabors.ImageSharp.Formats.Jpeg;

namespace MES.Controllers
{
    public class F03Controller : Controller
    {
        private readonly IF03Service _F03Service;

        public F03Controller(IF03Service F03Service)
        {
            _F03Service = F03Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonfind_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonadd_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonsave_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttondelete_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttoncancel_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttoninport_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonexport_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonexport_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonprint_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonhelp_Click(param));
        }

        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            return await ExecuteAction(param => _F03Service.Buttonclose_Click(param));
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "File not found" });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest(new { success = false, message = "Invalid format" });

            var rootFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Picture", "IQCNG_Customer2");
            var now = DateTime.Now;
            var dayFolder = Path.Combine(rootFolder, now.ToString("yyyy"), now.ToString("MM"), now.ToString("dd"));
            Directory.CreateDirectory(dayFolder);

            var fileName = $"IQCNG_C2_{now:yyyyMMdd_HHmmssfff}.jpg";
            var filePath = Path.Combine(dayFolder, fileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1920, 1080),
                    Mode = ResizeMode.Stretch
                }));

                await image.SaveAsJpegAsync(filePath, new JpegEncoder { Quality = 100 });
            }

            var relativePath = $"/Picture/IQCNG_Customer2/{now:yyyy}/{now:MM}/{now:dd}/{fileName}";

            return Ok(new { success = true, path = relativePath });
        }

        private async Task<BaseResult> ExecuteAction(Func<BaseParameter, Task<BaseResult>> action)
        {
            BaseResult result = new BaseResult();
            try
            {
                var param = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                result = await action(param);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}