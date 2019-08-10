import { Component, OnInit } from '@angular/core';
import { MyworkService } from '../service/mywork.service';
import { BaopheService } from '../service/baophe.service';
import { NgSelectModule, NgOption } from '@ng-select/ng-select';
import { ToastrService } from 'ngx-toastr';
import { appPublic } from '../appPublic';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from '../ui/modal.service';

@Component({
  selector: 'app-noidungbaophe',
  templateUrl: './noidungbaophe.component.html',
  styleUrls: ['./noidungbaophe.component.css']
})
export class NoidungbaopheComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListNDBaoPhe = [];
  private noidungbaophe = {};
  private A0035_ID: string;
  mNDBP: boolean;
  private mod = { checkAll: false };
  constructor(private baopheService: BaopheService, private workService: MyworkService, private toastr: ToastrService, private dg: ModalService) { }

  ngOnInit() {
    this.getListNDBaoPhe();
    this.getListSelect();
  }
  getListNDBaoPhe() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.baopheService.GetListNDBaoPhe(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListNDBaoPhe = data["data"];
      this.opition.total = data["count"];
      this.SetTotalPage();
    });
  }

  getListSelect() {
    this.baopheService.GetAllBoPhanMapper().subscribe(
      (then) => {
        this.listPhongBan = then["data"];
      }
    );
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListNDBaoPhe();
  }

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListNDBaoPhe();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListNDBaoPhe();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
    this.getListNDBaoPhe();
    this.toastr.success('Đang tải lại...', 'Làm mới');
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListNDBaoPhe();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListNDBaoPhe();
  }

  checkLen: number;
  toggleAll() {
    for (let index = 0; index < this.ListNDBaoPhe.length; index++) {
      this.ListNDBaoPhe[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListNDBaoPhe.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListNDBaoPhe.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0035_ID = row["A0035_ID"];
    this.modalNoidungbaophe("update");
  }

  modalNoidungbaophe(isType: string) {
    if (isType == "add") {
      this.A0035_ID = null;
    }
    this.noidungbaophe = { STT: this.ListNDBaoPhe.length + 1, trangThai: true };
    if (this.A0035_ID != null) {
      var obj = {};
      obj["A0035_ID"] = this.A0035_ID;
      this.baopheService.GetListNDBaoPheByID(obj).subscribe((data) => {
        this.noidungbaophe = data[0];
      });
    }
    this.mNDBP = true;
  }

  mPhongBan: boolean;
  listPhongBan = [];
  listPhongBanSelect = [];
  rowDMBPSelect = {};
  ModalCompanyBP(obj: object) {
    this.rowDMBPSelect = obj;
    this.baopheService.getPhongBanByDMBaoPheID(obj).subscribe(
      (then:Array<object>) => {
        this.listPhongBanSelect = then; 
      }
    );
    this.mPhongBan = true;
  }

  hideModal() {
    this.mNDBP = false;
    this.mPhongBan = false;
  }

  saveDanhMucBaoPhe() {
    this.baopheService.updateNDBaoPhe(this.noidungbaophe).subscribe(
      (then) => {
        this.hideModal();
        this.getListNDBaoPhe();
      }
    );
  }

  ChoisePhongBanDanhMuc() {
    var ListPBChoise = this.listPhongBan.filter(x => x.checked == true);
    this.baopheService.updatePhongBanToNDBaoPhe(this.rowDMBPSelect, ListPBChoise).subscribe(
      (then) => {
        this.ModalCompanyBP(this.rowDMBPSelect);
        this.getListNDBaoPhe();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListNDBaoPhe.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0035_ID"]);
            }
            this.baopheService.deleteNDBaoPhe(listID).subscribe(
              (then) => {
                this.getListNDBaoPhe();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }

  RefershDMBP() {
    this.dmBPSelect = [];
    this.ModalCompanyBP(this.rowDMBPSelect);
  }

  dmBPSelect = [];
  CheckDMBP() {
    this.dmBPSelect = this.listPhongBanSelect.filter(x => x.checked == true);
  }

  DeleteDMBP() { 
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
            var ListDMBP = [];
            this.dmBPSelect.forEach(item => {
              ListDMBP.push(item["A0034_ID"]);
            });
            var A0035_ID = this.rowDMBPSelect["A0035_ID"];
            this.baopheService.DeletePhongBanDMBP(ListDMBP, A0035_ID, 1).subscribe((data: Array<object>) => {
            this.toastr.success("Xóa bộ phận xử lý danh mục báo phế thành công", "Thông báo");
            this.ModalCompanyBP(this.rowDMBPSelect);
          });
        }
      }); 
  }
}
