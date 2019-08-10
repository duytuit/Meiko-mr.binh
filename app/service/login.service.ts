import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { appPublic } from '../appPublic';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }
  baseUrl: string = appPublic.api_Admin + 'api/Login/';

  Login(user: object) {
    return this.http.post(this.baseUrl + "CheckLogin", user);
  }

  CheckMenu(A0001_ID: string, A0006_ID: string, A0002_ID: string) {
    const formData: FormData = new FormData();
    formData.append('A0001_ID', A0001_ID);
    formData.append('A0006_ID', A0006_ID);
    formData.append('A0002_ID', A0002_ID);
    return this.http.post(this.baseUrl + "CheckRoleMenu", formData);
  }

}
