import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HosoxlcComponent } from './hosoxlc.component';

describe('HosoxlcComponent', () => {
  let component: HosoxlcComponent;
  let fixture: ComponentFixture<HosoxlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HosoxlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HosoxlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
