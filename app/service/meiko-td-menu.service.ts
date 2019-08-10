import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class MeikoTDMenuService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Menu/';

  getMenu() {
    return this.http.get(this.baseUrl + "R1_MenuGetByList")
  }

  getMenuByID(obj: object) {
    return this.http.post(this.baseUrl + "R1_MenuGetByID/", obj);
  }

  updateMenu(formData: FormData, istype: number) {
    var urlAction = "R2_AddMenu";
    if (istype == 2) {
      urlAction = "R3_UpdateMenu";
    }
    return this.http.post(this.baseUrl + urlAction, formData);
  }

  deleteMenu(Id: object) {
    return this.http.post(this.baseUrl + "R4_DeleteMenu", Id);
  }
}
