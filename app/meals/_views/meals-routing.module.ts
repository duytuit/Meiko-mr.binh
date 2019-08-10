import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DulieuquettheComponent } from './dulieuquetthes/dulieuquetthes.component';
import { KhongquettheComponent } from './khongquetthes/khongquetthes.component';
import { QuanlybaocomComponent } from './quanlybaocoms/quanlybaocoms.component';


const routes: Routes = [  
  {
    path: 'dulieuquetthe',
    component: DulieuquettheComponent,
    data: {
      title: 'Dữ liệu quẹt thẻ'
    }
  },
  {
    path: 'khongquetthe',
    component: KhongquettheComponent,
    data: {
      title: 'Không quẹt thẻ'
    }
  },
  {
    path: 'quanlybaocom',
    component: QuanlybaocomComponent,
    data: {
      title: 'Quản lý báo cơm'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MealRoutingModule {}