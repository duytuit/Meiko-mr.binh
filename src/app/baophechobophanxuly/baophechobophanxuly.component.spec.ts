import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophechobophanxulyComponent } from './baophechobophanxuly.component';

describe('BaophechobophanxulyComponent', () => {
  let component: BaophechobophanxulyComponent;
  let fixture: ComponentFixture<BaophechobophanxulyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophechobophanxulyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophechobophanxulyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
