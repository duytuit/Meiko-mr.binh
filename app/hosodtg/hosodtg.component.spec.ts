import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HosodtgComponent } from './hosodtg.component';

describe('HosodtgComponent', () => {
  let component: HosodtgComponent;
  let fixture: ComponentFixture<HosodtgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HosodtgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HosodtgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
