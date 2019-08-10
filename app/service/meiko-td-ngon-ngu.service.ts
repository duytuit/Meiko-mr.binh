import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TreeNode } from '@angular/router/src/utils/tree';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})

export class MeikoTDNgonNguService {
  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Language/';

  getLang(pz: number, p: number, sort: string, ob: string, s: string, sts: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    return this.http.post(this.baseUrl + "R1_LanguageGetByList", formData);
  }

  getLangbyID(id: string) {
    return this.http.get(this.baseUrl + "R1_LanguageGetByID/" + id);
  }

  updateLang(formData: FormData, istype: number) {
    var urlAction = "R2_AddLanguage";
    if (istype == 2) {
      urlAction = "R3_UpdateLanguage";
    }
    return this.http.post(this.baseUrl + urlAction, formData);
  }

  deleteLang(id: Array<string>) {
    return this.http.post(this.baseUrl + "R4_DeleteLanguage", id);
  }
}
