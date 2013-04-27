/// <reference path="../../../Scripts/typings/_references.test.d.ts" />

import helper = module( "tests/helper" );
import app = module( "durandal/app" );
import hub = module( "hubs/lobby" );
import unit = module( "viewmodels/createGame" );

QUnit.module( "viewmodels/createGame",
{
    teardown: function ()
    {
        helper.restore( require( "durandal/plugins/router" ).navigateTo );
        helper.restore( require( "hubs/lobby" ).createGame );

        unit.name( null );
        unit.selectedScenario( null );
    }
} );

asyncTest( "create() -> Calls lobby.createGame() with appropriate values, then navigates back to lobby", function ()
{
    var _name = "test name";
    var _scenario = "test scenario";

    unit.name( _name );
    unit.selectedScenario( _scenario );
    sinon.stub( require( "hubs/lobby" ), "createGame", function ( name: string, scenario: string )
    {
        equal( name, _name, "Name is correct" );
        equal( scenario, _scenario, "Scenario is correct" );
        
        return $.Deferred( function ( dfd )
        {
            dfd.resolve();
        } ).promise();
    } );

    sinon.stub( require( "durandal/plugins/router" ), "navigateTo", function ( path: string )
    {
        equal( path, "#/lobby", "Navigates to lobby" );
        start();
    } );

    unit.create();
} );