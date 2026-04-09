namespace MESService.Implement
{
    public class B03_1Service : BaseService<torderlist, ItorderlistRepository>
    , IB03_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B03_1Service(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null)
                {
                    string AAA = "%" + BaseParameter.ListSearchString[0] + "%";
                    string BBB = "%" + BaseParameter.ListSearchString[1] + "%";
                    string CCC = "%" + BaseParameter.ListSearchString[2] + "%";

                    string sql = @"SELECT  
TMBRCD.`BARCD_ID`, TMMTIN.`MTIN_DTM`, tspart.`PART_NO`, tspart.`PART_NM`, TMBRCD.`PKG_GRP`,  TMBRCD.`PKG_QTY`,
TMMTIN.`UTM`, TMMTIN.`QTY`, TMMTIN.`NET_WT`, TMMTIN.`GRS_WT`, TMMTIN.`PLET_NO`, TMMTIN.`SHPD_NO`,
TMMTIN.`DSCN_YN`,  tspart.`PART_LOC`, TMMTIN.`MTIN_RMK`
FROM     TMBRCD, TMMTIN, tspart
WHERE  TMBRCD.`MTIN_IDX` = TMMTIN.`MTIN_IDX` AND TMMTIN.`PART_IDX` = tspart.`PART_IDX` AND TMMTIN.`BRCD_PRNT` = 'Y' AND 
TMBRCD.`BARCD_ID` LIKE '" + AAA + "' AND tspart.`PART_NO` LIKE '" + BBB + "'  AND tspart.`PART_NM` LIKE '" + CCC + "' ORDER BY TMMTIN.`MTIN_DTM` DESC, tspart.`PART_NM`  LIMIT 100";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ListtmbrcdTranfer = new List<tmbrcdTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ListtmbrcdTranfer.AddRange(SQLHelper.ToList<tmbrcdTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null)
                {
                    string BARCODE_QR = BaseParameter.ListSearchString[0];
                    string BARCODE_AA = BaseParameter.ListSearchString[1];
                    string BARCODE_BB = BaseParameter.ListSearchString[2];
                    string BARCODE_CC = BaseParameter.ListSearchString[3];
                    string BARCODE_DD = BaseParameter.ListSearchString[4];
                    string BARCODE_EE = BaseParameter.ListSearchString[5];
                    string BARCODE_FF = BaseParameter.ListSearchString[6];
                    string BARCODE_GG = BaseParameter.ListSearchString[7];
                    string BARCODE_HH = BaseParameter.ListSearchString[8];
                    string BARCODE_ZZ = BaseParameter.ListSearchString[9];
                    StringBuilder SearchString = new StringBuilder();
                    string SheetName = this.GetType().Name;
                    string HTMLContent = GlobalHelper.CreateHTMLB03(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ);
                    SearchString.AppendLine(HTMLContent);
                    string contentHTML = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            contentHTML = r.ReadToEnd();
                        }
                    }
                    contentHTML = contentHTML.Replace(@"[Content]", SearchString.ToString());
                    //string fileName = BARCODE_QR + ".html";
                    string fileName = GlobalHelper.InitializationDateTimeCode + ".html";
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            await w.WriteLineAsync(contentHTML);
                        }
                    }
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

    }
}

