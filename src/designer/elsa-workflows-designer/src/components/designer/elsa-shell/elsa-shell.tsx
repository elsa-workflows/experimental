import {Component, h, Prop, Watch} from "@stencil/core";
import 'reflect-metadata';
import {Container} from "typedi";
import {ServerSettings} from "../../../services/server-settings";
import {ElsaApiClientProvider} from "../../../services/elsa-api-client-provider";
import ShellTunnel, {ShellState} from "./state";
import {ActivityDescriptor} from "../../../models";

@Component({
  tag: 'elsa-shell'
})
export class ElsaShell {

  @Prop({attribute: 'server'})
  public serverUrl: string;
  private activityDescriptors: Array<ActivityDescriptor>;

  @Watch('serverUrl')
  handleServerUrl(value: string) {
    const settings = Container.get(ServerSettings);
    settings.baseAddress = value;
  }

  async componentWillLoad() {
    this.handleServerUrl(this.serverUrl);

    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    const client = await elsaClientProvider.getClient();
    this.activityDescriptors = await client.activityDescriptorsApi.list();
  }

  render() {

    const tunnelState: ShellState = {
      activityDescriptors: this.activityDescriptors
    };

    return <ShellTunnel.Provider state={tunnelState}>
      <slot/>
    </ShellTunnel.Provider>;
  }
}
