///#source 1 1 /app/auth.js
define(["require", "exports", "hubs/lobby"], function(require, exports, __hub__) {
    var RegisteredClient = (function () {
        function RegisteredClient() { }
        return RegisteredClient;
    })();
    exports.RegisteredClient = RegisteredClient;    
    var hub = __hub__;

    function init() {
        return $.Deferred(function (dfd) {
            var _user = ($.parseJSON($.cookie("gemfire.state")));
            exports.user = _user;
            if(_user.registrationId) {
                var registrationId = _user.registrationId;
                delete _user.registrationId;
                $.cookie("gemfire.state", JSON.stringify(_user), {
                    path: '/',
                    expires: 30
                });
                hub.start(registrationId).fail(function () {
                    dfd.reject();
                }).done(function (result) {
                    _user.userId = result.userId;
                    dfd.resolve();
                });
            } else {
                dfd.reject();
            }
        }).promise();
    }
    exports.init = init;
    function logout() {
        $.removeCookie("gemfire.state");
        location.reload(true);
    }
    exports.logout = logout;
    exports.user;
})
//@ sourceMappingURL=auth.js.map

///#source 1 1 /app/config.js
define(["require", "exports"], function(require, exports) {
    var GameScenario = (function () {
        function GameScenario(name) {
            this.name = name;
        }
        return GameScenario;
    })();
    exports.GameScenario = GameScenario;    
    var _scenarios = [
        new GameScenario("Erin & Ander"), 
        new GameScenario("Flax's Shame"), 
        new GameScenario("Terrian's War"), 
        new GameScenario("Gemfire")
    ];
    exports.scenarios = _scenarios;
})
//@ sourceMappingURL=config.js.map

///#source 1 1 /app/main.js
define(["require", "exports", "durandal/app", "durandal/plugins/router", "durandal/system", "durandal/viewLocator", "auth"], function(require, exports, __app__, __router__, __system__, __viewLocator__, __auth__) {
    requirejs.config({
        paths: {
            'text': 'durandal/amd/text'
        }
    });
    var app = __app__;

    var router = __router__;

    var system = __system__;

    var viewLocator = __viewLocator__;

    var auth = __auth__;

    system.debug(true);
    app.title = "Gemfire";
    app.start().then(function () {
        viewLocator.useConvention();
        router.useConvention();
        router.mapAuto();
        auth.init().fail(function () {
            alert("Please refresh your browser");
        }).done(function (result) {
            app.setRoot("viewmodels/shell", "entrance", null);
        });
    });
})
//@ sourceMappingURL=main.js.map

///#source 1 1 /app/hubs/lobby.js
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

///#source 1 1 /app/viewmodels/create.js
define(["require", "exports", "config", "durandal/plugins/router", "hubs/lobby"], function(require, exports, __config__, __router__, __hub__) {
    var config = __config__;

    var router = __router__;

    var hub = __hub__;

    function activate() {
    }
    exports.activate = activate;
    function create() {
        hub.createGame(_name(), _selectedScenario()).then(function () {
            router.navigateTo("#/lobby/mine");
        });
    }
    exports.create = create;
    var _name = ko.observable();
    exports.name = _name;
    var _selectedScenario = ko.observable();
    exports.selectedScenario = _selectedScenario;
    exports.scenarios = config.scenarios;
})
//@ sourceMappingURL=create.js.map

///#source 1 1 /app/viewmodels/lobby.js
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
    hub.getGames().done(function (result) {
        _.each(result, function (a) {
            a.userList = ko.observableArray(a.players);
            _games.push(a);
        });
    });
    exports.filter = _filter;
    exports.filteredGames = ko.computed(function () {
        var result = _games();
        result = _.sortBy(result, function (a) {
            return a.createdAt;
        }).reverse();
        return result;
    });
    function canJoinGame(game) {
        return !_.find(game.userList(), function (a) {
            return a.id == auth.user.userId;
        });
    }
    exports.canJoinGame = canJoinGame;
    function canLeaveGame(game) {
        return _.find(game.userList(), function (a) {
            return a.id == auth.user.userId && a.id != game.creator;
        });
    }
    exports.canLeaveGame = canLeaveGame;
    function createdGame(game) {
        return game.creator == auth.user.userId;
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

///#source 1 1 /app/viewmodels/shell.js
define(["require", "exports", "durandal/plugins/router", "auth"], function(require, exports, ___router__, __auth__) {
    var _router = ___router__;

    var auth = __auth__;

    function activate() {
        return exports.router.activate("lobby");
    }
    exports.activate = activate;
    function logout() {
        auth.logout();
    }
    exports.logout = logout;
    exports.router = _router;
    exports.user = auth.user;
})
//@ sourceMappingURL=shell.js.map

