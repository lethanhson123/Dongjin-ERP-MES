import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets, Chart, ChartConfiguration, ChartData } from 'chart.js';
import { Color, Label, SingleDataSet, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';

import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';



import { ProductionOrderProductionPlanBackup } from 'src/app/shared/ERP/ProductionOrderProductionPlanBackup.model';
import { ProductionOrderProductionPlanBackupService } from 'src/app/shared/ERP/ProductionOrderProductionPlanBackup.service';

@Component({
  selector: 'app-production-order-production-plan-backup',
  templateUrl: './production-order-production-plan-backup.component.html',
  styleUrls: ['./production-order-production-plan-backup.component.css']
})
export class ProductionOrderProductionPlanBackupComponent implements OnInit {

   @ViewChild('ProductionOrderProductionPlanBackupSort') ProductionOrderProductionPlanBackupSort: MatSort;
  @ViewChild('ProductionOrderProductionPlanBackupPaginator') ProductionOrderProductionPlanBackupPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<ProductionOrderProductionPlanBackupComponent>,    
    

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ProductionOrderProductionPlanBackupService: ProductionOrderProductionPlanBackupService,
  ) {
    this.ProductionOrderProductionPlanBackupSearch();

  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
   Close() {
    this.DialogRef.close();
  }
  ProductionOrderProductionPlanBackupSearch() {    
    if (this.ProductionOrderProductionPlanBackupService.BaseParameter.ParentID > 0) {
      if (this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString && this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString.length > 0) {
        this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString = this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString.trim();
        // if (this.ProductionOrderProductionPlanBackupService.DataSource) {
        //   this.ProductionOrderProductionPlanBackupService.DataSource.filter = this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString.toLowerCase();
        // }

        let List = this.ProductionOrderProductionPlanBackupService.List.filter(o => o.SortOrder == 1);
        this.ProductionOrderProductionPlanBackupService.ListFilter = this.ProductionOrderProductionPlanBackupService.List.filter(o => o.SortOrder > 1 && (o.MaterialCode == this.ProductionOrderProductionPlanBackupService.BaseParameter.SearchString));
        if (List && List.length > 0) {
          this.ProductionOrderProductionPlanBackupService.ListFilter.push(List[0]);
        }
        this.ProductionOrderProductionPlanBackupService.ListFilter = this.ProductionOrderProductionPlanBackupService.ListFilter.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));        
        this.ProductionOrderProductionPlanBackupService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanBackupService.ListFilter);
        this.ProductionOrderProductionPlanBackupService.DataSource.sort = this.ProductionOrderProductionPlanBackupSort;
        this.ProductionOrderProductionPlanBackupService.DataSource.paginator = this.ProductionOrderProductionPlanBackupPaginator;
      }
      else {
        this.ProductionOrderProductionPlanBackupService.List = [];
        this.ProductionOrderProductionPlanBackupService.BaseParameter.ParentID = this.ProductionOrderProductionPlanBackupService.BaseParameter.ParentID;
        this.ProductionOrderProductionPlanBackupService.IsShowLoading = true;
        this.ProductionOrderProductionPlanBackupService.GetByParentIDToListAsync().subscribe(
          res => {
            this.ProductionOrderProductionPlanBackupService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProductionOrderProductionPlanBackupService.ListFilter = this.ProductionOrderProductionPlanBackupService.List;
            this.ProductionOrderProductionPlanBackupService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanBackupService.ListFilter);
            this.ProductionOrderProductionPlanBackupService.DataSource.sort = this.ProductionOrderProductionPlanBackupSort;
            this.ProductionOrderProductionPlanBackupService.DataSource.paginator = this.ProductionOrderProductionPlanBackupPaginator;
          },
          err => {
          },
          () => {
            this.ProductionOrderProductionPlanBackupService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProductionOrderProductionPlanBackupService.List = [];
      this.ProductionOrderProductionPlanBackupService.DataSource = new MatTableDataSource(this.ProductionOrderProductionPlanBackupService.List);
      this.ProductionOrderProductionPlanBackupService.DataSource.sort = this.ProductionOrderProductionPlanBackupSort;
      this.ProductionOrderProductionPlanBackupService.DataSource.paginator = this.ProductionOrderProductionPlanBackupPaginator;
    }
  }
}
