namespace MESService
{
    public static class ConfigureService
    {
        public static IServiceCollection AddJWT(this IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = GlobalHelper.Audience,
                    ValidIssuer = GlobalHelper.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalHelper.Key))
                };
            });
            return services;
        }
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddDbContext<MESContext.Context.Context>(opts => opts.UseMySql(GlobalHelper.MariaDBConectionString, ServerVersion.AutoDetect(GlobalHelper.MariaDBConectionString)));
            services.AddDbContext<Context.Context.Context>(opts => opts.UseMySql(GlobalHelper.ERP_MariaDBConectionString, ServerVersion.AutoDetect(GlobalHelper.ERP_MariaDBConectionString)));
            return services;
        }
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));

            services.AddTransient<ISETUP_FORMService, SETUP_FORMService>();
            services.AddTransient<IWMP_PLAYService, WMP_PLAYService>();
            services.AddTransient<IB01_1Service, B01_1Service>();
            services.AddTransient<IB03_1Service, B03_1Service>();
            services.AddTransient<IB04_2Service, B04_2Service>();
            services.AddTransient<IB07_1Service, B07_1Service>();
            services.AddTransient<IB07_2Service, B07_2Service>();
            services.AddTransient<IB07_3Service, B07_3Service>();
            services.AddTransient<IB08_1Service, B08_1Service>();
            services.AddTransient<IB08_REPRINTService, B08_REPRINTService>();
            services.AddTransient<IA01_FILEService, A01_FILEService>();
            services.AddTransient<IA01_PNADDService, A01_PNADDService>();
            services.AddTransient<IV01_1Service, V01_1Service>();
            services.AddTransient<IV01_2Service, V01_2Service>();
            services.AddTransient<IV01_3Service, V01_3Service>();
            services.AddTransient<IV01_4Service, V01_4Service>();
            services.AddTransient<IG01_1Service, G01_1Service>();
            services.AddTransient<IH01Service, H01Service>();
            services.AddTransient<IH02Service, H02Service>();
            services.AddTransient<IH03Service, H03Service>();
            services.AddTransient<IH04Service, H04Service>();
            services.AddTransient<IH05Service, H05Service>();
            services.AddTransient<IH06Service, H06Service>();
            services.AddTransient<IH07Service, H07Service>();
            services.AddTransient<IH09Service, H09Service>();
            services.AddTransient<IH08Service, H08Service>();
            services.AddTransient<IH10Service, H10Service>();
            services.AddTransient<IH11Service, H11Service>();
            services.AddTransient<IH12Service, H12Service>();
            services.AddTransient<IH13Service, H13Service>();
            services.AddTransient<IZ04_1Service, Z04_1Service>();
            services.AddTransient<IZ04_ADMINService, Z04_ADMINService>();
            services.AddTransient<IZ04_ADMIN_EXCELService, Z04_ADMIN_EXCELService>();
            services.AddTransient<ID04_LOC_YNService, D04_LOC_YNService>();
            services.AddTransient<ID04_PNO_CHKService, D04_PNO_CHKService>();
            services.AddTransient<ID04_POADDService, D04_POADDService>();
            services.AddTransient<ID04_QTY_YNService, D04_QTY_YNService>();
            services.AddTransient<ID04_RANKService, D04_RANKService>();
            services.AddTransient<IAdmin6UserService, Admin6UserService>();
            services.AddTransient<ID04_POLISTService, D04_POLISTService>();
            services.AddTransient<ID04_PLT_PRNTService, D04_PLT_PRNTService>();
            services.AddTransient<IC09_COUNTService, C09_COUNTService>();
            services.AddTransient<IC09_LISTService, C09_LISTService>();
            services.AddTransient<IC09_REPRINTService, C09_REPRINTService>();
            services.AddTransient<IC09_STOPService, C09_STOPService>();
            services.AddTransient<IC09_START_V3Service, C09_START_V3Service>();
            services.AddTransient<IC09_SPCService, C09_SPCService>();
            services.AddTransient<IC04_1Service, C04_1Service>();
            services.AddTransient<IC05_STOPService, C05_STOPService>();
            services.AddTransient<IC05_STARTService, C05_STARTService>();
            services.AddTransient<IC05_APPLICATIONService, C05_APPLICATIONService>();
            services.AddTransient<IC05_DC_READService, C05_DC_READService>();
            services.AddTransient<IC05_ERRORService, C05_ERRORService>();
            services.AddTransient<IC05_SPCService, C05_SPCService>();
            services.AddTransient<IC05_ER_LService, C05_ER_LService>();
            services.AddTransient<IC05_ER_RService, C05_ER_RService>();
            services.AddTransient<IC11_STOPService, C11_STOPService>();
            services.AddTransient<IC11_ER_LService, C11_ER_LService>();
            services.AddTransient<IC11_ER_RService, C11_ER_RService>();
            services.AddTransient<IC11_1Service, C11_1Service>();
            services.AddTransient<IC11_2Service, C11_2Service>();
            services.AddTransient<IC11_3Service, C11_3Service>();
            services.AddTransient<IC11_4Service, C11_4Service>();
            services.AddTransient<IC11_APPLICATIONService, C11_APPLICATIONService>();
            services.AddTransient<IC11_SPC_LService, C11_SPC_LService>();
            services.AddTransient<IC11_SPC_RService, C11_SPC_RService>();
            services.AddTransient<IC15_1Service, C15_1Service>();
            services.AddTransient<IC02_STOPService, C02_STOPService>();
            services.AddTransient<IC02_LISTService, C02_LISTService>();
            services.AddTransient<IC02_COUNTService, C02_COUNTService>();
            services.AddTransient<IC02_REPRINTService, C02_REPRINTService>();
            services.AddTransient<IC02_ERRORService, C02_ERRORService>();
            services.AddTransient<IC02_MTService, C02_MTService>();
            services.AddTransient<IC02_START_V2Service, C02_START_V2Service>();
            services.AddTransient<IC02_APPLICATIONService, C02_APPLICATIONService>();
            services.AddTransient<IC02_SPCService, C02_SPCService>();
            services.AddTransient<IX01Service, X01Service>();

            services.AddTransient<IMAINService, MAINService>();
            services.AddTransient<IMAIN_LoginService, MAIN_LoginService>();
            services.AddTransient<IMES_REPORTService, MES_REPORTService>();
            services.AddTransient<IZ01Service, Z01Service>();
            services.AddTransient<IZ02Service, Z02Service>();
            services.AddTransient<IZ07Service, Z07Service>();
            services.AddTransient<IZ07_1Service, Z07_1Service>();
            services.AddTransient<IZ05Service, Z05Service>();
            services.AddTransient<IZ04Service, Z04Service>();
            services.AddTransient<IA03Service, A03Service>();
            services.AddTransient<IA01Service, A01Service>();
            services.AddTransient<IA02Service, A02Service>();
            services.AddTransient<IA04Service, A04Service>();
            services.AddTransient<IA05Service, A05Service>();
            services.AddTransient<IA06Service, A06Service>();
            services.AddTransient<IA07Service, A07Service>();
            services.AddTransient<IA09Service, A09Service>();
            services.AddTransient<IA10Service, A10Service>();
            services.AddTransient<IA11Service, A11Service>();
            services.AddTransient<IV01Service, V01Service>();
            services.AddTransient<IV02Service, V02Service>();
            services.AddTransient<IV03Service, V03Service>();
            services.AddTransient<IV03_1Service, V03_1Service>();
            services.AddTransient<IV03_3Service, V03_3Service>();
            services.AddTransient<IV04Service, V04Service>();
            services.AddTransient<IV05Service, V05Service>();
            services.AddTransient<IV06Service, V06Service>();
            services.AddTransient<IV07Service, V07Service>();
            services.AddTransient<IB09Service, B09Service>();
            services.AddTransient<IB10Service, B10Service>();
            services.AddTransient<IC13Service, C13Service>();
            services.AddTransient<IC14Service, C14Service>();
            services.AddTransient<IE04Service, E04Service>();
            services.AddTransient<IE05Service, E05Service>();
            services.AddTransient<IC16Service, C16Service>();
            services.AddTransient<IC17Service, C17Service>();
            services.AddTransient<IB01Service, B01Service>();
            services.AddTransient<IB03Service, B03Service>();
            services.AddTransient<IB02Service, B02Service>();
            services.AddTransient<IB04Service, B04Service>();
            services.AddTransient<IB05Service, B05Service>();
            services.AddTransient<IB06Service, B06Service>();
            services.AddTransient<IB11Service, B11Service>();
            services.AddTransient<IB12Service, B12Service>();
            services.AddTransient<IB07Service, B07Service>();
            services.AddTransient<IB08Service, B08Service>();
            services.AddTransient<IC01Service, C01Service>();
            services.AddTransient<IC06Service, C06Service>();
            services.AddTransient<IZ03Service, Z03Service>();
            services.AddTransient<IC02Service, C02Service>();
            services.AddTransient<IC05Service, C05Service>();
            services.AddTransient<IC11Service, C11Service>();
            services.AddTransient<IC09Service, C09Service>();
            services.AddTransient<IC20Service, C20Service>();
            services.AddTransient<IC03Service, C03Service>();
            services.AddTransient<IC18Service, C18Service>();
            services.AddTransient<IC19Service, C19Service>();
            services.AddTransient<IC19_1Service, C19_1Service>();
            services.AddTransient<IC04Service, C04Service>();
            services.AddTransient<IZ06Service, Z06Service>();
            services.AddTransient<IC10Service, C10Service>();
            services.AddTransient<IC12Service, C12Service>();
            services.AddTransient<IC15Service, C15Service>();
            services.AddTransient<IC08Service, C08Service>();
            services.AddTransient<ID07Service, D07Service>();
            services.AddTransient<ID02Service, D02Service>();
            services.AddTransient<ID01Service, D01Service>();
            services.AddTransient<ID03Service, D03Service>();
            services.AddTransient<ID10Service, D10Service>();
            services.AddTransient<ID13Service, D13Service>();
            services.AddTransient<ID14Service, D14Service>();
            services.AddTransient<ID15Service, D15Service>();
            services.AddTransient<ID16Service, D16Service>();
            services.AddTransient<ID12Service, D12Service>();
            services.AddTransient<ID04Service, D04Service>();
            services.AddTransient<ID99Service, D99Service>();
            services.AddTransient<ID05Service, D05Service>();
            services.AddTransient<ID06Service, D06Service>();
            services.AddTransient<ID09Service, D09Service>();
            services.AddTransient<ID11Service, D11Service>();
            services.AddTransient<IE01Service, E01Service>();
            services.AddTransient<IE02Service, E02Service>();
            services.AddTransient<IE03Service, E03Service>();
            services.AddTransient<IE20Service, E20Service>();
            services.AddTransient<IF01Service, F01Service>();
            services.AddTransient<IF02Service, F02Service>();
            services.AddTransient<IF03Service, F03Service>();
            services.AddTransient<IF04Service, F04Service>();
            services.AddTransient<IG01Service, G01Service>();
            services.AddTransient<IG02Service, G02Service>();
            services.AddTransient<IG03Service, G03Service>();
            services.AddTransient<IG04Service, G04Service>();
            services.AddTransient<Iadmin1Service, Admin1Service>();
            services.AddTransient<Iadmin2Service, Admin2Service>();
            services.AddTransient<Iadmin3Service, Admin3Service>();
            services.AddTransient<Iadmin4Service, Admin4Service>();   
            services.AddTransient<IMESSettingService, MESSettingService>();
            services.AddTransient<IX02Service, X02Service>();
            services.AddTransient<IX03Service, X03Service>();
            services.AddTransient<IX04Service, X04Service>();
            services.AddTransient<IP04Service, P04Service>();
            services.AddTransient<IP06Service, P06Service>();
            services.AddTransient<IHR2Service, HR2Service>();
            services.AddTransient<IH14Service, H14Service>();
            services.AddTransient<IFA_M1Service, FA_M1Service>();
            services.AddTransient<IHR1Service, HR1Service>();
            services.AddTransient<IA12Service, A12Service>();
            services.AddTransient<IFA_M2Service, FA_M2Service>();
            services.AddTransient<IFA_M3Service, FA_M3Service>();
            services.AddTransient<IFA_M4Service, FA_M4Service>();
            services.AddTransient<IH15Service, H15Service>();
            services.AddTransient<IHR3Service, HR3Service>();
            services.AddTransient<IFA_M5Service, FA_M5Service>();
            services.AddTransient<ID17Service, D17Service>();
            services.AddTransient<IC21Service, C21Service>();


            

            return services;
        }
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<IKOMAXCheckErrorProofHistoryRepository, KOMAXCheckErrorProofHistoryRepository>();
            services.AddTransient<IIQCNGCustomer2Repository, IQCNGCustomer2Repository>();
            services.AddTransient<ITrolleyRepository, TrolleyRepository>();
            services.AddTransient<Itfg_inventoryRepository, tfg_inventoryRepository>();
            services.AddTransient<Itfg_packing_detailRepository, tfg_packing_detailRepository>();
            services.AddTransient<Itfg_historyRepository, tfg_historyRepository>();
            services.AddTransient<ISparePartScanOutRepository, SparePartScanOutRepository>();
            services.AddTransient<ISparePartScanInRepository, SparePartScanInRepository>();
            services.AddTransient<ISparePartRepository, SparePartRepository>();
            services.AddTransient<IPartSpareRepository, PartSpareRepository>();
            services.AddTransient<IPartSpareScanInRepository, PartSpareScanInRepository>();
            services.AddTransient<IPartSpareScanOutRepository, PartSpareScanOutRepository>();
            services.AddTransient<IMaintenanceHistoryRepository, MaintenanceHistoryRepository>();
            services.AddTransient<IToolShopRepository, ToolShopRepository>();
            services.AddTransient<IDowntimeRecordsRepository, DowntimeRecordsRepository>();
            services.AddTransient<IAttendanceRecordsRepository, AttendanceRecordsRepository>();
            services.AddTransient<IAttendanceSessionRepository, AttendanceSessionRepository>();
            services.AddTransient<IAttendanceRepository, AttendanceRepository>();
            services.AddTransient<IFAWorkOrderHistoryRepository, FAWorkOrderHistoryRepository>();
            services.AddTransient<ITaskTimeFARepository, TaskTimeFARepository>();
            services.AddTransient<IFAWorkOrderRepository, FAWorkOrderRepository>();
            services.AddTransient<IOQC_NGRepository, OQC_NGRepository>();
            services.AddTransient<INGListRepository, NGListRepository>();
            services.AddTransient<IEmployeeFileRepository, EmployeeFileRepository>();
            services.AddTransient<IEmployeeFinanceRepository, EmployeeFinanceRepository>();
            services.AddTransient<IEmployeeContractRepository, EmployeeContractRepository>();
            services.AddTransient<IEmployeeJobRepository, EmployeeJobRepository>();
            services.AddTransient<IPersonalInfoRepository, PersonalInfoRepository>();
            services.AddTransient<IROTestLogRepository, ROTestLogRepository>();
            services.AddTransient<IFAProductionRepository, FAProductionRepository>();
            services.AddTransient<IEmployeeFARepository, EmployeeFARepository>();
            services.AddTransient<ILineListRepository, LineListRepository>();
            services.AddTransient<ILineAssignmentRepository, LineAssignmentRepository>();
            services.AddTransient<IShiftTimeRepository, ShiftTimeRepository>();
            services.AddTransient<IaatableRepository, aatableRepository>();
            services.AddTransient<Iapqp_cdgrRepository, apqp_cdgrRepository>();
            services.AddTransient<Iapqp_codeRepository, apqp_codeRepository>();
            services.AddTransient<Iapqp_dlylstRepository, apqp_dlylstRepository>();
            services.AddTransient<Iapqp_filelstRepository, apqp_filelstRepository>();
            services.AddTransient<Iapqp_mstlstRepository, apqp_mstlstRepository>();
            services.AddTransient<Ikr_inspctn_stRepository, kr_inspctn_stRepository>();
            services.AddTransient<Ikr_inspctn_testRepository, kr_inspctn_testRepository>();
            services.AddTransient<Ikr_tdd_poplanRepository, kr_tdd_poplanRepository>();
            services.AddTransient<Ikr_tdpdmtimRepository, kr_tdpdmtimRepository>();
            services.AddTransient<Ikr_tdpdmtim_tmpRepository, kr_tdpdmtim_tmpRepository>();
            services.AddTransient<Ikr_tdpdmtim_tmp_outRepository, kr_tdpdmtim_tmp_outRepository>();
            services.AddTransient<Ikr_tdpdotplRepository, kr_tdpdotplRepository>();
            services.AddTransient<Ikr_tdpdotpl_inpoRepository, kr_tdpdotpl_inpoRepository>();
            services.AddTransient<Ikr_tiivtrRepository, kr_tiivtrRepository>();
            services.AddTransient<Ipd_asset_mmRepository, pd_asset_mmRepository>();
            services.AddTransient<Ipd_cmpny_costfileRepository, pd_cmpny_costfileRepository>();
            services.AddTransient<Ipd_cmpny_partRepository, pd_cmpny_partRepository>();
            services.AddTransient<Ipd_inout_partRepository, pd_inout_partRepository>();
            services.AddTransient<Ipd_mc_orderlistRepository, pd_mc_orderlistRepository>();
            services.AddTransient<Ipd_part_costRepository, pd_part_costRepository>();
            services.AddTransient<Ipd_tiivtrRepository, pd_tiivtrRepository>();
            services.AddTransient<IpdcdgrRepository, pdcdgrRepository>();
            services.AddTransient<IpdcdnmRepository, pdcdnmRepository>();
            services.AddTransient<IpdcmpnyRepository, pdcmpnyRepository>();
            services.AddTransient<IpdpartRepository, pdpartRepository>();
            services.AddTransient<Ipdpart_addlistRepository, pdpart_addlistRepository>();
            services.AddTransient<IpdpuschRepository, pdpuschRepository>();
            services.AddTransient<Itdd_ct_stRepository, tdd_ct_stRepository>();
            services.AddTransient<Itdd_poplanRepository, tdd_poplanRepository>();
            services.AddTransient<Itdd_poplan_djgRepository, tdd_poplan_djgRepository>();
            services.AddTransient<ItdpdmtimRepository, tdpdmtimRepository>();
            services.AddTransient<Itdpdmtim_autobc_listRepository, tdpdmtim_autobc_listRepository>();
            services.AddTransient<Itdpdmtim_delRepository, tdpdmtim_delRepository>();
            services.AddTransient<Itdpdmtim_histRepository, tdpdmtim_histRepository>();
            services.AddTransient<Itdpdmtim_locRepository, tdpdmtim_locRepository>();
            services.AddTransient<Itdpdmtim_reworkRepository, tdpdmtim_reworkRepository>();
            services.AddTransient<Itdpdmtim_tmpRepository, tdpdmtim_tmpRepository>();
            services.AddTransient<Itdpdmtin_serialRepository, tdpdmtin_serialRepository>();
            services.AddTransient<ItdpdotplRepository, tdpdotplRepository>();
            services.AddTransient<Itdpdotpl_alocRepository, tdpdotpl_alocRepository>();
            services.AddTransient<Itdpdotpl_etcRepository, tdpdotpl_etcRepository>();
            services.AddTransient<Itdpdotpl_labelRepository, tdpdotpl_labelRepository>();
            services.AddTransient<Itdpdotpl_tmpRepository, tdpdotpl_tmpRepository>();
            services.AddTransient<ItdpdotplmuRepository, tdpdotplmuRepository>();
            services.AddTransient<Itfg_monitorRepository, tfg_monitorRepository>();
            services.AddTransient<ItiivajRepository, tiivajRepository>();
            services.AddTransient<Itiivaj_historyRepository, tiivaj_historyRepository>();
            services.AddTransient<Itiivaj_leadRepository, tiivaj_leadRepository>();
            services.AddTransient<ItiivtrRepository, tiivtrRepository>();
            services.AddTransient<Itiivtr_excelRepository, tiivtr_excelRepository>();
            services.AddTransient<Itiivtr_historyRepository, tiivtr_historyRepository>();
            services.AddTransient<Itiivtr_leadRepository, tiivtr_leadRepository>();
            services.AddTransient<Itiivtr_lead_fgRepository, tiivtr_lead_fgRepository>();
            services.AddTransient<Itiivtr_lead_historyRepository, tiivtr_lead_historyRepository>();
            services.AddTransient<ItmbrcdRepository, tmbrcdRepository>();
            services.AddTransient<Itmbrcd_hisRepository, tmbrcd_hisRepository>();
            services.AddTransient<Itmbrcd_longtermRepository, tmbrcd_longtermRepository>();
            services.AddTransient<ItmmtinRepository, tmmtinRepository>();
            services.AddTransient<Itmmtin_dmmRepository, tmmtin_dmmRepository>();
            services.AddTransient<Itmmtin_dmm_appRepository, tmmtin_dmm_appRepository>();
            services.AddTransient<Itmmtin_dmm_cutRepository, tmmtin_dmm_cutRepository>();
            services.AddTransient<Itmmtin_dmm_leadRepository, tmmtin_dmm_leadRepository>();
            services.AddTransient<Itorder_barcodeRepository, torder_barcodeRepository>();
            services.AddTransient<Itorder_barcode_lpRepository, torder_barcode_lpRepository>();
            services.AddTransient<Itorder_barcode_spRepository, torder_barcode_spRepository>();
            services.AddTransient<Itorder_bomRepository, torder_bomRepository>();
            services.AddTransient<Itorder_bom_lpRepository, torder_bom_lpRepository>();
            services.AddTransient<Itorder_bom_not_climpRepository, torder_bom_not_climpRepository>();
            services.AddTransient<Itorder_bom_spst1Repository, torder_bom_spst1Repository>();
            services.AddTransient<Itorder_bom_spst2Repository, torder_bom_spst2Repository>();
            services.AddTransient<Itorder_bom_swRepository, torder_bom_swRepository>();
            services.AddTransient<Itorder_lead_bomRepository, torder_lead_bomRepository>();
            services.AddTransient<Itorder_lead_bom_exclRepository, torder_lead_bom_exclRepository>();
            services.AddTransient<Itorder_lead_bom_spstRepository, torder_lead_bom_spstRepository>();
            services.AddTransient<Itorder_lead_bom_spst_exclRepository, torder_lead_bom_spst_exclRepository>();
            services.AddTransient<Itorder_spcRepository, torder_spcRepository>();
            services.AddTransient<ItorderinspectionRepository, torderinspectionRepository>();
            services.AddTransient<Itorderinspection_lpRepository, torderinspection_lpRepository>();
            services.AddTransient<Itorderinspection_spstRepository, torderinspection_spstRepository>();
            services.AddTransient<Itorderinspection_swRepository, torderinspection_swRepository>();
            services.AddTransient<ItorderlistRepository, torderlistRepository>();
            services.AddTransient<Itorderlist_lpRepository, torderlist_lpRepository>();
            services.AddTransient<Itorderlist_lplistRepository, torderlist_lplistRepository>();
            services.AddTransient<Itorderlist_spstRepository, torderlist_spstRepository>();
            services.AddTransient<Itorderlist_swRepository, torderlist_swRepository>();
            services.AddTransient<Itorderlist_workRepository, torderlist_workRepository>();
            services.AddTransient<Itorderlist_wtimeRepository, torderlist_wtimeRepository>();
            services.AddTransient<Itrack_bc_tmpRepository, track_bc_tmpRepository>();
            services.AddTransient<ItrackmasterRepository, trackmasterRepository>();
            services.AddTransient<ItrackmtimRepository, trackmtimRepository>();
            services.AddTransient<Itrackmtim_lt_inspRepository, trackmtim_lt_inspRepository>();
            services.AddTransient<ItsauthRepository, tsauthRepository>();
            services.AddTransient<ItsbomRepository, tsbomRepository>();
            services.AddTransient<Itsbom_listRepository, tsbom_listRepository>();
            services.AddTransient<Itsbom_part_infRepository, tsbom_part_infRepository>();
            services.AddTransient<Itsbom_po_listRepository, tsbom_po_listRepository>();
            services.AddTransient<Itsbom_ver02Repository, tsbom_ver02Repository>();
            services.AddTransient<Itsbom_ver02_poRepository, tsbom_ver02_poRepository>();
            services.AddTransient<Itsbom_ver02_tmp1Repository, tsbom_ver02_tmp1Repository>();
            services.AddTransient<ItscdgrRepository, tscdgrRepository>();
            services.AddTransient<ItscmpnyRepository, tscmpnyRepository>();
            services.AddTransient<ItscodeRepository, tscodeRepository>();
            services.AddTransient<ItscostRepository, tscostRepository>();
            services.AddTransient<Itscost_listRepository, tscost_listRepository>();
            services.AddTransient<Itscut_st_uphRepository, tscut_st_uphRepository>();
            services.AddTransient<ItsmenuRepository, tsmenuRepository>();
            services.AddTransient<ItsmnauRepository, tsmnauRepository>();
            services.AddTransient<Itsmonitor_setRepository, tsmonitor_setRepository>();
            services.AddTransient<Itsnon_operRepository, tsnon_operRepository>();
            services.AddTransient<Itsnon_oper_andonRepository, tsnon_oper_andonRepository>();
            services.AddTransient<Itsnon_oper_andon_listRepository, tsnon_oper_andon_listRepository>();
            services.AddTransient<Itsnon_oper_mitorRepository, tsnon_oper_mitorRepository>();
            services.AddTransient<Itsnon_oper_workerRepository, tsnon_oper_workerRepository>();
            services.AddTransient<Itsnon_worktimeRepository, tsnon_worktimeRepository>();
            services.AddTransient<ItsnoticeRepository, tsnoticeRepository>();
            services.AddTransient<ItspartRepository, tspartRepository>();
            services.AddTransient<Itspart_addtnlRepository, tspart_addtnlRepository>();
            services.AddTransient<Itspart_ecnRepository, tspart_ecnRepository>();
            services.AddTransient<Itspart_fileRepository, tspart_fileRepository>();
            services.AddTransient<ItsurauRepository, tsurauRepository>();
            services.AddTransient<ItsuserRepository, tsuserRepository>();
            services.AddTransient<Itsuser_requRepository, tsuser_requRepository>();
            services.AddTransient<Itsuser_superRepository, tsuser_superRepository>();
            services.AddTransient<Itsyear_group_invRepository, tsyear_group_invRepository>();
            services.AddTransient<Itsyear_group_inv_histRepository, tsyear_group_inv_histRepository>();
            services.AddTransient<Itsyear_inventoryRepository, tsyear_inventoryRepository>();
            services.AddTransient<Itsyear_inventory_histRepository, tsyear_inventory_histRepository>();
            services.AddTransient<Ittc_barcodeRepository, ttc_barcodeRepository>();
            services.AddTransient<Ittc_bomRepository, ttc_bomRepository>();
            services.AddTransient<Ittc_orderRepository, ttc_orderRepository>();
            services.AddTransient<Ittc_partRepository, ttc_partRepository>();
            services.AddTransient<Ittc_rackmtinRepository, ttc_rackmtinRepository>();
            services.AddTransient<IttensilbndlstRepository, ttensilbndlstRepository>();
            services.AddTransient<IttensilforceRepository, ttensilforceRepository>();
            services.AddTransient<Ittensilforce_uswRepository, ttensilforce_uswRepository>();
            services.AddTransient<IttoolhistoryRepository, ttoolhistoryRepository>();
            services.AddTransient<IttoolmasterRepository, ttoolmasterRepository>();
            services.AddTransient<Ittoolmaster2Repository, ttoolmaster2Repository>();
            services.AddTransient<Ituser_logRepository, tuser_logRepository>();
            services.AddTransient<Ituser_log_chkRepository, tuser_log_chkRepository>();
            services.AddTransient<Ituser_log_chk_listRepository, tuser_log_chk_listRepository>();
            services.AddTransient<ItworkresultRepository, tworkresultRepository>();
            services.AddTransient<ItwtoolRepository, twtoolRepository>();
            services.AddTransient<ItwwkarRepository, twwkarRepository>();
            services.AddTransient<Itwwkar_lpRepository, twwkar_lpRepository>();
            services.AddTransient<Itwwkar_spstRepository, twwkar_spstRepository>();
            services.AddTransient<Ixsetting_timeRepository, xsetting_timeRepository>();
            services.AddTransient<Izadmin_functionRepository, zadmin_functionRepository>();
            services.AddTransient<Izt_devlpmnt_dbRepository, zt_devlpmnt_dbRepository>();
            services.AddTransient<Izt_help_dbRepository, zt_help_dbRepository>();
            services.AddTransient<Izt_log_dbRepository, zt_log_dbRepository>();
            services.AddTransient<Izz_mes_verRepository, zz_mes_verRepository>();
            

            return services;
        }
        public static IServiceCollection AddServiceERP(this IServiceCollection services)
        {
            services.AddTransient<ICategoryConfigService, CategoryConfigService>();
            services.AddTransient<ICategoryLevelService, CategoryLevelService>();
            services.AddTransient<ICategoryStatusService, CategoryStatusService>();
            services.AddTransient<ICategorySystemService, CategorySystemService>();
            services.AddTransient<ICategoryTypeService, CategoryTypeService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProjectFileService, ProjectFileService>();
            services.AddTransient<IProjectTaskService, ProjectTaskService>();
            services.AddTransient<IProjectTaskHistoryService, ProjectTaskHistoryService>();
            services.AddTransient<IProjectTaskMembershipService, ProjectTaskMembershipService>();

            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IMembershipTokenService, MembershipTokenService>();
            services.AddTransient<IMembershipHistoryURLService, MembershipHistoryURLService>();
            services.AddTransient<IMembershipMenuService, MembershipMenuService>();
            services.AddTransient<IMembershipCompanyService, MembershipCompanyService>();
            services.AddTransient<IMembershipDepartmentService, MembershipDepartmentService>();


            services.AddTransient<ICategoryPositionService, CategoryPositionService>();
            services.AddTransient<ICategoryDepartmentService, CategoryDepartmentService>();
            services.AddTransient<ICategoryRackService, CategoryRackService>();
            services.AddTransient<ICategoryLayerService, CategoryLayerService>();
            services.AddTransient<ICategoryLocationService, CategoryLocationService>();
            services.AddTransient<ICategoryLocationMaterialService, CategoryLocationMaterialService>();
            services.AddTransient<ICategoryMenuService, CategoryMenuService>();
            services.AddTransient<ICategoryUnitService, CategoryUnitService>();
            services.AddTransient<ICategoryDeliveryService, CategoryDeliveryService>();
            services.AddTransient<ICategoryFamilyService, CategoryFamilyService>();
            services.AddTransient<ICategoryVehicleService, CategoryVehicleService>();
            services.AddTransient<ICategoryMaterialService, CategoryMaterialService>();
            services.AddTransient<ICategoryTermService, CategoryTermService>();
            services.AddTransient<ICategoryToleranceService, CategoryToleranceService>();
            services.AddTransient<ICategoryParentheseService, CategoryParentheseService>();

            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IInventoryDetailService, InventoryDetailService>();
            services.AddTransient<IInventoryDetailBarcodeService, InventoryDetailBarcodeService>();


            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IMaterialConvertService, MaterialConvertService>();
            services.AddTransient<IMaterialReplaceService, MaterialReplaceService>();
            services.AddTransient<IMaterialUnitCovertService, MaterialUnitCovertService>();
            services.AddTransient<IFactoryService, FactoryService>();
            services.AddTransient<ICompanyService, CompanyService>();

            services.AddTransient<IBOMService, BOMService>();
            services.AddTransient<IBOMDetailService, BOMDetailService>();
            services.AddTransient<IBOMTermService, BOMTermService>();
            services.AddTransient<IBOMStageService, BOMStageService>();
            services.AddTransient<IBOMCompareService, BOMCompareService>();


            services.AddTransient<IProductionOrderService, ProductionOrderService>();
            services.AddTransient<IProductionOrderBOMService, ProductionOrderBOMService>();
            services.AddTransient<IProductionOrderBOMDetailService, ProductionOrderBOMDetailService>();
            services.AddTransient<IProductionOrderDetailService, ProductionOrderDetailService>();
            services.AddTransient<IProductionOrderProductionPlanService, ProductionOrderProductionPlanService>();
            services.AddTransient<IProductionOrderProductionPlanBackupService, ProductionOrderProductionPlanBackupService>();
            services.AddTransient<IProductionOrderProductionPlanSemiService, ProductionOrderProductionPlanSemiService>();
            services.AddTransient<IProductionOrderCuttingOrderService, ProductionOrderCuttingOrderService>();
            services.AddTransient<IProductionOrderSPSTOrderService, ProductionOrderSPSTOrderService>();
            services.AddTransient<IProductionOrderProductionPlanMaterialService, ProductionOrderProductionPlanMaterialService>();
            services.AddTransient<IProductionOrderMaterialService, ProductionOrderMaterialService>();
            services.AddTransient<IProductionOrderOutputScheduleService, ProductionOrderOutputScheduleService>();
            services.AddTransient<IProductionOrderFileService, ProductionOrderFileService>();

            services.AddTransient<ICategoryCompanyService, CategoryCompanyService>();
            services.AddTransient<ICategoryInvoiceService, CategoryInvoiceService>();

            services.AddTransient<IWarehouseInputService, WarehouseInputService>();
            services.AddTransient<IWarehouseInputDetailService, WarehouseInputDetailService>();
            services.AddTransient<IWarehouseInputDetailBarcodeService, WarehouseInputDetailBarcodeService>();
            services.AddTransient<IWarehouseInputDetailBarcodeMaterialService, WarehouseInputDetailBarcodeMaterialService>();
            services.AddTransient<IWarehouseInputMaterialService, WarehouseInputMaterialService>();
            services.AddTransient<IWarehouseInputDetailCountService, WarehouseInputDetailCountService>();
            services.AddTransient<IWarehouseInputFileService, WarehouseInputFileService>();
            services.AddTransient<IWarehouseOutputService, WarehouseOutputService>();
            services.AddTransient<IWarehouseOutputDetailService, WarehouseOutputDetailService>();
            services.AddTransient<IWarehouseOutputDetailBarcodeService, WarehouseOutputDetailBarcodeService>();
            services.AddTransient<IWarehouseOutputDetailBarcodeMaterialService, WarehouseOutputDetailBarcodeMaterialService>();
            services.AddTransient<IWarehouseOutputMaterialService, WarehouseOutputMaterialService>();
            services.AddTransient<IWarehouseOutputFileService, WarehouseOutputFileService>();
            services.AddTransient<IWarehouseInventoryService, WarehouseInventoryService>();
            services.AddTransient<IWarehouseStockService, WarehouseStockService>();
            services.AddTransient<IWarehouseStockDetailService, WarehouseStockDetailService>();
            services.AddTransient<IWarehouseRequestService, WarehouseRequestService>();
            services.AddTransient<IWarehouseRequestDetailService, WarehouseRequestDetailService>();
            services.AddTransient<IWarehouseRequestConfirmService, WarehouseRequestConfirmService>();
            services.AddTransient<IWarehouseRequestFileService, WarehouseRequestFileService>();


            services.AddTransient<IInvoiceInputService, InvoiceInputService>();
            services.AddTransient<IInvoiceInputDetailService, InvoiceInputDetailService>();
            services.AddTransient<IInvoiceInputFileService, InvoiceInputFileService>();
            services.AddTransient<IInvoiceOutputService, InvoiceOutputService>();
            services.AddTransient<IInvoiceOutputDetailService, InvoiceOutputDetailService>();
            services.AddTransient<IInvoiceOutputFileService, InvoiceOutputFileService>();
            services.AddTransient<IInvoiceInputInventoryService, InvoiceInputInventoryService>();

            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportDetailService, ReportDetailService>();

            services.AddTransient<IZaloTokenService, ZaloTokenService>();
            services.AddTransient<IZaloZNSService, ZaloZNSService>();
            services.AddTransient<INotificationService, NotificationService>();



            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            return services;
        }
        public static IServiceCollection AddRepositoryERP(this IServiceCollection services)
        {
            services.AddTransient<ICategoryConfigRepository, CategoryConfigRepository>();
            services.AddTransient<ICategoryLevelRepository, CategoryLevelRepository>();
            services.AddTransient<ICategoryStatusRepository, CategoryStatusRepository>();
            services.AddTransient<ICategorySystemRepository, CategorySystemRepository>();
            services.AddTransient<ICategoryTypeRepository, CategoryTypeRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectFileRepository, ProjectFileRepository>();
            services.AddTransient<IProjectTaskRepository, ProjectTaskRepository>();
            services.AddTransient<IProjectTaskHistoryRepository, ProjectTaskHistoryRepository>();
            services.AddTransient<IProjectTaskMembershipRepository, ProjectTaskMembershipRepository>();


            services.AddTransient<IMembershipRepository, MembershipRepository>();
            services.AddTransient<IMembershipTokenRepository, MembershipTokenRepository>();
            services.AddTransient<IMembershipHistoryURLRepository, MembershipHistoryURLRepository>();
            services.AddTransient<IMembershipMenuRepository, MembershipMenuRepository>();
            services.AddTransient<IMembershipCompanyRepository, MembershipCompanyRepository>();
            services.AddTransient<IMembershipDepartmentRepository, MembershipDepartmentRepository>();


            services.AddTransient<ICategoryPositionRepository, CategoryPositionRepository>();
            services.AddTransient<ICategoryDepartmentRepository, CategoryDepartmentRepository>();
            services.AddTransient<ICategoryRackRepository, CategoryRackRepository>();
            services.AddTransient<ICategoryLayerRepository, CategoryLayerRepository>();
            services.AddTransient<ICategoryLocationRepository, CategoryLocationRepository>();
            services.AddTransient<ICategoryLocationMaterialRepository, CategoryLocationMaterialRepository>();
            services.AddTransient<ICategoryMenuRepository, CategoryMenuRepository>();
            services.AddTransient<ICategoryUnitRepository, CategoryUnitRepository>();
            services.AddTransient<ICategoryDeliveryRepository, CategoryDeliveryRepository>();
            services.AddTransient<ICategoryFamilyRepository, CategoryFamilyRepository>();
            services.AddTransient<ICategoryVehicleRepository, CategoryVehicleRepository>();
            services.AddTransient<ICategoryMaterialRepository, CategoryMaterialRepository>();
            services.AddTransient<ICategoryToleranceRepository, CategoryToleranceRepository>();
            services.AddTransient<ICategoryTermRepository, CategoryTermRepository>();
            services.AddTransient<ICategoryParentheseRepository, CategoryParentheseRepository>();

            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryDetailRepository, InventoryDetailRepository>();
            services.AddTransient<IInventoryDetailBarcodeRepository, InventoryDetailBarcodeRepository>();


            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<IMaterialConvertRepository, MaterialConvertRepository>();
            services.AddTransient<IMaterialReplaceRepository, MaterialReplaceRepository>();
            services.AddTransient<IMaterialUnitCovertRepository, MaterialUnitCovertRepository>();
            services.AddTransient<IFactoryRepository, FactoryRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();

            services.AddTransient<IBOMRepository, BOMRepository>();
            services.AddTransient<IBOMDetailRepository, BOMDetailRepository>();
            services.AddTransient<IBOMTermRepository, BOMTermRepository>();
            services.AddTransient<IBOMStageRepository, BOMStageRepository>();
            services.AddTransient<IBOMCompareRepository, BOMCompareRepository>();


            services.AddTransient<IProductionOrderRepository, ProductionOrderRepository>();
            services.AddTransient<IProductionOrderBOMRepository, ProductionOrderBOMRepository>();
            services.AddTransient<IProductionOrderBOMDetailRepository, ProductionOrderBOMDetailRepository>();
            services.AddTransient<IProductionOrderDetailRepository, ProductionOrderDetailRepository>();
            services.AddTransient<IProductionOrderProductionPlanRepository, ProductionOrderProductionPlanRepository>();
            services.AddTransient<IProductionOrderProductionPlanBackupRepository, ProductionOrderProductionPlanBackupRepository>();
            services.AddTransient<IProductionOrderProductionPlanSemiRepository, ProductionOrderProductionPlanSemiRepository>();
            services.AddTransient<IProductionOrderCuttingOrderRepository, ProductionOrderCuttingOrderRepository>();
            services.AddTransient<IProductionOrderSPSTOrderRepository, ProductionOrderSPSTOrderRepository>();
            services.AddTransient<IProductionOrderProductionPlanMaterialRepository, ProductionOrderProductionPlanMaterialRepository>();
            services.AddTransient<IProductionOrderMaterialRepository, ProductionOrderMaterialRepository>();
            services.AddTransient<IProductionOrderOutputScheduleRepository, ProductionOrderOutputScheduleRepository>();
            services.AddTransient<IProductionOrderFileRepository, ProductionOrderFileRepository>();

            services.AddTransient<ICategoryCompanyRepository, CategoryCompanyRepository>();
            services.AddTransient<ICategoryInvoiceRepository, CategoryInvoiceRepository>();


            services.AddTransient<IWarehouseInputRepository, WarehouseInputRepository>();
            services.AddTransient<IWarehouseInputDetailRepository, WarehouseInputDetailRepository>();
            services.AddTransient<IWarehouseInputDetailBarcodeRepository, WarehouseInputDetailBarcodeRepository>();
            services.AddTransient<IWarehouseInputDetailBarcodeMaterialRepository, WarehouseInputDetailBarcodeMaterialRepository>();
            services.AddTransient<IWarehouseInputMaterialRepository, WarehouseInputMaterialRepository>();
            services.AddTransient<IWarehouseInputDetailCountRepository, WarehouseInputDetailCountRepository>();
            services.AddTransient<IWarehouseInputFileRepository, WarehouseInputFileRepository>();
            services.AddTransient<IWarehouseOutputRepository, WarehouseOutputRepository>();
            services.AddTransient<IWarehouseOutputDetailRepository, WarehouseOutputDetailRepository>();
            services.AddTransient<IWarehouseOutputDetailBarcodeRepository, WarehouseOutputDetailBarcodeRepository>();
            services.AddTransient<IWarehouseOutputDetailBarcodeMaterialRepository, WarehouseOutputDetailBarcodeMaterialRepository>();
            services.AddTransient<IWarehouseOutputMaterialRepository, WarehouseOutputMaterialRepository>();
            services.AddTransient<IWarehouseOutputFileRepository, WarehouseOutputFileRepository>();
            services.AddTransient<IWarehouseInventoryRepository, WarehouseInventoryRepository>();
            services.AddTransient<IWarehouseStockRepository, WarehouseStockRepository>();
            services.AddTransient<IWarehouseStockDetailRepository, WarehouseStockDetailRepository>();
            services.AddTransient<IWarehouseRequestRepository, WarehouseRequestRepository>();
            services.AddTransient<IWarehouseRequestDetailRepository, WarehouseRequestDetailRepository>();
            services.AddTransient<IWarehouseRequestConfirmRepository, WarehouseRequestConfirmRepository>();
            services.AddTransient<IWarehouseRequestFileRepository, WarehouseRequestFileRepository>();

            services.AddTransient<IInvoiceInputRepository, InvoiceInputRepository>();
            services.AddTransient<IInvoiceInputDetailRepository, InvoiceInputDetailRepository>();
            services.AddTransient<IInvoiceInputFileRepository, InvoiceInputFileRepository>();
            services.AddTransient<IInvoiceOutputRepository, InvoiceOutputRepository>();
            services.AddTransient<IInvoiceOutputDetailRepository, InvoiceOutputDetailRepository>();
            services.AddTransient<IInvoiceOutputFileRepository, InvoiceOutputFileRepository>();
            services.AddTransient<IInvoiceInputInventoryRepository, InvoiceInputInventoryRepository>();

            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportDetailRepository, ReportDetailRepository>();

            services.AddTransient<IZaloTokenRepository, ZaloTokenRepository>();
            services.AddTransient<IZaloZNSRepository, ZaloZNSRepository>();


            return services;
        }
    }
}

