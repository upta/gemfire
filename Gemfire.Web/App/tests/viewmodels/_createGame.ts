/// <reference path="../../../Scripts/typings/_references.test.d.ts" />

import helper = module( "tests/helper" );
import unit = module( "viewmodels/createGame" );

QUnit.module( "viewmodels/createGame",
{
    teardown: function ()
    {
        helper.restore( require( "durandal/plugins/router" ).activate );
        helper.restore( require( "auth" ).logout );

        unit.name( null );
        unit.selectedScenario( null );
    }
} );

asyncTest("create() -> Calls lobby.createGame() with appropriate values, then navigates back to lobby", function()
{
	
});