import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryScanDesktopComponent } from './inventory-scan-desktop.component';

describe('InventoryScanDesktopComponent', () => {
  let component: InventoryScanDesktopComponent;
  let fixture: ComponentFixture<InventoryScanDesktopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryScanDesktopComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InventoryScanDesktopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
