import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMCompare01Component } from './bomcompare01.component';

describe('BOMCompare01Component', () => {
  let component: BOMCompare01Component;
  let fixture: ComponentFixture<BOMCompare01Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMCompare01Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMCompare01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
