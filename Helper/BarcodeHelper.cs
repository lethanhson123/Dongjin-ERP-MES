namespace Helper
{
    public class BarcodeHelper
    {
        public static string CreateBarcodeViaString(string Code)
        {
            string result = GlobalHelper.InitializationString;           
            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_128;
            barcodeWriter.Options.Width = 194 * 4;
            barcodeWriter.Options.Height = 50 * 4;
            var barcodeBitmap = barcodeWriter.Write(Code);
            using (var ms = new MemoryStream())
            {
                barcodeBitmap.Save(ms, ImageFormat.Png);
                var SigBase64 = Convert.ToBase64String(ms.GetBuffer());
                result = "data:image/png;base64," + SigBase64;
            }
            return result;
        }
    }
}
