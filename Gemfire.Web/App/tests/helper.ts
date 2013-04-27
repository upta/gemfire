/// <reference path="../../Scripts/typings/_references.test.d.ts" />

export function restore( method: any )
{
    try
    {
        method.restore();
    }
    catch ( ex ) { }
}

// Forces require to create a new instance of the module so we don't have any residual state from a previous call
// If the module in question has been used as a dep, those dep instances will NOT get updated with this new instance
//
// This is useful when testing methods that change internal state of the module
export function reload( unitName:string ) : any
{
    return $.Deferred( function ( dfd )
    {
        requirejs.undef( unitName );

        require( [unitName], function ( unit )
        {
            dfd.resolve( unit );
        } );
    } ).promise();
}