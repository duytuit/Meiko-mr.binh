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
  selector: 'app-quytrinh',
  templateUrl: './quytrinh.component.html',
  styleUrls: ['./quytrinh.component.css']
})
export class QuytrinhComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListLCV = [];
  private ListCV = [];
  private QuyTrinh = [];
  private SteepKy = [];
  private ListPhongBan = [];
  private PhongBanList = [];
  private ListPhongBanSelect = [];
  private QuyTrinhPhongBan = [];
  private lcv = {};
  private mod = { checkAll: false };
  private rowGroupSignSelect = {};
  private TenPBSelect;
  private rowselect = {};
  private quytrinh = {};
  lengSSign = 0;
  constructor(private qtService: QuytrinhService, private dg: ModalService, private toastr: ToastrService) { }
  mConfigQuytrinh: boolean;
  A0015_ID: string;
  A0018_ID: string;
  A0020_ID: string;
  A0004_ID: string;
  checkLen: number;
  private mQT: boolean;
  Temp2 = [];
  arrfilter = [];
  ngOnInit() {
    this.getListLCV();
    this.getListCV();
    this.getStepsSign();
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
      //console.log(this.PhongBanList);
      this.getPhongBan();
    }, 500);
  }

  getPhongBan() {
    this.Temp2 = [];
    this.Temp = [];
    this.addToArray2(this.PhongBanList, null, 0);
    this.ListPhongBan = this.Temp2;

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

  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    if (filter.length > 0) {
      for (let index = 0; index < filter.length; index++) {
        var children = this.PhongBanList.filter(x => x.idcha == filter[index].id);
        var check = this.arrfilter.find(x => x.id == filter[index].id);
        if ((children.length > 0 || filter[index]["idcha"] == null)) {
          for (let i = 0; i < children.length; i++) {
            this.arrfilter.push(children[i]);
          }
          filter[index].children = children;
          if (check == null) {
            this.Temp2.push(filter[index]);
          }
          this.addToArray2(array, filter[index].id, lv);
        }
      }
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  getListLCV() {
    this.qtService.getLCV(100000, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListLCV = data["data"];
    });
  }

  getListCV() {
    this.qtService.getCV(100000, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.ListCV = data["data"];
    });
  }

  getStepsSign() {
    this.qtService.getstepsky().subscribe((data: Array<object>) => {
      this.SteepKy = data["data"];
    });
  }

  getQuyTrinh(row: object) {
    this.A0015_ID = row["A0015_ID"];
    var thanhvien;
    this.qtService.getQuyTrinhByLoaiCongViecID(row).subscribe((data: Array<object>) => {
      this.QuyTrinh = data["data"];
      this.QuyTrinh.forEach(item => {
        var SteepSign = item.SteepSign;
        SteepSign.forEach(ss => {
          if (ss.GroupSignSteep.length > 0) {
            var str = "";
            if (ss.GroupSignSteep[0]["thanhvien"].length > 0) {
              for (let i = 0; i < ss.GroupSignSteep[0]["thanhvien"].length; i++) {
                str += ss.GroupSignSteep[0]["thanhvien"][i] + ", ";
              }
              thanhvien = str.substring(0, str.length - 2);
            } else {
              thanhvien = "Chưa có thành viên nào trong nhóm ký";
            }
            ss.GroupSignSteep[0]["thanhvien"] = thanhvien;
            var A04ID = ss.GroupSignSteep[0].A0004_ID;
            var phongban = this.PhongBanList.find(x => x.id == A04ID);
            if (phongban != null) {
              ss.GroupSignSteep[0].tenphongban = phongban.bophan_ten;
            }
          }
        });
      });
    });
  }

  hideModal() {
    this.mConfigQuytrinh = false;
    this.mQT = false;
  }

  modalConfigQuyTrinh(quytrinh, row) {
    this.A0018_ID = quytrinh.A0018_ID;
    this.A0020_ID = row.A0020_ID;
    this.rowGroupSignSelect = {};
    this.mConfigQuytrinh = true;
    this.QuyTrinhPhongBan = [];
  }

  SelectModelQT(row) {
    this.QuyTrinhPhongBan = [];
    var obj = { A0004_ID: row.id };
    this.rowselect = row;
    this.TenPBSelect = row.bophan_ten;
    this.qtService.getQuyTrinhByPhongBanID(obj).subscribe((data: Array<object>) => {
      this.QuyTrinhPhongBan = data["data"];
      var thanhvien = "";
      this.QuyTrinhPhongBan.forEach(item => {
        var str = "";
        if (item["thanhvien"].length > 0) {
          for (let i = 0; i < item["thanhvien"].length; i++) {
            str += item["thanhvien"][i] + ", ";
          }
          thanhvien = str.substring(0, str.length - 2);
        } else {
          thanhvien = "Chưa có thành viên nào trong nhóm ký";
        }
        item["thanhvien"] = thanhvien;
      });
    });
  }

  SelectRowTable(r) {
    this.rowGroupSignSelect = {};
    this.QuyTrinhPhongBan.forEach(r => {
      r.select = false;
    });
    r.select = true;
    this.rowGroupSignSelect = r;
  }

  saveGroupSign(row) {
    var A0017_ID = this.rowGroupSignSelect["A0017_ID"];
    var obj = { A0015_ID: this.A0015_ID, A0004_ID: this.rowselect["id"], A0017_ID: A0017_ID, A0018_ID: this.A0018_ID, A0020_ID: this.A0020_ID };
    this.qtService.updateWorkFollowByCVID(obj).subscribe((then: Array<object>) => {
      if (then["error"] == 1) {
        this.dg.error("Thông báo", "Cấu hình quy trình ký không thành công !");
        return false;
      } else {
        var obj = { A0015_ID: this.A0015_ID };
        this.getQuyTrinh(obj);
        this.toastr.success('Cấu hình quy trình ký thành công !', 'Thông báo');
      }
    })
    this.mConfigQuytrinh = false;
  }

  deleteSignGroup(row) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.qtService.deleteWorkFollowByQuyTrinhID(row).subscribe((then: object) => {
            if (then["error"] == 0) {
              this.toastr.success("Xóa nhóm ký thành công !", "Thông báo");
              var obj = { A0015_ID: this.A0015_ID };
              this.getQuyTrinh(obj);
            } else {
              this.dg.error("Thông báo", "Xóa nhóm ký không thành công !");
              return false;
            }
          });
        }
      }).catch();
  }

  modalQuyTrinhCV(istype) {
    if (istype == "add") {
      this.quytrinh = { A0015_ID: this.A0015_ID, A0004_ID: null, trangThai: true };
    } else {      
      var obj = { A0018_ID: this.A0018_ID };
      this.qtService.getQuyTrinhbyID(obj).subscribe((data: Array<object>) => {
      this.quytrinh = data[0]; 
      });
    }
    this.mQT = true;
  }

  saveQuytrinhLCV() {
    this.qtService.updateQuyTrinh(this.quytrinh).subscribe(
      (then) => {
        this.hideModal();
        var obj = { A0015_ID: this.A0015_ID };
        this.getQuyTrinh(obj);
      }
    );
  }

  EditQuyTrinh(row:object)
  { 
    this.A0018_ID = row["A0018_ID"]; 
    this.modalQuyTrinhCV("update");
  }

}