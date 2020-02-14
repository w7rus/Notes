import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageLogoutComponent } from './page-logout.component';

describe('PageLogoutComponent', () => {
  let component: PageLogoutComponent;
  let fixture: ComponentFixture<PageLogoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageLogoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageLogoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
