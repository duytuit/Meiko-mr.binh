import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { appPublic } from '../appPublic';
@Component({
  selector: 'app-admins',
  templateUrl: './admins.component.html',
  styleUrls: ['./admins.component.css']
})
export class AdminsComponent implements OnInit {
  private menuMain = {};
  private MenuList = [];
  private ParentMenu = []
  private Counts = {}; 
  HideMenu = false;
  constructor(private router: Router) { }

  ngOnInit() { 
    this.ParentMenu = JSON.parse(localStorage.getItem("ParentMenu"));
    this.menuMain = JSON.parse(localStorage.getItem("ParentMain"));
    this.MenuList = JSON.parse(localStorage.getItem("MenuList"));
    this.MenuList.forEach(item => {
      item.count = 'c01';
    });
    var obj = { 'c01':1, 'c02': 0, 'c03': 0, 'c04': 0, 'c05': 0, 'c06': 0, 'c07': 0 }
    this.Counts = obj;
    console.log(this.HideMenu);
  } 

  SetUrl(row:object){
    this.MenuList.forEach(item => {
      item.isActive = false;
    });
    row["isActive"] = true;
  }

  setMain(row) { 
    this.ParentMenu.forEach(item => {
      item.isActive = false;
    });
    row["isActive"] = true;

    this.menuMain = row; 
    var ParentChildMenu = this.MenuList.filter(x => x.IDCha == row["A0007_ID"]);
    if (ParentChildMenu != null) {
      var MenuParentId = ParentChildMenu[0]["A0007_ID"];
      var ChildMenu = this.MenuList.filter(x => x.IDCha == MenuParentId); 
      if (ChildMenu != null) {
        var LinkMenu = ChildMenu[0]["Link"];
        this.router.navigate([LinkMenu]);
      } else {
        this.router.navigate(["/admin"]);
      }
    } else {
      this.router.navigate(["/admin"]);
    }
  }

  ToogleMenu(){
    this.HideMenu = !this.HideMenu;
  }
  
}
