import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AirportDetail } from './airport-detail';

describe('AirportDetail', () => {
  let component: AirportDetail;
  let fixture: ComponentFixture<AirportDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AirportDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(AirportDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
