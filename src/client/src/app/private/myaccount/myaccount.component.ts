import { Component, OnInit } from '@angular/core';
import { TdMediaService } from '@covalent/core';

@Component({
  selector: 'app-myAccount',
  templateUrl: './myAccount.component.html',
  styleUrls: ['./myAccount.component.scss']
})
export class MyAccountComponent implements OnInit {

  constructor(
    public media: TdMediaService,
  ) { }

  ngOnInit() {
  }
}
