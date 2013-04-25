/// <reference path="../../Scripts/typings/_references.d.ts" />

import config = module( "config" );
import router = module( "durandal/plugins/router" );
import hub = module( "hubs/lobby" );

export function activate()
{

}

export function create()
{
    hub.createGame( _name(), _selectedScenario() )
    .then( function ()
    {
        router.navigateTo( "#/lobby" );
    } );
}

var _name = ko.observable();
export var name = _name;

var _selectedScenario = ko.observable();
export var selectedScenario = _selectedScenario;

export var scenarios = config.scenarios;