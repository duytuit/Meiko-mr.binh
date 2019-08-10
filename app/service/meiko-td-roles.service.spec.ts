import { TestBed } from '@angular/core/testing';

import { MeikoTDRolesService } from './meiko-td-roles.service';

describe('MeikoTDRolesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDRolesService = TestBed.get(MeikoTDRolesService);
    expect(service).toBeTruthy();
  });
});
