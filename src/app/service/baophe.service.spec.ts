import { TestBed } from '@angular/core/testing';

import { BaopheService } from './baophe.service';

describe('BaopheService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BaopheService = TestBed.get(BaopheService);
    expect(service).toBeTruthy();
  });
});
