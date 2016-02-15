using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Enums
{
    [Flags]
    public enum UserRole
    {
        USER, MANAGER, ADMIN
            //Alle gebruikers zijn users. 
            //Er zijn 3 rollen voor een User. Iedereen is standaard een User. 
            //Daarnaast kan iemand een Administratiemedewerker of een Manager zijn
    }
}