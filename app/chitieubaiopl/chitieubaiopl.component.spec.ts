import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChitieubaioplComponent } from './chitieubaiopl.component';

describe('ChitieubaioplComponent', () => {
  let component: ChitieubaioplComponent;
  let fixture: ComponentFixture<ChitieubaioplComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChitieubaioplComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChitieubaioplComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
