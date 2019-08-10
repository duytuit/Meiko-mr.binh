import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongviechoanthanhComponent } from './congviechoanthanh.component';

describe('CongviechoanthanhComponent', () => {
  let component: CongviechoanthanhComponent;
  let fixture: ComponentFixture<CongviechoanthanhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongviechoanthanhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongviechoanthanhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
