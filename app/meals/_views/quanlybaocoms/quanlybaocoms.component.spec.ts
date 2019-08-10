import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuanlybaocomComponent } from './quanlybaocoms.component';

describe('QuanlybaocomComponent', () => {
  let component: QuanlybaocomComponent;
  let fixture: ComponentFixture<QuanlybaocomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuanlybaocomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuanlybaocomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
