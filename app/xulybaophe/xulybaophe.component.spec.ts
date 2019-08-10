import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XulybaopheComponent } from './xulybaophe.component';

describe('XulybaopheComponent', () => {
  let component: XulybaopheComponent;
  let fixture: ComponentFixture<XulybaopheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XulybaopheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XulybaopheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
