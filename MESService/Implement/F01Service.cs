namespace MESService.Implement
{
    public class F01Service : BaseService<torderlist, ItorderlistRepository>, IF01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public F01Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // ✅ Tìm kiếm lịch sử IQC NG
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string fromDate = BaseParameter?.FromDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
                string toDate = BaseParameter?.ToDate?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
                string search = BaseParameter?.SearchText?.Trim() ?? "";

                string sql = @"SELECT 
            ID, 
            SupplierCode, 
            InvoiceName, 
            Barcode, 
            MaterialCode, 
            MaterialName,
            Quantity,
            TotalInvoice,
            NGQty, 
            Unit, 
            ROUND((NGQty/Quantity*100), 2) AS Percentage,
            DATE_FORMAT(InputDate, '%Y-%m-%d') AS InputDate,
            DATE_FORMAT(IssueDate, '%Y-%m-%d') AS IssueDate,
            Week, 
            Month, 
            ErrorInfo, 
            Picture,
            ImprovementPlans,
            Note,
            CreateUser,
            DATE_FORMAT(CreateDate, '%Y-%m-%d %H:%i:%s') AS CreateDate
        FROM IQC_NG
        WHERE DATE(IssueDate) BETWEEN @FromDate AND @ToDate";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += @" AND (Barcode LIKE @Search OR InvoiceName LIKE @Search OR MaterialCode LIKE @Search 
                     OR SupplierCode LIKE @Search OR ErrorInfo LIKE @Search)";
                }

                sql += " ORDER BY IssueDate DESC, ID DESC LIMIT 1000";

                var parameters = new[]
                {
            new MySqlParameter("@FromDate", MySqlDbType.Date) { Value = fromDate },
            new MySqlParameter("@ToDate", MySqlDbType.Date) { Value = toDate },
            new MySqlParameter("@Search", MySqlDbType.VarChar) { Value = $"%{search}%" }
        };

                result.HistoryDataList = await MySQLHelperV2.QueryToListAsync<HistoryDataTranfer>(
                    GlobalHelper.MariaDBConectionString, sql, parameters);

                result.ErrorNumber = 0;
                result.Message = $"Found {result.HistoryDataList?.Count ?? 0} records";
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
                result.Error = ex.Message;
            }
            return result;
        }

        // ✅ Lấy thông tin lot code từ kho
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var lotCode = BaseParameter.LotCode.Trim().ToUpper();

                string query = @"
            SELECT 
                a.ID,
                a.ParentID,
                c.SupplierName,
                a.Barcode, 
                a.MaterialName as MaterialCode, 
                d.`Name` as MaterialName,
                e.CategoryUnitName,
                (SELECT SUM(Quantity) FROM WarehouseInputDetailBarcode WHERE ParentID=a.ParentID AND MaterialName=a.MaterialName) as QuantityInvoice,
                a.Quantity, 
                b.InvoiceInputName as InvoiceName, 
                IFNULL(a.DateScan, a.UpdateDate) as InputDate
            FROM WarehouseInputDetailBarcode a 
            JOIN WarehouseInput b ON a.ParentID = b.ID
            JOIN InvoiceInput c ON b.InvoiceInputID = c.ID
            JOIN Material d ON a.MaterialID = d.ID
            LEFT JOIN WarehouseInputDetail e ON e.ParentID = a.ParentID 
                                             AND e.MaterialID = a.MaterialID
            WHERE a.Barcode = @LotCode
                AND b.CustomerID IN (23,188) 
                AND b.InvoiceInputID > 0                                     
            LIMIT 1";

                var parameters = new[]
                {
            new MySqlParameter("@LotCode", MySqlDbType.VarChar) { Value = lotCode }
        };

                result.NGDataList = await MySQLHelperV2.QueryToListAsync<NGDataTranfer>(
                    GlobalHelper.ERP_MariaDBConectionString, query, parameters);

                if (result.NGDataList == null || result.NGDataList.Count == 0)
                {
                    result.ErrorNumber = 1;
                    result.Message = $"Lot code '{lotCode}' not found in warehouse";
                }
                else
                {
                    result.ErrorNumber = 0;
                    result.Message = "Lot code found";
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
                result.Error = ex.Message;
            }
            return result;
        }

        // ✅ Lưu dữ liệu NG mới
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"INSERT INTO IQC_NG (
        SupplierCode, Barcode, MaterialCode, MaterialName, Quantity, TotalInvoice, NGQty, 
        InvoiceName, Unit, InputDate, IssueDate, WarehouseInputDetailBarcodeID, 
        Week, Month, ErrorInfo, Picture, ImprovementPlans, Note, CreateUser, CreateDate
            ) VALUES (
        @SupplierCode, @Barcode, @MaterialCode, @MaterialName, @Quantity, @TotalInvoice, @NGQty, 
        @InvoiceName, @Unit, NOW(), NOW(), @ID,   -- ✅ Thay 'PCS' bằng @Unit
        WEEK(NOW()), MONTH(NOW()), @ErrorInfo, @Picture, 'waiting feedback', 'On going', @CreateUser, NOW()
            )";

                var parameters = new[]
         {
            new MySqlParameter("@SupplierCode", BaseParameter.GroupCode ?? "N/A"),
            new MySqlParameter("@Barcode", BaseParameter.LotCode ?? ""),
            new MySqlParameter("@MaterialCode", BaseParameter.PartNo ?? "N/A"),
            new MySqlParameter("@MaterialName", BaseParameter.PartName ?? "N/A"),
            new MySqlParameter("@Quantity", BaseParameter.Total > 0 ? BaseParameter.Total : BaseParameter.TotalNG),
            new MySqlParameter("@TotalInvoice", BaseParameter.TotalInvoice > 0 ? BaseParameter.TotalInvoice : BaseParameter.TotalNG),
            new MySqlParameter("@NGQty", BaseParameter.TotalNG),
            new MySqlParameter("@InvoiceName", BaseParameter.Invoice ?? "N/A"),
            new MySqlParameter("@Unit", BaseParameter.Unit ?? "PCS"),
            new MySqlParameter("@ID", BaseParameter.ID > 0 ? BaseParameter.ID : (object)DBNull.Value),
            new MySqlParameter("@ErrorInfo", BaseParameter.ErrorInfo ?? ""),
            new MySqlParameter("@Picture", BaseParameter.Picture ?? ""),
            new MySqlParameter("@CreateUser", BaseParameter.USER_ID ?? "")
        };

                var rowsAffected = await MySQLHelperV2.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString, sql, parameters);

                result.ErrorNumber = rowsAffected > 0 ? 0 : 1;
                result.Message = rowsAffected > 0 ? "Save successfully!" : "Save failed";
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
                result.Error = ex.Message;
            }
            return result;
        }

        // ✅ THAY FUNCTION NÀY VÀO F01SERVICE.CS

        public virtual async Task<BaseResult> ButtonUpdateHistory_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"UPDATE IQC_NG SET 
            NGQty = @NGQty,
            Unit = @Unit,
            ErrorInfo = @ErrorInfo,
            ImprovementPlans = @ImprovementPlans,
            Note = @Note,
            UpdateUser = @UpdateUser,
            UpdateDate = NOW()
        WHERE ID = @ID";

                var parameters = new[]
                {
            new MySqlParameter("@ID", BaseParameter.ID),
            new MySqlParameter("@NGQty", BaseParameter.TotalNG),
            new MySqlParameter("@Unit", BaseParameter.Unit),
            new MySqlParameter("@ErrorInfo", BaseParameter.ErrorInfo ?? ""),
            new MySqlParameter("@ImprovementPlans", BaseParameter.ImprovementPlans ?? "waiting feedback"),
            new MySqlParameter("@Note", BaseParameter.Note ?? "On going"),
            new MySqlParameter("@UpdateUser", BaseParameter.USER_ID ?? "")
        };

                var rowsAffected = await MySQLHelperV2.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString, sql, parameters);

                result.ErrorNumber = rowsAffected > 0 ? 0 : 1;
                result.Message = rowsAffected > 0 ? "Update successfully!" : "Update failed";
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.Ids == null || BaseParameter.Ids.Count == 0)
                {
                    result.ErrorNumber = 1;
                    result.Error = "No records selected";
                    return result;
                }

                // ✅ SAFE: Dùng parameterized query
                var placeholders = string.Join(",", BaseParameter.Ids.Select((_, i) => $"@id{i}"));
                string sql = $"DELETE FROM IQC_NG WHERE ID IN ({placeholders})";

                var parameters = BaseParameter.Ids.Select((id, i) =>
                    new MySqlParameter($"@id{i}", MySqlDbType.Int32) { Value = id }
                ).ToArray();

                var rowsAffected = await MySQLHelperV2.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString, sql, parameters);

                if (rowsAffected > 0)
                {
                    result.ErrorNumber = 0;
                    result.Message = $"Deleted {rowsAffected} record(s) successfully!";
                }
                else
                {
                    result.ErrorNumber = 1;
                    result.Error = "No records deleted. IDs may not exist.";
                }
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
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
    }
}