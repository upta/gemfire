/// <reference path="../../Scripts/typings/_references.d.ts" />

export class Triggers
{
    static gameCreated = "hub:gameCreated";
    static gameRemoved = "hub:gameRemoved";
    static joinedGame = "hub:joinedGame";
    static leftGame = "hub:leftGame";
}

import app = module("durandal/app");

$.connection.lobby.client.gameCreated = function ( game: GameDto )
{
    app.trigger( Triggers.gameCreated, game );
}

$.connection.lobby.client.gameRemoved = function ( gameId: string )
{
    app.trigger( Triggers.gameRemoved, gameId );
}

$.connection.lobby.client.joinedGame = function ( game: string, user : UserDto )
{
    app.trigger( Triggers.joinedGame, game, user );
}

$.connection.lobby.client.leftGame = function ( game: string, user: string )
{
    app.trigger( Triggers.leftGame, game, user );
}

export function createGame( name: string, scenario: string )
{
    return $.connection.lobby.server.createGame( name, scenario );
}

export function joinGame( gameId: string )
{
    return $.connection.lobby.server.joinGame( gameId );
}

export function leaveGame( gameId: string )
{
    return $.connection.lobby.server.leaveGame( gameId );
}

export function getGames()
{
    return $.connection.lobby.server.getGames();
}

export function start( registrationId: string )
{
    return $.Deferred( function ( dfd )
    {
        $.connection.hub.start( function ()
        {
            $.connection.lobby.server.initializeClient( registrationId )
            .done( function ( init )
            {
                if ( init )
                {
                    dfd.resolve( init );
                }
                else
                {
                    dfd.reject();
                }
            } );
        } );
    } ).promise();
}