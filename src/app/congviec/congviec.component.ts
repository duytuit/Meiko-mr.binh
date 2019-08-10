import { Component, OnInit } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { appPublic } from '../appPublic';
import { ToastrService } from 'ngx-toastr';

export interface TreeNode {
  label?: string;
  data?: any;
  icon?: any;
  expandedIcon?: any;
  collapsedIcon?: any;
  children?: TreeNode[];
  leaf?: boolean;
  expanded?: boolean;
  type?: string;
  parent?: TreeNode;
  partialSelected?: boolean;
  styleClass?: string;
  draggable?: boolean;
  droppable?: boolean;
  selectable?: boolean;
}

@Component({
  selector: 'app-congviec',
  templateUrl: './congviec.component.html',
  styleUrls: ['./congviec.component.css']
})
export class CongviecComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListCV = [];
  private ListLCV = [];
  private ListPhongBan = [];
  private PhongBanList = [];
  private ListCompageDiagram = [];
  private ListNhomKy = [];
  private ListNguoiKy = [];
  private FormSelect = [];
  private ListLoaiCongViec = [
    { 'loaiCongViec': 0, 'tenLoaiCongViec': 'Công việc quy trình cùng phòng ban' },
    { 'loaiCongViec': 1, 'tenLoaiCongViec': 'Công việc có đích người xử lý' },
    { 'loaiCongViec': 2, 'tenLoaiCongViec': 'Công việc liên phòng ban chọn người xử lý' }
  ];
  private cv = {};
  private mod = { checkAll: false };
  private gsConfig = {};
  constructor(private qtService: QuytrinhService, private dg: ModalService, private toastr: ToastrService) { }
  mCV: boolean;
  A0016_ID: string;
  checkLen: number;
  mSignGroupConfig: boolean;
  ngOnInit() {
    this.getListLCV();
    this.getListCV();
    this.getListForm();
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
      this.getPhongBan();
    }, 500);
  }

  getPhongBan() {
    this.Temp = [];
    this.addToArray(this.PhongBanList, null, 0);
    this.ListCompageDiagram = this.Temp;
  }

  getListForm() {
    this.qtService.getListFormSelect().subscribe((then: Array<object>) => {
      this.FormSelect = then["data"];
    });
  }

  Temp = [];
  addToArray(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    filter = this.sortBy(filter, 'bophan_ten');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "-----";
      }
      lv++;
      for (let i = 0; i < filter.length; i++) {
        filter[i].lv = lv;
        filter[i].close = true;
        filter[i].tenPhongBanMoi = sp + filter[i].bophan_ten;
        this.Temp.push(filter[i]);
        this.addToArray(array, filter[i].id, lv);
      }
    }
  };

  sortBy(arr: any[], field: string) {
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

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListCV();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListCV();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListCV();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListCV();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListCV();
  }

  getListCV() {
    this.qtService.getCV(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListCV = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  getListLCV() {
    this.qtService.getLCVSelect().subscribe((data: Array<object>) => {
      this.ListLCV = data["data"];
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListCV.length; index++) {
      this.ListCV[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListCV.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListCV.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0016_ID = row["A0016_ID"];
    this.modalCV("update");
  }

  modalCV(isType: string) {
    if (isType == "add") {
      this.A0016_ID = null;
    }
    this.cv = { A0015_ID: null, A0004_ID: null, A0032_ID: null, STT: this.ListCV.length + 1, loaiCongViec: 0, trangThai: true };
    if (this.A0016_ID != null) {
      this.qtService.getCVbyID(this.A0016_ID).subscribe((data) => {
        this.cv = data[0];
      });
    }
    this.mCV = true;
  }

  hideModal() {
    this.mCV = false;
    this.mSignGroupConfig = false;
  }

  saveCV() {
    this.qtService.updateCV(this.cv).subscribe(
      (then) => {
        this.hideModal();
        this.getListCV();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListCV.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0016_ID"]);
            }
            this.qtService.deleteLCV(listID).subscribe(
              (then) => {
                this.getListCV();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }

  modalConfigGroupSign(row: object) {
    this.ListNhomKy = [];
    this.ListNguoiKy = [];
    this.A0016_ID = row["A0016_ID"];
    this.qtService.getGroupSinConfig(row).subscribe((data) => {
      this.gsConfig = data[0];
      if (this.gsConfig == null) {
        this.gsConfig = { A0004_ID: null, A0021_ID: null, A0019_ID: null, A0016_ID: this.A0016_ID };
      } else {
        this.changePhongBan(this.gsConfig["A0004_ID"]);
      }
    });
    this.mSignGroupConfig = true;
  }

  saveSetGroupSignCV() {
    this.qtService.saveCVConfigGroupSign(this.gsConfig).subscribe(
      (then) => {
        if (then["error"] == 1) {
          this.toastr.error("Lỗi khi cấu hình người ký cho công việc", "Thông báo");
        } else {
          this.hideModal();
          this.getListCV();
          this.toastr.success("Cấu hình người ký cho công việc thành công", "Thông báo");
        }
      }
    );
  }

  changePhongBan(rowID: string) {
    this.ListNhomKy = [];
    this.ListNguoiKy = [];
    this.cvSelect["A0016_ID"] = this.A0016_ID;
    this.cvSelect["A0004_ID"] = rowID;
    this.getGroupSignByCVID();
  }

  private cvSelect = {};
  getGroupSignByCVID() {
    this.qtService.getWorkFollowCV(this.cvSelect).subscribe((data: Array<object>) => {
      var data = data["data"];
      if (data.length > 0) {
        this.ListNhomKy = data[0]["GroupSign"];
        if (this.ListNhomKy.length <= 0) {
          this.dg.error("Thông báo", "Hiện tại phòng ban gửi tới này chưa có quy trình, liên hệ với Admin để cấu hình quy trình !");
        } else {
          if (this.gsConfig["A0019_ID"] != null) {
            this.ChangeGroupSign();
          }
        }
      } else {
        this.dg.error("Thông báo", "Hiện tại phòng ban gửi tới này chưa có quy trình, liên hệ với Admin để cấu hình quy trình !");
      }
    });
  }

  ChangeGroupSign() {
    var rowGroupSign = this.ListNhomKy.find(x => x.A0021_ID == this.gsConfig["A0021_ID"]);
    this.qtService.getUserbyGroupSign(rowGroupSign).subscribe((then: Array<object>) => {
      this.ListNguoiKy = then["data"];
    });
  }

}
