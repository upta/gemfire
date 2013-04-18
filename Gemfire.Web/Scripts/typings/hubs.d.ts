// Get signalr.d.ts.ts from https://github.com/borisyankov/DefinitelyTyped (or delete the reference)
/// <reference path="signalr/signalr.d.ts" />
/// <reference path="jquery/jquery.d.ts" />

////////////////////
// available hubs //
////////////////////
//#region available hubs

interface SignalR {

    /**
      * The hub implemented by Gemfire.Lobby
      */
    lobby : Lobby;
}
//#endregion available hubs

///////////////////////
// Service Contracts //
///////////////////////
//#region service contracts

//#region lobby hub

interface Lobby {
    
    /**
      * This property lets you send messages to the lobby hub.
      */
    server : LobbyServer;

    /**
      * The functions on this property should be replaced if you want to receive messages from the lobby hub.
      */
    client : LobbyClient;
}

interface LobbyServer {

    /** 
      * Sends a "createGame" message to the lobby hub.
      * Contract Documentation: ---
      * @param name {string} 
      * @param scenario {string} 
      * @return {JQueryPromise of void}
      */
    createGame(name : string, scenario : string) : JQueryPromise; // JQueryPromise<void>

    /** 
      * Sends a "initializeClient" message to the lobby hub.
      * Contract Documentation: ---
      * @param registrationId {string} 
      * @return {JQueryPromise of Object}
      */
    initializeClient(registrationId : string) : JQueryPromise; // JQueryPromise<Object>

    /** 
      * Sends a "getGames" message to the lobby hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of IEnumerableOfGameDto}
      */
    getGames() : JQueryPromise; // JQueryPromise<IEnumerableOfGameDto>

    /** 
      * Sends a "joinGame" message to the lobby hub.
      * Contract Documentation: ---
      * @param gameId {string} 
      * @return {JQueryPromise of void}
      */
    joinGame(gameId : string) : JQueryPromise; // JQueryPromise<void>

    /** 
      * Sends a "leaveGame" message to the lobby hub.
      * Contract Documentation: ---
      * @param gameId {string} 
      * @return {JQueryPromise of void}
      */
    leaveGame(gameId : string) : JQueryPromise; // JQueryPromise<void>
}

interface LobbyClient
{

    /**
      * Set this function with a "function(game : GameDto){}" to receive the "gameCreated" message from the lobby hub.
      * Contract Documentation: ---
      * @param game {GameDto} 
      * @return {void}
      */
    gameCreated : (game : GameDto) => void;

    /**
      * Set this function with a "function(gameId : string){}" to receive the "gameRemoved" message from the lobby hub.
      * Contract Documentation: ---
      * @param gameId {string} 
      * @return {void}
      */
    gameRemoved : (gameId : string) => void;

    /**
      * Set this function with a "function(gameId : string, user : UserDto){}" to receive the "joinedGame" message from the lobby hub.
      * Contract Documentation: ---
      * @param gameId {string} 
      * @param user {UserDto} 
      * @return {void}
      */
    joinedGame : (gameId : string, user : UserDto) => void;

    /**
      * Set this function with a "function(gameId : string, userId : string){}" to receive the "leftGame" message from the lobby hub.
      * Contract Documentation: ---
      * @param gameId {string} 
      * @param userId {string} 
      * @return {void}
      */
    leftGame : (gameId : string, userId : string) => void;
}

//#endregion lobby hub

//#endregion service contracts



////////////////////
// Data Contracts //
////////////////////
//#region data contracts


/**
  * Data contract for Gemfire.UserDto
  */
interface UserDto {
    id : string;
    name : string;
}


/**
  * Data contract for Gemfire.GameDto
  */
interface GameDto {
    createdAt : string;
    creator : string;
    id : string;
    name : string;
    players : UserDto[];
    scenario : string;
}


/**
  * Data contract for System.Collections.Generic.IEnumerable`1[[Gemfire.GameDto, Gemfire.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
  */
interface IEnumerableOfGameDto {
}


/**
  * Data contract for System.Object
  */
interface Object {
}

//#endregion data contracts

