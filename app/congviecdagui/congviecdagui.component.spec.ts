import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongviecdaguiComponent } from './congviecdagui.component';

describe('CongviecdaguiComponent', () => {
  let component: CongviecdaguiComponent;
  let fixture: ComponentFixture<CongviecdaguiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongviecdaguiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongviecdaguiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
