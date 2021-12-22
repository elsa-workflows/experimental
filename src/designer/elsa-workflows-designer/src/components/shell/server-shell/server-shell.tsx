import {Component, h, Listen, Prop, Watch} from '@stencil/core';
import 'reflect-metadata';
import {Container} from 'typedi';
import {ElsaApiClientProvider, ElsaClient, ServerSettings} from '../../../services';
import ShellTunnel, {ShellState} from './state';
import {ActivityDescriptor, TriggerDescriptor} from '../../../models';
import {WorkflowUpdatedArgs} from '../../designer/workflow-editor/workflow-editor';

@Component({
  tag: 'elsa-server-shell'
})
export class ServerShell {

  @Prop({attribute: 'server'})
  public serverUrl: string;
  private activityDescriptors: Array<ActivityDescriptor>;
  private triggerDescriptors: Array<TriggerDescriptor>;
  private elsaClient: ElsaClient;

  @Watch('serverUrl')
  handleServerUrl(value: string) {
    const settings = Container.get(ServerSettings);
    settings.baseAddress = value;
  }

  @Listen('workflowUpdated')
  async handleWorkflowUpdated(e: CustomEvent<WorkflowUpdatedArgs>) {
    await this.elsaClient.workflows.post({
      workflow: e.detail.workflow,
      publish: false
    });
  }

  async componentWillLoad() {
    this.handleServerUrl(this.serverUrl);

    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    this.elsaClient = await elsaClientProvider.getClient();
    this.activityDescriptors = await this.elsaClient.descriptors.activities.list();
    this.triggerDescriptors = await this.elsaClient.descriptors.triggers.list();
  }

  render() {

    const tunnelState: ShellState = {
      activityDescriptors: this.activityDescriptors,
      triggerDescriptors: this.triggerDescriptors
    };

    return <ShellTunnel.Provider state={tunnelState}>
      <slot/>
    </ShellTunnel.Provider>;
  }
}
