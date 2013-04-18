/// <reference path="../Scripts/typings/_references.d.ts" />

export class GameScenario
{
    name: string;

    constructor( name: string )
    {
        this.name = name;
    }
}

var _scenarios = [
    new GameScenario( "Erin & Ander" ),
    new GameScenario( "Flax's Shame" ),
    new GameScenario( "Terrian's War" ),
    new GameScenario( "Gemfire" )
];

export var scenarios = _scenarios;