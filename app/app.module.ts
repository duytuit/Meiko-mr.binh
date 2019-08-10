
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RolesComponent } from './roles/roles.component';
import { HomepageComponent } from './homepage/homepage.component';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { OrderModule } from 'ngx-order-pipe';
import { DiaglogmodalComponent } from './ui/diaglogmodal.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from './ui/modal.service';
import { UserComponent } from './user/user.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModuleComponent } from './module/module.component';
import { MenuComponent } from './menu/menu.component';
import { TreemenuComponent } from './menu/treemenu.component';
import { FilterPipe } from './ui/filter';
import { filterWhere } from './ui/filterWhere';
import { TreeTableModule } from 'primeng/components/treetable/treetable';
import { RoleUserComponent } from './role-user/role-user.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { AuthGuard } from './auth/auth.guard';
import { LanguageComponent } from './language/language.component';
import { MainComponentComponent } from './main-component/main-component.component';
import { LabelpageComponent } from './labelpage/labelpage.component';
import { UserListComponent } from './user-list/user-list.component';
import { TreeModule } from 'primeng/primeng';
import { CompanyDiagramComponent } from './company-diagram/company-diagram.component';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CrystalGalleryModule } from 'ngx-crystal-gallery';
import { DiaglogNotifyModalComponent } from './ui/diaglog-notify-modal.component';
import { ToastrModule } from 'ngx-toastr';
import { DiaglogerrorComponent } from './ui/diaglogerror.component';
import { ResetpassComponent } from './resetpass/resetpass.component';
import { DatePipe } from 'node_modules/@angular/common';
import { LoaicongviecComponent } from './loaicongviec/loaicongviec.component';
import { CongviecComponent } from './congviec/congviec.component';
import { NhomkyComponent } from './nhomky/nhomky.component';
import { stepskyComponent } from './steepky/steepky.component';
import { ConfignguoikyComponent } from './confignguoiky/confignguoiky.component';
import { QuytrinhComponent } from './quytrinh/quytrinh.component';
import { AdminsComponent } from './admins/admins.component';
import { HosoxlcComponent } from './hosoxlc/hosoxlc.component';
import { HosodxlComponent } from './hosodxl/hosodxl.component';
import { HosotoilapComponent } from './hosotoilap/hosotoilap.component';
import { HosodtgComponent } from './hosodtg/hosodtg.component';
import { HosotheodoiComponent } from './hosotheodoi/hosotheodoi.component';
import { CongvieccuatoiComponent } from './congvieccuatoi/congvieccuatoi.component';
import { CongviectrinhkyComponent } from './congviectrinhky/congviectrinhky.component';
import { CongviecdaguiComponent } from './congviecdagui/congviecdagui.component';
import { CongviechoanthanhComponent } from './congviechoanthanh/congviechoanthanh.component';
import { CongviecboquaComponent } from './congviecboqua/congviecboqua.component';
import { CongviecdaxoaComponent } from './congviecdaxoa/congviecdaxoa.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormComponent } from './form/form.component';
import { NoidungbaopheComponent } from './noidungbaophe/noidungbaophe.component';
import { PhongbanmapperComponent } from './phongbanmapper/phongbanmapper.component';
import { BaophecamComponent } from './baophecam/baophecam.component';
import { BaophepheduyetComponent } from './baophepheduyet/baophepheduyet.component';
import { BaophehoanthanhComponent } from './baophehoanthanh/baophehoanthanh.component';
import { BophanbaopheComponent } from './bophanbaophe/bophanbaophe.component';
import { BaophequytrinhComponent } from './baophequytrinh/baophequytrinh.component';
import { BaophedaguiComponent } from './baophedagui/baophedagui.component';
import { XulybaopheComponent } from './xulybaophe/xulybaophe.component';
import { BaophexulyhoanthanhComponent } from './baophexulyhoanthanh/baophexulyhoanthanh.component';
import { BaophechobophanxulyComponent } from './baophechobophanxuly/baophechobophanxuly.component';
import { Ng4LoadingSpinnerModule } from 'ng4-loading-spinner';
import { KieubaioplComponent } from './kieubaiopl/kieubaiopl.component';
import { ChitieubaioplComponent } from './chitieubaiopl/chitieubaiopl.component'; 
/** Module Meal*/

//import { DulieuquettheComponent } from './meals/dulieuquetthes/dulieuquetthes.component';
//import { KhongquettheComponent } from './meals/khongquetthes/khongquetthes.component';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    RolesComponent,
    HomepageComponent,
    DiaglogmodalComponent,
    UserComponent,
    ModuleComponent,
    MenuComponent,
    TreemenuComponent,
    FilterPipe,
    filterWhere,
    RoleUserComponent,
    LoginComponent,
    NavbarComponent,
    LanguageComponent,
    MainComponentComponent,
    LabelpageComponent,
    UserListComponent,
    CompanyDiagramComponent,
    DiaglogNotifyModalComponent,
    DiaglogerrorComponent,
    ResetpassComponent,
    LoaicongviecComponent,
    CongviecComponent,
    NhomkyComponent,
    stepskyComponent,
    ConfignguoikyComponent,
    QuytrinhComponent,
    AdminsComponent,
    HosoxlcComponent,
    HosodxlComponent,
    HosotoilapComponent,
    HosodtgComponent,
    HosotheodoiComponent,
    CongvieccuatoiComponent,
    CongviectrinhkyComponent,
    CongviecdaguiComponent,
    CongviechoanthanhComponent,
    CongviecboquaComponent,
    CongviecdaxoaComponent,
    FormComponent,
    NoidungbaopheComponent,
    PhongbanmapperComponent,
    BaophecamComponent,
    BaophepheduyetComponent,
    BaophehoanthanhComponent,
    BophanbaopheComponent,
    BaophequytrinhComponent,
    BaophedaguiComponent,
    XulybaopheComponent,
    BaophexulyhoanthanhComponent,
    BaophechobophanxulyComponent,
    KieubaioplComponent,
    ChitieubaioplComponent,
    /** Module Meal */
    //DulieuquettheComponent,
    //KhongquettheComponent
    

  ],
  imports: [ 
    //SharedModule,   
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,    
    FormsModule,
    NgbModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TreeTableModule,
    TreeModule,
    NgxMaterialTimepickerModule,
    BrowserAnimationsModule,
    CrystalGalleryModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      closeButton: true,
      progressBar: true,
      progressAnimation: 'increasing',
      positionClass: 'toast-bottom-right',
    }),
    OrderModule,
    NgSelectModule,
    Ng4LoadingSpinnerModule.forRoot() 
  ],
  bootstrap: [AppComponent],
  providers: [ModalService, AuthGuard, DatePipe],
  entryComponents: [DiaglogmodalComponent, DiaglogNotifyModalComponent, DiaglogerrorComponent]
})
export class AppModule { }
