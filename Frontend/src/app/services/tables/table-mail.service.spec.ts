import { TestBed } from '@angular/core/testing';

import { TableMailService } from './table-mail.service';

describe('TableMailService', () => {
  let service: TableMailService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableMailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
