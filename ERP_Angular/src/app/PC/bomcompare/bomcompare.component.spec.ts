import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMCompareComponent } from './bomcompare.component';

describe('BOMCompareComponent', () => {
  let component: BOMCompareComponent;
  let fixture: ComponentFixture<BOMCompareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMCompareComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
