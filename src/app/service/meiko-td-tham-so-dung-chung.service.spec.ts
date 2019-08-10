import { TestBed } from '@angular/core/testing';

import { MeikoTDThamSoDungChungService } from './meiko-td-tham-so-dung-chung.service';

describe('MeikoTDThamSoDungChungService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDThamSoDungChungService = TestBed.get(MeikoTDThamSoDungChungService);
    expect(service).toBeTruthy();
  });
});
