import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';
@Injectable({
  providedIn: 'root'
})
export class MeikoTDUserService {
  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/User/';
  ApiAsoft: string = appPublic.apiAsoft;
  getUser(pz: number, p: number, sort: string, ob: string, s: string, sts: string, RoleID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('RoleID', RoleID);
    return this.http.post(this.baseUrl + "R1_UserGetByList", formData);
  }

  getUserNoSign(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_UserGetByListNoSign", formData);
  }

  getUserResetPassword(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_UserGetByListResetPass", formData);
  }

  BindListUser() {
    return this.http.get(this.baseUrl + "BindListUser");
  }

  BindListPhongBan() {
    return this.http.get(this.ApiAsoft + "EC0002");
  }

  getUserbyID(Id: string) {
    return this.http.get(this.baseUrl + "R1_UserGetByID/" + Id);
  }

  updateUser(type: number, formData: FormData) {
    var urlAction = "/R2_AddUser";
    if (type == 2) {
      urlAction = "R3_UpdateUser";
    }
    return this.http.post(this.baseUrl + urlAction, formData);
  }

  changePassword(user: object) {
    return this.http.post(this.baseUrl + "ChangePassword", user);
  }

  ResetPassword(user: object) {
    return this.http.post(this.baseUrl + "ResetPassword", user);
  }

  deleteUser(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeleteUser", id);
  }

  sysUserASoft(pagesize: number, page: number) {
    return this.http.get(this.ApiAsoft + "HT1400/" + pagesize + "/" + page);
  }

  saveUserAsoft() {
    var user = [];
    return this.http.post(this.baseUrl + "R2_ImportUser", user);
  }

  ResetPasswordUser(user: Array<object>) {
    return this.http.post(this.baseUrl + "ResetPasswordRamdom", user);
  }

  ExportUser() {
    return this.http.get(this.baseUrl + "ExportUser");
  }

  ExportUserResetPass() {
    return this.http.get(this.baseUrl + "ExportUserResetPass");
  }

  ExportUserNoSign() {
    return this.http.get(this.baseUrl + "ExportUserNoSign");
  }

  UserGetRole(row: object) {
    return this.http.get(this.baseUrl + "R1_UserGetRolePermisstion/" + row["A0002_ID"]);
  }


  UpdateRoleUserPremisstion(ListRole: Array<string>, userID: string) {
    var formData = new FormData();
    formData.append("RoleList", JSON.stringify(ListRole));
    formData.append("UserID", userID);
    return this.http.post(this.baseUrl + "R3_AddRoleUserPermisstion", formData);
  }

  DeleteRoleUserPremisstion(ListUser: Array<object>) {
    return this.http.post(this.baseUrl + "R4_DeleteRoleUser", ListUser);
  }
  
}
