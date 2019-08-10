import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BophanbaopheComponent } from './bophanbaophe.component';

describe('BophanbaopheComponent', () => {
  let component: BophanbaopheComponent;
  let fixture: ComponentFixture<BophanbaopheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BophanbaopheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BophanbaopheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
