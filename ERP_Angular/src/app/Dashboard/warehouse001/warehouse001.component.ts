import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets, Chart, ChartConfiguration, ChartData } from 'chart.js';
import { Color, Label, SingleDataSet, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Report } from 'src/app/shared/ERP/Report.model';
import { ReportService } from 'src/app/shared/ERP/Report.service';

import { ReportDetail } from 'src/app/shared/ERP/ReportDetail.model';
import { ReportDetailService } from 'src/app/shared/ERP/ReportDetail.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';

import { WarehouseInventory } from 'src/app/shared/ERP/WarehouseInventory.model';
import { WarehouseInventoryService } from 'src/app/shared/ERP/WarehouseInventory.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';


@Component({
  selector: 'app-warehouse001',
  templateUrl: './warehouse001.component.html',
  styleUrls: ['./warehouse001.component.css']
})
export class Warehouse001Component {

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;

  @ViewChild('WarehouseInventorySort') WarehouseInventorySort: MatSort;
  @ViewChild('WarehouseInventoryPaginator') WarehouseInventoryPaginator: MatPaginator;

  @ViewChild('WarehouseInventorySort01') WarehouseInventorySort01: MatSort;
  @ViewChild('WarehouseInventoryPaginator01') WarehouseInventoryPaginator01: MatPaginator;

  @ViewChild('ReportDetailSort') ReportDetailSort: MatSort;
  @ViewChild('ReportDetailPaginator') ReportDetailPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public ReportService: ReportService,
    public ReportDetailService: ReportDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public WarehouseInventoryService: WarehouseInventoryService,
    public CompanyService: CompanyService,

  ) {
    this.CompanySearch();
    this.GetAll();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.StartTimer5000();
    this.StartTimer();
  }
  ngOnDestroy(): void {
  }
  StartTimer5000() {
    setInterval(() => {
      if (this.ReportService.BaseParameter.CompanyID == 0) {
        this.CompanySearch();
      }
    }, 5000)
  }
  StartTimer() {
    setInterval(() => {
      this.GetAll();
    }, 600000)
  }
  GetAll() {
    if (this.ReportService.BaseParameter.CompanyID > 0) {
      this.WarehouseInventorySearch();
      this.WarehouseInputDetailBarcodeSearch();
      this.WarehouseOutputDetailBarcodeSearch();
      this.WarehouseInventorySearch001();
      this.ReportGetWarehouse001_001ToListAsync();
      this.ReportGetWarehouse001_002ToListAsync();
      this.ReportGetWarehouse001_003ToListAsync();
      this.ReportGetWarehouse001_004ToListAsync();
      this.ReportGetWarehouse001_005ToListAsync();
      this.ReportGetWarehouse001_006ToListAsync();
      this.ReportGetWarehouse001_007ToListAsync();
    }
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ReportService);
    if (this.CompanyService.ListFilter && this.CompanyService.ListFilter.length > 0) {
      this.CompanyService.ListFilter01 = this.CompanyService.ListFilter.filter(o => o.ParentID == environment.CategoryCompanyID);
      if (this.CompanyService.ListFilter01 && this.CompanyService.ListFilter01.length > 0 && this.ReportService.BaseParameter.CompanyID == 0) {
        this.CompanyChange(this.CompanyService.ListFilter01[0]);
      }
    }
  }
  CompanyChange(item: Company) {
    this.ReportService.BaseParameter.CompanyID = item.ID;
    this.WarehouseInventoryService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.WarehouseOutputDetailBarcodeService.BaseParameter.CompanyID = this.ReportService.BaseParameter.CompanyID;
    this.GetAll();
  }
  WarehouseInventorySearch() {
    if (this.WarehouseInventoryService.BaseParameter.SearchString.length > 0) {
      this.WarehouseInventoryService.BaseParameter.SearchString = this.WarehouseInventoryService.BaseParameter.SearchString.trim();
      this.WarehouseInventoryService.BaseParameter.SearchString.toLocaleLowerCase();
      let List0 = this.WarehouseInventoryService.List.filter(o => o.SortOrder == environment.InitializationNumber);
      let List = this.WarehouseInventoryService.List.filter(o => (o.Code && o.Code.length > 0 && o.Code.includes(this.WarehouseInventoryService.BaseParameter.SearchString))
        || (o.ParentName && o.ParentName.length > 0 && o.ParentName.includes(this.WarehouseInventoryService.BaseParameter.SearchString))
      );
      if (List0) {
        if (List0.length > 0) {
          List.push(List0[0]);
        }
      }
      List = List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      this.WarehouseInventoryService.DataSource = new MatTableDataSource(List);
      this.WarehouseInventoryService.DataSource.sort = this.WarehouseInventorySort;
      this.WarehouseInventoryService.DataSource.paginator = this.WarehouseInventoryPaginator;
    }
    else {
      this.WarehouseInventoryService.IsShowLoading = true;
      this.WarehouseInventoryService.BaseParameter.Year = environment.InitializationNumber;
      this.WarehouseInventoryService.BaseParameter.Month = environment.InitializationNumber;
      this.WarehouseInventoryService.GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync().subscribe(
        res => {
          this.WarehouseInventoryService.List = (res as BaseResult).List.sort((a, b) => (a.QuantityEnd < b.QuantityEnd ? 1 : -1));
          if (this.WarehouseInventoryService.List) {
            if (this.WarehouseInventoryService.List.length > 0) {
              if (this.WarehouseInventoryService.BaseParameter.Year == environment.InitializationNumber) {
                this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns003;

                this.WarehouseInventoryService.BaseParameter.BaseModel.ID = environment.InitializationNumber;
                this.WarehouseInventoryService.BaseParameter.BaseModel.SortOrder = environment.InitializationNumber;
                this.WarehouseInventoryService.BaseParameter.BaseModel.QuantityInput00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.QuantityOutput00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.Quantity00 = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.QuantityEnd = 100000000;
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input01 = "Before 2019";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input02 = "2019";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input03 = "2020";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input04 = "2021";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input05 = "2022";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input06 = "2023";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input07 = "2024";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input08 = "2025";
                this.WarehouseInventoryService.BaseParameter.BaseModel.Input09 = "2026";
                this.WarehouseInventoryService.List.push(this.WarehouseInventoryService.BaseParameter.BaseModel);

              }
              else {
                if (this.WarehouseInventoryService.BaseParameter.Month == environment.InitializationNumber) {
                  this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns002;
                }
                else {
                  this.WarehouseInventoryService.DisplayColumns000 = this.WarehouseInventoryService.DisplayColumns001;
                }
              }
              this.WarehouseInventoryService.List = this.WarehouseInventoryService.List.sort((a, b) => (a.QuantityEnd < b.QuantityEnd ? 1 : -1));
            }
          }
          this.WarehouseInventoryService.DataSource = new MatTableDataSource(this.WarehouseInventoryService.List);
          this.WarehouseInventoryService.DataSource.sort = this.WarehouseInventorySort;
          this.WarehouseInventoryService.DataSource.paginator = this.WarehouseInventoryPaginator;

          this.WarehouseInventoryService.ListFilter = this.WarehouseInventoryService.List.filter(o => o.Quantity01 > 0 || o.Quantity02 > 0 || o.Quantity03 > 0 || o.Quantity04 > 0 || o.Quantity05 > 0 || o.Quantity06 > 0 || o.Quantity07 > 0);
          if (this.WarehouseInventoryService.ListFilter && this.WarehouseInventoryService.ListFilter.length > 0) {
            this.WarehouseInventoryService.ListFilter.push(this.WarehouseInventoryService.BaseParameter.BaseModel);
          }
          this.WarehouseInventoryService.ListFilter = this.WarehouseInventoryService.ListFilter.sort((a, b) => (a.QuantityEnd < b.QuantityEnd ? 1 : -1));
          this.WarehouseInventoryService.DataSourceFilter = new MatTableDataSource(this.WarehouseInventoryService.ListFilter);
          this.WarehouseInventoryService.DataSourceFilter.sort = this.WarehouseInventorySort01;
          this.WarehouseInventoryService.DataSourceFilter.paginator = this.WarehouseInventoryPaginator01;
        },
        err => {
        },
        () => {
          this.WarehouseInventoryService.IsShowLoading = false;
        }
      );
    }
  }

  WarehouseInputDetailBarcodeSearch() {
    this.WarehouseInputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List;
        this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
        this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
        this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
      },
      err => {
      },
      () => {
        this.WarehouseInputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  WarehouseOutputDetailBarcodeSearch() {
    this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
    this.WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync().subscribe(
      res => {
        this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List;
        this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
        this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
        this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;
      },
      err => {
      },
      () => {
        this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
      }
    );
  }

  ReportGetWarehouse001_001ToListAsync() {
    //this.ReportService.IsShowLoading = true;    
    this.ReportService.GetWarehouse001_001ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData001 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
                this.ReportService.IsShowLoading = false;
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_002ToListAsync() {
    this.ReportService.GetWarehouse001_002ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData002 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_003ToListAsync() {
    this.ReportService.GetWarehouse001_003ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData003 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_004ToListAsync() {
    this.ReportService.GetWarehouse001_004ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData004 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_005ToListAsync() {
    this.ReportService.GetWarehouse001_005ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData005 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_006ToListAsync() {
    this.ReportService.GetWarehouse001_006ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.List = (res as BaseResult).List;
                if (this.ReportDetailService.List) {
                  if (this.ReportDetailService.List.length > 0) {
                    this.ReportDetailService.FormData006 = this.ReportDetailService.List[0];
                  }
                }
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  ReportGetWarehouse001_007ToListAsync() {
    this.ReportService.GetWarehouse001_007ToListAsync().subscribe(
      res => {
        this.ReportService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        if (this.ReportService.BaseParameter.BaseModel) {
          if (this.ReportService.BaseParameter.BaseModel.ID) {
            this.ReportDetailService.BaseParameter.ParentID = this.ReportService.BaseParameter.BaseModel.ID;
            this.ReportDetailService.GetByParentIDToListAsync().subscribe(
              res => {
                this.ReportDetailService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Quantity01 < b.Quantity01 ? 1 : -1));;
                this.ReportDetailService.DataSource = new MatTableDataSource(this.ReportDetailService.ListFilter);
                this.ReportDetailService.DataSource.sort = this.ReportDetailSort;
                this.ReportDetailService.DataSource.paginator = this.ReportDetailPaginator;
              },
              err => {
              },
              () => {
              }
            );
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }

  WarehouseInventorySearch001() {
    this.WarehouseInventoryService.IsShowLoading = true;
    this.WarehouseInventoryService.BaseParameter.Active = true;
    this.WarehouseInventoryService.BaseParameter.CategoryDepartmentID = environment.DepartmentID;
    this.WarehouseInventoryService.BaseParameter.Year = environment.InitializationNumber;
    this.WarehouseInventoryService.BaseParameter.Month = environment.InitializationNumber;
    this.WarehouseInventoryService.GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync().subscribe(
      res => {
        this.WarehouseInventoryService.ListYear = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        let ListReportLabel = [];
        let ListReportData001 = [];
        let ListReportData002 = [];
        for (let i = 0; i < this.WarehouseInventoryService.ListYear.length; i++) {
          ListReportLabel.push(this.WarehouseInventoryService.ListYear[i].Code);
          ListReportData001.push(this.WarehouseInventoryService.ListYear[i].Quantity00);
        }
        this.Report0001_Label = ListReportLabel;
        this.Report0001_Data = [
          { data: ListReportData001, label: "Raw Material", stack: 'a1' },
        ];

        this.Report0002_Label = ListReportLabel;
        this.Report0002_Data = [
          { data: ListReportData001, label: "Raw Material", stack: 'a1' },
        ];
      },
      err => {
      },
      () => {
        this.WarehouseInventoryService.IsShowLoading = false;
      }
    );
  }

  public Report0001_Option: ChartOptions = {
    responsive: true,
    legend: {
      display: true,
      position: 'bottom'
    },
    tooltips: {
      callbacks: {
        label: function (tooltipItem, data) {
          var label = data.labels[tooltipItem.index];
          var value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
          return '' + value;
        }
      }
    }
  };
  public Report0001_Color: Color[] = [
  ]
  public Report0001_Label: Label[] = [];
  public Report0001_Type: ChartType = 'bar';
  public Report0001_Legend = true;
  public Report0001_Plugin = [
  ];
  public Report0001_Data: ChartDataSets[] = [
  ];

  public Report0002_Option: ChartOptions = {
    responsive: true,
    legend: {
      display: true,
      position: 'bottom'
    },
    tooltips: {
      callbacks: {
        label: function (tooltipItem, data) {
          var label = data.labels[tooltipItem.index];
          var value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];
          return '' + value;
        }
      }
    }
  };
  public Report0002_Color: Color[] = [
  ]
  public Report0002_Label: Label[] = [];
  public Report0002_Type: ChartType = 'pie';
  public Report0002_Legend = true;
  public Report0002_Plugin = [
  ];
  public Report0002_Data: ChartDataSets[] = [
  ];

}
