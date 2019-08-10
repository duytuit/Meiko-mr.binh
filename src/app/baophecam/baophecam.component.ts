import { Component, OnInit } from '@angular/core';
import { WorkCAMService } from '../service/work-cam.service';
import { QuytrinhService } from '../service/quytrinh.service';
import { BaopheService } from '../service/baophe.service';
import { NgSelectModule, NgOption } from '@ng-select/ng-select';
import { ToastrService } from 'ngx-toastr';
import { appPublic } from '../appPublic';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from '../ui/modal.service';
import { isType } from '@angular/core/src/type';
import { MyworkService } from '../service/mywork.service';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-baophecam',
  templateUrl: './baophecam.component.html',
  styleUrls: ['./baophecam.component.css']
})
export class BaophecamComponent implements OnInit {
  private ListMyDocumentCam = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  private Hs = {};
  private rowHS = {};
  private rowSelect = null;
  private A0028_ID = "";
  private ListA0034 = [];
  FormSelect: string;
  private TrangthaiHS = [
    { 'trangThai': 1, 'tentrangThai': 'Tài liệu đang xử lý' },
    { 'trangThai': 2, 'tentrangThai': 'Tài liệu đã hoàn thành' },
    { 'trangThai': 3, 'tentrangThai': 'Tài liệu đã dừng' },
  ];
  mHSCV: boolean;
  mSend: boolean;
  mEventLogs: boolean;
  Istab = 1;
  private ListFile: any[] = [];
  private ListPhongBan = [];
  private ListPhongBanTD = [];
  private ListMaHang = [];
  tk = {};
  department = [];
  constructor(private qtService: QuytrinhService, private workService: MyworkService, private workCAMService: WorkCAMService, private baopheService: BaopheService, private toastr: ToastrService, private dg: ModalService, private ShowLoading: Ng4LoadingSpinnerService) { }
  ngOnInit() {
    this.tk = JSON.parse(localStorage.getItem("login"));
    this.getDMBaoPheSelect();
    this.getListMyDocumentCam();
  }

  listDMBaoPheSelect = [];
  getDMBaoPheSelect() {
    this.baopheService.GetListNDBaoPheBySelect().subscribe((then: Array<object>) => {
      this.listDMBaoPheSelect = then["data"];
    });

    this.baopheService.GetAllPhongBanMapper().subscribe((then: Array<object>) => {
      this.ListPhongBan = then["data"];
      this.ListPhongBanTD = then["data"];
    });

    this.baopheService.GetListAllMaHang().subscribe((then: Array<object>) => {
      this.ListMaHang = then;
    });

  }

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

  getListMyDocumentCam() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.workCAMService.MyWorkdocumentCam(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
      this.ListMyDocumentCam = data["data"];
      this.ListMyDocumentCam.forEach(item => {
        var phantram = Math.ceil((parseInt(item.CountSign) / (parseInt(item.Countprocess))) * 100);
        item.phantram = phantram;
      });
      this.opition.total = data["count"];
      this.SetTotalPage();
    });
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListMyDocumentCam();
  }

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListMyDocumentCam();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListMyDocumentCam();
    }
  }

  Refersh = function () {
    this.mo = { checkAll: false };
    this.Hs = {};
    this.rowHS = {};
    this.rowSelect = null;
    this.A0028_ID = "";
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
    this.getListMyDocumentCam();
    this.toastr.success('Đang tải lại...', 'Làm mới');
  }

  mForm: boolean;
  ListFormDSF003 = [];
  ModalHSCV(istype) {
    this.FormSelect = null;
    this.mForm = true;
  }

  isViewDK: boolean;
  mForm006: boolean;
  IDFormSelect: string;
  SF006 = {};
  SF006D = [];
  ViewDKSDEmail() {
    this.isViewDK = !this.isViewDK;
  }

  modalSF006(isType: string) {
    this.ShowLoading.show();
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.ListA0034 = [];
    this.mForm006 = true;
    this.SF006 = { A0032_ID: "SF006", maForm: "SF006", T001C: 'Viet Nam', T002C: this.tk["displayName"], T003C: '', T004C: null, T005C: '', T006C: '', T007C: '', T008C: '', T009C: '', T010C: '', T011C: '', T012C: null, T013C: '', T014C: '', T015C: '', T016C: '' };
    this.SF006D = [];

    if (this.A0028_ID == null) {
      this.ListPhongBan.forEach(item => {
        var ListDMBaoPhe = [];
        item["ListA0035"].forEach(item2 => {
          var objbp = { C001C: item["A0034_ID"], C002C: item2["tenTiengViet"], C003C: item2["tenTiengNhat"], C004C: 0, C005C: '', C006C: '', C007C: '', C008C: item2["A0035_ID"], C009C: '', C010C: ListDMBaoPhe.length + 1, ListBoPhan: item2.ListBoPhan };
          ListDMBaoPhe.push(objbp);

          // Add Bộ phân báo phế
          var PBReceiDMBP = item2.ListBoPhan;
          PBReceiDMBP.forEach(bp => {
            var checkbp = this.ListA0034.find(x => x.A0034_ID == bp.A0034_ID);
            if (checkbp == null) {
              this.ListA0034.push(bp);
            }
          });

        });
        var obj = { C001C: item["A0034_ID"], C002C: this.SF006D.length + 1, ListDMBaoPhe: ListDMBaoPhe };
        this.SF006D.push(obj);
      });
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workCAMService.WorkDocumentDetailCam(obj).subscribe((then: Array<object>) => {
        this.SF006 = then["data"][0];
        var SF006DList = then["data2"];
        this.ListA0034 = then["data3"];
        this.ListPhongBan.forEach(item => {
          var rowcheck = SF006DList.find(x => x.C001C == item.A0034_ID);
          if (rowcheck != null) {
            item.ListA0035.forEach(c => {
              var row = rowcheck.ListDMBaoPhe.find(x => x.C008C == c.A0035_ID);
              if (row == null) {
                var objbp = { C001C: rowcheck["C001C"], C002C: c["tenTiengViet"], C003C: c["tenTiengNhat"], C004C: 0, C005C: '', C006C: '', C007C: '', C008C: c["A0035_ID"], C009C: '', C010C: rowcheck.ListDMBaoPhe.length + 1, ListBoPhan: c.ListBoPhan };
                rowcheck.ListDMBaoPhe.push(objbp);
              } else {
                row.ListBoPhan = c.ListBoPhan;
                row.C004C = parseInt(row.C004C);
              }
            });
          } else {
            var ListDMBaoPhe = [];
            item.ListA0035.forEach(c => {
              var objdmbp = { C001C: item["A0034_ID"], C002C: c["tenTiengViet"], C003C: c["tenTiengNhat"], C004C: 0, C005C: '', C006C: '', C007C: '', C008C: c["A0035_ID"], C009C: '', C010C: ListDMBaoPhe.length + 1, ListBoPhan: c.ListBoPhan };
              ListDMBaoPhe.push(objdmbp);
            });
            var rowpb = { C001C: item["A0034_ID"], C002C: SF006DList.length + 1, ListDMBaoPhe: ListDMBaoPhe };
            SF006DList.push(rowpb);
          }
        });
        this.SF006D = SF006DList;

        // this.SF006D.forEach(item => {
        //   item.ListDMBaoPhe.forEach(c => {
        //     c.C004C = parseInt(c.C004C);
        //   });
        // });
        // var objadd = { C001C: null, C002C: this.SF006D.length + 1, C003C: null, C004C: false, C005C: '', C006C: '', C007C: '', C008C: '', C009C: '', C010C: '', ListDMBaoPhe: [] }
        // this.SF006D.push(objadd);      

      });
    }
    this.ShowLoading.hide();
  }

  mDMBaoPhe: boolean;
  listDMBaoPhe = [];
  rowPhongBanSelect = {};
  modalDMBaoPhe(obj: object) {
    this.mDMBaoPhe = true;
    this.rowPhongBanSelect = obj;
    this.baopheService.GetListNDBaoPheByPhongBan(obj).subscribe((then: Array<object>) => {
      this.listDMBaoPhe = then;
    });
  }

  ChoiseDMBaoPhe() {
    var DMBaoPheSelect = this.listDMBaoPhe.filter(x => x.checked == true);
    var DMBaoPheCheck = this.SF006D.filter(x => x.C001C == this.rowPhongBanSelect["C001C"]);
    if (DMBaoPheCheck.length > 0) {
      var ListDMBPCheck = DMBaoPheCheck[0].ListDMBaoPhe;
      DMBaoPheSelect.forEach(item => {
        var check = ListDMBPCheck.find(x => x.C008C == item["A0035_ID"]);
        if (check == null) {
          var objbp = { C001C: this.rowPhongBanSelect["C001C"], C002C: item["tenTiengViet"], C003C: item["tenTiengNhat"], C004C: 0, C005C: '', C006C: '', C007C: '', C008C: item["A0035_ID"], C009C: '', C010C: ListDMBPCheck.length + 1 };
          ListDMBPCheck.push(objbp);
        }

        // Add Bộ phân báo phế
        var PBReceiDMBP = item.ListBoPhan;
        PBReceiDMBP.forEach(bp => {
          var checkbp = this.ListA0034.find(x => x.A0034_ID == bp.A0034_ID);
          if (checkbp == null) {
            this.ListA0034.push(bp);
          }
        });
      });
    } else {
      var ListDMBaoPhe = [];
      DMBaoPheSelect.forEach(item => {
        var objbp = { C001C: this.rowPhongBanSelect["C001C"], C002C: item["tenTiengViet"], C003C: item["tenTiengNhat"], C004C: 0, C005C: '', C006C: '', C007C: '', C008C: item["A0035_ID"], C009C: '', C010C: ListDMBaoPhe.length + 1 };
        ListDMBaoPhe.push(objbp);
        var PBReceiDMBP = item.ListBoPhan;
        PBReceiDMBP.forEach(bp => {
          var checkbp = this.ListA0034.find(x => x.A0034_ID == bp.A0034_ID);
          if (checkbp == null) {
            this.ListA0034.push(bp);
          }
        });
      });
      var obj = { C001C: this.rowPhongBanSelect["C001C"], C002C: this.SF006D.length + 1, ListDMBaoPhe: ListDMBaoPhe };
      this.SF006D.push(obj);
    }
    this.mDMBaoPhe = false;
    var checkaddrow = 0;
    this.SF006D.forEach(item => {
      if (item["ListDMBaoPhe"].length == 0) {
        checkaddrow = 1;
      } else {
        if (checkaddrow == 0) {
          checkaddrow = 0;
        }
      }
    });

    if (checkaddrow == 0) {
      var objadd = { C001C: null, C002C: this.SF006D.length + 1, C003C: null, C004C: false, C005C: '', C006C: '', C007C: '', C008C: '', C009C: '', C010C: '', ListDMBaoPhe: [] }
      this.SF006D.push(objadd);
    }

    // var checkpb = this.SF006D.find(x => x.C001C == this.rowPhongBanSelect["C001C"]);
    // if (checkpb != null) {
    //   var row = this.ListPhongBan.find(x => x.A0034_ID == this.rowPhongBanSelect["C001C"]);
    //   if (row != null) {
    //     row.disabled = 'disabled';
    //   }
    // }
  }

  ChangeSeletPB(row: object) {
    var C001 = row["C001C"];
    var checkTonTai = this.SF006D.filter(x => x.C001C == row["C001C"]);
    if (checkTonTai.length > 1) {
      if (row["C001C"] == "null" || row["C001C"] == null) {
        row["ListDMBaoPhe"] = [];
      } else {
        this.dg.error('Thông báo', 'Phòng ban bạn chọn đã tồn tại, vui lòng chọn phòng ban khác !');
        row["C001C"] = null;
        return false;
      }
    } else {
      if (row["ListDMBaoPhe"].length > 0) {
        this.dg.confirm('Confirm', 'Bạn có chắc chắn muốn thay đổi phòng ban, dữ liệu báo phế của phòng ban sẽ bị xóa hết ?')
          .then((confirmed) => {
            if (confirmed) {
              row["ListDMBaoPhe"] = [];
            }
          }).catch();
      }
    }
  }

  hideModal() {
    this.mHSCV = false;
    this.mEventLogs = false;
    this.mSend = false;
    this.mForm = false;
  }

  hideModalDMBP() {
    this.mDMBaoPhe = false;
  }

  hideModalForm() {
    this.mForm006 = false;
    this.mForm = false;
  }

  DocSend = {};
  ModalSignAndSend() {
    this.mSend = true;
    this.DocSend = {};
  }

  SignAndSend() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.baopheService.SendNextDocument(this.rowSelect, Users_ID, this.DocSend["noiDung"]).subscribe((then) => {
      this.getListMyDocumentCam();
      this.hideModal();
    });
  }

  private ListDocSign = [];
  private ListEventNote = [];
  ModalEvenNote(row: object) {
    this.mEventLogs = true;
    this.workService.EventNote(row).subscribe((then) => {
      this.ListDocSign = then["data"];
      this.ListEventNote = then["data2"];
    });
  }

  UpdateSF006() {
    if ((this.SF006["T004C"] == null || this.SF006["T004C"] == undefined) && (this.SF006["T017C"] == null || this.SF006["T017C"] == '')) {
      this.dg.error("Thông báo", "Bạn chưa chọn hoặc nhập mã hàng !");
      return false;
    }

    this.ListA0034 = [];
    var ListD = [];
    this.SF006D.forEach(item => {
      item["ListDMBaoPhe"].forEach(bp => {
        if (bp["C004C"] != 'False' && bp["C004C"] != 0) {
          var PBReceiDMBP = bp.ListBoPhan;
          PBReceiDMBP.forEach(bp => {
            var checkbp = this.ListA0034.find(x => x.A0034_ID == bp.A0034_ID);
            if (checkbp == null) {
              this.ListA0034.push(bp);
            }
          });
          bp.C009C = item["C002C"];
          ListD.push(bp)
        }
      });
    });

    var checkreturn: boolean;
    ListD.forEach(item => {
      if (item.C004C == false) {
        checkreturn = true;
        return false;
      }
    });

    if (checkreturn == true || ListD.length == 0) {
      this.dg.error("Thông báo", "Bạn chưa chọn nội dung cần báo phế !");
      return false;
    }
    this.ShowLoading.show();
    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF006));
    formData.append("a0028D", JSON.stringify(ListD));
    formData.append("a0034", JSON.stringify(this.ListA0034));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('file', file);
    }
    var type = 1;
    if (this.SF006["A0028_ID"] != null && this.SF006["A0028_ID"] != "null") {
      type = 2;
    }
    this.workCAMService.updateWorkDocumentCam(type, formData).subscribe(
      (then) => {
        this.ShowLoading.hide();
        this.hideModalForm();
        this.getListMyDocumentCam();
      },
    );
  }

  onFileSelect(event) {
    if (event.target.files.length > 0) {
      for (var i = 0; i <= event.target.files.length - 1; i++) {
        var selectedFile = event.target.files[i];
        this.ListFile.push(selectedFile);
      }
    }
  }

  selectItem(row: object) {
    this.ListMyDocumentCam.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.rowSelect = row;
  }

  // private ListDocSign = [];
  // private ListEventNote = [];
  // ModalEvenNote(row: object) {
  //   this.mEventLogs = true;
  //   this.workCAMService.EventNote(row).subscribe((then) => {
  //     this.ListDocSign = then["data"];
  //     this.ListEventNote = then["data2"];
  //   });
  // }

  setTabLog(tabs) {
    this.Istab = tabs;
  }

  // SignAndSend() {
  //   var tk = JSON.parse(localStorage.getItem("login"));
  //   var Users_ID = tk["Users_ID"];
  //   this.workCAMService.SendNextDocument(this.rowSelect, Users_ID, this.DocSend["noiDung"]).subscribe((then) => {
  //     this.getListMyDocumentCam();
  //     this.hideModal();
  //   });
  // }

  selectEdit(row: object) {
    this.rowHS = row;
    this.A0028_ID = row["A0028_ID"];
    this.modalSF006('update');
  }

  openFile(row: object) {
    var url = appPublic.api_Admin + row["duongDan"];
    window.open(url);
  }

  removeFiles(index, type) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa File đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.ListFile.splice(index, 1);
        }
      }).catch();
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.workCAMService.DeleteDocumentCam(this.rowSelect).subscribe((then) => {
            if (then["error"] == 0) {
              this.getListMyDocumentCam();
              this.rowSelect = null;
            }
          });
        }
      }).catch();
  }

  DeleteRowBP(row: object, index) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          row["ListDMBaoPhe"].splice(index, 1);
        }
      }).catch();
  }

  ChangeMH(type) {
    if (type == 1) {
      if (this.SF006["T004C"] != null) {
        this.SF006["T017C"] = null;
      }
    } else if (type == 2) {
      if (this.SF006["T017C"] != null) {
        this.SF006["T004C"] = null;
      }
    }
  }

  resetCheckbox(istype, row: object) {
    if (istype == 1) {
      if (row["C004C"] == 1) {
        row["C004C"] = 0;
      }
    } else if (istype == 2) {
      if (row["C004C"] == 2) {
        row["C004C"] = 0;
      }
    } else if (istype == 3) {
      if (row["C004C"] == 3) {
        row["C004C"] = 0;
      }
    } else if (istype == 4) {
      if (row["C004C"] == 4) {
        row["C004C"] = 0;
      }
    }
  }
}
