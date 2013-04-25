/// <reference path="../../Scripts/typings/_references.test.d.ts" />

import helper = module( "tests/helper" );
import unit = module( "auth" );

QUnit.module( "auth",
{
    teardown: function ()
    {
        $.removeCookie( "gemfire.state" );
        helper.restore( require( "global" ).reload );
        helper.restore( require( "hubs/lobby" ).start );
    }
} );

test( "logout() -> Removes state cookie and reloads page", function ()
{
    $.cookie( "gemfire.state", "test cookie" );
    sinon.stub( require( "global" ), "reload", function ( events: string )
    {
        ok( true, "Reloaded page" );
    } );
    
    unit.logout();

    ok( !$.cookie( "gemfire.state" ), "Cookie removed" );
} );

asyncTest( "init() -> Rejects if the state cookie has no registrationId", function ()
{
    $.cookie( "gemfire.state", JSON.stringify( {} ) );

    unit.init()
    .fail( function ()
    {
        ok( true );
    } )
    .done( function ()
    {
        ok( false, "Shouldn't resolve if no registrationId" );
    } )
    .always( function ()
    {
        start();
    } );
} );

asyncTest( "init() -> Re-creates cookie without registrationId", function ()
{
    var reg = { registrationId: "12345" };
    $.cookie( "gemfire.state", JSON.stringify( reg ) );

    sinon.stub( require( "hubs/lobby" ), "start", function ()
    {
        ok( true, "Called lobby.start()" );

        return $.Deferred( function ( dfd )
        {
            dfd.resolve( { userId: 1 } );
        } ).promise();
    } );

    unit.init()
    .done( function ()
    {
        ok( true, "Resolved after successful lobby.start()" );
    } )
    .always( function ()
    {
        delete reg.registrationId;
        deepEqual( reg, $.parseJSON( $.cookie( "gemfire.state" ) ), "Removed registratonId from cookie" );

        start();
    } );
} );

asyncTest( "init() -> Rejects if lobby.start() fails", function ()
{
    var reg = { registrationId: "12345" };
    $.cookie( "gemfire.state", JSON.stringify( reg ) );

    sinon.stub( require( "hubs/lobby" ), "start", function ()
    {
        return $.Deferred( function ( dfd )
        {
            dfd.reject();
        } ).promise();
    } );

    unit.init()
    .fail( function ()
    {
        ok( true );
    } )
    .done( function ()
    {
        ok( false, "Shouldn't resolve if lobby.start() failed" );
    } )
    .always( function ()
    {
        start();
    } );
} );