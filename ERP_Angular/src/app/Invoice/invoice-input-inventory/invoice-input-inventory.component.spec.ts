import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputInventoryComponent } from './invoice-input-inventory.component';

describe('InvoiceInputInventoryComponent', () => {
  let component: InvoiceInputInventoryComponent;
  let fixture: ComponentFixture<InvoiceInputInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputInventoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
