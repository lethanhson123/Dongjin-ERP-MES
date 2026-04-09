import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceInputInfoComponent } from './invoice-input-info.component';

describe('InvoiceInputInfoComponent', () => {
  let component: InvoiceInputInfoComponent;
  let fixture: ComponentFixture<InvoiceInputInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceInputInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceInputInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
