import { Component, OnInit } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class FormComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListForm = [];
  private form = {};
  private mod = { checkAll: false };
  constructor(private qtService: QuytrinhService, private dg: ModalService) { }
  mForm: boolean;
  A0032_ID: string;
  checkLen: number;

  ngOnInit() {
    this.getListForm();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListForm();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListForm();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListForm();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListForm();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListForm();
  }

  getListForm() {
    this.qtService.getListForm(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListForm = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListForm.length; index++) {
      this.ListForm[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListForm.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListForm.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0032_ID = row["A0032_ID"];
    this.modalForm("update");
  }

  modalForm(isType: string) {
    if (isType == "add") {
      this.A0032_ID = null;
    }
    this.form = { thuTu: this.ListForm.length + 1, trangThai: true };
    if (this.A0032_ID != null) {
      this.qtService.getFormbyID(this.A0032_ID).subscribe((data) => {
        this.form = data[0];
      });
    }
    this.mForm = true;
  }

  hideModal() {
    this.mForm = false;
  }

  saveForm() {
    this.qtService.updateForm(this.form).subscribe(
      (then) => {
        this.hideModal();
        this.getListForm();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListForm.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0032_ID"]);
            }
            this.qtService.deleteForm(listID).subscribe(
              (then) => {
                this.getListForm();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
