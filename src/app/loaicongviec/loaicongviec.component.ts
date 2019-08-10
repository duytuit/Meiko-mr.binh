import { Component, OnInit } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-loaicongviec',
  templateUrl: './loaicongviec.component.html',
  styleUrls: ['./loaicongviec.component.css']
})
export class LoaicongviecComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListLCV = [];
  private lcv = {};
  private mod = { checkAll: false };
  constructor(private qtService: QuytrinhService, private dg: ModalService) { }
  mLCV: boolean;
  A0015_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListLCV();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListLCV();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListLCV();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListLCV();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListLCV();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListLCV();
  }

  getListLCV() {
    this.qtService.getLCV(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListLCV = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListLCV.length; index++) {
      this.ListLCV[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListLCV.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListLCV.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0015_ID = row["A0015_ID"];
    this.modalLCV("update");
  }

  modalLCV(isType: string) {
    if (isType == "add") {
      this.A0015_ID = null;
    }
    this.lcv = { STT: this.ListLCV.length + 1, trangThai: true };
    if (this.A0015_ID != null) {
      this.qtService.getLCVbyID(this.A0015_ID).subscribe((data) => {
        this.lcv = data[0];
      });
    }
    this.mLCV = true;
  }

  hideModal() {
    this.mLCV = false;
  }

  saveLCV() {
    this.qtService.updateLCV(this.lcv).subscribe(
      (then) => {
        this.hideModal();
        this.getListLCV();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListLCV.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0015_ID"]);
            }
            this.qtService.deleteLCV(listID).subscribe(
              (then) => {
                this.getListLCV();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
