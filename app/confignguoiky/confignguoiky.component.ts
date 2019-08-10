import { Component, OnInit, ViewChild } from '@angular/core';
import { QuytrinhService } from '../service/quytrinh.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserListComponent } from '../user-list/user-list.component';
import { appPublic } from '../appPublic';
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
  selector: 'app-confignguoiky',
  templateUrl: './confignguoiky.component.html',
  styleUrls: ['./confignguoiky.component.css']
})

export class ConfignguoikyComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListPhongBan = [];
  private PhongBanList = [];
  private ListGroupSignWorkfollow = [];
  private ListNguoiKy = [];
  private nhomky = {};
  private mod = { checkAll: false };
  private rowselect = {};
  constructor(private qtService: QuytrinhService, private dg: ModalService) { }
  mUserGroup: boolean;
  A0004_ID: string;
  A0017_ID: string;
  grouptitle: string;
  checkLen: number;
  Temp2 = [];
  files: TreeNode[];
  arrfilter = [];
  @ViewChild(UserListComponent) child: UserListComponent;
  ngOnInit() {
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
      this.getPhongBan();
    }, 500);
  }

  getPhongBan() {
    this.Temp2 = [];
    this.addToArray2(this.PhongBanList, null, 0);
    this.ListPhongBan = this.Temp2;
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

  Refersh() {
    this.getPhongBan();
  }

  SelectModel(row) {
    this.A0004_ID = row.id;
    this.getListNhomKy();
  }

  getListNhomKy() {
    var obj = { id: this.A0004_ID };
    var thanhvien;
    this.qtService.getGroupSignByPhongBanID(obj).subscribe((data: Array<object>) => {
      this.ListGroupSignWorkfollow = data["data"];
      this.ListGroupSignWorkfollow.forEach(item => {
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

  modalNhomKy(row: object) {
    this.rowselect = row;
    this.A0017_ID = row["A0017_ID"];
    this.grouptitle = row["tenNhomNguoiKy"];
    var obj = { A0004_ID: this.A0004_ID, A0017_ID: this.A0017_ID };
    this.qtService.getUserByGroupSignIDPhongBanID(obj).subscribe((data: Array<object>) => {
      this.ListNguoiKy = data["data"];
    });
    this.mUserGroup = true;
  }

  hideModal() {
    this.mUserGroup = false;
  }

  showusersModal() {
    this.child.mUserSelect = true;
  }

  ChoiceSelectUser(listUser: Array<string>) {
    var UserList = [];
    listUser.forEach(item => {
      UserList.push(item["A0002_ID"]);
    }); 

    var obj = { A0004_ID: this.A0004_ID, A0017_ID: this.A0017_ID };
    this.qtService.AddUserToGroupSignIDPhongBanID(obj, UserList).subscribe((data: Array<object>) => {
      if (data["error"] == 1) {
        this.dg.error("Thông báo", "Cập nhập tài khoản vào nhóm không thành công !");
        return false;
      } else if (data["error"] == 2) {
        this.dg.notify("Thông báo", "Tài khoản " + data["ms"] + " đã tồn tại trong nhóm !");
        this.modalNhomKy(this.rowselect);
      } else {
        this.dg.notify("Thông báo", "Cập nhật tài khoản vào nhóm thành công !");
        this.modalNhomKy(this.rowselect);
      }
      this.getListNhomKy();
      this.child.mUserSelect = false;
    });
  }

  deleteUserGroupSign(row: object) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.qtService.deleteUserGroupSignPhongBan(row).subscribe((data: Array<object>) => {
            if (data["error"] == 1) {
              this.dg.error("Thông báo", "Xóa tài khoản khỏi nhóm ký không thành công !");
              return false;
            } else {
              this.modalNhomKy(this.rowselect);
              this.getListNhomKy();
            }
          });
        }
      }).catch();
  }
}

