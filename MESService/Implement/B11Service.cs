
namespace MESService.Implement
{
    public class B11Service : BaseService<torderlist, ItorderlistRepository>
    , IB11Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B11Service(ItorderlistRepository torderlistRepository
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var A01 = BaseParameter.ListSearchString[0];
                        var A02 = BaseParameter.ListSearchString[1];
                        var A03 = BaseParameter.ListSearchString[2];
                        var A04 = BaseParameter.ListSearchString[3];
                        var A05 = BaseParameter.ListSearchString[4];
                        var A06 = BaseParameter.ListSearchString[5];
                        var A07 = BaseParameter.ListSearchString[6];
                        var A08 = BaseParameter.ListSearchString[7];
                        var A09 = BaseParameter.ListSearchString[8];
                        var A10 = BaseParameter.ListSearchString[9];
                        var A11 = BaseParameter.ListSearchString[10];
                        if (string.IsNullOrEmpty(A01))
                        {
                            A01 = "%%";
                        }
                        if (string.IsNullOrEmpty(A03))
                        {
                            A03 = "";
                        }
                        else
                        {
                            A03 = "  AND `SNP` = '" + A03 + "' ";
                        }
                        A04 = DateTime.Parse(A04).ToString("yyyy-MM-dd");
                        if (A07 == "ALL")
                        {
                            A07 = "";
                        }
                        if (A08 == "ALL")
                        {
                            A08 = "";
                        }

                        string sql = @"SELECT
(SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX) AS `PART_NO`, 
TMMTIN.`DESC`, IFNULL(TMMTIN.SNP_QTY, (SELECT tspart.`PART_SNP` FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX)) AS `SNP`,
TMMTIN.`MTIN_DTM`,
TMBRCD.`PKG_GRP_IDX`, TMBRCD.`BARCD_ID`, TMBRCD.`PKG_GRP`, TMBRCD.`DSCN_YN`,
TMBRCD.`PKG_QTY`, (SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX) AS `PART_LOC`, 
TMBRCD.`PKG_OUTQTY`, (TMBRCD.`PKG_QTY` - TMBRCD.`PKG_OUTQTY`) AS `QTY`,
TMMTIN.`PLET_NO`, TMMTIN.`SHPD_NO`, 
(SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = TMMTIN.PART_IDX AND tiivtr.LOC_IDX ='1') AS `STOCK`

FROM TMBRCD
LEFT JOIN TMMTIN
ON TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX

WHERE NOT(TMBRCD.DSCN_YN ='Y')  AND TMBRCD.BBCO = 'Y'

     AND TMMTIN.`MTIN_DTM` <= '" + A04 + "' AND TMBRCD.`PKG_GRP_IDX` LIKE '%" + A05 + "%' AND TMBRCD.`BARCD_ID` LIKE '%" + A06 + "%' AND TMBRCD.`PKG_GRP` LIKE '%" + A07 + "%' AND TMBRCD.`DSCN_YN`  LIKE '%" + A08 + "%' AND TMMTIN.`PLET_NO` LIKE '%" + A09 + "%' AND TMMTIN.`SHPD_NO` LIKE '%" + A10 + "%' HAVING  `PART_NO` LIKE '" + A01 + "' AND `DESC` LIKE '%" + A02 + "%' AND `PART_LOC` LIKE '%" + A11 + "%' " + A03 + "  ORDER BY `PART_NO`, `MTIN_DTM` DESC ";

                        sql = sql + " LIMIT 1000";
                        //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var A01 = BaseParameter.ListSearchString[0];
                        var A02 = BaseParameter.ListSearchString[1];
                        var A03 = BaseParameter.ListSearchString[2];
                        var A04 = BaseParameter.ListSearchString[3];
                        var A05 = BaseParameter.ListSearchString[4];
                        var A06 = BaseParameter.ListSearchString[5];
                        var A07 = BaseParameter.ListSearchString[6];
                        var A08 = BaseParameter.ListSearchString[7];
                        var A09 = BaseParameter.ListSearchString[8];
                        var A10 = BaseParameter.ListSearchString[9];
                        var A11 = BaseParameter.ListSearchString[10];
                        if (string.IsNullOrEmpty(A01))
                        {
                            A01 = "%%";
                        }
                        if (string.IsNullOrEmpty(A03))
                        {
                            A03 = "";
                        }
                        else
                        {
                            A03 = "  AND `SNP` = '" + A03 + "' ";
                        }
                        A04 = DateTime.Parse(A04).ToString("yyyy-MM-dd");
                        if (A07 == "ALL")
                        {
                            A07 = "";
                        }
                        if (A08 == "ALL")
                        {
                            A08 = "";
                        }

                        string sql = @"SELECT
(SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX) AS `PART_NO`, 
TMMTIN.`DESC`, IFNULL(TMMTIN.SNP_QTY, (SELECT tspart.`PART_SNP` FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX)) AS `SNP`,
TMMTIN.`MTIN_DTM`,
TMBRCD.`PKG_GRP_IDX`, TMBRCD.`BARCD_ID`, TMBRCD.`PKG_GRP`, TMBRCD.`DSCN_YN`,
TMBRCD.`PKG_QTY`, (SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.PART_IDX =  TMMTIN.PART_IDX) AS `PART_LOC`, 
TMBRCD.`PKG_OUTQTY`, (TMBRCD.`PKG_QTY` - TMBRCD.`PKG_OUTQTY`) AS `QTY`,
TMMTIN.`PLET_NO`, TMMTIN.`SHPD_NO`, 
(SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = TMMTIN.PART_IDX AND tiivtr.LOC_IDX ='1') AS `STOCK`

FROM TMBRCD
LEFT JOIN TMMTIN
ON TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX

WHERE NOT(TMBRCD.DSCN_YN ='Y')  AND TMBRCD.BBCO = 'Y'

     AND TMMTIN.`MTIN_DTM` <= '" + A04 + "' AND TMBRCD.`PKG_GRP_IDX` LIKE '%" + A05 + "%' AND TMBRCD.`BARCD_ID` LIKE '%" + A06 + "%' AND TMBRCD.`PKG_GRP` LIKE '%" + A07 + "%' AND TMBRCD.`DSCN_YN`  LIKE '%" + A08 + "%' AND TMMTIN.`PLET_NO` LIKE '%" + A09 + "%' AND TMMTIN.`SHPD_NO` LIKE '%" + A10 + "%' HAVING  `PART_NO` LIKE '" + A01 + "' AND `DESC` LIKE '%" + A02 + "%' AND `PART_LOC` LIKE '%" + A11 + "%' " + A03 + "  ORDER BY `PART_NO`, `MTIN_DTM` DESC ";



                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        string SheetName = this.GetType().Name;
                        string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                        var streamExport = new MemoryStream();
                        InitializationToExcelAsync(result.DataGridView1, streamExport, SheetName);
                        string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        string filePath = Path.Combine(physicalPathCreate, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            streamExport.CopyTo(stream);
                        }
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
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
        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;

                workSheet.Cells[row, column].Value = "PART_NO";
                column = column + 1;

                workSheet.Cells[row, column].Value = "PART_NAME";
                column = column + 1;

                workSheet.Cells[row, column].Value = "SNP";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Top Barcode";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Type";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Condition";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Packing Qty";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;

                workSheet.Cells[row, column].Value = "OUT_QTY";
                column = column + 1;

                workSheet.Cells[row, column].Value = "QTY";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Pallet NO";
                column = column + 1;

                workSheet.Cells[row, column].Value = "Shipping NO";
                column = column + 1;

                workSheet.Cells[row, column].Value = "STOCK";
                column = column + 1;



                for (int i = 1; i < column; i++)
                {
                    workSheet.Cells[row, i].Style.Font.Bold = true;
                    workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, i].Style.Font.Size = 14;
                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                int stt = 1;
                foreach (var item in list)
                {
                    row = row + 1;
                    stt = stt + 1;
                    workSheet.Cells[row, 1].Value = stt;
                    try
                    {

                        workSheet.Cells[row, 2].Value = item.PART_NO;
                        workSheet.Cells[row, 3].Value = item.DESC;
                        workSheet.Cells[row, 4].Value = item.SNP;
                        workSheet.Cells[row, 5].Value = item.MTIN_DTM.Value.ToString("yyyy-MM-dd");
                        workSheet.Cells[row, 6].Value = item.PKG_GRP_IDX;
                        workSheet.Cells[row, 7].Value = item.BARCD_ID;
                        workSheet.Cells[row, 8].Value = item.PKG_GRP;
                        workSheet.Cells[row, 9].Value = item.DSCN_YN;
                        workSheet.Cells[row, 10].Value = item.PKG_QTY;
                        workSheet.Cells[row, 11].Value = item.PART_LOC;
                        workSheet.Cells[row, 12].Value = item.PKG_OUTQTY;
                        workSheet.Cells[row, 13].Value = item.QTY;
                        workSheet.Cells[row, 14].Value = item.PLET_NO;
                        workSheet.Cells[row, 15].Value = item.SHPD_NO;
                        workSheet.Cells[row, 16].Value = item.STOCK;
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }

                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
    }
}

