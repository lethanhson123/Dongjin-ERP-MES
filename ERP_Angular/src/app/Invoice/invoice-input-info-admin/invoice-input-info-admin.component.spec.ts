import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputInfoAdminComponent } from './invoice-input-info-admin.component';

describe('InvoiceInputInfoAdminComponent', () => {
  let component: InvoiceInputInfoAdminComponent;
  let fixture: ComponentFixture<InvoiceInputInfoAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputInfoAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputInfoAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
