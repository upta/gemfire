define(["require", "exports", "config", "durandal/plugins/router", "hubs/lobby"], function(require, exports, __config__, __router__, __hub__) {
    var config = __config__;

    var router = __router__;

    var hub = __hub__;

    function activate() {
    }
    exports.activate = activate;
    function create() {
        hub.createGame(_name(), _selectedScenario()).then(function () {
            router.navigateTo("#/lobby");
        });
    }
    exports.create = create;
    var _name = ko.observable();
    exports.name = _name;
    var _selectedScenario = ko.observable();
    exports.selectedScenario = _selectedScenario;
    exports.scenarios = config.scenarios;
})
//@ sourceMappingURL=createGame.js.map
