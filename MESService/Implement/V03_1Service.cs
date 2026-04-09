

namespace MESService.Implement
{
    public class V03_1Service : BaseService<torderlist, ItorderlistRepository>
    , IV03_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public V03_1Service(ItorderlistRepository torderlistRepository

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
                    if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 2)
                    {
                        string AAA = BaseParameter.ListSearchString[0];
                        string Label7 = BaseParameter.ListSearchString[1];

                        string sql = @"SELECT `D`.`CMPNY_IDX`, `D`.`CMPNY_NM`, `D`.`CMPNY_DVS`,
                                    `A`.`PN_NM`, IFNULL(`A`.`COST`, 0) AS `COST`, `A`.`PD_COST_DATE`,
                                    `D`.`CMPNY_NO`, `D`.`CMPNY_ADDR`, `D`.`CMPNY_TEL`, `D`.`CMPNY_FAX`, `D`.`CMPNY_MNGR`, `D`.`CMPNY_RMK`,
                                    IFNULL(TIMESTAMPDIFF(DAY, `A`.`PD_COST_DATE`, NOW()), 0) AS `DAY`
                
                                    FROM PDCMPNY `D` LEFT JOIN
                                    (SELECT pdpart.`PN_NM`, pdpart.`PN_V`, pdpart.`PSPEC_V`, pdpart.`PN_K`, pdpart.`PSPEC_K`, 
                                    (SELECT PDCDNM.`CD_NM_VN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_V`,
                                    (SELECT PDCDNM.`CD_NM_HAN` FROM PDCDNM WHERE PDCDNM.`CD_IDX` = pdpart.`PUNIT_IDX`) AS `UNIT_K`,
                                    pdpart.`PQTY`, PD_PART_COST.`PD_COST_DATE`, IFNULL(PD_PART_COST.`PD_COST`, 0) AS `COST`, 
                                    PD_PART_COST.`CMPNY_IDX`, pdpart.`PUNIT_IDX`
                                    FROM pdpart LEFT JOIN PD_PART_COST ON pdpart.`pdpart_IDX` = PD_PART_COST.`pdpart_IDX`
                                    WHERE pdpart.`PN_DSCN_YN` = 'Y' AND pdpart.`PN_NM` = '" + Label7 + "') `A` ON `A`.`CMPNY_IDX` = `D`.`CMPNY_IDX` WHERE `D`.`CMPNY_NM` LIKE '%" + AAA + "%' OR `D`.`CMPNY_ADDR` LIKE '%" + AAA + "%' OR `D`.`CMPNY_MNGR` LIKE '%" + AAA + "%' ORDER BY `A`.`COST` DESC, `A`.`PN_NM`, `D`.CMPNY_NM";
        

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

