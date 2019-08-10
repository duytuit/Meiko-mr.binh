import { Component, OnInit,ViewChild,ElementRef } from "@angular/core";
import { FormBuilder, FormGroup, Validators,  NgForm,  FormControl } from "@angular/forms";
import {MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { MyworkService } from "../../../service/mywork.service";
import { QuytrinhService } from "../../../service/quytrinh.service";
import { NgSelectModule, NgOption } from "@ng-select/ng-select";
import { ToastrService } from "ngx-toastr";
import { appPublic } from "../../../appPublic";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ModalService } from "../../../ui/modal.service";
import { MatPaginator, MatSort, MatTableDataSource, MatDialog } from "@angular/material";
import { DuLieuQuetTheService, BuaAnService } from "../../_service";

import * as _moment from 'moment';
const moment =  _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'L',
  },
  display: {
    dateInput: 'L',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'L',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Component({
  selector: "app-quanlybaocoms",
  templateUrl: "./quanlybaocoms.component.html",
  styleUrls: ["./quanlybaocoms.component.scss"],
  providers:[
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
    DuLieuQuetTheService,BuaAnService]
})
export class QuanlybaocomComponent implements OnInit {
  constructor(
    private qtService: QuytrinhService,
    private workService: MyworkService,
    private toastr: ToastrService,
    private dg: ModalService,
    private _checkinService: DuLieuQuetTheService,
    private _buaanService: BuaAnService
  ) {}

  caLamViec = [{ma: 0, ten: 'Ngày'},{ma: 1, ten: 'Đêm'}];
  displayedColumns = ["manhanvien","hoten","gioquet","phong","ban","congdoan","mamay","maca","actions"];
  dataSource: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;   

  maIdFilter = new FormControl();
  phongFilter = new FormControl();
  filteredValues = { manhanvien:'',phong:''};
  
  globalFilter = '';

  _date = new FormControl(new Date());
  _ca = new FormControl(new String);
  currCa: any;
  ngOnInit() {
    this.currCa = 0;  
    this._ca.setValue(0) 
    this.getDataCheckIn()
    this.getBuaAnAll()
  }
  _buaan: any
  getBuaAnAll(){
    this._buaanService.getBuaAnList().subscribe((data: Array<Object>) => {
      this._buaan = data
    })
  }

  data_checkin: any;
  color = "primary";
  mode = "indeterminate";
  value = 50;
  displayProgressSpinner = false;
  spinnerWithoutBackdrop = false;
  getDataCheckIn() {    
    this.data_checkin = [];
    this.displayProgressSpinner = true;
    let date_select_moment = moment(this._date.value);
    let _date  = date_select_moment.format("MM-DD-YYYY")    
    this._checkinService.GetDataCheckInByIOAndDate(0, this._ca.value, _date).subscribe((data: Array<Object>) => {        
        this.data_checkin = data
        this.dataSource = new MatTableDataSource(this.data_checkin);
          setTimeout(() => {
            this.dataSource;
            this.displayProgressSpinner = false;
          }, 500);

          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.dataSource.filterPredicate = (
            data: any,
            filtersJson: string
          ) => {
            const matchFilter = [];
            const filters = JSON.parse(filtersJson);

            filters.forEach(filter => {
              const val = data[filter.id] === null ? "" : data[filter.id];
              matchFilter.push(
                val.toLowerCase().includes(filter.value.toLowerCase())
              );
            });
            return matchFilter.every(Boolean);
          };

          this.maIdFilter.valueChanges.subscribe((maIdFilterValue) => {
            this.filteredValues['manhanvien'] = maIdFilterValue;
            this.dataSource.filter = JSON.stringify(this.filteredValues);
          });

          this.phongFilter.valueChanges.subscribe((phongFilterValue) => {
            this.filteredValues['phong'] = phongFilterValue;
            this.dataSource.filter = JSON.stringify(this.filteredValues);
          });

          this.dataSource.filterPredicate = this.customFilterPredicate();
      });
  }

  customFilterPredicate() {
    const myFilterPredicate = (data: any, filter: string): boolean => {
      var globalMatch = !this.globalFilter;

      if (this.globalFilter) {
        // search all text fields
        globalMatch = data.manhanvien.toString().trim().toLowerCase().indexOf(this.globalFilter.toLowerCase()) !== -1;
      }

      if (!globalMatch) {
        return;
      }
      let searchString = JSON.parse(filter);
      return data.manhanvien.toString().trim().indexOf(searchString.manhanvien) !== -1 &&
      data.phong.toString().trim().toLowerCase().indexOf(searchString.phong.toLowerCase()) !== -1;
    }
    return myFilterPredicate;
  }

  AsyncEmpFromAsoft(){
    debugger
    this._checkinService.AsyncEmpFromAsoft().subscribe((data:any)=>{
      this.toastr.success('Đang thực hiện đồng bộ...', 'Thông báo');
      //console.log('aa')
      //this.dg.notify("Thông báo", "Đồng bộ dữ liệu thành công!");
    });
  }

  AsyncDataCheckin(){ 
    let date_select_moment = moment(this._date.value);
    let _date  = date_select_moment.format("MM-DD-YYYY")     
    this._checkinService.AsyncFromAsoft(_date,this._ca.value).subscribe((data: Array<Object>) =>{
      this.toastr.success('Đang thực hiện đồng bộ...', 'Thông báo');
      this.getDataCheckIn();
    });       
  }
  
  deleteItem(id: string) {    
    this.dg.confirm('Confirm', 'Bạn có muốn xóa những mục đã chọn ?')
      .then((confirmed) => {
        if (confirmed) {
          this._checkinService.deleteDuLieuQuetThe(id).subscribe((then) =>{
            this.toastr.success('Đang thực hiện xóa dữ liệu...', 'Thông báo');
            this.getDataCheckIn();
          })
        }
      }).catch();
  }  
}
