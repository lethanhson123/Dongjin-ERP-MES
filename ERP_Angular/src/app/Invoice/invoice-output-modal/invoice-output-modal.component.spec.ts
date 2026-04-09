import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceOutputModalComponent } from './invoice-output-modal.component';

describe('InvoiceOutputModalComponent', () => {
  let component: InvoiceOutputModalComponent;
  let fixture: ComponentFixture<InvoiceOutputModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceOutputModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceOutputModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
