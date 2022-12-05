using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoMLibrary
{
    public interface IxmlReader
    {
        string ListUnitDimensions();
        List<T> FindAliasesforUOM<T>(string uom);
        List<T> QuantityUnits<T>();
        List<T> ListAllUOMforQC<T>(string selectedText);


    }
}
