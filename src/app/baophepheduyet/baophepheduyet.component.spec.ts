import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophepheduyetComponent } from './baophepheduyet.component';

describe('BaophepheduyetComponent', () => {
  let component: BaophepheduyetComponent;
  let fixture: ComponentFixture<BaophepheduyetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophepheduyetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophepheduyetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
