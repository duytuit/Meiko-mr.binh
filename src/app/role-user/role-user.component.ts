import { Component, OnInit } from '@angular/core';
import { RoleUserService } from '../service/role-user.service';
import { MeikoTDRolesService } from '../service/meiko-td-roles.service';
import { ModalService } from '../ui/modal.service';
@Component({
  selector: 'app-role-user',
  templateUrl: './role-user.component.html',
  styleUrls: ['./role-user.component.css']
})
export class RoleUserComponent implements OnInit {
  private opition = { total: 0, totalpage: 1, p: 1, pz: 20, sort: "", ob: "", s: "", sts: "" };
  private Rolelist = [];
  private Rolemenu = [];
  A0001_ID: string;
  constructor(private roleservice: MeikoTDRolesService, private rolemenuservice: RoleUserService, private dg: ModalService) { }

  ngOnInit() {
    this.getRole();
    //this.getRoleMenu();
  }
  Temp2 = [];
  arrfilter = [];
  addToArray2(array, id, lv) {
    var filter = array.filter(x => x.IDCha == id);
    if (filter.length > 0) {
      for (let index = 0; index < filter.length; index++) {
        var children = this.Rolemenu.filter(x => x.IDCha == filter[index].A0007_ID);
        var check = this.arrfilter.find(x => x.A0007_ID == filter[index].A0007_ID);
        if ((children.length > 0 || filter[index]["IDCha"] == null)) {
          var children2 = [];
          for (let i = 0; i < children.length; i++) {
            this.arrfilter.push(children[i]);
          }
          filter[index].children = children;
          if (check == null) {
            this.Temp2.push(filter[index]);
          }
          this.addToArray2(array, filter[index].A0007_ID, lv);
        }
      }
    }
  }

  Temp3 = [];
  addtoArray3() {
    this.Temp2.forEach(element => {
      let tmp: any = {
        data: {},
        children: []
      };
      Object.keys(element).forEach(prop => {
        if (prop != 'children') {
          tmp.data[prop] = element[prop];
        } else {
          element[prop].forEach(c1 => {
            let tmp1: any = {
              data: {},
              children: []
            };
            Object.keys(c1).forEach(prop1 => {
              if (prop1 != 'children') {
                tmp1.data[prop1] = c1[prop1];
              } else {
                c1[prop1].forEach(c2 => {
                  let tmp2: any = {
                    data: {},
                    children: []
                  };
                  Object.keys(c2).forEach(prop2 => {
                    if (prop2 != 'children') {
                      tmp2.data[prop2] = c2[prop2];
                    } else {
                      c2[prop2].forEach(c3 => {
                        tmp2.children.push({ data: c3 });
                      });
                    }
                  });
                  tmp1.children.push(tmp2);
                });
              }
            });
            tmp.children.push(tmp1);
          });
        }
      });
      this.Temp3.push(tmp);
    });
  }

  getRole() {
    this.roleservice.getRoles(1000, this.opition.p, this.opition.sort, this.opition.ob, this.opition.s, this.opition.sts,null).subscribe((data) => {
      this.Rolelist = data["data"];
      if (this.Rolelist.length > 0) {
        this.A0001_ID = this.Rolelist[0].A0001_ID;
        this.Rolelist[0]["isSelected"] = true
        this.getRoleMenu();
      }
    });
  }

  getRoleMenu() {
    this.Temp2 = [];
    this.Temp3 = [];
    this.rolemenuservice.getRoleUser(this.A0001_ID, "").subscribe((data: Array<object>) => {
      this.Rolemenu = data;
      this.addToArray2(data, null, 0);
      this.addtoArray3();
      this.Rolemenu = this.Temp3;
      //console.log(this.Rolemenu);
    });
  }

  View(row: object) {
    this.A0001_ID = row["A0001_ID"];
    this.Rolelist.forEach((a) => {
      a.isSelected = false;
    });
    row["isSelected"] = true;
    this.getRoleMenu();
  }

  SubmitRole(type) {
    if (type == 1) {
      this.Submit(1);
    } else {
      this.dg.confirm('Confirm', 'Bạn có chắc chắn muốn lưu và áp dụng phân quyền cho tất cả User?')
        .then((confirmed) => {
          if (confirmed) {
            this.Submit(2);
          }
        }).catch();
    }
  }

  Submit(type) {
    var arr = [];
    for (let i = 0; i < this.Rolemenu.length; i++) {
      var childrens = this.Rolemenu[i].children;
      arr.push(this.Rolemenu[i]["data"]);
      if (childrens && childrens.length > 0) {
        for (let j = 0; j < childrens.length; j++) {
          arr.push(childrens[j]["data"]);
          var childrenss = childrens[j].children;
          if (childrenss && childrenss.length > 0) {
            for (let k = 0; k < childrenss.length; k++) {
              arr.push(childrenss[k]["data"]);
              var childrensss = childrenss[k].children;
              if (childrensss && childrensss.length > 0) {
                for (let k = 0; k < childrensss.length; k++) {
                  arr.push(childrensss[k]["data"]);
                }
              }
            }
          }
        }
      }
    }
    this.rolemenuservice.updateRoleMenu(arr, this.A0001_ID, type).subscribe((then) => {
      this.getRoleMenu();
    });
  }
}
