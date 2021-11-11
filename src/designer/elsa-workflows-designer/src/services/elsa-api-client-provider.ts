import axios, {AxiosInstance, AxiosRequestConfig} from "axios";
import {Service as MiddlewareService} from 'axios-middleware';
import {EventBus} from './event-bus';
import {ActivityDescriptor, ActivityDescriptorResponse,} from "../models";
import {EventTypes} from "../models/events";
import {Container, Service} from "typedi";
import {ServerSettings} from "./server-settings";

async function createHttpClient(baseAddress: string): Promise<AxiosInstance> {
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

async function createElsaClient(serverUrl: string): Promise<ElsaClient> {

  const httpClient: AxiosInstance = await createHttpClient(serverUrl);

  return {
    activityDescriptorsApi: {
      list: async () => {
        const response = await httpClient.get<ActivityDescriptorResponse>('api/activity-descriptors');
        return response.data.activityDescriptors;
      }
    },
  };
}

export interface ElsaClient {
  activityDescriptorsApi: ActivitiesApi;
}

export interface ActivitiesApi {
  list(): Promise<Array<ActivityDescriptor>>;
}

@Service()
export class ElsaApiClientProvider
{
  private _serverSettings: ServerSettings;
  private _elsaClient: ElsaClient;

  constructor(serverSettings: ServerSettings) {
    this._serverSettings = serverSettings;
  }

  public async getClient(): Promise<ElsaClient>
  {
    if(!!this._elsaClient)
      return this._elsaClient;

    return this._elsaClient = await createElsaClient(this._serverSettings.baseAddress);
  }
}
