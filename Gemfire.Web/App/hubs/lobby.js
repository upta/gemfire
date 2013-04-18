define(["require", "exports", "durandal/app"], function(require, exports, __app__) {
    var Triggers = (function () {
        function Triggers() { }
        Triggers.gameCreated = "hub:gameCreated";
        Triggers.gameRemoved = "hub:gameRemoved";
        Triggers.joinedGame = "hub:joinedGame";
        Triggers.leftGame = "hub:leftGame";
        return Triggers;
    })();
    exports.Triggers = Triggers;    
    var app = __app__;

    $.connection.lobby.client.gameCreated = function (game) {
        app.trigger(Triggers.gameCreated, game);
    };
    $.connection.lobby.client.gameRemoved = function (gameId) {
        app.trigger(Triggers.gameRemoved, gameId);
    };
    $.connection.lobby.client.joinedGame = function (game, user) {
        app.trigger(Triggers.joinedGame, game, user);
    };
    $.connection.lobby.client.leftGame = function (game, user) {
        app.trigger(Triggers.leftGame, game, user);
    };
    function createGame(name, scenario) {
        return $.connection.lobby.server.createGame(name, scenario);
    }
    exports.createGame = createGame;
    function joinGame(gameId) {
        return $.connection.lobby.server.joinGame(gameId);
    }
    exports.joinGame = joinGame;
    function leaveGame(gameId) {
        return $.connection.lobby.server.leaveGame(gameId);
    }
    exports.leaveGame = leaveGame;
    function getGames() {
        return $.connection.lobby.server.getGames();
    }
    exports.getGames = getGames;
    function start(registrationId) {
        return $.Deferred(function (dfd) {
            $.connection.hub.start(function () {
                $.connection.lobby.server.initializeClient(registrationId).done(function (init) {
                    if(init) {
                        dfd.resolve(init);
                    } else {
                        dfd.reject();
                    }
                });
            });
        }).promise();
    }
    exports.start = start;
})
//@ sourceMappingURL=lobby.js.map
