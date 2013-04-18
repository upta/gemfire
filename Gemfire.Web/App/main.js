define(["require", "exports", "durandal/app", "durandal/plugins/router", "durandal/system", "durandal/viewLocator", "auth"], function(require, exports, __app__, __router__, __system__, __viewLocator__, __auth__) {
    requirejs.config({
        paths: {
            'text': 'durandal/amd/text'
        }
    });
    var app = __app__;

    var router = __router__;

    var system = __system__;

    var viewLocator = __viewLocator__;

    var auth = __auth__;

    system.debug(true);
    app.title = "Gemfire";
    app.start().then(function () {
        viewLocator.useConvention();
        router.useConvention();
        router.mapAuto();
        auth.init().fail(function () {
            alert("Please refresh your browser");
        }).done(function (result) {
            app.setRoot("viewmodels/shell", "entrance", null);
        });
    });
})
//@ sourceMappingURL=main.js.map
