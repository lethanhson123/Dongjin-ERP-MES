import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputHistoryComponent } from './invoice-input-history.component';

describe('InvoiceInputHistoryComponent', () => {
  let component: InvoiceInputHistoryComponent;
  let fixture: ComponentFixture<InvoiceInputHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceInputHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
