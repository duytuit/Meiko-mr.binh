import { Component, OnInit } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nhomky',
  templateUrl: './nhomky.component.html',
  styleUrls: ['./nhomky.component.css']
})
export class NhomkyComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListNhomKy = [];
  private ListNhomKyWorkfollow = [];
  private nhomky = {};
  private mod = { checkAll: false };
  constructor(private qtService: QuytrinhService, private dg: ModalService) { }
  mNK: boolean;
  A0017_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListNhomKy();
    this.getListNhomKyByWorkFollow();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListNhomKy();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListNhomKy();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListNhomKy();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListNhomKy();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListNhomKy();
  }

  getListNhomKy() {
    this.qtService.getNhomKy(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListNhomKy = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  getListNhomKyByWorkFollow(){
    this.qtService.getQuyTrinhByGroupSign().subscribe((data: Array<object>) => {
      this.ListNhomKyWorkfollow = data["data"];  
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListNhomKy.length; index++) {
      this.ListNhomKy[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListNhomKy.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListNhomKy.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0017_ID = row["A0017_ID"];
    this.modalNhomKy("update");
  }

  modalNhomKy(isType: string) {
    if (isType == "add") {
      this.A0017_ID = null;
    }
    this.nhomky = { STT: this.ListNhomKy.length + 1, trangThai: true };
    if (this.A0017_ID != null) {
      this.qtService.getNhomKybyID(this.A0017_ID).subscribe((data) => {
        this.nhomky = data[0];
      });
    }
    this.mNK = true;
  }

  hideModal() {
    this.mNK = false;
  }

  saveNhomNguoiKy() {
    this.qtService.updateNhomKy(this.nhomky).subscribe(
      (then) => {
        this.hideModal();
        this.getListNhomKy();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListNhomKy.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0017_ID"]);
            }
            this.qtService.deleteNhomKy(listID).subscribe(
              (then) => {
                this.getListNhomKy();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
