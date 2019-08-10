import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
//import { FormsModule } from '@angular/forms';
import { OrderModule } from 'ngx-order-pipe';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TreeTableModule } from 'primeng/components/treetable/treetable';
import { TreeModule } from 'primeng/primeng';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CrystalGalleryModule } from 'ngx-crystal-gallery';
import { ToastrModule } from 'ngx-toastr';
import { DatePipe } from 'node_modules/@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { Ng4LoadingSpinnerModule } from 'ng4-loading-spinner';
//import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { getDutchPaginatorIntl } from './dutch-paginator-intl';
import {CdkTableModule} from '@angular/cdk/table';
import {
  MatAutocompleteModule,
  MatBadgeModule,
  MatBottomSheetModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatTreeModule,
  MatPaginatorIntl
} from '@angular/material';



@NgModule({
  imports: [
    RouterModule,
    CommonModule, 
  ],
  declarations: [    
  ],
  exports: [  
    OrderModule,
    NgbModule,
    BsDatepickerModule,
    TreeTableModule,
    TreeModule,
    NgxMaterialTimepickerModule,    
    CrystalGalleryModule,    
    NgSelectModule,  
    Ng4LoadingSpinnerModule,
    DatePipe,
    CdkTableModule, 
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
  ],
  providers: [
    { provide: MatPaginatorIntl, useValue: getDutchPaginatorIntl() }
  ], 
  
})
export class SharedModule {   
}