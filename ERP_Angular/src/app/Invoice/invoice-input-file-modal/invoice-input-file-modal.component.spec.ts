import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputFileModalComponent } from './invoice-input-file-modal.component';

describe('InvoiceInputFileModalComponent', () => {
  let component: InvoiceInputFileModalComponent;
  let fixture: ComponentFixture<InvoiceInputFileModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputFileModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputFileModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
