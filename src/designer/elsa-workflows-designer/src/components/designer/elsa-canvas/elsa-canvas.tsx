import {Component, h, Event, Method, Element, EventEmitter, Listen} from '@stencil/core';
import {ActivityComponent} from "../../activities/activity-component";
import {ActivityDescriptor} from "../../../models";

export interface AddActivityArgs {
  descriptor: ActivityDescriptor;
  x?: number;
  y?: number;
}

@Component({
  tag: 'elsa-canvas',
  styleUrl: 'elsa-canvas.scss',
})
export class ElsaCanvas {

  private root: ActivityComponent;

  @Method()
  public async addActivity(args: AddActivityArgs): Promise<void> {
    await this.root.addActivity(args);
  }

  @Method()
  public async updateLayout(): Promise<void> {
    console.debug('update from canvas');
    await this.root.updateLayout();
  }

  render() {
    return (
      <div class="absolute left-0 top-0 right-0 bottom-0">
        <elsa-free-flowchart ref={el => this.root = el} class="absolute left-0 top-0 right-0 bottom-0"/>
      </div>
    );
  }
}
