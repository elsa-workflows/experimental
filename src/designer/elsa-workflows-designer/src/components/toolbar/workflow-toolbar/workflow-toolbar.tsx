import {Component, h} from '@stencil/core';

@Component({
  tag: 'elsa-workflow-toolbar'
})
export class WorkflowToolbar {

  render() {
    return <nav class="bg-gray-800">
      <div class="mx-auto px-2 sm:px-6 lg:px-6">
        <div class="relative flex items-center justify-end h-16">

          <div class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0 z-20">

            {/* Publish */}
            <div class="flex-shrink-0">
              <elsa-workflow-publish-button/>
            </div>

            {/* Menu */}
            <elsa-workflow-toolbar-menu/>
          </div>
        </div>
      </div>
    </nav>;
  }
}
