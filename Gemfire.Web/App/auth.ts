/// <reference path="../Scripts/typings/_references.d.ts" />

export class RegisteredClient
{
    displayName: string;
    identity: string;
    photo: string;
    registrationId: string;
    userId: string;
}

import hub = module("hubs/lobby");

export function init()
{
    return $.Deferred( function ( dfd )
    {
        var _user: RegisteredClient = ( <any>$.parseJSON( $.cookie( "gemfire.state" ) ) );
        user = _user;

        if ( _user.registrationId )
        {
            var registrationId = _user.registrationId;
            delete _user.registrationId;

            $.cookie( "gemfire.state", JSON.stringify( _user ), { path: '/', expires: 30 } );
            
            hub.start( registrationId )
            .fail( function ()
            {
                dfd.reject();
            } )
            .done( function ( result )
            {
                _user.userId = result.userId;

                dfd.resolve();
            } );
        }
        else
        {
            dfd.reject();
        }

    } ).promise();
}

export function logout()
{
    $.removeCookie( "gemfire.state" );
    location.reload( true );
}

export var user: RegisteredClient;