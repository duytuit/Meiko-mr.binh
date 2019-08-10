import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Roles } from '../model/Roles';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TreeNode } from '@angular/router/src/utils/tree';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class MeikoTDRolesService {
  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Role/';
  ApiAsoft: string = appPublic.apiAsoft;
  getRoles(pz: number, p: number, sort: string, ob: string, s: string, sts: string, u: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('userID', u);
    return this.http.post(this.baseUrl + "R1_RoleGetByList", formData);
  }

  getRolesbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_RoleGetByID/" + id);
  }

  getRolesbySelect() {
    return this.http.get(this.baseUrl + "R1_RoleGetBySelect");
  }


  getUserbyRoleID(A0001_ID: string, s: string) {
    const formData: FormData = new FormData();
    formData.append('s', s);
    formData.append('A0001_ID', A0001_ID);
    return this.http.post(this.baseUrl + "R1_UserGetByRole", formData);
  }

  UpdateUsertoRole(A0001_ID: string, arr: Array<string>) {
    const formData: FormData = new FormData();
    formData.append('A0001_ID', A0001_ID);
    formData.append('ListUser', JSON.stringify(arr));
    return this.http.post(this.baseUrl + "R2_AddUserToRole", formData);
  }

  updateRoles(role: object) {
    var urlAction = "R2_AddRole";
    if (role["A0001_ID"] != null && role["A0001_ID"] != undefined) {
      urlAction = "R3_UpdateRole";
    }
    return this.http.post(this.baseUrl + urlAction, role);
  }

  deleteRoles(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeleteRole", id);
  }

  getBuaAn() {
    return this.http.get(appPublic.apiMeal + "api/buaans");
  }

  getPQBuaAnbyUser(A0001_ID: string, A0002_ID: string) {
    const formData: FormData = new FormData();
    formData.append('A0002_ID', A0002_ID);
    formData.append('A0001_ID', A0001_ID);
    return this.http.post(this.baseUrl + "R1_UserGetByRoleBuaAn", formData);
  }

  updateRoleBuaAnUser(rolebuaan: Array<object>, type: number) {
    const formData: FormData = new FormData();
    formData.append('rolebuaan', JSON.stringify(rolebuaan));
    var Url = "R2_AddRoleBuaAn";
    if (type == 2) {
      Url = "R2_UpdateRoleBuaAn";
    }
    return this.http.post(this.baseUrl + Url, formData);
  }

  updateRoleBuaAnUserAll(rolebuaan: Array<object>) {
    const formData: FormData = new FormData();
    formData.append('rolebuaan', JSON.stringify(rolebuaan));
    return this.http.post(this.baseUrl + "R2_AddRoleBuaAnAll", formData);
  }

  updateRoleBuaAnTime(row: object) {
    return this.http.post(this.baseUrl + "R3_SetTimeBARole", row);
  }

  deleteRoleBuaAn(row: object) {
    return this.http.post(this.baseUrl + "R4_DeleteRoleBuaAn", row);
  }

  deleteUserRole(row: object) {
    return this.http.post(this.baseUrl + "R4_DeleteUserRole", row);
  }

}
