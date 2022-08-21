import {Component, OnInit} from "@angular/core";

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styles: [
    `.verdanaFont{font-size:14px; font-family:Verdana;}
    .segoePrintFont{font-size:14px; font-family:"Segoe Print";}`
  ]
})
export class FeedbackComponent implements OnInit {

  private _captcha: string
  private _isResolved: boolean;

  public get isResolved(): boolean{
    return this._isResolved;
  }

  constructor() {
    this._captcha = '';
    this._isResolved = false
  }

  public resolve(captcha: string){
    this._captcha = captcha
    this._isResolved = true
  }

  public submit(): void
  {
    if (this._isResolved)
    {

    }
  }

  public ngOnInit(): void {
  }
}
