namespace Service.Implement
{
    public class trackmtimService : BaseService<BOM, IBOMRepository>
    , ItrackmtimService
    {
        private readonly IBOMRepository _BOMRepository;
        private readonly IMembershipRepository _MembershipRepository;

        public trackmtimService(IBOMRepository bOMRepository, IMembershipRepository membershipRepository) : base(bOMRepository)
        {
            _BOMRepository = bOMRepository;
            _MembershipRepository = membershipRepository;
        }
        public virtual async Task<BaseResult<trackmtim>> GetByCompanyID_LeadNo_FinishGoodsToListAsync(BaseParameter<trackmtim> BaseParameter)
        {
            var result = new BaseResult<trackmtim>();
            result.List = new List<trackmtim>();
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Name))
            {
                BaseParameter.Name = BaseParameter.Name.Trim();
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                //string sql = @"update trackmtim set FinishGoodsCode='' where RACKCODE NOT IN ('OUTPUT') AND FinishGoodsCode IS NULL";
                //await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                //sql = @"update trackmtim set POCode='' where RACKCODE NOT IN ('OUTPUT') AND POCode IS NULL";
                //await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                //sql = @"update trackmtim set ECN='' where RACKCODE NOT IN ('OUTPUT') AND ECN IS NULL";
                //await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM='" + BaseParameter.Name + "' AND FinishGoodsCode='" + BaseParameter.Code + "' AND ECN='" + BaseParameter.CreateUserName + "' AND POCode='" + BaseParameter.CreateUserCode + "'";
                if (BaseParameter.Active == true)
                {
                    sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM='" + BaseParameter.Name + "'";
                }
                //if (!string.IsNullOrEmpty(BaseParameter.Code))
                //{
                //    sql = sql + " AND FinishGoodsCode='" + BaseParameter.Code + "'";
                //}
                //if (!string.IsNullOrEmpty(BaseParameter.CreateUserName))
                //{
                //    sql = sql + " AND ECN='" + BaseParameter.CreateUserName + "'";
                //}
                //if (!string.IsNullOrEmpty(BaseParameter.CreateUserCode))
                //{
                //    sql = sql + " AND POCode='" + BaseParameter.CreateUserCode + "'";
                //}
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtrackmtim = new List<trackmtim>();
                if (ds.Tables.Count > 0)
                {
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                }
                if (Listtrackmtim.Count > 0)
                {
                    var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && o.MaterialCode == BaseParameter.Name).ToListAsync();
                    if (ListBOM.Count > 0)
                    {
                        var FinishGoodsList = string.Join(",", ListBOM.Select(o => o.ParentName).Distinct().ToList());
                        for (int i = 0; i < Listtrackmtim.Count; i++)
                        {
                            Listtrackmtim[i].FinishGoodsList = FinishGoodsList;
                        }
                    }
                    result.List = Listtrackmtim;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<trackmtim>> SaveByListID_PO_FinishGoods_ECNAsync(BaseParameter<trackmtim> BaseParameter)
        {
            var result = new BaseResult<trackmtim>();
            if (BaseParameter.ListID != null && BaseParameter.ListID.Count > 0 && BaseParameter.MembershipID > 0)
            {
                var ListTRACK_IDX = string.Join(",", BaseParameter.ListID);
                var Membership = await _MembershipRepository.GetByIDAsync(BaseParameter.MembershipID.Value);
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                if (!string.IsNullOrEmpty(BaseParameter.Display))
                {
                    BaseParameter.Display = BaseParameter.Display.Trim();
                    string sql = @"update trackmtim set FinishGoodsCode='" + BaseParameter.Display + "', UpdateDate=NOW(), UpdateUserID=" + Membership.ID + ", UpdateUserCode='" + Membership.UserName + "', UpdateUserName='" + Membership.Name + "' where TRACK_IDX in (" + ListTRACK_IDX + ")";
                    await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                }
                if (!string.IsNullOrEmpty(BaseParameter.UpdateUserCode))
                {
                    BaseParameter.UpdateUserCode = BaseParameter.UpdateUserCode.Trim();
                    string sql = @"update trackmtim set POCode='" + BaseParameter.UpdateUserCode + "' , UpdateDate=NOW(), UpdateUserID=" + Membership.ID + ", UpdateUserCode='" + Membership.UserName + "', UpdateUserName='" + Membership.Name + "' where TRACK_IDX in (" + ListTRACK_IDX + ")";
                    await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                }
                if (!string.IsNullOrEmpty(BaseParameter.UpdateUserName))
                {
                    BaseParameter.UpdateUserName = BaseParameter.UpdateUserName.Trim();
                    string sql = @"update trackmtim set ECN='" + BaseParameter.UpdateUserName + "' , UpdateDate=NOW(), UpdateUserID=" + Membership.ID + ", UpdateUserCode='" + Membership.UserName + "', UpdateUserName='" + Membership.Name + "' where TRACK_IDX in (" + ListTRACK_IDX + ")";
                    await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<trackmtim>> GetByCompanyID_LEADNM_Begin_EndToListAsync(BaseParameter<trackmtim> BaseParameter)
        {
            var result = new BaseResult<trackmtim>();
            result.List = new List<trackmtim>();
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Code))
            {
                BaseParameter.DateBegin = BaseParameter.DateBegin ?? GlobalHelper.InitializationDateTime;
                BaseParameter.DateEnd = BaseParameter.DateEnd ?? GlobalHelper.InitializationDateTime;

                BaseParameter.DateBegin = new DateTime(BaseParameter.DateBegin.Value.Year, BaseParameter.DateBegin.Value.Month, BaseParameter.DateBegin.Value.Day, 0, 0, 0);
                BaseParameter.DateEnd = new DateTime(BaseParameter.DateEnd.Value.Year, BaseParameter.DateEnd.Value.Month, BaseParameter.DateEnd.Value.Day, 23, 59, 59);

                var DateBegin = BaseParameter.DateBegin.Value.ToString("yyyy-MM-dd HH:mm:ss");
                var DateEnd = BaseParameter.DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

                BaseParameter.Code = BaseParameter.Code.Trim();

                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                string sql = GlobalHelper.InitializationString;
                if (BaseParameter.Active == true)
                {
                    sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') AND LEAD_NM in ('" + BaseParameter.Code + "') ORDER BY RACKDTM DESC";
                }
                else
                {
                    sql = @"select * from trackmtim where (RACKDTM BETWEEN '" + DateBegin + "' AND '" + DateEnd + "') AND LEAD_NM in ('" + BaseParameter.Code + "') ORDER BY RACKDTM DESC";
                }
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtrackmtim = new List<trackmtim>();
                if (ds.Tables.Count > 0)
                {
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                }
                result.List = Listtrackmtim;
            }
            return result;
        }
        public virtual async Task<BaseResult<trackmtim>> SaveAsync(BaseParameter<trackmtim> BaseParameter)
        {
            var result = new BaseResult<trackmtim>();
            if (BaseParameter.CompanyID > 0)
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.RACKCODE == "INPUT")
                    {
                        string sql = @"update trackmtim set RACKCODE='" + BaseParameter.BaseModel.RACKCODE + "', RACKOUT_DTM=null, RACKOUT_YN=null where TRACK_IDX in (" + BaseParameter.BaseModel.TRACK_IDX + ")";
                        await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                    }
                    if (BaseParameter.BaseModel.RACKCODE == "OUTPUT")
                    {
                        string sql = @"update trackmtim set RACKCODE='" + BaseParameter.BaseModel.RACKCODE + "', RACKOUT_DTM=NOW(), RACKOUT_YN='Y' where TRACK_IDX in (" + BaseParameter.BaseModel.TRACK_IDX + ")";
                        await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                    }
                }
            }
            return result;
        }
    }
}

