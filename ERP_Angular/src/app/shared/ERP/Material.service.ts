import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { Material } from './Material.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { MaterialConvert } from 'src/app/shared/ERP/MaterialConvert.model';
import { MaterialConvertService } from 'src/app/shared/ERP/MaterialConvert.service';

@Injectable({
    providedIn: 'root'
})
export class MaterialService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'Code', 'Name', 'Display', 'Note', 'SortOrder', 'Active', 'Save'];
    DisplayColumns002: string[] = ['No', 'ID', 'MESID', 'IsFactory01', 'IsFactory02', 'Code', 'Name', 'Display', 'QuantityInput', 'QuantityOutput', 'Quantity', 'Active'];
    DisplayColumns003: string[] = ['No', 'Code', 'Name', 'Display', 'Active'];
    DisplayColumns004: string[] = ['No', 'ID', 'ParentName', 'CategoryMaterialName', 'CategoryFamilyName', 'CategoryLocationName', 'Code', 'Name', 'Display', 'QuantitySNP', 'IsSNP', 'IsFactory01', 'IsFactory02', 'Active', 'MESID'];
    DisplayColumns005: string[] = ['No', 'ID', 'ParentName', 'CategoryMaterialName', 'CategoryFamilyName', 'CategoryLocationName', 'Code', 'PartNumber', 'Name', 'QuantitySNP', 'IsSNP', 'IsFactory01', 'IsFactory02', 'Active', 'MESID'];
    DisplayColumns006: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'CategoryMaterialName', 'CategoryFamilyName', 'CategoryLocationName', 'Code', 'PartNumber', 'Name', 'QuantitySNP', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns007: string[] = ['No', 'ID', 'CompanyName', 'ParentName', 'CategoryMaterialName', 'Code', 'Name', 'QuantitySNP', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns008: string[] = ['No', 'ID', 'CompanyName', 'CategoryMaterialName', 'Code', 'Name', 'QuantitySNP', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns009: string[] = ['No', 'ID', 'CompanyName', 'CategoryMaterialName', 'Code', 'Name', 'QuantitySNP', 'QuantitySNP_TMMTIN_Last', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns010: string[] = ['No', 'ID', 'CompanyName', 'CategoryMaterialName', 'CategoryFamilyName', 'Code', 'Name', 'QuantitySNP', 'QuantitySNP_TMMTIN_Last', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns011: string[] = ['No', 'ID', 'CompanyName', 'CategoryMaterialName', 'CategoryFamilyName', 'OriginalEquipmentManufacturer', 'CarMaker', 'CarType', 'Item', 'DevelopmentStage', 'Code', 'Name', 'QuantitySNP', 'QuantitySNP_TMMTIN_Last', 'IsSNP', 'Active', 'MESID'];
    DisplayColumns012: string[] = ['No', 'ID', 'CompanyName', 'CategoryMaterialName', 'CategoryFamilyName', 'OriginalEquipmentManufacturer', 'CarMaker', 'CarType', 'Item', 'DevelopmentStage', 'Code', 'Name', 'QuantitySNP', 'QuantitySNP_TMMTIN_Last', 'CategoryLineName', 'CategoryLocationName', 'IsSNP', 'Active', 'MESID'];

    List: Material[] | undefined;
    ListFilter: Material[] | undefined;
    FormData!: Material;
    APIURL: string = environment.APIMaterialURL;
    APIRootURL: string = environment.APIMaterialRootURL;
    constructor(public httpClient: HttpClient,
        public MaterialConvertService: MaterialConvertService,
    ) {
        super(httpClient);
        this.Controller = "Material";

    }
    SearchGetAllToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
                if (this.BaseParameter.Active001 == true) {
                    this.List = this.ListFilter;
                }
                else {
                    if (this.MaterialConvertService.ListFilter001.length == 0) {
                        this.List = this.ListFilter;
                    }
                    else {
                        this.List = [];
                        var List = [...new Map(this.MaterialConvertService.ListFilter001.map(item => [item.ParentID, item])).values()];
                        var ListID = List.map(function (a) { return a.ParentID; });
                        for (let i = 0; i < ListID.length; i++) {
                            let ListSub = this.ListFilter.filter(item => item.ID == ListID[i]);
                            if (ListSub) {
                                if (ListSub.length > 0) {
                                    this.List.push(ListSub[0]);
                                }
                            }
                        }
                    }
                }
                // if (this.BaseParameter.Active002 == true) {
                //     this.List = this.ListFilter;
                // }
                // else {
                //     this.List = this.ListFilter.filter(item => item.Quantity > 0);
                // }
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    ComponentGetByWarehouseInputIDToListAsync(Service: BaseService) {
        this.GetByWarehouseInputIDToListAsync().subscribe(
            res => {
                this.ListChild = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByWarehouseOutputIDToListAsync(Service: BaseService) {
        this.GetByWarehouseOutputIDToListAsync().subscribe(
            res => {
                this.ListParent = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByPurchaseOrderIDInPurchaseOrderMaterialToListAsync(Service: BaseService) {
        this.GetByPurchaseOrderIDInPurchaseOrderMaterialToListAsync().subscribe(
            res => {
                this.ListYear = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    ComponentGetByPurchaseOrderIDInPurchaseOrderBOMToListAsync(Service: BaseService) {
        this.GetByPurchaseOrderIDInPurchaseOrderBOMToListAsync().subscribe(
            res => {
                this.ListMonth = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
            },
            err => {
            },
            () => {
            }
        );
    }
    SaveLineAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveLineAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveLocaltionAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveLocaltionAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveFamilyAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveFamilyAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveLeadNoAndTerm1AndTerm2AndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveLeadNoAndTerm1AndTerm2AndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    CreateAutoAsync() {
        let url = this.APIURL + this.Controller + '/CreateAutoAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SyncParentChildAsync() {
        let url = this.APIURL + this.Controller + '/SyncParentChildAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    ExportToExcelAsync() {
        let url = this.APIURL + this.Controller + '/ExportToExcelAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByWarehouseInputIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseInputIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByWarehouseOutputIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByWarehouseOutputIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByPurchaseOrderIDInPurchaseOrderMaterialToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByPurchaseOrderIDInPurchaseOrderMaterialToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByPurchaseOrderIDInPurchaseOrderBOMToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByPurchaseOrderIDInPurchaseOrderBOMToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryMaterialIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryMaterialIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCategoryMaterialID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCategoryMaterialID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    PrintAsync() {
        let url = this.APIURL + this.Controller + '/PrintAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryMaterialID_SearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryMaterialID_SearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyID_CategoryMaterialID_ActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyID_CategoryMaterialID_ActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SearchGetByCategoryMaterialIDToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetByCategoryMaterialIDToListAsync().subscribe(
            res => {
                this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
                if (this.BaseParameter.Active001 == true) {
                    this.List = this.ListFilter;
                }
                else {
                    if (this.MaterialConvertService.ListFilter001.length == 0) {
                        this.List = this.ListFilter;
                    }
                    else {
                        this.List = [];
                        var List = [...new Map(this.MaterialConvertService.ListFilter001.map(item => [item.ParentID, item])).values()];
                        var ListID = List.map(function (a) { return a.ParentID; });
                        for (let i = 0; i < ListID.length; i++) {
                            let ListSub = this.ListFilter.filter(item => item.ID == ListID[i]);
                            if (ListSub) {
                                if (ListSub.length > 0) {
                                    this.List.push(ListSub[0]);
                                }
                            }
                        }
                    }
                }
                this.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    SearchByCategoryMaterialID(sort: MatSort, paginator: MatPaginator) {
        if (this.BaseParameter.SearchString.length > 0) {
            this.BaseParameter.SearchString = this.BaseParameter.SearchString.trim();
            if (this.DataSource) {
                this.DataSource.filter = this.BaseParameter.SearchString.toLowerCase();
            }
        }
        else {
            this.SearchGetByCategoryMaterialIDToListAsync(sort, paginator);
        }
    }
    ComponentGetByCategoryMaterialID_ActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByCategoryMaterialID_ActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
                        if (this.ListFilter) {
                            if (this.ListFilter.length > 100) {
                                this.ListFilter01 = this.ListFilter.slice(0, 20);
                                this.ListFilter02 = this.ListFilter.slice(0, 20);
                                this.ListFilter03 = this.ListFilter.slice(0, 20);
                                let List = this.ListFilter.filter(o => o.ID == this.BaseParameter.ID);
                                if (List) {
                                    if (List.length > 0) {
                                        this.ListFilter01.push(List[0]);
                                        this.ListFilter02.push(List[0]);
                                        this.ListFilter03.push(List[0]);
                                    }
                                }
                            }
                            else {
                                this.ListFilter01 = this.ListFilter;
                                this.ListFilter02 = this.ListFilter;
                                this.ListFilter03 = this.ListFilter;
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
    }
}

