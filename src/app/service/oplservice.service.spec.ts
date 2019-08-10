import { TestBed } from '@angular/core/testing';

import { OplserviceService } from './oplservice.service';

describe('OplserviceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OplserviceService = TestBed.get(OplserviceService);
    expect(service).toBeTruthy();
  });
});
