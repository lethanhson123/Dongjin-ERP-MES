

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MESService.Implement
{
    public class C09_SPCService : BaseService<torderlist, ItorderlistRepository>
    , IC09_SPCService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C09_SPCService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C09_SPC_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C09_START_V3_partbox = BaseParameter.SearchString;
                    string sql = @"SELECT 
                    (SELECT `LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `LEAD_PN`,
                    (SELECT `W_Diameter` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `W_Diameter`
                    FROM torder_lead_bom_spst WHERE torder_lead_bom_spst.`M_PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + C09_START_V3_partbox + "') AND(SELECT `LEAD_SCN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) = 'LEAD'   ";


                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    sql = @"SELECT 
                    MAX(`FORCE_USW_NM`) AS `Coln1`, MAX(`FORCE_USW_MIN_HORI`) AS `Coln2`, MAX(`FORCE_USW_MIN_VERT`) AS `Coln3`
                    FROM TTENSILFORCE_USW WHERE `FORCE_USW_NM` <= (SELECT 
                    MIN((SELECT `W_Diameter` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`)) AS `W_Diameter`
                    FROM torder_lead_bom_spst 
                    WHERE torder_lead_bom_spst.`M_PART_IDX` = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE 
                    torder_lead_bom.LEAD_PN = '" + C09_START_V3_partbox + "') AND(SELECT `LEAD_SCN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) = 'LEAD')";


                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var C09_START_V3_ORDER_NO_TEXT = BaseParameter.ListSearchString[0];
                        var RUS_01 = BaseParameter.ListSearchString[1] == null ? "0" : BaseParameter.ListSearchString[1];
                        var RUS_02 = BaseParameter.ListSearchString[2] == null ? "0" : BaseParameter.ListSearchString[2];
                        var MRUS = BaseParameter.ListSearchString[3];
                        var INSP_TEXT = BaseParameter.ListSearchString[4];                        
                      

                        string sql = @"INSERT INTO `torderinspection_spst` (`ORDER_IDX`, `RES_H`, `RES_V`, `IN_RESILT`, `COLSIP`) VALUES (" + C09_START_V3_ORDER_NO_TEXT + ", " + RUS_01 + ", " + RUS_02 + ", '" + MRUS + "', '" + INSP_TEXT + "') ON DUPLICATE KEY UPDATE `RES_H`= VALUES(`RES_H`), `RES_V`= VALUES(`RES_V`), `IN_RESILT`= VALUES(`IN_RESILT`)";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torderinspection_spst`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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

