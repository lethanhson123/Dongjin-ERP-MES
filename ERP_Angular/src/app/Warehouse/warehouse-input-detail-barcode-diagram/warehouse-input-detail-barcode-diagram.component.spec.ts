import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeDiagramComponent } from './warehouse-input-detail-barcode-diagram.component';

describe('WarehouseInputDetailBarcodeDiagramComponent', () => {
  let component: WarehouseInputDetailBarcodeDiagramComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeDiagramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeDiagramComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeDiagramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
