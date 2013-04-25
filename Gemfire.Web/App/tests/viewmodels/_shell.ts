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

test( "activate() -> Starts at lobby", function ()
{
    sinon.stub( require( "durandal/plugins/router" ), "activate", function ( view:string )
    {
        equal( view, "lobby" );
    } );

    unit.activate();
} );

test( "logout() -> Calls auth.logout()", function ()
{
    sinon.stub( require( "auth" ), "logout", function ( view: string )
    {
        ok( true );
    } );

    unit.logout();
} );