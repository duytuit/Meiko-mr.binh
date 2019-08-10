import { TestBed } from '@angular/core/testing';

import { CompanyDiagramService } from './company-diagram.service';

describe('CompanyDiagramService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CompanyDiagramService = TestBed.get(CompanyDiagramService);
    expect(service).toBeTruthy();
  });
});
