import {Component, h, Listen, Prop, Watch} from "@stencil/core";
import 'reflect-metadata';
import {Container} from "typedi";
import {ServerSettings} from "../../../services/server-settings";
import {ElsaApiClientProvider, ElsaClient} from "../../../services/elsa-api-client-provider";
import ShellTunnel, {ShellState} from "./state";
import {ActivityDescriptor} from "../../../models";
import {WorkflowUpdatedArgs} from "../../designer/elsa-workflow-editor/elsa-workflow-editor";

@Component({
  tag: 'elsa-server-shell'
})
export class ElsaServerShell {

  @Prop({attribute: 'server'})
  public serverUrl: string;
  private activityDescriptors: Array<ActivityDescriptor>;
  private elsaClient: ElsaClient;

  @Watch('serverUrl')
  handleServerUrl(value: string) {
    const settings = Container.get(ServerSettings);
    settings.baseAddress = value;
  }

  @Listen('workflowUpdated')
  async handleWorkflowUpdated(e: CustomEvent<WorkflowUpdatedArgs>) {
    await this.elsaClient.workflows.post(e.detail.workflow);
  }

  async componentWillLoad() {
    this.handleServerUrl(this.serverUrl);

    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    this.elsaClient = await elsaClientProvider.getClient();
    this.activityDescriptors = await this.elsaClient.activityDescriptors.list();
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
