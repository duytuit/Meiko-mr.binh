import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiaglogNotifyModalComponent } from './diaglog-notify-modal.component';

describe('DiaglogNotifyModalComponent', () => {
  let component: DiaglogNotifyModalComponent;
  let fixture: ComponentFixture<DiaglogNotifyModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiaglogNotifyModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiaglogNotifyModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
