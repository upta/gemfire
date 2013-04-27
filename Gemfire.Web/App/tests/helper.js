define(["require", "exports"], function(require, exports) {
    function restore(method) {
        try  {
            method.restore();
        } catch (ex) {
        }
    }
    exports.restore = restore;
    function reload(unitName) {
        return $.Deferred(function (dfd) {
            requirejs.undef(unitName);
            require([
                unitName
            ], function (unit) {
                dfd.resolve(unit);
            });
        }).promise();
    }
    exports.reload = reload;
})
//@ sourceMappingURL=helper.js.map
