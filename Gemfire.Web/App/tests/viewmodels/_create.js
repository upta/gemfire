define(["require", "exports", "tests/helper"], function(require, exports, __helper__) {
    var helper = __helper__;

    
    QUnit.module("viewmodels/shell", {
        teardown: function () {
            helper.restore(require("durandal/plugins/router").activate);
            helper.restore(require("auth").logout);
        }
    });
})
//@ sourceMappingURL=_create.js.map
