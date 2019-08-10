import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MeikoTDUserService } from '../service/meiko-td-user.service'
import {appPublic} from '../appPublic'
import { from } from 'rxjs';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  checkshowMenu = false;
  CMatkhau = {};
  linkHome:string = appPublic.linkHome;
  mModalPass: boolean;
  tk = {};
  constructor(private router: Router, private userService: MeikoTDUserService) { }

  ngOnInit() {
    // if(localStorage.getItem('login') !== null){
    //   this.checkshowMenu = true;
    // }
    this.tk = JSON.parse(localStorage.getItem("login"));
  }

  logOut() {
    localStorage.clear();
    this.router.navigate(["/login"]);
  }

  modalChangePass() {
    this.mModalPass = true;
  }

  ChangePass() {
    var token = JSON.parse(localStorage.getItem("login"));
    if(this.CMatkhau["mkcu"] == null || this.CMatkhau["mkcu"] == ""){
      alert("Bạn chưa nhập mật khẩu hiện tại");
      return false;
    }

    if(this.CMatkhau["mkmoi"] == null || this.CMatkhau["mkmoi"] == ""){
      alert("Bạn chưa nhập mật khẩu mới");
      return false;
    }

    if (this.CMatkhau["mkmoi"] != this.CMatkhau["mknhaplai"]) {
      alert("Nhập lại mật khẩu không chính xác");
    } else {
      this.CMatkhau["M0002_ID"] = token["u"]["M0002_ID"];
      this.CMatkhau["passWord"] = this.CMatkhau["mkmoi"];
      this.CMatkhau["passWordRandom"] = this.CMatkhau["mkcu"];
      this.userService.changePassword(this.CMatkhau).subscribe((then) =>{
        alert(then["ms"]);
        if(then["error"] == 0){
          this.hideModalPass();
        }
      });
    }
  }

  hideModalPass() {
    this.mModalPass = false;
  }

}
