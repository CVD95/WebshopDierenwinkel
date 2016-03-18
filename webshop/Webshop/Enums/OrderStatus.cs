using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Enums
{
    public enum OrderStatus
    {
        TOBEPAID, PAID, PROCESSING, EXPIRED, SENT, RETURNED
        //Een order kan de volgende statussen hebben: Nog niet betaald, Betaald, Vervallen, in behandeling, verstuurd
    }
}