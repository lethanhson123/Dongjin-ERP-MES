import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMComponent } from './bom.component';

describe('BOMComponent', () => {
  let component: BOMComponent;
  let fixture: ComponentFixture<BOMComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
