import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputAdminComponent } from './invoice-input-admin.component';

describe('InvoiceInputAdminComponent', () => {
  let component: InvoiceInputAdminComponent;
  let fixture: ComponentFixture<InvoiceInputAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
