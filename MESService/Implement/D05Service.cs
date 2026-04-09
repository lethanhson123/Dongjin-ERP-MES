

namespace MESService.Implement
{
    public class D05Service : BaseService<torderlist, ItorderlistRepository>
    , ID05Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D05Service(ItorderlistRepository torderlistRepository

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
                result.ListSuperResultTranfer = await SearchData(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<List<SuperResultTranfer>> SearchData(BaseParameter baseParameter)
        {
            var rs = new List<SuperResultTranfer>();
            DateTime currentDate = DateTime.ParseExact(baseParameter.DateTimePicker1, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int w_count = 1 - (int)currentDate.DayOfWeek;

            string D2_DT = currentDate.AddDays(w_count).ToString("yyyy-MM-dd");
            string D2_PNO = baseParameter.PartNo;
            string D2_PNM = baseParameter.PartName;

            string query = $@"
SELECT 
(SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `A`.`PART_IDX`) AS `PART_NO`,
(SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `A`.`PART_IDX`) AS `PART_NAME`,
`DVC`,
IFNULL(CAST(SUM(CASE WHEN WEEK(`A`.`M_DT`, 1) = `A`.`WEEK` THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `W1`,
IFNULL(CAST(SUM(CASE WHEN WEEK(DATE_ADD(`A`.`M_DT`, INTERVAL 7 DAY), 1) = `A`.`WEEK` THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `W2`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 0 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D00`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 1 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D01`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 2 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D02`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 3 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D03`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 4 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D04`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 5 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D05`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 6 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D06`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 7 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D07`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 8 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D08`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 9 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D09`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 10 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D10`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 11 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D11`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 12 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D12`,
IFNULL(CAST(SUM(CASE WHEN `A`.`DT` = DATE_ADD(`A`.`M_DT`, INTERVAL 13 DAY) THEN `A`.`QTY` END) AS CHAR(11)), '-') AS `D13`
FROM
(
    SELECT 
        `TDD_PPD_PNIDX` AS `PART_IDX`,
        `TDD_PPD_DT` AS `DT`,
        IF(`TDD_PPD_DVC` = 'C', 'CUSTOMER', 'DJG') AS `DVC`,
        `TDD_PPD_QTY` AS `QTY`,
        `TDD_PPD_BEQTY` AS `BE_QTY`,
        `TDD_PPD_DVC` AS `W0`,
        WEEK(`TDD_PPD_DT`, 1) AS `WEEK`,
        '{D2_DT}' AS `M_DT`
    FROM `tdd_poplan_djg`
    WHERE `TDD_PPD_DT` >= '{D2_DT}' AND `TDD_PPD_DT` <= DATE_ADD(`TDD_PPD_DT`, INTERVAL 13 DAY)
          AND `TDD_DSCN_YN` = 'Y'

    UNION

    SELECT 
        `TDD_PP_PNIDX` AS `PART_IDX`,
        `TDD_PP_DT` AS `DT`,
        'CUSTOMER' AS `DVC`,
        `TDD_PP_QTY` AS `QTY`,
        `TDD_PP_NTQTY` AS `BE_QTY`,
        'C' AS `W0`,
        WEEK(`TDD_PP_DT`, 1) AS `WEEK`,
        '{D2_DT}' AS `M_DT`
    FROM `tdd_poplan`
    WHERE `TDD_PP_DT` >= '{D2_DT}' AND `TDD_PP_DT` <= DATE_ADD(`TDD_PP_DT`, INTERVAL 13 DAY)
          AND `TDD_DSCN_YN` = 'Y'
    ORDER BY `DT`
) `A`
GROUP BY `A`.`PART_IDX`, `DVC`
HAVING `PART_NO` LIKE '%{D2_PNO}%' AND `PART_NAME` LIKE '%{D2_PNM}%'

                ";

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

            foreach (DataTable dt in ds.Tables)
            {
                rs.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }

            return rs;
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

