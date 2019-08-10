import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyDiagramComponent } from './company-diagram.component';

describe('CompanyDiagramComponent', () => {
  let component: CompanyDiagramComponent;
  let fixture: ComponentFixture<CompanyDiagramComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanyDiagramComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyDiagramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
