import { Component, OnInit, ViewChild } from '@angular/core';
import { MeikoTDMenuService } from '../service/meiko-td-menu.service';
import { MeikoTDModuleService } from '../service/meiko-td-module.service';
import { ModalService } from '../ui/modal.service';
import { TreemenuComponent } from './treemenu.component';
import { from } from 'rxjs';
//import { PrimeTemplate } from 'primeng/components/common/shared';

// export interface TreeNode {
//   label?: string;
//   data?: any;
//   icon?: any;
//   expandedIcon?: any;
//   collapsedIcon?: any;
//   children?: TreeNode[];
//   leaf?: boolean;
//   expanded?: boolean;
//   type?: string;
//   parent?: TreeNode;
//   partialSelected?: boolean;
//   styleClass?: string;
//   draggable?: boolean;
//   droppable?: boolean;
//   selectable?: boolean;
// }

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})

export class MenuComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private menu = [];
  private me = {};
  private rowmenu = {};
  private listSelectMenu = [];
  private listSelectModule = [];
  private mo = { checkAll: false };
  private Temp = [];
  private Temp2 = [];
  mMenu: boolean;
  A0007_ID: string;
  checkLen: number;
  cols: any[];
  private FileIcon: any[] = [];
  private FileIconDoc: any[] = [];
  constructor(private menuService: MeikoTDMenuService, private moduleService: MeikoTDModuleService, private dg: ModalService) {
  }

  ngOnInit() {
    this.getSelect();
    this.getMenu();
  }

  // onFileSelect(event) {
  //   if (event.target.files.length > 0) {
  //     this.ListFile = event.target.files;
  //   }
  // }

  onFileSelect(event, type) {
    if (type == 1) {
      if (event.target.files.length > 0) {
        this.FileIcon = event.target.files;
        var reader = new FileReader();
        var files = event.target.files;
        reader.readAsDataURL(files[0]);
        reader.onload = (_event) => {
          this.me["Icon"] = reader.result;
        }
      }
    } else {
      if (event.target.files.length > 0) {
        this.FileIconDoc = event.target.files;

        var reader = new FileReader();
        var files = event.target.files;
        reader.readAsDataURL(files[0]);
        reader.onload = (_event) => {
          this.me["anhDoc"] = reader.result;
        }
      }
    }
  }

  addToArray(array, id, lv) {
    var filter = array.filter(x => x.IDCha == id);
    if (filter.length > 0) {
      filter = this.sortBy(filter, 'thuThu');
    }
    if (filter.length > 0) {
      var sp = "";
      for (var i = 0; i < lv; i++) {
        sp += "---";
      }
      lv++;
      for (let index = 0; index < filter.length; index++) {
        filter[index].lv = lv;
        filter[index].close = true;
        filter[index].tenMenuNew = sp + " " + filter[index].tenMenu;
        this.Temp.push(filter[index]);
        this.addToArray(array, filter[index].A0007_ID, lv);
      }
    }
  }

  // arrfilter = [];
  // addToArray2(array, id, lv) {
  //   var filter = array.filter(x => x.IDCha == id);
  //   if (filter.length > 0) {
  //     for (let index = 0; index < filter.length; index++) {
  //       var children = this.menu.filter(x => x.IDCha == filter[index].A0007_ID);
  //       var check = this.arrfilter.find(x => x.A0007_ID == filter[index].A0007_ID);
  //       if ((children.length > 0 || filter[index]["IDCha"] == null)) {
  //         var children2 = [];
  //         for (let i = 0; i < children.length; i++) {
  //           this.arrfilter.push(children[i]);
  //           //var obj = { data: children[i] };
  //           //children2.push(obj);
  //         }
  //         filter[index].children = children;
  //         if (check == null) {
  //           //var data = { data: filter[index], children: children2 };
  //           this.Temp2.push(filter[index]);
  //         }
  //         this.addToArray2(array, filter[index].A0007_ID, lv);
  //       }
  //     }
  //   }
  // }

  // temp3 = [];
  // addtoArray3() {
  //   this.Temp2.forEach(element => {
  //     let tmp: any = {
  //       data: {},
  //       children: []
  //     };
  //     Object.keys(element).forEach(prop => {
  //       if (prop != 'children') {
  //         tmp.data[prop] = element[prop];
  //       } else {
  //         element[prop].forEach(c1 => {
  //           let tmp1: any = {
  //             data: {},
  //             children: []
  //           };
  //           Object.keys(c1).forEach(prop1 => {
  //             if (prop1 != 'children') {
  //               tmp1.data[prop1] = c1[prop1];
  //             } else {
  //               // c1[prop1].forEach(c2 => {
  //               //   tmp1.children.push({ data: c2 });
  //               // }); 
  //               c1[prop1].forEach(c2 => {
  //                 let tmp2: any = {
  //                   data: {},
  //                   children: []
  //                 };
  //                 Object.keys(c2).forEach(prop2 => {
  //                   if (prop2 != 'children') {
  //                     tmp2.data[prop2] = c2[prop2];
  //                   } else {
  //                     c2[prop2].forEach(c3 => {
  //                       tmp2.children.push({ data: c3 });
  //                     });
  //                   }
  //                 });
  //                 tmp1.children.push(tmp2);
  //               });
  //             }
  //           });
  //           tmp.children.push(tmp1);
  //         });
  //       }
  //     });
  //     this.temp3.push(tmp);
  //   });
  // }

  toogleModel(row: object) {
    row["close"] = !row["close"];
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

  getSelect() {
    this.moduleService.getModule(1000, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts).subscribe((data: Array<object>) => {
      this.listSelectModule = data["data"];
    });
  }

  getMenu() {
    this.Temp = [];
    //this.Temp2 = [];
    this.menuService.getMenu().subscribe((arr: Array<object>) => {
      //this.menu = arr["data"];
      this.addToArray(arr["data"], null, 0);
      // this.addToArray2(arr["data"], null, 0);
      // this.listSelectMenu = this.Temp;
      // this.addtoArray3();
      this.menu = this.Temp;
    });
  }

  selectEdit(row: object) {
    this.rowmenu = row;
    this.A0007_ID = row["A0007_ID"];
    this.modalMenu("update");
  }

  modalMenu(istype: string) {
    if (istype == "add") {
      this.A0007_ID = null;
    }
    this.me = { IDCha: null, ModuleID: null, thuThu: this.menu.length + 1, tinhTrang: true, istype: "add" };
    if (this.A0007_ID != null) {
      this.menuService.getMenuByID(this.rowmenu).subscribe((data) => {
        this.me = data[0];
        this.me["istype"] = "update";
      });
    }
    this.mMenu = true;
  }

  hideModal() {
    this.mMenu = false;
  }

  saveMenu() {
    var formData = new FormData();
    formData.append("menu", JSON.stringify(this.me));
    for (let i = 0; i < this.FileIcon.length; i++) {
      var file: File = this.FileIcon[i];
      formData.append('FileIcon', file);
    }

    for (let i = 0; i < this.FileIconDoc.length; i++) {
      var file: File = this.FileIconDoc[i];
      formData.append('FileIconDoc', file);
    }

    var istype = 1;
    if (this.me["istype"] != undefined && this.me["istype"] != "add") {
      istype = 2;
    }

    this.menuService.updateMenu(formData, istype).subscribe(
      (then) => {
        //this.Temp2 = [];
        //this.temp3 = [];
        this.getMenu();
        this.hideModal();
      }
    );
  }

  deleteMenu(row: object) {
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this.menuService.deleteMenu(row).subscribe((then) => {
            //this.Temp2 = [];
            //this.temp3 = [];
            this.getMenu();
          });
        }
      }).catch();
  }

}
