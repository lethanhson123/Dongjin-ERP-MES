namespace MESService.Implement
{
    public class D15Service : BaseService<torderlist, ItorderlistRepository>
    , ID15Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D15Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
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
                string sql = @"SELECT (IFNULL(MAX(`SEQ_NO`),0)+1) AS SEQ_NO  FROM tdpdmtin_serial WHERE `TDPD_DATE`= @CurentDate  AND Serial_no LIKE '%KGM%'";
                var parameters = new[] { new MySqlParameter("@CurentDate", MySqlDbType.VarChar) { Value = DateTime.Now.ToString("yyyy-MM-dd") } };
                string number = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                result.ErrorNumber = 0;
                result.BoxNumber = number;
                result.DATEString = DateTime.Now.ToString("yyyy-MM-dd");
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

                if (!string.IsNullOrEmpty(BaseParameter.Barcode))
                {
                    string QRCode = CleanStringProperties(BaseParameter.Barcode.ToUpper().Trim());
                    if (BaseParameter.Code == "LoadPackingList")
                    {
                        //lấy về thông tin toàn bộ packing list
                        result = await LoadPackingList(QRCode);
                    }
                    else
                    {

                    }

                }
                else
                {
                    if (BaseParameter.Code == "LoadHistory")
                    {
                        // thúc hiện tìm kiếm thông tin thông thường
                        result = await LoadHistory(BaseParameter);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> LoadHistory(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            string ADATE = baseParameter.FromDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string BDATE = baseParameter.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");

            string LPARKNO = "%" + baseParameter.PartNo.Replace("'", "") + "%";
            string SERIALID = "%" + baseParameter.PackingLotCode.Replace("'", "") + "%";
            string TBPART_NM = "%" + baseParameter.PartName.Replace("'", "") + "%";
            string TBPART_CLOG = "%" + baseParameter.LeadData.ToString().Replace("'", "") + "%";
            string query = "";

            if (baseParameter.FilterType == "Y")
            {
                query = $@"SELECT mainTB.* From
                           ( SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM,
                            null AS DeleteTime,
                          null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}' and tdpdmtim.VLID_DSCN_YN ='Y') as mainTB
                        WHERE mainTB.`PKG_GRP` LIKE '{SERIALID}' 
                              AND   mainTB.`PART_NO` LIKE '{LPARKNO}'  
                              AND   mainTB.`PART_CAR` LIKE '{TBPART_CLOG}' 
                              AND    mainTB.`PART_NM` LIKE '{TBPART_NM}'    
                        ORDER BY  mainTB.`MTIN_DTM` DESC, mainTB.`PART_NO`";
            }

            else if (baseParameter.FilterType == "N")
            {
                query = $@"SELECT mainTB.* From
                           ( SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM,
                            null AS DeleteTime,
                          null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}') as mainTB
                        WHERE mainTB.`PKG_GRP` LIKE '{SERIALID}' 
                              AND   mainTB.`PART_NO` LIKE '{LPARKNO}'  
                              AND   mainTB.`PART_CAR` LIKE '{TBPART_CLOG}' 
                              AND    mainTB.`PART_NM` LIKE '{TBPART_NM}'    
                        ORDER BY  mainTB.`MTIN_DTM` DESC, mainTB.`PART_NO`, mainTB.PKG_GRP ";

            }

            else if (baseParameter.FilterType == "D")
            {

                query = $@"SELECT mainTB.* From
                   ( SELECT 
                    B.PART_NO,
                    B.PART_NM,
                    B.PART_CAR,
                    B.PART_FML,
                    A.VLID_PART_SNP AS SNP_QTY,
                    A.VLID_GRP AS PKG_GRP,
                    C.QTY,
                    D.VLID_DTM AS MTIN_DTM,
                    D.CREATE_DTM,
                    TRIM(A.VLID_BARCODE) AS LOT_CODE,
                    A.VLID_REMARK AS REMARK,
                    D.CREATE_USER,
                    'D' AS DSCN_YN,
                    A.PDMTIN_IDX,
                    A.VLID_PART_IDX,                  
                    A.CREATE_DTM AS DeleteTime,
                    A.CREATE_USER AS DeleteBy
                FROM tdpdmtim_del AS A
                JOIN tspart AS B ON A.VLID_PART_IDX = B.PART_IDX
                LEFT JOIN tiivtr AS C ON A.VLID_PART_IDX = C.PART_IDX AND C.LOC_IDX = '2'
                JOIN tdpdmtim_hist AS D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
                                        AND A.VLID_GRP = D.VLID_GRP 
                                        AND A.VLID_BARCODE = D.VLID_BARCODE
                WHERE 
                     A.CREATE_DTM BETWEEN '{ADATE}' AND '{BDATE}' ) as mainTB
                Where mainTB.PART_NO LIKE '{LPARKNO}'
                    AND mainTB.PART_CAR LIKE '{TBPART_CLOG}'
                    AND mainTB.PART_NM LIKE '{TBPART_NM}'
                    AND mainTB.PKG_GRP LIKE '{SERIALID}'
                ORDER BY mainTB.MTIN_DTM DESC, mainTB.PART_NO, mainTB.PKG_GRP ";
            }
            else
            {

                query = $@"SELECT mainTB.* FROM 
                        (SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM ,
                            null AS DeleteTime,
                          null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}'

                        UNION

                        SELECT 
                            B.PART_NO, B.PART_NM, B.PART_CAR, B.PART_FML,
                            A.VLID_PART_SNP AS SNP_QTY, A.VLID_GRP AS PKG_GRP, C.QTY,
                            D.VLID_DTM AS MTIN_DTM, TRIM(A.VLID_BARCODE) AS LOT_CODE, A.VLID_REMARK AS REMARK,
                            D.CREATE_USER, 'D' AS DSCN_YN, A.PDMTIN_IDX,  A.VLID_PART_IDX,
                            D.CREATE_DTM,  A.CREATE_DTM AS DeleteTime, A.CREATE_USER AS DeleteBy
                        FROM tdpdmtim_del AS A
                        JOIN tspart AS B ON A.VLID_PART_IDX = B.PART_IDX
                        LEFT JOIN tiivtr AS C ON A.VLID_PART_IDX = C.PART_IDX AND C.LOC_IDX = '2'
                        JOIN tdpdmtim_hist AS D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
                                                AND A.VLID_GRP = D.VLID_GRP 
                                                AND A.VLID_BARCODE = D.VLID_BARCODE
                        WHERE 
                           A.CREATE_DTM BETWEEN '{ADATE}' AND '{BDATE}'
                            ) AS mainTB
                        WHERE mainTB.PART_NO LIKE '{LPARKNO}'
                            AND mainTB.PART_CAR LIKE '{TBPART_CLOG}'
                            AND mainTB.PART_NM LIKE '{TBPART_NM}'
                            AND mainTB.PKG_GRP LIKE '{SERIALID}' 
                        ORDER BY mainTB.MTIN_DTM DESC, mainTB.PART_NO,mainTB.PKG_GRP";

            }


            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
            result.ListtspartTranfer = new List<tspartTranfer>();

            foreach (DataTable dt in ds.Tables)
            {
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt));
            }

            return result;
        }


        /// <summary>
        /// xóa bỏ các ký tự thùa của Scanner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        private static string CleanStringProperties(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return new string(input.Where(c => !char.IsControl(c)).ToArray()).Trim();
        }



        /// <summary>
        /// trả về toàn bọ danh sách mã hàng đã scan trong thùng hàng
        /// </summary>
        /// <param name="QRCode">biến QR code của thùng hàng hoạc mã RO của một con hàng bên trong thùng</param>
        /// <returns></returns>
        private async Task<BaseResult> LoadPackingList(string QRCode)
        {
            BaseResult result = new BaseResult();
            string sql = "";
            string packingLot = "";
            QRCode = CleanStringProperties(QRCode);
            //tìm kiếm theo mã ID Card
            if (QRCode.Contains("3S") && QRCode.Contains("KGM"))
            {
                // lấy thông tin lịch sử scan hàng bang mã packing lot
                sql = @"Select * FROM  tdpdmtim  WHERE tdpdmtim.`VLID_GRP` = '" + QRCode + "'";
            }
            else
            {
                // tìm theo mã sản phẩm lotcode bất kỳ
                sql = @"Select VLID_GRP FROM  tdpdmtim  WHERE tdpdmtim.`VLID_BARCODE` = @QRCode";
                var parameters = new[] { new MySqlParameter("@QRCode", MySqlDbType.VarChar) { Value = QRCode } };
                packingLot = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                //truyền chuỗi truy vấn thành truy vấn theo mã packing lot
                // sql = @"Select * FROM  tdpdmtim  WHERE tdpdmtim.`VLID_GRP` = '" + packingLot.Trim() + "' ORDER BY CREATE_DTM desc";

                sql = @"SELECT * FROM tdpdmtim WHERE VLID_GRP = @PackingLot ORDER BY CREATE_DTM DESC;";

            }

            var parameters1 = new[] { new MySqlParameter("@PackingLot", MySqlDbType.VarChar) { Value = packingLot.Trim() } };
            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters1);
            result.ListtdpdmtimTranfer = new List<tdpdmtimTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtdpdmtimTranfer.AddRange(SQLHelper.ToList<tdpdmtimTranfer>(dt, true)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            if (result.ListtdpdmtimTranfer.Count > 0)
            {
                var part_idx = result.ListtdpdmtimTranfer[0].VLID_PART_IDX;
                var parking_lot = result.ListtdpdmtimTranfer[0].VLID_GRP.Trim();

                string sqlTSPart = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO` ,  tspart.`PART_NM`,  tspart.`PART_CAR` ,tspart.`PART_FML`, tspart.`PART_SNP`, 
                                    (SELECT TSCODE.`CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `PART_SCN`, 
                                    IFNULL((SELECT tiivtr.`QTY` FROM tiivtr WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX` AND tiivtr.`LOC_IDX` = '1'),0) AS `QTY`
                                    FROM tspart WHERE tspart.`PART_USENY` = 'Y' AND tspart.`PART_IDX` = '" + part_idx + "'";

                DataSet tsPart = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlTSPart);
                result.ListtspartTranfer = new List<tspartTranfer>();
                for (int i = 0; i < tsPart.Tables.Count; i++)
                {
                    DataTable dt = tsPart.Tables[i];
                    result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt, true)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
                }

                result.PackingLotCode = parking_lot;
                result.ErrorNumber = 0;
                result.Message = "Packing list result";
            }
            else
            {
                result.ErrorNumber = -1;
                result.Message = "Packing list is empty"; //thùng rỗng không có gì bên trong
            }


            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (!string.IsNullOrEmpty(BaseParameter.Barcode))
                {
                    string barcode = CleanStringProperties(BaseParameter.Barcode.ToUpper().Trim());
                    string partNo = BaseParameter.PartNo.ToUpper().Trim();
                    string IDCard = BaseParameter.PackingLotCode.ToUpper().Trim();
                    string User_ID = BaseParameter.USER_ID.ToUpper().Trim();
                    string TorqueBarcode = BaseParameter.TorqueBarcode.ToUpper().Trim();

                    if (!string.IsNullOrEmpty(IDCard)) //nếu có mã IDCrad gửi về tức là đang scan thêm vào thùng hiện tại vói mã IDCard này
                    {
                        result = await AddMoretoIDCardAsync(barcode, TorqueBarcode, partNo, IDCard, User_ID);
                    }
                    else // thêm mới mã sp vào một mã ID card tự tạo cho người dùng vafluuw vào database
                    {
                        result = await AddNewIDCardAsync(barcode, TorqueBarcode, partNo, User_ID, BaseParameter.CustomerCode, BaseParameter.CreateDate, BaseParameter.BoxNumber);
                    }


                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        private async Task<BaseResult> AddNewIDCardAsync(string barcode, string TorqueBarcode, string partNo, string User_ID, string customerCode, DateTime? CreateDate, int? Boxnumber)
        {
            BaseResult result = new BaseResult();

            // lấy thông tin sản phẩm
            string sql = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO`, tspart.`PART_NM`, tspart.`PART_CAR`, tspart.`PART_FML`, tspart.`PART_SNP`, 
                                    (SELECT TSCODE.`CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `PART_SCN`, 
                                    IFNULL((SELECT tiivtr.`QTY` FROM tiivtr WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX` AND tiivtr.`LOC_IDX` = '1'),0) AS `QTY`
                                   FROM tspart WHERE tspart.`PART_USENY` = 'Y' AND (tspart.`PART_NO` = '" + partNo + "' or  tspart.`PART_SUPL` = '" + partNo + "')";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListtspartTranfer = new List<tspartTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt, true)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
            }
            //kiểm tra nếu mã hàng không tồn tại thì trả về báo lỗi luôn khong cần xử lý các công việc sau đó
            if (result.ListtspartTranfer.Count <= 0)
            {
                result.ErrorNumber = -1;
                result.Message = "Product code does not exist";
                return result;
            }

            // lấy thông tin lịch sử scan hàng
            sql = @"Select * FROM  tdpdmtim  WHERE tdpdmtim.`VLID_BARCODE` = '" + barcode + "'";
            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListtdpdmtimTranfer = new List<tdpdmtimTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtdpdmtimTranfer.AddRange(SQLHelper.ToList<tdpdmtimTranfer>(dt, true)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            //kiêm tra các điều kiện
            if (result.ListtdpdmtimTranfer.Count > 0) // đã được Scan vào trước đó
            {
                if (result.ListtdpdmtimTranfer[0].VLID_DSCN_YN.Trim() == "N")
                {
                    result.ErrorNumber = 1; // đã tồn tại và có thể thêm
                    result.Message = "Exited in stock, not shipped"; // chưa ship hàng đi                                           
                }
                else
                {
                    result.ErrorNumber = 2; // đã tồn tại và không thể thêm
                    result.Message = "Exited in stock, is shipped"; // chưa ship hàng đi    
                }
            }
            else //chưa được Scan vào tức là Scan mới
            {
                var IDCard = await CreatePackingLotAsync(result, customerCode, User_ID, CreateDate, Boxnumber); // tạo mã packing cho các hàng của PKI
                AddtoDatabase(result, barcode, TorqueBarcode, IDCard, User_ID);
                result.PackingLotCode = IDCard;
                result.ErrorNumber = 0; // đã thêm mới
                result.Message = "New Added"; // đã thêm mới
            }

            return result;
        }
        /// <summary>
        /// thục hiện thêm vào database
        /// </summary>
        /// <param name="barcode">mã QR code</param>
        /// <param name="partNo">mã sản phẩm</param>
        /// <param name="iDCard">mã đóng gói</param>
        /// <exception cref="NotImplementedException"></exception>
        private void AddtoDatabase(BaseResult result, string barcode, string TorqueBarcode, string iDCard, string Create_User)
        {
            var part_idx = result.ListtspartTranfer[0].PART_IDX.ToString();
            string stringValues = "(" + part_idx + ", '" + iDCard + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + barcode + "', '" + TorqueBarcode + "', 'N', NOW(), '" + Create_User + "', (SELECT `PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx + "'), IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx + "'), ''))";

            //cập nhật vao danh sách đóng gói
            string sql = "INSERT INTO tdpdmtim (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, `VLID_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`, `BARCD_LOC`) VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);
            //cập nhật vào danhs sách lịch sử scan hàng
            string histSQL = "INSERT INTO tdpdmtim_hist (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, `VLID_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`, `BARCD_LOC`) VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, histSQL);

            //cập nhật vào số lượng tồn kho
            string QtySQL = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES " +
                "(" + part_idx + ", 2, 1 , NOW(), '" + Create_User + "')" +
                " On DUPLICATE KEY UPDATE `LOC_IDX`= 2, `QTY`= `QTY` + 1, `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + Create_User + "'";

            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, QtySQL);

        }

        /// <summary>
        /// thêm tiếp mã sản phẩm vào trong dánh sách đong gói đã có trước đó ( còn thiếu số lượng hàng thì cho phép Scan tiếp)
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="partNo"></param>
        /// <param name="iDCard"></param>
        /// <param name="Create_User"></param>
        /// <returns></returns>
        private async Task<BaseResult> AddMoretoIDCardAsync(string barcode, string torBarcode, string partNo, string IDCard, string Create_User)
        {
            BaseResult result = new BaseResult();

            // lấy thông tin sản phẩm
            string sql = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO`, tspart.`PART_NM`, tspart.`PART_CAR`, tspart.`PART_FML`, tspart.`PART_SNP`, 
                                    (SELECT TSCODE.`CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `PART_SCN`, 
                                    IFNULL((SELECT tiivtr.`QTY` FROM tiivtr WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX` AND tiivtr.`LOC_IDX` = '1'),0) AS `QTY`
                                    FROM tspart WHERE tspart.`PART_USENY` = 'Y' AND (tspart.`PART_NO` = '" + partNo + "' or  tspart.`PART_SUPL` = '" + partNo + "')";


            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListtspartTranfer = new List<tspartTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
            }

            if (result.ListtspartTranfer.Count <= 0)
            {
                result.ErrorNumber = -1;
                result.Message = "Product code does not exist";
                return result;
            }

            // lấy thông tin lịch sử scan hàng
            sql = @"Select * FROM  tdpdmtim  WHERE tdpdmtim.`VLID_BARCODE` = '" + barcode + "'";
            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListtdpdmtimTranfer = new List<tdpdmtimTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtdpdmtimTranfer.AddRange(SQLHelper.ToList<tdpdmtimTranfer>(dt)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            //kiêm tra các điều kiện
            if (result.ListtdpdmtimTranfer.Count > 0) // đã được Scan vào trước đó
            {
                if (result.ListtdpdmtimTranfer[0].VLID_DSCN_YN.Trim() == "N")
                {
                    result.ErrorNumber = 1; // đã tồn tại và có thể thêm
                    result.Message = "Exited in stock, not shipped"; // chưa ship hàng đi                                           
                }
                else
                {
                    result.ErrorNumber = 2; // đã tồn tại và không thể thêm
                    result.Message = "Exited in stock, is shipped"; // chưa ship hàng đi    
                }
            }
            else //chưa được Scan vào tức là Scan mới
            {
                AddtoDatabase(result, barcode, torBarcode, IDCard, Create_User);
                result.PackingLotCode = IDCard;
                result.ErrorNumber = 0; // đã thêm mới
                result.Message = "New Added"; // đã thêm mới
            }

            return result;

        }


        /// <summary>
        /// hàm tạo tự động mã packing lot
        /// </summary>
        /// <param name="result"></param>
        /// <param name="customerCode"></param>
        /// <param name="Create_User"></param>
        /// <returns></returns>
        private async Task<string> CreatePackingLotAsync(BaseResult result, string customerCode, string Create_User, DateTime? createDate, int? BoxNumber)
        {
            var curentDate = DateTime.Now;

            string DATE_DATE = "";

            string sql = @"SELECT (IFNULL(MAX(`SEQ_NO`),0)+1) AS SEQ_NO  FROM tdpdmtin_serial WHERE `TDPD_DATE`= @CurentDate  AND Serial_no LIKE '3S" + customerCode + "%'";
            var parameters = new[] { new MySqlParameter("@CurentDate", MySqlDbType.VarChar) { Value = curentDate.ToString("yyyy-MM-dd") } };
            string number = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, sql, parameters);

            string packingLot = "3SKGM" + BoxNumber.Value.ToString("00") + createDate.Value.ToString("yyMMdd") + (DateTime.Now.Ticks % 1000000).ToString("D6");  // Lấy 6 chữ số cuối

            string sqlStr = "INSERT INTO `tdpdmtin_serial`( `PART_IDX`, `TDPD_DATE`, `SEQ_NO`, `SERIAL_NO`, `CREATE_DTM`, `CREATE_USER`, `CustomerCode`) VALUES " +
                                "('" + result.ListtspartTranfer[0].PART_IDX + "', '" + curentDate.ToString("yyyy-MM-dd") + "', " + number + ", '" + packingLot + "', NOW(), '" + Create_User + "','" + customerCode + "')";

            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sqlStr);

            return packingLot;
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
            if (!string.IsNullOrEmpty(BaseParameter.PartNo) &&
                 !string.IsNullOrEmpty(BaseParameter.Barcode) &&
                 !string.IsNullOrEmpty(BaseParameter.PackingLotCode) &&
                 !string.IsNullOrEmpty(BaseParameter.Remark) &&
                 !string.IsNullOrEmpty(BaseParameter.USER_ID))
            {
                // lấy thông tin sản phẩm
                string sql = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO`, tspart.`PART_NM`, tspart.`PART_CAR`, tspart.`PART_FML`, tspart.`PART_SNP`, 
                                    (SELECT TSCODE.`CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `PART_SCN`, 
                                    IFNULL((SELECT tiivtr.`QTY` FROM tiivtr WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX` AND tiivtr.`LOC_IDX` = '1'),0) AS `QTY`
                                    FROM tspart WHERE tspart.`PART_USENY` = 'Y' AND tspart.`PART_NO` = '" + BaseParameter.PartNo + "'";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ListtspartTranfer = new List<tspartTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt, true)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
                }
                //kiểm tra nếu mã hàng không tồn tại thì trả về báo lỗi luôn khong cần xử lý các công việc sau đó
                if (result.ListtspartTranfer.Count <= 0)
                {
                    result.ErrorNumber = -1;
                    result.Message = "Product code does not exist";
                    return result;
                }

                DeletePacking(result, BaseParameter.Barcode, BaseParameter.PackingLotCode, BaseParameter.USER_ID, BaseParameter.Remark);
                result.ErrorNumber = 0;
                result.Message = "Deleted";
                return result;

            }
            else
            {
                if (BaseParameter.Code == "DeleteAll")
                {
                    foreach (var item in BaseParameter.ListtspartTranfer)
                    {
                        // lấy thông tin sản phẩm
                        string sql = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO`, tspart.`PART_NM`, tspart.`PART_CAR`, tspart.`PART_FML`, tspart.`PART_SNP`, 
                                    (SELECT TSCODE.`CD_SYS_NOTE` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `PART_SCN`, 
                                    IFNULL((SELECT tiivtr.`QTY` FROM tiivtr WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX` AND tiivtr.`LOC_IDX` = '1'),0) AS `QTY`
                                    FROM tspart WHERE tspart.`PART_USENY` = 'Y' AND tspart.`PART_NO` = '" + item.PART_NO + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.ListtspartTranfer = new List<tspartTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt, true)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
                        }
                        //kiểm tra nếu mã hàng không tồn tại thì trả về báo lỗi luôn khong cần xử lý các công việc sau đó
                        if (result.ListtspartTranfer.Count > 0)
                        {
                            DeletePacking(result, item.LOT_CODE, item.PKG_GRP, BaseParameter.USER_ID, BaseParameter.Remark);
                        }
                    }

                    result.ErrorNumber = 0;
                    result.Error = "deleted: " + BaseParameter.ListtspartTranfer.Count().ToString();
                }
                else
                {
                    result.ErrorNumber = -2;
                    result.Error = "no data";
                }

            }
            return result;
        }

        private void DeletePacking(BaseResult result, string barcode, string iDCard, string Create_User, string Remark)
        {
            var part_idx = result.ListtspartTranfer[0].PART_IDX.ToString();
            string stringValues = "(" + part_idx + ", '" + iDCard + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + barcode + "', '" + Remark + "', 'D', NOW(), '" + Create_User + "', (SELECT `PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx + "'))";

            //cập nhật vao danh sách đóng gói
            string sql = "INSERT INTO tdpdmtim_del (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, `VLID_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`) VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);

            //cập nhật vào số lượng tồn kho
            string QtySQL = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES " +
                "(" + part_idx + ", 2, 1 , NOW(), '" + Create_User + "')" +
                " On DUPLICATE KEY UPDATE `LOC_IDX`= 2, `QTY`= `QTY` - 1, `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + Create_User + "'";
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, QtySQL);

            //cập nhật vào danhs sách lịch sử scan hàng
            string histSQL = "delete from tdpdmtim where VLID_BARCODE ='" + barcode + "' and VLID_GRP ='" + iDCard + "' ";
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, histSQL);
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
    }
}