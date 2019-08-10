import { Component, OnInit, ViewChild } from '@angular/core';
import { BaopheService } from '../service/baophe.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { appPublic } from '../appPublic';
import { UserListComponent } from '../user-list/user-list.component';

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
  selector: 'app-baophequytrinh',
  templateUrl: './baophequytrinh.component.html',
  styleUrls: ['./baophequytrinh.component.css']
})
export class BaophequytrinhComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListQuyTrinhBaoPhe = [];
  private quytrinh = {};
  private mod = { checkAll: false };
  constructor(private baoPheService: BaopheService, private dg: ModalService) { }
  mNK: boolean;
  A0017_ID: string;
  checkLen: number;
  mUserSelect: boolean;
  mEmail: boolean;
  private ListPhongBan = [];
  private PhongBanList = [];
  private ListPhongBanSelect = [];

  @ViewChild(UserListComponent) child: UserListComponent;
  ngOnInit() {
    this.getListQuyTrinh();
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
      this.getPhongBan();
    }, 500);
  }

  getPhongBan() {
    this.Temp = [];
    this.addToArray(this.PhongBanList, null, 0);
    this.ListPhongBanSelect = this.Temp;
  }

  Temp = [];
  addToArray(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    filter = this.sortBy(filter, 'bophan_ten');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "---";
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

  rowselect = {};
  isTypeUser: number;
  ModalConfig(row, isType) {
    this.rowselect = row;
    this.isTypeUser = isType;
    this.child.mUserSelect = true;
  }

  ChoiceSelectUser(listUser: Array<string>) {
    if (listUser.length > 0) {
      if (this.isTypeUser == 1) {
        this.rowselect["nguoiKy1"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 2) {
        this.rowselect["nguoiKy2"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 3) {
        this.rowselect["nguoiKy3"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 4) {
        this.rowselect["nguoiKy4"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 5) {
        this.rowselect["nguoiKy5"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 6) {
        this.rowselect["nguoiKy6"] = listUser[0]["A0002_ID"];
      } else if (this.isTypeUser == 7) {
        this.rowselect["nguoiKy7"] = listUser[0]["A0002_ID"];
      }
    }

    this.baoPheService.updateQuyTrinhBaoPhe(this.rowselect, this.isTypeUser).subscribe(
      (then) => {
        this.child.mUserSelect = false;
        this.getListQuyTrinh();
      }
    );

  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListQuyTrinh();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListQuyTrinh();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListQuyTrinh();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListQuyTrinh();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListQuyTrinh();
  }

  getListQuyTrinh() {
    this.baoPheService.GetQuyTrinhBaoPhe().subscribe((data: Array<object>) => {
      this.ListQuyTrinhBaoPhe = JSON.parse(data["data"]);
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListQuyTrinhBaoPhe.length; index++) {
      this.ListQuyTrinhBaoPhe[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListQuyTrinhBaoPhe.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListQuyTrinhBaoPhe.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0017_ID = row["A0017_ID"];
    //this.modalNhomKy("update");
  }

  // modalNhomKy(isType: string) {
  //   if (isType == "add") {
  //     this.A0017_ID = null;
  //   }
  //   this.nhomky = { STT: this.ListQuyTrinh.length + 1, trangThai: true };
  //   if (this.A0017_ID != null) {
  //     this.baoPheService.getNhomKybyID(this.A0017_ID).subscribe((data) => {
  //       this.nhomky = data[0];
  //     });
  //   }
  //   this.mNK = true;
  // }

  hideModal() {
    this.mNK = false;
    this.mEmail = false;
  }

  saveNhomNguoiKy() {
    this.baoPheService.updateQuyTrinhBaoPhe(this.quytrinh, this.isTypeEmail).subscribe(
      (then) => {
        this.hideModal();
        this.getListQuyTrinh();
      }
    );
  }

  UserSign = {};
  rowEmailselect = {};
  isTypeEmail: number;
  ConfigEmail(row, isType) {
    this.quytrinh = row;
    this.isTypeEmail = isType;
    if (isType == 1) {
      this.UserSign = { Email: row["MailNguoiKy1"] };
    } else if (isType == 2) {
      this.UserSign = { Email: row["MailNguoiKy2"] };
    } else if (isType == 3) {
      this.UserSign = { Email: row["MailNguoiKy3"] };
    } else if (isType == 4) {
      this.UserSign = { Email: row["MailNguoiKy4"] };
    } else if (isType == 5) {
      this.UserSign = { Email: row["MailNguoiKy5"] };
    } else if (isType == 6) {
      this.UserSign = { Email: row["MailNguoiKy6"] };
    } else if (isType == 7) {
      this.UserSign = { Email: row["MailNguoiKy7"] };
    }
    this.mEmail = true;
  }

  UpdateMail() {
    if (this.isTypeEmail == 1) {
      this.quytrinh["MailNguoiKy1"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 2) {
      this.quytrinh["MailNguoiKy2"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 3) {
      this.quytrinh["MailNguoiKy3"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 4) {
      this.quytrinh["MailNguoiKy4"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 5) {
      this.quytrinh["MailNguoiKy5"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 6) {
      this.quytrinh["MailNguoiKy6"] = this.UserSign["Email"];
    } else if (this.isTypeEmail == 7) {
      this.quytrinh["MailNguoiKy7"] = this.UserSign["Email"];
    }
    this.saveNhomNguoiKy();
  }

  DeleteRowBP(row: object, isType) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa người ký hay không ?')
      .then((confirmed) => {
        if (confirmed) {
          if (isType == 1) {
            row["nguoiKy1"] = null;
          } else if (isType == 2) {
            row["nguoiKy2"] = null;
          } else if (isType == 3) {
            row["nguoiKy3"] = null;
          } else if (isType == 4) {
            row["nguoiKy4"] = null;
          } else if (isType == 5) {
            row["nguoiKy5"] = null;
          } else if (isType == 6) {
            row["nguoiKy6"] = null;
          } else if (isType == 7) {
            row["nguoiKy7"] = null;
          }
          this.baoPheService.updateQuyTrinhBaoPhe(row, isType).subscribe(
            (then) => {
              this.getListQuyTrinh();
            }
          );
        }
      });
  }
}
