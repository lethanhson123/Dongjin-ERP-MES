namespace MESService.Implement
{
    public class D04_PNO_CHKService : BaseService<torderlist, ItorderlistRepository>
    , ID04_PNO_CHKService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_PNO_CHKService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> PO_CODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                string sql = @"SELECT tdpdotpl.PO_CODE, tdpdotpl.CREATE_DTM
                FROM tdpdotpl
                GROUP BY tdpdotpl.PO_CODE
                ORDER BY  tdpdotpl.PDOTPL_IDX DESC
                LIMIT 10";

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
        public virtual async Task<BaseResult> TextBoxA2_KeyDown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        result.PART_IDX = BaseParameter.ListSearchString[0].ToUpper();
                        var ComboBox1 = BaseParameter.ListSearchString[1];
                        if (BaseParameter.CheckBox1 == true)
                        {
                            var chk_P = result.PART_IDX.Substring(0, 1);
                            if (chk_P == "P")
                            {
                                result.PART_IDX = result.PART_IDX.Substring(1, 24);
                            }
                            else
                            {
                                result.PART_IDX = result.PART_IDX;
                            }
                        }
                        string sql = @"SELECT `A`.`PO_CODE`,
                        (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NO`,
                        (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_GRP`,
                        (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NM`,
                        IFNULL(`PO_QTY`, 0) AS `PO_QTY`, 
                        IFNULL(`NT_QTY`, 0) AS `NT_QTY`, 
                        IFNULL(`MZ`.`PN_QYT`, 0) AS `QTY`, 
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2'), 0) AS `STOCK`,
                        IFNULL(`MZ`.`PN_QYT`, 0 ) AS `PACK_QTY`, 
                        (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) AS `Not_yet_packing`,
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2') - (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )), 0) AS `STATUS`

                        FROM tdpdotpl `A` LEFT JOIN

                        (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                        `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                        FROM tdpdmtim `ZZ`
                        WHERE `ZZ`.`VLID_DSCN_YN` ='Y' 
                        GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                        ON `A`.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                        WHERE `A`.`PO_CODE` = '" + ComboBox1 + "' AND `A`.`PART_IDX` = (SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = '" + result.PART_IDX + "')   ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_PNOCHK_01 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_PNOCHK_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

