import { Component, OnInit } from '@angular/core';
import { MeikoTDUserService } from '../service/meiko-td-user.service'
import { ModalService } from '../ui/modal.service'
import { from } from 'rxjs';
@Component({
  selector: 'app-resetpass',
  templateUrl: './resetpass.component.html',
  styleUrls: ['./resetpass.component.css']
})
export class ResetpassComponent implements OnInit {

  constructor(private uService: MeikoTDUserService, private dg: ModalService) { }
  resetpass = {};
  ngOnInit() {
  }

  ResetPassword() {
    if (this.resetpass["userName"] == "") {
      this.dg.error("Thông báo", "Bạn chưa nhập Tên Tài khoản");
      return false;
    }
    if (this.resetpass["CMTND"] == "") {
      this.dg.error("Thông báo", "Bạn chưa nhập số chứng minh thư nhân dân");
      return false;
    }

    this.uService.ResetPassword(this.resetpass).subscribe((then: Array<object>) => {
      if (then["error"] == 0) {
        this.dg.notify("Thông báo", "Bạn đã gửi yêu cầu đổi mật khẩu thành công !");
        this.resetpass = {};
      } else if (then["error"] == 1) {
        this.dg.error("Thông báo", "Yêu cầu đổi mật khẩu của bạn đã tồn tại !");
        this.resetpass = {};
      } else if (then["error"] == 2) {
        this.dg.error("Thông báo", "Thông tin tài khoản của bạn không chính xác, vui lòng kiểm tra lại");
        return false;
      }
    });
  }
}
