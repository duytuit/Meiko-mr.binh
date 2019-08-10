import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophehoanthanhComponent } from './baophehoanthanh.component';

describe('BaophehoanthanhComponent', () => {
  let component: BaophehoanthanhComponent;
  let fixture: ComponentFixture<BaophehoanthanhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophehoanthanhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophehoanthanhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
