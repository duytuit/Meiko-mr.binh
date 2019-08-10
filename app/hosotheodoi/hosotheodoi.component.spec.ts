import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HosotheodoiComponent } from './hosotheodoi.component';

describe('HosotheodoiComponent', () => {
  let component: HosotheodoiComponent;
  let fixture: ComponentFixture<HosotheodoiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HosotheodoiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HosotheodoiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
