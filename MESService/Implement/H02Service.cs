namespace MESService.Implement
{
    public class H02Service : BaseService<torderlist, ItorderlistRepository>
    , IH02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H02Service(ItorderlistRepository torderlistRepository

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
                // Lấy danh sách mã máy cho ComboBox1
                string sqlMcName = @"SELECT `TSNON_OPER_MCNM` FROM TSNON_OPER 
                                     WHERE CAST(RIGHT((SELECT TSCODE.CD_NM_HAN FROM TSCODE 
                                     WHERE TSCODE.CDGR_IDX ='8' AND TSCODE.CD_NM_EN = `TSNON_OPER_MCNM`), 3) AS UNSIGNED) < 500
                                     GROUP BY `TSNON_OPER_MCNM`";

                DataSet dsMcName = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlMcName);
                result.ComboBox1 = SQLHelper.ToList<SuperResultTranfer>(dsMcName.Tables[0]);

                // Lấy danh sách mã không hoạt động cho ComboBox2
                string sqlCode = "SELECT `TSNON_OPER_CODE` FROM TSNON_OPER GROUP BY `TSNON_OPER_CODE`";
                DataSet dsCode = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlCode);
                result.ComboBox2 = SQLHelper.ToList<SuperResultTranfer>(dsCode.Tables[0]);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> CB_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string filter = "";
                switch (BaseParameter.ComboBox5)
                {
                    case "ALL":
                        filter = "";
                        break;
                    case "KOMAX":
                        filter = "HAVING TSNON_OPER_MCNM LIKE 'A%'";
                        break;
                    case "TWIST":
                        filter = "HAVING TSNON_OPER_MCNM LIKE 'Z1%'";
                        break;
                    case "WELDING":
                        filter = "HAVING TSNON_OPER_MCNM LIKE 'S1%'";
                        break;
                }

                string sqlMcName = $@"SELECT `TSNON_OPER_MCNM` FROM TSNON_OPER 
                                      WHERE CAST(RIGHT((SELECT TSCODE.CD_NM_HAN FROM TSCODE 
                                      WHERE TSCODE.CDGR_IDX ='8' AND TSCODE.CD_NM_EN = `TSNON_OPER_MCNM`), 3) AS UNSIGNED) < 500
                                      GROUP BY `TSNON_OPER_MCNM` {filter}";

                DataSet dsMcName = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlMcName);
                result.ComboBox1 = SQLHelper.ToList<SuperResultTranfer>(dsMcName.Tables[0]);
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
                if (BaseParameter.TabSelected == "TabPage1")
                {
                    // Xử lý cho tab biểu đồ (VB_CHART)
                    string dtAA = DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyyMMdd") + BaseParameter.ComboBox3 + "00";
                    string dtBB = DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyyMMdd") + BaseParameter.ComboBox4 + "00";
                    string d3Code = BaseParameter.ComboBox2 == "ALL" ? "" : BaseParameter.ComboBox2;

                    // DataGridView2
                    string sqlData4 = $@"SELECT 'Minute' AS `TIMEH02`,
                SUM(CASE WHEN TSNON_OPER_MCNM='A501' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A501',
                SUM(CASE WHEN TSNON_OPER_MCNM='A502' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A502',
                SUM(CASE WHEN TSNON_OPER_MCNM='A801' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A801',
                SUM(CASE WHEN TSNON_OPER_MCNM='A802' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A802',
                SUM(CASE WHEN TSNON_OPER_MCNM='A803' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A803',
                SUM(CASE WHEN TSNON_OPER_MCNM='A804' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A804',
                SUM(CASE WHEN TSNON_OPER_MCNM='A805' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A805',
                SUM(CASE WHEN TSNON_OPER_MCNM='A806' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A806',
                SUM(CASE WHEN TSNON_OPER_MCNM='A807' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A807',
                SUM(CASE WHEN TSNON_OPER_MCNM='A808' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A808',
                SUM(CASE WHEN TSNON_OPER_MCNM='A809' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A809',
                SUM(CASE WHEN TSNON_OPER_MCNM='A810' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A810',
                SUM(CASE WHEN TSNON_OPER_MCNM='A811' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A811',
                SUM(CASE WHEN TSNON_OPER_MCNM='A812' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A812',
                SUM(CASE WHEN TSNON_OPER_MCNM='A813' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A813',
                SUM(CASE WHEN TSNON_OPER_MCNM='A814' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A814',
                SUM(CASE WHEN TSNON_OPER_MCNM='A815' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A815',
                SUM(CASE WHEN TSNON_OPER_MCNM='A816' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A816',
                SUM(CASE WHEN (TSNON_OPER_MCNM LIKE 'A8%' OR TSNON_OPER_MCNM LIKE 'A5%') THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'SUM'
                FROM TSNON_OPER WHERE DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{dtAA}' AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{dtBB}' AND (`TSNON_OPER_MCNM` LIKE 'A8%' OR TSNON_OPER_MCNM LIKE 'A5%')";

                    // DataGridView3
                    string sqlDataSub = $@"SELECT '{d3Code}' AS `CODEH02`,
                SUM(CASE WHEN TSNON_OPER_MCNM='A501' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A501',
                SUM(CASE WHEN TSNON_OPER_MCNM='A502' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A502',
                SUM(CASE WHEN TSNON_OPER_MCNM='A801' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A801',
                SUM(CASE WHEN TSNON_OPER_MCNM='A802' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A802',
                SUM(CASE WHEN TSNON_OPER_MCNM='A803' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A803',
                SUM(CASE WHEN TSNON_OPER_MCNM='A804' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A804',
                SUM(CASE WHEN TSNON_OPER_MCNM='A805' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A805',
                SUM(CASE WHEN TSNON_OPER_MCNM='A806' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A806',
                SUM(CASE WHEN TSNON_OPER_MCNM='A807' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A807',
                SUM(CASE WHEN TSNON_OPER_MCNM='A808' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A808',
                SUM(CASE WHEN TSNON_OPER_MCNM='A809' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A809',
                SUM(CASE WHEN TSNON_OPER_MCNM='A810' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A810',
                SUM(CASE WHEN TSNON_OPER_MCNM='A811' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A811',
                SUM(CASE WHEN TSNON_OPER_MCNM='A812' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A812',
                SUM(CASE WHEN TSNON_OPER_MCNM='A813' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A813',
                SUM(CASE WHEN TSNON_OPER_MCNM='A814' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A814',
                SUM(CASE WHEN TSNON_OPER_MCNM='A815' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A815',
                SUM(CASE WHEN TSNON_OPER_MCNM='A816' THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'A816',
                SUM(CASE WHEN (TSNON_OPER_MCNM LIKE 'A8%' OR TSNON_OPER_MCNM LIKE 'A5%') THEN (TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))/60 END) AS 'SUM'
                FROM TSNON_OPER WHERE DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{dtAA}' AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{dtBB}' AND (`TSNON_OPER_MCNM` LIKE 'A8%' OR TSNON_OPER_MCNM LIKE 'A5%') AND TSNON_OPER_CODE LIKE '%{d3Code}%'";

                    DataSet ds4 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlData4);
                    DataSet dsSub = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlDataSub);

                    result.DataGridView2 = SQLHelper.ToList<SuperResultTranfer>(ds4.Tables[0]);
                    result.DataGridView3 = SQLHelper.ToList<SuperResultTranfer>(dsSub.Tables[0]);
                }
                else if (BaseParameter.TabSelected == "TabPage2")
                {
                    // Xử lý cho tab chi tiết (DATA_LOAD)
                    string comm = BaseParameter.ComboBox5 ?? "ALL";
                    string com1 = BaseParameter.ComboBox1 ?? "ALL";
                    string com2 = BaseParameter.ComboBox2 == "ALL" ? "" : BaseParameter.ComboBox2;
                    string dtAA = DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyyMMdd") + BaseParameter.ComboBox3.Replace(":", "") + "00";
                    string dtBB = DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyyMMdd") + BaseParameter.ComboBox4.Replace(":", "") + "00";
                    string userNm = BaseParameter.TextBox1 ?? "";

                    switch (comm)
                    {
                        case "ALL": com1 = com1 == "ALL" ? "" : com1; break;
                        case "KOMAX": com1 = com1 == "ALL" ? "A" : com1; break;
                        case "TWIST": com1 = com1 == "ALL" ? "Z1" : com1; break;
                        case "WELDING": com1 = com1 == "ALL" ? "S1" : com1; break;
                    }

                    string sqlData = BaseParameter.CheckBox1 == true
       ? $@"SELECT `TSNON_OPER_CODE` AS `CODEH02`, `TSNON_OPER_DATE` AS `DATE`, `TSNON_OPER_USERNM` AS `S_USER`, `TSNON_OPER_MCNM` AS `MC_NAME`, `TSNON_OPER_STIME` AS `S_TIME`, `TSNON_OPER_ETIME` AS `E_TIME`,
TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) AS `TIME_S`, FORMAT(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) / 60, 2) AS `TIME_M`, `UPDATE_USER` AS `E_USER`
FROM TSNON_OPER
WHERE `TSNON_OPER_MCNM` LIKE '{com1}%' AND TSNON_OPER_CODE LIKE '%{com2}%' AND `TSNON_OPER_USERNM` LIKE '%{userNm}%' AND
DATE_FORMAT(`TSNON_OPER_DATE`, '%Y') = '{DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyy")}'"
       : $@"SELECT `TSNON_OPER_CODE` AS `CODEH02`, `TSNON_OPER_DATE` AS `DATE`, `TSNON_OPER_USERNM` AS `S_USER`, `TSNON_OPER_MCNM` AS `MC_NAME`, `TSNON_OPER_STIME` AS `S_TIME`, `TSNON_OPER_ETIME` AS `E_TIME`,
TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) AS `TIME_S`, FORMAT(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) / 60, 2) AS `TIME_M`, `UPDATE_USER` AS `E_USER`
FROM TSNON_OPER
WHERE `TSNON_OPER_MCNM` LIKE '{com1}%' AND
`TSNON_OPER_STIME` >= '{DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyy-MM-dd")} {BaseParameter.ComboBox3}:00' AND 
`TSNON_OPER_ETIME` <= '{DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyy-MM-dd")} {BaseParameter.ComboBox4}:00' AND
TSNON_OPER_CODE LIKE '%{com2}%' AND `TSNON_OPER_USERNM` LIKE '%{userNm}%'";

                    string sqlDataSum = BaseParameter.CheckBox1 == true
        ? $@"SELECT `TSNON_OPER_MCNM` AS `MC_NAME`,
ROUND(SUM(FORMAT(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) / 60, 2)), 1) AS `TIME_M`
FROM TSNON_OPER
WHERE CAST(RIGHT((SELECT TSCODE.CD_NM_HAN FROM TSCODE WHERE TSCODE.CDGR_IDX ='8' AND TSCODE.CD_NM_EN = `TSNON_OPER_MCNM`), 3) AS UNSIGNED) < 500 AND
DATE_FORMAT(`TSNON_OPER_DATE`, '%Y') = '{DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyy")}' AND
TSNON_OPER_CODE LIKE '%{com2}%' AND `TSNON_OPER_MCNM` LIKE '{com1}%'
GROUP BY `TSNON_OPER_MCNM`
ORDER BY `TSNON_OPER_MCNM`"
        : $@"SELECT `TSNON_OPER_MCNM` AS `MC_NAME`,
ROUND(SUM(FORMAT(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) / 60, 2)), 1) AS `TIME_M`
FROM TSNON_OPER
WHERE CAST(RIGHT((SELECT TSCODE.CD_NM_HAN FROM TSCODE WHERE TSCODE.CDGR_IDX ='8' AND TSCODE.CD_NM_EN = `TSNON_OPER_MCNM`), 3) AS UNSIGNED) < 500 AND
`TSNON_OPER_STIME` >= '{DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyy-MM-dd")} {BaseParameter.ComboBox3}:00' AND 
`TSNON_OPER_ETIME` <= '{DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyy-MM-dd")} {BaseParameter.ComboBox4}:00' AND
TSNON_OPER_CODE LIKE '%{com2}%' AND `TSNON_OPER_MCNM` LIKE '{com1}%'
GROUP BY `TSNON_OPER_MCNM`
ORDER BY `TSNON_OPER_MCNM`";

                    DataSet dsData = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlData);
                    DataSet dsDataSum = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlDataSum);

                    result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(dsData.Tables[0]);
                    result.DataGridView4 = SQLHelper.ToList<SuperResultTranfer>(dsDataSum.Tables[0]);
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

