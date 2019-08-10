import { TestBed } from '@angular/core/testing';

import { MyworkService } from './mywork.service';

describe('MyworkService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MyworkService = TestBed.get(MyworkService);
    expect(service).toBeTruthy();
  });
});
