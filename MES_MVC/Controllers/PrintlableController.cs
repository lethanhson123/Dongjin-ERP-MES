using Microsoft.AspNetCore.Mvc;

namespace MES_MVC.Controllers
{
    public class PrintlableController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Lables/index.cshtml");
        }


        /// <summary>
        /// print new label
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPost]
        public ActionResult Print([FromBody] PackingLable label)
        {
            TempData["PackingLabel"] = JsonConvert.SerializeObject(label);
            if (label.CustumerCode == "HMC")
            {
                return Content(Url.Action("ShowPackingLabelHMC", "Printlable"));
            }
            else if (label.CustumerCode == "PKI")
            {
                return Content(Url.Action("ShowPackingLabelPKI", "Printlable"));
            }
            else
                return null;
        }

        public ActionResult ShowPackingLabelHMC()
        {
            var labelJson = TempData["PackingLabel"] as string;
            var label = labelJson != null ? JsonConvert.DeserializeObject<PackingLable>(labelJson) : new PackingLable();
            return View("HMC_Lable", label);
        }

        public ActionResult ShowPackingLabelPKI()
        {
            var labelJson = TempData["PackingLabel"] as string;
            var label = labelJson != null ? JsonConvert.DeserializeObject<PackingLable>(labelJson) : new PackingLable();
            return View("PKI_Lable", label);
        }
    }
}
