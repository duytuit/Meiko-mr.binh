import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfignguoikyComponent } from './confignguoiky.component';

describe('ConfignguoikyComponent', () => {
  let component: ConfignguoikyComponent;
  let fixture: ComponentFixture<ConfignguoikyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfignguoikyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfignguoikyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
