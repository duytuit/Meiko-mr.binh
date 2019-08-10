import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoaicongviecComponent } from './loaicongviec.component';

describe('LoaicongviecComponent', () => {
  let component: LoaicongviecComponent;
  let fixture: ComponentFixture<LoaicongviecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoaicongviecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoaicongviecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
