import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaophecamComponent } from './baophecam.component';

describe('BaophecamComponent', () => {
  let component: BaophecamComponent;
  let fixture: ComponentFixture<BaophecamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaophecamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaophecamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
