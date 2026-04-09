import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceOutputInfoComponent } from './invoice-output-info.component';

describe('InvoiceOutputInfoComponent', () => {
  let component: InvoiceOutputInfoComponent;
  let fixture: ComponentFixture<InvoiceOutputInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceOutputInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvoiceOutputInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
