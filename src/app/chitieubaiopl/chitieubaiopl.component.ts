import { Component, OnInit } from '@angular/core';
import { OplserviceService } from '../service/oplservice.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CompanyDiagramService } from '../service/company-diagram.service';
import { BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker'; 

@Component({
  selector: 'app-chitieubaiopl',
  templateUrl: './chitieubaiopl.component.html',
  styleUrls: ['./chitieubaiopl.component.css']
})
export class ChitieubaioplComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListChiTieuBaiOpl = [];
  private ctb = {};
  private mod = { checkAll: false, monthselect: new Date() };
  constructor(private cmpDiagramService: CompanyDiagramService, private oplservice: OplserviceService, private dg: ModalService) { }
  
  mCTB: boolean;
  A0044_ID: string;
  checkLen: number;
  minMode: BsDatepickerViewMode = 'month';
  bsConfig: Partial<BsDatepickerConfig>; 
  ngOnInit() {
    this.bsConfig = Object.assign({}, {
      minMode: this.minMode,
      dateInputFormat: 'MM-YYYY'
    });
    this.getListCompanyDiagram();
    this.getListKBV();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListKBV();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListKBV();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListKBV();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  Temp2 = [];
  ListCompageDiagram = [];
  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    filter = this.sortBycpn(filter, 'bophan_ten');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "---";
      }
      lv++;
      for (let i = 0; i < filter.length; i++) {
        filter[i].lv = lv;
        if (lv == 1) {
          filter[i].color = "green";
          filter[i].bold = "bold";
        } else if (lv == 2) {
          filter[i].color = "#fb3f00";
          filter[i].bold = "bold";
        } else if (lv == 3) {
          filter[i].color = "#023f98";
          filter[i].bold = "bold";
        } else if (lv == 4) {
          filter[i].color = "#d9534f";
          filter[i].bold = "300";
        } else {
          filter[i].color = "black";
          filter[i].bold = "300";
        }
        filter[i].close = true;
        filter[i].tenPhongBanMoi = sp + filter[i].bophan_ten;
        filter[i].imgages = [{ preview: filter[i].logo, full: filter[i].logo, width: 32, height: 32 }]
        this.Temp2.push(filter[i]);
        this.addToArray2(array, filter[i].id, lv);
      }
    }
  };

  sortBycpn(arr: any[], field: string) {
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

  getListCompanyDiagram() {
    this.Temp2 = [];
    this.cmpDiagramService.getCompanyDiagram().subscribe((data: Array<object>) => {
      var phongban = data.filter(x => x["muc"] != "A01" && x["id"] != "BPC0000");
      this.addToArray2(phongban, null, 0);
      this.ListCompageDiagram = this.Temp2;
    });
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListKBV();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListKBV();
  }

  onValueChange(value: Date): void {
    this.mod.monthselect = value;
    this.getListKBV();
  }

  getListKBV() {
    var month = this.mod.monthselect.getMonth() + 1;
    var year = this.mod.monthselect.getFullYear();
    this.oplservice.getChitieubai(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, month, year).subscribe((data: Array<object>) => {
      this.ListChiTieuBaiOpl = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListChiTieuBaiOpl.length; index++) {
      this.ListChiTieuBaiOpl[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListChiTieuBaiOpl.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListChiTieuBaiOpl.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0044_ID = row["A0044_ID"];
    this.modalCTB("update");
  }

  modalCTB(isType: string) {
    if (isType == "add") {
      this.A0044_ID = null;
    }
    this.ctb = { STT: this.ListChiTieuBaiOpl.length + 1, trangThai: true };
    if (this.A0044_ID != null) {
      this.oplservice.getChitieubaiGetByID(this.A0044_ID).subscribe((data) => {
        this.ctb = data[0];
      });
    }
    this.mCTB = true;
  }

  hideModal() {
    this.mCTB = false;
  }

  saveKBV() {    
    var month = this.mod.monthselect.getMonth() + 1;
    var year = this.mod.monthselect.getFullYear();
    var phongban = this.ListCompageDiagram.find(x => x.id == this.ctb["boPhan_ID"]);
    this.ctb["tenBoPhan"] = phongban["bophan_ten"];
    this.ctb["Parent_ID"] = phongban["idcha"];
    this.ctb["thang"] = month;
    this.ctb["nam"] = year;
    this.oplservice.updateChitieubai(this.ctb).subscribe(
      (then) => {
        this.hideModal();
        this.getListKBV();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListChiTieuBaiOpl.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0044_ID"]);
            }
            this.oplservice.deleteChitieubai(listID).subscribe(
              (then) => {
                this.getListKBV();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }

}
