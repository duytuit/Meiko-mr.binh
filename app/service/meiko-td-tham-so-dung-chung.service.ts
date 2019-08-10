import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class MeikoTDThamSoDungChungService {
  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Label/';

  getLabel(s:string) {
    const formData: FormData = new FormData();
    formData.append('s', s);
    return this.http.post(this.baseUrl + "R1_LabelGetByList",formData);
  }

  getLabelbyID(row: object) {
    return this.http.post(this.baseUrl + "R1_LabelGetByID/", row);
  }

  addLabel(label: object) {
    var urlAction = "R2_AddLabel";
    return this.http.post(this.baseUrl + urlAction, label);
  }

  updateLabel(label: Array<object>) {
    var urlAction = "R3_UpdateLabel";
    return this.http.post(this.baseUrl + urlAction, label);
  }

  deleteLabel(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeleteLabel", id);
  }

}
