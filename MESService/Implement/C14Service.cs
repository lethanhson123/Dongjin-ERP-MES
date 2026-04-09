

namespace MESService.Implement
{
    public class C14Service : BaseService<torderlist, ItorderlistRepository>
    , IC14Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public C14Service(ItorderlistRepository torderlistRepository,
            IWebHostEnvironment webHostEnvironment) : base(torderlistRepository)
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CB_FCTRY1 = BaseParameter.ListSearchString[0].ToString();
                            string ComboBox2 = BaseParameter.ListSearchString[2].ToString();
                            string DateTimePicker2 = BaseParameter.ListSearchString[3].ToString();
                            DateTimePicker2 = DateTime.Parse(DateTimePicker2).ToString("yyyy-MM-dd");

                            string FAC_ST_1 = "";
                            if (CB_FCTRY1 == "Factory 1")
                            {
                                FAC_ST_1 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_1 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) > 60";
                            }

                            string CB2_TEXT = ComboBox2;
                            if (CB2_TEXT == "ALL")
                            {
                                CB2_TEXT = "%%";
                            }

                            string sql = @"SELECT 
                                (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14') AS `STAGE`, 
                                `TMMTIN_DATE` AS `DATE`
                                FROM TMMTIN_DMM_LEAD 
                                WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' 
                                AND `TMMTIN_DATE` = '" + DateTimePicker2 + "' " + FAC_ST_1 + @"
                                GROUP BY `TMMTIN_DATE`, `STAGE`
                                HAVING `STAGE` LIKE '" + CB2_TEXT + "'";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CB_FCTRY3 = BaseParameter.ListSearchString[0].ToString();
                            string T3_S1 = BaseParameter.ListSearchString[1].ToString();
                            string T3_S2 = BaseParameter.ListSearchString[2].ToString();
                            string T3_S3 = BaseParameter.ListSearchString[3].ToString();
                            string T3_S4 = BaseParameter.ListSearchString[4].ToString();

                            string CB3_TEXT = T3_S1;
                            string DGV_D1 = DateTime.Parse(T3_S2).ToString("yyyy-MM-dd");
                            string DGV_D2 = CB3_TEXT;

                            string FAC_ST_3 = "";
                            if (CB_FCTRY3 == "Factory 1")
                            {
                                FAC_ST_3 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_3 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) > 60";
                            }

                            if (CB3_TEXT == "ALL")
                            {
                                CB3_TEXT = "%%";
                                DGV_D2 = "ALL";
                            }

                            string sql = @"SELECT `TMMTIN_DSCN_YN` AS `DSCN`,
                                (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14') AS `STAGE`, 
                                `TMMTIN_DATE` AS `DATE`, 
                                `TMMTIN_PART` AS `DJG_CODE`, 
                                (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `PART_NO`,
                                (SELECT `LEAD_SCN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `TYPE`,
                                `TMMTIN_PART_SNP` AS `SNP`, 
                                SUM(`TMMTIN_QTY`) AS `QTY`, 
                                (SELECT `HOOK_RACK` FROM trackmaster WHERE trackmaster.LEAD_NO = (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`)) AS `LOC`, 
                                IFNULL(SUM(`TMMTIN_QTY`) / `TMMTIN_PART_SNP`, 0) AS `BOX`,
                                `TMMTIN_DMM_IDX` AS `CODE`, 
                                `TMMTIN_SHEETNO`
                                FROM TMMTIN_DMM_LEAD 
                                WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'Y' AND NOT(`TMMTIN_DSCN_YN` = 'Y') 
                                AND `TMMTIN_DATE` = '" + DGV_D1 + "' " + FAC_ST_3 + @"
                                GROUP BY `STAGE`, `DATE`, `PART_NO`
                                HAVING `STAGE` LIKE '" + CB3_TEXT + @"' 
                                AND `TMMTIN_SHEETNO` LIKE '%" + T3_S4 + @"%' 
                                AND (`PART_NO` LIKE '%" + T3_S3 + @"%' OR `TYPE` LIKE '%" + T3_S3 + "%')";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView4 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CB_FCTRY2 = BaseParameter.ListSearchString[0].ToString();
                            string T2_S1 = BaseParameter.ListSearchString[1].ToString();
                            string T2_S2 = BaseParameter.ListSearchString[2].ToString();
                            string T2_S3 = BaseParameter.ListSearchString[3].ToString();
                            string T2_S4 = BaseParameter.ListSearchString[4].ToString();
                            string T2_S5 = BaseParameter.ListSearchString[5].ToString();
                            string T2_S6 = BaseParameter.ListSearchString[6].ToString();

                            string FAC_ST_2 = "";
                            if (CB_FCTRY2 == "Factory 1")
                            {
                                FAC_ST_2 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_2 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14'), 2) > 60";
                            }

                            string T2_CB_TEXT = "";
                            if (T2_S1 == "ALL")
                            {
                                T2_CB_TEXT = "%%";
                            }
                            else
                            {
                                T2_CB_TEXT = T2_S1;
                            }

                            string CB_DATE = "";
                            if (T2_S6 == "ALL") CB_DATE = "";
                            if (T2_S6 == "Y") CB_DATE = " AND `TMMTIN_DSCN_YN` = 'Y'";
                            if (T2_S6 == "N") CB_DATE = " AND `TMMTIN_DSCN_YN` = 'N'";

                            string sql = "";
                            if (BaseParameter.CheckBox1 == false)
                            {
                                T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM-dd");

                                sql = @"SELECT 
                                    `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, 
                                    (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14') AS `STAGE`, 
                                    (SELECT `LEAD_SCN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `TYPE`,
                                    `TMMTIN_PART` AS `DJG_CODE`, 
                                    (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `PART_NO`,
                                    `TMMTIN_PART_SNP` AS `SNP`, 
                                    `TMMTIN_QTY` AS `QTY`, 
                                    `CREATE_DTM`,
                                    IF(`TMMTIN_REC_YN` = 'Y', 'ORDER', 'CANCEL') AS `ORDER`, 
                                    `TMMTIN_DMM_IDX` AS `CODE`, 
                                    `TMMTIN_DATE` AS `DATE`, 
                                    `TMMTIN_SHEETNO`
                                    FROM TMMTIN_DMM_LEAD 
                                    WHERE `TMMTIN_DATE` = '" + T2_S2 + "'" + CB_DATE + FAC_ST_2 + @"
                                    HAVING `STAGE` LIKE '" + T2_CB_TEXT + @"' 
                                    AND `TMMTIN_SHEETNO` LIKE '%" + T2_S5 + @"%' 
                                    AND `PART_NO` LIKE '%" + T2_S3 + @"%' 
                                    AND `TYPE` LIKE '%" + T2_S4 + @"%'
                                    ORDER BY IF(`TMMTIN_DSCN_YN` = 'N', 1, IF(`TMMTIN_DSCN_YN` = 'C', 2, 3)) ASC";
                            }
                            else
                            {
                                T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM");

                                sql = @"SELECT 
                                    `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, 
                                    `TMMTIN_PART` AS `DJG_CODE`, 
                                    'ALL STAGE' AS `STAGE`, 
                                    (SELECT `LEAD_SCN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `TYPE`, 
                                    (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `PART_NO`,
                                    `TMMTIN_PART_SNP` AS `SNP`, 
                                    SUM(`TMMTIN_QTY`) AS `QTY`, 
                                    '' AS `CREATE_DTM`, 
                                    '' AS `ORDER`, 
                                    '' AS `CODE`, 
                                    '' AS `DATE`, 
                                    '' AS `TMMTIN_SHEETNO`
                                    FROM TMMTIN_DMM_LEAD 
                                    WHERE `TMMTIN_DATE` >= '" + T2_S2 + "-01' AND `TMMTIN_DATE` <= '" + T2_S2 + @"-31'
                                    " + CB_DATE + FAC_ST_2 + @"
                                    GROUP BY `TMMTIN_PART`, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`
                                    HAVING `STAGE` LIKE '" + T2_CB_TEXT + @"' 
                                    AND `TMMTIN_SHEETNO` LIKE '%" + T2_S5 + @"%' 
                                    AND `PART_NO` LIKE '%" + T2_S3 + @"%' 
                                    AND `TYPE` LIKE '%" + T2_S4 + @"%'
                                    ORDER BY IF(`TMMTIN_DSCN_YN` = 'N', 1, IF(`TMMTIN_DSCN_YN` = 'C', 2, 3)) ASC";
                            }

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.T2_DGV1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.T2_DGV1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.DataGridView3 != null && BaseParameter.DataGridView3.Count > 0)
                        {
                            string DGV2_D1 = BaseParameter.ListSearchString[0].ToString();
                            string DGV2_D2 = BaseParameter.ListSearchString[1].ToString();

                            // Lấy SHEET_NO mới
                            string sql = @"SELECT IFNULL(MAX(`TMMTIN_SHEETNO`),0) + 1 AS `SHEET_NO` FROM `TMMTIN_DMM_LEAD` WHERE `TMMTIN_DATE` = '" + DGV2_D1 + "'";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            var DGV_C14_NO = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                DGV_C14_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            if (DGV_C14_NO != null && DGV_C14_NO.Count > 0)
                            {
                                string SHEET_NO = DGV_C14_NO[0].SHEET_NO.ToString();
                                SuperResultTranfer DataGridView3Item = new SuperResultTranfer();

                                StringBuilder Detail = new StringBuilder();
                                int no = 0;

                                // Lặp qua các dòng được chọn
                                foreach (var item in BaseParameter.DataGridView3)
                                {
                                    if (item.CHK == true)
                                    {
                                        no++;
                                        if (no == 1)
                                        {
                                            DataGridView3Item = item;
                                        }

                                        var DJG_CODEIDX = item.DJG_CODE;
                                        sql = @"UPDATE `TMMTIN_DMM_LEAD` SET `TMMTIN_CNF_YN`='Y', `TMMTIN_SHEETNO` = '" + SHEET_NO + @"' 
                                            WHERE `TMMTIN_DMM_STGC` = (SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` = '" + DGV2_D2 + @"' AND `CDGR_IDX` = '14')
                                            AND `TMMTIN_DATE` = '" + DGV2_D1 + @"' AND `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_PART` = '" + DJG_CODEIDX + "'";

                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        Detail.AppendLine(@"<tr>");
                                        Detail.AppendLine(@"<td>" + no + "</td>");
                                        Detail.AppendLine(@"<td>" + item.PART_NO + "</td>");
                                        Detail.AppendLine(@"<td>" + item.SNP + "</td>");
                                        Detail.AppendLine(@"<td>" + item.QTY + "</td>");
                                        Detail.AppendLine(@"<td></td>");
                                        Detail.AppendLine(@"<td>" + item.LOC + "</td>");
                                        Detail.AppendLine(@"<td></td>");
                                        Detail.AppendLine(@"<td>" + item.BOX + " Boxes</td>");
                                        try
                                        {
                                            Detail.AppendLine(@"<td>" + item.DATE.Value.ToString("yyyy-MM-dd") + "</td>");
                                        }
                                        catch
                                        {
                                            Detail.AppendLine(@"<td></td>");
                                        }
                                        Detail.AppendLine(@"</tr>");
                                    }
                                }

                                // Tạo report
                                string SheetName = this.GetType().Name;
                                string contentHTML = GlobalHelper.InitializationString;
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "C14.html");
                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                {
                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        contentHTML = r.ReadToEnd();
                                    }
                                }

                                contentHTML = contentHTML.Replace(@"[UserCode]", BaseParameter.USER_ID);
                                contentHTML = contentHTML.Replace(@"[UserName]", BaseParameter.USER_NM);
                                contentHTML = contentHTML.Replace(@"[STAGE]", DataGridView3Item.STAGE);
                                contentHTML = contentHTML.Replace(@"[TMMTIN_SHEETNO]", SHEET_NO);
                                contentHTML = contentHTML.Replace(@"[DATE]", DataGridView3Item.DATE.Value.ToString("yyyy-MM-dd"));
                                contentHTML = contentHTML.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                contentHTML = contentHTML.Replace(@"[Detail]", Detail.ToString());

                                string fileName = "C14_" + GlobalHelper.InitializationDateTimeCode + ".html";
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
                    }
                    else if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DataGridView4 != null)
                        {
                            foreach (var item in BaseParameter.DataGridView4)
                            {
                                if (item.CHK == true)
                                {
                                    var ORDER_CD = item.CODE;
                                    string sql = @"UPDATE `TMMTIN_DMM_LEAD` SET `TMMTIN_DSCN_YN` = 'Y' WHERE `TMMTIN_DMM_IDX` = '" + ORDER_CD + "'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
                        }
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
                if (BaseParameter != null && BaseParameter.DataGridView4 != null)
                {
                    foreach (var item in BaseParameter.DataGridView4)
                    {
                        if (item.CHK == true)
                        {
                            var ORDER_CD = item.CODE;
                            string sql = @"UPDATE `TMMTIN_DMM_LEAD` SET `TMMTIN_DSCN_YN` = 'C' WHERE `TMMTIN_DMM_IDX` = '" + ORDER_CD + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                }
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
                if (BaseParameter != null && BaseParameter.Action == 2 && BaseParameter.DataGridView4 != null)
                {
                    string SheetName = this.GetType().Name;
                    string contentHTML = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "C14.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            contentHTML = r.ReadToEnd();
                        }
                    }

                    SuperResultTranfer DataGridView4Item = BaseParameter.DataGridView4[0];
                    contentHTML = contentHTML.Replace(@"[UserCode]", BaseParameter.USER_ID);
                    contentHTML = contentHTML.Replace(@"[UserName]", BaseParameter.USER_NM);
                    contentHTML = contentHTML.Replace(@"[STAGE]", DataGridView4Item.STAGE);
                    contentHTML = contentHTML.Replace(@"[TMMTIN_SHEETNO]", DataGridView4Item.TMMTIN_SHEETNO.ToString());
                    contentHTML = contentHTML.Replace(@"[DATE]", DataGridView4Item.DATE.Value.ToString("yyyy-MM-dd"));
                    contentHTML = contentHTML.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                    StringBuilder Detail = new StringBuilder();
                    int no = 0;
                    foreach (var item in BaseParameter.DataGridView4)
                    {
                        no++;
                        Detail.AppendLine(@"<tr>");
                        Detail.AppendLine(@"<td>" + no + "</td>");
                        Detail.AppendLine(@"<td>" + item.PART_NO + "</td>");
                        Detail.AppendLine(@"<td>" + item.SNP + "</td>");
                        Detail.AppendLine(@"<td>" + item.QTY + "</td>");
                        Detail.AppendLine(@"<td></td>");
                        Detail.AppendLine(@"<td>" + item.LOC + "</td>");
                        Detail.AppendLine(@"<td></td>");
                        Detail.AppendLine(@"<td>" + item.BOX + " Boxes</td>");
                        try
                        {
                            Detail.AppendLine(@"<td>" + item.DATE.Value.ToString("yyyy-MM-dd") + "</td>");
                        }
                        catch
                        {
                            Detail.AppendLine(@"<td></td>");
                        }
                        Detail.AppendLine(@"</tr>");
                    }

                    contentHTML = contentHTML.Replace(@"[Detail]", Detail.ToString());
                    string fileName = "C14_" + GlobalHelper.InitializationDateTimeCode + ".html";
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

        public virtual async Task<BaseResult> CB_FCTRY_LINE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                             FROM TSCODE WHERE TSCODE.CDGR_IDX = '21'"; DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.CB_FCTRY1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.CB_FCTRY1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> COMLIST_LINE_1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                 FROM TSCODE 
                                 WHERE `CDGR_IDX` = '14' AND NOT(`CD_SYS_NOTE` = 'MP GROUP 00') "
                                 + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.T2_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.T2_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> COMLIST_LINE_2(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                 FROM TSCODE 
                                 WHERE `CDGR_IDX` = '14' AND NOT(`CD_SYS_NOTE` = 'MP GROUP 00') "
                                 + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox2 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> COMLIST_LINE_3(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                 FROM TSCODE 
                                 WHERE `CDGR_IDX` = '14' AND NOT(`CD_SYS_NOTE` = 'MP GROUP 00') "
                                 + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.T3_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.T3_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> DataT2DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string DGV_D1 = BaseParameter.ListSearchString[0].ToString();
                    string DGV_D2 = BaseParameter.ListSearchString[1].ToString();

                    string sql = @"SELECT `TMMTIN_DMM_IDX` AS `CODE`,
                        (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14') AS `STAGE`, 
                        `TMMTIN_DATE` AS `DATE`, 
                        `TMMTIN_PART` AS `DJG_CODE`, 
                        (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `PART_NO`,
                        (SELECT `LEAD_SCN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `TYPE`,
                        `TMMTIN_PART_SNP` AS `SNP`, 
                        SUM(`TMMTIN_QTY`) AS `QTY`,
                        IFNULL((SELECT tiivtr_lead.QTY FROM tiivtr_lead WHERE tiivtr_lead.PART_IDX = TMMTIN_DMM_LEAD.`TMMTIN_PART` AND tiivtr_lead.LOC_IDX='3'), 0) AS `STOCK`,
                        (SELECT `HOOK_RACK` FROM trackmaster WHERE trackmaster.LEAD_NO = (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`)) AS `LOC`,
                        IFNULL(SUM(`TMMTIN_QTY`) / `TMMTIN_PART_SNP`, 0) AS `BOX`
                        FROM TMMTIN_DMM_LEAD
                        WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_DATE` = '" + DGV_D1 + @"'
                        GROUP BY `STAGE`, `DATE`, `PART_NO`
                        HAVING `STAGE` = '" + DGV_D2 + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView3 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
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