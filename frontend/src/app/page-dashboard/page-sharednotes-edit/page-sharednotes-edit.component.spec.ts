import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSharednotesEditComponent } from './page-sharednotes-edit.component';

describe('PageSharednotesEditComponent', () => {
  let component: PageSharednotesEditComponent;
  let fixture: ComponentFixture<PageSharednotesEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageSharednotesEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSharednotesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
