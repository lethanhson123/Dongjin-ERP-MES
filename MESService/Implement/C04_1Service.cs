

namespace MESService.Implement
{
    public class C04_1Service : BaseService<torderlist, ItorderlistRepository>
    , IC04_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C04_1Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var Label5 = BaseParameter.ListSearchString[0];
                        var Label6 = BaseParameter.ListSearchString[1];
                        var Label7 = BaseParameter.ListSearchString[2];
                        var Label8 = BaseParameter.ListSearchString[3];
                        var Label1 = BaseParameter.ListSearchString[4];
                        

                        string sql = @"UPDATE `TORDERLIST` SET `TERM1` = '" + Label5 + "' , `SEAL1` = '" + Label6 + "', `TERM2` = '" + Label7 + "' , `SEAL2` = '" + Label8 + "' , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "' WHERE  `ORDER_IDX`= '" + Label1 + "'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO TORDERLIST_LP (`ORDER_IDX`, `MC`, `TOT_QTY`, `PERFORMN_L`, `PERFORMN_R`, `CONDITION`, `CREATE_DTM`, `CREATE_USER`)
                        SELECT `A`.`ORDER_IDX`, 'C00' AS `MC`, `A`.`TOT_QTY`, 0, 0, 'Stay', NOW(), 'MES' 
                        FROM TORDERLIST `A` LEFT JOIN TORDERLIST_LP `B` ON `A`.`ORDER_IDX` = `B`.`ORDER_IDX` 
                        WHERE (`A`.DSCN_YN = 'Y') AND NOT (`A`.`CONDITION` = 'Close') AND (`A`.`TERM1` LIKE '(%' OR `A`.`TERM2` LIKE '(%') AND `B`.`ORDER_IDX` IS NULL  AND `A`.`TOT_QTY` > 0
                        AND  (IF(NOT(`A`.`TERM1`) = '(899997)', IF(INSTR(`A`.`TERM1`, ')') >0, 2, IF(NOT(`A`.`TERM2`) ='(899997)', IF(INSTR(`A`.`TERM2`, ')')>0, 2, 0), IF(NOT(`A`.`TERM2`) ='(899997)', IF(INSTR(`A`.`TERM2`, ')') >0, 2, 0),0))),0) + 
                        IF(NOT(`A`.`TERM1`) = '(899998)', IF(INSTR(`A`.`TERM1`, ')') >0, 2, IF(NOT(`A`.`TERM2`) ='(899998)', IF(INSTR(`A`.`TERM2`, ')')>0, 2, 0), IF(NOT(`A`.`TERM2`) ='(899998)', IF(INSTR(`A`.`TERM2`, ')') >0, 2, 0),0))),0) +
                        IF(NOT(`A`.`TERM1`) = '(899999)', IF(INSTR(`A`.`TERM1`, ')') >0, 2, IF(NOT(`A`.`TERM2`) ='(899999)', IF(INSTR(`A`.`TERM2`, ')')>0, 2, 0), IF(NOT(`A`.`TERM2`) ='(899999)', IF(INSTR(`A`.`TERM2`, ')') >0, 2, 0),0))),1) ) = 6";
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

