import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LabelpageComponent } from './labelpage.component';

describe('LabelpageComponent', () => {
  let component: LabelpageComponent;
  let fixture: ComponentFixture<LabelpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LabelpageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LabelpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
