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
  selector: 'app-baophehoanthanh',
  templateUrl: './baophehoanthanh.component.html',
  styleUrls: ['./baophehoanthanh.component.css']
})
export class BaophehoanthanhComponent implements OnInit {
  private ListMyDocumentCam = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  private Hs = {};
  private rowHS = {};
  private rowSelect = null;
  private A0028_ID = "";
  private ListA0034 = [];
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
    this.workCAMService.CompletedDocumentCam(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
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
    this.SF006 = { A0032_ID: "SF006", maForm: "SF006", T001C: 'Viet Nam', T002C: 'DŨNG', T003C: '311', T004C: '0826-6786WK', T005C: '1905061405' };
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

  ListExcel = [];
  sttexcel = 0;
  // ExportExcel() {
  //   var htmltable = "";
  //   // htmltable += "<div class='Formtitle'>";
  //   // htmltable += "<h3>BIỂU YÊU CẦU BÁO PHẾ, THU HỒI CCSX<br /><span style='padding-top: 10px;display: inline-block;margin-bottom: 10px;'>冶工具破棄・返却依頼書</span></h3>";
  //   // htmltable += "</div>";
  //   htmltable += "<div style='width:1366px'>";
  //   htmltable += "<div style='padding-right: 0px;width:100px;float:left'>";
  //   htmltable += "<table style='margin-bottom:-1px;font-size: 12px'><thead><tr>";
  //   htmltable += "<th width='20' align='center' height='150'>STT</th>";
  //   htmltable += "<th width='80' align='center'>To:　Báo phế đến bộ phận 廃棄部門先</th>";
  //   htmltable += "</tr></thead><tbody>";
  //   this.ListA0034.forEach(item => {
  //     this.sttexcel = this.sttexcel + 1;
  //     htmltable += "<tr>";
  //     htmltable += "<td width='20' align='center'>" + this.sttexcel + "</td>";
  //     htmltable += "<td width='80' align='center'><b>" + item.tenPhongBan + "</b></td>";
  //     htmltable += "</tr>";
  //   });
  //   htmltable += "</tbody></table></div>";
  //   htmltable += "<div style='padding-left: 0px;width:1150px;float:left'>";
  //   htmltable += "<table style='table-layout: fixed;margin-bottom: -1px;font-size: 12px;width:100%'>";
  //   htmltable += "<thead>";
  //   htmltable += "<tr>";
  //   htmltable += "<th width='200' align='center' colspan='2'>基板製造投入工場<br />Nhà máy sản xuất bo mạch</th>";
  //   htmltable += "<th width='200' align='center' colspan='2'>" + this.SF006["T001C"] + "</th>";
  //   htmltable += "<th width='400' align='center' colspan='4'>";
  //   htmltable += "<div class='row'>";
  //   htmltable += "<div style='width:50%;float:left'>Phụ trách báo phế/廃棄担当者:</div>";
  //   htmltable += "<div style='width:25%;float:left'>" + this.SF006["T002C"] + "</div>";
  //   htmltable += "<div style='width:25%;float:left'>SĐT: " + this.SF006["T003C"] + "</div>";
  //   htmltable += "</div>";
  //   htmltable += "</th></tr>";
  //   htmltable += "<tr>";
  //   htmltable += "<th align='center' width='200' colspan='2'>品名コード（貨名代号）<br />Mã sản phẩm</th>";
  //   htmltable += "<th align='center' width='200' colspan='2'>" + this.SF006["T004C"] + "</th>";
  //   htmltable += "<th align='center' width='200' colspan='2'>文章NO.<br />Mã số báo phế</th>";
  //   htmltable += "<th align='center' width='200' colspan='2'>" + this.SF006["T005C"] + "</th>";
  //   htmltable += "</tr></thead></table>";
  //   htmltable += "<div class='row'>";
  //   htmltable += "<div style='padding-right: 0px;width:70%;float:left'>";
  //   htmltable += "<table class='table table-hover table-bordered' style='table-layout: fixed;font-size: 12px;margin-bottom: 0px'><thead><tr>";
  //   htmltable += "<th align='center' width='300'>Nội dung 内容</th>";
  //   htmltable += "<th align='center' width='60'>返却<br />Hoàn trả</th>";
  //   htmltable += "<th align='center' width='60'>変更<br />Thay đổi</th>";
  //   htmltable += "<th align='center' width='60'>破棄<br />Báo phế</th>";
  //   htmltable += "<th align='center' width='60'>差し替え<br />Thay thế</th>";
  //   htmltable += "<th align='center' width='80'>処置日<br />Ngày xử lý</th>";
  //   htmltable += "<th align='center' width='90'>処置内容<br />Nội dung xử lý</th>";
  //   htmltable += "<th align='center' width='80'>処置者<br />Người xử lý</th>";
  //   htmltable += "</tr></thead></table>";
  //   htmltable += "<table class='table table-hover table-bordered' style='table-layout: fixed;margin-top: -1px'>";

  //   for (let i = 0; i < this.SF006D.length; i++) {
  //     htmltable += "<tbody>";
  //     for (let j = 0; j < this.SF006D[i].ListDMBaoPhe.length; j++) {
  //       htmltable += "<tr>";
  //       if (j == 0) {
  //         var pb = this.ListPhongBan.find(x => x.A0034_ID == this.SF006D[i].C001C)
  //         htmltable += "<td align='center' width='150' [attr.rowspan]='item.ListDMBaoPhe.length' *ngIf='i == 0'>";
  //         htmltable += this.SF006D[i].ListDMBaoPhe[j].tenPhongBan;
  //         htmltable += "</td>";
  //       }
  //       htmltable += "<td align='left' width='150'> " + this.SF006D[i].ListDMBaoPhe[j].C002C + "<br />" + this.SF006D[i].ListDMBaoPhe[j].C003C + "</td>";
  //       htmltable += "<td align='center' width='60'><label>" + this.SF006D[i].ListDMBaoPhe[j].C004C + "</label></td>";
  //       htmltable += "<td align='center' width='60'><label>" + this.SF006D[i].ListDMBaoPhe[j].C004C + "</label></td>";
  //       htmltable += "<td align='center' width='60'><label>" + this.SF006D[i].ListDMBaoPhe[j].C004C + "</label></td>";
  //       htmltable += "<td align='center' width='60'><label>" + this.SF006D[i].ListDMBaoPhe[j].C004C + "</label></td>";
  //       htmltable += "<td align='left' width='80'><span>" + this.SF006D[i].ListDMBaoPhe[j].C005C + "</span></td>";
  //       htmltable += "<td align='left' width='90'><span>" + this.SF006D[i].ListDMBaoPhe[j].C006C + "</span></td>";
  //       htmltable += "<td align='left' width='80'><span>" + this.SF006D[i].ListDMBaoPhe[j].C007C + "</span></td>";
  //       htmltable += "</tr>";
  //     }
  //     htmltable += "</tbody>";
  //   }

  //   htmltable += "</table>";
  //   htmltable += "</div>";
  //   htmltable += "<div style='padding-left: 0px;width:30%;float:left'>";
  //   htmltable += "<table style='table-layout: fixed;margin-bottom: 5px'>";
  //   htmltable += "<thead>";
  //   htmltable += "<tr><th align='center' colspan='2'>変更内容(処置方法）<br />Nội dung thay đổi (Cách thức xử lý)</th></tr>";
  //   htmltable += "</thead>";
  //   htmltable += "<tbody>";
  //   htmltable += "<tr><td align='left'>Độ cấp thiết xử lý<br />処理緊急度 </td><td align='center'>" + this.SF006["T006C"] + "/" + this.SF006["T007C"] + "</td></tr>";
  //   htmltable += "<tr><td align='left' colspan='2'>Nội dung thay đổi 変更内容</td></tr>";
  //   htmltable += "<tr><td align='left' colspan='2'>" + this.SF006["T008C"] + "</td></tr>";
  //   htmltable += "<tr><td align='left' colspan='2'>Dữ liệu báo phế 廃棄データ</td></tr>";
  //   htmltable += "<tr><td align='left' colspan='2'>" + this.SF006["T009C"] + "</td></tr>";
  //   htmltable += "</tbody>";
  //   htmltable += "</table>";
  //   htmltable += "<div style='clear: both'></div>";
  //   htmltable += "<div style='padding: 0px'>";
  //   htmltable += "<table class='table table-hover' style='table-layout: fixed;border: 2px solid black;margin-left: 0px;margin-bottom: 0px'>";
  //   htmltable += "<tbody>";
  //   htmltable += "<tr><td align='left'>Mã số công cụ/ツール番号:</td></tr>";
  //   htmltable += "<tr><td align='left'>" + this.SF006["T010C"] + "</td></tr>";
  //   htmltable += "<tr><td align='left'>備考/Ghi chú:</td></tr>";
  //   htmltable += "<tr><td align='left'>" + this.SF006["T011C"] + "</td></tr>";
  //   htmltable += "</tbody>";
  //   htmltable += "</table>";
  //   htmltable += "</div>";
  //   htmltable += "<div style='clear: both'></div>";
  //   htmltable += "<div style='padding: 5px;margin-top: 5px'>";
  //   htmltable += "<h5 style='line-height: 20px;text-align: center;font-weight: bold;font-size: 13px;margin-top: -5px'>ランニングチェンジ品の処理方法<br />Phương pháp xử lý hàng running change</h5>";
  //   if (this.SF006["T012C"] == "1") {
  //     htmltable += "Dừng sản xuất ngay(đến khi có công cụ mới). - 生産をストップ（新データが出来るまで";
  //   } else if (this.SF006["T012C"] == "2") {
  //     htmltable += "Tiếp tục sản xuất khi hoàn thành công cụ sẽ tiến hành thay đổi(sản xuất liên tục). - 生産は続いて、ツール完了したら差し替え（継続生産";
  //   } else if (this.SF006["T012C"] == "3") {
  //     htmltable += "Chia lót ra để thay đổi(ghi rõ vào giấy lot tiến hành thay đổi). - ロット切り替えとする（切り替えロットを別紙で明確に）";
  //   }
  //   htmltable += "</div>";
  //   htmltable += "<div style='clear: both'></div>";
  //   htmltable += "<div style='width: 100%;margin: 5px 0px;border-top: 1px dashed black;padding: 1px 0px;margin-top: 15px;'>";
  //   htmltable += "</div>";
  //   htmltable += "<div style='clear: both'></div>";
  //   htmltable += "<div style='padding: 5px;padding-top: 0px;'>";
  //   htmltable += "<table class='table table-hover' border='0' style='margin-bottom: 0px'>";
  //   htmltable += "<tbody>";
  //   htmltable += "<tr><td align='center' style='border: 0px'>Phương pháp xử lý tồn kho của hàng trước khi thay đổi<br />変更前在庫分処理方法</td></tr>";
  //   htmltable += "<tr><td align='center' style='border: 0px'>" + this.SF006["T013C"] + "</td></tr>";
  //   htmltable += "</tbody></table>";
  //   htmltable += "</div><div style='clear: both'></div>";
  //   htmltable += "<div style='padding: 5px;border: 2px solid black;'>";
  //   htmltable += "<span style='display: block;position: relative;float: left;margin: 5px 0px 7px 0px;'>破棄在庫が１０ｍ２以上になる物は、下記項目を記入すること</span>";
  //   htmltable += "<span style='display: block;position: relative;float: left;margin-bottom: 7px'>Hàng tồn kho báo phế trên 10m2 thì nhập vào mục phía dưới</span>";
  //   htmltable += "<table class='table table-hover' border='1' style='table-layout: fixed;margin-left: 0px;margin-bottom: 0px;border: 1px solid #dddddd;'>";
  //   htmltable += "<tbody>";
  //   htmltable += "<tr>";
  //   htmltable += "<td align='center' width='60'>層数<br />Số lớp</td>";
  //   htmltable += "<td align='center' width='70'>枚数<br />Số sheet</td>";
  //   htmltable += "<td align='center'>１枚当ｍ２<br />Số m2/sheet</td>";
  //   htmltable += "</tr><tr>";
  //   htmltable += "<td align='left' width='60'>" + this.SF006["T014C"] + "</td>";
  //   htmltable += "<td align='left' width='70'>" + this.SF006["T015C"] + "</td>";
  //   htmltable += "<td align='left'>" + this.SF006["T016C"] + "</td>";
  //   htmltable += "</tr>";
  //   htmltable += "</tbody>";
  //   htmltable += "</table></div></div></div></div><div style='clear: both'></div>";
  //   htmltable += "</div>";
  //   var name = "Form Bao Phe";
  //   var style = "";

  //   var uri = 'data:application/vnd.ms-excel;base64,'
  //     , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta charset="utf-8"><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]-->' + style + '</head><body><table>{table}</table></body></html>'
  //     , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
  //     , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
  //   var ctx = { worksheet: name || 'Worksheet', table: htmltable.replace(/\u00a0/g, " ") };
  //   var url = uri + base64(format(template, ctx));
  //   var a = document.createElement('a');
  //   document.body.appendChild(a);
  //   a.href = url;
  //   a.download = name + '.xls';
  //   a.click();
  //   setTimeout(function () { window.URL.revokeObjectURL(url); }, 0);
  // };

  ExportExcel() {
    var obj = { A0028_ID: this.A0028_ID, A0034_ID: this.A0034_ID, A0037_ID: this.A0037_ID };
    this.workCAMService.ExportExcel(obj).subscribe((then) => {
      if (then["error"] == 0) {
        var linkfile = then["data"];
        window.open(this.baseUrl + linkfile);
      } else {
        this.dg.error("Thông báo", "Lỗi khi xuất dữ liệu kết quả thi !");
      }
    });
  }

}
