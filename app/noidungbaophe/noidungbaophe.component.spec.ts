import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoidungbaopheComponent } from './noidungbaophe.component';

describe('NoidungbaopheComponent', () => {
  let component: NoidungbaopheComponent;
  let fixture: ComponentFixture<NoidungbaopheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoidungbaopheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoidungbaopheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
