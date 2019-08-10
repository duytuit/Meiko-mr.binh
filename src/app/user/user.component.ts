import { Component, OnInit } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { MeikoTDUserService } from '../service/meiko-td-user.service'
import { ModalService } from '../ui/modal.service';
import { CompanyDiagramService } from '../service/company-diagram.service';
import { ExcelService } from '../service/excel.service';
import { MeikoTDRolesService } from '../service/meiko-td-roles.service';
import { appPublic } from '../appPublic';
import { ToastrService } from 'ngx-toastr';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  private User = [];
  private listUserNotSign = [];
  private listUserResetPass = [];
  private ListRoles = [];
  private us = {};
  private mo = { checkAll: false, checkAllPass: false };
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "", RoleID: null };
  private ListFile = FileList;

  A0002_ID: string;
  moUser: boolean;
  checkLen: number;
  checkLenPass: number;
  ListCompageDiagram = [];
  baseUrl: string = appPublic.api_Admin;
  constructor(private uService: MeikoTDUserService, private dg: ModalService, private cmpDiagramService: CompanyDiagramService, private rolesService: MeikoTDRolesService, private http: HttpClient, private toastr: ToastrService, private ShowLoading: Ng4LoadingSpinnerService) { }

  ngOnInit() {
    this.getListCompagyDiagram();
    this.userList();
    this.userListNoSign();
    this.userListResetPass();
    this.getListRolesbySelect();
  }
  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.userList();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.userList();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.userList();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "", RoleID: null };
    this.userList();
  }

  ListSearch() {
    this.opition.p = 1;
    this.userList();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.userList();
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
        filter[i].imgages = [{ preview: filter[i].logo, full: filter[i].logo, width: 32, height: 32 }]
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

  getListCompagyDiagram() {
    this.Temp = [];
    this.cmpDiagramService.getCompanyDiagram().subscribe((data: Array<object>) => {
      var phongban = data.filter(x => x["muc"] != "A01" && x["id"] != "BPC0000");
      this.addToArray(phongban, null, 0);
      this.ListCompageDiagram = this.Temp;
    });
  }

  public userList() {
    this.ShowLoading.show();
    this.uService.getUser(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, this.opition.RoleID).subscribe((data: Array<object>) => {
      this.User = data["data"];
      for (let i = 0; i < this.User.length; i++) {
        this.User[i].imgages = [{ preview: this.User[i].anhDaiDien, full: this.User[i].anhDaiDien, width: 32, height: 32 }]
      }
      this.opition.total = data["total"];
      this.SetTotalPage();
      this.ShowLoading.hide();
    });
  }

  public userListNoSign() {
    this.uService.getUserNoSign(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.listUserNotSign = data["data"];
    });
  }

  public userListResetPass() {
    this.uService.getUserResetPassword(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.listUserResetPass = data["data"];
    });
  }

  public getListRolesbySelect() {
    this.rolesService.getRolesbySelect().subscribe((data: Array<object>) => {
      this.ListRoles = data;
    });
  }

  FilterRoleUser(row) {
    this.userList();
  }

  toggleAll() {
    for (let index = 0; index < this.User.length; index++) {
      this.User[index]["checked"] = !this.mo.checkAll;
    }
    this.checkLen = this.User.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.User.filter(x => x.checked == true).length;
  }

  toggleAllPassword() {
    for (let index = 0; index < this.listUserResetPass.length; index++) {
      this.listUserResetPass[index]["checked"] = !this.mo.checkAllPass;
    }
    this.checkLenPass = this.listUserResetPass.filter(x => x.checked == true).length;
  }

  CheckChekedPassword() {
    this.checkLenPass = this.listUserResetPass.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0002_ID = row['A0002_ID'];
    this.modalUser('update');
  }

  public modalUser(istype: string) {
    if (istype == "add") {
      this.A0002_ID = null;
    }
    this.us = { A0004_ID: null, tinhTrang: 1, ngayVao: new Date(), passWordRandom: 123456, A0001_ID: null, anhDaiDien: 'assets/Img/noimages.gif', IsPosition: 0 };
    if (this.A0002_ID != null) {
      this.uService.getUserbyID(this.A0002_ID).subscribe((data: Array<object>) => {
        this.us = data[0];
        this.us["ngaySinh"] = new Date(this.us["ngaySinh"]);
        this.us["ngayVao"] = new Date(this.us["ngayVao"]);
      });
    }
    this.moUser = true;
  }

  hideModal() {
    this.moUser = false;
    this.mUserRole = false;
  }

  saveUser() {
    this.us["APKID"] = this.us["A0001_ID"];
    var formData = new FormData();
    formData.append("user", JSON.stringify(this.us));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('file', file);
    }
    var type = 1;
    if (this.us["A0002_ID"] != null && this.us["A0002_ID"] != "null") {
      type = 2;
    }
    this.uService.updateUser(type, formData).subscribe(
      (then) => {
        if (then["error"] == 1) {
          this.dg.error("Thông báo", "Có lỗi khi cập nhật tài khoản");
          return false;
        } else if (then["error"] == 2) {
          this.dg.error("Thông báo", "Tài khoản đã tồn tại, bạn vui lòng chọn tài khoản khác !");
          return false;
        }
        this.hideModal();
        this.userList();
      }
    );
  }

  onFileSelect(event) {
    if (event.target.files.length > 0) {
      this.ListFile = event.target.files;
    }
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listUserCheck = this.User.filter(x => x.checked == true);
          if (listUserCheck.length > 0) {
            for (let index = 0; index < listUserCheck.length; index++) {
              listID.push(listUserCheck[index].A0002_ID);
            }
            this.uService.deleteUser(listID).subscribe((then) => {
              this.userList();
            });
          } else {
            alert("Bạn chưa chọn mục nào để xóa");
          }
        }
      });
  }

  sysUserData(page) {
    // var data = [];
    // this.uService.sysUserASoft(1000, page).subscribe((then: Array<object>) => {
    //   data = then;
    //   // if (data.length > 0) {
    //   //   this.uService.saveUserAsoft(data).subscribe((then2) => {
    //   //     page = page + 1;
    //   //     this.sysUserData(page);
    //   //   });        
    //   // }
    // });
    this.uService.saveUserAsoft().subscribe((then) => {
      //this.sysUserData(page);
      this.dg.notify("Thông báo", "Đồng bộ dữ liệu thành công !");
      this.userListNoSign();
      this.userList();
    });
  }

  RefershPasswordUser(row) {
    this.dg.confirm('Confirm', 'Bạn có muốn Reset mật khẩu những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listUserCheck = [];
          listUserCheck.push(row);
          if (listUserCheck.length > 0) {
            for (let index = 0; index < listUserCheck.length; index++) {
              listID.push(listUserCheck[index]);
            }
            this.uService.ResetPasswordUser(listID).subscribe((then) => {
              this.dg.notify("Thông báo", "Reset lại mật khẩu thành công !");
              this.userListResetPass();
              this.userList();
            });
          } else {
            this.dg.error("Thông báo", "Bạn chưa chọn mục nào để Reset lại mật khẩu !");
            return false;
          }
        }
      });
  }

  RefershPassword() {
    this.dg.confirm('Confirm', 'Bạn có muốn Reset mật khẩu những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listUserCheck = this.listUserResetPass.filter(x => x.checked == true);
          if (listUserCheck.length > 0) {
            for (let index = 0; index < listUserCheck.length; index++) {
              listID.push(listUserCheck[index]);
            }
            this.uService.ResetPasswordUser(listID).subscribe((then) => {
              this.dg.notify("Thông báo", "Reset lại mật khẩu thành công !");
              this.userListResetPass();
              this.userList();
            });
          } else {
            this.dg.error("Thông báo", "Bạn chưa chọn mục nào để Reset lại mật khẩu !");
            return false;
          }
        }
      });
  }

  ExportExcel() {
    this.uService.ExportUser().subscribe((then) => {
      if (then["error"] == 0) {
        var linkfile = then["data"];
        window.open(this.baseUrl + linkfile);
      } else {
        this.dg.error("Thông báo", "Lỗi khi xuất dữ liệu người dùng !");
      }
    });
  }

  ExportRefershPassword() {
    this.uService.ExportUserResetPass().subscribe((then) => {
      if (then["error"] == 0) {
        var linkfile = then["data"];
        window.open(this.baseUrl + linkfile);
      } else {
        this.dg.error("Thông báo", "Lỗi khi xuất dữ liệu người dùng yêu cầu Reset mật khẩu !");
      }
    });
  }

  ExportUserNoSign() {
    this.uService.ExportUserNoSign().subscribe((then) => {
      if (then["error"] == 0) {
        var linkfile = then["data"];
        window.open(this.baseUrl + linkfile);
      } else {
        this.dg.error("Thông báo", "Lỗi khi xuất dữ liệu người dùng chưa có tài khoản !");
      }
    });
  }

  mUserRole: boolean;
  ListRoleUser = [];
  ListRole = [];
  rowuserSelect = {};
  userRoleSelect = [];
  ModalUserRole(row: object) {
    this.mUserRole = true;
    this.rowuserSelect = row;
    this.uService.UserGetRole(row).subscribe((data: Array<object>) => {
      this.ListRoleUser = data["data"];
      this.ListRole = data["data2"];
    });
  }

  SaveRoleUser() {
    var ListRoleSelect = [];
    var ListRole = this.ListRole.filter(x => x.checked == true);
    ListRole.forEach(item => {
      ListRoleSelect.push(item["A0001_ID"]);
    });
    this.uService.UpdateRoleUserPremisstion(ListRoleSelect, this.rowuserSelect["A0002_ID"]).subscribe((data: Array<object>) => {
      this.toastr.success("Cập nhật Role thao tác cho User thành công", "Thông báo");
      this.ModalUserRole(this.rowuserSelect);
    });
  }

  RefershURoles() {
    this.ModalUserRole(this.rowuserSelect);
  }

  CheckRoleUser() {
    this.userRoleSelect = this.ListRoleUser.filter(x => x.checked == true);
  }

  DeleteUserRoles() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.uService.DeleteRoleUserPremisstion(this.userRoleSelect).subscribe((data: Array<object>) => {
            this.toastr.success("Xóa Role khỏi User thành công", "Thông báo");
            this.ModalUserRole(this.rowuserSelect);
          });
        }
      });


  }
}
