import axios, {AxiosInstance, AxiosRequestConfig} from "axios";
import {Service as MiddlewareService} from 'axios-middleware';
import {EventBus} from './event-bus';
import {ActivityDescriptor, ActivityDescriptorResponse, EventTypes, Workflow} from "../models";
import 'reflect-metadata';
import {Container, Service} from "typedi";
import {ServerSettings} from "./server-settings";
import {Flowchart} from "../components/activities/elsa-free-flowchart/models";

export async function createHttpClient(baseAddress: string): Promise<AxiosInstance> {
  const config: AxiosRequestConfig = {
    baseURL: baseAddress
  };

  const eventBus = Container.get(EventBus);
  await eventBus.emit(EventTypes.HttpClient.ConfigCreated, this, {config});

  const httpClient = axios.create(config);
  const service = new MiddlewareService(httpClient);

  await eventBus.emit(EventTypes.HttpClient.ClientCreated, this, {service, httpClient});

  return httpClient;
}

export async function createElsaClient(serverUrl: string): Promise<ElsaClient> {

  const httpClient: AxiosInstance = await createHttpClient(serverUrl);

  return {
    activityDescriptors: {
      async list(): Promise<Array<ActivityDescriptor>> {
        const response = await httpClient.get<ActivityDescriptorResponse>('api/activity-descriptors');
        return response.data.activityDescriptors;
      }
    },
    workflows: {
      async post(workflow: Workflow): Promise<Workflow> {
        const response = await httpClient.post<Workflow>('api/workflows', workflow);
        return response.data;
      }
    }
  };
}

export interface ElsaClient {
  activityDescriptors: ActivityDescriptorsApi;
  workflows: WorkflowsApi;
}

export interface ActivityDescriptorsApi {
  list(): Promise<Array<ActivityDescriptor>>;
}

export interface WorkflowsApi {
  post(workflow: Workflow): Promise<Workflow>;
}

@Service()
export class ElsaApiClientProvider {
  private elsaClient: ElsaClient;

  constructor(private serverSettings: ServerSettings) {
  }

  public async getClient(): Promise<ElsaClient> {
    if (!!this.elsaClient)
      return this.elsaClient;

    return this.elsaClient = await createElsaClient(this.serverSettings.baseAddress);
  }
}
