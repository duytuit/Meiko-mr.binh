import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class CompanyDiagramService {
  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/CompanyDiagram/';
  ApiAsoft: string = appPublic.apiAsoft;
  getCompanyDiagram() {
    // return this.http.get(this.baseUrl + "R1_PhongBanGetByList");
    return this.http.get(this.ApiAsoft + "EC0002");
  }

  getCompanyDiagrambyID(id: string) {
    return this.http.get(this.baseUrl + "R1_PhongBanGetByID/" + id);
  }

  updateCompanyDiagram(obj: object) {
    var urlAction = "R2_AddPhongBan";
    if (obj["istype"] != undefined && obj["istype"] != "add") {
      urlAction = "R3_UpdatePhongBan";
    }
    return this.http.post(this.baseUrl + urlAction, obj);
  }

  deleteCompanyDiagram(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeletePhongBan", Id);
  }
}
