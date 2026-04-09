namespace MESService.Implement
{
    public class H04Service : BaseService<torderlist, ItorderlistRepository>, IH04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H04Service(ItorderlistRepository torderlistRepository)
            : base(torderlistRepository)
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
                string SQL_MCNAME = "SELECT `TSNON_OPER_MCNM` FROM TSNON_OPER GROUP BY `TSNON_OPER_MCNM`  HAVING TSNON_OPER_MCNM  LIKE 'ZA8%'";
                DataSet dsSQL_MCNAME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_MCNAME);
                result.ComboBox1 = SQLHelper.ToList<SuperResultTranfer>(dsSQL_MCNAME.Tables[0]);

                string SQL_LCODE = "SELECT `TSNON_OPER_CODE` FROM TSNON_OPER   WHERE `TSNON_OPER_MCNM`  LIKE 'ZA8%'  GROUP BY `TSNON_OPER_CODE`";
                DataSet dsSQL_LCODE = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_LCODE);
                result.ComboBox2 = SQLHelper.ToList<SuperResultTranfer>(dsSQL_LCODE.Tables[0]);
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
                    string DT_AA = DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyyMMdd") + BaseParameter.ComboBox3.Replace(":", "") + "00";
                    string DT_BB = DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyyMMdd") + BaseParameter.ComboBox4.Replace(":", "") + "00";
                    string D3_CODE = BaseParameter.ComboBox2 == "ALL" ? "" : BaseParameter.ComboBox2;

                    string DGV_DATA4 = $@"
SELECT 'Minute' AS `TIME`,
sum(case when TSNON_OPER_MCNM='ZA801' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA801',
sum(case when TSNON_OPER_MCNM='ZA802' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA802',
sum(case when TSNON_OPER_MCNM='ZA803' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA803',
sum(case when TSNON_OPER_MCNM='ZA804' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA804',
sum(case when TSNON_OPER_MCNM='ZA805' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA805',
sum(case when TSNON_OPER_MCNM='ZA806' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA806',
sum(case when TSNON_OPER_MCNM='ZA807' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA807',
sum(case when TSNON_OPER_MCNM='ZA808' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA808',
sum(case when TSNON_OPER_MCNM='ZA809' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA809',
sum(case when TSNON_OPER_MCNM='ZA810' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA810',
sum(case when TSNON_OPER_MCNM LIKE 'ZA8%' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'SUM'
FROM TSNON_OPER WHERE DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{DT_AA}'  AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{DT_BB}' AND  `TSNON_OPER_MCNM` LIKE 'ZA8%'";

                    string DGV_DATA_SUB = $@"
SELECT '{D3_CODE}' AS `CODE`,
sum(case when TSNON_OPER_MCNM='ZA801' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA801',
sum(case when TSNON_OPER_MCNM='ZA802' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA802',
sum(case when TSNON_OPER_MCNM='ZA803' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA803',
sum(case when TSNON_OPER_MCNM='ZA804' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA804',
sum(case when TSNON_OPER_MCNM='ZA805' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA805',
sum(case when TSNON_OPER_MCNM='ZA806' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA806',
sum(case when TSNON_OPER_MCNM='ZA807' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA807',
sum(case when TSNON_OPER_MCNM='ZA808' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA808',
sum(case when TSNON_OPER_MCNM='ZA809' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA809',
sum(case when TSNON_OPER_MCNM='ZA810' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'ZA810',
sum(case when TSNON_OPER_MCNM LIKE 'ZA8%' then (TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME))/60 end) as 'SUM'
FROM TSNON_OPER WHERE DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{DT_AA}'  AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{DT_BB}' AND  `TSNON_OPER_MCNM` LIKE 'ZA8%' AND TSNON_OPER_CODE LIKE '%{D3_CODE}%'";

                    DataSet dsDGV_04 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA4);
                    DataSet dsDGV_SUB = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_SUB);
                    result.DataGridView2 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_04.Tables[0]);
                    result.DataGridView3 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_SUB.Tables[0]);
                }
                else if (BaseParameter.TabSelected == "TabPage2")
                {
                    string COM1 = BaseParameter.ComboBox1 ?? "ALL";
                    string COM2 = BaseParameter.ComboBox2 == "ALL" ? "" : BaseParameter.ComboBox2;
                    string DT_AA = DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyyMMdd") + BaseParameter.ComboBox3.Replace(":", "") + "00";
                    string DT_BB = DateTime.Parse(BaseParameter.DateTimePicker2).ToString("yyyyMMdd") + BaseParameter.ComboBox4.Replace(":", "") + "00";
                    string USERNM = BaseParameter.TextBox1 ?? "";

                    if (COM1 == "ALL") { COM1 = "Z"; }
                    if (COM2 == "ALL") { COM2 = ""; }

                    string SQL_DATA = BaseParameter.CheckBox1 == true
                        ? $@"SELECT `TSNON_OPER_CODE` AS `Coln1`, `TSNON_OPER_DATE` AS `DATE`, `TSNON_OPER_USERNM` AS `S_USER`, `TSNON_OPER_MCNM` AS `MC_NAME`, `TSNON_OPER_STIME` AS `S_TIME`, `TSNON_OPER_ETIME` AS `E_TIME`,
TIMESTAMPDIFF(SECOND,TSNON_OPER_STIME,TSNON_OPER_ETIME)  AS `TIME_S`, format(TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME) / 60, 2)  AS `TIME_M`, `UPDATE_USER` AS `E_USER`
FROM TSNON_OPER
WHERE `TSNON_OPER_MCNM` LIKE '{COM1}%' AND TSNON_OPER_CODE LIKE '%{COM2}%' AND `TSNON_OPER_USERNM` LIKE '%{USERNM}%' AND
DATE_FORMAT(`TSNON_OPER_DATE`, '%Y') = '{DateTime.Parse(BaseParameter.DateTimePicker1).ToString("yyyy")}'"
                        : $@"SELECT `TSNON_OPER_CODE` AS `Coln1`, `TSNON_OPER_DATE` AS `DATE`, `TSNON_OPER_USERNM` AS `S_USER`, `TSNON_OPER_MCNM` AS `MC_NAME`, `TSNON_OPER_STIME` AS `S_TIME`, `TSNON_OPER_ETIME` AS `E_TIME`,
TIMESTAMPDIFF(SECOND,TSNON_OPER_STIME,TSNON_OPER_ETIME)  AS `TIME_S`, FORMAT(TIMESTAMPDIFF(SECOND ,TSNON_OPER_STIME,TSNON_OPER_ETIME) / 60, 2)  AS `TIME_M`, `UPDATE_USER` AS `E_USER`
FROM TSNON_OPER
WHERE `TSNON_OPER_MCNM` LIKE '{COM1}%' AND 
DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{DT_AA}'  AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{DT_BB}' AND
TSNON_OPER_CODE LIKE '%{COM2}%' AND `TSNON_OPER_USERNM` LIKE '%{USERNM}%'";

                    string SQL_DATA_SUM = $@"SELECT `TSNON_OPER_MCNM` AS `MC_NAME`,
ROUND(SUM(FORMAT(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME) / 60, 2)), 1) AS `TIME_M`
FROM TSNON_OPER
WHERE `TSNON_OPER_MCNM` LIKE 'ZA8%'
AND DATE_FORMAT(`TSNON_OPER_STIME`, '%Y%m%d%H%i') >= '{DT_AA}'  
AND DATE_FORMAT(`TSNON_OPER_ETIME`, '%Y%m%d%H%i') <= '{DT_BB}'
GROUP BY `TSNON_OPER_MCNM`
ORDER BY `TSNON_OPER_MCNM`";

                    DataSet dsSQL_DATA = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_DATA);
                    DataSet dsSQL_DATA_SUM = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_DATA_SUM);
                    result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(dsSQL_DATA.Tables[0]);
                    result.DataGridView4 = SQLHelper.ToList<SuperResultTranfer>(dsSQL_DATA_SUM.Tables[0]);
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
