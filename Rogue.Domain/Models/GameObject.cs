using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue.Domain.Models
{
    abstract class GameObject
    {
        public int X {  get; protected set; }
        public int Y {  get; protected set; }

        protected GameObject() {}
    }
}
