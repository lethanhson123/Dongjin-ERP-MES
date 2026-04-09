import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HookRackDetailComponent } from './hook-rack-detail.component';

describe('HookRackDetailComponent', () => {
  let component: HookRackDetailComponent;
  let fixture: ComponentFixture<HookRackDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HookRackDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HookRackDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
