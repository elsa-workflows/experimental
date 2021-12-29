import {Component, Event, EventEmitter, h, Host, Method, State} from '@stencil/core';
import {DefaultActions, PagedList, VersionOptions, WorkflowInstanceSummary, WorkflowSummary} from "../../../models";
import {Container} from "typedi";
import {ElsaApiClientProvider, ElsaClient} from "../../../services";
import {DeleteIcon, EditIcon} from "../../icons";
import {getStatusColor, updateSelectedWorkflowInstances} from "./utils";
import {formatTimestamp} from "../../../utils/utils";
import {PagerData} from "../../shared/pager/pager";

@Component({
  tag: 'elsa-workflow-instance-browser',
  shadow: false,
})
export class WorkflowInstanceBrowser {
  static readonly DEFAULT_PAGE_SIZE = 15;
  static readonly MIN_PAGE_SIZE = 5;
  static readonly MAX_PAGE_SIZE = 100;
  static readonly START_PAGE = 0;

  private elsaClient: ElsaClient;
  private modalDialog: HTMLElsaModalDialogElement;
  private selectAllCheckbox: HTMLInputElement;

  @Event() public workflowInstanceSelected: EventEmitter<WorkflowInstanceSummary>;
  @State() private workflowInstances: PagedList<WorkflowInstanceSummary> = {items: [], totalCount: 0};
  @State() workflows: Array<WorkflowSummary> = [];
  @State() private selectAllChecked: boolean;
  @State() private selectedWorkflowInstanceIds: Array<string> = [];
  @State() currentPage: number = 0;
  @State() currentPageSize: number = WorkflowInstanceBrowser.DEFAULT_PAGE_SIZE;
  @State() currentSearchTerm?: string;

  @Method()
  public async show() {
    await this.modalDialog.show();
    await this.loadWorkflowInstances();
  }

  @Method()
  public async hide() {
    await this.modalDialog.hide();
  }

  public async componentWillLoad() {
    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    this.elsaClient = await elsaClientProvider.getClient();
  }

  public render() {
    const workflows = this.workflows;
    const workflowInstances = this.workflowInstances;
    const totalCount = workflowInstances.totalCount
    const closeAction = DefaultActions.Close();
    const actions = [closeAction];

    return (
      <Host class="block">

        <elsa-modal-dialog ref={el => this.modalDialog = el} actions={actions} size="sm:w-fit max-w-fit">
          <div class="pt-4">
            <h2 class="text-lg font-medium ml-4 mb-2">Workflow Instances</h2>
            <div class="align-middle inline-block min-w-full border-b border-gray-200">
              <table>
                <thead>
                <tr>
                  <th>
                    <input type="checkbox" value="true" checked={this.getSelectAllState()}
                           onChange={e => this.onSelectAllCheckChange(e)}
                           ref={el => this.selectAllCheckbox = el}/>
                  </th>
                  <th><span class="lg:pl-2">ID</span></th>
                  <th class="optional">Correlation</th>
                  <th>Workflow</th>
                  <th class="align-right">Version</th>
                  <th class="optional">Name</th>
                  <th>Status</th>
                  <th class="optional">Created</th>
                  <th class="optional">Finished</th>
                  <th class="optional">Executed</th>
                  <th class="optional">Faulted</th>
                  <th/>
                </tr>
                </thead>
                <tbody class="bg-white divide-y divide-gray-100">
                {workflowInstances.items.map(workflowInstance => {
                  const statusColor = getStatusColor(workflowInstance.workflowStatus);
                  const isSelected = this.selectedWorkflowInstanceIds.findIndex(x => x === workflowInstance.id) >= 0;
                  const workflow: WorkflowSummary = workflows.find(x => x.definitionId == workflowInstance.definitionId && x.version == workflowInstance.version);
                  const workflowName = !!workflow ? (workflow.name || 'Untitled') : '(Definition not found)';

                  return (
                    <tr>
                      <td>
                        <input type="checkbox" value={workflowInstance.id} checked={isSelected} onChange={e => this.onWorkflowInstanceCheckChange(e, workflowInstance)}/>
                      </td>
                      <td>
                        <div class="flex items-center space-x-3 lg:pl-2">
                          <a onClick={e => this.onWorkflowInstanceClick(e, workflowInstance)} href="#" class="truncate hover:text-gray-600"><span>{workflowInstance.id}</span></a>
                        </div>
                      </td>

                      <td class="optional">{workflowInstance.correlationId}</td>
                      <td class="optional">{workflowName}</td>
                      <td class="align-right">{workflowInstance.version}</td>
                      <td class="optional">{workflowInstance.name}</td>
                      <td>
                        <div class="flex items-center space-x-3 lg:pl-2">
                          <div class={`flex-shrink-0 w-2.5 h-2.5 rounded-full ${statusColor}`}/>
                          <span>{workflowInstance.workflowStatus}</span>
                        </div>
                      </td>
                      <td class="optional">{formatTimestamp(workflowInstance.createdAt, '-')}</td>
                      <td class="optional">{formatTimestamp(workflowInstance.finishedAt, '-')}</td>
                      <td class="optional">{formatTimestamp(workflowInstance.lastExecutedAt, '-')}</td>
                      <td class="optional">{formatTimestamp(workflowInstance.faultedAt, '-')}</td>
                      <td class="pr-6">
                        <elsa-context-menu menuItems={[
                          {text: 'Edit', clickHandler: e => this.onWorkflowInstanceClick(e, workflowInstance), icon: <EditIcon/>},
                          {text: 'Delete', clickHandler: e => this.onDeleteClick(e, workflowInstance), icon: <DeleteIcon/>}
                        ]}/>
                      </td>
                    </tr>
                  );
                })}
                </tbody>
              </table>
              <elsa-pager page={this.currentPage} pageSize={this.currentPageSize} totalCount={totalCount} onPaginated={this.onPaginated}/>
            </div>

            {/*<confirm-dialog ref={el => this.confirmDialog = el} culture={this.culture}/>*/}
          </div>
        </elsa-modal-dialog>
      </Host>
    );
  }

  private async loadWorkflowInstances() {
    const elsaClient = this.elsaClient;
    const page = this.currentPage;
    const pageSize = this.currentPageSize;
    const workflowInstances = await elsaClient.workflowInstances.list({page: page, pageSize: pageSize});
    const definitionIds = new Set(workflowInstances.items.map(x => x.definitionId));

    await this.loadWorkflowBlueprints(Array.from(definitionIds));
    this.workflowInstances = workflowInstances;
  }

  private loadWorkflowBlueprints = async (definitionIds: Array<string>) => {
    debugger;
    const elsaClient = this.elsaClient;
    const versionOptions: VersionOptions = {allVersions: true};
    this.workflows = await elsaClient.workflows.getMany({definitionIds, versionOptions});
  };

  private getSelectAllState = () => {
    const {items} = this.workflowInstances;
    const selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds;
    return items.findIndex(item => !selectedWorkflowInstanceIds.includes(item.id)) < 0;
  }

  private setSelectAllIndeterminateState = () => {
    if (this.selectAllCheckbox) {
      const selectedItems = this.workflowInstances.items.filter(item => this.selectedWorkflowInstanceIds.includes(item.id));
      this.selectAllCheckbox.indeterminate = selectedItems.length != 0 && selectedItems.length != this.workflowInstances.items.length;
    }
  }

  private async onDeleteClick(e: MouseEvent, workflowInstance: WorkflowInstanceSummary) {

    // const result = await this.confirmDialog.show(t('DeleteConfirmationModel.Title'), t('DeleteConfirmationModel.Message'));
    //
    // if (!result)
    //   return;
    //
    // const elsaClient = await this.createClient();
    // await elsaClient.workflowDefinitionsApi.delete(workflowDefinition.definitionId);
    // await this.loadWorkflowDefinitions();
  }

  private onWorkflowInstanceClick = async (e: MouseEvent, workflowInstance: WorkflowInstanceSummary) => {
    e.preventDefault();
    this.workflowInstanceSelected.emit(workflowInstance);
    await this.hide();
  }

  private onSelectAllCheckChange(e: Event) {
    const checkBox = e.target as HTMLInputElement;
    const isChecked = checkBox.checked;
    this.selectAllChecked = isChecked;
    this.selectedWorkflowInstanceIds = updateSelectedWorkflowInstances(isChecked, this.workflowInstances, this.selectedWorkflowInstanceIds);
  }

  private onWorkflowInstanceCheckChange(e: Event, workflowInstance: WorkflowInstanceSummary) {
    const checkBox = e.target as HTMLInputElement;
    const isChecked = checkBox.checked;

    if (isChecked)
      this.selectedWorkflowInstanceIds = [...this.selectedWorkflowInstanceIds, workflowInstance.id];
    else
      this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.filter(x => x != workflowInstance.id);

    this.setSelectAllIndeterminateState();
  }

  onPaginated = async (e: CustomEvent<PagerData>) => {
    this.currentPage = e.detail.page;
    await this.loadWorkflowInstances();
  };
}