import { TestBed } from '@angular/core/testing';

import { MeikoTDUserService } from './meiko-td-user.service';

describe('MeikoTDUserService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDUserService = TestBed.get(MeikoTDUserService);
    expect(service).toBeTruthy();
  });
});
