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

@Component({
  selector: 'app-baophechobophanxuly',
  templateUrl: './baophechobophanxuly.component.html',
  styleUrls: ['./baophechobophanxuly.component.css']
})
export class BaophechobophanxulyComponent implements OnInit {
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
  tk = {};
  department = [];
  constructor(private qtService: QuytrinhService, private workService: MyworkService, private workCAMService: WorkCAMService, private baopheService: BaopheService, private toastr: ToastrService, private dg: ModalService) { }
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
    this.workCAMService.WaitingProcessDocumentCam(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
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

  A0034_ID = null;
  A0037_ID = null;
  rowSelectBP = {};

  ViewAllBP() {
    if (this.A0034_ID != null && this.A0037_ID) {
      this.A0034_ID = null;
      this.A0037_ID = null;
      this.rowSelectBP = {};
      this.ListA0034.forEach(item => {
        item["isSelected"] = false;
      });
      this.modalSF006('update');
    }
  }

  ViewDetailBoPhan(row: object) {
    this.ListA0034.forEach((a) => {
      a.isSelected = false;
    });
    this.rowSelectBP = row;
    this.A0034_ID = row["A0034_ID"];
    this.A0037_ID = row["A0037_ID"];
    this.modalSF006('update');
  }

  modalSF006(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.ListA0034 = [];
    this.mForm006 = true;
    this.SF006 = { A0032_ID: "SF006", maForm: "SF006", T001C: '', T002C: '', T003C: '', T004C: '', T005C: '' };
    this.SF006D = [
      { C001C: null, C002C: 1, C003C: null, C004C: 0, C005C: '', C006C: '', C007C: '', C008C: '', C009C: '', C010C: '', ListDMBaoPhe: [] }
    ];
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID, A0034_ID: this.A0034_ID, A0037_ID: this.A0037_ID };
      this.workCAMService.WorkDocumentDetailCam(obj).subscribe((then: Array<object>) => {
        this.SF006 = then["data"][0];
        this.SF006D = then["data2"];
        this.ListA0034 = then["data3"];
        var ListA0039 = then["data5"];
        if (this.A0034_ID != null && this.A0037_ID) {
          this.ListA0034.forEach(item => {
            if (item["A0037_ID"] == this.A0037_ID) {
              item["isSelected"] = true;
            }
          });
        }
        this.SF006D.forEach(item => {
          item.ListDMBaoPhe.forEach(c => {
            c.C004C = parseInt(c.C004C);
            if (this.A0037_ID != null && this.A0037_ID != undefined) {
              var rowA039 = ListA0039.find(x => x.A0028D_ID == c.A0028D_ID);
              if (rowA039 != null && rowA039 != undefined) {
                if (rowA039["ngayXuLy"] != null) {
                  c.C005C = new Date(rowA039["ngayXuLy"]);
                } else {
                  c.C005C = null;
                }
                c.C006C = rowA039["nguoiXuLy"];
                c.C007C = rowA039["noiDungXuLy"];
              } else {
                c.C005C = null;
              }
            } else {
              c.C005C = null;
              c.C006C = null;
              c.C007C = null;
            }
          });
        });

      });
    }
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

  private ListDocSign = [];
  private ListEventNote = [];
  ModalEvenNote(row: object) {
    this.mEventLogs = true;
    this.workService.EventNote(row).subscribe((then) => {
      this.ListDocSign = then["data"];
      this.ListEventNote = then["data2"];
    });
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

  setTabLog(tabs) {
    this.Istab = tabs;
  }

  selectEdit(row: object) {
    this.A0034_ID = null;
    this.A0037_ID = null;
    this.rowSelectBP = {};
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
}
