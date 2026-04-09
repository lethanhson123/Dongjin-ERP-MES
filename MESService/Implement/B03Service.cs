
namespace MESService.Implement
{
    public class B03Service : BaseService<torderlist, ItorderlistRepository>
    , IB03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B03Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null)
                {
                    string such1 = BaseParameter.ListSearchString[0];
                    string such2 = BaseParameter.ListSearchString[1];
                    string such3 = BaseParameter.ListSearchString[2];
                    string such4 = BaseParameter.ListSearchString[3];
                    string such5 = BaseParameter.ListSearchString[4];

                    such2 = "%" + such2 + "%";
                    such3 = "%" + such3 + "%";
                    such4 = "%" + such4 + "%";
                    such5 = "%" + such5 + "%";

                    string sql = @"SELECT  FALSE AS `CHK`,  `A`.`MTIN_DTM`, `B`.`PART_NO`, `B`.`PART_NM`, `A`.`UTM`, `A`.`SNP_QTY`,
                                        `A`.`QTY`, `A`.`NET_WT`, `A`.`GRS_WT`, `A`.`PLET_NO`, `A`.`SHPD_NO`, `B`.`PART_LOC`,
                                        IFNULL(`A`.`MTIN_RMK`,'') AS MTIN_RMK, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, `A`.`MTIN_IDX`, `A`.`PART_IDX`
                                        FROM     TMMTIN `A`, tspart `B`
                                        WHERE  `A`.`PART_IDX` = `B`.`PART_IDX` AND `A`.`QTY` > 0  AND `A`.`MTIN_DTM` = '" + such1 + "' AND `B`.`PART_NO` LIKE '" + such2 + "' AND `B`.`PART_NM` LIKE '" + such3 + "' AND `A`.`BRCD_PRNT` = 'N' AND `A`.`SHPD_NO` LIKE '" + such4 + "' AND `A`.`PLET_NO` LIKE '" + such5 + "'";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.SearchString != null)
                {
                    string uName = BaseParameter.SearchString;


                    string sql = @"SELECT `PART_NO`, `PART_IDX`, `PART_NM`, `PART_SNP` FROM tspart WHERE  `PART_NO` = '" + uName + "' AND `PART_SCN` = '5'   ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_B03_01 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_B03_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    string SV2 = BaseParameter.ListSearchString[1];
                    string SV3 = BaseParameter.ListSearchString[2];
                    string SV4 = BaseParameter.ListSearchString[3];
                    string SV5 = BaseParameter.ListSearchString[4];
                    string SV6 = BaseParameter.ListSearchString[5];
                    string SV7 = BaseParameter.ListSearchString[6];
                    string SV8 = BaseParameter.ListSearchString[7];
                    string SV9 = BaseParameter.ListSearchString[8];
                    string SV10 = BaseParameter.ListSearchString[9];
                    string Date1 = BaseParameter.ListSearchString[10];
                    if (string.IsNullOrEmpty(SV5))
                    {
                        SV5 = "" + GlobalHelper.InitializationNumber;
                    }
                    if (string.IsNullOrEmpty(SV6))
                    {
                        SV6 = "" + GlobalHelper.InitializationNumber;
                    }
                    if (BaseParameter.Action == 0)
                    {
                        string SV1 = BaseParameter.ListSearchString[0];

                        string sql = @"INSERT INTO TMMTIN (`PART_IDX`, `UTM`, `DESC`, `QTY`, `NET_WT`, `GRS_WT`, `PLET_NO`, `SHPD_NO`, `MTIN_DTM`, `DSCN_YN`, `MTIN_RMK`, `SNP_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES  ('" + SV1 + "', '" + SV3 + "', '" + SV2 + "', '" + SV4 + "', '" + SV5 + "', '" + SV6 + "', '" + SV7 + "', '" + SV8 + "', '" + Date1 + "', 'N', '" + SV9 + "', '" + SV10 + "' , NOW(), '" + USER_IDX + "')";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                    if (BaseParameter.Action == 1)
                    {
                        string SV1 = BaseParameter.ListSearchString[11];

                        string sql = @"UPDATE TMMTIN    SET `UTM` = '" + SV3 + "', `DESC` = '" + SV2 + "', `QTY` = '" + SV4 + "', `NET_WT` = '" + SV5 + "', `GRS_WT` = '" + SV6 + "', `SNP_QTY` = '" + SV10 + "', `PLET_NO` = '" + SV7 + "', `SHPD_NO` = '" + SV8 + "', `MTIN_DTM` = '" + Date1 + "', `DSCN_YN` = 'N', `MTIN_RMK` = '" + SV9 + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "' WHERE(`MTIN_IDX` = '" + SV1 + "')";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
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
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
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
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
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
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
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
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            string SheetName = this.GetType().Name;
            try
            {
                if (BaseParameter != null)
                {
                    result.ListSearchString = new List<string>();
                    StringBuilder SearchString = new StringBuilder();
                    string USER_IDX = BaseParameter.USER_IDX;
                    string BARCODE_QR = GlobalHelper.InitializationString;
                    string BARCODE_AA = GlobalHelper.InitializationString;
                    string BARCODE_BB = GlobalHelper.InitializationString;
                    string BARCODE_CC = GlobalHelper.InitializationString;
                    string BARCODE_DD = GlobalHelper.InitializationString;
                    string BARCODE_EE = GlobalHelper.InitializationString;
                    string BARCODE_FF = GlobalHelper.InitializationString;
                    string BARCODE_GG = GlobalHelper.InitializationString;
                    string BARCODE_HH = GlobalHelper.InitializationString;
                    string BARCODE_ZZ = GlobalHelper.InitializationString;

                    string sql = GlobalHelper.InitializationString;
                    string VALUES1 = GlobalHelper.InitializationString;
                    string VALUES2 = GlobalHelper.InitializationString;
                    string VALUESSUM = GlobalHelper.InitializationString;
                    string sql_A = GlobalHelper.InitializationString;
                    string VALUES_A = GlobalHelper.InitializationString;
                    string VALUESSUM_A = GlobalHelper.InitializationString;


                    sql = @"SELECT  MAX(BARCD_IDX) AS BARCD_IDX FROM TMBRCD";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Listtmbrcd = new List<tmbrcd>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Listtmbrcd.AddRange(SQLHelper.ToList<tmbrcd>(dt));
                    }
                    result.tmbrcd = new tmbrcd();
                    int BAR_CNT = 0;
                    if (result.Listtmbrcd.Count > 0)
                    {
                        result.tmbrcd = result.Listtmbrcd[0];
                        BAR_CNT = result.tmbrcd.BARCD_IDX.Value;
                    }
                    sql = GlobalHelper.InitializationString;

                    foreach (var tmmtinTranfer in BaseParameter.DataGridView)
                    {
                        bool chk = tmmtinTranfer.CHK.Value;
                        if (chk == true)
                        {
                            BAR_CNT = BAR_CNT + 1;
                            string Bar_A = tmmtinTranfer.PART_NO + "$$" + BAR_CNT;
                            string Bar_B = "";
                            decimal MT_A = 0;
                            decimal MT_B = 0;
                            try
                            {
                                MT_A = tmmtinTranfer.SNP_QTY.Value;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            try
                            {
                                MT_B = (int)tmmtinTranfer.QTY.Value;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            decimal MT_C = 0;
                            decimal Bar_C = 0;
                            string MT_LOC = tmmtinTranfer.PART_LOC;
                            if (MT_A == 0)
                            {
                                MT_A = MT_B;
                            }
                            MT_C = Math.Ceiling(MT_B / MT_A);
                            decimal Bar_J = MT_B;
                            int Bar_D = tmmtinTranfer.MTIN_IDX.Value;
                            int Bar_I = 0;
                            string Bar_tmp = Bar_A;
                            string BBCO = GlobalHelper.InitializationString;
                            if (MT_C > 1)
                            {
                                BBCO = "N";
                            }
                            else
                            {
                                BBCO = "Y";
                            }
                            VALUES1 = "('" + Bar_A + "$$0" + "', '" + Bar_A + "$$0" + "', '" + "Large" + "', " + MT_B + ", " + Bar_D + ", NOW(), '" + USER_IDX + "', '" + BBCO + "', '" + MT_LOC + "', 0)";
                            if (VALUESSUM == "")
                            {
                                VALUESSUM = VALUES1;
                            }
                            else
                            {
                                VALUESSUM = VALUESSUM + ", " + VALUES1;
                            }
                            VALUES_A = tmmtinTranfer.MTIN_IDX.ToString();
                            if (VALUESSUM_A == "")
                            {
                                VALUESSUM_A = VALUES_A;
                            }
                            else
                            {
                                VALUESSUM_A = VALUESSUM_A + ", " + VALUES_A;
                            }
                            BARCODE_QR = Bar_A + "$$0";
                            BARCODE_AA = "Large";
                            if (MT_B <= MT_A)
                            {
                                BARCODE_BB = MT_B + "(" + MT_C + ")";
                                BARCODE_CC = MT_B.ToString();
                            }
                            else
                            {
                                BARCODE_BB = MT_A + "(" + MT_C + ")";
                                BARCODE_CC = MT_B.ToString();
                            }
                            BARCODE_DD = tmmtinTranfer.PART_NO;
                            BARCODE_EE = tmmtinTranfer.PART_NM;
                            BARCODE_FF = tmmtinTranfer.SHPD_NO;
                            BARCODE_GG = tmmtinTranfer.UTM;
                            BARCODE_HH = tmmtinTranfer.PART_LOC;
                            BARCODE_ZZ = tmmtinTranfer.MTIN_DTM.ToString();
                            string HTMLContent = GlobalHelper.CreateHTMLB03(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ);
                            SearchString.AppendLine(HTMLContent);
                            if (MT_C > 1)
                            {
                                if (MT_C > 500)
                                {
                                    MT_C = 500;
                                }
                                for (Bar_I = 1; Bar_I <= MT_C; Bar_I++)
                                {
                                    Bar_A = Bar_tmp + "$$" + Bar_I;
                                    Bar_J = Bar_J - MT_A;
                                    if (Bar_J < 0)
                                    {
                                        Bar_C = MT_A + Bar_J;
                                    }
                                    else
                                    {
                                        Bar_C = MT_A;
                                    }
                                    VALUES2 = "('" + Bar_A + "', '" + Bar_tmp + "$$0', '" + "Small" + "', " + Bar_C + ", " + Bar_D + ", NOW(), '" + USER_IDX + "', 'Y', '" + MT_LOC + "', 0)";
                                    if (VALUESSUM == "")
                                    {
                                        VALUESSUM = VALUES2;
                                    }
                                    else
                                    {
                                        VALUESSUM = VALUESSUM + ", " + VALUES2;
                                    }
                                    BARCODE_QR = Bar_A;
                                    BARCODE_AA = "Small " + Bar_I;
                                    BARCODE_BB = Bar_C.ToString();
                                    BARCODE_CC = MT_B.ToString();
                                    BARCODE_DD = tmmtinTranfer.PART_NO;
                                    BARCODE_EE = tmmtinTranfer.PART_NM;
                                    BARCODE_FF = tmmtinTranfer.SHPD_NO;
                                    BARCODE_GG = tmmtinTranfer.UTM;
                                    BARCODE_HH = tmmtinTranfer.PART_LOC;
                                    BARCODE_ZZ = tmmtinTranfer.MTIN_DTM.ToString();
                                    HTMLContent = GlobalHelper.CreateHTMLB03(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ);
                                    SearchString.AppendLine(HTMLContent);
                                }
                            }
                        }
                    }
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
                    string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".html";
                    // string fileName = FileNameHelper.GetSafeFileName(BARCODE_QR, ".html");

                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
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
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                    // result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + FileNameHelper.EncodeFileNameForUrl(fileName);



                    string sqlResult = GlobalHelper.InitializationString;
                    sql = "INSERT INTO TMBRCD (`BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `MTIN_IDX`, `CREATE_DTM`, `CREATE_USER`, `BBCO`, `BARCD_LOC`, `PKG_OUTQTY`) VALUES ";
                    if (VALUESSUM.Length > 0)
                    {
                        sql = sql + VALUESSUM;
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql_A = "UPDATE TMMTIN SET `BRCD_PRNT`='Y',`UPDATE_DTM`=NOW(), `UPDATE_USER`='" + USER_IDX + "' WHERE MTIN_IDX  IN ";

                        sql_A = sql_A + "(" + VALUESSUM_A + ")";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql_A);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
             

        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
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
        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
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

    }
}

