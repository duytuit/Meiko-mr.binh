import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { appPublic } from '../appPublic';

export interface TreeNode {
  label?: string;
  data?: any;
  icon?: any;
  expandedIcon?: any;
  collapsedIcon?: any;
  children?: TreeNode[];
  leaf?: boolean;
  expanded?: boolean;
  type?: string;
  parent?: TreeNode;
  partialSelected?: boolean;
  styleClass?: string;
  draggable?: boolean;
  droppable?: boolean;
  selectable?: boolean;
}


@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  mUserSelect: boolean;
  ListUser = [];
  ListPhongBan = [];
  ListRole = [];
  PhongBanList = [];
  files: TreeNode[];
  arrfilter = [];
  Temp2 = [];
  convert = {};
  constructor() { }
  @Output('choiceUser')
  choiceUser = new EventEmitter<Array<string>>();

  ngOnInit() {
    setTimeout(() => {
      if(this.ListUser.length == 0){
        this.Temp2 = [];
        this.ListUser = appPublic.listUser;
        this.PhongBanList = appPublic.listPhongBan;
        this.ListRole = appPublic.listChucDanh;
        this.getPhongBan();
      } 
    }, 1000);
  }

  getPhongBan() {
    this.addToArray2(this.PhongBanList, null, 0);
    this.ListPhongBan = this.Temp2;
  }

  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.idcha == id);
    if (filter.length > 0) {
      for (let index = 0; index < filter.length; index++) {
        var children = this.PhongBanList.filter(x => x.idcha == filter[index].id);
        var check = this.arrfilter.find(x => x.id == filter[index].id);
        if ((children.length > 0 || filter[index]["idcha"] == null)) {
          for (let i = 0; i < children.length; i++) {
            this.arrfilter.push(children[i]);
          }
          filter[index].children = children;
          if (check == null) {
            this.Temp2.push(filter[index]);
          }
          this.addToArray2(array, filter[index].id, lv);
        }
      }
    }
  }

  SelectModel(row: object) {
    var id = row["id"];
    this.ListUser = appPublic.listUser.filter(x => x.id == id);
  }

  refershModel() {
    this.ListUser = appPublic.listUser;
    this.convert = {};
  }

  hideModal() {
    this.mUserSelect = false;
  }

  checkAllUST() {

  }

  setCD(cd) {
    if (cd == null)
      this.convert["A0001_ID"] = "";
    else
      this.convert["A0001_ID"] = cd.A0001_ID;
  }

  ChoiceUser() {
    var Arr = [];
    for (let i = 0; i < this.ListUser.length; i++) {
      if (this.ListUser[i].users.length > 0) {
        var Umodels = this.ListUser[i].users;
        for (let j = 0; j < Umodels.length; j++) {
          if (Umodels[j].isCheck == true) {
            Arr.push(Umodels[j]);
          }
        }
      }
    }
    this.choiceUser.emit(Arr);
  }

}
