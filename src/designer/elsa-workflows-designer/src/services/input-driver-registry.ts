import _ from 'lodash';
import {Service} from 'typedi';
import {InputDriver, RenderInputContext} from './input-driver';
import {DefaultInputDriver} from "../drivers/input/default-input-driver";

@Service()
export class InputDriverRegistry {
  private drivers: Array<InputDriver> = [new DefaultInputDriver()];

  public add(driver: InputDriver) {
    const drivers: Array<InputDriver> = [...this.drivers, driver]
    this.drivers = _.orderBy(drivers, x => x.priority, 'desc');
  }

  public get(context: RenderInputContext): InputDriver {
    return this.drivers.find(x => x.supportsInput(context));
  }
}
