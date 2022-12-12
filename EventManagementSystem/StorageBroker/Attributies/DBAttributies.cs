using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBroker
{
    internal class ExcludeFromSQL : System.Attribute
    {

    }
    internal class NotNull : System.Attribute
    {

    }
    internal class Unique : System.Attribute
    {

    }
    internal class Identity : System.Attribute
    {
        public int start { get; set; }
        public int added { get; set; }
        public Identity(int start, int added)
        {
            this.start = start;
            this.added = added;
        }
    }

    class MyClass
    {
        public int MyProperty { get; set; }
    }
}
