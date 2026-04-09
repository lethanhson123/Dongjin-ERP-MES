import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryScanMobileComponent } from './inventory-scan-mobile.component';

describe('InventoryScanMobileComponent', () => {
  let component: InventoryScanMobileComponent;
  let fixture: ComponentFixture<InventoryScanMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryScanMobileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryScanMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
