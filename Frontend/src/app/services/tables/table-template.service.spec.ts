import { TestBed } from '@angular/core/testing';

import { TableTemplateService } from './table-template.service';

describe('TableTemplateService', () => {
  let service: TableTemplateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TableTemplateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
