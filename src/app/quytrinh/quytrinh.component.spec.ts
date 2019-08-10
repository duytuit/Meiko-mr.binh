import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuytrinhComponent } from './quytrinh.component';

describe('QuytrinhComponent', () => {
  let component: QuytrinhComponent;
  let fixture: ComponentFixture<QuytrinhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuytrinhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuytrinhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
