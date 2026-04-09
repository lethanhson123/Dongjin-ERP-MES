import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceOutputComponent } from './invoice-output.component';

describe('InvoiceOutputComponent', () => {
  let component: InvoiceOutputComponent;
  let fixture: ComponentFixture<InvoiceOutputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceOutputComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceOutputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
