import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { MeikoTDNgonNguService } from '../service/meiko-td-ngon-ngu.service';
import { ModalService } from '../ui/modal.service';
@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private LangList = [];
  private lang = {};
  private mo = { checkAll: false };
  mLang: boolean;
  A0008_ID: string;
  checkLen: number;
  private ListFile = FileList;
  constructor(private langService: MeikoTDNgonNguService, private dg: ModalService) { }

  ngOnInit() {
    this.getListLang();
  }

  onFileSelect(event) {
    if (event.target.files.length > 0) {
      this.ListFile = event.target.files;
    }
  }

  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListLang();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListLang();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListLang();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListLang();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListLang();
  }

  public getListLang() {
    this.langService.getLang(this.opition.pz, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.LangList = data["data"];
      for (let i = 0; i < this.LangList.length; i++) {
        this.LangList[i].imgages = [{ preview: this.LangList[i].Icon, full: this.LangList[i].Icon, width: 32, height: 32 }]
      }
      this.opition.total = data["total"];
      this.SetTotalPage();
    });
  }

  toggleAll() {
    for (let index = 0; index < this.LangList.length; index++) {
      this.LangList[index]["checked"] = !this.mo.checkAll;
    }
    this.checkLen = this.LangList.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.LangList.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0008_ID = row['A0008_ID'];
    this.ModalLanguage('update');
  }

  ModalLanguage(istype: string) {
    if (istype == 'add') {
      this.A0008_ID = null;
    }
    this.lang = { thuTu: this.LangList.length + 1, tinhTrang: true };
    if (this.A0008_ID != null) {
      this.langService.getLangbyID(this.A0008_ID).subscribe((data: Array<object>) => {
        this.lang = data[0];
      });
    }
    this.mLang = true;
  }

  hideModal() {
    this.mLang = false;
  }

  saveLang() {

    var formData = new FormData();
    formData.append("lang", JSON.stringify(this.lang));
    for (let i = 0; i < this.ListFile.length; i++) {
      var file: File = this.ListFile[i];
      formData.append('file', file);
    }
    var istype = 1;
    if (this.lang["A0008_ID"] != undefined && this.lang["A0008_ID"] != "add") {
      istype = 2;
    }

    this.langService.updateLang(formData, istype).subscribe((then) => {
      this.hideModal();
      this.getListLang();
    });
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listCheck = this.LangList.filter(x => x.checked == true);
          if (listCheck.length > 0) {
            for (let index = 0; index < listCheck.length; index++) {
              listID.push(listCheck[index].A0008_ID);
            }
            this.langService.deleteLang(listID).subscribe((then) => {
              this.getListLang();
            });
          } else {
            alert("Bạn chưa chọn mục nào để xóa");
          }
        }
      }).catch();
  }

}
