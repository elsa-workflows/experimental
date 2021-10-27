import {Component, h, Host} from "@stencil/core";

export interface ActivityPickerStateChangedArgs {
  expanded: boolean;
}

@Component({
  tag: 'elsa-activity',
})
export class ElsaActivity {

  render() {
    return (
      <Host class="border border-solid border-blue-600 rounded-md bg-blue-400 text-white overflow-hidden">
        <div class="flex flex-row">
          <div class="flex flex-shrink items-center bg-blue-500">
            <div class="px-2 py-1">
              <svg class="h-6 w-6 text-white" width="24" height="24" viewBox="0 0 24 24"
                   stroke-width="2"
                   stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                <path stroke="none" d="M0 0h24v24H0z"/>
                <path d="M7 18a4.6 4.4 0 0 1 0 -9h0a5 4.5 0 0 1 11 2h1a3.5 3.5 0 0 1 0 7h-12"/>
              </svg>
            </div>
          </div>
          <div class="flex items-center">
            <div class="px-2 py-1">
              HTTP Trigger
            </div>
          </div>
        </div>
      </Host>
    );
  }
}
