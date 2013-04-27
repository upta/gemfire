/// <reference path="../../Scripts/typings/_references.d.ts" />

export interface ListedGame extends GameDto
{
    userList: KnockoutObservableArray;
}

import app = module( "durandal/app" );
import auth = module( "auth" );
import hub = module( "hubs/lobby" );

var _games = ko.observableArray();
var _filter = ko.observable();

app.on( hub.Triggers.gameCreated, function ( result: ListedGame )
{
    result.userList = ko.observableArray( result.players );
    _games.push( result );
} );

app.on( hub.Triggers.gameRemoved, function ( gameId: string )
{
    var game = _.find( _games(), function ( a: ListedGame )
    {
        return a.id == gameId;
    } );

    _games.remove( game );
} );

app.on( hub.Triggers.joinedGame, function ( gameId : string, user : UserDto )
{
    var _game: ListedGame = _.find( _games(), function ( a: ListedGame )
    {
        return gameId == a.id;
    } );

    _game.userList.push( user );
} );

app.on( hub.Triggers.leftGame, function ( gameId: string, userId: string )
{
    var _game: ListedGame = _.find( _games(), function ( a: ListedGame )
    {
        return gameId == a.id;
    } );

    var _user = _.find( _game.userList(), function ( a: UserDto )
    {
        return a.id == userId;
    } );

    _game.userList.remove( _user );
} );

var init = _.once( function ()
{
    hub.getGames()
    .done( function ( result: ListedGame[] )
    {
        _.each( result, function ( a: ListedGame )
        {
            a.userList = ko.observableArray( a.players );
            _games.push( a );
        } );
    } );
} );

export function activate()
{
    init();
}

export var filter = _filter;

export var filteredGames = ko.computed( function ()
{
    var result = _games();

    result = _.sortBy( result, function ( a: GameDto )
    {
        return a.createdAt;
    } ).reverse();

    return result;
} )

export function canJoinGame( game: ListedGame ) : bool
{
    return !Boolean( _.find( game.userList(), function ( a: UserDto )
    {
        return a.id === auth.user.userId;
    } ) );
}

export function canLeaveGame( game: ListedGame ) : bool
{
    return Boolean( _.find( game.userList(), function ( a: UserDto )
    {
        return a.id === auth.user.userId && a.id !== game.creator;
    } ) );
}

export function createdGame( game: ListedGame ) : bool
{
    return game.creator === auth.user.userId;
}

export function joinGame( game: ListedGame )
{
    hub.joinGame( game.id );
}

export function leaveGame( game: ListedGame )
{
    hub.leaveGame( game.id );
}