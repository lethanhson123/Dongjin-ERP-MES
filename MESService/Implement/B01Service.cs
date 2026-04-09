namespace MESService.Implement
{
    public class B01Service : BaseService<torderlist, ItorderlistRepository>
    , IB01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B01Service(ItorderlistRepository torderlistRepository

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
                if (BaseParameter.ListB01 != null)
                {
                    result.ListB01 = BaseParameter.ListB01;
                    string VALUES = GlobalHelper.InitializationString;
                    string VALUESSUM = GlobalHelper.InitializationString;

                    int count = GlobalHelper.InitializationNumber;
                    int counterr = GlobalHelper.InitializationNumber;

                    string sql = @"SELECT `PART_NO`, `PART_IDX`, `PART_SNP`  FROM  tspart";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_B01_01 = new List<tspart>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_B01_01.AddRange(SQLHelper.ToList<tspart>(dt));
                    }
                    result.SaveSuccessNumber = GlobalHelper.InitializationNumber;
                    result.SaveNotSuccessNumber = GlobalHelper.InitializationNumber;
                    for (int i = 0; i < result.ListB01.Count; i++)
                    {
                        B01 item = result.ListB01[i];
                        if (item.dgvcheck == true)
                        {
                            bool IsSave = false;
                            tspart tspart = result.DGV_B01_01.Where(o => o.PART_NO == item.PARTNO).FirstOrDefault();
                            if (tspart != null)
                            {
                                if (!string.IsNullOrEmpty(tspart.PART_NO))
                                {
                                    IsSave = true;
                                }
                            }
                            if (IsSave == true)
                            {

                                string UTM = item.UNIT.Replace(@"'", @"");
                                string DESC = item.GOODS.Replace(@"'", @"");
                                int QTY = item.QUANTITY.Value;
                                string NET_WT = item.NWEIGHT.Replace(@"'", @"");
                                string GRS_WT = item.GWEIGHT.Replace(@"'", @"");
                                string PLET_NO = item.PalletNo.Replace(@"'", @"");
                                string SHPD_NO = item.ShippedNo.Replace(@"'", @"");
                                string MTIN_DTM = item.inputdate.Replace(@"'", @"");
                                string DSCN_YN = "N";
                                string CREATE_USER = BaseParameter.USER_IDX;
                                int partno = tspart.PART_IDX.Value;
                                int PSRT_SNP = tspart.PART_SNP.Value;
                                if (PSRT_SNP == 0)
                                {
                                    PSRT_SNP = QTY;
                                }
                                if (string.IsNullOrEmpty(NET_WT))
                                {
                                    NET_WT = "" + GlobalHelper.InitializationNumber;
                                }
                                if (string.IsNullOrEmpty(GRS_WT))
                                {
                                    GRS_WT = "" + GlobalHelper.InitializationNumber;
                                }

                                VALUES = "('" + partno + "', '" + UTM + "', '" + DESC + "' , " + QTY + ", " + NET_WT + ", " + GRS_WT + ", '" + PLET_NO + "', '" + SHPD_NO + "', '" + MTIN_DTM + "', '" + DSCN_YN + "', NOW(), '" + CREATE_USER + "', '" + PSRT_SNP + "')";
                                if (string.IsNullOrEmpty(VALUESSUM))
                                {
                                    VALUESSUM = VALUES;
                                }
                                else
                                {
                                    VALUESSUM = VALUESSUM + ", " + VALUES;
                                }
                                result.ListB01[i].dgvcheck = true;
                                result.SaveSuccessNumber = result.SaveSuccessNumber + 1;
                            }
                            else
                            {
                                result.ListB01[i].dgvcheck = false;
                                result.SaveNotSuccessNumber = result.SaveNotSuccessNumber + 1;
                            }
                        }
                    }
                    sql = "INSERT INTO TMMTIN (`PART_IDX`, `UTM`, `DESC`, `QTY`, `NET_WT`, `GRS_WT`, `PLET_NO`, `SHPD_NO`, `MTIN_DTM`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `SNP_QTY`) VALUES " + VALUESSUM;

                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                string SheetName = this.GetType().Name;
                string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationToExcelAsync(BaseParameter.ListB01, streamExport, SheetName);
                var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
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
        private void InitializationToExcelAsync(List<B01> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;

                Type temp = typeof(tspartTranfer);
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    workSheet.Cells[row, column].Value = pro.Name;
                    column = column + 1;
                }

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

                row = row + 1;

                int stt = 1;
                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = stt;
                        workSheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        int columnSub = 2;
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            workSheet.Cells[row, columnSub].Value = pro.GetValue(item);
                            columnSub = columnSub + 1;
                        }

                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        stt = stt + 1;
                        row = row + 1;
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

