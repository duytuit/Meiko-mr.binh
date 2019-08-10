import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophedaguiComponent } from './baophedagui.component';

describe('BaophedaguiComponent', () => {
  let component: BaophedaguiComponent;
  let fixture: ComponentFixture<BaophedaguiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophedaguiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophedaguiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
