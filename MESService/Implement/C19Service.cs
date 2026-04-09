namespace MESService.Implement
{
    public class C19Service : BaseService<torderlist, ItorderlistRepository>, IC19Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public C19Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.Action == 1 && BaseParameter?.ListSearchString != null) // TabPage1
                {
                    if (BaseParameter.ListSearchString.Count >= 5)
                    {
                        string LEAD_NO = BaseParameter.ListSearchString[0];
                        string TY = BaseParameter.ListSearchString[1];
                        string WIRE = BaseParameter.ListSearchString[2];
                        string T1 = BaseParameter.ListSearchString[3];
                        string S1 = BaseParameter.ListSearchString[4];
                        string T2 = BaseParameter.ListSearchString[3];
                        string S2 = BaseParameter.ListSearchString[4];

                        LEAD_NO = "%" + LEAD_NO + "%";

                        if (TY == "ALL")
                        {
                            TY = "";
                        }
                        else
                        {
                            TY = "%" + TY + "%";
                        }

                        if (string.IsNullOrEmpty(WIRE))
                        {
                            WIRE = "%%";
                        }

                        T1 = "%" + T1 + "%";
                        S1 = "%" + S1 + "%";
                        T2 = "%" + T2 + "%";
                        S2 = "%" + S2 + "%";

                        string DGV_DATA1 = @"SELECT 
                    `LEAD_INDEX`,`LEAD_SCN`, `LEAD_PN`, 
                    IFNULL(`BUNDLE_SIZE`, 0) AS `BUNDLE_SIZE`,
                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `W_PN_IDX`), '') AS `W_PN`,
                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T1_PN_IDX`), '') AS `T1_PN`,
                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S1_PN_IDX`), '') AS `S1_PN`,
                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T2_PN_IDX`), '') AS `T2_PN`,
                    IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S2_PN_IDX`), '') AS `S2_PN`,
                    IFNULL(`STRIP1`, 0) AS `STRIP1`, 
                    IFNULL(`STRIP2`, 0) AS `STRIP2`, 
                    IFNULL(`CCH_W1`, '') AS `CCH_W1`, 
                    IFNULL(`ICH_W1`, '') AS `ICH_W1`, 
                    IFNULL(`CCH_W2`, '') AS `CCH_W2`, 
                    IFNULL(`ICH_W2`, '') AS `ICH_W2`, 
                    IFNULL(`T1NO`, '') AS `T1NO`, 
                    IFNULL(`T2NO`, '') AS `T2NO`, 
                    IFNULL(`WR_NO`, '') AS `WR_NO`, 
                    IFNULL(`WIRE_NM`, '') AS `WIRE_NM`, 
                    IFNULL(`W_Diameter`, '') AS `W_Diameter`, 
                    IFNULL(`W_LINK`, '') AS `W_LINK`, 
                    IFNULL(`W_Color`, '') AS `W_Color`, 
                    IFNULL(`W_Length`, 0) AS `W_Length`, 
                    IFNULL(`DSCN_YN`, 'N') AS `DSCN_YN`, 
                    IFNULL(`CREATE_DTM`, '') AS `CREATE_DTM`, 
                    IFNULL(`CREATE_USER`, '') AS `CREATE_USER`, 
                    IFNULL(`UPDATE_DTM`, '') AS `UPDATE_DTM`, 
                    IFNULL(`UPDATE_USER`, '') AS `UPDATE_USER`
                    FROM torder_lead_bom  
                    WHERE `LEAD_PN` LIKE '%" + LEAD_NO + "%' AND `LEAD_SCN` LIKE '%" + TY + "%' HAVING `W_PN` LIKE '" + WIRE + "' AND(`T1_PN` LIKE '%" + T1 + "%' OR `T2_PN` LIKE '%" + T2 + "%') AND(`S1_PN` LIKE '%" + S1 + "%' OR `S2_PN` LIKE '%" + S2 + "%')";

                        DGV_DATA1 += " LIMIT 100";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        if (ds.Tables.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                }
                else if (BaseParameter?.Action == 5 && BaseParameter?.ListSearchString != null) // TabPage5 - MAG_BACODE
                {
                    string textBox5 = BaseParameter.ListSearchString[0];

                    string DGV_DATA5 = "SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` LIKE '%" + textBox5 + "%' ORDER BY torder_lead_bom.`LEAD_PN`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA5);
                    result.DataGridView6 = new List<SuperResultTranfer>();
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string LEAD_NO6 = BaseParameter.ListSearchString[0];
                System.Diagnostics.Debug.WriteLine($"DGV_LOAD - LEAD_NO6: {LEAD_NO6}");

                // Phần 1: Lấy thông tin chi tiết LEAD - giữ nguyên
                string DGV_DATA6 = "SELECT `LEAD_PN`, `DSCN_YN`, `LEAD_INDEX`, " +
                    "IFNULL((SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`LEAD_NO`=`LEAD_PN`),'-') AS `HOOK_RACK`, " +
                    "IFNULL((SELECT trackmaster.`SFTY_STK` FROM trackmaster WHERE trackmaster.`LEAD_NO`=LEAD_PN),0) AS `Safety_Stock`, " +
                    "IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `W_PN_IDX`), '') AS `WIRE`, " +
                    "IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T1_PN_IDX`),'') AS `TERM1`, " +
                    "IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S1_PN_IDX`),'') AS `SEAL1`, " +
                    "IFNULL(`STRIP1`, 0) AS `STRIP1`, IFNULL(`CCH_W1`,'') AS `CCH_W1`,  IFNULL(`ICH_W1`, '') AS `ICH_W1`, " +
                    "IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `T2_PN_IDX`),'') AS `TERM2`, " +
                    "IFNULL((SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = `S2_PN_IDX`),'') AS `SEAL2`, " +
                    "IFNULL(`STRIP2`, 0) AS `STRIP2`, IFNULL(`CCH_W2`,'') AS `CCH_W2`,  IFNULL(`ICH_W2`, '') AS `ICH_W2`, " +
                    "`LEAD_SCN`, IFNULL(`W_LINK`, '') AS `W_LINK`, IFNULL(`WR_NO`, '') AS `WR_NO`, IFNULL(`WIRE_NM`, '') AS `WIRE_NM`, " +
                    "IFNULL(`W_Diameter`, '') AS `W_Diameter`, IFNULL(`W_Color`, '') AS `W_Color`, " +
                    "IFNULL(`W_Length`, 0) AS `W_Length`, IFNULL(`T1NO`, '') AS `T1NO`, IFNULL(`T2NO`, '') AS `T2NO`, IFNULL(`BUNDLE_SIZE`, 0) AS `BUNDLE_QTY` " +
                    "FROM torder_lead_bom " +
                    "WHERE torder_lead_bom.`LEAD_PN` = '" + LEAD_NO6 + "' " +
                    "ORDER BY torder_lead_bom.`LEAD_PN`";

                DataSet dsDGV_6 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA6);

                // Xử lý dữ liệu LEAD detail - giữ nguyên
                if (dsDGV_6.Tables.Count > 0 && dsDGV_6.Tables[0].Rows.Count > 0)
                {
                    result.TextBox6 = dsDGV_6.Tables[0].Rows[0]["LEAD_PN"].ToString();
                    result.DSCN = dsDGV_6.Tables[0].Rows[0]["DSCN_YN"].ToString();
                    result.LEAD_INDEX = dsDGV_6.Tables[0].Rows[0]["LEAD_INDEX"].ToString();
                    result.TextBox7 = dsDGV_6.Tables[0].Rows[0]["HOOK_RACK"].ToString();
                    result.TextBox19 = dsDGV_6.Tables[0].Rows[0]["Safety_Stock"].ToString();
                    result.TextBox8 = dsDGV_6.Tables[0].Rows[0]["WIRE"].ToString();
                    result.TextBox9 = dsDGV_6.Tables[0].Rows[0]["TERM1"].ToString();
                    result.TextBox10 = dsDGV_6.Tables[0].Rows[0]["SEAL1"].ToString();
                    result.TextBox11 = dsDGV_6.Tables[0].Rows[0]["STRIP1"].ToString();
                    result.TextBox12 = dsDGV_6.Tables[0].Rows[0]["CCH_W1"].ToString();
                    result.TextBox13 = dsDGV_6.Tables[0].Rows[0]["ICH_W1"].ToString();
                    result.TextBox14 = dsDGV_6.Tables[0].Rows[0]["TERM2"].ToString();
                    result.TextBox15 = dsDGV_6.Tables[0].Rows[0]["SEAL2"].ToString();
                    result.TextBox16 = dsDGV_6.Tables[0].Rows[0]["STRIP2"].ToString();
                    result.TextBox17 = dsDGV_6.Tables[0].Rows[0]["CCH_W2"].ToString();
                    result.TextBox18 = dsDGV_6.Tables[0].Rows[0]["ICH_W2"].ToString();
                    result.LEAD_SCN = dsDGV_6.Tables[0].Rows[0]["LEAD_SCN"].ToString();
                    result.TextBox22 = dsDGV_6.Tables[0].Rows[0]["W_LINK"].ToString();
                    result.TextBox23 = dsDGV_6.Tables[0].Rows[0]["WR_NO"].ToString();
                    result.TextBox24 = dsDGV_6.Tables[0].Rows[0]["WIRE_NM"].ToString();
                    result.TextBox25 = dsDGV_6.Tables[0].Rows[0]["W_Diameter"].ToString();
                    result.TextBox26 = dsDGV_6.Tables[0].Rows[0]["W_Color"].ToString();
                    result.TextBox27 = dsDGV_6.Tables[0].Rows[0]["W_Length"].ToString();
                    result.TextBox20 = dsDGV_6.Tables[0].Rows[0]["T1NO"].ToString();
                    result.TextBox21 = dsDGV_6.Tables[0].Rows[0]["T2NO"].ToString();
                    result.TextBox29 = dsDGV_6.Tables[0].Rows[0]["BUNDLE_QTY"].ToString();
                }

                // Phần 2: Lấy danh sách subpart - sử dụng truy vấn đơn giản
                string DGV_DATA7 = @"
            SELECT 
                torder_lead_bom_spst.S_PART_IDX,
                torder_lead_bom_spst.S_LR
            FROM 
                torder_lead_bom_spst
            WHERE 
                torder_lead_bom_spst.M_PART_IDX = (
                    SELECT torder_lead_bom.LEAD_INDEX 
                    FROM torder_lead_bom 
                    WHERE torder_lead_bom.LEAD_PN = '" + LEAD_NO6 + @"'
                )";

                DataSet dsDGV_7 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA7);

                // Khởi tạo danh sách subpart
                result.DataGridView7 = new List<SuperResultTranfer>();

                if (dsDGV_7.Tables.Count > 0 && dsDGV_7.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDGV_7.Tables[0].Rows.Count; i++)
                    {
                        var partIdx = dsDGV_7.Tables[0].Rows[i]["S_PART_IDX"].ToString();

                        // Truy vấn bổ sung để lấy LEAD_PN
                        string partNoQuery = "SELECT LEAD_PN FROM torder_lead_bom WHERE LEAD_INDEX = " + partIdx;
                        DataSet partNoResult = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, partNoQuery);

                        string partNo = "";
                        if (partNoResult.Tables.Count > 0 && partNoResult.Tables[0].Rows.Count > 0)
                        {
                            partNo = partNoResult.Tables[0].Rows[0]["LEAD_PN"].ToString();
                        }

                        SuperResultTranfer item = new SuperResultTranfer
                        {
                            NO = (i + 1).ToString(),
                            PARTNO = partNo,
                            PART_CODE = dsDGV_7.Tables[0].Rows[i]["S_LR"].ToString()
                        };
                        result.DataGridView7.Add(item);
                    }
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DGV_LOAD error: {ex.Message}");
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> CheckPart(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string tspart_IDX = BaseParameter.ListSearchString[0];

                string DGV_DATA90 = "SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + tspart_IDX + "' ";
                DataSet dsDGV_90 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA90);

                bool hasData = dsDGV_90.Tables.Count > 0 && dsDGV_90.Tables[0].Rows.Count > 0;
                result.Message = hasData ? "true" : "false";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> AddSubPart(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox28 = BaseParameter.ListSearchString[0]; // LEAD_NO mới cần thêm

                string DGV_DATA99 = "SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + TextBox28 + "'";
                DataSet dsDGV_99 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA99);

                // Trả về kết quả với thông tin của lead nếu tìm thấy
                result.DataGridView7 = new List<SuperResultTranfer>();
                if (dsDGV_99.Tables.Count > 0 && dsDGV_99.Tables[0].Rows.Count > 0)
                {
                    // Lead tồn tại, trả về một đối tượng SuperResultTranfer mới
                    SuperResultTranfer newItem = new SuperResultTranfer();
                    newItem.NO = "NEW";
                    newItem.PARTNO = TextBox28;
                    newItem.PART_CODE = "L";

                    result.DataGridView7.Add(newItem);
                }

                result.Success = true;
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

        // Fix trong SaveLead method - chỉ cần thay đổi 2 dòng

        public virtual async Task<BaseResult> SaveLead(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox6 = (BaseParameter.ListSearchString[0] ?? "").Trim();
                string TextBox7 = (BaseParameter.ListSearchString[1] ?? "").Trim();
                string TextBox19 = (BaseParameter.ListSearchString[2] ?? "").Trim();
                string TextBox8 = (BaseParameter.ListSearchString[3] ?? "").Trim();
                string TextBox9 = (BaseParameter.ListSearchString[4] ?? "").Trim();
                string TextBox10 = (BaseParameter.ListSearchString[5] ?? "").Trim();
                string TextBox11 = (BaseParameter.ListSearchString[6] ?? "").Trim();
                string TextBox12 = (BaseParameter.ListSearchString[7] ?? "").Trim();
                string TextBox13 = (BaseParameter.ListSearchString[8] ?? "").Trim();
                string TextBox14 = (BaseParameter.ListSearchString[9] ?? "").Trim();
                string TextBox15 = (BaseParameter.ListSearchString[10] ?? "").Trim();
                string TextBox16 = (BaseParameter.ListSearchString[11] ?? "").Trim();
                string TextBox17 = (BaseParameter.ListSearchString[12] ?? "").Trim();
                string TextBox18 = (BaseParameter.ListSearchString[13] ?? "").Trim();
                string TextBox22 = (BaseParameter.ListSearchString[14] ?? "").Trim();
                string TextBox23 = (BaseParameter.ListSearchString[15] ?? "").Trim();
                string TextBox24 = (BaseParameter.ListSearchString[16] ?? "").Trim();
                string TextBox25 = (BaseParameter.ListSearchString[17] ?? "").Trim();
                string TextBox26 = (BaseParameter.ListSearchString[18] ?? "").Trim();
                string TextBox27 = (BaseParameter.ListSearchString[19] ?? "").Trim();
                string TextBox20 = (BaseParameter.ListSearchString[20] ?? "").Trim();
                string TextBox21 = (BaseParameter.ListSearchString[21] ?? "").Trim();
                string TextBox29 = (BaseParameter.ListSearchString[22] ?? "").Trim();
                string DS_YN = (BaseParameter.ListSearchString[23] ?? "").Trim();
                string LEAD_YN = (BaseParameter.ListSearchString[24] ?? "").Trim();
                string C_USER = (BaseParameter.USER_IDX ?? "").Trim();

                // ✅ FIX: Default empty string to "0" for numeric fields
                if (string.IsNullOrWhiteSpace(TextBox11)) TextBox11 = "0";
                if (string.IsNullOrWhiteSpace(TextBox16)) TextBox16 = "0";
                if (string.IsNullOrWhiteSpace(TextBox27)) TextBox27 = "0";
                if (string.IsNullOrWhiteSpace(TextBox29)) TextBox29 = "0";
                if (string.IsNullOrWhiteSpace(TextBox19)) TextBox19 = "0";

                string sqlInsert = "INSERT INTO torder_lead_bom " +
                    "(`LEAD_PN`, `W_PN_IDX`, `T1_PN_IDX`, `S1_PN_IDX`, `T2_PN_IDX`, `S2_PN_IDX`, `STRIP1`, `STRIP2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`, `LEAD_SCN`, `W_LINK`, `WR_NO`, `WIRE_NM`, `W_Diameter`, `W_Color`, `W_Length`, `T1NO`, `T2NO`, `BUNDLE_SIZE`) " +
                    "VALUES ('" + TextBox6.Trim() + "', " +
                    "IFNULL((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + TextBox8 + "'), NULL), " +
                    "IFNULL((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + TextBox9 + "'),NULL), " +
                    "IFNULL((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + TextBox10 + "'),NULL), " +
                    "IFNULL((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + TextBox14 + "'),NULL), " +
                    "IFNULL((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + TextBox15 + "'),NULL), " +

                    // ✅ FIX: Dùng string trực tiếp, không Convert.ToInt32()
                    "'" + TextBox11 + "', '" + TextBox16 + "', " +

                    "'" + TextBox12 + "', '" + TextBox13 + "', '" + TextBox17 + "', '" + TextBox18 + "', '" + DS_YN + "', NOW(), '" + C_USER + "', '" + LEAD_YN + "', " +
                    "'" + TextBox22 + "', " +
                    "'" + TextBox23 + "', " +
                    "'" + TextBox24 + "', " +
                    "'" + TextBox25 + "', " +
                    "'" + TextBox26 + "', " +
                    "'" + TextBox27 + "', " +
                    "'" + TextBox20 + "', " +
                    "'" + TextBox21 + "', " +
                    "'" + TextBox29 + "') " +
                    "ON DUPLICATE KEY UPDATE " +
                    "`W_PN_IDX` = VALUES(`W_PN_IDX`), " +
                    "`T1_PN_IDX` = VALUES(`T1_PN_IDX`), " +
                    "`S1_PN_IDX` = VALUES(`S1_PN_IDX`), " +
                    "`T2_PN_IDX` = VALUES(`T2_PN_IDX`), " +
                    "`S2_PN_IDX` = VALUES(`S2_PN_IDX`), " +
                    "`STRIP1` = VALUES(`STRIP1`), " +
                    "`STRIP2` = VALUES(`STRIP2`), " +
                    "`CCH_W1` = VALUES(`CCH_W1`), " +
                    "`ICH_W1` = VALUES(`ICH_W1`), " +
                    "`CCH_W2` = VALUES(`CCH_W2`), " +
                    "`ICH_W2` = VALUES(`ICH_W2`), " +
                    "`DSCN_YN` = VALUES(`DSCN_YN`), " +
                    "`UPDATE_DTM`= VALUES(`CREATE_DTM`), " +
                    "`UPDATE_USER`=  VALUES(`CREATE_USER`), " +
                    "`LEAD_SCN` = VALUES(`LEAD_SCN`), " +
                    "`W_LINK` = VALUES(`W_LINK`), " +
                    "`WR_NO` = VALUES(`WR_NO`), " +
                    "`WIRE_NM` = VALUES(`WIRE_NM`), " +
                    "`W_Diameter` = VALUES(`W_Diameter`), " +
                    "`W_Color` = VALUES(`W_Color`), " +
                    "`W_Length` = VALUES(`W_Length`), " +
                    "`T1NO` = VALUES(`T1NO`), " +
                    "`T2NO` = VALUES(`T2NO`), " +
                    "`BUNDLE_SIZE` = VALUES(`BUNDLE_SIZE`)";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsert);

                string trackMasterSql = "INSERT INTO trackmaster (`LEAD_NO`,  `HOOK_RACK`, `SFTY_STK`, `CREATE_DTM`, `CREATE_USER`) " +
                    "VALUES ('" + TextBox6.Trim() + "', '" + TextBox7 + "', '" + TextBox19 + "', NOW(), '" + C_USER + "') " +
                    "ON DUPLICATE KEY UPDATE " +
                    "`HOOK_RACK` = '" + TextBox7 + "', " +
                    "`SFTY_STK` = '" + TextBox19 + "', " +
                    "`UPDATE_DTM`= NOW(), " +
                    "`UPDATE_USER`= '" + C_USER + "'";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, trackMasterSql);

                string inventorySql = "INSERT INTO tiivtr_lead (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) " +
                    "(SELECT `torder_lead_bom`.`LEAD_INDEX`, '3', '0', NOW(), '" + C_USER + "' FROM `torder_lead_bom` LEFT JOIN `tiivtr_lead` ON `torder_lead_bom`.`LEAD_INDEX` = `tiivtr_lead`.`PART_IDX`  WHERE `tiivtr_lead`.`PART_IDX` IS NULL) " +
                    "ON DUPLICATE KEY UPDATE " +
                    "`UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + C_USER + "'";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, inventorySql);

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `torder_lead_bom` AUTO_INCREMENT= 1");
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `trackmaster` AUTO_INCREMENT= 1");
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tiivtr_lead` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                    // Bỏ qua lỗi reset AUTO_INCREMENT
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> SaveSubPart(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox6 = BaseParameter.ListSearchString[0];
                string C_USER = BaseParameter.USER_IDX;

                List<SuperResultTranfer> subPartItems = BaseParameter.SubPartItems;

                foreach (var item in subPartItems)
                {
                    string KK = (item.Coln1 ?? "").Trim();  
                    string GGG = (item.Coln2 ?? "").Trim(); 
                    string LHRH = (item.Coln3 ?? "").Trim(); 

                    switch (KK)
                    {
                        case "NEW":
                            string SQL2 = "INSERT INTO `torder_lead_bom_spst` (`M_PART_IDX`, `S_PART_IDX`, `RQR_MENT`, `S_LR`, `CREATE_DTM`, `CREATE_USER`) " +
                                "(SELECT (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + TextBox6 + "'), " +
                                "(SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + GGG + "'), '1', '" + LHRH + "', NOW(), '" + C_USER + "') " +
                                "ON DUPLICATE KEY UPDATE  `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + C_USER + "' ";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL2);
                            break;

                        case "DEL":
                            string SQL3 = "DELETE FROM `torder_lead_bom_spst` " +
                                "WHERE  `S_PART_IDX`= (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + GGG + "') AND " +
                                "`M_PART_IDX`= (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + TextBox6 + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL3);
                            break;

                        case "EDIT":
                            string SQL4 = "UPDATE `torder_lead_bom_spst`   SET   `S_LR`='" + LHRH + "'   " +
                                "WHERE  `S_PART_IDX`= (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + GGG + "') AND " +
                                "`M_PART_IDX`= (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = '" + TextBox6 + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL4);
                            break;
                    }
                }

                try
                {
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `torder_lead_bom_spst` AUTO_INCREMENT= 1");
                }
                catch (Exception)
                {
                    // Bỏ qua lỗi reset AUTO_INCREMENT
                }

                result.Success = true;
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

        // Phương thức MAG_BACODE giữ nguyên cách chuyển tiếp đến Buttonfind_Click với Action = 5
        public virtual async Task<BaseResult> MAG_BACODE(BaseParameter BaseParameter)
        {
            // Redirect to Buttonfind_Click with Action = 5
            BaseParameter.Action = 5;
            return await Buttonfind_Click(BaseParameter);
        }
    }
}
