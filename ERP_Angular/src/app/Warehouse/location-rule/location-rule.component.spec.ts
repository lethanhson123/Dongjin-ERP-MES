import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationRuleComponent } from './location-rule.component';

describe('LocationRuleComponent', () => {
  let component: LocationRuleComponent;
  let fixture: ComponentFixture<LocationRuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LocationRuleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocationRuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
