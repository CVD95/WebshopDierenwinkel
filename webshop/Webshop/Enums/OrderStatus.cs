using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webshop.Enums
{
    public enum OrderStatus
    {
        PROCESSING, PAYED, SHIPPING, FINISHED, RETURNING, RETURNED
        //Een order kan de volgende statussen hebben, Betaald, Verlopen, Lopend, en Verzonden
    }
}