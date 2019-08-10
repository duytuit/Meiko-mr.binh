import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DiaglogmodalComponent } from './diaglogmodal.component';
import { DiaglogNotifyModalComponent } from './diaglog-notify-modal.component';
import { DiaglogerrorComponent } from './diaglogerror.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  constructor(private modalService: NgbModal) { }

  public confirm(
    title: string,
    message: string,
    btnOkText: string = 'Đồng ý',
    btnCancelText: string = 'Trở lại',
    dialogSize: 'sm' | 'lg' = 'sm'): Promise<boolean> {
    const modalRef = this.modalService.open(DiaglogmodalComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
    modalRef.componentInstance.btnCancelText = btnCancelText;
    return modalRef.result;
  }

  public notify(
    title: string,
    message: string,
    btnOkText: string = 'Thoát',
    dialogSize: 'sm' | 'lg' = 'sm') {
    const modalRef = this.modalService.open(DiaglogNotifyModalComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
  }

  public error(
    title: string,
    message: string,
    btnOkText: string = 'Thoát',
    dialogSize: 'sm' | 'lg' = 'sm') {
    const modalRef = this.modalService.open(DiaglogerrorComponent, { size: dialogSize });
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnOkText = btnOkText;
  }
}
