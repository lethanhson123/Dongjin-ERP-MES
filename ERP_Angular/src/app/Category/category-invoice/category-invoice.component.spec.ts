import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryInvoiceComponent } from './category-invoice.component';

describe('CategoryInvoiceComponent', () => {
  let component: CategoryInvoiceComponent;
  let fixture: ComponentFixture<CategoryInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryInvoiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
