namespace MESService.Implement
{
    public class B02Service : BaseService<torderlist, ItorderlistRepository>
    , IB02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public B02Service(ItorderlistRepository torderlistRepository

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
                        result = await SUCHK_Change(BaseParameter);
                    }
                    if (BaseParameter.Action == 2)
                    {
                        string BC_TEXT = BaseParameter.ListSearchString[1];

                        string sql = @"SELECT 
                        (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`)) AS `PART_NO`,
                        (SELECT TMMTIN.`DESC` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`) AS `NAME`,
                        IFNULL(TMBRCD.`BARCD_LOC`, IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`)), '')) AS `LOC`,
                        (TMBRCD.`PKG_QTY` - TMBRCD.`PKG_OUTQTY`) AS `QTY`,
                        TMBRCD.`BARCD_ID`
                        FROM TMBRCD
                        WHERE TMBRCD.`BARCD_ID` ='" + BC_TEXT + "' AND NOT(TMBRCD.`DSCN_YN` = 'Y')";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.ListtmbrcdTranfer01 = new List<tmbrcdTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.ListtmbrcdTranfer01.AddRange(SQLHelper.ToList<tmbrcdTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        string AAA = BaseParameter.ListSearchString[2];
                        bool RadioButton1 = bool.Parse(BaseParameter.ListSearchString[3]);
                        bool RadioButton2 = bool.Parse(BaseParameter.ListSearchString[4]);
                        string WHERE_SQL = GlobalHelper.InitializationString;
                        if (RadioButton1 == true)
                        {
                            WHERE_SQL = "AND  (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`)) LIKE '%" + AAA + "%'   ";
                        }
                        if (RadioButton2 == true)
                        {
                            WHERE_SQL = "AND  IFNULL(TMBRCD.`BARCD_LOC`, (SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`))) LIKE '%" + AAA + "%'   ";
                        }
                        string sql = @"SELECT TMBRCD.`BARCD_ID`, 
                        (SELECT TMMTIN.`MTIN_DTM` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`) AS `DATE`,
                        (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`)) AS `PART_NO`,
                        (SELECT TMMTIN.`DESC` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`) AS `NAME`,
                        IFNULL(TMBRCD.`BARCD_LOC`, (SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`))) AS `LOC`,
                        (TMBRCD.`PKG_QTY` - TMBRCD.`PKG_OUTQTY`) AS `QTY`
                        FROM TMBRCD
                        WHERE NOT(TMBRCD.`DSCN_YN` = 'Y') " + WHERE_SQL + "ORDER BY `DATE` DESC";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.ListtmbrcdTranfer02 = new List<tmbrcdTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.ListtmbrcdTranfer02.AddRange(SQLHelper.ToList<tmbrcdTranfer>(dt));
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        result = await SUCHK_Change(BaseParameter);
                        string TBBarcode = BaseParameter.ListSearchString[1];
                        string sql = @"SELECT  `BARCD_IDX`, `BARCD_ID` FROM TMBRCD   WHERE  `DSCN_YN` = 'N' AND `BARCD_ID` = '" + TBBarcode + "'  AND  `PKG_GRP` = 'Small'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Listtmbrcd = new List<tmbrcd>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Listtmbrcd.AddRange(SQLHelper.ToList<tmbrcd>(dt));
                        }
                        result.BarcodeDGV = new List<B02>();
                        B02 B02 = new B02();
                        B02.Barcode = TBBarcode;
                        if (result.Listtmbrcd.Count > 0)
                        {
                            B02.Result = "Small Barcode";
                        }
                        else
                        {
                            B02.Result = "Not Find";
                        }
                        result.BarcodeDGV.Add(B02);
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListtmbrcdTranfer != null)
                        {
                            if (BaseParameter.ListtmbrcdTranfer.Count > 0)
                            {                                
                                string USER_ID = BaseParameter.USER_ID;
                                string VALUES = GlobalHelper.InitializationString;
                                string VALUESSUM = GlobalHelper.InitializationString;
                                foreach (tmbrcdTranfer tmbrcdTranfer in BaseParameter.ListtmbrcdTranfer)
                                {
                                    if (tmbrcdTranfer.CHK == true)
                                    {
                                        VALUES = tmbrcdTranfer.MTIN_IDX.ToString();
                                        if (VALUESSUM == "")
                                        {
                                            VALUESSUM = VALUES;
                                        }
                                        else
                                        {
                                            VALUESSUM = VALUESSUM + ", " + VALUES;
                                        }

                                        string A = tmbrcdTranfer.PART_IDX.ToString();
                                        string B = tmbrcdTranfer.STOCK.ToString();
                                        string C = tmbrcdTranfer.PKG_QTY.ToString();
                                        string D = tmbrcdTranfer.BARCD_IDX.ToString();
                                        string sql2 = @"INSERT tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + A + ", 1, (SELECT   TMBRCD.PKG_QTY   FROM     TMBRCD   WHERE  `BARCD_IDX` = '" + D + "' ), NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `QTY`= (`QTY` +IFNULL((SELECT TMBRCD.PKG_QTY   FROM     TMBRCD   WHERE  `BARCD_IDX` = '" + D + "') , 0)), `UPDATE_DTM`= NOW(), `UPDATE_USER` = '" + USER_ID + "'  ";
                                        string sql2Result = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);
                                    }
                                }
                                string sql = @"UPDATE TMMTIN Set `DSCN_YN`='Y',`UPDATE_DTM`=NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE `MTIN_IDX`  IN ";
                                sql = sql + "(" + VALUESSUM + ")";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AAA = BaseParameter.ListSearchString[0];
                            string BBB = BaseParameter.ListSearchString[1];
                            string sql = @"UPDATE `TMBRCD` SET `BARCD_LOC`='" + BBB + "' WHERE  `BARCD_ID`= '" + AAA + "'";
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
        private async Task<BaseResult> SUCHK_Change(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null)
                {
                    string Date_bar = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");

                    string sql = @"SELECT(SELECT `PART_NO` FROM tspart WHERE TMMTIN.`PART_IDX` = tspart.`PART_IDX`) AS `PART_NO`,
                    (SELECT `PART_NM` FROM tspart WHERE TMMTIN.`PART_IDX` = tspart.`PART_IDX`) AS `PART_NM`, 
                    TMMTIN.`SNP_QTY` AS `PART_SNP`, TMMTIN.`UTM`,
                    IFNULL((SELECT  `QTY`  FROM     tiivtr  WHERE  `LOC_IDX` = '1' AND `PART_IDX` = TMMTIN.`PART_IDX`), 0) AS `STOCK`,
                    TMMTIN.`QTY`, TMBRCD.`PKG_QTY`, TMMTIN.`NET_WT`, TMMTIN.`GRS_WT`, TMMTIN.`PLET_NO`, TMMTIN.`SHPD_NO`, TMMTIN.`MTIN_DTM`,
                    TMMTIN.`DSCN_YN`, TMMTIN.`MTIN_RMK`, TMMTIN.`CREATE_DTM`, TMMTIN.`CREATE_USER`, TMMTIN.`UPDATE_DTM`, TMMTIN.`UPDATE_USER`,
                    TMBRCD.`BARCD_ID`, TMMTIN.`PART_IDX`, TMMTIN.`BRCD_PRNT`, TMBRCD.`MTIN_IDX`, TMBRCD.`BARCD_IDX`, FALSE AS `CHK`
                    FROM TMBRCD, TMMTIN
                    WHERE TMBRCD.`MTIN_IDX` = TMMTIN.`MTIN_IDX` AND TMMTIN.DSCN_YN = 'N' AND TMBRCD.DSCN_YN = 'N' AND
                    TMMTIN.`MTIN_DTM` = '" + Date_bar + "' AND TMBRCD.`PKG_GRP` = 'Large'  AND TMMTIN.`QTY` > 0";

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
    }
}

