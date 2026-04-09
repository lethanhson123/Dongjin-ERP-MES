namespace MESService.Implement
{
    public class MAINService : BaseService<tsmenu, ItsmenuRepository>
    , IMAINService
    {
        private readonly ItsmenuRepository _tsmenuRepository;
        public MAINService(ItsmenuRepository tsmenuRepository

            ) : base(tsmenuRepository)
        {
            _tsmenuRepository = tsmenuRepository;
        }
        public override void Initialization(tsmenu model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Main_Shown(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (BaseParameter != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.USER_IDX))
                {
                    string sql = @" Select * FROM
                    (SELECT B.AUTH_NM, D.SCRN_PATH, D.DECYN, D.MENU_LVL, D.MENU_CD, IFNULL(C.MENU_AUTH_YN, 'FALSE') AS `MENU_AUTH_YN`, 
                   C.IQ_AUTH_YN, C.RGST_AUTH_YN, C.MDFY_AUTH_YN, C.DEL_AUTH_YN, C.CAN_AUTH_YN, C.EXCL_AUTH_YN, C.DNLD_AUTH_YN, 
                   C.PRNT_AUTH_YN, C.ETC1_AUTH_YN, C.ETC2_AUTH_YN, C.ETC3_AUTH_YN, 
                   C.MENU_AUTH_IDX, A.USER_IDX, B.AUTH_IDX,  D.MENU_IDX 
                   FROM TSAUTH B, tsurau A, tsmnau C, tsmenu D 
                   WHERE B.AUTH_IDX = A.AUTH_IDX AND A.AUTH_IDX = C.AUTH_IDX AND C.MENU_IDX = D.MENU_IDX 
                   AND (A.USER_IDX = '" + BaseParameter.USER_IDX + "') ORDER BY D.MENU_CD ) as mn WHERE mn.MENU_AUTH_YN = 'True' ";

                    //DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    //result.ListtsauthTranfer = new List<tsauthTranfer>();
                    //for (int i = 0; i < ds.Tables.Count; i++)
                    //{
                    //    DataTable dt = ds.Tables[i];
                    //    result.ListtsauthTranfer.AddRange(SQLHelper.ToList<tsauthTranfer>(dt));
                    //}

                    result.ListtsauthTranfer = await MySQLHelperV2.QueryToListAsync<tsauthTranfer>(GlobalHelper.MariaDBConectionString, sql);
                    result.ListtsmenuTranfer = SetMenu();
                    result.ListtsmenuTranfer = result.ListtsmenuTranfer.Where(item => (item.MENU_LVL == 1 && item.ParentID == BaseParameter.ParentID) || (item.MENU_LVL > 1)).ToList();
                    foreach (tsauthTranfer tsauthTranfer in result.ListtsauthTranfer)
                    {
                        for (int i = 0; i < result.ListtsmenuTranfer.Count; i++)
                        {
                            if (string.IsNullOrEmpty(result.ListtsmenuTranfer[i].Code))
                            {
                                switch (result.ListtsmenuTranfer[i].MENU_IDX)
                                {
                                    case 1:
                                        result.ListtsmenuTranfer[i].Code = "PROJECT PLAN";
                                        break;
                                    case 14:
                                        result.ListtsmenuTranfer[i].Code = "MC_Setting";
                                        break;
                                    case 18:
                                        result.ListtsmenuTranfer[i].Code = "FA Management";
                                        break;
                                    case 19:
                                        result.ListtsmenuTranfer[i].Code = "Human Resources";
                                        break;
                                    //case 17:
                                    //    result.ListtsmenuTranfer[i].Code = "MES REPORT";
                                    //    break;
                                    case 16:
                                        result.ListtsmenuTranfer[i].Code = "ADMIN";
                                        break;
                                    case 13:
                                        result.ListtsmenuTranfer[i].Code = "Factory 2 Monitoring";
                                        break;
                                    case 95:
                                        result.ListtsmenuTranfer[i].Code = "Spare Part";
                                        break;
                                    case 96:
                                        result.ListtsmenuTranfer[i].Code = "Blade List";
                                        break;
                                    case 97:
                                        result.ListtsmenuTranfer[i].Code = "TaskTime DJG";
                                        break;
                                    case 98:
                                        result.ListtsmenuTranfer[i].Code = "TaskTime Standard";
                                        break;
                                    case 814:
                                        result.ListtsmenuTranfer[i].Code = "PO Confirm";
                                        break;
                                    case 133:
                                        result.ListtsmenuTranfer[i].Code = "FA Monitoring";
                                        break;
                                    default:

                                        if (result.ListtsmenuTranfer[i].MENU_LVL == 1)
                                        {
                                            result.ListtsmenuTranfer[i].Code = "M" + result.ListtsmenuTranfer[i].MENU_NM_EN + "01";
                                        }
                                        else if (result.ListtsmenuTranfer[i].MENU_LVL == 2)
                                        {
                                            result.ListtsmenuTranfer[i].Code = result.ListtsmenuTranfer[i].MENU_NM_EN;
                                        }
                                        break;
                                }
                            }


                            if (result.ListtsmenuTranfer[i].MENU_NM_EN == tsauthTranfer.SCRN_PATH)
                            {
                                result.ListtsmenuTranfer[i].Visible = bool.Parse(tsauthTranfer.MENU_AUTH_YN);
                            }
                        }
                    }

                }
            }
            return result;
        }

        private List<tsmenuTranfer> SetMenu()
        {
            List<tsmenuTranfer> result = new List<tsmenuTranfer>();

            tsmenuTranfer tsmenuTranfer = new tsmenuTranfer();
            //tsmenuTranfer.MENU_IDX = 1;
            //tsmenuTranfer.ParentID = -1;
            //tsmenuTranfer.MENU_LVL = 1;
            //tsmenuTranfer.SCRN_PATH = "AP_MAIN";
            //tsmenuTranfer.MENU_NM_EN = "AminPR";
            //result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 2;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "Z";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 3;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "A";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 4;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "V";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 5;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "Y";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 6;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "B";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 7;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "C";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 8;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "D";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 9;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "E";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 10;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "F";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 11;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "G";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 12;
            tsmenuTranfer.ParentID = -2;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "H";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 13;
            tsmenuTranfer.ParentID = -2;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "H";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 14;
            tsmenuTranfer.ParentID = -3;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "MESSetting";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 15;
            tsmenuTranfer.ParentID = 14;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "MESSetting";
            tsmenuTranfer.MENU_NM_EN = "MESSetting";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 16;
            tsmenuTranfer.ParentID = -3;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "ADMIN";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 17;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "P";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 18;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "FA_M";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 19;
            tsmenuTranfer.ParentID = -1;
            tsmenuTranfer.MENU_LVL = 1;
            tsmenuTranfer.SCRN_PATH = "";
            tsmenuTranfer.MENU_NM_EN = "HR";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 201;
            tsmenuTranfer.ParentID = 16;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Admin1";
            tsmenuTranfer.MENU_NM_EN = "Admin1";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 202;
            tsmenuTranfer.ParentID = 16;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Admin2";
            tsmenuTranfer.MENU_NM_EN = "Admin2";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 203;
            tsmenuTranfer.ParentID = 16;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Admin3";
            tsmenuTranfer.MENU_NM_EN = "Admin3";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 204;
            tsmenuTranfer.ParentID = 16;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Admin4";
            tsmenuTranfer.MENU_NM_EN = "Admin4";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 21;
            tsmenuTranfer.ParentID = 2;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z01";
            tsmenuTranfer.MENU_NM_EN = "Z01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 22;
            tsmenuTranfer.ParentID = 2;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z02";
            tsmenuTranfer.MENU_NM_EN = "Z02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 23;
            tsmenuTranfer.ParentID = 2;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z07";
            tsmenuTranfer.MENU_NM_EN = "Z07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 24;
            tsmenuTranfer.ParentID = 2;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z05";
            tsmenuTranfer.MENU_NM_EN = "Z05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 25;
            tsmenuTranfer.ParentID = 2;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z04";
            tsmenuTranfer.MENU_NM_EN = "Z04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 31;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A03";
            tsmenuTranfer.MENU_NM_EN = "A03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 32;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A01";
            tsmenuTranfer.MENU_NM_EN = "A01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 33;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A02";
            tsmenuTranfer.MENU_NM_EN = "A02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 34;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A04";
            tsmenuTranfer.MENU_NM_EN = "A04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 35;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A05";
            tsmenuTranfer.MENU_NM_EN = "A05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 36;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A06";
            tsmenuTranfer.MENU_NM_EN = "A06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 37;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A07";
            tsmenuTranfer.MENU_NM_EN = "A07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 38;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A09";
            tsmenuTranfer.MENU_NM_EN = "A09";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 39;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A10";
            tsmenuTranfer.MENU_NM_EN = "A10";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 40;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A11";
            tsmenuTranfer.MENU_NM_EN = "A11";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 26;
            tsmenuTranfer.ParentID = 3;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "A12";
            tsmenuTranfer.MENU_NM_EN = "A12";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 41;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V01";
            tsmenuTranfer.MENU_NM_EN = "V01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 42;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V02";
            tsmenuTranfer.MENU_NM_EN = "V02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 43;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V03";
            tsmenuTranfer.MENU_NM_EN = "V03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 44;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V04";
            tsmenuTranfer.MENU_NM_EN = "V04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 45;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V05";
            tsmenuTranfer.MENU_NM_EN = "V05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 46;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V06";
            tsmenuTranfer.MENU_NM_EN = "V06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 47;
            tsmenuTranfer.ParentID = 4;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "V07";
            tsmenuTranfer.MENU_NM_EN = "V07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 51;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B09";
            tsmenuTranfer.MENU_NM_EN = "B09";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 52;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B10";
            tsmenuTranfer.MENU_NM_EN = "B10";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 53;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C13";
            tsmenuTranfer.MENU_NM_EN = "C13";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 54;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C14";
            tsmenuTranfer.MENU_NM_EN = "C14";
            result.Add(tsmenuTranfer);           

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 56;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E05";
            tsmenuTranfer.MENU_NM_EN = "E05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 57;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C16";
            tsmenuTranfer.MENU_NM_EN = "C16";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 58;
            tsmenuTranfer.ParentID = 5;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C17";
            tsmenuTranfer.MENU_NM_EN = "C17";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 61;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B01";
            tsmenuTranfer.MENU_NM_EN = "B01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 62;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B03";
            tsmenuTranfer.MENU_NM_EN = "B03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 63;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B02";
            tsmenuTranfer.MENU_NM_EN = "B02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 64;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B04";
            tsmenuTranfer.MENU_NM_EN = "B04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 65;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B05";
            tsmenuTranfer.MENU_NM_EN = "B05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 66;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B06";
            tsmenuTranfer.MENU_NM_EN = "B06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 67;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B11";
            tsmenuTranfer.MENU_NM_EN = "B11";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 68;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B12";
            tsmenuTranfer.MENU_NM_EN = "B12";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 69;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B07";
            tsmenuTranfer.MENU_NM_EN = "B07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 610;
            tsmenuTranfer.ParentID = 6;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "B08";
            tsmenuTranfer.MENU_NM_EN = "B08";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 71;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C01";
            tsmenuTranfer.MENU_NM_EN = "C01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 72;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C06";
            tsmenuTranfer.MENU_NM_EN = "C06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 73;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z03";
            tsmenuTranfer.MENU_NM_EN = "Z03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 74;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C02";
            tsmenuTranfer.MENU_NM_EN = "C02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 75;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C05";
            tsmenuTranfer.MENU_NM_EN = "C05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 76;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C11";
            tsmenuTranfer.MENU_NM_EN = "C11";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 77;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C09";
            tsmenuTranfer.MENU_NM_EN = "C09";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 78;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C20";
            tsmenuTranfer.MENU_NM_EN = "C20";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 79;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C03";
            tsmenuTranfer.MENU_NM_EN = "C03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 80;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C21";
            tsmenuTranfer.MENU_NM_EN = "C21";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 710;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C18";
            tsmenuTranfer.MENU_NM_EN = "C18";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 711;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C19";
            tsmenuTranfer.MENU_NM_EN = "C19";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 712;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C04";
            tsmenuTranfer.MENU_NM_EN = "C04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 713;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "Z06";
            tsmenuTranfer.MENU_NM_EN = "Z06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 714;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C10";
            tsmenuTranfer.MENU_NM_EN = "C10";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 715;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C12";
            tsmenuTranfer.MENU_NM_EN = "C12";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 716;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C15";
            tsmenuTranfer.MENU_NM_EN = "C15";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 717;
            tsmenuTranfer.ParentID = 7;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "C08";
            tsmenuTranfer.MENU_NM_EN = "C08";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 81;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D07";
            tsmenuTranfer.MENU_NM_EN = "D07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 82;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D02";
            tsmenuTranfer.MENU_NM_EN = "D02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 83;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D01";
            tsmenuTranfer.MENU_NM_EN = "D01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 84;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D03";
            tsmenuTranfer.MENU_NM_EN = "D03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 114;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D10";
            tsmenuTranfer.MENU_NM_EN = "D10";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 85;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D13";
            tsmenuTranfer.MENU_NM_EN = "D13";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 86;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D14";
            tsmenuTranfer.MENU_NM_EN = "D14";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 87;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D15";
            tsmenuTranfer.MENU_NM_EN = "D15";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 88;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D16";
            tsmenuTranfer.MENU_NM_EN = "D16";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 90;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D17";
            tsmenuTranfer.MENU_NM_EN = "D17";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 89;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D12";
            tsmenuTranfer.MENU_NM_EN = "D12";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 810;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D04";
            tsmenuTranfer.MENU_NM_EN = "D04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 811;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D99";
            tsmenuTranfer.MENU_NM_EN = "D99";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 812;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D05";
            tsmenuTranfer.MENU_NM_EN = "D05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 813;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D06";
            tsmenuTranfer.MENU_NM_EN = "D06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 814;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D09";
            tsmenuTranfer.MENU_NM_EN = "D09";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 815;
            tsmenuTranfer.ParentID = 8;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "D11";
            tsmenuTranfer.MENU_NM_EN = "D11";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 91;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E01";
            tsmenuTranfer.MENU_NM_EN = "E01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 92;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E02";
            tsmenuTranfer.MENU_NM_EN = "E02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 93;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E03";
            tsmenuTranfer.MENU_NM_EN = "E03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 94;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E04";
            tsmenuTranfer.MENU_NM_EN = "E04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 95;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "X01";
            tsmenuTranfer.MENU_NM_EN = "X01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 96;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "X02";
            tsmenuTranfer.MENU_NM_EN = "X02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 97;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "X03";
            tsmenuTranfer.MENU_NM_EN = "X03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 98;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "X04";
            tsmenuTranfer.MENU_NM_EN = "X04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 94;
            tsmenuTranfer.ParentID = 9;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "E20";
            tsmenuTranfer.MENU_NM_EN = "E20";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 101;
            tsmenuTranfer.ParentID = 10;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "F01";
            tsmenuTranfer.MENU_NM_EN = "F01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 102;
            tsmenuTranfer.ParentID = 10;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "F02";
            tsmenuTranfer.MENU_NM_EN = "F02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 103;
            tsmenuTranfer.ParentID = 10;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "F03";
            tsmenuTranfer.MENU_NM_EN = "F03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 104;
            tsmenuTranfer.ParentID = 10;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "F04";
            tsmenuTranfer.MENU_NM_EN = "F04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 110;
            tsmenuTranfer.ParentID = 11;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "G01";
            tsmenuTranfer.MENU_NM_EN = "G01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 111;
            tsmenuTranfer.ParentID = 11;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "G02";
            tsmenuTranfer.MENU_NM_EN = "G02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 112;
            tsmenuTranfer.ParentID = 11;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "G03";
            tsmenuTranfer.MENU_NM_EN = "G03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 113;
            tsmenuTranfer.ParentID = 11;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "G04";
            tsmenuTranfer.MENU_NM_EN = "G04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 120;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H01";
            tsmenuTranfer.MENU_NM_EN = "H01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 121;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H02";
            tsmenuTranfer.MENU_NM_EN = "H02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 123;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H03";
            tsmenuTranfer.MENU_NM_EN = "H03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 131;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H04";
            tsmenuTranfer.MENU_NM_EN = "H04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 126;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H05";
            tsmenuTranfer.MENU_NM_EN = "H05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 127;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H06";
            tsmenuTranfer.MENU_NM_EN = "H06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 129;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H07";
            tsmenuTranfer.MENU_NM_EN = "H07";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 125;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H08";
            tsmenuTranfer.MENU_NM_EN = "H08";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 122;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H09";
            tsmenuTranfer.MENU_NM_EN = "H09";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 124;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H10";
            tsmenuTranfer.MENU_NM_EN = "H10";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 132;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H11";
            tsmenuTranfer.MENU_NM_EN = "H11";
            result.Add(tsmenuTranfer);


            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 130;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H12";
            tsmenuTranfer.MENU_NM_EN = "H12";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 128;
            tsmenuTranfer.ParentID = 13;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H13";
            tsmenuTranfer.MENU_NM_EN = "H13";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 133;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H14";
            tsmenuTranfer.MENU_NM_EN = "H14";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 134;
            tsmenuTranfer.ParentID = 12;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "H15";
            tsmenuTranfer.MENU_NM_EN = "H15";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 171;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P01";
            tsmenuTranfer.MENU_NM_EN = "P01";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 172;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P02";
            tsmenuTranfer.MENU_NM_EN = "P02";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 173;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P03";
            tsmenuTranfer.MENU_NM_EN = "P03";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 174;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P04";
            tsmenuTranfer.MENU_NM_EN = "P04";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 175;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P05";
            tsmenuTranfer.MENU_NM_EN = "P05";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 176;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P06";
            tsmenuTranfer.MENU_NM_EN = "P06";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 178;
            tsmenuTranfer.ParentID = 17;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "P08";
            tsmenuTranfer.MENU_NM_EN = "P08";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 213;
            tsmenuTranfer.ParentID = 18;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "FA_M1";
            tsmenuTranfer.MENU_NM_EN = "FA_M1";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 212;
            tsmenuTranfer.ParentID = 18;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "FA_M2";
            tsmenuTranfer.MENU_NM_EN = "FA_M2";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 215;
            tsmenuTranfer.ParentID = 18;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "FA_M3";
            tsmenuTranfer.MENU_NM_EN = "FA_M3";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 217;
            tsmenuTranfer.ParentID = 18;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "FA_M4";
            tsmenuTranfer.MENU_NM_EN = "FA_M4";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 219;
            tsmenuTranfer.ParentID = 18;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "FA_M5";
            tsmenuTranfer.MENU_NM_EN = "FA_M5";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 214;
            tsmenuTranfer.ParentID = 19;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "HR1";
            tsmenuTranfer.MENU_NM_EN = "HR1";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 216;
            tsmenuTranfer.ParentID = 19;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "HR2";
            tsmenuTranfer.MENU_NM_EN = "HR2";
            result.Add(tsmenuTranfer);

            tsmenuTranfer = new tsmenuTranfer();
            tsmenuTranfer.MENU_IDX = 218;
            tsmenuTranfer.ParentID = 19;
            tsmenuTranfer.MENU_LVL = 2;
            tsmenuTranfer.SCRN_PATH = "HR3";
            tsmenuTranfer.MENU_NM_EN = "HR3";
            result.Add(tsmenuTranfer);


            return result;
        }
    }
}

