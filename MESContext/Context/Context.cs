
namespace MESContext.Context
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }
        public Context(DbContextOptions<Context>
    options)
    : base(options)
        {
        }
        public virtual DbSet<KOMAXCheckErrorProofHistory> KOMAXCheckErrorProofHistory { get; set; }
        public virtual DbSet<IQCNGCustomer2> IQCNGCustomer2 { get; set; }
        public virtual DbSet<Trolley> Trolley { get; set; }
        public virtual DbSet<ToolShop> ToolShop { get; set; }
        public virtual DbSet<tfg_inventory> tfg_inventory { get; set; }
        public virtual DbSet<tfg_packing_detail> tfg_packing_detail { get; set; }
        public virtual DbSet<tfg_history> tfg_history { get; set; }
        public virtual DbSet<PartSpare> PartSpare { get; set; }
        public virtual DbSet<PartSpareScanIn> PartSpareScanIn { get; set; }
        public virtual DbSet<PartSpareScanOut> PartSpareScanOut { get; set; }
        public virtual DbSet<MaintenanceHistory> MaintenanceHistory { get; set; }
        public virtual DbSet<DowntimeRecords> DowntimeRecords { get; set; }
        public virtual DbSet<AttendanceRecords> AttendanceRecords { get; set; }
        public virtual DbSet<AttendanceSession> AttendanceSession { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<FAWorkOrderHistory> FAWorkOrderHistory { get; set; }
        public virtual DbSet<TaskTimeFA> TaskTimeFA { get; set; }
        public virtual DbSet<FAWorkOrder> FAWorkOrder { get; set; }
        public virtual DbSet<NGList> NGList { get; set; }
        public virtual DbSet<OQC_NG> OQC_NG { get; set; }
        public virtual DbSet<EmployeeFile> EmployeeFile { get; set; }
        public virtual DbSet<EmployeeFinance> EmployeeFinance { get; set; }
        public virtual DbSet<EmployeeContract> EmployeeContract { get; set; }
        public virtual DbSet<ROTestLog> ROTestLog { get; set; }
        public virtual DbSet<EmployeeJob> EmployeeJob { get; set; }
        public virtual DbSet<PersonalInfo> PersonalInfo { get; set; }
        public virtual DbSet<FAProduction> FAProduction { get; set; }
        public virtual DbSet<LineList> LineList { get; set; }
        public virtual DbSet<LineAssignment> LineAssignment { get; set; }
        public virtual DbSet<ShiftTime> ShiftTime { get; set; }
        public virtual DbSet<EmployeeFA> EmployeeFA { get; set; }
        public virtual DbSet<aatable> aatable { get; set; }
        public virtual DbSet<apqp_cdgr> apqp_cdgr { get; set; }
        public virtual DbSet<apqp_code> apqp_code { get; set; }
        public virtual DbSet<apqp_dlylst> apqp_dlylst { get; set; }
        public virtual DbSet<apqp_filelst> apqp_filelst { get; set; }
        public virtual DbSet<apqp_mstlst> apqp_mstlst { get; set; }
        public virtual DbSet<kr_inspctn_st> kr_inspctn_st { get; set; }
        public virtual DbSet<kr_inspctn_test> kr_inspctn_test { get; set; }
        public virtual DbSet<kr_tdd_poplan> kr_tdd_poplan { get; set; }
        public virtual DbSet<kr_tdpdmtim> kr_tdpdmtim { get; set; }
        public virtual DbSet<kr_tdpdmtim_tmp> kr_tdpdmtim_tmp { get; set; }
        public virtual DbSet<kr_tdpdmtim_tmp_out> kr_tdpdmtim_tmp_out { get; set; }
        public virtual DbSet<kr_tdpdotpl> kr_tdpdotpl { get; set; }
        public virtual DbSet<kr_tdpdotpl_inpo> kr_tdpdotpl_inpo { get; set; }
        public virtual DbSet<kr_tiivtr> kr_tiivtr { get; set; }
        public virtual DbSet<pd_asset_mm> pd_asset_mm { get; set; }
        public virtual DbSet<pd_cmpny_costfile> pd_cmpny_costfile { get; set; }
        public virtual DbSet<pd_cmpny_part> pd_cmpny_part { get; set; }
        public virtual DbSet<pd_inout_part> pd_inout_part { get; set; }
        public virtual DbSet<pd_mc_orderlist> pd_mc_orderlist { get; set; }
        public virtual DbSet<pd_part_cost> pd_part_cost { get; set; }
        public virtual DbSet<pd_tiivtr> pd_tiivtr { get; set; }
        public virtual DbSet<pdcdgr> pdcdgr { get; set; }
        public virtual DbSet<pdcdnm> pdcdnm { get; set; }
        public virtual DbSet<pdcmpny> pdcmpny { get; set; }
        public virtual DbSet<pdpart> pdpart { get; set; }
        public virtual DbSet<pdpart_addlist> pdpart_addlist { get; set; }
        public virtual DbSet<pdpusch> pdpusch { get; set; }
        public virtual DbSet<tdd_ct_st> tdd_ct_st { get; set; }
        public virtual DbSet<tdd_poplan> tdd_poplan { get; set; }
        public virtual DbSet<tdd_poplan_djg> tdd_poplan_djg { get; set; }
        public virtual DbSet<tdpdmtim> tdpdmtim { get; set; }
        public virtual DbSet<tdpdmtim_autobc_list> tdpdmtim_autobc_list { get; set; }
        public virtual DbSet<tdpdmtim_del> tdpdmtim_del { get; set; }
        public virtual DbSet<tdpdmtim_hist> tdpdmtim_hist { get; set; }
        public virtual DbSet<tdpdmtim_loc> tdpdmtim_loc { get; set; }
        public virtual DbSet<tdpdmtim_rework> tdpdmtim_rework { get; set; }
        public virtual DbSet<tdpdmtim_tmp> tdpdmtim_tmp { get; set; }
        public virtual DbSet<tdpdmtin_serial> tdpdmtin_serial { get; set; }
        public virtual DbSet<tdpdotpl> tdpdotpl { get; set; }
        public virtual DbSet<tdpdotpl_aloc> tdpdotpl_aloc { get; set; }
        public virtual DbSet<tdpdotpl_etc> tdpdotpl_etc { get; set; }
        public virtual DbSet<tdpdotpl_label> tdpdotpl_label { get; set; }
        public virtual DbSet<tdpdotpl_tmp> tdpdotpl_tmp { get; set; }
        public virtual DbSet<tdpdotplmu> tdpdotplmu { get; set; }
        public virtual DbSet<tfg_monitor> tfg_monitor { get; set; }
        public virtual DbSet<tiivaj> tiivaj { get; set; }
        public virtual DbSet<tiivaj_history> tiivaj_history { get; set; }
        public virtual DbSet<tiivaj_lead> tiivaj_lead { get; set; }
        public virtual DbSet<tiivtr> tiivtr { get; set; }
        public virtual DbSet<tiivtr_excel> tiivtr_excel { get; set; }
        public virtual DbSet<tiivtr_history> tiivtr_history { get; set; }
        public virtual DbSet<tiivtr_lead> tiivtr_lead { get; set; }
        public virtual DbSet<tiivtr_lead_fg> tiivtr_lead_fg { get; set; }
        public virtual DbSet<tiivtr_lead_history> tiivtr_lead_history { get; set; }
        public virtual DbSet<tmbrcd> tmbrcd { get; set; }
        public virtual DbSet<tmbrcd_his> tmbrcd_his { get; set; }
        public virtual DbSet<tmbrcd_longterm> tmbrcd_longterm { get; set; }
        public virtual DbSet<tmmtin> tmmtin { get; set; }
        public virtual DbSet<tmmtin_dmm> tmmtin_dmm { get; set; }
        public virtual DbSet<tmmtin_dmm_app> tmmtin_dmm_app { get; set; }
        public virtual DbSet<tmmtin_dmm_cut> tmmtin_dmm_cut { get; set; }
        public virtual DbSet<tmmtin_dmm_lead> tmmtin_dmm_lead { get; set; }
        public virtual DbSet<torder_barcode> torder_barcode { get; set; }
        public virtual DbSet<torder_barcode_lp> torder_barcode_lp { get; set; }
        public virtual DbSet<torder_barcode_sp> torder_barcode_sp { get; set; }
        public virtual DbSet<torder_bom> torder_bom { get; set; }
        public virtual DbSet<torder_bom_lp> torder_bom_lp { get; set; }
        public virtual DbSet<torder_bom_not_climp> torder_bom_not_climp { get; set; }
        public virtual DbSet<torder_bom_spst1> torder_bom_spst1 { get; set; }
        public virtual DbSet<torder_bom_spst2> torder_bom_spst2 { get; set; }
        public virtual DbSet<torder_bom_sw> torder_bom_sw { get; set; }
        public virtual DbSet<torder_lead_bom> torder_lead_bom { get; set; }
        public virtual DbSet<torder_lead_bom_excl> torder_lead_bom_excl { get; set; }
        public virtual DbSet<torder_lead_bom_spst> torder_lead_bom_spst { get; set; }
        public virtual DbSet<torder_lead_bom_spst_excl> torder_lead_bom_spst_excl { get; set; }
        public virtual DbSet<torder_spc> torder_spc { get; set; }
        public virtual DbSet<torderinspection> torderinspection { get; set; }
        public virtual DbSet<torderinspection_lp> torderinspection_lp { get; set; }
        public virtual DbSet<torderinspection_spst> torderinspection_spst { get; set; }
        public virtual DbSet<torderinspection_sw> torderinspection_sw { get; set; }
        public virtual DbSet<torderlist> torderlist { get; set; }
        public virtual DbSet<torderlist_lp> torderlist_lp { get; set; }
        public virtual DbSet<torderlist_lplist> torderlist_lplist { get; set; }
        public virtual DbSet<torderlist_spst> torderlist_spst { get; set; }
        public virtual DbSet<torderlist_sw> torderlist_sw { get; set; }
        public virtual DbSet<torderlist_work> torderlist_work { get; set; }
        public virtual DbSet<torderlist_wtime> torderlist_wtime { get; set; }
        public virtual DbSet<track_bc_tmp> track_bc_tmp { get; set; }
        public virtual DbSet<trackmaster> trackmaster { get; set; }
        public virtual DbSet<trackmtim> trackmtim { get; set; }
        public virtual DbSet<trackmtim_lt_insp> trackmtim_lt_insp { get; set; }
        public virtual DbSet<tsauth> tsauth { get; set; }
        public virtual DbSet<tsbom> tsbom { get; set; }
        public virtual DbSet<tsbom_list> tsbom_list { get; set; }
        public virtual DbSet<tsbom_part_inf> tsbom_part_inf { get; set; }
        public virtual DbSet<tsbom_po_list> tsbom_po_list { get; set; }
        public virtual DbSet<tsbom_ver02> tsbom_ver02 { get; set; }
        public virtual DbSet<tsbom_ver02_po> tsbom_ver02_po { get; set; }
        public virtual DbSet<tsbom_ver02_tmp1> tsbom_ver02_tmp1 { get; set; }
        public virtual DbSet<tscdgr> tscdgr { get; set; }
        public virtual DbSet<tscmpny> tscmpny { get; set; }
        public virtual DbSet<tscode> tscode { get; set; }
        public virtual DbSet<tscost> tscost { get; set; }
        public virtual DbSet<tscost_list> tscost_list { get; set; }
        public virtual DbSet<tscut_st_uph> tscut_st_uph { get; set; }
        public virtual DbSet<tsmenu> tsmenu { get; set; }
        public virtual DbSet<tsmnau> tsmnau { get; set; }
        public virtual DbSet<tsmonitor_set> tsmonitor_set { get; set; }
        public virtual DbSet<tsnon_oper> tsnon_oper { get; set; }
        public virtual DbSet<tsnon_oper_andon> tsnon_oper_andon { get; set; }
        public virtual DbSet<tsnon_oper_andon_list> tsnon_oper_andon_list { get; set; }
        public virtual DbSet<tsnon_oper_mitor> tsnon_oper_mitor { get; set; }
        public virtual DbSet<tsnon_oper_worker> tsnon_oper_worker { get; set; }
        public virtual DbSet<tsnon_worktime> tsnon_worktime { get; set; }
        public virtual DbSet<tsnotice> tsnotice { get; set; }
        public virtual DbSet<tspart> tspart { get; set; }
        public virtual DbSet<tspart_addtnl> tspart_addtnl { get; set; }
        public virtual DbSet<tspart_ecn> tspart_ecn { get; set; }
        public virtual DbSet<tspart_file> tspart_file { get; set; }
        public virtual DbSet<tsurau> tsurau { get; set; }
        public virtual DbSet<tsuser> tsuser { get; set; }
        public virtual DbSet<tsuser_requ> tsuser_requ { get; set; }
        public virtual DbSet<tsuser_super> tsuser_super { get; set; }
        public virtual DbSet<tsyear_group_inv> tsyear_group_inv { get; set; }
        public virtual DbSet<tsyear_group_inv_hist> tsyear_group_inv_hist { get; set; }
        public virtual DbSet<tsyear_inventory> tsyear_inventory { get; set; }
        public virtual DbSet<tsyear_inventory_hist> tsyear_inventory_hist { get; set; }
        public virtual DbSet<ttc_barcode> ttc_barcode { get; set; }
        public virtual DbSet<ttc_bom> ttc_bom { get; set; }
        public virtual DbSet<ttc_order> ttc_order { get; set; }
        public virtual DbSet<ttc_part> ttc_part { get; set; }
        public virtual DbSet<ttc_rackmtin> ttc_rackmtin { get; set; }
        public virtual DbSet<ttensilbndlst> ttensilbndlst { get; set; }
        public virtual DbSet<ttensilforce> ttensilforce { get; set; }
        public virtual DbSet<ttensilforce_usw> ttensilforce_usw { get; set; }
        public virtual DbSet<ttoolhistory> ttoolhistory { get; set; }
        public virtual DbSet<ttoolmaster> ttoolmaster { get; set; }
        public virtual DbSet<ttoolmaster2> ttoolmaster2 { get; set; }
        public virtual DbSet<tuser_log> tuser_log { get; set; }
        public virtual DbSet<tuser_log_chk> tuser_log_chk { get; set; }
        public virtual DbSet<tuser_log_chk_list> tuser_log_chk_list { get; set; }
        public virtual DbSet<tworkresult> tworkresult { get; set; }
        public virtual DbSet<twtool> twtool { get; set; }
        public virtual DbSet<twwkar> twwkar { get; set; }
        public virtual DbSet<twwkar_lp> twwkar_lp { get; set; }
        public virtual DbSet<twwkar_spst> twwkar_spst { get; set; }
        public virtual DbSet<xsetting_time> xsetting_time { get; set; }
        public virtual DbSet<zadmin_function> zadmin_function { get; set; }
        public virtual DbSet<zt_devlpmnt_db> zt_devlpmnt_db { get; set; }
        public virtual DbSet<zt_help_db> zt_help_db { get; set; }
        public virtual DbSet<zt_log_db> zt_log_db { get; set; }
        public virtual DbSet<zz_mes_ver> zz_mes_ver { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var decimalProps = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));
            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

