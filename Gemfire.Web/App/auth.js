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
