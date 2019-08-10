import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { appPublic } from '../../appPublic';

@Injectable()
export class BuaAnService {

  
  constructor(private http : HttpClient) { }

  postBuaAn(buaan){
    return this.http.post(`${appPublic.apiAsoft}/api/BuaAns`, buaan);     
  }

  putBuaAn(id, buaan) {
    return this.http.put(`${appPublic.apiAsoft}/api/BuaAns/` + id, buaan);
  }
  getBuaAnList(){
    return this.http.get(`${appPublic.apiAsoft}/api/BuaAns`);        
  }  

  deleteBuaAn(id: string) {
    return this.http.delete(`${appPublic.apiAsoft}/api/BuaAns/` + id); 
  }
}