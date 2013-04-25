define(["require", "exports", "tests/helper", "viewmodels/createGame"], function(require, exports, __helper__, __unit__) {
    var helper = __helper__;

    var unit = __unit__;

    QUnit.module("viewmodels/createGame", {
        teardown: function () {
            helper.restore(require("durandal/plugins/router").navigateTo);
            helper.restore(require("hubs/lobby").createGame);
            unit.name(null);
            unit.selectedScenario(null);
        }
    });
    asyncTest("create() -> Calls lobby.createGame() with appropriate values, then navigates back to lobby", function () {
        var _name = "test name";
        var _scenario = "test scenario";
        unit.name(_name);
        unit.selectedScenario(_scenario);
        sinon.stub(require("hubs/lobby"), "createGame", function (name, scenario) {
            equal(name, _name, "Name is correct");
            equal(scenario, _scenario, "Scenario is correct");
            return $.Deferred(function (dfd) {
                dfd.resolve();
            }).promise();
        });
        sinon.stub(require("durandal/plugins/router"), "navigateTo", function (path) {
            equal(path, "#/lobby", "Navigates to lobby");
            start();
        });
        unit.create();
    });
})
//@ sourceMappingURL=_createGame.js.map
