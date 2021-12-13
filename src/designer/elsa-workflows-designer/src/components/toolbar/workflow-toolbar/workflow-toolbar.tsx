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
            <div class="ml-3 relative">
              <div>
                <button type="button"
                        class="bg-gray-800 flex text-sm rounded-full focus:outline-none focus:ring-1 focus:ring-offset-1 focus:ring-gray-600"
                        aria-expanded="false" aria-haspopup="true">
                  <span class="sr-only">Open user menu</span>
                  <svg class="h-8 w-8 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                          d="M12 5v.01M12 12v.01M12 19v.01M12 6a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2z"/>
                  </svg>
                </button>
              </div>

              {/*
                Dropdown menu, show/hide based on menu state.

                Entering: "transition ease-out duration-100"
                  From: "transform opacity-0 scale-95"
                  To: "transform opacity-100 scale-100"
                Leaving: "transition ease-in duration-75"
                  From: "transform opacity-100 scale-100"
                  To: "transform opacity-0 scale-95"
              */}
              <div
                class="hidden origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
                role="menu" aria-orientation="vertical" aria-labelledby="user-menu-button" tabindex="-1">
                {/*Active: "bg-gray-100", Not Active: ""*/}
                <a href="#" class="block px-4 py-2 text-sm text-gray-700" role="menuitem" tabindex="-1"
                   id="user-menu-item-0">Workflows</a>
                <a href="#" class="block px-4 py-2 text-sm text-gray-700" role="menuitem" tabindex="-1"
                   id="user-menu-item-1">Edit</a>
                <a href="#" class="block px-4 py-2 text-sm text-gray-700" role="menuitem" tabindex="-1"
                   id="user-menu-item-2">Settings</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>;
  }
}
