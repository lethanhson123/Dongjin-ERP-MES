

namespace MESService.Implement
{
    public class C06Service : BaseService<torderlist, ItorderlistRepository>
    , IC06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C06Service(ItorderlistRepository torderlistRepository

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
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 1)
                    {

                    }
                    if (BaseParameter.Action == 2)
                    {
                        var AA = BaseParameter.SearchString;
                        string sql = @"SELECT
                        `AAA`.`CHK`, `AAA`.`OR_NO`, `AAA`.`PO_DT`, `AAA`.`WORK_WEEK`,
                         `AAA`.`FCTRY_NM`, `AAA`.`TORDER_FG`,  `AAA`.`LEAD_NO`, `AAA`.`PO_QTY`, `AAA`.`SAFTY_QTY`,
                        `AAA`.`PO_QTY` - IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`PO_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`PO_QTY`, 0, 
		                        `AAA`.`PO_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) AS `USED_STOCK`,

                        IF(`AAA`.`OR_NO` = 'EVENT', `AAA`.`TOEXCEL_QTY`,

                        IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`PO_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`PO_QTY`, 0, 
		                        `AAA`.`PO_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) ) + `AAA`.`SAFTY_QTY` AS `MES_ORDER`,
                        `AAA`.`MC`, `AAA`.`BUNDLE_SIZE`, `AAA`.`LEAD_COUNT`,  `AAA`.`REP`, `AAA`.`ORDER_IDX`
                        FROM (
                        SELECT
                        `AA`.`CHK`, `AA`.`OR_NO`, `AA`.`WORK_WEEK`, `AA`.`FCTRY_NM`, `AA`.`TORDER_FG`,  `AA`.`LEAD_NO`, `AA`.`PO_QTY`, `AA`.`SAFTY_QTY`, 
                        `AA`.`STCK_CHK`, `AA`.`STOCK`, 
                        `AA`.`ACT_STOCK`,
                        IF(`AA`.`ROWNO` = 1, IF((`AA`.`ACT_STOCK`-`AA`.`PO_QTY`) > 0, `AA`.`ACT_STOCK`-`AA`.`PO_QTY`, 0), 
                            IF((IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`PO_QTY`) > 0, 
                                   (IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`PO_QTY`), 0)) AS `BE_DATE`,

                        `AA`.`ROWNO`, `AA`.`MC`, `AA`.`BUNDLE_SIZE`, `AA`.`LEAD_COUNT`, `AA`.`PO_DT`, `AA`.`REP`, `AA`.`ORDER_IDX`,  `AA`.`TOEXCEL_QTY`


                        FROM (
                        SELECT 
                        `A`.`CHK`, `A`.`OR_NO`, `A`.`WORK_WEEK`, `A`.`FCTRY_NM`, `A`.`TORDER_FG`,  `A`.`LEAD_NO`, `A`.`PO_QTY`, `A`.`SAFTY_QTY`, 
                        `A`.`STCK_CHK`, 
                        IFNULL(`A`.`STOCK`, 0) AS `STOCK`, 
                        IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) AS `ACT_STOCK`,
                        IFNULL(IF((IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`PO_QTY`) > 0, (IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`PO_QTY`), 0), 0) AS `STOCK_DTE`,
                        `A`.`ROWNO`, 
                        `A`.`MC`, `A`.`BUNDLE_SIZE`, `A`.`LEAD_COUNT`, `A`.`PO_DT`, `A`.`REP`, `A`.`ORDER_IDX`, `A`.`TOEXCEL_QTY`
                        FROM (
                        SELECT  'FALSE' AS `CHK`, `OR_NO`, `WORK_WEEK`, `TORDER_FG`,  `FCTRY_NM`, `LEAD_NO`, `PO_QTY`, `SAFTY_QTY`, 
                        IF((SELECT tiivtr_lead_fg.`QTY` FROM tiivtr_lead_fg
                        WHERE tiivtr_lead_fg.`PART_IDX` = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `LEAD_NO`)) > 0, 'Y', 'N') AS `STCK_CHK`,

                        (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead  WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`)) AS `STOCK`,
                        RANK() OVER (PARTITION BY `LEAD_NO` ORDER BY `LEAD_NO`, `TORDER_FG`) AS `ROWNO`, 
                        `MC`,  IF(`BUNDLE_SIZE` = 0 , (SELECT torder_lead_bom.`BUNDLE_SIZE` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`), `BUNDLE_SIZE`)   AS `BUNDLE_SIZE`,
                        (SELECT COUNT(M_PART_IDX) FROM torder_lead_bom_spst WHERE `M_PART_IDX` = (SELECT LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`)) AS `LEAD_COUNT`,
                        TORDERLIST_SPST.`PO_DT`, `REP`, `ORDER_IDX`, `TOEXCEL_QTY`
                        FROM     TORDERLIST_SPST

                        WHERE  `CREATE_DTM` = '" + AA + "'  AND `DSCN_YN` = 'N' ) `A` ORDER BY `A`.`LEAD_NO`, `A`.`TORDER_FG`, `A`.`ROWNO`  ) `AA`   ) `AAA`  ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        var AA = BaseParameter.SearchString;
                        string sql = @"SELECT
                        `AAA`.`CHK`, `AAA`.`OR_NO`, `AAA`.`PO_DT`, `AAA`.`WORK_WEEK`,
                         `AAA`.`FCTRY_NM`, `AAA`.`TORDER_FG`,  `AAA`.`LEAD_NO`, `AAA`.`PO_QTY`, `AAA`.`SAFTY_QTY`,
                        `AAA`.`PO_QTY` - IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`PO_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`PO_QTY`, 0, 
		                        `AAA`.`PO_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) AS `USED_STOCK`,

                        IF(`AAA`.`OR_NO` = 'EVENT', `AAA`.`TOEXCEL_QTY`,

                        IF(`AAA`.`ROWNO` = 1, IF(`AAA`.`BE_DATE` > 0, 0, `AAA`.`PO_QTY`-`AAA`.`STOCK`), 
		                        IF((LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`))>`AAA`.`PO_QTY`, 0, 
		                        `AAA`.`PO_QTY`-(LAG(`AAA`.`BE_DATE`)  OVER  (ORDER BY `AAA`.`LEAD_NO`, `AAA`.`TORDER_FG`, `AAA`.`ROWNO`)))) ) + `AAA`.`SAFTY_QTY` AS `MES_ORDER`,
                        `AAA`.`MC`, `AAA`.`BUNDLE_SIZE`, `AAA`.`LEAD_COUNT`,  `AAA`.`REP`, `AAA`.`ORDER_IDX`
                        FROM (
                        SELECT
                        `AA`.`CHK`, `AA`.`OR_NO`, `AA`.`WORK_WEEK`, `AA`.`FCTRY_NM`, `AA`.`TORDER_FG`,  `AA`.`LEAD_NO`, `AA`.`PO_QTY`, `AA`.`SAFTY_QTY`, 
                        `AA`.`STCK_CHK`, `AA`.`STOCK`, 
                        `AA`.`ACT_STOCK`,
                        IF(`AA`.`ROWNO` = 1, IF((`AA`.`ACT_STOCK`-`AA`.`PO_QTY`) > 0, `AA`.`ACT_STOCK`-`AA`.`PO_QTY`, 0), 
                            IF((IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`PO_QTY`) > 0, 
                                   (IFNULL(LAG(`AA`.`STOCK_DTE`)  OVER  (ORDER BY `AA`.`LEAD_NO`, `AA`.`TORDER_FG`, `AA`.`ROWNO`), 0) - `AA`.`PO_QTY`), 0)) AS `BE_DATE`,

                        `AA`.`ROWNO`, `AA`.`MC`, `AA`.`BUNDLE_SIZE`, `AA`.`LEAD_COUNT`, `AA`.`PO_DT`, `AA`.`REP`, `AA`.`ORDER_IDX`,  `AA`.`TOEXCEL_QTY`


                        FROM (
                        SELECT 
                        `A`.`CHK`, `A`.`OR_NO`, `A`.`WORK_WEEK`, `A`.`FCTRY_NM`, `A`.`TORDER_FG`,  `A`.`LEAD_NO`, `A`.`PO_QTY`, `A`.`SAFTY_QTY`, 
                        `A`.`STCK_CHK`, 
                        IFNULL(`A`.`STOCK`, 0) AS `STOCK`, 
                        IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) AS `ACT_STOCK`,
                        IFNULL(IF((IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`PO_QTY`) > 0, (IF(`A`.`ROWNO` = 1, IFNULL(`A`.`STOCK`, 0), 0) - `A`.`PO_QTY`), 0), 0) AS `STOCK_DTE`,
                        `A`.`ROWNO`, 
                        `A`.`MC`, `A`.`BUNDLE_SIZE`, `A`.`LEAD_COUNT`, `A`.`PO_DT`, `A`.`REP`, `A`.`ORDER_IDX`, `A`.`TOEXCEL_QTY`
                        FROM (
                        SELECT  'FALSE' AS `CHK`, `OR_NO`, `WORK_WEEK`, `TORDER_FG`,  `FCTRY_NM`, `LEAD_NO`, `PO_QTY`, `SAFTY_QTY`, 
                        IF((SELECT tiivtr_lead_fg.`QTY` FROM tiivtr_lead_fg
                        WHERE tiivtr_lead_fg.`PART_IDX` = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `LEAD_NO`)) > 0, 'Y', 'N') AS `STCK_CHK`,

                        (SELECT tiivtr_lead.`QTY` FROM tiivtr_lead  WHERE tiivtr_lead.`PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`)) AS `STOCK`,
                        RANK() OVER (PARTITION BY `LEAD_NO` ORDER BY `LEAD_NO`, `TORDER_FG`) AS `ROWNO`, 
                        `MC`,  IF(`BUNDLE_SIZE` = 0 , (SELECT torder_lead_bom.`BUNDLE_SIZE` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`), `BUNDLE_SIZE`)   AS `BUNDLE_SIZE`,
                        (SELECT COUNT(M_PART_IDX) FROM torder_lead_bom_spst WHERE `M_PART_IDX` = (SELECT LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `LEAD_NO`)) AS `LEAD_COUNT`,
                        TORDERLIST_SPST.`PO_DT`, `REP`, `ORDER_IDX`, `TOEXCEL_QTY`
                        FROM     TORDERLIST_SPST

                        WHERE  `CREATE_DTM` = '" + AA + "'  AND `DSCN_YN` = 'N' ) `A` ORDER BY `A`.`LEAD_NO`, `A`.`TORDER_FG`, `A`.`ROWNO`  ) `AA`   ) `AAA`  ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string CREATE_USER = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (BaseParameter.DataGridView1 != null)
                            {
                                if (BaseParameter.DataGridView1.Count > 0)
                                {
                                    var DateTimePicker3 = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                                    var FACTORY_NM = BaseParameter.ListSearchString[1];
                                    var Label5 = BaseParameter.ListSearchString[2];
                                    var VALUES = "";
                                    var VALUESSUM = "";
                                    foreach (var item in BaseParameter.DataGridView1)
                                    {
                                        if (item.CHK == true)
                                        {
                                            var AA1 = item.LEAD_NO;
                                            var AA2 = item.ORDER_QTY;
                                            var AA3 = item.SAFETY_QTY == null ? "0" : item.SAFETY_QTY;
                                            var AA4 = item.Machine;
                                            var AA5 = item.BUNDLE_SIZE;
                                            var AA6 = item.REP;
                                            var AA7 = item.ASSY_NO;
                                            if (AA2 == "")
                                            {
                                                AA2 = "0";
                                            }
                                            if (AA3 == "")
                                            {
                                                AA3 = "0";
                                            }
                                            if (AA4 == "")
                                            {
                                                AA4 = "PLAN";
                                            }
                                            if (AA5 == null)
                                            {
                                                AA5 = 0;
                                            }
                                            if (BaseParameter.CheckBox1 == true)
                                            {
                                                VALUES = "('EVENT'," + Label5 + ", '" + DateTimePicker3 + "', '" + AA1 + "', '" + AA2 + "', '" + AA3 + "', '" + AA4 + "', '" + AA5 + "', '0', 'Stay', '0', 'N', 'N', 'N', NOW(), '" + CREATE_USER + "', '" + FACTORY_NM + "', '" + AA6 + "', '" + AA7 + "', '" + AA2 + "')";
                                            }
                                            else
                                            {
                                                VALUES = "('NORMAL'," + Label5 + ", '" + DateTimePicker3 + "', '" + AA1 + "', '" + AA2 + "', '" + AA3 + "', '" + AA4 + "', '" + AA5 + "', '0', 'Stay', '0', 'N', 'N', 'N', NOW(), '" + CREATE_USER + "', '" + FACTORY_NM + "', '" + AA6 + "', '" + AA7 + "', '" + AA2 + "')";
                                            }
                                            if (VALUESSUM == "")
                                            {
                                                VALUESSUM = VALUES;
                                            }
                                            else
                                            {
                                                VALUESSUM = VALUESSUM + ", " + VALUES;
                                            }
                                        }
                                    }
                                    string sql = @"INSERT INTO  `TORDERLIST_SPST` (`OR_NO`, `WORK_WEEK`, `PO_DT`, `LEAD_NO`, `PO_QTY`, `SAFTY_QTY`, `MC`, `BUNDLE_SIZE`, `PERFORMN`, `CONDITION`, `LEAD_COUNT`, `PO_YN`, `DSCN_YN`, `ERROR_YN`, `CREATE_DTM`, `CREATE_USER`, `FCTRY_NM`, `REP`, `TORDER_FG`, `TOEXCEL_QTY`) VALUES ";
                                    sql = sql + VALUESSUM;
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DataGridView3 != null)
                        {
                            if (BaseParameter.DataGridView3.Count > 0)
                            {
                                foreach (var item in BaseParameter.DataGridView3)
                                {
                                    if (item.CHK == true)
                                    {
                                        var AAA = item.MES_ORDER;
                                        var BBB = item.SAFTY_QTY == null ? 0 : item.SAFTY_QTY;
                                        var CCC = item.ORDER_IDX == null ? 0 : item.ORDER_IDX;
                                        string sql = @"UPDATE   TORDERLIST_SPST   SET  `PO_QTY` = '" + AAA + "' , `SAFTY_QTY` ='" + BBB + "',  `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "' WHERE  `ORDER_IDX` = " + CCC;
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.DataGridView2 != null)
                        {
                            if (BaseParameter.DataGridView2.Count > 0)
                            {
                                foreach (var item in BaseParameter.DataGridView2)
                                {
                                    var AAA = item.MES_ORDER;
                                    var BBB = item.SAFTY_QTY == null ? 0 : item.SAFTY_QTY;
                                    var CCC = item.ORDER_IDX == null ? 0 : item.ORDER_IDX;
                                    string sql = @"UPDATE   TORDERLIST_SPST   SET  `PO_QTY` = '" + AAA + "' , `SAFTY_QTY` ='" + BBB + "', `PO_YN`='Y',  `DSCN_YN`='Y',   `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "' WHERE  `ORDER_IDX` = " + CCC;
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 3)
                    {
                        var DEL_AA = BaseParameter.SearchString;
                        string sql = @"DELETE    FROM   TORDERLIST_SPST    WHERE    `CREATE_DTM` = '" + DEL_AA + "'   AND  `PO_YN` = 'N'   AND   `DSCN_YN` = 'N' ";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> COM_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var ORDER_DT = DateTime.Parse(BaseParameter.SearchString).ToString("yyyy-MM-dd");
                    string sql = @"SELECT  `CREATE_DTM`  FROM   TORDERLIST_SPST    WHERE   `PO_DT` = '" + ORDER_DT + "' AND  `DSCN_YN` = 'N' AND `PO_YN` = 'N'   GROUP BY   `CREATE_DTM`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    for (int i = 0; i < result.ComboBox1.Count; i++)
                    {
                        result.ComboBox1[i].Description = result.ComboBox1[i].CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }

            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COM_LIST2(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT  `CREATE_DTM`  FROM   TORDERLIST_SPST    WHERE   `PO_DT` >= DATE_ADD(NOW(), INTERVAL -10 DAY)  AND  `DSCN_YN` = 'N' AND `PO_YN` = 'N'   GROUP BY   `CREATE_DTM`";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox3 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
                for (int i = 0; i < result.ComboBox3.Count; i++)
                {
                    result.ComboBox3[i].Description = result.ComboBox3[i].CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss");
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

