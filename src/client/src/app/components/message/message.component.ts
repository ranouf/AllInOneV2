import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { CanColorCtor, mixinColor, CanColor, ThemePalette } from '@angular/material';


// Boilerplate for applying mixins to MessageComponent.
/** @docs-private */
class MessageComponentBase {
  constructor(public _elementRef: ElementRef) { }
}
const _MessageComponentMixinBase: CanColorCtor & typeof MessageComponentBase = mixinColor(MessageComponentBase);


@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent extends _MessageComponentMixinBase implements CanColor, OnInit {

  @Input() title: string;
  @Input() icon: string;
  @Input() color: ThemePalette;

  constructor(
    elementRef: ElementRef) {
    super(elementRef);
  }

  ngOnInit() {

  }

}
