define(["require", "exports", "viewmodels/lobby", "tests/helper", "auth"], function(require, exports, __unit__, __helper__, __auth__) {
    var unitName = "viewmodels/lobby";
    var unit = __unit__;

    var helper = __helper__;

    var auth = __auth__;

    QUnit.module(unitName, {
        setup: function () {
            var u = unit;
            auth.user = new auth.RegisteredClient();
            auth.user.userId = "user-id";
        },
        teardown: function () {
            helper.restore(require("hubs/lobby").getGames);
            helper.restore(require("hubs/lobby").leaveGame);
            helper.restore(require("hubs/lobby").joinGame);
        }
    });
    asyncTest("activate() -> Calls lobby.getGames(), populates games collection on success", function () {
        var game = {
            id: "test-id"
        };
        var _unit = null;
        sinon.stub(require("hubs/lobby"), "getGames", function () {
            ok(true, "Called lobby.getGames()");
            setTimeout(function () {
                var g = _.find(_unit.filteredGames(), function (a) {
                    return a.id === game.id;
                });
                deepEqual(g, game, "Found game in filtered list");
            }, 0);
            return $.Deferred(function (dfd) {
                dfd.resolve([
                    game
                ]);
            }).promise();
        });
        helper.reload(unitName).always(function (u) {
            _unit = u;
            _unit.activate();
            start();
        });
    });
    asyncTest("activate() -> Only calls lobby.getGames() the first time its run", function () {
        var runCount = 0;
        sinon.stub(require("hubs/lobby"), "getGames", function () {
            runCount++;
            return $.Deferred(function (dfd) {
                dfd.resolve();
            }).promise();
        });
        helper.reload(unitName).always(function (u) {
            u.activate();
            u.activate();
            equal(runCount, 1, "Didn't reinitialize games");
            start();
        });
    });
    test("canJoinGame() -> Returns true if they aren't already a member of the game, false if they are", function () {
        var user = {
            id: auth.user.userId
        };
        var game = {
            userList: ko.observableArray([
                user
            ])
        };
        equal(unit.canJoinGame(game), false, "They were in the game, so it said no");
        game.userList.removeAll();
        equal(unit.canJoinGame(game), true, "They weren't in the game, so it said yes");
    });
    test("canLeaveGame() -> Returns true if they are in the game and not the owner, otherwise false", function () {
        var user = {
            id: auth.user.userId
        };
        var game = {
            userList: ko.observableArray([
                user
            ])
        };
        equal(unit.canLeaveGame(game), true, "They were in the game and not the owner, so it said yes");
        game.creator = user.id;
        equal(unit.canLeaveGame(game), false, "They were in the game and not the owner, so it said yes");
        game.userList.removeAll();
        equal(unit.canLeaveGame(game), false, "They weren't in the game, so it said no");
    });
    test("createdGame() -> Returns true if they are the game creator", function () {
        var user = {
            id: auth.user.userId
        };
        var game = {
            creator: user.id,
            userList: ko.observableArray([])
        };
        ok(unit.createdGame(game));
    });
    test("joinGame() -> Calls lobby.joinGame() with correct id", function () {
        var game = {
            userList: ko.observableArray([])
        };
        game.id = "game-id";
        sinon.stub(require("hubs/lobby"), "joinGame", function (id) {
            equal(id, game.id);
        });
        unit.joinGame(game);
    });
    test("leaveGame() -> Calls lobby.leaveGame() with correct id", function () {
        var game = {
            userList: ko.observableArray([])
        };
        game.id = "game-id";
        sinon.stub(require("hubs/lobby"), "leaveGame", function (id) {
            equal(id, game.id);
        });
        unit.leaveGame(game);
    });
})
//@ sourceMappingURL=_lobby.js.map
