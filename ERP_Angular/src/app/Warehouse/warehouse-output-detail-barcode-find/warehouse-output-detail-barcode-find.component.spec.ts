import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeFindComponent } from './warehouse-output-detail-barcode-find.component';

describe('WarehouseOutputDetailBarcodeFindComponent', () => {
  let component: WarehouseOutputDetailBarcodeFindComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeFindComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeFindComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeFindComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
