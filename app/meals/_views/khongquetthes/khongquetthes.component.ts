import { Component, OnInit } from '@angular/core';
import { MyworkService } from '../../../service/mywork.service';
import { QuytrinhService } from '../../../service/quytrinh.service';
import { NgSelectModule, NgOption } from '@ng-select/ng-select';
import { ToastrService } from 'ngx-toastr';
import { appPublic } from '../../../appPublic';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from '../../../ui/modal.service';
import { isType } from '@angular/core/src/type';

@Component({
  selector: 'app-khongquetthes',
  templateUrl: './khongquetthes.component.html',
  styleUrls: ['./khongquetthes.component.css']
})
export class KhongquettheComponent implements OnInit {
  private ListMyDocument = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  private Hs = {};
  private rowHS = {};
  private DocSend = {};
  private rowSelect = null;
  private A0028_ID = "";
  private ListCVGroupLCV = [];
  private FilesAttach = [];
  private DocSign = [];
  private DocTran = [];
  private ListPhongBanQuyTrinh = [];
  private ListNhomKy = [];
  private ListNguoiKy = [];
  private ListForm = [];
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
  private ListFile2: any[] = [];
  private ListFile3: any[] = [];
  private PhongBanList = [];
  private ListPhongBanSelect = [];
  tk = {};
  department = [];
  constructor(private qtService: QuytrinhService, private workService: MyworkService, private toastr: ToastrService, private dg: ModalService) { }
  ngOnInit() {
    this.getListLCV();
    this.getListForm();
    this.getListMyDocument();
    this.getphongbanInit();
  }

  getphongbanInit() {
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
      this.getPhongBan();
    }, 500);
  }

  getPhongBan() {
    this.Temp = [];
    this.addToArray(this.PhongBanList, null, 0);
    this.ListPhongBanSelect = this.Temp;
    this.tk = JSON.parse(localStorage.getItem("login"));
    this.department = this.PhongBanList.find(x => x.id == this.tk["Department"]);
    if (this.department == null) {
      this.getphongbanInit();
    }
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

  getListMyDocument() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.workService.MyWorkdocument(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
      this.ListMyDocument = data["data"];
      this.ListMyDocument.forEach(item => {
        var phantram = Math.ceil((parseInt(item.CountSign) / (parseInt(item.Countprocess))) * 100);
        item.phantram = phantram;
      });
      this.opition.total = data["count"];
      this.SetTotalPage();
    });
  }

  getListLCV() {
    this.qtService.getCVGroupLCVSelect().subscribe((data: Array<object>) => {
      this.ListCVGroupLCV = data["data"];
    });
  }

  getListForm() {
    this.qtService.getListFormSelect().subscribe((data: Array<object>) => {
      this.ListForm = data["data"];
    });
  }

  A0016_ID: string;
  changeSelectCV(rowID: string) {
    this.A0016_ID = rowID;
  }

  changePhongDept(Dept: object) {
    Dept["ListNhomKy"] = [];
    Dept["ListNguoiKy"] = [];
    Dept["A0016_ID"] = this.SF002["A0016_ID"];
    this.getGroupSignByDept(Dept);
  }

  getGroupSignByDept(Dept: object) {
    Dept["A0004_ID"] = Dept["C001C"];
    this.qtService.getWorkFollowCV(Dept).subscribe((data: Array<object>) => {
      var data = data["data"];
      if (data.length > 0) {
        Dept["ListNhomKy"] = data[0]["GroupSign"];
        if (Dept["ListNhomKy"].length <= 0) {
          this.dg.error("Thông báo", "Hiện tại phòng ban gửi tới này chưa có quy trình, liên hệ với Admin để cấu hình quy trình !");
        }
      } else {
        this.dg.error("Thông báo", "Hiện tại phòng ban gửi tới này chưa có quy trình, liên hệ với Admin để cấu hình quy trình !");
      }
      if (this.A0028_ID != null) {
        this.ChangeGroupSignByDept(Dept);
      }
    });
  }

  ChangeGroupSignByDept(Dept: object) {
    var rowGroupSign = Dept["ListNhomKy"].find(x => x.A0021_ID == Dept["C002C"]);
    this.qtService.getUserbyGroupSign(rowGroupSign).subscribe((then: Array<object>) => {
      Dept["ListNguoiKy"] = then["data"];
    });
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
        }
      } else {
        this.dg.error("Thông báo", "Hiện tại phòng ban gửi tới này chưa có quy trình, liên hệ với Admin để cấu hình quy trình !");
      }
    });
  }

  ChangeGroupSign() {
    var rowGroupSign = this.ListNhomKy.find(x => x.A0021_ID == this.Hs["Nhomky"]);
    this.qtService.getUserbyGroupSign(rowGroupSign).subscribe((then: Array<object>) => {
      this.ListNguoiKy = then["data"];
    });
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListMyDocument();
  }

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListMyDocument();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListMyDocument();
    }
  }

  Refersh = function () {
    this.mo = { checkAll: false };
    this.Hs = {};
    this.rowHS = {};
    this.rowSelect = null;
    this.A0028_ID = "";
    this.ListCVGroupLCV = [];
    this.FilesAttach = [];
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
    this.getListMyDocument();
    this.getListLCV();
    this.toastr.success('Đang tải lại...', 'Làm mới');
  }

  mForm: boolean;
  ListFormDSF003 = [];
  ModalHSCV(istype) {
    // if (istype == "add") {
    //   this.A0028_ID = null;
    // }
    // var tk = JSON.parse(localStorage.getItem("login"));
    // this.Hs = { A0002_ID: tk["Users_ID"], phongBan: null, Nhomky: null, Nguoiky: null, trangThai: 1 };
    // if (this.A0028_ID != null) {
    //   this.workService.WorkDocumentDetail(this.rowHS).subscribe((then: object) => {
    //     this.Hs = then["data"][0];
    //     this.Hs["ngayXuLyMongMuon"] = new Date(this.Hs["ngayXuLyMongMuon"]);
    //     this.FilesAttach = then["data2"];
    //   });
    // }
    this.FormSelect = null;
    this.mForm = true;
  }

  selectEdit(row: object) {
    this.rowHS = row;
    this.A0028_ID = row["A0028_ID"];
    this.workService.CheckWorkFollowForm(row).subscribe(
      (then) => {
        this.ListCVGroupLCV = then["data"];
        if (this.ListCVGroupLCV.length == 0) {
          this.dg.error("Thông báo", "Vui lòng liên hệ với Quản trị để cấu hình công việc cho Form, hiện tại Form đang bị lỗi !");
          return false;
        } else { 
          if (this.rowHS["maForm"] == "SF001") {
            this.modalSF001('update');
          } else if (this.rowHS["maForm"] == "SF002") {
            this.modalSF002('update')
          } else if (this.rowHS["maForm"] == "SF003") {
            this.modalSF003('update');
          } else if (this.rowHS["maForm"] == "SF004") {
            this.modalSF004('update');
          } else if (this.rowHS["maForm"] == "SF005") {
            this.modalSF005('update');
          } else if (this.rowHS["maForm"] == "SF007") {
            this.modalSF007('update');
          }
        }
      }
    );
  }

  isViewDK: boolean;
  mForm001: boolean;
  mForm002: boolean;
  mForm003: boolean;
  mForm004: boolean;
  mForm005: boolean;
  mForm006: boolean;
  mForm007: boolean;
  mForm008: boolean;
  mForm009: boolean;
  mForm010: boolean;
  mForm011: boolean;
  mForm012: boolean;
  mForm013: boolean;
  mForm014: boolean;
  mForm015: boolean;
  mForm016: boolean;
  mForm017: boolean;
  mForm018: boolean;
  mForm019: boolean;
  mForm020: boolean;
  IDFormSelect: string;
  selectForm() {
    var objform = this.ListForm.find(x => x.maForm == this.FormSelect);
    if (objform != null) {
      this.IDFormSelect = objform["A0032_ID"];
    } else {
      this.IDFormSelect = null;
    }
    this.workService.CheckWorkFollowForm(objform).subscribe(
      (then) => {
        this.ListCVGroupLCV = then["data"];
        if (this.ListCVGroupLCV.length == 0) {
          this.dg.error("Thông báo", "Vui lòng liên hệ với Quản trị để cấu hình công việc cho Form, hiện tại Form đang bị lỗi !");
          return false;
        } else {
          this.hideModalForm();
          if (this.FormSelect == "SF001") {
            this.modalSF001('add');
          } else if (this.FormSelect == "SF002") {
            this.modalSF002('add')
          } else if (this.FormSelect == "SF003") {
            this.modalSF003('add');
          } else if (this.FormSelect == "SF004") {
            this.modalSF004('add');
          } else if (this.FormSelect == "SF005") {
            this.modalSF005('add');
          } else if (this.FormSelect == "SF007") {
            this.modalSF007('add');
          }
        }
      }
    );
  }

  SF001 = {};
  SF002 = {};
  SF002D = [];
  SF003 = {};
  SF004 = {};
  SF005 = {};
  SF007 = {};
  ListSF004DPC = [];
  ListSF004DPM = [];

  ViewDKSDEmail() {
    this.isViewDK = !this.isViewDK;
  }

  modalSF001(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.FilesAttach = [];
    this.isViewDK = false;
    this.SF001 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T001C: this.department["bophan_ten"], T002C: false, T003C: false, T004C: false, T005C: false, T006C: false };
    if (this.ListCVGroupLCV.length == 1) {
      this.SF001["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF001 = then["data"][0];
        if (this.SF001["T002C"] == 'False') {
          this.SF001["T002C"] = false;
        } else {
          this.SF001["T002C"] = true;
        }

        if (this.SF001["T003C"] == 'False') {
          this.SF001["T003C"] = false;
        } else {
          this.SF001["T003C"] = true;
        }

        if (this.SF001["T004C"] == 'False') {
          this.SF001["T004C"] = false;
        } else {
          this.SF001["T004C"] = true;
        }

        if (this.SF001["T005C"] == 'False') {
          this.SF001["T005C"] = false;
        } else {
          this.SF001["T005C"] = true;
        }

        if (this.SF001["T007C"] == 'False') {
          this.SF001["T007C"] = false;
        } else {
          this.SF001["T007C"] = true;
        }

      });
    }
    this.mForm001 = true;
  }

  modalSF002(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.FilesAttach = [];
    this.SF002 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T002C: this.tk["displayName"], T001C: this.department["bophan_ten"] };
    this.SF002D = [{ 'C001C': null, 'C002C': null, 'C003C': null, 'C004C': '', 'C005C': '', IsPostion: 1 }];
    if (this.ListCVGroupLCV.length == 1) {
      this.SF002["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF002 = then["data"][0];
        this.SF002D = then["data2"];
        if (this.SF002D.length > 0) {
          this.SF002D.forEach(item => {
            this.changePhongDept(item);
          });
        }
      });
    }
    this.mForm002 = true;
  }

  addRowItemSF002(type) {
    var obj = { 'C001C': null, 'C002C': null, 'C003C': null, 'C004C': '', 'C005C': '', IsPostion: 1 };
    this.SF002D.push(obj);
  }

  removeRowSF002D(type, index) {
    this.SF002D.splice(index, 1);
  }

  modalSF003(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.FilesAttach = [];
    this.SF003 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T001C: this.tk["displayName"], T002C: this.tk["UserCode"], T003C: this.department["bophan_ten"], T004C: new Date() };
    if (this.ListCVGroupLCV.length == 1) {
      this.SF003["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF003 = then["data"][0];
      });
    }
    this.mForm003 = true;
  }

  modalSF004(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }

    this.ListFile = [];
    this.FilesAttach = [];
    this.SF004 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T001C: this.tk["displayName"], T002C: this.tk["UserCode"], T003C: this.department["bophan_ten"], T004C: new Date() };
    if (this.ListCVGroupLCV.length == 1) {
      this.SF004["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    this.ListSF004DPC = [
      { C001C: null, C002C: null, C003C: null, C004C: null, IsPostion: 1 }
    ];

    this.ListSF004DPM = [
      { C001C: null, C002C: null, C003C: null, C004C: null, IsPostion: 2 }
    ];

    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF004 = then["data"][0];
        var data2 = then["data2"];
        this.ListSF004DPC = data2.filter(x => x.IsPostion == 1);
        this.ListSF004DPM = data2.filter(x => x.IsPostion == 2);
      });
    }
    this.mForm004 = true;
  }

  modalSF005(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.FilesAttach = [];
    this.mForm005 = true;
    this.SF005 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T001C: false, T002C: false, T003C: false, T004C: this.tk["displayName"], T005C: this.tk["UserCode"], T006C: new Date(), T008C: false, T009C: false, T010C: false, T011C: false, T012C: false, T013C: false, T014C: false, T015C: false, T016C: false };
    if (this.ListCVGroupLCV.length == 1) {
      this.SF005["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF005 = then["data"][0];
        if (this.SF005["T001C"] == 'False') {
          this.SF005["T001C"] = false;
        } else {
          this.SF005["T001C"] = true;
        }

        if (this.SF005["T002C"] == 'False') {
          this.SF005["T002C"] = false;
        } else {
          this.SF005["T002C"] = true;
        }

        if (this.SF005["T003C"] == 'False') {
          this.SF005["T003C"] = false;
        } else {
          this.SF005["T003C"] = true;
        }

        if (this.SF005["T008C"] == 'False') {
          this.SF005["T008C"] = false;
        } else {
          this.SF005["T008C"] = true;
        }

        if (this.SF005["T009C"] == 'False') {
          this.SF005["T009C"] = false;
        } else {
          this.SF005["T009C"] = true;
        }

        if (this.SF005["T010C"] == 'False') {
          this.SF005["T010C"] = false;
        } else {
          this.SF005["T010C"] = true;
        }

        if (this.SF005["T011C"] == 'False') {
          this.SF005["T011C"] = false;
        } else {
          this.SF005["T011C"] = true;
        }

        if (this.SF005["T012C"] == 'False') {
          this.SF005["T012C"] = false;
        } else {
          this.SF005["T012C"] = true;
        }

        if (this.SF005["T013C"] == 'False') {
          this.SF005["T013C"] = false;
        } else {
          this.SF005["T013C"] = true;
        }

        if (this.SF005["T014C"] == 'False') {
          this.SF005["T014C"] = false;
        } else {
          this.SF005["T014C"] = true;
        }

        if (this.SF005["T015C"] == 'False') {
          this.SF005["T015C"] = false;
        } else {
          this.SF005["T015C"] = true;
        }

        if (this.SF005["T016C"] == 'False') {
          this.SF005["T016C"] = false;
        } else {
          this.SF005["T016C"] = true;
        }
      });
    }
  }

  FileAttachView1 = [];
  FileAttachView2 = [];
  FileAttachView3 = [];
  modalSF007(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }

    this.ListFile = [];
    this.ListFile2 = [];
    this.ListFile3 = [];
    this.FileAttachView1 = [];
    this.FileAttachView2 = [];
    this.FileAttachView3 = []; 
    this.SF007 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T003C: this.tk["displayName"], T004C: this.tk["UserCode"], T007C: this.department["bophan_ten"], T009C: new Date() };
    if (this.ListCVGroupLCV.length == 1) {
      this.SF007["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF007 = then["data"][0];
        if(this.SF007["T001C"] != null){
          this.SF007["T001C"] = parseInt(this.SF007["T001C"]);
        }
        var fileattach = then["data3"];
        fileattach.forEach(item => {
          item["file"] = item["duongDan"];
          item["stt"] = item["thuTu"];
        }); 
        this.FileAttachView1 = fileattach.filter(x => x.kieuFile == 1);
        this.FileAttachView2 = fileattach.filter(x => x.kieuFile == 2);
        this.FileAttachView3 = fileattach.filter(x => x.kieuFile == 3);
      });
    }
    this.mForm007 = true;
  }

  onFileSelectOPL(event, type) {
    if (type == 1) {
      if (event.target.files.length > 0) {
        for (var i = 0; i <= event.target.files.length - 1; i++) {
          var selectedFile = event.target.files[i];
          this.ListFile.push(selectedFile);
          var reader = new FileReader();
          reader.onload = (event: any) => {
            var stt = this.FileAttachView1.length + 1;
            var obj = { stt: this.FileAttachView1.length + 1, file: event.target.result, name: "file" + stt };
            this.FileAttachView1.push(obj);
          }
          reader.readAsDataURL(event.target.files[i]);
        }
      }
    } else if (type == 2) {
      if (event.target.files.length > 0) {
        for (var i = 0; i <= event.target.files.length - 1; i++) {
          var selectedFile = event.target.files[i];
          this.ListFile2.push(selectedFile);
          var reader = new FileReader();
          reader.onload = (event: any) => {
            var stt = this.FileAttachView2.length + 1;
            var obj = { stt: this.FileAttachView2.length + 1, file: event.target.result, name: "file" + stt };
            this.FileAttachView2.push(obj);
          }
          reader.readAsDataURL(event.target.files[i]);
        }
      }
    } else if (type == 3) {
      if (event.target.files.length > 0) {
        for (var i = 0; i <= event.target.files.length - 1; i++) {
          var selectedFile = event.target.files[i];
          this.ListFile3.push(selectedFile);
          var reader = new FileReader();
          reader.onload = (event: any) => {
            var stt = this.FileAttachView3.length + 1;
            var obj = { stt: this.FileAttachView3.length + 1, file: event.target.result, name: "file" + stt };
            this.FileAttachView3.push(obj);
          }
          reader.readAsDataURL(event.target.files[i]);
        }
      }
    }
  }

  UpArrowImages(istype, item, index) {
    if (istype == 1 && this.FileAttachView1.length > 1) {
      if (index != 0) {
        var sttold = item.stt;
        item.stt = sttold - 1;
        var indexfind = index - 1;
        this.FileAttachView1[indexfind].stt = sttold;
      }
    }
  }

  DownArrowImages(istype, item, index) {
    if (istype == 1 && this.FileAttachView1.length > 1) {
      if (index != this.FileAttachView1.length - 1) {
        var sttold = item.stt;
        item.stt = sttold + 1;
        var indexfind = index + 1;
        this.FileAttachView1[indexfind].stt = sttold;
      }
    }
  }

  addRowItemSF004(type: number) {
    if (type == 1) {
      var obj = {};
      obj["C001C"] = null;
      obj["C002C"] = null;
      obj["C003C"] = null;
      obj["C004C"] = null;
      obj["IsPostion"] = 1;
      this.ListSF004DPC.push(obj);
    } else {
      var obj = {};
      obj["C001C"] = null;
      obj["C002C"] = null;
      obj["C003C"] = null;
      obj["C004C"] = null;
      obj["IsPostion"] = 2;
      this.ListSF004DPM.push(obj);
    }
  }

  removeRowSF004(type, index) {
    if (type == 1) {
      this.ListSF004DPC.splice(index, 1);
    } else {
      this.ListSF004DPM.splice(index, 1);
    }

  }

  selectStatusPCSF004 = [
    { 'IDStatus': 1, 'NameStatus': 'New' },
    { 'IDStatus': 2, 'NameStatus': 'Secondhand' },
    { 'IDStatus': 3, 'NameStatus': 'Unknown' }
  ];

  selectTypePMSF004 = [
    { 'IDType': 1, 'NameType': 'Licence' },
    { 'IDType': 2, 'NameType': 'Free' },
    { 'IDType': 3, 'NameType': 'Unknown' }
  ];

  hideModal() {
    this.mHSCV = false;
    this.mEventLogs = false;
    this.mSend = false;
    this.mForm = false;
    this.mForm003 = false;
  }

  hideModalForm() {
    this.mForm001 = false;
    this.mForm002 = false;
    this.mForm003 = false;
    this.mForm004 = false;
    this.mForm005 = false;
    this.mForm006 = false;
    this.mForm007 = false;
    this.mForm008 = false;
    this.mForm009 = false;
    this.mForm010 = false;
    this.mForm011 = false;
    this.mForm012 = false;
    this.mForm013 = false;
    this.mForm014 = false;
    this.mForm015 = false;
    this.mForm016 = false;
    this.mForm017 = false;
    this.mForm018 = false;
    this.mForm019 = false;
    this.mForm020 = false;
    this.mForm = false;
  }

  UpdateSF001() {
    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF001));
    formData.append("a0028D", JSON.stringify(null));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }
    var type = 1;
    if (this.SF005["A0028_ID"] != null && this.SF005["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  UpdateSF002() {
    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF002));
    formData.append("a0028D", JSON.stringify(this.SF002D));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }
    var type = 1;
    if (this.SF005["A0028_ID"] != null && this.SF005["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  UpdateSF003() {
    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF003));
    formData.append("a0028D", JSON.stringify(null));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }
    var type = 1;
    if (this.SF005["A0028_ID"] != null && this.SF005["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  UpdateSF004() {
    var a0028D = [];
    this.ListSF004DPC.forEach(item => {
      a0028D.push(item);
    });

    this.ListSF004DPM.forEach(item => {
      a0028D.push(item);
    });

    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF004));
    formData.append("a0028D", JSON.stringify(a0028D));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }
    var type = 1;
    if (this.SF005["A0028_ID"] != null && this.SF005["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  UpdateSF005() {
    var formData = new FormData();
    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF005));
    formData.append("a0028D", JSON.stringify(null));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }
    var type = 1;
    if (this.SF005["A0028_ID"] != null && this.SF005["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  UpdateSF007() {
    var formData = new FormData();     
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('FileAttach1', file);
    }

    for (let i = 0; i < this.ListFile2.length; i++) {
      var file: File = this.ListFile2[i];
      formData.append('FileAttach2', file);
    }

    for (let i = 0; i < this.ListFile3.length; i++) {
      var file: File = this.ListFile3[i];
      formData.append('FileAttach3', file);
    }

    formData.append("tk", JSON.stringify(this.tk));
    formData.append("a0028", JSON.stringify(this.SF007));
    formData.append("a0028D", JSON.stringify(null));    
    var type = 1;
    if (this.SF007["A0028_ID"] != null && this.SF007["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModalForm();
        this.getListMyDocument();
      }
    );
  }

  DeleteImages(istype, index) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          if (istype == 1) {
            this.FileAttachView1.splice(index, 1);
            this.ListFile.splice(index, 1);
          } else if (istype == 2) {
            this.FileAttachView2.splice(index, 1);
            this.ListFile2.splice(index, 1);
          } else if (istype == 3) {
            this.FileAttachView3.splice(index, 1);
            this.ListFile3.splice(index, 1);
          }
        }
      }).catch();
  }

  updateHSCV() {
    var formData = new FormData();
    formData.append("a0028", JSON.stringify(this.Hs));
    // formData.append("A0004_ID", JSON.stringify(this.Hs["phongBan"]));
    // formData.append("A0019_ID", this.Hs["Nguoiky"]);
    // formData.append("A0021_ID", this.Hs["Nhomky"]);
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('file', file);
    }
    var type = 1;
    if (this.Hs["A0028_ID"] != null && this.Hs["A0028_ID"] != "null") {
      type = 2;
    }
    this.workService.updateWorkDocument(type, formData).subscribe(
      (then) => {
        this.hideModal();
        this.getListMyDocument();
      }
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
    this.ListMyDocument.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.rowSelect = row;
  }

  ModalSignAndSend() {
    this.mSend = true;
    this.DocSend = {};
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

  setTabLog(tabs) {
    this.Istab = tabs;
  }

  SignAndSend() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.workService.SendNextDocument(this.rowSelect, Users_ID, this.DocSend["noiDung"]).subscribe((then) => {
      this.getListMyDocument();
      this.hideModal();
    });
  }

  openFile(row: object) {
    var url = appPublic.api_Admin + row["duongDan"];
    window.open(url);
  }

  DeleteFiles(index, row: object) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.workService.DeleteFileDocument(row).subscribe((then) => {
            if (then["error"] == 0) {
              this.FilesAttach.splice(index, 1);
            }
          });
        }
      }).catch();
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
          this.workService.DeleteDocument(this.rowSelect).subscribe((then) => {
            if (then["error"] == 0) {
              this.getListMyDocument();
            }
          });
        }
      }).catch();
  }

}
