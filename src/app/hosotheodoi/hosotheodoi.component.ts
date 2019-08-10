import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-hosotheodoi',
  templateUrl: './hosotheodoi.component.html',
  styleUrls: ['./hosotheodoi.component.css']
})
export class HosotheodoiComponent implements OnInit {
  private ListHSCV = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  constructor() { }

  ngOnInit() {
  }

  getListHSCV() {
    this.getListHSCV();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListHSCV();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListHSCV();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListHSCV();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }
}
