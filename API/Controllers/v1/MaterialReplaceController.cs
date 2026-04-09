namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MaterialReplaceController : BaseController<MaterialReplace, IMaterialReplaceService>
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IMaterialReplaceService _MaterialReplaceService;      

        public MaterialReplaceController(IMaterialReplaceService MaterialReplaceService
            , IWebHostEnvironment WebHostEnvironment        

            ) : base(MaterialReplaceService, WebHostEnvironment)
        {
            _MaterialReplaceService = MaterialReplaceService;
            _WebHostEnvironment = WebHostEnvironment;
        
        }      
    }
}

