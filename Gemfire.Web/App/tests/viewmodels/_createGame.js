define(["require", "exports", "tests/helper", "viewmodels/createGame"], function(require, exports, __helper__, __unit__) {
    var helper = __helper__;

    var unit = __unit__;

    QUnit.module("viewmodels/createGame", {
        teardown: function () {
            helper.restore(require("durandal/plugins/router").activate);
            helper.restore(require("auth").logout);
            unit.name(null);
            unit.selectedScenario(null);
        }
    });
    asyncTest("create() -> Calls lobby.createGame() with appropriate values, then navigates back to lobby", function () {
    });
})
//@ sourceMappingURL=_createGame.js.map
