import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputInfoComponent } from './warehouse-output-info.component';

describe('WarehouseOutputInfoComponent', () => {
  let component: WarehouseOutputInfoComponent;
  let fixture: ComponentFixture<WarehouseOutputInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
