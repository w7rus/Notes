import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSharednotesListComponent } from './page-sharednotes-list.component';

describe('PageSharednotesListComponent', () => {
  let component: PageSharednotesListComponent;
  let fixture: ComponentFixture<PageSharednotesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageSharednotesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSharednotesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
