import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class QuytrinhService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/QuyTrinh/';

  //#region Loại Công Việc
  getLCV(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_LCVGetByList", formData);
  }

  getCVGroupLCVSelect() {
    return this.http.get(this.baseUrl + "R1_CVGroupLCVSelect");
  }

  getLCVSelect() {
    return this.http.get(this.baseUrl + "R1_LCVGetBySelect");
  }

  getLCVbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_LCVGetByID/" + id);
  }

  updateLCV(mod: object) {
    var urlAction = "R2_AddLCV";
    if (mod["A0015_ID"] != undefined && mod["A0015_ID"] != null) {
      urlAction = "R3_UpdateLCV";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteLCV(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteLCV", Id);
  }
  //#endregion

  //#region Công việc
  getCV(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_CongViecGetByList", formData);
  }

  getCVbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_CongViecGetByID/" + id);
  }

  updateCV(mod: object) {
    var urlAction = "R2_AddCongViec";
    if (mod["A0016_ID"] != undefined && mod["A0016_ID"] != null) {
      urlAction = "R3_UpdateCongViec";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteCV(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteCongViec", Id);
  }

  getGroupSinConfig(obj: object) {
    return this.http.get(this.baseUrl + "R1_GetGroupSignByCVID/" + obj["A0016_ID"]);
  }

  saveCVConfigGroupSign(mod: object) {
    var urlAction = "R2_AddConfigSignGroupCV";
    if (mod["A0033_ID"] != undefined && mod["A0033_ID"] != null) {
      urlAction = "R3_UpdateConfigSignGroupCV";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }
  //#endregion

  //#region Nhóm người ký
  getNhomKy(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_NhomKyGetByList", formData);
  }

  getNhomKybyID(id: string) {
    return this.http.get(this.baseUrl + "R1_NhomKyGetByID/" + id);
  }

  updateNhomKy(mod: object) {
    var urlAction = "R2_AddNhomKy";
    if (mod["A0017_ID"] != undefined && mod["A0017_ID"] != null) {
      urlAction = "R3_UpdateNhomKy";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteNhomKy(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteNhomKy", Id);
  }

  //#endregion

  //#region Cấu hình người ký

  getQuyTrinhByGroupSign() {
    return this.http.get(this.baseUrl + "R1_NhomKyGetByGroupSign");
  }

  getGroupSignByPhongBanID(row: object) {
    return this.http.post(this.baseUrl + "R1_GroupSignGetByPhongBanID", row);
  }

  getUserByGroupSignIDPhongBanID(row: object) {
    return this.http.post(this.baseUrl + "R1_UserGetByGroupSignIDPhongBanID", row);
  }

  AddUserToGroupSignIDPhongBanID(obj: object, UserList: Array<string>) {
    const formData: FormData = new FormData();
    formData.append('A0019', JSON.stringify(obj));
    formData.append('UserList', JSON.stringify(UserList));
    return this.http.post(this.baseUrl + "R2_AddUserToGroupSign", formData);
  }

  deleteUserGroupSignPhongBan(obj: object) {
    return this.http.post(this.baseUrl + "R4_DeleteUserGroupSign", obj);
  }

  //#endregion

  //#region Steep ký
  getstepsky() {
    return this.http.get(this.baseUrl + "R1_SteepKyGetByList");
  }

  getstepsbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_SteepKyGetByID/" + id);
  }

  updatestepsky(mod: object) {
    var urlAction = "R2_AddSteepKy";
    if (mod["A0020_ID"] != undefined && mod["A0020_ID"] != null) {
      urlAction = "R3_UpdateSteepKy";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deletestepsky(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteSteepKy", Id);
  }
  //#endregion

  //#region Quy trình

  getQuyTrinh() {
    return this.http.get(this.baseUrl + "R1_QuytrinhGetByList");
  }

  getQuyTrinhbyID(row: object) {
    return this.http.post(this.baseUrl + "R1_QuytrinhGetByID", row);
  }

  updateQuyTrinh(mod: object) {
    var urlAction = "R2_AddQuytrinh";
    if (mod["A0018_ID"] != undefined && mod["A0018_ID"] != null) {
      urlAction = "R3_UpdateQuytrinh";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteQuyTrinh(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteQuytrinh", Id);
  }

  getQuyTrinhByLoaiCongViecID(row: object) {
    return this.http.post(this.baseUrl + "R1_WorkFollowByLCVID", row);
  }

  getQuyTrinhByPhongBanID(row: object) {
    return this.http.post(this.baseUrl + "R1_WorkFollowByPhongBanID", row);
  }

  updateWorkFollowByCVID(obj: object) {
    return this.http.post(this.baseUrl + "R2_AddWorkFollowByCVID", obj);
  }

  deleteWorkFollowByQuyTrinhID(row: object) {
    return this.http.post(this.baseUrl + "R4_DeleteWorkFollowByQuyTrinhID", row);
  }
  //#endregion

  //#region GetQuyTrinh CV

  getWorkFollowCV(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0016_ID', row["A0016_ID"].toString());
    formData.append('A0004_ID', row["A0004_ID"].toString());
    return this.http.post(this.baseUrl + "R1_GroupSignByCVIDPBID", formData);
  }

  getUserbyGroupSign(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0004_ID', row["A0004_ID"].toString());
    formData.append('A0017_ID', row["A0017_ID"].toString());
    return this.http.post(this.baseUrl + "R1_GetUserbyGroupSign", formData);
  }

  //#endregion

  //#region getForm

  getListFormSelect() {
    return this.http.get(this.baseUrl + "R1_FormGetBySelect");
  }

  getListForm(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_FormGetByList", formData);
  }

  getFormbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_FormGetByID/" + id);
  }

  updateForm(mod: object) {
    var urlAction = "R2_AddForm";
    if (mod["A0032_ID"] != undefined && mod["A0032_ID"] != null) {
      urlAction = "R3_UpdateForm";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteForm(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteForm", Id);
  }

  //#endregion

}

