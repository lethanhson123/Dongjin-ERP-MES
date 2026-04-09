
namespace MESService.Implement
{
    public class C08Service : BaseService<torderlist, ItorderlistRepository>
    , IC08Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C08Service(ItorderlistRepository torderlistRepository
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
                await Task.Run(() => { });
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
                await Task.Run(() => { });
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
                await Task.Run(() => { });
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
        public virtual async Task<BaseResult> SearchLead(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string searchText = "";
                if (BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count > 1)
                {
                    searchText = BaseParameter.ListSearchString[1]; 
                }

                string query = @"SELECT 
            torder_lead_bom.LEAD_PN, 
            torder_lead_bom.BUNDLE_SIZE, 
            (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = torder_lead_bom.LEAD_PN) AS HOOK_RACK 
        FROM 
            torder_lead_bom 
        WHERE 
            torder_lead_bom.LEAD_PN LIKE '%" + searchText + @"%' 
            AND torder_lead_bom.DSCN_YN = 'Y'
        ORDER BY torder_lead_bom.LEAD_PN
        LIMIT 50";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                result.DataGridView1 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(dt);
                }
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
                string leadNo = BaseParameter.ListSearchString[0];
                string quantity = BaseParameter.ListSearchString[1];
                int numericUpDownValue = int.Parse(BaseParameter.ListSearchString[2]);

                result.Barcodes = new List<string>();

                for (int i = 1; i <= numericUpDownValue; i++)
                {
                    int timeS = int.Parse(DateTime.Now.ToString("HHmmss")) + i + new Random().Next(1, 100);

                    string query = "SELECT (MAX(TRACK_BC_TMP.TRACK_BC_IDX)+1) FROM TRACK_BC_TMP";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
                    int barcodeQrSeq = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    string barcodeQr = leadNo.ToUpper() + "$$" + quantity + "$$" + DateTime.Now.ToString("yyyy-MM-dd") + "$$0$$" + barcodeQrSeq + "@@";

                    string insertSql = "INSERT INTO TRACK_BC_TMP (`TRACK_BC_LEAD`, `TRACK_BC_NAME`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + leadNo + "', '" + barcodeQr + "', NOW(), '" + BaseParameter.USER_IDX + "')";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);

                    result.Barcodes.Add(barcodeQr + "|" + timeS.ToString());
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button4_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string leadNo = BaseParameter.ListSearchString[0];
                string quantity = BaseParameter.ListSearchString[1];
                int numericUpDownValue = int.Parse(BaseParameter.ListSearchString[2]);

                // Tạo danh sách barcodes để trả về frontend
                result.Barcodes = new List<string>();

                int timeS = int.Parse(DateTime.Now.ToString("HHmmss"));

                for (int i = 1; i <= numericUpDownValue; i++)
                {
                    timeS = timeS + i;

                    // Lấy index mới từ TRACK_BC_TMP
                    string query = "SELECT (MAX(TRACK_BC_TMP.TRACK_BC_IDX)+1) FROM TRACK_BC_TMP";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
                    int barcodeQrSeq = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    // Tạo chuỗi barcode
                    string barcodeQr = leadNo.ToUpper() + "$$" + quantity + "$$" + DateTime.Now.ToString("yyyy-MM-dd") + "$$0$$" + barcodeQrSeq + "@@";

                    // Lưu thông tin barcode vào DB
                    string insertSql = "INSERT INTO TRACK_BC_TMP (`TRACK_BC_LEAD`, `TRACK_BC_NAME`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + leadNo + "', '" + barcodeQr + "', NOW(), '" + BaseParameter.USER_IDX + "')";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);

                    // Thêm barcode vào kết quả để trả về frontend
                    result.Barcodes.Add(barcodeQr);
                }
                result.TimeS = timeS.ToString();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> TextBox4_KeyDown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string textBox4Value = BaseParameter.TextBox4;
                string label37Value = BaseParameter.Label37;

                if (string.IsNullOrEmpty(textBox4Value))
                {
                    return result;
                }

                // Lấy phần đầu của barcode trước ký tự $$
                string leadNo = textBox4Value.Substring(0, textBox4Value.IndexOf("$$"));
                string leadChk01 = textBox4Value.Substring(0, textBox4Value.LastIndexOf("$$"));

                // Kiểm tra barcode trong DB
                string checkQuery = @"SELECT 
            TBCTOTAL.`TORDER_BARCODENM`, TBCTOTAL.`Barcode_SEQ`, MID(TBCTOTAL.`TORDER_BARCODENM`, 1, INSTR(TBCTOTAL.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, 
            (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = MID(TBCTOTAL.`TORDER_BARCODENM`, 1, INSTR(TBCTOTAL.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`,
            (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(TBCTOTAL.`TORDER_BARCODENM`, 1, INSTR(TBCTOTAL.`TORDER_BARCODENM`, '$$')-1)) AS `HOOR_RACK`,
            (SELECT COUNT(TBCTOTAL.`TORDER_BARCODENM`) AS `BC_COUNT` FROM ((SELECT * FROM (SELECT * FROM TORDER_BARCODE WHERE `DSCN_YN`='Y' UNION SELECT * FROM torder_barcode_lp WHERE `DSCN_YN`='Y') 
            AS `TB1`) UNION SELECT * FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='Y') AS `TBCTOTAL` WHERE `TBCTOTAL`.`TORDER_BARCODENM` LIKE '" + leadChk01 + @"%') AS `BC_count`, 
            (SELECT COUNT(trackmtim.`BARCODE_NM`) FROM trackmtim WHERE trackmtim.`BARCODE_NM` LIKE '" + leadChk01 + @"%') AS `IN_count`,
            `TBCTOTAL`.`TORDER_BARCODE_IDX`, `TBCTOTAL`.`TB_NM` FROM ((SELECT * FROM (SELECT *, 'KOMAX' AS `TB_NM` FROM TORDER_BARCODE WHERE `DSCN_YN`='Y' UNION SELECT *, 'LP' AS `TB_NM` FROM torder_barcode_lp WHERE `DSCN_YN`='Y')
            AS `TB1`) UNION SELECT *, 'SPST' AS `TB_NM` FROM TORDER_BARCODE_SP WHERE `DSCN_YN`='Y')
            AS `TBCTOTAL` WHERE `TBCTOTAL`.`TORDER_BARCODENM` = '" + textBox4Value + "' ORDER BY `UPDATE_DTM` DESC LIMIT 500";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkQuery);

                bool leadChk = false;

                if (ds.Tables[0].Rows.Count <= 0)
                {
                    // Kiểm tra 6 ký tự cuối
                    string textChk = textBox4Value.Substring(textBox4Value.Length - 6);
                    int textCount = textChk.LastIndexOf("$");

                    if (textCount <= 1)
                    {
                        leadChk = true;
                    }
                    else
                    {
                        result.Error = "NONE HOOK Master DATA. Please Check Again. Barcode Please Check Again.";
                        return result;
                    }
                }
                else
                {
                    leadChk = true;
                }

                if (leadChk)
                {
                    // Trích xuất thông tin từ barcode
                    string bbb = textBox4Value.Substring(textBox4Value.IndexOf("$$") + 2);
                    string bb = bbb.Substring(0, bbb.IndexOf("$$")); // Số lượng

                    // Lưu vào bảng trackmtim
                    string insertSql = @"INSERT INTO trackmtim (trackmtim.RACK_IDX, trackmtim.RACKCODE, trackmtim.TABLE_IDX, trackmtim.TABLE_NM, trackmtim.LEAD_NM, trackmtim.BARCODE_NM, trackmtim.RACKDTM, 
            trackmtim.QTY, trackmtim.RACKIN_YN, trackmtim.RACKOUT_YN, trackmtim.CREATE_DTM, trackmtim.CREATE_USER) 
            SELECT (SELECT trackmaster.RACK_IDX FROM trackmaster WHERE trackmaster.LEAD_NO = MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1)) AS `RACK_IDX`, 'INPUT', 
            0, 'USER', MID(`TBCTOTAL`.`TORDER_BARCODENM`, 1, INSTR(`TBCTOTAL`.`TORDER_BARCODENM`, '$$')-1) AS `LEAD_NO`, `TBCTOTAL`.`TORDER_BARCODENM`, NOW() , '" + bb + "', 'Y', 'N', NOW(), '" + BaseParameter.USER_IDX + "'FROM(SELECT '" + textBox4Value + "' AS `TORDER_BARCODENM`) AS `TBCTOTAL`";
        

            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);

                    // Cập nhật bảng tiivtr_lead
                    string updateSql = @"INSERT INTO tiivtr_lead (tiivtr_lead.`PART_IDX`, tiivtr_lead.`LOC_IDX`, tiivtr_lead.`QTY`, tiivtr_lead.`CREATE_DTM`, tiivtr_lead.`CREATE_USER`)
            SELECT (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = trackmtim.LEAD_NM) AS `LEAD_IDX`, '3', trackmtim.`QTY`, NOW(), '" + BaseParameter.USER_IDX + "'FROM trackmtim WHERE trackmtim.BARCODE_NM = '" + textBox4Value + "' ON DUPLICATE KEY UPDATE tiivtr_lead.`QTY` = (tiivtr_lead.`QTY` +trackmtim.`QTY`), tiivtr_lead.`UPDATE_DTM` = NOW(), tiivtr_lead.`UPDATE_USER` = '" + BaseParameter.USER_IDX + "'";
        

            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                    // Cập nhật số lượng đã scan
                    int scanCount = int.Parse(label37Value) + 1;
                    result.Label37 = scanCount.ToString();
                    result.Success = true;

                    // Đặt AudioCue để frontend phát âm thanh thành công
                    result.AudioCue = "success";
                    result.ClearInput = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.AudioCue = "error";
            }
            return result;
        }
        public virtual async Task<BaseResult> PrintBarcode(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.Barcodes != null && BaseParameter.Barcodes.Count > 0 && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 7)
                {
                    StringBuilder SearchString = new StringBuilder();
                    string SheetName = this.GetType().Name;

                    foreach (var barcodeWithTime in BaseParameter.Barcodes)
                    {
                        string[] parts = barcodeWithTime.Split('|');
                        string barcode = parts[0];
                        string timeS = (parts.Length > 1) ? parts[1] : DateTime.Now.Ticks.ToString();

                        string BARCODE_QR = barcode;
                        string BARCODE_AA = BaseParameter.ListSearchString[1];
                        string BARCODE_BB = BaseParameter.ListSearchString[2];
                        string BARCODE_CC = BaseParameter.ListSearchString[3];
                        string BARCODE_DD = BaseParameter.ListSearchString[4];
                        string BARCODE_EE = BaseParameter.ListSearchString[5];
                        string BARCODE_FF = BaseParameter.ListSearchString[6];

                        string HTMLContent = GlobalHelper.CreateHTMLC08(SheetName, _WebHostEnvironment.WebRootPath,
                            BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, timeS);
                        SearchString.AppendLine(HTMLContent);
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

                    string fileName = "Barcodes_" + DateTime.Now.Ticks + ".html";
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
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> PrintBarcodeOnly(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.Barcodes != null && BaseParameter.Barcodes.Count > 0 && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 3)
                {
                    StringBuilder SearchString = new StringBuilder();
                    string SheetName = this.GetType().Name;

                    foreach (var barcode in BaseParameter.Barcodes)
                    {
                        string BARCODE_QR = barcode;
                        string BARCODE_AA = BaseParameter.ListSearchString[1]; // lead no
                        string BARCODE_BB = BaseParameter.ListSearchString[2]; // quantity

                        string HTMLContent = GlobalHelper.CreateHTMLC08BarcodeOnly(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB);
                        SearchString.AppendLine(HTMLContent);
                    }

                    string contentHTML = "";
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            contentHTML = r.ReadToEnd();
                        }
                    }

                    contentHTML = contentHTML.Replace(@"[Content]", SearchString.ToString());

                    string fileName = "BarcodeOnly_" + DateTime.Now.Ticks + ".html";
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
                    result.Success = true;
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

