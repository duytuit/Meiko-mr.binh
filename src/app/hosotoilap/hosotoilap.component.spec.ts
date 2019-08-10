import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HosotoilapComponent } from './hosotoilap.component';

describe('HosotoilapComponent', () => {
  let component: HosotoilapComponent;
  let fixture: ComponentFixture<HosotoilapComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HosotoilapComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HosotoilapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
