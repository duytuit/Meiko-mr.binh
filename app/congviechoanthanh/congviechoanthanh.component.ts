import { Component, OnInit, ViewChild } from '@angular/core';
import { MyworkService } from '../service/mywork.service';
import { QuytrinhService } from '../service/quytrinh.service';
import { NgSelectModule, NgOption } from '@ng-select/ng-select';
import { ToastrService } from 'ngx-toastr';
import { appPublic } from '../appPublic';
import { UserListComponent } from '../user-list/user-list.component';
import { ModalService } from '../ui/modal.service';

@Component({
  selector: 'app-congviechoanthanh',
  templateUrl: './congviechoanthanh.component.html',
  styleUrls: ['./congviechoanthanh.component.css']
})
export class CongviechoanthanhComponent implements OnInit {
  private ListMyDocument = [];
  private ListCVGroupLCV = [];
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private mo = { checkAll: false };
  private DocSend = {};
  mHSCV: boolean;
  private Hscv = {};
  private HSCVDocument = null;
  private TrangthaiHS = [
    { 'trangThai': 1, 'tentrangThai': 'Tài liệu đang xử lý' },
    { 'trangThai': 2, 'tentrangThai': 'Tài liệu đã hoàn thành' },
    { 'trangThai': 3, 'tentrangThai': 'Tài liệu đã dừng' },
  ];
  tk = {};
  department = {};
  PhongBanList = [];
  constructor(private qtService: QuytrinhService, private workService: MyworkService, private toastr: ToastrService, private dg: ModalService) { }
  @ViewChild(UserListComponent) child: UserListComponent;
  ngOnInit() {
    setTimeout(() => {
      this.PhongBanList = appPublic.listPhongBan;
    this.tk = JSON.parse(localStorage.getItem("login"));
    this.department = this.PhongBanList.find(x => x.id == this.tk["Department"]);
    }, 500); 
    this.getListLCV();
    this.getListMyDocument();
  }

  getListMyDocument() {
    var tk = JSON.parse(localStorage.getItem("login"));
    var Users_ID = tk["Users_ID"];
    this.workService.CompletedDocument(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, Users_ID).subscribe((data: Array<object>) => {
      this.ListMyDocument = data["data"];
      this.ListMyDocument.forEach(item => {
        var phantram = Math.ceil((parseInt(item.CountSign) / (parseInt(item.Countprocess))) * 100);
        item.phantram = phantram;
      });
      this.opition.total = data["count"];
      this.SetTotalPage();
    });
  }

  hideModal() {
    this.mSend = false;
    this.mEventLogs = false;
    this.mHSCV = false;
    this.mCHSCV = false;
  }

  mEventLogs: boolean;
  Istab = 1;
  private ListDocSign = [];
  private ListEventNote = [];
  rowSelect = null;
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

  selectItem(row: object) {
    this.ListMyDocument.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.rowSelect = row;
  }

  rowHS:object;
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
  SF001 = {};
  SF002 = {};
  SF003 = {};
  SF004 = {};
  SF005 = {};
  SF007 = {};
  ListSF004DPC = [];
  ListSF004DPM = [];
  A0028_ID:string;
  IDFormSelect: string;
  FormSelect:string;

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
  
  ViewDKSDEmail() {
    this.isViewDK = !this.isViewDK;
  }

  modalSF001(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = [];
    this.isViewDK = false;
    this.SF001 = { A0032_ID: this.IDFormSelect, maForm: this.FormSelect, T002C: false, T003C: false, T004C: false, T005C: false, T006C: false };
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
    this.SF002 = { A0032_ID: this.IDFormSelect,maForm:this.FormSelect};
    if (this.ListCVGroupLCV.length == 1) {
      this.SF002["A0016_ID"] = this.ListCVGroupLCV[0].A0016_ID;
    }
    if (this.A0028_ID != null) {
      var obj = { A0028_ID: this.A0028_ID };
      this.workService.WorkDocumentDetail(obj).subscribe((then: Array<object>) => {
        this.SF002 = then["data"][0];
      });
    }
    this.mForm002 = true;
  }

  modalSF003(isType: string) {
    if (isType == "add") {
      this.A0028_ID = null;
    }
    this.ListFile = []; 
    this.SF003 = { A0032_ID: this.IDFormSelect,maForm:this.FormSelect};
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
    this.SF004 = { A0032_ID: this.IDFormSelect,maForm:this.FormSelect};
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
    this.mForm005 = true;
    this.SF005 = { A0032_ID: this.IDFormSelect,maForm:this.FormSelect, T001C: false, T002C: false, T003C: false};
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
  ListFile2 = [];
  ListFile3 = [];
  modalSF007(isType: string) {
    this.ListFile = [];
    this.ListFile2 = [];
    this.ListFile3 = [];
    this.FileAttachView1 = [];
    this.FileAttachView2 = [];
    this.FileAttachView3 = []; 
    this.SF007 = {};
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
  }

  Refersh = function () {
    this.mo = { checkAll: false };
    this.Hs = {};
    this.rowHS = {};
    this.rowSelect = null;
    this.ListCVGroupLCV = [];
    this.FilesAttach = [];
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
    this.getListMyDocument();
    this.getListLCV();
    this.toastr.success('Đang tải lại...', 'Làm mới');
  }

  getListLCV() {
    this.qtService.getCVGroupLCVSelect().subscribe((data: Array<object>) => {
      this.ListCVGroupLCV = data["data"];
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

  mSend: boolean;
  ModalSignAndSend() {
    this.mSend = true;
  }

  private Hs = {};
  private ListFile: any[] = [];

  ModalHSCV(row: object) {
    var tk = JSON.parse(localStorage.getItem("login"));
    this.Hs = { A0002_ID: tk["Users_ID"], trangThai: 1 };
    this.workService.WorkDocumentDetail(row).subscribe((then: object) => {
      this.Hs = then["data"][0];
      this.Hs["ngayXuLyMongMuon"] = new Date(this.Hs["ngayXuLyMongMuon"]);
      this.ListFile = then["data2"];
    });
    this.mHSCV = true;
  }

  openFile(row: object) {
    var url = appPublic.api_Admin + row["duongDan"];
    window.open(url);
  }

  mCHSCV: boolean;
  ModalCreateHSCV() {
    this.mCHSCV = true;
    this.HSCVDocument = this.rowSelect;
    this.Hscv = { trangThai: 1 };
  }

  typeChoise: number;
  showusersModal(type) {
    this.child.mUserSelect = true;
    this.typeChoise = type;
  }

  CheckUserChoise(ID, isType) {
    if (isType == 1) {
      var indexDXL = this.UserDXL.findIndex(x => x.A0002_ID == ID);
      if (indexDXL != -1) {
        this.UserDXL.splice(indexDXL, 1);
      }

      var indexDTG = this.UserDTD.findIndex(x => x.A0002_ID == ID);
      if (indexDTG != -1) {
        this.UserDTD.splice(indexDTG, 1);
      }

      var index = this.UserXLC.findIndex(x => x.A0002_ID == ID);
      if (index != -1) {
        return 1;
      }

    } else if (isType != 1) {
      var indexDXL = this.UserDXL.findIndex(x => x.A0002_ID == ID);
      var indexDTG = this.UserDTD.findIndex(x => x.A0002_ID == ID);
      var indexXLC = this.UserXLC.findIndex(x => x.A0002_ID == ID);

      if (indexDXL != -1 || indexDTG != -1 || indexXLC != -1) {
        return 1;
      }
    }
    return -1;
  }

  UserXLC = [];
  UserDXL = [];
  UserDTD = [];

  ChoiceSelectUser(listUser: Array<object>) {
    this.child.mUserSelect = false;
    for (let i = 0; i < listUser.length; i++) {
      var index = this.CheckUserChoise(listUser[i]["A0002_ID"], this.typeChoise);
      if (index == -1) {
        var obj = {};
        obj["A0002_ID"] = listUser[i]["A0002_ID"];
        obj["anhDaiDien"] = listUser[i]["anhDaiDien"];
        obj["hoVaTen"] = listUser[i]["hoVaTen"];
        if (this.typeChoise == 1) {
          this.UserXLC.push(obj);
        } else if (this.typeChoise == 2) {
          this.UserDXL.push(obj);
        } else if (this.typeChoise == 3) {
          this.UserDTD.push(obj);
        }
      }
    }
  }

  removeUser(index, type) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          if (type == 1) {
            this.UserXLC.splice(index, 1);
          } else if (type == 2) {
            this.UserDXL.splice(index, 1);
          } else if (type == 3) {
            this.UserDTD.splice(index, 1);
          }
        }
      }).catch();
  }

  CleanUserChoise(type) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa tất cả những User đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          if (type == 1) {
            this.UserXLC = [];
          } else if (type == 2) {
            this.UserDXL = [];
          } else if (type == 3) {
            this.UserDTD = [];
          }
        }
      }).catch();
  }
}