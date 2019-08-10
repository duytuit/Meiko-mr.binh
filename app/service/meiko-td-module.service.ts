import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class MeikoTDModuleService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Module/';

  getModule(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_ModuleGetByList", formData);
  }

  getModulebyID(id: string) {
    return this.http.get(this.baseUrl + "R1_ModuleGetByID/" + id);
  }

  updateModule(mod: object) {
    var urlAction = "R2_AddModule";
    if (mod["A0006_ID"] != undefined && mod["A0006_ID"] != null) {
      urlAction = "R3_UpdateModule";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteModule(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteModule", Id);
  }
}
