import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class OplserviceService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/KieuBaiViet/';
  baseUrlCTBV: string = appPublic.api_Admin + 'api/ChiTieuBai/';

  //#region Kiểu bài viết

  getKieuBaiViet(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_KieuBaiVietGetByList", formData);
  }

  getKieuBaiVietSelect() {
    return this.http.get(this.baseUrl + "R1_KieuBaiVietGetBySelect");
  }

  getKieuBaiVietGetByID(id: string) {
    return this.http.get(this.baseUrl + "R1_KieuBaiVietGetByID/" + id);
  }

  updateKieuBaiViet(mod: object) {
    var urlAction = "R2_AddKieuBaiViet";
    if (mod["A0043_ID"] != undefined && mod["A0043_ID"] != null) {
      urlAction = "R3_UpdateKieuBaiViet";
    }
    return this.http.post(this.baseUrl + urlAction, mod);
  }

  deleteKieuBaiViet(Id: Array<String>) {
    return this.http.post(this.baseUrl + "R4_DeleteKieuBaiViet", Id);
  }

  //#endregion

  //#region Chi tieu bài viết

  getChitieubai(pz: number, p: number, sort: string, ob: string, s: string, sts: string, month: number, year: number) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('thang', month.toString());
    formData.append('nam', year.toString());
    return this.http.post(this.baseUrlCTBV + "R1_ChiTieuBaiGetByList", formData);
  }

  getChitieubaiSelect() {
    return this.http.get(this.baseUrlCTBV + "R1_ChiTieuBaiGetBySelect");
  }

  getChitieubaiGetByID(id: string) {
    return this.http.get(this.baseUrlCTBV + "R1_ChiTieuBaiGetByID/" + id);
  }

  updateChitieubai(mod: object) {
    var urlAction = "R2_AddChiTieuBai";
    if (mod["A0044_ID"] != undefined && mod["A0044_ID"] != null) {
      urlAction = "R3_UpdateChiTieuBai";
    }
    return this.http.post(this.baseUrlCTBV + urlAction, mod);
  }

  deleteChitieubai(Id: Array<String>) {
    return this.http.post(this.baseUrlCTBV + "R4_DeleteChiTieuBai", Id);
  }

  //#endregion
}
