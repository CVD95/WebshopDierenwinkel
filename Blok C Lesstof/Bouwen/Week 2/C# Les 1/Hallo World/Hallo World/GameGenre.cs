using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallo_World
{
    /// <summary>
    /// 
    /// </summary>
   public class GameGenre
    {
       public int ID { get; set; }
       public bool Verslavend { get; set; }
       public string Naam { get; set; }

            public GameGenre()
            {

            }


       public override string ToString()
            {
                string verslavingsCheck = null; 

                    if(Verslavend == true)
                    {
                        verslavingsCheck = "is verslavend";
                    }
                    else
                    {
                        verslavingsCheck = "is niet verslavend";
                    }

                return String.Format("Het genre : {0} met ID {1}  {2} ",
                                         Naam, ID, verslavingsCheck);
            }
                

    }

}
