import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongvieccuatoiComponent } from './congvieccuatoi.component';

describe('CongvieccuatoiComponent', () => {
  let component: CongvieccuatoiComponent;
  let fixture: ComponentFixture<CongvieccuatoiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongvieccuatoiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongvieccuatoiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
