import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputProductionComponent } from './warehouse-output-production.component';

describe('WarehouseOutputProductionComponent', () => {
  let component: WarehouseOutputProductionComponent;
  let fixture: ComponentFixture<WarehouseOutputProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputProductionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseOutputProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
