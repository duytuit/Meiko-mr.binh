import { TestBed } from '@angular/core/testing';

import { MeikoTDModuleService } from './meiko-td-module.service';

describe('MeikoTDModuleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeikoTDModuleService = TestBed.get(MeikoTDModuleService);
    expect(service).toBeTruthy();
  });
});
