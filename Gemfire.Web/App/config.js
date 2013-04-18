define(["require", "exports"], function(require, exports) {
    var GameScenario = (function () {
        function GameScenario(name) {
            this.name = name;
        }
        return GameScenario;
    })();
    exports.GameScenario = GameScenario;    
    var _scenarios = [
        new GameScenario("Erin & Ander"), 
        new GameScenario("Flax's Shame"), 
        new GameScenario("Terrian's War"), 
        new GameScenario("Gemfire")
    ];
    exports.scenarios = _scenarios;
})
//@ sourceMappingURL=config.js.map
