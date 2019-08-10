import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { MeikoTDRolesService } from '../service/meiko-td-roles.service';
import { Roles } from '../model/Roles';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { UserListComponent } from '../user-list/user-list.component';
import { RoleUserService } from '../service/role-user.service';
import { forEach } from '@angular/router/src/utils/collection';
import { ToastrService } from 'ngx-toastr';
import * as moment from 'moment';
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
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.css']
})
export class RolesComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "", su: "" };
  private roles = [];
  private NhomUserCD = [];
  private listBuaAn = [];
  private listQuyenBuaAnUser = [];
  private ro = {};
  private mo = { checkAll: false };
  addNewPostForm: FormGroup;
  constructor(private rolesService: MeikoTDRolesService, private dg: ModalService, private rolemenuservice: RoleUserService, private toastr: ToastrService) { }
  mRolesCD: boolean;
  mRoles: boolean;
  mClockBA: boolean;
  A0001_ID: string;
  A0002_ID: string;
  checkLen: number;
  mUserSelect: boolean;
  mRolesCDBA: boolean;
  roleIDModal: string;
  A0004_ID: string;
  private tk = {};
  private Rolemenu = [];
  private ListPhongBan = [];
  private PhongBanList = [];
  private ListPhongBanSelect = [];
  private BuaAn = {};

  @ViewChild(UserListComponent) child: UserListComponent;
  ngOnInit() {
    this.tk = JSON.parse(localStorage.getItem("login"));
    this.mUserSelect = true;
    this.getListRoles();    
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

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListRoles();
  };

  showSuccess() {
    this.toastr.success('Đang tải lại...', 'Tải lại trang');
  }

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListRoles();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListRoles();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "", su: "" };
    this.getListRoles();
    this.showSuccess();
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListRoles();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListRoles();
  }

  public getListRoles() { 
    this.rolesService.getRoles(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts,this.tk["Users_ID"]).subscribe((data: Array<object>) => {
      this.roles = data["data"];
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  public getListPhongBan() {

  }

  toggleAll() {
    for (let index = 0; index < this.roles.length; index++) {
      this.roles[index]["checked"] = !this.mo.checkAll;
    }
    this.checkLen = this.roles.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.roles.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0001_ID = row['A0001_ID'];
    this.ModalRoles('update');
  }

  ModalRoles(istype: string) {
    if (istype == 'add') {
      this.A0001_ID = null;
    }
    this.ro = { thuTu: this.roles.length + 1, tinhTrang: true };
    if (this.A0001_ID != null) {
      this.rolesService.getRolesbyID(this.A0001_ID).subscribe((data: Array<object>) => {
        this.ro = data[0];
      });
    }
    this.mRoles = true;
  }

  rowrole = {};
  ModalUserRole(row: object) {
    this.A0002_ID = null;
    this.rowrole = row;
    this.roleIDModal = row["A0001_ID"];
    this.rolesService.getUserbyRoleID(row["A0001_ID"], this.opition.su).subscribe((data: Array<object>) => {
      this.NhomUserCD = data;
    });
    this.mRolesCD = true;
    this.getRoleMenu();
  }

  RefershURoles() {
    this.A0002_ID = null;
    this.NhomUserCD.forEach((a) => {
      a.isSelected = false;
    });
    this.getRoleMenu();
  }

  SearchMUserCD() {
    var row = { A0001_ID: this.roleIDModal };
    this.ModalUserRole(row);
  }

  SearchMUserCDPQBA() {
    var row = { A0001_ID: this.roleIDModal };
    this.ModalUserRoleBuaAn(row);
  }

  hideModal() {
    this.mRoles = false;
    this.mRolesCD = false;
    this.mRolesCDBA = false;
    this.mClockBA = false;
  }

  saveRoles() {
    this.rolesService.updateRoles(this.ro).subscribe((then) => {
      this.hideModal();
      this.getListRoles();
    });
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listCheck = this.roles.filter(x => x.checked == true);
          if (listCheck.length > 0) {
            for (let index = 0; index < listCheck.length; index++) {
              listID.push(listCheck[index].A0001_ID);
            }
            this.rolesService.deleteRoles(listID).subscribe((then) => {
              this.getListRoles();
            });
          } else {
            alert("Bạn chưa chọn mục nào để xóa");
          }
        }
      }).catch();
  }

  showusersModal() {
    this.child.mUserSelect = true;
  }

  ChoiceSelectUser(listUser: Array<string>) {
    var UserList = [];
    listUser.forEach(item => {
      UserList.push(item["A0002_ID"]);
    });  
    this.rolesService.UpdateUsertoRole(this.roleIDModal, UserList).subscribe((then) => {
      this.child.mUserSelect = false;
      var row = { A0001_ID: this.roleIDModal };
      this.ModalUserRole(row);
      this.getListRoles();
    });
  }

  Temp2 = [];
  arrfilter = [];
  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.IDCha == id);
    if (filter.length > 0) {
      for (let index = 0; index < filter.length; index++) {
        var children = this.Rolemenu.filter(x => x.IDCha == filter[index].A0007_ID);
        var check = this.arrfilter.find(x => x.A0007_ID == filter[index].A0007_ID);
        if ((children.length > 0 || filter[index]["IDCha"] == null)) {
          var children2 = [];
          for (let i = 0; i < children.length; i++) {
            this.arrfilter.push(children[i]);
          }
          filter[index].children = children;
          if (check == null) {
            this.Temp2.push(filter[index]);
          }
          this.addToArray2(array, filter[index].A0007_ID, lv);
        }
      }
    }
  }

  Temp3 = [];
  addtoArray3() {
    this.Temp2.forEach(element => {
      let tmp: any = {
        data: {},
        children: []
      };
      Object.keys(element).forEach(prop => {
        if (prop != 'children') {
          tmp.data[prop] = element[prop];
        } else {
          element[prop].forEach(c1 => {
            let tmp1: any = {
              data: {},
              children: []
            };
            Object.keys(c1).forEach(prop1 => {
              if (prop1 != 'children') {
                tmp1.data[prop1] = c1[prop1];
              } else {
                c1[prop1].forEach(c2 => {
                  let tmp2: any = {
                    data: {},
                    children: []
                  };
                  Object.keys(c2).forEach(prop2 => {
                    if (prop2 != 'children') {
                      tmp2.data[prop2] = c2[prop2];
                    } else {
                      c2[prop2].forEach(c3 => {
                        tmp2.children.push({ data: c3 });
                      });
                    }
                  });
                  tmp1.children.push(tmp2);
                });
              }
            });
            tmp.children.push(tmp1);
          });
        }
      });
      this.Temp3.push(tmp);
    });
  }

  getRoleMenu() {
    this.Temp2 = [];
    this.Temp3 = [];
    this.rolemenuservice.getRoleUser(this.roleIDModal, this.A0002_ID).subscribe((data: Array<object>) => {
      this.Rolemenu = data;
      this.addToArray2(data, null, 0);
      this.addtoArray3();
      this.Rolemenu = this.Temp3;
    });
  }

  userRoleSelect = null;
  ViewPermisstion(row: object) { 
    this.A0002_ID = row["A0002_ID"];
    this.NhomUserCD.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.userRoleSelect = row;
    this.getRoleMenu();
  }

  Submit() {
    var arr = [];
    for (let i = 0; i < this.Rolemenu.length; i++) {
      var childrens = this.Rolemenu[i].children;
      arr.push(this.Rolemenu[i]["data"]);
      if (childrens && childrens.length > 0) {
        for (let j = 0; j < childrens.length; j++) {
          arr.push(childrens[j]["data"]);
          var childrenss = childrens[j].children;
          if (childrenss && childrenss.length > 0) {
            for (let k = 0; k < childrenss.length; k++) {
              arr.push(childrenss[k]["data"]);
              var childrensss = childrenss[k].children;
              if (childrensss && childrensss.length > 0) {
                for (let k = 0; k < childrensss.length; k++) {
                  arr.push(childrensss[k]["data"]);
                }
              }
            }
          }
        }
      }
    }
    this.rolemenuservice.updateRoleMenuUser(arr, this.roleIDModal, this.A0002_ID).subscribe((then) => {
      if (then["error"] == 1) {
        alert("Bạn không thể phân quyền cho User vì Role chưa được phân quyền");
        return false;
      } else {
        this.getRoleMenu();
      }
    });
  }

  ModalUserRoleBuaAn(row: object) {
    this.A0002_ID = null;
    this.A0004_ID = null;
    this.roleIDModal = row["A0001_ID"];
    this.rolesService.getUserbyRoleID(row["A0001_ID"], this.opition.su).subscribe((data: Array<object>) => {
      this.NhomUserCD = data;
    });
    this.mRolesCDBA = true;
    this.getListBuaAn(1);
  }

  ViewPermisstionBA(row: object) {
    this.A0002_ID = row["A0002_ID"];
    this.A0004_ID = row["A0004_ID"];
    this.NhomUserCD.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.getListBuaAn(2);
  }

  getRoleBuaAnByUser() {
    this.rolesService.getPQBuaAnbyUser(this.roleIDModal, this.A0002_ID).subscribe((then: Array<object>) => {
      this.listQuyenBuaAnUser = then;
      var data2 = [];
      if (this.listQuyenBuaAnUser.length > 0) {
        for (let i = 0; i < this.listQuyenBuaAnUser.length; i++) {
          var ba = this.listBuaAn.find(x => x.id == this.listQuyenBuaAnUser[i].BuaAnID);
          if (ba != null) {
            this.listQuyenBuaAnUser[i].ten = ba.ten;
            this.listQuyenBuaAnUser[i].ma = ba.ma;
          }
          this.listQuyenBuaAnUser[i].thoiGianGioiHan = new Date(this.listQuyenBuaAnUser[i].thoiGianGioiHan);
          this.listQuyenBuaAnUser[i].thoiGian = this.returnDate(this.listQuyenBuaAnUser[i].thoiGianGioiHan);
        }
        //this.listBuaAn.forEach(item => {
        //var check = this.listQuyenBuaAnUser.find(x => x.BuaAnID == item.id);
        // if (check == null) {

        // }
        //item.A0004_ID = this.A0004_ID;
        //data2.push(item);
        //});
        //this.listBuaAn = data2; 
      }
    });
  }

  returnDate(date: Date) {
    var hours = "";
    var min = "";
    if (date.getMinutes() < 10) {
      min = "0" + date.getMinutes();
    } else {
      min = date.getMinutes().toString();
    }

    if (date.getHours() < 10) {
      hours = "0" + date.getHours();
    } else {
      hours = date.getHours().toString();
    }
    return hours + ":" + min + ":" + "00";
  }

  getListBuaAn(type) {
    this.rolesService.getBuaAn().subscribe((data: Array<object>) => {
      this.listBuaAn = data;
      this.listBuaAn.forEach(item => {
        item.A0004_ID = this.A0004_ID;
      });
      if (type == 2) {
        this.getRoleBuaAnByUser();
      }
    });
  }

  SubmitPQBuaAn() {
    var checkedBA = this.listBuaAn.filter(x => x.checked == true);
    if (checkedBA.length == 0) {
      this.dg.error("Thông báo", "Bạn chưa chọn bữa ăn nào đề phân quyền");
      return false;
    }

    var listCheck = [];
    if (this.A0002_ID != null) {
      this.listBuaAn.forEach(item => {
        if (item.checked == true) {
          if (item.ma == "S") {
            item.thoiGianGioiHan = new Date(new Date().setHours(4, 0, 0));
          } else if (item.ma == "T") {
            item.thoiGianGioiHan = new Date(new Date().setHours(10, 0, 0));
          } else if (item.ma == "C") {
            item.thoiGianGioiHan = new Date(new Date().setHours(15, 0, 0));
          } else if (item.ma == "N") {
            item.thoiGianGioiHan = new Date(new Date().setHours(19, 0, 0));
          } else if (item.ma == "F") {
            item.thoiGianGioiHan = new Date(new Date().setHours(19, 0, 0));
          } else {
            item.thoiGianGioiHan = new Date(new Date().setHours(10, 0, 0));
          }

          var thoiGianGioiHan = moment(item.thoiGianGioiHan).utc().format('YYYY-MM-DDTHH:mm:ssZZ');
          item.thoiGianGioiHan = thoiGianGioiHan;

          item.BuaAnID = item.id;
          item.A0002_ID = this.A0002_ID;
          item.A0001_ID = this.roleIDModal;
          item.quyenThem = true;
          listCheck.push(item);
        }
      });
      this.rolesService.updateRoleBuaAnUser(listCheck, 1).subscribe((then) => {
        this.toastr.success("Phân quyền bữa ăn thành công", "Thông báo");
        this.getListBuaAn(2);
      });
    } else if (this.A0002_ID == null) {
      for (let i = 0; i < this.NhomUserCD.length; i++) {
        var A0002ID = this.NhomUserCD[i].A0002_ID;
        for (let j = 0; j < checkedBA.length; j++) {
          var obj = {};
          if (checkedBA[j]["ma"] == "S") {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(4, 0, 0));
          } else if (checkedBA[j]["ma"] == "T") {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(10, 0, 0));
          } else if (checkedBA[j]["ma"] == "C") {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(15, 0, 0));
          } else if (checkedBA[j]["ma"] == "N") {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(19, 0, 0));
          } else if (checkedBA[j]["ma"] == "F") {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(19, 0, 0));
          } else {
            obj["thoiGianGioiHan"] = new Date(new Date().setHours(10, 0, 0));
          }

          var thoiGianGioiHan = moment(obj["thoiGianGioiHan"]).utc().format('YYYY-MM-DDTHH:mm:ssZZ');
          obj["thoiGianGioiHan"] = thoiGianGioiHan;
          obj["BuaAnID"] = checkedBA[j]["id"];
          obj["A0002_ID"] = A0002ID;
          obj["A0004_ID"] = this.NhomUserCD[i]["A0004_ID"];
          obj["A0001_ID"] = this.roleIDModal;
          obj["quyenThem"] = true;
          listCheck.push(obj);
        }
      }
      this.rolesService.updateRoleBuaAnUserAll(listCheck).subscribe((then) => {
        this.toastr.success("Phân quyền bữa ăn thành công", "Thông báo");
        this.getListBuaAn(2);
      });
    }
  }

  SubmitSavePQBuaAn() {
    this.listQuyenBuaAnUser.forEach(item => {
      var thoiGianSet = new Date().setHours(item.thoiGian.toString().substring(0, 2), item.thoiGian.toString().substring(3, 5), 0);
      var thoiGian = moment(new Date(thoiGianSet)).utc().format('YYYY-MM-DDTHH:mm:ssZZ');
      item.thoiGianGioiHan = thoiGian;
    });

    this.rolesService.updateRoleBuaAnUser(this.listQuyenBuaAnUser, 2).subscribe((then) => {
      this.getListBuaAn(2);
    });
  }

  RefershPQBA() {
    this.getListBuaAn(2);
  }

  ModalClockUser() {
    this.mClockBA = true;
    this.BuaAn = { BuaAnID: null, A0001_ID: this.roleIDModal };
  }

  savePQTimeBA() {
    var thoiGianSet = new Date().setHours(this.BuaAn["thoiGian"].toString().substring(0, 2), this.BuaAn["thoiGian"].toString().substring(3, 5), 0);
    var thoiGian = moment(new Date(thoiGianSet)).utc().format('YYYY-MM-DDTHH:mm:ssZZ');
    this.BuaAn["thoiGianGioiHan"] = thoiGian;
    this.rolesService.updateRoleBuaAnTime(this.BuaAn).subscribe((then) => {
      this.toastr.success("Phân quyền thời gian bữa ăn cho tất cả User thành công", "Thông báo");
      this.getListBuaAn(2);
    });
    this.mClockBA = false;
  }

  deletePermisstion(row: object) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.rolesService.deleteRoleBuaAn(row).subscribe((then) => {
            this.getListBuaAn(2);
          });
        }
      }).catch();
  }

  DeleteUserRoles()
  {
    this.userRoleSelect["A0001_ID"] = this.rowrole["A0001_ID"];
    this.dg.confirm('Confirm', 'Bạn có muốn xóa người dùng khỏi Roles không ?')
    .then((confirmed) => {
      if (confirmed) {
        this.rolesService.deleteUserRole(this.userRoleSelect).subscribe((then) => {
          this.userRoleSelect = null;
          this.ModalUserRole(this.rowrole);
        });
      }
    }).catch();
  }

}
