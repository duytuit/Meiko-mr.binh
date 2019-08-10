import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { appPublic } from '../../appPublic';

@Injectable()
export class DuLieuQuetTheService {
  constructor(private http : HttpClient) { }  
  
  /*Lấy dữ liệu quẹt thẻ theo In_out,ca : Ngày or Đêm, Ngay*/
  GetDataCheckInByIOAndDate(IOCode, IsDay, date){    
    return this.http.get(`${appPublic.apiAsoft}/HT2408/`+ IOCode +'/'+ IsDay +'/'+date)         
  }
  /*Đồng bộ dữ liệu cơ cấu tổ chức, nhân sự từ asoft*/
  AsyncEmpFromAsoft(){    
    return this.http.get(`${appPublic.apiAsoft}/E00003/asyncdata`)
  }
  /*Đồng bộ và lấy dữ liệu check in IOCode: = 0 checkin; = 1 check out sang data mkvc Chi lấy dữ liệu checkin*/
  AsyncFromAsoft(date, calaviec){
    let IOCode = 0;  //Chi dong bo du lieu check in  
    return this.http.get(`${appPublic.apiAsoft}/HT2408/asyncdata/`+ IOCode +'/'+ calaviec +'/'+date)       
  }

  deleteDuLieuQuetThe(id: string){    
    return this.http.delete(`${appPublic.apiAsoft}/api/DulieuQuetThes/` + id)    
  }
}