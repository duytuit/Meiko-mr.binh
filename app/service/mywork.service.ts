import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class MyworkService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Work/';

  //#region  MyWorkDocument

  MyWorkdocument(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_MyWorkDocument", formData);
  }

  MyWaitingForSign(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_WaitingForSignDocument", formData);
  }

  SenddingDocument(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_SenddingDocument", formData);
  }

  CompletedDocument(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_CompletedDocument", formData);
  }

  TrashDocument(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_TrashDocument", formData);
  }

  PassDocumment(pz: number, p: number, sort: string, ob: string, s: string, sts: string, userID: string) {
    const formData: FormData = new FormData();
    formData.append('pz', pz.toString());
    formData.append('p', p.toString());
    formData.append('sort', sort);
    formData.append('ob', ob);
    formData.append('s', s);
    formData.append('sts', sts);
    formData.append('A0002_ID', userID);
    return this.http.post(this.baseUrl + "R1_PassDocument", formData);
  }

  WorkDocumentDetail(row: object) {
    return this.http.get(this.baseUrl + "R1_MyWorkDocumentDetail/" + row["A0028_ID"]);
  }

  updateWorkDocument(type: number, formData: FormData) {
    var UrlAction = "R2_AddWorkDocument";
    if (type == 2) {
      UrlAction = "R3_UpdateWorkdocument";
    }
    return this.http.post(this.baseUrl + UrlAction, formData);
  }

  EventNote(row: object) {
    return this.http.get(this.baseUrl + "R1_EventNote/" + row["A0028_ID"]);
  }

  SendNextDocument(row: object, userID: string,noiDung:string) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0002_ID', userID);
    formData.append('Note', noiDung);
    return this.http.post(this.baseUrl + "R2_SignAndSend", formData);
  }

  RejectDocument(row: object, userID: string,noiDung:string) {
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0002_ID', userID);
    formData.append('Note', noiDung);
    return this.http.post(this.baseUrl + "R2_RejectDocument", formData);
  } 

  DeleteFileDocument(row:object){
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]);
    formData.append('A0031_ID', row["A0031_ID"]);
    return this.http.post(this.baseUrl + "R4_DeleteFileAttach", formData);
  }

  DeleteDocument(row:object){
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]); 
    return this.http.post(this.baseUrl + "R4_DeleteDoccument", formData);
  }
  
  DeleteDocumentTrash(row:object){
    const formData: FormData = new FormData();
    formData.append('A0028_ID', row["A0028_ID"]); 
    return this.http.post(this.baseUrl + "R4_DeleteDoccumentTrash", formData);
  }

  CheckWorkFollowForm(row:object){
    const formData: FormData = new FormData();
    formData.append('A0032_ID', row["A0032_ID"]); 
    return this.http.post(this.baseUrl + "R1_WorkFollowForm", formData);
  }

  //#endregion
}
