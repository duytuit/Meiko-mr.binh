import { Component, OnInit } from '@angular/core';
import { CompanyDiagramService } from '../service/company-diagram.service';
import { ModalService } from '../ui/modal.service';
import { from } from 'rxjs';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-company-diagram',
  templateUrl: './company-diagram.component.html',
  styleUrls: ['./company-diagram.component.css']
})
export class CompanyDiagramComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private ListCompageDiagram = [];
  private companydiagram = {};
  private mod = { checkAll: false };
  constructor(private cmpDiagramService: CompanyDiagramService, private dg: ModalService) { }
  mCompanyDiagram: boolean;
  A0004_ID: string;
  checkLen: number;
  ngOnInit() {
    this.getListCompanyDiagram();
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

  getListCompanyDiagram() {
    this.Temp = [];
    this.cmpDiagramService.getCompanyDiagram().subscribe((data: Array<object>) => {
      var phongban = data.filter(x => x["muc"] != "A01" && x["id"] != "BPC0000");
      this.addToArray(phongban, null, 0);
      this.ListCompageDiagram = this.Temp;
    });
  }

  toggleAll() {
    for (let index = 0; index < this.ListCompageDiagram.length; index++) {
      this.ListCompageDiagram[index]["checked"] = !this.mod.checkAll;
    }
    this.checkLen = this.ListCompageDiagram.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.ListCompageDiagram.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.A0004_ID = row["A0004_ID"];
    this.modalCompageDiagram("update");
  }

  modalCompageDiagram(isType: string) {
    if (isType == "add") {
      this.A0004_ID = null;
    }
    this.companydiagram = { IDCha: null, thuTu: this.ListCompageDiagram.length + 1, tinhTrang: true };
    if (this.A0004_ID != null) {
      this.cmpDiagramService.getCompanyDiagrambyID(this.A0004_ID).subscribe((data) => {
        this.companydiagram = data[0];
      });
    }
    this.mCompanyDiagram = true;
  }

  hideModal() {
    this.mCompanyDiagram = false;
  }

  saveCompanyDiagram() {
    this.cmpDiagramService.updateCompanyDiagram(this.companydiagram).subscribe(
      (then) => {
        this.hideModal();
        this.getListCompanyDiagram();
      }
    );
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listChecked = this.ListCompageDiagram.filter(x => x.checked == true);
          if (listChecked.length > 0) {
            for (let index = 0; index < listChecked.length; index++) {
              listID.push(listChecked[index]["A0004_ID"]);
            }
            this.cmpDiagramService.deleteCompanyDiagram(listID).subscribe(
              (then) => {
                this.getListCompanyDiagram();
              }
            );
          } else {
            alert("Bạn chưa chọn mục nào để xóa !")
          }
        }
      }).catch();
  }
}
