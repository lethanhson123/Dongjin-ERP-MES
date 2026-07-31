import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { Material } from './Material.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';



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
    
    constructor(public httpClient: HttpClient,        
    ) {
        super(httpClient);
        this.Controller = "Material";

    }
    
}

