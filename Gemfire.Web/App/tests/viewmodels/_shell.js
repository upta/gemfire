define(["require", "exports", "tests/helper", "viewmodels/shell"], function(require, exports, __helper__, __unit__) {
    var helper = __helper__;

    var unit = __unit__;

    QUnit.module("viewmodels/shell", {
        teardown: function () {
            helper.restore(require("durandal/plugins/router").activate);
            helper.restore(require("auth").logout);
        }
    });
    test("activate() -> Starts at lobby", function () {
        sinon.stub(require("durandal/plugins/router"), "activate", function (view) {
            equal(view, "lobby");
        });
        unit.activate();
    });
    test("logout() -> Calls auth.logout()", function () {
        sinon.stub(require("auth"), "logout", function (view) {
            ok(true);
        });
        unit.logout();
    });
})
//@ sourceMappingURL=_shell.js.map
