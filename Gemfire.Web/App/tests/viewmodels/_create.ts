/// <reference path="../../../Scripts/typings/_references.test.d.ts" />

import helper = module( "tests/helper" );
import unit = module( "viewmodels/shell" );

QUnit.module( "viewmodels/shell",
{
    teardown: function ()
    {
        helper.restore( require( "durandal/plugins/router" ).activate );
        helper.restore( require( "auth" ).logout );
    }
} );