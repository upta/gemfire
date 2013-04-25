/// <reference path="../Scripts/typings/_references.test.d.ts" />

requirejs.config(
{
    paths:
    {
        'text': 'durandal/amd/text'
    }
} );

require( 
[
    "tests/_auth",
    "tests/hubs/_lobby",
    "tests/viewmodels/_createGame",
    "tests/viewmodels/_shell"
] );