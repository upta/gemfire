requirejs.config({
    paths: {
        'text': 'durandal/amd/text'
    }
});
require([
    "tests/_auth", 
    "tests/hubs/_lobby", 
    "tests/viewmodels/_createGame", 
    "tests/viewmodels/_lobby", 
    "tests/viewmodels/_shell"
]);
//@ sourceMappingURL=test-main.js.map
