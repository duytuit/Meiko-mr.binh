import { Component, OnInit } from '@angular/core';
import { BaopheService } from '../service/baophe.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { CompanyDiagramService } from '../service/company-diagram.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-phongbanmapper',
  templateUrl: './phongbanmapper.component.html',
  styleUrls: ['./phongbanmapper.component.css']
})
export class PhongbanmapperComponent implements OnInit {

  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListPBMapper = [];
  private ListCompageDiagram = [];
  private listDanhMucBaoPhe = [];
  private listDanhMucBaoPheSelect = [];
  private PBMapper = {};
  private mod = { checkAll: false };
  constructor(private cmpDiagramService: CompanyDiagramService, private baopheService: BaopheService, private dg: ModalService, private toastr: ToastrService) { }
  mPBMapper: boolean;
  mDMBaoPhe: boolean;
  A0034_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListCompanyDiagram();
    this.getListphongBanMapper();
    this.getListDMBaoPhe();
  }

  Temp2 = [];
  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    filter = this.sortBycpn(filter, 'bophan_ten');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "---";
      }
      lv++;
      for (let i = 0; i < filter.length; i++) {
        filter[i].lv = lv;
        if (lv == 1) {
          filter[i].color = "green";
          filter[i].bold = "bold";
        } else if (lv == 2) {
          filter[i].color = "#fb3f00";
          filter[i].bold = "bold";
        } else if (lv == 3) {
          filter[i].color = "#023f98";
          filter[i].bold = "bold";
        } else if (lv == 4) {
          filter[i].color = "#d9534f";
          filter[i].bold = "300";
        } else {
          filter[i].color = "black";
          filter[i].bold = "300";
        }
        filter[i].close = true;
        filter[i].tenPhongBanMoi = sp + filter[i].bophan_ten;
        filter[i].imgages = [{ preview: filter[i].logo, full: filter[i].logo, width: 32, height: 32 }]
        this.Temp2.push(filter[i]);
        this.addToArray2(array, filter[i].id, lv);
      }
    }
  };

  sortBycpn(arr: any[], field: string) {
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

  getListCompanyDiagram() {
    this.Temp2 = [];
    this.cmpDiagramService.getCompanyDiagram().subscribe((data: Array<object>) => {
      var phongban = data.filter(x => x["muc"] != "A01" && x["id"] != "BPC0000");
      this.addToArray2(phongban, null, 0);
      this.ListCompageDiagram = this.Temp2;
    });
  }

  Temp = [];
  addToArray(array, id, lv) {
    var filter = array.filter(x => x.Parent_ID == id);
    filter = this.sortBy(filter, 'tenPhongBan');
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "---";
      }
      lv++;
      for (let i = 0; i < filter.length; i++) {
        filter[i].lv = lv;
        if (lv == 1) {
          filter[i].color = "green";
          filter[i].bold = "bold";
        } else if (lv == 2) {
          filter[i].color = "#fb3f00";
          filter[i].bold = "bold";
        } else if (lv == 3) {
          filter[i].color = "#023f98";
          filter[i].bold = "bold";
        } else if (lv == 4) {
          filter[i].color = "#d9534f";
          filter[i].bold = "300";
        } else {
          filter[i].color = "black";
          filter[i].bold = "300";
        }
        filter[i].close = true;
        filter[i].tenPhongBanMoi = sp + filter[i].tenPhongBan;
        this.Temp.push(filter[i]);
        this.addToArray(array, filter[i].A0034_ID, lv);
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

  getListDMBaoPhe() {
    this.baopheService.GetListNDBaoPheBySelect().subscribe((data: Array<object>) => {
      this.listDanhMucBaoPhe = data["data"];
    });
  }

  getListphongBanMapper() {
    this.Temp = [];
    this.baopheService.GetListPhongBanMapper(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts, 1).subscribe((data: Array<object>) => {
      var pb = data["data"];
      this.addToArray(pb, null, 0);
      this.ListPBMapper = this.Temp;
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListPBMapper.length; index++) {
      this.ListPBMapper[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListPBMapper.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListPBMapper.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0034_ID = row["A0034_ID"];
    this.modalPhongBanMapper("update");
  }


  modalPhongBanMapper(isType: string) {
    if (isType == "add") {
      this.A0034_ID = null;
    }
    this.PBMapper = { Parent_ID: null, phongBanMapID: null, STT: this.ListPBMapper.length + 1, kieuPhongBan: 1, trangThai: true };
    if (this.A0034_ID != null) {
      var obj = {};
      obj["A0034_ID"] = this.A0034_ID;
      this.baopheService.GetPhongBanMapperByID(obj).subscribe((data) => {
        this.PBMapper = data;
      });
    }
    this.mPBMapper = true;
  }

  hideModal() {
    this.mPBMapper = false;
  }

  savePBMapper() {
    this.baopheService.updatePhongBanMapper(this.PBMapper).subscribe(
      (then) => {
        this.hideModal();
        this.getListphongBanMapper();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListPBMapper.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0034_ID"]);
            }
            this.baopheService.deletePhongBanMapper(listID).subscribe(
              (then) => {
                this.getListphongBanMapper();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }

  rowBPSelect = {};
  modalDMBaoPhePB(obj: object) {
    this.dmBPSelect = [];
    this.rowBPSelect = obj;
    this.baopheService.GetDMBaoPheByPhongBanID(obj).subscribe((data: Array<object>) => {
      this.listDanhMucBaoPheSelect = data;
    });
    this.mDMBaoPhe = true;
  }

  hideModalDMBPPB() {
    this.mDMBaoPhe = false;
  }

  ChoiseDanhMucPhongBan() {
    var ListDMChoise = this.listDanhMucBaoPhe.filter(x => x.checked == true);
    this.baopheService.updateDMBaoPheToPhongBan(this.rowBPSelect, ListDMChoise).subscribe(
      (then) => {
        this.modalDMBaoPhePB(this.rowBPSelect);
        this.getListphongBanMapper();
      }
    );
  }

  RefershDMBP() {
    this.dmBPSelect = [];
    this.modalDMBaoPhePB(this.rowBPSelect);
  }

  dmBPSelect = [];
  CheckDMBP() {
    this.dmBPSelect = this.listDanhMucBaoPheSelect.filter(x => x.checked == true);
  }

  DeleteDMBP() { 
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
            var ListDMBP = [];
            this.dmBPSelect.forEach(item => {
              ListDMBP.push(item["A0035_ID"]);
            });
            var A0034_ID = this.rowBPSelect["A0034_ID"];
            this.baopheService.DeleteDMBPPhongBan(ListDMBP, A0034_ID, 0).subscribe((data: Array<object>) => {
            this.toastr.success("Xóa danh mục báo phế khỏi phòng ban thành công", "Thông báo");
            this.modalDMBaoPhePB(this.rowBPSelect);
          });
        }
      });
  }

}
