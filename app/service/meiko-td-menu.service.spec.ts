import { TestBed } from '@angular/core/testing';

import { MeikoTDMenuService } from './meiko-td-menu.service';

describe('MeikoTDMenuService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDMenuService = TestBed.get(MeikoTDMenuService);
    expect(service).toBeTruthy();
  });
});
