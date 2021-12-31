import {h} from '@stencil/core';
import {Service} from "typedi";
import {UIHint} from "../models";

export type InputControl = (RenderInputContext) => any;

// A registry of input controls mapped against UI hints.
@Service()
export class InputControlRegistry {
  private inputMap: Map<UIHint, InputControl> = new Map<UIHint, InputControl>();

  constructor() {
    this.add('single-line', c => <elsa-single-line-input renderContext={c}/>)
  }
  
  public add(uiHint: UIHint, control: InputControl) {
    this.inputMap[uiHint] = control;
  }

  public get(uiHint: UIHint): InputControl {
    return this.inputMap.get(uiHint);
  }

  has(uiHint: string): boolean {
    return this.inputMap.has(uiHint);
  }
}
