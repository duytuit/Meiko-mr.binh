import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class RoleUserService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/RoleMenu/';

  getRoleUser(A0001_ID: string, A0002_ID: string) {
    const formData: FormData = new FormData();
    formData.append('A0001_ID', A0001_ID);
    formData.append('A0002_ID', A0002_ID);
    return this.http.post(this.baseUrl + "R1_RoleMenuGetByList/", formData);
  }

  updateRoleMenu(rolemenu: Array<object>, A0001_ID: string, type: number) {
    const formData: FormData = new FormData();
    formData.append('rolemenu', JSON.stringify(rolemenu));
    formData.append('A0001_ID', A0001_ID);
    formData.append('type', type.toString());
    return this.http.post(this.baseUrl + "R2_AddRoleMenu", formData);
    // return this.http.post(this.baseUrl + "R2_AddRoleMenu", A0001_ID);
  }

  updateRoleMenuUser(rolemenu: Array<object>, A0001_ID: string, A0002_ID: string) {
    const formData: FormData = new FormData();
    formData.append('rolemenu', JSON.stringify(rolemenu));
    formData.append('A0001_ID', A0001_ID);
    formData.append('A0002_ID', A0002_ID);
    return this.http.post(this.baseUrl + "R2_AddRoleMenuUser", formData);
  }
}
