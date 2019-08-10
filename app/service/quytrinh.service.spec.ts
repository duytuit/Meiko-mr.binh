import { TestBed } from '@angular/core/testing';

import { QuytrinhService } from './quytrinh.service';

describe('QuytrinhService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: QuytrinhService = TestBed.get(QuytrinhService);
    expect(service).toBeTruthy();
  });
});
