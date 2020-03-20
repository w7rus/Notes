import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PagePublicnotesListComponent } from './page-publicnotes-list.component';

describe('PagePublicnotesListComponent', () => {
  let component: PagePublicnotesListComponent;
  let fixture: ComponentFixture<PagePublicnotesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PagePublicnotesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PagePublicnotesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
