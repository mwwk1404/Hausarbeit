using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fifastats1._0
{
    public partial class Spielergebnisse
    {
        public override string ToString()
        {
            return "Spiel " + SpielID + ": " + "---" + Tore + " : " + Gegentore + "---Mannschaft: " + MannschaftsID + "---Formation: " + FormationsID;
        }

        public string Ergebnis { get {
                if (Gegentore > Tore)
                {
                    return "Verloren";
                }
                else if (Tore > Gegentore)
                { return "Gewonnen"; }
                else
                    return "Unentschieden";
                    
                    } 
            set { } }
    }
}
