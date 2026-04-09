import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationDiagramComponent } from './location-diagram.component';

describe('LocationDiagramComponent', () => {
  let component: LocationDiagramComponent;
  let fixture: ComponentFixture<LocationDiagramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LocationDiagramComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocationDiagramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
