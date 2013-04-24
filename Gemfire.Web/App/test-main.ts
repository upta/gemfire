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
    "tests/hubs/_lobby"
] );
