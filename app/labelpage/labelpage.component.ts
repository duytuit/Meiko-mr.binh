import { Component, OnInit } from '@angular/core';
import { MeikoTDThamSoDungChungService } from '../service/meiko-td-tham-so-dung-chung.service';
import { ModalService } from '../ui/modal.service';

@Component({
  selector: 'app-labelpage',
  templateUrl: './labelpage.component.html',
  styleUrls: ['./labelpage.component.css']
})
export class LabelpageComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private LabelList = [];
  private label = {};
  private labelupdate = [];
  private rowlabel = {};
  private mo = { checkAll: false };
  mLabel: boolean;
  mLabelUpdate: boolean;
  maCode: string;
  checkLen: number;
  constructor(private labelService: MeikoTDThamSoDungChungService, private dg: ModalService) { }

  ngOnInit() {
    this.getListLabel();
  }
  SetTotalPage() {
    this.opition.totalpage = Math.ceil(this.opition.total / this.opition.pz);
  }

  setPageSize = function (pz) {
    this.opition.p = 1;
    this.opition.pz = pz;
    this.getListLabel();
  };

  PrevPage = function () {
    if (this.opition.p > 1) {
      this.opition.p--;
      this.getListLabel();
    }
  }

  NextPage = function () {
    if (this.opition.p < this.opition.totalpage) {
      this.opition.p++;
      this.getListLabel();
    }
  }

  Refersh = function () {
    this.opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "DESC", s: "" };
  }

  ListSearch() {
    this.opition.p = 1;
    this.getListLabel();
  }

  FilterStatus = function (sts) {
    this.opition.p = 1;
    this.opition.sts = sts;
    this.getListLabel();
  }

  public getListLabel() {
    this.labelService.getLabel(this.opition.s).subscribe((data: Array<object>) => {
      this.LabelList = data["data"];
    });
  }

  toggleAll() {
    for (let index = 0; index < this.LabelList.length; index++) {
      this.LabelList[index]["checked"] = !this.mo.checkAll;
    }
    this.checkLen = this.LabelList.filter(x => x.checked == true).length;
  }

  CheckCheked() {
    this.checkLen = this.LabelList.filter(x => x.checked == true).length;
  }

  selectEdit(row: object) {
    this.maCode = row['maCode'];
    this.rowlabel = row;
    this.ModalLabel('update');
  }

  ModalLabel(istype: string) {
    if (istype == 'add') {
      this.maCode = null;
    }
    this.label = {};
    if (this.maCode != null) {
      this.labelService.getLabelbyID(this.rowlabel).subscribe((data: Array<object>) => {
        this.labelupdate = data;
      });
    }
    if (this.maCode != null) {
      this.mLabelUpdate = true;
    } else {
      this.mLabel = true;
    }
  }

  hideModal() {
    this.mLabel = false;
    this.mLabelUpdate = false;
  }

  hideModalUpdate() {
    this.mLabelUpdate = false;
  }

  addLabel() {
    this.labelService.addLabel(this.label).subscribe((then) => {
      this.hideModal();
      this.getListLabel();
    });
  }

  updateLabel() {
    this.labelService.updateLabel(this.labelupdate).subscribe((then) => {
      this.hideModal();
      this.getListLabel();
    });
  }

  deleteList() {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          var listID = [];
          var listCheck = this.LabelList.filter(x => x.checked == true);
          if (listCheck.length > 0) {
            for (let index = 0; index < listCheck.length; index++) {
              listID.push(listCheck[index].maCode);
            }
            this.labelService.deleteLabel(listID).subscribe((then) => {
              this.getListLabel();
            });
            this.checkLen = 0;
          } else {
            alert("Bạn chưa chọn mục nào để xóa");
          }
        }
      }).catch();
  }

}
