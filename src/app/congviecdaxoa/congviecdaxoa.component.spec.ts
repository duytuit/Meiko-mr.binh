import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongviecdaxoaComponent } from './congviecdaxoa.component';

describe('CongviecdaxoaComponent', () => {
  let component: CongviecdaxoaComponent;
  let fixture: ComponentFixture<CongviecdaxoaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongviecdaxoaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongviecdaxoaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
