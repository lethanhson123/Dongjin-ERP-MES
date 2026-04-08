namespace Helper
{
    public class GlobalHelper
    {
        #region Initialization
        public static string Saturday
        {
            get
            {
                return "Saturday";
            }
        }
        public static string Sunday
        {
            get
            {
                return "Sunday";
            }
        }

        public static bool InitializationBool
        {
            get
            {
                return true;
            }
        }
        public static string InitializationString
        {
            get
            {
                return string.Empty;
            }
        }
        public static DateTime InitializationDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }
        public static DateTime DateTimeBegin
        {
            get
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            }
        }
        public static DateTime DateTimeEnd
        {
            get
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }
        }
        public static string InitializationGUICode
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        public static string InitializationDateTimeCode
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Ticks.ToString();
            }
        }
        /// <summary>
        /// lấy vesion mới nhất của Scrip khi có cập nhật mới
        /// </summary>
        /// <param name="ScripName">tên Scrip cần load</param>
        /// <returns></returns>
        public static string GetVersionScrip(string ScripName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Scripts", ScripName);
            if (File.Exists(path))
            {
                return File.GetLastWriteTimeUtc(path).Ticks.ToString();
            }
            return DateTime.UtcNow.Ticks.ToString(); // fallback nếu không tìm thấy file            
        }

        public static string InitializationDateTimeCode0001
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }
        public static int InitializationNumber
        {
            get
            {
                return 0;
            }
        }
        public static int InitializationSortOrder
        {
            get
            {
                return -1;
            }
        }
        #endregion
        #region JWT   	
        public static string? Key
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("Jwt").GetSection("Key").Value;
            }
        }
        public static string? Issuer
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("Jwt").GetSection("Issuer").Value;
            }
        }
        public static string? Audience
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("Jwt").GetSection("Audience").Value;
            }
        }
        public static string? Subject
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("Jwt").GetSection("Subject").Value;
            }
        }
        #endregion
        #region AppSettings    
        public static string ZaloRefreshTokenAPIURL
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ZaloRefreshTokenAPIURL").Value;
            }
        }
        public static string ZaloTokenNote
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ZaloTokenNote").Value;
            }
        }
        public static string ZaloTemplateID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ZaloTemplateID").Value;
            }
        }
        public static string ZaloZNSAPIURL
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ZaloZNSAPIURL").Value;
            }
        }
        public static string SS1
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("SS1").Value;
            }
        }
        public static string Tube1
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Tube1").Value;
            }
        }
        public static string SS2
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("SS2").Value;
            }
        }
        public static string Tube2
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Tube2").Value;
            }
        }
        public static string Tape
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Tape").Value;
            }
        }
        public static string WIRE
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("WIRE").Value;
            }
        }
        public static string Office
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Office").Value;
            }
        }
        public static string APIUpload
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("APIUpload").Value;
            }
        }
        public static string APIUploadSite
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("APIUploadSite").Value;
            }
        }
        public static string Dayoff
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Dayoff").Value;
            }
        }
        public static string SMTPServer
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("SMTPServer").Value;
            }
        }
        public static int SMTPPort
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("SMTPPort").Value);
            }
        }
        public static int IsMailUsingSSL
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("IsMailUsingSSL").Value);
            }
        }
        public static int IsMailBodyHtml
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("IsMailBodyHtml").Value);
            }
        }
        public static string MasterEmailUser
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MasterEmailUser").Value;
            }
        }
        public static string MasterEmailPassword
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MasterEmailPassword").Value;
            }
        }
        public static string MasterEmailDisplay
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MasterEmailDisplay").Value;
            }
        }
        public static string? ERPSite
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ERPSite").Value;
            }
        }
        public static int? SortOrder
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("SortOrder").Value);
            }
        }
        public static int? NotificationCount
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("NotificationCount").Value);
            }
        }
        public static int? DaySpan
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("DaySpan").Value);
            }
        }
        public static int? YearStock
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("YearStock").Value);
            }
        }
        public static long? WarehouseInputIDStock
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("WarehouseInputIDStock").Value);
            }
        }
        public static long? WarehouseInputID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("WarehouseInputID").Value);
            }
        }
        public static long? CategoryLocationID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("CategoryLocationID").Value);
            }
        }
        public static long? CategoryMaterialID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("CategoryMaterialID").Value);
            }
        }
        public static long? DepartmentIDOffice
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("DepartmentIDOffice").Value);
            }
        }
        public static long? DepartmentIDCutting
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("DepartmentIDCutting").Value);
            }
        }
        public static long? DepartmentIDFG
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("DepartmentIDFG").Value);
            }
        }
        public static long? DepartmentID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("DepartmentID").Value);
            }
        }
        public static long? CompanyID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("CompanyID").Value);
            }
        }
        public static long? PositionID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return long.Parse(builder.Build().GetSection("AppSettings").GetSection("PositionID").Value);
            }
        }
        public static int? TokenExpired
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("TokenExpired").Value);
            }
        }
        public static int? PageSize
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("PageSize").Value);
            }
        }
        public static int? YearBegin
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("YearBegin").Value);
            }
        }
        public static int? YearEnd
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("YearEnd").Value);
            }
        }
        public static int? ListCount2000
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("ListCount2000").Value);
            }
        }
        public static int? ListCount
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return int.Parse(builder.Build().GetSection("AppSettings").GetSection("ListCount").Value);
            }
        }
        public static string? Excel
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Excel").Value;
            }
        }
        public static string? DomainSite
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("DomainSite").Value;
            }
        }
        public static string? ControllerName
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ControllerName").Value;
            }
        }
        public static string? WMP_PLAY_FLNM
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("WMP_PLAY_FLNM").Value;
            }
        }
        public static string? WMP_PLAY
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("WMP_PLAY").Value;
            }
        }
        public static string? GetFileServer
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("GetFileServer").Value;
            }
        }
        public static string? URLCodeDefault
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("URLCodeDefault").Value;
            }
        }
        public static string? USER_IDX
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("USER_IDX").Value;
            }
        }
        public static string? USER_NM
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("USER_NM").Value;
            }
        }
        public static string? UserID
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("UserID").Value;
            }
        }
        public static string? Password
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Password").Value;
            }
        }
        public static string? IsRemember
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("IsRemember").Value;
            }
        }
        public static string? MenuParent
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MenuParent").Value;
            }
        }
        public static string? Language
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Language").Value;
            }
        }
        public static string? LanguageCodeVI
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("LanguageCodeVI").Value;
            }
        }
        public static string? LanguageCodeEN
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("LanguageCodeEN").Value;
            }
        }
        public static string? LanguageCodeKR
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("LanguageCodeKR").Value;
            }
        }
        public static string? YSJ4947
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("YSJ4947").Value;
            }
        }
        public static string? QRCode
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("QRCode").Value;
            }
        }
        public static string? HTML
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("HTML").Value;
            }
        }
        public static string? Upload
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Upload").Value;
            }
        }
        public static string? Download
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("Download").Value;
            }
        }
        public static string? URLSite
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("URLSite").Value;
            }
        }
        public static string? APISite
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("APISite").Value;
            }
        }
        public static string? MariaDBConectionString
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MariaDBConectionString").Value;
            }
        }
        public static string? MariaDBConectionStringDJM
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("MariaDBConectionStringDJM").Value;
            }
        }
        public static string? ERP_MariaDBConectionString
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ERP_MariaDBConectionString").Value;
            }
        }
        public static string? ERP_SQLServerConectionString
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                return builder.Build().GetSection("AppSettings").GetSection("ERP_SQLServerConectionString").Value;
            }
        }
        #endregion
        #region Functions        
        public static string ExtractAllDigits(string inputString)
        {
            string onlyNumbers = new string(inputString.Where(char.IsDigit).ToArray());
            return onlyNumbers;
        }
        public static bool IsPasswordValidWithRegex(string password)
        {
            string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
            return Regex.IsMatch(password, passwordRegex);
        }
        public static int CovertMonthNameToMonth(string MonthName)
        {
            var result = 1;
            MonthName = MonthName.Trim().ToLower();
            switch (MonthName)
            {
                case "january":
                    result = 1;
                    break;
                case "february":
                    result = 2;
                    break;
                case "march":
                    result = 3;
                    break;
                case "april":
                    result = 4;
                    break;
                case "may":
                    result = 5;
                    break;
                case "june":
                    result = 6;
                    break;
                case "july":
                    result = 7;
                    break;
                case "august":
                    result = 8;
                    break;
                case "september":
                    result = 9;
                    break;
                case "october":
                    result = 10;
                    break;
                case "november":
                    result = 11;
                    break;
                case "december":
                    result = 12;
                    break;
            }
            return result;
        }
        public static int GetQuarterByDateTime(DateTime DateTime)
        {
            return (DateTime.Month + 2) / 3;
        }
        public static int GetWeekByDateTime(DateTime DateTime)
        {
            return ISOWeek.GetWeekOfYear(DateTime);
        }
        public static string GetDateNameByDateTime(DateTime? DateTime)
        {
            var result = InitializationString;
            if (DateTime != null)
            {
                result = DateTime.Value.ToString("dddd");
            }
            return result;
        }
        public static void DeleteFilesByPath(string Path)
        {
            DirectoryInfo Directory = new DirectoryInfo(Path);
            FileInfo[] Files = Directory.GetFiles();
            foreach (FileInfo file in Files)
            {
                DateTime now = GlobalHelper.InitializationDateTime;
                TimeSpan difference = now.Date - file.CreationTime.Date;
                int days = (int)difference.TotalDays;
                if (days > 0)
                {
                    file.Delete();
                }
            }
        }
        public static QRCodeModel CreateQRCode(string BARCODE_QR, string SheetName, string WebRootPath)
        {
            string physicalPathCreate = Path.Combine(WebRootPath, GlobalHelper.QRCode, SheetName);
            Directory.CreateDirectory(physicalPathCreate);
            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
            QRCodeModel result = QRCodeHelper.CreateQRCode(BARCODE_QR, physicalPathCreate);
            result.URL = GlobalHelper.URLSite + "/" + GlobalHelper.QRCode + "/" + SheetName + "/" + BARCODE_QR + ".png";
            return result;
        }
        public static string CreateHTMLWarehouseInputBarcode2025(string SheetName, string WebRootPath, string Display, string MaterialName, string ProductionCode, string ProductID, string QuantityInvoice, string Quantity, string ProductName, string CategoryLocationName, string Note, string Barcode, string Date, string Week, string Invoice)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "Barcode2025.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string FileName = QRCodeHelper.CreateQRCodeViaString(Barcode);

            string MaterialNameSize = "16";
            string MaterialNameSize01 = "12";
            if (MaterialName.Length > 10)
            {
                MaterialNameSize = "12";
                MaterialNameSize01 = "9";
            }
            if (MaterialName.Length > 16)
            {
                MaterialNameSize = "11";
                MaterialNameSize01 = "8";
            }
            if (ProductID == "0")
            {
                ProductID = "";
            }
            //if (Quantity == "0")
            //{
            //    Quantity = QuantityInvoice;
            //}
            string Package = Barcode;
            if (!string.IsNullOrEmpty(Package))
            {
                Package = Package.Split('$')[Package.Split('$').Length - 1];
            }
            if (!string.IsNullOrEmpty(Barcode) && Barcode.Length > 30)
            {
                Barcode = Barcode.Substring(0, 30);
            }
            contentHTML = contentHTML.Replace("[FileName]", FileName);
            contentHTML = contentHTML.Replace("[Display]", Display);
            contentHTML = contentHTML.Replace("[MaterialName]", MaterialName);
            contentHTML = contentHTML.Replace("[ProductionCode]", ProductionCode);
            contentHTML = contentHTML.Replace("[ProductID]", ProductID);
            contentHTML = contentHTML.Replace("[QuantityInvoice]", QuantityInvoice);
            contentHTML = contentHTML.Replace("[Quantity]", Quantity);
            contentHTML = contentHTML.Replace("[ProductName]", ProductName);
            contentHTML = contentHTML.Replace("[CategoryLocationName]", CategoryLocationName);
            contentHTML = contentHTML.Replace("[Note]", Note);
            contentHTML = contentHTML.Replace("[Barcode]", Barcode);
            contentHTML = contentHTML.Replace("[Package]", Package);
            contentHTML = contentHTML.Replace("[Invoice]", Invoice);
            contentHTML = contentHTML.Replace("[Date]", Date);
            contentHTML = contentHTML.Replace("[Week]", Week);
            contentHTML = contentHTML.Replace("[MaterialNameSize]", MaterialNameSize);
            contentHTML = contentHTML.Replace("[MaterialNameSize01]", MaterialNameSize01);
            return contentHTML;
        }
        public static string CreateHTMLWarehouseInputBarcode(string SheetName, string WebRootPath, string Display, string MaterialName, string ProductionCode, string ProductID, string QuantityInvoice, string Quantity, string ProductName, string CategoryLocationName, string Note, string Barcode, string Date, string Week, string Invoice)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "Barcode.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string FileName = QRCodeHelper.CreateQRCodeViaString(Barcode);

            string MaterialNameSize = "16";
            string MaterialNameSize01 = "12";
            if (MaterialName.Length > 10)
            {
                MaterialNameSize = "12";
                MaterialNameSize01 = "9";
            }
            if (MaterialName.Length > 16)
            {
                MaterialNameSize = "11";
                MaterialNameSize01 = "8";
            }
            if (ProductID == "0")
            {
                ProductID = "";
            }
            //if (Quantity == "0")
            //{
            //    Quantity = QuantityInvoice;
            //}
            string Package = Barcode;
            if (!string.IsNullOrEmpty(Package))
            {
                Package = Package.Split('$')[Package.Split('$').Length - 1];
            }
            string BarcodeLarge = Barcode.Replace(Package, "00");
            BarcodeLarge = QRCodeHelper.CreateQRCodeViaString(BarcodeLarge);
            if (!string.IsNullOrEmpty(Barcode) && Barcode.Length > 30)
            {
                Barcode = Barcode.Substring(0, 30);
            }
            contentHTML = contentHTML.Replace("[FileName]", FileName);
            contentHTML = contentHTML.Replace("[Display]", Display);
            contentHTML = contentHTML.Replace("[MaterialName]", MaterialName);
            contentHTML = contentHTML.Replace("[ProductionCode]", ProductionCode);
            contentHTML = contentHTML.Replace("[ProductID]", ProductID);
            contentHTML = contentHTML.Replace("[QuantityInvoice]", QuantityInvoice);
            contentHTML = contentHTML.Replace("[Quantity]", Quantity);
            contentHTML = contentHTML.Replace("[ProductName]", ProductName);
            contentHTML = contentHTML.Replace("[CategoryLocationName]", CategoryLocationName);
            contentHTML = contentHTML.Replace("[Note]", Note);
            contentHTML = contentHTML.Replace("[Barcode]", Barcode);
            contentHTML = contentHTML.Replace("[BarcodeLarge]", BarcodeLarge);
            contentHTML = contentHTML.Replace("[Package]", Package);
            contentHTML = contentHTML.Replace("[Invoice]", Invoice);
            contentHTML = contentHTML.Replace("[Date]", Date);
            contentHTML = contentHTML.Replace("[Week]", Week);
            contentHTML = contentHTML.Replace("[MaterialNameSize]", MaterialNameSize);
            contentHTML = contentHTML.Replace("[MaterialNameSize01]", MaterialNameSize01);
            return contentHTML;
        }
        public static string CreateHTMLInventory(string WebRootPath, string MaterialName, string CategoryLocationName, string Quantity, string CreateUserCode, string CreateUserName, string SeriNumber, DateTime CreateDate)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "Inventory.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string CreateDateString = CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            SeriNumber = SeriNumber.Replace(",", "");
            contentHTML = contentHTML.Replace("[MaterialName]", MaterialName);
            contentHTML = contentHTML.Replace("[Quantity]", Quantity);
            contentHTML = contentHTML.Replace("[CategoryLocationName]", CategoryLocationName);
            contentHTML = contentHTML.Replace("[CreateUserCode]", CreateUserCode);
            contentHTML = contentHTML.Replace("[CreateUserName]", CreateUserName);
            contentHTML = contentHTML.Replace("[CreateDate]", CreateDateString);
            contentHTML = contentHTML.Replace("[SeriNumber]", SeriNumber);
            return contentHTML;
        }
        public static string CreateHTMLInventory2025(string WebRootPath, string MaterialName, string CategoryLocationName, string Quantity, string CreateUserCode, string CreateUserName, string SeriNumber, DateTime CreateDate)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "Inventory202501.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string CreateDateString = CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            string CreateDateSub = CreateDate.ToString("yyyy-MM-dd");
            string CreateDateHour = CreateDate.ToString("HH:mm:ss");
            SeriNumber = SeriNumber.Replace(",", "");
            contentHTML = contentHTML.Replace("[MaterialName]", MaterialName);
            contentHTML = contentHTML.Replace("[Quantity]", Quantity);
            contentHTML = contentHTML.Replace("[CategoryLocationName]", CategoryLocationName);
            contentHTML = contentHTML.Replace("[CreateUserCode]", CreateUserCode);
            contentHTML = contentHTML.Replace("[CreateUserName]", CreateUserName);
            contentHTML = contentHTML.Replace("[CreateDate]", CreateDateString);
            contentHTML = contentHTML.Replace("[CreateDateSub]", CreateDateSub);
            contentHTML = contentHTML.Replace("[CreateDateHour]", CreateDateHour);
            contentHTML = contentHTML.Replace("[SeriNumber]", SeriNumber);
            return contentHTML;
        }
        public static string CreateHTMLB03(string SheetName, string WebRootPath, string BARCODE_QR, string BARCODE_AA, string BARCODE_BB, string BARCODE_CC, string BARCODE_DD, string BARCODE_EE, string BARCODE_FF, string BARCODE_GG, string BARCODE_HH, string BARCODE_ZZ)
        {
            string result = GlobalHelper.InitializationString;
            string fileName = BARCODE_QR + ".html";
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "B03Sub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);

            DateTime BARCODE_ZZDate = DateTime.Parse(BARCODE_ZZ);
            int BARCODE_ZZ2 = System.Globalization.ISOWeek.GetWeekOfYear(BARCODE_ZZDate);

            string BARCODE_CC1 = BARCODE_CC;
            if (BARCODE_AA.Contains("Small"))
            {
                BARCODE_CC1 = BARCODE_BB;
            }
            //BARCODE_DD = "HVMC2P12FV-22_SY";
            string BARCODE_DDSize = "16";
            string BARCODE_DD1Size = "12";
            string BARCODE_DD1 = BARCODE_DD;
            if (BARCODE_DD.Length > 10)
            {
                BARCODE_DDSize = "14";
                BARCODE_DD1Size = "10";
            }
            if (BARCODE_DD.Length > 16)
            {
                BARCODE_DDSize = "12";
                BARCODE_DD1Size = "9";
            }
            if (BARCODE_FF.Length > 24)
            {
                BARCODE_FF = BARCODE_FF.Substring(0, 24);
            }
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[BARCODE_QR]", BARCODE_QR);
            contentHTML = contentHTML.Replace("[BARCODE_AA]", BARCODE_AA);
            contentHTML = contentHTML.Replace("[BARCODE_BB]", BARCODE_BB);
            contentHTML = contentHTML.Replace("[BARCODE_CC]", BARCODE_CC);
            contentHTML = contentHTML.Replace("[BARCODE_DD]", BARCODE_DD);
            contentHTML = contentHTML.Replace("[BARCODE_DD]", BARCODE_DD);
            contentHTML = contentHTML.Replace("[BARCODE_DD1]", BARCODE_DD1);
            contentHTML = contentHTML.Replace("[BARCODE_DDSize]", BARCODE_DDSize);
            contentHTML = contentHTML.Replace("[BARCODE_DD1Size]", BARCODE_DD1Size);
            contentHTML = contentHTML.Replace("[BARCODE_EE]", BARCODE_EE);
            contentHTML = contentHTML.Replace("[BARCODE_FF]", BARCODE_FF);
            contentHTML = contentHTML.Replace("[BARCODE_GG]", BARCODE_GG);
            contentHTML = contentHTML.Replace("[BARCODE_HH]", BARCODE_HH);
            contentHTML = contentHTML.Replace("[BARCODE_ZZ1]", BARCODE_ZZDate.ToString("yyyy-MM-dd"));
            contentHTML = contentHTML.Replace("[BARCODE_ZZ2]", BARCODE_ZZ2.ToString());
            contentHTML = contentHTML.Replace("[BARCODE_CC1]", BARCODE_CC1);
            return contentHTML;
        }
        public static string CreateHTMLB04(string SheetName, string WebRootPath, string BARCODE_QR, string BARCODE_AA, string BARCODE_BB, string BARCODE_CC, string BARCODE_DD, string BARCODE_EE, string BARCODE_FF, string BARCODE_GG, string BARCODE_HH, string BARCODE_ZZ, string BARCODE_CC1)
        {
            string result = GlobalHelper.InitializationString;
            string fileName = BARCODE_QR + ".html";
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "B03Sub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);
            if (BARCODE_QR.Length > 10)
            {
                BARCODE_QR = BARCODE_QR.Substring(0, 10);
            }
            if (BARCODE_FF.Length > 10)
            {
                BARCODE_FF = BARCODE_FF.Substring(0, 10);
            }
            DateTime BARCODE_ZZDate = DateTime.Parse(BARCODE_ZZ);
            int BARCODE_ZZ2 = System.Globalization.ISOWeek.GetWeekOfYear(BARCODE_ZZDate);
            string BARCODE_DDSize = "16";
            string BARCODE_DD1Size = "12";
            string BARCODE_DD1 = BARCODE_DD;
            if (BARCODE_DD.Length > 10)
            {
                BARCODE_DDSize = "14";
                BARCODE_DD1Size = "10";
            }
            if (BARCODE_DD.Length > 16)
            {
                BARCODE_DDSize = "12";
                BARCODE_DD1Size = "9";
            }
            if (BARCODE_DD.Length > 18)
            {
                BARCODE_DDSize = "10";
            }
            if (BARCODE_FF.Length > 24)
            {
                BARCODE_FF = BARCODE_FF.Substring(0, 24);
            }
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[BARCODE_QR]", BARCODE_QR);
            contentHTML = contentHTML.Replace("[BARCODE_AA]", BARCODE_AA);
            contentHTML = contentHTML.Replace("[BARCODE_BB]", BARCODE_BB);
            contentHTML = contentHTML.Replace("[BARCODE_CC]", BARCODE_CC);
            contentHTML = contentHTML.Replace("[BARCODE_DD]", BARCODE_DD);
            contentHTML = contentHTML.Replace("[BARCODE_DD1]", BARCODE_DD1);
            contentHTML = contentHTML.Replace("[BARCODE_DDSize]", BARCODE_DDSize);
            contentHTML = contentHTML.Replace("[BARCODE_DD1Size]", BARCODE_DD1Size);
            contentHTML = contentHTML.Replace("[BARCODE_EE]", BARCODE_EE);
            contentHTML = contentHTML.Replace("[BARCODE_FF]", BARCODE_FF);
            contentHTML = contentHTML.Replace("[BARCODE_GG]", BARCODE_GG);
            contentHTML = contentHTML.Replace("[BARCODE_HH]", BARCODE_HH);
            contentHTML = contentHTML.Replace("[BARCODE_ZZ1]", BARCODE_ZZDate.ToString("yyyy-MM-dd"));
            contentHTML = contentHTML.Replace("[BARCODE_ZZ2]", BARCODE_ZZ2.ToString());
            contentHTML = contentHTML.Replace("[BARCODE_CC1]", BARCODE_CC1);
            return contentHTML;
        }
        public static string CreateHTMLB08(string WebRootPath, string TTC_BARCODENM, string TC_PART_NM, string TC_DESC, double QTY, int Barcode_SEQ, string TC_LOC, DateTime TTC_PO_DT)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "B08_REPRINTSub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = QRCodeHelper.CreateQRCodeViaString(TTC_BARCODENM);

            int TTC_PO_DT2 = System.Globalization.ISOWeek.GetWeekOfYear(TTC_PO_DT);

            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[TTC_BARCODENM]", TTC_BARCODENM);
            contentHTML = contentHTML.Replace("[TC_PART_NM]", TC_PART_NM);
            contentHTML = contentHTML.Replace("[TC_DESC]", TC_DESC);
            contentHTML = contentHTML.Replace("[QTY]", QTY.ToString());
            contentHTML = contentHTML.Replace("[Barcode_SEQ]", Barcode_SEQ.ToString());
            contentHTML = contentHTML.Replace("[TC_LOC]", TC_LOC);
            contentHTML = contentHTML.Replace("[TTC_PO_DT]", TTC_PO_DT.ToString("yyyy-MM-dd"));
            contentHTML = contentHTML.Replace(@"[TTC_PO_DT2]", TTC_PO_DT2.ToString());

            return contentHTML;
        }
        public static string CreateHTMLD11(string WebRootPath, string QRCode, string Barcode)
        {
            string result = GlobalHelper.InitializationString;
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "D11Sub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = QRCodeHelper.CreateQRCodeViaString(QRCode);
            string BarcodeFileName = BarcodeHelper.CreateBarcodeViaString(Barcode);
            contentHTML = contentHTML.Replace("[QRCode]", QRCode);
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[Barcode]", Barcode);
            contentHTML = contentHTML.Replace("[BarcodeFileName]", BarcodeFileName);

            return contentHTML;
        }

        public static string CreateHTMLC08(string SheetName, string WebRootPath, string BARCODE_QR, string BARCODE_AA, string BARCODE_BB, string BARCODE_CC, string BARCODE_DD, string BARCODE_EE, string BARCODE_FF, string BARCODE_TIME_S)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "C08.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[PR1]", BARCODE_AA ?? ""); // Model (từ TextBox5)
            contentHTML = contentHTML.Replace("[PR2]", BARCODE_CC ?? ""); // User
            contentHTML = contentHTML.Replace("[PR3]", BARCODE_BB ?? ""); // Date
            contentHTML = contentHTML.Replace("[PR4]", BARCODE_DD ?? ""); // Hook Rack
            contentHTML = contentHTML.Replace("[PR5]", BARCODE_EE ?? ""); // Lead No
            contentHTML = contentHTML.Replace("[PR6]", "");
            contentHTML = contentHTML.Replace("[PR7]", BARCODE_FF ?? ""); // Quantity
            contentHTML = contentHTML.Replace("[PR8]", "");
            contentHTML = contentHTML.Replace("[PR9]", "");
            contentHTML = contentHTML.Replace("[PR10]", "");
            contentHTML = contentHTML.Replace("[PR11]", "");
            contentHTML = contentHTML.Replace("[PR12]", "");
            contentHTML = contentHTML.Replace("[PR13]", "");
            contentHTML = contentHTML.Replace("[PR14]", "");
            contentHTML = contentHTML.Replace("[PR15]", "");
            contentHTML = contentHTML.Replace("[PR16]", "");
            // Sử dụng giá trị timeS
            contentHTML = contentHTML.Replace("[PR20]", BARCODE_TIME_S);
            contentHTML = contentHTML.Replace("[PR21]", "");
            contentHTML = contentHTML.Replace("[PR22]", "");
            return contentHTML;
        }
        public static string CreateHTMLC08BarcodeOnly(string SheetName, string WebRootPath, string BARCODE_QR, string BARCODE_AA, string BARCODE_BB)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "C08_BarcodeOnly.html");

            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }


            string QRCodeFileName = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);


            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[BARCODE_QR]", BARCODE_QR);
            contentHTML = contentHTML.Replace("[BARCODE_AA]", BARCODE_AA);
            contentHTML = contentHTML.Replace("[BARCODE_BB]", BARCODE_BB);




            return contentHTML;
        }
        public static string CreateHTMLD04_PLT_PRNT(string SheetName, string WebRootPath, string POCODE, string PALLLETNO)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "D04_PLT_PRNTSub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodePOCODE = Helper.QRCodeHelper.CreateQRCodeViaString(POCODE);
            string QRCodePALLLETNO = Helper.QRCodeHelper.CreateQRCodeViaString(PALLLETNO);
            contentHTML = contentHTML.Replace("[POCODE]", POCODE);
            contentHTML = contentHTML.Replace("[PALLLETNO]", PALLLETNO);
            contentHTML = contentHTML.Replace("[QRCodePOCODE]", QRCodePOCODE);
            contentHTML = contentHTML.Replace("[QRCodePALLLETNO]", QRCodePALLLETNO);
            return contentHTML;
        }
        public static string CreateHTMLD04Report(string SheetName, string WebRootPath, string PARTGRP, string PALLLETNO, string List, string PrintDate, string PrintTime)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "D04Report.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }

            int PARTGRPSize = 150;

            if (PARTGRP.Length > 6)
            {
                PARTGRPSize = 100;
            }
            int PARTGRPHeight = PARTGRPSize + 10;

            string QRCodePALLLETNO = Helper.QRCodeHelper.CreateQRCodeViaString(PALLLETNO);
            contentHTML = contentHTML.Replace("[PALLLETNO]", PALLLETNO);
            contentHTML = contentHTML.Replace("[QRCodePALLLETNO]", QRCodePALLLETNO);
            contentHTML = contentHTML.Replace("[PARTGRP]", PARTGRP);
            contentHTML = contentHTML.Replace("[List]", List);
            contentHTML = contentHTML.Replace("[PrintDate]", PrintDate);
            contentHTML = contentHTML.Replace("[PrintTime]", PrintTime);
            contentHTML = contentHTML.Replace("[PARTGRPSize]", PARTGRPSize.ToString());
            contentHTML = contentHTML.Replace("[PARTGRPHeight]", PARTGRPHeight.ToString());
            return contentHTML;
        }
        public static string CreateHTMLD04Report_AIR(string SheetName, string WebRootPath, string PARTGRP, string PARTNAME, string PALLLETNO, string List, string PrintDate, string PrintTime)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "D04Report_AIR.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodePALLLETNO = Helper.QRCodeHelper.CreateQRCodeViaString(PALLLETNO);
            contentHTML = contentHTML.Replace("[PALLLETNO]", PALLLETNO);
            contentHTML = contentHTML.Replace("[QRCodePALLLETNO]", QRCodePALLLETNO);
            contentHTML = contentHTML.Replace("[PARTGRP]", PARTGRP);
            contentHTML = contentHTML.Replace("[PARTNAME]", PARTNAME);
            contentHTML = contentHTML.Replace("[List]", List);
            contentHTML = contentHTML.Replace("[PrintDate]", PrintDate);
            contentHTML = contentHTML.Replace("[PrintTime]", PrintTime);
            return contentHTML;
        }
        public static string CreateHTMLD04Report_H(string SheetName, string WebRootPath, string PART_NM, string SHIPNO, string PALLLETNO, string List, string PrintDate, string PrintTime)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "D04Report_H.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodePALLLETNO = Helper.QRCodeHelper.CreateQRCodeViaString(PALLLETNO);
            contentHTML = contentHTML.Replace("[PALLLETNO]", PALLLETNO);
            contentHTML = contentHTML.Replace("[QRCodePALLLETNO]", QRCodePALLLETNO);
            contentHTML = contentHTML.Replace("[PART_NM]", PART_NM);
            contentHTML = contentHTML.Replace("[SHIPNO]", SHIPNO);
            contentHTML = contentHTML.Replace("[List]", List);
            contentHTML = contentHTML.Replace("[PrintDate]", PrintDate);
            contentHTML = contentHTML.Replace("[PrintTime]", PrintTime);
            return contentHTML;
        }
        public static string CreateHTMLC15(string SheetName, string WebRootPath, string BARCODE_QR, string PR1, string PR2, string PR3, string PR4, string PR5, string PR6, string PR7, string PR8, string PR9, string PR10, string PR11, string PR12, string PR13, string PR14, string PR15, string PR16, string PR20, string PR21, string PR22)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "C15Sub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[PR1]", PR1);
            contentHTML = contentHTML.Replace("[PR2]", PR2);
            contentHTML = contentHTML.Replace("[PR3]", PR3);
            contentHTML = contentHTML.Replace("[PR4]", PR4);
            contentHTML = contentHTML.Replace("[PR5]", PR5);
            contentHTML = contentHTML.Replace("[PR6]", PR6);
            contentHTML = contentHTML.Replace("[PR7]", PR7);
            contentHTML = contentHTML.Replace("[PR8]", PR8);
            contentHTML = contentHTML.Replace("[PR9]", PR9);
            contentHTML = contentHTML.Replace("[PR10]", PR10);
            contentHTML = contentHTML.Replace("[PR11]", PR11);
            contentHTML = contentHTML.Replace("[PR12]", PR12);
            contentHTML = contentHTML.Replace("[PR13]", PR13);
            contentHTML = contentHTML.Replace("[PR14]", PR14);
            contentHTML = contentHTML.Replace("[PR15]", PR15);
            contentHTML = contentHTML.Replace("[PR16]", PR16);
            contentHTML = contentHTML.Replace("[PR20]", PR20);
            contentHTML = contentHTML.Replace("[PR21]", PR21);
            contentHTML = contentHTML.Replace("[PR22]", PR22);
            return contentHTML;
        }
        public static string CreateHTMLC02(string SheetName, string WebRootPath, string BARCODE_QR, string PR1, string PR2, string PR3, string PR4, string PR5, string PR6, string PR7, string PR8, string PR9, string PR10, string PR11, string PR12, string PR13, string PR14, string PR15, string PR16, string PR17, string PR18, string PR19, string PR20, string PR21, string PR22, string PR23, string PR24, string PR25)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "C02Sub.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            //if (PR17.Length > 20)
            //{
            //    //PR17 = PR17.Substring(0, 20);
            //}
            //if (PR6.Length > 20)
            //{
            //    PR6 = PR6.Substring(0, 20);
            //}
            string QRCodeFileName = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[PR1]", PR1);
            contentHTML = contentHTML.Replace("[PR2]", PR2);
            contentHTML = contentHTML.Replace("[PR3]", PR3);
            contentHTML = contentHTML.Replace("[PR4]", PR4);
            contentHTML = contentHTML.Replace("[PR5]", PR5);
            contentHTML = contentHTML.Replace("[PR6]", PR6);
            contentHTML = contentHTML.Replace("[PR7]", PR7);
            contentHTML = contentHTML.Replace("[PR8]", PR8);
            contentHTML = contentHTML.Replace("[PR9]", PR9);
            contentHTML = contentHTML.Replace("[PR10]", PR10);
            contentHTML = contentHTML.Replace("[PR11]", PR11);
            contentHTML = contentHTML.Replace("[PR12]", PR12);
            contentHTML = contentHTML.Replace("[PR13]", PR13);
            contentHTML = contentHTML.Replace("[PR14]", PR14);
            contentHTML = contentHTML.Replace("[PR15]", PR15);
            contentHTML = contentHTML.Replace("[PR16]", PR16);
            contentHTML = contentHTML.Replace("[PR17]", PR17);
            contentHTML = contentHTML.Replace("[PR18]", PR18);
            contentHTML = contentHTML.Replace("[PR19]", PR19);
            contentHTML = contentHTML.Replace("[PR20]", PR20);
            contentHTML = contentHTML.Replace("[PR21]", PR21);
            contentHTML = contentHTML.Replace("[PR22]", PR22);
            contentHTML = contentHTML.Replace("[PR23]", PR23);
            contentHTML = contentHTML.Replace("[PR24]", PR24);
            contentHTML = contentHTML.Replace("[PR25]", PR25);
            return contentHTML;
        }
        public static string CreateHTMLC09_REPRINT(string SheetName, string WebRootPath, string BARCODE_QR, string PR1, string PR2, string PR3, string PR4, string PR5, string PR7, string PR8, string PR20, string PR23, string PR24, string M_LEN, string S_LEN, List<string> LEAD_BOM)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "C09_REPRINT.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string QRCodeFileName = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_QR);
            contentHTML = contentHTML.Replace("[QRCodeFileName]", QRCodeFileName);
            contentHTML = contentHTML.Replace("[PR1]", PR1);
            contentHTML = contentHTML.Replace("[PR2]", PR2);
            contentHTML = contentHTML.Replace("[PR3]", PR3);
            contentHTML = contentHTML.Replace("[PR4]", PR4);
            contentHTML = contentHTML.Replace("[PR5]", PR5);
            contentHTML = contentHTML.Replace("[PR7]", PR7);
            contentHTML = contentHTML.Replace("[PR8]", PR8);
            contentHTML = contentHTML.Replace("[PR20]", PR20);
            contentHTML = contentHTML.Replace("[PR23]", PR23);
            contentHTML = contentHTML.Replace("[PR24]", PR24);
            contentHTML = contentHTML.Replace("[M_LEN]", M_LEN);
            contentHTML = contentHTML.Replace("[S_LEN]", S_LEN);
            for (int i = 0; i < 30; i++)
            {
                contentHTML = contentHTML.Replace("[LEAD_BOM" + i + "]", LEAD_BOM[i]);
            }
            return contentHTML;
        }
        public static string CreateHTMLV03Tab1(string SheetName, string WebRootPath, string DGV_NO, string DGV_DEP, string DGV_USER,
   string DGV_DATE, string DGV_PN_NM, string DGV_PN_KRVN, string DGV_SPEC_KRVN, string DGV_UNIT_KRVN,
   string DGV_PQTY, string DGV_MEMO, string DGV_STOCK, string DGV_QTY, string DGV_BE_COST, string DGV_COST,
   string DGV_SUM_COST, string DGV_VAT, string DGV_ECT_COST, string DGV_TOT_COST, string DGV_COMP_NM)
        {
            string contentHTML = GlobalHelper.InitializationString;
            string folderPath = Path.Combine(WebRootPath, GlobalHelper.HTML, "V03Tab1.html");
            using (FileStream fs = new FileStream(folderPath, FileMode.Open))
            {
                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                {
                    contentHTML = r.ReadToEnd();
                }
            }
            string specPart1 = "";
            string specPart2 = "";

            if (!string.IsNullOrEmpty(DGV_SPEC_KRVN))
            {
                string[] specParts = DGV_SPEC_KRVN.Split(new[] { '\n' }, 2);
                specPart1 = specParts[0];
                specPart2 = specParts.Length > 1 ? specParts[1] : "";
            }
            string unitPart = "";
            string pqtyPart = "";

            if (!string.IsNullOrEmpty(DGV_UNIT_KRVN))
            {
                string[] unitParts = DGV_UNIT_KRVN.Split(new[] { '\n' }, 2);
                unitPart = unitParts[0]; // UNIT_VN/UNIT_KR
                pqtyPart = unitParts.Length > 1 ? unitParts[1] : ""; // PQTY đã định dạng
            }
            // Tạo dòng sản phẩm cho bảng
            string productRow = $@"
<tr>
    <td>{DGV_DEP}</td>
    <td>{DGV_PN_NM}</td>
    <td>
        <div style=""font-weight:normal;"">{specPart1}</div>
        <div style=""height:1px; background-color:#000; margin:5px 0;""></div>
        <div style=""font-weight:bold;"">{specPart2}</div>
    </td>
    <td>
        <div style=""font-weight:normal;"">{unitPart}</div>
        <div style=""height:1px; background-color:#000; margin:5px 0;""></div>
        <div style=""font-weight:bold;"">{pqtyPart}</div>
    </td>
    <td>{DGV_MEMO}</td>
    <td>{DGV_QTY}</td>
    <td>{DGV_BE_COST}</td>
    <td>
        <div style=""font-weight:normal;"">{DGV_COST}</div>
        <div style=""height:1px; background-color:#000; margin:5px 0;""></div>
        <div style=""font-weight:bold;"">{DGV_SUM_COST}</div>
    </td>
    <td>{DGV_VAT}</td>
    <td>{DGV_ECT_COST}</td>
    <td>{DGV_TOT_COST}</td>
    <td>{DGV_COMP_NM}</td>
</tr>";

            // Thay thế tất cả placeholder trong template
            var replacements = new Dictionary<string, string>
            {
                ["[DGV_NO]"] = string.IsNullOrEmpty(DGV_NO) ? "-" : DGV_NO,
                ["[DGV_DATE]"] = string.IsNullOrEmpty(DGV_DATE) ? "-" : DGV_DATE,
                ["[DGV_USER]"] = string.IsNullOrEmpty(DGV_USER) ? "-" : DGV_USER,
                ["[DGV_PRINT_DATE]"] = DateTime.Now.ToString("yyyy-MM-dd"),
                ["[DGV_CONTENT]"] = productRow,
                ["[DGV_SUM_COST]"] = string.IsNullOrEmpty(DGV_SUM_COST) ? "0" : DGV_SUM_COST,
                ["[DGV_VAT]"] = string.IsNullOrEmpty(DGV_VAT) ? "0" : DGV_VAT,
                ["[DGV_ECT_COST]"] = string.IsNullOrEmpty(DGV_ECT_COST) ? "0" : DGV_ECT_COST,
                ["[DGV_TOT_COST]"] = string.IsNullOrEmpty(DGV_TOT_COST) ? "0" : DGV_TOT_COST
            };

            // Thay thế tất cả placeholder trong một lần lặp
            foreach (var replacement in replacements)
            {
                contentHTML = contentHTML.Replace(replacement.Key, replacement.Value);
            }

            return contentHTML;
        }
        public static string CreateHTML(string SheetName, string WebRootPath, string HTMLContent)
        {
            string result = GlobalHelper.InitializationString;
            if (!string.IsNullOrEmpty(HTMLContent.ToString()))
            {
                string contentHTML = GlobalHelper.InitializationString;
                string physicalPathOpen = Path.Combine(WebRootPath, GlobalHelper.HTML, "Empty.html");
                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        contentHTML = r.ReadToEnd();
                    }
                }
                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                string physicalPathCreate = Path.Combine(WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(contentHTML);
                    }
                }
                result = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
            }
            return result;
        }
        public static string CreateHTMLClose(string SheetName, string WebRootPath, string HTMLContent)
        {
            string result = GlobalHelper.InitializationString;
            if (!string.IsNullOrEmpty(HTMLContent.ToString()))
            {
                string contentHTML = GlobalHelper.InitializationString;
                string physicalPathOpen = Path.Combine(WebRootPath, GlobalHelper.HTML, "EmptyClose.html");
                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        contentHTML = r.ReadToEnd();
                    }
                }
                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                string physicalPathCreate = Path.Combine(WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(contentHTML);
                    }
                }
                result = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
            }
            return result;
        }
        public static string SetName(string fileName)
        {
            string fileNameReturn = fileName;
            if (!string.IsNullOrEmpty(fileNameReturn))
            {
                fileNameReturn = fileNameReturn.ToLower();
                fileNameReturn = fileNameReturn.Replace("’", "-");
                fileNameReturn = fileNameReturn.Replace("“", "-");
                fileNameReturn = fileNameReturn.Replace("--", "-");
                fileNameReturn = fileNameReturn.Replace("+", "-");
                fileNameReturn = fileNameReturn.Replace("/", "-");
                fileNameReturn = fileNameReturn.Replace(@"\", "-");
                fileNameReturn = fileNameReturn.Replace(":", "-");
                fileNameReturn = fileNameReturn.Replace(";", "-");
                fileNameReturn = fileNameReturn.Replace("%", "-");
                fileNameReturn = fileNameReturn.Replace("`", "-");
                fileNameReturn = fileNameReturn.Replace("~", "-");
                fileNameReturn = fileNameReturn.Replace("#", "-");
                fileNameReturn = fileNameReturn.Replace("$", "-");
                fileNameReturn = fileNameReturn.Replace("^", "-");
                fileNameReturn = fileNameReturn.Replace("&", "-");
                fileNameReturn = fileNameReturn.Replace("*", "-");
                fileNameReturn = fileNameReturn.Replace("(", "-");
                fileNameReturn = fileNameReturn.Replace(")", "-");
                fileNameReturn = fileNameReturn.Replace("|", "-");
                fileNameReturn = fileNameReturn.Replace("'", "-");
                fileNameReturn = fileNameReturn.Replace(",", "-");
                fileNameReturn = fileNameReturn.Replace(".", "-");
                fileNameReturn = fileNameReturn.Replace("?", "-");
                fileNameReturn = fileNameReturn.Replace("<", "-");
                fileNameReturn = fileNameReturn.Replace(">", "-");
                fileNameReturn = fileNameReturn.Replace("]", "-");
                fileNameReturn = fileNameReturn.Replace("[", "-");
                fileNameReturn = fileNameReturn.Replace(@"""", "-");
                fileNameReturn = fileNameReturn.Replace(@" ", "-");
                fileNameReturn = fileNameReturn.Replace("á", "a");
                fileNameReturn = fileNameReturn.Replace("à", "a");
                fileNameReturn = fileNameReturn.Replace("ả", "a");
                fileNameReturn = fileNameReturn.Replace("ã", "a");
                fileNameReturn = fileNameReturn.Replace("ạ", "a");
                fileNameReturn = fileNameReturn.Replace("ă", "a");
                fileNameReturn = fileNameReturn.Replace("ắ", "a");
                fileNameReturn = fileNameReturn.Replace("ằ", "a");
                fileNameReturn = fileNameReturn.Replace("ẳ", "a");
                fileNameReturn = fileNameReturn.Replace("ẵ", "a");
                fileNameReturn = fileNameReturn.Replace("ặ", "a");
                fileNameReturn = fileNameReturn.Replace("â", "a");
                fileNameReturn = fileNameReturn.Replace("ấ", "a");
                fileNameReturn = fileNameReturn.Replace("ầ", "a");
                fileNameReturn = fileNameReturn.Replace("ẩ", "a");
                fileNameReturn = fileNameReturn.Replace("ẫ", "a");
                fileNameReturn = fileNameReturn.Replace("ậ", "a");
                fileNameReturn = fileNameReturn.Replace("í", "i");
                fileNameReturn = fileNameReturn.Replace("ì", "i");
                fileNameReturn = fileNameReturn.Replace("ỉ", "i");
                fileNameReturn = fileNameReturn.Replace("ĩ", "i");
                fileNameReturn = fileNameReturn.Replace("ị", "i");
                fileNameReturn = fileNameReturn.Replace("ý", "y");
                fileNameReturn = fileNameReturn.Replace("ỳ", "y");
                fileNameReturn = fileNameReturn.Replace("ỷ", "y");
                fileNameReturn = fileNameReturn.Replace("ỹ", "y");
                fileNameReturn = fileNameReturn.Replace("ỵ", "y");
                fileNameReturn = fileNameReturn.Replace("ó", "o");
                fileNameReturn = fileNameReturn.Replace("ò", "o");
                fileNameReturn = fileNameReturn.Replace("ỏ", "o");
                fileNameReturn = fileNameReturn.Replace("õ", "o");
                fileNameReturn = fileNameReturn.Replace("ọ", "o");
                fileNameReturn = fileNameReturn.Replace("ô", "o");
                fileNameReturn = fileNameReturn.Replace("ố", "o");
                fileNameReturn = fileNameReturn.Replace("ồ", "o");
                fileNameReturn = fileNameReturn.Replace("ổ", "o");
                fileNameReturn = fileNameReturn.Replace("ỗ", "o");
                fileNameReturn = fileNameReturn.Replace("ộ", "o");
                fileNameReturn = fileNameReturn.Replace("ơ", "o");
                fileNameReturn = fileNameReturn.Replace("ớ", "o");
                fileNameReturn = fileNameReturn.Replace("ờ", "o");
                fileNameReturn = fileNameReturn.Replace("ở", "o");
                fileNameReturn = fileNameReturn.Replace("ỡ", "o");
                fileNameReturn = fileNameReturn.Replace("ợ", "o");
                fileNameReturn = fileNameReturn.Replace("ú", "u");
                fileNameReturn = fileNameReturn.Replace("ù", "u");
                fileNameReturn = fileNameReturn.Replace("ủ", "u");
                fileNameReturn = fileNameReturn.Replace("ũ", "u");
                fileNameReturn = fileNameReturn.Replace("ụ", "u");
                fileNameReturn = fileNameReturn.Replace("ư", "u");
                fileNameReturn = fileNameReturn.Replace("ứ", "u");
                fileNameReturn = fileNameReturn.Replace("ừ", "u");
                fileNameReturn = fileNameReturn.Replace("ử", "u");
                fileNameReturn = fileNameReturn.Replace("ữ", "u");
                fileNameReturn = fileNameReturn.Replace("ự", "u");
                fileNameReturn = fileNameReturn.Replace("é", "e");
                fileNameReturn = fileNameReturn.Replace("è", "e");
                fileNameReturn = fileNameReturn.Replace("ẻ", "e");
                fileNameReturn = fileNameReturn.Replace("ẽ", "e");
                fileNameReturn = fileNameReturn.Replace("ẹ", "e");
                fileNameReturn = fileNameReturn.Replace("ê", "e");
                fileNameReturn = fileNameReturn.Replace("ế", "e");
                fileNameReturn = fileNameReturn.Replace("ề", "e");
                fileNameReturn = fileNameReturn.Replace("ể", "e");
                fileNameReturn = fileNameReturn.Replace("ễ", "e");
                fileNameReturn = fileNameReturn.Replace("ệ", "e");
                fileNameReturn = fileNameReturn.Replace("đ", "d");
                fileNameReturn = fileNameReturn.Replace("--", "-");
            }
            return fileNameReturn;
        }
        public static DateTime CovertStringToDateTime(string DateTime)
        {
            var result = new DateTime();
            var DateFlag = new DateTime(1900, 1, 1);
            if (!string.IsNullOrEmpty(DateTime))
            {
                try
                {
                    var DayString = DateTime;
                    var day = int.Parse(DayString);
                    day = day - 2;
                    result = DateFlag.AddDays(day);
                }
                catch (Exception ex1)
                {
                    try
                    {
                        var DateString = DateTime;
                        DateString = DateString.Split(' ')[0];
                        var year = DateString.Split('/')[2];
                        var month = DateString.Split('/')[0];
                        var day = DateString.Split('/')[1];
                        result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            var DateString = DateTime;
                            DateString = DateString.Split(' ')[0];
                            var year = DateString.Split('/')[2];
                            var month = DateString.Split('/')[1];
                            var day = DateString.Split('/')[0];
                            result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
                        }
                        catch (Exception ex3)
                        {
                        }
                    }
                }
            }
            return result;
        }
        public static string? MESConectionStringByCompanyID(long CompanyID)
        {
            var result = GlobalHelper.MariaDBConectionString;
            switch (CompanyID)
            {
                case 17:
                    result = GlobalHelper.MariaDBConectionStringDJM;
                    break;
            }
            return result;
        }
        #endregion
    }
}
