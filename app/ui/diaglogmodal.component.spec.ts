import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiaglogmodalComponent } from './diaglogmodal.component';

describe('DiaglogmodalComponent', () => {
  let component: DiaglogmodalComponent;
  let fixture: ComponentFixture<DiaglogmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiaglogmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiaglogmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
