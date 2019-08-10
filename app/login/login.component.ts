import { Component, OnInit } from '@angular/core';
import { LoginService } from '../service/login.service';
import { Router } from '@angular/router';
import { appPublic } from '../appPublic';
import { ModalService } from '../ui/modal.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user = {};
  err = {};
  constructor(private loginservice: LoginService, private router: Router, private dg: ModalService) { }

  ngOnInit() {
    if (localStorage.getItem('login') !== null) {
      this.router.navigate(["/roles"]);
    }
  }
  Login() {
    if (this.user["userName"] != null && this.user["passWord"] != null) {
      this.loginservice.Login(this.user).subscribe((then) => {
        if (then["error"] == 0) {
          var data = then;
          //console.log(data);
          localStorage.setItem('login', JSON.stringify(data["u"]));
          var RoleID = "";
          var ModuleID = "";
          if (data["Roles"] != null && data["Roles"].length > 0) {
            RoleID = data["Roles"][0].RoleID;
          }
          localStorage.setItem("ParentMenu", JSON.stringify(data["parentsmenu"]));
          localStorage.setItem("ChildsMenu", JSON.stringify(data["childsmenu"]));
          localStorage.setItem("MenuList", JSON.stringify(data["menu"]));
          this.router.navigate(["/main"]);
        } else {
          this.dg.error("Thông báo", "Đăng nhập không thành công !");
        }
      });
    }
  }
}
