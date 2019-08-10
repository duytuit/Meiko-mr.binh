import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophequytrinhComponent } from './baophequytrinh.component';

describe('BaophequytrinhComponent', () => {
  let component: BaophequytrinhComponent;
  let fixture: ComponentFixture<BaophequytrinhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophequytrinhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophequytrinhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
