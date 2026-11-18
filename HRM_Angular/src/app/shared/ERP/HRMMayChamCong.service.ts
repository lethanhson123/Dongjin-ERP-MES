import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HRMMayChamCong } from './HRMMayChamCong.model';
import { BaseService } from './Base.service';

@Injectable({
  providedIn: 'root',
})
export class HRMMayChamCongService extends BaseService {
  DisplayColumns003: string[] = ['No', 'ID', 'Active', 'Name', 'Description', 'Note', 'Save'];

  List: HRMMayChamCong[] | undefined;
  FormData!: HRMMayChamCong;
  APIURL: string = environment.APIHRMURL;
  APIRootURL: string = environment.APIHRMRootURL;

  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'HRMMayChamCong';
  }
}
