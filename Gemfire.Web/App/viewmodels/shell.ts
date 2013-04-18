/// <reference path="../../Scripts/typings/_references.d.ts" />

import _router = module( "durandal/plugins/router" );
import auth = module( "auth");

export function activate()
{
    return router.activate( "lobby" );
}

export function logout()
{
    auth.logout();
}
export var router = _router;
export var user = auth.user;