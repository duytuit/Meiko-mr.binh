import { Component, OnInit } from '@angular/core';
import { appPublic } from '../appPublic';
import { Router } from '@angular/router';
@Component({
  selector: 'app-main-component',
  templateUrl: './main-component.component.html',
  styleUrls: ['./main-component.component.css']
})
export class MainComponentComponent implements OnInit {
  private ParentMenu = []
  constructor(private router: Router) { }

  ngOnInit() {
    this.ParentMenu = JSON.parse(localStorage.getItem("ParentMenu"));
  }

  setMain(row) {
    localStorage.setItem("ParentMain", JSON.stringify(row));
    var MenuList = JSON.parse(localStorage.getItem("MenuList"));
    var ParentChildMenu = MenuList.filter(x => x.IDCha == row["A0007_ID"]);
    if (ParentChildMenu != null) {
      var MenuParentId = ParentChildMenu[0]["A0007_ID"];
      var ChildMenu = MenuList.filter(x => x.IDCha == MenuParentId);
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
}
