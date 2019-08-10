// Angular
import { CommonModule} from '@angular/common';
//import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { DulieuquettheComponent } from './dulieuquetthes/dulieuquetthes.component';
import { KhongquettheComponent } from './khongquetthes/khongquetthes.component';
import { HttpClientModule } from '@angular/common/http'
import { SharedModule } from '../../shared/shared.module';
import { QuanlybaocomComponent } from './quanlybaocoms/quanlybaocoms.component';
//user Routing
import { MealRoutingModule } from './meals-routing.module';



@NgModule({
  imports: [
    MealRoutingModule,      
    ReactiveFormsModule,    
    FormsModule,
    HttpClientModule,
    SharedModule,    
    CommonModule   
  ],
  declarations: [    
    DulieuquettheComponent, 
    KhongquettheComponent,
    QuanlybaocomComponent
  ],
  entryComponents: [    
  ],
})
export class MealsModule { }