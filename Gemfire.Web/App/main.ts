/// <reference path="../Scripts/typings/_references.d.ts" />

requirejs.config(
{
    paths:
    {
        'text': 'durandal/amd/text'
    }
} );

import app = module( "durandal/app" );
import router = module( "durandal/plugins/router" );
import system = module( "durandal/system" );
import viewLocator = module( "durandal/viewLocator" );
import auth = module( "auth" );

system.debug( true );

app.title = "Gemfire";
app.start()
   .then( function ()
   {
       viewLocator.useConvention();

       router.useConvention();
       router.mapAuto();

       auth.init()
       .fail( function ()
       {
           alert( "Please refresh your browser" );
       } )
       .done( function ( result )
       {
           //app.adaptToDevice();
           app.setRoot( "viewmodels/shell", "entrance", null );
       } );
   } );