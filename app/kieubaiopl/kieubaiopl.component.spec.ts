import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KieubaioplComponent } from './kieubaiopl.component';

describe('KieubaioplComponent', () => {
  let component: KieubaioplComponent;
  let fixture: ComponentFixture<KieubaioplComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ KieubaioplComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KieubaioplComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
