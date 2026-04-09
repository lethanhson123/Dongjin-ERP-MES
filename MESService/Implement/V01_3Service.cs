namespace MESService.Implement
{
    public class V01_3Service : BaseService<torderlist, ItorderlistRepository>
    , IV01_3Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V01_3Service(ItorderlistRepository torderlistRepository
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
                string sql = @"SELECT  `PDP_NO`  FROM  PDPUSCH  WHERE  `PDP_CONF` = 'Report'  AND  `PDP_REC_YN` = 'Y'   GROUP BY `PDP_NO`";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> ComboBox1_SelectedIndexChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {                   
                    var ComboBox1= BaseParameter.SearchString;
                    string sql = @"SELECT 
                        `A`.`PDP_CONF`,`A`.PDP_NO, `A`.`PDP_DATE1`, (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEP`,
                        `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                        `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, `B`.`PQTY`, `A`.`PDP_QTY`,
                        `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `NAME`, `A`.`PDP_PART`,
                        IFNULL((SELECT `QTY` FROM pd_tiivtr WHERE `PART_IDX` = `A`.`PDP_PART` AND `A`.`PDPUSCH_IDX` = `ORDER_IDX`),0) AS `STOCK`, `A`.`PDPUSCH_IDX`, 
                        IFNULL(`A`.`PDP_COST`, 0) AS `PDP_COST`, 
                        IFNULL((`A`.`PDP_COST` * `A`.`PDP_QTY`), 0) AS `SUM_COST`,
                        IFNULL(`A`.`PDP_VAT`, 0) AS `PDP_VAT`,
                        IFNULL(`A`.`PDP_ECTCOST`, 0) AS `PDP_ECTCOST`, 
                        IFNULL(`A`.`PDP_TOTCOST`, 0) AS `PDP_TOTCOST`, 
                        IFNULL(`A`.`PDP_BE_COST`, 0) AS `PDP_BE_COST`,
                        IFNULL(`A`.`PDP_CMPY`, '') AS `PDP_CMPY`, IFNULL((SELECT CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = `A`.`PDP_CMPY`),'') AS `COMP_NM`
                        FROM PDPUSCH `A` LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                        WHERE `A`.`PDP_REC_YN` ='Y' AND `A`.`PDP_CONF` = 'Report' AND `PDP_CNF_YN` = 'N' AND `A`.PDP_NO = '" + ComboBox1 + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

