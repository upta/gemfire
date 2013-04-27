define(["require", "exports", "durandal/app", "auth", "hubs/lobby"], function(require, exports, __app__, __auth__, __hub__) {
    var app = __app__;

    var auth = __auth__;

    var hub = __hub__;

    var _games = ko.observableArray();
    var _filter = ko.observable();
    app.on(hub.Triggers.gameCreated, function (result) {
        result.userList = ko.observableArray(result.players);
        _games.push(result);
    });
    app.on(hub.Triggers.gameRemoved, function (gameId) {
        var game = _.find(_games(), function (a) {
            return a.id == gameId;
        });
        _games.remove(game);
    });
    app.on(hub.Triggers.joinedGame, function (gameId, user) {
        var _game = _.find(_games(), function (a) {
            return gameId == a.id;
        });
        _game.userList.push(user);
    });
    app.on(hub.Triggers.leftGame, function (gameId, userId) {
        var _game = _.find(_games(), function (a) {
            return gameId == a.id;
        });
        var _user = _.find(_game.userList(), function (a) {
            return a.id == userId;
        });
        _game.userList.remove(_user);
    });
    var init = _.once(function () {
        hub.getGames().done(function (result) {
            _.each(result, function (a) {
                a.userList = ko.observableArray(a.players);
                _games.push(a);
            });
        });
    });
    function activate() {
        init();
    }
    exports.activate = activate;
    exports.filter = _filter;
    exports.filteredGames = ko.computed(function () {
        var result = _games();
        result = _.sortBy(result, function (a) {
            return a.createdAt;
        }).reverse();
        return result;
    });
    function canJoinGame(game) {
        return !Boolean(_.find(game.userList(), function (a) {
            return a.id === auth.user.userId;
        }));
    }
    exports.canJoinGame = canJoinGame;
    function canLeaveGame(game) {
        return Boolean(_.find(game.userList(), function (a) {
            return a.id === auth.user.userId && a.id !== game.creator;
        }));
    }
    exports.canLeaveGame = canLeaveGame;
    function createdGame(game) {
        return game.creator === auth.user.userId;
    }
    exports.createdGame = createdGame;
    function joinGame(game) {
        hub.joinGame(game.id);
    }
    exports.joinGame = joinGame;
    function leaveGame(game) {
        hub.leaveGame(game.id);
    }
    exports.leaveGame = leaveGame;
})
//@ sourceMappingURL=lobby.js.map
