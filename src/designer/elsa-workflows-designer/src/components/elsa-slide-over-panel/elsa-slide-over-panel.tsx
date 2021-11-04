import {Component, h, Prop} from "@stencil/core";

@Component({
  tag: 'elsa-slide-over-panel'
})
export class ElsaSlideOverPanel {
  @Prop()
  public isVisible: boolean;

  public render() {
    if (!this.isVisible)
      return null;

    return this.renderPanel();
  }

  private renderPanel() {
    return (
      <div class="fixed inset-0 overflow-hidden z-10" aria-labelledby="slide-over-title" role="dialog"
           aria-modal="true">
        <div class="absolute inset-0 overflow-hidden">
          <div class="absolute inset-0 bg-gray-100 opacity-50"/>
          <div class="absolute inset-0" aria-hidden="true">
            <div class="fixed inset-y-0 right-0 pl-10 max-w-full flex sm:pl-16">

              <div class="w-screen max-w-2xl">
                <form class="h-full flex flex-col bg-white shadow-xl">
                  <div class="flex flex-col flex-1">

                    <div class="px-4 py-6 bg-gray-50 sm:px-6">
                      <div class="flex items-start justify-between space-x-3">
                        <div class="space-y-1">
                          <h2 class="text-lg font-medium text-gray-900" id="slide-over-title">
                            Properties
                          </h2>
                        </div>
                        <div class="h-7 flex items-center">
                          <button type="button" class="text-gray-400 hover:text-gray-500">
                            <span class="sr-only">Close panel</span>
                            <svg class="h-6 w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                                 stroke="currentColor" aria-hidden="true">
                              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                    d="M6 18L18 6M6 6l12 12"/>
                            </svg>
                          </button>
                        </div>
                      </div>
                    </div>

                    <div class="border-b border-gray-200">
                      <nav class="-mb-px flex" aria-label="Tabs">
                        <a href="#"
                           class="border-blue-500 text-blue-600 py-4 px-1 text-center border-b-2 font-medium text-sm flex-1">
                          Common
                        </a>

                        <a href="#"
                           class="border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 py-4 px-1 text-center border-b-2 font-medium text-sm flex-1">
                          Advanced
                        </a>

                        <a href="#"
                           class="border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300 py-4 px-1 text-center border-b-2 font-medium text-sm flex-1">
                          JSON
                        </a>
                      </nav>
                    </div>

                    <div class="flex-1 relative">

                      <div class="absolute inset-0 overflow-y-scroll">
                        <div class="p-4">
                          <label htmlFor="activity-name" class="block text-sm font-medium text-gray-700">
                            Name
                          </label>
                          <div class="mt-1">
                            <input type="text" name="activity-name" id="activity-name"
                                   class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                          </div>
                          <p class="mt-2 text-sm text-gray-500">The name of the activity.</p>
                        </div>

                        <div class="p-4">
                          <label htmlFor="activity-description" class="block text-sm font-medium text-gray-700">
                            Description
                          </label>
                          <div class="mt-1">
                        <textarea name="activity-description" id="activity-description"
                                  class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
                          </div>
                          <p class="mt-2 text-sm text-gray-500">A description for this activity.</p>
                        </div>
                      </div>
                    </div>
                  </div>

                  <div class="flex-shrink-0 px-4 border-t border-gray-200 py-5 sm:px-6">
                    <div class="space-x-3 flex justify-end">
                      <button type="button"
                              class="bg-white py-2 px-4 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                        Cancel
                      </button>
                      <button type="submit"
                              class="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500">
                        Create
                      </button>
                    </div>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
