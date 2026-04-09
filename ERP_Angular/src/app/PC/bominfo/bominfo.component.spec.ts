import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMInfoComponent } from './bominfo.component';

describe('BOMInfoComponent', () => {
  let component: BOMInfoComponent;
  let fixture: ComponentFixture<BOMInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
