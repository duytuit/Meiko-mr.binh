import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongviectrinhkyComponent } from './congviectrinhky.component';

describe('CongviectrinhkyComponent', () => {
  let component: CongviectrinhkyComponent;
  let fixture: ComponentFixture<CongviectrinhkyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongviectrinhkyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongviectrinhkyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
