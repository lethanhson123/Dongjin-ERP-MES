import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestInfoComponent } from './warehouse-request-info.component';

describe('WarehouseRequestInfoComponent', () => {
  let component: WarehouseRequestInfoComponent;
  let fixture: ComponentFixture<WarehouseRequestInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
