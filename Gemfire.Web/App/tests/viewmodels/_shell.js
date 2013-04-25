define(["require", "exports", "tests/helper"], function(require, exports, __helper__) {
    var helper = __helper__;

    
    QUnit.module("auth", {
        teardown: function () {
            $.removeCookie("gemfire.state");
            helper.restore(require("global").reload);
            helper.restore(require("hubs/lobby").start);
        }
    });
})
//@ sourceMappingURL=_shell.js.map
