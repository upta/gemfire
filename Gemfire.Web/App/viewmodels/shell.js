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
