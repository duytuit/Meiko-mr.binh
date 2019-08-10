import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KhongquettheComponent } from './khongquetthes.component';

describe('KhongquettheComponent', () => {
  let component: KhongquettheComponent;
  let fixture: ComponentFixture<KhongquettheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KhongquettheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KhongquettheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
