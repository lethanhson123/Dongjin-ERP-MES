
namespace MESService.Implement
{
    public class V01Service : BaseService<torderlist, ItorderlistRepository>
    , IV01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01Service(ItorderlistRepository torderlistRepository
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
                result = await COMBO_LIST();
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
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var S_AAA = BaseParameter.ListSearchString[0];
                            var S_CCC = BaseParameter.ListSearchString[1];
                            var S_DDD = BaseParameter.ListSearchString[2];
                            if (BaseParameter.RadioButton7 == true)
                            {
                                S_DDD = "Y";
                            }
                            if (BaseParameter.RadioButton6 == true)
                            {
                                S_DDD = "N";
                            }
                            string sql = @"SELECT `PDPART_IDX`, `PN_V`, `PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_VN`,
                            `PN_K`, `PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_KR`,
                            `PQTY`, `PN_NM`, `CREATE_DTM`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE `USER_ID` = pdpart.`CREATE_USER`) AS `CREATE_USER`, 
                             `UPDATE_DTM`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE `USER_ID` = pdpart.`UPDATE_USER`) AS `UPDATE_USER`, `PN_DSCN_YN`, `PN_PHOTO`  
                             FROM pdpart WHERE  `PN_GROUP` = 'Normal' AND (`PN_V` LIKE '%" + S_AAA + "%' OR `PN_K` LIKE '%" + S_AAA + "%' OR `PSPEC_V` LIKE '%" + S_AAA + "%' OR `PSPEC_K` LIKE '%" + S_AAA + "%') AND `PN_NM` LIKE '%" + S_CCC + "' AND  `PN_DSCN_YN` = '" + S_DDD + "'  ";

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
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var S_AAA = BaseParameter.ListSearchString[0];
                            var S_BBB = BaseParameter.ListSearchString[1];
                            var S_FFF = BaseParameter.ListSearchString[2];
                            var S_DDD = BaseParameter.ListSearchString[3];
                            if (BaseParameter.RadioButton8 == true)
                            {
                                S_DDD = "Y";
                            }
                            if (BaseParameter.RadioButton9 == true)
                            {
                                S_DDD = "N";
                            }
                            string sql = @"SELECT `PDPART_IDX`,  `PN_K`, `PSPEC_V`, `PSPEC_K`,  `PN_V`,  (SELECT   CD_NM_VN   FROM PDCDNM WHERE PDCDNM.CD_IDX = pdpart.PUNIT_IDX) AS `UNIT_EN`,
                             `PQTY`, `PN_NM`, `CREATE_DTM`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE `USER_ID` = `CREATE_USER`) AS `CREATE_USER`, 
                             `UPDATE_DTM`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE `USER_ID` = pdpart.`UPDATE_USER`) AS `UPDATE_USER`, `PN_DSCN_YN`, `PN_PHOTO`  
                             FROM pdpart  WHERE  `PN_GROUP` = 'ME' AND (`PN_K` LIKE '%" + S_AAA + "%' OR `PN_V` LIKE '%" + S_AAA + "%'  OR `PSPEC_K` LIKE '%" + S_AAA + "%') AND `PN_NM` LIKE '%" + S_FFF + "%' AND  `PSPEC_V` LIKE '%" + S_BBB + "%'   AND `PN_DSCN_YN` = '" + S_DDD + "'  ";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var BBB = BaseParameter.ListSearchString[1];
                            var CCC = BaseParameter.ListSearchString[2];

                            string sql = @"SELECT  FALSE AS `CHK`, `CMPNY_IDX`, `CMPNY_NM`, IFNULL(`CMPNY_DVS`, '') AS `CMPNY_DVS`, `CMPNY_NO`, `CMPNY_ADDR`, `CMPNY_TEL`, `CMPNY_FAX`, 
                            IFNULL(`CMPNY_MNGR`, '') AS `CMPNY_MNGR`, `CMPNY_RMK`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`
                            FROM     PDCMPNY  WHERE  `CMPNY_NM` LIKE '%" + BBB + "%' AND IFNULL(`CMPNY_DVS`, '') LIKE '%" + AAA + "%' AND IFNULL(`CMPNY_MNGR`, '') LIKE '%" + CCC + "%'";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_01 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var S_AAA = BaseParameter.ListSearchString[0];
                            var S_BBB = BaseParameter.ListSearchString[1];
                            var S_CCC = BaseParameter.ListSearchString[2];
                            var CM = int.Parse(BaseParameter.ListSearchString[3]);
                            var S_CCC_SQL = "";
                            if (S_CCC == "")
                            {
                                S_CCC_SQL = "";
                            }
                            else
                            {
                                S_CCC_SQL = "  AND `PN_NM` = '" + S_CCC + "'  ";
                            }
                            var DAT_SQL = "";
                            switch (CM)
                            {
                                case 0:
                                    DAT_SQL = "";
                                    break;
                                case 1:
                                    DAT_SQL = " AND TIMESTAMPDIFF(DAY, `PD_COST_DATE`, NOW()) <= 30";
                                    break;
                                case 2:
                                    DAT_SQL = " AND TIMESTAMPDIFF(DAY, `PD_COST_DATE`, NOW()) <= 60";
                                    break;
                                case 3:
                                    DAT_SQL = " AND TIMESTAMPDIFF(DAY, `PD_COST_DATE`, NOW()) <= 90";
                                    break;
                            }
                            var COMP_TEXT = "";
                            if (S_BBB.Length > 0)
                            {
                                COMP_TEXT = "  HAVING  `Company` = '" + S_BBB + "'   ";
                            }
                            string sql = @"SELECT
                                pdpart.`PN_NM`, pdpart.`PN_V`, pdpart.`PSPEC_V`, pdpart.`PN_K`, pdpart.`PSPEC_K`, 
                                (SELECT PDCDNM.`CD_NM_VN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_V`,
                                (SELECT PDCDNM.`CD_NM_HAN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_K`,
                                pdpart.`PQTY`, `TA`.`PD_COST_DATE`, IFNULL(`TA`.`PD_COST`, 0) AS `COST`, 
                                (SELECT PDCMPNY.`CMPNY_NM` FROM PDCMPNY WHERE PDCMPNY.`CMPNY_IDX` = `TA`.`CMPNY_IDX`) AS `Company`,
                                `TA`.`CREATE_DTM`, `TA`.`CREATE_USER`, `TA`.`UPDATE_DTM`, `TA`.`UPDATE_USER`,
                                `TA`.`CMPNY_IDX`, pdpart.`PUNIT_IDX`
                                FROM   pdpart   LEFT JOIN    

                                (SELECT `TAA`.`PDPART_IDX`, `TAA`.`CMPNY_IDX`, `TAA`.`PD_COST_DATE`, `TAA`.`PD_COST`,
                                `TAA`.`CREATE_DTM`, `TAA`.`CREATE_USER`, `TAA`.`UPDATE_DTM`, `TAA`.`UPDATE_USER`
                                FROM ( SELECT 
                                ROW_NUMBER() OVER (PARTITION BY PD_PART_COST.`PDPART_IDX`, PD_PART_COST.`CMPNY_IDX` ORDER BY PD_PART_COST.`PD_COST_DATE` DESC) AS `RUM`,
                                PD_PART_COST.`PDPART_IDX`, PD_PART_COST.`CMPNY_IDX`, PD_PART_COST.`PD_COST_DATE`, PD_PART_COST.`PD_COST`,
                                PD_PART_COST.`CREATE_DTM`, PD_PART_COST.`CREATE_USER`, PD_PART_COST.`UPDATE_DTM`, PD_PART_COST.`UPDATE_USER`
                                FROM  PD_PART_COST) `TAA`
                                WHERE `TAA`.`RUM` = '1') `TA` ON pdpart.`PDPART_IDX` = `TA`.`PDPART_IDX` WHERE pdpart.`PN_DSCN_YN` = 'Y'  AND (`PN_V` LIKE '%" + S_AAA + "%' OR `PN_K` LIKE '%" + S_AAA + "%' OR `PSPEC_V` LIKE '%" + S_AAA + "%' OR `PSPEC_K` LIKE '%" + S_AAA + "%') " + S_CCC_SQL + DAT_SQL + COMP_TEXT;

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.V01_DMPS_COST_DataGridView3 = new List<V01_DMPS_COST>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.V01_DMPS_COST_DataGridView3.AddRange(SQLHelper.ToList<V01_DMPS_COST>(dt));
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
                        string USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0].ToString();
                            var BBB = BaseParameter.ListSearchString[1].ToString();
                            var CCC = BaseParameter.ListSearchString[2].ToString();
                            var DDD = BaseParameter.ListSearchString[3].ToString();
                            var PART_ADD05 = BaseParameter.ListSearchString[4].ToString();
                            var EEE = PART_ADD05.Trim();
                            var FFF = BaseParameter.ListSearchString[5].ToString();
                            var GGG = BaseParameter.ListSearchString[6].ToString();
                            var HHH = "";
                            var KKK = BaseParameter.ListSearchString[7].ToString();
                            string sql = "";
                            DataSet ds = new DataSet();
                            if (PART_ADD05 == "DJG-???")
                            {
                                sql = @"SELECT IFNULL(MAX(CAST((SUBSTRING_INDEX(`PN_NM`, '-', -1)) AS UNSIGNED)) + 1, 1)  AS `NO` FROM pdpart   WHERE   `PN_GROUP` = 'Normal'";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                var DGV_C0V_NO = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    DGV_C0V_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (DGV_C0V_NO.Count > 0)
                                {
                                    EEE = "DJG-" + DGV_C0V_NO[0].NO;
                                }
                            }

                            if (BaseParameter.RadioButton4 == true)
                            {
                                HHH = "Y";
                            }
                            if (BaseParameter.RadioButton5 == true)
                            {
                                HHH = "N";
                            }

                            sql = @"INSERT INTO `pdpart` (`PN_V`, `PSPEC_V`, `PN_K`, `PSPEC_K`, `PN_NM`, `PUNIT_IDX`, `PQTY`, `CREATE_DTM`, `CREATE_USER`, `PN_DSCN_YN`, `PN_GROUP`, `PN_PHOTO`)  VALUES ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + FFF + "', '" + GGG + "', NOW(), '" + USER_ID + "', '" + HHH + "', 'Normal', '" + KKK + "')  ON DUPLICATE KEY UPDATE  `PQTY` = VALUES(`PQTY`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`), `PN_V`= '" + AAA + "', `PSPEC_V` = '" + BBB + "', `PN_K` = '" + CCC + "', `PSPEC_K`= '" + DDD + "', `PN_DSCN_YN` = VALUES(`PN_DSCN_YN`), `PN_GROUP` = VALUES(`PN_GROUP`)";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO `pdpart_ADDLIST` (`PN_V_AL`, `PSPEC_V_AL`, `PN_K_AL`, `PSPEC_K_AL`, `PN_NM_AL`, `PQTY_AL`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "', NOW(), '" + USER_ID + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `pdpart`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        string USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0].ToString();
                            var BBB = BaseParameter.ListSearchString[1].ToString();
                            var CCC = BaseParameter.ListSearchString[2].ToString();
                            var DDD = BaseParameter.ListSearchString[3].ToString();
                            var T2_PN_NM = BaseParameter.ListSearchString[4].ToString();
                            var EEE = T2_PN_NM.Trim();
                            var FFF = "9";
                            var GGG = "1";
                            var HHH = "N";
                            var KKK = BaseParameter.ListSearchString[5].ToString();

                            string sql = "";
                            DataSet ds = new DataSet();
                            if (T2_PN_NM == "DJGMC-???")
                            {
                                sql = @"SELECT IFNULL(MAX(CAST((SUBSTRING_INDEX(`PN_NM`, '-', -1)) AS UNSIGNED)) + 1, 1)  AS `NO` FROM pdpart   WHERE    `PN_GROUP` = 'ME'";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                var DGV_C0V_NO = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    DGV_C0V_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (DGV_C0V_NO.Count > 0)
                                {
                                    EEE = "DJGMC-" + DGV_C0V_NO[0].NO;
                                }
                            }
                            if (BaseParameter.RadioButton14 == true)
                            {
                                HHH = "Y";
                            }
                            if (BaseParameter.RadioButton13 == true)
                            {
                                HHH = "N";
                            }

                            sql = @"INSERT INTO `pdpart` (`PN_V`, `PSPEC_V`, `PN_K`, `PSPEC_K`, `PN_NM`, `PUNIT_IDX`, `PQTY`, `CREATE_DTM`, `CREATE_USER`, `PN_DSCN_YN`, `PN_GROUP`, `PN_PHOTO`) VALUES ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + FFF + "', '" + GGG + "', NOW(), '" + USER_ID + "', '" + HHH + "', 'ME', '" + KKK + "') ON DUPLICATE KEY UPDATE  `PQTY` = VALUES(`PQTY`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`), `PN_V`= '" + AAA + "', `PSPEC_V` = '" + BBB + "', `PN_K` = '" + CCC + "', `PSPEC_K`= '" + DDD + "', `PN_DSCN_YN` = VALUES(`PN_DSCN_YN`), `PN_GROUP` = VALUES(`PN_GROUP`)";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO `pdpart_ADDLIST` (`PN_V_AL`, `PSPEC_V_AL`, `PN_K_AL`, `PSPEC_K_AL`, `PN_NM_AL`, `PQTY_AL`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "', NOW(), '" + USER_ID + "')";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `pdpart`     AUTO_INCREMENT= 1";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        string USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0].ToString();
                            var BBB = BaseParameter.ListSearchString[1].ToString();
                            var CCC = BaseParameter.ListSearchString[2].ToString();
                            var DDD = BaseParameter.ListSearchString[3].ToString();
                            var EEE = BaseParameter.ListSearchString[4].ToString();
                            var FFF = BaseParameter.ListSearchString[5].ToString();
                            var GGG = BaseParameter.ListSearchString[6].ToString();
                            var HHH = BaseParameter.ListSearchString[7].ToString();
                            var KKK = BaseParameter.ListSearchString[8].ToString();
                            string sql = "";
                            DataSet ds = new DataSet();

                            if (AAA == "New")
                            {
                                sql = @"INSERT INTO   PDCMPNY  (`CMPNY_NM`, `CMPNY_DVS`, `CMPNY_ADDR`, `CMPNY_TEL`, `CMPNY_FAX`, `CMPNY_MNGR`, `CMPNY_RMK`, `CMPNY_NO`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + BBB + "', '" + CCC + "', '" + DDD + "', '" + EEE + "', '" + FFF + "', '" + GGG + "', '" + HHH + "', '" + KKK + "', NOW(), '" + USER_ID + "')";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            else
                            {
                                sql = @"UPDATE   PDCMPNY   SET  `CMPNY_DVS` = '" + CCC + "', `CMPNY_ADDR` = '" + DDD + "', `CMPNY_TEL` = '" + EEE + "', `CMPNY_FAX` = '" + FFF + "', `CMPNY_MNGR` = '" + GGG + "', `CMPNY_RMK` = '" + HHH + "', `CMPNY_RMK` = '" + KKK + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE(CMPNY_IDX = '" + AAA + "')";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> UpdateKoreanNames(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ImportData != null && BaseParameter.ImportData.Count > 0)
                {
                    foreach (var item in BaseParameter.ImportData)
                    {
                        if (!string.IsNullOrEmpty(item.PN_V) && !string.IsNullOrEmpty(item.PN_K))
                        { 
                            string sql = @"UPDATE pdpart 
                                 SET PN_K = '" + item.PN_K + "' WHERE PN_V = '" + item.PN_V + "' AND PN_GROUP = 'Normal'";
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
        public virtual async Task<BaseResult> COMBO_LIST()
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_VN`, `CD_NM_HAN`  FROM PDCDNM WHERE PDCDNM.CDGR_IDX = '1'";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DGV_V01_CB1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DGV_V01_CB1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var TextBox15 = BaseParameter.ListSearchString[0].ToString();
                        var DGV_Manager = BaseParameter.ListSearchString[1].ToString();
                        var DGV_TEL = BaseParameter.ListSearchString[2].ToString();
                        var DGV_ADDR = BaseParameter.ListSearchString[3].ToString();
                        if (BaseParameter.V01_DMPS_COST_DataGridView3 != null)
                        {
                            if (BaseParameter.V01_DMPS_COST_DataGridView3.Count > 0)
                            {
                                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Excel, "V01_Request_quotation.xlsx");
                                FileInfo fileLocation = new FileInfo(physicalPath);
                                if (fileLocation.Length > 0)
                                {
                                    if (fileLocation.Extension == ".xlsx" || fileLocation.Extension == ".xls")
                                    {
                                        string SheetName = this.GetType().Name;
                                        string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                                        physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                                        fileLocation.CopyTo(physicalPath);
                                        fileLocation = new FileInfo(physicalPath);
                                        using (ExcelPackage package = new ExcelPackage(fileLocation))
                                        {
                                            if (package.Workbook.Worksheets.Count > 0)
                                            {
                                                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                                if (workSheet != null)
                                                {
                                                    workSheet.Cells[4, 3].Value = TextBox15;
                                                    workSheet.Cells[5, 3].Value = DGV_Manager;
                                                    workSheet.Cells[6, 3].Value = DGV_TEL;
                                                    workSheet.Cells[8, 3].Value = DGV_ADDR;
                                                    workSheet.Cells[5, 12].Value = USER_ID;
                                                    var EXCEL_II = 16;
                                                    var no = 1;
                                                    foreach (var item in BaseParameter.V01_DMPS_COST_DataGridView3)
                                                    {
                                                        workSheet.Cells[EXCEL_II, 1].Value = no;
                                                        workSheet.Cells[EXCEL_II, 2].Value = item.PN_V;
                                                        workSheet.Cells[EXCEL_II, 6].Value = item.PN_V;
                                                        workSheet.Cells[EXCEL_II, 10].Value = item.PN_NM;
                                                        workSheet.Cells[EXCEL_II, 12].Value = item.UNIT_V + "(" + item.PQTY + ")";
                                                        no = no + 1;
                                                        EXCEL_II = EXCEL_II + 1;
                                                    }
                                                }
                                            }
                                            package.Save();
                                        }

                                        result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                                    }
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
    }
}

