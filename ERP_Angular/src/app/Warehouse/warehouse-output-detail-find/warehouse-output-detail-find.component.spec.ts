import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailFindComponent } from './warehouse-output-detail-find.component';

describe('WarehouseOutputDetailFindComponent', () => {
  let component: WarehouseOutputDetailFindComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailFindComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailFindComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailFindComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
