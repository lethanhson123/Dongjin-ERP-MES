import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailComponent } from './warehouse-input-detail.component';

describe('WarehouseInputDetailComponent', () => {
  let component: WarehouseInputDetailComponent;
  let fixture: ComponentFixture<WarehouseInputDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
