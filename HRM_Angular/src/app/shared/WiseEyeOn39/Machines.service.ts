import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { Machines } from './Machines.model';
import { BaseService } from '../ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class MachinesService extends BaseService {
    DisplayColumns001: string[] = ['No', 'ID', 'MachineID', 'MachineName', 'COMPort', 'IPAddress', 'Port', ];    


    List: Machines[] | undefined;
    ListFilter: Machines[] | undefined;
    FormData!: Machines;
    APIURL: string = environment.APIWiseEyeOn39URL;
    APIRootURL: string = environment.APIWiseEyeOn39RootURL;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "Machines";
    }    
   
}

