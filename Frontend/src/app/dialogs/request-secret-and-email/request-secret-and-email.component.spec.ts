import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestSecretAndEmailComponent } from './request-secret-and-email.component';

describe('RequestSecretAndEmailComponent', () => {
  let component: RequestSecretAndEmailComponent;
  let fixture: ComponentFixture<RequestSecretAndEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RequestSecretAndEmailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestSecretAndEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
