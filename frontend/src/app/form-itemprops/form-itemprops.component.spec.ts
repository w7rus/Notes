import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormItempropsComponent } from './form-itemprops.component';

describe('FormItempropsComponent', () => {
  let component: FormItempropsComponent;
  let fixture: ComponentFixture<FormItempropsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormItempropsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormItempropsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
