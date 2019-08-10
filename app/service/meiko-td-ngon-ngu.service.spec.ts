import { TestBed } from '@angular/core/testing';

import { MeikoTDNgonNguService } from './meiko-td-ngon-ngu.service';

describe('MeikoTDNgonNguService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDNgonNguService = TestBed.get(MeikoTDNgonNguService);
    expect(service).toBeTruthy();
  });
});
