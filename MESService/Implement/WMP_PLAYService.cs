namespace MESService.Implement
{
	public class WMP_PLAYService : BaseService<tsmenu, ItsmenuRepository>
	, IWMP_PLAYService
	{
		private readonly ItsmenuRepository _tsmenuRepository;
		public WMP_PLAYService(ItsmenuRepository tsmenuRepository

			) : base(tsmenuRepository)
		{
			_tsmenuRepository = tsmenuRepository;
		}
		public override void Initialization(tsmenu model)
		{
			BaseInitialization(model);
		}
		public virtual async Task<BaseResult> Load(BaseParameter BaseParameter)
		{
			BaseResult result = new BaseResult();
			try
			{
				if (BaseParameter != null)
				{
					if (!string.IsNullOrEmpty(BaseParameter.SearchString))
					{
						string FLNM = BaseParameter.SearchString;
						string sql = @"SELECT CONCAT(ZT_HELP_DB.TAB_NAME, ' Rev(', ZT_HELP_DB.REV_NO, ')') AS `LIST`, ZT_HELP_DB.FILE_NM, ZT_HELP_DB.FILE_EX  
							FROM ZT_HELP_DB    WHERE  ZT_HELP_DB.DN_YN ='Y'  AND  ZT_HELP_DB.MENU_NM = '" + FLNM + "'";

						DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
						result.DGV_WMP_PLIST = new List<SuperResultTranfer>();
						for (int i = 0; i < ds.Tables.Count; i++)
						{
							DataTable dt = ds.Tables[i];
							result.DGV_WMP_PLIST.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

