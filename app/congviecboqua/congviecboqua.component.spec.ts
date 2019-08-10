import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongviecboquaComponent } from './congviecboqua.component';

describe('CongviecboquaComponent', () => {
  let component: CongviecboquaComponent;
  let fixture: ComponentFixture<CongviecboquaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongviecboquaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongviecboquaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
