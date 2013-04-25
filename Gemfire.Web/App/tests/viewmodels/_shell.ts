/// <reference path="../../Scripts/typings/_references.test.d.ts" />

import helper = module( "tests/helper" );
import unit = module( "shell" );

QUnit.module( "auth",
{
    teardown: function ()
    {
        $.removeCookie( "gemfire.state" );
        helper.restore( require( "global" ).reload );
        helper.restore( require( "hubs/lobby" ).start );
    }
} );