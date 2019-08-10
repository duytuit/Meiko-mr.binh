import { Component, OnInit } from '@angular/core';
import { OplserviceService } from '../service/oplservice.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-kieubaiopl',
  templateUrl: './kieubaiopl.component.html',
  styleUrls: ['./kieubaiopl.component.css']
})
export class KieubaioplComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListKieuBaiOpl = [];
  private kbv = {};
  private mod = { checkAll: false };
  constructor(private oplservice: OplserviceService, private dg: ModalService) { }
  mKBVOPL: boolean;
  A0043_ID: string;
  checkLen: number;
  ngOnInit() {
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

  ListSearch() {
    this.opition.p = 1;
    this.getListKBV();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListKBV();
  }

  getListKBV() {
    this.oplservice.getKieuBaiViet(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListKieuBaiOpl = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListKieuBaiOpl.length; index++) {
      this.ListKieuBaiOpl[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListKieuBaiOpl.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListKieuBaiOpl.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0043_ID = row["A0043_ID"];
    this.modalKieuBaiOPL("update");
  }

  modalKieuBaiOPL(isType: string) {
    if (isType == "add") {
      this.A0043_ID = null;
    }
    this.kbv = { STT: this.ListKieuBaiOpl.length + 1, trangThai: true };
    if (this.A0043_ID != null) {
      this.oplservice.getKieuBaiVietGetByID(this.A0043_ID).subscribe((data) => {
        this.kbv = data[0];
      });
    }
    this.mKBVOPL = true;
  }

  hideModal() {
    this.mKBVOPL = false;
  }

  saveKBV() {
    this.oplservice.updateKieuBaiViet(this.kbv).subscribe(
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
          var listChecked = this.ListKieuBaiOpl.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0043_ID"]);
            }
            this.oplservice.deleteKieuBaiViet(listID).subscribe(
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
