import { Component, OnInit } from '@angular/core';
import { MeikoTDModuleService } from '../service/meiko-td-module.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private module = [];
  private mo = {};
  private mod = { checkAll: false };
  constructor(private moduleService: MeikoTDModuleService, private dg: ModalService) { }
  mModule: boolean;
  A0006_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListModule();
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListModule();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListModule();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListModule();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListModule();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListModule();
  }

  getListModule() {
    this.moduleService.getModule(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.module = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.module.length; index++) {
      this.module[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.module.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.module.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0006_ID = row["A0006_ID"];
    this.modalModule("update");
  }

  modalModule(isType: string) {
    if (isType == "add") {
      this.A0006_ID = null;
    }
    this.mo = { thuTu: this.module.length + 1, tinhTrang: true };
    if (this.A0006_ID != null) {
      this.moduleService.getModulebyID(this.A0006_ID).subscribe((data) => {
        this.mo = data[0];
      });
    }
    this.mModule = true;
  }

  hideModal() {
    this.mModule = false;
  }

  saveModule() {
    this.moduleService.updateModule(this.mo).subscribe(
      (then) => {
        this.hideModal();
        this.getListModule();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.module.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0006_ID"]);
            }
            this.moduleService.deleteModule(listID).subscribe(
              (then) => {
                this.getListModule();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
