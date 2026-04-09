

namespace MESService.Implement
{
    public class D13Service : BaseService<torderlist, ItorderlistRepository>
    , ID13Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D13Service(ItorderlistRepository torderlistRepository

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
                await Task.Run(() => { result.DATEString = DateTime.Now.ToString("yyyy-MM-dd"); });
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

            try
            {
                // Xử lý ngày tháng tối ưu
                DateTime fromDate = baseParameter.FromDate.Value.Date;
                DateTime toDate = baseParameter.ToDate.Value.Date.AddDays(1).AddSeconds(-1);

                string ADATE = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                string BDATE = toDate.ToString("yyyy-MM-dd HH:mm:ss");

                // Escape các giá trị LIKE
                string partNo = EscapeLikeString(baseParameter.PartNo);
                string packingLotCode = EscapeLikeString(baseParameter.PackingLotCode);
                string partName = EscapeLikeString(baseParameter.PartName);
                string leadData = EscapeLikeString(baseParameter.LeadData?.ToString() ?? "");

                // Xây dựng query và parameters
                string query;
                List<MySqlParameter> parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@FromDate", ADATE),
            new MySqlParameter("@ToDate", BDATE),
            new MySqlParameter("@PartNo", $"%{partNo}%"),
            new MySqlParameter("@PackingLotCode", $"%{packingLotCode}%"),
            new MySqlParameter("@PartName", $"%{partName}%"),
            new MySqlParameter("@LeadData", $"%{leadData}%")
        };

                // Build query dựa trên FilterType
                switch (baseParameter.FilterType)
                {
                    case "Y":
                        query = GetDiscardQuery();
                        parameters.Add(new MySqlParameter("@DiscardYn", "Y"));
                        break;

                    case "N":
                        query = GetDiscardQuery();
                        parameters.Add(new MySqlParameter("@DiscardYn", "N"));
                        break;

                    case "D":
                        query = GetDeletedQuery();
                        break;

                    case "REWORK":
                        query = GetReworkQuery();
                        break;

                    default: // ALL
                        query = GetCombinedQuery();
                        break;
                }

                // Thực thi query
                result.ListtspartTranfer = await MySQLHelperV2.QueryToListAsync<tspartTranfer>(
                    GlobalHelper.MariaDBConectionString,
                    query,
                    parameters.ToArray()
                );

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                // Log error here
            }

            return result;
        }

        private string GetDiscardQuery()
        {
            return @"
        SELECT 
            tspart.PART_NO,
            tspart.PART_NM,
            tspart.PART_CAR,
            tspart.PART_FML,
            t.VLID_PART_SNP AS SNP_QTY,
            t.VLID_GRP AS PKG_GRP,
            IFNULL(iv.QTY, 0) AS QTY,
            COALESCE(rw.rework_count, 0) AS REWORK_QTY,
            t.VLID_DTM AS MTIN_DTM,
            TRIM(t.VLID_BARCODE) AS LOT_CODE,
            t.VLID_REMARK AS REMARK,
            t.CREATE_USER,
            t.VLID_DSCN_YN AS DSCN_YN,
            t.REWORK_YN,
            t.PDMTIN_IDX,
            t.VLID_PART_IDX,
            t.CREATE_DTM,
            NULL AS DeleteTime,
            NULL AS DeleteBy
        FROM tdpdmtim t
        INNER JOIN tspart ON tspart.PART_IDX = t.VLID_PART_IDX
        LEFT JOIN tiivtr iv ON iv.PART_IDX = t.VLID_PART_IDX AND iv.LOC_IDX = '2'
        LEFT JOIN (
            SELECT VLID_PART_IDX, COUNT(*) AS rework_count
            FROM tdpdmtim
            WHERE REWORK_YN = 'Y' AND VLID_DSCN_YN = 'N'
            GROUP BY VLID_PART_IDX
        ) rw ON rw.VLID_PART_IDX = t.VLID_PART_IDX
        WHERE t.VLID_DTM BETWEEN @FromDate AND @ToDate
            AND t.VLID_DSCN_YN = @DiscardYn
            AND t.REWORK_YN = 'N'
            AND tspart.PART_NO LIKE @PartNo
            AND tspart.PART_NM LIKE @PartName
            AND tspart.PART_CAR LIKE @LeadData
            AND t.VLID_GRP LIKE @PackingLotCode
        ORDER BY t.VLID_DTM DESC, tspart.PART_NO, t.VLID_GRP";
        }

        private string GetDeletedQuery()
        {
            return @"
        SELECT 
            B.PART_NO,
            B.PART_NM,
            B.PART_CAR,
            B.PART_FML,
            A.VLID_PART_SNP AS SNP_QTY,
            A.VLID_GRP AS PKG_GRP,
            IFNULL(C2.QTY, 0) AS QTY,
            COALESCE(rw.rework_count, 0) AS REWORK_QTY,
            D.VLID_DTM AS MTIN_DTM,
            TRIM(A.VLID_BARCODE) AS LOT_CODE,
            A.VLID_REMARK AS REMARK,
            D.CREATE_USER,
            'D' AS DSCN_YN,
            A.REWORK_YN,
            A.PDMTIN_IDX,
            A.VLID_PART_IDX,
            D.CREATE_DTM,
            A.CREATE_DTM AS DeleteTime,
            A.CREATE_USER AS DeleteBy
        FROM tdpdmtim_del A
        INNER JOIN tspart B ON A.VLID_PART_IDX = B.PART_IDX
        LEFT JOIN tiivtr C2 ON A.VLID_PART_IDX = C2.PART_IDX AND C2.LOC_IDX = '2'
        INNER JOIN tdpdmtim_hist D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
            AND A.VLID_GRP = D.VLID_GRP 
            AND A.VLID_BARCODE = D.VLID_BARCODE
        LEFT JOIN (
            SELECT VLID_PART_IDX, COUNT(*) AS rework_count
            FROM tdpdmtim_del
            WHERE REWORK_YN = 'Y'
            GROUP BY VLID_PART_IDX
        ) rw ON rw.VLID_PART_IDX = A.VLID_PART_IDX
        WHERE A.CREATE_DTM BETWEEN @FromDate AND @ToDate
            AND B.PART_NO LIKE @PartNo
            AND B.PART_CAR LIKE @LeadData
            AND B.PART_NM LIKE @PartName
            AND A.VLID_GRP LIKE @PackingLotCode
        ORDER BY D.VLID_DTM DESC, B.PART_NO, A.VLID_GRP";
        }


        private string GetReworkQuery()
        {
            return @"
        SELECT 
            tspart.PART_NO,
            tspart.PART_NM,
            tspart.PART_CAR,
            tspart.PART_FML,
            t.VLID_PART_SNP AS SNP_QTY,
            t.VLID_GRP AS PKG_GRP,
            IFNULL(iv.QTY, 0) AS QTY,
            1 AS REWORK_QTY,
            t.VLID_DTM AS MTIN_DTM,
            TRIM(t.VLID_BARCODE) AS LOT_CODE,
            t.VLID_REMARK AS REMARK,
            t.CREATE_USER,
            t.VLID_DSCN_YN AS DSCN_YN,
            t.REWORK_YN,
            t.PDMTIN_IDX,
            t.VLID_PART_IDX,
            t.CREATE_DTM,
            NULL AS DeleteTime,
            NULL AS DeleteBy
        FROM tdpdmtim t
        INNER JOIN tspart ON tspart.PART_IDX = t.VLID_PART_IDX
        LEFT JOIN tiivtr iv ON iv.PART_IDX = t.VLID_PART_IDX AND iv.LOC_IDX = '2'
        WHERE t.VLID_DTM BETWEEN @FromDate AND @ToDate
            AND t.REWORK_YN = 'Y'
            AND tspart.PART_NO LIKE @PartNo
            AND tspart.PART_CAR LIKE @LeadData
            AND tspart.PART_NM LIKE @PartName
            AND t.VLID_GRP LIKE @PackingLotCode
        ORDER BY t.VLID_DTM DESC, tspart.PART_NO, t.VLID_GRP";
        }

        private string GetCombinedQuery()
        {
            return @"
        SELECT * FROM (
            -- Active records
            SELECT 
                tspart.PART_NO,
                tspart.PART_NM,
                tspart.PART_CAR,
                tspart.PART_FML,
                t.VLID_PART_SNP AS SNP_QTY,
                t.VLID_GRP AS PKG_GRP,
                IFNULL(iv.QTY, 0) AS QTY,
                COALESCE(rw_active.rework_count, 0) AS REWORK_QTY,
                t.VLID_DTM AS MTIN_DTM,
                TRIM(t.VLID_BARCODE) AS LOT_CODE,
                t.VLID_REMARK AS REMARK,
                t.CREATE_USER,
                t.VLID_DSCN_YN AS DSCN_YN,
                t.REWORK_YN,
                t.PDMTIN_IDX,
                t.VLID_PART_IDX,
                t.CREATE_DTM,
                NULL AS DeleteTime,
                NULL AS DeleteBy
            FROM tdpdmtim t
            INNER JOIN tspart ON tspart.PART_IDX = t.VLID_PART_IDX
            LEFT JOIN tiivtr iv ON iv.PART_IDX = t.VLID_PART_IDX AND iv.LOC_IDX = '2'
            LEFT JOIN (
                SELECT VLID_PART_IDX, COUNT(*) AS rework_count
                FROM tdpdmtim
                WHERE REWORK_YN = 'Y' AND VLID_DSCN_YN = 'N'
                GROUP BY VLID_PART_IDX
            ) rw_active ON rw_active.VLID_PART_IDX = t.VLID_PART_IDX
            WHERE t.VLID_DTM BETWEEN @FromDate AND @ToDate

            UNION ALL

            -- Deleted records
            SELECT 
                B.PART_NO,
                B.PART_NM,
                B.PART_CAR,
                B.PART_FML,
                A.VLID_PART_SNP AS SNP_QTY,
                A.VLID_GRP AS PKG_GRP,
                IFNULL(C2.QTY, 0) AS QTY,
                COALESCE(rw_deleted.rework_count, 0) AS REWORK_QTY,
                D.VLID_DTM AS MTIN_DTM,
                TRIM(A.VLID_BARCODE) AS LOT_CODE,
                A.VLID_REMARK AS REMARK,
                D.CREATE_USER,
                'D' AS DSCN_YN,
                A.REWORK_YN,
                A.PDMTIN_IDX,
                A.VLID_PART_IDX,
                D.CREATE_DTM,
                A.CREATE_DTM AS DeleteTime,
                A.CREATE_USER AS DeleteBy
            FROM tdpdmtim_del A
            INNER JOIN tspart B ON A.VLID_PART_IDX = B.PART_IDX
            LEFT JOIN tiivtr C2 ON A.VLID_PART_IDX = C2.PART_IDX AND C2.LOC_IDX = '2'
            INNER JOIN tdpdmtim_hist D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
                AND A.VLID_GRP = D.VLID_GRP 
                AND A.VLID_BARCODE = D.VLID_BARCODE
            LEFT JOIN (
                SELECT VLID_PART_IDX, COUNT(*) AS rework_count
                FROM tdpdmtim_del
                WHERE REWORK_YN = 'Y'
                GROUP BY VLID_PART_IDX
            ) rw_deleted ON rw_deleted.VLID_PART_IDX = A.VLID_PART_IDX
            WHERE A.CREATE_DTM BETWEEN @FromDate AND @ToDate
        ) combined
        WHERE PART_NO LIKE @PartNo
            AND PART_CAR LIKE @LeadData
            AND PART_NM LIKE @PartName
            AND PKG_GRP LIKE @PackingLotCode
        ORDER BY MTIN_DTM DESC, PART_NO, PKG_GRP";
        }


        private string EscapeLikeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            return input
                .Replace("'", "''")
                .Replace("[", "[[]")
                .Replace("%", "[%]")
                .Replace("_", "[_]");
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
            CleanStringProperties(QRCode);
            //tìm kiếm theo mã ID Card
            if (QRCode.Contains("3S") && QRCode.Contains("HMC"))
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

                string sqlTSPart = @"SELECT p.`PART_IDX`, 
                                            p.`PART_NO`, 
                                            p.`PART_NM`, 
                                            p.`PART_CAR`, 
                                            p.`PART_FML`, 
                                            p.`PART_SNP`,
                                            c.`CD_SYS_NOTE` AS `PART_SCN`,
                                            IFNULL(v.`QTY`, 0) AS `QTY`
                                        FROM tspart AS p
                                        LEFT JOIN TSCODE AS c ON p.`PART_SCN` = c.`CD_IDX`
                                        LEFT JOIN tiivtr AS v ON p.`PART_IDX` = v.`PART_IDX` AND v.`LOC_IDX` = '1'
                                        WHERE p.`PART_USENY` = 'Y' 
                                          AND p.`PART_IDX` = '"+ part_idx + "';";

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
                    string reworkYN = BaseParameter.ReworkYN ?? "N";
                    if (!string.IsNullOrEmpty(IDCard)) //nếu có mã IDCrad gửi về
                    {
                        result = await AddMoretoIDCardAsync(barcode, partNo, IDCard, User_ID, reworkYN); // ✅ Truyền xuống
                    }
                    else // thêm mới
                    {
                        result = await AddNewIDCardAsync(barcode, partNo, User_ID, BaseParameter.CustomerCode, reworkYN); // ✅ Truyền xuống
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        private async Task<BaseResult> AddNewIDCardAsync(string barcode, string partNo, string User_ID, string customerCode, string reworkYN = "N")
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
                var IDCard = await CreatePackingLotAsync(result, customerCode, User_ID); // tạo mã packing cho các hàng của PKI
                AddtoDatabase(result, barcode, IDCard, User_ID, reworkYN);
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
        private void AddtoDatabase(BaseResult result, string barcode, string iDCard,
                  string Create_User, string reworkYN = "N")
        {
            var part_idx = result.ListtspartTranfer[0].PART_IDX.ToString();

            string stringValues = "(" + part_idx + ", '" + iDCard + "', '" +
                                 DateTime.Now.ToString("yyyy-MM-dd") +
                                 "', '" + barcode + "', 'Scan on new MES', 'N', '" + reworkYN +
                                 "', NOW(), '" + Create_User +
                                 "', (SELECT `PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx +
                                 "'), IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx + "'), ''))";

            // Insert tdpdmtim
            string sql = @"INSERT INTO tdpdmtim 
        (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, 
         `VLID_DSCN_YN`, `REWORK_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`, `BARCD_LOC`) 
        VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);

            // Insert history
            string histSQL = @"INSERT INTO tdpdmtim_hist 
        (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, 
         `VLID_DSCN_YN`, `REWORK_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`, `BARCD_LOC`) 
        VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, histSQL);

            // ✅ CHỈ CẬP NHẬT LOC_IDX = 2 (Stock Qty)
            // Normal và Rework đều vào đây
            string QtySQL = @"INSERT INTO tiivtr 
        (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) 
        VALUES (" + part_idx + ", 2, 1, NOW(), '" + Create_User + @"') 
        ON DUPLICATE KEY UPDATE 
        `QTY` = `QTY` + 1, 
        `UPDATE_DTM` = NOW(), 
        `UPDATE_USER` = '" + Create_User + "'";
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, QtySQL);

            // ❌ XÓA: Không còn cập nhật LOC_IDX=3
        }

        /// <summary>
        /// thêm tiếp mã sản phẩm vào trong dánh sách đong gói đã có trước đó ( còn thiếu số lượng hàng thì cho phép Scan tiếp)
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="partNo"></param>
        /// <param name="iDCard"></param>
        /// <param name="Create_User"></param>
        /// <returns></returns>
        private async Task<BaseResult> AddMoretoIDCardAsync(string barcode, string partNo, string IDCard, string Create_User, string reworkYN = "N")
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
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt,true)); // lưu thông tin sản phẩm vào biến: ListtspartTranfer
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
                AddtoDatabase(result, barcode, IDCard, Create_User, reworkYN);
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
        private async Task<string> CreatePackingLotAsync(BaseResult result, string customerCode, string Create_User)
        {
            var curentDate = DateTime.Now;

            string DATE_DATE = "";
            string DATE_CHK = curentDate.ToString("MM");

            if (int.Parse(DATE_CHK) > 9)
            {
                if (DATE_CHK == "10")
                {
                    DATE_DATE = curentDate.ToString("yy") + "A" + curentDate.ToString("dd");
                }
                if (DATE_CHK == "11")
                {
                    DATE_DATE = curentDate.ToString("yy") + "B" + curentDate.ToString("dd");
                }
                if (DATE_CHK == "12")
                {
                    DATE_DATE = curentDate.ToString("yy") + "C" + curentDate.ToString("dd");
                }
            }
            else
            {
                DATE_DATE = curentDate.ToString("yy") +
                            curentDate.ToString("MM").Substring(1, 1) +
                            curentDate.ToString("dd");
            }

            string sql = @"SELECT (IFNULL(MAX(`SEQ_NO`),0)+1) AS SEQ_NO  FROM tdpdmtin_serial WHERE `TDPD_DATE`= @CurentDate  AND Serial_no LIKE '%" + customerCode + "'";
            var parameters = new[] { new MySqlParameter("@CurentDate", MySqlDbType.VarChar) { Value = curentDate.ToString("yyyy-MM-dd") } };
            string number = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, sql, parameters);

            string packingLot = "3S" + DATE_DATE + int.Parse(number).ToString("0000") + customerCode.ToUpper().Trim();

            string sqlStr = "INSERT INTO `tdpdmtin_serial`( `PART_IDX`, `TDPD_DATE`, `SEQ_NO`, `SERIAL_NO`, `CREATE_DTM`, `CREATE_USER`, `CustomerCode`) VALUES " +
                                "('" + result.ListtspartTranfer[0].PART_IDX + "', '" + curentDate.ToString("yyyy-MM-dd") + "', " + number + ", '" + packingLot + "', NOW(), '" + Create_User + "','"+ customerCode + "')";

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

        private void DeletePacking(BaseResult result, string barcode, string iDCard,
                   string Create_User, string Remark)
        {
            var part_idx = result.ListtspartTranfer[0].PART_IDX.ToString();

            // Lấy REWORK_YN để lưu vào deleted table
            string getReworkSQL = @"SELECT REWORK_YN 
                           FROM tdpdmtim 
                           WHERE VLID_BARCODE = @Barcode 
                           AND VLID_GRP = @IDCard";

            var parameters = new[] {
        new MySqlParameter("@Barcode", MySqlDbType.VarChar) { Value = barcode },
        new MySqlParameter("@IDCard", MySqlDbType.VarChar) { Value = iDCard }
    };

            var ds = MySQLHelper.FillDataSetBySQL(GlobalHelper.MariaDBConectionString,
                                                  getReworkSQL, parameters);

            string reworkYN = "N";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                reworkYN = ds.Tables[0].Rows[0]["REWORK_YN"]?.ToString() ?? "N";
            }

            string stringValues = "(" + part_idx + ", '" + iDCard + "', '" +
                                 DateTime.Now.ToString("yyyy-MM-dd") +
                                 "', '" + barcode + "', '" + Remark + "', 'D', '" + reworkYN +
                                 "', NOW(), '" + Create_User +
                                 "', (SELECT `PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = '" + part_idx + "'))";

            // Insert deleted
            string sql = @"INSERT INTO tdpdmtim_del 
        (`VLID_PART_IDX`, `VLID_GRP`, `VLID_DTM`, `VLID_BARCODE`, `VLID_REMARK`, 
         `VLID_DSCN_YN`, `REWORK_YN`, `CREATE_DTM`, `CREATE_USER`, `VLID_PART_SNP`) 
        VALUES " + stringValues;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);

            // ✅ CHỈ TRỪ LOC_IDX = 2 (Stock Qty)
            string QtySQL = @"INSERT INTO tiivtr 
        (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) 
        VALUES (" + part_idx + ", 2, -1, NOW(), '" + Create_User + @"') 
        ON DUPLICATE KEY UPDATE 
        `QTY` = `QTY` - 1, 
        `UPDATE_DTM` = NOW(), 
        `UPDATE_USER` = '" + Create_User + "'";
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, QtySQL);

            // ❌ XÓA: Không còn trừ LOC_IDX=3

            // Delete từ bảng chính
            string deleteSql = @"DELETE FROM tdpdmtim 
                        WHERE VLID_BARCODE = @Barcode 
                        AND VLID_GRP = @IDCard";
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, deleteSql, parameters);
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

