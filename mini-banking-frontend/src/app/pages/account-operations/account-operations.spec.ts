import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountOperations } from './account-operations';

describe('AccountOperations', () => {
  let component: AccountOperations;
  let fixture: ComponentFixture<AccountOperations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccountOperations],
    }).compileComponents();

    fixture = TestBed.createComponent(AccountOperations);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
