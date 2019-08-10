import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class WorkCAMService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Baophe/';

  //#region  MyWorkDocument

  MyWorkdocumentCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_MyWorkDocumentCam", formData);
  }

  MyWaitingForSignCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_WaitingForSignDocumentCam", formData);
  }

  MyWaitingConFirmBaoPhe(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_WaitingConfirmDocument", formData);
  }

  WaitingProcessDocumentCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_WaitingProcessDocumentCam", formData);
  }

  SenddingDocumentCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_SenddingDocumentCam", formData);
  }

  CompletedDocumentCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_CompletedDocumentCam", formData);
  }

  CompletedDocumentConfirm(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_CompletedDocumentConfirm", formData);
  }

  TrashDocumentCam(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_TrashDocumentCam", formData);
  }

  WorkDocumentDetailCam(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0034_ID', row["A0034_ID"]);
    formData.append('A0037_ID', row["A0037_ID"]);
    return this.http.post(this.baseUrl + "R1_MyWorkDocumentDetail/", formData);
  }

  WorkDocumentDetailCamConfirm(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0037_ID', row["A0037_ID"]);
    formData.append('A0034_ID', row["A0034_ID"]);
    return this.http.post(this.baseUrl + "R1_MyWorkDocumentDetailConfirm", formData);
  }

  updateWorkDocumentCam(type: number, formData: FormData) {
    var UrlAction = "R2_AddWorkDocumentCam";
    if (type == 2) {
      UrlAction = "R3_UpdateWorkDocumentCam";
    }
    return this.http.post(this.baseUrl + UrlAction, formData);
  }

  DeleteFileDocumentCam(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0031_ID', row["A0031_ID"]);
    return this.http.post(this.baseUrl + "R4_DeleteFileAttach", formData);
  }

  DeleteDocumentCam(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    return this.http.post(this.baseUrl + "R4_DeleteDoccumentCam", formData);
  }

  SendNextDocument(row: object, userID: string, noiDung: string) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0037_ID', row["A0037_ID"]);
    formData.append('A0002_ID', userID);
    formData.append('Note', noiDung);
    return this.http.post(this.baseUrl + "R2_SignAndSendBPConfirm", formData);
  }

  EventNote(row: object) {
    return this.http.get(this.baseUrl + "R1_EventNoteCam/" + row["A0037_ID"]);
  }

  updateWorkDocumentCamConfirm(formData: FormData) {
    var UrlAction = "R3_UpdateWorkdocumentCamConfirm";
    return this.http.post(this.baseUrl + UrlAction, formData);
  }

  ExportExcel(row: object) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0037_ID', row["A0037_ID"]);
    formData.append('A0034_ID', row["A0034_ID"]);
    return this.http.post(this.baseUrl + "ExportKetQua", formData);
  }

}
