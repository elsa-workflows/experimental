import {Component, h, Prop, Watch} from "@stencil/core";
import 'reflect-metadata';
import {Container} from "typedi";
import {ServerSettings} from "../../../services/server-settings";

@Component({
  tag: 'elsa-shell'
})
export class ElsaShell {

  @Prop({attribute: 'server'})
  public serverUrl: string;

  @Watch('serverUrl')
  handleServerUrl(value: string) {
    const settings = Container.get(ServerSettings);
    settings.baseAddress = value;
  }

  async componentWillLoad() {
    this.handleServerUrl(this.serverUrl);
  }

  render() {
    return <slot/>;
  }
}
