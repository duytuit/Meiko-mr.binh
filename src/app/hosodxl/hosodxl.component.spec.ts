import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HosodxlComponent } from './hosodxl.component';

describe('HosodxlComponent', () => {
  let component: HosodxlComponent;
  let fixture: ComponentFixture<HosodxlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HosodxlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HosodxlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
