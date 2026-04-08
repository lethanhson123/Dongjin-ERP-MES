namespace Helper
{
    public class QRCodeHelper
    {
        public static QRCodeModel CreateQRCode(string code, string path)
        {
            bool isFolderExists = System.IO.Directory.Exists(path);
            if (!isFolderExists)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            QRCodeModel model = new QRCodeModel();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.L))
            using (QRCoder.QRCode qrCode = new QRCode(qrCodeData))
            {
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                string fileName = code + ".png";
                path = Path.Combine(path, fileName);
                qrCodeImage.Save(path, ImageFormat.Png);
                model.Code = code;
                model.FileName = path;
            }
            return model;
        }
        public static QRCodeModel CreateQRCodeURL(string code, string url, string path)
        {
            bool isFolderExists = System.IO.Directory.Exists(path);
            if (!isFolderExists)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            QRCodeModel model = new QRCodeModel();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L))
            using (QRCoder.QRCode qrCode = new QRCode(qrCodeData))
            {
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                string fileName = code + ".png";
                path = Path.Combine(path, fileName);
                qrCodeImage.Save(path, ImageFormat.Png);
                model.Code = code;
                model.FileName = fileName;
            }
            return model;
        }
        public static Bitmap CreateQRCodeViaBitmap(string code)
        {
            Bitmap result = new Bitmap(1, 1);                      
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.L))
            using (QRCoder.QRCode qrCode = new QRCode(qrCodeData))
            {                
                result = qrCode.GetGraphic(20);
            }
            return result;
        }
        public static string CreateQRCodeViaString(string code)
        {
            string result=GlobalHelper.InitializationString;
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.L))
            using (QRCoder.QRCode qrCode = new QRCode(qrCodeData))
            {
                Bitmap qrCodeImage = qrCode.GetGraphic(10);
                using (var ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    var SigBase64 = Convert.ToBase64String(ms.GetBuffer());
                    result = "data:image/png;base64," + SigBase64;
                }
            }
            return result;
        }       
    }
}
