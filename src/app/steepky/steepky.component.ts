import { Component, OnInit } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-steepky',
  templateUrl: './steepky.component.html',
  styleUrls: ['./steepky.component.css']
})
export class stepskyComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private Liststepsky = [];
  private stepsky = {};
  private mod = { checkAll: false };
  constructor(private qtService: QuytrinhService, private dg: ModalService) { }
  mSK: boolean;
  A0020_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListstepsky();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListstepsky();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListstepsky();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListstepsky();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListstepsky();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListstepsky();
  }

  getListstepsky() {
    this.qtService.getstepsky().subscribe((data: Array<object>) => {
      this.Liststepsky = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.Liststepsky.length; index++) {
      this.Liststepsky[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.Liststepsky.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.Liststepsky.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0020_ID = row["A0020_ID"];
    this.modalstepsky("update");
  }

  modalstepsky(isType: string) {
    if (isType == "add") {
      this.A0020_ID = null;
    }
    this.stepsky = { STT: this.Liststepsky.length + 1, trangThai: true };
    if (this.A0020_ID != null) {
      this.qtService.getstepsbyID(this.A0020_ID).subscribe((data) => {
        this.stepsky = data[0];
      });
    }
    this.mSK = true;
  }

  hideModal() {
    this.mSK = false;
  }

  savestepsKy() {
    this.qtService.updatestepsky(this.stepsky).subscribe(
      (then) => {
        this.hideModal();
        this.getListstepsky();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.Liststepsky.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0020_ID"]);
            }
            this.qtService.deletestepsky(listID).subscribe(
              (then) => {
                this.getListstepsky();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
