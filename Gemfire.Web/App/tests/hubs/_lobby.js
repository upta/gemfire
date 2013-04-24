define(["require", "exports", "tests/helper", "hubs/lobby"], function(require, exports, __helper__, __unit__) {
    var helper = __helper__;

    var unit = __unit__;

    QUnit.module("hubs/lobby", {
        teardown: function () {
            helper.restore($.connection.hub.start);
            helper.restore($.connection.lobby.server.initializeClient);
            helper.restore(require("durandal/app").trigger);
            helper.restore($.connection.lobby.server.createGame);
            helper.restore($.connection.lobby.server.joinGame);
            helper.restore($.connection.lobby.server.leaveGame);
            helper.restore($.connection.lobby.server.getGames);
        }
    });
    asyncTest("start() -> Starts hub connection", 1, function () {
        sinon.stub($.connection.hub, "start", function () {
            ok(true);
            start();
        });
        unit.start("reg-id");
    });
    asyncTest("start() -> Requests client initialization upon connecting", 1, function () {
        sinon.stub($.connection.hub, "start", function (callback) {
            callback();
        });
        sinon.stub($.connection.lobby.server, "initializeClient", function () {
            ok(true);
            start();
            return $.Deferred().promise();
        });
        unit.start("reg-id");
    });
    asyncTest("start() -> Resolves if initialized", 1, function () {
        sinon.stub($.connection.hub, "start", function (callback) {
            callback();
        });
        sinon.stub($.connection.lobby.server, "initializeClient", function () {
            return $.Deferred(function (dfd) {
                var initObject = {
                };
                dfd.resolve(initObject);
            }).promise();
        });
        unit.start("reg-id").fail(function () {
            ok(false, "Shouldn't have failed with an init object");
        }).done(function () {
            ok(true);
        }).always(function () {
            start();
        });
    });
    asyncTest("start() -> Rejects if failed to initialize", 1, function () {
        sinon.stub($.connection.hub, "start", function (callback) {
            callback();
        });
        sinon.stub($.connection.lobby.server, "initializeClient", function () {
            return $.Deferred(function (dfd) {
                var initObject = null;
                dfd.resolve(initObject);
            }).promise();
        });
        unit.start("reg-id").fail(function () {
            ok(true);
        }).done(function () {
            ok(false, "Shouldn't have resolved without an init object");
        }).always(function () {
            start();
        });
    });
    test("Wires $.connection.lobby.client.gameCreated to publish Triggers.gameCreated", function () {
        sinon.stub(require("durandal/app"), "trigger", function (events) {
            ok(events === unit.Triggers.gameCreated);
        });
        $.connection.lobby.client.gameCreated(null);
    });
    test("Wires $.connection.lobby.client.gameRemoved to publish Triggers.gameRemoved", function () {
        sinon.stub(require("durandal/app"), "trigger", function (events) {
            ok(events === unit.Triggers.gameRemoved);
        });
        $.connection.lobby.client.gameRemoved(null);
    });
    test("Wires $.connection.lobby.client.joinedGame to publish Triggers.joinedGame", function () {
        sinon.stub(require("durandal/app"), "trigger", function (events) {
            ok(events === unit.Triggers.joinedGame);
        });
        $.connection.lobby.client.joinedGame(null, null);
    });
    test("Wires $.connection.lobby.client.leftGame to publish Triggers.leftGame", function () {
        sinon.stub(require("durandal/app"), "trigger", function (events) {
            ok(events === unit.Triggers.leftGame);
        });
        $.connection.lobby.client.leftGame(null, null);
    });
    test("createGame() => Calls createGame on hub", function () {
        sinon.stub($.connection.lobby.server, "createGame", function (events) {
            ok(true);
        });
        unit.createGame(null, null);
    });
    test("joinGame() => Calls joinGame on hub", function () {
        sinon.stub($.connection.lobby.server, "joinGame", function (events) {
            ok(true);
        });
        unit.joinGame(null);
    });
    test("leaveGame() => Calls leaveGame on hub", function () {
        sinon.stub($.connection.lobby.server, "leaveGame", function (events) {
            ok(true);
        });
        unit.leaveGame(null);
    });
    test("getGames() => Calls getGames on hub", function () {
        sinon.stub($.connection.lobby.server, "getGames", function (events) {
            ok(true);
        });
        unit.getGames();
    });
})
//@ sourceMappingURL=_lobby.js.map
