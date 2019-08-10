import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DulieuquettheComponent } from './dulieuquetthes.component';

describe('DulieuquettheComponent', () => {
  let component: DulieuquettheComponent;
  let fixture: ComponentFixture<DulieuquettheComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DulieuquettheComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DulieuquettheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
