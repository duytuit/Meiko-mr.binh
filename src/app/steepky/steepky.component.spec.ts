import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SteepkyComponent } from './steepky.component';

describe('SteepkyComponent', () => {
  let component: SteepkyComponent;
  let fixture: ComponentFixture<SteepkyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SteepkyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SteepkyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
