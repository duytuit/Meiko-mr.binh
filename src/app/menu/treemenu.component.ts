import { Component, Input } from '@angular/core';

@Component({
  selector: 'treemenu',
  templateUrl: './treemenu.component.html',
  styleUrls: ['./treemenu.component.css']
})
export class TreemenuComponent {
  @Input() parentId: string;
  @Input() data: any[];


  removeItemNotParent = (data, parentId) => {
    return data.filter(item => item.IDCha != parentId)
  }

  toogleModel(row:object){
    row["close"] = !row["close"];
  }
}
