import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestComponent } from './warehouse-request.component';

describe('WarehouseRequestComponent', () => {
  let component: WarehouseRequestComponent;
  let fixture: ComponentFixture<WarehouseRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
