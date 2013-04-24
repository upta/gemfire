define(["require", "exports"], function(require, exports) {
    function restore(method) {
        try  {
            method.restore();
        } catch (ex) {
        }
    }
    exports.restore = restore;
})
//@ sourceMappingURL=helper.js.map
