import { Component, OnInit, Output } from '@angular/core';
import {Router } from '@angular/router';
import { MeikoTDUserService } from './service/meiko-td-user.service';
import { appPublic } from './appPublic';
import { from } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'MeikoWebsite';
  checkshowMenu = false;
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  constructor(private uService: MeikoTDUserService,private router: Router) {
  }
  ngOnInit() {
    if(localStorage.getItem('login') == null || localStorage.getItem('login') == undefined){
      this.router.navigate(["/login"]);
    }

    if (localStorage.getItem('login') != null) {
      this.checkshowMenu = true;
    }
    this.userList();
  }

  Temp = [];
  addToArray(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    filter = this.sortBy(filter, 'bophan_ten');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "-----";
      }
      lv++;
      for (let i = 0; i < filter.length; i++) {
        filter[i].lv = lv;
        filter[i].close = true;
        filter[i].tenPhongBanMoi = sp + filter[i].bophan_ten;
        this.Temp.push(filter[i]);
        this.addToArray(array, filter[i].id, lv);
      }
    }
  };

  sortBy(arr: any[], field: string) {
    arr.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return -1;
      } else if (a[field] > b[field]) {
        return 1;
      } else {
        return 0;
      }
    });
    return arr;
  }

  public userList() {
    this.Temp = [];
    this.uService.BindListPhongBan().subscribe((then: Array<object>) => {
      var phongban = then.filter(x => x["muc"] != "A01" && x["id"] != "BPC0000");
      this.uService.BindListUser().subscribe((data: Array<object>) => {
        var user = data["users"];
        var role = data["cds"];
        var obj = { id: "Null", idcha: null, bophan_ten: 'Không xác định' };
        phongban.push(obj);
        this.addToArray(phongban, null, 0);
        for (let i = 0; i < this.Temp.length; i++) {
          if (this.Temp[i].id != "Null") {
            var filter = user.filter(x => x.A0004_ID == this.Temp[i].id);
            filter = this.sortBy(filter, 'FullName');
            this.Temp[i].users = filter;
          } else {
            var filter = user.filter(x => x.A0004_ID == null || x.A0004_ID == "");
            filter = this.sortBy(filter, 'FullName');
            this.Temp[i].users = filter;
          }
        }
        appPublic.listUser = this.Temp;
        appPublic.listChucDanh = role;
        appPublic.listPhongBan = phongban;
      });
    });
  }

}
