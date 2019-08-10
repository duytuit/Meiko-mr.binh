import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophexulyhoanthanhComponent } from './baophexulyhoanthanh.component';

describe('BaophexulyhoanthanhComponent', () => {
  let component: BaophexulyhoanthanhComponent;
  let fixture: ComponentFixture<BaophexulyhoanthanhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophexulyhoanthanhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophexulyhoanthanhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
