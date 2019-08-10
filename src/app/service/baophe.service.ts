import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';
import { stringify } from 'querystring';

@Injectable({
  providedIn: 'root'
})
export class BaopheService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Baophe/';

  GetListNDBaoPhe(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_NoiDungBPGetByList", formData);
  }

  GetListNDBaoPheBySelect() {
    return this.http.get(this.baseUrl + "R1_NoiDungBPGetBySelect");
  }

  GetListNDBaoPheByID(row: object) {
    return this.http.get(this.baseUrl + "R1_NoiDungBPGetByID/" + row["A0035_ID"]);
  }

  getPhongBanByDMBaoPheID(row: object) {
    return this.http.get(this.baseUrl + "getPhongBanByDMBaoPheID/" + row["A0035_ID"]);
  }

  GetListNDBaoPheByPhongBan(row: object) {
    return this.http.get(this.baseUrl + "R1_NoiDungBPGetByPhongBan/" + row["C001C"]);
  }

  GetDMBaoPheByPhongBanID(row: object) {
    return this.http.get(this.baseUrl + "R1_GetDMBaoPheByPhongBanID/" + row["A0034_ID"]);
  }

  updateNDBaoPhe(obj: object) {
    var urlAction = "R2_AddNoiDungBP";
    if (obj["A0035_ID"] != undefined && obj["A0035_ID"] != null) {
      urlAction = "R3_UpdateNoiDungBP";
    }
    return this.http.post(this.baseUrl + urlAction, obj);
  }

  updatePhongBanToNDBaoPhe(obj: object, listPB: Array<object>) {
    const formData: FormData = new FormData();
    formData.append('A0035_ID', obj["A0035_ID"]);
    formData.append('ListA0034', JSON.stringify(listPB));
    return this.http.post(this.baseUrl + "R2_AddPBToDMBaoPhe", formData);
  }

  updateDMBaoPheToPhongBan(obj: object, listDM: Array<object>) {
    const formData: FormData = new FormData();
    formData.append('A0034_ID', obj["A0034_ID"]);
    formData.append('ListA0035', JSON.stringify(listDM));
    return this.http.post(this.baseUrl + "R2_AddDMBaoPheToPhongBan", formData);
  }

  deleteNDBaoPhe(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeleteNoiDungBP", id);
  }

  GetListPhongBanMapper(pz: number, p: number, sort: string, ob: string, s: string, sts: string, istype: number) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('kieuPhongBan', istype.toString());
    return this.http.post(this.baseUrl + "R1_PhongBanMapperGetByList", formData);
  }

  GetPhongBanMapperByID(row: object) {
    return this.http.get(this.baseUrl + "R1_PhongBanMapperGetByID/" + row["A0034_ID"]);
  }

  updatePhongBanMapper(obj: object) {
    var urlAction = "R2_AddPhongBanMapper";
    if (obj["A0034_ID"] != undefined && obj["A0034_ID"] != null) {
      urlAction = "R3_UpdatePhongBanMapper";
    }
    return this.http.post(this.baseUrl + urlAction, obj);
  }

  deletePhongBanMapper(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeletePhongBanMapper", id);
  }

  GetAllPhongBanMapper() {
    return this.http.get(this.baseUrl + "R1_PhongBanGetBaoPhe");
  }

  GetAllBoPhanMapper() {
    return this.http.get(this.baseUrl + "R1_BoPhanGetAll");
  }

  GetQuyTrinhBaoPhe() {
    return this.http.get(this.baseUrl + "R1_GetAllQuyTrinh");
  }

  SendNextDocument(row: object, userID: string, noiDung: string) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0002_ID', userID);
    formData.append('Note', noiDung);
    return this.http.post(this.baseUrl + "R2_SignAndSend", formData);
  }

  updateQuyTrinhBaoPhe(obj: object, istype) {
    const formData: FormData = new FormData();
    formData.append('A0040', JSON.stringify(obj));
    formData.append('IsTypeSign', istype);
    return this.http.post(this.baseUrl + "R2_UpdateQuyTrinhBaoPhe", formData);
  }

  GetListAllMaHang() {
    return this.http.get(appPublic.apiParadigmAPI + "Data0050");
  }

  DeleteDMBPPhongBan(ListDMBP: Array<string>, A0034_ID, Istype) {
    const formData: FormData = new FormData();
    formData.append('ListA0035', JSON.stringify(ListDMBP));
    formData.append('A0034_ID', A0034_ID);
    formData.append('IsType', Istype);
    return this.http.post(this.baseUrl + "R4_DeleteDMBPPhongBan", formData);
  }

  DeletePhongBanDMBP(ListDMBP: Array<string>, A0035_ID, Istype) {
    const formData: FormData = new FormData();
    formData.append('ListA0034', JSON.stringify(ListDMBP));
    formData.append('A0035_ID', A0035_ID);
    formData.append('IsType', Istype);
    return this.http.post(this.baseUrl + "R4_DeletePBDMBP", formData);
  }

}
