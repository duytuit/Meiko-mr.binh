import { TestBed } from '@angular/core/testing';

import { WorkCAMService } from './work-cam.service';

describe('WorkCAMService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkCAMService = TestBed.get(WorkCAMService);
    expect(service).toBeTruthy();
  });
});
