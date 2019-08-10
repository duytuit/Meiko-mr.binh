import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiaglogerrorComponent } from './diaglogerror.component';

describe('DiaglogerrorComponent', () => {
  let component: DiaglogerrorComponent;
  let fixture: ComponentFixture<DiaglogerrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiaglogerrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiaglogerrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
