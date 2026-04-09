import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Warehouse001Component } from './warehouse001.component';

describe('Warehouse001Component', () => {
  let component: Warehouse001Component;
  let fixture: ComponentFixture<Warehouse001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Warehouse001Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Warehouse001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
