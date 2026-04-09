import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { ProductionOrderProductionPlanBackup } from './ProductionOrderProductionPlanBackup.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class ProductionOrderProductionPlanBackupService extends BaseService {   
    DisplayColumns001: string[] = ['No', 'ID', 'UpdateDate', 'MaterialName', 'Priority', 'Quantity00', 'Date01', 'Date02', 'Date03', 'Date04', 'Date05', 'Date06', 'Date07', 'Date08', 'Date09', 'Date10', 'Date11', 'Date12', 'Date13', 'Date14', 'Date15', 'Date16', 'Date17', 'Date18', 'Date19', 'Date20', 'Date21', 'Date22', 'Date23', 'Date24', 'Date25', 'Date26', 'Date27', 'Date28', 'Date29', 'Date30', 'Date31', 'Date32', 'Date33', 'Date34', 'Date35', 'Date36', 'Date37', 'Date38', 'Date39', 'Date40', 'Date41', 'Date42', 'Date43', 'Date44', 'Date45', 'Date46', 'Date47', 'Date48', 'Date49', 'Date50', 'Date51', 'Date52', 'Date53', 'Date54', 'Date55', 'Date56', 'Date57', 'Date58', 'Date59', 'Date60', 'Date61', 'Date62', 'Date63', 'Date64', 'Date65', 'Date66', 'Date67', 'Date68', 'Date69', 'Date70', 'Date71', 'Date72', 'Date73', 'Date74', 'Date75', 'Date76', 'Date77', 'Date78', 'Date79', 'Date80', 'Date81', 'Date82', 'Date83', 'Date84', 'Date85', 'Date86', 'Date87', 'Date88', 'Date89', 'Date90', 'Date91', 'Date92', 'Date93', 'Date94', 'Date95', 'Date96', 'Date97', 'Date98', 'Date99', 'Date100', 'Date101', 'Date102', 'Date103', 'Date104', 'Date105'];    
    DisplayColumns002: string[] = ['No', 'ID', 'UpdateDate', 'UpdateUserCode', 'UpdateUserName', 'MaterialName', 'Priority', 'Quantity00', 'Date01', 'Date02', 'Date03', 'Date04', 'Date05', 'Date06', 'Date07', 'Date08', 'Date09', 'Date10', 'Date11', 'Date12', 'Date13', 'Date14', 'Date15', 'Date16', 'Date17', 'Date18', 'Date19', 'Date20', 'Date21', 'Date22', 'Date23', 'Date24', 'Date25', 'Date26', 'Date27', 'Date28', 'Date29', 'Date30', 'Date31', 'Date32', 'Date33', 'Date34', 'Date35', 'Date36', 'Date37', 'Date38', 'Date39', 'Date40', 'Date41', 'Date42', 'Date43', 'Date44', 'Date45', 'Date46', 'Date47', 'Date48', 'Date49', 'Date50', 'Date51', 'Date52', 'Date53', 'Date54', 'Date55', 'Date56', 'Date57', 'Date58', 'Date59', 'Date60', 'Date61', 'Date62', 'Date63', 'Date64', 'Date65', 'Date66', 'Date67', 'Date68', 'Date69', 'Date70', 'Date71', 'Date72', 'Date73', 'Date74', 'Date75', 'Date76', 'Date77', 'Date78', 'Date79', 'Date80', 'Date81', 'Date82', 'Date83', 'Date84', 'Date85', 'Date86', 'Date87', 'Date88', 'Date89', 'Date90', 'Date91', 'Date92', 'Date93', 'Date94', 'Date95', 'Date96', 'Date97', 'Date98', 'Date99', 'Date100', 'Date101', 'Date102', 'Date103', 'Date104', 'Date105'];    

    List: ProductionOrderProductionPlanBackup[] | undefined;
    ListFilter: ProductionOrderProductionPlanBackup[] | undefined;
    FormData!: ProductionOrderProductionPlanBackup;
    APIURL: string = environment.APIProductionOrderURL;
    APIRootURL: string = environment.APIProductionOrderRootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "ProductionOrderProductionPlanBackup";
    }
    
}

