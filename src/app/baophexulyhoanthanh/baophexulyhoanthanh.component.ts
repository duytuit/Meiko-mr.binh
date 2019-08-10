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
  selector: 'app-baophexulyhoanthanh',
  templateUrl: './baophexulyhoanthanh.component.html',
  styleUrls: ['./baophexulyhoanthanh.component.css']
})
export class BaophexulyhoanthanhComponent implements OnInit {
  private ListMyDocumentCam = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  private Hs = {};
  private rowHS = {};
  private rowSelect = null;
  private A0028_ID = "";
  private ListA0034 = [];
  private ListPhongBan = [];
  private ListPhongBanTD = [];
  private ListA0039 = [];
  FormSelect: string;
  baseUrl: string = appPublic.api_Admin;
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
  tk = {};
  department = [];
  constructor(private qtService: QuytrinhService, private workService: MyworkService, private workCAMService: WorkCAMService, private baopheService: BaopheService, private toastr: ToastrService, private dg: ModalService) { }
  ngOnInit() {
    this.tk = JSON.parse(localStorage.getItem("login"));
    this.getListMyDocumentCam();
    this.getDMBaoPheSelect();
  }

  listDMBaoPheSelect = [];
  getDMBaoPheSelect() {
    this.baopheService.GetListNDBaoPheBySelect().subscribe((then: Array<object>) => {
      this.listDMBaoPheSelect = then["data"];
    });

    this.baopheService.GetAllPhongBanMapper().subscribe((then: Array<object>) => {
      this.ListPhongBan = then["data"];
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
    this.workCAMService.CompletedDocumentConfirm(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
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
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.ListA0034 = [];
    this.ListA0039 = [];
    this.mForm006 = true;
    this.SF006 = { A0032_ID: "SF006", maForm: "SF006", T001C: 'Viet Nam', T002C: 'DŨNG', T003C: '311', T004C: '0826-6786WK', T005C: '1905061405' };
    this.SF006D = [
      { C001C: null, C002C: 1, C003C: null, C004C: 0, C005C: '', C006C: '', C007C: '', C008C: '', C009C: '', C010C: '', ListDMBaoPhe: [] }
    ];
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID, A0037_ID: this.rowHS["A0037_ID"], A0034_ID: this.rowHS["A0034_ID"] };
      this.workCAMService.WorkDocumentDetailCamConfirm(obj).subscribe((then: Array<object>) => {
        this.SF006 = then["data"][0];
        this.SF006D = then["data2"];
        this.ListA0034 = then["data3"];
        this.ListA0039 = then["data4"];
        this.SF006D.forEach(item => {
          item.ListDMBaoPhe.forEach(c => {
            c.C004C = parseInt(c.C004C);
            var rowA039 = this.ListA0039.find(x => x.A0028D_ID == c.A0028D_ID);
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
          });
        });
      });
    }
  }

  ExportExcel() {
    var obj = { A0028_ID: this.A0028_ID, A0037_ID: this.rowHS["A0037_ID"], A0034_ID: this.rowHS["A0034_ID"] };
    this.workCAMService.ExportExcel(obj).subscribe((then) => {
      if (then["error"] == 0) {
        var linkfile = then["data"];
        window.open(this.baseUrl + linkfile);
      } else {
        this.dg.error("Thông báo", "Lỗi khi xuất dữ liệu kết quả thi !");
      }
    });
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

  DocSend = {};
  ModalSignAndSend() {
    this.mSend = true;
    this.DocSend = {};
  }

  SignAndSend() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.workCAMService.SendNextDocument(this.rowSelect, Users_ID, this.DocSend["noiDung"]).subscribe((then) => {
      this.getListMyDocumentCam();
      this.hideModal();
    });
  }

  private ListDocSign = [];
  private ListEventNote = [];
  ModalEvenNote(row: object) {
    this.mEventLogs = true;
    this.workCAMService.EventNote(row).subscribe((then) => {
      this.ListDocSign = then["data"];
      this.ListEventNote = [];
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
