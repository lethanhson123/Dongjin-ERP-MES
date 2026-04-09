import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputModalComponent } from './invoice-input-modal.component';

describe('InvoiceInputModalComponent', () => {
  let component: InvoiceInputModalComponent;
  let fixture: ComponentFixture<InvoiceInputModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
