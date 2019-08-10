import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RolesComponent } from './roles/roles.component';
import { UserComponent } from './user/user.component';
import { HomepageComponent } from './homepage/homepage.component';
import { ModuleComponent } from './module/module.component';
import { MenuComponent } from './menu/menu.component';
import { RoleUserComponent } from './role-user/role-user.component';
import { LoginComponent } from './login/login.component';
import { LanguageComponent } from './language/language.component';
import { LabelpageComponent } from './labelpage/labelpage.component';
import { AuthGuard } from './auth/auth.guard';
import { MainComponentComponent } from './main-component/main-component.component';
import { CompanyDiagramComponent } from './company-diagram/company-diagram.component';
import { ResetpassComponent } from './resetpass/resetpass.component';
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
import { FormComponent } from './form/form.component';
import { NoidungbaopheComponent } from './noidungbaophe/noidungbaophe.component';
import { PhongbanmapperComponent } from './phongbanmapper/phongbanmapper.component';
import { BaophecamComponent } from './baophecam/baophecam.component';
import { BaophepheduyetComponent } from './baophepheduyet/baophepheduyet.component';
import { BaophedaguiComponent } from './baophedagui/baophedagui.component';
import { BaophehoanthanhComponent } from './baophehoanthanh/baophehoanthanh.component';
import { XulybaopheComponent } from './xulybaophe/xulybaophe.component';
import { BophanbaopheComponent } from './bophanbaophe/bophanbaophe.component';
import { BaophechobophanxulyComponent } from './baophechobophanxuly/baophechobophanxuly.component';
import { BaophequytrinhComponent } from './baophequytrinh/baophequytrinh.component';
import { BaophexulyhoanthanhComponent } from './baophexulyhoanthanh/baophexulyhoanthanh.component';
import { KieubaioplComponent } from './kieubaiopl/kieubaiopl.component';
import { ChitieubaioplComponent } from './chitieubaiopl/chitieubaiopl.component';

const routes: Routes = [

  //Main layout no layout
  { path: 'main', component: MainComponentComponent, canActivate: [AuthGuard] },
  { path: '', component: MainComponentComponent, pathMatch: 'full' },
  {
    path: '',
    component: AdminsComponent,
    children: [
      { path: 'roles', component: RolesComponent },
      { path: 'user', component: UserComponent },
      { path: 'module', component: ModuleComponent },
      { path: 'menu', component: MenuComponent },
      { path: 'rolemenu', component: RoleUserComponent },
      { path: 'language', component: LanguageComponent },
      { path: 'labelweb', component: LabelpageComponent },
      { path: 'companydiagram', component: CompanyDiagramComponent },
      { path: 'loaicongviec', component: LoaicongviecComponent },
      { path: 'congviec', component: CongviecComponent },
      { path: 'nhomnguoiky', component: NhomkyComponent },
      { path: 'stepsky', component: stepskyComponent },
      { path: 'nguoiky', component: ConfignguoikyComponent },
      { path: 'quytrinh', component: QuytrinhComponent },
      { path: 'hosoxulychinh', component: HosoxlcComponent },
      { path: 'hosodongxuly', component: HosodxlComponent },
      { path: 'hosotoilap', component: HosotoilapComponent },
      { path: 'hosoduoctheodoi', component: HosodtgComponent },
      { path: 'theodoihoso', component: HosotheodoiComponent },
      { path: 'congvieccuatoi', component: CongvieccuatoiComponent },
      { path: 'congviectrinhky', component: CongviectrinhkyComponent },
      { path: 'congviecdagui', component: CongviecdaguiComponent },
      { path: 'congviechoanthanh', component: CongviechoanthanhComponent },
      { path: 'congviecbiboqua', component: CongviecboquaComponent },
      { path: 'congviecdaxoa', component: CongviecdaxoaComponent },
      { path: 'form', component: FormComponent },
      { path: 'danhmucbaophe', component: NoidungbaopheComponent },
      { path: 'phongbanbaophe', component: PhongbanmapperComponent },
      { path: 'baophecam', component: BaophecamComponent },
      { path: 'pheduyetbaophe', component: BaophepheduyetComponent },
      { path: 'baophedagui', component: BaophedaguiComponent },
      { path: 'baophehoanthanh', component: BaophehoanthanhComponent },
      { path: 'xulybaophe', component: XulybaopheComponent },
      { path: 'baophexulyhoanthanh', component: BaophexulyhoanthanhComponent },
      { path: 'baophechobophanxuly', component: BaophechobophanxulyComponent }, 
      { path: 'bophanbaophe', component: BophanbaopheComponent },
      { path: 'quytrinhbaophe', component: BaophequytrinhComponent },
      { path: 'kieubaiopl', component: KieubaioplComponent },
      { path: 'chitieubaiopl', component: ChitieubaioplComponent },
    ],
    canActivate: [AuthGuard],
  },
  //no layout routes
  { path: 'login', component: LoginComponent },
  { path: 'doimatkhau', component: ResetpassComponent },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
